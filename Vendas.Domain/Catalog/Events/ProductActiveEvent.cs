using Vendas.Domain.Events;

namespace Vendas.Domain.Catalog.Events;

public sealed record ProductActiveEvent(Guid ProductId) : DomainEventBase;