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
    [Route("number-sequences")]
    [Authorize(Roles = "Manager")]
    public class NumberSequencesController : ControllerBase
    {
        private readonly INumberSequencesService _numberSequencesService;

        public NumberSequencesController(INumberSequencesService numberSequencesService)
        {
            _numberSequencesService = numberSequencesService;
        }

        [HttpGet("customer/{id}")]
        public async Task<ActionResult<IEnumerable<DocumentNumberSequence>>> GetByCustomerId(int id)
        {
            var request = new DocumentNumberSequenceSearchRequest
            {
                CustomerId = id
            };

            var response = (IWrappedResponse<IQueryable<DocumentNumberSequence>>) await _numberSequencesService.Search(request);
            
            var responseToConvert = new WrappedResponse<IEnumerable<DocumentNumberSequence>>
            {
                Data = response.Data.AsEnumerable(),
                Errors = response.Errors,
                Id = response.Id,
                State = response.State,
                Warnings = response.Warnings,
                ResultType = response.ResultType
            };
            
            return await Task.FromResult<IWrappedResponse>(responseToConvert).Convert<IEnumerable<DocumentNumberSequence>>(this);
        }
        
        [HttpGet]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions, [FromQuery] DocumentNumberSequenceSearchRequest request)
        {
            var response =
                (WrappedResponse<IQueryable<DocumentNumberSequence>>) await _numberSequencesService
                    .Search(request);
            return await DataSourceLoader.LoadAsync(response.Data, loadOptions);
        }
        
        [HttpPost]
        public async Task<ActionResult<DocumentNumberSequence>> Post([FromForm] string values)
        {
            var request = new DocumentNumberSequenceCreateRequest();
            JsonConvert.PopulateObject(values, request);
            return await _numberSequencesService.Create(request).Convert<DocumentNumberSequence>(this);
        }

        [HttpPut]
        public async Task<ActionResult<DocumentNumberSequence>> Put([FromForm] int key, [FromForm] string values)
        {
            var request = new ValuesUpdateRequest
            {
                Key = key,
                Values = values
            };

            return await _numberSequencesService.Update(request).Convert<DocumentNumberSequence>(this);
        }

        [HttpDelete]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<DocumentNumberSequence>> Delete(int key)
        {
            var request = new DocumentNumberSequenceDeleteRequest
            {
                Id = key
            };
            return await _numberSequencesService.Delete(request).Convert<DocumentNumberSequence>(this);
        }
    }
}