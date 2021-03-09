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
    [Route("dpl-employee")]
    public class DplEmployeeController : ControllerBase
    {
        private readonly IDplEmployeeService _employeeeService;

        public DplEmployeeController(IDplEmployeeService employeeeService)
        {
            _employeeeService = employeeeService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<DplEmployeeCustomer>>> SearchCustomers(string searchTerm)
        {
            return this._employeeeService.SearchCustomers(searchTerm).Convert<IEnumerable<DplEmployeeCustomer>>(this);
        }
    }
}