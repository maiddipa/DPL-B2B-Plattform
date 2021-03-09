using System;
using System.Linq;
using AutoMapper;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.ExpressCode;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal.Models.Lms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Z.EntityFramework.Plus;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.ExpressCode.Shared
{
    public class OrderLoadValidRule : BaseValidationWithServiceProviderRule<OrderLoadValidRule,
        OrderLoadValidRule.ContextModel>
    {
        public OrderLoadValidRule(ExpressCodesSearchRequest request, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            Context.PostingAccountRepository = ServiceProvider.GetService<IRepository<Olma.PostingAccount>>();
            Context.LoadCarrierRepository = ServiceProvider.GetService<IRepository<Olma.LoadCarrier>>();
            Context.LmsAvail2deliRepository = ServiceProvider.GetService<IRepository<LmsAvail2deli>>();
            Context.LmsQuali2palletRepository = ServiceProvider.GetService<IRepository<LmsQuali2pallet>>();
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
                RuleState.Add(ResourceName,
                    new RuleState(Context.ExpressCode) {RuleStateItems = {new DigitalCodeInvalid()}});
            }

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<ExpressCodesSearchRequest>
        {
            public ContextModel(ExpressCodesSearchRequest parent, IRule rule) : base(parent, rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }
            private string DigitalCode => Parent.ExpressCode;
            private PrintType? PrintType => Parent.PrintType;
            public Contracts.Models.ExpressCode ExpressCodeResult => GetExpressCodeByDigitalCode();
            public Contracts.Models.ExpressCode ExpressCode { get; set; }

            public IRepository<Olma.LoadCarrier> LoadCarrierRepository { get; protected internal set; }
            public IRepository<Olma.PostingAccount> PostingAccountRepository { get; protected internal set; }
            public IRepository<LmsAvail2deli> LmsAvail2deliRepository { get; protected internal set; }
            public IRepository<LmsQuali2pallet> LmsQuali2palletRepository { get; protected internal set; }
            public IMapper Mapper { get; protected internal set; }

            private Contracts.Models.ExpressCode GetExpressCodeByDigitalCode()
            {
                var avail2deli = LmsAvail2deliRepository.FindAll()
                    .Where(ad => ad.ExpressCode == DigitalCode 
                        && ad.State == 2
                        && ad.FrachtpapiereErstellt.Value
                        && ad.DeletionDate == null
                                 )
                    .Include(ad => ad.Availability)
                    .Include(ad => ad.Delivery).AsNoTracking().FirstOrDefault();

                if (avail2deli == null) return null;

                var loadCarriers = LoadCarrierRepository.FindAll()
                    .Include(lc => lc.Quality).ThenInclude(q => q.Mappings)
                    .Include(lc => lc.Type)
                    .FromCache().ToList();

                var loadCarrier = loadCarriers
                    .SingleOrDefault(lc => lc.Type.RefLmsLoadCarrierTypeId == avail2deli.PalletTypeId && lc.Quality.Mappings.SingleOrDefault(qm => qm.RefLmsQualityId == avail2deli.QualityId) != null);

                Olma.LoadCarrier baseLoadCarrier = null;
                if (avail2deli.BaseQuantity != null && avail2deli.BaseQuantity != 0)
                {
                    baseLoadCarrier = loadCarriers
                        .SingleOrDefault(lc => lc.Type.RefLmsLoadCarrierTypeId == avail2deli.BasePalletTypeId && lc.Quality.Mappings.SingleOrDefault(qm => qm.RefLmsQualityId == avail2deli.BaseQualityId) != null);
                }

                if (loadCarrier == null) return null;

                int? ltmsAccountId;

                switch (PrintType)
                {
                    case B2b.Common.Enumerations.PrintType.LoadCarrierReceiptDelivery:
                        ltmsAccountId = avail2deli.Delivery.LtmsAccountId;
                        break;
                    case B2b.Common.Enumerations.PrintType.LoadCarrierReceiptPickup:
                        ltmsAccountId = avail2deli.Availability.LtmsAccountId;
                        break;
                    default:
                        return null;
                }

                var postingAccount = PostingAccountRepository.FindAll()
                    .Where(pa => pa.RefLtmsAccountId == ltmsAccountId)
                    .AsNoTracking().FirstOrDefault();

                if (postingAccount == null) return null;

                var expressCode = new Contracts.Models.ExpressCode
                {
                    DigitalCode = DigitalCode,
                    LoadCarrierReceiptPreset = new LoadCarrierReceiptPreset
                    {
                        DeliveryNoteNumber = avail2deli.Delivery?.DeliveryNoteNo,
                        PickupNoteNumber = avail2deli.Availability?.ContractNo,
                        RefLmsBusinessTypeId = avail2deli.Delivery?.BusinessTypeId,
                        LoadCarrierId = loadCarrier.Id,
                        LoadCarrierQuantity = avail2deli.Quantity,
                        BaseLoadCarrierId = baseLoadCarrier?.Id,
                        BaseLoadCarrierQuantity = avail2deli.BaseQuantity,
                        PostingAccountId = postingAccount.Id,
                        Type = PrintType == B2b.Common.Enumerations.PrintType.LoadCarrierReceiptPickup
                        ? LoadCarrierReceiptType.Pickup
                        : LoadCarrierReceiptType.Delivery,
                        PlannedFulfillmentDateTime = avail2deli.LadeterminDatum,
                        RefLtmsTransactionRowGuid = avail2deli.RefLtmsTransactionRowGuid
                    }
                };
                
                return expressCode;
            }
        }

        /// <summary>
        ///     Bundles of rules
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