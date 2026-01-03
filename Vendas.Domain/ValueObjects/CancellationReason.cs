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

    private CancellationReason(CancellationReasonCode code)
    {
        Code = code;
        Description = StandardReasons[code];
    }

    public static CancellationReason CostumerGaveUp() => new (CancellationReasonCode.CostumerGaveUp);
    public static CancellationReason PaymentError() => new (CancellationReasonCode.PaymentError);
    public static CancellationReason ItemOutOfStock() => new (CancellationReasonCode.ItemOutOfStock);
    public static CancellationReason InvalidAddress() => new (CancellationReasonCode.InvalidAddress);
    public static CancellationReason Other() =>  new (CancellationReasonCode.Other);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Description;
    }
}