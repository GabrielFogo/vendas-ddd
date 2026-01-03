using Vendas.Domain.ValueObjects;

namespace Vendas.Domain.Events;

public record OrderSentEvent(
    Guid OrderId,
    Guid CustomerId,
    DeliveryAddress DeliveryAddress): DomainEventBase;