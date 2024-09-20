using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface ITokenRepository
{
    Task<int> CreateRefreshTokenAsync(RefreshToken model);
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
}