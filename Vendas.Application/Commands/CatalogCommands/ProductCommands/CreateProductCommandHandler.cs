using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Catalog.Entities;
using Vendas.Domain.Catalog.ValueObjects;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Application.Commands.CatalogCommands.ProductCommands;

public sealed class CreateProductCommandHandler(
    ICategoryRepository categoryRepository,
    IProductRepository productRepository)
    : ICommandHandler<CreateProductCommand, CreateProductResultDto>
{
    public async Task<CreateProductResultDto> HandleAsync(CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetCategoryByIdAsync(command.CategoryId, cancellationToken) ??
                       throw new DomainException("Category not found");

        Guard.Against<DomainException>(!category.IsActive, "Category is not active");

        var name = new ProductName(command.Name);
        var code = new ProductCode(command.Code);
        var preco = new ProductPrice(command.Price);

        var product = Product.Create(
            name,
            code,
            preco,
            command.CategoryId,
            command.InitialStock,
            command.Description);

        await productRepository.AddAsync(product);

        return new CreateProductResultDto(
            product.Id,
            product.Name.Value,
            product.Price.Value,
            nameof(product.Status));
    }
}