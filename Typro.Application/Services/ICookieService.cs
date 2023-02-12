using Typro.Application.Models.Auth;

namespace Typro.Application.Services;

public interface ICookieService
{
    void SetRefreshTokenCookie(RefreshTokenDto dto);
    void RemoveRefreshTokenCookie();
    bool TryGetRefreshTokenFromCookie(out string token);
}