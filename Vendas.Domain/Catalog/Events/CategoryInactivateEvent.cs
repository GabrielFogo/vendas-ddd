using Vendas.Domain.Events;

namespace Vendas.Domain.Catalog.Events;

public sealed record CategoryInactivateEvent(Guid CategoryId): DomainEventBase;