using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Poc.Cliente.Domain.Entities;

namespace Poc.Cliente.Infra.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> ListForUser(string document);
    }
}
