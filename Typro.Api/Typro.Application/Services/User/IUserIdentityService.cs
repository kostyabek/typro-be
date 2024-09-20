using Typro.Domain.Enums.User;

namespace Typro.Application.Services.User;

public interface IUserIdentityService
{
    public int UserId { get; }
    public string UserEmail { get; }
    public UserRole UserRole { get; }
}