using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Create
{
    public class BlacklistTermsRule : BaseValidationWithServiceProviderRule<BlacklistTermsRule, BlacklistTermsRule.ContextModel>
    {
        public BlacklistTermsRule(MainRule.ContextModel context, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new BlacklistMatch();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator
                .Eval(Context.Rules.BlacklistLicensePlate)
                .Eval(Context.Rules.BlacklistOther);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        private bool ValidLicensePlate(ContextModel context)
        {
            var dbContext = ServiceProvider.GetService<OlmaDbContext>();
            var blacklistFactory = ServiceProvider.GetService<IBlacklistFactory>();
            
            var customerPartner = dbContext.CustomerPartners
                .FirstOrDefault(p => p.RowGuid == context.RecipientGuid);
            
            var blacklistRule = new BlacklistRule(
                new [] {context.LicensePlate}, 
                blacklistFactory.CreateLicensePlateBlacklist("CompanyName", context.TruckDriverCompanyName, customerPartner?.CompanyName), 
                parentRule: this);
            
            return blacklistRule.Validate();
        }

        private bool ValidOther(ContextModel context)
        {
            var blacklistFactory = ServiceProvider.GetService<IBlacklistFactory>();
            
            var blacklistRule = new BlacklistRule(
                new[] {context.TruckDriverCompanyName},
                blacklist: blacklistFactory.CreateCommonFieldsBlacklist(),
                parentRule: this);
            
            return blacklistRule.Validate();
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public Guid RecipientGuid => Parent.RecipientGuid;
            public string LicensePlate => Parent.LicensePlate;
            public string TruckDriverCompanyName => Parent.TruckDriverCompanyName;
            
            public ContextModel(MainRule.ContextModel parent, BlacklistTermsRule termsRule) : base(parent, termsRule)
            {
                Rules=new RulesBundle(this, termsRule);
            }

            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly ValidOperatorRule<ContextModel> BlacklistLicensePlate;
            public readonly ValidOperatorRule<ContextModel> BlacklistOther;
            
            public RulesBundle(ContextModel context, BlacklistTermsRule termsRule)
            {
                BlacklistLicensePlate = new ValidOperatorRule<ContextModel>(
                    context,
                    termsRule.ValidLicensePlate,
                    new BlacklistMatch(),
                    parentRule: termsRule, 
                    nameof(BlacklistLicensePlate));

                BlacklistOther = new ValidOperatorRule<ContextModel>(
                    context,
                    termsRule.ValidOther,
                    new BlacklistMatch(),
                    parentRule: termsRule, 
                    nameof(BlacklistOther));
            }
        }

        #endregion
    }
}