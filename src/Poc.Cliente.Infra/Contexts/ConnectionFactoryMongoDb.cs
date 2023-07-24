using System;
using Poc.Cliente.Domain.Contexts;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Poc.Cliente.Infra.Contexts;

public class ConnectionFactoryMongoDb : IConnectionFactoryMongoDb
{
    private IMongoClient _client;
    private readonly string _connectionString;

    public ConnectionFactoryMongoDb(IConfiguration cfg)
    {
        _connectionString = cfg.GetConnectionString("mongoDB");

        if (string.IsNullOrEmpty(_connectionString))
            throw new Exception("A string de conexão com o banco de dados não pode ser nula ou vazia.");
    }

    public IMongoDatabase GetDatabase(string databaseName)
    {
        return GetClient().GetDatabase(databaseName);
    }

    public IMongoClient GetClient()
    {
        if (_client == null)
            _client = new MongoClient(_connectionString);

        return _client;
    }
}
