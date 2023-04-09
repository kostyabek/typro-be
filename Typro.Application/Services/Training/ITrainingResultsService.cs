using FluentResults;
using Typro.Application.Models.Training;
using Typro.Application.Models.User;

namespace Typro.Application.Services.Training;

public interface ITrainingResultsService
{
    Task<Result<int>> CreateTrainingResultsAsync(FullTrainingResultsDto dto);
    Task<Result<int>> UpdateTrainingResultsAsync(UpdateTrainingResultsDto dto);
    Task<Result<HighLevelProfileInfoDto>> GetHighLevelProfileInfoAsync(int userId);
    Task<Result<IEnumerable<HighLevelTrainingResultDto>>> GetHighLevelTrainingResultsAsync(int userId);
}