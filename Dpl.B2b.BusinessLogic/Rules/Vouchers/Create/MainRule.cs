using System;
using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Vouchers;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Create
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithAuthorizationSkill
    {
        private static readonly string BlacklistMessageId = new BlacklistMatch().Id;
        
        public MainRule(VouchersCreateRequest request)
        {
            // Create Context
            Context = new ContextModel(request, this);
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
                .Eval(Context.Rules.CustomerDivisionAuthorization)
                .Eval(Context.Rules.IssuerDivisionHasPostingAccount) // Soll auf alle fälle ein Fehler sein!
                .Eval(Context.Rules.RecipientHasDefaultPostingAccount) // Für DEMO erst mal nur Warnung
                .Eval(Context.Rules.DocumentType)
                .Eval(Context.Rules.Voucher)
                .Eval(Context.Rules.LocalizationLanguageValid)
                .Eval(Context.Rules.PrintCountValid)
                .Eval(Context.Rules.BlacklistTerms);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);

            // Falls ein verletzung der Blacklist Regel vorliegt dann entferne den allgemeinen RuleState für den ResourceName  
            CleanupRuleStateIfExists(BlacklistMessageId, ResourceName);
        }

        /// <summary>
        /// Remove RuleState for key 
        /// </summary>
        /// <param name="key">RuleStateDictionary Key</param>
        private void CleanupRuleStateIfExists(string messageId, string key)
        {
            var drop = RuleState.Any(rs =>
                rs.Value.Any(item => item.MessageId == messageId));

            if(drop)  
                RuleState.Remove(key);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<VouchersCreateRequest>
        {
            public ContextModel(VouchersCreateRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public int CustomerDivisionId => Parent.CustomerDivisionId;
            public Olma.CustomerDivision CustomerDivision => Rules.CustomerDivisionAuthorization.Context[0];
            public string ExpressCode => Parent.ExpressCode;
            public Guid RecipientGuid => Parent.RecipientGuid;
            public Guid ShipperGuid => Parent.ShipperGuid;
            public Guid SupplierGuid => Parent.SupplierGuid;
            public Guid SubShipperGuid => Parent.SubShipperGuid;
            public VoucherType Type => Parent.Type;
            public int PrintLanguageId => Parent.PrintLanguageId;
            public int PrintDateTimeOffset => Parent.PrintDateTimeOffset;
            public int PrintCount => Parent.PrintCount;
            public string LicensePlate => Parent.LicensePlate;
            public string TruckDriverCompanyName => Parent.TruckDriverCompanyName;

        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, MainRule rule)
            {
                CustomerDivisionAuthorization =
                    new ResourceAuthorizationRule<
                            Olma.CustomerDivision,
                            CanCreateVoucherRequirement,
                            DivisionPermissionResource>(context.CustomerDivisionId, parentRule: rule)
                        .Include(i => i.PostingAccount);

                IssuerDivisionHasPostingAccount = new ValidOperatorRule<ContextModel>(
                    (ctx) => ctx.Type == VoucherType.Direct,
                    context,
                    ctx => ctx.CustomerDivision.PostingAccount != null,
                    new PostingAccountRequired(),
                    parentRule: rule,
                    nameof(IssuerDivisionHasPostingAccount));

                RecipientHasDefaultPostingAccount = new ValidOperatorRule<ContextModel>(
                    (ctx) => ctx.Type == VoucherType.Direct,
                    context,
                    ctx =>
                    {
                        var repository = rule.ServiceProvider.GetService<IRepository<Olma.CustomerPartner>>();
                        
                        var customerPartner = repository.FindByCondition(cp => 
                                cp.RowGuid == ctx.Parent.RecipientGuid 
                                && !cp.IsDeleted)
                            .IgnoreQueryFilters()
                            .Include(cp => cp.Partner).ThenInclude(p => p.DefaultPostingAccount)
                            .FirstOrDefault();
                        
                        return customerPartner?.Partner?.DefaultPostingAccount != null;
                    },
                    new RecipientHasDefaultPostingAccount(),
                    parentRule: rule,
                    nameof(RecipientHasDefaultPostingAccount));

                PrintCountValid = new ValidOperatorRule<ContextModel>(
                    context,
                    ctx => ctx.PrintCount > 0,
                    new NotAllowedByRule(),
                    parentRule: rule,
                    nameof(PrintCountValid));
                
                DocumentType = new DocumentTypeRule(context, parentRule: rule);

                Voucher = new VoucherRule(context, parentRule: rule);
                
                LocalizationLanguageValid = new LocalizationLanguageValidRule(context.PrintLanguageId, parentRule: rule);
                
                BlacklistTerms= new BlacklistTermsRule(context, parentRule: rule);
            }

            public readonly ResourceAuthorizationRule<
                Olma.CustomerDivision,
                CanCreateVoucherRequirement,
                DivisionPermissionResource> CustomerDivisionAuthorization;

            public readonly ValidOperatorRule<ContextModel> IssuerDivisionHasPostingAccount;
            public readonly ValidOperatorRule<ContextModel> RecipientHasDefaultPostingAccount;
            public readonly DocumentTypeRule DocumentType;
            public readonly VoucherRule Voucher;
            public readonly ValidOperatorRule<ContextModel> PrintCountValid;
            public readonly LocalizationLanguageValidRule LocalizationLanguageValid;
            public readonly BlacklistTermsRule BlacklistTerms;
            
        }

        #endregion
        
        // TODO CHeck if User ist allowed to transfer to DestinationAccount, discuss how we should control this/define this via MasterData
        public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (
            Context.Rules.CustomerDivisionAuthorization.Authorization.AuthorizationRule,
            Context.Rules.CustomerDivisionAuthorization.Authorization.ResourceRule
        );
    }
}