using InTicket.Application.Contracts;
using InTicket.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTicket.Application.Feauters.Authentication.Commands.ResetPassword;

public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequest, bool>
{
    private const int MaxOtpAttempts       = 10;
    private const int LockoutWindowMinutes = 15;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOtpService _otpService;

    public ResetPasswordRequestHandler(
        UserManager<ApplicationUser> userManager,
        IOtpService otpService)
    {
        _userManager = userManager;
        _otpService  = otpService;
    }

    public async Task<bool> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return false;

        if (IsLockedOut(user))
            return false;

        ResetCounterIfWindowExpired(user);

        user.OtpAttempts++;
        user.LastOtpAttemptAt = DateTime.UtcNow;

        if (!_otpService.ValidateOtp(request.Otp, user.PasswordResetOtp, user.PasswordResetOtpExpiration))
        {
            await _userManager.UpdateAsync(user);
            return false;
        }

        var token  = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!result.Succeeded)
            return false;

        user.PasswordResetOtp           = null;
        user.PasswordResetOtpExpiration = null;
        user.OtpAttempts                = 0;
        user.LastOtpAttemptAt           = null;

        await _userManager.UpdateAsync(user);
        return true;
    }

    /// <summary>Returns true when the user has exceeded the attempt limit within the lockout window.</summary>
    private static bool IsLockedOut(ApplicationUser user) =>
        user.OtpAttempts >= MaxOtpAttempts &&
        user.LastOtpAttemptAt.HasValue &&
        DateTime.UtcNow < user.LastOtpAttemptAt.Value.AddMinutes(LockoutWindowMinutes);

    /// <summary>Resets the attempt counter only after the lockout window has fully expired.</summary>
    private static void ResetCounterIfWindowExpired(ApplicationUser user)
    {
        if (user.OtpAttempts >= MaxOtpAttempts &&
            user.LastOtpAttemptAt.HasValue &&
            DateTime.UtcNow >= user.LastOtpAttemptAt.Value.AddMinutes(LockoutWindowMinutes))
        {
            user.OtpAttempts = 0;
        }
    }
}