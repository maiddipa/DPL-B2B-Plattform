using System;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Create
{
    public class DocumentTypeRule : BaseValidationWithServiceProviderRule<DocumentTypeRule, DocumentTypeRule.ContextModel>
    {
        public DocumentTypeRule(MainRule.ContextModel request, IRule parentRule)
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

            Context.DocumentType = GetDocumentType(Context.VoucherType);
        }
        
        private DocumentTypeEnum GetDocumentType(VoucherType type)
        {
            switch (type)
            {
                case VoucherType.Original:
                    return DocumentTypeEnum.VoucherOriginal;
                case VoucherType.Digital:
                    return DocumentTypeEnum.VoucherDigital;
                case VoucherType.Direct:
                    return DocumentTypeEnum.VoucherDirectReceipt;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            public VoucherType VoucherType => Parent.Type;
            public DocumentTypeEnum DocumentType { get; protected internal set; }
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