namespace BookManagement.Features.Books;

public record CreateBookRequest(string Title, string Author, int Year);