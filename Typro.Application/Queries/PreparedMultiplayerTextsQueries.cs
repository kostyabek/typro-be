namespace Typro.Application.Queries;

public static class PreparedMultiplayerTextsQueries
{
    public const string GetByLobbyId = @"
SELECT LobbyId, [Text], TimeModeType, WordsModeType, LanguageId, IsPunctuationEnabled, AreNumbersEnabled
FROM dbo.PreparedMultiplayerTexts
WHERE LobbyId = @LobbyId;";

    public const string InsertNewText = @"
INSERT INTO dbo.PreparedMultiplayerTexts ([LobbyId], [Text], TimeModeType, WordsModeType, LanguageId, IsPunctuationEnabled, AreNumbersEnabled)
VALUES (@LobbyId, @Text, @TimeModeType, @WordsModeType, @LanguageId, @IsPunctuationEnabled, @AreNumbersEnabled);";
    
    public const string Update = @"
UPDATE dbo.PreparedMultiplayerTexts SET
    [Text] = @Text,
    TimeModeType = @TimeModeType,
    WordsModeType = @WordsModeType,
    LanguageId = @LanguageId,
    IsPunctuationEnabled = @IsPunctuationEnabled,
    AreNumbersEnabled = @AreNumbersEnabled
WHERE LobbyId = @LobbyId;";
    
    public const string Delete = @"
DELETE FROM dbo.PreparedMultiplayerTexts
WHERE LobbyId = @LobbyId;";
    
    public const string CheckIfLLobbyExists = @"
IF EXISTS (SELECT 1 FROM [dbo].[PreparedMultiplayerTexts] WHERE LobbyId = @LobbyId)
BEGIN
    SELECT CONVERT(bit, 1);
END
ELSE
BEGIN
    SELECT CONVERT(bit, 0);
END";
}