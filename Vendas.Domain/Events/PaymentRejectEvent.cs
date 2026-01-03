namespace Vendas.Domain.Events;

public record PaymentRejectEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Price,
    DateTime PaymentDate,
    string? PaymentCode
    ): DomainEventBase;