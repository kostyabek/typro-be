using Dapper;
using Typro.Application.Database;
using Typro.Application.Models.User;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;
using Typro.Infrastructure.Database;

namespace Typro.Infrastructure.Repositories;

public class UserRepository : DatabaseConnectable, IUserRepository
{
    public UserRepository(IDatabaseConnector databaseConnector) : base(databaseConnector)
    {
    }
    
    public Task<int> CreateUserAsync(CreateUserModel model)
    {
        var connection = DatabaseConnector.GetConnection();
        return connection.ExecuteAsync(UserQueries.InsertUser, model);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        var connection = DatabaseConnector.GetConnection();
        return connection.QuerySingleOrDefaultAsync<User?>(UserQueries.GetUserById, new { UserId = id });
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        var connection = DatabaseConnector.GetConnection();
        return connection.QuerySingleOrDefaultAsync<User?>(UserQueries.GetUserByEmail, new { UserEmail = email });
    }
}