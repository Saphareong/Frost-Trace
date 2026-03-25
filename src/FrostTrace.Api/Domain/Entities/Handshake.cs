using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FrostTrace.Api.Domain.Entities;

public enum HandshakeStatus
{
    PENDING,
    ACCEPTED,
    REJECTED,
    EXPIRED
}

public class Handshake
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string BatchId { get; set; } = default!;

    /// <summary>UUID/QR code token used for handshake verification.</summary>
    public string Token { get; set; } = Guid.NewGuid().ToString();

    public string InitiatorId { get; set; } = default!;
    public string RecipientId { get; set; } = default!;

    public HandshakeStatus Status { get; set; } = HandshakeStatus.PENDING;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
