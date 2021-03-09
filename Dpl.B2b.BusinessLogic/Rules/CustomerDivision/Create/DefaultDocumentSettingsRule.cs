using System;
using System.Collections.Generic;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.CustomerDivision.Create
{
    public class DefaultDocumentSettingsRule : BaseValidationWithServiceProviderRule<DefaultDocumentSettingsRule, DefaultDocumentSettingsRule.ContextModel>
    {
        public DefaultDocumentSettingsRule(int customerId, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(customerId, this);
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
            // {{TODO}}
            

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
            public ContextModel(int parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public ICollection<Olma.CustomerDivisionDocumentSetting> CustomerDivisionDocumentSettings =>
                GenerateDefaultCustomerDivisionDocumentSetting();

            private ICollection<Olma.CustomerDivisionDocumentSetting> GenerateDefaultCustomerDivisionDocumentSetting()
            {
                var customerDivisionDocumentSettings = new List<Olma.CustomerDivisionDocumentSetting>();

                foreach (var documentTypeEnum in (DocumentTypeEnum[]) Enum.GetValues(typeof(DocumentTypeEnum)))
                {
                    customerDivisionDocumentSettings.Add(
                        new Olma.CustomerDivisionDocumentSetting
                        {
                            Override = true,
                            DefaultPrintCount = 2,
                            DocumentTypeId = (int)documentTypeEnum,
                            PrintCountMax = 5,
                            PrintCountMin = 1,
                            DocumentNumberSequence = new Olma.DocumentNumberSequence
                            {
                                Counter = 0,
                                //Prefix = "Doc",
                                Separator = "-",
                                CustomerId = Parent,
                                DisplayName = "Default",
                                DocumentType = documentTypeEnum,
                                PrefixForCounter = "-",
                                PaddingLengthForCounter = 4,
                                CanAddDivisionShortName = true,
                                CanAddDocumentTypeShortName = true,
                                PaddingLengthForCustomerNumber = 0,
                                CanAddPaddingForCounter = true
                            }
                        });
                }

                return customerDivisionDocumentSettings;
            }
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