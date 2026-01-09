using System.Text.RegularExpressions;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Customers.ValuesObjects;

public sealed class Email : ValueObject
{
    public string Address { get; private set; }

    private static readonly Regex _regex = new(@"^[\w\.-]+@[\w\.-]+\w{2,}$");

    public Email(string address)
    {
        Guard.AgainstNullOrWhiteSpace(address, nameof(address));
        Guard.Against<DomainException>(!_regex.IsMatch(address), "Invalid email address format");

        Address = address.Trim().ToLowerInvariant();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }
}