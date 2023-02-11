using Typro.Presentation.Models.Request.Base;

namespace Typro.Presentation.Models.Request
{
    public record UserSignUpRequest
        (string Email, string Password, string ConfirmPassword) : CredentialsRequestModel(Email, Password);
}
