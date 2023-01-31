using FluentResults;
using MediatR;
using Typro.Application.Models.User;
using Typro.Application.Repositories;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Application.CQRS.Auth.UserSignIn;

public class UserSignInCommandHandler : IRequestHandler<UserSignInCommand, Result<UserGeneralInfoModel>>
{
    private readonly IUserRepository _userRepository;

    public UserSignInCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserGeneralInfoModel>> Handle(UserSignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Fail(new InvalidOperationError("Invalid login/password."));
        }

        var isValidPassword = BC.Verify(request.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            return Result.Fail(new InvalidOperationError("Invalid login/password."));
        }

        var userInfo = new UserGeneralInfoModel
        {
            Email = user.Email
        };
        
        return Result.Ok(userInfo);
    }
}