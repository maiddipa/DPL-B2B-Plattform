using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Authorization;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("document-settings")]
    [Authorize]
    public class DocumentSettingsController : ControllerBase
    {
        private readonly IDocumentSettingsService _documentSettingsService;

        public DocumentSettingsController(IDocumentSettingsService documentSettingsService)
        {
            _documentSettingsService = documentSettingsService;
        }

        [HttpGet]
        // HACK Removed outdated permission usage
        //[HasPermission(Permissions.DocumentRead)]
        public Task<ActionResult<DocumentSettings>> Get()
        {
            return this._documentSettingsService.GetAll().Convert<DocumentSettings>(this);
        }

        [HttpPatch("customer/{id}")]
        public Task<ActionResult<CustomerDocumentSettings>> PatchCustomerDocumentSettings(int id, [FromBody] CustomerDocumentSettingsUpdateRequest request)
        {
            return this._documentSettingsService.UpdateCustomerDocumentSettings(id, request).Convert<CustomerDocumentSettings>(this);
        }

        [HttpPatch("division/{id}")]
        public Task<ActionResult<CustomerDivisionDocumentSettings>> PatchDivisionCustomerDocumentSettings(int id, [FromBody] CustomerDivisionDocumentSettingsUpdateRequest request)
        {
            return this._documentSettingsService.UpdateDivisionCustomerDocumentSettings(id, request).Convert<CustomerDivisionDocumentSettings>(this);
        }
    }
}