using FluentResults;
using Typro.Domain.Database.Models;

namespace Typro.Application.Services.Training;

public interface ISupportedLanguagesService
{
    Task<Result<IEnumerable<SupportedLanguage>>> GetSupportedLanguagesAsync();
}