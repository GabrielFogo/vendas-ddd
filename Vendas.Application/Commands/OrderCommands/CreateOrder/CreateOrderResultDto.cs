namespace Vendas.Application.Commands.OrderCommands.CreateOrder;

public sealed record CreateOrderResultDto(
    Guid Id,
    string Code,
    DateTime CreatedAt,
    decimal TotalPrice,
    string Status);