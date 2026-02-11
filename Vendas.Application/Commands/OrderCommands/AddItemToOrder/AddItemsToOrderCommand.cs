namespace Vendas.Application.Commands.OrderCommands.AddItemToOrder;

public record AddItemsToOrderCommand(
    Guid OrderId,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);