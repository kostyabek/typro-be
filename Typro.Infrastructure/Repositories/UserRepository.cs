using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Models.User;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;
using Typro.Infrastructure.Database;

namespace Typro.Infrastructure.Repositories;

public class UserRepository : DatabaseConnectable, IUserRepository
{
    public UserRepository(ConnectionWrapper connectionWrapper) : base(connectionWrapper)
    {
    }

    public Task<int> CreateUserAsync(CreateUserDto model)
        => ConnectionWrapper.Connection.ExecuteScalarAsync<int>(UserQueries.CreateUser, model,
            ConnectionWrapper.Transaction);

    public Task<User?> GetUserByIdAsync(int id)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<User?>(UserQueries.GetUserById,
            new { UserId = id }, ConnectionWrapper.Transaction);

    public Task<User?> GetUserByEmailAsync(string email)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<User?>(UserQueries.GetUserByEmail,
            new { UserEmail = email },
            ConnectionWrapper.Transaction);
}