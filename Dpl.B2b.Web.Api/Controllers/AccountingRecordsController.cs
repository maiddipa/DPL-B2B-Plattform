using System;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
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
    [Authorize]
    public class AccountingRecordsController : ControllerBase
    {
        private readonly IAccountingRecordsService _accountingRecordsService;

        public AccountingRecordsController(IAccountingRecordsService accountingRecordsService)
        {
            _accountingRecordsService = accountingRecordsService;
        }

        [HttpGet]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions,[FromQuery] AccountingRecordsSearchRequest request)
        {
            var response =
                await _accountingRecordsService.Search(request) as WrappedResponse<IQueryable<AccountingRecord>>;
            return await DataSourceLoader.LoadAsync(response?.Data, loadOptions);
        }

        [HttpGet("{id}")]
        public Task<ActionResult<AccountingRecord>> GetById(int id)
        {
            return this._accountingRecordsService.GetById(id).Convert<AccountingRecord>(this);
        }
    }
}