using Vendas.Domain.Orders.Entities;

namespace Vendas.Application.Abstractions.Persistence;

public interface IOrderRepository
{
    Task<Order?> GetOrderAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateOrderAsync(Order order, CancellationToken cancellationToken = default);
}