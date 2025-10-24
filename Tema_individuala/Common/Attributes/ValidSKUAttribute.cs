using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Tema_individuala.Common.Attributes
{
    public class ValidSKUAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("SKU is required.");

            string sku = value.ToString()!;
            var pattern = "^[a-zA-Z0-9-]{5,20}$";

            if (!Regex.IsMatch(sku, pattern))
                return new ValidationResult("SKU must be 5-20 characters long and can contain only letters, numbers, and hyphens.");

            return ValidationResult.Success;
        }
    }
}