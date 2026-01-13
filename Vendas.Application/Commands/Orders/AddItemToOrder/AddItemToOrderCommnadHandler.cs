using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.AddItemToOrder;

public sealed class AddItemToOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public AddItemToOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<AddItemToOrderResultDto> HandleAsync(AddItemsToOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetOrderAsync(command.OrderId, cancellationToken);

        if (order is null) throw new InvalidOperationException($"Order with id {command.OrderId} does not exist.");
        
        order.AddItem(command.ProductId, command.ProductName, command.UnitPrice, command.Quantity);
        
        await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        
        return new AddItemToOrderResultDto(order.Id, order.TotalPrice, order.Status.ToString());
    }
}