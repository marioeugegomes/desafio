using MongoDB.Bson.Serialization.Attributes;

namespace Poc.Cliente.Domain.Entities;

public abstract class Entidade
{
    [BsonId]
    [BsonElement("id")]
    public string Id { get; set; }
}