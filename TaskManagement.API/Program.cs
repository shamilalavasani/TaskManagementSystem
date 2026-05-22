using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagement.API.Endpoints;
using TaskManagement.API.Extensions;
using TaskManagement.Application.Extensions;
using TaskManagement.Infrastructure.Extensions;
using TaskManagement.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.ValidateJwtSettings();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApiAuthorization();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseGlobalExceptionHandling();
app.UseRequestResponseLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Seed roles
await app.SeedRolesAsync();

app.MapHealthChecks("/health");

app.MapAuthEndpoints();
app.MapTodoItemEndpoints();
app.MapCategoryEndpoints();

app.Run();
