namespace Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings
{
    public class AcceptingTransportFailedMustBeAllocated : ErrorBase
    {
        public override string Meaning => "AcceptingTransportFailedMustBeAllocated";
        public override string Description => "Accepting transport failed. Transports can only be accepted when status is status 'Allocated'";
    }
}