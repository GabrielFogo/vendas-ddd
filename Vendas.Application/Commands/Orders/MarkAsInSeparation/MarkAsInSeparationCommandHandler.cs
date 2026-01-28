using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.MarkAsInSeparation;

public class MarkAsInSeparationCommandHandler : ICommandHandler<MarkAsInSeparationCommand, MarkAsInSeparationResultDto>
{
    private readonly IOrderRepository _orderRepository;

    public MarkAsInSeparationCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<MarkAsInSeparationResultDto> HandleAsync(MarkAsInSeparationCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderAsync(command.OrderId, cancellationToken) ?? 
                    throw new ArgumentNullException($"Order with id {command.OrderId} not found");
        
        order.MarkAsInSeparation();
        
        await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        
        return new MarkAsInSeparationResultDto();
    }
}