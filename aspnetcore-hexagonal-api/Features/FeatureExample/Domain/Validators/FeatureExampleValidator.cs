using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Models;
using FluentValidation;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Validators;

public class FeatureExampleValidator : AbstractValidator<FeatureExampleEntity>
{
    public FeatureExampleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid status value");

        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .WithMessage("CreatedAt is required");
    }
}