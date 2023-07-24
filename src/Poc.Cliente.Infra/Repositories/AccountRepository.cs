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

public class AccountRepository : MongoDbRepository<Account>, IAccountRepository
{
    private readonly ILogger<RequestLoggerMiddleware> _logger;
    protected readonly IMongoCollection<Transaction> _collectionTransaction;

       public AccountRepository(
        IConfiguration cfg,
        IConnectionFactoryMongoDb connectionFactory) : base(cfg, connectionFactory)
    {

        var collectionName = typeof(Account).Name;
        _collectionTransaction = connectionFactory.GetDatabase(_databaseName).GetCollection<Transaction>(collectionName);
    }

}
