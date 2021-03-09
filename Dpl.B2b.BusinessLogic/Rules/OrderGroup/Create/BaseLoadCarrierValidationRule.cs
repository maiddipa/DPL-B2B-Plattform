using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Dynamic.Core;
using System.Linq;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderGroup;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.OrderGroup.Create
{
    public class BaseLoadCarrierValidationRule : BaseValidationWithServiceProviderRule<BaseLoadCarrierValidationRule, BaseLoadCarrierValidationRule.ContextModel>
    {
        public BaseLoadCarrierValidationRule(OrderGroupsCreateRequest request, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new BaseLoadCarrierNotAllowed();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            var loadCarrierRepository = ServiceProvider.GetService<IRepository<Olma.LoadCarrier>>();
            Olma.LoadCarrier loadCarrier = loadCarrierRepository
                .FindByCondition(lc => lc.Id == Context.Request.LoadCarrierId)
                .Include(lct => lct.Type).ThenInclude(blc => blc.BaseLoadCarrierMappings).FirstOrDefault();
            if (loadCarrier == null) return;

            switch (loadCarrier.Type.BaseLoadCarrier)
            {
                case BaseLoadCarrierInfo.None when Context.Request.BaseLoadCarrierId != null:
                    RuleState.Add(ResourceName, new RuleState {RuleStateItems = {Message}});
                    break;
                case BaseLoadCarrierInfo.Required when Context.Request.BaseLoadCarrierId == null:
                    RuleState.Add(ResourceName, new RuleState {RuleStateItems = {new BaseLoadCarrierRequired()}});
                    break;
                default:
                {
                    if (loadCarrier.Type.BaseLoadCarrier != BaseLoadCarrierInfo.None &&
                        Context.Request.BaseLoadCarrierId != null && loadCarrier.Type.BaseLoadCarrierMappings.All(blc =>
                            blc.LoadCarrierId != Context.Request.BaseLoadCarrierId))
                    {
                        RuleState.Add(ResourceName, new RuleState {RuleStateItems = {new BaseLoadCarrierError()}});
                    }
                    break;
                }
            }
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<OrderGroupsCreateRequest>
        {
            public ContextModel(OrderGroupsCreateRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public OrderGroupsCreateRequest Request => Parent;

            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {

            }
        }

        #endregion
    }
}