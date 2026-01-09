using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Enums.Payments;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;
using Vendas.Domain.Events;
using Vendas.Domain.ValueObjects;

namespace Vendas.Domain.Orders.Entities;

public sealed class Order : AggregateRoot
{
    public Guid CustomerId { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
    public decimal TotalPrice { get; private set; }
    public OrderStatus Status { get; private set; }
    public string Code { get; private set; } = string.Empty;

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private readonly List<Payment> _payments = [];
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    private Order(Guid customerId, DeliveryAddress deliveryAddress)
    {
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstNull(deliveryAddress, nameof(deliveryAddress));

        CustomerId = customerId;
        DeliveryAddress = deliveryAddress;
        Status = OrderStatus.Pending;
        TotalPrice = 0;

        GenerateCode();
    }

    private void GenerateCode()
    {
        Code = "ORDER-" + Guid.NewGuid().ToString()[..8].ToUpper();
    }


    private void CalculateTotalPrice()
    {
        TotalPrice = _items.Sum(i => i.TotalPrice);
    }

    public static Order Create(Guid customerId, DeliveryAddress deliveryAddress)
    {
        return new Order(customerId, deliveryAddress);
    }

    public void AddItem(Guid productId, string productName, decimal price, int quantity)
    {
        Guard.Against<DomainException>(
            Status != OrderStatus.Pending,
            "Cannot add an order to a pending order."
        );

        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
            _items.Add(new OrderItem(productId, productName, price, quantity));
        else
            item.IncreaseQuantity(quantity);

        CalculateTotalPrice();
        SetModifiedAt();
    }

    public void RemoveItem(Guid itemId)
    {
        Guard.AgainstEmptyGuid(itemId, nameof(itemId));
        Guard.Against<DomainException>(Status != OrderStatus.Pending, "Cannot remove an order to a pending order.");
        Guard.Against<DomainException>(_items.Count == 1, "Order must have at least one item.");

        var item = _items.FirstOrDefault(i => i.Id == itemId);

        Guard.AgainstNull(item, "Item does not exist.");

        _items.Remove(item!);

        CalculateTotalPrice();
        SetModifiedAt();
    }

    public Payment InitiatePayment(PaymentMethod method)
    {
        Guard.Against<DomainException>(
            _items.Count == 0,
            "Cannot initiate a payment in an order with no items");
        Guard.Against<DomainException>(
            Status != OrderStatus.Pending,
            "Payment can be initialize only if order is pending");

        Guard.Against<DomainException>(
            _payments.Any(p => p.Status == PaymentStatus.Pending),
            "Order already has a pending payment");

        var newPayment = Payment.Create(Id, method, TotalPrice);

        _payments.Add(newPayment);

        SetModifiedAt();

        return newPayment;
    }

    public void HandlePaymentApproved(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == paymentId);

        if (payment is null) return;

        Guard.Against<DomainException>(Status != OrderStatus.Pending, "Order must be pending");

        Status = OrderStatus.PaymentConfirmed;

        SetModifiedAt();
    }

    public void HandlePaymentRejected(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == paymentId);

        if (payment is null) return;

        Guard.Against<DomainException>(Status != OrderStatus.Pending, "Order must be pending");

        Status = OrderStatus.Cancelled;

        SetModifiedAt();

        AddDomainEvent(new OrderCancelledEvent(
            Id,
            CustomerId,
            paymentId,
            Status,
            CancellationReason.PaymentError())
        );
    }

    public void SetAsInSeparation()
    {
        Guard.Against<DomainException>(
            Status != OrderStatus.PaymentConfirmed,
            "Order can only be set as in separation if payment is confirmed");

        Status = OrderStatus.InSeparation;

        SetModifiedAt();
    }

    public void SetAsSent()
    {
        Guard.Against<DomainException>(
            Status != OrderStatus.InSeparation,
            "Order can only be set as sent if order is in separation");

        Status = OrderStatus.Sent;

        SetModifiedAt();
        AddDomainEvent(new OrderSentEvent(Id, CustomerId, DeliveryAddress));
    }

    public void SetAsDelivered()
    {
        Guard.Against<DomainException>(
            Status != OrderStatus.Sent,
            "Order can only be set as delivered if order was sent");

        Status = OrderStatus.Delivered;

        SetModifiedAt();
        AddDomainEvent(new OrderDeliveredEvent(Id, CustomerId));
    }

    public void Cancel(CancellationReason reason)
    {
        Guard.Against<DomainException>(
            Status != OrderStatus.InSeparation,
            "Order can only be set as cancelled if order is in separation");

        Status = OrderStatus.Cancelled;

        SetModifiedAt();
        AddDomainEvent(new OrderCancelledEvent(
            Id,
            CustomerId,
            _payments.LastOrDefault()!.Id,
            Status,
            reason)
        );
    }
}