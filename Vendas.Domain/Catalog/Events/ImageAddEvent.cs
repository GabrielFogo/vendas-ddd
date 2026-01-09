using Vendas.Domain.Events;

namespace Vendas.Domain.Catalog.Events;

public sealed record ImageAddEvent(Guid ProductId, string Url, int Order): DomainEventBase;