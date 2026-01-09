using System.Text.RegularExpressions;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Customers.Entities;

public sealed class Address : Entity
{
    public string Cep { get; private set; }
    public string Street { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }

    private static void Validate(string cep, string street, string neighborhood, string city, string state,
        string country)
    {
        Guard.AgainstNullOrWhiteSpace(cep, nameof(cep));
        Guard.Against<DomainException>(!Regex.IsMatch(cep, @"^\d{8}$"), "Invalid cep");
        Guard.AgainstNullOrWhiteSpace(street, nameof(street));
        Guard.AgainstNullOrWhiteSpace(neighborhood, nameof(neighborhood));
        Guard.AgainstNullOrWhiteSpace(city, nameof(city));
        Guard.AgainstNullOrWhiteSpace(state, nameof(state));
        Guard.AgainstNullOrWhiteSpace(country, nameof(country));
    }

    public Address(string cep, string street, string neighborhood, string city, string state, string country)
    {
        Validate(cep, street, neighborhood, city, state, country);

        Cep = cep;
        Street = street;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
    }

    internal void Update(string cep, string street, string neighborhood, string city, string state, string country)
    {
        Validate(cep, street, neighborhood, city, state, country);

        Cep = cep;
        Street = street;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
    }
}