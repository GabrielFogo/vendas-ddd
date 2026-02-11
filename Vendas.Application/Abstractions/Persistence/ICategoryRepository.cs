using Vendas.Domain.Catalog.Entities;

namespace Vendas.Application.Abstractions.Persistence;

public interface ICategoryRepository
{
    public Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
}