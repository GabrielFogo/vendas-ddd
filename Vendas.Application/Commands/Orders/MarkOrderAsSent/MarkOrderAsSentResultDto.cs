namespace Vendas.Application.Commands.Orders.MarkOrderAsSent;

public sealed record MarkOrderAsSentResultDto(Guid OrderId, string Status);