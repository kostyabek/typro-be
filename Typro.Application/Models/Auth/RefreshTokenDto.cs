namespace Typro.Application.Models.Auth;

public record RefreshTokenDto(string Token, DateTime ExpirationDate);