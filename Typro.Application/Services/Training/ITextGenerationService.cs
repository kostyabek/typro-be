using FluentResults;
using Typro.Application.Models.Training;

namespace Typro.Application.Services.Training;

public interface ITextGenerationService
{
    Task<Result<IEnumerable<string>>> GenerateText(TrainingConfigurationDto dto);

    Result<IEnumerable<IEnumerable<char>>> ConvertWordsToSymbols(IEnumerable<string> words);
}