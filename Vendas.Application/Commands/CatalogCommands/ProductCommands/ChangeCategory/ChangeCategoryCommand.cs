namespace Vendas.Application.Commands.CatalogCommands.ProductCommands.ChangeCategory;

public sealed record ChangeCategoryCommand(Guid ProductId, Guid NewCategoryId);