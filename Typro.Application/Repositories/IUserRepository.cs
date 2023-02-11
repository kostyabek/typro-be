using Typro.Application.Models.User;
using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface IUserRepository : IRepository
{
    Task<int> CreateUserAsync(CreateUserDto user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<int> CreateRefreshTokenAsync(RefreshToken model);
}