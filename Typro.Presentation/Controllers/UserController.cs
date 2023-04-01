using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGeneralInfoAsync()
    {
        throw new NotImplementedException();
    }
}