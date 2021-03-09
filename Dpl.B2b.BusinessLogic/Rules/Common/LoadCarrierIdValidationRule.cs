using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class LoadCarrierIdValidationRule : BaseValidationWithServiceProviderRule<LoadCarrierIdValidationRule,
        LoadCarrierIdValidationRule.ContextModel>
    {
        public LoadCarrierIdValidationRule(int loadCarrierId, IRule parentRule)
        {
            ResourceName = "LoadCarrier";
            Context = new ContextModel(loadCarrierId, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new LoadCarrierNotFound();

        protected override void EvaluateInternal()
        {
            if (Context.LoadCarrierId == 0)
            {
                RuleState.Add(ResourceName, new RuleState {RuleStateItems = {new LoadCarrierRequired()}});
                return;
            }

            var loadCarrierRepository = ServiceProvider.GetService<IRepository<Olma.LoadCarrier>>();
            var loadCarrierExists = loadCarrierRepository.FindAll().Any(lc => lc.Id == Context.LoadCarrierId);

            AddMessage(!loadCarrierExists, ResourceName, Message);
        }

        public class ContextModel : ContextModelBase<int>
        {
            public ContextModel(int loadCarrierId,
                [NotNull] LoadCarrierIdValidationRule rule) :
                base(loadCarrierId, rule)
            {
            }

            public int LoadCarrierId => Parent;
        }
    }
}