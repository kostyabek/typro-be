namespace Typro.Application.Queries;

public static class WordsQueries
{
    public const string GetNRandomWordsByLanguage = @"
SELECT TOP (@NumberOfWords)
    Id,
    Name,
    LanguageId
FROM dbo.Words
WHERE LanguageId = @LanguageId
ORDER BY NEWID();";

    public const string InsertNewWord = @"
INSERT INTO dbo.Words (Name, LanguageId) VALUES (@Name, @LanguageId)";
}