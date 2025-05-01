using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Caching
{
    public interface ICachingService
    {
        void Set<T>(string key, T value, TimeSpan timeSpan);

        bool TryGetValue<T>(string key, out T? cachedResponse);

        void Remove(Func<string, bool> Predicate);
    }
}
