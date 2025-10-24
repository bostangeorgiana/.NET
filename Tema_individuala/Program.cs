using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Tema_individuala.Persistence;
using Tema_individuala.Features.Products;
using Tema_individuala.Features.Products.DTOs;
using Tema_individuala.Validators;

var builder = WebApplication.CreateBuilder(args);

// --- EF Core + SQLite ---
builder.Services.AddDbContext<ProductManagementContext>(options =>
    options.UseSqlite("Data Source=productmanagement.db"));

// --- Handlers + Validators ---
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<GetAllProductsHandler>();
builder.Services.AddScoped<CreateProductProfileValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductProfileValidator>();

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Creeaza baza de date daca nu exista ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductManagementContext>();
    db.Database.EnsureCreated();
}

// --- Swagger UI ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<Tema_individuala.Common.Middleware.CorrelationMiddleware>();

// --- Endpoint pentru creare produse ---
app.MapPost("/products", async (
    CreateProductProfileRequest req,
    CreateProductHandler handler,
    CreateProductProfileValidator validator) =>
{
    var validationResult = await validator.ValidateAsync(req);

    if (!validationResult.IsValid)
        return Results.BadRequest(validationResult.Errors);

    return await handler.Handle(req);
});

// --- Endpoint pentru listare produse ---
app.MapGet("/products", async (GetAllProductsHandler handler) =>
{
    return await handler.Handle();
});

app.Run();