using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Models.Training;
using Typro.Application.Queries;
using Typro.Application.Repositories;

namespace Typro.Infrastructure.Repositories.Training;

public class TrainingResultsRepository : DatabaseConnectable, ITrainingResultsRepository
{
    public TrainingResultsRepository(ConnectionWrapper connectionWrapper) : base(connectionWrapper)
    {
    }
    
    public Task<int> CreateTrainingResultsAsync(TrainingResultsDto dto)
        => ConnectionWrapper.Connection.ExecuteAsync(TrainingResultsQueries.InsertTrainingResults,
            new
            {
                dto.WordsPerMinute,
                dto.Accuracy,
                dto.TimeInMilliseconds,
                dto.LanguageId,
                dto.TimeModeType,
                dto.WordsModeType,
                dto.DateConducted,
                CorrectLetters = dto.CharactersStats.Correct,
                IncorrectLetters = dto.CharactersStats.Incorrect,
                ExtraLetters = dto.CharactersStats.Extra,
                InitialLetters = dto.CharactersStats.Initial,
                dto.UserId
            },
            transaction: ConnectionWrapper.Transaction);
}