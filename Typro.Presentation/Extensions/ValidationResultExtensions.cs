using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Presentation.Extensions;

public static class ValidationResultExtensions
{
    public static IActionResult ToActionResult(this ValidationResult validationResult)
    {
        IEnumerable<ValidationError>? errors = validationResult
            .Errors
            .Select(e => new ValidationError(e.ErrorMessage) { Metadata = { { "field", e.PropertyName } } });

        Result? result = Result.Fail(errors);
        return result.ToActionResult();
    }
}