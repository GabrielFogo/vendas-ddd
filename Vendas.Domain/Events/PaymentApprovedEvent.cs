namespace Vendas.Domain.Events;

public record PaymentApprovedEvent (
    Guid PaymentId,
    Guid OrderId,
    decimal Price,
    DateTime PaymentDate,
    string? PaymentCode) : DomainEventBase;