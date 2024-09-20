using FluentResults;

namespace Typro.Domain.Models.Result.Errors;

public class InvalidOperationError(string msg) : Error(msg)
{
}
