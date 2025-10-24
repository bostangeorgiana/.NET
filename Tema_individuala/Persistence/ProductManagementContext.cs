using Microsoft.EntityFrameworkCore;
using Tema_individuala.Features.Products;

namespace Tema_individuala.Persistence;

// DbContext-ul gestioneaza comunicarea dintre aplicatie si baza de date.
// Fiecare DbSet<T> devine un tabel in baza de date.
public class ProductManagementContext : DbContext
{
    public ProductManagementContext(DbContextOptions<ProductManagementContext> options)
        : base(options)
    {
    }

    // Reprezinta tabelul "Products" in baza de date
    public DbSet<Product> Products { get; set; } = null!;
}