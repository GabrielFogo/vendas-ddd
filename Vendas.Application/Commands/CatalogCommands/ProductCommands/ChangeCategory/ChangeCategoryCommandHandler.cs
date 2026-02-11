using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Application.Commands.CatalogCommands.ProductCommands.ChangeCategory;

public sealed class ChangeCategoryCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository) : ICommandHandler<ChangeCategoryCommand, ChangeCategoryResultDto>
{
    public async Task<ChangeCategoryResultDto> HandleAsync(ChangeCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetCategoryByIdAsync(command.NewCategoryId, cancellationToken) ??
                       throw new DomainException("Category not found");

        Guard.Against<DomainException>(!category.IsActive, "Category is not active");

        var product = await productRepository.GetProductByIdAsync(command.ProductId, cancellationToken) ??
                      throw new DomainException("Product not found");
        
        product.ChangeCategoryId(command.NewCategoryId);

        await productRepository.UpdateProductAsync(product, cancellationToken);
        
        return new ChangeCategoryResultDto(product.Id, product.CategoryId);
    }
}