using FluentValidation;
using Tema_individuala.Features.Products.DTOs;

namespace Tema_individuala.Validators;

// Valideaza campurile din CreateProductProfileRequest
public class CreateProductValidator : AbstractValidator<CreateProductProfileRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(3).WithMessage("Product name must be at least 3 characters long.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required.");

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.")
            .Length(3, 20).WithMessage("SKU must be between 3 and 20 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.ReleaseDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Release date cannot be in the future.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
    }
}