using FluentResults;
using Typro.Application.Services.Training;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Services.Training;

public class WordsService(IUnitOfWork unitOfWork) : IWordsService
{
    public async Task<Result<IEnumerable<Word>>> GetNRandomWordsByLanguageAsync(int languageId, int numberOfWords)
    {
        IEnumerable<Word>? words = await unitOfWork.WordRepository.GetNRandomWordsByLanguageAsync(languageId, numberOfWords);
        return Result.Ok(words);
    }
}