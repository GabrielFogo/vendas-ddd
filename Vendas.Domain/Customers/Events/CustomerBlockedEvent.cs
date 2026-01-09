using Vendas.Domain.Events;

namespace Vendas.Domain.Customers.Events;

public sealed record CustomerBlockedEvent(Guid CustomerId, string Cpf) : DomainEventBase;