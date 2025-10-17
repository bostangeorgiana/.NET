namespace BookManagement.Features.Books;

public record GetAllBookRequest(
    string? Author,
    string? SortBy,         
    bool Descending = false,
    int Page = 1,
    int PageSize = 10
);
