namespace Vendas.Application.Commands.OrderCommands.InitiatePayment;

public sealed record InitiatePaymentResultDto(
    Guid OrderId,
    Guid PaymentId,
    string OrderStatus,
    string PaymentStatus);