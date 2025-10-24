using System.ComponentModel.DataAnnotations;
using Tema_individuala.Common.Attributes;
using Tema_individuala.Validators;

namespace Tema_individuala.Features.Products.DTOs
{
    // DTO pentru request-ul de creare produs
    public class CreateProductProfileRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Brand { get; set; } = string.Empty;
        
        [Required]
        [ValidSKU]
        public string SKU { get; set; } = string.Empty;
        
        [Required]
        [ProductCategory]
        public ProductCategory Category { get; set; }
        
        [Required]
        [PriceRange(1, 10000)]
        public decimal Price { get; set; }
        
        
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
        public string? ImageUrl { get; set; }
        public int StockQuantity { get; set; } = 1;
        
        [ProductImageValidator] // Custom validator pentru imagine
        public IFormFile? Image { get; set; }

    }
}