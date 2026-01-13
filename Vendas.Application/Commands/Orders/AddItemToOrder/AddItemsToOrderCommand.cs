namespace Vendas.Application.Commands.Orders.AddItemToOrder;

public record AddItemsToOrderCommand(
    Guid OrderId,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);