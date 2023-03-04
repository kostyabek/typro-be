using FluentResults;
using Typro.Application.Models.Auth;
using Typro.Application.Models.User;
using Typro.Application.Services;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Enums;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;
    private readonly IUserIdentityService _userIdentityService;

    public AuthService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        ICookieService cookieService,
        IUserIdentityService userIdentityService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _cookieService = cookieService;
        _userIdentityService = userIdentityService;
    }

    public async Task<Result<UserSignUpResponseDto>> SignUpAsync(UserSignUpDto dto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(dto.Email);
        if (user is not null)
        {
            return Result.Fail(new InvalidOperationError("The user already exists."));
        }

        var passwordHash = BC.HashPassword(dto.Password);
        var createUserModel = new CreateUserDto(dto.Email, passwordHash, UserRole.User);

        _unitOfWork.BeginTransaction();
        try
        {
            var insertedUserId = await _unitOfWork.UserRepository.CreateUserAsync(createUserModel);

            user = await _unitOfWork.UserRepository.GetUserByEmailAsync(dto.Email);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(insertedUserId);
            _unitOfWork.CommitTransaction();

            var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.ExpirationDate);
            _cookieService.SetRefreshTokenCookie(refreshTokenDto);

            var responseDto = new UserSignUpResponseDto(accessToken);
            return Result.Ok(responseDto);
        }
        catch (Exception)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<Result<UserSignInResponseDto>> SignInAsync(UserSignInDto dto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(dto.Email);
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
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

        var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.ExpirationDate);
        _cookieService.SetRefreshTokenCookie(refreshTokenDto);

        var responseDto = new UserSignInResponseDto(accessToken);
        return Result.Ok(responseDto);
    }

    public Result SignOut()
    {
        _cookieService.RemoveRefreshTokenCookie();
        return Result.Ok();
    }

    public async Task<Result<AccessTokenResponseDto>> RefreshAccessTokenAsync()
    {
        if (!_cookieService.TryGetRefreshTokenFromCookie(out var refreshToken))
        {
            SignOut();
            return Result.Fail(new InvalidOperationError("Invalid token."));
        }

        var validationResult = await _tokenService.ValidateRefreshToken(refreshToken);
        if (validationResult.IsFailed)
        {
            SignOut();
            return validationResult;
        }

        var userId = _userIdentityService.UserId;
        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        var accessToken = _tokenService.GenerateAccessToken(user);
        var responseDto = new AccessTokenResponseDto(accessToken);

        return Result.Ok(responseDto);
    }
}