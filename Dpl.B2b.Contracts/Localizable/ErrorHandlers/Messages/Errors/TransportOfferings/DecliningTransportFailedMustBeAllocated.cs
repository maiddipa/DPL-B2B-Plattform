namespace Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings
{
    public class DeclineTransportFailedMustBeAllocated : ErrorBase
    {
        public override string Meaning => "DeclineTransportFailedMustBeAllocated";
        public override string Description => "Decline transport failed. Transports can only be accepted when status is status 'Allocated'";
    }
}