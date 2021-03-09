using System;
//using System.Data.Entity;
using System.Linq;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.TransportOfferings.Shared;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.GetById
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithRessourceSkill
    {
        public MainRule(int transportId, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(transportId, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

            // Get DivisionIds
            Context.DivisionIds = ServiceProvider
                .GetService<IAuthorizationDataService>()
                .GetDivisionIds()
                .ToArray();
            
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();
            
            // Assign rules to the Evaluator
            rulesEvaluator
                .Eval(Context.Rules.TransportValid)
                .Eval(Context.Rules.ModifyTransportOffering)
                .Eval(Context.Rules.MaskAddressIfNecessary);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<int>
        {
            public ContextModel(int parent, MainRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            
            public int TransportId => Parent;
            
            public TransportOffering TransportOffering => Rules.TransportValid.Context.TransportOffering;
            public int[] DivisionIds { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                TransportValid=new TransportValidRule(context, rule);

                var getOperator = new GetOperatorRule<ContextModel, TransportOffering>(context, (c)=>c.TransportOffering);

                MaskAddressIfNecessary = new MaskAddressIfNecessaryRule(getOperator);
                ModifyTransportOffering = new ModifyTransportOfferingRule(context, rule);
            }

            public TransportValidRule TransportValid { get; protected internal set; }

            public MaskAddressIfNecessaryRule MaskAddressIfNecessary { get; set; }
            
            public ModifyTransportOfferingRule ModifyTransportOffering { get; set; }
        }

        #endregion

        public IValidationRule ResourceRule => Context.Rules.TransportValid;
    }
}