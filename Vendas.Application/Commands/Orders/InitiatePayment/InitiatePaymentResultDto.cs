using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Enums.Payments;

namespace Vendas.Application.Commands.Orders.InitiatePayment;

public sealed record InitiatePaymentResultDto(
    Guid OrderId,
    Guid PaymentId,
    string OrderStatus,
    string PaymentStatus);