using Tema_individuala.Persistence;
using Tema_individuala.Features.Products.DTOs;
using Microsoft.Extensions.Logging;

namespace Tema_individuala.Features.Products;

// Handlerul se ocupa cu logica de creare a produselor.
// Este similar cu un "service" care proceseaza un request.
public class CreateProductHandler
{
    private readonly ProductManagementContext _context;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(ProductManagementContext context, ILogger<CreateProductHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult> Handle(CreateProductProfileRequest request)
    {
        _logger.LogInformation("Creating product: {Name}, Brand: {Brand}", request.Name, request.Brand);

        // TODO: vom adauga validare FluentValidation aici (PAS 7)

        // Creem un nou produs (manual, fara mapping)
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Brand = request.Brand,
            SKU = request.SKU,
            Category = request.Category,
            Price = request.Price,
            ReleaseDate = request.ReleaseDate,
            ImageUrl = request.ImageUrl,
            StockQuantity = request.StockQuantity,
            IsAvailable = request.StockQuantity > 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);

        return Results.Created($"/products/{product.Id}", product);
    }
}