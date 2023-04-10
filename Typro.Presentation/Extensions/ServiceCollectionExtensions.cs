using System.Data;
using System.Data.SqlClient;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Typro.Application.Helpers;
using Typro.Application.Models.Options;
using Typro.Application.Services.Auth;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Infrastructure;
using Typro.Infrastructure.Helpers;
using Typro.Infrastructure.Services.Auth;
using Typro.Infrastructure.Services.Training;
using Typro.Infrastructure.Services.User;
using Typro.Presentation.Models.Request.Auth;
using Typro.Presentation.Models.Request.Training;
using Typro.Presentation.Validators.Auth;
using Typro.Presentation.Validators.Training;

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
                    ValidateAudience = false,
                    ValidateLifetime = true
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
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISupportedLanguagesService, SupportedLanguagesService>();
        services.AddScoped<ITrainingConfigurationService, TrainingConfigurationService>();
        services.AddScoped<ITextGenerationService, TextGenerationService>();
        services.AddScoped<IWordsService, WordsService>();
        services.AddScoped<ITrainingResultsService, TrainingResultsService>();

        return services;
    }

    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddScoped<INicknameHelper, NicknameHelper>();

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
        services.AddScoped<IValidator<UpdateTrainingConfigurationRequest>, UpdateTrainingConfigurationRequestValidator>();

        return services;
    }
}