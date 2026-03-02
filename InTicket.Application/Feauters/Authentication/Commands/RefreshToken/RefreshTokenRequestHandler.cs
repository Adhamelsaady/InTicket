using System.IdentityModel.Tokens.Jwt;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace InTicket.Application.Feauters.Authentication.Commands.RefreshToken;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, AuthResult>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IJwtTokenGeneration _jwtTokenGeneration;
    private readonly UserManager<ApplicationUser> _userManager;

    public RefreshTokenRequestHandler(IRefreshTokenRepository refreshTokenRepository
        , TokenValidationParameters tokenValidationParameters
        , IJwtTokenGeneration jwtTokenGeneration
        , UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenValidationParameters = tokenValidationParameters;
        _jwtTokenGeneration = jwtTokenGeneration;
    }

    public async Task<AuthResult> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await VerifyRefreshToken(request);
        if (result == null)
            return new AuthenticationResponse() { Success = false, Errors = new List<string>() { "Invalid token" } };
        return result;
    }

    private async Task<AuthResult> VerifyRefreshToken(RefreshTokenRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(request.Token, _tokenValidationParameters, out var validatedToken);
        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        {
            var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.CurrentCultureIgnoreCase);
            if (!result)
            {
                return null;
            }
            
        }
        
        var utcExpiryDate =
            long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);
        var expiryDate = UnixTimeToDateTime(utcExpiryDate);
        if (expiryDate > DateTime.UtcNow)
        {
            return new AuthResult()
            {
                Success = false, Errors = new List<string>() { "The Token Is Not Expired" }
            };
        }

        var refreshTokenEntity = await _refreshTokenRepository.GetRefreshTokenAsync(request.RefreshToken);

        if (refreshTokenEntity == null)
        {
            return new AuthResult()
            {
                Success = false, Errors = new List<string>() { "Refresh Token not found" }
            };
        }

        if (refreshTokenEntity.isUsed == true)
        {
            return new AuthResult()
            {
                Success = false, Errors = new List<string>() { "Refresh Token is used" }
            };
        }

        if (refreshTokenEntity.isRevoked == true)
        {
            return new AuthResult()
            {
                Success = false, Errors = new List<string>() { "Refresh Token is revoked" }
            };
        }

        if (refreshTokenEntity.ExpiresAt < DateTime.UtcNow)
        {
            return new AuthResult()
            {
                Success = false, Errors = new List<string>() { "Refresh Token is expired" }
            };
        }

        var jti = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        if (jti != refreshTokenEntity.JwtId)
        {
            return new AuthResult()
            {
                Success = false, Errors = new List<string>() { "Refresh Token reference does not match the jwt token" }
            };
        }

        if (!await _refreshTokenRepository.MarkRefreshTokenAsUsedAsync(refreshTokenEntity))
        {
            return new AuthResult()
            {
                Success = false, Errors = new List<string>() { "Something went wrong" }
            };
        }
        
        var roles = await _userManager.GetRolesAsync(refreshTokenEntity.User);
        var tokenResult = await _jwtTokenGeneration.GenerateJwtToken(refreshTokenEntity.User, roles.ToList());
        return new AuthResult()
        {
            Success = true,
            RefreshToken = tokenResult.RefreshToken,
            Token = tokenResult.Token,
        };
    }

    private DateTime UnixTimeToDateTime(long unixTime)
    {
        var result = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return result.AddSeconds(unixTime).ToUniversalTime();
    }
}