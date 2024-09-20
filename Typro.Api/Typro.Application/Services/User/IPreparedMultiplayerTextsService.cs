using FluentResults;
using Typro.Application.Models.Training;

namespace Typro.Application.Services.User;

public interface IPreparedMultiplayerTextsService
{
    Task<Result<PreparedMultiplayerTextInfoDto>> GetOrCreateTrainingTextAsync(
        TrainingConfigurationDto dto,
        string lobbyId,
        bool isForceRewrite);
    Task<Result> DeleteLobby(string lobbyId);
    Task<Result<bool>> CheckIfLobbyExists(string lobbyId);
}