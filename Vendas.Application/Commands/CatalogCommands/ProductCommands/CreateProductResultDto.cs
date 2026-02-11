namespace Vendas.Application.Commands.CatalogCommands.ProductCommands;

public record CreateProductResultDto(
    Guid Id,
    string Name,
    decimal Price,
    string Status);