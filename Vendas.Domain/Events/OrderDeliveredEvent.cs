namespace Vendas.Domain.Events;

public record OrderDeliveredEvent(
    Guid OrderId,
    Guid CustomerId): DomainEventBase;