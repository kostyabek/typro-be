using Typro.Domain.Enums.Training;

namespace Typro.Presentation.Models.Request.Training;

public record UpdateTrainingConfigurationRequest(
    bool IsPunctuationEnabled,
    bool AreNumbersEnabled,
    WordsModeType WordsModeType,
    TimeModeType TimeModeType,
    int LanguageId);