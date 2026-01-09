using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validation;

namespace Vendas.Domain.Customers.ValuesObjects;

public sealed class Cpf : ValueObject
{
    public string Number { get; private set; }

    public Cpf(string number)
    {
        Guard.AgainstNull(number, nameof(number));

        var digits = new string(number.Where(c => !char.IsDigit(c)).ToArray());

        Guard.Against<DomainException>(digits.Length != 11, "Cpf number must contain 11 digits");
        Guard.Against<DomainException>(!ValidateCpf(digits), "Cpf number is invalid");

        Number = digits;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }

    private static bool ValidateCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        var invalidos = new string[]
        {
            "00000000000", "11111111111", "22222222222",
            "33333333333", "44444444444", "55555555555",
            "66666666666", "77777777777", "88888888888",
            "99999999999"
        };

        if (invalidos.Contains(cpf))
            return false;

        var numeros = cpf.Select(c => c - '0').ToArray();
        var soma = 0;

        for (var i = 0; i < 9; i++)
            soma += numeros[i] * (10 - i);

        var resto = soma % 11;
        var primeiroDigito = resto < 2 ? 0 : 11 - resto;

        if (numeros[9] != primeiroDigito)
            return false;

        soma = 0;

        for (var i = 0; i < 10; i++)
            soma += numeros[i] * (11 - i);

        resto = soma % 11;

        var segundoDigito = resto < 2 ? 0 : 11 - resto;

        if (numeros[10] != segundoDigito)
            return false;

        return true;
    }
}