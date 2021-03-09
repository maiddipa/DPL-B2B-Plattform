using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Reporting;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Document;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using Olma = Dpl.B2b.Dal.Models;
using PostingRequest = Dpl.B2b.Contracts.Models.PostingRequest;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class LoadCarrierReceiptsService : BaseService, ILoadCarrierReceiptsService
    {
        private readonly Dal.OlmaDbContext _olmaDbContext;
        private readonly IRepository<Olma.LoadCarrierReceipt> _olmaLoadCarrierReceiptRepo;
        private readonly IRepository<Olma.LoadCarrier> _olmaLoadCarrierRepo;
        private readonly IRepository<Olma.CustomerPartner> _olmaCustomerPartnerRepo;
        private readonly IRepository<Olma.CustomerDivision> _olmaCustomerDivisionRepo;
        private readonly INumberSequencesService _numberSequencesService;
        private readonly IDocumentsService _documentsService;
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly IPostingRequestsService _postingRequestsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISynchronizationsService _synchronizationsService;

        public LoadCarrierReceiptsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            Dal.OlmaDbContext olmaDbContext,
            IRepository<Olma.LoadCarrierReceipt> olmaLoadCarrierReceiptRepo,
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo,
            IRepository<Olma.CustomerPartner> olmaCustomerPartnerRepo,
            IRepository<Olma.CustomerDivision> olmaCustomerDivisionRepo,
            ISynchronizationsService synchronizationsService,
            INumberSequencesService numberSequencesService,
            IDocumentsService documentsService,
            Reporting.IReportGeneratorService reportGeneratorService,
            IPostingRequestsService postingRequestsService,
            IServiceProvider serviceProvider) : base(authData, mapper)
        {
            _olmaDbContext = olmaDbContext;
            _olmaLoadCarrierReceiptRepo = olmaLoadCarrierReceiptRepo;
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
            _olmaCustomerPartnerRepo = olmaCustomerPartnerRepo;
            _olmaCustomerDivisionRepo = olmaCustomerDivisionRepo;
            _numberSequencesService = numberSequencesService;
            _documentsService = documentsService;
            _reportGeneratorService = reportGeneratorService;
            _postingRequestsService = postingRequestsService;
            _serviceProvider = serviceProvider;
            _synchronizationsService = synchronizationsService;
        }
        
        public async Task<IWrappedResponse> GetSortingOptionsByReceiptId(int id)
        {
            var cmd = ServiceCommand<LoadCarrierReceipt, Rules.LoadCarrierReceipts.GetSortingOptionsByReceiptId.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadCarrierReceipts.GetSortingOptionsByReceiptId.MainRule(id))
                .Then(GetSortingOptionsByReceiptIdAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> GetSortingOptionsByReceiptIdAction(Rules.LoadCarrierReceipts.GetSortingOptionsByReceiptId.MainRule mainRule)
        {
            var olmaLoadCarrierReceipt = mainRule.Context.Rules.LoadCarrierReceiptResourceRule.Context.Resource;
            
            var loadCarriers = await _olmaLoadCarrierRepo.FindAll()
                .Include(lc => lc.Quality)
                .FromCacheAsync();

            var loadCarrierReceiptSortingOption = new LoadCarrierReceiptSortingOption
            {
                Id = olmaLoadCarrierReceipt.Id,
                SortingPositions = new List<LoadCarrierReceiptSortingPosition>()
            };
            
            foreach (var loadCarrierTypePosition in olmaLoadCarrierReceipt.Positions)
            {
                var sortingOption = new LoadCarrierReceiptSortingPosition
                {
                    Quantity = loadCarrierTypePosition.LoadCarrierQuantity,
                    LoadCarrierId = loadCarrierTypePosition.LoadCarrierId
                };
                
                var possibleSortingQualities = loadCarriers.Where(lc => lc.TypeId == loadCarrierTypePosition.LoadCarrier.TypeId)
                    .Select(lc => lc.Quality).Where(q => q.Id > 0);
                
                //Exclude unnecessary qualities 
                var excludedSortingQualities = new List<int> {1, 13, 30, 31, 32};
                possibleSortingQualities =
                    possibleSortingQualities.Where(lc => !excludedSortingQualities.Contains(lc.Id));

                if (olmaLoadCarrierReceipt.DepotPreset?.Category != LoadCarrierReceiptDepotPresetCategory.Internal)
                {
                    possibleSortingQualities =
                        possibleSortingQualities.Where(lc =>
                            lc.Order <= loadCarrierTypePosition.LoadCarrier.Quality.Order);
                }

                sortingOption.PossibleSortingQualities = Mapper.Map<IEnumerable<LoadCarrierQuality>>(possibleSortingQualities.OrderBy(sq => sq.Order).ToList());
                
                loadCarrierReceiptSortingOption.SortingPositions.Add(sortingOption);
            }

            loadCarrierReceiptSortingOption.LoadCarrierReceipt = Mapper.Map<LoadCarrierReceipt>(olmaLoadCarrierReceipt);

            return Ok(loadCarrierReceiptSortingOption);
        }

        [Obsolete]
        public async Task<IWrappedResponse> GetById(int id)
        {
            var cmd = ServiceCommand<Contracts.Models.LoadCarrierReceipt, Rules.LoadCarrierReceipts.GetById.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadCarrierReceipts.GetById.MainRule(id))
                .Then(GetByIdAction);
            
            return await cmd.Execute();
        }

        [Obsolete]
        private async Task<IWrappedResponse> GetByIdAction(Rules.LoadCarrierReceipts.GetById.MainRule mainRule)
        {            
            return await Task.FromResult(mainRule.Context.LoadCarrierReceipt);
        }

        public async Task<IWrappedResponse> Cancel(int id, LoadCarrierReceiptsCancelRequest request)
        {
            var cmd = ServiceCommand<IWrappedResponse, Rules.LoadCarrierReceipts.Cancel.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadCarrierReceipts.Cancel.MainRule((id, request)))
                .Then(CancelAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CancelAction(Rules.LoadCarrierReceipts.Cancel.MainRule rule)
        {
            var id = rule.Context.Parent.Id;
            var request = rule.Context.Parent.Request;
            var loadCarrierReceipt = rule.Context.LoadCarrierReceipt;

            if (loadCarrierReceipt.Document != null)
            {
                loadCarrierReceipt.Document.CancellationReason = request.Reason;
                loadCarrierReceipt.Document.StateId = 255;
            }

            var syncDate = DateTime.UtcNow;
            foreach (var postingRequest in loadCarrierReceipt.PostingRequests)
            {
                postingRequest.Status = PostingRequestStatus.Canceled;
                postingRequest.SyncDate = syncDate;
            }

            var rollbackFulfillmentSyncRequest = false;
            if (loadCarrierReceipt.OrderLoadDetail != null)
            {
                loadCarrierReceipt.OrderLoadDetail.Status = OrderLoadStatus.TransportPlanned;
                loadCarrierReceipt.OrderLoadDetail.ActualFulfillmentDateTime = null;
                loadCarrierReceipt.OrderLoadDetail.LoadCarrierReceiptId = null;
                loadCarrierReceipt.OrderLoadDetailId = null;
                rollbackFulfillmentSyncRequest = true;
            }

            var response = _olmaLoadCarrierReceiptRepo
                .Update<Olma.LoadCarrierReceipt, Olma.LoadCarrierReceipt, LoadCarrierReceipt>(id,
                    loadCarrierReceipt);

            await _synchronizationsService.SendPostingRequestsAsync(new PostingRequestsSyncRequest
            {
                PostingRequestCancelSyncRequests = new List<PostingRequestCancelSyncRequest>
                {
                    new PostingRequestCancelSyncRequest
                    {
                        RefLtmsTransactionId =
                            loadCarrierReceipt.PostingRequests.First().RefLtmsTransactionId,
                        Reason = request.Reason
                    }
                }
            });

            if (rollbackFulfillmentSyncRequest)
            {
                await _synchronizationsService.SendOrdersAsync(new OrdersSyncRequest
                {
                    OrderRollbackFulfillmentSyncRequests = new List<OrderRollbackFulfillmentSyncRequest>
                    {
                        new OrderRollbackFulfillmentSyncRequest
                        {
                            DigitalCode = loadCarrierReceipt.DigitalCode
                        }
                    }
                });
            }

            return response;
        }

        public async Task<IWrappedResponse> UpdateIsSortingRequired(int id, LoadCarrierReceiptsUpdateIsSortingRequiredRequest request)
        {
            var loadCarrierReceipt = _olmaLoadCarrierReceiptRepo.FindByCondition(r => r.Id == id && !r.IsDeleted)
                .IgnoreQueryFilters().Include(r => r.DplNotes).Single();
            if (loadCarrierReceipt.IsSortingRequired != request.IsSortingRequired)
            {
                loadCarrierReceipt.IsSortingRequired = request.IsSortingRequired;
                if (request.DplNote != null && request.DplNote.UserId != 0)
                {
                    loadCarrierReceipt.DplNotes ??= new List<Olma.EmployeeNote>();
                    loadCarrierReceipt.DplNotes.Add(Mapper.Map<Olma.EmployeeNote>(request.DplNote));
                }
                _olmaLoadCarrierReceiptRepo.Save();
            }

            var result = Mapper.Map<LoadCarrierReceipt>(loadCarrierReceipt);
            return Ok(result);
        }

        public async Task<IWrappedResponse> Create(LoadCarrierReceiptsCreateRequest request)
        {
            var cmd = ServiceCommand<Contracts.Models.LoadCarrierReceipt, Rules.LoadCarrierReceipts.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadCarrierReceipts.Create.MainRule(request))
                .Then(CreateAction);

            var result = cmd.Execute();

            return await result;
        }

        /// <summary>
        /// Erzeugt eine Quittungen. Diese Action hat abhängigkeiten zum NumberSequencesService und zum PostingRequestsService.
        /// </summary>
        /// <param name="mainRule"></param>
        /// <returns></returns>
        private async Task<IWrappedResponse> CreateAction(Rules.LoadCarrierReceipts.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Parent;
            var printLanguageId = request.PrintLanguageId;
            var loadCarrierReceipt = mainRule.Context.LoadCarrierReceipt;
            var customerDivision = mainRule.Context.CustomerDivision;
            var isSupply = mainRule.Context.IsSupply;
            var targetRefLtmsAccountId = mainRule.Context.TargetRefLtmsAccountId;
            var orderLoad = mainRule.Context.OrderLoad;
            var refLtmsTransactionId = mainRule.Context.RefLtmsTransactionId;
            var isSelfService = mainRule.Context.IsSelfService;
            var documentType = mainRule.Context.DocumentType;
            var lmsAvail2deli = mainRule.Context.LmsAvail2deli;

            var documentNumber = _numberSequencesService
                .GetDocumentNumber(documentType, request.CustomerDivisionId).Result;
            mainRule.Context.LoadCarrierReceipt.Document.Number = documentNumber;

            var strategy = _olmaDbContext.Database.CreateExecutionStrategy();
            var result = await strategy.ExecuteAsync(async () =>
            {
                await using var ctxTransaction = await _olmaDbContext.Database.BeginTransactionAsync();
                var strategyResult = _olmaLoadCarrierReceiptRepo
                    .Create<Olma.LoadCarrierReceipt, Olma.LoadCarrierReceipt, LoadCarrierReceipt>(
                        loadCarrierReceipt);
                var reportGeneratorServiceResponse =
                    (IWrappedResponse<Report>) _reportGeneratorService.GenerateReportForLanguage(
                        strategyResult.Data.DocumentId, printLanguageId, request.PrintCount - 1,
                        request.PrintDateTimeOffset);

                var downloadLink = (IWrappedResponse<string>) await _documentsService
                    .GetDocumentDownload(strategyResult.Data.DocumentId, DocumentFileType.Composite);

                if (downloadLink.ResultType != ResultType.Ok)
                {
                    await ctxTransaction.RollbackAsync();
                    return new WrappedResponse<LoadCarrierReceipt>
                    {
                        ResultType = ResultType.Failed,
                        State = ErrorHandler.Create().AddMessage(new DocumentLinkError()).GetServiceState()
                    };
                }
                strategyResult.Data.DownloadLink = downloadLink.Data;

                var postingRequestsCreateRequest = new PostingRequestsCreateRequest
                {
                    Type = isSupply
                        ? PostingRequestType.Credit
                        : PostingRequestType
                            .Charge, //HACK Only to fix UI, from LTMS WebApp viewpoint  it would be a charge
                    ReferenceNumber = loadCarrierReceipt.Document?.Number,
                    Reason = PostingRequestReason.LoadCarrierReceipt,
                    LoadCarrierReceiptId = strategyResult.Data.Id,
                    IsSortingRequired = strategyResult.Data.IsSortingRequired,
                    DocumentFileName = reportGeneratorServiceResponse.Data.DocumentArchiveName,
                    IsSelfService = isSelfService,
                    Positions = loadCarrierReceipt.Positions.Select(lcr =>
                        new PostingRequestPosition
                        {
                            LoadCarrierId = lcr.LoadCarrierId,
                            LoadCarrierQuantity = lcr.LoadCarrierQuantity
                        }),
                    RefLtmsProcessId = Guid.NewGuid(),
                    RefLtmsTransactionId = refLtmsTransactionId ?? Guid.NewGuid(),
                    PostingAccountId = customerDivision.PostingAccountId.Value,
                    RefLtmsProcessTypeId =
                        isSupply ? (int) RefLtmsProcessType.Excemption : (int) RefLtmsProcessType.DepotAcceptance,
                    SourceRefLtmsAccountId =
                        isSupply ? customerDivision.PostingAccount.RefLtmsAccountId : targetRefLtmsAccountId,
                    DestinationRefLtmsAccountId = !isSupply
                        ? customerDivision.PostingAccount.RefLtmsAccountId
                        : targetRefLtmsAccountId,
                    DigitalCode = loadCarrierReceipt.DigitalCode,
                    RefLmsBusinessTypeId = request.RefLmsBusinessTypeId,
                    RefLtmsTransactionRowGuid = request.RefLtmsTransactionRowGuid,
                    DeliveryNoteNumber = loadCarrierReceipt.DeliveryNoteNumber,
                    PickUpNoteNumber = loadCarrierReceipt.PickUpNoteNumber
                };

                var postingRequestsServiceResponse =
                    (IWrappedResponse<IEnumerable<PostingRequest>>) await _postingRequestsService
                        .Create(postingRequestsCreateRequest);
                if (postingRequestsServiceResponse.ResultType != ResultType.Created)
                {
                    await ctxTransaction.RollbackAsync();
                    return new WrappedResponse<LoadCarrierReceipt>
                    {
                        ResultType = postingRequestsServiceResponse.ResultType,
                        State = postingRequestsServiceResponse.State
                    };
                }

                if (lmsAvail2deli != null)
                {
                    var orderFulfillSyncRequest = new OrderFulfillSyncRequest
                    {
                        DigitalCode = loadCarrierReceipt.DigitalCode,
                        FulfillmentDateTime = DateTime.UtcNow,
                        RefLmsDeliveryRowGuid = lmsAvail2deli.Delivery.RowGuid,
                        RefLmsAvailabilityRowGuid = lmsAvail2deli.Availability.RowGuid
                    };

                    var ordersSyncRequest = new OrdersSyncRequest
                    {
                        OrderFulfillSyncRequests = new List<OrderFulfillSyncRequest>
                        {
                            orderFulfillSyncRequest
                        }
                    };

                    //Reine Sync-Fehler für Updates werden geloggt und müssen manuell behoben werden. 
                    await _synchronizationsService.SendOrdersAsync(ordersSyncRequest);
                }
                await ctxTransaction.CommitAsync();
                return strategyResult;
            });

            return result;
        }
        
        public async Task<IWrappedResponse> Search(LoadCarrierReceiptsSearchRequest request)
        {
            var cmd = ServiceCommand<LoadCarrierReceipt, Rules.LoadCarrierReceipts.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadCarrierReceipts.Search.MainRule(request))
                .Then(SearchAction);
            
            return await cmd.Execute();
        }


        private async Task<IWrappedResponse> SearchAction(Rules.LoadCarrierReceipts.Search.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            
            var query = _olmaLoadCarrierReceiptRepo.FindAll()
                .Where(cd => cd.CustomerDivisionId == request.CustomerDivisionId).AsNoTracking();
            return Ok(query.ProjectTo<LoadCarrierReceipt>(Mapper.ConfigurationProvider));
        }

        private IQueryable<Olma.LoadCarrierReceipt> BuildLoadCarrierReceiptSearchQuery(LoadCarrierReceiptsSearchRequest request)
        {
            var query = _olmaLoadCarrierReceiptRepo.FindAll()
                .AsNoTracking();

            if (request.Type != null)
            {
                query = query.Where(v => request.Type.Contains(v.Type));
            }

            if (!string.IsNullOrEmpty(request.DocumentNumber))
            {
                query = query.Where(v => v.Document.Number.Contains(request.DocumentNumber)); //TODO compare as SQL LIKE
            }

            if (request.LoadCarrierTypes?.Count > 0)
            {
                query = query.Where(i => i.Positions.Any(p => request.LoadCarrierTypes.Contains(p.LoadCarrier.TypeId)));
            }

            if (request.IssueDateFrom != null)
            {
                query = query.Where(i => i.CreatedAt >= request.IssueDateFrom);
            }

            if (request.IssueDateTo != null)
            {
                query = query.Where(i => i.CreatedAt >= request.IssueDateTo);
            }

            return query;
        }
    }
}
