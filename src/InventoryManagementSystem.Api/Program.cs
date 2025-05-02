using Hangfire;
using Serilog;
using InventoryManagementSystem.Api.Extensions;
using InventoryManagementSystem.Api.Middlewares;
using InventoryManagementSystem.Application;
using InventoryManagementSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("IMS_");
builder.Services.AddControllers();
builder.Services.AddApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddOpenApi();

builder.Host.UseSerilog((context, cfg) =>
{
    cfg.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();
app.StartBackgroudJobs();
app.ApplyMigrations();

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
app.UseRateLimiter();
app.UseMiddleware<GlobalExceptionHandling>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
