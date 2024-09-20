using FluentResults;
using Typro.Domain.Database.Models;

namespace Typro.Application.Services.Training;

public interface IWordsService
{
    Task<Result<IEnumerable<Word>>> GetNRandomWordsByLanguageAsync(int languageId, int numberOfWords);
}