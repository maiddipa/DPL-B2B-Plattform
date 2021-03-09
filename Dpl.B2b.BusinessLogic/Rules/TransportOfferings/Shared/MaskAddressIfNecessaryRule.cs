using System;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.Shared
{
    public class MaskAddressIfNecessaryRule : IRule<TransportOffering>, IParentChildRule
    {
        public MaskAddressIfNecessaryRule(IRule<TransportOffering> transportOfferingRule, IRule parentRule = null)
        {
            ParentRule = parentRule;
            TransportOfferingRule = transportOfferingRule;
        }

        private IRule<TransportOffering> TransportOfferingRule { get; }
        public IRule ParentRule { get; }

        void IRule.Evaluate()
        {
            var transportOffering = TransportOfferingRule.Evaluate();
            MaskAddressIfNecessary(transportOffering);
        }

        public TransportOffering Evaluate()
        {
            var transportOffering = TransportOfferingRule.Evaluate();
            MaskAddressIfNecessary(transportOffering);
            return transportOffering;
        }

        public bool IsMatch()
        {
            return true;
        }

        public string RuleName => this.NamespaceName();

        private static void MaskAddressIfNecessary(TransportOffering transport)
        {
            if (transport.WinningBid != null) return;

            transport.SupplyInfo.Address.PostalCode = transport.SupplyInfo.Address.PostalCode.Substring(0, 2).PadRight(5, 'X');
            transport.SupplyInfo.Address.City = "".PadRight(10, 'X');
            transport.SupplyInfo.Address.Street1 = "".PadRight(10, 'X');

            transport.DemandInfo.Address.PostalCode = transport.DemandInfo.Address.PostalCode.Substring(0, 2).PadRight(5, 'X');
            transport.DemandInfo.Address.City = "".PadRight(10, 'X');
            transport.DemandInfo.Address.Street1 = "".PadRight(10, 'X');
            transport.Distance = transport.Distance == null
                ? transport.Distance
                : Convert.ToInt64(Math.Round(transport.Distance.Value / 10000d) * 10000);
        }
    }
}