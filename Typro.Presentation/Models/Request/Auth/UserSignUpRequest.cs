using Typro.Presentation.Models.Request.Auth.Base;

namespace Typro.Presentation.Models.Request.Auth
{
    public record UserSignUpRequest(string Email, string Password, string ConfirmPassword) : CredentialsRequest(Email,
        Password);
}