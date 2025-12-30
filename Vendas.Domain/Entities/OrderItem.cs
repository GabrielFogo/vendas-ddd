using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Entities;

public sealed class OrderItem : Entity
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalPrice { get; private set; }
    
    public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.AgainstNullOrWhiteSpace(productName, nameof(productName));
        Guard.Against<DomainException>(unitPrice <= 0, "unit price must be positive");
        Guard.Against<DomainException>(quantity <= 0, "quantity must be positive");
        
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        
        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = (UnitPrice * Quantity) - Discount;
    }

    public void ApplyDiscount(decimal discount)
    {
        Guard.Against<DomainException>(discount <= 0, "discount must be positive");
        Guard.Against<DomainException>(discount > UnitPrice * Quantity, "discount can not be greater than total price");
        
        Discount = discount;
        
        SetModifiedAt();
        CalculateTotalPrice();
    }

    public void IncreaseQuantity(int quantity)
    {
        Guard.Against<DomainException>(quantity <= 0, "quantity must be positive");
        
        Quantity += quantity;
        
        SetModifiedAt();
        CalculateTotalPrice();
    }

    public void DecreaseQuantity(int quantity)
    {
        Guard.Against<DomainException>(quantity <= 0, "quantity must be positive");
        Guard.Against<DomainException>(quantity > Quantity, "it is not possible remove more than quantity");
        
        Quantity -= quantity;
        
        Guard.Against<DomainException>(Quantity == 0, "an item can not has quantity equal to zero");
        
        SetModifiedAt();
        CalculateTotalPrice();
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        Guard.Against<DomainException>(unitPrice <= 0, "unit price must be positive");
        
        UnitPrice = unitPrice;
        
        SetModifiedAt();
        CalculateTotalPrice();
    }
}