using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InTicket.Application.Services;

public class JwtTokenGeneration : IJwtTokenGeneration
{
    private readonly IConfiguration _configuration;
    private readonly IBaseRepository <RefreshToken> _refreshTokens;
    public JwtTokenGeneration(IConfiguration configuration , IBaseRepository <RefreshToken> refreshTokens)
    {
        _configuration = configuration;
        _refreshTokens = refreshTokens;
    }
    public async Task<TokenResult> GenerateJwtToken(ApplicationUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:Lifetime")), // todo : update it to 5 minutes
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = new RefreshToken()
        {
            CreatedAt = DateTime.UtcNow,
            Token = $"{GenerateRefreshToken(25)}_{Guid.NewGuid()}",
            UserId = user.Id,
            isRevoked = false,
            isUsed = false,
            JwtId = token.Id,
            ExpiresAt = DateTime.UtcNow.AddMonths(1)
        };
        await _refreshTokens.AddAsync(refreshToken);
        // add the refreshToken to the db
        var tokenResult = new TokenResult()
        {
            Token = tokenString,
            RefreshToken = refreshToken.Token,
        };
        return tokenResult;
    }

    private string GenerateRefreshToken(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@!$#_";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}