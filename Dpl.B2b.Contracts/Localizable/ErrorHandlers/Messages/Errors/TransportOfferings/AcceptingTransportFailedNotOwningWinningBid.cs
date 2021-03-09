namespace Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings
{
    public class AcceptingTransportFailedNotOwningWinningBid : ErrorBase
    {
        public override string Meaning => "AcceptingTransportFailedNotOwningWinningBid";
        public override string Description => "Accepting transport failed. Transports can only be accepted when owning the winning bid.";
    }
}