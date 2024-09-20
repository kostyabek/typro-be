using Typro.Domain.Enums.Training;

namespace Typro.Application.Models.Training;

public record HighLevelTrainingResultDto(
    WordsModeType WordsModeType,
    TimeModeType TimeModeType,
    double WordsPerMinute,
    double Accuracy,
    DateTime DateConducted);
