using FluentResults;
using Typro.Application.Models.User;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.User;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> CreateUserAsync(CreateUserDto model)
    {
        var generatedUserId = await _unitOfWork.UserRepository.CreateUserAsync(model);
        return Result.Ok(generatedUserId);
    }

    public async Task<Result<Domain.Database.Models.User>> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
        return user is null ?
            Result.Fail(new NotFoundError("User not found")) :
            Result.Ok(user);
    }
    
    public async Task<Result<Domain.Database.Models.User>> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
        return user is null ?
            Result.Fail(new NotFoundError("User not found")) :
            Result.Ok(user);
    }
}