using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.RemoveItemFromOrder;

public sealed class RemoveItemFromOrderCommandHandler(IOrderRepository orderRepository): 
    ICommandHandler<RemoveItemFromOrderCommand, RemoveItemFromOrderResultDto>
{
    public async Task<RemoveItemFromOrderResultDto> HandleAsync(
        RemoveItemFromOrderCommand command, 
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderAsync(command.OrderId, cancellationToken);
        
        if(order is null) throw new InvalidOperationException($"Order with id {command.OrderId} not found");
        
        order.RemoveItem(command.ItemId);
        
        await orderRepository.UpdateOrderAsync(order, cancellationToken);
        
        return new RemoveItemFromOrderResultDto(order.Id, order.TotalPrice, order.Status.ToString());
    }
}