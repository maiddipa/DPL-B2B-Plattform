namespace Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings
{
    public class DemandLatestFulfillmentDateFromAndToRequired: ErrorBase
    {
        public override string Meaning => "DemandLatestFulfillmentDateFromAndToRequired";
        public override string Description => "Both DemandLatestFulfillmentDateFrom and DemandLatestFulfillmentDateTo need to be provided when filtering on DemandLatestFulfillmentDate";
    }
}