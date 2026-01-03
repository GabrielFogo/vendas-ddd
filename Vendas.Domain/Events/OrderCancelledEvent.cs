using Vendas.Domain.Common.Enums;
using Vendas.Domain.ValueObjects;

namespace Vendas.Domain.Events;

public record OrderCancelledEvent(
    Guid OrderId,
    Guid CustomerId,
    Guid PaymentId,
    OrderStatus PreviusStatus,
    CancellationReason CancellationReason
    ):  DomainEventBase;