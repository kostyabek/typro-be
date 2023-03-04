using Typro.Presentation.Models.Request.Auth.Base;

namespace Typro.Presentation.Models.Request.Auth;

public record UserSignInRequest(string Email, string Password) : CredentialsRequest(Email, Password);