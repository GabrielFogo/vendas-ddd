using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Catalog.ValueObjects;

public sealed class ProductName: ValueObject
{
    public string Value { get; }

    public ProductName(string value)
    {
        Guard.AgainstNullOrWhiteSpace(nameof(value), value);
        Guard.Against<DomainException>(value.Length > 150, "Product name is too long");
        
        Value = value.Trim();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}