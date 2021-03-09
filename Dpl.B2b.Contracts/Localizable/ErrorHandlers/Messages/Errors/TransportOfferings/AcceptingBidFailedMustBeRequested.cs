namespace Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings
{
    public class AcceptingBidFailedMustBeRequested: ErrorBase
    {
        public override string Meaning => "AcceptingFailedMustBeRequested";
        public override string Description => "Accepting bid failed. Bids can only be accepted for transports with status 'Requested'.";
    }
}