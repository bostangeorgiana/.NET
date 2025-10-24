using Microsoft.EntityFrameworkCore;
using Tema_individuala.Persistence;

namespace Tema_individuala.Features.Products;

// Handlerul se ocupă cu extragerea produselor din baza de date
public class GetAllProductsHandler
{
    private readonly ProductManagementContext _context;

    public GetAllProductsHandler(ProductManagementContext context)
    {
        _context = context;
    }

    // Returneaza toate produsele existente in baza
    public async Task<IResult> Handle()
    {
        var products = await _context.Products.ToListAsync();
        return Results.Ok(products);
    }
}