using AutoMapper;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InTicket.Application.Feauters.Authentication.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOtpService _otpService;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IJwtTokenGeneration _jwtTokenGeneration;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        IOtpService otpService,
        IMapper mapper,
        IEmailService emailService,
        IJwtTokenGeneration jwtTokenGeneration)
    {
        _userManager = userManager;
        _otpService = otpService;
        _mapper = mapper;
        _emailService = emailService;
        _jwtTokenGeneration = jwtTokenGeneration;
    }

    public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new Exception($"Email '{request.Email}' is already registered.");
        }

        var existingNationalId = await _userManager.Users
            .FirstOrDefaultAsync(u => u.NationalId == request.NationalId);
        if (existingNationalId != null)
        {
            throw new Exception($"National Id '{request.NationalId}' is already registered.");
        }

        var otp = _otpService.GenerateOtp();
        var user = _mapper.Map<ApplicationUser>(request);
        user.InTicketId = Guid.NewGuid();
        user.EmailConfirmationOtp = otp;
        user.EmailConfirmationOtpExpiration = DateTime.UtcNow.AddMinutes(20);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return null;
        }

        await _userManager.AddToRoleAsync(user, "User");
        await _emailService.SendEmailConfirmationOtpAsync(
            user.Email!,
            user.FirstName,
            otp);

        var token = _jwtTokenGeneration.GenerateJwtToken(user, new List<string> { "User" });

        return new AuthenticationResponse
        {
            Token = token,
            Email = user.Email!,
            FullName = user.FirstName + user.LastName,
            Roles = new List<string> { "User" }
        };
    }
}