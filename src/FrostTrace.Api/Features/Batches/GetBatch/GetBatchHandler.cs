using FrostTrace.Api.Infrastructure.Persistence;
using MediatR;
using MongoDB.Driver;

namespace FrostTrace.Api.Features.Batches.GetBatch;

public record GetBatchQuery(string Id) : IRequest<GetBatchResponse?>;

public class GetBatchHandler : IRequestHandler<GetBatchQuery, GetBatchResponse?>
{
    private readonly MongoDbContext _db;

    public GetBatchHandler(MongoDbContext db)
    {
        _db = db;
    }

    public async Task<GetBatchResponse?> Handle(GetBatchQuery request, CancellationToken cancellationToken)
    {
        var batch = await _db.Batches
            .Find(b => b.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (batch is null) return null;

        return new GetBatchResponse(
            batch.Id,
            batch.Name,
            batch.OwnerId,
            batch.ParentId,
            batch.MinTempCelsius,
            batch.MaxTempCelsius,
            batch.Status.ToString(),
            batch.CreatedAt,
            batch.UpdatedAt
        );
    }
}
