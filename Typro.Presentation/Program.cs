using MediatR;
using Typro.Application;
using Typro.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddJwtAuthentication(builder.Configuration)
    .AddDatabaseConnector(builder.Configuration)
    .AddRepositories()
    .AddServices()
    .AddMediatR(typeof(MediatrEntryPoint).Assembly);

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