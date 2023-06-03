using FluentResults;
using Typro.Application.Models.User;

namespace Typro.Application.Services.User;

public interface IUserService
{
    Task<Result<int>> CreateUserAsync(CreateUserDto model);
    Task<Result<Domain.Database.Models.User>> GetUserByEmailAsync(string email);
    Task<Result<Domain.Database.Models.User>> GetUserByIdAsync(int id);
    Task<Result<string>> GetNicknameByIdAsync(int id);
    Task<Result<string>> UpdateNicknameByIdAsync(string nickname, int id);

    Task<Result<IEnumerable<WordsPerMinuteToAccuracyDto>>> GetWordsPerMinuteToAccuracyStatsAsync(
        WordsPerMinuteToAccuracyRequestDto dto);
}