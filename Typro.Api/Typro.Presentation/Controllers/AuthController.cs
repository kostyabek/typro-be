using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Typro.Application.Models.Auth;
using Typro.Application.Services.Auth;
using Typro.Presentation.Extensions;
using Typro.Presentation.Models.Request.Auth;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService,
    IValidator<UserSignUpRequest> signUpRequestValidator,
    IValidator<UserSignInRequest> signInRequestValidator) : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync(UserSignUpRequest request)
    {
        ValidationResult? validationResult = await signUpRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }
        
        var dto = new UserSignUpDto(request.Email, request.Password);
        Result<UserAuthResponseDto>? result = await authService.SignUpAsync(dto);

        return result.ToActionResult();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync(UserSignInRequest request)
    {
        ValidationResult? validationResult = await signInRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }
        
        var dto = new UserSignInDto(request.Email, request.Password);
        Result<UserAuthResponseDto>? result = await authService.SignInAsync(dto);
        return result.ToActionResult();
    }
    
    [HttpPost("sign-out")]
    [Authorize]
    public Task<IActionResult> SignOutAsync()
    {
        Result? result = authService.SignOut();
        return Task.FromResult(result.ToActionResult());
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshAccessTokenAsync()
    {
        Result<AccessTokenResponseDto>? result = await authService.RefreshAccessTokenAsync();
        return result.ToActionResult();
    }
}