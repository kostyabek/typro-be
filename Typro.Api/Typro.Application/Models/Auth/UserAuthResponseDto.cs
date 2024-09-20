namespace Typro.Application.Models.Auth;

public record UserAuthResponseDto(string AccessToken, string Email, string Nickname) : AccessTokenResponseDto(AccessToken);