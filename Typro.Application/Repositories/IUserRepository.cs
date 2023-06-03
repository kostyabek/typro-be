using Typro.Application.Models.User;
using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface IUserRepository
{
    Task<int> CreateUserAsync(CreateUserDto user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<string?> GetNicknameByIdAsync(int id);
    Task<int> UpdateNicknameByIdAsync(string nickname, int id);

    Task<IEnumerable<WordsPerMinuteToAccuracyDto>> GetWordsPerMinuteToAccuracyStatsAsync(
        WordsPerMinuteToAccuracyRequestDto dto);
}