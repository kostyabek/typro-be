using MediatR;
using Microsoft.AspNetCore.Mvc;
using Typro.Application.CQRS.Auth.UserSignIn;
using Typro.Application.CQRS.Auth.UserSignUp;
using Typro.Presentation.Extensions;
using Typro.Presentation.Models.Request;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync(UserSignUpRequestModel request)
    {
        var command = new UserSignUpCommand(request.Email, request.Password, request.ConfirmPassword);

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync(UserSignInRequestModel request)
    {
        var command = new UserSignInCommand(request.Email, request.Password);

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}