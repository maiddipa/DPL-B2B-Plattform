using System.Collections.Generic;
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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<Customer>>> Get()
        {
            return this._customersService.GetAll().Convert<IEnumerable<Customer>>(this);
        }

        [HttpPost]
        public Task<ActionResult<Customer>> Post([FromBody] CustomerCreateRequest request)
        {
            return this._customersService.Create(request).Convert<Customer>(this);
        }

        // [HttpPatch("{id}")]
        // public Task<ActionResult<Customer>> Patch(int id, [FromBody] CustomerUpdateRequest request)
        // {
        //     return this._customersService.Update(id, request).Convert<Customer>(this);
        // }
    }
}