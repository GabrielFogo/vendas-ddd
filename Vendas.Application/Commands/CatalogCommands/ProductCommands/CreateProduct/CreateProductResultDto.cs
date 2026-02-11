namespace Vendas.Application.Commands.CatalogCommands.ProductCommands.CreateProduct;

public record CreateProductResultDto(
    Guid Id,
    string Name,
    decimal Price,
    string Status);