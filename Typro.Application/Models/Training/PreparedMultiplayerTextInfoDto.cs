using Typro.Domain.Enums.Training;

namespace Typro.Application.Models.Training;

public record PreparedMultiplayerTextInfoDto(
    IEnumerable<IEnumerable<char>> Symbols,
    bool IsPunctuationEnabled,
    bool AreNumbersEnabled,
    WordsModeType WordsMode,
    TimeModeType TimeMode,
    int LanguageId);