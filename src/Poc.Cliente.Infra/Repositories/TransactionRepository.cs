using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Poc.Cliente.Domain.Contexts;
using Poc.Cliente.Domain.Entities;
using Poc.Cliente.Domain.Repositories;
using Poc.Cliente.Infra.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;


namespace Poc.Cliente.Infra.Repositories;

public class TransactionRepository : MongoDbRepository<Transaction>, ITransactionRepository
{
    private readonly ILogger<RequestLoggerMiddleware> _logger;
    protected readonly IMongoCollection<Transaction> _collectionTransaction;

    public TransactionRepository(
        IConfiguration cfg,
        IConnectionFactoryMongoDb connectionFactory) : base(cfg, connectionFactory)
    {

        var collectionName = typeof(Transaction).Name;
        _collectionTransaction = connectionFactory.GetDatabase(_databaseName).GetCollection<Transaction>(collectionName);
    }


    public async void Save(Transaction entitie) {
        await InsertAsync(entitie);
    }

    public async Task<List<Transaction>> GetAll()
    {
        return await GetAllAsync();
    }

    public async Task<List<Transaction>> GetTransactionsForUser(string document) {
        var query = Builders<Transaction>.Filter.Eq(x => x.account.document, document);
        return await _collectionTransaction.Find(query).ToListAsync();
    }
}
