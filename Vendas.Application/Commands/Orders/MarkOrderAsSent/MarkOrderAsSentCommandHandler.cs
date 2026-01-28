using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.MarkOrderAsSent;

public sealed class MarkOrderAsSentCommandHandler : ICommandHandler<MarkOrderAsSentCommand, MarkOrderAsSentResultDto>
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsSentCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<MarkOrderAsSentResultDto> HandleAsync(MarkOrderAsSentCommand command,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderAsync(command.OrderId, cancellationToken) ?? 
                    throw new ArgumentNullException($"Order with id {command.OrderId} not found");
        
        order.MarkAsSent();

        await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        
        return new MarkOrderAsSentResultDto(order.Id, order.Status.ToString());
    }
}