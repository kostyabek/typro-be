using Typro.Domain.Models.Training;

namespace Typro.Application.Models.Training;

public record UpdateTrainingResultsDto(
    int Id,
    float WordsPerMinute,
    float Accuracy,
    int TimeInMilliseconds,
    CharacterStats CharactersStats);