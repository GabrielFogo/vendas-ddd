namespace Vendas.Application.Commands.Orders.CancelOrder;

public record CancelOrderResultDto(Guid OrderId, string Status);