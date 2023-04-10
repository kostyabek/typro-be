using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface ISupportedLanguagesRepository
{
    Task<IEnumerable<SupportedLanguage>> GetSupportedLanguagesAsync();
}