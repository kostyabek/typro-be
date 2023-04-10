using FluentResults;
using Typro.Application.Models.Training;

namespace Typro.Application.Services.Training;

public interface ITextGenerationService
{
    Task<Result<IEnumerable<IEnumerable<char>>>> GenerateText(TrainingConfigurationDto dto);
}