using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Models.User;
using Typro.Application.Queries;
using Typro.Application.Repositories;

namespace Typro.Infrastructure.Repositories.User;

public class UserRepository : DatabaseConnectable, IUserRepository
{
    public UserRepository(ConnectionWrapper connectionWrapper) : base(connectionWrapper)
    {
    }

    public Task<int> CreateUserAsync(CreateUserDto model)
        => ConnectionWrapper.Connection.ExecuteScalarAsync<int>(UserQueries.InsertUser, model,
            ConnectionWrapper.Transaction);

    public Task<Domain.Database.Models.User?> GetUserByIdAsync(int id)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<Domain.Database.Models.User?>(UserQueries.GetUserById,
            new { UserId = id }, ConnectionWrapper.Transaction);

    public Task<Domain.Database.Models.User?> GetUserByEmailAsync(string email)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<Domain.Database.Models.User?>(UserQueries.GetUserByEmail,
            new { UserEmail = email },
            ConnectionWrapper.Transaction);
}