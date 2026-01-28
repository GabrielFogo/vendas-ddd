using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.MarkOrderAsDelivered;

public class MarkOrderAsDeliveredCommandHandler : ICommandHandler<MarkOrderAsDeliveredCommand, MarkOrderAsDeliveredResultDto>
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsDeliveredCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<MarkOrderAsDeliveredResultDto> HandleAsync(MarkOrderAsDeliveredCommand command,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderAsync(command.OrderId, cancellationToken) ?? 
                    throw new ArgumentNullException($"Order with id {command.OrderId} not found");
        
        order.MarkAsDelivered();

        await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        
        return new MarkOrderAsDeliveredResultDto(order.Id, order.Status.ToString());
    }
}