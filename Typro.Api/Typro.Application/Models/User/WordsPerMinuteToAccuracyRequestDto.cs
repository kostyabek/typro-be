using Typro.Domain.Enums.Training;

namespace Typro.Application.Models.User;

public record WordsPerMinuteToAccuracyRequestDto(int UserId, DateTime FromDate, int LanguageId,
    WordsModeType WordsModeType, TimeModeType TimeModeType);