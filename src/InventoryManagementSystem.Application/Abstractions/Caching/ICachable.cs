using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Caching
{
    public interface ICachable
    {
        string CacheKey { get; }
        int DurationInSeconds { get; }
    }
}
