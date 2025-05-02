using Hangfire;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.BackgroundJobs
{
    public class ArchiveOldTransactionsJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ArchiveOldTransactionsJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute()
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                transactionRepository.ArchiveTransactionsOlderThan1Year();
            }
            await Task.CompletedTask;
        }
    }
}
