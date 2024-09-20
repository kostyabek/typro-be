using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Typro.Application.Services.User;
using Typro.Domain.Enums.User;

namespace Typro.Infrastructure.Services.User;

public class UserIdentityService(IHttpContextAccessor httpContextAccessor) : IUserIdentityService
{
    public int UserId => int.Parse(httpContextAccessor.HttpContext?.User.FindFirst("id").Value);
    public string UserEmail => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email).Value;

    public UserRole UserRole =>
        Enum.Parse<UserRole>(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role).Value, true);
}