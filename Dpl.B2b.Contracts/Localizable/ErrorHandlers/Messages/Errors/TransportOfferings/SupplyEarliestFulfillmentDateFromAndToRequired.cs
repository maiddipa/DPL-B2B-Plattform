namespace Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings
{
    public class SupplyEarliestFulfillmentDateFromAndToRequired: ErrorBase
    {
        public override string Meaning => "SupplyEarliestFulfillmentDateFromAndToRequired";
        public override string Description => "Both SupplyEarliestFulfillmentDateFrom and SupplyEarliestFulfillmentDateTo need to be provided when filtering on SupplyEarliestFulfillmentDate";
    }
}