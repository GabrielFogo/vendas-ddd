using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Common.Validation;

internal static class Guard
{
    public static void AgainstEmptyGuid(Guid id, string paramName)
    {
        if (id == Guid.Empty)
            throw new DomainException($"{paramName} can't be empty");
    }

    public static void AgainstNull<T>(T obj, string paramName)
    {
        if (obj == null)
            throw new DomainException($"{paramName} can't be null");
    }

   public static void Against<TException>(bool condition, string message) where TException : Exception, new()
    {
        if (condition) 
            throw (TException)Activator.CreateInstance(typeof(TException), message)!;
    }
}