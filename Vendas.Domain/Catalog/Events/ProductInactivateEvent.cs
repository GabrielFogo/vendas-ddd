using Vendas.Domain.Events;

namespace Vendas.Domain.Catalog.Events;

public sealed record ProductInactivateEvent(Guid ProductId): DomainEventBase;