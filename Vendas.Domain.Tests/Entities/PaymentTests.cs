using System.Reflection;
using FluentAssertions;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Enums.Payments;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Events;
using Vendas.Domain.Orders.Entities;

namespace Vendas.Domain.Tests.Entities;

public class PaymentTests
{
    private static Payment CreateValidPayment()
    {
        return Payment.Create(Guid.NewGuid(), PaymentMethod.BankTransfer, 100);
    }

    [Fact]
    public void Create_ShouldCreatedPayment_WhenDataIsValid()
    {
        var orderId = Guid.NewGuid();
        var paymentMethod = PaymentMethod.BankTransfer;
        var price = 100m;

        var payment = Payment.Create(orderId, paymentMethod, price);

        payment.OrderId.Should().Be(orderId);
        payment.PaymentMethod.Should().Be(paymentMethod);
        payment.Price.Should().Be(price);
        payment.PaymentCode.Should().BeNull();
        payment.Status.Should().Be(PaymentStatus.Pending);
        payment.PaymentDate.Should().BeNull();
    }

    [Theory]
    [InlineData(null, 100, "orderId can't be empty")]
    [InlineData("guid", 0, "price must be greater than 0")]
    public void Create_ShouldNotCreatePayment_WhenDataIsInvalid(string? idType, decimal price, string message)
    {
        var orderId = idType is not null ? Guid.NewGuid() : Guid.Empty;
        var paymentMethod = PaymentMethod.BankTransfer;

        var act = () => Payment.Create(orderId, paymentMethod, price);

        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact]
    public void GeneratePaymentCode_ShouldGeneratePaymentCode_WhenStatusIsPending()
    {
        var payment = CreateValidPayment();

        payment.GenerateLocalPaymentCode();

        payment.PaymentCode.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GeneratePaymentCode_ShouldNotGeneratePaymentCode_WhenStatusIsNotPending()
    {
        var payment = CreateValidPayment();

        var statusProperty = typeof(Payment).GetProperty(
            nameof(Payment.Status),
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        statusProperty!.SetValue(payment, PaymentStatus.Approved);

        var act = () => payment.GenerateLocalPaymentCode();

        act.Should().Throw<DomainException>().WithMessage("status is not pending");
    }

    [Fact]
    public void ApprovePayment_ShouldApprovePayment_WhenStatusIsPending()
    {
        var payment = CreateValidPayment();

        payment.GenerateLocalPaymentCode();
        payment.ApprovePayment();

        payment.Status.Should().Be(PaymentStatus.Approved);
        payment.PaymentDate.Should().NotBeNull();
        payment.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public void ApprovePayment_ShouldAddPaymentApprovedEvent_WhenPaymentIsApproved()
    {
        var payment = CreateValidPayment();

        payment.GenerateLocalPaymentCode();
        payment.ApprovePayment();

        payment.DomainEvents.Count.Should().Be(1);
        payment.DomainEvents.Should().Contain(e => e.GetType() == typeof(PaymentApprovedEvent));
    }

    [Fact]
    public void ApprovePayment_ShouldNotApprovePayment_WhenStatusIsNotPending()
    {
        var payment = CreateValidPayment();

        var statusProperty = typeof(Payment).GetProperty(
            nameof(Payment.Status),
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        statusProperty!.SetValue(payment, PaymentStatus.Approved);

        var act = () => payment.ApprovePayment();

        act.Should().Throw<DomainException>().WithMessage("payment is not pending");
    }

    [Fact]
    public void RejectPayment_ShouldRejectPayment_WhenStatusIsPending()
    {
        var payment = CreateValidPayment();

        payment.GenerateLocalPaymentCode();
        payment.RejectPayment();

        payment.Status.Should().Be(PaymentStatus.Rejected);
        payment.PaymentDate.Should().NotBeNull();
        payment.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public void RejectPayment_ShouldAddPaymentRejectedEvent_WhenPaymentIsRejected()
    {
        var payment = CreateValidPayment();

        payment.GenerateLocalPaymentCode();
        payment.RejectPayment();

        payment.DomainEvents.Count.Should().Be(1);
        payment.DomainEvents.Should().Contain(e => e.GetType() == typeof(PaymentRejectEvent));
    }

    [Fact]
    public void RejectPayment_ShouldNotRejectPayment_WhenStatusIsNotPending()
    {
        var payment = CreateValidPayment();

        var statusProperty = typeof(Payment).GetProperty(
            nameof(Payment.Status),
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        statusProperty!.SetValue(payment, PaymentStatus.Approved);

        var act = () => payment.RejectPayment();

        act.Should().Throw<DomainException>().WithMessage("payment is not pending");
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenIdAreTheSame()
    {
        var firstItem = CreateValidPayment();
        var secondItem = CreateValidPayment();

        typeof(Entity).GetProperty(nameof(firstItem.Id))!.SetValue(secondItem, firstItem.Id);

        (firstItem == secondItem).Should().BeTrue();
        firstItem.Equals(secondItem).Should().BeTrue();
    }
}