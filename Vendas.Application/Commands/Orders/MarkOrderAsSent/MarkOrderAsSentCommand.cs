namespace Vendas.Application.Commands.Orders.MarkOrderAsSent;

public sealed record MarkOrderAsSentCommand(Guid OrderId);