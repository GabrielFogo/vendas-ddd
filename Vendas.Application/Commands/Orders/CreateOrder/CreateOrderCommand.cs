using Vendas.Domain.ValueObjects;

namespace Vendas.Application.Commands.Orders.CreateOrder;

public sealed record CreateOrderCommand(
    Guid CostumerId,
    DeliveryAddress DeliveryAddress
);