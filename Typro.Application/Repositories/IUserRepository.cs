using Typro.Application.Models.User;
using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface IUserRepository
{
    Task<int> CreateUserAsync(CreateUserModel user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
}