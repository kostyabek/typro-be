using Typro.Application.Models.Training;
using Typro.Application.Models.User;

namespace Typro.Application.Repositories;

public interface ITrainingResultsRepository
{
    Task<int> CreateTrainingResultsAsync(FullTrainingResultsDto dto);
    Task<int> UpdateTrainingResultsAsync(UpdateTrainingResultsDto dto);
    Task<HighLevelProfileInfoDto> GetTrainingCountAsync(int userId);
    Task<IEnumerable<HighLevelTrainingResultDto>> GetBestResultsAsync(int userId);
}