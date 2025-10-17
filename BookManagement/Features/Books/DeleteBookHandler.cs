using Microsoft.EntityFrameworkCore;
using BookManagement.Persistence;

namespace BookManagement.Features.Books;

public class DeleteBookHandler(BookManagementContext dbContext)
{
    private readonly BookManagementContext _dbContext = dbContext;
    
    public async Task<IResult> Handle(DeletebookRequest request)
    {
        var book = await _dbContext.Books.FirstOrDefaultAsync( b=>b.Id == request.Id);
        if (book == null)
        {
            return Results.NotFound();
        }

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
}