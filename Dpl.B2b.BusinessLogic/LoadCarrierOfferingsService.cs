using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExpress.DataProcessing;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Ltms = Dpl.B2b.Dal.Ltms;
using Lms = Dpl.B2b.Dal.Models.Lms;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class LoadCarrierOfferingsService : BaseService, ILoadCarrierOfferingsService
    {
        private readonly IRepository<Olma.LmsOrder> _lmsOrderGroupRepo;
        private readonly IServiceProvider _serviceProvider;

        public LoadCarrierOfferingsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IRepository<Olma.LmsOrder> lmsOrderGroupRepo
            ) : base(authData, mapper)
        {
            _serviceProvider = serviceProvider;
            _lmsOrderGroupRepo = lmsOrderGroupRepo;
        }


        public async Task<IWrappedResponse> Search(LoadCarrierOfferingsSearchRequest request)
        {
            var cmd = ServiceCommand<LoadCarrierOffering, Rules.LoadCarrierOfferings.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadCarrierOfferings.Search.MainRule(request))
                .Then(SearchAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> SearchAction(Rules.LoadCarrierOfferings.Search.MainRule rule)
        {
            return Ok(rule.Context.LoadCarrierOfferings);
        }
    }
}
