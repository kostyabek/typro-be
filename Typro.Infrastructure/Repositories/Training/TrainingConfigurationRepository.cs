using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Repositories.Training;

public class TrainingConfigurationRepository : DatabaseConnectable, ITrainingConfigurationRepository
{
    public TrainingConfigurationRepository(ConnectionWrapper connectionWrapper) : base(connectionWrapper)
    {
    }

    public Task<int> CreateDefaultTrainingConfigurationAsync()
        => ConnectionWrapper.Connection.ExecuteAsync(TrainingConfigurationQueries.InsertDefaultTrainingConfiguration,
            transaction: ConnectionWrapper.Transaction);

    public Task<TrainingConfiguration?> GetTrainingConfigurationByIdAsync(int id)
        => ConnectionWrapper.Connection.QuerySingleAsync<TrainingConfiguration?>(
            TrainingConfigurationQueries.GetTrainingConfigurationById,
            new { Id = id },
            transaction: ConnectionWrapper.Transaction);

    public Task<int> UpdateTrainingConfigurationAsync(TrainingConfiguration trainingConfiguration)
        => ConnectionWrapper.Connection.ExecuteAsync(TrainingConfigurationQueries.UpdateTrainingConfiguration,
            trainingConfiguration,
            transaction: ConnectionWrapper.Transaction);
}