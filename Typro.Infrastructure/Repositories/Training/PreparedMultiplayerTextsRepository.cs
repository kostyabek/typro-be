using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Models.Training;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Repositories.Training;

public class PreparedMultiplayerTextsRepository : DatabaseConnectable, IPreparedMultiplayerTextsRepository
{
    public PreparedMultiplayerTextsRepository(ConnectionWrapper connectionWrapper) : base(connectionWrapper)
    {
    }

    public Task<PreparedMultiplayerTextInfo?> GetPreparedTextByLobbyId(string lobbyId)
        => ConnectionWrapper.Connection.QuerySingleOrDefaultAsync<PreparedMultiplayerTextInfo?>(
            PreparedMultiplayerTextsQueries.GetByLobbyId,
            new
            {
                LobbyId = lobbyId
            },
            ConnectionWrapper.Transaction);

    public Task<int> Insert(PreparedMultiplayerTextInfo preparedMultiplayerText)
        => ConnectionWrapper.Connection.ExecuteAsync(
            PreparedMultiplayerTextsQueries.InsertNewText,
            new
            {
                preparedMultiplayerText.Text,
                preparedMultiplayerText.LobbyId,
                preparedMultiplayerText.LanguageId,
                preparedMultiplayerText.TimeModeType,
                preparedMultiplayerText.WordsModeType,
                preparedMultiplayerText.IsPunctuationEnabled,
                preparedMultiplayerText.AreNumbersEnabled
            },
            ConnectionWrapper.Transaction);

    public Task<int> Update(PreparedMultiplayerTextInfo preparedMultiplayerText)
        => ConnectionWrapper.Connection.ExecuteAsync(
            PreparedMultiplayerTextsQueries.Update,
            new
            {
                preparedMultiplayerText.Text,
                preparedMultiplayerText.LobbyId,
                preparedMultiplayerText.LanguageId,
                preparedMultiplayerText.TimeModeType,
                preparedMultiplayerText.WordsModeType,
                preparedMultiplayerText.IsPunctuationEnabled,
                preparedMultiplayerText.AreNumbersEnabled
            },
            ConnectionWrapper.Transaction);

    public Task<int> Delete(string lobbyId)
        => ConnectionWrapper.Connection.ExecuteAsync(
            PreparedMultiplayerTextsQueries.Delete,
            new
            {
                LobbyId = lobbyId
            },
            ConnectionWrapper.Transaction);
    
    public Task<bool> CheckIfLobbyExists(string lobbyId)
        => ConnectionWrapper.Connection.ExecuteScalarAsync<bool>(
            PreparedMultiplayerTextsQueries.CheckIfLLobbyExists,
            new
            {
                LobbyId = lobbyId
            },
            ConnectionWrapper.Transaction);
}