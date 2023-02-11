using Typro.Presentation.Models.Request.Base;

namespace Typro.Presentation.Models.Request;

public record UserSignInRequest(string Email, string Password) : CredentialsRequestModel(Email, Password);