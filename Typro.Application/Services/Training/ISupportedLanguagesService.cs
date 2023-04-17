using FluentResults;
using Typro.Application.Models.Training;
using Typro.Domain.Database.Models;

namespace Typro.Application.Services.Training;

public interface ISupportedLanguagesService
{
    Task<Result<IEnumerable<SupportedLanguageDto>>> GetSupportedLanguagesAsync();
}