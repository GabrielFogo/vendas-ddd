using FluentAssertions;
using Vendas.Domain.Catalog.Entities;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Catalog.Entities;

public class CategoryTests
{
    private Category CreateValidCategory(bool active)
    {
        const string name = "name";
        const string description = "description";
        var category = new Category(name, description);

        if (!active)
            category.Deactivate();

        return category;
    }

    [Fact]
    public void Create_ShouldCreateCategory_WhenDataIsValid()
    {
        const string name = "name";
        const string description = "description";
        var category = new Category(name, description);

        category.Should().NotBeNull();
        category.Name.Should().Be(name);
        category.Description.Should().Be(description);
        category.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "", "name can't be null or empty")]
    [InlineData("ca", "", "Category name must be at least 3 characters long.")]
    public void Create_ShouldNotCreateCategory_WhenDataIsInvalid(
        string name,
        string description,
        string message)
    {
        var act = () => new Category(name, description);

        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact]
    public void Activate_ShouldActivateCategory_WhenDataIsValid()
    {
        var category = CreateValidCategory(false);

        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Activate_ShouldNotActivateCategory_WhenCategoryIsActive()
    {
        var category = CreateValidCategory(true);

        var act = () => category.Activate();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Deactivate_ShouldDeactivateCategory_WhenDataIsValid()
    {
        var category = CreateValidCategory(true);

        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_ShouldNotActivateCategory_WhenCategoryIsDeactivate()
    {
        var category = CreateValidCategory(false);

        var act = () => category.Deactivate();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void ChangeName_ShouldChangeName_WhenDataIsValid()
    {
        var category = CreateValidCategory(true);

        category.ChangeName("name");
        category.Name.Should().Be("name");
    }

    [Fact]
    public void ChangeDescription_ShouldChangeDescription_WhenDataIsValid()
    {
        var category = CreateValidCategory(true);
        category.ChangeDescription("description");
        category.Description.Should().Be("description");
    }
}