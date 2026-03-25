using FrostTrace.Api.Domain.Entities;
using FrostTrace.Api.Infrastructure.Persistence;
using MediatR;
using MongoDB.Driver;

namespace FrostTrace.Api.Features.Batches.CreateBatch;

public record CreateBatchCommand(
    string Name,
    string OwnerId,
    double MinTempCelsius,
    double MaxTempCelsius,
    string? ParentId
) : IRequest<CreateBatchResponse>;

public class CreateBatchHandler : IRequestHandler<CreateBatchCommand, CreateBatchResponse>
{
    private readonly MongoDbContext _db;

    public CreateBatchHandler(MongoDbContext db)
    {
        _db = db;
    }

    public async Task<CreateBatchResponse> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = new Batch
        {
            Name = request.Name,
            OwnerId = request.OwnerId,
            MinTempCelsius = request.MinTempCelsius,
            MaxTempCelsius = request.MaxTempCelsius,
            ParentId = request.ParentId,
        };

        await _db.Batches.InsertOneAsync(batch, cancellationToken: cancellationToken);

        return new CreateBatchResponse(
            batch.Id,
            batch.Name,
            batch.Status.ToString(),
            batch.CreatedAt
        );
    }
}
