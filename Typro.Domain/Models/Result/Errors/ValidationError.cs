using FluentResults;

namespace Typro.Domain.Models.Result.Errors
{
    public class ValidationError(string msg) : Error(msg)
    {
    }
}
