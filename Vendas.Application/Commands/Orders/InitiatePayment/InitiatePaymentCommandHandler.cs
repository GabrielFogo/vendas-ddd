using Vendas.Application.Abstractions.Commands;
using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Orders.InitiatePayment;

public class InitiatePaymentCommandHandler : ICommandHandler<InitiatePaymentCommand, InitiatePaymentResultDto>
{
    private readonly IOrderRepository _orderRepository;
    
    public InitiatePaymentCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<InitiatePaymentResultDto> HandleAsync(InitiatePaymentCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderAsync(command.OrderId, cancellationToken);
        
        if(order is null) throw new InvalidOperationException($"Order with id {command.OrderId} not found");
        
        var payment = order.InitiatePayment(command.PaymentMethod);
        
        await _orderRepository.UpdateOrderAsync(order, cancellationToken);

        return new InitiatePaymentResultDto(order.Id, payment.Id, order.Status.ToString(), payment.Status.ToString());
    }
}