using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.LoadCarrierOfferings;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;
using Z.EntityFramework.Plus;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierOfferings.Search
{
    public class LoadCarrierOfferingValidRule : BaseValidationWithServiceProviderRule<LoadCarrierOfferingValidRule, LoadCarrierOfferingValidRule.ContextModel>
    {
        public LoadCarrierOfferingValidRule(MainRule.ContextModel context, IRule parentRule)
        {
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new LoadCarrierOfferingValid();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            // Assign all values for Context
            Context.ServiceProvider = ServiceProvider;
            Context.Mapper = ServiceProvider.GetService<IMapper>();
            Context.LmsOrderGroupRepo = ServiceProvider.GetService<IRepository<Olma.LmsOrder>>();
            Context.OlmaLoadCarrierRepo = ServiceProvider.GetService<IRepository<Olma.LoadCarrier>>();
            Context.AuthData = ServiceProvider.GetService<IAuthorizationDataService>();

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create();

            // Assign rules to the Evaluator
            rulesEvaluator
                .Eval(Context.Rules.LongitudeValidRule)
                .Eval(Context.Rules.LatitudeValidRule)
                .Eval(Context.Rules.RadiusValidRule)
                .Eval(Context.Rules.MinStackHighValidRule)
                .Eval(Context.Rules.MaxStackHighValidRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
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

            public IRepository<Olma.LmsOrder> LmsOrderGroupRepo { get; protected internal set; }
            public IRepository<Olma.LoadCarrier> OlmaLoadCarrierRepo { get; protected internal set; }
            public IAuthorizationDataService AuthData { get; protected internal set; }

            public IEnumerable<LoadCarrierOffering> LoadCarrierOfferings => ListLoadCarrierOffering();

            private IQueryable<Olma.LmsOrder> BuildOfferingsDetailsQuery()
            {
                var lmsQuery = LmsOrderGroupRepo.FindAll()
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Include(i => i.LoadingLocation).ThenInclude(i => i.Address)
                    .Include(i => i.LoadingLocation.BusinessHours)
                    .AsQueryable();

                // filter invalid entries
                //lmsQuery = lmsQuery.Where(i => i.LoadingLocationId.HasValue && i.LoadingLocation.Address.GeoLocation != null);
                //lmsQuery = lmsQuery.Where(i => i.FromDate.HasValue && i.UntilDate.HasValue);

                #region where clause

                lmsQuery = lmsQuery.Where(i => i.ShowOnline == true);
                lmsQuery = lmsQuery.Where(i => i.IsAvailable == true);

                // when sombody wants to drop (supply) of we need to find demand and vice versa
                var orderType = Type == OrderType.Demand ? OrderType.Supply : OrderType.Demand;
                lmsQuery = lmsQuery.Where(i => i.Type == orderType);

                // filter for orders of other organizations
                var postingAccountIds = AuthData.GetPostingAccountIds();
                lmsQuery = lmsQuery.Where(i =>
                    !i.LoadingLocation.CustomerDivision.PostingAccountId.HasValue || !postingAccountIds.Contains(i.LoadingLocation.CustomerDivision.PostingAccountId.Value));

                lmsQuery = lmsQuery.Where(i => i.SupportsPartialMatching == true 
                    ? i.AvailableQuantity >= CalculatedLoadCarrierQuantity 
                    : i.AvailableQuantity == CalculatedLoadCarrierQuantity
                );

                // TODO: REMOVE after discussion
                // selbstransporte sind wenns ie im LMS ankommen schon nicht mehr verfügbar
                // only return orders where opposite side wants a transport handled for them
                // query = query.Where(i => i.TransportType == OrderTransportType.ProvidedByOthers);


                //// TODO think about if we actually need this as the view does a join with group names or null
                //// which filters out any rows that are blocked to persons which is the "not available" cireteria currently
                //// Geblockt (boolean)
                //// Geblockt für´group LMS_Gruppe => Gruppenname
                //// GeblocktFür
                //// lmsQuery = lmsQuery.Where(i => !i.Geblockt.HasValue || !i.Geblockt.Value);

                lmsQuery = lmsQuery.Where(i => i.LoadCarrierId == LoadCarrierId);

                // Note: review null value in list hack null = -99999999 as null cannot be provided as value in a list, maybe change to int min or so
                var baseLoadCarrierIds = BaseLoadCarrierId.Select(i => i == -99999999 ? null : i).ToList();
                lmsQuery = lmsQuery.Where(i => baseLoadCarrierIds.Contains(i.BaseLoadCarrierId));

                // filter by radius
                var loadCarrierOfferingsSearchRequestPoint = new Point(Lng, Lat) { SRID = 4326 };
                lmsQuery = lmsQuery.Where(i => i.LoadingLocation.Address.GeoLocation.Distance(loadCarrierOfferingsSearchRequestPoint) <= Radius);

                // filter by stackheight
                lmsQuery = lmsQuery.Where(i => i.StackHeightMin <= StackHeightMax && StackHeightMin <= i.StackHeightMax);

                if (SupportsRearLoading)
                {
                    lmsQuery = lmsQuery.Where(i => i.SupportsRearLoading == SupportsRearLoading);
                }

                if (SupportsSideLoading)
                {
                    lmsQuery = lmsQuery.Where(i => i.SupportsSideLoading == SupportsSideLoading);
                }

                if (SupportsJumboVehicles)
                {
                    lmsQuery = lmsQuery.Where(i => i.SupportsJumboVehicles == SupportsJumboVehicles);
                }

                var loadCarrierOfferingsSearchRequestDaysOfWeek = Date.Select(i => i.DayOfWeek).ToArray();
                lmsQuery = lmsQuery.Where(i => i.LoadingLocation.BusinessHours.Any(bh => loadCarrierOfferingsSearchRequestDaysOfWeek.Contains(bh.DayOfWeek)));

                // filter dates
                lmsQuery = lmsQuery.WhereAny(Date, date => i => i.FromDate.Value.Date <= date.Date && date.Date <= i.UntilDate.Value.Date);

                #endregion

                #region ordering

                lmsQuery = lmsQuery.OrderBy(i => i.LoadingLocation.Address.GeoLocation.Distance(loadCarrierOfferingsSearchRequestPoint));

                #endregion#

                return lmsQuery;
            }

            private IEnumerable<LoadCarrierOffering> ListLoadCarrierOffering()
            {
                var offeringDetails = BuildOfferingsDetailsQuery().Take(TakeCount).ToList();

                var result = offeringDetails
                    .GroupBy(i => i.LoadingLocationId)
                    .Select(i =>
                    {
                        var loadingLocation = i.First().LoadingLocation;
                        return new LoadCarrierOffering()
                        {
                            Address = Mapper.Map<Contracts.Models.Address>(loadingLocation.Address),
                            BusinessHours = Mapper.Map<IEnumerable<BusinessHours>>(loadingLocation.BusinessHours),
                            Lng = loadingLocation.Address.GeoLocation.X,
                            Lat = loadingLocation.Address.GeoLocation.Y,
                            // distance returns a weird unit 0.027 = 2.7km
                            // when using distance emthod inside sql server it returns meters
                            // since we use meters everwhere we need to convert it to meters
                            Distance = Math.Round(loadingLocation.Address.GeoLocation.Distance(LoadCarrierOfferingsSearchRequestPoint) * 100000, 0),
                            Details = Mapper.Map<IEnumerable<LoadCarrierOfferingDetail>>(i).Select(i =>
                            {
                                i.AvailabilityFrom = i.AvailabilityFrom;
                                i.AvailabilityTo = i.AvailabilityTo;
                                return i;
                            })
                        };
                    });

                return result;
            }

            private int GetLoadCarrierQuantityPerEur(int loadCarrierId)
            {
                var loadCarrier = OlmaLoadCarrierRepo.FindAll()
                    .Where(i => i.Id == loadCarrierId)
                    .Include(i => i.Type).FromCache().SingleOrDefault();

                return loadCarrier?.Type.QuantityPerEur ?? 1;
            }

            private LoadCarrierOfferingsSearchRequest Request => Parent.Parent;
            public int QuantityPerEur { get; set; }

            public Point LoadCarrierOfferingsSearchRequestPoint => new Point(Lng, Lat) { SRID = 4326 };

            public double Lng => Request.Lng;

            public double Lat => Request.Lat;

            public int LoadCarrierId => Request.LoadCarrierId;
            public OrderQuantityType QuantityType => Request.QuantityType;

            // HACK centralize cacluation of load to load carrier quantity

            public int CalculatedLoadCarrierQuantity
            {
                get
                {
                    if (Request.QuantityType == OrderQuantityType.Load)
                    {
                        return 1 * 33 * Request.StackHeightMax * GetLoadCarrierQuantityPerEur(Request.LoadCarrierId);
                    }

                    return Request.LoadCarrierQuantity;
                }
            }

            public int LoadCarrierQuantity => Request.LoadCarrierQuantity;

            public int PostingAccountId => Request.PostingAccountId;
            public int StackHeightMin => Request.StackHeightMin;
            public int StackHeightMax => Request.StackHeightMax;
            public List<DateTime> Date => Request.Date;

            public OrderType Type => Request.Type;

            public bool SupportsRearLoading => Request.SupportsRearLoading;
            public bool SupportsSideLoading => Request.SupportsSideLoading;
            public bool SupportsJumboVehicles => Request.SupportsJumboVehicles;

            public List<int?> BaseLoadCarrierId => Request.BaseLoadCarrierId;

            public int Radius => Request.Radius;

            public IMapper Mapper { get; protected internal set; }
            public IServiceProvider ServiceProvider { get; protected internal set; }

            public int TakeCount => Parent.TakeCount;

            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly LatitudeValidRule LatitudeValidRule;
            public readonly LongitudeValidRule LongitudeValidRule;
            public readonly StackHighValidRule MinStackHighValidRule;
            public readonly StackHighValidRule MaxStackHighValidRule;
            public readonly RadiusValidRule RadiusValidRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                LongitudeValidRule = new LongitudeValidRule(context.Lat, rule);
                LatitudeValidRule = new LatitudeValidRule(context.Lat, rule);
                MinStackHighValidRule = new StackHighValidRule(context.StackHeightMin, rule);
                MaxStackHighValidRule = new StackHighValidRule(context.StackHeightMax, rule);
                RadiusValidRule = new RadiusValidRule(context.Radius, rule);
            }
        }

        #endregion
    }
}