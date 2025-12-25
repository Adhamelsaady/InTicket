using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using InTicket.Application.Contracts;
using InTicket.Application.DTOs;
using InTicket.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InTicket.Application.Services;

public class AuthService : IAuthService
{
    private readonly SignInManager <ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public AuthService(SignInManager<ApplicationUser> signInManager, 
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration , IMapper mapper) 
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
    }
    
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        var exsitingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        
        if (exsitingUser != null)
        {
            return null;
        }
        var user = _mapper.Map(registerDto, exsitingUser);
        var newUser =  await _userManager.CreateAsync(user, registerDto.Password);
        if (!newUser.Succeeded)
        {
            return null;
        }

        await _userManager.AddToRoleAsync(user, "User");
        var token = GenerateJwtToken(user, new List<string> { "User" });

        return new AuthResponseDto
        {
            Token = token ,
            Email = user.Email ,
            FullName = user.FirstName + " " + user.LastName ,
            Roles = new List<string> {"User"}
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return null;
        }

        // Check password
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
        {
            return null;
        }

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        // Generate JWT token
        var token = GenerateJwtToken(user, roles.ToList());

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            FullName = user.FirstName + " " + user.LastName,
            Roles = roles.ToList()
        };
    }
    
    private string GenerateJwtToken(ApplicationUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FirstName + user.LastName),
            new Claim(ClaimTypes.Email, user.Email!)
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