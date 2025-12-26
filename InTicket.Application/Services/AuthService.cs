using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.DTOs;
using InTicket.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InTicket.Application.Services;

public class AuthService : IAuthService
{
    private readonly SignInManager <ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper; 
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    public AuthService(SignInManager<ApplicationUser> signInManager, 
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration , IMapper mapper , 
        IOtpService otpService,
        IEmailService emailService) 
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
        _otpService = otpService;
        _emailService = emailService;
    }
    
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return null;
        }
        var existingNationalId = await _userManager.Users
            .FirstOrDefaultAsync(u => u.NationalId == registerDto.NationalId);
        if (existingNationalId != null)
        {
            return null;
        }
        var otp = _otpService.GenerateOtp();
        var user = _mapper.Map<ApplicationUser> (registerDto);
        user.InTicketId = Guid.NewGuid();
        user.EmailConfirmationOtp = otp;
        user.EmailConfirmationOtpExpiration = DateTime.UtcNow.AddMinutes(20);
        
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return null;
        }
        await _userManager.AddToRoleAsync(user, "User");
        await _emailService.SendEmailConfirmationOtpAsync(
            user.Email!, 
            user.FirstName, 
            otp);

        var token = GenerateJwtToken(user, new List<string> { "User" });

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!, 
            FullName = user.FirstName + user.LastName,
            Roles = new List<string> { "User" }
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return null;
        }

        if (!user.EmailConfirmed)
        {
            throw new Exception("Email not confirmed");
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = GenerateJwtToken(user, roles.ToList());

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            FullName = user.FirstName + " " + user.LastName,
            Roles = roles.ToList()
        };
    }

    public async Task<bool> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
    {
        var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);
        if (user == null)
        {
            return false;
        }
        
        if (user.OtpAttempts >= 10 && 
            user.LastOtpAttemptAt.HasValue && 
            DateTime.UtcNow < user.LastOtpAttemptAt.Value.AddMinutes(15))
        {
            return false; 
        }

        if (!user.LastOtpAttemptAt.HasValue || 
            DateTime.UtcNow >= user.LastOtpAttemptAt.Value.AddMinutes(15))
        {
            user.OtpAttempts = 0;
        }

        user.OtpAttempts++;
        user.LastOtpAttemptAt = DateTime.UtcNow;

        if (!_otpService.ValidateOtp(confirmEmailDto.Otp, user.EmailConfirmationOtp, user.EmailConfirmationOtpExpiration))
        {
            await _userManager.UpdateAsync(user);
            return false;
        }

        user.EmailConfirmed = true;
        user.EmailConfirmationOtp = null;
        user.EmailConfirmationOtpExpiration = null;
        user.OtpAttempts = 0;
        user.LastOtpAttemptAt = null;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ResendEmailConfirmationOtpAsync(ResendOtpDto resendOtpDto)
    {
        var user = await _userManager.FindByEmailAsync(resendOtpDto.Email);
        if (user == null || user.EmailConfirmed)
        {
            return false;
        }

        var otp = _otpService.GenerateOtp();
        user.EmailConfirmationOtp = otp;
        user.EmailConfirmationOtpExpiration = DateTime.UtcNow.AddMinutes(10);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return false;
        }

        await _emailService.SendEmailConfirmationOtpAsync(
            user.Email!, 
            user.FirstName, 
            otp);

        return true;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
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
            return false;
        }
        
        await _emailService.SendPasswordResetOtpAsync(user.Email, user.FirstName, otp);
        return true;

    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null)
        {
            return false;
        }

        if (user.OtpAttempts >= 10 && 
            user.LastOtpAttemptAt.HasValue && 
            DateTime.UtcNow < user.LastOtpAttemptAt.Value.AddMinutes(15))
        {
            Console.WriteLine("The problem in 210");
            return false;
        }

        if (!user.LastOtpAttemptAt.HasValue || 
            DateTime.UtcNow >= user.LastOtpAttemptAt.Value.AddMinutes(15))
        {
            Console.WriteLine("The problem in 217");
            user.OtpAttempts = 0;
        }

        user.OtpAttempts++;
        user.LastOtpAttemptAt = DateTime.UtcNow;

        if (!_otpService.ValidateOtp(resetPasswordDto.Otp, user.PasswordResetOtp, user.PasswordResetOtpExpiration))
        {
            Console.WriteLine("The problem in 226");
            await _userManager.UpdateAsync(user);
            return false;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            return false;
        }
        user.PasswordResetOtp = null;
        user.PasswordResetOtpExpiration = null;
        user.OtpAttempts = 0;
        user.LastOtpAttemptAt = null;
        await _userManager.UpdateAsync(user);
        return true;
    }

    private string GenerateJwtToken(ApplicationUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("NationalId", user.NationalId),
            new Claim("InTicketId", user.InTicketId.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}