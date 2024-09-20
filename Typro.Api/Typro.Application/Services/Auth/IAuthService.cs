using FluentResults;
using Typro.Application.Models.Auth;

namespace Typro.Application.Services.Auth;

public interface IAuthService
{
    Task<Result<UserAuthResponseDto>> SignUpAsync(UserSignUpDto dto);
    Task<Result<UserAuthResponseDto>> SignInAsync(UserSignInDto dto);
    Result SignOut();
    Task<Result<AccessTokenResponseDto>> RefreshAccessTokenAsync();
}