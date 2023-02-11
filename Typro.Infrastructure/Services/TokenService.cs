using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Typro.Application.Models.Options;
using Typro.Application.Repositories;
using Typro.Application.Services;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly TokenOptions _jwtOptions;
    private readonly IUserRepository _userRepository;

    public TokenService(IOptions<TokenOptions> jwtOptions, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateAccessToken(User user)
    {
        var roleClaimValue = Enum.GetName(user.RoleId).ToLower();
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, roleClaimValue)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var expirationDate = DateTime.UtcNow.AddDays(_jwtOptions.TokenLifetimeInMinutes);

        var securityToken = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials,
            expires: expirationDate);

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var jwt = securityTokenHandler.WriteToken(securityToken);

        return jwt;
    }

    public RefreshToken GenerateRefreshToken(int userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return new RefreshToken
        {
            Token = token,
            CreatedDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeInDays),
            IsRevoked = false,
            UserId = userId
        };
    }
}