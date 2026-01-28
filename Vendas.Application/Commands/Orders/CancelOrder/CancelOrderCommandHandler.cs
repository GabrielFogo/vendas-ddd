using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.ValueObjects;

namespace Vendas.Application.Commands.Orders.CancelOrder;

public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, CancelOrderResultDto>
{
    private readonly IOrderRepository _orderRepository;
    
    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<CancelOrderResultDto> HandleAsync(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderAsync(command.OrderId, cancellationToken) ?? 
                    throw new ArgumentNullException($"Order with id {command.OrderId} not found");
        
        var cancellationReason = new CancellationReason((CancellationReasonCode)command.CancellationCode);
        
        order.Cancel(cancellationReason);
        
        await _orderRepository.UpdateOrderAsync(order, cancellationToken);

        return new CancelOrderResultDto(order.Id, order.Status.ToString());
    }
}