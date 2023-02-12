using System.Data;
using System.Data.SqlClient;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Typro.Application.Models.Options;
using Typro.Application.Services;
using Typro.Application.UnitsOfWork;
using Typro.Infrastructure.Services;
using Typro.Infrastructure.UnitsOfWork;
using Typro.Presentation.Models.Request.Auth;
using Typro.Presentation.Validators.Auth;

namespace Typro.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var issuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                configuration.GetSection(
                    $"{TokenOptions.SectionName}:{nameof(TokenOptions.SecretKey)}").Value));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = issuerSigningKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        return services;
    }

    public static IServiceCollection AddDatabaseConnection(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IDbConnection, SqlConnection>(_ =>
            new SqlConnection(configuration.GetConnectionString("Default")));

        return services;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IUserIdentityService, UserIdentityService>();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.SectionName));

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Please enter a valid token without `Bearer`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddFluentValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserSignUpRequest>, UserSignUpRequestValidator>();
        services.AddScoped<IValidator<UserSignInRequest>, UserSignInRequestValidator>();

        return services;
    }
}