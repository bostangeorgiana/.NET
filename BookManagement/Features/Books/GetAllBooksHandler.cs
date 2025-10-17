using Microsoft.EntityFrameworkCore;
using BookManagement.Persistence;
namespace BookManagement.Features.Books;

public class GetAllBooksHandler(BookManagementContext context)
{
    private readonly BookManagementContext _context = context;
    
    public async Task<IResult> Handle(GetAllBookRequest request)
    {
        IQueryable<Book> query = _context.Books;
        
        //Foltering:
        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            query = query.Where(b => b.Author.Contains(request.Author));
        }
        
        //Sorting:
        query = request.SortBy?.ToLower() switch
        {
            "title" => request.Descending
                ? query.OrderByDescending(b => b.Title)
                : query.OrderBy(b => b.Title),

            "year" => request.Descending
                ? query.OrderByDescending(b => b.Year)
                : query.OrderBy(b => b.Year),

            _ => query
        };
        
        //Pagination:
            query = query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);
        
        var books = await _context.Books.ToListAsync();
        return Results.Ok(books);
    }
}