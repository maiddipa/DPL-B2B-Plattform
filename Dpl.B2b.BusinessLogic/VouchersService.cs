using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExpress.DataProcessing;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Reporting;
using Dpl.B2b.BusinessLogic.Rules.Vouchers;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Document;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.ReportGenerator;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Voucher;
using Dpl.B2b.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class VouchersService : BaseService, IVouchersService
    {
        private readonly Dal.OlmaDbContext _olmaDbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Olma.Voucher> _olmaVoucherRepo;
        private readonly IRepository<Olma.CustomerPartner> _olmaCustomerPartnerRepo;
        private readonly IRepository<Olma.Partner> _olmaPartnerRepo;
        private readonly IRepository<Olma.PartnerDirectory> _olmaPartnerDirectoryRepo;
        private readonly INumberSequencesService _numberSequencesService;
        private readonly IDocumentTypesService _documentTypesService;
        private readonly IDocumentsService _documentsService;
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly IPostingRequestsService _postingRequestsService;
        private readonly ISynchronizationsService _synchronizationsService;

        public VouchersService(
            IAuthorizationDataService authData,
            IAuthorizationService authService,
            IMapper mapper,
            Dal.OlmaDbContext olmaDbContext,
            IServiceProvider serviceProvider,
            IRepository<Olma.Voucher> olmaVoucherRepo,
            IRepository<Olma.CustomerPartner> olmaCustomerPartnerRepo,
            IRepository<Olma.PartnerDirectory> olmaPartnerDirectoryRepo,
            IRepository<Olma.Partner> olmaPartnerRepo,
            INumberSequencesService numberSequencesService,
            IDocumentTypesService documentTypesService,
            IDocumentsService documentsService,
            IPostingRequestsService postingRequestsService,
            Reporting.IReportGeneratorService reportGeneratorService, 
            ISynchronizationsService synchronizationsService) : base(authData, authService, mapper)
        {
            _olmaDbContext = olmaDbContext;
            _serviceProvider = serviceProvider;
            _olmaVoucherRepo = olmaVoucherRepo;
            _olmaCustomerPartnerRepo = olmaCustomerPartnerRepo;
            _olmaPartnerDirectoryRepo = olmaPartnerDirectoryRepo;
            _olmaPartnerRepo = olmaPartnerRepo;
            _numberSequencesService = numberSequencesService;
            _documentTypesService = documentTypesService;
            _documentsService = documentsService;
            _reportGeneratorService = reportGeneratorService;
            _synchronizationsService = synchronizationsService;
            _postingRequestsService = postingRequestsService;
        }

        public async Task<IWrappedResponse> Summary(VouchersSearchRequest request)
        {
            var cmd = ServiceCommand<IWrappedResponse, Rules.Vouchers.Summary.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Vouchers.Summary.MainRule(request))
                .Then(SummaryAction);

            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> SummaryAction(Rules.Vouchers.Summary.MainRule rule)
        {
            return Ok(rule.Context.Rules.VouchersSummaryValidRule.Context.Result);
        }

        public async Task<IWrappedResponse> GetById(int id)
        {
            var cmd = ServiceCommand<IWrappedResponse, Rules.Vouchers.GetById.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Vouchers.GetById.MainRule(id))
                .Then(GetByIdAction);

            return await cmd.Execute();
        }

        public async Task<IWrappedResponse> GetByIdAction(Rules.Vouchers.GetById.MainRule rule)
        {
            var voucher = rule.Context.Voucher;
            var voucherDto = Mapper.Map<Voucher>(voucher);
            
            return Ok(voucherDto);
        }

        public async Task<IWrappedResponse> GetByNumber(string number)
        {
            var cmd = ServiceCommand<IWrappedResponse, Rules.Vouchers.GetByNumber.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Vouchers.GetByNumber.MainRule(number))
                .Then(GetByNumberAction);

            return await cmd.Execute();
        }
        
        public async Task<IWrappedResponse> GetByNumberAction(Rules.Vouchers.GetByNumber.MainRule rule)
        {
            var voucher = rule.Context.Voucher;
            var voucherDto = Mapper.Map<Voucher>(voucher);
            
            return Ok(voucherDto);
        }

        public async Task<IWrappedResponse> Cancel(int id, VouchersCancelRequest request)
        {
            var cmd = ServiceCommand<IWrappedResponse, Rules.Vouchers.Cancel.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Vouchers.Cancel.MainRule((id, request)))
                .Then(CancelAction);

            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> CancelAction(Rules.Vouchers.Cancel.MainRule rule)
        {
            var id = rule.Context.Parent.Id;
            var request = rule.Context.Parent.Request;
            var voucher = rule.Context.Voucher;
            
            voucher.Status = VoucherStatus.Canceled;
            if (voucher.Document != null)
            {
                voucher.Document.CancellationReason = request.Reason;
                voucher.Document.StateId = 255;
            }
            
            var response = _olmaVoucherRepo.Update<Olma.Voucher, Olma.Voucher, Voucher>(id, voucher);

            var sendVoucherRequestResult = _synchronizationsService.SendVoucherRequestsAsync(new VoucherSyncRequest
            {
                VoucherCancelSyncRequest = new VoucherCancelSyncRequest
                {
                    Id = voucher.RowGuid,
                    CancellationReason = request.Reason,
                    Number = voucher.Document?.Number
                }
            });
            
            return response;
        }

        public async Task<IWrappedResponse> Create(VouchersCreateRequest request)
        {
            // Use ServiceCommand to create typically execution pattern
            var cmd = ServiceCommand<PostingRequest, Rules.Vouchers.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Vouchers.Create.MainRule(request))
                .Then(CreateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.Vouchers.Create.MainRule rule)
        {
            var voucher = rule.Context.Rules.Voucher.Context.Voucher;
            var digitalCode = rule.Context.ExpressCode;
            
            #region map partner to CustomerPartner

            // if not exists: CustomerPartner has to copied from global Partner and stored in special 'ExpressCode' PartnerDirectory

            var partnerDictionary = new Dictionary<EntryType, Guid>
            {
                {EntryType.Recipient, rule.Context.RecipientGuid},
                {EntryType.Shipper, rule.Context.ShipperGuid},
                {EntryType.SubShipper, rule.Context.SubShipperGuid},
                {EntryType.Supplier, rule.Context.SupplierGuid}
            };

            //If Guid identifies a Partner, gets the matching CustomerPartner or creates a new one if does not exist

            var customerPartnerDictionary = ConvertToCustomerPartner(partnerDictionary);

            foreach (var customerPartner in customerPartnerDictionary)
            {
                switch (customerPartner.Key)
                {
                    case EntryType.Default:
                        break;
                    case EntryType.Recipient:
                        voucher.Recipient = customerPartner.Value;
                        break;
                    case EntryType.Supplier:
                        voucher.Supplier = customerPartner.Value;
                        break;
                    case EntryType.Shipper:
                        voucher.Shipper = customerPartner.Value;
                        break;
                    case EntryType.SubShipper:
                        voucher.SubShipper = customerPartner.Value;
                        break;
                    default:
                        return new WrappedResponse
                        {
                            ResultType = ResultType.Failed,
                            State = ErrorHandler.Create()
                                .AddMessage(new CustomerPartnerNotFoundError())
                                .GetServiceState()
                        };
                }
            }

            #endregion

            var documentType = rule.Context.Rules.DocumentType.Context.DocumentType;

            voucher.ReceivingCustomerId = voucher.Recipient.Partner?.Customer?.Id;
            voucher.ReceivingPostingAccountId = voucher.Recipient.Partner?.DefaultPostingAccountId;

            var strategy = _olmaDbContext.Database.CreateExecutionStrategy();
            var result = await strategy.ExecuteAsync(operation: async () =>
            {
                await using var ctxTransaction = await _olmaDbContext.Database.BeginTransactionAsync();
               
                var documentNumber =
                    _numberSequencesService.GetDocumentNumber(documentType, rule.Context.CustomerDivisionId);
                var typeId = _documentTypesService.GetIdByDocumentType(documentType);

                voucher.Document = new Olma.Document()
                {
                    Number = documentNumber.Result,
                    TypeId = typeId,
                    StateId = 1,
                    CustomerDivisionId = rule.Context.CustomerDivisionId,
                    IssuedDateTime = DateTime.UtcNow,
                    LanguageId = rule.Context.PrintLanguageId,
                };

                var wrappedResponse = _olmaVoucherRepo.Create<Olma.Voucher, Olma.Voucher, Voucher>(voucher);

                var reportGeneratorServiceResponse = _reportGeneratorService.GenerateReportForLanguage(
                    wrappedResponse.Data.DocumentId,
                    rule.Context.PrintLanguageId,
                    rule.Context.PrintCount - 1,
                    rule.Context.PrintDateTimeOffset);
                if (reportGeneratorServiceResponse.ResultType != ResultType.Created)
                {
                    await ctxTransaction.RollbackAsync();
                    return new WrappedResponse<Voucher>
                    {
                        ResultType = ResultType.Failed,
                        State = ErrorHandler.Create().AddMessage(new GenerateReportError()).GetServiceState()
                    };
                }

                var downloadLink =
                    (WrappedResponse<string>) await _documentsService.GetDocumentDownload(
                        wrappedResponse.Data.DocumentId, DocumentFileType.Composite);
                if (downloadLink.ResultType != ResultType.Ok)
                {
                    await ctxTransaction.RollbackAsync();
                    return new WrappedResponse<Voucher>
                    {
                        ResultType = ResultType.Failed,
                        State = ErrorHandler.Create().AddMessage(new DocumentLinkError()).GetServiceState()
                    };
                }
                wrappedResponse.Data.DownloadLink = downloadLink.Data;
                var refLtmsTransactionId = Guid.NewGuid();

                #region Send Voucher to LTMS

                //Make sure voucher.Position.Loadcarrier is loaded, because RefLtmsPalettId is need when
                //mapping to VoucherCreateSyncRequest
                foreach (var position in voucher.Positions)
                {
                    _olmaDbContext.Entry(position).Reference(c => c.LoadCarrier).Load();
                }

                //Make sure voucher.ReasonType is loaded, because RefLtmsReasonTypeId is needed when mapping to VoucherCreateSyncRequest
                _olmaDbContext.Entry(voucher).Reference(r => r.ReasonType).Load();
                var voucherCreateRequest = Mapper.Map<VoucherCreateSyncRequest>(voucher);
                var voucherSyncRequest = new VoucherSyncRequest {VoucherCreateSyncRequest = voucherCreateRequest};
                voucherSyncRequest.VoucherCreateSyncRequest.RefLtmsTransactionRowGuid = refLtmsTransactionId;
                var synchronizationsServiceResponse =
                    await _synchronizationsService.SendVoucherRequestsAsync(voucherSyncRequest);
                if (synchronizationsServiceResponse.ResultType == ResultType.Failed)
                {
                    await ctxTransaction.RollbackAsync();
                    return new WrappedResponse<Voucher>
                    {
                        ResultType = ResultType.Failed,
                        State = ErrorHandler.Create().AddMessage(new TechnicalError()).GetServiceState()
                    };
                }
                    
                #endregion

                if (rule.Context.Type == VoucherType.Direct && rule.Context.CustomerDivision != null)
                {
                    var postingRequestsCreateRequest = new PostingRequestsCreateRequest
                    {
                        Type = PostingRequestType.Charge, //HACK Only to fix UI, from LTMS W
                        ReferenceNumber = voucher.Document.Number,
                        Reason = PostingRequestReason.Voucher,
                        VoucherId = wrappedResponse.Data.Id,
                        RefLtmsProcessId = Guid.NewGuid(),
                        RefLtmsProcessTypeId = (int) RefLtmsProcessType.DirectBooking,
                        RefLtmsTransactionId = refLtmsTransactionId,
                        // ReSharper disable once PossibleInvalidOperationException -> secured by rule
                        PostingAccountId = rule.Context.CustomerDivision.PostingAccountId.Value,
                        // ReSharper disable once PossibleInvalidOperationException -> secured by rule
                        SourceRefLtmsAccountId =
                            (int) voucher.Recipient.Partner?.DefaultPostingAccount.RefLtmsAccountId,
                        DestinationRefLtmsAccountId =
                            rule.Context.CustomerDivision.PostingAccount.RefLtmsAccountId,
                        Positions = voucher.Positions.Select(v => new PostingRequestPosition()
                        {
                            LoadCarrierId = v.LoadCarrierId,
                            LoadCarrierQuantity = v.LoadCarrierQuantity
                        }),
                        Note = voucher.Shipper?.CompanyName,
                        DigitalCode = digitalCode
                    };

                    var postingRequestsServiceResponse =
                        (IWrappedResponse<IEnumerable<PostingRequest>>) await _postingRequestsService.Create(
                            postingRequestsCreateRequest);

                    if (postingRequestsServiceResponse.ResultType != ResultType.Created)
                    {
                        await ctxTransaction.RollbackAsync();
                        return new WrappedResponse<Voucher>
                        {
                            ResultType = ResultType.Failed,
                            State = ErrorHandler.Create().AddMessage(new TechnicalError()).GetServiceState()
                        };
                    }
                }
                await ctxTransaction.CommitAsync();
                    
                return wrappedResponse;
            });
            return result;
        }

        public async Task<IWrappedResponse> Update(int id, VouchersUpdateRequest request)
        {
            var cmd = ServiceCommand<IWrappedResponse<Contracts.Models.Voucher>, Rules.Vouchers.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Vouchers.Update.MainRule((id,request)))
                .Then(UpdateAction);

            return await cmd.Execute();
        }
        
        private Task<IWrappedResponse> UpdateAction(Rules.Vouchers.Update.MainRule rule)
        {
            var result = _olmaVoucherRepo.Update<Olma.Voucher, VouchersUpdateRequest, Contracts.Models.Voucher>(rule.Context.Parent.id, rule.Context.Parent.request) ;
            var task = Task.Factory.StartNew<IWrappedResponse>(() => result);
            return task;
        }

        public async Task<IWrappedResponse<Contracts.Models.Voucher>> AddToSubmission(int id, VouchersAddToSubmissionRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<IWrappedResponse<Contracts.Models.Voucher>> RemoveFromSubmission(int id, VouchersRemoveFromSubmissionRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IWrappedResponse> Search(VouchersSearchRequest request)
        {
            var cmd = ServiceCommand<IWrappedResponse, Rules.Vouchers.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Vouchers.Search.MainRule(request))
                .Then(SearchAction);

            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> SearchAction(Rules.Vouchers.Search.MainRule rule)
        {
            return Ok(rule.Context.PaginationResult);
        }
        
        #region Helper

        private Olma.CustomerPartner FindOrAddCustomerPartnerByPartner(Olma.Partner partner)
        {

            //TODO Use PartnerService to create new CustomerPartner
            var customerPartner =
                _olmaCustomerPartnerRepo.FindByCondition(cp => cp.PartnerId != null && cp.PartnerId == partner.Id).SingleOrDefault();//TODO Discuss if there are more then one CustomerPartner possible
            if (customerPartner != null)
                return customerPartner;

            // if does not exist Create new customrerPartner
            customerPartner = new Olma.CustomerPartner()
            {
                Type = partner.Type,
                CompanyName = partner.CompanyName,
                Address = partner.DefaultAddress,
                LoadingLocations = partner.LoadingLocations,
                PartnerId = partner.Id,
                Partner = partner,
                RowGuid = Guid.NewGuid()
            };

            var directory = FindOrAddDirectoryByType(Olma.PartnerDirectoryType.ExpressCodeImportCustom, "DigitalCode Import");

            //Grant Access to Directory Entry
            var directoryAccess = new Olma.CustomerPartnerDirectoryAccess() { Partner = customerPartner };
            directory.CustomerPartnerDirectoryAccesses?.Add(directoryAccess);
            _olmaCustomerPartnerRepo.Create(customerPartner);
            _olmaCustomerPartnerRepo.Save();
            return customerPartner;
        }

        private Olma.PartnerDirectory FindOrAddDirectoryByType(Olma.PartnerDirectoryType partnerDirectoryType, string directoryName)
        {
            var directory = _olmaPartnerDirectoryRepo
                .FindByCondition(d => d.Type == Olma.PartnerDirectoryType.ExpressCodeImportCustom)
                .Include(d => d.CustomerPartnerDirectoryAccesses)
                .Include(d => d.OrganizationPartnerDirectories)
                .SingleOrDefault();

            // Create if does not exist
            if (directory != null)
                return directory;
            if (string.IsNullOrWhiteSpace(directoryName))
                directoryName = "Default Directory";
            directory = new Olma.PartnerDirectory()
            {
                Type = partnerDirectoryType,
                Name = directoryName,
                CustomerPartnerDirectoryAccesses = new List<Olma.CustomerPartnerDirectoryAccess>(),
                OrganizationPartnerDirectories = new List<Olma.OrganizationPartnerDirectory>() { new Olma.OrganizationPartnerDirectory() { OrganizationId = AuthData.GetOrganizationId() } }
            };
            _olmaPartnerDirectoryRepo.Create(directory);
            _olmaCustomerPartnerRepo.Save();
            return directory;
        }

        private Dictionary<EntryType, Olma.CustomerPartner> ConvertToCustomerPartner(
            Dictionary<EntryType, Guid> guidDictionary) //TODO Maybe change to local enum
        {
            var dict = new Dictionary<EntryType, Olma.CustomerPartner>();

            foreach (var element in guidDictionary.Where(g => g.Value != Guid.Empty))
            {
                //First search in CustomerPartners in case the Guid is from a CustomerPartner
                var customerPartner = _olmaCustomerPartnerRepo.FindByCondition(cp =>
                        cp.RowGuid == element.Value && !cp.IsDeleted)
                    .IgnoreQueryFilters()
                    .Include(cp => cp.Address)
                    .Include(cp => cp.Partner).ThenInclude(p => p.DefaultPostingAccount)
                    .Include(cp => cp.Partner).ThenInclude(p => p.Customer)
                    .FirstOrDefault();

                if (customerPartner != null)
                {
                    dict.Add(element.Key, customerPartner);
                }
                else //If not found search in Partner and find or create customerPartner
                {
                    var partner = _olmaPartnerRepo.FindByCondition(p =>
                            p.RowGuid == element.Value && !p.IsDeleted)
                        .IgnoreQueryFilters()
                        .Include(p => p.DefaultPostingAccount)
                        .Include(p => p.Customer)
                        .FirstOrDefault();
                    if (partner == null)
                        throw new Exception($"Partner with Guid {element.Value} could not be found");
                    customerPartner = FindOrAddCustomerPartnerByPartner(partner);
                    dict.Add(element.Key, customerPartner);
                }
            }

            return dict;
        }

        private enum EntryType
        {
            Default,
            Recipient,
            Supplier,
            Shipper,
            SubShipper
        }

        #endregion
    }
}
