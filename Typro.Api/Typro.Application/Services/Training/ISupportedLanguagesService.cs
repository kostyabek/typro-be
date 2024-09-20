using FluentResults;
using Typro.Application.Models.Training;

namespace Typro.Application.Services.Training;

public interface ISupportedLanguagesService
{
    Task<Result<IEnumerable<SupportedLanguageDto>>> GetSupportedLanguagesAsync();
}