namespace Vendas.Application.Commands.OrderCommands.RemoveItemFromOrder;

public record RemoveItemFromOrderCommand(Guid OrderId, Guid ItemId);