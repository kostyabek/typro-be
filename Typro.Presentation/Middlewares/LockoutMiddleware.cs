using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Typro.Application.Services;
using Typro.Application.UnitsOfWork;

namespace Typro.Presentation.Middlewares;

public class LockoutMiddleware
{
    private readonly RequestDelegate _next;
    
    public LockoutMiddleware(RequestDelegate next)
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
        var cancellationToken = context.RequestAborted;

        var userId = userIdentityService.UserId;
        var userEmail = userIdentityService.UserEmail;
        var userRole = userIdentityService.UserRole;
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
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
    public static IApplicationBuilder UseLockout(this WebApplication webApplication)
        => webApplication.UseMiddleware<LockoutMiddleware>();
}