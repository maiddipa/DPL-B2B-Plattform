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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Search
{
    public class VouchersValidRule : BaseValidationWithServiceProviderRule<VouchersValidRule, VouchersValidRule.ContextModel>
    {
        public VouchersValidRule(MainRule.ContextModel context, IRule parentRule)
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
            Context.CustomerDivisionRepo = ServiceProvider.GetService<IRepository<Olma.CustomerDivision>>();
            Context.CustomerPartnerRepo = ServiceProvider.GetService<IRepository<Olma.CustomerPartner>>();
            
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // No need to validate
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            var query = Context.VoucherSearchQuery;
            
            query = OrderingQuery(Context.VouchersSearchRequest, query);

            var projectedQuery = query.ProjectTo<Contracts.Models.Voucher>(Context.Mapper.ConfigurationProvider);

            var paginationResult = projectedQuery.ToPaginationResult(Context.VouchersSearchRequest);

            Fetch(paginationResult);

            Context.PaginationResult = paginationResult;
            
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

          /// <summary>
        /// we need to fetch partners separately, because the Queryfilters in Olma DbContext
        /// would not allow to get vouchers if Recipient,Shipper,SubShipper or Supplier
        /// are not in CustomerPartnerDirector of the actual user
        /// </summary>
        /// <param name="result"></param>
        private void Fetch(IPaginationResult<Voucher> result)
        {
            var vouchers = result.Data.ToList();
            var recipients = GetPartnerDict(vouchers.Select(c => c.RecipientId).Distinct());
            var suppliers = GetPartnerDict(vouchers.Where(c => c.SupplierId != null).Select(c => (int) c.SupplierId).Distinct());
            var shippers = GetPartnerDict(vouchers.Where(c => c.ShipperId != null).Select(c => (int) c.ShipperId).Distinct());
            var subShippers = GetPartnerDict(vouchers.Where(c => c.SubShipperId != null).Select(c => (int) c.SubShipperId).Distinct());

            var issuers = GetIssuerDict(vouchers.Select(i => i.DivisionId).Distinct());
            var issuerCustomers = GetIssuerCustomerDict(vouchers.Select(i => i.DivisionId).Distinct());

            foreach (var voucher in vouchers)
            {
                voucher.IssuerCompanyName = issuerCustomers[voucher.DivisionId].Name;
                voucher.DivisionName = issuers[voucher.DivisionId].Name;
                voucher.Recipient = recipients[voucher.RecipientId]?.CompanyName;
                if (voucher.SupplierId != null)
                {
                    voucher.Supplier = suppliers[voucher.SupplierId.Value]?.CompanyName;
                }

                if (voucher.ShipperId != null)
                {
                    voucher.Shipper = shippers[voucher.ShipperId.Value]?.CompanyName;
                }

                if (voucher.SubShipperId != null)
                {
                    voucher.SubShipper = subShippers[voucher.SubShipperId.Value]?.CompanyName;
                }
            }
        }

        private static IQueryable<Olma.Voucher> OrderingQuery(VouchersSearchRequest request, IQueryable<Olma.Voucher> query)
        {
            switch (request.SortBy)
            {
                case VouchersSearchRequestSortOptions.IssuedBy:
                    query = query.OrderBy(i => i.CustomerDivision.Name, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.IssuanceDate:
                    // HACK Sorting - Using CreatedAt as we dont have field for IssuedAt
                    query = query.OrderBy(i => i.CreatedAt, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.ValidUntil:
                    query = query.OrderBy(i => i.ValidUntil, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.RecipientType:
                    // HACK Sorting - Enum Value vs Localized Name RecipientType
                    query = query.OrderBy(i => i.RecipientType, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.Recipient:
                    query = query.OrderBy(i => i.Recipient.CompanyName, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.Reason:
                    // HACK Sorting - Not Localized VoucherReasonType
                    query = query.OrderBy(i => i.ReasonType.Name, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.Type:
                    // HACK Sorting - Enum Value vs Localized Name VoucherType
                    query = query.OrderBy(i => i.Type, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.Status:
                    // HACK Sorting - Enum Value vs Localized Name VoucherStatus
                    query = query.OrderBy(i => i.Status, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.Quantity:
                    query = query.OrderBy(o => o.Positions.Where(p => p.LoadCarrier.TypeId == request.LoadCarrierTypes.First()).Sum(p => p.LoadCarrierQuantity), request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.DocumentNumber:
                    query = query.OrderBy(i => i.Document.Number, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.Supplier:
                    query = query.OrderBy(i => i.Supplier.CompanyName, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.Shipper:
                    query = query.OrderBy(i => i.Shipper.CompanyName, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.SubShipper:
                    query = query.OrderBy(i => i.SubShipper.CompanyName, request.SortDirection);
                    break;
                case VouchersSearchRequestSortOptions.HasDplNote:
                    query = query.OrderBy(i => i.DplNotes.Any());
                    break;
                case VouchersSearchRequestSortOptions.CustomerReference:
                    query = query.OrderBy(i => i.CustomerReference, request.SortDirection);
                    break;
                default:
                    query = query.OrderBy(i => i.CreatedAt, System.ComponentModel.ListSortDirection.Descending);
                    break;
            }

            return query;
        }

        //TODO Implement logic to catch the 3 different Use Cases for Issuers, Recipients and Suppliers Viewpoints
        private IDictionary<int, Contracts.Models.CustomerPartner> GetPartnerDict(IEnumerable<int> partnerIds)
        {
            var partners = Context.CustomerPartnerRepo.FindAll()
                .IgnoreQueryFilters() //HACK to allways get Results for the demo
                .AsNoTracking()
                .Where(p => partnerIds.Contains(p.Id) && !p.IsDeleted)
                .ProjectTo<Contracts.Models.CustomerPartner>(Context.Mapper.ConfigurationProvider);
            return partners.ToDictionary(i => i.Id, i => i);
        }

        private IDictionary<int, Contracts.Models.CustomerDivision> GetIssuerDict(IEnumerable<int> issuerIds)
        {
            var partners = Context.CustomerDivisionRepo.FindAll()
                .IgnoreQueryFilters() //HACK to allways get Results for the demo
                .AsNoTracking()
                .Where(p => issuerIds.Contains(p.Id) && !p.IsDeleted)
                .ProjectTo<Contracts.Models.CustomerDivision>(Context.Mapper.ConfigurationProvider);
            return partners.ToDictionary(i => i.Id, i => i);
        }

        private IDictionary<int, Contracts.Models.Customer> GetIssuerCustomerDict(IEnumerable<int> issuerIds)
        {
            var partners = Context.CustomerDivisionRepo.FindAll()
                .IgnoreQueryFilters() //HACK to allways get Results for the demo
                .AsNoTracking()
                .Where(p => issuerIds.Contains(p.Id) && !p.IsDeleted)
                .Select(d => new
                {
                    d.Id,
                    Customer = new Contracts.Models.Customer() {Id = d.CustomerId, Name = d.Customer.Name}
                });
            return partners.ToDictionary(i => i.Id, i => i.Customer);
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
            
            public IPaginationResult<Voucher> PaginationResult { get; protected internal set; }
            
            public IRepository<Olma.CustomerDivision> CustomerDivisionRepo { get; protected internal set; }
            public IRepository<Olma.CustomerPartner> CustomerPartnerRepo { get; protected internal set; }
            
            public IMapper Mapper { get; protected internal set; }
            public IAuthorizationDataService AuthData { get; protected internal set; }

            public VouchersSearchRequest VouchersSearchRequest => Parent.VouchersSearchRequest;
            
            public IQueryable<Olma.Voucher> VoucherSearchQuery => Parent.Rules.VouchersSearchQueryRule.Context.VoucherSearchQuery;
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