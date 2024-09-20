using FluentResults;
using Typro.Application.Models.Leaderboard;
using Typro.Application.Models.Training;
using Typro.Application.Models.User;
using Typro.Application.Services.Training;
using Typro.Application.UnitsOfWork;

namespace Typro.Infrastructure.Services.Training;

public class TrainingResultsService(IUnitOfWork unitOfWork) : ITrainingResultsService
{
    public async Task<Result<int>> CreateTrainingResultsAsync(FullTrainingResultsDto dto)
    {
        int generatedTrainingConfigurationId =
            await unitOfWork.TrainingResultsRepository.CreateTrainingResultsAsync(dto);

        return Result.Ok(generatedTrainingConfigurationId);
    }

    public async Task<Result<int>> UpdateTrainingResultsAsync(UpdateTrainingResultsDto dto)
    {
        int generatedTrainingConfigurationId =
            await unitOfWork.TrainingResultsRepository.UpdateTrainingResultsAsync(dto);

        return Result.Ok(generatedTrainingConfigurationId);
    }

    public async Task<Result<HighLevelProfileInfoDto>> GetHighLevelProfileInfoAsync(int userId)
    {
        HighLevelProfileInfoDto highLevelProfileInfo =
            await unitOfWork.TrainingResultsRepository.GetTrainingCountAsync(userId);

        return Result.Ok(highLevelProfileInfo);
    }

    public async Task<Result<IEnumerable<HighLevelTrainingResultDto>>> GetHighLevelTrainingResultsAsync(int userId)
    {
        IEnumerable<HighLevelTrainingResultDto> highLevelTrainingResults =
            await unitOfWork.TrainingResultsRepository.GetBestResultsAsync(userId);

        return Result.Ok(highLevelTrainingResults);
    }
    
    public async Task<Result<IEnumerable<LeaderboardEntryDto>>> GetLeaderboardAsync(LeaderboardFilterDto dto)
    {
        IEnumerable<LeaderboardEntryDto> leaderboardEntries =
            await unitOfWork.TrainingResultsRepository.GetLeaderboardAsync(dto);

        return Result.Ok(leaderboardEntries);
    }
}