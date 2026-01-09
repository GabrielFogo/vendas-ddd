using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Catalog.ValueObjects;

public sealed class ProductImage: ValueObject
{
    public string  Url { get; }
    public int Order { get; }
    
    public ProductImage(string url, int order)
    {
        Guard.AgainstNullOrWhiteSpace(url, nameof(url));
        Guard.Against<DomainException>(order < 1, "Order must be greater than zero.");
        
        Url = url;
        Order = order;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
        yield return Order;
    }
}