using FluentAssertions;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.ValueObjects;

namespace Vendas.Domain.Tests.ValueObjects;

public class DeliveryAddressTests
{
    [Fact]
    public void Create_ShouldReturnDeliveryAddress_WhenDataIsValid()
    {
        var cep = "12345-678";
        var street = "123 Main St";
        var neighborhood = "123 Main Street";
        var city = "Berlin";
        var state = "TX";
        var country = "USA";
        
        var address = DeliveryAddress.Create(
            cep,
            street,
            neighborhood,
            city,
            state,
            country);
        
        address.Should().NotBeNull();
        address.Cep.Should().Be(cep);
        address.Street.Should().Be(street);
        address.Neighborhood.Should().Be(neighborhood);
        address.City.Should().Be(city);
        address.State.Should().Be(state);
        address.Country.Should().Be(country);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("12-345678")]
    [InlineData("a2345-678")]
    public static void Create_ShouldReturnDeliveryAddress_WhenCepIsValid(string cep)
    {
        var street = "123 Main St";
        var neighborhood = "123 Main Street";
        var city = "Berlin";
        var state = "TX";
        var country = "USA";
        
        var act = () => DeliveryAddress.Create(cep, street, neighborhood, city, state, country);
        
        act
            .Should()
            .Throw<DomainException>()
            .WithMessage("Invalid CEP");;
    }

    [Fact]
    public void AddressShouldBeEqual_WhenHasTheSameValues()
    {
        var cep = "12345-678";
        var street = "123 Main St";
        var neighborhood = "123 Main Street";
        var city = "Berlin";
        var state = "TX";
        var country = "USA";
        
        var firstAddress = DeliveryAddress.Create(
            cep,
            street,
            neighborhood,
            city,
            state,
            country);

        var secondAddress = DeliveryAddress.Create(
            cep,
            street,
            neighborhood,
            city,
            state,
            country);
        
        firstAddress.Equals(secondAddress).Should().BeTrue();
        (firstAddress == secondAddress).Should().BeTrue();
    }

    [Fact]
    public void AddressShouldBeNotEquals_WhenHasTheDifferentValues()
    {
        var cep = "12345-678";
        var street = "123 Main St";
        var neighborhood = "123 Main Street";
        var city = "Berlin";
        var state = "TX";
        var country = "USA";

        var firstAddress = DeliveryAddress.Create(
            cep,
            street,
            neighborhood,
            city,
            state,
            country);

        var secondAddress = DeliveryAddress.Create(
            cep,
            street,
            neighborhood,
            city,
            state,
            "a");

        firstAddress.Should().NotBe(secondAddress);
        (firstAddress != secondAddress).Should().Be(true);
    }
}