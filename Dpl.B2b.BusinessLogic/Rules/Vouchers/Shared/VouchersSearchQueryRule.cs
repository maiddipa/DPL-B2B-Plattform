using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;
using LinqKit;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Shared
{
    public class VouchersSearchQueryRule : BaseValidationWithServiceProviderRule<VouchersSearchQueryRule, VouchersSearchQueryRule.ContextModel>
    {
        public VouchersSearchQueryRule(VouchersSearchRequest context, IRule parentRule)
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

            Context.AuthData = ServiceProvider.GetService<IAuthorizationDataService>();
            Context.Mapper = ServiceProvider.GetService<IMapper>();
            Context.VoucherRepo= ServiceProvider.GetService<IRepository<Olma.Voucher>>();
            
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // No validation is needed

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

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

            public RulesBundle Rules { get; protected internal set; }
            

            public VouchersSearchRequest VouchersSearchRequest => Parent;

            public IRepository<Olma.Voucher> VoucherRepo { get; protected internal set; }

            public IMapper Mapper { get; protected internal set; }
            public IAuthorizationDataService AuthData { get; protected internal set; }

            public IQueryable<Olma.Voucher> VoucherSearchQuery => BuildVoucherSearchQuery();

            private IQueryable<Olma.Voucher> BuildVoucherSearchQuery()
            {
                var query = VoucherRepo
                    .FindAll()
                    .AsNoTracking()
                    .Where(i => i.IsDeleted == false);

                //var postingAccountIds = AuthData.GetPostingAccountIds().ToArray();

                if (VouchersSearchRequest.CustomerId?.Count > 0)
                {
                    var predicate = PredicateBuilder.New<Olma.Voucher>(v => VouchersSearchRequest.CustomerId.Contains(v.CustomerDivision.CustomerId));
                    predicate.Or(v => v.ReceivingCustomerId != null && VouchersSearchRequest.CustomerId.Contains(v.ReceivingCustomerId.Value));
                    query = query.Where(predicate);
                }

                if (VouchersSearchRequest.Type != null)
                {
                    query = query.Where(v => v.Type == VouchersSearchRequest.Type);
                }

                if (!string.IsNullOrEmpty(VouchersSearchRequest.DocumentNumber))
                {
                    query = query.Where(v => v.Document.Number.Contains(VouchersSearchRequest.DocumentNumber)); //TODO compare as SQL LIKE
                }

                if (VouchersSearchRequest.FromIssueDate != null)
                {
                    query = query.Where(v => v.Document.IssuedDateTime >= VouchersSearchRequest.FromIssueDate.Value.Date);
                }

                if (VouchersSearchRequest.ToIssueDate != null)
                {
                    query = query.Where(v => v.Document.IssuedDateTime <= VouchersSearchRequest.ToIssueDate.Value.Date);
                }

                if (VouchersSearchRequest.States != null)
                {
                    query = query.Where(i => VouchersSearchRequest.States.Contains(i.Status));
                }

                if (VouchersSearchRequest.ReasonTypes?.Count > 0)
                {
                    query = query.Where(i => VouchersSearchRequest.ReasonTypes.Contains(i.ReasonTypeId));
                }

                if (VouchersSearchRequest.LoadCarrierTypes?.Count > 0)
                {
                    query = query.Where(i => i.Positions.Any(p => VouchersSearchRequest.LoadCarrierTypes.Contains(p.LoadCarrier.TypeId)));
                }

                if (!string.IsNullOrEmpty(VouchersSearchRequest.Shipper))
                {
                    query = query.Where(v => v.Shipper != null && v.Shipper.CompanyName.Contains(VouchersSearchRequest.Shipper));
                }

                if (!string.IsNullOrEmpty(VouchersSearchRequest.SubShipper))
                {
                    query = query.Where(v => v.SubShipper != null && v.SubShipper.CompanyName.Contains(VouchersSearchRequest.SubShipper));
                }

                if (!string.IsNullOrEmpty(VouchersSearchRequest.Recipient))
                {
                    query = query.Where(v => v.Recipient != null && v.Recipient.CompanyName.Contains(VouchersSearchRequest.Recipient));
                }

                if (VouchersSearchRequest.RecipientType != null)
                {
                    query = query.Where(v => v.RecipientType == VouchersSearchRequest.RecipientType);
                }

                if (VouchersSearchRequest.ValidFrom != null)
                {
                    query = query.Where(v => v.ValidUntil >= VouchersSearchRequest.ValidFrom.Value.Date);
                }

                if (VouchersSearchRequest.ValidTo != null)
                {
                    query = query.Where(v => v.ValidUntil <= VouchersSearchRequest.ValidTo.Value.Date);
                }

                if (VouchersSearchRequest.QuantityFrom != null)
                {
                    query = query.Where(v => v.Positions.Any(p => p.LoadCarrierQuantity >= VouchersSearchRequest.QuantityFrom));
                }

                if (VouchersSearchRequest.QuantityTo != null)
                {
                    query = query.Where(v => v.Positions.Any(p => p.LoadCarrierQuantity <= VouchersSearchRequest.QuantityTo));
                }

                if (!string.IsNullOrEmpty(VouchersSearchRequest.Supplier))
                {
                    query = query.Where(v => v.Supplier != null && v.Supplier.CompanyName.Contains(VouchersSearchRequest.Supplier));
                }

                if (VouchersSearchRequest.HasDplNote.HasValue)
                {
                    query = query.Where(v => v.DplNotes.Any() == VouchersSearchRequest.HasDplNote.Value);
                }
                if (!string.IsNullOrEmpty(VouchersSearchRequest.CustomerReference))
                {
                    query = query.Where(v => v.CustomerReference.Contains(VouchersSearchRequest.CustomerReference));
                }

                return query;
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