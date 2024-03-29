﻿using FluentResults;
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
public class TrainingController : ControllerBase
{
    private readonly IValidator<UpdateTrainingConfigurationRequest> _updateTrainingConfigurationRequestValidator;

    private readonly ISupportedLanguagesService _supportedLanguagesService;
    private readonly ITrainingConfigurationService _trainingConfigurationService;
    private readonly ITextGenerationService _textGenerationService;
    private readonly IUserIdentityService _userIdentityService;
    private readonly ITrainingResultsService _trainingResultsService;
    private readonly IPreparedMultiplayerTextsService _preparedMultiplayerTextsService;

    public TrainingController(
        ITrainingConfigurationService trainingConfigurationService,
        ISupportedLanguagesService supportedLanguagesService,
        IValidator<UpdateTrainingConfigurationRequest> updateTrainingConfigurationRequestValidator,
        ITextGenerationService textGenerationService,
        IUserIdentityService userIdentityService,
        ITrainingResultsService trainingResultsService,
        IPreparedMultiplayerTextsService preparedMultiplayerTextsService)
    {
        _trainingConfigurationService = trainingConfigurationService;
        _supportedLanguagesService = supportedLanguagesService;
        _updateTrainingConfigurationRequestValidator = updateTrainingConfigurationRequestValidator;
        _textGenerationService = textGenerationService;
        _userIdentityService = userIdentityService;
        _trainingResultsService = trainingResultsService;
        _preparedMultiplayerTextsService = preparedMultiplayerTextsService;
    }

    [HttpGet("supported-languages")]
    public async Task<IActionResult> GetSupportedLanguagesAsync()
    {
        Result<IEnumerable<SupportedLanguageDto>>
            result = await _supportedLanguagesService.GetSupportedLanguagesAsync();
        return result.ToActionResult();
    }

    [HttpGet("configurations/{configurationId:int}")]
    [Authorize]
    public async Task<IActionResult> GetTrainingConfigurationByIdAsync([FromRoute] int configurationId)
    {
        Result<TrainingConfiguration> result =
            await _trainingConfigurationService.GetTrainingConfigurationByIdAsync(configurationId);
        return result.ToActionResult();
    }

    [HttpPut("configurations")]
    [Authorize]
    public async Task<IActionResult> UpdateTrainingConfigurationAsync(
        [FromBody] UpdateTrainingConfigurationRequest request)
    {
        ValidationResult validationResult = await _updateTrainingConfigurationRequestValidator.ValidateAsync(request);
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

        Result result = await _trainingConfigurationService.UpdateTrainingConfigurationAsync(dto);
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

        Result<IEnumerable<string>> wordsResult = await _textGenerationService.GenerateText(dto);
        if (wordsResult.IsFailed)
        {
            return wordsResult.ToActionResult();
        }

        Result<IEnumerable<IEnumerable<char>>> symbolsResult =
            _textGenerationService.ConvertWordsToSymbols(wordsResult.Value);

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
            await _preparedMultiplayerTextsService.GetOrCreateTrainingTextAsync(dto, lobbyId, isForceRewrite);

        return result.ToActionResult();
    }
    
    [HttpGet("lobby")]
    [Authorize]
    public async Task<IActionResult> CheckIfLobbyExistsAsync([FromQuery] string lobbyId)
    {
        Result<bool> result = await _preparedMultiplayerTextsService.CheckIfLobbyExists(lobbyId);
        return result.ToActionResult();
    }
    
    [HttpDelete("lobby")]
    [Authorize]
    public async Task<IActionResult> DeleteLobbyInfoAsync([FromQuery] string lobbyId)
    {
        Result result = await _preparedMultiplayerTextsService.DeleteLobby(lobbyId);
        return result.ToActionResult();
    }

    [HttpPost("results")]
    [Authorize]
    public async Task<IActionResult> CreateTrainingResultsAsync(
        [FromBody] CreateTrainingResultsRequest request)
    {
        int userId = _userIdentityService.UserId;
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

        Result<int> result = await _trainingResultsService.CreateTrainingResultsAsync(dto);
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

        Result<int> result = await _trainingResultsService.UpdateTrainingResultsAsync(dto);
        return result.ToActionResult();
    }
}
