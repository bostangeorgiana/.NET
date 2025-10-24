namespace Tema_individuala.Features.Products.DTOs;

// Acest DTO este folosit pentru a afisa produsele in API responses.
// Nu contine logica, doar campurile care se trimit catre client.
public class ProductProfileDto
{
    public Guid Id { get; set; }                  // Identificator unic
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string CategoryDisplayName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FormattedPrice { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public int StockQuantity { get; set; }
    public string ProductAge { get; set; } = string.Empty;
    public string BrandInitials { get; set; } = string.Empty;
    public string AvailabilityStatus { get; set; } = string.Empty;
}