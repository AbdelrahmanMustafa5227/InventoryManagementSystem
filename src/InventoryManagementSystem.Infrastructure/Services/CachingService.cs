using InventoryManagementSystem.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Services
{
    internal class CachingService : ICachingService
    {
        private readonly IMemoryCache _cache;

        private List<string> _cacheKeys = new();

        public CachingService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set<T>(string key, T value, TimeSpan timeSpan)
        {
            _cache.Set(key, value, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeSpan });
            _cacheKeys.Add(key);
        }

        public bool TryGetValue<T>(string key, out T? cachedResponse)
        {
            return _cache.TryGetValue(key, out cachedResponse);
        }

        public void Remove(Func<string, bool> Predicate)
        {
            var keysToRemove = _cacheKeys.Where(Predicate).ToList();

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                _cacheKeys.Remove(key);
            }
        }
    }
}
