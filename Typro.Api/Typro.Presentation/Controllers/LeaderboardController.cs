﻿using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Typro.Application.Models.Leaderboard;
using Typro.Application.Services.Training;
using Typro.Presentation.Extensions;
using Typro.Presentation.Models.Request.Leaderboard;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("leaderboards")]
public class LeaderboardController(ITrainingResultsService trainingResultsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLeaderboardAsync([FromQuery] GetLeaderboardRequest request)
    {
        var filterDto = new LeaderboardFilterDto(
            request.TimeModeType,
            request.WordsModeType,
            request.LanguageId,
            request.FromDate,
            request.ToDate,
            request.PageNumber,
            request.PageSize);

        Result<IEnumerable<LeaderboardEntryDto>> result =
            await trainingResultsService.GetLeaderboardAsync(filterDto);

        return result.ToActionResult();
    }
}