namespace Vendas.Domain.Events;

public abstract record DomainEventBase : IDomainEvent
{
    public DateTime DataOccurred { get; protected set; } = DateTime.UtcNow;
}