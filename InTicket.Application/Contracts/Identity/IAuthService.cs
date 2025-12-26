using InTicket.Application.DTOs;

namespace InTicket.Application.Contracts;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<bool> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
    Task<bool> ResendEmailConfirmationOtpAsync(ResendOtpDto resendOtpDto);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}