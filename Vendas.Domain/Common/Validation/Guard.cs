using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Common.Validation;

internal static class Guard
{
    public static void AgainstEmptyGuid(Guid id, string paramName)
    {
        var message = $"{paramName} can't be empty";
        
        DomainException.ThrowIf(Guid.Empty.Equals(id), message);
    }

    public static void AgainstNull<T>(T obj, string paramName)
    {
        var message = $"{paramName} can't be null";
        
        DomainException.ThrowIf(obj == null, message);
    }

    public static void AgainstNullOrWhiteSpace(string value, string paramName)
    {
        var message = $"{paramName} can't be null or empty";
        
        DomainException.ThrowIf(string.IsNullOrWhiteSpace(value), message);
    }

    public static void Against<TException>(bool condition, string message) where TException : Exception, new()
    {
        if (condition) 
            throw (TException)Activator.CreateInstance(typeof(TException), message)!;
    }
}