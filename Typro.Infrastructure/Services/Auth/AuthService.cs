using FluentResults;
using Typro.Application.Helpers;
using Typro.Application.Models.Auth;
using Typro.Application.Models.User;
using Typro.Application.Services.Auth;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;
using Typro.Domain.Enums.User;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.Auth;

public class AuthService(
    ITrainingConfigurationService trainingConfigurationService,
    ITokenService tokenService,
    ICookieService cookieService,
    IUserIdentityService userIdentityService,
    IUserService userService,
    IUnitOfWork unitOfWork,
    INicknameHelper nicknameHelper) : IAuthService
{
    public async Task<Result<UserAuthResponseDto>> SignUpAsync(UserSignUpDto dto)
    {
        Result<Domain.Database.Models.User>? userResult = await userService.GetUserByEmailAsync(dto.Email);
        if (!userResult.HasError<NotFoundError>())
        {
            return Result.Fail(new InvalidOperationError("The user already exists."));
        }

        string? passwordHash = BC.HashPassword(dto.Password);

        unitOfWork.BeginTransaction();
        try
        {
            Result<int>? trainingConfigurationCreationResult =
                await trainingConfigurationService.CreateDefaultTrainingConfigurationAsync();

            DateTime createdDate = DateTime.UtcNow;
            string? nickname = nicknameHelper.GenerateNicknameFromDate(createdDate);
            var createUserModel =
                new CreateUserDto(dto.Email, passwordHash, UserRole.User, trainingConfigurationCreationResult.Value,
                    nickname, createdDate);

            Result<int>? userCreationResult = await userService.CreateUserAsync(createUserModel);

            userResult = await userService.GetUserByEmailAsync(dto.Email);
            Domain.Database.Models.User? user = userResult.Value;

            string? accessToken = tokenService.GenerateAccessToken(user);
            RefreshToken? refreshToken = await tokenService.GenerateRefreshTokenAsync(userCreationResult.Value);
            unitOfWork.CommitTransaction();

            var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.ExpirationDate);
            cookieService.SetRefreshTokenCookie(refreshTokenDto);

            var responseDto = new UserAuthResponseDto(accessToken, user.Email, user.Nickname);
            return Result.Ok(responseDto);
        }
        catch (Exception)
        {
            unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<Result<UserAuthResponseDto>> SignInAsync(UserSignInDto dto)
    {
        Result<Domain.Database.Models.User>? userResult = await userService.GetUserByEmailAsync(dto.Email);
        if (userResult.HasError<NotFoundError>())
        {
            return Result.Fail(new InvalidOperationError("Invalid login/password."));
        }

        Domain.Database.Models.User? user = userResult.Value;

        bool isValidPassword = BC.Verify(dto.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            return Result.Fail(new InvalidOperationError("Invalid login/password."));
        }

        string? accessToken = tokenService.GenerateAccessToken(user);
        RefreshToken? refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id);

        var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.ExpirationDate);
        cookieService.SetRefreshTokenCookie(refreshTokenDto);

        var responseDto = new UserAuthResponseDto(accessToken, user.Email, user.Nickname);
        return Result.Ok(responseDto);
    }

    public Result SignOut()
    {
        cookieService.RemoveRefreshTokenCookie();
        return Result.Ok();
    }

    public async Task<Result<AccessTokenResponseDto>> RefreshAccessTokenAsync()
    {
        if (!cookieService.TryGetRefreshTokenFromCookie(out string? refreshToken))
        {
            SignOut();
            return Result.Fail(new InvalidOperationError("Invalid token."));
        }

        Result? validationResult = await tokenService.ValidateRefreshToken(refreshToken);
        if (validationResult.IsFailed)
        {
            SignOut();
            return validationResult;
        }

        int userId = userIdentityService.UserId;
        Result<Domain.Database.Models.User>? userResult = await userService.GetUserByIdAsync(userId);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        string? accessToken = tokenService.GenerateAccessToken(userResult.Value);
        var responseDto = new AccessTokenResponseDto(accessToken);

        return Result.Ok(responseDto);
    }
}