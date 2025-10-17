using Microsoft.EntityFrameworkCore;
using BookManagement.Features.Books;

namespace BookManagement.Persistence;

public class BookManagementContext(DbContextOptions<BookManagementContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
}