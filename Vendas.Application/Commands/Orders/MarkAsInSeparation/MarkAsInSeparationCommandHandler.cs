using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.MarkAsInSeparation;

public class MarkAsInSeparationCommandHandler(IOrderRepository orderRepository)
    : ICommandHandler<MarkAsInSeparationCommand, MarkAsInSeparationResultDto>
{
    public async Task<MarkAsInSeparationResultDto> HandleAsync(MarkAsInSeparationCommand command,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderAsync(command.OrderId, cancellationToken) ??
                    throw new ArgumentNullException($"Order with id {command.OrderId} not found");

        order.MarkAsInSeparation();

        await orderRepository.UpdateOrderAsync(order, cancellationToken);

        return new MarkAsInSeparationResultDto();
    }
}