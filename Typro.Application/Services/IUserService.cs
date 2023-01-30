using Typro.Application.Models.User;
using Typro.Domain.Database.Models;

namespace Typro.Application.Services;

public interface IUserService
{
    Task<int> CreateUserAsync(CreateUserModel model);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
}