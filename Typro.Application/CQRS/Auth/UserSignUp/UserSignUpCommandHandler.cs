using FluentResults;
using MediatR;
using Typro.Application.Models.User;
using Typro.Application.Repositories;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Application.CQRS.Auth.UserSignUp;

public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, Result<UserGeneralInfoModel>>
{
    private readonly IUserRepository _userRepository;

    public UserSignUpCommandHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserGeneralInfoModel>> Handle(UserSignUpCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user is not null)
        {
            return Result.Fail(new InvalidOperationError("The user already exists."));
        }

        var passwordHash = BC.HashPassword(request.Password);
        var createUserModel = new CreateUserModel
        {
            Email = request.Email,
            PasswordHash = passwordHash
        };

        await _userRepository.CreateUserAsync(createUserModel);

        user = await _userRepository.GetUserByEmailAsync(request.Email);
        var userGeneralInfoModel = new UserGeneralInfoModel
        {
            Email = user.Email
        };

        return Result.Ok(userGeneralInfoModel);
    }
}