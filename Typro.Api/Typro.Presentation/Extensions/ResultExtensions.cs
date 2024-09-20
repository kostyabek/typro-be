using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Presentation.Extensions;

public class UniversalResponse<T>
{
    public T? Value { get; set; }
    public IEnumerable<IReason> Reasons { get; set; }
};

public static class UniversalResponse
{
    public static UniversalResponse<object> FromMessages(IEnumerable<IReason> reasons) =>
        new() { Reasons = reasons };
}

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        return TryGetErrorActionResult(result, out IActionResult? actionResult)
            ? actionResult
            : new OkObjectResult(UniversalResponse.FromMessages(result.Reasons));
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        var nonGenericResult = result.ToResult();

        return TryGetErrorActionResult(nonGenericResult, out IActionResult? actionResult)
            ? actionResult
            : new OkObjectResult(new UniversalResponse<T>
            {
                Value = result.ValueOrDefault,
                Reasons = result.Reasons
            });
    }

    private static bool TryGetErrorActionResult(ResultBase result, out IActionResult actionResult)
    {
        actionResult = default;

        if (result.IsSuccess)
        {
            return false;
        }

        List<IReason>? reasons = result.Reasons;

        if (result.HasError<NotFoundError>())
        {
            actionResult = new NotFoundObjectResult(UniversalResponse.FromMessages(result.Reasons));
            return true;
        }

        if (result.HasError<InvalidOperationError>())
        {
            actionResult = new BadRequestObjectResult(UniversalResponse.FromMessages(result.Reasons));
            return true;
        }

        if (result.HasError<UnauthorizedError>())
        {
            actionResult = new UnauthorizedObjectResult(UniversalResponse.FromMessages(result.Reasons));
            return true;
        }

        if (result.HasError<ValidationError>())
        {
            actionResult = new BadRequestObjectResult(UniversalResponse.FromMessages(result.Reasons));
            return true;
        }

        actionResult = new ObjectResult(UniversalResponse.FromMessages(result.Reasons))
            { StatusCode = StatusCodes.Status500InternalServerError };

        return true;
    }
}