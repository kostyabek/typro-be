using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Presentation.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        return TryGetErrorActionResult(result, out var actionResult) ?
            actionResult :
            new OkObjectResult(new
            {
                messages = result.Reasons ?? new List<IReason>()
            });
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        var nonGenericResult = result.ToResult();

        return TryGetErrorActionResult(nonGenericResult, out var actionResult) ?
            actionResult :
            new OkObjectResult(new
            {
                value = result.ValueOrDefault,
                messages = result.Reasons ?? new List<IReason>()
            });
    }

    private static bool TryGetErrorActionResult(Result result, out IActionResult actionResult)
    {
        actionResult = default;

        if (result.IsSuccess)
        {
            return false;
        }

        var reasons = result.Reasons;

        if (result.HasError<NotFoundError>())
        {
            actionResult = new NotFoundObjectResult(reasons);
            return true;
        }

        if (result.HasError<InvalidOperationError>())
        {
            actionResult = new BadRequestObjectResult(reasons);
            return true;
        }

        if (result.HasError<UnauthorizedError>())
        {
            actionResult = new UnauthorizedObjectResult(reasons);
            return true;
        }

        actionResult = new ObjectResult(reasons) { StatusCode = StatusCodes.Status500InternalServerError };

        return true;
    }
}