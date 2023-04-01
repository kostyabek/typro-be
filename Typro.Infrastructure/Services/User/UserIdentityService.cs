using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Typro.Application.Services.User;
using Typro.Domain.Enums.User;

namespace Typro.Infrastructure.Services.User;

public class UserIdentityService : IUserIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public int UserId => int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst("id").Value);
    public string UserEmail => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email).Value;

    public UserRole UserRole =>
        Enum.Parse<UserRole>(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role).Value, true);

    public UserIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}