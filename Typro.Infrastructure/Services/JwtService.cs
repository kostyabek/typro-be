using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Typro.Application.Models.Options;
using Typro.Application.Services;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(User user)
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
}