using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Catalog.ValueObjects;

public sealed class ProductCode: ValueObject
{
    public string Value { get; }
    
    public ProductCode(string value)
    {
        Guard.AgainstNullOrWhiteSpace(nameof(value), value);
        Guard.Against<DomainException>(value.Length < 3, "Code is too short");
        
        Value = value.Trim().ToUpper();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}