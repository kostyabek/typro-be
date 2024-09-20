using Typro.Domain.Database.Models;

namespace Typro.Application.Repositories;

public interface IWordRepository
{
    Task<IEnumerable<Word>> GetNRandomWordsByLanguageAsync(int languageId, int numberOfWords);
}