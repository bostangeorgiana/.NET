using System.ComponentModel.DataAnnotations;
using System.Drawing;
using SixLabors.ImageSharp;

namespace Tema_individuala.Validators
{
    // Verifica formatul si dimensiunea imaginii
    public class ProductImageValidator : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private readonly long _maxFileSize = 2 * 1024 * 1024; // 2 MB
        private readonly int _minWidth = 200;
        private readonly int _minHeight = 200;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Imaginea e optionala

            var file = value as IFormFile;
            if (file == null)
                return new ValidationResult("Invalid image format.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return new ValidationResult("Image must be .jpg, .jpeg, .png, .gif, or .webp");

            if (file.Length > _maxFileSize)
                return new ValidationResult("Image size cannot exceed 2 MB.");

            try
            {
                using (var image = Image.FromStream(file.OpenReadStream()))
                {
                    if (image.Width < _minWidth || image.Height < _minHeight)
                        return new ValidationResult("Image must be at least 200x200 px.");
                }
            }
            catch
            {
                return new ValidationResult("Could not read image. File might be corrupted.");
            }

            return ValidationResult.Success;
        }
    }
}