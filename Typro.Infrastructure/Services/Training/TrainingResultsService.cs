using FluentResults;
using Typro.Application.Models.Training;
using Typro.Application.Models.User;
using Typro.Application.Services.Training;
using Typro.Application.UnitsOfWork;

namespace Typro.Infrastructure.Services.Training;

public class TrainingResultsService : ITrainingResultsService
{
    private readonly IUnitOfWork _unitOfWork;

    public TrainingResultsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> CreateTrainingResultsAsync(FullTrainingResultsDto dto)
    {
        int generatedTrainingConfigurationId =
            await _unitOfWork.TrainingResultsRepository.CreateTrainingResultsAsync(dto);

        return Result.Ok(generatedTrainingConfigurationId);
    }

    public async Task<Result<int>> UpdateTrainingResultsAsync(UpdateTrainingResultsDto dto)
    {
        int generatedTrainingConfigurationId =
            await _unitOfWork.TrainingResultsRepository.UpdateTrainingResultsAsync(dto);

        return Result.Ok(generatedTrainingConfigurationId);
    }

    public async Task<Result<HighLevelProfileInfoDto>> GetHighLevelProfileInfoAsync(int userId)
    {
        HighLevelProfileInfoDto highLevelProfileInfo =
            await _unitOfWork.TrainingResultsRepository.GetTrainingCountAsync(userId);

        return Result.Ok(highLevelProfileInfo);
    }

    public async Task<Result<IEnumerable<HighLevelTrainingResultDto>>> GetHighLevelTrainingResultsAsync(int userId)
    {
        IEnumerable<HighLevelTrainingResultDto> highLevelTrainingResults =
            await _unitOfWork.TrainingResultsRepository.GetBestResultsAsync(userId);

        return Result.Ok(highLevelTrainingResults);
    }
}