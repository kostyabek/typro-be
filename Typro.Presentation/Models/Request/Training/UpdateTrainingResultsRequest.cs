using Typro.Domain.Models.Training;

namespace Typro.Presentation.Models.Request.Training;

public record UpdateTrainingResultsRequest(
    float WordsPerMinute,
    float Accuracy,
    int TimeInMilliseconds,
    CharacterStats CharactersStats);