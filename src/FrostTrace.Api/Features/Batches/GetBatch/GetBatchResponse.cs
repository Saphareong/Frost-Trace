namespace FrostTrace.Api.Features.Batches.GetBatch;

public record GetBatchResponse(
    string Id,
    string Name,
    string OwnerId,
    string? ParentId,
    double MinTempCelsius,
    double MaxTempCelsius,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
