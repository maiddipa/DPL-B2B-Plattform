using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Ltms = Dpl.B2b.Dal.Ltms;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class BalanceTransfersService : BaseService, IBalanceTransfersService
    {
        private readonly IPostingRequestsService _postingRequestService;
        private readonly IServiceProvider _serviceProvider;

        public BalanceTransfersService(
            IAuthorizationDataService authData,
            IAuthorizationService authService,
            IMapper mapper,
            IPostingRequestsService postingRequestService,
            IServiceProvider serviceProvider
        ) : base(authData, authService, mapper)
        {
            _postingRequestService = postingRequestService;
            _serviceProvider = serviceProvider;
        }

        public Task<IWrappedResponse<BalanceTransfer>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IWrappedResponse> Create(BalanceTransferCreateRequest request)
        {
            // Use ServiceCommand to create typically execution pattern
            // SetAuthorizedResourceById TODO CHeck if User ist allowed to transfer to DestinationAccount, discuss how we should control this/define this via MasterData
            var cmd = ServiceCommand<PostingRequest, Rules.BalanceTransfer.Create.MainRule, PostingAccountPermissionResource, CanCreateBalanceTransferRequirement>
                .Create(_serviceProvider)
                .SetAuthorizedResourceById<Olma.PostingAccount>(request.SourceAccountId)
                .When(new Rules.BalanceTransfer.Create.MainRule(request))
                .Then(CreateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.BalanceTransfer.Create.MainRule rule)
        {
            var sourceAccount = rule.Context.SourceAccount;
            var destinationAccount = rule.Context.DestinationAccount;

            var loadCarrierId = (int) rule.Context.LoadCarrierId;
            var quantity = (int) rule.Context.LoadCarrierQuantity;

            var postingRequest = new PostingRequestsCreateRequest()
            {
                Type = PostingRequestType.Charge, //HACK Only to fix UI, from LTMS WebApp viewpoint  it would be a credit
                Reason = PostingRequestReason.Transfer,
                RefLtmsProcessTypeId = (int) RefLtmsProcessType.BalanceTransfer,
                Positions = new List<PostingRequestPosition>()
                {
                    new PostingRequestPosition()
                    {
                        LoadCarrierId = loadCarrierId,
                        LoadCarrierQuantity = Math.Abs(quantity)
                    }
                },
                RefLtmsProcessId = Guid.NewGuid(),
                RefLtmsTransactionId = Guid.NewGuid(),
                PostingAccountId = sourceAccount.Id,
                SourceRefLtmsAccountId = destinationAccount.RefLtmsAccountId, // on LTMS ViewPoint source and destination have to be switched
                DestinationRefLtmsAccountId = sourceAccount.RefLtmsAccountId, // on LTMS ViewPoint source and destination have to be switched
                Note = rule.Context.Parent.Note,
                DplNote = rule.Context.Parent.DplNote
            };
            var response = (IWrappedResponse<IEnumerable<PostingRequest>>) await _postingRequestService.Create(postingRequest);
            //To avoit breaking changes with UI
            //Only one PostingRequest is returned
            var result = new WrappedResponse<PostingRequest>()
            {
                ResultType = response.ResultType, Data = response.Data.First(), Errors = response.Errors,
                Id = response.Id, Warnings = response.Warnings
            };
            ;
            return result;
        }

        public Task<IWrappedResponse<BalanceTransfer>> Cancel(int id, BalanceTransferCancelRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IWrappedResponse<IPaginationResult<BalanceTransfer>>> Search(BalanceTransferSearchRequest request)
        {
            throw new NotImplementedException();
        }
    }
}