using Vendas.Domain.ValueObjects;

namespace Vendas.Application.Commands.Orders.UpdateDeliveryAddress;

public record UpdateDeliveryCommand(Guid OrderId, DeliveryAddress DeliveryAddress);