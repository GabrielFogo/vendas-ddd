using Vendas.Domain.Events;

namespace Vendas.Domain.Catalog.Events;

public sealed record CategoryActiveEvent(Guid CategoryId) : DomainEventBase;    