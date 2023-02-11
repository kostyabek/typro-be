using Typro.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwagger()
    .AddJwtAuthentication(builder.Configuration)
    .AddDatabaseConnection(builder.Configuration)
    .AddRepositories()
    .AddOptions(builder.Configuration)
    .AddServices()
    .AddHttpContextAccessor()
    .AddFluentValidators();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();