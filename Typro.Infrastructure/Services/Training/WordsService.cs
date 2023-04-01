using FluentResults;
using Typro.Application.Services.Training;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Services.Training;

public class WordsService : IWordsService
{
    private readonly IUnitOfWork _unitOfWork;

    public WordsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<Word>>> GetNRandomWordsByLanguageAsync(int languageId, int numberOfWords)
    {
        var words = await _unitOfWork.WordRepository.GetNRandomWordsByLanguageAsync(languageId, numberOfWords);
        return Result.Ok(words);
    }
}