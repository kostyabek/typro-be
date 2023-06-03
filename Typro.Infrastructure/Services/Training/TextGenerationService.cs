using System.Text;
using FluentResults;
using Typro.Application.Models.Training;
using Typro.Application.Services.Training;
using Typro.Domain.Database.Models;
using Typro.Domain.Enums.Training;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.Training;

public class TextGenerationService : ITextGenerationService
{
    private readonly char[] _punctuationSymbols = { '.', ',', '!', '?', ';', ':' };
    private readonly ISupportedLanguagesService _supportedLanguagesService;
    private readonly IWordsService _wordsService;

    public TextGenerationService(
        ISupportedLanguagesService supportedLanguagesService,
        IWordsService wordsService)
    {
        _supportedLanguagesService = supportedLanguagesService;
        _wordsService = wordsService;
    }

    public async Task<Result<IEnumerable<string>>> GenerateText(TrainingConfigurationDto dto)
    {
        Result<IEnumerable<SupportedLanguageDto>> supportedLanguagesResult =
            await _supportedLanguagesService.GetSupportedLanguagesAsync();
        if (supportedLanguagesResult.IsFailed)
        {
            return Result.Fail(supportedLanguagesResult.Errors);
        }

        IEnumerable<SupportedLanguageDto>? supportedLanguages = supportedLanguagesResult.Value;
        SupportedLanguageDto? targetLanguage = supportedLanguages.SingleOrDefault(e => e.Id == dto.LanguageId);
        if (targetLanguage is null)
        {
            return Result.Fail(new NotFoundError("Could not find specified language."));
        }

        int numberOfWords;
        if (dto.WordsMode != WordsModeType.TurnedOff)
        {
            numberOfWords = (int)dto.WordsMode;
        }
        else
        {
            numberOfWords = 150;
        }

        Result<IEnumerable<Word>> wordsResult =
            await _wordsService.GetNRandomWordsByLanguageAsync(dto.LanguageId, numberOfWords);

        List<string> words = wordsResult
            .Value
            .Select(w => w.Name)
            .ToList();

        var random = new Random();
        if (dto.AreNumbersEnabled)
        {
            for (var i = 0; i < words.Count; i++)
            {
                int index = random.Next(0, words.Count);
                bool shouldInsertNumber = random.Next(0, 10) is >= 3 and <= 6;
                if (!shouldInsertNumber)
                {
                    continue;
                }

                int randomNumber = random.Next(0, 101);
                words[index] = randomNumber.ToString();
            }
        }

        if (dto.IsPunctuationEnabled)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < words.Count; i++)
            {
                stringBuilder.Clear();

                bool shouldInsertPunctuation = random.Next(0, 10) is >= 0 and < 2 or 7;
                if (!shouldInsertPunctuation)
                {
                    continue;
                }

                char punctuationSymbol = _punctuationSymbols[random.Next(0, _punctuationSymbols.Length)];
                stringBuilder.Append(words[i]).Append(punctuationSymbol);
                words[i] = stringBuilder.ToString();
            }
        }

        return Result.Ok(words.AsEnumerable());
    }

    public Result<IEnumerable<IEnumerable<char>>> ConvertWordsToSymbols(IEnumerable<string> words)
    {
        List<string> wordsList = words.ToList();

        var result = new List<IEnumerable<char>>();
        foreach (string word in wordsList)
        {
            List<char> symbols = word.ToList();
            result.Add(symbols);
        }

        return Result.Ok(result.AsEnumerable());
    }
}
