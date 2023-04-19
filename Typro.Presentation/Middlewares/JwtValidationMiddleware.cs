using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;
using Typro.Domain.Enums.User;

namespace Typro.Presentation.Middlewares;

public class JwtValidationMiddleware
{
    private readonly RequestDelegate _next;
    
    public JwtValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserIdentityService userIdentityService, IUnitOfWork unitOfWork)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            await _next(context);
            return;
        }
        CancellationToken cancellationToken = context.RequestAborted;

        int userId = userIdentityService.UserId;
        string? userEmail = userIdentityService.UserEmail;
        UserRole userRole = userIdentityService.UserRole;
        User? user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        if (user is null || user.Email != userEmail || user.RoleId != userRole)
        {
            var problem = new ProblemDetails
            {
                Title = "Invalid token.",
                Status = (int)HttpStatusCode.Forbidden,
                Instance = context.Request.Path
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(problem), cancellationToken);
            return;
        }

        await _next(context);
    }
}

public static partial class WebApplicationExtensions
{
    public static IApplicationBuilder UseJwtValidation(this WebApplication webApplication)
        => webApplication.UseMiddleware<JwtValidationMiddleware>();
}