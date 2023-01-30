using Microsoft.AspNetCore.Authentication.JwtBearer;
using Typro.Application.Database;
using Typro.Application.Repositories;
using Typro.Application.Services;
using Typro.Infrastructure.Database;
using Typro.Infrastructure.Repositories;
using Typro.Infrastructure.Services;

namespace Typro.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => { });

        return services;
    }

    public static IServiceCollection AddDatabaseConnector(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IDatabaseConnector, DatabaseConnector>(_ =>
            new DatabaseConnector(configuration.GetConnectionString("Default")));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}