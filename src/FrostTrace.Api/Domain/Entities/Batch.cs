using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using FrostTrace.Api.Domain.Enums;

namespace FrostTrace.Api.Domain.Entities;

public class Batch
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string OwnerId { get; set; } = default!;

    /// <summary>Parent batch (for lineage tracing). Null if genesis batch.</summary>
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ParentId { get; set; }

    public double MinTempCelsius { get; set; }
    public double MaxTempCelsius { get; set; }

    public BatchStatus Status { get; set; } = BatchStatus.CREATED;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
