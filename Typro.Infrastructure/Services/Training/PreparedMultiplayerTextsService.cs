using System.Text;
using FluentResults;
using Typro.Application.Models.Training;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.Training;

public class PreparedMultiplayerTextsService : IPreparedMultiplayerTextsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITextGenerationService _textGenerationService;

    public PreparedMultiplayerTextsService(
        ITextGenerationService textGenerationService,
        IUnitOfWork unitOfWork)
    {
        _textGenerationService = textGenerationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PreparedMultiplayerTextInfoDto>> GetOrCreateTrainingTextAsync(
        TrainingConfigurationDto dto,
        string lobbyId,
        bool isForceRewrite)
    {
        PreparedMultiplayerTextInfo? preparedTextInfo =
            await _unitOfWork.PreparedMultiplayerTextsRepository
                .GetPreparedTextByLobbyId(lobbyId);

        if (preparedTextInfo is not null)
        {
            if (isForceRewrite)
            {
                Result<IEnumerable<string>> forcedGeneratedWordsResult = await _textGenerationService.GenerateText(dto);
                if (forcedGeneratedWordsResult.IsFailed)
                {
                    return forcedGeneratedWordsResult.ToResult();
                }

                string forceGeneratedText = PrepareText(forcedGeneratedWordsResult.Value);
                var preparedMultiplayerText = new PreparedMultiplayerTextInfo
                {
                    LobbyId = lobbyId,
                    Text = forceGeneratedText,
                    TimeModeType = dto.TimeMode,
                    WordsModeType = dto.WordsMode,
                    LanguageId = dto.LanguageId,
                    AreNumbersEnabled = dto.AreNumbersEnabled,
                    IsPunctuationEnabled = dto.IsPunctuationEnabled
                };
                int forceRowsAffected =
                    await _unitOfWork.PreparedMultiplayerTextsRepository.Update(preparedMultiplayerText);
                Result<IEnumerable<IEnumerable<char>>> forceRewriteSymbolsResult =
                    _textGenerationService.ConvertWordsToSymbols(forceGeneratedText.Split(' '));

                if (forceRewriteSymbolsResult.IsFailed)
                {
                    return forceRewriteSymbolsResult.ToResult();
                }

                return forceRowsAffected == 0
                    ? Result.Fail(new Error("Could not save prepared words"))
                    : Result.Ok(new PreparedMultiplayerTextInfoDto(forceRewriteSymbolsResult.Value,
                        dto.IsPunctuationEnabled, dto.AreNumbersEnabled, dto.WordsMode, dto.TimeMode, dto.LanguageId));
            }

            IEnumerable<string> preparedWords = preparedTextInfo.Text.Split(' ');
            Result<IEnumerable<IEnumerable<char>>> symbolsResult =
                _textGenerationService.ConvertWordsToSymbols(preparedWords);
            if (symbolsResult.IsFailed)
            {
                return symbolsResult.ToResult();
            }

            return Result.Ok(new PreparedMultiplayerTextInfoDto(symbolsResult.Value,
                preparedTextInfo.IsPunctuationEnabled, preparedTextInfo.AreNumbersEnabled,
                preparedTextInfo.WordsModeType, preparedTextInfo.TimeModeType, preparedTextInfo.LanguageId));
        }

        Result<IEnumerable<string>> generatedWordsResult = await _textGenerationService.GenerateText(dto);
        if (generatedWordsResult.IsFailed)
        {
            return generatedWordsResult.ToResult();
        }

        string newGeneratedText = PrepareText(generatedWordsResult.Value);
        var preparedMultiplayerTextModel = new PreparedMultiplayerTextInfo
        {
            LobbyId = lobbyId,
            Text = newGeneratedText,
            TimeModeType = dto.TimeMode,
            WordsModeType = dto.WordsMode,
            LanguageId = dto.LanguageId,
            AreNumbersEnabled = dto.AreNumbersEnabled,
            IsPunctuationEnabled = dto.IsPunctuationEnabled
        };

        int rowsAffected = await _unitOfWork.PreparedMultiplayerTextsRepository.Insert(preparedMultiplayerTextModel);
        if (rowsAffected == 0)
        {
            return Result.Fail(new Error("Could not save prepared words"));
        }

        Result<IEnumerable<IEnumerable<char>>> newSymbolsResult =
            _textGenerationService.ConvertWordsToSymbols(newGeneratedText.Split(' '));
        if (newSymbolsResult.IsFailed)
        {
            return newSymbolsResult.ToResult();
        }

        return Result.Ok(new PreparedMultiplayerTextInfoDto(newSymbolsResult.Value, dto.IsPunctuationEnabled,
            dto.AreNumbersEnabled, dto.WordsMode, dto.TimeMode, dto.LanguageId));
    }

    public async Task<Result> DeleteLobby(string lobbyId)
    {
        int rowsAffected = await _unitOfWork.PreparedMultiplayerTextsRepository.Delete(lobbyId);
        return rowsAffected == 0
            ? Result.Fail(new NotFoundError("Lobby does not exist"))
            : Result.Ok();
    }
    
    public async Task<Result<bool>> CheckIfLobbyExists(string lobbyId)
    {
        bool exists = await _unitOfWork.PreparedMultiplayerTextsRepository.CheckIfLobbyExists(lobbyId);
        return Result.Ok(exists);
    }
    
    private static string PrepareText(IEnumerable<string> words)
    {
        IEnumerable<string> wordsList = words.ToList();
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < wordsList.Count(); i++)
        {
            stringBuilder.Append(wordsList.ElementAt(i));
            if (i < wordsList.Count() - 1)
            {
                stringBuilder.Append(' ');
            }
        }

        return stringBuilder.ToString();
    }
}