using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Poc.Cliente.Domain.Contexts;
using Poc.Cliente.Domain.Entities;
using Poc.Cliente.Domain.Repositories;
using Poc.Cliente.Infra.Extensions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Poc.Cliente.Infra.Repositories;

public abstract class MongoDbRepository<T> : IMongoDbRepository<T> where T : Entidade
{
    protected readonly IMongoCollection<T> _collection;
    protected readonly string _databaseName;

    protected MongoDbRepository(IConfiguration cfg, IConnectionFactoryMongoDb connectionFactory)
    {
        RegistrarConvencaoCamelCase();

        _databaseName = cfg["mongoDB:DatabaseName"];
        if (string.IsNullOrEmpty(_databaseName))
            throw new Exception("Não foi possível obter o nome do banco de dados.");

        var collectionName = typeof(T).Name;

        _collection = connectionFactory.GetDatabase(_databaseName).GetCollection<T>(collectionName);
    }

    private void RegistrarConvencaoCamelCase()
    {
        var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filtro = Builders<T>.Filter.Where(x => x.Id == id);
        await _collection.DeleteOneAsync(filtro, cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var collections = await _collection
                .Find(_ => true)
                .ToListAsync(cancellationToken);

            return collections;
        }
        catch
        {
            throw;
        }
    }

    public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var collection = await _collection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            return collection;
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<T>> GetByCondition(FilterDefinition<T> filter)
    {
        try
        {
            var result = await _collection. Find(filter).ToListAsync();

            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task InsertAsync(T obj, CancellationToken cancellationToken = default)
    {
        try
        {
            await _collection.InsertOneAsync(obj, cancellationToken: cancellationToken);
        }
        catch
        {
            throw;
        }
    }

    public async Task InsertManyAsync(List<T> lstObj, CancellationToken cancellationToken = default)
    {
        try
        {
            var lstInserir = new List<T>();
            for (int i = 0; i < lstObj.Count; i++)
            {
                var novoObj = lstObj[i];
                novoObj.Id = Guid.NewGuid().ToString();

                lstInserir.Add(novoObj);
            }

            await _collection.InsertManyAsync(lstInserir, cancellationToken: cancellationToken);
        }
        catch
        {
            throw;
        }
    }

    public async Task UpdateAsync(string id, T obj, CancellationToken cancellationToken = default)
    {
        try
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, obj, cancellationToken: cancellationToken);
        }
        catch
        {
            throw;
        }
    }
}