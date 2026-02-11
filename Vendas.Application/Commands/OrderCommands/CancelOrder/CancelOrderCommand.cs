namespace Vendas.Application.Commands.OrderCommands.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId, int CancellationCode);