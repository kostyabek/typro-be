using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Repositories.Auth;

public class TokenRepository : DatabaseConnectable, ITokenRepository
{
    public TokenRepository(ConnectionWrapper connectionWrapper) : base(connectionWrapper)
    {
    }

    public Task<int> CreateRefreshTokenAsync(RefreshToken model)
        => ConnectionWrapper.Connection.ExecuteAsync(RefreshTokenQueries.CreateToken, model,
            ConnectionWrapper.Transaction);

    public Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<RefreshToken?>(
            RefreshTokenQueries.GetRefreshTokenByToken,
            new { Token = token },
            ConnectionWrapper.Transaction);
}