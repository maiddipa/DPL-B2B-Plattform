using AutoMapper;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Newtonsoft.Json;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    public class NumberSequencesService : BaseService, INumberSequencesService
    {
        private readonly IRepository<Olma.CustomerDivisionDocumentSetting> _customerDivisionDocumentSettingRepo;
        private readonly IRepository<Olma.DocumentNumberSequence> _documentNumberSequenceRepo;
        private static Random _random = new Random();
        private readonly IServiceProvider _serviceProvider;

        public NumberSequencesService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.CustomerDivisionDocumentSetting> customerDivisionDocumentSettingRepo,
            IRepository<Olma.DocumentNumberSequence> documentNumberSequenceRepo,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            _customerDivisionDocumentSettingRepo = customerDivisionDocumentSettingRepo;
            _documentNumberSequenceRepo = documentNumberSequenceRepo;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetProcessNumber(ProcessType processType, int id)
        {
            string prefix = string.Empty;
            string process = processType.ToString();

            for (int i = 0; i < process.Length; i++)
            {
                if (char.IsUpper(process[i]))
                {
                    prefix += process[i];
                }
            }

            StringBuilder consecutiveNumberBuilder = new StringBuilder(id.ToString());

            for (int i = 0; i < 5 - consecutiveNumberBuilder.Length; i++)
            {
                consecutiveNumberBuilder.Insert(0, "0");
            }

            string prefixPart = _random.Next(100, 999).ToString();
            string bodyPart = _random.Next(100, 999).ToString();

            return $"{prefix}{prefixPart}-{bodyPart}-{consecutiveNumberBuilder}";
        }

        public async Task<string> GetPostingRequestNumber(PostingRequestType requestType)
        {
            //TODO Implement real logic
            //HACK generate Random Number
            var number = _random.Next(maxValue: 9999);
            return "ONL-" + Convert.ToString(number + 10000);
        }

        public async Task<string> GetTransportNumber()
        {
            //TODO Implement real logic
            //HACK generate Random Number
            var number = _random.Next(maxValue: 9999);
            return "TR-" + Convert.ToString(number + 10000);
        }

        public async Task<string> GetDigitalCode()
        {
            const string chars = "ACDEFGHJKLNPRTUVWXYZ234679";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[_random.Next(s.Length)]).ToArray()) + "O";
        }

        public async Task<IWrappedResponse> Search(DocumentNumberSequenceSearchRequest request)
        {
            var query = _documentNumberSequenceRepo.FindAll().IgnoreQueryFilters().AsNoTracking().Where(ns => !ns.IsDeleted);

            if (request.CustomerId.HasValue)
            {
                query = query.Where(dns => dns.CustomerId == request.CustomerId);
                return Ok(query.ProjectTo<DocumentNumberSequence>(Mapper.ConfigurationProvider));
            }

            return new WrappedResponse
            {
                ResultType = ResultType.BadRequest,
                State = ErrorHandler.Create().AddMessage(new GeneralError()).GetServiceState()
            };
        }

        public async Task<string> GetDocumentNumber(DocumentTypeEnum documentType, int customerDivisionId)
        {
            var query = _customerDivisionDocumentSettingRepo.FindByCondition(ds => ds.DivisionId == customerDivisionId)
                .Include(dt => dt.DocumentType)
                .Include(ns => ns.DocumentNumberSequence)
                .Include(d => d.Division).ThenInclude(c => c.Customer);

            var divisionDocumentSetting = query.FirstOrDefault(t => t.DocumentType.Type == documentType);
            // DivisionDocumentSetting muss immer gesetzt werden. Am Anfang mit Defaultwerte.
            var documentNumberSequence = divisionDocumentSetting?.DocumentNumberSequence;
            var documentNumber = string.Empty;

            documentNumber += $"{documentNumberSequence.Prefix}{documentNumberSequence.SeparatorAfterPrefix}";

            if (documentNumberSequence.CanAddDocumentTypeShortName)
            {
                documentNumber += divisionDocumentSetting.DocumentType.ShortName;
            }

            var paddingCustomerNumber = string.Empty;
            if (documentNumberSequence.CanAddPaddingForCustomerNumber)
            {
                for (int i = 0; i < documentNumberSequence.PaddingLengthForCustomerNumber - 5; i++)
                {
                    paddingCustomerNumber += '0';
                }
            }

            documentNumber +=
                $"{paddingCustomerNumber}{divisionDocumentSetting.Division.Customer.RefErpCustomerNumber}{documentNumberSequence.PostfixForCustomerNumber}{documentNumberSequence.Separator}";

            if (documentNumberSequence.CanAddDivisionShortName)
            {
                documentNumber += divisionDocumentSetting.Division.ShortName;
            }

            documentNumber += documentNumberSequence.Separator;
            
            bool retrySave;
            do
            {
                var paddingCounter = string.Empty;
                var nextCounter = documentNumberSequence.Counter + 1;
                var counterLength = nextCounter.ToString().Length;
                if (documentNumberSequence.CanAddPaddingForCounter && counterLength < documentNumberSequence.PaddingLengthForCounter )
                {
                    for (int i = 0; i < documentNumberSequence.PaddingLengthForCounter - counterLength; i++)
                    {
                        paddingCounter += '0';
                    }
                }
                
                ++documentNumberSequence.Counter;
                retrySave = false;

                try
                {
                    _documentNumberSequenceRepo.Save();
                    documentNumber += $"{paddingCounter}{documentNumberSequence.Counter}";
                }
                catch (DbUpdateConcurrencyException e)
                {
                    retrySave = true;
                    await e.Entries.Single().ReloadAsync();
                }
                
            } while (retrySave);

            return documentNumber;
        }
        
         public async Task<IWrappedResponse> Create(DocumentNumberSequenceCreateRequest request)
        {
            var cmd = ServiceCommand<DocumentNumberSequence, Rules.DocumentNumberSequence.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.DocumentNumberSequence.Create.MainRule(request))
                .Then(CreateAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.DocumentNumberSequence.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaDocumentNumberSequence= Mapper.Map<Olma.DocumentNumberSequence>(request);
            _documentNumberSequenceRepo.Create(olmaDocumentNumberSequence);
            _documentNumberSequenceRepo.Save();
            var response = _documentNumberSequenceRepo
                .GetById<Olma.DocumentNumberSequence, DocumentNumberSequence>(olmaDocumentNumberSequence.Id);
            return response;
        }
        
        public async Task<IWrappedResponse> Update(ValuesUpdateRequest request)
        {
            var cmd = ServiceCommand<DocumentNumberSequence, Rules.DocumentNumberSequence.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.DocumentNumberSequence.Update.MainRule(request))
                .Then(UpdateAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> UpdateAction(Rules.DocumentNumberSequence.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaDocumentNumberSequence = mainRule.Context.DocumentNumberSequence;

            JsonConvert.PopulateObject(request.Values, olmaDocumentNumberSequence);

            _documentNumberSequenceRepo.Save();
            var documentNumberSequence =
                Mapper.Map<DocumentNumberSequence>(olmaDocumentNumberSequence);

            return Updated(documentNumberSequence);
        }

        public async Task<IWrappedResponse> Delete(DocumentNumberSequenceDeleteRequest request)
        {
            var cmd = ServiceCommand<DocumentNumberSequence, Rules.DocumentNumberSequence.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.DocumentNumberSequence.Delete.MainRule(request))
                .Then(DeleteAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.DocumentNumberSequence.Delete.MainRule mainRule)
        {
            var documentNumberSequence = mainRule.Context.DocumentNumberSequence;
            _documentNumberSequenceRepo.Delete(documentNumberSequence);
            _documentNumberSequenceRepo.Save();
            return Deleted<DocumentNumberSequence>(null);
        }
    }
}
