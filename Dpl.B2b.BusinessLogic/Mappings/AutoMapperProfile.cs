using AutoMapper;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LTMS = Dpl.B2b.Dal.Ltms;
using Olma = Dpl.B2b.Dal.Models;
using Lms = Dpl.B2b.Dal.Models.Lms;

namespace Dpl.B2b.BusinessLogic.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region AuthorizationResources

            CreateMap<Olma.Organization, OrganizationPermissionResource>()
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.Id))
                ;

            CreateMap<Olma.Customer, CustomerPermissionResource>()
                .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.OrganizationId))
                ;

            CreateMap<Olma.CustomerDivision, DivisionPermissionResource>()
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.Customer.OrganizationId))
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.PostingAccountId))
                ;

            CreateMap<Olma.Order, DivisionPermissionResource>()
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.DivisionId))
                .ForMember(dest => dest.CustomerId,
                    act => act.MapFrom(src => src.Division.CustomerId))
                .ForMember(dest => dest.OrganizationId,
                    act => act.MapFrom(src => src.Division.Customer.OrganizationId))
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.PostingAccountId))
                ;

            CreateMap<Olma.OrderLoad, DivisionPermissionResource>()
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.Order.DivisionId))
                .ForMember(dest => dest.CustomerId,
                    act => act.MapFrom(src => src.Order.Division.CustomerId))
                .ForMember(dest => dest.OrganizationId,
                    act => act.MapFrom(src => src.Order.Division.Customer.OrganizationId))
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.Order.PostingAccountId))
                ;

            CreateMap<Olma.OrderGroup, DivisionPermissionResource>()
                .ForMember(dest => dest.DivisionId,
                    act => act.MapFrom(src => src.Orders.First().DivisionId))
                .ForMember(dest => dest.CustomerId,
                    act => act.MapFrom(src => src.Orders.First().Division.CustomerId))
                .ForMember(dest => dest.OrganizationId,
                    act => act.MapFrom(src =>
                        src.Orders.First().Division.Customer.OrganizationId))
                .ForMember(dest => dest.PostingAccountId,
                    act => act.MapFrom(src => src.Orders.First().PostingAccountId))
                ;

            CreateMap<Olma.PostingAccount, PostingAccountPermissionResource>()
                //TODO Check if we can realy map to Partner here. At this point (16.06.20) Partner Data is not seed. But so far only PostingAccountId is used
                .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.Partner.Customer.Id))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.Partner.Customer.OrganizationId))
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.Id))
                ;
            CreateMap<Olma.LoadCarrierReceipt, DivisionPermissionResource>()
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.CustomerDivisionId))
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.PostingAccountId))
                .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.CustomerDivision.Customer.Id))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.CustomerDivision.Customer.OrganizationId))
                ;

            //´to enable transport security we need to connect division with transport
            // which in business terms manage which transport a specific division can see
            // we do NOT want to specify permissions for each transport but rather on transport provider groups
            //CreateMap<Olma.TransportProviderGroup, DivisionPermissionResource>()
            //    .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.CustomerDivisionId))
            //    .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.CustomerDivision.CustomerId))
            //    .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.CustomerDivision.Customer.OrganizationId))
            //    .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.CustomerDivision.PostingAccountId))
            //    ;

            CreateMap<Olma.Voucher, DivisionPermissionResource>()
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.CustomerDivisionId))
                .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.CustomerDivision.CustomerId))
                .ForMember(dest => dest.OrganizationId,
                    act => act.MapFrom(src => src.CustomerDivision.Customer.OrganizationId))
                .ForMember(dest => dest.PostingAccountId,
                    act => act.MapFrom(src => src.CustomerDivision.PostingAccountId))
                ;

            CreateMap<Olma.Order, PostingAccountPermissionResource>()
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.PostingAccountId))
                .ForMember(dest => dest.CustomerId, act => act.Ignore())
                .ForMember(dest => dest.OrganizationId, act => act.Ignore());

            #endregion

            // enums info
            // auto mapper maps enums by name

            #region Address

            CreateMap<Olma.Address, Contracts.Models.Address>()
                .ForMember(dest => dest.State, act => act.MapFrom(src => src.StateId))
                .ForMember(dest => dest.Country, act => act.MapFrom(src => src.CountryId))
                ; 
            
            CreateMap<Olma.Address, Contracts.Models.AddressAdministration>()
                .ForMember(dest => dest.State, act => act.MapFrom(src => src.StateId))
                .ForMember(dest => dest.Country, act => act.MapFrom(src => src.CountryId))
                ;

            CreateMap<Contracts.Models.Address, Olma.Address>()
                .ForMember(dest => dest.StateId, act => act.MapFrom(src => src.State))
                .ForMember(dest => dest.CountryId, act => act.MapFrom(src => src.Country))
                .ForMember(dest => dest.State, act => act.Ignore())
                .ForMember(dest => dest.Country, act => act.Ignore())
                .ForMember(dest => dest.RefLmsAddressNumber, act => act.Ignore())
                .ForMember(dest => dest.GeoLocation, act => act.Ignore())
                .ForMember(dest => dest.CreatedById, act => act.Ignore())
                .ForMember(dest => dest.CreatedBy, act => act.Ignore())
                .ForMember(dest => dest.CreatedAt, act => act.Ignore())
                .ForMember(dest => dest.ChangedById, act => act.Ignore())
                .ForMember(dest => dest.ChangedBy, act => act.Ignore())
                .ForMember(dest => dest.ChangedAt, act => act.Ignore())
                .ForMember(dest => dest.DeletedById, act => act.Ignore())
                .ForMember(dest => dest.DeletedBy, act => act.Ignore())
                .ForMember(dest => dest.DeletedAt, act => act.Ignore())
                .ForMember(dest => dest.IsDeleted, act => act.Ignore())
                ;

            #endregion

            #region Bookings
            
            CreateMap<LTMS.Bookings, AccountingRecord>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.TransactionId, act => act.Ignore())
                .ForMember(dest => dest.ProcessId, act => act.Ignore())
                // .ForMember(dest => dest.Status, act => act.Ignore())
                .ForMember(dest => dest.Status, act => act.MapFrom( src => 
                    src.Transaction.Cancellation != null ?  "Storniert"
                    : src.Matched? "Abgestimmt"
                    : src.IncludeInBalance && !src.Matched && src.ReportBookings.Any(rb => rb.Report.DeleteTime == null && rb.Report.ReportStateId == "U") ? "In Abstimmung"
                    : src.IncludeInBalance? "Unabgestimmt"
                    : "Vorläufig"))
                .ForMember(dest => dest.Type,
                    act => act.MapFrom(src => src.BookingTypeId))
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.Quantity, act => act.Ignore())
                .ForMember(dest => dest.Charge, act => act.MapFrom(src => src.Quantity > 0 ? src.Quantity : null))
                .ForMember(dest => dest.Credit, act => act.MapFrom(src => src.Quantity < 0 ? src.Quantity * -1 : null))
                .ForMember(dest => dest.HasDplNote, act => act.MapFrom(src => src.ExtDescription != null && src.ExtDescription != string.Empty))
                .ForMember(dest => dest.LoadCarrierTypeName, act => act.MapFrom(src => src.Article.Name))
                // these properties need to be manually mapped as the data for these come from the olma db
                .ForMember(dest => dest.LoadCarrierId, act => act.Ignore())
                .ForMember(dest => dest.PostingAccountId, act => act.Ignore())
                .ForMember(dest => dest.DplNote, act => act.Ignore())
                ;
            
            CreateMap<LTMS.Transactions, AccountingRecordTransaction>()
                .ForMember(dest => dest.Records, act => act.MapFrom(src => src.Bookings));

            CreateMap<LTMS.Processes, AccountingRecordProcess>();

            CreateMap<PaginationResult<LTMS.Processes>, PaginationResult<AccountingRecordProcess>>();
            CreateMap<IPaginationResult<LTMS.Bookings>, PaginationResult<AccountingRecord>>();

            #endregion

            #region CachedUser

            CreateMap<Olma.User, CachedUser>()
                .ForMember(dest => dest.OrganizationIds, act => act.Ignore())
                .ForMember(dest => dest.CustomerIds, act => act.Ignore())
                .ForMember(dest => dest.CustomerDivisionIds, act => act.Ignore())
                .ForMember(dest => dest.PostingAccountIds, act => act.Ignore())
                ;

            #endregion

            #region Countries / States

            CreateMap<Olma.CountryState, Contracts.Models.CountryState>();
            CreateMap<Olma.Country, Contracts.Models.Country>();

            #endregion

            #region Organization

            CreateMap<Olma.Organization, Organization>();

            CreateMap<OrganizationCreateRequest, Olma.Organization>()
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Address, act => act.Ignore())
                .ForMember(dest => dest.Customers, act => act.Ignore())
                .ForMember(dest => dest.Users, act => act.Ignore())
                .ForMember(dest => dest.CustomerIpSecurityRules, act => act.Ignore())
                .IgnoreAllAuditableProperties();
            
            CreateMap<OrganizationUpdateRequest, Olma.Organization>()
                .ForMember(dest => dest.Address, act => act.Ignore())
                .ForMember(dest => dest.Customers, act => act.Ignore())
                .ForMember(dest => dest.Users, act => act.Ignore())
                .ForMember(dest => dest.CustomerIpSecurityRules, act => act.Ignore())
                .IgnoreAllAuditableProperties();

            CreateMap<Olma.Organization, OrganizationScopedDataSet>()
                .ForMember(dest => dest.Scope, act => act.MapFrom(src => DomainScope.Organization))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.ParentId, act => act.Ignore());
                
            CreateMap<Olma.Customer, OrganizationScopedDataSet>()
                .ForMember(dest => dest.Scope, act => act.MapFrom(src => DomainScope.Customer))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.Organization.Id))
                .ForMember(dest => dest.ParentId, act => act.MapFrom(src => src.Organization.Id));
            
            CreateMap<Olma.CustomerDivision, OrganizationScopedDataSet>()
                .ForMember(dest => dest.Scope, act => act.MapFrom(src => DomainScope.Division))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.Customer.Organization.Id))
                .ForMember(dest => dest.ParentId, act => act.MapFrom(src => src.Customer.Id));
            
            CreateMap<Olma.LoadingLocation, OrganizationScopedDataSet>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => $"{src.Id}-{src.Address.City}"))
                .ForMember(dest => dest.Scope, act => act.MapFrom(src => DomainScope.LoadingLocation))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.CustomerDivision.Customer.Organization.Id))
                .ForMember(dest => dest.ParentId, act => act.MapFrom(src => src.CustomerDivision.Id));

            #endregion

            #region Customers

            //CreateMap<Olma.User, User>()
            //    .ForMember(dest => dest.Claims, act => act.Ignore());
            CreateMap<Olma.CustomerDivision, Contracts.Models.CustomerDivision>();
            CreateMap<CustomerDivisionCreateRequest, Olma.CustomerDivision>()
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Customer, act => act.Ignore())
                .ForMember(dest => dest.Vouchers, act => act.Ignore())
                .ForMember(dest => dest.DocumentSettings, act => act.Ignore())
                .ForMember(dest => dest.LoadingLocations, act => act.Ignore())
                .ForMember(dest => dest.PostingAccount, act => act.Ignore())
                .ForMember(dest => dest.DefaultLoadingLocation, act => act.Ignore())
                .IgnoreAllAuditableProperties();
            
            CreateMap<CustomerDivisionUpdateRequest, Olma.CustomerDivision>()
                .ForMember(dest => dest.Customer, act => act.Ignore())
                .ForMember(dest => dest.Vouchers, act => act.Ignore())
                .ForMember(dest => dest.DocumentSettings, act => act.Ignore())
                .ForMember(dest => dest.LoadingLocations, act => act.Ignore())
                .ForMember(dest => dest.PostingAccount, act => act.Ignore())
                .ForMember(dest => dest.DefaultLoadingLocation, act => act.Ignore())
                .IgnoreAllAuditableProperties();

            CreateMap<Olma.Customer, Contracts.Models.Customer>()
                .ForMember(dest => dest.ParentCustomerId, act => act.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.LoadCarrierReceiptDepotPresets, act => act.MapFrom(src => src.LoadCarrierReceiptDepotPresets.Select(i => i.LoadCarrierReceiptDepotPreset)));

            CreateMap<CustomerCreateRequest, Olma.Customer>()
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Address, act => act.Ignore())
                .ForMember(dest => dest.CustomDocumentLabels, act => act.Ignore())
                .ForMember(dest => dest.Children, act => act.Ignore())
                .ForMember(dest => dest.Divisions, act => act.Ignore())
                .ForMember(dest => dest.Organization, act => act.Ignore())
                .ForMember(dest => dest.Parent, act => act.Ignore())
                .ForMember(dest => dest.Partner, act => act.Ignore())
                .ForMember(dest => dest.PostingAccounts, act => act.Ignore())
                .ForMember(dest => dest.Users, act => act.Ignore())
                .ForMember(dest => dest.DocumentSettings, act => act.Ignore())
                .ForMember(dest => dest.DocumentNumberSequences, act => act.Ignore())
                .ForMember(dest => dest.SortingWorkerMapping, act => act.Ignore())
                .ForMember(dest => dest.LogoUrl, act => act.Ignore())
                .ForMember(dest => dest.RefErpCustomerNumberString, act => act.Ignore())
                .ForMember(dest => dest.LoadCarrierReceiptDepotPresets, act => act.Ignore())
                .IgnoreAllAuditableProperties();

            CreateMap<CustomerUpdateRequest, Olma.Customer>()
                .ForMember(dest => dest.Address, act => act.Ignore())
                .ForMember(dest => dest.Children, act => act.Ignore())
                .ForMember(dest => dest.CustomDocumentLabels, act => act.Ignore())
                .ForMember(dest => dest.Divisions, act => act.Ignore())
                .ForMember(dest => dest.Organization, act => act.Ignore())
                .ForMember(dest => dest.Parent, act => act.Ignore())
                .ForMember(dest => dest.Partner, act => act.Ignore())
                .ForMember(dest => dest.PostingAccounts, act => act.Ignore())
                .ForMember(dest => dest.Users, act => act.Ignore())
                .ForMember(dest => dest.DocumentSettings, act => act.Ignore())
                .ForMember(dest => dest.DocumentNumberSequences, act => act.Ignore())
                .ForMember(dest => dest.SortingWorkerMapping, act => act.Ignore())
                .ForMember(dest => dest.LogoUrl, act => act.Ignore())
                .ForMember(dest => dest.RefErpCustomerNumberString, act => act.Ignore())
                .ForMember(dest => dest.OrganizationId, act => act.Ignore())
                .ForMember(dest => dest.LoadCarrierReceiptDepotPresets, act => act.Ignore())
                .IgnoreAllAuditableProperties();

            CreateMap<Olma.LoadCarrierReceiptDepotPreset, LoadCarrierReceiptDepotPreset>()
                .ForMember(dest => dest.LoadCarriersIds, act => act.MapFrom(src => src.LoadCarriers.Select(i => i.LoadCarrierId)));

            #endregion

            #region Documents

            CreateMap<Olma.CustomDocumentLabel, Contracts.Models.CustomDocumentLabel>();
            CreateMap<Olma.Document, Contracts.Models.Document>()
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.CustomerDivisionId))
                .ForMember(dest => dest.FileIds, act => act.MapFrom(src => src.Files.Select(i => i.File.Id)));

            #endregion

            #region DocumentSettings

            CreateMap<Olma.CustomerDivisionDocumentSetting, CustomerDivisionDocumentSetting>();
            CreateMap<CustomerDivisionDocumentSettingCreateRequest, Olma.CustomerDivisionDocumentSetting>()
                .IgnoreAllAuditableProperties()
                .ForMember(dest => dest.DocumentNumberSequence, act => act.Ignore())
                .ForMember(dest => dest.DocumentType, act => act.Ignore())
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Division, act => act.Ignore());
            CreateMap<DocumentNumberSequenceCreateRequest, Olma.DocumentNumberSequence>()
                .IgnoreAllAuditableProperties()
                .ForMember(dest => dest.Id, act => act.Ignore());
            CreateMap<Olma.DocumentType, DocumentType>();
            CreateMap<Olma.DocumentNumberSequence, DocumentNumberSequence>();
            CreateMap<Olma.CustomerDocumentSetting, CustomerDocumentSettings>();
            CreateMap<Olma.CustomerDocumentSetting, CustomerDocumentSettingCreateRequest>()
                .ReverseMap();
            CreateMap<Olma.CustomerDivisionDocumentSetting, CustomerDivisionDocumentSettings>();
            CreateMap<Olma.Organization, DocumentSettings>()
                .ForMember(dest => dest.Customers,
                    act => act.MapFrom(src => src.Customers.SelectMany(i => i.DocumentSettings)))
                .ForMember(dest => dest.Divisions,
                    act => act.MapFrom(src =>
                        src.Customers.SelectMany(i => i.Divisions.SelectMany(d => d.DocumentSettings))));

            CreateMap<CustomerDocumentSettingsUpdateRequest, Olma.CustomerDocumentSetting>()
                .ForMember(dest => dest.MaxQuantity, 
                    act => act.MapFrom(src => src.MaxQuantity))
                .ForMember(dest => dest.ThresholdForWarningQuantity,
                    act => act.MapFrom(src => src.ThresholdForWarningQuantity))
                .ForMember(dest => dest.CancellationTimeSpan, 
                    act => act.MapFrom(src => src.CancellationTimeSpan))
                .ForAllOtherMembers(act => act.Ignore());

            CreateMap<CustomerDivisionDocumentSettingsUpdateRequest, Olma.CustomerDivisionDocumentSetting>()
                .ForMember(dest => dest.PrintCountMin, act => act.MapFrom(src => src.PrintCountMin))
                .ForMember(dest => dest.PrintCountMax, act => act.MapFrom(src => src.PrintCountMax))
                .ForMember(dest => dest.DefaultPrintCount, act => act.MapFrom(src => src.DefaultPrintCount))
                .ForMember(dest => dest.Override, act => act.MapFrom(src => src.Override))
                .ForAllOtherMembers(act => act.Ignore());

            #endregion

            #region DocumentState

            CreateMap<Olma.DocumentState, Contracts.Models.DocumentState>();

            #endregion

            #region DplEmployeeCustomer

            CreateMap<Olma.Customer, Contracts.Models.DplEmployeeCustomer>()
                .ForMember(dest => dest.Type, act => act.MapFrom(i => DplEmployeeCustomerSelectionEntryType.Customer))
                .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Address))
                .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.DisplayName,
                    act => act.MapFrom(src => src.Name + " (" + src.RefErpCustomerNumber + ")"))
                .ForMember(dest => dest.DisplayNameLong,
                    act => act.MapFrom(src => src.Name + " (" + src.RefErpCustomerNumber + ")"))
                .ForMember(dest => dest.DivisionId, act => act.Ignore())
                .ForMember(dest => dest.PostingAccountId, act => act.Ignore())
                ;

            CreateMap<Olma.CustomerDivision, Contracts.Models.DplEmployeeCustomer>()
                .ForMember(dest => dest.Type, act => act.MapFrom(i => DplEmployeeCustomerSelectionEntryType.Division))
                .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Customer.Address))
                .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.DisplayName,
                    act => act.MapFrom(src => src.Customer.Name + " (" + src.Customer.RefErpCustomerNumber + ")"))
                .ForMember(dest => dest.DisplayNameLong,
                    act => act.MapFrom(src =>
                        src.Customer.Name + " (" + src.Customer.RefErpCustomerNumber + ")" + " | " + src.Name + " (" +
                        src.ShortName + ")" + " | " + src.PostingAccount.DisplayName + " (" +
                        src.PostingAccount.RefLtmsAccountId + ")"))
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.PostingAccountId))
                ;

            #endregion

            #region ExpressCode

            CreateMap<Olma.ExpressCode, Contracts.Models.ExpressCode>()
                .ForMember(dest => dest.BookingText, act => act.MapFrom(src => src.CustomBookingText))
                .ForMember(dest => dest.IssuingDivisionName, act => act.MapFrom(src => src.IssuingDivision.Name))
                .ForMember(dest => dest.VoucherPresets, act => act.Ignore())
                .ForMember(dest => dest.LoadCarrierReceiptPreset, act => act.Ignore())
                .ForMember(dest => dest.DestinationAccountPreset, act => act.MapFrom(src => src.PostingAccountPreset))
                .ReverseMap();
            CreateMap<Olma.PostingAccountPreset, DestinationAccountPreset>()
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.DestinationAccountId))
                .ReverseMap();

            #endregion

            #region EmployeeNote

            CreateMap<Olma.EmployeeNote, Contracts.Models.EmployeeNoteCreateRequest>()
                .ReverseMap();

            CreateMap<Olma.EmployeeNote, Contracts.Models.EmployeeNote>();

            #endregion

            #region Language

            CreateMap<Olma.LocalizationLanguage, Language>();

            #endregion

            #region Load Carrier

            CreateMap<Olma.LoadCarrierQuality, Contracts.Models.LoadCarrierQuality>()
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type));
            CreateMap<Olma.LoadCarrierType, Contracts.Models.LoadCarrierType>()
                .ForMember(dest => dest.BaseLoadCarrier, act => act.MapFrom(i => i.BaseLoadCarrier))
                .ForMember(dest => dest.BaseLoadCarriers,
                    act => act.MapFrom(i => i.BaseLoadCarrierMappings.Select(i => i.LoadCarrier)));
            CreateMap<Olma.LoadCarrier, Contracts.Models.LoadCarrier>();

            #endregion

            #region LoadCarrier Offering

            CreateMap<Olma.LmsOrder, LoadCarrierOfferingDetail>()
                .ForMember(dest => dest.Guid, act => act.MapFrom(i => i.RowGuid))
                .ForMember(dest => dest.AvailabilityFrom, act => act.MapFrom(i => i.FromDate.Value.Date))
                .ForMember(dest => dest.AvailabilityTo, act => act.MapFrom(i => i.UntilDate.Value.Date))
                ;

            #endregion

            #region LoadCarrierReceipt

            // reverse map
            CreateMap<Olma.LoadCarrierReceipt, LoadCarrierReceiptsCreateRequest>()
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type))
                .ForMember(dest => dest.PrintLanguageId, act => act.Ignore())
                .ForMember(dest => dest.PrintCount, act => act.Ignore())
                .ForMember(dest => dest.PrintDateTimeOffset, act => act.Ignore())
                .ForMember(dest => dest.TargetPostingAccountId, act => act.Ignore())
                .ForMember(dest => dest.DplNote, act => act.Ignore())
                .ForMember(dest => dest.RefLmsBusinessTypeId, act => act.Ignore())
                .ForMember(dest => dest.RefLtmsTransactionRowGuid, act => act.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.DplNotes, act => act.MapFrom(src => src.DplNote != null
                    ? new List<EmployeeNoteCreateRequest>() {src.DplNote}
                    : null))
                .ForMember(dest => dest.IsSortingCompleted, act => act.Ignore())
                ;

            // reverse map
            CreateMap<Olma.LoadCarrierReceiptPosition, LoadCarrierReceiptsCreateRequestPosition>()
                .ForMember(dest => dest.Quantity, act => act.MapFrom(src => src.LoadCarrierQuantity))
                .ReverseMap();

            CreateMap<Olma.LoadCarrierReceipt, Contracts.Models.LoadCarrierReceipt>()
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.CustomerDivisionId))
                .ForMember(dest => dest.IssuedDate, act => act.MapFrom(src => src.Document.IssuedDateTime))
                .ForMember(dest => dest.DocumentId, act => act.MapFrom(src => src.Document.Id))
                .ForMember(dest => dest.DocumentNumber, act => act.MapFrom(src => src.Document.Number))
                .ForMember(dest => dest.DepotPresetCategory, act => act.MapFrom(src => src.DepotPreset.Category))
                .ForMember(dest => dest.DownloadLink, act => act.Ignore())
                .ForMember(dest => dest.DivisionName, act => act.Ignore())
                .ForMember(dest => dest.IssuerCompanyName, act => act.Ignore())
                .ForMember(dest => dest.Positions, act => act.MapFrom(src => src.Positions))
                .ForMember(dest => dest.AccountingRecords, act => act.Ignore())
                .ForMember(dest => dest.DownloadLink, act => act.Ignore());

            CreateMap<Olma.LoadCarrierReceiptPosition, Contracts.Models.LoadCarrierReceiptPosition>()
                .ForMember(dest => dest.Quantity, act => act.MapFrom(src => src.LoadCarrierQuantity));

            #endregion

            #region LoadCarrierSorting

            CreateMap<LoadCarrierSortingCreateRequest, Olma.LoadCarrierSorting>()
                .ForMember(dest => dest.LoadCarrierReceipt, act => act.Ignore())
                .ForMember(dest => dest.CreatedById, act => act.Ignore())
                .ForMember(dest => dest.CreatedBy, act => act.Ignore())
                .ForMember(dest => dest.CreatedAt, act => act.Ignore())
                .ForMember(dest => dest.ChangedById, act => act.Ignore())
                .ForMember(dest => dest.ChangedBy, act => act.Ignore())
                .ForMember(dest => dest.ChangedAt, act => act.Ignore())
                .ForMember(dest => dest.DeletedById, act => act.Ignore())
                .ForMember(dest => dest.DeletedBy, act => act.Ignore())
                .ForMember(dest => dest.DeletedAt, act => act.Ignore())
                .ForMember(dest => dest.IsDeleted, act => act.Ignore())
                .ForMember(dest => dest.Id, act => act.Ignore())
                ;
            CreateMap<LoadCarrierSortingResult, Olma.LoadCarrierSortingResult>()

                .ForMember(dest => dest.LoadCarrierId, act => act.MapFrom(src => src.LoadCarrierId))
                .ForAllOtherMembers(act => act.Ignore())
                //.ForMember(dest => dest.Outputs, act => act.MapFrom(src => src.Outputs))
                                                        //.ForMember(dest => dest.Id, act => act.Ignore())
                                                        //.ForMember(dest => dest.LoadCarrierSortingId, act => act.Ignore())
                                                        //.ForMember(dest => dest.LoadCarrierSorting, act => act.Ignore())
                                                        //.ForMember(dest => dest.LoadCarrier, act => act.Ignore())
                                                        //.ForMember(dest => dest.InputQuantity, act => act.Ignore())
                                                        //.ForMember(dest => dest.RemainingQuantity, act => act.Ignore())
                                                        //.ReverseMap()
                ;
            CreateMap<Olma.LoadCarrierSortingResultOutput,LoadCarrierSortingResultOutput>()
                .ForMember(dest => dest.Quantity, act => act.MapFrom(src => src.LoadCarrierQuantity))
                .ForMember(dest => dest.LoadCarrierQualityId, act => act.MapFrom(src => src.LoadCarrier.QualityId))
                //.ReverseMap()
                ;
            CreateMap<Olma.LoadCarrierSorting, LoadCarrierSorting>()
                .ForMember(dest => dest.Positions, act => act.MapFrom(src => src.Positions))
                ;
            CreateMap<Olma.LoadCarrierSortingResult, LoadCarrierSortingResult>();


            #endregion

            #region LoadingLocation

            CreateMap<Olma.BusinessHour, BusinessHours>()
                .ReverseMap();
            CreateMap<Olma.BusinessHourException, BusinessHourException>()
                .ReverseMap();
            CreateMap<Olma.PublicHoliday, Contracts.Models.PublicHoliday>();
            CreateMap<Olma.LoadingLocation, LoadingLocationsCreateRequest>()
                .ReverseMap()
                .ForMember(dest => dest.BusinessHours, act => act.Ignore())
                .ForMember(dest => dest.BusinessHourExceptions, act => act.Ignore())
                ;
            CreateMap<Olma.LoadingLocation, LoadingLocationsUpdateRequest>()
                .ReverseMap();
            CreateMap<Olma.LoadingLocation, LoadingLocationAdministration>();
            
            CreateMap<Olma.LoadingLocation, LoadingLocationDetail>();

            CreateMap<Olma.LoadingLocation, Contracts.Models.LoadingLocation>()
                .ForMember(dest => dest.Detail, act => act.MapFrom(src => src))
                .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Address))
                .ForMember(dest => dest.BusinessHours, act => act.MapFrom(src => src.BusinessHours))
                .ForMember(dest => dest.PublicHolidays, act => act.Ignore());

            CreateMap<Contracts.Models.LoadingLocation, Olma.LoadingLocation>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Address, act => act.Ignore())
                .ForMember(dest => dest.BusinessHours, act => act.Ignore())
                .ForMember(dest => dest.StackHeightMin, act => act.MapFrom(src => src.Detail.StackHeightMin))
                .ForMember(dest => dest.StackHeightMax, act => act.MapFrom(src => src.Detail.StackHeightMax))
                .ForMember(dest => dest.SupportsPartialMatching,
                    act => act.MapFrom(src => src.Detail.SupportsPartialMatching))
                .ForMember(dest => dest.SupportsRearLoading, act => act.MapFrom(src => src.Detail.SupportsRearLoading))
                .ForMember(dest => dest.SupportsSideLoading, act => act.MapFrom(src => src.Detail.SupportsSideLoading))
                .ForMember(dest => dest.SupportsJumboVehicles,
                    act => act.MapFrom(src => src.Detail.SupportsJumboVehicles))
                .ForMember(dest => dest.PartnerId, act => act.Ignore())
                .ForMember(dest => dest.Partner, act => act.Ignore())
                .ForMember(dest => dest.CustomerPartnerId, act => act.Ignore())
                .ForMember(dest => dest.CustomerPartner, act => act.Ignore())
                .ForMember(dest => dest.CustomerDivisionId, act => act.Ignore())
                .ForMember(dest => dest.CustomerDivision, act => act.Ignore())
                .ForMember(dest => dest.AddressId, act => act.Ignore())
                .ForMember(dest => dest.AddressId, act => act.Ignore())
                .ForMember(dest => dest.BusinessHourExceptions, act => act.Ignore())
                .ForMember(dest => dest.CreatedById, act => act.Ignore())
                .ForMember(dest => dest.CreatedBy, act => act.Ignore())
                .ForMember(dest => dest.CreatedAt, act => act.Ignore())
                .ForMember(dest => dest.ChangedById, act => act.Ignore())
                .ForMember(dest => dest.ChangedBy, act => act.Ignore())
                .ForMember(dest => dest.ChangedAt, act => act.Ignore())
                .ForMember(dest => dest.DeletedById, act => act.Ignore())
                .ForMember(dest => dest.DeletedBy, act => act.Ignore())
                .ForMember(dest => dest.DeletedAt, act => act.Ignore())
                .ForMember(dest => dest.IsDeleted, act => act.Ignore());

            #endregion

            #region Order

            CreateMap<Olma.Order, Contracts.Models.Order>()

                .ForMember(dest => dest.Type, act => act.MapFrom(i => i.Type))
                .ForMember(dest => dest.QuantityType, act => act.MapFrom(i => i.QuantityType))
                .ForMember(dest => dest.Status, act => act.MapFrom(i => i.Status))
                .ForMember(dest => dest.TransportType, act => act.MapFrom(i => i.TransportType))
                .ForMember(dest => dest.Address, act => act.MapFrom(i => i.LoadingLocation != null ? i.LoadingLocation.Address : null))
                //HACK hardcode fallback to 33 stacks because stacks currently do not get filled when creating ordergroups / orders
                .ForMember(dest => dest.NumberOfStacks, act => act.MapFrom(i =>
                    i.QuantityType == OrderQuantityType.LoadCarrierQuantity
                        ? null
                        : i.NumberOfStacks.HasValue
                            ? i.NumberOfStacks
                            : 33))
                .ForMember(dest => dest.Loads, act => act.MapFrom(i => i.Loads.Where(l => l.Detail.Status != OrderLoadStatus.Pending)))
                .ForMember(dest => dest.HasDplNote, act => act.MapFrom(src => src.DplNotes.Any() || src.Loads.Any(i => i.Detail.LoadCarrierReceipt.DplNotes.Any())))
                ;

            CreateMap<Olma.OrderGroup, Contracts.Models.OrderGroup>()
                .ForMember(dest => dest.OrderIds, act => act.MapFrom(src => src.Orders.Select(o => o.Id)))
                ;

            CreateMap<OrderGroupsCreateRequest, Olma.Order>()
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.RefLmsOrderRowGuid, act => act.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.GroupId, act => act.Ignore())
                .ForMember(dest => dest.Group, act => act.Ignore())
                .ForMember(dest => dest.Status, act => act.MapFrom(src => OrderStatus.Pending))
                .ForMember(dest => dest.DplNotes,
                    act => act.MapFrom(src =>
                        src.DplNote != null ? new List<EmployeeNoteCreateRequest>() {src.DplNote} : null))
                .ForMember(dest => dest.PostingAccount, act => act.Ignore())
                .ForMember(dest => dest.LoadCarrier, act => act.Ignore())
                .ForMember(dest => dest.BaseLoadCarrier, act => act.Ignore())
                .ForMember(dest => dest.LoadingLocation, act => act.Ignore())
                .ForMember(dest => dest.Loads, act => act.Ignore())
                .ForMember(dest => dest.CreatedAt, act => act.Ignore())
                .ForMember(dest => dest.CreatedBy, act => act.Ignore())
                .ForMember(dest => dest.CreatedById, act => act.Ignore())
                .ForMember(dest => dest.ChangedAt, act => act.Ignore())
                .ForMember(dest => dest.ChangedBy, act => act.Ignore())
                .ForMember(dest => dest.ChangedById, act => act.Ignore())
                .ForMember(dest => dest.DeletedAt, act => act.Ignore())
                .ForMember(dest => dest.DeletedBy, act => act.Ignore())
                .ForMember(dest => dest.DeletedById, act => act.Ignore())
                .ForMember(dest => dest.IsDeleted, act => act.Ignore())
                .ForMember(dest => dest.SyncDate, act => act.Ignore())
                .ForMember(dest => dest.SyncNote, act => act.Ignore())
                .ForMember(dest => dest.OrderNumber, act => act.Ignore())
                .ForMember(dest => dest.Division, act => act.Ignore());

            CreateMap<OrderGroupsUpdateRequest, Olma.Order>()
                .ForMember(dest => dest.DplNotes,
                    act => act.MapFrom(src =>
                        src.DplNote != null ? new List<EmployeeNoteCreateRequest>() {src.DplNote} : null))
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.GroupId, act => act.Ignore())
                .ForMember(dest => dest.Group, act => act.Ignore())
                .ForMember(dest => dest.Status, act => act.Ignore())
                .ForMember(dest => dest.LoadCarrier, act => act.Ignore())
                .ForMember(dest => dest.LoadingLocation, act => act.Ignore())
                .ForMember(dest => dest.RefLmsOrderRowGuid, act => act.Ignore())
                .ForMember(dest => dest.PostingAccount, act => act.Ignore())
                .ForMember(dest => dest.BaseLoadCarrier, act => act.Ignore())
                .ForMember(dest => dest.Loads, act => act.Ignore())
                .ForMember(dest => dest.CreatedAt, act => act.Ignore())
                .ForMember(dest => dest.CreatedBy, act => act.Ignore())
                .ForMember(dest => dest.CreatedById, act => act.Ignore())
                .ForMember(dest => dest.ChangedAt, act => act.Ignore())
                .ForMember(dest => dest.ChangedBy, act => act.Ignore())
                .ForMember(dest => dest.ChangedById, act => act.Ignore())
                .ForMember(dest => dest.DeletedAt, act => act.Ignore())
                .ForMember(dest => dest.DeletedBy, act => act.Ignore())
                .ForMember(dest => dest.DeletedById, act => act.Ignore())
                .ForMember(dest => dest.IsDeleted, act => act.Ignore())
                .ForMember(dest => dest.SyncDate, act => act.Ignore())
                .ForMember(dest => dest.SyncNote, act => act.Ignore())
                .ForMember(dest => dest.OrderNumber, act => act.Ignore())
                .ForMember(dest => dest.DivisionId, act => act.Ignore())
                .ForMember(dest => dest.Division, act => act.Ignore());

            CreateMap<Olma.Order, OrderUpdateRequest>()
                .ForMember(dest => dest.Code, act => act.Ignore())
                .ForMember(dest => dest.DplNote, act => act.Ignore())
                .ReverseMap();

            CreateMap<Olma.Order, OrderCreateSyncRequest>()
                .ForMember(dest => dest.OrderRowGuid, act => act.MapFrom(src => src.RefLmsOrderRowGuid))
                .ForMember(dest => dest.Address, act => act.MapFrom(src => src.LoadingLocation.Address))
                .ForMember(dest => dest.Person, act => act.MapFrom(src => src.CreatedBy.Person))
                .ForMember(dest => dest.VauNumber, act => act.Ignore())
                // HACK the fact that we have to parse the customer number is a red flag, this should be an int value
                .ForMember(dest => dest.CustomerNumber,
                    act => act.MapFrom(src => src.PostingAccount.RefErpCustomerNumber.HasValue 
                        ? src.PostingAccount.RefErpCustomerNumber.Value 
                        : src.Division.Customer.RefErpCustomerNumber))
                .ForMember(dest => dest.RefLmsLoadCarrierId,
                    act => act.MapFrom(src => src.LoadCarrier.RefLmsQuality2PalletId))
                .ForMember(dest => dest.RefLmsBaseLoadCarrierId,
                    act => act.MapFrom(src =>
                        src.BaseLoadCarrier != null ? src.BaseLoadCarrier.RefLmsQuality2PalletId : 0))
                .ForMember(dest => dest.ExternalReferenceNumber, act => act.Ignore())
                .ForMember(dest => dest.RefLmsAvailabilityRowGuid, act => act.Ignore())
                .ForMember(dest => dest.RefLmsDeliveryRowGuid, act => act.Ignore())
                .ForMember(dest => dest.RefLmsPermanentAvailabilityRowGuid, act => act.Ignore())
                .ForMember(dest => dest.RefLmsPermanentDeliveryRowGuid, act => act.Ignore())
                .ForMember(dest => dest.DigitalCode, act => act.Ignore())
                .ForMember(dest => dest.BaseLoadCarrierQuantity, act => act.Ignore())
                .ForMember(dest => dest.CustomerNumberLoadingLocation, act => act.MapFrom(src => src.LoadingLocation.CustomerDivision
                    .Customer.RefErpCustomerNumber))
                .ForMember(dest => dest.RefLtmsAccountId, act => act.MapFrom(src => src.PostingAccount.RefLtmsAccountId))
                .ForMember(dest => dest.RefLtmsAccountNumber, act => act.MapFrom(src => src.PostingAccount.RefLtmsAccountNumber))
                .ForMember(dest => dest.BusinessHoursString, act => act.MapFrom(src => src.LoadingLocation.BusinessHours.ConvertToString()))
                .ForMember(dest => dest.IsPermanent, act => act.Ignore())
                ;

            CreateMap<Olma.Order, OrderCancelSyncRequest>()
                .ForMember(dest => dest.OrderRowGuid, act => act.MapFrom(src => src.RefLmsOrderRowGuid))
                .ForMember(dest => dest.Person, act => act.MapFrom(src => src.CreatedBy.Person))
                .ForMember(dest => dest.Note, act => act.Ignore())
                .ForMember(dest => dest.IsApproved, act => act.Ignore())
                ;
            
            CreateMap<Olma.OrderLoad, OrderCancelSyncRequest>()
                .ForMember(dest => dest.OrderRowGuid, act => act.MapFrom(src => src.Order.RefLmsOrderRowGuid))
                .ForMember(dest => dest.Person, act => act.MapFrom(src => src.CreatedBy.Person))
                .ForMember(dest => dest.Person, act => act.MapFrom(src => src.CreatedBy.Person))
                .ForMember(dest => dest.Note, act => act.Ignore())
                .ForMember(dest => dest.IsApproved, act => act.Ignore())
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Order.Type))
                ;

            #endregion

            #region OrderLoad

            CreateMap<Olma.OrderLoad, Contracts.Models.OrderLoad>()
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Order.Type))
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.Order.PostingAccountId))
                .ForMember(dest => dest.LoadingLocationId, act => act.MapFrom(src => src.Order.LoadingLocationId))
                .ForMember(dest => dest.Status, act => act.MapFrom(src => src.Detail.Status))
                .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Detail.Address))
                .ForMember(dest => dest.PlannedFulfillmentDateTime,
                    act => act.MapFrom(src => src.Detail.PlannedFulfillmentDateTime))
                .ForMember(dest => dest.ActualFulfillmentDateTime,
                    act => act.MapFrom(src => src.Detail.ActualFulfillmentDateTime))
                .ForMember(dest => dest.LoadCarrierReceipt, act => act.MapFrom(src => src.Detail.LoadCarrierReceipt))
                .ForMember(dest => dest.OrderMatchId,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand ? src.DemandOrderMatchId : src.SupplyOrderMatchId))
                .ForMember(dest => dest.LoadCarrierId,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.LoadCarrierId
                            : src.SupplyOrderMatch.LoadCarrierId))
                .ForMember(dest => dest.LoadCarrierQuantity,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.LoadCarrierQuantity
                            : src.SupplyOrderMatch.LoadCarrierQuantity))
                .ForMember(dest => dest.LoadCarrierStackHeight,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.LoadCarrierStackHeight
                            : src.SupplyOrderMatch.LoadCarrierStackHeight))
                .ForMember(dest => dest.NumberOfStacks,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.NumberOfStacks
                            : src.SupplyOrderMatch.NumberOfStacks))
                .ForMember(dest => dest.BaseLoadCarrierId,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.BaseLoadCarrierId
                            : src.SupplyOrderMatch.BaseLoadCarrierId))
                .ForMember(dest => dest.BaseLoadCarrierQuantity,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.BaseLoadCarrierQuantity
                            : src.SupplyOrderMatch.BaseLoadCarrierQuantity))
                .ForMember(dest => dest.DigitalCode,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.DigitalCode
                            : src.SupplyOrderMatch.DigitalCode))
                .ForMember(dest => dest.TransportType, act => act.MapFrom(src => src.Order.Type == OrderType.Demand
                    ? src.DemandOrderMatch.TransportType == OrderTransportType.Self &&
                      src.DemandOrderMatch.SelfTransportSide == OrderType.Demand
                        ? OrderTransportType.Self
                        : OrderTransportType.ProvidedByOthers
                    : src.SupplyOrderMatch.TransportType == OrderTransportType.Self &&
                      src.SupplyOrderMatch.SelfTransportSide == OrderType.Supply
                        ? OrderTransportType.Self
                        : OrderTransportType.ProvidedByOthers
                ))
                .ForMember(dest => dest.SupportsRearLoading,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.SupportsRearLoading
                            : src.SupplyOrderMatch.SupportsRearLoading))
                .ForMember(dest => dest.SupportsSideLoading,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.SupportsSideLoading
                            : src.SupplyOrderMatch.SupportsRearLoading))
                .ForMember(dest => dest.SupportsJumboVehicles,
                    act => act.MapFrom(src =>
                        src.Order.Type == OrderType.Demand
                            ? src.DemandOrderMatch.SupportsJumboVehicles
                            : src.SupplyOrderMatch.SupportsRearLoading))
                ;

            #endregion

            #region OrderMatch

            CreateMap<Olma.OrderMatch, Contracts.Models.OrderMatch>()
                // HACK Supply /Demand copmany name not correctly mapped, we need a relation to a division / customer on the order in addition to posting account
                .ForMember(dest => dest.DemandCompanyName,
                    act => act.MapFrom(src => src.Demand.Order.PostingAccount.CustomerDivisions.First().Customer.Name))
                .ForMember(dest => dest.SupplyCompanyName,
                    act => act.MapFrom(src => src.Supply.Order.PostingAccount.CustomerDivisions.First().Customer.Name))
                ;

            CreateMap<Olma.OrderMatch, OrderMatchesCreateRequest>()
                .ForMember(dest => dest.SupplyOrderRowGuid, act => act.MapFrom(src => src.RefLmsAvailabilityRowGuid))
                .ForMember(dest => dest.DemandOrderRowGuid, act => act.MapFrom(src => src.RefLmsDeliveryRowGuid))
                .ForMember(dest => dest.SkipValidation, act => act.Ignore())
                .ForMember(dest => dest.DemandFulfillmentDateTime, act => act.Ignore())
                .ForMember(dest => dest.SupplyFulfillmentDateTime, act => act.Ignore())
                .ReverseMap()
                ;

            // mapping for self transport
            CreateMap<Olma.Order, OrderMatchesCreateRequest>()
                .ForMember(dest => dest.TransportType, act => act.MapFrom(src => OrderTransportType.Self))
                .ForMember(dest => dest.SkipValidation, act => act.MapFrom(src => false))
                .ForMember(dest => dest.SelfTransportSide, act => act.MapFrom(src => src.Type))
                .ForMember(dest => dest.SupplyFulfillmentDateTime,
                    act => act.MapFrom(src => src.EarliestFulfillmentDateTime))
                .ForMember(dest => dest.DemandFulfillmentDateTime,
                    act => act.MapFrom(src => src.EarliestFulfillmentDateTime))
                .ForMember(dest => dest.SupplyOrderRowGuid, act => act.Ignore())
                .ForMember(dest => dest.DemandOrderRowGuid, act => act.Ignore())
                .ForMember(dest => dest.LoadCarrierStackHeight, act => act.Ignore())
                .ForMember(dest => dest.NumberOfStacks, act => act.Ignore())
                .ForMember(dest => dest.LoadCarrierQuantity, act => act.Ignore())
                .ForMember(dest => dest.BaseLoadCarrierQuantity, act => act.Ignore())
                .ReverseMap();

            CreateMap<OrderMatchesCreateRequest, OrderMatchQuantity>()
                .ReverseMap();

            #endregion

            #region Postingaccounts + Balances

            CreateMap<Olma.PostingAccount, Contracts.Models.PostingAccountAdministration>()
                .ForMember(dest => dest.CustomerNumber, act => act.MapFrom(src => src.RefErpCustomerNumber))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(t => (PostingAccountType) t.Type))
                ;            
            CreateMap<Olma.PostingAccount, Contracts.Models.PostingAccount>()
                .ForMember(dest => dest.CustomerNumber, act => act.MapFrom(src => src.RefErpCustomerNumber))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(t => (Contracts.Models.PostingAccountType) t.Type))
                .ForMember(dest => dest.PickupConditions, act => act.Ignore())
                .ForMember(dest => dest.DropoffConditions, act => act.Ignore())
                .ForMember(dest => dest.DemandConditions, act => act.Ignore())
                .ForMember(dest => dest.SupplyConditions, act => act.Ignore())
                .ForMember(dest => dest.TransferConditions, act => act.Ignore())
                .ForMember(dest => dest.VoucherConditions, act => act.Ignore())
                .ForMember(dest => dest.RefLtmsAccountId, opt => opt.MapFrom(src => src.RefLtmsAccountId))

                // Had to remove for perfromance reasons getUser/Get took too long
                // including the conditions multiplied the number of rows returtned to 100.000 due to this
                //.ForMember(dest => dest.PickupConditions, opt => opt.MapFrom(
                //    src => src.OrderConditions.Where(oc => oc.OrderType == OrderType.Demand).SelectMany(oc => oc.LoadCarrierConditions)))
                //.ForMember(dest => dest.DropoffConditions, opt => opt.MapFrom(
                //     src => src.OrderConditions.Where(oc => oc.OrderType == OrderType.Supply).SelectMany(oc => oc.LoadCarrierConditions)))

                //.ForMember(dest => dest.DemandConditions, opt => opt.MapFrom(
                //    src => src.OrderConditions.Where(oc => oc.OrderType == OrderType.Demand).SelectMany(oc => oc.LoadCarrierConditions)))
                //.ForMember(dest => dest.SupplyConditions, opt => opt.MapFrom(
                //     src => src.OrderConditions.Where(oc => oc.OrderType == OrderType.Supply).SelectMany(oc => oc.LoadCarrierConditions)))
                .ForMember(dest => dest.Balances, act => act.Ignore());

            CreateMap<Olma.LoadCarrierCondition, PostingAccountOrderCondition>();

            CreateMap<Olma.CalculatedBalancePosition, Balance>()
                .ForMember(dest => dest.LastBookingDateTime, act => act.Ignore())
                .ForMember(dest => dest.AvailableBalance, act => act.Ignore());

            CreateMap<Olma.PostingAccount, PostingAccountsCreateRequest>()
                .ForMember(dest => dest.CustomerNumber, act => act.MapFrom(src => src.RefErpCustomerNumber))
                .ReverseMap()
                .ForMember(dest => dest.Customer, act => act.Ignore())
                ;
            // not sure if we will need this but its likely, hence keeping it here for reference
            // if we need it we need to set divisionIds mapping above to ignore
            //.AfterMap((src, dest) => dest.CustomerDivision = src.DivisionIds.Select(id => new Olma.CustomerDivision() { Id = id }).ToList());

            CreateMap<Olma.PostingAccount, PostingAccountsUpdateRequest>()
                .ForMember(dest => dest.CustomerNumber, act => act.MapFrom(src => src.RefErpCustomerNumber))
                .ReverseMap()
                .ForMember(dest => dest.Customer, act => act.Ignore());
            
            CreateMap<Olma.Address, AddressesCreateRequest>()
                .ReverseMap();
            CreateMap<Olma.Address, AddressesUpdateRequest>()
                .ReverseMap();
            
            CreateMap<LTMS.Accounts, LtmsAccount>();

            #endregion

            #region PostingRequests

            CreateMap<Olma.PostingRequest, Contracts.Models.PostingRequest>()
                .ReverseMap();
            CreateMap<Olma.PostingRequest, PostingRequestsCreateRequest>()
                .ForMember(dest => dest.Positions, act => act.Ignore())
                .ForMember(dest => dest.IsSelfService, act => act.Ignore())
                .ForMember(dest => dest.DocumentFileName, act => act.Ignore())
                .ForMember(dest => dest.DigitalCode, act => act.Ignore())
                .ForMember(dest => dest.DeliveryNoteNumber, act => act.Ignore())
                .ForMember(dest => dest.PickUpNoteNumber, act => act.Ignore())
                .ForMember(dest => dest.RefLmsBusinessTypeId, act => act.Ignore())
                .ForMember(dest => dest.RefLtmsTransactionRowGuid, act => act.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.RowGuid, act => act.MapFrom(src => Guid.NewGuid()));

            CreateMap<Olma.PostingRequest, PostingRequestsUpdateRequest>()
                .ReverseMap();
            CreateMap<Olma.PostingRequest, AccountingRecord>()
                .ForMember(dest => dest.TransactionId, act => act.Ignore())
                .ForMember(dest => dest.ProcessId, act => act.Ignore())
                .ForMember(dest => dest.Status, act => act.MapFrom(src => "In Übermittlung"))
                // HACK for enum we need a value conversion to our API Enums
                .ForMember(dest => dest.Type, act => act.MapFrom(src =>
                    src.Reason == PostingRequestReason.LoadCarrierReceipt
                        ? (src.Type == PostingRequestType.Charge
                            ? Contracts.Models.AccountingRecordType.ANL
                            //ToDo (Dominik): hier muss noch überprüft werden, welcher AccountType und welche PalletQuality gebucht werden. 
                            : Contracts.Models.AccountingRecordType.ABH)
                        : src.Reason == PostingRequestReason.Transfer
                            ? Contracts.Models.AccountingRecordType.KUZ
                            : src.Reason == PostingRequestReason.Voucher
                                ? Contracts.Models.AccountingRecordType.AUSDPG
                                // fallback = Undefined
                                : Contracts.Models.AccountingRecordType.U))
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Quantity,
                    act => act.MapFrom(src =>
                        src.Type == PostingRequestType.Credit ? src.LoadCarrierQuantity : -1 * src.LoadCarrierQuantity))
                .ForMember(dest => dest.CreateTime, act => act.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdateTime, act => act.MapFrom(src => src.ChangedAt))
                .ForMember(dest => dest.HasDplNote, act => act.MapFrom(src => src.DplNote != null))                
                .ForMember(dest => dest.ExtDescription, act => act.Ignore())
                .ForMember(dest => dest.Credit, act => act.MapFrom(src => src.Type == PostingRequestType.Credit ? src.LoadCarrierQuantity : (int?) null))
                .ForMember(dest => dest.Charge, act => act.MapFrom(src => src.Type == PostingRequestType.Charge ? src.LoadCarrierQuantity : (int?) null))
                .ForMember(dest => dest.QualityName, act => act.MapFrom(src => src.LoadCarrier.Quality.Name))
                .ForMember(dest => dest.LoadCarrierTypeName, act => act.MapFrom(src => src.LoadCarrier.Type.Name))
                // HACK once this is included PostingRecordTypeMapping fails
                .ForMember(dest => dest.DplNote, act => act.Ignore())
                ;

            CreateMap<PostingRequestsCreateRequest, LTMS.Bookings>()
                // HACK Fake reference number from ID
                .ForMember(dest => dest.ReferenceNumber, act => act.MapFrom(src => src.ReferenceNumber))
                .ForMember(dest => dest.CreateTime, act => act.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreateUser, act => act.MapFrom(src => "OLMA"))
                .ForMember(dest => dest.BookingDate, act => act.MapFrom(src => DateTime.Today))
                .ForMember(dest => dest.IncludeInBalance, act => act.MapFrom(src => false))
                .ForMember(dest => dest.Matched, act => act.MapFrom(src => false))
                .ForMember(dest => dest.ExtDescription, act => act.MapFrom(src => src.Note))
                .ForMember(dest => dest.AccountId,
                    act => act.MapFrom(src => 6686)) // HACK Hardcoded AccountId of "Konto unbekannt"
                .ForMember(dest => dest.ArticleId, act => act.MapFrom(src => 0)) // Preset Article undefined
                .ForMember(dest => dest.QualityId, act => act.MapFrom(src => 0)) // Preset Quality undefined
                .ForMember(dest => dest.Quantity, act => act.Ignore())
                .ForAllOtherMembers(act => act.Ignore());
            CreateMap<PostingRequestsCreateRequest, LTMS.Transactions>()
                .ForMember(dest => dest.ReferenceNumber, act => act.MapFrom(src => src.ReferenceNumber))
                .ForMember(dest => dest.CreateTime, act => act.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreateUser, act => act.MapFrom(src => "OLMA"))
                .ForMember(dest => dest.Valuta, act => act.MapFrom(src => DateTime.Today))
                .ForMember(dest => dest.RowGuid, act => act.MapFrom(src => src.RefLtmsTransactionId))
                .ForMember(dest => dest.Process, act => act.MapFrom(src => src))
                .ForMember(dest => dest.TransactionTypeId, act => act.MapFrom(src => "M"))
                .ForAllOtherMembers(act => act.Ignore());
            CreateMap<PostingRequestsCreateRequest, LTMS.Processes>()
                .ForMember(dest => dest.CreateTime, act => act.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreateUser, act => act.MapFrom(src => "OLMA"))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => "OLMA Simulated Process"))
                .ForMember(dest => dest.ProcessStateId, act => act.MapFrom(src => 1))
                .ForMember(dest => dest.ProcessTypeId, act => act.MapFrom(src => (short) src.RefLtmsProcessTypeId))
                .ForMember(dest => dest.RowGuid, act => act.MapFrom(src => src.RefLtmsProcessId))
                .ForAllOtherMembers(act => act.Ignore());

            #endregion

            #region (Customer)Partners

            CreateMap<Olma.Partner, Contracts.Models.Partner>();

            CreateMap<Olma.Partner, Contracts.Models.CustomerPartner>()
                .ForMember(dest => dest.PostingAccountId, act => act.MapFrom(src => src.DefaultPostingAccountId))
                .ForMember(dest => dest.Address, act => act.MapFrom(src => src.DefaultAddress))
                .ForMember(dest => dest.Guid, act => act.MapFrom(src => src.RowGuid))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type))
                .ForMember(dest => dest.DirectoryIds,
                    act => act.MapFrom(src => src.DirectoryAccesses.Select(i => i.DirectoryId)))
                .ForMember(dest => dest.DirectoryNames,
                    act => act.MapFrom(src => src.DirectoryAccesses.Select(i => i.Directory.Name)));
            CreateMap<Olma.CustomerPartner, Contracts.Models.CustomerPartner>()
                .ForMember(dest => dest.Guid, act => act.MapFrom(src => src.RowGuid))
                .ForMember(dest => dest.PostingAccountId,
                    act => act.MapFrom(src => src.Partner == null ? null : src.Partner.DefaultPostingAccountId))
                .ForMember(dest => dest.DirectoryIds,
                    act => act.MapFrom(src => src.DirectoryAccesses.Select(i => i.DirectoryId)))
                .ForMember(dest => dest.DirectoryNames, act => act.MapFrom(src =>
                    src.DirectoryAccesses.Select(i => i.Directory == null ? string.Empty : i.Directory.Name)));
            CreateMap<CustomerPartnersCreateRequest, Olma.CustomerPartner>()
                .ForMember(dest => dest.CompanyName, act => act.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.RowGuid, act => act.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Address, act => act.Ignore())
                .ForMember(dest => dest.CustomerReference, act => act.Ignore())
                .ForMember(dest => dest.CreatedById, act => act.Ignore())
                .ForMember(dest => dest.CreatedBy, act => act.Ignore())
                .ForMember(dest => dest.CreatedAt, act => act.Ignore())
                .ForMember(dest => dest.ChangedById, act => act.Ignore())
                .ForMember(dest => dest.ChangedBy, act => act.Ignore())
                .ForMember(dest => dest.ChangedAt, act => act.Ignore())
                .ForMember(dest => dest.DeletedById, act => act.Ignore())
                .ForMember(dest => dest.DeletedBy, act => act.Ignore())
                .ForMember(dest => dest.DeletedAt, act => act.Ignore())
                .ForMember(dest => dest.IsDeleted, act => act.Ignore())
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.LoadingLocations, act => act.Ignore())
                .ForMember(dest => dest.PartnerId, act => act.Ignore())
                .ForMember(dest => dest.Partner, act => act.Ignore())
                .ForMember(dest => dest.DirectoryAccesses, act => act.Ignore());

            #endregion

            #region Partner directories

            CreateMap<Olma.PartnerDirectory, Contracts.Models.PartnerDirectory>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.Partners, act => act.MapFrom(src => src.CustomerPartnerDirectoryAccesses.Select(pp => pp.CustomerPartnerId).ToList()));

            #endregion

            #region Person

            CreateMap<Olma.Person, Contracts.Models.Person>();
            CreateMap<Olma.Person, Contracts.Models.UserCreateRequest>()
                .ForMember(dest => dest.Upn, act => act.Ignore())
                .ForMember(dest => dest.Role, act => act.Ignore())
                .ForMember(dest => dest.CustomerId, act => act.Ignore())
                .ReverseMap();

            #endregion

            #region Transport Offering

            // this uses projection parameters, please see LoadCarrierOfferingsService Search fucntion (projection call)
            // on how to supply the parameter when executing the map
            NetTopologySuite.Geometries.Point transportOfferingsSearchRequestPoint = null;
            int[] transportOfferingsSearchRequestDivisionIds = { };

            CreateMap<Olma.Transport, TransportOffering>()
                .ForMember(dest => dest.Status, act => act.MapFrom(src =>
                    src.Status == Olma.TransportStatus.Requested
                        ? src.Bids.Any(i =>
                            i.Status == TransportBidStatus.Declined && i.IsDeleted == false &&
                            transportOfferingsSearchRequestDivisionIds.Contains(i.DivisionId))
                            ? TransportOfferingStatus.Declined
                            : src.Bids.Any(i =>
                                i.Status == TransportBidStatus.Active && i.IsDeleted == false &&
                                transportOfferingsSearchRequestDivisionIds.Contains(i.DivisionId))
                                ? TransportOfferingStatus.BidPlaced
                                : TransportOfferingStatus.Available
                        : src.Status == Olma.TransportStatus.Canceled
                            ? TransportOfferingStatus.Canceled
                            : src.WinningBid != null &&
                              transportOfferingsSearchRequestDivisionIds.Contains(src.WinningBid.DivisionId)
                                ? src.WinningBid.Status == TransportBidStatus.Accepted
                                    ? TransportOfferingStatus.Accepted
                                    : TransportOfferingStatus.Won
                                : src.Bids.Any(i =>
                                    i.Status == TransportBidStatus.Declined && i.IsDeleted == false &&
                                    transportOfferingsSearchRequestDivisionIds.Contains(i.DivisionId))
                                    ? TransportOfferingStatus.Declined
                                    : src.Status == Olma.TransportStatus.Allocated
                                        ? src.Bids.Any(i =>
                                            i.IsDeleted == false &&
                                            transportOfferingsSearchRequestDivisionIds.Contains(i.DivisionId))
                                            ? TransportOfferingStatus.BidPlaced
                                            : TransportOfferingStatus.Available
                                        : TransportOfferingStatus.Lost))
                .ForMember(dest => dest.LoadType, act => act.MapFrom(i => TransportLoadType.LoadCarrier))
                .ForMember(dest => dest.SupplyInfo, act => act.MapFrom(i => i.OrderMatch.Supply.Order))
                .ForMember(dest => dest.DemandInfo, act => act.MapFrom(i => i.OrderMatch.Demand.Order))
                .ForMember(dest => dest.Bids, act => act.MapFrom(i => i.Bids.OrderByDescending(i => i.CreatedAt)))
                .ForMember(dest => dest.Distance, act => act.MapFrom(src => src.RoutedDistance))
                //Math.Round(src.OrderMatch.Demand.LoadingLocation.Address.GeoLocation.Distance(src.OrderMatch.Supply.LoadingLocation.Address.GeoLocation) / 10000) * 10000
                // : src.OrderMatch.Demand.LoadingLocation.Address.GeoLocation.Distance(src.OrderMatch.Supply.LoadingLocation.Address.GeoLocation)));
                .ForMember(dest => dest.LoadCarrierLoad, act => act.MapFrom(src => src.OrderMatch))
                .ForMember(dest => dest.StackHeightMin, act => act.Ignore())
                .ForMember(dest => dest.StackHeightMax, act => act.Ignore())
                .ForMember(dest => dest.SupportsRearLoading, act => act.Ignore())
                .ForMember(dest => dest.SupportsSideLoading, act => act.Ignore())
                .ForMember(dest => dest.SupportsJumboVehicles, act => act.Ignore())

                //.ForMember(dest => dest.Distance,
                //    act => act.MapFrom(i => i.LoadingLocation.Address.GeoLocation.Distance(loadCarrierOfferingsSearchRequestPoint)))
                ;

            CreateMap<Olma.OrderMatch, TransportOfferingLoadCarrierLoad>()
                .ForMember(dest => dest.LoadCarrierId, act => act.MapFrom(src => src.LoadCarrierId))
                .ForMember(dest => dest.LoadCarrierTypeId, act => act.MapFrom(src => src.LoadCarrier.TypeId))
                // TODO Hack assumes 33 stacks per load, we need to make number of statcks dependent on vehicle type (Jumbo/non Jumbo for starters)
                .ForMember(dest => dest.LoadCarrierQuantity,
                    act => act.MapFrom(src => 33 * src.LoadCarrierStackHeight * src.LoadCarrier.Type.QuantityPerEur))
                ;

            CreateMap<Olma.Order, TransportOfferingInfo>()
                .ForMember(dest => dest.Address, act => act.MapFrom(i => i.LoadingLocation.Address))
                .ForMember(dest => dest.BusinessHours, act => act.MapFrom(i => i.LoadingLocation.BusinessHours))
                ;

            CreateMap<Olma.TransportBid, TransportOfferingBid>()
                .ForMember(dest => dest.SubmittedDateTime, act => act.MapFrom(i => i.CreatedAt))
                ;

            CreateMap<Olma.TransportBid, TransportOfferingBidCreateRequest>()
                .ReverseMap()
                .ForMember(dest => dest.Status, act => act.MapFrom(i => TransportBidStatus.Active));

            #endregion

            #region Vouchers / Voucher Reason Types

            // reverse map
            CreateMap<Olma.Voucher, VouchersCreateRequest>()
                .ForMember(dest => dest.ExpressCode, act => act.Ignore())
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type))
                .ForMember(dest => dest.RecipientGuid, act => act.Ignore())
                .ForMember(dest => dest.SupplierGuid, act => act.Ignore())
                .ForMember(dest => dest.ShipperGuid, act => act.Ignore())
                .ForMember(dest => dest.SubShipperGuid, act => act.Ignore())
                .ForMember(dest => dest.PrintLanguageId, act => act.Ignore())
                .ForMember(dest => dest.PrintCount, act => act.Ignore())
                .ForMember(dest => dest.PrintDateTimeOffset, act => act.Ignore())
                .ForMember(dest => dest.DplNote, act => act.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.RowGuid, act => act.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ExpressCode, act => act.Ignore())
                .ForMember(dest => dest.DplNotes, act => act.MapFrom(src => src.DplNote != null
                    ? new List<EmployeeNoteCreateRequest>() {src.DplNote}
                    : null));

            CreateMap<Olma.Voucher, VoucherCreateSyncRequest>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.RowGuid))
                .ForMember(dest => dest.Number, act => act.MapFrom(src =>  src.Document.Number))
                .ForMember(dest => dest.Type, act => act.MapFrom(src =>(int) src.Type))
                .ForMember(dest => dest.VoucherReason, act => act.MapFrom(src => src.ReasonType.RefLtmsReasonTypeId))
                .ForMember(dest => dest.Recipient, act => act.MapFrom(src => src.Recipient.CompanyName))
                .ForMember(dest => dest.RefLtmsIssuerAccountId, act => act.MapFrom(src => src.CustomerDivision.PostingAccount.RefLtmsAccountId))
                .ForMember(dest => dest.RefLtmsRecipientAccountId, act => act.MapFrom(src => src.Type == VoucherType.Direct ? src.ReceivingPostingAccount.RefLtmsAccountId : (int?) null))
                .ForMember(dest => dest.IssueDate, act => act.MapFrom(src => (DateTime)src.CreatedAt))
                .ForMember(dest => dest.DateOfExpiry, act => act.MapFrom(src => (DateTime)src.ValidUntil))
                .ForMember(dest => dest.RefLtmsTransactionRowGuid, act => act.MapFrom(src => src.PostingRequests.FirstOrDefault() != null ? src.PostingRequests.FirstOrDefault().RefLtmsTransactionId : (Guid?) null))
                ;
            CreateMap<Olma.VoucherPosition, VoucherRequestPosition>()
                .ForMember(dest => dest.RefLtmsPalletId, act => act.MapFrom(src => src.LoadCarrier.RefLtmsPalletId))
                .ForMember(dest => dest.Quantity, act => act.MapFrom(src => src.LoadCarrierQuantity));
            // reverse map
            CreateMap<Olma.VoucherPosition, VouchersCreateRequestPosition>()
                .ForMember(dest => dest.Quantity, act => act.MapFrom(src => src.LoadCarrierQuantity))
                .ReverseMap();

            CreateMap<Olma.Voucher, Contracts.Models.Voucher>()
                .ForMember(dest => dest.DivisionId, act => act.MapFrom(src => src.CustomerDivisionId))
                .ForMember(dest => dest.RecipientType, act => act.MapFrom(src => src.RecipientType))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type))
                .ForMember(dest => dest.Status, act => act.MapFrom(src => src.Status))
                .ForMember(dest => dest.IssuedDate, act => act.MapFrom(src => src.Document.IssuedDateTime))
                .ForMember(dest => dest.CancellationReason, act => act.MapFrom(src => src.Document.CancellationReason))
                .ForMember(dest => dest.DocumentId, act => act.MapFrom(src => src.Document.Id))
                .ForMember(dest => dest.DocumentNumber, act => act.MapFrom(src => src.Document.Number))
                .ForMember(dest => dest.HasDplNote, act => act.MapFrom(src => src.DplNotes.Any()))

                //.ForMember(dest => dest.RecipientType, act => act.MapFrom(src => (PartnerType)src.Recipient.Type))
                //.ForMember(dest => dest.Recipient, act => act.MapFrom(src => src.Recipient.CompanyName))
                .ForMember(dest => dest.Supplier, act => act.MapFrom(src => src.Supplier.CompanyName))
                .ForMember(dest => dest.Shipper, act => act.MapFrom(src => src.Shipper.CompanyName))
                .ForMember(dest => dest.SubShipper, act => act.MapFrom(src => src.SubShipper.CompanyName))

                // HACK to get results for Recipients
                .ForMember(dest => dest.DivisionName, act => act.Ignore())
                .ForMember(dest => dest.IssuerCompanyName, act => act.Ignore())
                .ForMember(dest => dest.Recipient, act => act.Ignore())
                //.ForMember(dest => dest.DivisionName, act => act.MapFrom(src => src.CustomerDivision.Name))
                //.ForMember(dest => dest.IssuerCompanyName, act => act.MapFrom(src => src.CustomerDivision.Customer.Name))
                //.ForMember(dest => dest.Positions, act => act.MapFrom(src => src.Positions))
                .ForMember(dest => dest.AccountingRecords, act => act.Ignore())
                .ForMember(dest => dest.DownloadLink, act => act.Ignore());

            CreateMap<Olma.VoucherPosition, Contracts.Models.VoucherPosition>()
                .ForMember(dest => dest.Quantity, act => act.MapFrom(src => src.LoadCarrierQuantity));

            CreateMap<Olma.VoucherReasonType, Contracts.Models.VoucherReasonType>();

            #endregion

            #region Users + Permissions

            CreateMap<Olma.User, Contracts.Models.User>()
                .ForMember(dest => dest.Settings, act => act.MapFrom(src => src.UserSettings.Data))
                .ForMember(dest => dest.Customers, act => act.Ignore())
                .ForMember(dest => dest.PostingAccounts, act => act.Ignore())
                .ForMember(dest => dest.Permissions, act => act.Ignore());

            CreateMap<Olma.Permission, UserPermission>()
                .ForMember(dest => dest.Resource, act => act.MapFrom(src => src.Resource))
                .ForMember(dest => dest.Action, act => act.MapFrom<ActionResolver>())
                ;
            CreateMap<Olma.User, UserListItem>()
                .ForMember(dest => dest.PersonId, act => act.MapFrom(src => src.Person.Id))
                .ForMember(dest => dest.Gender, act => act.MapFrom(src => src.Person.Gender))
                .ForMember(dest => dest.Salutation, act => act.MapFrom(src => src.Person.Salutation))
                .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.Person.LastName))
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Person.Email))
                .ForMember(dest => dest.PhoneNumber, act => act.MapFrom(src => src.Person.PhoneNumber))
                .ForMember(dest => dest.MobileNumber, act => act.MapFrom(src => src.Person.MobileNumber))
                .ForMember(dest => dest.FirstLoginPassword, act => act.Ignore());
            
            CreateMap<Olma.Permission, GroupPermission>()
                .ForMember(dest => dest.Resource, act => act.MapFrom(src => src.Resource));

            CreateMap<Olma.UserGroup, UserGroupMembership>()
                .ForMember(dest => dest.Users, act => act.MapFrom(src => src.Users.Select(u => u.User)))
                .ForMember(dest => dest.UserGroup, act => act.MapFrom(src => src))
                
                ;
            
            CreateMap<UserGroupCreateRequest, Olma.OrganizationUserGroup>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsSystemGroup, act => act.MapFrom(src => src.IsSystemGroup))
                .ForMember(dest => dest.IsHidden, act => act.MapFrom(src => src.IsHidden))
                .ForMember(dest => dest.OrganizationId, act => act.MapFrom(src => src.OrganizationId))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => Olma.UserGroupType.Organization))
                .ForMember(dest => dest.Permissions, act => act.MapFrom(src => src.Permissions))
                .ForAllOtherMembers(act => act.Ignore())
                ;
            CreateMap<UserGroupCreateRequest, Olma.CustomerUserGroup>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsSystemGroup, act => act.MapFrom(src => src.IsSystemGroup))
                .ForMember(dest => dest.IsHidden, act => act.MapFrom(src => src.IsHidden))
                .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => Olma.UserGroupType.Customer))
                .ForMember(dest => dest.Permissions, act => act.MapFrom(src => src.Permissions))
                .ForAllOtherMembers(act => act.Ignore())
                ;
            CreateMap<UserGroupCreateRequest, Olma.CustomerDivisionUserGroup>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsSystemGroup, act => act.MapFrom(src => src.IsSystemGroup))
                .ForMember(dest => dest.IsHidden, act => act.MapFrom(src => src.IsHidden))
                .ForMember(dest => dest.CustomerDivisionId, act => act.MapFrom(src => src.DivisionId))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => Olma.UserGroupType.CustomerDivision))
                .ForMember(dest => dest.Permissions, act => act.MapFrom(src => src.Permissions))
                .ForAllOtherMembers(act => act.Ignore())
                ;

            CreateMap<GroupPermission, Olma.Permission>()
                .ForMember(dest => dest.Action, act => act.MapFrom(src => src.Action))
                .ForMember(dest => dest.Resource, act => act.MapFrom(src => src.Resource))
                .ForMember(dest => dest.ReferenceId, act => act.MapFrom(src => src.ReferenceId))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => Olma.PermissionType.Allow))
                .ForMember(dest => dest.Scope, act => act.MapFrom(src => Olma.PermissionScope.Group))
                .ForAllOtherMembers(act => act.Ignore())
                ;
            
            CreateMap<Olma.UserGroup, UserGroupListItem>();
            CreateMap<Olma.OrganizationUserGroup, UserGroupListItem>();
            CreateMap<Olma.CustomerDivisionUserGroup, UserGroupListItem>();
            CreateMap<Olma.CustomerUserGroup, UserGroupListItem>();

            #endregion
        }
    }

    public class ActionResolver : IValueResolver<Olma.Permission, UserPermission, ResourceAction>
    {
        public ResourceAction Resolve(Olma.Permission permission, UserPermission destination, ResourceAction destMember,
            ResolutionContext context)
        {
            return (ResourceAction) Enum.Parse(typeof(ResourceAction), permission.Action);

            //switch (permission.Resource)
            //{
            //    case PermissionResourceType.Organization:
            //        return (OrganizationAction)Enum.Parse(typeof(OrganizationAction), permission.Action);
            //    case PermissionResourceType.Customer:
            //        return (CustomerAction)Enum.Parse(typeof(CustomerAction), permission.Action);
            //    case PermissionResourceType.Division:
            //        return (DivisionAction)Enum.Parse(typeof(DivisionAction), permission.Action);
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }
    }
}