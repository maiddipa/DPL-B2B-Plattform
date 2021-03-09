using System;
using System.Linq;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.LoadCarrierReceipts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierReceipts.Create
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithAuthorizationSkill
    {
        private static readonly string BlacklistMessageId = new BlacklistMatch().Id;
        
        public MainRule(LoadCarrierReceiptsCreateRequest request, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
            ValidEvaluate += OnValidEvaluate;
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
                .Eval(Context.Rules.CustomerDivisionAuthorizationRule)
                .Eval(Context.Rules.DigitalCodePristine)    
                .Eval(Context.Rules.TruckDriverNameRequired)
                .Eval(Context.Rules.LicensePlateRequired)
                .Eval(Context.Rules.LicensePlateCountryIdRequired)
                .Eval(Context.Rules.PositionsValid)
                .Eval(Context.Rules.MoreInformationValid)
                .Eval(Context.Rules.BlacklistTerms);
            

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            // Add all Message from ruleResult
            MergeFromResult(ruleResult);
            
            // Add Message if ruleResult is not success
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            
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

        private void OnValidEvaluate(object sender, EvaluateInternalEventArgs evaluateInternalEventArgs)
        {
            var mapper = ServiceProvider.GetService<IMapper>();
            var request = Context.Request; 
            
            var documentTypesService = ServiceProvider.GetService<IDocumentTypesService>();
            var expressCodeRepository = ServiceProvider.GetService<IRepository<Olma.ExpressCode>>();
            var dbContext = ServiceProvider.GetService<Dal.OlmaDbContext>(); 
            var orderLoadRepository = ServiceProvider.GetService<IRepository<Olma.OrderLoad>>();
            
            var documentType = DocumentTypeEnum.LoadCarrierReceiptExchange;
            
            switch (request.Type)
            {
                case LoadCarrierReceiptType.Delivery:
                    documentType = DocumentTypeEnum.LoadCarrierReceiptDelivery;
                    break;
                case LoadCarrierReceiptType.Pickup:
                    documentType = DocumentTypeEnum.LoadCarrierReceiptPickup;
                    break;
                case LoadCarrierReceiptType.Exchange:
                    documentType = DocumentTypeEnum.LoadCarrierReceiptExchange;
                    break;
            }

            Context.DocumentType = documentType;
            Context.LoadCarrierReceipt = mapper.Map<Olma.LoadCarrierReceipt>(Context.Request);
            
            var typeId = documentTypesService.GetIdByDocumentType(documentType);
            
            Context.LoadCarrierReceipt.Document = new Olma.Document()
            {
                Number = null,  // assign by action
                TypeId = typeId,
                StateId = 1,
                CustomerDivisionId = request.CustomerDivisionId,
                IssuedDateTime = DateTime.UtcNow,
                LanguageId = request.PrintLanguageId,
            };
            Context.LoadCarrierReceipt.PostingAccountId = Context.CustomerDivision.PostingAccountId;
            Context.IsSupply = request.Type == LoadCarrierReceiptType.Pickup;
            Context.LoadCarrierReceipt.Trigger = LoadCarrierReceiptTrigger.Manual;

            Context.TargetRefLtmsAccountId = 12321; // Konto unbekannt

            if (Context.LoadCarrierReceipt.DigitalCode == null)
            {
                if (dbContext != null)
                    Context.TargetRefLtmsAccountId = dbContext.PostingAccounts
                        .Where(i => i.Id == request.TargetPostingAccountId)
                        // TODO SECURE usage fo ignore query filters
                        .IgnoreQueryFilters()
                        .Select(pa => pa.RefLtmsAccountId)
                        .Single();
            }
            else
            {
                var lmsAvail2deliRepository = ServiceProvider.GetService<IRepository<Olma.Lms.LmsAvail2deli>>();
                var avail2deli = lmsAvail2deliRepository.FindAll()
                    .Where(ad => ad.ExpressCode == Context.LoadCarrierReceipt.DigitalCode 
                                 && ad.State == 2
                                 && ad.FrachtpapiereErstellt.Value
                                 && ad.DeletionDate == null
                    )
                    .Include(ad => ad.Availability)
                    .Include(ad => ad.Delivery).AsNoTracking().FirstOrDefault();

                if (avail2deli != null)
                {
                    Context.LmsAvail2deli = avail2deli;
                    Context.RefLtmsTransactionId = avail2deli.RefLtmsTransactionRowGuid;
                    Context.LoadCarrierReceipt.TargetPostingAccount = null;
                    Context.IsSelfService = avail2deli.Lieferkategorie == (int) RefLmsDeliveryCategory.SelfPickup;
                    Context.LoadCarrierReceipt.Trigger = LoadCarrierReceiptTrigger.OrderMatch;

                    switch (request.Type)
                    {
                        case LoadCarrierReceiptType.Delivery:
                            if (avail2deli.Availability.LtmsAccountId != null)
                                Context.TargetRefLtmsAccountId = avail2deli.Availability.LtmsAccountId.Value;
                            break;
                        case LoadCarrierReceiptType.Pickup:
                            if (avail2deli.Delivery.LtmsAccountId != null)
                                Context.TargetRefLtmsAccountId = avail2deli.Delivery.LtmsAccountId.Value;
                            break;
                    }

                    var orderLoad = orderLoadRepository
                        .FindByCondition(i => i.Order.DivisionId == request.CustomerDivisionId
                                              && (i.Order.Type == OrderType.Supply
                                                  ? i.SupplyOrderMatch.DigitalCode ==
                                                    Context.LoadCarrierReceipt.DigitalCode
                                                  : i.DemandOrderMatch.DigitalCode ==
                                                    Context.LoadCarrierReceipt.DigitalCode))
                        .Include(o => o.Detail)
                        .Include(o => o.Order)
                        .Include(o => o.SupplyOrderMatch).ThenInclude(sm => sm.Demand).ThenInclude(d => d.Detail)
                        .ThenInclude(lc => lc.LoadCarrierReceipt).ThenInclude(dt => dt.PostingRequests)
                        .Include(o => o.DemandOrderMatch).ThenInclude(sm => sm.Supply).ThenInclude(d => d.Detail)
                        .ThenInclude(lc => lc.LoadCarrierReceipt).ThenInclude(dt => dt.PostingRequests)
                        .Include(o => o.Order.PostingAccount)
                        .IgnoreQueryFilters()
                        .SingleOrDefault();
                    
                    if (orderLoad == null) return;
                    orderLoad.Detail.Status = OrderLoadStatus.Fulfilled;
                    orderLoad.Detail.ActualFulfillmentDateTime = DateTime.UtcNow;

                    Context.OrderLoad = orderLoad;
                    Context.LoadCarrierReceipt.OrderLoadDetailId = orderLoad.Detail.Id;
                    Context.IsSupply = orderLoad.Order.Type == OrderType.Supply;

                }
                else
                {
                    var expressCode = expressCodeRepository?.FindAll()
                        .Where(c => c.DigitalCode == Context.LoadCarrierReceipt.DigitalCode)
                        .Include(c => c.PartnerPresets)
                        .ThenInclude(p => p.Partner)
                        .ThenInclude(pa => pa.DefaultPostingAccount).Where(p => !p.IsDeleted).IgnoreQueryFilters()
                        .SingleOrDefault();
                    
                    var recipientPartner = expressCode?.PartnerPresets.SingleOrDefault(pp => pp.Type == Olma.ExpressCodePresetType.Recipient);
                    if (recipientPartner != null)
                        Context.TargetRefLtmsAccountId =
                            recipientPartner.Partner.DefaultPostingAccount.RefLtmsAccountId;
                }
            }
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<LoadCarrierReceiptsCreateRequest>
        {
            public ContextModel(LoadCarrierReceiptsCreateRequest parent, MainRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            
            public LoadCarrierReceiptsCreateRequest Request => Parent;
            
            public DocumentTypeEnum DocumentType { get; protected internal set; }
            
            public Olma.LoadCarrierReceipt LoadCarrierReceipt { get; protected internal set; }
            public Olma.OrderLoad OrderLoad { get; protected internal set; }
            public Olma.Lms.LmsAvail2deli LmsAvail2deli { get; protected internal set; }
            
            public int TargetRefLtmsAccountId { get; protected internal set; }
            public Guid? RefLtmsTransactionId { get; protected internal set; }
            public bool? IsSelfService { get; protected internal set; }

            public Olma.CustomerDivision CustomerDivision => Rules.CustomerDivisionAuthorizationRule.Context[0];
            
            public bool IsSupply { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, MainRule rule)
            {
                CustomerDivisionAuthorizationRule = new ResourceAuthorizationRule<
                    Olma.CustomerDivision,
                    CanCreateLoadCarrierReceiptRequirement,
                    DivisionPermissionResource>(context.Request.CustomerDivisionId, rule)
                    .Include(customerDivision=>customerDivision.PostingAccount);
                
                LicensePlateCountryIdRequired = new ValidOperatorRule<ContextModel>(context,
                    model => model.Request.LicensePlateCountryId > 0,
                    new NotAllowedByRule(),
                    parentRule: rule,
                    ruleName: nameof(LicensePlateCountryIdRequired));
                
                TruckDriverNameRequired = new ValidOperatorRule<ContextModel>(context,
                    model => !string.IsNullOrEmpty(model.Request.TruckDriverName),
                    new NotAllowedByRule(),
                    parentRule: rule,
                    ruleName: nameof(TruckDriverNameRequired));

                LicensePlateRequired = new ValidOperatorRule<ContextModel>(context,
                    model => !string.IsNullOrEmpty(model.Request.LicensePlate),
                    new NotAllowedByRule(),
                    parentRule: rule,
                    ruleName: nameof(LicensePlateRequired));

                PositionsValid = new ValidOperatorRule<ContextModel>(context,
                    model =>
                        model.Request.Positions.Any()
                        && model.Request.Positions.All(p => p.Quantity > 0),
                    new NotAllowedByRule(),
                    parentRule: rule,
                    ruleName: nameof(PositionsValid));
                
                MoreInformationValid = new ValidOperatorRule<ContextModel>(context,
                    model =>
                    {
                        if (context.Request.DigitalCode != null) 
                            return true;
                        
                        switch (context.Request.Type)
                        {
                            case LoadCarrierReceiptType.Delivery:
                                return !string.IsNullOrEmpty(model.Request.DeliveryNoteNumber);
                            case LoadCarrierReceiptType.Pickup:
                                return !string.IsNullOrEmpty(model.Request.PickUpNoteNumber);
                            case LoadCarrierReceiptType.Exchange: 
                            default:
                                return true;
                        }
                    },
                    new NotAllowedByRule(),
                    parentRule: rule,
                    ruleName: nameof(MoreInformationValid));

                DigitalCodePristine = new ValidOperatorRule<ContextModel>(
                    condition: ctx => !string.IsNullOrWhiteSpace(ctx.Request.DigitalCode),
                    context,
                    model =>
                    {
                        var expressCodeRepository = rule.ServiceProvider.GetService<IRepository<Olma.ExpressCode>>();
                        var expressCodeExists = expressCodeRepository.FindAll()
                            .Any(code => code.DigitalCode == context.Request.DigitalCode);

                        if (expressCodeExists)
                        {
                            return true;
                        }

                        var repository = rule.ServiceProvider.GetService<IRepository<Olma.LoadCarrierReceipt>>();
                        var exists = repository
                            .Exists(loadCarrierReceipt => loadCarrierReceipt.DigitalCode == context.Request.DigitalCode
                                                          && loadCarrierReceipt.Type == context.Request.Type
                                                          && !loadCarrierReceipt.Document.State.IsCanceled);

                        return !exists;
                    },
                    new DigitalCodeAlreadyUsed(),
                    parentRule: rule,
                    ruleName: nameof(DigitalCodePristine));
                
                BlacklistTerms=new BlacklistTermsRule(context, rule);
            }

            public readonly ResourceAuthorizationRule<
                Olma.CustomerDivision, 
                CanCreateLoadCarrierReceiptRequirement, 
                DivisionPermissionResource> CustomerDivisionAuthorizationRule;
            public readonly ValidOperatorRule<ContextModel> LicensePlateCountryIdRequired;
            public readonly ValidOperatorRule<ContextModel> TruckDriverNameRequired;
            public readonly ValidOperatorRule<ContextModel> LicensePlateRequired;            
            public readonly ValidOperatorRule<ContextModel> PositionsValid;
            public readonly ValidOperatorRule<ContextModel> MoreInformationValid;
            public readonly ValidOperatorRule<ContextModel> DigitalCodePristine;
            public readonly BlacklistTermsRule BlacklistTerms;
        }

        #endregion

        public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (
            Context.Rules.CustomerDivisionAuthorizationRule.Authorization.AuthorizationRule,
            Context.Rules.CustomerDivisionAuthorizationRule.Authorization.ResourceRule);
    }
}