using Typro.Domain.Database.Models;

namespace Typro.Application.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(int userId);
}