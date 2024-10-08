﻿using FluentResults;
using Typro.Application.Models.User;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.User;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Result<int>> CreateUserAsync(CreateUserDto model)
    {
        int generatedUserId = await unitOfWork.UserRepository.CreateUserAsync(model);
        return Result.Ok(generatedUserId);
    }

    public async Task<Result<Domain.Database.Models.User>> GetUserByEmailAsync(string email)
    {
        Domain.Database.Models.User? user = await unitOfWork.UserRepository.GetUserByEmailAsync(email);
        return user is null ? Result.Fail(new NotFoundError("User not found")) : Result.Ok(user);
    }

    public async Task<Result<Domain.Database.Models.User>> GetUserByIdAsync(int id)
    {
        Domain.Database.Models.User? user = await unitOfWork.UserRepository.GetUserByIdAsync(id);
        return user is null ? Result.Fail(new NotFoundError("User not found")) : Result.Ok(user);
    }

    public async Task<Result<string>> UpdateNicknameByIdAsync(string nickname, int userId)
    {
        int rowsAffected = await unitOfWork.UserRepository.UpdateNicknameByIdAsync(nickname, userId);

        return rowsAffected == 0 ? Result.Fail(new NotFoundError("User not found")) : Result.Ok(nickname);
    }

    public async Task<Result<string>> GetNicknameByIdAsync(int id)
    {
        string? nickname = await unitOfWork.UserRepository.GetNicknameByIdAsync(id);
        return nickname is null ? Result.Fail(new NotFoundError("User not found")) : Result.Ok(nickname);
    }

    public async Task<Result<IEnumerable<WordsPerMinuteToAccuracyDto>>> GetWordsPerMinuteToAccuracyStatsAsync(
        WordsPerMinuteToAccuracyRequestDto dto)
    {
        IEnumerable<WordsPerMinuteToAccuracyDto> dtos =
            await unitOfWork.UserRepository.GetWordsPerMinuteToAccuracyStatsAsync(dto);
        return Result.Ok(dtos);
    }
}