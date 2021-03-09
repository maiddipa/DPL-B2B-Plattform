using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class TransportOfferingsService : BaseService, ITransportOfferingsService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Olma.Transport> _olmaTransportRepo;
        private readonly IRepository<TransportBid> _olmaTransportBidRepo;

        public TransportOfferingsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IRepository<Olma.Transport> olmaTransportRepo,
            IRepository<Olma.TransportBid> olmaTransportBidRepo
            ) : base(authData, mapper)
        {
            _serviceProvider = serviceProvider;
            _olmaTransportRepo = olmaTransportRepo;
            _olmaTransportBidRepo = olmaTransportBidRepo;
        }

      

        public async Task<IWrappedResponse> GetById(int id)
        {
            var cmd = ServiceCommand<TransportOffering, Rules.TransportOfferings.GetById.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.TransportOfferings.GetById.MainRule(id))
                .Then(GetByIdAction);
            
            return await cmd.Execute();
        }

        public async Task<IWrappedResponse> GetByIdAction(Rules.TransportOfferings.GetById.MainRule mainRule)
        {
            return Ok( mainRule.Context.TransportOffering);
        }

        public async Task<IWrappedResponse> Search(TransportOfferingsSearchRequest request)
        {
            // TODO go-live implement restrictions who can see which transport
            var cmd = ServiceCommand<TransportOffering, Rules.TransportOfferings.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.TransportOfferings.Search.MainRule(request))
                .Then(SearchAction);
            
            return await cmd.Execute();
        }
        
        public async Task<IWrappedResponse> SearchAction(Rules.TransportOfferings.Search.MainRule mainRule)
        {
            return Ok(mainRule.Context.TransportOfferingsPaginationResult);
        }
        
        public async Task<IWrappedResponse> CreateBid(int transportId, TransportOfferingBidCreateRequest request)
        {
            // Welche Notwendigkeit besteht dafür? (redundanz)
            request.TransportId = transportId;
            
            var cmd = ServiceCommand<TransportOfferingBidCreateRequest, Rules.TransportOfferings.CreateBid.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.TransportOfferings.CreateBid.MainRule(request))
                .Then(CreateBidAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateBidAction([NotNull] Rules.TransportOfferings.CreateBid.MainRule mainRule)
        {
            if (mainRule == null) throw new ArgumentNullException(nameof(mainRule));

            foreach (var bid in mainRule.Context.Transport.Bids)
            {
                bid.Status = TransportBidStatus.Canceled;
            }
            
            var response = _olmaTransportBidRepo.Create<Olma.TransportBid,
                TransportOfferingBidCreateRequest,
                TransportOfferingBid>(mainRule.Context.Parent);

            return response;
        }

        public async Task<IWrappedResponse> CancelBid(int transportId, int bidId)
        {
            var cmd = ServiceCommand<TransportOfferingBid, Rules.TransportOfferings.CancelBid.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.TransportOfferings.CancelBid.MainRule((transportId, bidId)))
                .Then(CancelBidAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> CancelBidAction(Rules.TransportOfferings.CancelBid.MainRule mainRule)
        {
            var bid = mainRule.Context.Bid;

            bid.Status = TransportBidStatus.Canceled;

            _olmaTransportBidRepo.Save();

            var offeringBid = Mapper.Map<TransportOfferingBid>(bid);

            return Updated(offeringBid);
        }

        public async Task<IWrappedResponse> AcceptBid(int transportId, TransportOfferingBidAcceptRequest request)
        {
            // TODO Make sure customer can only accept bids for transports he is allowed to see
            var cmd = ServiceCommand<TransportOfferingBid, Rules.TransportOfferings.AcceptBid.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.TransportOfferings.AcceptBid.MainRule((transportId, request)))
                .Then(AcceptBidAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> AcceptBidAction(Rules.TransportOfferings.AcceptBid.MainRule mainRule)
        {
            var transport = mainRule.Context.Transport;
            var winningBid = mainRule.Context.WinningBid;
            
            winningBid.Status = TransportBidStatus.Won;

            transport.Status = TransportStatus.Allocated;
            transport.WinningBid = winningBid;

            _olmaTransportRepo.Save();

            var transportOffering = Mapper.Map<TransportOffering>(transport);

            return Ok(transportOffering);
        }
        
        public async Task<IWrappedResponse> AcceptTransport(int transportId)
        {
            // TODO Make sure customer can only accept bids for transports he is allowed to see
            var cmd = ServiceCommand<TransportOfferingBid, Rules.TransportOfferings.AcceptTransport.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.TransportOfferings.AcceptTransport.MainRule(transportId))
                .Then(AcceptTransportAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> AcceptTransportAction(Rules.TransportOfferings.AcceptTransport.MainRule mainRule)
        {            
            var transport = mainRule.Context.Transport;
            
            transport.Status = TransportStatus.Planned;
            transport.WinningBid.Status = TransportBidStatus.Accepted;
            transport.OrderMatch.Status = OrderMatchStatus.TransportScheduled;
            transport.OrderMatch.Supply.Detail.Status = OrderLoadStatus.TransportPlanned;
            transport.OrderMatch.Demand.Detail.Status = OrderLoadStatus.TransportPlanned;

            // let all others no that they have lost
            var allOtherActiveBids = _olmaTransportBidRepo.FindAll()
                .IgnoreQueryFilters()
                .Where(i => i.TransportId == mainRule.Context.TransportId
                            && i.Id != transport.WinningBid.Id
                            && i.Status == TransportBidStatus.Active)
                .ToList();

            foreach (var bid in allOtherActiveBids)
            {
                bid.Status = TransportBidStatus.Lost;
            }

            _olmaTransportRepo.Save();

            var transportOffering = Mapper.Map<TransportOffering>(transport);

            return Ok(transportOffering);
        }

        public async Task<IWrappedResponse> DeclineTransport(int transportId)
        {
            // TODO Make sure customer can only accept bids for transports he is allowed to see
            var cmd = ServiceCommand<TransportOfferingBid, Rules.TransportOfferings.DeclineTransport.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.TransportOfferings.DeclineTransport.MainRule(transportId))
                .Then(DeclineTransportAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> DeclineTransportAction(Rules.TransportOfferings.DeclineTransport.MainRule mainRule)
        {
            var transport = mainRule.Context.Transport;
            
            transport.WinningBid.Status = TransportBidStatus.Declined;
            transport.Status = TransportStatus.Requested;
            transport.WinningBid = null;

            _olmaTransportRepo.Save();

            var transportOffering = Mapper.Map<TransportOffering>(transport);

            return Ok(transportOffering);
        }
    }


    [Serializable]
    public class TransportOfferingServiceException : Exception
    {
        public TransportOfferingServiceException() { }
        public TransportOfferingServiceException(string message) : base(message) { }
        public TransportOfferingServiceException(string message, Exception inner) : base(message, inner) { }
        protected TransportOfferingServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}


