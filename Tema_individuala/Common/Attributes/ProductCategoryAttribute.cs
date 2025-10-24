using System.ComponentModel.DataAnnotations;
using Tema_individuala.Features.Products;

namespace Tema_individuala.Common.Attributes
{
    public class ProductCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Category is required.");

            if (!Enum.IsDefined(typeof(ProductCategory), value))
                return new ValidationResult("Invalid product category.");

            return ValidationResult.Success;
        }
    }
}