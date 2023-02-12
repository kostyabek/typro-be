using Typro.Domain.Enums;

namespace Typro.Application.Services;

public interface IUserIdentityService
{
    public int UserId { get; }
    public string UserEmail { get; }
    public UserRole UserRole { get; }
}