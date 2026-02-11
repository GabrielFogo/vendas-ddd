namespace Vendas.Application.Commands.OrderCommands.AddItemToOrder;

public sealed record AddItemToOrderResultDto(
    Guid Id,
    decimal TotalPrice,
    string Status);