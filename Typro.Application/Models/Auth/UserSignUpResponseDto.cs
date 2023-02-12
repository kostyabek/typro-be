namespace Typro.Application.Models.Auth;

public record UserSignUpResponseDto(string AccessToken) : AccessTokenResponseDto(AccessToken);