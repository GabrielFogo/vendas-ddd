using System.Text.RegularExpressions;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.ValueObjects;

public class DeliveryAddress : ValueObject
{
    private DeliveryAddress(
        string cep,
        string street,
        string neighborhood,
        string city,
        string state,
        string country)
    {
        Guard.AgainstNullOrWhiteSpace(cep, nameof(cep));
        Guard.AgainstNullOrWhiteSpace(street, nameof(street));
        Guard.AgainstNullOrWhiteSpace(neighborhood, nameof(neighborhood));
        Guard.AgainstNullOrWhiteSpace(city, nameof(city));
        Guard.AgainstNullOrWhiteSpace(state, nameof(state));
        Guard.AgainstNullOrWhiteSpace(country, nameof(country));

        DomainException.ThrowIf(!Regex.IsMatch(cep, @"^\d{5}-\d{3}$"), "Invalid CEP");

        Cep = cep;
        Street = street;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cep!;
        yield return Street!;
        yield return Neighborhood!;
        yield return City!;
        yield return State!;
        yield return Country!;
    }

    public string? Cep { get; private set; }
    public string? Street { get; private set; }
    public string? Neighborhood { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Country { get; private set; }

    public static DeliveryAddress Create(
        string cep,
        string street,
        string neighborhood,
        string city,
        string state,
        string country)
    {
        return new DeliveryAddress(cep, street, neighborhood, city, state, country);
    }

    public string FormatAddress()
    {
        return $"{Cep}, {Street}, {Neighborhood}, {City}, {State}";
    }
}