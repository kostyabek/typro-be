using Typro.Domain.Enums.Training;
using Typro.Presentation.Models.Request.Base;

namespace Typro.Presentation.Models.Request.Leaderboard;

public record GetLeaderboardRequest(
    TimeModeType TimeModeType,
    WordsModeType WordsModeType,
    int LanguageId,
    DateTime FromDate,
    DateTime ToDate,
    int PageNumber,
    int PageSize) : PaginatedRequest(PageNumber, PageSize);