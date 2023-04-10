using Typro.Domain.Enums.Training;

namespace Typro.Application.Models.Training;

public record TrainingConfigurationDto(
    bool IsPunctuationEnabled,
    bool AreNumbersEnabled,
    WordsModeType WordsMode,
    TimeModeType TimeMode,
    int LanguageId);