namespace Typro.Application.Queries;

public static class TrainingResultsQueries
{
    public const string InsertTrainingResults = @"
INSERT INTO dbo.TrainingResults(UserId, WordsPerMinute, Accuracy, TimeInMilliseconds, CorrectLetters, IncorrectLetters, ExtraLetters, InitialLetters, WordsModeType, TimeModeType, LanguageId, DateConducted)
OUTPUT INSERTED.Id
VALUES (@UserId, @WordsPerMinute, @Accuracy, @TimeInMilliseconds, @CorrectLetters, @IncorrectLetters, @ExtraLetters, @InitialLetters, @WordsModeType, @TimeModeType, @LanguageId, @DateConducted);";
}
