using Typro.Application.Models.Training;

namespace Typro.Application.Repositories;

public interface ITrainingResultsRepository
{
    Task<int> CreateTrainingResultsAsync(TrainingResultsDto dto);
}