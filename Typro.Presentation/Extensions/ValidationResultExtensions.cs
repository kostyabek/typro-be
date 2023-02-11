using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Typro.Presentation.Extensions;

public static class ValidationResultExtensions
{
    public static IActionResult ToActionResult(this ValidationResult validationResult)
    {
        var result = Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage));
        return result.ToActionResult();
    }
}