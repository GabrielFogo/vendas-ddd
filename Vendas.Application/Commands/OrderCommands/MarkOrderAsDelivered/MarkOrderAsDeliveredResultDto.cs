namespace Vendas.Application.Commands.OrderCommands.MarkOrderAsDelivered;

public record MarkOrderAsDeliveredResultDto(Guid OrderId, string Status);