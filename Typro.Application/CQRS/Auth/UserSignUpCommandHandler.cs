using System.Text;
using FluentResults;
using MediatR;
using Typro.Application.Models.User;
using Typro.Application.Repositories;
using Typro.Application.Services;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Application.CQRS.Auth;

public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, Result<UserGeneralInfoModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UserSignUpCommandHandler(
        IUserRepository userRepository,
        IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<Result<UserGeneralInfoModel>> Handle(UserSignUpCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user is not null)
        {
            return Result.Fail(new InvalidOperationError("The user already exists."));
        }

        _authService.GeneratePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var createUserModel = new CreateUserModel
        {
            Email = request.Email,
            PasswordHash = Encoding.UTF8.GetString(passwordHash),
            PasswordSalt = Encoding.UTF8.GetString(passwordSalt)
        };

        await _userRepository.CreateUserAsync(createUserModel);

        user = await _userRepository.GetUserByEmailAsync(request.Email);
        var userGeneralInfoModel = new UserGeneralInfoModel
        {
            Email = user.Email
        };

        return userGeneralInfoModel;
    }
}