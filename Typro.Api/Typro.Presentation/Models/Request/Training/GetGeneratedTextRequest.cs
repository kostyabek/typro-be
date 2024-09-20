using Typro.Domain.Enums.Training;

namespace Typro.Presentation.Models.Request.Training;

public record GetGeneratedTextRequest(
    int LanguageId,
    TimeModeType TimeMode,
    WordsModeType WordsMode,
    bool IsPunctuationGenerated,
    bool AreNumbersGenerated);