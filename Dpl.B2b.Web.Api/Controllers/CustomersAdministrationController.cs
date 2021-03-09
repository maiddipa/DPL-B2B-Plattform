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
    [Authorize(Roles = "Manager")]
    public class CustomersAdministrationController : ControllerBase
    {
        private readonly ICustomersService _customersService;

        public CustomersAdministrationController(ICustomersService customersService)
        {
            _customersService = customersService;
        }
        
        [HttpGet("{id}")]
        public Task<ActionResult<Customer>> GetById(int id)
        {
            return _customersService.GetById(id).Convert<Customer>(this);
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<Customer>>> Get()
        {
            return _customersService.GetAll().Convert<IEnumerable<Customer>>(this);
        }

        [HttpPost]
        public Task<ActionResult<Customer>> Post([FromBody] CustomerCreateRequest request)
        {
            return _customersService.Create(request).Convert<Customer>(this);
        }

        [HttpPatch]
        public Task<ActionResult<Customer>> Patch([FromBody] CustomerUpdateRequest request)
        {
            return _customersService.Update(request).Convert<Customer>(this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            var request = new CustomerDeleteRequest
            {
                Id = id
            };
            return await _customersService.Delete(request).Convert<Customer>(this);
        }
    }
}