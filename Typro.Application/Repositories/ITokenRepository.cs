using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface ITokenRepository
{
    Task<int> CreateRefreshTokenAsync(RefreshToken model);
    Task<int> RevokeRefreshTokenAsync(int userId, string token);
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
}