namespace Typro.Application.Queries;

public static class TrainingConfigurationQueries
{
    public const string InsertDefaultTrainingConfiguration = @"
INSERT INTO dbo.TrainingConfigurations(IsPunctuationEnabled, AreNumbersEnabled, WordsModeType, TimeModeType, LanguageId)
OUTPUT INSERTED.Id
VALUES (0, 0, 25, 0, 1);";

    public const string GetTrainingConfigurationById = @"
SELECT
    Id,
    IsPunctuationEnabled,
    AreNumbersEnabled,
    WordsModeType,
    TimeModeType,
    LanguageId
FROM dbo.TrainingConfigurations
WHERE Id = @Id;";

    public const string UpdateTrainingConfiguration = @"
UPDATE dbo.TrainingConfigurations
SET IsPunctuationEnabled = @IsPunctuationEnabled,
    AreNumbersEnabled = @AreNumbersEnabled,
    WordsModeType = @WordsModeType,
    TimeModeType = @TimeModeType,
    LanguageId = @LanguageId
WHERE Id = @Id";
}