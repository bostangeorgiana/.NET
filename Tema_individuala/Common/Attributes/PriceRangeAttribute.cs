using System.ComponentModel.DataAnnotations;

namespace Tema_individuala.Common.Attributes
{
    public class PriceRangeAttribute : ValidationAttribute
    {
        private readonly decimal _min;
        private readonly decimal _max;

        public PriceRangeAttribute(double min, double max)
        {
            _min = (decimal)min;
            _max = (decimal)max;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Price is required.");

            if (value is not decimal price)
                return new ValidationResult("Invalid price format.");

            if (price < _min || price > _max)
                return new ValidationResult($"Price must be between {_min} and {_max}.");

            return ValidationResult.Success;
        }
    }
}