using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTicket.Application.Feauters.Authentication.Commands.ForgotPassword;

public class ForgotPasswordRequestHandler : IRequestHandler<ForgotPasswordRequest, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;

    public ForgotPasswordRequestHandler(UserManager<ApplicationUser> userManager,
        IOtpService otpService,
        IEmailService emailService)
    {
        _userManager = userManager;
        _otpService = otpService;
        _emailService = emailService;
    }


    public async Task<bool> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.EmailConfirmed)
        {
            return true;
        }

        var otp = _otpService.GenerateOtp();
        user.PasswordResetOtp = otp;
        user.PasswordResetOtpExpiration = DateTime.UtcNow.AddMinutes(20);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception("Incorrect email or password");
        }

        await _emailService.SendPasswordResetOtpAsync(user.Email, user.FirstName, otp);
        return true;
    }
}