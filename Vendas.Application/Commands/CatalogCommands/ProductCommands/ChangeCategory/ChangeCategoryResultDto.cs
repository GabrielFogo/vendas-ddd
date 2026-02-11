namespace Vendas.Application.Commands.CatalogCommands.ProductCommands.ChangeCategory;

public sealed record ChangeCategoryResultDto(Guid ProductId, Guid CategoryId);