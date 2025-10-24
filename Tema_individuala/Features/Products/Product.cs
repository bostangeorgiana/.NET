namespace Tema_individuala.Features.Products;

// Entitatea Product reprezinta structura tabelului din baza de date
// Va fi mapata de EF Core automat
//Product este o entitate EF Core -> reprezintă tabelul Products în baza de date
public class Product
{
    public Guid Id { get; set; } // Identificator unic
    public string Name { get; set; } = string.Empty; // Numele produsului
    public string Brand { get; set; } = string.Empty; // Marca
    public string SKU { get; set; } = string.Empty; // Cod unic produs
    public ProductCategory Category { get; set; } // Categoria
    public decimal Price { get; set; } // Pretul
    public DateTime ReleaseDate { get; set; } // Data lansarii
    public string? ImageUrl { get; set; } // Poza optionala
    public bool IsAvailable { get; set; } // Disponibilitate
    public int StockQuantity { get; set; } // Numar stoc
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Cand a fost creat
    public DateTime? UpdatedAt { get; set; } // Cand a fost modificat ultima oara
}