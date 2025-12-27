using InTicket.Application.Feauters.Authentication.Commands.ForgotPassword;
using InTicket.Application.Feauters.Authentication.Commands.ResetPassword;
using InTicket.Application.Feauters.Authentication.Confirmations.EmailConfirmations;
using InTicket.Application.Feauters.Authentication.Confirmations.ResendEmailConfirmationOtp;
using InTicket.Application.Feauters.Authentication.Login;
using InTicket.Application.Feauters.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InTicket.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> register(RegisterCommand registerCommand)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(registerCommand);

        return Ok(new
        {
            message = "Registration successful! Please check your email for OTP to confirm your account.",
            data = result
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> login(LoginCommand loginCommand)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(loginCommand);
        return Ok(result);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(EmailConfirmationRequest emailConfirmationRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(emailConfirmationRequest);
        if (!result)
        {
            return BadRequest(new { message = "Invalid Or Expired Otp , Please try again" });
        }

        return Ok();
    }

    [HttpPost("resend-confirmation-otp")]
    public async Task<IActionResult> ResendConfirmationDto(ResendEmailConfirmationOtpRequest
        resendEmailConfirmationOtpRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(resendEmailConfirmationOtpRequest);
        if (!result)
        {
            return BadRequest(new
                { message = "Could not resend OTP. Email may already be confirmed or does not exist." });
        }

        return Ok(new { message = "OTP resent successfully! Please check your email." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(forgotPasswordRequest);
        if (!result)
        {
            return BadRequest(new { message = "Could not forgot password. Please try again." });
        }

        return Ok(new { message = "If your email exists, you will receive a password reset OTP shortly." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(resetPasswordRequest);

        if (!result)
        {
            return BadRequest(new
                { message = "Invalid or expired OTP. Please try again or request a new password reset." });
        }

        return Ok(new { message = "Password reset successfully! You can now login with your new password." });
    }
}