using FluentResults;
using Typro.Application.Helpers;
using Typro.Application.Models.Auth;
using Typro.Application.Models.User;
using Typro.Application.Services.Auth;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Enums.User;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITrainingConfigurationService _trainingConfigurationService;
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;
    private readonly IUserIdentityService _userIdentityService;
    private readonly IUserService _userService;
    private readonly INicknameHelper _nicknameHelper;

    public AuthService(
        ITrainingConfigurationService trainingConfigurationService,
        ITokenService tokenService,
        ICookieService cookieService,
        IUserIdentityService userIdentityService,
        IUserService userService,
        IUnitOfWork unitOfWork,
        INicknameHelper nicknameHelper)
    {
        _trainingConfigurationService = trainingConfigurationService;
        _tokenService = tokenService;
        _cookieService = cookieService;
        _userIdentityService = userIdentityService;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _nicknameHelper = nicknameHelper;
    }

    public async Task<Result<UserAuthResponseDto>> SignUpAsync(UserSignUpDto dto)
    {
        var userResult = await _userService.GetUserByEmailAsync(dto.Email);
        if (!userResult.HasError<NotFoundError>())
        {
            return Result.Fail(new InvalidOperationError("The user already exists."));
        }

        var passwordHash = BC.HashPassword(dto.Password);

        _unitOfWork.BeginTransaction();
        try
        {
            var trainingConfigurationCreationResult =
                await _trainingConfigurationService.CreateDefaultTrainingConfigurationAsync();

            var createdDate = DateTime.UtcNow;
            var nickname = _nicknameHelper.GenerateNicknameFromDate(createdDate);
            var createUserModel =
                new CreateUserDto(dto.Email, passwordHash, UserRole.User, trainingConfigurationCreationResult.Value,
                    nickname, createdDate);

            var userCreationResult = await _userService.CreateUserAsync(createUserModel);

            userResult = await _userService.GetUserByEmailAsync(dto.Email);
            var user = userResult.Value;

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(userCreationResult.Value);
            _unitOfWork.CommitTransaction();

            var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.ExpirationDate);
            _cookieService.SetRefreshTokenCookie(refreshTokenDto);

            var responseDto = new UserAuthResponseDto(accessToken, user.Email);
            return Result.Ok(responseDto);
        }
        catch (Exception)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<Result<UserAuthResponseDto>> SignInAsync(UserSignInDto dto)
    {
        var userResult = await _userService.GetUserByEmailAsync(dto.Email);
        if (userResult.HasError<NotFoundError>())
        {
            return Result.Fail(new InvalidOperationError("Invalid login/password."));
        }

        var user = userResult.Value;

        var isValidPassword = BC.Verify(dto.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            return Result.Fail(new InvalidOperationError("Invalid login/password."));
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

        var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.ExpirationDate);
        _cookieService.SetRefreshTokenCookie(refreshTokenDto);

        var responseDto = new UserAuthResponseDto(accessToken, user.Email);
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
        var userResult = await _userService.GetUserByIdAsync(userId);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        var accessToken = _tokenService.GenerateAccessToken(userResult.Value);
        var responseDto = new AccessTokenResponseDto(accessToken);

        return Result.Ok(responseDto);
    }
}