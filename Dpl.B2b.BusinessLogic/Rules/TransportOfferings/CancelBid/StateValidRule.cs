﻿using System;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.OrderGroup;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.CancelBid
{
    public class StateValidRule : BaseValidationWithServiceProviderRule<StateValidRule, StateValidRule.ContextModel>
    {
        public StateValidRule(MainRule.ContextModel context, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(context, this);
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

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator.Eval(Context.Rules.NotInStateWon);
            rulesEvaluator.Eval(Context.Rules.NotInStateLost);
            rulesEvaluator.Eval(Context.Rules.NotInStateCanceled);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, StateValidRule rule) : base(parent, rule)
            {

            }
            
            public RulesBundle Rules { get; protected internal set; }

            public Olma.TransportBid Bid => Parent.Bid;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                NotInStateWon = new ValidOperatorRule<ContextModel>(
                    context, 
                    (c) => c.Bid.Status != TransportBidStatus.Won, 
                    new WinningBidsCannotCanceled(), 
                    rule);
                
                NotInStateCanceled = new ValidOperatorRule<ContextModel>(context, 
                    (c) => c.Bid.Status != TransportBidStatus.Canceled, 
                    new BidAlreadyCanceled(), 
                    rule);
                
                NotInStateLost = new ValidOperatorRule<ContextModel>(context,
                    (c) => c.Bid.Status != TransportBidStatus.Lost,
                    new LostBidsCannotBeCanceled(),
                    rule);
            }

            public readonly ValidOperatorRule<ContextModel> NotInStateWon;
            public readonly ValidOperatorRule<ContextModel> NotInStateCanceled;
            public readonly ValidOperatorRule<ContextModel> NotInStateLost;
        }

        #endregion
    }
}