using System.Collections.Generic;
using System.Threading.Tasks;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Contracts;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("partner-directories")]
    public class PartnerDirectoriesController : ControllerBase
    {
        private readonly IPartnerDirectoriesService _partnerDirectoriesService;

        public PartnerDirectoriesController(IPartnerDirectoriesService partnerDirectoriesService)
        {
            _partnerDirectoriesService = partnerDirectoriesService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<PartnerDirectory>>> Get()
        {
            return this._partnerDirectoriesService.GetAll().Convert<IEnumerable<PartnerDirectory>>(this);
        }
    }
}
