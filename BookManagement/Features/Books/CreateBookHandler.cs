using BookManagement.Persistence;
using BookManagement.Validators;

namespace BookManagement.Features.Books;

public class CreateBookHandler(BookManagementContext context)
{
    private readonly BookManagementContext _context = context;
    
    public async Task<IResult> Handle(CreateBookRequest request)
    {
        // TODO - create a middleware for validation
        var validator = new CreateBookValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }
        var book = new Book(Guid.NewGuid(), request.Title, request.Author, request.Year);
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return Results.Created($"/books/{book.Id}", book);
    }
}