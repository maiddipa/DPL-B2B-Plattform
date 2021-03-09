namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderMatches
{
    public class QuantityExceededSupplyError : ErrorBase
    {
        public override string Meaning => "QuantityExceededSupplyError";
        public override string Description => "Available Quantity exceeded for supply order";
    }
}