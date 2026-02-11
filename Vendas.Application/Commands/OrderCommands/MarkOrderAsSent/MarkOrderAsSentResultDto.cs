namespace Vendas.Application.Commands.OrderCommands.MarkOrderAsSent;

public sealed record MarkOrderAsSentResultDto(Guid OrderId, string Status);