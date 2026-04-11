using Serilog;
using TaskManagement.API.Endpoints;
using TaskManagement.API.Extensions;
using TaskManagement.Application.Extensions;
using TaskManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Serilog 
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

//Register services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//Map endpoints
app.MapAuthEndpoints();
app.MapTodoItemEndpoints();
app.MapCategoryEndpoints();

app.Run();
