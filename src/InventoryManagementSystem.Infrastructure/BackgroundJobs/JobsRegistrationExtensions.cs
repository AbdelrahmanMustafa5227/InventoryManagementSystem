using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.BackgroundJobs
{
    public static class JobsRegistrationExtensions
    {
        public static void RegisterHangfireJobs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(cfg => cfg.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            var alljobs = typeof(JobsRegistrationExtensions).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IJob).IsAssignableFrom(t))
                .ToList();

            foreach (var job in alljobs)
            {
                services.AddSingleton(typeof(IJob), job);
            }

        }
    }
}
