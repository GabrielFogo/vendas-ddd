using Vendas.Domain.ValueObjects;

namespace Vendas.Application.Commands.OrderCommands.UpdateDeliveryAddress;

public sealed record UpdateDeliveryCommand(Guid OrderId, DeliveryAddress DeliveryAddress);