using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface IPreparedMultiplayerTextsRepository
{
    Task<PreparedMultiplayerTextInfo?> GetPreparedTextByLobbyId(string lobbyId);

    Task<int> Insert(PreparedMultiplayerTextInfo preparedMultiplayerText);
    Task<int> Update(PreparedMultiplayerTextInfo preparedMultiplayerText);
    Task<int> Delete(string lobbyId);
    Task<bool> CheckIfLobbyExists(string lobbyId);
}