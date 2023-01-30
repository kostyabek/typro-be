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
        using var connection = DatabaseConnector.CreateConnection();
        return connection.ExecuteAsync(UserQueries.InsertUser, model);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        using var connection = DatabaseConnector.CreateConnection();
        return connection.QuerySingleAsync<User>(UserQueries.GetUserById, new { UserId = id });
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        using var connection = DatabaseConnector.CreateConnection();
        return connection.QuerySingleAsync<User>(UserQueries.GetUserByEmail, new { UserEmail = email });
    }
}