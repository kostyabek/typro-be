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
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
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
            await _userRepository.CreateUserAsync(createUserModel);

            user = await _userRepository.GetUserByEmailAsync(dto.Email);
        
            var jwt = _jwtService.GenerateToken(user);
            transaction.Commit();
            var responseDto = new UserSignUpResponseDto(jwt);

            return Result.Ok(responseDto);
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return Result.Fail(e.Message);
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
        
        var jwt = _jwtService.GenerateToken(user);

        var responseDto = new UserSignInResponseDto(jwt);

        return Result.Ok(responseDto);
    }
}