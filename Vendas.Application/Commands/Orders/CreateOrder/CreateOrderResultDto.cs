namespace Vendas.Application.Commands.Orders.CreateOrder;

public sealed record CreateOrderResultDto(
    Guid Id,
    string Code,
    DateTime CreatedAt,
    decimal TotalPrice,
    string Status);