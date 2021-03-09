using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Contracts;
using Dpl.B2b.Web.Api.Authorization;
using Dpl.B2b.Web.Api.Authorization.Requirements;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [Authorize]
    public class LoadCarrierReceiptsController : ControllerBase
    {
        private readonly ILoadCarrierReceiptsService _loadCarrierReceiptsService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthorizationDataService _authData;

        public LoadCarrierReceiptsController(ILoadCarrierReceiptsService loadCarrierReceiptsService, IAuthorizationService authorizationService, IAuthorizationDataService authData)
        {
            _loadCarrierReceiptsService = loadCarrierReceiptsService;
            _authorizationService = authorizationService;
            _authData = authData;
        }
        
        [HttpGet]
        [HasPermission(typeof(AttributeRequirement<CanReadLoadCarrierReceiptRequirement>))]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions,[FromQuery] LoadCarrierReceiptsSearchRequest request)
        {
            var response = await _loadCarrierReceiptsService.Search(request) as WrappedResponse<IQueryable<LoadCarrierReceipt>>;
            return await DataSourceLoader.LoadAsync(response?.Data, loadOptions);
        }

        [HttpGet("{id}")]
        [HasPermission(typeof(AttributeRequirement<CanReadLoadCarrierReceiptRequirement>))]
        [Obsolete]
        public async Task<ActionResult<LoadCarrierReceipt>> GetById(int id)
        {
            var response =  _loadCarrierReceiptsService.GetById(id);
            return await response.Convert(this);
        }
        
        [HttpGet("{id}/sort-options")]
        [HasPermission(typeof(AttributeRequirement<CanReadLoadCarrierReceiptRequirement>))]
        public async Task<ActionResult<LoadCarrierReceiptSortingOption>> GetSortingOptionsByReceiptId(int id)
        {
            return await _loadCarrierReceiptsService.GetSortingOptionsByReceiptId(id).Convert<LoadCarrierReceiptSortingOption>(this);
        }
        

        [HttpPost]
        [HasPermission(typeof(AttributeRequirement<CanCreateLoadCarrierReceiptRequirement>))]
        public Task<ActionResult<LoadCarrierReceipt>> Post([FromBody] LoadCarrierReceiptsCreateRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._loadCarrierReceiptsService.Create(request).Convert<LoadCarrierReceipt>(this);
        }
      
       
        [HttpPatch("{id}/cancel")]
        [HasPermission(typeof(AttributeRequirement<CanCancelLoadCarrierReceiptRequirement>))]
        public Task<ActionResult<LoadCarrierReceipt>> PatchLoadCarrierReceiptCancel(int id,[FromBody] LoadCarrierReceiptsCancelRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._loadCarrierReceiptsService.Cancel(id,request).Convert<LoadCarrierReceipt>(this);
        }

        [HttpPatch("{id}/sortingrequired")]
        [HasPermission(typeof(AttributeRequirement<CanCancelLoadCarrierReceiptRequirement>))] //TODO Requirement for Set IsSortingRequired on LoadCarrierReceipts
        public Task<ActionResult<LoadCarrierReceipt>> PatchLoadCarrierReceiptIsSortingRequired(int id,[FromBody] LoadCarrierReceiptsUpdateIsSortingRequiredRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._loadCarrierReceiptsService.UpdateIsSortingRequired(id, request)
                .Convert<LoadCarrierReceipt>(this);
        }
    }
}
