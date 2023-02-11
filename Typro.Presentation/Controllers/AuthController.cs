﻿using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Typro.Application.Models.Auth;
using Typro.Application.Services;
using Typro.Presentation.Extensions;
using Typro.Presentation.Models.Request;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync(UserSignUpRequest request)
    {
        var dto = new UserSignUpDto(request.Email, request.Password);
        var result = await _authService.SignUpAsync(dto);

        return result.ToActionResult();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync(UserSignInRequest request)
    {
        var dto = new UserSignInDto(request.Email, request.Password);
        var result = await _authService.SignInAsync(dto);
        return result.ToActionResult();
    }
    
    [HttpPost("sign-out")]
    public Task<IActionResult> SignOutAsync()
    {
        var result = _authService.SignOutAsync();
        return Task.FromResult(result.ToActionResult());
    }
}