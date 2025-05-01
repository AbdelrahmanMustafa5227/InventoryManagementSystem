using Hangfire;
using Serilog;
using InventoryManagementSystem.Api.Extensions;
using InventoryManagementSystem.Api.Filters;
using InventoryManagementSystem.Api.Helpers;
using InventoryManagementSystem.Api.Middlewares;
using InventoryManagementSystem.Application;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext,UserContext>();
builder.Configuration.AddEnvironmentVariables("IMS_");
builder.Services.AddScoped<IdempotencyFilter>();

builder.Host.UseSerilog((context, cfg) =>
{
    cfg.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddOpenApi();

var app = builder.Build();
app.StartBackgroudJobs();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHangfireDashboard(options: new DashboardOptions
    {
        Authorization = [],
        DarkModeEnabled = false,
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandling>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
