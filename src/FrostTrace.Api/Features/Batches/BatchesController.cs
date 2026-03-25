using FrostTrace.Api.Features.Batches.CreateBatch;
using FrostTrace.Api.Features.Batches.GetBatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrostTrace.Api.Features.Batches;

[ApiController]
[Route("api/[controller]")]
public class BatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BatchesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new genesis or child batch.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateBatchResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBatch([FromBody] CreateBatchRequest request, CancellationToken ct)
    {
        // FluentValidation is automatically triggered by [ApiController] if using the correct middleware,
        // but since we are in Vertical Slice, we can also manually send it to mediator.
        // We'll keep the MediatoR pattern consistent.

        var command = new CreateBatchCommand(
            request.Name,
            request.OwnerId,
            request.MinTempCelsius,
            request.MaxTempCelsius,
            request.ParentId
        );

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetBatch), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get a batch by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetBatchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBatch([FromRoute] string id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBatchQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }
}
