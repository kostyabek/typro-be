using FluentResults;

namespace Typro.Domain.Models.Result.Errors
{
    public class UnauthorizedError : Error
    {
        public UnauthorizedError(string msg) : base(msg)
        {
        }
    }
}