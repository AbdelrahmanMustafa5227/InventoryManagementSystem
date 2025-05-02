using InventoryManagementSystem.Infrastructure.BackgroundJobs;
using Hangfire;
using InventoryManagementSystem.Infrastructure;
using InventoryManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace InventoryManagementSystem.Api.Extensions
{
    public static class WebApplicationExtension
    {
        public static IApplicationBuilder StartBackgroudJobs(this IApplicationBuilder app)
        {
            var jobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();
            var jobs = app.ApplicationServices.GetServices<IJob>();

            foreach (var job in jobs)
            {
                var jobName = job!.GetType().Name.ToLower();
                jobManager.AddOrUpdate(jobName, () => job.Execute(), Cron.Daily);
            }
            return app;
        }

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (dbContext.Database.GetMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
