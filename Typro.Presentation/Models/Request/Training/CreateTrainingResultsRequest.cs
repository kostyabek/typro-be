using Typro.Domain.Enums.Training;

namespace Typro.Presentation.Models.Request.Training;

public record CreateTrainingResultsRequest(
    int LanguageId,
    TimeModeType TimeModeType,
    WordsModeType WordsModeType,
    DateTime DateConducted);