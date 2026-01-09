using Vendas.Domain.Events;

namespace Vendas.Domain.Customers.Events;

public sealed record MainAddressChangedEvent(Guid CustomerId, Guid NewMainAddressId) : DomainEventBase;