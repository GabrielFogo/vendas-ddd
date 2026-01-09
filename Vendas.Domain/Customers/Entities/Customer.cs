using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;
using Vendas.Domain.Customers.Enums;
using Vendas.Domain.Customers.Events;
using Vendas.Domain.Customers.ValuesObjects;

namespace Vendas.Domain.Customers.Entities;

public sealed class Customer : AggregateRoot
{
    public CompleteName Name { get; private set; }
    public Cpf Cpf { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }
    public Phone Phone { get; private set; }
    public Guid MainAddressId { get; private set; }
    public CustomerStatus Status { get; private set; }
    public MaritalStatus MaritalStatus { get; private set; }
    public Gender Gender { get; private set; }
    
    private readonly List<Address> _addresses = new ();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    public Customer(
        CompleteName name,
        Cpf cpf,
        Email email,
        Phone phone,
        Address mainAddress,
        Gender gender,
        MaritalStatus maritalStatus = MaritalStatus.Unknown)
    {
        Validate(name, cpf, email, phone, mainAddress);
        
        Name = name;
        Cpf = cpf;
        Email = email;
        Phone = phone;
        MainAddressId = mainAddress.Id;
        Address = mainAddress;
        MaritalStatus = maritalStatus;
        Gender = gender;
        
        _addresses.Add(mainAddress);
        
        MainAddressId = mainAddress.Id;
        
        AddDomainEvent(new CustomerRegisterEvent(Id, Name.NameFormated, Cpf.Number, Email.Address));
    }

    public void AddAddress(Address address)
    {
        Guard.AgainstNull(address, nameof(address));
        
        _addresses.Add(address);
        
        SetModifiedAt();
    }

    public void RemoveAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(b => b.Id == addressId);
        
        Guard.AgainstNull(address, nameof(address));
        Guard.Against<DomainException>(_addresses.Count == 1, "Cannot remove address from existing address");
        
        _addresses.Remove(address!);

        if (addressId == MainAddressId)
        {
            MainAddressId = _addresses.First().Id;
            
            AddDomainEvent(new MainAddressChangedEvent(Id, MainAddressId));
        }
        
        SetModifiedAt();
    }
    
    public void UpdateAddress(
        Guid addressId,
        string cep,
        string street,
        string neighborhood,
        string city,
        string state,
        string country)
    {
        var address = _addresses.FirstOrDefault(b => b.Id == addressId);
        
        Guard.AgainstNull(address, nameof(address));

        address!.Update(cep, street, neighborhood, city, state, country);
        
        SetModifiedAt();
    }
    
    public void SetMainAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(b => b.Id == addressId);
        
        Guard.AgainstNull(address, nameof(address));
        
        MainAddressId = address!.Id;
        
        AddDomainEvent(new MainAddressChangedEvent(Id, MainAddressId));
        SetModifiedAt();
    }

    public Address GetMainAddress()
    {
        return _addresses.First(b => b.Id == MainAddressId);
    }

    public void UpdateProfile(
        CompleteName name,
        Email email,
        Phone phone,
        Gender gender,
        MaritalStatus maritalStatus)
    {
        Guard.Against<DomainException>(Status == CustomerStatus.Blocked, "Cannot update profile because it is blocked");
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(phone, nameof(phone));
        Guard.AgainstNull(maritalStatus, nameof(maritalStatus));
        Guard.AgainstNull(gender, nameof(gender));

        Name = name;
        Email = email;
        Phone = phone;
        Gender = gender;
        MaritalStatus = maritalStatus;

        SetModifiedAt();
    }

    public void Block()
    {
        if (Status == CustomerStatus.Blocked) return;
        
        Status = CustomerStatus.Blocked;
        
        AddDomainEvent(new CustomerBlockedEvent(Id, Cpf.Number));
        SetModifiedAt();
    }

    private void Validate(
        CompleteName name,
        Cpf cpf,
        Email email,
        Phone phone,
        Address mainAddress)
    {
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(cpf, nameof(cpf));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(phone, nameof(phone));
        Guard.AgainstNull(mainAddress, nameof(mainAddress));
    }
}