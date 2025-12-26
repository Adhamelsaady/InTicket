using InTicket.Application.Contracts;
using InTicket.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InTicket.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(registerDto);
        if (result == null)
        {
            return BadRequest(new {message = "Registration failed"});
        }
        return Ok(new 
        { 
            message = "Registration successful! Please check your email for OTP to confirm your account.",
            data = result 
        });
    }

    [HttpPost("confirm-email")]

    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _authService.ConfirmEmailAsync(confirmEmailDto);
        if (!result)
        {
            return BadRequest(new { message = "Invalid Or Expired Otp , Please try again" });
        }
        return Ok();
    }

    [HttpPost("resend-confirmation-otp")]
    public async Task<IActionResult> ResendConfirmationDto(ResendOtpDto  resendOtpDto )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _authService.ResendEmailConfirmationOtpAsync(resendOtpDto);
        if (! result)
        {
            return BadRequest(new { message = "Could not resend OTP. Email may already be confirmed or does not exist." });
        }
        return Ok(new { message = "OTP resent successfully! Please check your email." });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        return Ok(result);
        
    }
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _authService.ForgotPasswordAsync(dto);
        return Ok(new { message = "If your email exists, you will receive a password reset OTP shortly." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.ResetPasswordAsync(dto);

        if (!result)
        {
            return BadRequest(new { message = "Invalid or expired OTP. Please try again or request a new password reset." });
        }

        return Ok(new { message = "Password reset successfully! You can now login with your new password." });
    }
    
}