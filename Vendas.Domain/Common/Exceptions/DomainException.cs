namespace Vendas.Domain.Common.Exceptions;

public class DomainException(string message) : Exception(message)
{
    public static void ThrowIf(bool condition, string message)
    {
        if (condition) 
            throw new DomainException(message);
    }
}