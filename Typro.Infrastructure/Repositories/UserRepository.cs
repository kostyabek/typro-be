using System.Data;
using Dapper;
using Typro.Application.Models.User;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;
using Typro.Infrastructure.Database;

namespace Typro.Infrastructure.Repositories;

public class UserRepository : DatabaseConnectable, IUserRepository
{
    public UserRepository(IDbConnection dbConnection) : base(dbConnection)
    {
    }

    public Task<int> CreateUserAsync(CreateUserDto model)
        => Connection.ExecuteAsync(UserQueries.InsertUser, model, Transaction);

    public Task<User?> GetUserByIdAsync(int id)
        => Connection.QuerySingleOrDefaultAsync<User?>(UserQueries.GetUserById, new { UserId = id }, Transaction);

    public Task<User?> GetUserByEmailAsync(string email)
        => Connection.QuerySingleOrDefaultAsync<User?>(UserQueries.GetUserByEmail, new { UserEmail = email }, Transaction);
}