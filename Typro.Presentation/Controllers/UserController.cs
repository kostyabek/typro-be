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
public class UserController(
    IUserIdentityService userIdentityService,
    ITrainingResultsService trainingResultsService,
    IUserService userService) : ControllerBase
{
    [HttpGet("nickname")]
    public async Task<IActionResult> GetNicknameAsync()
    {
        int userId = userIdentityService.UserId;
        Result<string> result = await userService.GetNicknameByIdAsync(userId);
        return result.ToActionResult();
    }

    [HttpGet("high-level-stats-info")]
    public async Task<IActionResult> GetHighLevelProfileInfoAsync()
    {
        int userId = userIdentityService.UserId;
        Result<HighLevelProfileInfoDto> result = await trainingResultsService.GetHighLevelProfileInfoAsync(userId);
        return result.ToActionResult();
    }

    [HttpGet("high-level-training-results")]
    public async Task<IActionResult> GetHighLevelTrainingResultsAsync()
    {
        int userId = userIdentityService.UserId;
        Result<IEnumerable<HighLevelTrainingResultDto>> result =
            await trainingResultsService.GetHighLevelTrainingResultsAsync(userId);
        return result.ToActionResult();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateNicknameByIdAsync([FromBody] string nickname)
    {
        int userId = userIdentityService.UserId;
        Result<string> result = await userService.UpdateNicknameByIdAsync(nickname, userId);
        return result.ToActionResult();
    }

    [HttpGet("words-per-minute-to-accuracy-stats")]
    public async Task<IActionResult> GetWordsPerMinuteToAccuracyStatsAsync(
        [FromQuery] GetWordsPerMinuteToAccuracyStatsRequest request)
    {
        int userId = userIdentityService.UserId;
        var dto = new WordsPerMinuteToAccuracyRequestDto(userId, request.FromDate, request.LanguageId,
            request.WordsModeType, request.TimeModeType);

        Result<IEnumerable<WordsPerMinuteToAccuracyDto>> result =
            await userService.GetWordsPerMinuteToAccuracyStatsAsync(dto);
        return result.ToActionResult();
    }
}