using Vendas.Domain.ValueObjects;

namespace Vendas.Application.Commands.Orders.CreateOrder;

public sealed class CreateOrderCommand
{
    public Guid CostumerId { get; }
    public DeliveryAddress DeliveryAddress { get; }

    public CreateOrderCommand(Guid costumerId, DeliveryAddress deliveryAddress)
    {
        CostumerId = costumerId;
        DeliveryAddress = deliveryAddress;
    }
}