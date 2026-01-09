using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Catalog.ValueObjects;

public class ProductPrice: ValueObject
{
    public decimal Value { get; }

    public ProductPrice(decimal value)
    {
        Guard.Against<DomainException>(value <= 0, "Price must be greater than 0");
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}