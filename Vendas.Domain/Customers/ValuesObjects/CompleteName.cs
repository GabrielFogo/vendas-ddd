using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Customers.ValuesObjects;

public class CompleteName : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string NameFormated { get; private set; }

    public CompleteName(string firstName, string lastName)
    {
        Guard.AgainstNullOrWhiteSpace(firstName, nameof(firstName));
        Guard.AgainstNullOrWhiteSpace(lastName, nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        NameFormated = (firstName + " " + lastName).Trim().ToLowerInvariant();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NameFormated;
    }
}