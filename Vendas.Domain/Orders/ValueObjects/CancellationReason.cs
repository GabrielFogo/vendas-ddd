using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Enums;

namespace Vendas.Domain.ValueObjects;

public sealed class CancellationReason : ValueObject
{
    public CancellationReasonCode Code { get; }
    public string Description { get; }

    private static readonly Dictionary<CancellationReasonCode, string> StandardReasons = new()
    {
        { CancellationReasonCode.CostumerGaveUp, "Costumer Gave Up" },
        { CancellationReasonCode.PaymentError, "Payment Error" },
        { CancellationReasonCode.ItemOutOfStock, "Items Out Stock" },
        { CancellationReasonCode.InvalidAddress, "Invalid Address" },
        { CancellationReasonCode.Other, "Other reason" }
    };

    public CancellationReason(CancellationReasonCode code)
    {
        Code = code;
        Description = StandardReasons[code];
    }

    public static CancellationReason CostumerGaveUp()
    {
        return new CancellationReason(CancellationReasonCode.CostumerGaveUp);
    }

    public static CancellationReason PaymentError()
    {
        return new CancellationReason(CancellationReasonCode.PaymentError);
    }

    public static CancellationReason ItemOutOfStock()
    {
        return new CancellationReason(CancellationReasonCode.ItemOutOfStock);
    }

    public static CancellationReason InvalidAddress()
    {
        return new CancellationReason(CancellationReasonCode.InvalidAddress);
    }

    public static CancellationReason Other()
    {
        return new CancellationReason(CancellationReasonCode.Other);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Description;
    }
}