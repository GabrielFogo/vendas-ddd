namespace Vendas.Application.Commands.CatalogCommands.ProductCommands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Code,
    decimal Price,
    Guid CategoryId,
    int InitialStock = 0,
    string? Description = null);