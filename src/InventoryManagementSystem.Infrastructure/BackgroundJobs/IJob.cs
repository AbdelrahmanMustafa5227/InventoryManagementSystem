using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.BackgroundJobs
{
    public interface IJob
    {
        Task Execute();
    }
}
