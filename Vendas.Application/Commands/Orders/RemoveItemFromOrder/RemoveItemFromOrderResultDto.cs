namespace Vendas.Application.Commands.Orders.RemoveItemFromOrder;

public record RemoveItemFromOrderResultDto(
    Guid Id,
    decimal TotalPrice,
    string Status);