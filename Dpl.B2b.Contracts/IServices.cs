using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Models;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dpl.B2b.Contracts
{
    public interface IAccountingRecordsService
    {
        Task<IWrappedResponse> Search(AccountingRecordsSearchRequest request);
        Task<IWrappedResponse> GetById(int id);
    }

    public interface IDplEmployeeService
    {
        Task<IWrappedResponse> SearchCustomers(string searchTerm);
    }

    public interface IAuthorizationResourceDataService<TDbEntity, TResourceData>
        where TDbEntity : class
        where TResourceData : class
    {
        TResourceData Get(int id);
    }
    
    public interface IAddressesService
    {
        Task<IWrappedResponse> GetCountries();
        Task<IWrappedResponse> GetStatesByCountryId(int id);
        Task<IWrappedResponse> Search(AddressesSearchRequest request);
        Task<IWrappedResponse> Create(AddressesCreateRequest request);
        Task<IWrappedResponse> Update(ValuesUpdateRequest request);
        Task<IWrappedResponse> Delete(AddressesDeleteRequest request);
    }

    public interface IBalanceTransfersService
    {
        [Obsolete]
        Task<IWrappedResponse<IPaginationResult<BalanceTransfer>>> Search(BalanceTransferSearchRequest request);
        [Obsolete]
        Task<IWrappedResponse<BalanceTransfer>> GetById(int id);
        Task<IWrappedResponse> Create(BalanceTransferCreateRequest request);
        [Obsolete]
        Task<IWrappedResponse<BalanceTransfer>> Cancel(int id, BalanceTransferCancelRequest request);
    }

    public interface ICountriesService
    {
        Task<IWrappedResponse> GetAll();
    }


    public interface IChatUsersService
    {
        Task<IWrappedResponse<IEnumerable<ChatUser>>> Search(ChatUsersSearchRequest request);

        // TODO implement remainig methods on chat user service
        //Task<IWrappedResponse<Customer>> Create(CustomerCreateRequest request);
        //Task<IWrappedResponse<Customer>> Update(int id, CustomerUpdateRequest request);
    }

    public interface ICustomersService
    {
        Task<IWrappedResponse> GetAll();
        Task<IWrappedResponse> GetById(int id);
        Task<IWrappedResponse> Create(CustomerCreateRequest request);
        Task<IWrappedResponse> Update(CustomerUpdateRequest request);
        Task<IWrappedResponse> Delete(CustomerDeleteRequest request);
    }
    
    public interface ICustomerDivisionsService
    {
        Task<IWrappedResponse> GetAll();
        Task<IWrappedResponse> GetById(int id);
        Task<IWrappedResponse> Create(CustomerDivisionCreateRequest request);
        Task<IWrappedResponse> Update(CustomerDivisionUpdateRequest request);
        Task<IWrappedResponse> Delete(CustomerDivisionDeleteRequest request);
    }

    public interface IDocumentsService
    {
        Task<IWrappedResponse> Search(DocumentSearchRequest request);

        Task<IWrappedResponse> GetDocumentDownload(int id, DocumentFileType type = DocumentFileType.Copy);

        //Task<IWrappedResponse<string>> GetDocumentDownload(int id);
    }

    public interface IDivisionDocumentSettingsService
    {
        Task<IWrappedResponse> GetDocumentTypes();
        Task<IWrappedResponse> Search(CustomerDivisionDocumentSettingSearchRequest request);
        Task<IWrappedResponse> Create(CustomerDivisionDocumentSettingCreateRequest request);
        Task<IWrappedResponse> Update(ValuesUpdateRequest request);
        Task<IWrappedResponse> Delete(CustomerDivisionDocumentSettingDeleteRequest request);
    }
    
    public interface ICustomerDocumentSettingsService
    {
        Task<IWrappedResponse> Search(CustomerDocumentSettingSearchRequest request);
        Task<IWrappedResponse> Create(CustomerDocumentSettingCreateRequest request);
        Task<IWrappedResponse> Update(ValuesUpdateRequest request);
        Task<IWrappedResponse> Delete(CustomerDocumentSettingDeleteRequest request);
    }

    public interface IDocumentSettingsService
    {
        Task<IWrappedResponse> GetAll();

        Task<IWrappedResponse> UpdateCustomerDocumentSettings(int customerId, CustomerDocumentSettingsUpdateRequest request);
        Task<IWrappedResponse> UpdateDivisionCustomerDocumentSettings(int divisionId, CustomerDivisionDocumentSettingsUpdateRequest request);
    }
    public interface IDocumentTypesService
    {
        int GetIdByDocumentType(DocumentTypeEnum documentType);
    }

    public interface IExpressCodesService
    {
        Task<IWrappedResponse> GetAll();
        //Task<IWrappedResponse<ExpressCode>> GetById(int id);
        Task<IWrappedResponse> GetByCode(ExpressCodesSearchRequest request);
        Task<IWrappedResponse> Cancel(int id);
        Task<IWrappedResponse> Create(ExpressCodeCreateRequest request);
        Task<IWrappedResponse> Update(int id,ExpressCodeUpdateRequest request);
        Task<IWrappedResponse> Search(ExpressCodesSearchRequest request);
    }
    public interface IImportService
    {
        Task<IWrappedResponse> ImportCustomerPartners(int directoryId, IEnumerable<CustomerPartnerImportRecord> records);
    }

    public interface ILoadCarrierOfferingsService
    {
        Task<IWrappedResponse> Search(LoadCarrierOfferingsSearchRequest request);
    }

    public interface ILoadCarrierReceiptsService
    {
        Task<IWrappedResponse> Search(LoadCarrierReceiptsSearchRequest request);
        [Obsolete]
        Task<IWrappedResponse> GetById(int id);
        Task<IWrappedResponse> GetSortingOptionsByReceiptId(int id);
        Task<IWrappedResponse> Create(LoadCarrierReceiptsCreateRequest request);
        [Obsolete]
        Task<IWrappedResponse> Cancel(int id, LoadCarrierReceiptsCancelRequest request);
        Task<IWrappedResponse> UpdateIsSortingRequired(int id, LoadCarrierReceiptsUpdateIsSortingRequiredRequest request);
    }

    public interface ILoadCarriersService
    {
        Task<IWrappedResponse> GetAll();
    }


    public interface ILoadCarrierSortingService
    {
        Task<IWrappedResponse> Create(LoadCarrierSortingCreateRequest request);
        Task<IWrappedResponse> GetByLoadCarrierReceiptId(int id);
    }

    public interface ILoadingLocationsService
    {
        Task<IWrappedResponse> GetById(int id);
        Task<IWrappedResponse> Create(LoadingLocationsCreateRequest request);
        Task<IWrappedResponse> Update(LoadingLocationsUpdateRequest request);
        Task<IWrappedResponse> Delete(LoadingLocationsDeleteRequest request);
        Task<IWrappedResponse> Search(LoadingLocationsSearchRequest request);
        [Obsolete]
        Task<IWrappedResponse> Search(LoadingLocationSearchRequest request);
        [Obsolete]
        Task<IWrappedResponse<LoadingLocation>> Patch(int id, UpdateLoadingLocationRequest request);
    }

    public interface ILocalizationService
    {
        int GetGermanLanguageId();
        IList<Language> GetSupportedLanguages();
        string GetLocale(int languageId);
        string GetLocalizationTextForEntity(int languageId, string localizationItemName, int id, string fieldName = null);
        string GetLocalizationTextForEnum<T>(int languageId, T value, string fieldName = null) where T : System.Enum;
    }

    public interface ILocalizationUpdateService
    {
        Task GenerateLocalizableEntries();
        Task UpdateLocalizationTexts();
        Task ExportLocalizationsToPath(string path);
        Task UploadLocalizationsFromFile(string filePath);

        Task DeleteFrontendTermsWithoutContext();
    }

    public interface ILtmsReferenceLookupService
    {
        int GetLtmsAccountId(int olmaPostingAccountId);

        IDictionary<int, int> GetLtmsAccountIds(int[] accountIds);
        IDictionary<Tuple<short, short>, int> GetOlmaLoadCarrierIds(Tuple<short, short>[] articleAndQualityIds);

        IDictionary<int, Tuple<short, short, short>> GetLtmsPalletInfos(int[] loadCarrierIds = null);
        IDictionary<int, short> GetLtmsArticleIds(int[] loadCarrierTypeIds);


        IDictionary<int, int> GetOlmaPostingAccountIds(int[] accountIds);
    }

    public interface IOrganizationsService
    {
        Task<IWrappedResponse> GetOrganizationById(int id);
        Task<IWrappedResponse> GetById(int id);
        Task<IWrappedResponse> GetAll();
        Task<IWrappedResponse> Create(OrganizationCreateRequest request);
        Task<IWrappedResponse> Update(OrganizationUpdateRequest request);
        Task<IWrappedResponse> Delete(OrganizationDeleteRequest request);
    }

    public interface IMasterDataService
    {
        Task<IWrappedResponse> Get();
    }
    public interface IMapsService
    {
        Task<Maps.IGeocodedLocation> Geocode(Address address);
        Task<List<MapsRoutingResult>> GetDirections(List<Tuple<Point, Point>> points);
        Task<long?> GetRoutedDistance(Point from, Point to);
    }

    public interface INumberSequencesService
    {
        Task<string> GetDocumentNumber(DocumentTypeEnum documentType, int customerDivisionId);
        Task<string> GetProcessNumber(ProcessType processType, int id);
        Task<string> GetTransportNumber();
        Task<string> GetDigitalCode();
        Task<IWrappedResponse> Search(DocumentNumberSequenceSearchRequest request);
        Task<IWrappedResponse> Create(DocumentNumberSequenceCreateRequest request);
        Task<IWrappedResponse> Update(ValuesUpdateRequest request);
        Task<IWrappedResponse> Delete(DocumentNumberSequenceDeleteRequest request);
    }

    public interface IPostingAccountsService
    {
        Task<IWrappedResponse> GetAll();
        Task<IWrappedResponse> GetByCustomerId(int customerId);
        Task<IWrappedResponse> GetByCustomerId(PostingAccountsSearchRequest request);
        Task<IWrappedResponse> GetLtmsAccountsByCustomerNumber(string number);
        Task<IWrappedResponse> Create(PostingAccountsCreateRequest request);
        Task<IWrappedResponse> Update(ValuesUpdateRequest request);
        Task<IWrappedResponse> GetBalances(int id);
        Task<IWrappedResponse> GetAllowedDestinationAccounts();
        Task<IWrappedResponse> Delete(PostingAccountsDeleteRequest request);
        Task<IWrappedResponse> Search(PostingAccountsSearchRequest request);


        // used by CLI to insert calculated balances
        Task GenerateCalculatedBalances();
    }

    public interface IPostingAccountBalancesService
    {
        Task<IWrappedResponse> Search(BalancesSearchRequest request);
    }
    
    public interface IPostingAccountConditionsService
    {
        Task<IWrappedResponse<IEnumerable<PostingAccountCondition>>> Search(PostingAccountConditionsSearchRequest request);
    }

    public interface IOrderLoadsService
    {
        Task<IWrappedResponse> Search(OrderLoadSearchRequest request);
        Task<IWrappedResponse> Cancel(int id, OrderLoadCancelRequest request);        
    }

    public interface IOrderMatchesService
    {
        [Obsolete]
        Task<IWrappedResponse<IPaginationResult<OrderMatch>>> Search(OrderMatchesSearchRequest request);
        [Obsolete]
        Task<IWrappedResponse<OrderMatch>> GetById(int id);
        Task<IWrappedResponse> Create(OrderMatchesCreateRequest request);
        [Obsolete]
        Task<IWrappedResponse<OrderMatch>> Update(int id, OrderMatchesUpdateRequest request);
        [Obsolete]
        Task<IWrappedResponse<OrderMatch>> Cancel(int id);
    }

    public interface IOrdersService
    {
        Task<IWrappedResponse> Search(OrderSearchRequest request);
        [Obsolete]
        Task<IWrappedResponse<Order>> GetById(int id);
        [Obsolete]
        Task<IWrappedResponse<Order>> Update(int id, OrderUpdateRequest request);
        Task<IWrappedResponse> Cancel(int id, OrderCancelRequest request);
        Task<IWrappedResponse> Summary(OrderSearchRequest request);
    }

    public interface IOrderGroupsService
    {
        [Obsolete]
        Task<IWrappedResponse<OrderGroup>> GetById(int id);
        Task<IWrappedResponse> Search(OrderGroupsSearchRequest request);
        Task<IWrappedResponse> Create(OrderGroupsCreateRequest request);
        Task<IWrappedResponse<OrderGroup>> Update(int id, OrderGroupsUpdateRequest request);
        [Obsolete]
        Task<IWrappedResponse<OrderGroup>> Cancel(int id, OrderGroupCancelRequest request);
    }

    public interface IPartnerDirectoriesService
    {
        Task<IWrappedResponse> GetAll();
        [Obsolete]
        Task<IWrappedResponse<PartnerDirectory>> GetById(int id);
        Task<IWrappedResponse> Delete(int id);
        Task<IWrappedResponse> Create(PartnerDirectoriesCreateRequest request);
        [Obsolete]
        Task<IWrappedResponse<PartnerDirectory>> Update(int id, PartnerDirectoriesUpdateRequest request);
    }
    
    public interface IPartnersService
    {
        Task<IWrappedResponse> Search(PartnersSearchRequest request);
        Task<IWrappedResponse> Delete(PartnersDeleteRequest request);
        Task<IWrappedResponse> Create(PartnersCreateRequest request);
        Task<IWrappedResponse> Update(ValuesUpdateRequest request);
    }

    public interface ICustomerPartnersService
    {
        Task<IWrappedResponse> Search(CustomerPartnersSearchRequest request);
        Task<IWrappedResponse> GetById(int id);
        [Obsolete]
        Task<IWrappedResponse> Delete(int id);
        Task<IWrappedResponse> Create(CustomerPartnersCreateRequest request);
        [Obsolete]
        Task<IWrappedResponse<CustomerPartner>> Update(int id, CustomerPartnersUpdateRequest request);
    }
    public interface IStorageService
    {
        Task<System.IO.Stream> Read(string fileName);

        /// <summary>
        /// Writes a stream to storage and returns a download link
        /// </summary>
        Task<string> Write(string fileName, System.IO.MemoryStream stream);
        Task<string> GetDownloadLink(string fileName);
    }

    public interface ISyncService
    {
        Task<bool> CreateLtmsBookings(IEnumerable<PostingRequestsCreateRequest> postingRequest);
    }

    public interface ITransportOfferingsService
    {
        Task<IWrappedResponse> Search(TransportOfferingsSearchRequest request);
        Task<IWrappedResponse> GetById(int id);
        Task<IWrappedResponse> CreateBid(int transportId, TransportOfferingBidCreateRequest request);
        Task<IWrappedResponse> CancelBid(int transportId, int bidId);
        Task<IWrappedResponse> AcceptBid(int id, TransportOfferingBidAcceptRequest request);
        Task<IWrappedResponse> AcceptTransport(int id);
        Task<IWrappedResponse> DeclineTransport(int id);
    }

    public interface IUploadsService
    {
        Task<IWrappedResponse<IPaginationResult<Upload>>> Search(UploadSearchRequest request);
        Task<IWrappedResponse<Upload>> GetById(int id);
        Task<IWrappedResponse> Delete(int id);
        //ToDo: ?
        Task<IWrappedResponse<IEnumerable<string>>> GetBalances(int id);
        Task<IWrappedResponse<Upload>> Patch(int id, UpdateUploadRequest request);
        Task<IWrappedResponse<Upload>> Post(CreateUploadRequest request);
    }

    public interface IUserService
    {
        Task<IWrappedResponse> Get();
        Task<IWrappedResponse> GetByCustomerId(int customerId);
        Task<IWrappedResponse> UpdateSettings(Newtonsoft.Json.Linq.JObject settings);
        IEnumerable<UserListItem> Search(UsersSearchRequest request);
        Task<IWrappedResponse> All(UsersSearchRequest request);
        Task<IWrappedResponse> UpdateLocked(UsersLockRequest request);
        Task<IWrappedResponse> Create(UserCreateRequest request);
        Task<IWrappedResponse> Update(UserUpdateRequest request);
        Task<IWrappedResponse> AddToCustomer(UserAddToCustomerRequest request);
        Task<IWrappedResponse> RemoveFromCustomer(RemoveFromCustomerRequest request);
        Task<IWrappedResponse> AllByOrganization(UsersByOrganizationRequest request);
        Task<IWrappedResponse> ResetPassword(UserResetPasswordRequest request);
    }
    public interface IUserGroupService
    {
        Task<IWrappedResponse<UserGroupListItem>> UpdatePermissions(int groupId,UserGroupUpdatePermissionsRequest request);
        Task<IWrappedResponse<UserGroupListItem>> UpdateMembers(int groupId,UserGroupUpdateMembershipRequest request);
        Task<IWrappedResponse<UserGroupListItem>> AddUser(int groupId, int userId);
        Task<IWrappedResponse<UserGroupListItem>> RemoveUser(int groupId, int userId);
        IEnumerable<UserGroupListItem> Search(UserGroupsSearchRequest request);
        IEnumerable<UserGroupMembership> GetMembers(UserGroupsSearchRequest request);
        Task<IWrappedResponse> Create(UserGroupCreateRequest request);
    }

    public interface IVouchersService
    {
        Task<IWrappedResponse> Summary(VouchersSearchRequest request);
        Task<IWrappedResponse> Search(VouchersSearchRequest request);
        [Obsolete]
        Task<IWrappedResponse> GetById(int id);
        [Obsolete]
        Task<IWrappedResponse> GetByNumber(string number);
        Task<IWrappedResponse> Create(VouchersCreateRequest request);
        //TODO Discuss necessity, Updates should not be possible
        [Obsolete]
        Task<IWrappedResponse> Update(int id,VouchersUpdateRequest request);
        Task<IWrappedResponse<Voucher>> AddToSubmission(int id,VouchersAddToSubmissionRequest request);
        Task<IWrappedResponse<Voucher>> RemoveFromSubmission(int id,VouchersRemoveFromSubmissionRequest request);
        Task<IWrappedResponse> Cancel(int id, VouchersCancelRequest request);
    }

    public interface IPermissionsService
    {
        Task<IEnumerable<CachedUser>> GetAll();
        Task<IEnumerable<UserPermission>> GetPermissions();
        Task<CachedUser> GetUser(string upn);
        Task UpdateCache();

        bool HasPermission(PermissionResourceType resource, ResourceAction action, int referenceId);
        bool HasAttributePermission(ResourceAction action);
    }

    public interface IPermissionsAdminService
    {
        Task<IWrappedResponse> All(UsersSearchRequest request);
    }

    public interface IPostingRequestsService
    {
        Task<IWrappedResponse> Search(PostingRequestsSearchRequest request);
        [Obsolete]
        Task<IWrappedResponse<PostingRequest>> GetById(int id);
        Task<IWrappedResponse> Create(PostingRequestsCreateRequest request);
        [Obsolete]
        Task<IWrappedResponse> Update(int id, PostingRequestsUpdateRequest request);
        Task<IWrappedResponse> Cancel(int id);
        Task<IWrappedResponse> Rollback(Guid refLtmsTransactionId);
    }

    public interface ISynchronizationsService
    {
        Task<IWrappedResponse> SendOrdersAsync(OrdersSyncRequest request);
        Task<IWrappedResponse> SendPostingRequestsAsync(PostingRequestsSyncRequest request);
        Task<IWrappedResponse> SendVoucherRequestsAsync(VoucherSyncRequest request);
        Task<IWrappedResponse<long>> SchedulePostingRequestsAsync(PostingRequestsSyncRequest request,
            DateTimeOffset dateTimeOffset);
        Task<IWrappedResponse> CancelScheduledPostingRequestsAsync(long sequenceNumber);
    }
}
