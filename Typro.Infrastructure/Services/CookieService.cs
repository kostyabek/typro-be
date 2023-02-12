using Microsoft.AspNetCore.Http;
using Typro.Application.Models.Auth;
using Typro.Application.Services;

namespace Typro.Infrastructure.Services;

public class CookieService : ICookieService
{
    private const string RefreshTokenCookieName = "refreshToken";

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

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(RefreshTokenCookieName, dto.Token, cookieOptions);
    }

    public void RemoveRefreshTokenCookie()
        => _httpContextAccessor.HttpContext?.Response.Cookies.Delete(RefreshTokenCookieName);

    public bool TryGetRefreshTokenFromCookie(out string token)
        => _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(RefreshTokenCookieName, out token);
}