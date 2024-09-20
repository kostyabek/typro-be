using FluentResults;

namespace Typro.Domain.Models.Result.Errors
{
    public class NotFoundError(string msg) : Error(msg)
    {
    }
}
