namespace Vendas.Application.Commands.Orders.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId, int CancellationCode);