using FluentResults;
using Typro.Application.Models.Training;

namespace Typro.Application.Services.Training;

public interface ITrainingResultsService
{
    Task<Result<int>> SaveTrainingResultsAsync(TrainingResultsDto dto);
}