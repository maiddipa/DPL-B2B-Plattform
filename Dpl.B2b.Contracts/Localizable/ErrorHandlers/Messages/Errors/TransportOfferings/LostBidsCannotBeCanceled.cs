namespace Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings
{
    public class LostBidsCannotBeCanceled: ErrorBase
    {
        public override string Meaning => "NotInStateLost";
        public override string Description => "Lost bids cannot be canceled";
    }
}