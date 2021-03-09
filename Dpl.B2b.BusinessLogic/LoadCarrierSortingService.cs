using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class LoadCarrierSortingService : BaseService, ILoadCarrierSortingService
    {
        private readonly Dal.OlmaDbContext _olmaDbContext;
        private readonly IRepository<Olma.LoadCarrier> _olmaLoadCarrierRepo;
        private readonly IRepository<Olma.LoadCarrierReceipt> _olmaLoadCarrierReceiptRepo;
        private readonly IRepository<Olma.LoadCarrierSorting> _olmaLoadCarrierSortingRepo;
        private readonly IPostingRequestsService _postingRequestsService;

        public LoadCarrierSortingService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo, IRepository<Olma.LoadCarrierReceipt> olmaLoadCarrierReceiptRepo, IRepository<Olma.LoadCarrierSorting> olmaLoadCarrierSortingRepo, OlmaDbContext olmaDbContext, IPostingRequestsService postingRequestsService) : base(authData, mapper)
        {
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
            _olmaLoadCarrierReceiptRepo = olmaLoadCarrierReceiptRepo;
            _olmaLoadCarrierSortingRepo = olmaLoadCarrierSortingRepo;
            _olmaDbContext = olmaDbContext;
            _postingRequestsService = postingRequestsService;
        }


        public async Task<IWrappedResponse> Create(LoadCarrierSortingCreateRequest request)
        {
            var loadCarrierReceipt = _olmaLoadCarrierReceiptRepo.FindByCondition(r => r.Id == request.LoadCarrierReceiptId)
                .Include(r => r.Positions)
                .Include(r => r.PostingAccount)
                .Include(r => r.PostingRequests)
                .Include(r => r.Document)
                .Single();
            var loadCarrierSorting = Mapper.Map<Olma.LoadCarrierSorting>(request);
            foreach (var position in request.Positions)
            {
                var receiptPosition = loadCarrierReceipt.Positions.Single(p => p.LoadCarrierId == position.LoadCarrierId);
                var sortingPosition = loadCarrierSorting.Positions.Single(p => p.LoadCarrierId == position.LoadCarrierId);
                sortingPosition.Outputs = new List<Olma.LoadCarrierSortingResultOutput>();

                var inputQuantity = receiptPosition.LoadCarrierQuantity;
                sortingPosition.InputQuantity = inputQuantity;

                foreach (var output in position.Outputs)
                {
                    var inputLoadCarrier = _olmaLoadCarrierRepo.GetByKey(position.LoadCarrierId);
                    var outputLoadCarrier = _olmaLoadCarrierRepo.FindByCondition(l =>
                        l.TypeId == inputLoadCarrier.TypeId && l.QualityId == output.LoadCarrierQualityId).Single();
                    var _output = new Olma.LoadCarrierSortingResultOutput()
                    {
                        
                        LoadCarrierId = outputLoadCarrier.Id,
                        LoadCarrierQuantity = output.Quantity
                    };
                    sortingPosition.Outputs.Add(_output);
                }

                var sumOutputQuantities = sortingPosition.Outputs.Sum(o => o.LoadCarrierQuantity);
                if (sumOutputQuantities > inputQuantity)
                    throw new ArgumentException("Sum of output quantities does not match input");
                sortingPosition.RemainingQuantity = inputQuantity - sumOutputQuantities;
            }

            var strategy = _olmaDbContext.Database.CreateExecutionStrategy();
            var result = await strategy.ExecuteAsync(async () =>
            {
                await using var ctxTransaction = await _olmaDbContext.Database.BeginTransactionAsync();
                // Create LoadCarrierSorting
                var loadCarrierSortingResult = _olmaLoadCarrierSortingRepo
                    .Create<Olma.LoadCarrierSorting, Olma.LoadCarrierSorting, LoadCarrierSorting>(loadCarrierSorting);

                // Create PostingRequestsCreateRequest

                #region PostingRequestsCreateRequest
                var postingAccount = loadCarrierReceipt.PostingAccount;
                foreach (var receiptPosition in loadCarrierReceipt.Positions)
                {
                    var sortingPosition =
                        loadCarrierSorting.Positions.Single(p => p.LoadCarrierId == receiptPosition.LoadCarrierId);
                    var postingRequestsCreateRequest = new PostingRequestsCreateRequest
                    {
                        Type = PostingRequestType.Charge, //HACK Only to fix UI, from LTMS WebApp viewpoint  it would be a charge
                        ReferenceNumber = loadCarrierReceipt.Document?.Number,
                        Reason = PostingRequestReason.LoadCarrierReceipt,
                        LoadCarrierReceiptId = loadCarrierReceipt.Id,
                        RefLtmsProcessId = loadCarrierReceipt.PostingRequests.First().RefLtmsProcessId,
                        RefLtmsTransactionId = Guid.NewGuid(),
                        PostingAccountId = postingAccount.Id,
                        RefLtmsProcessTypeId = (int)RefLtmsProcessType.Sorting,
                        SourceRefLtmsAccountId = postingAccount.RefLtmsAccountId,
                        DestinationRefLtmsAccountId = postingAccount.RefLtmsAccountId,
                        DeliveryNoteNumber = loadCarrierReceipt.DeliveryNoteNumber,
                        PickUpNoteNumber = loadCarrierReceipt.PickUpNoteNumber
                    };
                    var debitPosition =
                                new PostingRequestPosition //TODO: make it support 1 + n positions
                            {
                                    LoadCarrierId = sortingPosition.LoadCarrierId,
                                    LoadCarrierQuantity = -sortingPosition.InputQuantity
                                };
                    var creditPositions = sortingPosition.Outputs.Select(o =>
                        new PostingRequestPosition
                        {
                            LoadCarrierId = o.LoadCarrierId,
                            LoadCarrierQuantity = o.LoadCarrierQuantity
                        }
                    );
                    var positions = new List<PostingRequestPosition>() { debitPosition };
                    positions.AddRange(creditPositions);
                    postingRequestsCreateRequest.Positions = positions;

                    var postingRequestsServiceResponse =
                        (IWrappedResponse<IEnumerable<PostingRequest>>)await _postingRequestsService
                            .Create(postingRequestsCreateRequest);
                    if (postingRequestsServiceResponse.ResultType != ResultType.Created)
                    {
                        await ctxTransaction.RollbackAsync();
                        return new WrappedResponse<LoadCarrierSorting>
                        {
                            ResultType = postingRequestsServiceResponse.ResultType,
                            State = postingRequestsServiceResponse.State
                        };
                    }
                }
                

                #endregion

            //Set IsSortingCompleted Flag on LoadCarrierReceipt
            if (loadCarrierSorting.Positions.Count == loadCarrierReceipt.Positions.Count &&
                loadCarrierSorting.Positions.SelectMany(p => p.Outputs).Sum(o => o.LoadCarrierQuantity) == loadCarrierReceipt.Positions.Sum(p => p.LoadCarrierQuantity))
            {
                loadCarrierReceipt.IsSortingCompleted = true;
                _olmaLoadCarrierRepo.Save();
            }

            // Commit
                await ctxTransaction.CommitAsync();
                return loadCarrierSortingResult;
            });
            
            return Created(result.Data);
        }

        public async Task<IWrappedResponse> GetByLoadCarrierReceiptId(int id)
        {
            var loadCarrierSorting = _olmaLoadCarrierSortingRepo.FindByCondition(s => s.LoadCarrierReceiptId == id)
                .Include(s => s.Positions)
                .ThenInclude(p => p.Outputs)
                .ThenInclude(o => o.LoadCarrier)
                .AsNoTracking().Single();
            var response =  Mapper.Map<LoadCarrierSorting>(loadCarrierSorting);

            return Ok(response);
        }
    }
}
