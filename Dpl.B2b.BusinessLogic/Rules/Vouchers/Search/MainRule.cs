using System;
using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Vouchers.Shared;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Search
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(VouchersSearchRequest request, IRule parentRule=null)
        {
            // Create Context
            Context = new ContextModel(request, this);
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
            rulesEvaluator
                .Eval(Context.Rules.VouchersSearchQueryRule)
                .Eval(Context.Rules.VouchersValidRule);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            // TODO AuthorizationRule
            // Der QueryFilter sorgt bereits für den einen Datenschutz Filter.
            // Die DAL Objekte können für die Autorisierung nicht herangezogen werden
            // foreach (var page in Context.PaginationResult.Data)
            // {
            //     var getOperatorRule=new GetOperatorRule<ContextModel, Voucher>(Context, model => page);
            //     
            //     var authorizationRule = new AuthorizationRule<
            //         Voucher,
            //         ContextModel,
            //         DivisionPermissionResource,
            //         CanReadVoucherRequirement>(getOperatorRule, this);
            //         
            //     authorizationRule.Validate(RuleState);
            // }

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<VouchersSearchRequest>
        {
            public ContextModel(VouchersSearchRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public VouchersSearchRequest VouchersSearchRequest => Parent;

            public RulesBundle Rules { get; protected internal set; }

            public IPaginationResult<Voucher> PaginationResult => Rules.VouchersValidRule.Context.PaginationResult;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, MainRule rule)
            {
                VouchersSearchQueryRule= new VouchersSearchQueryRule(context.VouchersSearchRequest, rule);
                VouchersValidRule= new VouchersValidRule(context, rule);
            }
            
            public readonly VouchersSearchQueryRule VouchersSearchQueryRule;
            
            public readonly VouchersValidRule VouchersValidRule;
        }

        #endregion
    }
}