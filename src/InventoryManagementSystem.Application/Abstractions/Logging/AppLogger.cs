using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Logging
{
    internal class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILogger<T> logger)
        {
            _logger = logger;
        }
        public void LogError(string? message, params object?[] args)
        {
            _logger.LogError(null, message, args);
        }
        public void LogWarning(string? message, params object?[] args)
        {
            _logger.LogWarning(message, args);
        }
        public void LogInformation(string? message, params object?[] args)
        {
            _logger.LogInformation(message, args);
        }
    }
}
