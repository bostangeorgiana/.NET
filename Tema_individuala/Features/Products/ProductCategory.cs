namespace Tema_individuala.Features.Products;

// Enumul defineste categoriile disponibile pentru produse.
// Fiecare valoare are un int asociat (incepand de la 0).
// Enumurile sunt tipuri speciale care definesc un set fix de valori posibile.
// Aici definim tipurile de produse pe care API-ul nostru le va accepta.
public enum ProductCategory
{
    Electronics = 0,
    Clothing = 1,
    Books = 2,
    Home = 3
}