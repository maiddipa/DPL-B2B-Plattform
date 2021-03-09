using System.Threading.Tasks;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    //[Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsService _documentsService;

        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [HttpGet]
        public Task<ActionResult<IPaginationResult<Document>>> Get([FromQuery] DocumentSearchRequest request)
        {
            return this._documentsService.Search(request).Convert<IPaginationResult<Document>>(this);
        }

        [HttpGet("{id}/download-link")]
        public Task<ActionResult<string>> GetDocumentDownload(int id)
        {
            return this._documentsService.GetDocumentDownload(id).Convert<string>(this);
        }
    }
}