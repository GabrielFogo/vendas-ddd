using Vendas.Domain.Common.Enums;

namespace Vendas.Application.Commands.OrderCommands.InitiatePayment;

public sealed record InitiatePaymentCommand(Guid OrderId, PaymentMethod PaymentMethod);