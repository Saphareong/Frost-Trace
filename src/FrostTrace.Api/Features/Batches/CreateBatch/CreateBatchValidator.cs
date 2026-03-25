using FluentValidation;

namespace FrostTrace.Api.Features.Batches.CreateBatch;

public class CreateBatchValidator : AbstractValidator<CreateBatchRequest>
{
    public CreateBatchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Batch name is required.")
            .MaximumLength(200);

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");

        RuleFor(x => x.MinTempCelsius)
            .LessThan(x => x.MaxTempCelsius)
            .WithMessage("Min temperature must be less than max temperature.");

        RuleFor(x => x.MaxTempCelsius)
            .GreaterThan(x => x.MinTempCelsius)
            .WithMessage("Max temperature must be greater than min temperature.");
    }
}
