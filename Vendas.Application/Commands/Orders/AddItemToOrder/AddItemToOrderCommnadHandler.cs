using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.AddItemToOrder;

public sealed class AddItemToOrderCommandHandler(IOrderRepository orderRepository): ICommandHandler<AddItemsToOrderCommand, AddItemToOrderResultDto>
{
    public async Task<AddItemToOrderResultDto> HandleAsync(AddItemsToOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetOrderAsync(command.OrderId, cancellationToken);

        if (order is null) throw new InvalidOperationException($"Order with id {command.OrderId} does not exist.");
        
        order.AddItem(command.ProductId, command.ProductName, command.UnitPrice, command.Quantity);
        
        await orderRepository.UpdateOrderAsync(order, cancellationToken);
        
        return new AddItemToOrderResultDto(order.Id, order.TotalPrice, order.Status.ToString());
    }
}