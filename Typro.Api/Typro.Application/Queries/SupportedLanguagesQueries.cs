namespace Typro.Application.Queries;

public static class SupportedLanguagesQueries
{
    public const string GetSupportedLanguages = @"
SELECT Id, Name
FROM dbo.SupportedLanguages;";
}