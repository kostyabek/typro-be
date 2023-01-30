using FluentResults;

namespace Typro.Domain.Models.Result.Errors
{
    public class InvalidOperationError : Error
    {
        public InvalidOperationError(string msg) : base(msg)
        {
        }
    }
}
