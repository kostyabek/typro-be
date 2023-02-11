using Microsoft.AspNetCore.Http;
using Typro.Application.Models.Auth;
using Typro.Application.Services;

namespace Typro.Infrastructure.Services;

public class CookieService : ICookieService
{
    private const string CookieName = "refreshToken";
        
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetRefreshTokenCookie(RefreshTokenDto dto)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = dto.ExpirationDate
        };
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(CookieName, dto.Token, cookieOptions);
    }

    public void RemoveRefreshTokenCookie()
        => _httpContextAccessor.HttpContext?.Response.Cookies.Delete(CookieName);
}