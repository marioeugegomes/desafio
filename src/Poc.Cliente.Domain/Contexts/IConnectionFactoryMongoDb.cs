using MongoDB.Driver;

namespace Poc.Cliente.Domain.Contexts;

public interface IConnectionFactoryMongoDb
{
    IMongoDatabase GetDatabase(string databaseName);
    IMongoClient GetClient();
}
