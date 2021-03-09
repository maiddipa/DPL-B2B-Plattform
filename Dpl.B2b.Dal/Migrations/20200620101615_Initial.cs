using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingRecordTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefLtmBookingTypeId = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingRecordTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    LicensePlateCode = table.Column<string>(maxLength: 255, nullable: true),
                    Iso2Code = table.Column<string>(maxLength: 255, nullable: true),
                    Iso3Code = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    CanBeCanceled = table.Column<bool>(nullable: false),
                    IsCanceled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    ShortName = table.Column<string>(maxLength: 255, nullable: true),
                    HasReport = table.Column<bool>(nullable: false),
                    BusinessDomain = table.Column<int>(nullable: false),
                    PrintType = table.Column<int>(nullable: false),
                    OriginalAvailableForMinutes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    InternalFullPathAndName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierQualities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefLtmsQualityId = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Order = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierQualities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefLtmsArticleId = table.Column<short>(nullable: false),
                    RefLmsLoadCarrierTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Order = table.Column<float>(nullable: false),
                    QuantityPerEur = table.Column<int>(nullable: false),
                    MaxStackHeight = table.Column<int>(nullable: false),
                    MaxStackHeightJumbo = table.Column<int>(nullable: false),
                    BaseLoadCarrier = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalizationItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    FieldName = table.Column<string>(maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalizationLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Locale = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostingRequestSyncError",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefLtmsTransactionId = table.Column<Guid>(nullable: false),
                    MessageId = table.Column<string>(maxLength: 255, nullable: true),
                    SessionId = table.Column<string>(maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    MessageDeliveryCount = table.Column<int>(nullable: false),
                    EnqueuedDateTime = table.Column<DateTime>(nullable: false),
                    DeadLetterReason = table.Column<string>(maxLength: 255, nullable: true),
                    DeadLetterErrorDescription = table.Column<string>(maxLength: 255, nullable: true),
                    SyncErrorStatus = table.Column<int>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostingRequestSyncError", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoucherReasonTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefLtmsReasonTypeId = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherReasonTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountryStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryStates_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierQualityMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefLmsQualityId = table.Column<int>(nullable: false),
                    LoadCarrierQualityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierQualityMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarrierQualityMappings_LoadCarrierQualities_LoadCarrierQualityId",
                        column: x => x.LoadCarrierQualityId,
                        principalTable: "LoadCarrierQualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarriers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefLmsQuality2PalletId = table.Column<int>(nullable: false),
                    RefLtmsPalletId = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Order = table.Column<float>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    QualityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarriers_LoadCarrierQualities_QualityId",
                        column: x => x.QualityId,
                        principalTable: "LoadCarrierQualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarriers_LoadCarrierTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "LoadCarrierTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocalizationTexts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneratedId = table.Column<string>(maxLength: 255, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    LocalizationItemId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: true),
                    LocalizationLanguageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalizationTexts_LocalizationItems_LocalizationItemId",
                        column: x => x.LocalizationItemId,
                        principalTable: "LocalizationItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocalizationTexts_LocalizationLanguages_LocalizationLanguageId",
                        column: x => x.LocalizationLanguageId,
                        principalTable: "LocalizationLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicHolidays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    CountryId = table.Column<int>(nullable: true),
                    CountryStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHolidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicHolidays_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublicHolidays_CountryStates_CountryStateId",
                        column: x => x.CountryStateId,
                        principalTable: "CountryStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseLoadCarrierMapping",
                columns: table => new
                {
                    LoadCarrierId = table.Column<int>(nullable: false),
                    LoadCarrierTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseLoadCarrierMapping", x => new { x.LoadCarrierId, x.LoadCarrierTypeId });
                    table.ForeignKey(
                        name: "FK_BaseLoadCarrierMapping_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseLoadCarrierMapping_LoadCarrierTypes_LoadCarrierTypeId",
                        column: x => x.LoadCarrierTypeId,
                        principalTable: "LoadCarrierTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalFieldValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdditionalFieldId = table.Column<int>(nullable: false),
                    ValueString = table.Column<string>(maxLength: 255, nullable: true),
                    ValueInt = table.Column<int>(nullable: true),
                    ValueFloat = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalFieldValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPartners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RowGuid = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    AddressId = table.Column<int>(nullable: true),
                    PartnerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPartners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RefLmsCustomerId = table.Column<Guid>(nullable: false),
                    CustomerNumber = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    LogoUrl = table.Column<string>(maxLength: 255, nullable: true),
                    DefaultLanguage = table.Column<string>(maxLength: 255, nullable: true),
                    IsPoolingPartner = table.Column<bool>(nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    PartnerId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Customers_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierReceipts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Trigger = table.Column<int>(nullable: false),
                    DigitalCode = table.Column<string>(maxLength: 255, nullable: true),
                    OrderLoadDetailId = table.Column<int>(nullable: true),
                    RefLmsAvail2DeliId = table.Column<int>(nullable: true),
                    PostingAccountId = table.Column<int>(nullable: true),
                    TargetPostingAccountId = table.Column<int>(nullable: true),
                    DocumentId = table.Column<int>(nullable: false),
                    CustomerDivisionId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(maxLength: 255, nullable: true),
                    TruckDriverName = table.Column<string>(maxLength: 255, nullable: true),
                    TruckDriverCompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    LicensePlate = table.Column<string>(maxLength: 255, nullable: true),
                    LicensePlateCountryId = table.Column<int>(nullable: true),
                    DeliveryNoteNumber = table.Column<string>(maxLength: 255, nullable: true),
                    DeliveryNoteShown = table.Column<bool>(nullable: true),
                    PickUpNoteNumber = table.Column<string>(maxLength: 255, nullable: true),
                    PickUpNoteShown = table.Column<bool>(nullable: true),
                    CustomerReference = table.Column<string>(maxLength: 255, nullable: true),
                    ShipperCompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    ShipperAddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceipts_Countries_LicensePlateCountryId",
                        column: x => x.LicensePlateCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadingLocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    CustomerDivisionId = table.Column<int>(nullable: true),
                    CustomerPartnerId = table.Column<int>(nullable: true),
                    PartnerId = table.Column<int>(nullable: true),
                    StackHeightMin = table.Column<int>(nullable: false),
                    StackHeightMax = table.Column<int>(nullable: false),
                    SupportsPartialMatching = table.Column<bool>(nullable: false),
                    SupportsRearLoading = table.Column<bool>(nullable: false),
                    SupportsSideLoading = table.Column<bool>(nullable: false),
                    SupportsJumboVehicles = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadingLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadingLocations_CustomerPartners_CustomerPartnerId",
                        column: x => x.CustomerPartnerId,
                        principalTable: "CustomerPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderLoadDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    PlannedFulfillmentDateTime = table.Column<DateTime>(nullable: false),
                    ActualFulfillmentDateTime = table.Column<DateTime>(nullable: true),
                    LoadCarrierReceiptId = table.Column<int>(nullable: true),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLoadDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RowGuid = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    IsPoolingPartner = table.Column<bool>(nullable: false),
                    DefaultAddressId = table.Column<int>(nullable: true),
                    AddressId = table.Column<int>(nullable: false),
                    DefaultPostingAccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Salutation = table.Column<string>(maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 255, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 255, nullable: true),
                    AddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Upn = table.Column<string>(maxLength: 255, nullable: true),
                    Role = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    UserSettingId = table.Column<int>(nullable: false),
                    PersonId = table.Column<int>(nullable: false),
                    Locked = table.Column<bool>(nullable: false),
                    OrderNotificationsEnabled = table.Column<bool>(nullable: false),
                    FirstLoginDate = table.Column<DateTime>(nullable: true),
                    LastLoginDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    PrintType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RefLmsAddressNumber = table.Column<int>(nullable: true),
                    Street1 = table.Column<string>(maxLength: 255, nullable: true),
                    Street2 = table.Column<string>(maxLength: 255, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 255, nullable: true),
                    City = table.Column<string>(maxLength: 255, nullable: true),
                    StateId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    GeoLocation = table.Column<Point>(nullable: true),
                    PartnerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_CountryStates_StateId",
                        column: x => x.StateId,
                        principalTable: "CountryStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessHourExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    FromDateTime = table.Column<DateTime>(nullable: false),
                    ToDateTime = table.Column<DateTime>(nullable: false),
                    LoadingLocationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessHourExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessHourExceptions_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessHourExceptions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessHourExceptions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessHourExceptions_LoadingLocations_LoadingLocationId",
                        column: x => x.LoadingLocationId,
                        principalTable: "LoadingLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LoadingLocationId = table.Column<int>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false),
                    FromTime = table.Column<DateTime>(nullable: false),
                    ToTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessHours_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessHours_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessHours_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessHours_LoadingLocations_LoadingLocationId",
                        column: x => x.LoadingLocationId,
                        principalTable: "LoadingLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDocumentSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true),
                    DocumentTypeId = table.Column<int>(nullable: false),
                    LoadCarrierTypeId = table.Column<int>(nullable: false),
                    ThresholdForWarningQuantity = table.Column<int>(nullable: false),
                    MaxQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDocumentSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDocumentSettings_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDocumentSettings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDocumentSettings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDocumentSettings_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDocumentSettings_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDocumentSettings_LoadCarrierTypes_LoadCarrierTypeId",
                        column: x => x.LoadCarrierTypeId,
                        principalTable: "LoadCarrierTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerIpSecurityRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IpVersion = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 255, nullable: true),
                    OrganizationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerIpSecurityRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerIpSecurityRules_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerIpSecurityRules_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerIpSecurityRules_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerIpSecurityRules_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentNumberSequences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DocumentType = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 255, nullable: true),
                    Counter = table.Column<int>(nullable: false),
                    Prefix = table.Column<string>(maxLength: 255, nullable: true),
                    SeparatorAfterPrefix = table.Column<string>(maxLength: 255, nullable: true),
                    CanAddDocumentTypeShortName = table.Column<bool>(nullable: false),
                    CanAddPaddingForCustomerNumber = table.Column<bool>(nullable: false),
                    PaddingLengthForCustomerNumber = table.Column<int>(nullable: false),
                    PostfixForCustomerNumber = table.Column<string>(maxLength: 255, nullable: true),
                    CanAddDivisionShortName = table.Column<bool>(nullable: false),
                    PrefixForCounter = table.Column<string>(maxLength: 255, nullable: true),
                    CanAddPaddingForCounter = table.Column<bool>(nullable: false),
                    PaddingLengthForCounter = table.Column<int>(nullable: false),
                    Separator = table.Column<string>(maxLength: 255, nullable: true),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentNumberSequences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentNumberSequences_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentNumberSequences_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentNumberSequences_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentNumberSequences_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentReports_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentReports_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentReports_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    PrintType = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTemplates_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentTemplates_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentTemplates_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentTemplates_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierReceiptPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    LoadCarrierQuantity = table.Column<int>(nullable: false),
                    ReceiptId = table.Column<int>(nullable: false),
                    LoadCarrierReceiptId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierReceiptPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptPosition_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptPosition_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptPosition_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptPosition_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptPosition_LoadCarrierReceipts_LoadCarrierReceiptId",
                        column: x => x.LoadCarrierReceiptId,
                        principalTable: "LoadCarrierReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderGroups_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderGroups_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderGroups_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderSeries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSeries_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderSeries_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderSeries_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartnerDirectories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerDirectories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerDirectories_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartnerDirectories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartnerDirectories_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SortingInterruptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    FromDateTime = table.Column<DateTime>(nullable: false),
                    ToDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortingInterruptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SortingInterruptions_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingInterruptions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingInterruptions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SortingWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceStaffNumber = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortingWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SortingWorkers_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingWorkers_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingWorkers_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubmitterProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmitterProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmitterProfiles_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubmitterProfiles_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubmitterProfiles_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostingAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 255, nullable: true),
                    RefLtmsAccountId = table.Column<int>(nullable: false),
                    RefLtmsAccountCustomerNumber = table.Column<string>(maxLength: 255, nullable: true),
                    AddressId = table.Column<int>(nullable: true),
                    PartnerId = table.Column<int>(nullable: true),
                    CalculatedBalanceId = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    PartnerId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostingAccounts", x => x.Id);
                    table.UniqueConstraint("AK_PostingAccounts_RefLtmsAccountId", x => x.RefLtmsAccountId);
                    table.ForeignKey(
                        name: "FK_PostingAccounts_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingAccounts_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingAccounts_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingAccounts_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingAccounts_PostingAccounts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingAccounts_Partners_PartnerId1",
                        column: x => x.PartnerId1,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplateLabel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: true),
                    DocumentTemplateId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 255, nullable: true),
                    Text = table.Column<string>(maxLength: 255, nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplateLabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTemplateLabel_DocumentTemplates_DocumentTemplateId",
                        column: x => x.DocumentTemplateId,
                        principalTable: "DocumentTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentTemplateLabel_LocalizationLanguages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LocalizationLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPartnerDirectoryAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerPartnerId = table.Column<int>(nullable: false),
                    DirectoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPartnerDirectoryAccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPartnerDirectoryAccesses_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerPartnerDirectoryAccesses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerPartnerDirectoryAccesses_CustomerPartners_CustomerPartnerId",
                        column: x => x.CustomerPartnerId,
                        principalTable: "CustomerPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerPartnerDirectoryAccesses_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerPartnerDirectoryAccesses_PartnerDirectories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "PartnerDirectories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationPartnerDirectories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(nullable: false),
                    PartnerDirectoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationPartnerDirectories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationPartnerDirectories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationPartnerDirectories_PartnerDirectories_PartnerDirectoryId",
                        column: x => x.PartnerDirectoryId,
                        principalTable: "PartnerDirectories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartnerDirectoryAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PartnerId = table.Column<int>(nullable: false),
                    DirectoryId = table.Column<string>(maxLength: 255, nullable: true),
                    DirectoryId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerDirectoryAccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerDirectoryAccesses_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartnerDirectoryAccesses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartnerDirectoryAccesses_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartnerDirectoryAccesses_PartnerDirectories_DirectoryId1",
                        column: x => x.DirectoryId1,
                        principalTable: "PartnerDirectories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartnerDirectoryAccesses_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSortingWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: true),
                    SortingWorkerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSortingWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSortingWorkers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSortingWorkers_SortingWorkers_SortingWorkerId",
                        column: x => x.SortingWorkerId,
                        principalTable: "SortingWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SortingShiftLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    FromDateTime = table.Column<DateTime>(nullable: false),
                    ToDateTime = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(maxLength: 255, nullable: true),
                    ResponsibleUserId = table.Column<int>(nullable: false),
                    SortingWorkerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortingShiftLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogs_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogs_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogs_Users_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogs_SortingWorkers_SortingWorkerId",
                        column: x => x.SortingWorkerId,
                        principalTable: "SortingWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalculatedBalances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastBookingDateTime = table.Column<DateTime>(nullable: false),
                    LastPostingRequestDateTime = table.Column<DateTime>(nullable: false),
                    PostingAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedBalances_PostingAccounts_PostingAccountId",
                        column: x => x.PostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDivisions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    ShortName = table.Column<string>(maxLength: 255, nullable: true),
                    PostingAccountId = table.Column<int>(nullable: true),
                    DefaultLoadingLocationId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDivisions_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisions_LoadingLocations_DefaultLoadingLocationId",
                        column: x => x.DefaultLoadingLocationId,
                        principalTable: "LoadingLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisions_PostingAccounts_PostingAccountId",
                        column: x => x.PostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderConditions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PostingAccountId = table.Column<int>(nullable: false),
                    OrderType = table.Column<int>(nullable: false),
                    LatestDeliveryDayOfWeek = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderConditions_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderConditions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderConditions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderConditions_PostingAccounts_PostingAccountId",
                        column: x => x.PostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RefLmsOrderRowGuid = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    OrderNumber = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    TransportType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PostingAccountId = table.Column<int>(nullable: false),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    BaseLoadCarrierId = table.Column<int>(nullable: true),
                    LoadingLocationId = table.Column<int>(nullable: true),
                    QuantityType = table.Column<int>(nullable: false),
                    NumberOfStacks = table.Column<int>(nullable: true),
                    StackHeightMin = table.Column<int>(nullable: false),
                    StackHeightMax = table.Column<int>(nullable: false),
                    LoadCarrierQuantity = table.Column<int>(nullable: true),
                    SupportsPartialMatching = table.Column<bool>(nullable: false),
                    SupportsRearLoading = table.Column<bool>(nullable: false),
                    SupportsSideLoading = table.Column<bool>(nullable: false),
                    SupportsJumboVehicles = table.Column<bool>(nullable: false),
                    EarliestFulfillmentDateTime = table.Column<DateTime>(nullable: false),
                    LatestFulfillmentDateTime = table.Column<DateTime>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    SyncNote = table.Column<string>(maxLength: 255, nullable: true),
                    OrderSerieId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.UniqueConstraint("AK_Orders_RefLmsOrderRowGuid", x => x.RefLmsOrderRowGuid);
                    table.ForeignKey(
                        name: "FK_Orders_LoadCarriers_BaseLoadCarrierId",
                        column: x => x.BaseLoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "OrderGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_LoadingLocations_LoadingLocationId",
                        column: x => x.LoadingLocationId,
                        principalTable: "LoadingLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderSeries_OrderSerieId",
                        column: x => x.OrderSerieId,
                        principalTable: "OrderSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_PostingAccounts_PostingAccountId",
                        column: x => x.PostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SubmitterType = table.Column<int>(nullable: false),
                    SubmitterUserId = table.Column<int>(nullable: true),
                    SubmitterProfileId = table.Column<int>(nullable: true),
                    PostingAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_PostingAccounts_PostingAccountId",
                        column: x => x.PostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_SubmitterProfiles_SubmitterProfileId",
                        column: x => x.SubmitterProfileId,
                        principalTable: "SubmitterProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_SubmitterUserId",
                        column: x => x.SubmitterUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SortingShiftLogPositions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    SortingShiftLogId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SortingShiftLogPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogPositions_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogPositions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogPositions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogPositions_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SortingShiftLogPositions_SortingShiftLogs_SortingShiftLogId",
                        column: x => x.SortingShiftLogId,
                        principalTable: "SortingShiftLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalculatedBalancePositions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    ProvisionalBalance = table.Column<int>(nullable: false),
                    CoordinatedBalance = table.Column<int>(nullable: false),
                    ProvisionalCharge = table.Column<int>(nullable: false),
                    ProvisionalCredit = table.Column<int>(nullable: false),
                    InCoordinationCharge = table.Column<int>(nullable: false),
                    InCoordinationCredit = table.Column<int>(nullable: false),
                    UncoordinatedCharge = table.Column<int>(nullable: false),
                    UncoordinatedCredit = table.Column<int>(nullable: false),
                    LatestUncoordinatedCharge = table.Column<DateTime>(nullable: false),
                    LatestUncoordinatedCredit = table.Column<DateTime>(nullable: false),
                    PostingRequestBalanceCredit = table.Column<int>(nullable: false),
                    PostingRequestBalanceCharge = table.Column<int>(nullable: false),
                    CalculatedBalanceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedBalancePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedBalancePositions_CalculatedBalances_CalculatedBalanceId",
                        column: x => x.CalculatedBalanceId,
                        principalTable: "CalculatedBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalculatedBalancePositions_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDivisionDocumentSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DivisionId = table.Column<int>(nullable: false),
                    DocumentTypeId = table.Column<int>(nullable: false),
                    DocumentNumberSequenceId = table.Column<int>(nullable: false),
                    PrintCountMin = table.Column<int>(nullable: false),
                    PrintCountMax = table.Column<int>(nullable: false),
                    DefaultPrintCount = table.Column<int>(nullable: false),
                    Override = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDivisionDocumentSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDivisionDocumentSettings_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisionDocumentSettings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisionDocumentSettings_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisionDocumentSettings_CustomerDivisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisionDocumentSettings_DocumentNumberSequences_DocumentNumberSequenceId",
                        column: x => x.DocumentNumberSequenceId,
                        principalTable: "DocumentNumberSequences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDivisionDocumentSettings_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpressCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DigitalCode = table.Column<string>(maxLength: 255, nullable: true),
                    CustomBookingText = table.Column<string>(maxLength: 255, nullable: true),
                    ValidFrom = table.Column<DateTime>(nullable: true),
                    ValidTo = table.Column<DateTime>(nullable: true),
                    ValidDischarges = table.Column<int>(nullable: true),
                    IsCanceled = table.Column<bool>(nullable: false),
                    IssuingCustomerId = table.Column<int>(nullable: true),
                    VoucherReasonTypeId = table.Column<int>(nullable: true),
                    IssuingDivisionId = table.Column<int>(nullable: true),
                    PostingAccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpressCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpressCodes_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpressCodes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpressCodes_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpressCodes_Customers_IssuingCustomerId",
                        column: x => x.IssuingCustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpressCodes_CustomerDivisions_IssuingDivisionId",
                        column: x => x.IssuingDivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpressCodes_PostingAccounts_PostingAccountId",
                        column: x => x.PostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    IsSystemGroup = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    CustomerDivisionId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    OrganizationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_CustomerDivisions_CustomerDivisionId",
                        column: x => x.CustomerDivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierCondition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    MinQuantity = table.Column<int>(nullable: false),
                    MaxQuantity = table.Column<int>(nullable: false),
                    OrderConditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarrierCondition_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierCondition_OrderConditions_OrderConditionId",
                        column: x => x.OrderConditionId,
                        principalTable: "OrderConditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderLoad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    DemandOrderMatchId = table.Column<int>(nullable: true),
                    SupplyOrderMatchId = table.Column<int>(nullable: true),
                    DetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLoad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLoad_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLoad_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLoad_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLoad_OrderLoadDetail_DetailId",
                        column: x => x.DetailId,
                        principalTable: "OrderLoadDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLoad_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderSyncError",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: false),
                    MessageId = table.Column<string>(maxLength: 255, nullable: true),
                    SessionId = table.Column<string>(maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    MessageDeliveryCount = table.Column<int>(nullable: false),
                    EnqueuedDateTime = table.Column<DateTime>(nullable: false),
                    DeadLetterReason = table.Column<string>(maxLength: 255, nullable: true),
                    DeadLetterErrorDescription = table.Column<string>(maxLength: 255, nullable: true),
                    SyncErrorStatus = table.Column<int>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSyncError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSyncError_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryNotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DeliveryNoteNumber = table.Column<string>(maxLength: 255, nullable: true),
                    SubmissionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryNotes_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryNotes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryNotes_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryNotes_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpressCodePartnerPresets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    ExpressCodeId = table.Column<int>(nullable: false),
                    PartnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpressCodePartnerPresets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpressCodePartnerPresets_ExpressCodes_ExpressCodeId",
                        column: x => x.ExpressCodeId,
                        principalTable: "ExpressCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpressCodePartnerPresets_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostingAccountPresets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpressCodeId = table.Column<int>(nullable: false),
                    DestinationAccountId = table.Column<int>(nullable: true),
                    LoadCarrierId = table.Column<int>(nullable: true),
                    LoadCarrierQuantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostingAccountPresets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostingAccountPresets_PostingAccounts_DestinationAccountId",
                        column: x => x.DestinationAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingAccountPresets_ExpressCodes_ExpressCodeId",
                        column: x => x.ExpressCodeId,
                        principalTable: "ExpressCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ExpressCodeId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ValidUntil = table.Column<DateTime>(nullable: false),
                    ReasonTypeId = table.Column<int>(nullable: false),
                    RecipientId = table.Column<int>(nullable: false),
                    RecipientType = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: true),
                    ShipperId = table.Column<int>(nullable: true),
                    SubShipperId = table.Column<int>(nullable: true),
                    DocumentId = table.Column<int>(nullable: false),
                    CustomerDivisionId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(maxLength: 255, nullable: true),
                    ReceivingPostingAccountId = table.Column<int>(nullable: true),
                    ReceivingCustomerId = table.Column<int>(nullable: true),
                    TruckDriverName = table.Column<string>(maxLength: 255, nullable: true),
                    TruckDriverCompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    LicensePlate = table.Column<string>(maxLength: 255, nullable: true),
                    LicensePlateCountryId = table.Column<int>(nullable: true),
                    SubmissionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_CustomerDivisions_CustomerDivisionId",
                        column: x => x.CustomerDivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_ExpressCodes_ExpressCodeId",
                        column: x => x.ExpressCodeId,
                        principalTable: "ExpressCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_Countries_LicensePlateCountryId",
                        column: x => x.LicensePlateCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_VoucherReasonTypes_ReasonTypeId",
                        column: x => x.ReasonTypeId,
                        principalTable: "VoucherReasonTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_Customers_ReceivingCustomerId",
                        column: x => x.ReceivingCustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_PostingAccounts_ReceivingPostingAccountId",
                        column: x => x.ReceivingPostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_CustomerPartners_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "CustomerPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_CustomerPartners_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "CustomerPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_CustomerPartners_SubShipperId",
                        column: x => x.SubShipperId,
                        principalTable: "CustomerPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_CustomerPartners_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "CustomerPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Scope = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    UserGroupId = table.Column<int>(nullable: true),
                    Resource = table.Column<int>(nullable: false),
                    ReferenceId = table.Column<int>(nullable: true),
                    Action = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserUserGroupRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUserGroupRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserUserGroupRelations_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserUserGroupRelations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserUserGroupRelations_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserUserGroupRelations_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserUserGroupRelations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderMatches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrderMatchNumber = table.Column<string>(maxLength: 255, nullable: true),
                    RefLmsAvailabilityRowGuid = table.Column<Guid>(nullable: false),
                    RefLmsDeliveryRowGuid = table.Column<Guid>(nullable: false),
                    DigitalCode = table.Column<string>(maxLength: 255, nullable: true),
                    TransportType = table.Column<int>(nullable: false),
                    SelfTransportSide = table.Column<int>(nullable: true),
                    TransportId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    BaseLoadCarrierId = table.Column<int>(nullable: true),
                    DocumentId = table.Column<int>(nullable: true),
                    LoadCarrierStackHeight = table.Column<int>(nullable: false),
                    LoadCarrierQuantity = table.Column<int>(nullable: false),
                    NumberOfStacks = table.Column<int>(nullable: false),
                    BaseLoadCarrierQuantity = table.Column<int>(nullable: false),
                    SupportsRearLoading = table.Column<bool>(nullable: false),
                    SupportsSideLoading = table.Column<bool>(nullable: false),
                    SupportsJumboVehicles = table.Column<bool>(nullable: false),
                    DemandId = table.Column<int>(nullable: true),
                    SupplyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderMatches_LoadCarriers_BaseLoadCarrierId",
                        column: x => x.BaseLoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMatches_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMatches_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMatches_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMatches_OrderLoad_DemandId",
                        column: x => x.DemandId,
                        principalTable: "OrderLoad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMatches_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMatches_OrderLoad_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "OrderLoad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VoucherPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    LoadCarrierQuantity = table.Column<int>(nullable: false),
                    VoucherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherPosition_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherPosition_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherPosition_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherPosition_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherPosition_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Number = table.Column<string>(maxLength: 255, nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    IssuedDateTime = table.Column<DateTime>(nullable: false),
                    CancellationReason = table.Column<string>(maxLength: 255, nullable: true),
                    LtmsImportDateTime = table.Column<DateTime>(nullable: true),
                    CustomerDivisionId = table.Column<int>(nullable: false),
                    VoucherId = table.Column<int>(nullable: true),
                    LoadCarrierReceiptId = table.Column<int>(nullable: true),
                    OrderMatchId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_CustomerDivisions_CustomerDivisionId",
                        column: x => x.CustomerDivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_LocalizationLanguages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LocalizationLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_LoadCarrierReceipts_LoadCarrierReceiptId",
                        column: x => x.LoadCarrierReceiptId,
                        principalTable: "LoadCarrierReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_OrderMatches_OrderMatchId",
                        column: x => x.OrderMatchId,
                        principalTable: "OrderMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentStates_StateId",
                        column: x => x.StateId,
                        principalTable: "DocumentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RowGuid = table.Column<Guid>(nullable: false),
                    ReferenceNumber = table.Column<string>(maxLength: 255, nullable: true),
                    RefLtmsProcessId = table.Column<Guid>(nullable: false),
                    RefLtmsProcessTypeId = table.Column<int>(nullable: false),
                    RefLtmsTransactionId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Reason = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    VoucherId = table.Column<int>(nullable: true),
                    LoadCarrierReceiptId = table.Column<int>(nullable: true),
                    SubmissionId = table.Column<int>(nullable: true),
                    OrderMatchId = table.Column<int>(nullable: true),
                    PostingAccountId = table.Column<int>(nullable: false),
                    SourceRefLtmsAccountId = table.Column<int>(nullable: false),
                    DestinationRefLtmsAccountId = table.Column<int>(nullable: false),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    LoadCarrierQuantity = table.Column<int>(nullable: false),
                    Note = table.Column<string>(maxLength: 255, nullable: true),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    SyncNote = table.Column<string>(maxLength: 255, nullable: true),
                    OrderLoadDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostingRequests_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_LoadCarrierReceipts_LoadCarrierReceiptId",
                        column: x => x.LoadCarrierReceiptId,
                        principalTable: "LoadCarrierReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_OrderLoadDetail_OrderLoadDetailId",
                        column: x => x.OrderLoadDetailId,
                        principalTable: "OrderLoadDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_OrderMatches_OrderMatchId",
                        column: x => x.OrderMatchId,
                        principalTable: "OrderMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_PostingAccounts_PostingAccountId",
                        column: x => x.PostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostingRequests_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false),
                    DocumentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentFile_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentFile_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportBids",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "smallmoney", nullable: false),
                    PickupDate = table.Column<DateTime>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(maxLength: 255, nullable: true),
                    DivisionId = table.Column<int>(nullable: false),
                    TransportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportBids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportBids_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportBids_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportBids_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportBids_CustomerDivisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "CustomerDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReferenceNumber = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    OrderMatchId = table.Column<int>(nullable: true),
                    RefLmsAvail2DeliRowId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PlannedLatestBy = table.Column<DateTime>(nullable: false),
                    ConfirmedLatestBy = table.Column<DateTime>(nullable: false),
                    WinningBidId = table.Column<int>(nullable: true),
                    WinningBidId1 = table.Column<int>(nullable: true),
                    RoutedDistance = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transports_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transports_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transports_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transports_OrderMatches_OrderMatchId",
                        column: x => x.OrderMatchId,
                        principalTable: "OrderMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transports_TransportBids_WinningBidId1",
                        column: x => x.WinningBidId1,
                        principalTable: "TransportBids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Iso2Code", "Iso3Code", "LicensePlateCode", "Name" },
                values: new object[] { 1, "DE", "GER", "D", "Germany" });

            migrationBuilder.InsertData(
                table: "DocumentStates",
                columns: new[] { "Id", "CanBeCanceled", "IsCanceled", "Name" },
                values: new object[,]
                {
                    { 254, false, false, "erledigt" },
                    { 4, false, false, "gesendet" },
                    { 255, false, true, "storniert" },
                    { 3, false, false, "erstellt (beleglos)" },
                    { 2, true, false, "gedruckt" },
                    { 99, false, false, "Verfallen" },
                    { 1, true, false, "erstellt (mit Belegdruck)" },
                    { 200, false, false, "Ausgeglichen" },
                    { 5, false, false, "gespeichert" }
                });

            migrationBuilder.InsertData(
                table: "DocumentTemplates",
                columns: new[] { "Id", "ChangedAt", "ChangedById", "CreatedAt", "CreatedById", "CustomerId", "Data", "DeletedAt", "DeletedById", "IsDefault", "IsDeleted", "PrintType", "Version" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, null, null, null, true, false, 1, 1 },
                    { 2, null, null, null, null, null, null, null, null, true, false, 2, 1 },
                    { 3, null, null, null, null, null, null, null, null, true, false, 3, 1 },
                    { 4, null, null, null, null, null, null, null, null, true, false, 4, 1 },
                    { 5, null, null, null, null, null, null, null, null, true, false, 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "BusinessDomain", "HasReport", "Name", "OriginalAvailableForMinutes", "PrintType", "ShortName", "Type" },
                values: new object[,]
                {
                    { 6, 0, true, "LoadCarrierReceiptDelivery", 30, 4, "LCRD", 6 },
                    { 7, 0, true, "TransportVoucher", 0, 5, "TV", 7 },
                    { 5, 0, true, "LoadCarrierReceiptPickup", 30, 3, "LCRP", 5 },
                    { 3, 0, true, "VoucherOriginal", 0, 1, "VO", 3 },
                    { 2, 0, true, "VoucherDigital", 0, 1, "VD", 2 },
                    { 1, 0, true, "VoucherDirectReceipt", 0, 1, "VDR", 1 },
                    { 4, 0, true, "LoadCarrierReceiptExchange", 30, 2, "LCRE", 4 }
                });

            migrationBuilder.InsertData(
                table: "LoadCarrierQualities",
                columns: new[] { "Id", "Description", "Name", "Order", "RefLtmsQualityId", "Type" },
                values: new object[,]
                {
                    { 21, null, "Chep", 14f, (short)21, 0 },
                    { 32, null, "C", 5f, (short)29, 0 },
                    { 31, null, "B", 5f, (short)29, 0 },
                    { 30, null, "A", 5f, (short)29, 0 },
                    { 29, null, "2A+", 5f, (short)29, 0 },
                    { 28, null, "2BG", 5f, (short)28, 0 },
                    { 27, null, "OD oK", 20f, (short)27, 0 },
                    { 26, null, "OD mK", 19f, (short)26, 0 },
                    { 25, null, "EigVerw", 18f, (short)25, 0 },
                    { 24, null, "2B Rmp", 17f, (short)24, 0 },
                    { 23, null, "2B MT", 16f, (short)23, 0 },
                    { 22, null, "MPal", 15f, (short)22, 0 },
                    { 20, null, "Schrott", 13f, (short)20, 1 },
                    { 12, null, "2B", 5f, (short)12, 0 },
                    { 18, null, "Ungerein.", 11f, (short)18, 0 },
                    { -1, null, "Undef.", 0f, (short)0, 0 },
                    { 1, null, "Intakt", 1f, (short)1, 0 },
                    { 19, null, "neu", 1f, (short)19, 0 },
                    { 10, null, "1A", 2f, (short)10, 0 },
                    { 11, null, "2A", 4f, (short)11, 0 },
                    { 2, null, "Defekt", 6f, (short)2, 1 },
                    { 13, null, "2+", 3f, (short)13, 0 },
                    { 14, null, "Einweg", 7f, (short)14, 0 },
                    { 15, null, "GD mK", 8f, (short)15, 0 },
                    { 16, null, "GD oK", 9f, (short)16, 0 },
                    { 17, null, "Gereinigt", 10f, (short)17, 0 }
                });

            migrationBuilder.InsertData(
                table: "LoadCarrierTypes",
                columns: new[] { "Id", "BaseLoadCarrier", "Description", "MaxStackHeight", "MaxStackHeightJumbo", "Name", "Order", "QuantityPerEur", "RefLmsLoadCarrierTypeId", "RefLtmsArticleId" },
                values: new object[,]
                {
                    { 21, 1, null, 15, 20, "EPAL7", 2f, 2, 0, (short)21 },
                    { 10, 2, null, 18, 24, "E1KR", 5.5f, 4, 12, (short)10 },
                    { 9, 2, null, 18, 24, "E1EP", 5f, 4, 11, (short)9 },
                    { 6, 2, null, 12, 16, "E2EP", 6f, 4, 9, (short)6 },
                    { 20, 0, null, 15, 20, "H1GD", 4.5f, 1, 0, (short)20 },
                    { 19, 0, null, 0, 0, "H1_DMiete", 4.6f, 0, 0, (short)19 },
                    { 18, 0, null, 15, 20, "H1", 4f, 1, 5, (short)18 },
                    { 16, 0, null, 0, 0, "GB_DMiete", 90f, 0, 0, (short)16 },
                    { 4, 2, null, 12, 16, "E2KR", 6.5f, 4, 10, (short)4 },
                    { 8, 0, null, 15, 20, "EUR", 1f, 1, 2, (short)8 },
                    { -1, 0, null, 0, 0, "U", 99f, 1, 0, (short)0 },
                    { 1, 0, null, 15, 20, "CR1", 90f, 1, 0, (short)1 },
                    { 2, 0, null, 15, 20, "CR3", 90f, 1, 0, (short)2 },
                    { 13, 0, null, 2, 3, "GB", 90f, 0, 6, (short)13 },
                    { 5, 0, null, 15, 20, "DplHaPa", 90f, 2, 0, (short)5 },
                    { 7, 0, null, 15, 20, "Einweg", 90f, 1, 0, (short)7 },
                    { 3, 1, null, 15, 20, "DD", 3f, 2, 1, (short)3 }
                });

            migrationBuilder.InsertData(
                table: "LocalizationItems",
                columns: new[] { "Id", "FieldName", "Name", "Reference", "Type" },
                values: new object[,]
                {
                    { 40000, null, "OrderGroupHasOrders", "Dpl.B2b.Contracts.Rules.Messages.Warnings.Common.NoGuarantee", 3 },
                    { 107, null, "OrderStatus", "Dpl.B2b.Common.Enumerations.OrderStatus", 1 },
                    { 108, null, "OrderMatchStatus", "Dpl.B2b.Common.Enumerations.OrderMatchStatus", 1 },
                    { 109, null, "PostingAccountType", "Dpl.B2b.Contracts.Models.PostingAccountType", 1 },
                    { 110, null, "SubmissionType", "Dpl.B2b.Contracts.Models.SubmissionType", 1 },
                    { 111, null, "ResourceType", "Dpl.B2b.Common.Enumerations.PermissionResourceType", 1 },
                    { 112, null, "VoucherType", "Dpl.B2b.Common.Enumerations.VoucherType", 1 },
                    { 113, null, "VoucherStatus", "Dpl.B2b.Common.Enumerations.VoucherStatus", 1 },
                    { 114, null, "PartnerType", "Dpl.B2b.Common.Enumerations.PartnerType", 1 },
                    { 115, null, "DayOfWeek", "System.DayOfWeek", 1 },
                    { 116, null, "AdhocTranslations", "Dpl.B2b.Common.Enumerations.AdhocTranslations", 1 },
                    { 118, null, "PersonGender", "Dpl.B2b.Common.Enumerations.PersonGender", 1 },
                    { 119, null, "LoadCarrierReceiptType", "Dpl.B2b.Common.Enumerations.LoadCarrierReceiptType", 1 },
                    { 120, null, "TransportOfferingStatus", "Dpl.B2b.Contracts.Models.TransportOfferingStatus", 1 },
                    { 20006, null, "OrderQuantityType", "Dpl.B2b.Common.Enumerations.OrderQuantityType", 1 },
                    { 122, null, "DocumentTypeEnum", "Dpl.B2b.Common.Enumerations.DocumentTypeEnum", 1 },
                    { 123, "ReportVoucherCommonTitle", "DocumentTypeEnum", "Dpl.B2b.Common.Enumerations.DocumentTypeEnum", 1 },
                    { 124, null, "OrderLoadStatus", "Dpl.B2b.Common.Enumerations.OrderLoadStatus", 1 },
                    { 30000, null, "NotAllowedByRule", "Dpl.B2b.Contracts.Rules.Messages.Errors.Common.NotAllowedByRule", 2 },
                    { 30001, null, "OrderGroupCancel", "Dpl.B2b.Contracts.Rules.Messages.Errors.OrderGroup.OrderGroupCancel", 2 },
                    { 30002, null, "OrderGroupHasOpenOrConfirmedOrders", "Dpl.B2b.Contracts.Rules.Messages.Errors.OrderGroup.OrderGroupHasOpenOrConfirmedOrders", 2 },
                    { 30003, null, "OrderGroupHasOrders", "Dpl.B2b.Contracts.Rules.Messages.Errors.OrderGroup.OrderGroupHasOrders", 2 },
                    { 117, null, "UserRole", "Dpl.B2b.Common.Enumerations.UserRole", 1 },
                    { 121, null, "TransportBidStatus", "Dpl.B2b.Common.Enumerations.TransportBidStatus", 1 },
                    { 20005, null, "OrderTransportType", "Dpl.B2b.Common.Enumerations.OrderTransportType", 1 },
                    { 20003, null, "LoadCarrierQualityType", "Dpl.B2b.Common.Enumerations.LoadCarrierQualityType", 1 },
                    { 10001, null, "LoadCarriers", null, 0 },
                    { 10010, null, "LoadCarrierQualities", null, 0 },
                    { 10020, null, "LoadCarrierTypes", null, 0 },
                    { 10021, "LongName", "LoadCarrierTypes", null, 0 },
                    { 10030, null, "Countries", null, 0 },
                    { 10040, null, "CountryStates", null, 0 },
                    { 20004, null, "OrderType", "Dpl.B2b.Common.Enumerations.OrderType", 1 },
                    { 10060, null, "DocumentTypes", null, 0 },
                    { 10061, "ShortName", "DocumentTypes", null, 0 },
                    { 10062, "Description", "DocumentTypes", null, 0 },
                    { 10050, null, "PublicHolidays", null, 0 },
                    { 10080, null, "VoucherReasonTypes", null, 0 },
                    { 10070, null, "DocumentStates", null, 0 },
                    { 20001, null, "AccountingRecordStatus", "Dpl.B2b.Contracts.Models.AccountingRecordStatus", 1 },
                    { 20008, "description", "AccountingRecordType", "Dpl.B2b.Contracts.Models.AccountingRecordType", 1 },
                    { 20007, "name", "AccountingRecordType", "Dpl.B2b.Contracts.Models.AccountingRecordType", 1 },
                    { 20002, null, "LoadCarrierTransferStatus", "Dpl.B2b.Contracts.Models.BalanceTransferStatus", 1 },
                    { 10090, null, "LocalizationLanguages", null, 0 },
                    { 10083, "DescriptionReport", "VoucherReasonTypes", null, 0 },
                    { 10082, "Description", "VoucherReasonTypes", null, 0 },
                    { 10081, "ShortName", "VoucherReasonTypes", null, 0 },
                    { 20000, null, "AccountingRecordType", "Dpl.B2b.Contracts.Models.AccountingRecordType", 1 }
                });

            migrationBuilder.InsertData(
                table: "LocalizationLanguages",
                columns: new[] { "Id", "Locale", "Name" },
                values: new object[,]
                {
                    { 1, "de", "Deutsch" },
                    { 2, "en", "Englisch" },
                    { 3, "fr", "Französisch" },
                    { 4, "pl", "Polnisch" }
                });

            migrationBuilder.InsertData(
                table: "VoucherReasonTypes",
                columns: new[] { "Id", "Description", "Name", "Order", "RefLtmsReasonTypeId" },
                values: new object[,]
                {
                    { 2, "kein Tausch erfolgt. Aussteller tauschte nicht.", "KTG", 1, "KTM" },
                    { -1, "Keine Angaben", "k.A.", 99, "KAG" },
                    { 1, "kein Tausch notwendig.  Kunde ist DPL-Pooling Partner.", "PT.", 2, "IPT" },
                    { 3, "kein Tausch gewünscht. Fahrer verweigert.", "KTM", 3, "KTG" }
                });

            migrationBuilder.InsertData(
                table: "CountryStates",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Baden-Württemberg" },
                    { 16, 1, "Thuringia" },
                    { 15, 1, "Schleswig-Holstein" },
                    { 13, 1, "Saxony" },
                    { 12, 1, "Saarland" },
                    { 11, 1, "Rhineland-Palatinate" },
                    { 10, 1, "North Rhine-Westphalia" },
                    { 9, 1, "Mecklenburg-Vorpommern" },
                    { 14, 1, "Saxony-Anhalt" },
                    { 7, 1, "Hesse" },
                    { 6, 1, "Hamburg" },
                    { 5, 1, "Bremen" },
                    { 4, 1, "Brandenburg" },
                    { 3, 1, "Berlin" },
                    { 2, 1, "Bavaria" },
                    { 8, 1, "Lower Saxony" }
                });

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "CustomerId", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 4004, null, 4, "tableCell2", 1, "Achtung Annahmestelle!   Den quittierten Beleg unbedingt in Kopie aufbewahren!", 0 },
                    { 4003, null, 4, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 4002, null, 4, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 3993, null, 4, "tableCell12", 1, "Qualität", 0 },
                    { 4005, null, 4, "tableCell1", 1, "Hinweis:", 0 },
                    { 3988, null, 4, "label1", 1, "Datum", 0 },
                    { 3990, null, 4, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 3957, null, 3, "xrLabel23", 1, "Empfänger:", 0 },
                    { 3985, null, 3, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 3996, null, 4, "tableCell21", 1, "Angelieferte Ladungsträger:", 0 },
                    { 3991, null, 4, "label4", 1, "Kennzeichen", 0 },
                    { 3995, null, 4, "tableCell23", 1, "Menge", 0 },
                    { 4015, null, 4, "xrLabel19", 1, "Aussteller (Annahme-Quittung):", 0 },
                    { 3992, null, 4, "tableCell26", 1, "Bestätigung:", 0 },
                    { 3998, null, 4, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 3997, null, 4, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 3999, null, 4, "tableCell5", 1, "Anlieferung für Firma:", 0 },
                    { 4001, null, 4, "tableCell7", 1, "Firma auf anlieferndem LKW:", 0 },
                    { 4014, null, 4, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 3986, null, 3, "xrLabel19", 1, "Aussteller (Ausgabe-Quittung):", 0 },
                    { 4000, null, 4, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 3989, null, 4, "xrLabel23", 1, "Empfänger (Annahme-Quittung):", 0 },
                    { 6133, null, 5, "label5", 1, "Ladevorschriften:", 0 },
                    { 6142, null, 5, "tableCell1", 1, "", 0 },
                    { 6154, null, 5, "tableCell12", 1, "Qualität", 0 },
                    { 3994, null, 4, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 3969, null, 3, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 3963, null, 3, "tableCell23", 1, "Menge", 0 },
                    { 3967, null, 3, "tableCell5", 1, "Abholung für Firma:", 0 },
                    { 4027, null, 2, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 4026, null, 2, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 4028, null, 2, "tableCell5", 1, "Transport für Firma:", 0 },
                    { 4029, null, 2, "tableCell7", 1, "Firma auf LKW:", 0 },
                    { 4030, null, 2, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 4047, null, 2, "xrLabel19", 1, "Aussteller:", 0 },
                    { 4046, null, 2, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 4018, null, 2, "xrLabel23", 1, "Empfänger:", 0 },
                    { 3956, null, 3, "label1", 1, "Datum", 0 },
                    { 3958, null, 3, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 3959, null, 3, "label4", 1, "Kennzeichen", 0 },
                    { 3976, null, 3, "tableCell1", 1, "Hinweis:", 0 },
                    { 3970, null, 3, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 3971, null, 3, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 3972, null, 3, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 3973, null, 3, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 3974, null, 3, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 3975, null, 3, "tableCell2", 1, "Achtung Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 3964, null, 3, "tableCell21", 1, "Ausgegebene Ladungsträger:", 0 },
                    { 6139, null, 5, "tableCell13", 1, "", 0 },
                    { 3962, null, 3, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 3960, null, 3, "tableCell26", 1, "Bestätigung:", 0 },
                    { 3961, null, 3, "tableCell29", 1, "Qualität", 0 },
                    { 3966, null, 3, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 3965, null, 3, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 3968, null, 3, "tableCell7", 1, "Firma auf abholendem LKW:", 0 },
                    { 6152, null, 5, "tableCell15", 1, "Heckbeladung:", 0 },
                    { 6148, null, 5, "tableCell26", 1, "Jumbo LKW:", 0 },
                    { 6145, null, 5, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 2169, null, 1, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 2198, null, 1, "xrLabel22", 3, "Signature", 0 },
                    { 2194, null, 1, "xrLabel23", 3, "Destinataire :", 0 },
                    { 2192, null, 1, "xrLabel7", 3, "Palettenannahme-Quittung FR TODO", 0 },
                    { 4039, null, 2, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 4044, null, 2, "xrLabel22", 3, "Signature", 0 },
                    { 4038, null, 2, "xrLabel23", 3, "Destinataire :", 0 },
                    { 3978, null, 3, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 3983, null, 3, "xrLabel22", 3, "Signature", 0 },
                    { 3977, null, 3, "xrLabel23", 3, "Destinataire :", 0 },
                    { 4007, null, 4, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 4012, null, 4, "xrLabel22", 3, "Signature", 0 },
                    { 4006, null, 4, "xrLabel23", 3, "Destinataire :", 0 },
                    { 2191, null, 1, "label1", 4, "DPL-Code:", 0 },
                    { 2201, null, 1, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 2196, null, 1, "xrLabel22", 4, "Podpis", 0 },
                    { 2193, null, 1, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 2197, null, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 },
                    { 4042, null, 2, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 4043, null, 2, "xrLabel22", 4, "Podpis", 0 },
                    { 4045, null, 2, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 3981, null, 3, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 3982, null, 3, "xrLabel22", 4, "Podpis", 0 },
                    { 3984, null, 3, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 4010, null, 4, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 2189, null, 1, "label1", 3, "DPL-Code:", 0 },
                    { 3987, null, 4, "xrLabel23", 2, "Recipient:", 0 },
                    { 4008, null, 4, "xrLabel22", 2, "Signature", 0 },
                    { 4009, null, 4, "xrLabel19", 2, "Issuer:", 0 },
                    { 6150, null, 5, "tableCell19", 1, "Seitenbeladung:", 0 },
                    { 6149, null, 5, "tableCell20", 1, "", 0 },
                    { 6135, null, 5, "tableCell21", 1, "Zu transportierende Ladungsträger:", 0 },
                    { 6134, null, 5, "tableCell23", 1, "Menge", 0 },
                    { 6153, null, 5, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 4022, null, 2, "tableCell29", 1, "Qualität", 0 },
                    { 6147, null, 5, "tableCell27", 1, "", 0 },
                    { 6141, null, 5, "tableCell28", 1, "Ladezeit:", 0 },
                    { 6137, null, 5, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 6138, null, 5, "tableCell30", 1, "Anlieferzeit:", 0 },
                    { 6146, null, 5, "tableCell32", 1, "Stapelhöhe:", 0 },
                    { 6136, null, 5, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 6151, null, 5, "tableCell16", 1, "", 0 },
                    { 6140, null, 5, "tableCell5", 1, "Anlieferstelle:", 0 },
                    { 6144, null, 5, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 2195, null, 1, "label1", 2, "DPL-Code:", 0 },
                    { 2200, null, 1, "xrLabel19", 2, "Issuer:", 0 },
                    { 2190, null, 1, "xrLabel22", 2, "Signature", 0 },
                    { 2202, null, 1, "xrLabel23", 2, "Recipient:", 0 },
                    { 2199, null, 1, "xrLabel7", 2, "Palettenannahme-Beleg TODO", 0 },
                    { 4041, null, 2, "xrLabel19", 2, "Issuer:", 0 },
                    { 4040, null, 2, "xrLabel22", 2, "Signature", 0 },
                    { 4016, null, 2, "xrLabel23", 2, "Recipient:", 0 },
                    { 3980, null, 3, "xrLabel19", 2, "Issuer:", 0 },
                    { 3979, null, 3, "xrLabel22", 2, "Signature", 0 },
                    { 3955, null, 3, "xrLabel23", 2, "Recipient:", 0 },
                    { 6143, null, 5, "tableCell7", 1, "Ladestelle:", 0 },
                    { 4021, null, 2, "tableCell26", 1, "Bestätigung:", 0 },
                    { 4032, null, 2, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 4024, null, 2, "tableCell23", 1, "Menge", 0 },
                    { 2173, null, 1, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 2180, null, 1, "tableCell21", 1, "Beleg-Einlösung / Ladungsträger-Ausgabe:", 0 },
                    { 2179, null, 1, "tableCell19", 1, "", 0 },
                    { 2178, null, 1, "tableCell18", 1, "Grund für die Belegerstellung:", 0 },
                    { 2177, null, 1, "tableCell16", 1, "Ladungsträger-Typ", 0 },
                    { 2176, null, 1, "tableCell15", 1, "Menge", 0 },
                    { 2175, null, 1, "tableCell13", 1, "Nicht getauschte Ladungsträger:", 0 },
                    { 2185, null, 1, "tableCell12", 1, "Dieser Beleg ist nicht an Dritte übertragbar.", 0 },
                    { 2187, null, 1, "tableCell11", 1, "Hinweis:", 0 },
                    { 2204, null, 1, "tableCell1", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 2203, null, 1, "label4", 1, "[Number]", 0 },
                    { 2181, null, 1, "label3", 1, "Unterschrift ([TruckDriver])", 0 },
                    { 2182, null, 1, "label2", 1, "Kennzeichen", 0 },
                    { 2184, null, 1, "label1", 1, "Datum", 0 },
                    { 4023, null, 2, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 2174, null, 1, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 2172, null, 1, "tableCell5", 1, "Empfänger:", 0 },
                    { 4013, null, 4, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 2170, null, 1, "tableCell9", 1, "DPL-Digitalcode:", 0 },
                    { 4025, null, 2, "tableCell21", 1, "Angenommene und ausgegebene Ladungsträger:", 0 },
                    { 2171, null, 1, "tableCell7", 1, "Aussteller:", 0 },
                    { 4036, null, 2, "tableCell2", 1, "Achtung Annahme-/Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 4035, null, 2, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 4034, null, 2, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 4011, null, 4, "xrLabel22", 4, "Podpis", 0 },
                    { 4031, null, 2, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 4033, null, 2, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 4020, null, 2, "label4", 1, "Kennzeichen", 0 },
                    { 4019, null, 2, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 4017, null, 2, "label1", 1, "Datum", 0 },
                    { 2188, null, 1, "xrLabel7", 1, "DPL-Pooling-Gutschrift-Nr.", 0 },
                    { 2183, null, 1, "xrLabel23", 1, "Empfänger:", 0 },
                    { 2186, null, 1, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 2205, null, 1, "xrLabel19", 1, "Aussteller:", 0 },
                    { 4037, null, 2, "tableCell1", 1, "Hinweis:", 0 }
                });

            migrationBuilder.InsertData(
                table: "LoadCarrierQualityMappings",
                columns: new[] { "Id", "LoadCarrierQualityId", "RefLmsQualityId" },
                values: new object[,]
                {
                    { 10, 19, 5 },
                    { 15, 29, 23 },
                    { 14, 28, 25 },
                    { 12, 19, 8 },
                    { 11, 19, 6 },
                    { 9, 12, 17 },
                    { 13, 28, 24 },
                    { 7, 12, 1 },
                    { 1, 2, 2 },
                    { 8, 12, 10 },
                    { 3, 10, 15 },
                    { 2, 10, 3 },
                    { 6, 11, 16 },
                    { 5, 11, 9 },
                    { 4, 11, 4 }
                });

            migrationBuilder.InsertData(
                table: "LoadCarriers",
                columns: new[] { "Id", "Name", "Order", "QualityId", "RefLmsQuality2PalletId", "RefLtmsPalletId", "TypeId" },
                values: new object[,]
                {
                    { 201, "EUR 1A", 1f, 10, 2003, (short)201, 8 },
                    { 200, "EUR U", 5f, -1, 2018, (short)200, 8 },
                    { 3, "EUR C", 3f, 32, 2001, (short)203, 8 },
                    { 2, "EUR B", 2f, 31, 2004, (short)202, 8 },
                    { 1, "EUR A", 1f, 30, 2003, (short)201, 8 },
                    { -701, "Einweg I", 0f, 1, 0, (short)-701, 7 },
                    { -512, "DplHaPa 2B", 0f, 12, 0, (short)-512, 5 },
                    { 112, "DD 2APlus", 10f, 29, 1023, (short)112, 3 },
                    { 111, "DD 2BG", 9f, 28, 1024, (short)111, 3 },
                    { 110, "DD Chep", 8f, 21, 0, (short)110, 3 },
                    { 108, "DD 2BRmp", 6f, 24, 1001, (short)108, 3 },
                    { 107, "DD 2BMT", 5f, 23, 1001, (short)107, 3 },
                    { 106, "DD Neu", 9f, 19, 1005, (short)106, 3 },
                    { 105, "DD I", 0f, 1, 1001, (short)105, 3 },
                    { 104, "DD D", 4f, 2, 1002, (short)104, 3 },
                    { 103, "DD 2B", 3f, 12, 1001, (short)103, 3 },
                    { 102, "DD 2A", 2f, 11, 1004, (short)102, 3 },
                    { 101, "DD 1A", 1f, 10, 1003, (short)101, 3 },
                    { 100, "DD U", -1f, -1, 1018, (short)100, 3 },
                    { -12, "U 2B", 0f, 12, 0, (short)-12, -1 },
                    { -1, "U U", 0f, -1, 0, (short)0, -1 },
                    { 109, "DD Schr", 7f, 20, 1028, (short)109, 3 },
                    { 202, "EUR 2A", 2f, 11, 2004, (short)202, 8 },
                    { 211, "EUR MPal", 9f, 22, 2001, (short)211, 8 },
                    { 204, "EUR D", 4f, 2, 2002, (short)204, 8 },
                    { 411, "H1 EW", 10f, 14, 0, (short)411, 18 },
                    { 450, "H1 DMiete I", 1f, 1, 0, (short)450, 19 },
                    { 502, "H1GD I", 1f, 1, 0, (short)502, 20 },
                    { 701, "E2KR Neu", 1f, 19, 10005, (short)701, 4 },
                    { 702, "E2KR I", 2f, 1, 10026, (short)702, 4 },
                    { 703, "E2KR D", 3f, 2, 10002, (short)703, 4 },
                    { 601, "E2EP Neu", 1f, 19, 9005, (short)601, 6 },
                    { 410, "H1 Schr", 9f, 20, 5002, (short)410, 18 },
                    { 602, "E2EP I", 2f, 1, 9026, (short)602, 6 },
                    { 901, "E1EP Neu", 1f, 19, 11005, (short)901, 9 },
                    { 902, "E1EP I", 2f, 1, 11026, (short)902, 9 },
                    { 903, "E1EP D", 3f, 2, 11002, (short)903, 9 },
                    { 801, "E1KR Neu", 1f, 19, 12005, (short)801, 10 },
                    { 802, "E1KR I", 2f, 1, 12026, (short)802, 10 },
                    { 803, "E1KR D", 3f, 2, 12002, (short)803, 10 },
                    { 151, "EPAL7 Neu", 0f, 19, 0, (short)151, 21 },
                    { 603, "E2EP D", 3f, 2, 9002, (short)603, 6 },
                    { 203, "EUR 2B", 3f, 12, 2001, (short)203, 8 },
                    { 409, "H1 ODoK", 8f, 27, 5011, (short)409, 18 },
                    { 407, "H1 D", 2f, 2, 5002, (short)407, 18 },
                    { 205, "EUR I", 0f, 1, 2001, (short)205, 8 },
                    { 206, "EUR Neu", 10f, 19, 2005, (short)206, 8 },
                    { 208, "EUR Schr", 6f, 20, 2028, (short)208, 8 },
                    { 209, "EUR EW", 7f, 14, 0, (short)209, 8 },
                    { 210, "EUR Chep", 8f, 21, 0, (short)210, 8 },
                    { -813, "EUR 2+", 0f, 13, 0, (short)-813, 8 },
                    { 300, "GB U", 1f, -1, 6011, (short)300, 13 },
                    { 408, "H1 ODmK", 7f, 26, 5011, (short)408, 18 },
                    { 301, "GB 1A", 0f, 10, 6005, (short)301, 13 },
                    { 303, "GB D", 2f, 2, 6002, (short)303, 13 },
                    { 305, "GB EigV", 4f, 25, 6011, (short)305, 13 },
                    { 400, "H1 U", 6f, -1, 5012, (short)400, 18 },
                    { 401, "H1 neu", 0f, 10, 5005, (short)401, 18 },
                    { 403, "H1 mK", 3f, 15, 5011, (short)403, 18 },
                    { 404, "H1 oK", 4f, 16, 5011, (short)404, 18 },
                    { 405, "H1 sauber", 5f, 17, 5011, (short)405, 18 },
                    { 406, "H1 dreckig", 6f, 18, 5012, (short)406, 18 },
                    { 302, "GB I", 1f, 1, 6011, (short)302, 13 },
                    { 402, "H1 I", 1f, 1, 5011, (short)402, 18 }
                });

            migrationBuilder.InsertData(
                table: "PublicHolidays",
                columns: new[] { "Id", "CountryId", "CountryStateId", "Date", "Name" },
                values: new object[,]
                {
                    { 2, 1, null, new DateTime(2020, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tag der Deutschen Einheit" },
                    { 1, 1, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Neujahr" }
                });

            migrationBuilder.InsertData(
                table: "PublicHolidays",
                columns: new[] { "Id", "CountryId", "CountryStateId", "Date", "Name" },
                values: new object[] { 3, null, 2, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Allerheiligen" });

            migrationBuilder.InsertData(
                table: "PublicHolidays",
                columns: new[] { "Id", "CountryId", "CountryStateId", "Date", "Name" },
                values: new object[] { 4, null, 10, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Allerheiligen" });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_ChangedById",
                table: "AdditionalFields",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_CreatedById",
                table: "AdditionalFields",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_DeletedById",
                table: "AdditionalFields",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFieldValues_AdditionalFieldId",
                table: "AdditionalFieldValues",
                column: "AdditionalFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ChangedById",
                table: "Addresses",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CreatedById",
                table: "Addresses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DeletedById",
                table: "Addresses",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PartnerId",
                table: "Addresses",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_RefLmsAddressNumber",
                table: "Addresses",
                column: "RefLmsAddressNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StateId",
                table: "Addresses",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseLoadCarrierMapping_LoadCarrierTypeId",
                table: "BaseLoadCarrierMapping",
                column: "LoadCarrierTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHourExceptions_ChangedById",
                table: "BusinessHourExceptions",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHourExceptions_CreatedById",
                table: "BusinessHourExceptions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHourExceptions_DeletedById",
                table: "BusinessHourExceptions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHourExceptions_LoadingLocationId",
                table: "BusinessHourExceptions",
                column: "LoadingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHours_ChangedById",
                table: "BusinessHours",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHours_CreatedById",
                table: "BusinessHours",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHours_DeletedById",
                table: "BusinessHours",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHours_LoadingLocationId",
                table: "BusinessHours",
                column: "LoadingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedBalancePositions_CalculatedBalanceId",
                table: "CalculatedBalancePositions",
                column: "CalculatedBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedBalancePositions_LoadCarrierId",
                table: "CalculatedBalancePositions",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedBalances_PostingAccountId",
                table: "CalculatedBalances",
                column: "PostingAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryStates_CountryId",
                table: "CountryStates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisionDocumentSettings_ChangedById",
                table: "CustomerDivisionDocumentSettings",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisionDocumentSettings_CreatedById",
                table: "CustomerDivisionDocumentSettings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisionDocumentSettings_DeletedById",
                table: "CustomerDivisionDocumentSettings",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisionDocumentSettings_DivisionId",
                table: "CustomerDivisionDocumentSettings",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisionDocumentSettings_DocumentNumberSequenceId",
                table: "CustomerDivisionDocumentSettings",
                column: "DocumentNumberSequenceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisionDocumentSettings_DocumentTypeId",
                table: "CustomerDivisionDocumentSettings",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_ChangedById",
                table: "CustomerDivisions",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_CreatedById",
                table: "CustomerDivisions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_CustomerId",
                table: "CustomerDivisions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_DefaultLoadingLocationId",
                table: "CustomerDivisions",
                column: "DefaultLoadingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_DeletedById",
                table: "CustomerDivisions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_PostingAccountId",
                table: "CustomerDivisions",
                column: "PostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDocumentSettings_ChangedById",
                table: "CustomerDocumentSettings",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDocumentSettings_CreatedById",
                table: "CustomerDocumentSettings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDocumentSettings_CustomerId",
                table: "CustomerDocumentSettings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDocumentSettings_DeletedById",
                table: "CustomerDocumentSettings",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDocumentSettings_DocumentTypeId",
                table: "CustomerDocumentSettings",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDocumentSettings_LoadCarrierTypeId",
                table: "CustomerDocumentSettings",
                column: "LoadCarrierTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIpSecurityRules_ChangedById",
                table: "CustomerIpSecurityRules",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIpSecurityRules_CreatedById",
                table: "CustomerIpSecurityRules",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIpSecurityRules_DeletedById",
                table: "CustomerIpSecurityRules",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIpSecurityRules_OrganizationId",
                table: "CustomerIpSecurityRules",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartnerDirectoryAccesses_ChangedById",
                table: "CustomerPartnerDirectoryAccesses",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartnerDirectoryAccesses_CreatedById",
                table: "CustomerPartnerDirectoryAccesses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartnerDirectoryAccesses_CustomerPartnerId",
                table: "CustomerPartnerDirectoryAccesses",
                column: "CustomerPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartnerDirectoryAccesses_DeletedById",
                table: "CustomerPartnerDirectoryAccesses",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartnerDirectoryAccesses_DirectoryId",
                table: "CustomerPartnerDirectoryAccesses",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartners_AddressId",
                table: "CustomerPartners",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartners_ChangedById",
                table: "CustomerPartners",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartners_CreatedById",
                table: "CustomerPartners",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartners_DeletedById",
                table: "CustomerPartners",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartners_PartnerId",
                table: "CustomerPartners",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressId",
                table: "Customers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ChangedById",
                table: "Customers",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedById",
                table: "Customers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_DeletedById",
                table: "Customers",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_OrganizationId",
                table: "Customers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ParentId",
                table: "Customers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PartnerId",
                table: "Customers",
                column: "PartnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSortingWorkers_CustomerId",
                table: "CustomerSortingWorkers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSortingWorkers_SortingWorkerId",
                table: "CustomerSortingWorkers",
                column: "SortingWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryNotes_ChangedById",
                table: "DeliveryNotes",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryNotes_CreatedById",
                table: "DeliveryNotes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryNotes_DeletedById",
                table: "DeliveryNotes",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryNotes_SubmissionId",
                table: "DeliveryNotes",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFile_DocumentId",
                table: "DocumentFile",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFile_FileId",
                table: "DocumentFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentNumberSequences_ChangedById",
                table: "DocumentNumberSequences",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentNumberSequences_CreatedById",
                table: "DocumentNumberSequences",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentNumberSequences_CustomerId",
                table: "DocumentNumberSequences",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentNumberSequences_DeletedById",
                table: "DocumentNumberSequences",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentReports_ChangedById",
                table: "DocumentReports",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentReports_CreatedById",
                table: "DocumentReports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentReports_DeletedById",
                table: "DocumentReports",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ChangedById",
                table: "Documents",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatedById",
                table: "Documents",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CustomerDivisionId",
                table: "Documents",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DeletedById",
                table: "Documents",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LanguageId",
                table: "Documents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LoadCarrierReceiptId",
                table: "Documents",
                column: "LoadCarrierReceiptId",
                unique: true,
                filter: "[LoadCarrierReceiptId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_OrderMatchId",
                table: "Documents",
                column: "OrderMatchId",
                unique: true,
                filter: "[OrderMatchId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StateId",
                table: "Documents",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TypeId",
                table: "Documents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_VoucherId",
                table: "Documents",
                column: "VoucherId",
                unique: true,
                filter: "[VoucherId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateLabel_DocumentTemplateId",
                table: "DocumentTemplateLabel",
                column: "DocumentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateLabel_LanguageId",
                table: "DocumentTemplateLabel",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_ChangedById",
                table: "DocumentTemplates",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_CreatedById",
                table: "DocumentTemplates",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_CustomerId",
                table: "DocumentTemplates",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplates_DeletedById",
                table: "DocumentTemplates",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressCodePartnerPresets_ExpressCodeId",
                table: "ExpressCodePartnerPresets",
                column: "ExpressCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressCodePartnerPresets_PartnerId",
                table: "ExpressCodePartnerPresets",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressCodes_ChangedById",
                table: "ExpressCodes",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressCodes_CreatedById",
                table: "ExpressCodes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressCodes_DeletedById",
                table: "ExpressCodes",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressCodes_IssuingCustomerId",
                table: "ExpressCodes",
                column: "IssuingCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressCodes_IssuingDivisionId",
                table: "ExpressCodes",
                column: "IssuingDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressCodes_PostingAccountId",
                table: "ExpressCodes",
                column: "PostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierCondition_LoadCarrierId",
                table: "LoadCarrierCondition",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierCondition_OrderConditionId",
                table: "LoadCarrierCondition",
                column: "OrderConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierQualities_RefLtmsQualityId",
                table: "LoadCarrierQualities",
                column: "RefLtmsQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierQualityMappings_LoadCarrierQualityId",
                table: "LoadCarrierQualityMappings",
                column: "LoadCarrierQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierQualityMappings_RefLmsQualityId",
                table: "LoadCarrierQualityMappings",
                column: "RefLmsQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptPosition_ChangedById",
                table: "LoadCarrierReceiptPosition",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptPosition_CreatedById",
                table: "LoadCarrierReceiptPosition",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptPosition_DeletedById",
                table: "LoadCarrierReceiptPosition",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptPosition_LoadCarrierId",
                table: "LoadCarrierReceiptPosition",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptPosition_LoadCarrierReceiptId",
                table: "LoadCarrierReceiptPosition",
                column: "LoadCarrierReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_ChangedById",
                table: "LoadCarrierReceipts",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_CreatedById",
                table: "LoadCarrierReceipts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_CustomerDivisionId",
                table: "LoadCarrierReceipts",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_DeletedById",
                table: "LoadCarrierReceipts",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_LicensePlateCountryId",
                table: "LoadCarrierReceipts",
                column: "LicensePlateCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_OrderLoadDetailId",
                table: "LoadCarrierReceipts",
                column: "OrderLoadDetailId",
                unique: true,
                filter: "[OrderLoadDetailId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_PostingAccountId",
                table: "LoadCarrierReceipts",
                column: "PostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_ShipperAddressId",
                table: "LoadCarrierReceipts",
                column: "ShipperAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_TargetPostingAccountId",
                table: "LoadCarrierReceipts",
                column: "TargetPostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarriers_QualityId",
                table: "LoadCarriers",
                column: "QualityId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarriers_TypeId",
                table: "LoadCarriers",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierTypes_RefLmsLoadCarrierTypeId",
                table: "LoadCarrierTypes",
                column: "RefLmsLoadCarrierTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierTypes_RefLtmsArticleId",
                table: "LoadCarrierTypes",
                column: "RefLtmsArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingLocations_AddressId",
                table: "LoadingLocations",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingLocations_ChangedById",
                table: "LoadingLocations",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingLocations_CreatedById",
                table: "LoadingLocations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingLocations_CustomerDivisionId",
                table: "LoadingLocations",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingLocations_CustomerPartnerId",
                table: "LoadingLocations",
                column: "CustomerPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingLocations_DeletedById",
                table: "LoadingLocations",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingLocations_PartnerId",
                table: "LoadingLocations",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationTexts_LocalizationItemId",
                table: "LocalizationTexts",
                column: "LocalizationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationTexts_LocalizationLanguageId",
                table: "LocalizationTexts",
                column: "LocalizationLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConditions_ChangedById",
                table: "OrderConditions",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConditions_CreatedById",
                table: "OrderConditions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConditions_DeletedById",
                table: "OrderConditions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConditions_PostingAccountId",
                table: "OrderConditions",
                column: "PostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderGroups_ChangedById",
                table: "OrderGroups",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderGroups_CreatedById",
                table: "OrderGroups",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderGroups_DeletedById",
                table: "OrderGroups",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLoad_ChangedById",
                table: "OrderLoad",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLoad_CreatedById",
                table: "OrderLoad",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLoad_DeletedById",
                table: "OrderLoad",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLoad_DetailId",
                table: "OrderLoad",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLoad_OrderId",
                table: "OrderLoad",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLoadDetail_AddressId",
                table: "OrderLoadDetail",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_BaseLoadCarrierId",
                table: "OrderMatches",
                column: "BaseLoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_ChangedById",
                table: "OrderMatches",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_CreatedById",
                table: "OrderMatches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_DeletedById",
                table: "OrderMatches",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_DemandId",
                table: "OrderMatches",
                column: "DemandId",
                unique: true,
                filter: "[DemandId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_DigitalCode",
                table: "OrderMatches",
                column: "DigitalCode",
                unique: true,
                filter: "[DigitalCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_LoadCarrierId",
                table: "OrderMatches",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_SupplyId",
                table: "OrderMatches",
                column: "SupplyId",
                unique: true,
                filter: "[SupplyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BaseLoadCarrierId",
                table: "Orders",
                column: "BaseLoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ChangedById",
                table: "Orders",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedById",
                table: "Orders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeletedById",
                table: "Orders",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GroupId",
                table: "Orders",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LoadCarrierId",
                table: "Orders",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LoadingLocationId",
                table: "Orders",
                column: "LoadingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderSerieId",
                table: "Orders",
                column: "OrderSerieId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PostingAccountId",
                table: "Orders",
                column: "PostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSeries_ChangedById",
                table: "OrderSeries",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSeries_CreatedById",
                table: "OrderSeries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSeries_DeletedById",
                table: "OrderSeries",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSyncError_MessageId",
                table: "OrderSyncError",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSyncError_OrderId",
                table: "OrderSyncError",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPartnerDirectories_OrganizationId",
                table: "OrganizationPartnerDirectories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPartnerDirectories_PartnerDirectoryId",
                table: "OrganizationPartnerDirectories",
                column: "PartnerDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AddressId",
                table: "Organizations",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ChangedById",
                table: "Organizations",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CreatedById",
                table: "Organizations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_DeletedById",
                table: "Organizations",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectories_ChangedById",
                table: "PartnerDirectories",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectories_CreatedById",
                table: "PartnerDirectories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectories_DeletedById",
                table: "PartnerDirectories",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectoryAccesses_ChangedById",
                table: "PartnerDirectoryAccesses",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectoryAccesses_CreatedById",
                table: "PartnerDirectoryAccesses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectoryAccesses_DeletedById",
                table: "PartnerDirectoryAccesses",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectoryAccesses_DirectoryId1",
                table: "PartnerDirectoryAccesses",
                column: "DirectoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectoryAccesses_PartnerId",
                table: "PartnerDirectoryAccesses",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_ChangedById",
                table: "Partners",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_CreatedById",
                table: "Partners",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_DefaultAddressId",
                table: "Partners",
                column: "DefaultAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_DefaultPostingAccountId",
                table: "Partners",
                column: "DefaultPostingAccountId",
                unique: true,
                filter: "[DefaultPostingAccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_DeletedById",
                table: "Partners",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ChangedById",
                table: "Permissions",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreatedById",
                table: "Permissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_DeletedById",
                table: "Permissions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserGroupId",
                table: "Permissions",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_AddressId",
                table: "Persons",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ChangedById",
                table: "Persons",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CreatedById",
                table: "Persons",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_DeletedById",
                table: "Persons",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccountPresets_DestinationAccountId",
                table: "PostingAccountPresets",
                column: "DestinationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccountPresets_ExpressCodeId",
                table: "PostingAccountPresets",
                column: "ExpressCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_AddressId",
                table: "PostingAccounts",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_ChangedById",
                table: "PostingAccounts",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_CreatedById",
                table: "PostingAccounts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_DeletedById",
                table: "PostingAccounts",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_ParentId",
                table: "PostingAccounts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_PartnerId1",
                table: "PostingAccounts",
                column: "PartnerId1");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_RefLtmsAccountCustomerNumber",
                table: "PostingAccounts",
                column: "RefLtmsAccountCustomerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_ChangedById",
                table: "PostingRequests",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_CreatedById",
                table: "PostingRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_DeletedById",
                table: "PostingRequests",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_LoadCarrierId",
                table: "PostingRequests",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_LoadCarrierReceiptId",
                table: "PostingRequests",
                column: "LoadCarrierReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_OrderLoadDetailId",
                table: "PostingRequests",
                column: "OrderLoadDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_OrderMatchId",
                table: "PostingRequests",
                column: "OrderMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_PostingAccountId",
                table: "PostingRequests",
                column: "PostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_SubmissionId",
                table: "PostingRequests",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_VoucherId",
                table: "PostingRequests",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequestSyncError_MessageId",
                table: "PostingRequestSyncError",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicHolidays_CountryId",
                table: "PublicHolidays",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicHolidays_CountryStateId",
                table: "PublicHolidays",
                column: "CountryStateId");

            migrationBuilder.CreateIndex(
                name: "IX_SortingInterruptions_ChangedById",
                table: "SortingInterruptions",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingInterruptions_CreatedById",
                table: "SortingInterruptions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingInterruptions_DeletedById",
                table: "SortingInterruptions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogPositions_ChangedById",
                table: "SortingShiftLogPositions",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogPositions_CreatedById",
                table: "SortingShiftLogPositions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogPositions_DeletedById",
                table: "SortingShiftLogPositions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogPositions_LoadCarrierId",
                table: "SortingShiftLogPositions",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogPositions_SortingShiftLogId",
                table: "SortingShiftLogPositions",
                column: "SortingShiftLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogs_ChangedById",
                table: "SortingShiftLogs",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogs_CreatedById",
                table: "SortingShiftLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogs_CustomerId",
                table: "SortingShiftLogs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogs_DeletedById",
                table: "SortingShiftLogs",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogs_ResponsibleUserId",
                table: "SortingShiftLogs",
                column: "ResponsibleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SortingShiftLogs_SortingWorkerId",
                table: "SortingShiftLogs",
                column: "SortingWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_SortingWorkers_ChangedById",
                table: "SortingWorkers",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingWorkers_CreatedById",
                table: "SortingWorkers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SortingWorkers_DeletedById",
                table: "SortingWorkers",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ChangedById",
                table: "Submissions",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_CreatedById",
                table: "Submissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_DeletedById",
                table: "Submissions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_PostingAccountId",
                table: "Submissions",
                column: "PostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SubmitterProfileId",
                table: "Submissions",
                column: "SubmitterProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SubmitterUserId",
                table: "Submissions",
                column: "SubmitterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitterProfiles_ChangedById",
                table: "SubmitterProfiles",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitterProfiles_CreatedById",
                table: "SubmitterProfiles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubmitterProfiles_DeletedById",
                table: "SubmitterProfiles",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransportBids_ChangedById",
                table: "TransportBids",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransportBids_CreatedById",
                table: "TransportBids",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransportBids_DeletedById",
                table: "TransportBids",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransportBids_DivisionId",
                table: "TransportBids",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportBids_TransportId",
                table: "TransportBids",
                column: "TransportId");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_ChangedById",
                table: "Transports",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_CreatedById",
                table: "Transports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_DeletedById",
                table: "Transports",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_OrderMatchId",
                table: "Transports",
                column: "OrderMatchId",
                unique: true,
                filter: "[OrderMatchId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_WinningBidId1",
                table: "Transports",
                column: "WinningBidId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_CustomerDivisionId",
                table: "UserGroups",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_CustomerId",
                table: "UserGroups",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_OrganizationId",
                table: "UserGroups",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_ChangedById",
                table: "UserGroups",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_CreatedById",
                table: "UserGroups",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_DeletedById",
                table: "UserGroups",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChangedById",
                table: "Users",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerId",
                table: "Users",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedById",
                table: "Users",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonId",
                table: "Users",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_ChangedById",
                table: "UserSettings",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_CreatedById",
                table: "UserSettings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_DeletedById",
                table: "UserSettings",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroupRelations_ChangedById",
                table: "UserUserGroupRelations",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroupRelations_CreatedById",
                table: "UserUserGroupRelations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroupRelations_DeletedById",
                table: "UserUserGroupRelations",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroupRelations_UserGroupId",
                table: "UserUserGroupRelations",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroupRelations_UserId",
                table: "UserUserGroupRelations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherPosition_ChangedById",
                table: "VoucherPosition",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherPosition_CreatedById",
                table: "VoucherPosition",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherPosition_DeletedById",
                table: "VoucherPosition",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherPosition_LoadCarrierId",
                table: "VoucherPosition",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherPosition_VoucherId",
                table: "VoucherPosition",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ChangedById",
                table: "Vouchers",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_CreatedById",
                table: "Vouchers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_CustomerDivisionId",
                table: "Vouchers",
                column: "CustomerDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_DeletedById",
                table: "Vouchers",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ExpressCodeId",
                table: "Vouchers",
                column: "ExpressCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_LicensePlateCountryId",
                table: "Vouchers",
                column: "LicensePlateCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ReasonTypeId",
                table: "Vouchers",
                column: "ReasonTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ReceivingCustomerId",
                table: "Vouchers",
                column: "ReceivingCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ReceivingPostingAccountId",
                table: "Vouchers",
                column: "ReceivingPostingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_RecipientId",
                table: "Vouchers",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ShipperId",
                table: "Vouchers",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_SubShipperId",
                table: "Vouchers",
                column: "SubShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_SubmissionId",
                table: "Vouchers",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_SupplierId",
                table: "Vouchers",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFieldValues_AdditionalFields_AdditionalFieldId",
                table: "AdditionalFieldValues",
                column: "AdditionalFieldId",
                principalTable: "AdditionalFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPartners_Users_ChangedById",
                table: "CustomerPartners",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPartners_Users_CreatedById",
                table: "CustomerPartners",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPartners_Users_DeletedById",
                table: "CustomerPartners",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPartners_Partners_PartnerId",
                table: "CustomerPartners",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPartners_Addresses_AddressId",
                table: "CustomerPartners",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_ChangedById",
                table: "Customers",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_CreatedById",
                table: "Customers",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_DeletedById",
                table: "Customers",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Partners_PartnerId",
                table: "Customers",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Organizations_OrganizationId",
                table: "Customers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Addresses_AddressId",
                table: "Customers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_Users_ChangedById",
                table: "LoadCarrierReceipts",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_Users_CreatedById",
                table: "LoadCarrierReceipts",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_Users_DeletedById",
                table: "LoadCarrierReceipts",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_PostingAccounts_PostingAccountId",
                table: "LoadCarrierReceipts",
                column: "PostingAccountId",
                principalTable: "PostingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_PostingAccounts_TargetPostingAccountId",
                table: "LoadCarrierReceipts",
                column: "TargetPostingAccountId",
                principalTable: "PostingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_CustomerDivisions_CustomerDivisionId",
                table: "LoadCarrierReceipts",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_Addresses_ShipperAddressId",
                table: "LoadCarrierReceipts",
                column: "ShipperAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_OrderLoadDetail_OrderLoadDetailId",
                table: "LoadCarrierReceipts",
                column: "OrderLoadDetailId",
                principalTable: "OrderLoadDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingLocations_Users_ChangedById",
                table: "LoadingLocations",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingLocations_Users_CreatedById",
                table: "LoadingLocations",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingLocations_Users_DeletedById",
                table: "LoadingLocations",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingLocations_Partners_PartnerId",
                table: "LoadingLocations",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingLocations_CustomerDivisions_CustomerDivisionId",
                table: "LoadingLocations",
                column: "CustomerDivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingLocations_Addresses_AddressId",
                table: "LoadingLocations",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLoadDetail_Addresses_AddressId",
                table: "OrderLoadDetail",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_ChangedById",
                table: "Organizations",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_CreatedById",
                table: "Organizations",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_DeletedById",
                table: "Organizations",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Addresses_AddressId",
                table: "Organizations",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Users_ChangedById",
                table: "Partners",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Users_CreatedById",
                table: "Partners",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Users_DeletedById",
                table: "Partners",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_PostingAccounts_DefaultPostingAccountId",
                table: "Partners",
                column: "DefaultPostingAccountId",
                principalTable: "PostingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Addresses_DefaultAddressId",
                table: "Partners",
                column: "DefaultAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Users_ChangedById",
                table: "Persons",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Users_CreatedById",
                table: "Persons",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Users_DeletedById",
                table: "Persons",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Addresses_AddressId",
                table: "Persons",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportBids_Transports_TransportId",
                table: "TransportBids",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_ChangedById",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_CreatedById",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_DeletedById",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_Users_ChangedById",
                table: "CustomerDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_Users_CreatedById",
                table: "CustomerDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_Users_DeletedById",
                table: "CustomerDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPartners_Users_ChangedById",
                table: "CustomerPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPartners_Users_CreatedById",
                table: "CustomerPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPartners_Users_DeletedById",
                table: "CustomerPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_ChangedById",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_CreatedById",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_DeletedById",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadingLocations_Users_ChangedById",
                table: "LoadingLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadingLocations_Users_CreatedById",
                table: "LoadingLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadingLocations_Users_DeletedById",
                table: "LoadingLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderGroups_Users_ChangedById",
                table: "OrderGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderGroups_Users_CreatedById",
                table: "OrderGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderGroups_Users_DeletedById",
                table: "OrderGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLoad_Users_ChangedById",
                table: "OrderLoad");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLoad_Users_CreatedById",
                table: "OrderLoad");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLoad_Users_DeletedById",
                table: "OrderLoad");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMatches_Users_ChangedById",
                table: "OrderMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMatches_Users_CreatedById",
                table: "OrderMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMatches_Users_DeletedById",
                table: "OrderMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_ChangedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CreatedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_DeletedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderSeries_Users_ChangedById",
                table: "OrderSeries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderSeries_Users_CreatedById",
                table: "OrderSeries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderSeries_Users_DeletedById",
                table: "OrderSeries");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_ChangedById",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_CreatedById",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_DeletedById",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Users_ChangedById",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Users_CreatedById",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Users_DeletedById",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Users_ChangedById",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Users_CreatedById",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Users_DeletedById",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_PostingAccounts_Users_ChangedById",
                table: "PostingAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostingAccounts_Users_CreatedById",
                table: "PostingAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostingAccounts_Users_DeletedById",
                table: "PostingAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportBids_Users_ChangedById",
                table: "TransportBids");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportBids_Users_CreatedById",
                table: "TransportBids");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportBids_Users_DeletedById",
                table: "TransportBids");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Users_ChangedById",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Users_CreatedById",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Users_DeletedById",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryStates_Countries_CountryId",
                table: "CountryStates");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Partners_PartnerId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPartners_Partners_PartnerId",
                table: "CustomerPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Partners_PartnerId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadingLocations_Partners_PartnerId",
                table: "LoadingLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_PostingAccounts_Partners_PartnerId1",
                table: "PostingAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_CountryStates_StateId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMatches_LoadCarriers_BaseLoadCarrierId",
                table: "OrderMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMatches_LoadCarriers_LoadCarrierId",
                table: "OrderMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_LoadCarriers_BaseLoadCarrierId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_LoadCarriers_LoadCarrierId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_LoadingLocations_DefaultLoadingLocationId",
                table: "CustomerDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_LoadingLocations_LoadingLocationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDivisions_PostingAccounts_PostingAccountId",
                table: "CustomerDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PostingAccounts_PostingAccountId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportBids_CustomerDivisions_DivisionId",
                table: "TransportBids");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLoadDetail_Addresses_AddressId",
                table: "OrderLoadDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_OrderMatches_OrderMatchId",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportBids_Transports_TransportId",
                table: "TransportBids");

            migrationBuilder.DropTable(
                name: "AccountingRecordTypes");

            migrationBuilder.DropTable(
                name: "AdditionalFieldValues");

            migrationBuilder.DropTable(
                name: "BaseLoadCarrierMapping");

            migrationBuilder.DropTable(
                name: "BusinessHourExceptions");

            migrationBuilder.DropTable(
                name: "BusinessHours");

            migrationBuilder.DropTable(
                name: "CalculatedBalancePositions");

            migrationBuilder.DropTable(
                name: "CustomerDivisionDocumentSettings");

            migrationBuilder.DropTable(
                name: "CustomerDocumentSettings");

            migrationBuilder.DropTable(
                name: "CustomerIpSecurityRules");

            migrationBuilder.DropTable(
                name: "CustomerPartnerDirectoryAccesses");

            migrationBuilder.DropTable(
                name: "CustomerSortingWorkers");

            migrationBuilder.DropTable(
                name: "DeliveryNotes");

            migrationBuilder.DropTable(
                name: "DocumentFile");

            migrationBuilder.DropTable(
                name: "DocumentReports");

            migrationBuilder.DropTable(
                name: "DocumentTemplateLabel");

            migrationBuilder.DropTable(
                name: "ExpressCodePartnerPresets");

            migrationBuilder.DropTable(
                name: "LoadCarrierCondition");

            migrationBuilder.DropTable(
                name: "LoadCarrierQualityMappings");

            migrationBuilder.DropTable(
                name: "LoadCarrierReceiptPosition");

            migrationBuilder.DropTable(
                name: "LocalizationTexts");

            migrationBuilder.DropTable(
                name: "OrderSyncError");

            migrationBuilder.DropTable(
                name: "OrganizationPartnerDirectories");

            migrationBuilder.DropTable(
                name: "PartnerDirectoryAccesses");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PostingAccountPresets");

            migrationBuilder.DropTable(
                name: "PostingRequests");

            migrationBuilder.DropTable(
                name: "PostingRequestSyncError");

            migrationBuilder.DropTable(
                name: "PublicHolidays");

            migrationBuilder.DropTable(
                name: "SortingInterruptions");

            migrationBuilder.DropTable(
                name: "SortingShiftLogPositions");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "UserUserGroupRelations");

            migrationBuilder.DropTable(
                name: "VoucherPosition");

            migrationBuilder.DropTable(
                name: "AdditionalFields");

            migrationBuilder.DropTable(
                name: "CalculatedBalances");

            migrationBuilder.DropTable(
                name: "DocumentNumberSequences");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "DocumentTemplates");

            migrationBuilder.DropTable(
                name: "OrderConditions");

            migrationBuilder.DropTable(
                name: "LocalizationItems");

            migrationBuilder.DropTable(
                name: "PartnerDirectories");

            migrationBuilder.DropTable(
                name: "SortingShiftLogs");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "LocalizationLanguages");

            migrationBuilder.DropTable(
                name: "LoadCarrierReceipts");

            migrationBuilder.DropTable(
                name: "DocumentStates");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "SortingWorkers");

            migrationBuilder.DropTable(
                name: "ExpressCodes");

            migrationBuilder.DropTable(
                name: "VoucherReasonTypes");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "SubmitterProfiles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "CountryStates");

            migrationBuilder.DropTable(
                name: "LoadCarriers");

            migrationBuilder.DropTable(
                name: "LoadCarrierQualities");

            migrationBuilder.DropTable(
                name: "LoadCarrierTypes");

            migrationBuilder.DropTable(
                name: "LoadingLocations");

            migrationBuilder.DropTable(
                name: "CustomerPartners");

            migrationBuilder.DropTable(
                name: "PostingAccounts");

            migrationBuilder.DropTable(
                name: "CustomerDivisions");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "OrderMatches");

            migrationBuilder.DropTable(
                name: "OrderLoad");

            migrationBuilder.DropTable(
                name: "OrderLoadDetail");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrderGroups");

            migrationBuilder.DropTable(
                name: "OrderSeries");

            migrationBuilder.DropTable(
                name: "Transports");

            migrationBuilder.DropTable(
                name: "TransportBids");
        }
    }
}
