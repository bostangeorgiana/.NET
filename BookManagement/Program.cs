using FluentValidation;
using Microsoft.EntityFrameworkCore;
using BookManagement.Features.Books;
using BookManagement.Persistence;
using BookManagement.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<BookManagementContext>(options =>
    options.UseSqlite("Data Source=bookmanagement.db"));
builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddScoped<GetAllBooksHandler>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();

var app = builder.Build();


// Ensure the database is created at runtime
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookManagementContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/books", async (CreateBookRequest req, CreateBookHandler handler) =>
    await handler.Handle(req));
//app.MapGet("/books", async (GetAllBooksHandler handler) =>
//    await handler.Handle(new GetAllBookRequest()));
app.MapGet("/books", async (string? author, string? sortBy, bool? descending, int? page, int? pageSize,
    GetAllBooksHandler handler) =>
{
    var request = new GetAllBookRequest(Author: author, SortBy: sortBy, Descending: descending ?? false, Page: page ?? 1, PageSize: pageSize ?? 10);
    return await handler.Handle(request);
});

app.MapDelete("/books/{id:guid}", async (Guid id, DeleteBookHandler handler) =>
{
    await handler.Handle(new DeletebookRequest(id));
});


app.Run();
