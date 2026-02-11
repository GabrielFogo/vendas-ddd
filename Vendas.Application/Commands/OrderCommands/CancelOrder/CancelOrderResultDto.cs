namespace Vendas.Application.Commands.OrderCommands.CancelOrder;

public record CancelOrderResultDto(Guid OrderId, string Status);