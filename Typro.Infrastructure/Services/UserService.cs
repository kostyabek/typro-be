using Typro.Application.Models.User;
using Typro.Application.Repositories;
using Typro.Application.Services;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<int> CreateUserAsync(CreateUserModel model)
    {
        return _userRepository.CreateUserAsync(model);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        return _userRepository.GetUserByIdAsync(id);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _userRepository.GetUserByEmailAsync(email);
    }
}