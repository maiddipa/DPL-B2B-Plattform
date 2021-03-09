namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.TransportOfferings
{
    public class TransportValid : ErrorBase
    {
        public override string Meaning => "TransportValid";
        public override string Description => "Bids can only placed when transport status is 'Requested' or 'Allocated";
    }
}
