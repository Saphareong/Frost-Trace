namespace FrostTrace.Api.Features.Batches.CreateBatch;

public record CreateBatchRequest(
    string Name,
    string OwnerId,
    double MinTempCelsius,
    double MaxTempCelsius,
    string? ParentId = null
);

public record CreateBatchResponse(
    string Id,
    string Name,
    string Status,
    DateTime CreatedAt
);
