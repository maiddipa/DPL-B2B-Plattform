using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.TransportOfferings.Shared;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.TransportOfferings;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.Search
{
    public class QueryTransportOfferingsRule : BaseValidationWithServiceProviderRule<QueryTransportOfferingsRule, QueryTransportOfferingsRule.ContextModel>
    {
        public QueryTransportOfferingsRule(MainRule.ContextModel context, IRule parentRule = null)
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
            
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // No need to validate

            Context.TransportOfferingsPaginationResult = CreateTransportOfferingPaginationResult();

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        private IPaginationResult<TransportOffering> CreateTransportOfferingPaginationResult()
        {
            var request = Context.Parent.Parent;
            var olmaTransportRepo = ServiceProvider.GetService<IRepository<Olma.Transport>>();
            var olmaTransportBidRepo = ServiceProvider.GetService<IRepository<Olma.TransportBid>>();
            var mapper = ServiceProvider.GetService<IMapper>();
            
            var divisionIds = Context.DivisionIds;

            var query = olmaTransportRepo.FindAll()
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(i => i.IsDeleted == false);

            if (request.DivisionId == 0)
            {
                throw new TransportOfferingServiceException("Division id needs to be specified.");
            }

            var authDivisionIds = Context.AuthDivisionIds;
            if (!authDivisionIds.Contains(request.DivisionId))
            {
                throw new UnauthorizedAccessException("At least one division id was provided the users does not have access to.");
            }

            // filter invalid localtion entries
            query = query.Where(i => i.OrderMatch.Demand.Order.LoadingLocation.Address.GeoLocation != null
                                     && i.OrderMatch.Supply.Order.LoadingLocation.Address.GeoLocation != null);

            #region where clause

            var secureBid = PredicateBuilder.New<Olma.TransportBid>(i => i.IsDeleted == false && divisionIds.Contains(i.Id));

            if (request.Status?.Count > 0)
            {
                var plannedOrConfirmed = new[] {Olma.TransportStatus.Planned, Olma.TransportStatus.Confirmed};
                var predicate = PredicateBuilder.New<Olma.Transport>(false);
                foreach (var status in request.Status)
                {
                    switch (status)
                    {
                        case TransportOfferingStatus.Available:
                            predicate = predicate.Or(i => i.Status == Olma.TransportStatus.Requested);
                            break;
                        case TransportOfferingStatus.Won:
                            predicate = predicate.Or(i => plannedOrConfirmed.Contains(i.Status) && i.WinningBid != null && divisionIds.Contains(i.WinningBid.DivisionId));
                            break;
                        case TransportOfferingStatus.Lost:
                            predicate = predicate.Or(i => plannedOrConfirmed.Contains(i.Status) && i.WinningBid != null && !divisionIds.Contains(i.WinningBid.DivisionId));
                            break;
                        case TransportOfferingStatus.Canceled:
                            predicate = predicate.Or(i => i.Status == Olma.TransportStatus.Canceled && i.WinningBid != null && !divisionIds.Contains(i.WinningBid.DivisionId));
                            break;
                        case TransportOfferingStatus.BidPlaced:
                            predicate = predicate.Or(i =>
                                i.Status == Olma.TransportStatus.Requested && i.Bids.Count(secureBid.Compile()) > 0 &&
                                i.Bids.Any(secureBid.And(b => b.Status == TransportBidStatus.Active).Compile()));
                            break;
                        case TransportOfferingStatus.Declined:
                            predicate = predicate.Or(i =>
                                i.Status == Olma.TransportStatus.Requested && i.WinningBid != null && i.WinningBid.Status == TransportBidStatus.Declined &&
                                divisionIds.Contains(i.WinningBid.DivisionId));
                            break;
                        case TransportOfferingStatus.Accepted:
                            predicate = predicate.Or(i =>
                                i.Status == Olma.TransportStatus.Planned && i.WinningBid != null && i.WinningBid.Status == TransportBidStatus.Accepted &&
                                divisionIds.Contains(i.WinningBid.DivisionId));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"Unknown status: {request.Status}");
                    }
                }

                query = query.Where(predicate);
            }

            if (request.LoadCarrierType?.Count > 0)
            {
                query = query.Where(i => request.LoadCarrierType.Contains(i.OrderMatch.LoadCarrier.TypeId));
            }

            if (request.SupplyEarliestFulfillmentDateFrom.HasValue && request.SupplyEarliestFulfillmentDateFrom.HasValue)
            {
                query = query.Where(i =>
                    request.SupplyEarliestFulfillmentDateFrom.Value.Date <= i.OrderMatch.Supply.Order.EarliestFulfillmentDateTime.Date &&
                    i.OrderMatch.Supply.Order.EarliestFulfillmentDateTime.Date <= request.SupplyEarliestFulfillmentDateFrom.Value.Date);
            }
            else
            {
                AddMessage(request.SupplyEarliestFulfillmentDateFrom.HasValue
                           || request.SupplyEarliestFulfillmentDateTo.HasValue,
                    RuleName,
                    new SupplyEarliestFulfillmentDateFromAndToRequired());
            }

            if (request.DemandLatestFulfillmentDateFrom.HasValue && request.DemandLatestFulfillmentDateTo.HasValue)
            {
                query = query.Where(i =>
                    request.DemandLatestFulfillmentDateFrom.Value.Date <= i.OrderMatch.Demand.Order.LatestFulfillmentDateTime.Date &&
                    i.OrderMatch.Supply.Order.EarliestFulfillmentDateTime.Date <= request.DemandLatestFulfillmentDateTo.Value.Date);
            }
            else
            {
                AddMessage(request.DemandLatestFulfillmentDateFrom.HasValue || request.DemandLatestFulfillmentDateTo.HasValue,
                    RuleName,
                    new DemandLatestFulfillmentDateFromAndToRequired());
            }

            if (request.SupplyPostalCode != null)
            {
                query = query.Where(i => i.OrderMatch.Supply.Order.LoadingLocation.Address.PostalCode.StartsWith(request.SupplyPostalCode));
            }

            if (request.DemandPostalCode != null)
            {
                query = query.Where(i => i.OrderMatch.Demand.Order.LoadingLocation.Address.PostalCode.StartsWith(request.DemandPostalCode));
            }

            // TODO discuss if its possible to search for distance from demand + supply at the same time
            // or only one at a time, if one at a time we need an enum if both we need all lat/long/radius fields twice
            // searching for both could be intersting as logitic partber wants to fill a hole in the schedule
            var transportOfferingsSearchRequestPoint = new Point(request.Lng, request.Lat) {SRID = 4326};
            if (false)
            {
                query = query.Where(i => i.OrderMatch.Demand.Order.LoadingLocation.Address.GeoLocation.Distance(transportOfferingsSearchRequestPoint) <= request.Radius);
            }

            if (request.StackHeightMin.HasValue && request.StackHeightMax.HasValue)
            {
                query = query.Where(i =>
                    request.StackHeightMin.Value <= i.OrderMatch.LoadCarrierStackHeight && i.OrderMatch.LoadCarrierStackHeight <= request.StackHeightMin.Value);
            }
            
            if (request.SupportsRearLoading)
            {
                query = query.Where(i => i.OrderMatch.SupportsJumboVehicles == request.SupportsRearLoading);
            }

            if (request.SupportsSideLoading)
            {
                query = query.Where(i => i.OrderMatch.SupportsSideLoading == request.SupportsSideLoading);
            }

            if (request.SupportsJumboVehicles)
            {
                query = query.Where(i => i.OrderMatch.SupportsJumboVehicles == request.SupportsJumboVehicles);
            }

            // TODO discuss what kinde of date filter is needed,
            // code below is copied from load carrier offerings for reference only
            //var predicate = PredicateBuilder.New<Olma.Transport>(false);
            //foreach (var date in request.Date)
            //{
            //    predicate = predicate.Or(i => i.OrderMatch.SupplyFulfillmentDateTime <= date && date <= i.OrderMatch.DemandFulfillmentDateTime);
            //}
            //query = query.Where(predicate);

            #endregion

            #region ordering

            switch (request.SortBy)
            {
                case TransportOfferingSortOption.SubmittedDate:
                    query = query.OrderBy(i => i.OrderMatch.CreatedAt, request.SortDirection);
                    break;
                case TransportOfferingSortOption.SupplyFulfillmentDate:
                    query = query.OrderBy(i => i.OrderMatch.Supply.Order.EarliestFulfillmentDateTime, request.SortDirection);
                    break;
                case TransportOfferingSortOption.DemandFulfillmentDate:
                    query = query.OrderBy(i => i.OrderMatch.Demand.Order.LatestFulfillmentDateTime, request.SortDirection);
                    break;
                case TransportOfferingSortOption.SupplyPostalCode:
                    query = query.OrderBy(i => i.OrderMatch.Supply.Order.LoadingLocation.Address.PostalCode, request.SortDirection);
                    break;
                case TransportOfferingSortOption.DemandPostalCode:
                    query = query.OrderBy(i => i.OrderMatch.Demand.Order.LoadingLocation.Address.PostalCode, request.SortDirection);
                    break;
                case TransportOfferingSortOption.Status:
                    query = query.OrderBy(i => i.Status, request.SortDirection);
                    break;
                case TransportOfferingSortOption.SupplyCountry:
                    query = query.OrderBy(i => i.OrderMatch.Supply.Order.LoadingLocation.Address.Country.Name, request.SortDirection);
                    break;
                case TransportOfferingSortOption.DemandCountry:
                    query = query.OrderBy(i => i.OrderMatch.Demand.Order.LoadingLocation.Address.Country.Name, request.SortDirection);
                    break;
                case TransportOfferingSortOption.Distance:
                    query = query.OrderBy(
                        i => i.OrderMatch.Demand.Order.LoadingLocation.Address.GeoLocation.Distance(i.OrderMatch.Supply.Order.LoadingLocation.Address.GeoLocation),
                        request.SortDirection);
                    break;
                case TransportOfferingSortOption.ReferenceNumber:
                default:
                    query = query.OrderBy(i => i.ReferenceNumber, request.SortDirection);
                    break;
            }

            #endregion

            var transportOfferingsSearchRequestDivisionIds = divisionIds;
            var projectedQuery = query.ProjectTo<TransportOffering>(
                mapper.ConfigurationProvider,
                new {transportOfferingsSearchRequestPoint, transportOfferingsSearchRequestDivisionIds});


            var transportIds = projectedQuery.Select(i => i.Id);

            var bidIds = olmaTransportBidRepo.FindAll()
                .AsNoTracking()
                .Where(i => transportIds.Contains(i.TransportId) && i.IsDeleted == false && divisionIds.Contains(i.DivisionId))
                .Select(i => i.Id)
                .ToHashSet();

            foreach (var transport in projectedQuery)
            {
                if (transport.WinningBid != null && !bidIds.Contains(transport.WinningBid.Id))
                {
                    transport.WinningBid = null;
                }

                transport.Bids = transport.Bids.Where(i => bidIds.Contains(i.Id)).ToList();

                var operatorRule = new GetOperatorRule<ContextModel, TransportOffering>(Context, c => transport);

                new MaskAddressIfNecessaryRule(operatorRule).Evaluate();
            }

            return projectedQuery.ToPaginationResult(request);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, QueryTransportOfferingsRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            
            public IPaginationResult<TransportOffering> TransportOfferingsPaginationResult { get; protected internal set; }
            
            public int[] DivisionIds => Parent.DivisionIds;
            public int[] AuthDivisionIds => Parent.AuthDivisionIds;
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