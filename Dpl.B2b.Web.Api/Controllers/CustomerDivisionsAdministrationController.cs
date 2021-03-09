using System.Collections.Generic;
using System.Threading.Tasks;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [Authorize(Roles = "Manager")]
    public class CustomerDivisionsAdministrationController : ControllerBase
    {
        private readonly ICustomerDivisionsService _divisionsService;

        public CustomerDivisionsAdministrationController(ICustomerDivisionsService customersService)
        {
            _divisionsService = customersService;
        }
        
        [HttpGet("{id}")]
        public Task<ActionResult<CustomerDivision>> GetById(int id)
        {
            return _divisionsService.GetById(id).Convert<CustomerDivision>(this);
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<CustomerDivision>>> Get()
        {
            return _divisionsService.GetAll().Convert<IEnumerable<CustomerDivision>>(this);
        }

        [HttpPost]
        public Task<ActionResult<CustomerDivision>> Post([FromBody] CustomerDivisionCreateRequest request)
        {
            return _divisionsService.Create(request).Convert<CustomerDivision>(this);
        }

        [HttpPatch]
        public Task<ActionResult<CustomerDivision>> Patch([FromBody] CustomerDivisionUpdateRequest request)
        {
            return _divisionsService.Update(request).Convert<CustomerDivision>(this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerDivision>> Delete(int id)
        {
            var request = new CustomerDivisionDeleteRequest
            {
                Id = id
            };
            return await _divisionsService.Delete(request).Convert<CustomerDivision>(this);
        }
    }
}