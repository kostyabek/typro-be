using FluentResults;
using Typro.Application.Models.Training;
using Typro.Domain.Database.Models;

namespace Typro.Application.Services.Training;

public interface ITrainingConfigurationService
{
    Task<Result<int>> CreateDefaultTrainingConfigurationAsync();
    Task<Result<TrainingConfiguration>> GetTrainingConfigurationByIdAsync(int trainingConfigurationId);
    Task<Result> UpdateTrainingConfigurationAsync(TrainingConfigurationDto dto);
}