using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tema_individuala.Features.Products;
using Tema_individuala.Features.Products.DTOs;
using Tema_individuala.Persistence;

namespace Tema_individuala.Validators
{
    public class CreateProductProfileValidator : AbstractValidator<CreateProductProfileRequest>
    {
        private readonly ProductManagementContext _context;
        private readonly ILogger<CreateProductProfileValidator> _logger;

        private readonly List<string> _inappropriateWords = new() { "fake", "illegal", "banned", "adult" };
        private readonly List<string> _technologyKeywords = new() { "tech", "smart", "digital", "ai", "device", "gadget" };
        private readonly List<string> _homeRestrictedWords = new() { "weapon", "explosive", "toxic" };

        public CreateProductProfileValidator(ProductManagementContext context, ILogger<CreateProductProfileValidator> logger)
        {
            _context = context;
            _logger = logger;

            // --- NAME ---
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(1, 200).WithMessage("Product name must be between 1 and 200 characters.")
                .Must(BeValidName).WithMessage("Product name contains inappropriate words.")
                .MustAsync(BeUniqueName).WithMessage("A product with the same name already exists for this brand.");

            // --- BRAND ---
            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .Length(2, 100).WithMessage("Brand must be between 2 and 100 characters.")
                .Must(BeValidBrandName).WithMessage("Brand name contains invalid characters.");

            // --- SKU ---
            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .Matches("^[a-zA-Z0-9-]{5,20}$").WithMessage("SKU must be alphanumeric, 5-20 chars, and may include hyphens.")
                .MustAsync(BeUniqueSKU).WithMessage("SKU already exists in the system.");

            // --- CATEGORY ---
            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("Invalid product category.");

            // --- PRICE ---
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .LessThan(10000).WithMessage("Price must be less than $10,000.");

            // --- RELEASE DATE ---
            RuleFor(x => x.ReleaseDate)
                .Must(d => d.Year >= 1900).WithMessage("Release date cannot be before 1900.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Release date cannot be in the future.");

            // --- STOCK QUANTITY ---
            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.")
                .LessThanOrEqualTo(100000).WithMessage("Stock quantity cannot exceed 100,000.");

            // --- IMAGE URL ---
            When(x => !string.IsNullOrEmpty(x.ImageUrl), () =>
            {
                RuleFor(x => x.ImageUrl!)
                    .Must(BeValidImageUrl).WithMessage("Invalid image URL format.");
            });

            // --- BUSINESS RULES ---
            RuleFor(x => x)
                .MustAsync(PassBusinessRules).WithMessage("Product violates business rules.");
        }

        // ---------- HELPER METHODS ----------

        private bool BeValidName(string name)
        {
            return !_inappropriateWords.Any(word => name.Contains(word, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> BeUniqueName(CreateProductProfileRequest request, string name, CancellationToken token)
        {
            try
            {
                return !await _context.Products
                    .AnyAsync(p => p.Name == name && p.Brand == request.Brand, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating unique name for product {Name}", name);
                return false;
            }
        }

        private bool BeValidBrandName(string brand)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(brand, @"^[a-zA-Z0-9\s\-'\.]+$");
        }

        private bool BeValidImageUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                && (url.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                    || url.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                    || url.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                    || url.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                    || url.EndsWith(".webp", StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> BeUniqueSKU(string sku, CancellationToken token)
        {
            try
            {
                return !await _context.Products.AnyAsync(p => p.SKU == sku, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating SKU uniqueness for {SKU}", sku);
                return false;
            }
        }

        private async Task<bool> PassBusinessRules(CreateProductProfileRequest request, CancellationToken token)
        {
            try
            {
                // Rule 1: max 500 products/day
                var todayCount = await _context.Products.CountAsync(p => p.CreatedAt.Date == DateTime.UtcNow.Date, token);
                if (todayCount >= 500)
                {
                    _logger.LogWarning("Daily product creation limit reached ({Count}/500)", todayCount);
                    return false;
                }

                // Rule 2: Electronics minimum price
                if (request.Category == ProductCategory.Electronics && request.Price < 50)
                    return false;

                // Rule 3: Home restricted words
                if (request.Category == ProductCategory.Home && _homeRestrictedWords.Any(w => request.Name.Contains(w, StringComparison.OrdinalIgnoreCase)))
                    return false;

                // Rule 4: High-value product stock limit
                if (request.Price > 500 && request.StockQuantity > 10)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking business rules for product {Name}", request.Name);
                return false;
            }
        }
    }
}
