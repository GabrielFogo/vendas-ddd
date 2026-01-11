using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Orders.Entities;

namespace Vendas.Application.Commands.Orders.CreateOrder;

public sealed class CreateOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CreateOrderResultDto> HandleAsync(CreateOrderCommand command, 
        CancellationToken cancellationToken = default)
    {
        var order = Order.Create(command.CostumerId, command.DeliveryAddress);
        
        await _orderRepository.AddOrderAsync(order, cancellationToken);
        
        return new CreateOrderResultDto(
            order.Id,
            order.Code,
            order.CreatedAt,
            order.TotalPrice,
            order.Status.ToString());
    }
}