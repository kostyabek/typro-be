using Typro.Domain.Enums.Training;

namespace Typro.Presentation.Models.Request.Leaderboard;

public record GetLeaderboardRequest(
    TimeModeType TimeModeType,
    WordsModeType WordsModeType,
    int LanguageId,
    DateTime FromDate,
    DateTime ToDate);