using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Typro.Application.Models.Training;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Domain.Database.Models;
using Typro.Domain.Models.Training;
using Typro.Presentation.Extensions;
using Typro.Presentation.Models.Request.Training;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("training")]
public class TrainingController(
    ITrainingConfigurationService trainingConfigurationService,
    ISupportedLanguagesService supportedLanguagesService,
    IValidator<UpdateTrainingConfigurationRequest> updateTrainingConfigurationRequestValidator,
    ITextGenerationService textGenerationService,
    IUserIdentityService userIdentityService,
    ITrainingResultsService trainingResultsService,
    IPreparedMultiplayerTextsService preparedMultiplayerTextsService) : ControllerBase
{
    [HttpGet("supported-languages")]
    public async Task<IActionResult> GetSupportedLanguagesAsync()
    {
        Result<IEnumerable<SupportedLanguageDto>>
            result = await supportedLanguagesService.GetSupportedLanguagesAsync();
        return result.ToActionResult();
    }

    [HttpGet("configurations/{configurationId:int}")]
    [Authorize]
    public async Task<IActionResult> GetTrainingConfigurationByIdAsync([FromRoute] int configurationId)
    {
        Result<TrainingConfiguration> result =
            await trainingConfigurationService.GetTrainingConfigurationByIdAsync(configurationId);
        return result.ToActionResult();
    }

    [HttpPut("configurations")]
    [Authorize]
    public async Task<IActionResult> UpdateTrainingConfigurationAsync(
        [FromBody] UpdateTrainingConfigurationRequest request)
    {
        ValidationResult validationResult = await updateTrainingConfigurationRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }

        var dto = new TrainingConfigurationDto(
            request.IsPunctuationEnabled,
            request.AreNumbersEnabled,
            request.WordsModeType,
            request.TimeModeType,
            request.LanguageId);

        Result result = await trainingConfigurationService.UpdateTrainingConfigurationAsync(dto);
        return result.ToActionResult();
    }

    [HttpGet("text-generation")]
    public async Task<IActionResult> GetGeneratedTextAsync([FromQuery] GetGeneratedTextRequest request)
    {
        var dto = new TrainingConfigurationDto(
            request.IsPunctuationGenerated,
            request.AreNumbersGenerated,
            request.WordsMode,
            request.TimeMode,
            request.LanguageId);

        Result<IEnumerable<string>> wordsResult = await textGenerationService.GenerateText(dto);
        if (wordsResult.IsFailed)
        {
            return wordsResult.ToActionResult();
        }

        Result<IEnumerable<IEnumerable<char>>> symbolsResult =
            textGenerationService.ConvertWordsToSymbols(wordsResult.Value);

        return symbolsResult.ToActionResult();
    }

    [HttpGet("multiplayer-text-generation")]
    [Authorize]
    public async Task<IActionResult> GetMultiplayerGeneratedTextAsync(
        [FromQuery] GetGeneratedTextRequest request,
        [FromQuery] string lobbyId,
        [FromQuery] bool isForceRewrite)
    {
        var dto = new TrainingConfigurationDto(
            request.IsPunctuationGenerated,
            request.AreNumbersGenerated,
            request.WordsMode,
            request.TimeMode,
            request.LanguageId);

        Result<PreparedMultiplayerTextInfoDto> result =
            await preparedMultiplayerTextsService.GetOrCreateTrainingTextAsync(dto, lobbyId, isForceRewrite);

        return result.ToActionResult();
    }
    
    [HttpGet("lobby")]
    [Authorize]
    public async Task<IActionResult> CheckIfLobbyExistsAsync([FromQuery] string lobbyId)
    {
        Result<bool> result = await preparedMultiplayerTextsService.CheckIfLobbyExists(lobbyId);
        return result.ToActionResult();
    }
    
    [HttpDelete("lobby")]
    [Authorize]
    public async Task<IActionResult> DeleteLobbyInfoAsync([FromQuery] string lobbyId)
    {
        Result result = await preparedMultiplayerTextsService.DeleteLobby(lobbyId);
        return result.ToActionResult();
    }

    [HttpPost("results")]
    [Authorize]
    public async Task<IActionResult> CreateTrainingResultsAsync(
        [FromBody] CreateTrainingResultsRequest request)
    {
        int userId = userIdentityService.UserId;
        var dto = new FullTrainingResultsDto(
            -1.0f,
            -1.0f,
            -1,
            new CharacterStats(-1, -1, -1, -1),
            request.LanguageId,
            request.TimeModeType,
            request.WordsModeType,
            request.DateConducted,
            userId);

        Result<int> result = await trainingResultsService.CreateTrainingResultsAsync(dto);
        return result.ToActionResult();
    }

    [HttpPatch("results/{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateTrainingResultsAsync(
        [FromBody] UpdateTrainingResultsRequest request, int id)
    {
        var dto = new UpdateTrainingResultsDto(
            id,
            request.WordsPerMinute,
            request.Accuracy,
            request.TimeInMilliseconds,
            request.CharactersStats);

        Result<int> result = await trainingResultsService.UpdateTrainingResultsAsync(dto);
        return result.ToActionResult();
    }
}
