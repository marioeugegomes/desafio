using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Poc.Cliente.Domain.Entities;

namespace Poc.Cliente.Domain.Repositories;
public interface ITransactionRepository
{
   void Save(Transaction transaction);

   Task<List<Transaction>> GetTransactionsForUser(string document);
}
