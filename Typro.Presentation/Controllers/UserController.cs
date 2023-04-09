using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Typro.Application.Models.Training;
using Typro.Application.Models.User;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Presentation.Extensions;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserIdentityService _userIdentityService;
    private readonly ITrainingResultsService _trainingResultsService;

    public UserController(
        IUserIdentityService userIdentityService,
        ITrainingResultsService trainingResultsService)
    {
        _userIdentityService = userIdentityService;
        _trainingResultsService = trainingResultsService;
    }

    [HttpGet("high-level-profile-info")]
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
}