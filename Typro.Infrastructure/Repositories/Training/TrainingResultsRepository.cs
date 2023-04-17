using System.Data;
using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Models.Leaderboard;
using Typro.Application.Models.Training;
using Typro.Application.Models.User;
using Typro.Application.Queries;
using Typro.Application.Repositories;

namespace Typro.Infrastructure.Repositories.Training;

public class TrainingResultsRepository : DatabaseConnectable, ITrainingResultsRepository
{
    public TrainingResultsRepository(ConnectionWrapper connectionWrapper) : base(connectionWrapper)
    {
    }

    public Task<int> CreateTrainingResultsAsync(FullTrainingResultsDto dto)
        => ConnectionWrapper.Connection.ExecuteScalarAsync<int>(TrainingResultsQueries.InsertTrainingResults,
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

    public Task<int> UpdateTrainingResultsAsync(UpdateTrainingResultsDto dto)
        => ConnectionWrapper.Connection.ExecuteAsync(TrainingResultsQueries.UpdateTrainingResults,
            new
            {
                dto.Id,
                dto.WordsPerMinute,
                dto.Accuracy,
                dto.TimeInMilliseconds,
                CorrectLetters = dto.CharactersStats.Correct,
                IncorrectLetters = dto.CharactersStats.Incorrect,
                ExtraLetters = dto.CharactersStats.Extra,
                InitialLetters = dto.CharactersStats.Initial
            },
            transaction: ConnectionWrapper.Transaction);

    public Task<HighLevelProfileInfoDto> GetTrainingCountAsync(int userId)
        => ConnectionWrapper.Connection.QuerySingleAsync<HighLevelProfileInfoDto>(
            "dbo.ProfileHighLevelInfo",
            commandType: CommandType.StoredProcedure,
            param: new { UserId = userId },
            transaction: ConnectionWrapper.Transaction);
    
    public Task<IEnumerable<HighLevelTrainingResultDto>> GetBestResultsAsync(int userId)
        => ConnectionWrapper.Connection.QueryAsync<HighLevelTrainingResultDto>(
            "dbo.BestResults",
            commandType: CommandType.StoredProcedure,
            param: new { UserId = userId },
            transaction: ConnectionWrapper.Transaction);
    
    public Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardAsync(LeaderboardFilterDto dto)
        => ConnectionWrapper.Connection.QueryAsync<LeaderboardEntryDto>(
            "dbo.Leaderboard",
            commandType: CommandType.StoredProcedure,
            param: dto,
            transaction: ConnectionWrapper.Transaction);
}