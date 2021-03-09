using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;
using Ltms = Dpl.B2b.Dal.Ltms;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Dal.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dpl.B2b.BusinessLogic
{

    public class DocumentTypesService : IDocumentTypesService
    {
        private Dictionary<DocumentTypeEnum, int> dict;

        public DocumentTypesService(
            IServiceProvider serviceProvider
        )
        {
            this.PopulateCache(serviceProvider);
        }

        private void PopulateCache(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var olmaDocumentTypeRepo = scope.ServiceProvider.GetService<IRepository<Olma.DocumentType>>();
                dict = olmaDocumentTypeRepo.FindAll()
                 .AsNoTracking()
                 .ToDictionary(i => i.Type, i => i.Id);
            }
        }

        public int GetIdByDocumentType(DocumentTypeEnum documentType)
        {
            return dict[documentType];
        }
    }
}
