#if DEBUG

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dpl.B2b.Web.Api.Extensions;
using System.Net.Http;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.Web.Api.Controllers
{
    /// <summary>
    /// DEBUG ONLY - DEBUG ONLY - DEBUG ONLY - DEBUG ONLY - DEBUG ONLY - DEBUG ONLY - DEBUG ONLY - DEBUG ONLY - DEBUG ONLY - DEBUG ONLY - 
    /// </summary>
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DownloadsController : ControllerBase
    {
        private readonly IRepository<Olma.Document> documentRepo;
        private readonly IStorageService storage;

        public DownloadsController(IRepository<Olma.Document> documentRepo, IStorageService storage)
        {
            this.documentRepo = documentRepo;
            this.storage = storage;
        }
        
        [HttpGet("{filename}")]
        public async Task<IActionResult> Get(string fileName)
        {
            // HACK this method only works for already created reports
            var file = documentRepo.FindAll()
                .SelectMany(i => i.Files)
                .Select(i => i.File)
                .Where(i => i.InternalFullPathAndName == fileName)
                .FirstOrDefault();

            var stream = storage.Read(file.Name).Result;

            // this allows the borwser to display the pdf in a tab
            return new FileStreamResult(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);

            // this automatically triggers a download
            //return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf, file.Name);
        }
    }
}

#endif