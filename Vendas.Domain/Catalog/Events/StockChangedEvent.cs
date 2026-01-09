using Vendas.Domain.Events;

namespace Vendas.Domain.Catalog.Events;

public sealed record StockChangedEvent(Guid ProductId, int Quantity, string Reason) : DomainEventBase;