using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Services
{
    public class IdempotencyService
    {
        private readonly IMemoryCache _cache;

        public IdempotencyService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public bool IdempotentRequestExists(Guid guid)
        {
            var obj = _cache.Get(guid.ToString());

            return obj != null;
        }

        public int GetIdempotentRequest(Guid guid)
        {
            return (int)_cache.Get(guid.ToString())!;
        }

        public void CreateIdempotentRequest(Guid guid, int statusCode)
        {
            _cache.Set(
               guid.ToString(),
               statusCode,
               new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10) });
        }
    }
}
