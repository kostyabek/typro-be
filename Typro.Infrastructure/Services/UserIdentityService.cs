using Microsoft.AspNetCore.Http;
using Typro.Application.Services;

namespace Typro.Infrastructure.Services;

public class UserIdentityService : IUserIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public int UserId => int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value);

    public UserIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}