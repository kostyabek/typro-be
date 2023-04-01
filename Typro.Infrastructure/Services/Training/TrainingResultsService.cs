using FluentResults;
using Typro.Application.Models.Training;
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
    
    public async Task<Result<int>> SaveTrainingResultsAsync(TrainingResultsDto dto)
    {
        var generatedTrainingConfigurationId =
            await _unitOfWork.TrainingResultsRepository.CreateTrainingResultsAsync(dto);

        return Result.Ok(generatedTrainingConfigurationId);
    }
}