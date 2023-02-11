using FluentResults;
using Typro.Application.Models.Auth;
using Typro.Application.Models.User;
using Typro.Application.Repositories;
using Typro.Application.Services;
using Typro.Domain.Enums;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        ICookieService cookieService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _cookieService = cookieService;
    }

    public async Task<Result<UserSignUpResponseDto>> SignUpAsync(UserSignUpDto dto)
    {
        var user = await _userRepository.GetUserByEmailAsync(dto.Email);
        if (user is not null)
        {
            return Result.Fail(new InvalidOperationError("The user already exists."));
        }

        var passwordHash = BC.HashPassword(dto.Password);
        var createUserModel = new CreateUserDto(dto.Email, passwordHash, UserRole.User);

        using var transaction = _userRepository.BeginTransaction();
        try
        {
            var insertedUserId = await _userRepository.CreateUserAsync(createUserModel);

            user = await _userRepository.GetUserByEmailAsync(dto.Email);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(insertedUserId);
            await _userRepository.CreateRefreshTokenAsync(refreshToken);
            transaction.Commit();

            var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.ExpirationDate);
            _cookieService.SetRefreshTokenCookie(refreshTokenDto);
            
            var responseDto = new UserSignUpResponseDto(accessToken);
            return Result.Ok(responseDto);
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<Result<UserSignInResponseDto>> SignInAsync(UserSignInDto dto)
    {
        var user = await _userRepository.GetUserByEmailAsync(dto.Email);
        if (user is null)
        {
            return Result.Fail(new InvalidOperationError("Invalid login/password."));
        }

        var isValidPassword = BC.Verify(dto.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            return Result.Fail(new InvalidOperationError("Invalid login/password."));
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user.Id);
        await _userRepository.CreateRefreshTokenAsync(refreshToken);

        var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.ExpirationDate);
        _cookieService.SetRefreshTokenCookie(refreshTokenDto);
        
        var responseDto = new UserSignInResponseDto(accessToken);
        return Result.Ok(responseDto);
    }
    
    public Result SignOutAsync()
    {
        _cookieService.RemoveRefreshTokenCookie();
        return Result.Ok();
    }
}