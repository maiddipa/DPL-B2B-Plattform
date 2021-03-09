using System.Linq;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Partner;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.CustomerPartner.Create
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        private static readonly string BlacklistMessageId = new BlacklistMatch().Id;
        
        public MainRule(CustomerPartnersCreateRequest request)
        {
            // Create Context
            Context = new ContextModel(request, this);
        }
        
        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new PartnerCanNotCreated();

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
                .Eval(Context.Rules.CustomerNameRequiredRule)
                .Eval(Context.Rules.PartnerDirectoryResourceRule)
                .Eval(Context.Rules.BlacklistTermsRule);

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
        /// <param name="messageId">Search for</param>
        /// <param name="key">Remove RuleStateDictionary Key</param>
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
        public class ContextModel : ContextModelBase<CustomerPartnersCreateRequest>
        {
            public ContextModel(CustomerPartnersCreateRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            
            public Olma.PartnerDirectory PartnerDirectory => Rules.PartnerDirectoryResourceRule.Context.Resource;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                PartnerDirectoryResourceRule = new ResourceRule<Olma.PartnerDirectory>(context.Parent.DirectoryId, rule)
                    .Include(i=>i.OrganizationPartnerDirectories);
                
                CustomerNameRequiredRule=new ValidOperatorRule<ContextModel>(
                    context, 
                    model => !string.IsNullOrEmpty(model.Parent.CompanyName),
                    new CustomerNameRequired());
                
                BlacklistTermsRule= new BlacklistTermsRule(context, rule);
            }

            public ResourceRule<Olma.PartnerDirectory> PartnerDirectoryResourceRule { get; }
            
            public ValidOperatorRule<ContextModel> CustomerNameRequiredRule{ get; }
            
            public BlacklistTermsRule BlacklistTermsRule{ get; }
        }

        #endregion
    }
}