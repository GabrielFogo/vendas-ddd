using Vendas.Domain.ValueObjects;

namespace Vendas.Application.Commands.OrderCommands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid CostumerId,
    DeliveryAddress DeliveryAddress
);