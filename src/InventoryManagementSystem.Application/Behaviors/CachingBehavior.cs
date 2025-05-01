using InventoryManagementSystem.Application.Abstractions.Caching;
using InventoryManagementSystem.Application.Abstractions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Behaviors
{
    internal class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachable
    {
        private readonly IAppLogger<TRequest> _logger;
        private readonly ICachingService _cachingService;

        public CachingBehaviour(IAppLogger<TRequest> logger, ICachingService cachingService)
        {
            _logger = logger;
            _cachingService = cachingService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            if (_cachingService.TryGetValue(request.CacheKey, out TResponse? cachedResponse))
            {
                _logger.LogInformation("{0} - Fetching Response from Memory", "Cache Hit");
                return cachedResponse!;
            }

            _logger.LogInformation("{0} - Fetching Response from Database", "Cache Miss");
            var response = await next();

            _cachingService.Set(request.CacheKey, response, TimeSpan.FromSeconds(request.DurationInSeconds));
            return response;
        }
    }
}
