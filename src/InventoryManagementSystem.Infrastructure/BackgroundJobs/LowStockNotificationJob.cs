using Hangfire;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.EmailTemplates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.BackgroundJobs
{
    public class LowStockNotificationJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ILogger<LowStockNotificationJob> _logger;

        public LowStockNotificationJob(ILogger<LowStockNotificationJob> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var warehouseRepository = scope.ServiceProvider.GetRequiredService<IWarehouseRepository>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var warehouses = await warehouseRepository.GetWarehousesWithLowStockProducts();
                List<string> lowStockAlerts = new List<string>();

                foreach (var warehouse in warehouses)
                    foreach (var product in warehouse.ProductWarehouses)
                        lowStockAlerts.Add($"Low stock alert for product {product.Product.Name} in warehouse {warehouse.Name}. Current stock: {product.Quantity}");

                await emailService.SendAsync("LowStockNotification", "abdelrahman.mustafa5227@gmail.com", LowStockEmailTemplate.Get(lowStockAlerts));
            }
        }
           
    }

}
