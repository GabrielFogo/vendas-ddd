namespace Vendas.Application.Commands.Orders.UpdateDeliveryAddress;

public record UpdateDeliveryAddressResultDto(
    Guid OrderId, 
    string DeliveryAddress, 
    string Status);