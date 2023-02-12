using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;
using Typro.Infrastructure.Database;

namespace Typro.Infrastructure.Repositories;

public class TokenRepository : DatabaseConnectable, ITokenRepository
{
    public TokenRepository(ConnectionWrapper connectionWrapper) : base(connectionWrapper)
    {
    }

    public Task<int> CreateRefreshTokenAsync(RefreshToken model)
        => ConnectionWrapper.Connection.ExecuteAsync(RefreshTokenQueries.CreateToken, model,
            ConnectionWrapper.Transaction);

    public Task<int> RevokeRefreshTokenAsync(int userId, string token)
        => ConnectionWrapper.Connection.ExecuteAsync(
            RefreshTokenQueries.CreateToken,
            new { UserId = userId, Token = token },
            ConnectionWrapper.Transaction);

    public Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<RefreshToken?>(
            RefreshTokenQueries.GetRefreshTokenByToken,
            new { Token = token },
            ConnectionWrapper.Transaction);
}