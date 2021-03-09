namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderMatches
{
    public class QuantityExceededDemandError : ErrorBase
    {
        public override string Meaning => "QuantityExceededDemandError";
        public override string Description => "Available Quantity exceeded for demand order";
    }
}