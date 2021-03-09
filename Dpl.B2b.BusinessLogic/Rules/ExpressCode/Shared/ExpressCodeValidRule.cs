using System;
using System.Linq;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.ExpressCode;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.ExpressCode.Shared
{
    public class ExpressCodeValidRule : BaseValidationWithServiceProviderRule<ExpressCodeValidRule, ExpressCodeValidRule.ContextModel>
    {
        public ExpressCodeValidRule(ExpressCodesSearchRequest request, IRule parentRule)
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
            Context.ExpressCodeRepository = ServiceProvider.GetService<IRepository<Olma.ExpressCode>>();
            Context.ExpressCodeUsageConditionRepository = ServiceProvider.GetService<IRepository<Olma.ExpressCodeUsageCondition>>();
            Context.Mapper = ServiceProvider.GetService<IMapper>();

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // Not things to validate
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            Context.ExpressCode = Context.ExpressCodeResult;
            
            if (Context.ExpressCode == null)
            {
                RuleState.Add(ResourceName, new RuleState(Context.ExpressCode) {RuleStateItems = {new DigitalCodeInvalid()}});
            }
            else
            {
                if (Context.Parent.PrintType == PrintType.LoadCarrierReceiptDelivery &&
                    (Context.ExpressCodeUsageCondition == null || !Context.ExpressCodeUsageCondition.AcceptForDropOff))
                {
                    RuleState.Add(ResourceName, new RuleState(Context.ExpressCodeUsageCondition) {RuleStateItems = {new ReceivingUnplannedDeliveriesNotAllowed()}});
                }
            }
            
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<ExpressCodesSearchRequest>
        {
            public ContextModel(ExpressCodesSearchRequest parent, IRule rule) : base(parent, rule)
            {
            }
            public IRepository<Olma.ExpressCode> ExpressCodeRepository { get; protected internal set; }
            public IMapper Mapper { get; protected internal set; }
            public RulesBundle Rules { get; protected internal set; }
            private string DigitalCode => Parent.ExpressCode;
            private ExpressCodesSearchRequest ExpressCodesSearchRequest => Parent;

            public Olma.Partner OlmaRecipientPartner { get; set; }
            public Contracts.Models.ExpressCode ExpressCodeResult => GetExpressCodeByDigitalCode();
            public Contracts.Models.ExpressCode ExpressCode { get; set; }
            public Olma.ExpressCodeUsageCondition ExpressCodeUsageCondition => GetExpressCodeUsageConditions();
            public IRepository<Olma.ExpressCodeUsageCondition> ExpressCodeUsageConditionRepository { get; protected internal set; }

            private Olma.ExpressCodeUsageCondition GetExpressCodeUsageConditions()
            {
                var olmaExpressCodeUsageCondition =
                    ExpressCodeUsageConditionRepository.FindByCondition(ex =>
                        ex.PostingAccountId == Parent.IssuingPostingAccountId).AsNoTracking().SingleOrDefault();
                
                return olmaExpressCodeUsageCondition;
            }

            private Contracts.Models.ExpressCode GetExpressCodeByDigitalCode()
            {
                if (ExpressCodesSearchRequest.PrintType != PrintType.LoadCarrierReceiptDelivery
                && ExpressCodesSearchRequest.PrintType != PrintType.VoucherCommon) return null;
                
                var olmaExpressCode =
                    ExpressCodeRepository.FindByCondition(x => x.DigitalCode == DigitalCode)
                        .Include(x => x.PartnerPresets)
                        .ThenInclude(p => p.Partner).ThenInclude(a => a.DefaultAddress)
                        .Include(x => x.PartnerPresets)
                        .ThenInclude(p => p.Partner).ThenInclude(pa => pa.DefaultPostingAccount).ThenInclude(ecu => ecu.ExpressCodeCondition)
                        .Include(x => x.PostingAccountPreset)
                        .IgnoreQueryFilters()
                        .Where(ec => !ec.IsDeleted)
                        .AsNoTracking().SingleOrDefault();

                var recipientPartnerPreset =
                    olmaExpressCode?.PartnerPresets.SingleOrDefault(pp => pp.Type == Olma.ExpressCodePresetType.Recipient);
                
                if (recipientPartnerPreset == null) return null;

                OlmaRecipientPartner = recipientPartnerPreset.Partner;
                
                var expressCode = Mapper.Map<Contracts.Models.ExpressCode>(olmaExpressCode);

                if (ExpressCodesSearchRequest.PrintType == PrintType.VoucherCommon)
                {
                    expressCode.VoucherPresets = new VoucherExpressCodeDetails();
                    foreach (var preset in olmaExpressCode.PartnerPresets)
                        switch (preset.Type)
                        {
                            case Olma.ExpressCodePresetType.Recipient:
                                expressCode.VoucherPresets.Recipient =
                                    Mapper.Map<Contracts.Models.CustomerPartner>(preset.Partner);
                                break;
                            case Olma.ExpressCodePresetType.Supplier:
                                expressCode.VoucherPresets.Supplier =
                                    Mapper.Map<Contracts.Models.CustomerPartner>(preset.Partner);
                                break;
                            case Olma.ExpressCodePresetType.Shipper:
                                expressCode.VoucherPresets.Shipper =
                                    Mapper.Map<Contracts.Models.CustomerPartner>(preset.Partner);
                                break;
                            case Olma.ExpressCodePresetType.SubShipper:
                                expressCode.VoucherPresets.SubShipper =
                                    Mapper.Map<Contracts.Models.CustomerPartner>(preset.Partner);
                                break;
                        }
                }
                else
                {
                    if (OlmaRecipientPartner.DefaultPostingAccountId != null)
                        expressCode.LoadCarrierReceiptPreset = new LoadCarrierReceiptPreset
                        {
                            PostingAccountId = OlmaRecipientPartner.DefaultPostingAccountId.Value
                        };
                }

                return expressCode;

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