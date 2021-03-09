using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using Ltms = Dpl.B2b.Dal.Ltms;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class AccountingRecordsService : BaseService, IAccountingRecordsService
    {
        private readonly IRepository<Ltms.Bookings> _ltmsBookingsRepo;
        private readonly IRepository<Olma.PostingRequest> _olmaPostingRequestRepo;
        private readonly ILtmsReferenceLookupService _ltmsReferenceLookupService;
        private readonly IServiceProvider _serviceProvider;

        public AccountingRecordsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Ltms.Bookings> ltmsBookingsRepo,
            IRepository<Olma.PostingRequest> olmaPostingRequestRepo,
            ILtmsReferenceLookupService ltmsReferenceLookupService,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            //_olmaPostingRequestRepo = olmaPostingRequestRepo;
            _ltmsBookingsRepo = ltmsBookingsRepo;
            _olmaPostingRequestRepo = olmaPostingRequestRepo;
            _ltmsReferenceLookupService = ltmsReferenceLookupService;
            _serviceProvider = serviceProvider;
        }

        [Obsolete]
        public async Task<IWrappedResponse> GetById(int id)
        {
            var cmd = ServiceCommand<AccountingRecord, Rules.AccountingRecords.GetById.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.AccountingRecords.GetById.MainRule(id))
                .Then(GetByIdAction);
            
            return await cmd.Execute();   
        }
        
        [Obsolete]
        private async Task<IWrappedResponse> GetByIdAction(Rules.AccountingRecords.GetById.MainRule mainRule)
        {
            return await Task.FromResult(
                Ok(mainRule.Context.AccountingRecord) as IWrappedResponse);
        }

        public async Task<IWrappedResponse> Search(AccountingRecordsSearchRequest request)
        {
            var query = _ltmsBookingsRepo.FindAll().Where(b =>
                b.AccountId == request.RefLtmsAccountId
                && b.ArticleId == request.RefLtmsArticleId
                && !b.DeleteTime.HasValue
                && b.BookingTypeId != "POOL"
                && b.BookingTypeId != "STORNO")
                .AsNoTracking();

            switch (request.Status)
            {
                case AccountingRecordStatus.Canceled:
                    query = query.Where(b => b.Transaction.CancellationId != null);
                    break;
                case AccountingRecordStatus.Coordinated:
                    query = query.Where(b => b.Transaction.CancellationId == null
                                             && b.Matched);
                    break;
                case AccountingRecordStatus.InCoordination:
                    query = query.Where(b => b.Transaction.CancellationId == null
                                             && b.IncludeInBalance
                                             && !b.Matched
                                             && b.ReportBookings.Any(rb => rb.Report.DeleteTime == null
                                                                           && rb.Report.ReportStateId == "U"));
                    break;
                case AccountingRecordStatus.Uncoordinated:
                    query = query.Where(b => b.Transaction.CancellationId == null
                                             && b.IncludeInBalance
                                             && !b.Matched
                                             && b.ReportBookings.All(rb => rb.Report.DeleteTime == null
                                                                           && rb.Report.ReportStateId == "I"));
                    break;
                case AccountingRecordStatus.Provisional:
                    query = query.Where(b => b.Transaction.CancellationId == null
                                             && !b.IncludeInBalance);
                    break;
            }

            var accountingRecords = query.ProjectTo<AccountingRecord>(Mapper.ConfigurationProvider);
            return Ok(accountingRecords);
        }

    }
}