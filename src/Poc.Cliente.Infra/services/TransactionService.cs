using System.Collections.Generic;
using System.Threading.Tasks;
using Poc.Cliente.Domain.Entities;
using Poc.Cliente.Domain.Repositories;

namespace Poc.Cliente.Infra.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _repository;

        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Transaction>> ListForUser(string document) {
            return await _repository.GetTransactionsForUser(document);
        }
    }
}
