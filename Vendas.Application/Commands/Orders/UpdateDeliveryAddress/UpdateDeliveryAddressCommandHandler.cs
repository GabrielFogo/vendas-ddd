using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.UpdateDeliveryAddress;

public sealed class UpdateDeliveryAddressCommandHandler 
    : ICommandHandler<UpdateDeliveryCommand,UpdateDeliveryAddressResultDto>
{
    private readonly IOrderRepository _orderRepository;
    
    public UpdateDeliveryAddressCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<UpdateDeliveryAddressResultDto> HandleAsync(UpdateDeliveryCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderAsync(command.OrderId, cancellationToken);
        
        if(order is null) throw new InvalidOperationException($"Order with id {command.OrderId} not found");

        order.UpdateDeliveryAddress(command.DeliveryAddress);
        
        await  _orderRepository.UpdateOrderAsync(order, cancellationToken);
        
        return new UpdateDeliveryAddressResultDto(
            order.Id,
            order.DeliveryAddress.ToString(),
            order.Status.ToString());
    }
}