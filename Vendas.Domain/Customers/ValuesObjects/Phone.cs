using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Customers.ValuesObjects;

public sealed class Phone : ValueObject
{
    public string Number { get; private set; }

    public Phone(string number)
    {
        Guard.AgainstNullOrWhiteSpace(number, nameof(number));

        var digits = new string(number.Where(char.IsDigit).ToArray());

        Guard.Against<DomainException>(digits.Length is < 10 or > 11, "Invalid phone number");

        Number = digits;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}