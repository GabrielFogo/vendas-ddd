namespace Vendas.Application.Commands.Orders.AddItemToOrder;

public sealed record AddItemToOrderResultDto(
    Guid Id,
    decimal TotalPrice,
    string Status);