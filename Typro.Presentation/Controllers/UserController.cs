using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Typro.Application.Models.Training;
using Typro.Application.Models.User;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Presentation.Extensions;
using Typro.Presentation.Models.Request.User;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserIdentityService _userIdentityService;
    private readonly ITrainingResultsService _trainingResultsService;
    private readonly IUserService _userService;

    public UserController(
        IUserIdentityService userIdentityService,
        ITrainingResultsService trainingResultsService,
        IUserService userService)
    {
        _userIdentityService = userIdentityService;
        _trainingResultsService = trainingResultsService;
        _userService = userService;
    }

    [HttpGet("nickname")]
    public async Task<IActionResult> GetNicknameAsync()
    {
        int userId = _userIdentityService.UserId;
        Result<string> result = await _userService.GetNicknameByIdAsync(userId);
        return result.ToActionResult();
    }

    [HttpGet("high-level-stats-info")]
    public async Task<IActionResult> GetHighLevelProfileInfoAsync()
    {
        int userId = _userIdentityService.UserId;
        Result<HighLevelProfileInfoDto> result = await _trainingResultsService.GetHighLevelProfileInfoAsync(userId);
        return result.ToActionResult();
    }

    [HttpGet("high-level-training-results")]
    public async Task<IActionResult> GetHighLevelTrainingResultsAsync()
    {
        int userId = _userIdentityService.UserId;
        Result<IEnumerable<HighLevelTrainingResultDto>> result =
            await _trainingResultsService.GetHighLevelTrainingResultsAsync(userId);
        return result.ToActionResult();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateNicknameByIdAsync([FromBody] string nickname)
    {
        int userId = _userIdentityService.UserId;
        Result<string> result = await _userService.UpdateNicknameByIdAsync(nickname, userId);
        return result.ToActionResult();
    }

    [HttpGet("words-per-minute-to-accuracy-stats")]
    public async Task<IActionResult> GetWordsPerMinuteToAccuracyStatsAsync(
        [FromQuery] GetWordsPerMinuteToAccuracyStatsRequest request)
    {
        int userId = _userIdentityService.UserId;
        var dto = new WordsPerMinuteToAccuracyRequestDto(userId, request.FromDate, request.LanguageId,
            request.WordsModeType, request.TimeModeType);

        Result<IEnumerable<WordsPerMinuteToAccuracyDto>> result =
            await _userService.GetWordsPerMinuteToAccuracyStatsAsync(dto);
        return result.ToActionResult();
    }
}