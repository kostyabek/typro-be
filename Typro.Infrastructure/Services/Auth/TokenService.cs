using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Typro.Application.Models.Options;
using Typro.Application.Services.Auth;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.Auth;

public class TokenService : ITokenService
{
    private readonly TokenOptions _tokenOptions;

    private readonly IUnitOfWork _unitOfWork;

    public TokenService(
        IOptions<TokenOptions> tokenOptions,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _tokenOptions = tokenOptions.Value;
    }

    public string GenerateAccessToken(Domain.Database.Models.User user)
    {
        string? roleClaimValue = Enum.GetName(user.RoleId).ToLower();
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("email", user.Email),
            new("role", roleClaimValue)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        DateTime expirationDate = DateTime.UtcNow.AddDays(_tokenOptions.TokenLifetimeInMinutes);

        var securityToken = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials,
            expires: expirationDate);

        var securityTokenHandler = new JwtSecurityTokenHandler();
        string? accessToken = securityTokenHandler.WriteToken(securityToken);

        return accessToken;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
    {
        string? token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshTokenModel = new RefreshToken
        {
            Token = token,
            CreatedDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenLifetimeInDays),
            IsRevoked = false,
            UserId = userId
        };

        await _unitOfWork.TokenRepository.CreateRefreshTokenAsync(refreshTokenModel);

        return refreshTokenModel;
    }

    public async Task<Result> ValidateRefreshToken(string token)
    {
        RefreshToken? refreshToken = await _unitOfWork.TokenRepository.GetRefreshTokenByTokenAsync(token);
        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.ExpirationDate < DateTime.UtcNow)
        {
            return Result.Fail(new InvalidOperationError("Invalid token."));
        }

        return Result.Ok();
    }
}