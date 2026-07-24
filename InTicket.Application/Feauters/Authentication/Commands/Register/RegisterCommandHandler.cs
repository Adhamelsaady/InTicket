using AutoMapper;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        _userManager        = userManager;
        _otpService         = otpService;
        _mapper             = mapper;
        _emailService       = emailService;
        _jwtTokenGeneration = jwtTokenGeneration;
    }

    public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Guard: duplicate email
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Failure($"{existingUser.FirstName} {existingUser.LastName} is already registered.");

        // Guard: duplicate national ID
        // Note: plain FirstOrDefault (no EF async) is intentional — UserManager.Users
        // is an in-memory queryable in tests and works correctly in production too.
        var duplicateNid = await _userManager.Users
            .FirstOrDefaultAsync(u => u.NationalId == request.NationalId);
        if (duplicateNid != null)
            return Failure("This national ID is already registered.");

        // Build and persist the new user
        var otp  = _otpService.GenerateOtp();
        var user = _mapper.Map<ApplicationUser>(request);
        user.InTicketId                     = Guid.NewGuid();
        user.EmailConfirmationOtp           = otp;
        user.EmailConfirmationOtpExpiration = DateTime.UtcNow.AddMinutes(20);

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Failure("Something went wrong. Please try again.");

        await _userManager.AddToRoleAsync(user, "User");
        await _emailService.SendEmailConfirmationOtpAsync(user.Email!, user.FirstName, otp);

        var token = await _jwtTokenGeneration.GenerateJwtToken(user, new List<string> { "User" });

        return new AuthenticationResponse
        {
            Success      = true,
            Token        = token.Token,
            RefreshToken = token.RefreshToken,
            Email        = user.Email!,
            FullName     = $"{user.FirstName} {user.LastName}",
            Roles        = new List<string> { "User" }
        };
    }

    private static AuthenticationResponse Failure(string error) => new()
    {
        Success = false,
        Errors  = new List<string> { error }
    };
}