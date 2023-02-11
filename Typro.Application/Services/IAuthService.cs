using FluentResults;
using Typro.Application.Models.Auth;

namespace Typro.Application.Services;

public interface IAuthService
{
    Task<Result<UserSignUpResponseDto>> SignUpAsync(UserSignUpDto dto);
    Task<Result<UserSignInResponseDto>> SignInAsync(UserSignInDto dto);
}