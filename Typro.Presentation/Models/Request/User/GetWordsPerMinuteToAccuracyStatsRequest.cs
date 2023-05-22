using Typro.Domain.Enums.Training;

namespace Typro.Presentation.Models.Request.User;

public record GetWordsPerMinuteToAccuracyStatsRequest(DateTime FromDate, int LanguageId, WordsModeType WordsModeType,
    TimeModeType TimeModeType);