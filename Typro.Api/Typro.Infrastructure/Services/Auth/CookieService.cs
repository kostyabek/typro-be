using Microsoft.AspNetCore.Http;
using Typro.Application.Models.Auth;
using Typro.Application.Services.Auth;

namespace Typro.Infrastructure.Services.Auth;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    private const string RefreshTokenCookieName = "refreshToken";

    public void SetRefreshTokenCookie(RefreshTokenDto dto)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = dto.ExpirationDate,
            SameSite = SameSiteMode.None,
            Secure = true
        };

        httpContextAccessor.HttpContext?.Response.Cookies.Append(RefreshTokenCookieName, dto.Token, cookieOptions);
    }

    public void RemoveRefreshTokenCookie()
        => httpContextAccessor.HttpContext?.Response.Cookies.Delete(RefreshTokenCookieName);

    public bool TryGetRefreshTokenFromCookie(out string token)
        => httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(RefreshTokenCookieName, out token);
}