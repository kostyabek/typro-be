using Typro.Domain.Enums.Training;
using Typro.Domain.Models.Training;

namespace Typro.Application.Models.Training;

public record FullTrainingResultsDto(
    float WordsPerMinute,
    float Accuracy,
    int TimeInMilliseconds,
    CharacterStats CharactersStats,
    int LanguageId,
    TimeModeType TimeModeType,
    WordsModeType WordsModeType,
    DateTime DateConducted,
    int UserId);