namespace Vendas.Application.Commands.OrderCommands.RemoveItemFromOrder;

public sealed record RemoveItemFromOrderResultDto(
    Guid Id,
    decimal TotalPrice,
    string Status);