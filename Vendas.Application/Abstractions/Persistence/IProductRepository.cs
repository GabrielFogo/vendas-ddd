using Vendas.Domain.Catalog.Entities;

namespace Vendas.Application.Abstractions.Persistence;

public interface IProductRepository
{
    public Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task AddProductAsync(Product product, CancellationToken cancellationToken);
    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken);
}