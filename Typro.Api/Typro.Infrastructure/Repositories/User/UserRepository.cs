using System.Data;
using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Models.User;
using Typro.Application.Queries;
using Typro.Application.Repositories;

namespace Typro.Infrastructure.Repositories.User;

public class UserRepository(ConnectionWrapper connectionWrapper) : DatabaseConnectable(connectionWrapper), IUserRepository
{
    public Task<int> CreateUserAsync(CreateUserDto model)
        => ConnectionWrapper.Connection.ExecuteScalarAsync<int>(UserQueries.InsertUser, model,
            transaction: ConnectionWrapper.Transaction);

    public Task<Domain.Database.Models.User?> GetUserByIdAsync(int id)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<Domain.Database.Models.User?>(UserQueries.GetUserById,
            new { UserId = id }, transaction: ConnectionWrapper.Transaction);

    public Task<Domain.Database.Models.User?> GetUserByEmailAsync(string email)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<Domain.Database.Models.User?>(
            UserQueries.GetUserByEmail,
            new { UserEmail = email },
            transaction: ConnectionWrapper.Transaction);

    public Task<int> UpdateNicknameByIdAsync(string nickname, int id)
        => ConnectionWrapper.Connection.ExecuteAsync(UserQueries.EditNicknameById,
            new { Id = id, Nickname = nickname },
            transaction: ConnectionWrapper.Transaction);

    public Task<string?> GetNicknameByIdAsync(int id)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<string?>(UserQueries.GetNicknameById,
            new { UserId = id }, transaction: ConnectionWrapper.Transaction);

    public Task<IEnumerable<WordsPerMinuteToAccuracyDto>> GetWordsPerMinuteToAccuracyStatsAsync(
        WordsPerMinuteToAccuracyRequestDto dto)
        => ConnectionWrapper.Connection.QueryAsync<WordsPerMinuteToAccuracyDto>(
            "dbo.WpmToAccuracyStats",
            commandType: CommandType.StoredProcedure,
            param: dto,
            transaction: ConnectionWrapper.Transaction);
}