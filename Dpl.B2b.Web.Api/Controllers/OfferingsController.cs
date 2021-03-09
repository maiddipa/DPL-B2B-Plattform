using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    //[Authorize]
    public class OfferingsController : ControllerBase
    {
        private readonly ILoadCarrierOfferingsService _loadCarrierOfferingsService;
        private readonly ITransportOfferingsService _transportOfferingsService;

        public OfferingsController(ILoadCarrierOfferingsService loadCarrierOfferingsService, ITransportOfferingsService transportOfferingsService)
        {
            _loadCarrierOfferingsService = loadCarrierOfferingsService;
            _transportOfferingsService = transportOfferingsService;
        }

        [HttpGet("load-carriers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DplProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public Task<ActionResult<IEnumerable<LoadCarrierOffering>>> SearchLoadCarrierOfferings([FromQuery] LoadCarrierOfferingsSearchRequest request)
        {
            return this._loadCarrierOfferingsService.Search(request).Convert<IEnumerable<LoadCarrierOffering>>(this);
        }

        [HttpGet("transports")]
        public Task<ActionResult<IPaginationResult<TransportOffering>>> SearchTransportOfferings([FromQuery] TransportOfferingsSearchRequest request)
        {
            return this._transportOfferingsService.Search(request).Convert<IPaginationResult<TransportOffering>>(this);
        }

        [HttpGet("transports/{id}")]
        public Task<ActionResult<TransportOffering>> GetTransportOffering(int id)
        {
            return this._transportOfferingsService.GetById(id).Convert<TransportOffering>(this);
        }

        [HttpPost("transports/{id}/bids")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public Task<ActionResult<TransportOfferingBid>> CreateTransportOfferingBid(int id, [FromBody] TransportOfferingBidCreateRequest request)
        {
            return this._transportOfferingsService.CreateBid(id, request).Convert<TransportOfferingBid>(this);
        }

        [HttpPost("transports/{id}/bids/{bidId}")]
        public Task<ActionResult<TransportOfferingBid>> CancelTransportOfferingBid(int id, int bidId)
        {
            return this._transportOfferingsService.CancelBid(id, bidId).Convert<TransportOfferingBid>(this);
        }

        [HttpPatch("transports/{id}/accept-bid")]
        public Task<ActionResult<TransportOffering>> AcceptBidForTransportOffering(int id, [FromBody] TransportOfferingBidAcceptRequest request)
        {
            return this._transportOfferingsService.AcceptBid(id, request).Convert<TransportOffering>(this);
        }

        [HttpPatch("transports/{id}/accept")]
        public Task<ActionResult<TransportOffering>> AcceptTransportOffering(int id)
        {
            return this._transportOfferingsService.AcceptTransport(id).Convert<TransportOffering>(this);
        }

        [HttpPatch("transports/{id}/decline")]
        public Task<ActionResult<TransportOffering>> DeclineTransportOffering(int id)
        {
            return this._transportOfferingsService.DeclineTransport(id).Convert<TransportOffering>(this);
        }
    }
}