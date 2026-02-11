using FluentAssertions;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Orders.Entities;

namespace Vendas.Domain.Tests.Entities;

public class OrderItemTests
{
    private static OrderItem CreateValidaItem(decimal price = 100m, int quantity = 1)
    {
        return new OrderItem(Guid.NewGuid(), "name", price, quantity);
    }

    [Fact]
    public void Create_ShouldCreateOrderItem_WhenDataIsValid()
    {
        var productId = Guid.NewGuid();
        var productName = Guid.NewGuid().ToString();
        var unitPrice = 100m;
        var quantity = 1;

        var item = new OrderItem(productId, productName, unitPrice, quantity);

        item.ProductId.Should().Be(productId);
        item.ProductName.Should().Be(productName);
        item.UnitPrice.Should().Be(unitPrice);
        item.Quantity.Should().Be(quantity);
        item.Discount.Should().Be(0);
        item.TotalPrice.Should().Be(100m);
    }

    [Theory]
    [InlineData("", "name", 10, 1, "productId can't be empty")]
    [InlineData("guid", "", 10, 1, "productName can't be null or empty")]
    [InlineData("guid", "name", 0, 1, "unit price must be positive")]
    [InlineData("guid", "name", 10, 0, "quantity must be positive")]
    public void Create_ShouldNotCreateOrderItem_WhenDataIsInvalid(
        string idType,
        string productName,
        decimal unitPrice,
        int quantity,
        string message)
    {
        var productId = idType == "guid" ? Guid.NewGuid() : Guid.Empty;
        var act = () => new OrderItem(productId, productName, unitPrice, quantity);

        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact]
    public void ApplyDiscount_ShouldCalculateDiscount_WhenDataIsValid()
    {
        var item = CreateValidaItem(200m, 2);

        item.ApplyDiscount(100m);

        item.Discount.Should().Be(100m);
        item.TotalPrice.Should().Be(300m);
        item.ModifiedAt.Should().NotBe(null);
    }

    [Theory]
    [InlineData(0, "discount must be positive")]
    [InlineData(401, "discount can not be greater than total price")]
    public void CalculateDiscount_ShouldNotCalculateDiscount_WhenDataIsInvalid(decimal discount, string message)
    {
        var item = CreateValidaItem(200m, 2);
        var act = () => item.ApplyDiscount(discount);

        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact]
    public void IncreaseQuantity_ShouldIncreaseQuantity_WhenDataIsValid()
    {
        var item = CreateValidaItem(200m, 2);

        item.IncreaseQuantity(1);

        item.Quantity.Should().Be(3);
        item.TotalPrice.Should().Be(600m);
        item.ModifiedAt.Should().NotBe(null);
    }

    [Fact]
    public void IncreaseQuantity_ShouldNotIncreaseQuantity_WhenDataIsInvalid()
    {
        var item = CreateValidaItem();
        var act = () => item.IncreaseQuantity(0);
        var exceptionMessage = "quantity must be positive";

        act.Should().Throw<DomainException>(exceptionMessage);
    }

    [Fact]
    public void DecreaseQuantity_ShouldDecreaseQuantity_WhenDataIsValid()
    {
        var item = CreateValidaItem(200m, 2);

        item.DecreaseQuantity(1);

        item.Quantity.Should().Be(1);
        item.TotalPrice.Should().Be(200m);
        item.ModifiedAt.Should().NotBe(null);
    }

    [Fact]
    public void DecreaseQuantity_ShouldNotDecreaseQuantity_WhenDataIsInvalid()
    {
        var item = CreateValidaItem();
        var act = () => item.DecreaseQuantity(0);
        var exceptionMessage = "quantity must be positive";

        act.Should().Throw<DomainException>(exceptionMessage);
    }

    [Fact]
    public void UpdateUnitPrice_ShouldUpdateUnitPrice_WhenDataIsValid()
    {
        var item = CreateValidaItem(200m, 2);

        item.UpdateUnitPrice(400m);

        item.UnitPrice.Should().Be(400m);
        item.TotalPrice.Should().Be(800m);
        item.ModifiedAt.Should().NotBe(null);
    }

    [Fact]
    public void UpdateUnitPrice_ShouldNotUpdateUnitPrice_WhenDataIsInvalid()
    {
        var item = CreateValidaItem();
        var act = () => item.UpdateUnitPrice(0);
        var exceptionMessage = "unit price must be positive";

        act.Should().Throw<DomainException>(exceptionMessage);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenIdAreTheSame()
    {
        var firstItem = CreateValidaItem();
        var secondItem = CreateValidaItem();

        typeof(Entity).GetProperty(nameof(firstItem.Id))!.SetValue(secondItem, firstItem.Id);

        (firstItem == secondItem).Should().BeTrue();
        firstItem.Equals(secondItem).Should().BeTrue();
    }
}