using Typro.Domain.Enums.Training;

namespace Typro.Application.Models.Leaderboard;

public record LeaderboardFilterDto(
    TimeModeType TimeModeType,
    WordsModeType WordsModeType,
    int LanguageId,
    DateTime FromDate,
    DateTime ToDate);