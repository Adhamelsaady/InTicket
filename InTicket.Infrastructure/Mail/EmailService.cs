using System.Net.Mail;
using InTicket.Application.Contracts.Infrasructure;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace InTicket.Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailConfirmationOtpAsync(string email, string firstName, string otp)
    {
        var subject = "Confirm Your Email - InTicket";
        var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                    <h1 style='color: white; margin: 0;'>Welcome to InTicket! 🎉</h1>
                </div>
                
                <div style='background-color: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;'>
                    <h2 style='color: #333;'>Hello {firstName},</h2>
                    <p>Thank you for registering with InTicket! To complete your registration, please verify your email address.</p>
                    
                    <div style='background-color: white; padding: 20px; text-align: center; margin: 20px 0; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                        <p style='margin: 0 0 10px 0; color: #666;'>Your verification code is:</p>
                        <h1 style='color: #667eea; font-size: 48px; letter-spacing: 8px; margin: 0; font-weight: bold;'>{otp}</h1>
                    </div>
                    
                    <p>This code will expire in <strong>10 minutes</strong>.</p>
                    
                    <div style='background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 12px; margin: 20px 0; border-radius: 4px;'>
                        <p style='margin: 0; color: #856404;'>⚠️ If you didn't create an account with InTicket, please ignore this email.</p>
                    </div>
                    
                    <p style='margin-top: 30px; color: #666; font-size: 14px;'>
                        Best regards,<br>
                        <strong>The InTicket Team</strong>
                    </p>
                </div>
                
                <div style='text-align: center; padding: 20px; color: #999; font-size: 12px;'>
                    <p>© 2024 InTicket. All rights reserved.</p>
                    <p>Your Premier Event Ticketing Platform</p>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetOtpAsync(string email, string firstName, string otp)
    {
        var subject = "Reset Your Password - InTicket";
        var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <div style='background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                    <h1 style='color: white; margin: 0;'>Password Reset Request 🔐</h1>
                </div>
                
                <div style='background-color: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;'>
                    <h2 style='color: #333;'>Hello {firstName},</h2>
                    <p>We received a request to reset your password. If you made this request, please use the verification code below:</p>
                    
                    <div style='background-color: white; padding: 20px; text-align: center; margin: 20px 0; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                        <p style='margin: 0 0 10px 0; color: #666;'>Your password reset code is:</p>
                        <h1 style='color: #f5576c; font-size: 48px; letter-spacing: 8px; margin: 0; font-weight: bold;'>{otp}</h1>
                    </div>
                    
                    <p>This code will expire in <strong>10 minutes</strong>.</p>
                    
                    <div style='background-color: #f8d7da; border-left: 4px solid #dc3545; padding: 12px; margin: 20px 0; border-radius: 4px;'>
                        <p style='margin: 0; color: #721c24;'>⚠️ If you didn't request a password reset, please ignore this email. Your password will remain unchanged.</p>
                    </div>
                    
                    <p style='margin-top: 30px; color: #666; font-size: 14px;'>
                        Best regards,<br>
                        <strong>The InTicket Team</strong>
                    </p>
                </div>
                
                <div style='text-align: center; padding: 20px; color: #999; font-size: 12px;'>
                    <p>© 2024 InTicket. All rights reserved.</p>
                    <p>Your Premier Event Ticketing Platform</p>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var otpMatch = System.Text.RegularExpressions.Regex.Match(
            body, 
            @"letter-spacing: 8px;[^>]*>(\d{6})</h1>");
        
        var otp = otpMatch.Success ? otpMatch.Groups[1].Value : "N/A";
        
        _logger.LogInformation("========================================");
        _logger.LogInformation("📧 Sending email to: {Email}", toEmail);
        _logger.LogInformation("📝 Subject: {Subject}", subject);
        _logger.LogInformation("🔑 OTP: {Otp}", otp);
        _logger.LogInformation("========================================");
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            emailSettings["SenderName"], 
            emailSettings["SenderEmail"]));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(
                emailSettings["SmtpServer"], 
                int.Parse(emailSettings["SmtpPort"]!), 
                SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                emailSettings["SmtpUsername"], 
                emailSettings["SmtpPassword"]);
            await client.SendAsync(message);
            
            _logger.LogInformation("✅ Email sent successfully via Brevo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to send email via Brevo");
            _logger.LogWarning("💡 OTP is available in logs above for testing");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}