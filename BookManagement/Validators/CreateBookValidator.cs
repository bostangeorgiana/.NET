using FluentValidation;
using BookManagement.Features.Books;

namespace BookManagement.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookValidator()
    {
        RuleFor(x=> x.Title).NotNull().NotEmpty().MinimumLength(3).WithMessage("FullName must be at least 3 characters long.");
        RuleFor(x => x.Author).NotNull().NotEmpty().MinimumLength(3).WithMessage("A valid email is required.");
        RuleFor(x => x.Year).GreaterThan(0).WithMessage("Year must be a positive number.");
    }
}