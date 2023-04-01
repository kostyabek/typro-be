using Typro.Domain.Enums.Training;
using Typro.Domain.Models.Training;

namespace Typro.Presentation.Models.Request.Training;

public record SetTrainingResultsRequest(
    float WordsPerMinute,
    float Accuracy,
    int TimeInMilliseconds,
    CharacterStats CharactersStats,
    int LanguageId,
    TimeModeType TimeModeType,
    WordsModeType WordsModeType,
    DateTime DateConducted);