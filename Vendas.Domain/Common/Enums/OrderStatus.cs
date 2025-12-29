namespace Vendas.Domain.Common.Enums.Orders;

public enum OrderStatus
{
    Pending = 1,
    PaymentConfirmed = 2,
    InSeparation = 3,
    Sent = 4,
    Delivered = 5,
    Cancelled = 6,
}