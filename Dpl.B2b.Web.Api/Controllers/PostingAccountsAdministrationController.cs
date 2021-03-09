using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dpl.B2b.Common.Enumerations;
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
    public class PostingAccountsAdministrationController : ControllerBase
    {
        private readonly IPostingAccountsService _postingAccountsService;

        public PostingAccountsAdministrationController(IPostingAccountsService postingAccountsService)
        {
            _postingAccountsService = postingAccountsService;
        }
        
        [HttpGet("ref-accounts/{number}")]
        public async Task<ActionResult<IEnumerable<LtmsAccount>>> GetLtmsAccountsByCustomerNumber(string number)
        {
            return await _postingAccountsService.GetLtmsAccountsByCustomerNumber(number).Convert<IEnumerable<LtmsAccount>>(this);
        }
        
        [HttpGet("customer/{id}")]
        public async Task<ActionResult<IEnumerable<PostingAccountAdministration>>> GetByCustomerId(int id)
        {
            var request = new PostingAccountsSearchRequest
            {
                CustomerId = id
            };
            return await _postingAccountsService.GetByCustomerId(request).Convert<IEnumerable<PostingAccountAdministration>>(this);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostingAccountAdministration>> GetById(int id)
        {
            var request = new PostingAccountsSearchRequest
            {
                PostingAccountId = id
            };
            return await _postingAccountsService.Search(request).Convert<PostingAccountAdministration>(this);
        }

        [HttpGet]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions, [FromQuery] PostingAccountsSearchRequest request)
        {
            var response = (WrappedResponse<IQueryable<PostingAccountAdministration>>) await _postingAccountsService.Search(request);
            return await DataSourceLoader.LoadAsync(response.Data, loadOptions);
        }
        
        [HttpPost]
        public async Task<ActionResult<PostingAccountAdministration>> Post([FromForm] string values)
        {
            var request = new PostingAccountsCreateRequest();
            JsonConvert.PopulateObject(values, request);
            return await _postingAccountsService.Create(request).Convert<PostingAccountAdministration>(this);
        }

        [HttpPut]
        public async Task<ActionResult<PostingAccountAdministration>> Put([FromForm] int key, [FromForm] string values)
        {
            var request = new ValuesUpdateRequest
            {
                Key = key,
                Values = values
            };
            
            return await _postingAccountsService.Update(request).Convert<PostingAccountAdministration>(this);
        }

        [HttpDelete]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<PostingAccountAdministration>> Delete([FromForm] int key)
        {
            var request = new PostingAccountsDeleteRequest {Id = key};
            return await _postingAccountsService.Delete(request).Convert<PostingAccountAdministration>(this);
        }
    }
}