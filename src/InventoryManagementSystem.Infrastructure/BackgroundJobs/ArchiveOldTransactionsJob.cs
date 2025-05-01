using Hangfire;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Infrastructure.Persistence.Repositories;
using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.BackgroundJobs
{
    public class ArchiveOldTransactionsJob
    {
        private readonly ITransactionRepository _transactionRepository;

        public ArchiveOldTransactionsJob(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public void Execute()
        {
            _transactionRepository.ArchiveTransactionsOlderThan1Year(); 
        }
    }
}
