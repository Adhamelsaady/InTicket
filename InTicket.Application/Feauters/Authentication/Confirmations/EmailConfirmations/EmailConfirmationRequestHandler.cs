using InTicket.Application.Contracts;
using InTicket.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTicket.Application.Feauters.Authentication.Confirmations.EmailConfirmations;

public class EmailConfirmationRequestHandler : IRequestHandler<EmailConfirmationRequest, bool>
{
    private const int MaxOtpAttempts       = 10;
    private const int LockoutWindowMinutes = 15;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOtpService _otpService;

    public EmailConfirmationRequestHandler(
        UserManager<ApplicationUser> userManager,
        IOtpService otpService)
    {
        _userManager = userManager;
        _otpService  = otpService;
    }

    public async Task<bool> Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return false;

        if (IsLockedOut(user))
            return false;

        ResetCounterIfWindowExpired(user);

        user.OtpAttempts++;
        user.LastOtpAttemptAt = DateTime.UtcNow;

        if (!_otpService.ValidateOtp(request.Otp, user.EmailConfirmationOtp, user.EmailConfirmationOtpExpiration))
        {
            await _userManager.UpdateAsync(user);
            return false;
        }

        user.EmailConfirmed                 = true;
        user.EmailConfirmationOtp           = null;
        user.EmailConfirmationOtpExpiration = null;
        user.OtpAttempts                    = 0;
        user.LastOtpAttemptAt               = null;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
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