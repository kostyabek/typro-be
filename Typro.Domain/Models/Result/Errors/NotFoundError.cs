using FluentResults;

namespace Typro.Domain.Models.Result.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string msg) : base(msg)
        {
        }
    }
}
