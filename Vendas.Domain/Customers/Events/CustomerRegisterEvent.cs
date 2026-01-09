using Vendas.Domain.Events;

namespace Vendas.Domain.Customers.Events;

public sealed record CustomerRegisterEvent(
    Guid CustomerId,
    string Nome,
    string Cpf,
    string Email): DomainEventBase;