using FluentResults;
using Typro.Application.Models.Training;
using Typro.Application.Services.Training;
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

    public async Task<Result<IEnumerable<IEnumerable<char>>>> GenerateText(TrainingConfigurationDto dto)
    {
        var supportedLanguagesResult = await _supportedLanguagesService.GetSupportedLanguagesAsync();
        if (supportedLanguagesResult.IsFailed)
        {
            return Result.Fail(supportedLanguagesResult.Errors);
        }

        var supportedLanguages = supportedLanguagesResult.Value;
        var targetLanguage = supportedLanguages.SingleOrDefault(e => e.Id == dto.LanguageId);
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
            numberOfWords = 100;
        }

        var generatedText = await GenerateSymbols(
            numberOfWords,
            dto.LanguageId,
            dto.IsPunctuationEnabled,
            dto.AreNumbersEnabled);
        
        return Result.Ok(generatedText);
    }

    private async Task<IEnumerable<IEnumerable<char>>> GenerateSymbols(
        int numberOfWords,
        int languageId,
        bool isPunctuationEnabled,
        bool areNumbersEnabled)
    {
        var wordsResult = await _wordsService.GetNRandomWordsByLanguageAsync(languageId, numberOfWords);
        
        var words = wordsResult
            .Value
            .Select(w => w.Name)
            .ToList();

        var random = new Random();
        if (areNumbersEnabled)
        {
            var indicesSet = new HashSet<int>();
            foreach (var _ in words)
            {
                var shouldBeInserted = random.Next(0, 10) is >= 3 and <= 6;
                if (shouldBeInserted)
                {
                    indicesSet.Add(random.Next(0, words.Count));
                }
            }

            var orderedByDescIndicesList = indicesSet
                .OrderDescending()
                .ToList();
            
            foreach (var index in orderedByDescIndicesList)
            {
                var randomNumber = random.Next(0, 101);
                words[index] = randomNumber.ToString();
            }
        }
        
        var result = new List<IEnumerable<char>>();
        foreach (var word in words)
        {
            var symbols = word.ToList();
            if (isPunctuationEnabled)
            {
                InsertPunctuation(symbols, random);
            }

            result.Add(symbols);
        }

        return result;
    }

    private void InsertPunctuation(ICollection<char> chars, Random random)
    {
        var randomNumber = random.Next(0, 10);
        var shouldBeInserted = randomNumber is >= 0 and < 2 or 7 ;
        if (!shouldBeInserted)
        {
            return;
        }

        var punctuationSymbol = _punctuationSymbols[random.Next(0, _punctuationSymbols.Length)];
        chars.Add(punctuationSymbol);
    }
}