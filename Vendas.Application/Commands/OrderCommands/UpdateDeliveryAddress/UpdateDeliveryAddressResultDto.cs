namespace Vendas.Application.Commands.OrderCommands.UpdateDeliveryAddress;

public record UpdateDeliveryAddressResultDto(
    Guid OrderId,
    string DeliveryAddress,
    string Status);