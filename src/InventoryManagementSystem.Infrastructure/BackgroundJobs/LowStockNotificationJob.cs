using Hangfire;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.EmailTemplates;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.BackgroundJobs
{
    public class LowStockNotificationJob
    {
        private readonly IWarehouseRepository _productWarehouseRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<LowStockNotificationJob> _logger;

        public LowStockNotificationJob(IWarehouseRepository productWarehouseRepository, ILogger<LowStockNotificationJob> logger, IEmailService emailService)
        {
            _productWarehouseRepository = productWarehouseRepository;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Execute()
        {
            var warehouses = await _productWarehouseRepository.GetWarehousesWithLowStockProducts();
            List<string> lowStockAlerts = new List<string>();

            foreach (var warehouse in warehouses)
                foreach (var product in warehouse.ProductWarehouses)
                    lowStockAlerts.Add($"Low stock alert for product {product.Product.Name} in warehouse {warehouse.Name}. Current stock: {product.Quantity}");

            await _emailService.SendAsync("LowStockNotification", "abdelrahman.mustafa5227@gmail.com", LowStockEmailTemplate.Get(lowStockAlerts));
        }
    }

}
