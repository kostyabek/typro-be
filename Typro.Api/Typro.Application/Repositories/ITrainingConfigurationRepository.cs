using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface ITrainingConfigurationRepository
{
    Task<int> CreateDefaultTrainingConfigurationAsync();
    Task<TrainingConfiguration?> GetTrainingConfigurationByIdAsync(int id);
    Task<int> UpdateTrainingConfigurationAsync(TrainingConfiguration trainingConfiguration);
}