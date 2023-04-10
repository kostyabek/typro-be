using Typro.Presentation.Extensions;
using Typro.Presentation.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwagger()
    .AddJwtAuthentication(builder.Configuration)
    .AddDatabaseConnection(builder.Configuration)
    .AddUnitOfWork()
    .AddOptions(builder.Configuration)
    .AddServices()
    .AddHelpers()
    .AddHttpContextAccessor()
    .AddFluentValidators();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(e =>
{
    e.WithOrigins("https://typro.local:5286")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseJwtValidation();
app.UseAuthorization();

app.MapControllers();

app.Run();