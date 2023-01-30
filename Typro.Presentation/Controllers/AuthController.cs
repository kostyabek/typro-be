using Microsoft.AspNetCore.Mvc;
using Typro.Presentation.Models.Request;
using Typro.Presentation.Models.Response;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<ActionResult<UserSignUpResponseModel>> SignUpAsync(UserSignUpRequestModel request)
    {
        throw new NotImplementedException();
    }
}