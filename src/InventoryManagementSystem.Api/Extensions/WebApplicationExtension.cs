using InventoryManagementSystem.Infrastructure.BackgroundJobs;
using Hangfire;
namespace InventoryManagementSystem.Api.Extensions
{
    public static class WebApplicationExtension
    {
        public static IApplicationBuilder StartBackgroudJobs(this IApplicationBuilder app)
        {
            var jobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();
            jobManager.AddOrUpdate<ArchiveOldTransactionsJob>("archive_old_transaction", x => x.Execute(), Cron.Daily);
            jobManager.AddOrUpdate<LowStockNotificationJob>("low_stock_notification", x => x.Execute(), Cron.Daily);
            return app;
        }
    }
}
