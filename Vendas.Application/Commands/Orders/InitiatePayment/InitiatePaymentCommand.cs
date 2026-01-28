using Vendas.Domain.Common.Enums;

namespace Vendas.Application.Commands.Orders.InitiatePayment;

public sealed record InitiatePaymentCommand (Guid OrderId, PaymentMethod PaymentMethod);