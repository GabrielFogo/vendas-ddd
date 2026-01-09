using Vendas.Domain.Catalog.Enums;
using Vendas.Domain.Catalog.Events;
using Vendas.Domain.Catalog.ValueObjects;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Catalog.Entities;

public sealed class Product : Entity
{
    public ProductName Name { get; private set; }
    public ProductCode Code { get; private set; }
    public ProductPrice Price { get; private set; }
    public string? Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public ProductStatus Status { get; private set; }
    public int Stock { get; private set; }

    private readonly List<ProductImage> _images = [];
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    private Product(
        ProductName name,
        ProductCode code,
        ProductPrice price,
        Guid categoryId,
        int initialStock = 0,
        string? description = null)
    {
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(code, nameof(code));
        Guard.AgainstNull(price, nameof(price));
        Guard.AgainstEmptyGuid(categoryId, nameof(categoryId));
        Guard.Against<DomainException>(initialStock < 0, "Initial stock must be positive");

        Name = name;
        Code = code;
        Price = price;
        CategoryId = categoryId;
        Stock = initialStock;
        Description = description?.Trim();
        Status = ProductStatus.Active;
    }

    public static Product Create(
        ProductName name,
        ProductCode code,
        ProductPrice price,
        Guid categoryId,
        int initialStock = 0,
        string? description = null)
    {
        return new Product(name, code, price, categoryId, initialStock, description);
    }

    public void ChangeName(ProductName newName)
    {
        Guard.AgainstNull(newName, nameof(newName));
        
        Name = newName;
        
        SetModifiedAt();
    }

    public void ChangePrice(ProductPrice price)
    {
        Guard.AgainstNull(price, nameof(price));

        var oldPrice = Price.Value;
        
        Price = price;
        
        SetModifiedAt();
        AddDomainEvent(new ProductPriceChangedEvent(Id, oldPrice, price.Value));
    }

    public void ChangeCategoryId(Guid newCategoryId)
    {
        Guard.AgainstEmptyGuid(newCategoryId, nameof(newCategoryId));
        
        CategoryId = newCategoryId;
        
        SetModifiedAt();
    }

    public void ChangeDescription(string? description)
    {
        Description = description?.Trim();
        
        SetModifiedAt();
    }

    public void AdjustStock(int quantity, string reason)
    {
        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));
        Guard.Against<DomainException>(Stock + quantity < 0, "Stock must be positive");
        
        Stock += quantity;
        
        SetModifiedAt();
        AddDomainEvent(new StockChangedEvent(Id, quantity, reason));
    }
    
    public void Activate()
    {
        Guard.Against<DomainException>(Status == ProductStatus.Active, "Product status must be Inactive");
        
        Status = ProductStatus.Active;
        
        SetModifiedAt();
        AddDomainEvent(new ProductActiveEvent(Id));
    }

    public void Inactivate()
    {
        Guard.Against<DomainException>(Status == ProductStatus.Inactive, "Product status must be Active");
        
        Status = ProductStatus.Inactive;
        
        SetModifiedAt();
        AddDomainEvent(new ProductInactivateEvent(Id));
    }

    public void AddImage(ProductImage image)
    {
        Guard.AgainstNull(image, nameof(image));
        Guard.Against<DomainException>(_images.Any(i => i.Order == image.Order), 
            "Image order must be unique");
        
        _images.Add(image);
        
        SetModifiedAt();
        AddDomainEvent(new ImageAddEvent(Id, image.Url, image.Order));
    }
}