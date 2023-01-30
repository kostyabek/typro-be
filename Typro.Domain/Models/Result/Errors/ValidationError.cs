using FluentResults;

namespace Typro.Domain.Models.Result.Errors
{
    public class ValidationError : Error
    {
        public ValidationError(string msg) : base(msg)
        {
        }
    }
}
