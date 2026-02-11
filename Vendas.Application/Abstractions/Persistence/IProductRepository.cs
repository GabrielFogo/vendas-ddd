using Vendas.Domain.Catalog.Entities;

namespace Vendas.Application.Abstractions.Persistence;

public interface IProductRepository
{
    public Task AddAsync(Product product);
}