using FluentResults;

namespace Typro.Domain.Models.Result.Errors;

public class UnauthorizedError(string msg) : Error(msg)
{
}