using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.PostingRequest;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class PostingRequestsService : BaseService, IPostingRequestsService
    {
        private readonly IRepository<Olma.PostingRequest> _olmaPostingRequestRepo;
        private readonly IRepository<Olma.LoadCarrier> _olmaLoadCarrierRepo;
        private readonly INumberSequencesService _numberSequencesService;
        private readonly ISynchronizationsService _synchronizationsService;

        public PostingRequestsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            ISynchronizationsService synchronizationsService,
            IRepository<Olma.PostingRequest> olmaPostingRequestRepo,
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo,
            INumberSequencesService numberSequencesService) : base(authData, mapper)
        {
            _olmaPostingRequestRepo = olmaPostingRequestRepo;
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
            _synchronizationsService = synchronizationsService;
            _numberSequencesService = numberSequencesService;
        }

        public async Task<IWrappedResponse> Search(PostingRequestsSearchRequest request)
        {
            var query = _olmaPostingRequestRepo.FindAll()
                .Where(pr =>pr.Status == PostingRequestStatus.Pending
                    && pr.PostingAccountId == request.PostingAccountId
                    && pr.LoadCarrier.TypeId == request.LoadCarrierTypeId).AsNoTracking();

            var postingRequests = query.ProjectTo<AccountingRecord>(Mapper.ConfigurationProvider);
            return Ok(postingRequests);
        }

        public async Task<IWrappedResponse<PostingRequest>> GetById(int id)
        {
            var response = _olmaPostingRequestRepo.GetById<Olma.PostingRequest, PostingRequest>(id);
            return response;
        }

        public async Task<IWrappedResponse> Create(PostingRequestsCreateRequest request)
        {
            var olmaPostingRequests = new List<Olma.PostingRequest>();

            foreach (var postingRequestPosition in request.Positions)
            {
                var olmaPostingRequest = Mapper.Map<Olma.PostingRequest>(request);
                olmaPostingRequest.LoadCarrierId = postingRequestPosition.LoadCarrierId;
                olmaPostingRequest.LoadCarrierQuantity = postingRequestPosition.LoadCarrierQuantity;
                olmaPostingRequest.SyncDate = DateTime.UtcNow;

                _olmaPostingRequestRepo.Create(olmaPostingRequest);
                _olmaPostingRequestRepo.Save();

                var postingRequest = olmaPostingRequest;
                olmaPostingRequest = _olmaPostingRequestRepo.FindByCondition(pr => pr.Id == postingRequest.Id)
                    .Include(lc => lc.LoadCarrier)
                    .Include(pa => pa.PostingAccount)
                    .FirstOrDefault();
               
                if (olmaPostingRequest == null)
                {
                    return Failed<IEnumerable<PostingRequest>>();
                }

                if (string.IsNullOrEmpty(request.ReferenceNumber))
                {
                    olmaPostingRequest.ReferenceNumber =
                        await _numberSequencesService.GetProcessNumber(ProcessType.PostingRequest,
                            olmaPostingRequest.Id);
                }

                olmaPostingRequests.Add(olmaPostingRequest);
            }

            var postingRequestsSyncRequest = new PostingRequestsSyncRequest
            {
                PostingRequestCreateSyncRequests = new List<PostingRequestCreateSyncRequest>()
            };

            var postingRequestCreateSyncRequest =
                new PostingRequestCreateSyncRequest
                {
                    IsSortingRequired = olmaPostingRequests.Select(i => i.IsSortingRequired).FirstOrDefault(),
                    Positions = olmaPostingRequests.Select(pr => new PostingRequestPosition
                    {
                        RefLtmsPalletId = pr.LoadCarrier.RefLtmsPalletId,
                        LoadCarrierQuantity = pr.LoadCarrierQuantity,
                        RefLtmsBookingRowGuid = pr.RowGuid
                    }),
                    CreditRefLtmsAccountId =
                        olmaPostingRequests.Select(pr => pr.DestinationRefLtmsAccountId).FirstOrDefault(),
                    DebitRefLtmsAccountId =
                        olmaPostingRequests.Select(pr => pr.SourceRefLtmsAccountId).FirstOrDefault(),
                    ReferenceNumber = olmaPostingRequests.Select(pr => pr.ReferenceNumber).FirstOrDefault(),
                    IssuedByRefLtmsAccountId = olmaPostingRequests.Select(pr => pr.PostingAccount.RefLtmsAccountId)
                        .FirstOrDefault(),
                    Note = olmaPostingRequests.Select(pr => pr.Note).FirstOrDefault(),
                    RefLtmsProcessId = olmaPostingRequests.Select(pr => pr.RefLtmsProcessId).FirstOrDefault(),
                    RefLtmsProcessType =
                        (RefLtmsProcessType) olmaPostingRequests.Select(pr => pr.RefLtmsProcessTypeId)
                            .FirstOrDefault(),
                    RefLtmsTransactionId = request.RefLtmsTransactionRowGuid ?? olmaPostingRequests.Select(pr => pr.RefLtmsTransactionId).FirstOrDefault(),
                    DocumentFileName = request.DocumentFileName,
                    IsSelfService = request.IsSelfService,
                    DigitalCode = request.DigitalCode,
                    DeliveryNoteNumber = request.DeliveryNoteNumber,
                    PickUpNoteNumber = request.PickUpNoteNumber,
                    RefLmsBusinessTypeId = request.RefLmsBusinessTypeId
                };

            var result =
                Mapper.Map<IEnumerable<Olma.PostingRequest>, IEnumerable<PostingRequest>>(olmaPostingRequests);

            postingRequestsSyncRequest.PostingRequestCreateSyncRequests.Add(postingRequestCreateSyncRequest);
            var syncResult = await _synchronizationsService.SendPostingRequestsAsync(postingRequestsSyncRequest);

            // Switch (override) Type and loadCarrierQuantity sign when quantity is negative to support Split Bookings
            // PostingRequests have to have positive quantities all the time 
            if (olmaPostingRequests.Any(r => r.LoadCarrierQuantity < 0))
            {
                foreach (var postingRequest in olmaPostingRequests.Where(r => r.LoadCarrierQuantity < 0))
                {
                    postingRequest.LoadCarrierQuantity = Math.Abs(postingRequest.LoadCarrierQuantity);
                    postingRequest.Type = PostingRequestType.Credit;
                }
                _olmaPostingRequestRepo.Save();
            }

            if (syncResult.ResultType != ResultType.Ok)
            {
                return new WrappedResponse
                {
                    ResultType = ResultType.Failed
                };
            }
            return Created(result);
        }

        public async Task<IWrappedResponse> Update(int id, PostingRequestsUpdateRequest request)
        {
            #region security

            // TODO add security

            #endregion

            var response = _olmaPostingRequestRepo.Update<Olma.PostingRequest, PostingRequestsUpdateRequest, PostingRequest>(id, request);
            _olmaPostingRequestRepo.Save();
            return response;
        }

        public async Task<IWrappedResponse> Cancel(int id)
        {
            #region security

            // TODO add security

            #endregion

            var postingRequestResponse = _olmaPostingRequestRepo.GetById<Olma.PostingRequest,PostingRequest>(id);

            if (postingRequestResponse.ResultType == ResultType.NotFound)
            {
                return postingRequestResponse;
            }

            var postingRequest = postingRequestResponse.Data;

            if (postingRequest.Status == PostingRequestStatus.Confirmed)
            {
                return new WrappedResponse<PostingRequest>()
                {
                    ResultType = ResultType.Failed,
                    State = ErrorHandler.Create().AddMessage(new PostingRequestNotCanceledError()).GetServiceState()
                };
            }

            postingRequest.Status = PostingRequestStatus.Canceled;

            var response = _olmaPostingRequestRepo.Update<Olma.PostingRequest, PostingRequest, PostingRequest>(id, postingRequest);
            _olmaPostingRequestRepo.Save();
            return response;
        }

        public async Task<IWrappedResponse> Rollback(Guid refLtmsTransactionId)
        {
            PostingRequestRollbackSyncRequest requestRollbackSyncRequest = new PostingRequestRollbackSyncRequest
            {
                RefLtmsTransactionId = refLtmsTransactionId
            };
            var postingRequestsSyncRequest = new PostingRequestsSyncRequest
            {
                PostingRequestRollbackSyncRequests = new List<PostingRequestRollbackSyncRequest>{requestRollbackSyncRequest}

            };

            var result = await _synchronizationsService.SendPostingRequestsAsync(postingRequestsSyncRequest);


            if (result.ResultType == ResultType.Failed)
            {
                return new WrappedResponse
                {
                    ResultType = ResultType.Failed,
                    State = ErrorHandler.Create().AddMessage(new PostingRequestNotRolledBackError()).GetServiceState()
                };
            }

            return new WrappedResponse
            {
                ResultType = ResultType.Ok
            };
        }
    }
}
