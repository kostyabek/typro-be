namespace Typro.Application.Models.Auth;

public record UserSignInResponseDto(string AccessToken) : AccessTokenResponseDto(AccessToken);