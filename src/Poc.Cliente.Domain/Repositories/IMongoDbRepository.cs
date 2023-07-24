using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Poc.Cliente.Domain.Repositories;
public interface IMongoDbRepository<T>
{
    Task<List<T>> GetByCondition(FilterDefinition<T> filter);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task InsertAsync(T obj, CancellationToken cancellationToken = default);
    Task InsertManyAsync(List<T> lstObj, CancellationToken cancellationToken = default);
    Task UpdateAsync(string id, T obj, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}