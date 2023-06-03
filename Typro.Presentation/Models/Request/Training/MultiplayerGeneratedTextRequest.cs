using Typro.Domain.Enums.Training;

namespace Typro.Presentation.Models.Request.Training;

public record MultiplayerGeneratedTextRequest(
    int LanguageId,
    TimeModeType TimeMode,
    WordsModeType WordsMode,
    bool IsPunctuationGenerated,
    bool AreNumbersGenerated,
    string LobbyId): GetGeneratedTextRequest(LanguageId, TimeMode, WordsMode, IsPunctuationGenerated, AreNumbersGenerated);