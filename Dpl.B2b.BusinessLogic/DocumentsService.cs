using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class DocumentsService : BaseService, IDocumentsService
    {
        private readonly IStorageService _storage;
        private readonly IRepository<Olma.Document> _olmaDocumentRepo;
        private readonly Reporting.IReportGeneratorService _reportGeneratorService;
        private readonly IServiceProvider _serviceProvider;

        public DocumentsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IStorageService storage,
            IRepository<Olma.Document> olmaDocumentRepo,
            Reporting.IReportGeneratorService reportGeneratorService, 
            IServiceProvider serviceProvider
            
        ) : base(authData, mapper)
        {
            _storage = storage;
            _olmaDocumentRepo = olmaDocumentRepo;
            _reportGeneratorService = reportGeneratorService;
            _serviceProvider = serviceProvider;
        }

        public async Task<IWrappedResponse> GetDocumentDownload(int id, DocumentFileType type = DocumentFileType.Copy)
        {
            var cmd = ServiceCommand<Document, Rules.Documents.GetDocumentDownload.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Documents.GetDocumentDownload.MainRule((id, type)))
                .Then(GetDocumentDownloadAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> GetDocumentDownloadAction(Rules.Documents.GetDocumentDownload.MainRule mainRule)
        {
            var result = Ok(mainRule.Context.DownloadLink);
            return result;
        }

        public async Task<IWrappedResponse> Search(DocumentSearchRequest request)
        {
            var cmd = ServiceCommand<Document, Rules.Documents.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Documents.Search.MainRule(request))
                .Then(SearchAction);

            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> SearchAction(Rules.Documents.Search.MainRule mainRule)
        {
            var result = Ok(mainRule.Context.DocumentPagination);

            return await Task.FromResult(result);
        }
    }
}
