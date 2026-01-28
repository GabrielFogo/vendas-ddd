namespace Vendas.Application.Commands.Orders.MarkOrderAsDelivered;

public record MarkOrderAsDeliveredResultDto(Guid OrderId, string Status);