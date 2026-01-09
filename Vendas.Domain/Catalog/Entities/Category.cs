using Vendas.Domain.Catalog.Events;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Catalog.Entities;

public sealed class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    
    public Category(string name, string description)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.Against<DomainException>(name.Length < 3, "Category name must be at least 3 characters long.");
        
        Name = name.Trim();
        Description = description;
        IsActive = true;
    }

    public void Activate()
    {
        Guard.Against<DomainException>(IsActive, "Can't activate a category already active.");
        
        IsActive = true;
        
        SetModifiedAt();
        AddDomainEvent(new CategoryActiveEvent(Id));
    }
    
    public void Deactivate()
    {
        Guard.Against<DomainException>(!IsActive, "Can't deactivate a category already active.");
        
        IsActive = false;
        
        SetModifiedAt();
        AddDomainEvent(new CategoryInactivateEvent(Id));
    }

    public void SetName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.Against<DomainException>(name.Length < 3, "Category name must be at least 3 characters long.");
        
        Name = name.Trim();
        
        SetModifiedAt();
    }
    
    public void SetDescription(string description)
    {
        Guard.AgainstNullOrWhiteSpace(description, nameof(description));
        
        Description = description;
        
        SetModifiedAt();
    }
}