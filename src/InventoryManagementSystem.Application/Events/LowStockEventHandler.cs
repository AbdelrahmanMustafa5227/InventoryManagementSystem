using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.EmailTemplates;
using InventoryManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Events
{
    public record LowStockEvent(string ProductName , string WarehouseName) : INotification;

    public class LowStockEventHandler : INotificationHandler<LowStockEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUserContext _userContext;

        public LowStockEventHandler(IEmailService emailService, IUserContext userContext)
        {
            _emailService = emailService;
            _userContext = userContext;
        }
        public async Task Handle(LowStockEvent notification, CancellationToken cancellationToken)
        {
            await _emailService.SendAsync("A Product is Low in Stock", _userContext.GetLoggedUserEmail, LowStockEmailTemplate.Get(
                    $"Product {notification.ProductName} On WareHouse {notification.WarehouseName} Has Gone Low on Stock"
                    ));
        }
    }
}
