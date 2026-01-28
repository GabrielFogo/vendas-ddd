namespace Vendas.Application.Commands.Orders.RemoveItemFromOrder;

public record RemoveItemFromOrderCommand(Guid OrderId, Guid ItemId);