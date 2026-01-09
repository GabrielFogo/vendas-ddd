using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Enums.Payments;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;
using Vendas.Domain.Events;

namespace Vendas.Domain.Orders.Entities;

public sealed class Payment : Entity
{
    public Guid OrderId { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public decimal Price { get; private set; }
    public DateTime? PaymentDate { get; private set; }
    public string? PaymentCode { get; private set; }

    private Payment(Guid orderId, PaymentMethod paymentMethod, decimal price)
    {
        Guard.AgainstEmptyGuid(orderId, nameof(orderId));
        Guard.Against<DomainException>(price <= 0, "price must be greater than 0");
        Guard.Against<DomainException>(!Enum.IsDefined(paymentMethod), "payment method is not defined");

        OrderId = orderId;
        PaymentMethod = paymentMethod;
        Price = price;
        Status = PaymentStatus.Pending;
        PaymentDate = null;
        PaymentCode = null;
    }

    public static Payment Create(Guid orderId, PaymentMethod paymentMethod, decimal price)
    {
        return new Payment(orderId, paymentMethod, price);
    }

    public void GenerateLocalPaymentCode()
    {
        if (PaymentCode is not null) return;

        var code = "LOCAL-" + Guid.NewGuid().ToString()[..8].ToUpper();

        SetPaymentCode(code);
    }

    public void SetPaymentCode(string code)
    {
        Guard.Against<DomainException>(Status != PaymentStatus.Pending, "status is not pending");

        PaymentCode = code;

        SetModifiedAt();
    }

    public void ApprovePayment()
    {
        Guard.Against<DomainException>(Status != PaymentStatus.Pending, "payment is not pending");
        Guard.AgainstNull(PaymentCode, nameof(PaymentCode));

        Status = PaymentStatus.Approved;
        PaymentDate = DateTime.Now;

        SetModifiedAt();
        AddDomainEvent(new PaymentApprovedEvent(
            Id,
            OrderId,
            Price,
            PaymentDate.Value,
            PaymentCode)
        );
    }

    public void RejectPayment()
    {
        Guard.Against<DomainException>(Status != PaymentStatus.Pending, "payment is not pending");
        Guard.AgainstNull(PaymentCode, nameof(PaymentCode));

        Status = PaymentStatus.Rejected;
        PaymentDate = DateTime.Now;

        SetModifiedAt();
        AddDomainEvent(new PaymentRejectEvent(
            Id,
            OrderId,
            Price,
            PaymentDate.Value,
            PaymentCode)
        );
    }
}