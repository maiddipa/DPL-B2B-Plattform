using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Dal.Models;
using Dpl.B2b.Dal.Models.Lms;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Dpl.B2b.Dal
{
    public class OlmaDbContext : DbContext
    {
        private readonly IAuthorizationDataService _authData;

        public OlmaDbContext()
        {
        }

        public OlmaDbContext(DbContextOptions<OlmaDbContext> options,
            IServiceProvider serviceProvider)
            : base(options)
        {
            _authData = serviceProvider.GetService<IAuthorizationDataService>();
        }

        #region Dbsets

        public DbSet<Address> Addresses { get; set; }
        public DbSet<PostingAccount> PostingAccounts { get; set; }
        public DbSet<PostingRequest> PostingRequests { get; set; }
        public DbSet<AccountingRecordType> AccountingRecordTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryState> CountryStates { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerDivision> CustomerDivisions { get; set; }
        public DbSet<CustomerPartner> CustomerPartners { get; set; }
        public DbSet<ExpressCode> ExpressCodes { get; set; }
        public DbSet<PostingAccountPreset> PostingAccountPresets { get; set; }
        public DbSet<LoadCarrier> LoadCarriers { get; set; }
        public DbSet<LoadingLocation> LoadingLocations { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AdditionalField> AdditionalFields { get; set; }
        public DbSet<AdditionalFieldValue> AdditionalFieldValues { get; set; }
        public DbSet<BusinessHour> BusinessHours { get; set; }
        public DbSet<BusinessHourException> BusinessHourExceptions { get; set; }
        public DbSet<CalculatedBalance> CalculatedBalances { get; set; }
        public DbSet<CalculatedBalancePosition> CalculatedBalancePositions { get; set; }
        public DbSet<CustomerDivisionDocumentSetting> CustomerDivisionDocumentSettings { get; set; }
        public DbSet<CustomerDocumentSetting> CustomerDocumentSettings { get; set; }
        public DbSet<CustomerIpSecurityRule> CustomerIpSecurityRules { get; set; }
        public DbSet<OrganizationPartnerDirectory> OrganizationPartnerDirectories { get; set; }
        public DbSet<CustomerPartnerDirectoryAccess> CustomerPartnerDirectoryAccesses { get; set; }
        public DbSet<PartnerPreset> PartnerPresets { get; set; }
        public DbSet<CustomerSortingWorker> CustomerSortingWorkers { get; set; }
        public DbSet<DeliveryNote> DeliveryNotes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentNumberSequence> DocumentNumberSequences { get; set; }
        public DbSet<DocumentReport> DocumentReports { get; set; }
        public DbSet<DocumentState> DocumentStates { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<LoadCarrierQuality> LoadCarrierQualities { get; set; }
        public DbSet<LoadCarrierQualityMapping> LoadCarrierQualityMappings { get; set; }
        public DbSet<LoadCarrierReceipt> LoadCarrierReceipts { get; set; }
        public DbSet<LoadCarrierSorting> LoadCarrierSortings { get; set; }
        public DbSet<LoadCarrierType> LoadCarrierTypes { get; set; }
        public DbSet<LocalizationItem> LocalizationItems { get; set; }
        public DbSet<LocalizationLanguage> LocalizationLanguages { get; set; }
        public DbSet<LocalizationText> LocalizationTexts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCondition> OrderConditions { get; set; }
        public DbSet<OrderMatch> OrderMatches { get; set; }
        public DbSet<OrderSerie> OrderSeries { get; set; }
        public DbSet<OrderGroup> OrderGroups { get; set; }
        public DbSet<PartnerDirectory> PartnerDirectories { get; set; }
        public DbSet<PartnerDirectoryAccess> PartnerDirectoryAccesses { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PublicHoliday> PublicHolidays { get; set; }
        public DbSet<SortingInterruption> SortingInterruptions { get; set; }
        public DbSet<SortingShiftLog> SortingShiftLogs { get; set; }
        public DbSet<SortingShiftLogPosition> SortingShiftLogPositions { get; set; }
        public DbSet<SortingWorker> SortingWorkers { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<SubmitterProfile> SubmitterProfiles { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<TransportBid> TransportBids { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserUserGroupRelation> UserUserGroupRelations { get; set; }
        public DbSet<UserCustomerRelation> UserCustomerRelations { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherReasonType> VoucherReasonTypes { get; set; }

        public DbSet<LmsOrder> LmsOrderGroups { get; set; }

        #region LMS

        public virtual DbSet<LmsAvailability> LMS_Availability { get; set; }
        public virtual DbSet<LmsDelivery> LMS_Delivery { get; set; }
        public virtual DbSet<LmsAvail2deli> LMS_Avail2deli { get; set; }

        #endregion

        #region LTMS

        public virtual DbSet<LtmsAccoutingRecord> LtmsAccoutingRecords { get; set; }
        public virtual DbSet<Ltms.Accounts> LtmsAccounts { get; set; }
        public virtual DbSet<Ltms.Bookings> LtmsBookings { get; set; }
        public virtual DbSet<Ltms.Pallet> LtmsPallets { get; set; }
        public virtual DbSet<Ltms.Transactions> LtmsTransactions { get; set; }

        #endregion

        #endregion

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // identify IAuditable entities with changes
            var entries = this.ChangeTracker
                .Entries<OlmaAuditable>()
                .Where(e => e.Entity != null && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified || e.State == EntityState.Deleted))
                .ToArray();

            if (entries.Length > 0)
            {
                int? userId = _authData.GetUserId();
                // userId = 0 is the system, in which case the value should be 0 as there is no user with id 0
                userId = userId != 0 ? userId : null;
                var now = DateTime.UtcNow;
                foreach (var entityEntry in entries)
                {
                    entityEntry.Entity.ChangedById = userId;
                    entityEntry.Entity.ChangedAt = now;

                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Entity.CreatedById = userId;
                        entityEntry.Entity.CreatedAt = now;
                    }

                    if (entityEntry.State == EntityState.Deleted)
                    {
                        // auditable entities are never deleted, instead IsDeleted flag is set
                        entityEntry.State = EntityState.Modified;
                        entityEntry.Entity.IsDeleted = true;

                        entityEntry.Entity.DeletedById = userId;
                        entityEntry.Entity.DeletedAt = now;
                    }
                }
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Disable cascading deletes

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            #endregion

            #region Add query filters to removed soft deleted entries

            var olmaAuditableType = typeof(OlmaAuditable);
            var auditableEntityTypes = modelBuilder.Model
                .GetEntityTypes()
                .Where(i => i.ClrType.IsSubclassOf(olmaAuditableType))
                // this is important as query filters only need to be supplied on the root entity of a hierachie
                .Select(i => i.GetRootType())
                .Distinct()
                .Cast<IConventionEntityType>()
                .ToList();

            // in essence we need to call HasQueryFilter on each entity type with isDeleted == false
            // since this method is a generic method we need to use reflection to call it since we have a list of types
            foreach (var entityType in auditableEntityTypes)
            {
                // the below describes what the entire content of the loop does in simple terms
                // entityType.Builder.HasQueryFilter(entity => ((LamaAuditable)entity).IsDeleted == false) <= this is what we actually want todo
                // entityType.Builder.HasQueryFilter(entity => EF.Property<bool>(entity, nameof(LamaAuditable.IsDeleted)) == false) <= this is what we are constructing via reflection (same meaning as above)

                var parameter = Expression.Parameter(entityType.ClrType);

                // EF.Property<bool>(auditable, "IsDeleted")
                var propertyMethodInfo = typeof(EF).GetMethod(nameof(EF.Property)).MakeGenericMethod(typeof(bool));

                // nameof(LamaAuditable.IsDeleted)
                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant(nameof(OlmaAuditable.IsDeleted)));

                // EF.Property<bool>(auditable, "IsDeleted") == false
                BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

                // post => EF.Property<bool>(auditable, "IsDeleted") == false
                var lambda = Expression.Lambda(compareExpression, parameter);

                entityType.Builder.HasQueryFilter(lambda);
            }

            #endregion

            #region Add security query filters

            AddQueryFilterSecurity<Customer>(modelBuilder, i => _authData.GetCustomerIds().Contains(i.Id));

            AddQueryFilterSecurity<CustomerDivision>(modelBuilder, i => _authData.GetDivisionIds().Contains(i.Id));

            AddQueryFilterSecurity<CustomerPartner>(modelBuilder, i => i.DirectoryAccesses.Any(a =>
                    a.Directory.OrganizationPartnerDirectories.Any(o => o.OrganizationId == _authData.GetOrganizationId())));

            // TODO discuss if I remembered correctly (niko) and that CustomerPartners are only attached to the root customer
            // ToDo (Amine): Die Organisation ist dafür besser geeignet.
            AddQueryFilterSecurity<PartnerDirectory>(modelBuilder, i => i.OrganizationPartnerDirectories.Any(c => c.OrganizationId == _authData.GetOrganizationId()));

            // HACK commented out the document query filter as it is not correct and needs to be changed
            //AddQueryFilterSecurity<Document>(modelBuilder, i => _authData.GetDivisionIds().Contains(i.CustomerDivisionId));

            AddQueryFilterSecurity<Order>(modelBuilder, i => _authData.GetPostingAccountIds().Contains(i.PostingAccountId));

            AddQueryFilterSecurity<OrderGroup>(modelBuilder, i => i.Orders.Any(o => _authData.GetPostingAccountIds().Contains(o.PostingAccountId)));
            AddQueryFilterSecurity<OrderLoad>(modelBuilder, i => _authData.GetPostingAccountIds().Contains(i.Order.PostingAccountId));

            AddQueryFilterSecurity<Organization>(modelBuilder, i => i.Id == _authData.GetOrganizationId());

            AddQueryFilterSecurity<PostingAccount>(modelBuilder, i => _authData.GetPostingAccountIds().Contains(i.Id));

            AddQueryFilterSecurity<PostingRequest>(modelBuilder, i => _authData.GetPostingAccountIds().Contains(i.PostingAccountId));

            AddQueryFilterSecurity<Voucher>(modelBuilder, i => _authData.GetDivisionIds().Contains(i.CustomerDivisionId)
                    || (i.ReceivingPostingAccountId.HasValue && _authData.GetPostingAccountIds().Contains(i.ReceivingPostingAccountId.Value))
                    || (i.ReceivingCustomerId.HasValue && _authData.GetCustomerIds().Contains(i.ReceivingCustomerId.Value))); ;

            AddQueryFilterSecurity<LoadCarrierReceipt>(modelBuilder, i => _authData.GetDivisionIds().Contains(i.CustomerDivisionId)
                    || (i.PostingAccountId.HasValue && _authData.GetPostingAccountIds().Contains(i.PostingAccountId.Value)));

            modelBuilder.Entity<User>()
                .AddQueryFilter(i => i.Id == _authData.GetUserId());

            #endregion

            #region Add Ltms query filters

            modelBuilder.Entity<Ltms.Bookings>().AddQueryFilter(i => i.DeleteTime == null);

            AddQueryFilterSecurity<Ltms.Bookings>(modelBuilder, i => _authData.GetPostingAccountIds().Contains(i.OlmaPostingAccount.Id));


            #endregion

            #region Entity Configuration

            #region Set string default length to 255

            // sets default string length to 255
            // TODO Replace with custom convetion when its available (like ef core 5)
            // https://github.com/dotnet/efcore/issues/214
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
#pragma warning disable EF1001 // Internal EF Core API usage.
                property.AsProperty().Builder
                    .HasMaxLength(255, ConfigurationSource.Convention);
#pragma warning restore EF1001 // Internal EF Core API usage.
            }

            #endregion

            modelBuilder.Entity<DocumentNumberSequence>(entity =>
            {
                entity.Property(p => p.Counter).IsConcurrencyToken();
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasIndex(i => i.RefLmsAddressNumber);
            });

            modelBuilder.Entity<BaseLoadCarrierMapping>(entity =>
            {
                entity.HasKey(i => new { i.LoadCarrierId, i.LoadCarrierTypeId });

                entity.HasOne(i => i.LoadCarrier)
                    .WithMany(i => i.BaseLoadCarrierMappings)
                    .HasForeignKey(u => u.LoadCarrierId);

                entity.HasOne(i => i.LoadCarrierType)
                    .WithMany(i => i.BaseLoadCarrierMappings)
                    .HasForeignKey(u => u.LoadCarrierTypeId);
            });

            modelBuilder.Entity<CustomDocumentLabel>(entity =>
            {
                entity.Property(i => i.Text).HasMaxLength(1000);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Settings)
                    .HasConversion(v => v.ToString(Newtonsoft.Json.Formatting.None), v => Newtonsoft.Json.Linq.JObject.Parse(v));

                entity.Property(e => e.RefErpCustomerNumberString)
                    .HasComputedColumnSql<string>("CONVERT(nvarchar(255), [RefErpCustomerNumber])")
                    .IsRequired(true);

                entity.HasIndex(i => i.Name);
                entity.HasIndex(i => i.RefErpCustomerNumber);
                entity.HasIndex(i => i.RefErpCustomerNumberString);

                entity.HasOne(a => a.CreatedBy)
                    .WithMany()
                    .HasForeignKey(u => u.CreatedById);

                entity.HasOne(a => a.ChangedBy)
                    .WithMany()
                    .HasForeignKey(u => u.ChangedById);

                entity.HasOne(a => a.DeletedBy)
                    .WithMany()
                    .HasForeignKey(u => u.DeletedById);
            });

            modelBuilder.Entity<CustomerDivision>(entity =>
            {
                entity.HasIndex(i => i.Name);
                entity.HasIndex(i => i.ShortName);

                entity.HasOne(i => i.DefaultLoadingLocation)
                    .WithMany()
                    .HasForeignKey(i => i.DefaultLoadingLocationId)
                    .IsRequired(false);
            });

            //ToDo: EF5 Remove after upgrade
            modelBuilder.Entity<CustomerLoadCarrierReceiptDepotPreset>(entity =>
            {
                entity.HasKey(i => new { i.CustomerId, i.LoadCarrierReceiptDepotPresetId });
            });

            modelBuilder.Entity<CustomerPartner>(entity =>
            {
                entity.HasIndex(i => i.CustomerReference)
                    .IsUnique();
            });

            modelBuilder.Entity<EmployeeNote>(entity =>
            {
                entity.HasOne(i => i.User)
                    .WithMany()
                    .HasForeignKey(i => i.UserId)
                    .IsRequired(true);
            });

            modelBuilder.Entity<ExpressCodeUsageCondition>(entity =>
                {
                    entity.HasKey(i => i.PostingAccountId);
                }

            );

            modelBuilder.Entity<LmsOrder>(entity =>
            {
                entity.ToView("LmsOrders_View", schema: "dbo");
            });

            modelBuilder.Entity<LoadCarrier>(entity =>
            {
                //ToDo: EF5
                //entity.Property<ICollection<LoadCarrierReceiptDepoPreset>>("LoadCarrierReceiptDepoPresets");
            });

            modelBuilder.Entity<LoadCarrierReceipt>(entity =>
            {
                entity.HasOne(a => a.Document)
                    .WithOne(a => a.LoadCarrierReceipt)
                    .HasForeignKey<LoadCarrierReceipt>(c => c.DocumentId);
            });

            //ToDo: EF5 Remove after upgrade
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>(entity =>
            {
                entity.HasKey(i => new { i.LoadCarrierReceiptDepotPresetId, i.LoadCarrierId });
            });

            //ToDo: EF5
            // modelBuilder.Entity<LoadCarrierReceiptDepotPreset>(entity =>
            // {
            //     entity.HasMany(a => a.LoadCarriers)
            //         .WithMany("LoadCarrierReceiptDepotPresets")
            //         .UsingEntity(i => i.ToTable("LoadCarrierReceiptDepotPresetLoadCarrier"));
            // });

            modelBuilder.Entity<LoadCarrierType>(entity =>
            {
                entity.HasIndex(i => i.RefLmsLoadCarrierTypeId);
                entity.HasIndex(i => i.RefLtmsArticleId);
            });

            modelBuilder.Entity<LoadCarrierQuality>(entity =>
            {
                entity.HasIndex(i => i.RefLtmsQualityId);
            });

            modelBuilder.Entity<LoadCarrierQualityMapping>(entity =>
            {
                entity.HasIndex(i => i.RefLmsQualityId);
            });

            modelBuilder.Entity<LoadingLocation>(entity =>
            {
                entity.HasOne(i => i.CustomerDivision)
                    .WithMany(i => i.LoadingLocations)
                    .HasForeignKey(i => i.CustomerDivisionId);
            });

            modelBuilder.Entity<LocalizationText>(entity =>
            {
                entity.Property(i => i.Text).HasMaxLength(1000);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasAlternateKey(i => i.RefLmsOrderRowGuid);
            });

            modelBuilder.Entity<OrderLoadDetail>(entity =>
            {
                entity.HasOne(a => a.LoadCarrierReceipt)
                   .WithOne(a => a.OrderLoadDetail)
                   .HasForeignKey<LoadCarrierReceipt>(u => u.OrderLoadDetailId);
            });

            modelBuilder.Entity<OrderMatch>(entity =>
            {
                entity.HasOne(a => a.Document)
                    .WithOne(a => a.OrderMatch)
                    .HasForeignKey<OrderMatch>(c => c.DocumentId);

                entity.HasOne(a => a.Supply)
                    .WithOne(a => a.SupplyOrderMatch)
                    .HasForeignKey<OrderMatch>(i => i.SupplyId);

                entity.HasOne(a => a.Demand)
                    .WithOne(a => a.DemandOrderMatch)
                    .HasForeignKey<OrderMatch>(i => i.DemandId);
            });

            modelBuilder.Entity<OrderSyncError>(entity =>
            {
                entity.HasIndex(ose => ose.MessageId);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasOne(a => a.CreatedBy)
                    .WithMany()
                    .HasForeignKey(u => u.CreatedById);

                entity.HasOne(a => a.ChangedBy)
                    .WithMany()
                    .HasForeignKey(u => u.ChangedById);

                entity.HasOne(a => a.DeletedBy)
                    .WithMany()
                    .HasForeignKey(u => u.DeletedById);
            });

            modelBuilder.Entity<PartnerPreset>(entity =>
            {
                entity.ToTable("ExpressCodePartnerPresets");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Permissions)
                    .HasForeignKey(u => u.UserId);

                entity.HasOne(a => a.CreatedBy)
                    .WithMany()
                    .HasForeignKey(u => u.CreatedById);

                entity.HasOne(a => a.ChangedBy)
                    .WithMany()
                    .HasForeignKey(u => u.ChangedById);

                entity.HasOne(a => a.DeletedBy)
                    .WithMany()
                    .HasForeignKey(u => u.DeletedById);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasOne(a => a.CreatedBy)
                    .WithMany()
                    .HasForeignKey(u => u.CreatedById);

                entity.HasOne(a => a.ChangedBy)
                    .WithMany()
                    .HasForeignKey(u => u.ChangedById);

                entity.HasOne(a => a.DeletedBy)
                    .WithMany()
                    .HasForeignKey(u => u.DeletedById);
            });

            modelBuilder.Entity<PostingAccount>(entity =>
            {
                entity.Property(e => e.RefLtmsAccountIdString)
                    .HasComputedColumnSql<string>("CONVERT(nvarchar(255), [RefLtmsAccountId])")
                    .IsRequired(true);

                // TODO SKIPVIEWRELATIONS, can be removed once fixed https://github.com/dotnet/efcore/issues/21165
                entity.HasAlternateKey(i => i.RefLtmsAccountId);

                entity.HasIndex(i => i.DisplayName);
                entity.HasIndex(i => i.RefLtmsAccountIdString);

                entity.HasOne(a => a.CalculatedBalance)
                    .WithOne(a => a.PostingAccount)
                    .HasForeignKey<CalculatedBalance>(c => c.PostingAccountId);

                entity.HasOne(a => a.Customer)
                    .WithMany(a => a.PostingAccounts)
                    .HasForeignKey(a => a.CustomerId);

                entity.HasOne(a => a.Partner)
                    .WithOne(a => a.DefaultPostingAccount)
                    .HasForeignKey<Partner>(c => c.DefaultPostingAccountId);
            });

            modelBuilder.Entity<PostingRequestSyncError>(entity =>
            {
                entity.HasIndex(ose => ose.MessageId);
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasOne(a => a.SubmitterUser)
                    .WithMany(a => a.Submissions)
                    .HasForeignKey(u => u.SubmitterUserId);

                entity.HasOne(a => a.CreatedBy)
                    .WithMany()
                    .HasForeignKey(u => u.CreatedById);

                entity.HasOne(a => a.ChangedBy)
                    .WithMany()
                    .HasForeignKey(u => u.ChangedById);

                entity.HasOne(a => a.DeletedBy)
                    .WithMany()
                    .HasForeignKey(u => u.DeletedById);
            });

            modelBuilder.Entity<Transport>(entity =>
            {
                entity.HasMany(a => a.Bids)
                    .WithOne(a => a.Transport)
                    .HasForeignKey(i => i.TransportId);

                entity.HasOne(a => a.OrderMatch)
                    .WithOne(a => a.Transport)
                    .HasForeignKey<Transport>(i => i.OrderMatchId);
            });

            modelBuilder.Entity<TransportBid>(entity =>
            {
                entity.Property(i => i.Price)
                    .HasColumnType("smallmoney");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(a => a.Person)
                    .WithOne()
                    .HasForeignKey<User>(u => u.PersonId);

                entity.HasOne(a => a.UserSettings)
                    .WithOne()
                    .HasForeignKey<UserSetting>(u => u.UserId);

                entity.HasOne(a => a.CreatedBy)
                    .WithMany()
                    .HasForeignKey(u => u.CreatedById);

                entity.HasOne(a => a.ChangedBy)
                    .WithMany()
                    .HasForeignKey(u => u.ChangedById);

                entity.HasOne(a => a.DeletedBy)
                    .WithMany()
                    .HasForeignKey(u => u.DeletedById);
                entity.HasOne(a => a.Organization)
                    .WithMany(o => o.Users)
                    .HasForeignKey(u => u.OrganizationId);
                entity.HasIndex(u => u.Upn).IsUnique();
            });

            modelBuilder.Entity<UserUserGroupRelation>(entity =>
            {
                entity.HasOne(a => a.User)
                    .WithMany(u => u.UserGroups)
                    .HasForeignKey(f => f.UserId);

                entity.HasOne(a => a.UserGroup)
                    .WithMany(u => u.Users)
                    .HasForeignKey(f => f.UserGroupId);
            });

            modelBuilder.Entity<UserCustomerRelation>(entity =>
            {
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Customers)
                    .HasForeignKey(f => f.UserId);

                entity.HasOne(a => a.Customer)
                    .WithMany(u => u.Users)
                    .HasForeignKey(f => f.CustomerId);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasDiscriminator(i => i.Type)
                    .HasValue<OrganizationUserGroup>(UserGroupType.Organization)
                    .HasValue<CustomerUserGroup>(UserGroupType.Customer)
                    .HasValue<CustomerDivisionUserGroup>(UserGroupType.CustomerDivision);
            });

            modelBuilder.Entity<UserSetting>(entity =>
            {
                entity.Property(e => e.Data)
                    .HasConversion(v => v.ToString(Newtonsoft.Json.Formatting.None), v => Newtonsoft.Json.Linq.JObject.Parse(v));

                entity.HasOne(a => a.CreatedBy)
                    .WithMany()
                    .HasForeignKey(u => u.CreatedById);

                entity.HasOne(a => a.ChangedBy)
                    .WithMany()
                    .HasForeignKey(u => u.ChangedById);

                entity.HasOne(a => a.DeletedBy)
                    .WithMany()
                    .HasForeignKey(u => u.DeletedById);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasOne(a => a.Document)
                    .WithOne(a => a.Voucher)
                    .HasForeignKey<Voucher>(c => c.DocumentId);
                entity.HasIndex(i => i.RowGuid);

            });

            #endregion

            #region Ltms + Lms Entitiy Configuration

            // set all ltms or lms entities to be views, meaning readonly + no schema generation
            var ltmsOrLmsEntities = modelBuilder.Model
                .GetEntityTypes()
                .Where(i => i.ClrType.Namespace.Contains(".Ltms") || i.ClrType.Namespace.Contains(".Lms"))
                .Distinct()
                .Cast<IConventionEntityType>()
                .ToList();

            foreach (var ltmsEntity in ltmsOrLmsEntities)
            {
                var schema = ltmsEntity.ClrType.Namespace.Contains(".Ltms") ? "LTMS" : "LMS";
                var tableName = ltmsEntity.GetTableName().Replace("Ltms", "").Replace("Lms", "");
                modelBuilder.Entity(ltmsEntity.Name).ToView(tableName, schema: schema);
            }

            modelBuilder.Entity<LmsAvailability>(entity =>
            {
                entity.HasKey(a => a.AvailabilityId);
            });

            modelBuilder.Entity<LmsDelivery>(entity =>
            {
                entity.HasKey(a => a.DiliveryId);
            });

            modelBuilder.Entity<LmsDeliverydetail>(entity =>
            {
                entity.HasKey(a => a.DeliveryDetailId);
            });

            modelBuilder.Entity<LmsAvail2deli>(entity =>
            {
                entity.HasKey(a => a.Avail2DeliId);
            });
            modelBuilder.Entity<LtmsAccoutingRecord>(entity =>
            {
                entity.ToView("DoNotUseDirectly");

                entity.HasOne(d => d.Booking)
                    .WithOne()
                    .HasForeignKey<LtmsAccoutingRecord>(d => d.BookingId);

                entity.HasOne(d => d.PostingRequest)
                    .WithOne()
                    .HasForeignKey<LtmsAccoutingRecord>(d => d.PostingRequestId);
            });

            modelBuilder.Entity<Ltms.Accounts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountNumber)
                    .IsUnicode(false);

                entity.Property(e => e.AccountTypeId)
                    .HasColumnName("AccountType_ID")
                    .IsUnicode(false);

                entity.Property(e => e.CreateUser)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerNumber)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceAccountId)
                    .HasColumnName("InvoiceAccount_ID");

                entity.Property(e => e.KeyAccountManagerId)
                    .HasColumnName("KeyAccountManager_ID")
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("Parent_ID");

                entity.Property(e => e.ResponsiblePersonId)
                    .HasColumnName("ResponsiblePerson_ID")
                    .IsUnicode(false);

                entity.Property(e => e.SalespersonId)
                    .HasColumnName("Salesperson_ID")
                    .IsUnicode(false);

                entity.Property(e => e.UpdateUser)
                    .IsUnicode(false);

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.Articles>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ltms.BookingDependent>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("smallmoney");

                entity.Property(e => e.ArticleId).HasColumnName("Article_ID");

                entity.Property(e => e.QualityId).HasColumnName("Quality_ID");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.BookingDependent)
                    .HasForeignKey<Ltms.BookingDependent>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.BookingTypes>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsUnicode(false);

                entity.Property(e => e.LtmsUserTask)
                    .IsUnicode(false);

                entity.Property(e => e.Name);
            });

            modelBuilder.Entity<Ltms.Bookings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountDirection)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime)
                    .HasConversion(ValueConverters.CestToUtcConverter);

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.ArticleId).HasColumnName("Article_ID");

                entity.Property(e => e.BookingDate).HasColumnType("date");

                entity.Property(e => e.BookingTypeId)
                    .HasColumnName("BookingType_ID")
                    .IsUnicode(false);

                entity.Property(e => e.CreateUser)
                    .IsUnicode(false);

                entity.Property(e => e.DeleteUser)
                    .IsUnicode(false);

                entity.Property(e => e.ExtDescription)
                    .IsUnicode(false);

                entity.Property(e => e.MatchedUser)
                    .IsUnicode(false);

                entity.Property(e => e.QualityId).HasColumnName("Quality_ID");

                entity.Property(e => e.ReferenceNumber)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.Property(e => e.UpdateUser)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ArticleId);

                entity.HasOne(d => d.BookingType)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BookingTypeId);

                entity.HasOne(d => d.Quality)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.QualityId);

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TransactionId);

#if (SKIPVIEWRELATIONS != true)
                entity.HasOne(d => d.OlmaPostingAccount)
                    .WithMany()
                    .HasPrincipalKey(d => d.RefLtmsAccountId)
                    .HasForeignKey(i => i.AccountId);
#endif
            });

            modelBuilder.Entity<Ltms.Conditions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.CreateUser)
                    .IsUnicode(false);

                entity.Property(e => e.TermId)
                    .HasColumnName("Term_ID")
                    .IsUnicode(false);

                entity.Property(e => e.UpdateUser)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Conditions)
                    .HasForeignKey(d => d.AccountId);

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.Conditions)
                    .HasForeignKey(d => d.TermId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.ExceptionalOppositeSideAccounts>(entity =>
            {
                entity.HasKey(e => new { e.ConditionId, e.AccountId });

                entity.Property(e => e.ConditionId).HasColumnName("Condition_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ExceptionalOppositeSideAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Condition)
                    .WithMany(p => p.ExceptionalOppositeSideAccounts)
                    .HasForeignKey(d => d.ConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.ExceptionalOppositeSideBookingTypes>(entity =>
            {
                entity.HasKey(e => new { e.ConditionId, e.BookingTypeId });

                entity.Property(e => e.ConditionId).HasColumnName("Condition_ID");

                entity.Property(e => e.BookingTypeId)
                    .HasColumnName("BookingType_ID")
                    .IsUnicode(false);

                entity.HasOne(d => d.BookingType)
                    .WithMany(p => p.ExceptionalOppositeSideBookingTypes)
                    .HasForeignKey(d => d.BookingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Condition)
                    .WithMany(p => p.ExceptionalOppositeSideBookingTypes)
                    .HasForeignKey(d => d.ConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.Fees>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.BookingId).HasColumnName("Booking_ID");

                entity.Property(e => e.ConditionId).HasColumnName("Condition_ID");

                entity.Property(e => e.CreateUser)
                    .IsUnicode(false);

                entity.Property(e => e.DeleteUser)
                    .IsUnicode(false);

                entity.Property(e => e.TermId)
                    .HasColumnName("Term_ID")
                    .IsUnicode(false);

                entity.Property(e => e.UpdateUser)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.AccountId);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Condition)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.ConditionId);

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.TermId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.OnlyValidForOppositeSideAccounts>(entity =>
            {
                entity.HasKey(e => new { e.ConditionId, e.AccountId });

                entity.Property(e => e.ConditionId).HasColumnName("Condition_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.OnlyValidForOppositeSideAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Condition)
                    .WithMany(p => p.OnlyValidForOppositeSideAccounts)
                    .HasForeignKey(d => d.ConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.OnlyValidForOppositeSideBookingTypes>(entity =>
            {
                entity.HasKey(e => new { e.ConditionId, e.BookingTypeId });

                entity.Property(e => e.ConditionId).HasColumnName("Condition_ID");

                entity.Property(e => e.BookingTypeId)
                    .HasColumnName("BookingType_ID")
                    .IsUnicode(false);

                entity.HasOne(d => d.BookingType)
                    .WithMany(p => p.OnlyValidForOppositeSideBookingTypes)
                    .HasForeignKey(d => d.BookingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Condition)
                    .WithMany(p => p.OnlyValidForOppositeSideBookingTypes)
                    .HasForeignKey(d => d.ConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.Pallet>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.ArticleId).HasColumnName("Article_ID");

                entity.Property(e => e.Description)
                    .IsUnicode(false);

                entity.Property(e => e.QualityId).HasColumnName("Quality_ID");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Pallet)
                    .HasForeignKey(d => d.ArticleId);

                entity.HasOne(d => d.Quality)
                    .WithMany(p => p.Pallet)
                    .HasForeignKey(d => d.QualityId);
            });

            modelBuilder.Entity<Ltms.ProcessStates>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ltms.ProcessTypes>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ltms.Processes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateUser)
                    .IsUnicode(false);

                entity.Property(e => e.DeleteUser)
                    .IsUnicode(false);

                entity.Property(e => e.ExtDescription)
                    .IsUnicode(false);

                entity.Property(e => e.IntDescription)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessStateId).HasColumnName("ProcessState_ID");

                entity.Property(e => e.ProcessTypeId).HasColumnName("ProcessType_ID");

                entity.Property(e => e.ReferenceNumber)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateUser)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProcessState)
                    .WithMany(p => p.Processes)
                    .HasForeignKey(d => d.ProcessStateId);

                entity.HasOne(d => d.ProcessType)
                    .WithMany(p => p.Processes)
                    .HasForeignKey(d => d.ProcessTypeId);
            });

            modelBuilder.Entity<Ltms.Qualities>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ltms.ReportBookings>(entity =>
            {
                entity.HasKey(e => new { e.ReportId, e.BookingId });

                entity.Property(e => e.ReportId).HasColumnName("Report_ID");

                entity.Property(e => e.BookingId).HasColumnName("Booking_ID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.ReportBookings)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportBookings)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.ReportTypes>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsUnicode(false);

                entity.Property(e => e.FileFormat)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ltms.Reports>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdditionalParams).HasColumnType("xml");

                entity.Property(e => e.CreateUser)
                    .IsUnicode(false);

                entity.Property(e => e.DeleteUser)
                    .IsUnicode(false);

                entity.Property(e => e.DocId).HasColumnName("DOC_ID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.GdocId).HasColumnName("gdoc_id");

                entity.Property(e => e.MatchedTime).HasColumnType("date");

                entity.Property(e => e.MatchedUser)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryAccountId).HasColumnName("PrimaryAccount_ID");


                entity.Property(e => e.ReportStateId)
                    .HasColumnName("ReportState_ID")
                    .IsUnicode(false);

                entity.Property(e => e.ReportTypeId)
                    .HasColumnName("ReportType_ID")
                    .IsUnicode(false);

                entity.Property(e => e.UpdateUser)
                    .IsUnicode(false);

                entity.HasOne(d => d.PrimaryAccount)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.PrimaryAccountId);

                entity.HasOne(d => d.ReportType)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ReportTypeId);
            });

            modelBuilder.Entity<Ltms.Swaps>(entity =>
            {
                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Swaps)
                    .HasForeignKey<Ltms.Swaps>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ltms.Terms>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .IsUnicode(false);

                entity.Property(e => e.BookingTypeId)
                    .HasColumnName("BookingType_ID")
                    .IsUnicode(false);

                entity.Property(e => e.ChargeWithId)
                    .HasColumnName("ChargeWith_ID")
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsUnicode(false);

                entity.Property(e => e.SwapAccountId).HasColumnName("SwapAccount_ID");

                entity.Property(e => e.SwapInBookingTypeId)
                    .HasColumnName("SwapInBookingType_ID")
                    .IsUnicode(false);

                entity.Property(e => e.SwapInPalletId).HasColumnName("SwapInPallet_ID");

                entity.Property(e => e.SwapOutBookingTypeId)
                    .HasColumnName("SwapOutBookingType_ID")
                    .IsUnicode(false);

                entity.Property(e => e.SwapOutPalletId).HasColumnName("SwapOutPallet_ID"); ;

                entity.HasOne(d => d.BookingType)
                    .WithMany(p => p.TermsBookingType)
                    .HasForeignKey(d => d.BookingTypeId);

                entity.HasOne(d => d.ChargeWith)
                    .WithMany(p => p.InverseChargeWith)
                    .HasForeignKey(d => d.ChargeWithId);

                entity.HasOne(d => d.SwapAccount)
                    .WithMany(p => p.Terms)
                    .HasForeignKey(d => d.SwapAccountId);

                entity.HasOne(d => d.SwapInBookingType)
                    .WithMany(p => p.TermsSwapInBookingType)
                    .HasForeignKey(d => d.SwapInBookingTypeId);

                entity.HasOne(d => d.SwapInPallet)
                    .WithMany(p => p.TermsSwapInPallet)
                    .HasForeignKey(d => d.SwapInPalletId);

                entity.HasOne(d => d.SwapOutBookingType)
                    .WithMany(p => p.TermsSwapOutBookingType)
                    .HasForeignKey(d => d.SwapOutBookingTypeId);

                entity.HasOne(d => d.SwapOutPallet)
                    .WithMany(p => p.TermsSwapOutPallet)
                    .HasForeignKey(d => d.SwapOutPalletId);
            });

            modelBuilder.Entity<Ltms.TransactionStates>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ltms.TransactionTypes>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ltms.Transactions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CancellationId).HasColumnName("Cancellation_ID");

                entity.Property(e => e.CreateUser)
                    .IsUnicode(false);

                entity.Property(e => e.DeleteUser)
                    .IsUnicode(false);

                entity.Property(e => e.ExtDescription)
                    .IsUnicode(false);

                entity.Property(e => e.IntDescription)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessId).HasColumnName("Process_ID");

                entity.Property(e => e.ReferenceNumber)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionStateId).HasColumnName("TransactionState_ID");

                entity.Property(e => e.TransactionTypeId)
                    .HasColumnName("TransactionType_ID")
                    .IsUnicode(false); ;

                entity.Property(e => e.UpdateUser)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cancellation)
                    .WithMany(p => p.InverseCancellation)
                    .HasForeignKey(d => d.CancellationId);

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ProcessId);

                entity.HasOne(d => d.TransactionState)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransactionStateId);

                entity.HasOne(d => d.TransactionType)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransactionTypeId);
            });

            #endregion

            #region Data Seed

            #region Countries / CountryStates

            AddCountries(modelBuilder);

            #endregion

            #region DocumentStates

            //TODO Discuss witch DocumentStates do we really need, this states are all take from OPMS
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 200, Name = "Ausgeglichen", IsCanceled = false, CanBeCanceled = false });
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 1, Name = "erstellt (mit Belegdruck)", IsCanceled = false, CanBeCanceled = true });
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 99, Name = "Verfallen", IsCanceled = false, CanBeCanceled = false });
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 5, Name = "gespeichert", IsCanceled = false, CanBeCanceled = false });
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 3, Name = "erstellt (beleglos)", IsCanceled = false, CanBeCanceled = false });
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 255, Name = "storniert", IsCanceled = true, CanBeCanceled = false });
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 4, Name = "gesendet", IsCanceled = false, CanBeCanceled = false });
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 2, Name = "gedruckt", IsCanceled = false, CanBeCanceled = true });
            modelBuilder.Entity<DocumentState>().HasData(
                new DocumentState() { Id = 254, Name = "erledigt", IsCanceled = false, CanBeCanceled = false });

            #endregion

            #region DocumentTypes

            //TODO Check witch DocumentTypes from OPMS are necessary
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType() { Id = 1, Type = DocumentTypeEnum.VoucherDirectReceipt, PrintType = PrintType.VoucherCommon, ShortName = "VDR", Name = "VoucherDirectReceipt", HasReport = true, BusinessDomain = BusinessDomain.Pooling, OriginalAvailableForMinutes = 0 });
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType() { Id = 2, Type = DocumentTypeEnum.VoucherDigital, PrintType = PrintType.VoucherCommon, ShortName = "VD", Name = "VoucherDigital", HasReport = true, BusinessDomain = BusinessDomain.Pooling, OriginalAvailableForMinutes = 0 });
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType() { Id = 3, Type = DocumentTypeEnum.VoucherOriginal, PrintType = PrintType.VoucherCommon, ShortName = "VO", Name = "VoucherOriginal", HasReport = true, BusinessDomain = BusinessDomain.Pooling, OriginalAvailableForMinutes = 0 });
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType() { Id = 4, Type = DocumentTypeEnum.LoadCarrierReceiptExchange, PrintType = PrintType.LoadCarrierReceiptExchange, ShortName = "LCRE", Name = "LoadCarrierReceiptExchange", HasReport = true, BusinessDomain = BusinessDomain.Pooling, OriginalAvailableForMinutes = 30 });
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType() { Id = 5, Type = DocumentTypeEnum.LoadCarrierReceiptPickup, PrintType = PrintType.LoadCarrierReceiptPickup, ShortName = "LCRP", Name = "LoadCarrierReceiptPickup", HasReport = true, BusinessDomain = BusinessDomain.Pooling, OriginalAvailableForMinutes = 30 });
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType() { Id = 6, Type = DocumentTypeEnum.LoadCarrierReceiptDelivery, PrintType = PrintType.LoadCarrierReceiptDelivery, ShortName = "LCRD", Name = "LoadCarrierReceiptDelivery", HasReport = true, BusinessDomain = BusinessDomain.Pooling, OriginalAvailableForMinutes = 30 });
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType() { Id = 7, Type = DocumentTypeEnum.TransportVoucher, PrintType = PrintType.TransportVoucher, ShortName = "TV", Name = "TransportVoucher", HasReport = true, BusinessDomain = BusinessDomain.Pooling, OriginalAvailableForMinutes = 0 });

            #endregion

            #region DocumentTemplates + Template Labels

            // VoucherCommon
            modelBuilder.Entity<DocumentTemplate>().HasData(
                new DocumentTemplate() { Id = 1, Customer = null, IsDefault = true, PrintType = PrintType.VoucherCommon, Version = 1 });
            // LoadCarrierReceiptExchange
            modelBuilder.Entity<DocumentTemplate>().HasData(
                new DocumentTemplate() { Id = 2, Customer = null, IsDefault = true, PrintType = PrintType.LoadCarrierReceiptExchange, Version = 1 });
            // LoadCarrierReceiptPickup
            modelBuilder.Entity<DocumentTemplate>().HasData(
                new DocumentTemplate() { Id = 3, Customer = null, IsDefault = true, PrintType = PrintType.LoadCarrierReceiptPickup, Version = 1 });
            // LoadCarrierReceiptDelivery
            modelBuilder.Entity<DocumentTemplate>().HasData(
                new DocumentTemplate() { Id = 4, Customer = null, IsDefault = true, PrintType = PrintType.LoadCarrierReceiptDelivery, Version = 1 });
            // TransportVoucher
            modelBuilder.Entity<DocumentTemplate>().HasData(
                new DocumentTemplate() { Id = 5, Customer = null, IsDefault = true, PrintType = PrintType.TransportVoucher, Version = 1 });

         #region Template Labels
         #region DB_Skript
         /*
          * DB Skript zum erzeugen derin C# Seed Einträge aus DB
          * 
         DECLARE @PRINT_TYPE_ID int;
         SET @PRINT_TYPE_ID = 3;
         DECLARE @DOCUMENT_TEMPLATE_ID int;
         SET @DOCUMENT_TEMPLATE_ID = 3;

         SELECT [Id]
               ,[CustomerId]
               ,[DocumentTemplateId]
               ,[LanguageId]
               ,[Label]
               ,[Text]
               ,[Version]
               ,'            modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = ' + CAST([Id] AS nvarchar(10)) + ', DocumentTemplateId = ' + CAST(@DOCUMENT_TEMPLATE_ID AS nvarchar(10)) + ', LanguageId = ' +  CAST([LanguageId] AS nvarchar(10)) + ', Label = "' + [Label] + '", Text = @"' + [Text] + '" });'
           FROM [dbo].[DocumentTemplateLabel]
           WHERE [DocumentTemplateId] = (SELECT MAX(ID) FROM [dbo].[DocumentTemplates] WHERE dbo.[DocumentTemplates].PrintType = @PRINT_TYPE_ID)
           ORDER BY Label, LanguageId
           */
         #endregion

         #region Labels PrintType 1 VoucherCommon
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10040, DocumentTemplateId = 1, LanguageId = 1, Label = "label1", Text = @"Datum (Zeit)" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10051, DocumentTemplateId = 1, LanguageId = 2, Label = "label1", Text = @"DPL-Code:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10045, DocumentTemplateId = 1, LanguageId = 3, Label = "label1", Text = @"DPL-Code:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10047, DocumentTemplateId = 1, LanguageId = 4, Label = "label1", Text = @"DPL-Code:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10038, DocumentTemplateId = 1, LanguageId = 1, Label = "label2", Text = @"Kennzeichen" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10037, DocumentTemplateId = 1, LanguageId = 1, Label = "label3", Text = @"Unterschrift ([TruckDriver])" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10059, DocumentTemplateId = 1, LanguageId = 1, Label = "label4", Text = @"[Number]" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10061, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell1", Text = @"Beleg-Typ / -Einlösung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10062, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell11", Text = @"Hinweis:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10044, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell12", Text = @"Dieser Beleg ist nicht an Dritte übertragbar." });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10031, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell13", Text = @"Nicht getauschte Ladungsträger:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10032, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell15", Text = @"Menge" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10033, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell16", Text = @"Ladungsträger-Typ" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10034, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell18", Text = @"Grund für die Belegerstellung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10035, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell19", Text = @"" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10036, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell21", Text = @"Beleg-Einlösung / Ladungsträger-Ausgabe:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10024, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell23", Text = @"Referenznummer (Ausst.):" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10026, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell24", Text = @"" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10025, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell26", Text = @"Beschaffungslogistik:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10029, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell27", Text = @"Kommentar:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10030, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell28", Text = @"[Comment]" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10027, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell3", Text = @"Kontakt zu DPL:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10028, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell4", Text = @"[DPLAddressAndContact]" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10041, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell5", Text = @"Empfänger:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10042, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell7", Text = @"Aussteller:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10043, DocumentTemplateId = 1, LanguageId = 1, Label = "tableCell9", Text = @"DPL-Digitalcode:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10064, DocumentTemplateId = 1, LanguageId = 1, Label = "xrLabel19", Text = @"Aussteller:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10058, DocumentTemplateId = 1, LanguageId = 2, Label = "xrLabel19", Text = @"Issuer:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10023, DocumentTemplateId = 1, LanguageId = 3, Label = "xrLabel19", Text = @"Emetteur : " });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10063, DocumentTemplateId = 1, LanguageId = 4, Label = "xrLabel19", Text = @"Wystawiajacy:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10053, DocumentTemplateId = 1, LanguageId = 1, Label = "xrLabel22", Text = @"Unterschrift ([IssuerName])" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10046, DocumentTemplateId = 1, LanguageId = 2, Label = "xrLabel22", Text = @"Signature" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10056, DocumentTemplateId = 1, LanguageId = 3, Label = "xrLabel22", Text = @"Signature" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10052, DocumentTemplateId = 1, LanguageId = 4, Label = "xrLabel22", Text = @"Podpis" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10039, DocumentTemplateId = 1, LanguageId = 1, Label = "xrLabel23", Text = @"Empfänger: (Name des Fahrers)" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10055, DocumentTemplateId = 1, LanguageId = 2, Label = "xrLabel23", Text = @"Recipient:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10050, DocumentTemplateId = 1, LanguageId = 3, Label = "xrLabel23", Text = @"Destinataire :" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10049, DocumentTemplateId = 1, LanguageId = 4, Label = "xrLabel23", Text = @"Odbiorca:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10060, DocumentTemplateId = 1, LanguageId = 1, Label = "xrLabel7", Text = @"DPL-Pooling-Gutschrift-Nr." });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10057, DocumentTemplateId = 1, LanguageId = 2, Label = "xrLabel7", Text = @"Palettenannahme-Beleg TODO" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10048, DocumentTemplateId = 1, LanguageId = 3, Label = "xrLabel7", Text = @"Palettenannahme-Quittung FR TODO" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 10054, DocumentTemplateId = 1, LanguageId = 4, Label = "xrLabel7", Text = @"Palettenannahme-Quittung PL TODO" });
         #endregion

         #region Labels PrintType 2 LoadCarrierReceiptExchange
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11067, DocumentTemplateId = 2, LanguageId = 1, Label = "label1", Text = @"Datum" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11069, DocumentTemplateId = 2, LanguageId = 1, Label = "label3", Text = @"Unterschrift ([TruckDriverName])" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11070, DocumentTemplateId = 2, LanguageId = 1, Label = "label4", Text = @"Kennzeichen" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11087, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell1", Text = @"Hinweis:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11081, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell11", Text = @"Sonstige-Nr.:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11082, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell13", Text = @"Abholschein-Nr.:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11083, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell15", Text = @"Lieferschein-Nr.:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11084, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell17", Text = @"DPL-Digitalcode:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11085, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell19", Text = @"Beleg-Typ / -Einlösung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11086, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell2", Text = @"Achtung Annahme-/Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11075, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell21", Text = @"Angenommene und ausgegebene Ladungsträger:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11074, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell23", Text = @"Menge" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11073, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell24", Text = @"Ladungsträger-Typ" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11071, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell26", Text = @"Bestätigung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11072, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell29", Text = @"Qualität" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11077, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell3", Text = @"Kontakt zu DPL:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11076, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell4", Text = @"[DPLAddressAndContact]" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11078, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell5", Text = @"Transport für Firma:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11079, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell7", Text = @"Firma auf LKW:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11080, DocumentTemplateId = 2, LanguageId = 1, Label = "tableCell9", Text = @"Aussteller-ID:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11097, DocumentTemplateId = 2, LanguageId = 1, Label = "xrLabel19", Text = @"Aussteller:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11092, DocumentTemplateId = 2, LanguageId = 2, Label = "xrLabel19", Text = @"Issuer:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11094, DocumentTemplateId = 2, LanguageId = 3, Label = "xrLabel19", Text = @"Emetteur : " });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11091, DocumentTemplateId = 2, LanguageId = 4, Label = "xrLabel19", Text = @"Wystawiajacy:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11096, DocumentTemplateId = 2, LanguageId = 1, Label = "xrLabel22", Text = @"Unterschrift ([IssuerName])" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11093, DocumentTemplateId = 2, LanguageId = 2, Label = "xrLabel22", Text = @"Signature" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11089, DocumentTemplateId = 2, LanguageId = 3, Label = "xrLabel22", Text = @"Signature" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11090, DocumentTemplateId = 2, LanguageId = 4, Label = "xrLabel22", Text = @"Podpis" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11068, DocumentTemplateId = 2, LanguageId = 1, Label = "xrLabel23", Text = @"Empfänger:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11066, DocumentTemplateId = 2, LanguageId = 2, Label = "xrLabel23", Text = @"Recipient:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11095, DocumentTemplateId = 2, LanguageId = 3, Label = "xrLabel23", Text = @"Destinataire :" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 11088, DocumentTemplateId = 2, LanguageId = 4, Label = "xrLabel23", Text = @"Odbiorca:" });
         #endregion

         #region Labels PrintType 3 LoadCarrierReceiptPickup
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12099, DocumentTemplateId = 3, LanguageId = 1, Label = "label1", Text = @"Datum (Zeit)" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12101, DocumentTemplateId = 3, LanguageId = 1, Label = "label3", Text = @"Unterschrift ([TruckDriverName])" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12102, DocumentTemplateId = 3, LanguageId = 1, Label = "label4", Text = @"Kennzeichen" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12119, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell1", Text = @"Hinweis:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12113, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell11", Text = @"Sonstige-Nr.:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12114, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell13", Text = @"Abholschein-Nr.:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12115, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell15", Text = @"Lieferschein-Nr.:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12116, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell17", Text = @"DPL-Digitalcode:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12117, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell19", Text = @"Beleg-Typ / -Einlösung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12118, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell2", Text = @"Achtung Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12107, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell21", Text = @"Ausgegebene Ladungsträger:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12106, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell23", Text = @"Menge" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12105, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell24", Text = @"Ladungsträger-Typ" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12103, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell26", Text = @"Bestätigung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12104, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell29", Text = @"Qualität" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12109, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell3", Text = @"Kontakt zu DPL:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12108, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell30", Text = @"Kommentar:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12110, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell5", Text = @"Abholung für Firma:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12111, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell7", Text = @"Firma auf abholendem LKW:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12112, DocumentTemplateId = 3, LanguageId = 1, Label = "tableCell9", Text = @"Aussteller-ID:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12129, DocumentTemplateId = 3, LanguageId = 1, Label = "xrLabel19", Text = @"Aussteller (Ausgabe-Quittung):" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12123, DocumentTemplateId = 3, LanguageId = 2, Label = "xrLabel19", Text = @"Issuer:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12121, DocumentTemplateId = 3, LanguageId = 3, Label = "xrLabel19", Text = @"Emetteur : " });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12124, DocumentTemplateId = 3, LanguageId = 4, Label = "xrLabel19", Text = @"Wystawiajacy:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12128, DocumentTemplateId = 3, LanguageId = 1, Label = "xrLabel22", Text = @"Unterschrift ([IssuerName])" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12122, DocumentTemplateId = 3, LanguageId = 2, Label = "xrLabel22", Text = @"Signature" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12126, DocumentTemplateId = 3, LanguageId = 3, Label = "xrLabel22", Text = @"Signature" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12125, DocumentTemplateId = 3, LanguageId = 4, Label = "xrLabel22", Text = @"Podpis" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12100, DocumentTemplateId = 3, LanguageId = 1, Label = "xrLabel23", Text = @"Empfänger: (Name des Fahrers)" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12098, DocumentTemplateId = 3, LanguageId = 2, Label = "xrLabel23", Text = @"Recipient:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12120, DocumentTemplateId = 3, LanguageId = 3, Label = "xrLabel23", Text = @"Destinataire :" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 12127, DocumentTemplateId = 3, LanguageId = 4, Label = "xrLabel23", Text = @"Odbiorca:" });
         #endregion

         #region Labels PrintType 4 LoadCarrierReceiptDelivery
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13131, DocumentTemplateId = 4, LanguageId = 1, Label = "label1", Text = @"Datum (Zeit)" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13133, DocumentTemplateId = 4, LanguageId = 1, Label = "label3", Text = @"Unterschrift ([TruckDriverName])" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13134, DocumentTemplateId = 4, LanguageId = 1, Label = "label4", Text = @"Kennzeichen" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13148, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell1", Text = @"Hinweis:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13136, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell12", Text = @"Qualität" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13145, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell17", Text = @"DPL-Digitalcode:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13146, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell19", Text = @"Beleg-Typ / -Einlösung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13147, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell2", Text = @"Achtung Annahmestelle!   Den quittierten Beleg unbedingt in Kopie aufbewahren!" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13139, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell21", Text = @"Angelieferte Ladungsträger:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13138, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell23", Text = @"Menge" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13137, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell24", Text = @"Ladungsträger-Typ" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13135, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell26", Text = @"Bestätigung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13141, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell3", Text = @"Kontakt zu DPL:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13140, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell4", Text = @"[DPLAddressAndContact]" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13142, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell5", Text = @"Anlieferung für Firma:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13144, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell7", Text = @"Firma auf anlieferndem LKW:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13157, DocumentTemplateId = 4, LanguageId = 1, Label = "tableCell9", Text = @"Aussteller-ID:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13158, DocumentTemplateId = 4, LanguageId = 1, Label = "xrLabel19", Text = @"Aussteller (Annahme-Quittung):" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13152, DocumentTemplateId = 4, LanguageId = 2, Label = "xrLabel19", Text = @"Issuer:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13150, DocumentTemplateId = 4, LanguageId = 3, Label = "xrLabel19", Text = @"Emetteur : " });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13153, DocumentTemplateId = 4, LanguageId = 4, Label = "xrLabel19", Text = @"Wystawiajacy:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13143, DocumentTemplateId = 4, LanguageId = 1, Label = "xrLabel22", Text = @"Unterschrift ([IssuerName])" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13151, DocumentTemplateId = 4, LanguageId = 2, Label = "xrLabel22", Text = @"Signature" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13155, DocumentTemplateId = 4, LanguageId = 3, Label = "xrLabel22", Text = @"Signature" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13154, DocumentTemplateId = 4, LanguageId = 4, Label = "xrLabel22", Text = @"Podpis" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13132, DocumentTemplateId = 4, LanguageId = 1, Label = "xrLabel23", Text = @"Empfänger (Annahme-Quittung):" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13130, DocumentTemplateId = 4, LanguageId = 2, Label = "xrLabel23", Text = @"Recipient:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13149, DocumentTemplateId = 4, LanguageId = 3, Label = "xrLabel23", Text = @"Destinataire :" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 13156, DocumentTemplateId = 4, LanguageId = 4, Label = "xrLabel23", Text = @"Odbiorca:" });
         #endregion


         #region Labels PrintType 5 TransportVoucher
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14159, DocumentTemplateId = 5, LanguageId = 1, Label = "label5", Text = @"Ladevorschriften:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14168, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell1", Text = @"" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14180, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell12", Text = @"Qualität" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14165, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell13", Text = @"" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14178, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell15", Text = @"Heckbeladung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14177, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell16", Text = @"" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14171, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell17", Text = @"DPL-Digitalcode:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14176, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell19", Text = @"Seitenbeladung:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14175, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell20", Text = @"" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14161, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell21", Text = @"Zu transportierende Ladungsträger:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14160, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell23", Text = @"Menge" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14179, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell24", Text = @"Ladungsträger-Typ" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14174, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell26", Text = @"Jumbo LKW:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14173, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell27", Text = @"" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14167, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell28", Text = @"Ladezeit:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14163, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell3", Text = @"Kontakt zu DPL:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14164, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell30", Text = @"Anlieferzeit:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14172, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell32", Text = @"Stapelhöhe:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14162, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell4", Text = @"[DPLAddressAndContact]" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14166, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell5", Text = @"Anlieferstelle:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14169, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell7", Text = @"Ladestelle:" });
         modelBuilder.Entity<DocumentTemplateLabel>().HasData(new DocumentTemplateLabel() { Id = 14170, DocumentTemplateId = 5, LanguageId = 1, Label = "tableCell9", Text = @"Aussteller-ID:" });
         #endregion

         #endregion

         #endregion

         #region LoadCarriers


         modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = -1, RefLtmsQualityId = 0, Name = "Undef.", Order = 0, Type = LoadCarrierQualityType.Intact }); //HACK: To Fix Problem with 0 as Primary Key
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 1, RefLtmsQualityId = 1, Name = "Intakt", Order = 1, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 2, RefLtmsQualityId = 2, Name = "Defekt", Order = 6, Type = LoadCarrierQualityType.Defect });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 10, RefLtmsQualityId = 10, Name = "1A", Order = 2, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 11, RefLtmsQualityId = 11, Name = "2A", Order = 4, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 12, RefLtmsQualityId = 12, Name = "2B", Order = 5, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 13, RefLtmsQualityId = 13, Name = "2+", Order = 3, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 14, RefLtmsQualityId = 14, Name = "Einweg", Order = 7, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 15, RefLtmsQualityId = 15, Name = "GD mK", Order = 8, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 16, RefLtmsQualityId = 16, Name = "GD oK", Order = 9, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 17, RefLtmsQualityId = 17, Name = "Gereinigt", Order = 10, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 18, RefLtmsQualityId = 18, Name = "Ungerein.", Order = 11, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 19, RefLtmsQualityId = 19, Name = "neu", Order = 1, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 20, RefLtmsQualityId = 20, Name = "Schrott", Order = 13, Type = LoadCarrierQualityType.Defect });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 21, RefLtmsQualityId = 21, Name = "Chep", Order = 14, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 22, RefLtmsQualityId = 22, Name = "MPal", Order = 15, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 23, RefLtmsQualityId = 23, Name = "2B MT", Order = 16, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 24, RefLtmsQualityId = 24, Name = "2B Rmp", Order = 17, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 25, RefLtmsQualityId = 25, Name = "EigVerw", Order = 18, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 26, RefLtmsQualityId = 26, Name = "OD mK", Order = 19, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 27, RefLtmsQualityId = 27, Name = "OD oK", Order = 20, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 28, RefLtmsQualityId = 28, Name = "2BG", Order = 5, Type = LoadCarrierQualityType.Intact });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 29, RefLtmsQualityId = 29, Name = "2A+", Order = 5, Type = LoadCarrierQualityType.Intact });

            //TODO CHeck QUantityPerEur

            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 30, RefLtmsQualityId = 29, Name = "A", Order = 5 });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 31, RefLtmsQualityId = 29, Name = "B", Order = 5 });
            modelBuilder.Entity<LoadCarrierQuality>().HasData(new LoadCarrierQuality() { Id = 32, RefLtmsQualityId = 29, Name = "C", Order = 5 });

            modelBuilder.Entity<LoadCarrierQualityMapping>(entity =>
            {
                entity.HasData(new LoadCarrierQualityMapping() { Id = 1, LoadCarrierQualityId = 2, RefLmsQualityId = 2 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 2, LoadCarrierQualityId = 10, RefLmsQualityId = 3 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 3, LoadCarrierQualityId = 10, RefLmsQualityId = 15 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 4, LoadCarrierQualityId = 11, RefLmsQualityId = 4 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 5, LoadCarrierQualityId = 11, RefLmsQualityId = 9 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 6, LoadCarrierQualityId = 11, RefLmsQualityId = 16 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 7, LoadCarrierQualityId = 12, RefLmsQualityId = 1 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 8, LoadCarrierQualityId = 12, RefLmsQualityId = 10 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 9, LoadCarrierQualityId = 12, RefLmsQualityId = 17 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 10, LoadCarrierQualityId = 19, RefLmsQualityId = 5 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 11, LoadCarrierQualityId = 19, RefLmsQualityId = 6 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 12, LoadCarrierQualityId = 19, RefLmsQualityId = 8 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 13, LoadCarrierQualityId = 28, RefLmsQualityId = 24 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 14, LoadCarrierQualityId = 28, RefLmsQualityId = 25 });
                entity.HasData(new LoadCarrierQualityMapping() { Id = 15, LoadCarrierQualityId = 29, RefLmsQualityId = 23 });
            });

            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = -1, RefLtmsArticleId = 0, RefLmsLoadCarrierTypeId = 0, Name = "U", Order = 99, QuantityPerEur = 1, MaxStackHeight = 0, MaxStackHeightJumbo = 0 });  //HACK To Fix Problem with 0 as Primary Key
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 1, RefLtmsArticleId = 1, RefLmsLoadCarrierTypeId = 0, Name = "CR1", Order = 90, QuantityPerEur = 1, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 2, RefLtmsArticleId = 2, RefLmsLoadCarrierTypeId = 0, Name = "CR3", Order = 90, QuantityPerEur = 1, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 3, RefLtmsArticleId = 3, RefLmsLoadCarrierTypeId = 1, Name = "DD", Order = 3, QuantityPerEur = 2, BaseLoadCarrier = BaseLoadCarrierInfo.Optional, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 5, RefLtmsArticleId = 5, RefLmsLoadCarrierTypeId = 0, Name = "DplHaPa", Order = 90, QuantityPerEur = 2, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 7, RefLtmsArticleId = 7, RefLmsLoadCarrierTypeId = 0, Name = "Einweg", Order = 90, QuantityPerEur = 1, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 8, RefLtmsArticleId = 8, RefLmsLoadCarrierTypeId = 2, Name = "EUR", Order = 1, QuantityPerEur = 1, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 13, RefLtmsArticleId = 13, RefLmsLoadCarrierTypeId = 6, Name = "GB", Order = 90, QuantityPerEur = 0, MaxStackHeight = 2, MaxStackHeightJumbo = 3 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 16, RefLtmsArticleId = 16, RefLmsLoadCarrierTypeId = 0, Name = "GB_DMiete", Order = 90, QuantityPerEur = 0, MaxStackHeight = 0, MaxStackHeightJumbo = 0 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 18, RefLtmsArticleId = 18, RefLmsLoadCarrierTypeId = 5, Name = "H1", Order = 4, QuantityPerEur = 1, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 19, RefLtmsArticleId = 19, RefLmsLoadCarrierTypeId = 0, Name = "H1_DMiete", Order = 4.6F, QuantityPerEur = 0, MaxStackHeight = 0, MaxStackHeightJumbo = 0 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 20, RefLtmsArticleId = 20, RefLmsLoadCarrierTypeId = 0, Name = "H1GD", Order = 4.5F, QuantityPerEur = 1, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 4, RefLtmsArticleId = 4, RefLmsLoadCarrierTypeId = 10, Name = "E2KR", Order = 6.5F, QuantityPerEur = 4, BaseLoadCarrier = BaseLoadCarrierInfo.Required, MaxStackHeight = 12, MaxStackHeightJumbo = 16 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 6, RefLtmsArticleId = 6, RefLmsLoadCarrierTypeId = 9, Name = "E2EP", Order = 6, QuantityPerEur = 4, BaseLoadCarrier = BaseLoadCarrierInfo.Required, MaxStackHeight = 12, MaxStackHeightJumbo = 16 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 9, RefLtmsArticleId = 9, RefLmsLoadCarrierTypeId = 11, Name = "E1EP", Order = 5, QuantityPerEur = 4, BaseLoadCarrier = BaseLoadCarrierInfo.Required, MaxStackHeight = 18, MaxStackHeightJumbo = 24 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 10, RefLtmsArticleId = 10, RefLmsLoadCarrierTypeId = 12, Name = "E1KR", Order = 5.5F, QuantityPerEur = 4, BaseLoadCarrier = BaseLoadCarrierInfo.Required, MaxStackHeight = 18, MaxStackHeightJumbo = 24 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 21, RefLtmsArticleId = 21, RefLmsLoadCarrierTypeId = 15, Name = "EPAL7", Order = 2, QuantityPerEur = 2, BaseLoadCarrier = BaseLoadCarrierInfo.Optional, MaxStackHeight = 15, MaxStackHeightJumbo = 20 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 30, RefLtmsArticleId = 30, RefLmsLoadCarrierTypeId = 14, Name = "HDB1208mD", Order = 0, QuantityPerEur = 1, MaxStackHeight = 8, MaxStackHeightJumbo = 8 });
            modelBuilder.Entity<LoadCarrierType>().HasData(new LoadCarrierType { Id = 31, RefLtmsArticleId = 31, RefLmsLoadCarrierTypeId = 13, Name = "HDB1208D", Order = 0, QuantityPerEur = 1, MaxStackHeight = 0, MaxStackHeightJumbo = 0 });

            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 1, RefLtmsPalletId = 201, RefLmsQuality2PalletId = 2003, Name = "EUR A", Order = 1, TypeId = 8, QualityId = 30 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 2, RefLtmsPalletId = 202, RefLmsQuality2PalletId = 2004, Name = "EUR B", Order = 2, TypeId = 8, QualityId = 31 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 3, RefLtmsPalletId = 203, RefLmsQuality2PalletId = 2001, Name = "EUR C", Order = 3, TypeId = 8, QualityId = 32 });

            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 100, RefLtmsPalletId = 100, RefLmsQuality2PalletId = 1018, Name = "DD U", Order = -1, TypeId = 3, QualityId = -1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 101, RefLtmsPalletId = 101, RefLmsQuality2PalletId = 1003, Name = "DD 1A", Order = 1, TypeId = 3, QualityId = 10 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 102, RefLtmsPalletId = 102, RefLmsQuality2PalletId = 1004, Name = "DD 2A", Order = 2, TypeId = 3, QualityId = 11 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 103, RefLtmsPalletId = 103, RefLmsQuality2PalletId = 1001, Name = "DD 2B", Order = 3, TypeId = 3, QualityId = 12 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 104, RefLtmsPalletId = 104, RefLmsQuality2PalletId = 1002, Name = "DD D", Order = 4, TypeId = 3, QualityId = 2 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 105, RefLtmsPalletId = 105, RefLmsQuality2PalletId = 1001, Name = "DD I", Order = 0, TypeId = 3, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 106, RefLtmsPalletId = 106, RefLmsQuality2PalletId = 1005, Name = "DD Neu", Order = 9, TypeId = 3, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 107, RefLtmsPalletId = 107, RefLmsQuality2PalletId = 1001, Name = "DD 2BMT", Order = 5, TypeId = 3, QualityId = 23 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 108, RefLtmsPalletId = 108, RefLmsQuality2PalletId = 1001, Name = "DD 2BRmp", Order = 6, TypeId = 3, QualityId = 24 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 109, RefLtmsPalletId = 109, RefLmsQuality2PalletId = 1028, Name = "DD Schr", Order = 7, TypeId = 3, QualityId = 20 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 110, RefLtmsPalletId = 110, RefLmsQuality2PalletId = 0, Name = "DD Chep", Order = 8, TypeId = 3, QualityId = 21 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 111, RefLtmsPalletId = 111, RefLmsQuality2PalletId = 1024, Name = "DD 2BG", Order = 9, TypeId = 3, QualityId = 28 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 112, RefLtmsPalletId = 112, RefLmsQuality2PalletId = 1023, Name = "DD 2APlus", Order = 10, TypeId = 3, QualityId = 29 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 151, RefLtmsPalletId = 151, RefLmsQuality2PalletId = 15005, Name = "EPAL7 Neu", Order = 0, TypeId = 21, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 200, RefLtmsPalletId = 200, RefLmsQuality2PalletId = 2018, Name = "EUR U", Order = 5, TypeId = 8, QualityId = -1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 201, RefLtmsPalletId = 201, RefLmsQuality2PalletId = 2003, Name = "EUR 1A", Order = 1, TypeId = 8, QualityId = 10 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 202, RefLtmsPalletId = 202, RefLmsQuality2PalletId = 2004, Name = "EUR 2A", Order = 2, TypeId = 8, QualityId = 11 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 203, RefLtmsPalletId = 203, RefLmsQuality2PalletId = 2001, Name = "EUR 2B", Order = 3, TypeId = 8, QualityId = 12 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 204, RefLtmsPalletId = 204, RefLmsQuality2PalletId = 2002, Name = "EUR D", Order = 4, TypeId = 8, QualityId = 2 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 205, RefLtmsPalletId = 205, RefLmsQuality2PalletId = 2001, Name = "EUR I", Order = 0, TypeId = 8, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 206, RefLtmsPalletId = 206, RefLmsQuality2PalletId = 2005, Name = "EUR Neu", Order = 10, TypeId = 8, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 208, RefLtmsPalletId = 208, RefLmsQuality2PalletId = 2028, Name = "EUR Schr", Order = 6, TypeId = 8, QualityId = 20 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 209, RefLtmsPalletId = 209, RefLmsQuality2PalletId = 0, Name = "EUR EW", Order = 7, TypeId = 8, QualityId = 14 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 210, RefLtmsPalletId = 210, RefLmsQuality2PalletId = 0, Name = "EUR Chep", Order = 8, TypeId = 8, QualityId = 21 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 211, RefLtmsPalletId = 211, RefLmsQuality2PalletId = 2001, Name = "EUR MPal", Order = 9, TypeId = 8, QualityId = 22 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 300, RefLtmsPalletId = 300, RefLmsQuality2PalletId = 6011, Name = "GB U", Order = 1, TypeId = 13, QualityId = -1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 301, RefLtmsPalletId = 301, RefLmsQuality2PalletId = 6005, Name = "GB 1A", Order = 0, TypeId = 13, QualityId = 10 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 302, RefLtmsPalletId = 302, RefLmsQuality2PalletId = 6011, Name = "GB I", Order = 1, TypeId = 13, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 303, RefLtmsPalletId = 303, RefLmsQuality2PalletId = 6002, Name = "GB D", Order = 2, TypeId = 13, QualityId = 2 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 305, RefLtmsPalletId = 305, RefLmsQuality2PalletId = 6011, Name = "GB EigV", Order = 4, TypeId = 13, QualityId = 25 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 306, RefLtmsPalletId = 306, RefLmsQuality2PalletId = 6029, Name = "GB EW", Order = 5, TypeId = 13, QualityId = 14 });

            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 311, RefLtmsPalletId = 311, RefLmsQuality2PalletId = 14005, Name = "HDB1208mD Neu", Order = 0, TypeId = 30, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 312, RefLtmsPalletId = 312, RefLmsQuality2PalletId = 14030, Name = "HDB1208mD I", Order = 1, TypeId = 30, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 313, RefLtmsPalletId = 313, RefLmsQuality2PalletId = 14002, Name = "HDB1208mD D", Order = 2, TypeId = 30, QualityId = 2 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 321, RefLtmsPalletId = 321, RefLmsQuality2PalletId = 13005, Name = "HDB1208D Neu", Order = 0, TypeId = 31, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 322, RefLtmsPalletId = 322, RefLmsQuality2PalletId = 13030, Name = "HDB1208D I", Order = 1, TypeId = 31, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 323, RefLtmsPalletId = 23, RefLmsQuality2PalletId = 13002, Name = "HDB1208D D", Order = 2, TypeId = 31, QualityId = 2 });

            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 400, RefLtmsPalletId = 400, RefLmsQuality2PalletId = 5012, Name = "H1 U", Order = 6, TypeId = 18, QualityId = -1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 401, RefLtmsPalletId = 401, RefLmsQuality2PalletId = 5005, Name = "H1 neu", Order = 0, TypeId = 18, QualityId = 10 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 402, RefLtmsPalletId = 402, RefLmsQuality2PalletId = 5011, Name = "H1 I", Order = 1, TypeId = 18, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 403, RefLtmsPalletId = 403, RefLmsQuality2PalletId = 5011, Name = "H1 mK", Order = 3, TypeId = 18, QualityId = 15 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 404, RefLtmsPalletId = 404, RefLmsQuality2PalletId = 5011, Name = "H1 oK", Order = 4, TypeId = 18, QualityId = 16 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 405, RefLtmsPalletId = 405, RefLmsQuality2PalletId = 5011, Name = "H1 sauber", Order = 5, TypeId = 18, QualityId = 17 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 406, RefLtmsPalletId = 406, RefLmsQuality2PalletId = 5012, Name = "H1 dreckig", Order = 6, TypeId = 18, QualityId = 18 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 407, RefLtmsPalletId = 407, RefLmsQuality2PalletId = 5002, Name = "H1 D", Order = 2, TypeId = 18, QualityId = 2 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 408, RefLtmsPalletId = 408, RefLmsQuality2PalletId = 5011, Name = "H1 ODmK", Order = 7, TypeId = 18, QualityId = 26 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 409, RefLtmsPalletId = 409, RefLmsQuality2PalletId = 5011, Name = "H1 ODoK", Order = 8, TypeId = 18, QualityId = 27 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 410, RefLtmsPalletId = 410, RefLmsQuality2PalletId = 5002, Name = "H1 Schr", Order = 9, TypeId = 18, QualityId = 20 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 411, RefLtmsPalletId = 411, RefLmsQuality2PalletId = 0, Name = "H1 EW", Order = 10, TypeId = 18, QualityId = 14 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 450, RefLtmsPalletId = 450, RefLmsQuality2PalletId = 0, Name = "H1 DMiete I", Order = 1, TypeId = 19, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 502, RefLtmsPalletId = 502, RefLmsQuality2PalletId = 0, Name = "H1GD I", Order = 1, TypeId = 20, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 601, RefLtmsPalletId = 601, RefLmsQuality2PalletId = 9005, Name = "E2EP Neu", Order = 1, TypeId = 6, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 602, RefLtmsPalletId = 602, RefLmsQuality2PalletId = 9026, Name = "E2EP I", Order = 2, TypeId = 6, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 603, RefLtmsPalletId = 603, RefLmsQuality2PalletId = 9002, Name = "E2EP D", Order = 3, TypeId = 6, QualityId = 2 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 701, RefLtmsPalletId = 701, RefLmsQuality2PalletId = 10005, Name = "E2KR Neu", Order = 1, TypeId = 4, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 702, RefLtmsPalletId = 702, RefLmsQuality2PalletId = 10026, Name = "E2KR I", Order = 2, TypeId = 4, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 703, RefLtmsPalletId = 703, RefLmsQuality2PalletId = 10002, Name = "E2KR D", Order = 3, TypeId = 4, QualityId = 2 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 801, RefLtmsPalletId = 801, RefLmsQuality2PalletId = 12005, Name = "E1KR Neu", Order = 1, TypeId = 10, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 802, RefLtmsPalletId = 802, RefLmsQuality2PalletId = 12026, Name = "E1KR I", Order = 2, TypeId = 10, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 803, RefLtmsPalletId = 803, RefLmsQuality2PalletId = 12002, Name = "E1KR D", Order = 3, TypeId = 10, QualityId = 2 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 901, RefLtmsPalletId = 901, RefLmsQuality2PalletId = 11005, Name = "E1EP Neu", Order = 1, TypeId = 9, QualityId = 19 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 902, RefLtmsPalletId = 902, RefLmsQuality2PalletId = 11026, Name = "E1EP I", Order = 2, TypeId = 9, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = 903, RefLtmsPalletId = 903, RefLmsQuality2PalletId = 11002, Name = "E1EP D", Order = 3, TypeId = 9, QualityId = 2 });

            //Legacy LoadCarrier, caused by data migrations from old systems
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = -1, RefLtmsPalletId = 0, RefLmsQuality2PalletId = 0, Name = "U U", Order = 0, TypeId = -1, QualityId = -1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = -701, RefLtmsPalletId = -701, RefLmsQuality2PalletId = 0, Name = "Einweg I", Order = 0, TypeId = 7, QualityId = 1 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = -12, RefLtmsPalletId = -12, RefLmsQuality2PalletId = 0, Name = "U 2B", Order = 0, TypeId = -1, QualityId = 12 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = -512, RefLtmsPalletId = -512, RefLmsQuality2PalletId = 0, Name = "DplHaPa 2B", Order = 0, TypeId = 5, QualityId = 12 });
            modelBuilder.Entity<LoadCarrier>().HasData(new LoadCarrier() { Id = -813, RefLtmsPalletId = -813, RefLmsQuality2PalletId = 0, Name = "EUR 2+", Order = 0, TypeId = 8, QualityId = 13 });

            // BaseLoadCarrierMappings
            // DD -> EUR 2B
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 3, LoadCarrierId = 203 });
            // E2KR -> EUR 2B, H1 I
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 4, LoadCarrierId = 203 });
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 4, LoadCarrierId = 402 });
            // E2EP -> EUR 2B, H1 I
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 6, LoadCarrierId = 203 });
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 6, LoadCarrierId = 402 });
            // E1EP -> EUR 2B, H1 I
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 9, LoadCarrierId = 203 });
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 9, LoadCarrierId = 402 });
            // E1KR -> EUR 2B, H1 I
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 10, LoadCarrierId = 203 });
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 10, LoadCarrierId = 402 });
            // EPAL7 -> EUR 2B
            modelBuilder.Entity<BaseLoadCarrierMapping>().HasData(new BaseLoadCarrierMapping() { LoadCarrierTypeId = 21, LoadCarrierId = 203 });

            #endregion

            #region LoadCarrierReceiptPresets

            modelBuilder.Entity<LoadCarrierReceiptDepotPreset>().HasData(new LoadCarrierReceiptDepotPreset() { Id = 1, Category = LoadCarrierReceiptDepotPresetCategory.External, Name = "[EUR und DD]: Depotannahme (von Dritten)", IsSortingRequired = true });
            modelBuilder.Entity<LoadCarrierReceiptDepotPreset>().HasData(new LoadCarrierReceiptDepotPreset() { Id = 2, Category = LoadCarrierReceiptDepotPresetCategory.External, Name = "[H1]: Depotannahme (von Dritten)", IsSortingRequired = true });
            modelBuilder.Entity<LoadCarrierReceiptDepotPreset>().HasData(new LoadCarrierReceiptDepotPreset() { Id = 3, Category = LoadCarrierReceiptDepotPresetCategory.Internal, Name = "[EUR und DD]:  Einlagerung (für DPL)", IsSortingRequired = false });
            modelBuilder.Entity<LoadCarrierReceiptDepotPreset>().HasData(new LoadCarrierReceiptDepotPreset() { Id = 4, Category = LoadCarrierReceiptDepotPresetCategory.Internal, Name = "[H1]:  Einlagerung (für DPL)", IsSortingRequired = false });
            modelBuilder.Entity<LoadCarrierReceiptDepotPreset>().HasData(new LoadCarrierReceiptDepotPreset() { Id = 5, Category = LoadCarrierReceiptDepotPresetCategory.Internal, Name = "[H1 und E2KR]:  Einlagerung (für DPL)", IsSortingRequired = false });
            modelBuilder.Entity<LoadCarrierReceiptDepotPreset>().HasData(new LoadCarrierReceiptDepotPreset() { Id = 6, Category = LoadCarrierReceiptDepotPresetCategory.Internal, Name = "[H1 und E2EP]:  Einlagerung (für DPL)", IsSortingRequired = false });
            modelBuilder.Entity<LoadCarrierReceiptDepotPreset>().HasData(new LoadCarrierReceiptDepotPreset() { Id = 7, Category = LoadCarrierReceiptDepotPresetCategory.Internal, Name = "[H1 und E1EP]:  Einlagerung (für DPL)", IsSortingRequired = false });
            modelBuilder.Entity<LoadCarrierReceiptDepotPreset>().HasData(new LoadCarrierReceiptDepotPreset() { Id = 8, Category = LoadCarrierReceiptDepotPresetCategory.Internal, Name = "[H1 und E1KR]:  Einlagerung (für DPL)", IsSortingRequired = false });

            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 1, LoadCarrierId = 103 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 1, LoadCarrierId = 104 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 1, LoadCarrierId = 203 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 1, LoadCarrierId = 204 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 1, LoadCarrierId = 208 });

            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 2, LoadCarrierId = 406 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 2, LoadCarrierId = 410 });

            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 3, LoadCarrierId = 103 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 3, LoadCarrierId = 104 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 3, LoadCarrierId = 203 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 3, LoadCarrierId = 204 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 3, LoadCarrierId = 208 });

            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 4, LoadCarrierId = 406 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 4, LoadCarrierId = 410 });

            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 5, LoadCarrierId = 406 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 5, LoadCarrierId = 410 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 5, LoadCarrierId = 702 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 5, LoadCarrierId = 703 });

            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 6, LoadCarrierId = 406 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 6, LoadCarrierId = 410 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 6, LoadCarrierId = 602 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 6, LoadCarrierId = 603 });

            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 7, LoadCarrierId = 406 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 7, LoadCarrierId = 410 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 7, LoadCarrierId = 902 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 7, LoadCarrierId = 903 });

            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 8, LoadCarrierId = 406 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 8, LoadCarrierId = 410 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 8, LoadCarrierId = 802 });
            modelBuilder.Entity<LoadCarrierReceiptDepotPresetLoadCarrier>().HasData(new LoadCarrierReceiptDepotPresetLoadCarrier() { LoadCarrierReceiptDepotPresetId = 8, LoadCarrierId = 803 });

            #endregion

            #region LocalizationItem

            var fieldNames = new
            {
                ShortName = "ShortName",
                LongName = "LongName",
                Description = "Description"
            };

            // Entities
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10001, Name = "LoadCarriers", Type = LocalizationItemType.Entity });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10010, Name = "LoadCarrierQualities", Type = LocalizationItemType.Entity });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10020, Name = "LoadCarrierTypes", Type = LocalizationItemType.Entity });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10021, Name = "LoadCarrierTypes", Type = LocalizationItemType.Entity, FieldName = fieldNames.LongName });

            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10030, Name = "Countries", Type = LocalizationItemType.Entity });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10040, Name = "CountryStates", Type = LocalizationItemType.Entity });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10050, Name = "PublicHolidays", Type = LocalizationItemType.Entity });

            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10060, Name = "DocumentTypes", Type = LocalizationItemType.Entity });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10061, Name = "DocumentTypes", Type = LocalizationItemType.Entity, FieldName = fieldNames.ShortName });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10062, Name = "DocumentTypes", Type = LocalizationItemType.Entity, FieldName = fieldNames.Description });

            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10070, Name = "DocumentStates", Type = LocalizationItemType.Entity });

            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10080, Name = "VoucherReasonTypes", Type = LocalizationItemType.Entity });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10081, Name = "VoucherReasonTypes", Type = LocalizationItemType.Entity, FieldName = fieldNames.ShortName });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10082, Name = "VoucherReasonTypes", Type = LocalizationItemType.Entity, FieldName = fieldNames.Description });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10083, Name = "VoucherReasonTypes", Type = LocalizationItemType.Entity, FieldName = "DescriptionReport" });

            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 10090, Name = "LocalizationLanguages", Type = LocalizationItemType.Entity });

            // Enum types for the Namespace Dpl.B2b.Contracts.Models
            // possibility exists to build by Refleciton however the Ids could change by the sorting
            // var enumNames = GetEnumsNamesForAssembly("Dpl.B2b.Contracts", "Dpl.B2b.Contracts.Models").Select(x => x.Name);

            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20000, Name = "AccountingRecordType", Reference = typeof(Contracts.Models.AccountingRecordType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20007, Name = "AccountingRecordType", Reference = typeof(Contracts.Models.AccountingRecordType).FullName, Type = LocalizationItemType.Enum, FieldName = "name" });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20008, Name = "AccountingRecordType", Reference = typeof(Contracts.Models.AccountingRecordType).FullName, Type = LocalizationItemType.Enum, FieldName = "description" });

            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20001, Name = "AccountingRecordStatus", Reference = typeof(Contracts.Models.AccountingRecordStatus).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20002, Name = "LoadCarrierTransferStatus", Reference = typeof(Contracts.Models.BalanceTransferStatus).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20003, Name = "LoadCarrierQualityType", Reference = typeof(LoadCarrierQualityType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20004, Name = "OrderType", Reference = typeof(OrderType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20005, Name = "OrderTransportType", Reference = typeof(OrderTransportType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 20006, Name = "OrderQuantityType", Reference = typeof(OrderQuantityType).FullName, Type = LocalizationItemType.Enum });

            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 107, Name = "OrderStatus", Reference = typeof(OrderStatus).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 108, Name = "OrderMatchStatus", Reference = typeof(OrderMatchStatus).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 109, Name = "PostingAccountType", Reference = typeof(Contracts.Models.PostingAccountType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 110, Name = "SubmissionType", Reference = typeof(Contracts.Models.SubmissionType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 111, Name = "PermissionResourceType", Reference = typeof(PermissionResourceType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 112, Name = "VoucherType", Reference = typeof(VoucherType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 113, Name = "VoucherStatus", Reference = typeof(VoucherStatus).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 114, Name = "PartnerType", Reference = typeof(PartnerType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 115, Name = "DayOfWeek", Reference = typeof(DayOfWeek).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 116, Name = "AdhocTranslations", Reference = typeof(Common.Enumerations.AdhocTranslations).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 117, Name = "UserRole", Reference = typeof(Common.Enumerations.UserRole).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 118, Name = "PersonGender", Reference = typeof(Common.Enumerations.PersonGender).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 119, Name = "LoadCarrierReceiptType", Reference = typeof(Common.Enumerations.LoadCarrierReceiptType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 120, Name = "TransportOfferingStatus", Reference = typeof(Contracts.Models.TransportOfferingStatus).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 121, Name = "TransportBidStatus", Reference = typeof(Common.Enumerations.TransportBidStatus).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 122, Name = "DocumentTypeEnum", Reference = typeof(Common.Enumerations.DocumentTypeEnum).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 123, Name = "DocumentTypeEnum", Reference = typeof(Common.Enumerations.DocumentTypeEnum).FullName, Type = LocalizationItemType.Enum, FieldName = "ReportVoucherCommonTitle" });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 124, Name = "OrderLoadStatus", Reference = typeof(Common.Enumerations.OrderLoadStatus).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 125, Name = "EmployeeNoteType", Reference = typeof(Common.Enumerations.EmployeeNoteType).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 126, Name = "EmployeeNoteReason", Reference = typeof(Common.Enumerations.EmployeeNoteReason).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 127, Name = "LoadCarrierReceiptDepoPresetCategory", Reference = typeof(Common.Enumerations.LoadCarrierReceiptDepotPresetCategory).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 128, Name = "ResourceAction", Reference = typeof(Common.Enumerations.ResourceAction).FullName, Type = LocalizationItemType.Enum });
            modelBuilder.Entity<LocalizationItem>().HasData(new LocalizationItem() { Id = 129, Name = "BusinessHourExceptionType", Reference = typeof(Common.Enumerations.BusinessHourExceptionType).FullName, Type = LocalizationItemType.Enum });

            #endregion

            #region LocalizationLanguage

            modelBuilder.Entity<LocalizationLanguage>().HasData(new LocalizationLanguage() { Id = 1, Name = "Deutsch", Locale = "de" });
            modelBuilder.Entity<LocalizationLanguage>().HasData(new LocalizationLanguage() { Id = 2, Name = "Englisch", Locale = "en" });
            modelBuilder.Entity<LocalizationLanguage>().HasData(new LocalizationLanguage() { Id = 3, Name = "Französisch", Locale = "fr" });
            modelBuilder.Entity<LocalizationLanguage>().HasData(new LocalizationLanguage() { Id = 4, Name = "Polnisch", Locale = "pl" });

            #endregion

            #region OrderMatch

            modelBuilder.Entity<OrderMatch>().HasIndex(dc => dc.DigitalCode).IsUnique();

            #endregion

            #region VoucherReasonTypes

            modelBuilder.Entity<VoucherReasonType>().HasData(new VoucherReasonType() { Id = -1, RefLtmsReasonTypeId = "KAG", Description = "Keine Angaben", Name = "k.A.", Order = 99 });
            modelBuilder.Entity<VoucherReasonType>().HasData(new VoucherReasonType() { Id = 1, RefLtmsReasonTypeId = "IPT", Description = "kein Tausch notwendig.  Kunde ist DPL-Pooling Partner.", Name = "PT.", Order = 2 });
            modelBuilder.Entity<VoucherReasonType>().HasData(new VoucherReasonType() { Id = 2, RefLtmsReasonTypeId = "KTM", Description = "kein Tausch erfolgt. Aussteller tauschte nicht.", Name = "KTG", Order = 1 });
            modelBuilder.Entity<VoucherReasonType>().HasData(new VoucherReasonType() { Id = 3, RefLtmsReasonTypeId = "KTG", Description = "kein Tausch gewünscht. Fahrer verweigert.", Name = "KTM", Order = 3 });

            #endregion

            #region OrderMatch

            modelBuilder.Entity<OrderMatch>().HasIndex(dc => dc.DigitalCode).IsUnique();

            #endregion

            #region AccountingRecord
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 1,
                    RefLtmBookingTypeId = "ABH",
                    Name = "Abholung",
                    Description = "Abholung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 2,
                    RefLtmBookingTypeId = "ABHL",
                    Name = "Abholung (Ladungsweise)",
                    Description = "Ladungsweise Abholung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 3,
                    RefLtmBookingTypeId = "AKD",
                    Name = "Ausgleich körperlich an DPL",
                    Description = "Ausgleich körperlich an DPL"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 4,
                    RefLtmBookingTypeId = "AKK",
                    Name = "Ausgleich körperlich an Kunde",
                    Description = "Ausgleich körperlich an Kunde"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 5,
                    RefLtmBookingTypeId = "ANL",
                    Name = "Anlieferung",
                    Description = "Anlieferung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 6,
                    RefLtmBookingTypeId = "ANLDEP",
                    Name = "Depotabgabe",
                    Description = "Anlieferung/Abgabe am Depot"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 7,
                    RefLtmBookingTypeId = "ANLDPK",
                    Name = "Depotabgabe Kleinmengen",
                    Description = "Anlieferung/Abgabe am Depot in Kleinmengen"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 8,
                    RefLtmBookingTypeId = "AUSDPG",
                    Name = "Ausgleich DPL-OPG",
                    Description = "Ausgleich DPL-OPG Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 9,
                    RefLtmBookingTypeId = "AUSOPG",
                    Name = "Ausgang OPG",
                    Description = "Ausgang OPG Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 10,
                    RefLtmBookingTypeId = "DIFF",
                    Name = "Differenzbuchung",
                    Description = "Rückbuchung der Differenz von freigestellten/abgelehnten Paletten"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 11,
                    RefLtmBookingTypeId = "DQ",
                    Name = "Defektquote",
                    Description = "Abschreibung über Defektquoten Konto"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 12,
                    RefLtmBookingTypeId = "EINOPG",
                    Name = "Eingang OPG",
                    Description = "Eingang OPG Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 13,
                    RefLtmBookingTypeId = "ENTSOR",
                    Name = "Entsorgung",
                    Description = "Entsorgung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 14,
                    RefLtmBookingTypeId = "FSD",
                    Name = "Freistellung Defekt",
                    Description = "Freistellung defekter Paletten an DPL"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 15,
                    RefLtmBookingTypeId = "FSI",
                    Name = "Freistellung Intakt",
                    Description = "Freistellung körperlich an DPL"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 16,
                    RefLtmBookingTypeId = "GUTDPG",
                    Name = "Gutsch. DPL-OPG",
                    Description = "Gutschrift DPL-OPG Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 17,
                    RefLtmBookingTypeId = "GUTOPG",
                    Name = "Gutschrift OPG",
                    Description = "Gutschrift OPG Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 18,
                    RefLtmBookingTypeId = "K",
                    Name = "Klärfall",
                    Description = "Kontoumbuchung an Klärkonto"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 19,
                    RefLtmBookingTypeId = "KAUF",
                    Name = "Kauf",
                    Description = "Kauf von Paletten"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 20,
                    RefLtmBookingTypeId = "KBU",
                    Name = "Palettenkorrekturbuchung",
                    Description = "Abholung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 21,
                    RefLtmBookingTypeId = "KDPG",
                    Name = "Kauf DPL-OPG",
                    Description = "Abholung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 22,
                    RefLtmBookingTypeId = "KKD",
                    Name = "Abholung",
                    Description = "Abholung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 23,
                    RefLtmBookingTypeId = "KKK",
                    Name = "Kauf aus Konto durch Kunde",
                    Description = "Abholung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 24,
                    RefLtmBookingTypeId = "KUA",
                    Name = "Kontoumbuchung",
                    Description = "Kontoumbuchung (Abgang)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 25,
                    RefLtmBookingTypeId = "KUABA",
                    Name = "Ablehnung",
                    Description = "Kontoumbuchung an Ablehnungskonto (Abgang)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 26,
                    RefLtmBookingTypeId = "KUABZ",
                    Name = "Abholung",
                    Description = "Kontoumbuchung an Ablehnungskonto (Zugang)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 27,
                    RefLtmBookingTypeId = "KUFSA",
                    Name = "Freistellung",
                    Description = "Kontoumbuchung an Freistellung (Abgang)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 28,
                    RefLtmBookingTypeId = "KUFSZ",
                    Name = "Freistellung",
                    Description = "Kontoumbuchung an Freistellung (Zugang)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 29,
                    RefLtmBookingTypeId = "KUZ",
                    Name = "Kontoumbuchung",
                    Description = "Kontoumbuchung (Zugang)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 30,
                    RefLtmBookingTypeId = "KZA",
                    Name = "Korrekturzahlung",
                    Description = "Korrekturzahlung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 31,
                    RefLtmBookingTypeId = "LIPAK",
                    Name = "Lieferung Pooling an Kunde",
                    Description = "Lieferung von Paletten im DPL-Pooling an Kunde"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 32,
                    RefLtmBookingTypeId = "MAN",
                    Name = "Manuelle",
                    Description = "Manuelle Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 33,
                    RefLtmBookingTypeId = "PAT",
                    Name = "Palettenarten-Tausch",
                    Description = "Palettenarten-Tausch"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 34,
                    RefLtmBookingTypeId = "PM",
                    Name = "Preisminderung",
                    Description = "Berechnung einer geringeren Poolinggebühr"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 35,
                    RefLtmBookingTypeId = "POOL",
                    Name = "Abrechn. Pooling",
                    Description = "Abrechnung Pooling Tausch"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 36,
                    RefLtmBookingTypeId = "QT",
                    Name = "Qualitätentausch",
                    Description = "Palettenqualitäten-Tausch"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 37,
                    RefLtmBookingTypeId = "REIN",
                    Name = "Reinigung",
                    Description = "Reinigung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 38,
                    RefLtmBookingTypeId = "REP",
                    Name = "Repa. g. Gebühr",
                    Description = "Reparatur gegen Gebühr (Aus defektem Guthaben)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 39,
                    RefLtmBookingTypeId = "REPAUF",
                    Name = "Reparaturauftrag",
                    Description = "Einreichung Reparaturauftrag für defekte Paletten vom Handel"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 40,
                    RefLtmBookingTypeId = "REPKND",
                    Name = "Repa Kunde",
                    Description = "Berechnung eines Reparaturpreises vom Kunden an DPL"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 41,
                    RefLtmBookingTypeId = "REPQU",
                    Name = "Reparatur-Quote",
                    Description = "Reparatur-Quote"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 42,
                    RefLtmBookingTypeId = "RETOUR",
                    Name = "Retoure",
                    Description = "Retoure/Stornierung einer Versandmeldung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 43,
                    RefLtmBookingTypeId = "ROH",
                    Name = "Rückgabe PG an Kunde",
                    Description = "Rückgabe Palettengutschrift an Kunde"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 44,
                    RefLtmBookingTypeId = "RPT",
                    Name = "Reparatur-Tausch",
                    Description = "Reparatur-Tausch (Aus defektem Guthaben)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 45,
                    RefLtmBookingTypeId = "SCHULD",
                    Name = "Schuldschein",
                    Description = "Einreichung Schuldschein"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 46,
                    RefLtmBookingTypeId = "SORT",
                    Name = "Sortierung",
                    Description = "Sortierung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 47,
                    RefLtmBookingTypeId = "STORNO",
                    Name = "Stornierung",
                    Description = "Stornierungsbuchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 48,
                    RefLtmBookingTypeId = "SYS",
                    Name = "System",
                    Description = "System"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 49,
                    RefLtmBookingTypeId = "TADIFF",
                    Name = "Transportdifferenz",
                    Description = "Transportdifferenz"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 50,
                    RefLtmBookingTypeId = "U",
                    Name = "Unbestimmt",
                    Description = "Unbestimmt Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 51,
                    RefLtmBookingTypeId = "VERK",
                    Name = "Verkauf",
                    Description = "Verkauf von Paletten"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 52,
                    RefLtmBookingTypeId = "VERLUS",
                    Name = "Verlust",
                    Description = "Abschreibung als Verlust"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 53,
                    RefLtmBookingTypeId = "VERS",
                    Name = "Versandmeldung",
                    Description = "Versandmeldung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 54,
                    RefLtmBookingTypeId = "VERSA",
                    Name = "Versand (Ausland)",
                    Description = "Versandmeldung (ins Ausland)"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 55,
                    RefLtmBookingTypeId = "WMP",
                    Name = "Wandlung monetäre Vergütung",
                    Description = "Wandlung monetäre Vergütung in Palettengutschrift"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 56,
                    RefLtmBookingTypeId = "GUTDDG",
                    Name = "Gutsch. DPL-DPG",
                    Description = "Gutschrift DPL-DPG Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 57,
                    RefLtmBookingTypeId = "AUSDDG",
                    Name = "Ausgleich DPL-DPG",
                    Description = "Ausgleich DPL-DPG Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 58,
                    RefLtmBookingTypeId = "GUTDBG",
                    Name = "Direktgutschrift",
                    Description = "Gutschrift Direkt Buchung"
                }
            );
            modelBuilder.Entity<AccountingRecordType>().HasData(
                new AccountingRecordType()
                {
                    Id = 59,
                    RefLtmBookingTypeId = "AUSDBG",
                    Name = "Direktbuchung",
                    Description = "Ausgleich Direkt Buchung"
                }
            );

            #endregion

            #endregion

        }

        private void AddCountries(ModelBuilder modelBuilder)
        {
            var countries = new Country[]
            {
                new Country()
                {
                    Name = "Germany",
                    Iso2Code = "DE",
                    Iso3Code = "DEU",
                    LicensePlateCode = "D",
                    States = new CountryState[] {
                        new CountryState() { Name= "Baden-Württemberg", Iso2Code="BW" },
                        new CountryState() { Name= "Bavaria", PublicHolidays =  new PublicHoliday[]{
                                new PublicHoliday() { Date = new DateTime(2020,11,1), Name = "Allerheiligen" }
                            }
                        },
                        new CountryState() { Name= "Berlin", Iso2Code="BE" },
                        new CountryState() { Name= "Brandenburg", Iso2Code="BB" },
                        new CountryState() { Name= "Bremen", Iso2Code="HB" },
                        new CountryState() { Name= "Hamburg", Iso2Code="HH" },
                        new CountryState() { Name= "Hesse", Iso2Code="HE" },
                        new CountryState() { Name= "Lower Saxony", Iso2Code="NI" },
                        new CountryState() { Name= "Mecklenburg-Vorpommern", Iso2Code="MV" },
                        new CountryState() { Name= "North Rhine-Westphalia", Iso2Code="NW", PublicHolidays =  new PublicHoliday[]{
                                new PublicHoliday() { Date = new DateTime(2020,11,1), Name = "Allerheiligen" }
                            }
                        },
                        new CountryState() { Name= "Rhineland-Palatinate", Iso2Code="RP" },
                        new CountryState() { Name= "Saarland", Iso2Code="SL" },
                        new CountryState() { Name= "Saxony", Iso2Code="SN" },
                        new CountryState() { Name= "Saxony-Anhalt", Iso2Code="ST" },
                        new CountryState() { Name= "Schleswig-Holstein", Iso2Code="SH" },
                        new CountryState() { Name= "Thuringia", Iso2Code="TH" },
                    },
                    PublicHolidays = new PublicHoliday[]{
                        new PublicHoliday() { Date = new DateTime(2020, 1, 1), Name = "Neujahr" },
                        new PublicHoliday() { Date = new DateTime(2020, 10, 3), Name = "Tag der Deutschen Einheit" }
                    },
                },
                new Country() {
                    Name="France",
                    Iso2Code = "FR",
                    Iso3Code = "FRA",
                    LicensePlateCode = "F"
                },
                new Country() {
                    Name="Austria",
                    Iso2Code = "AT",
                    Iso3Code = "AUT",
                    LicensePlateCode = "AT"
                },
                new Country() { Name="Switzerland",
                    Iso2Code = "CH",
                    Iso3Code = "CHE",
                    LicensePlateCode = "CH"
                },
                new Country() { Name="Poland",
                    Iso2Code = "PL",
                    Iso3Code = "POL",
                    LicensePlateCode = "PL"
                },
                new Country() { Name="United Kingdom",
                    Iso2Code = "GB",
                    Iso3Code = "GBR",
                    LicensePlateCode = "GB"
                },
                new Country() { Name="Czech Republic",
                    Iso2Code = "CZ",
                    Iso3Code = "CZE",
                    LicensePlateCode = "CZ"
                },
                new Country() { Name="Netherlands",
                    Iso2Code = "NL",
                    Iso3Code = "NLD",
                    LicensePlateCode = "NL"
                },
                new Country() { Name="Italy",
                    Iso2Code = "IT",
                    Iso3Code = "ITA",
                    LicensePlateCode = "I"
                },
                new Country() { Name="Belgium",
                    Iso2Code = "BE",
                    Iso3Code = "BEL",
                    LicensePlateCode = "B"
                },
                new Country() { Name="Greece",
                    Iso2Code = "GR",
                    Iso3Code = "GRC",
                    LicensePlateCode = "GR"
                },
                new Country() { Name="Denmark",
                    Iso2Code = "DK",
                    Iso3Code = "DNK",
                    LicensePlateCode = "DK"
                },
                new Country() { Name="Luxembourg",
                    Iso2Code = "LU",
                    Iso3Code = "LUX",
                    LicensePlateCode = "L"
                },
                new Country() { Name="Bulgaria",
                    Iso2Code = "BG",
                    Iso3Code = "BGR",
                    LicensePlateCode = "BG"
                },
                new Country() { Name="Slovakia",
                    Iso2Code = "SK",
                    Iso3Code = "SVK",
                    LicensePlateCode = "SK"
                },
                new Country() { Name="Slovenia",
                    Iso2Code = "SI",
                    Iso3Code = "SVN",
                    LicensePlateCode = "SLO"
                },
                new Country() { Name="Hungary",
                    Iso2Code = "HU",
                    Iso3Code = "HUN",
                    LicensePlateCode = "H"
                },
                new Country() { Name="Sweden",
                    Iso2Code = "SE",
                    Iso3Code = "SWE",
                    LicensePlateCode = "S"
                },
                new Country() { Name="Finland",
                    Iso2Code = "FI",
                    Iso3Code = "FIN",
                    LicensePlateCode = "FIN"
                },
                new Country() { Name="Spain",
                    Iso2Code = "ES",
                    Iso3Code = "ESP",
                    LicensePlateCode = "E"
                },
                new Country() { Name="Lithuania",
                    Iso2Code = "LT",
                    Iso3Code = "LTU",
                    LicensePlateCode = "LT"
                },
                new Country() { Name="Serbia",
                    Iso2Code = "RS",
                    Iso3Code = "SRB",
                    LicensePlateCode = "SRB"
                },
                new Country() { Name="Romania",
                    Iso2Code = "RO",
                    Iso3Code = "ROU",
                    LicensePlateCode = "RO" 
                },
                new Country() { Name="Ukraine",
                    Iso2Code = "UA",
                    Iso3Code = "UKR",
                    LicensePlateCode = "UA"
                },
                new Country() { Name="Liechtenstein",
                    Iso2Code = "LI",
                    Iso3Code = "LIE",
                    LicensePlateCode = "FL"
                },
                new Country() { Name="Croatia",
                    Iso2Code = "HR",
                    Iso3Code = "HRV",
                    LicensePlateCode = "HR"
                },
                new Country() { Name="Latvia",
                    Iso2Code = "LV",
                    Iso3Code = "LVA",
                    LicensePlateCode = "LV"
                },
                new Country() { Name="Cyprus",
                    Iso2Code = "CY",
                    Iso3Code = "CYP",
                    LicensePlateCode = "CY"
                },
                new Country() { Name="Macedonia",
                    Iso2Code = "MK",
                    Iso3Code = "MKD",
                    LicensePlateCode = "NMK"
                },
                new Country() { Name="Portugal",
                    Iso2Code = "PT",
                    Iso3Code = "PRT",
                    LicensePlateCode = "P"
                },
                new Country() { Name="Estonia",
                    Iso2Code = "EE",
                    Iso3Code = "EST",
                    LicensePlateCode = "EST"
                },
                new Country() { Name="Bosnia and Herzegovina",
                    Iso2Code = "BH",
                    Iso3Code = "BIH",
                    LicensePlateCode = "BIH"
                },
                new Country() { Name="Norway",
                    Iso2Code = "NO",
                    Iso3Code = "NOR0",
                    LicensePlateCode = "N"
                },
                new Country() { Name="Republic of Kosovo",
                    Iso2Code = "XK",
                    Iso3Code = "XKX",
                    LicensePlateCode = "RKS"
                },
                new Country() { Name="Montenegro",
                    Iso2Code = "ME",
                    Iso3Code = "MNE",
                    LicensePlateCode = "MNE"
                },
                new Country() { Name="Turkey",
                    Iso2Code = "TR",
                    Iso3Code = "TUR",
                    LicensePlateCode = "TR"
                },
                new Country() { Name="China",
                    Iso2Code = "CN",
                    Iso3Code = "CHN",
                    LicensePlateCode = "CHN"
                },
                new Country() { Name="United States",
                    Iso2Code = "US",
                    Iso3Code = "USA",
                    LicensePlateCode = "USA"
                },
                new Country() { Name="Ireland",
                    Iso2Code = "IE",
                    Iso3Code = "IRL",
                    LicensePlateCode = "IRL"
                },
            };

            var countryId = 0;
            var countryStateId = 0;
            var publicHolidayId = 0;

            foreach (var country in countries)
            {
                var data = new Country
                {
                    Id = ++countryId,
                    Name = country.Name,
                    Iso2Code = country.Iso2Code,
                    Iso3Code = country.Iso3Code,
                    LicensePlateCode = country.LicensePlateCode,
                };

                modelBuilder.Entity<Country>().HasData(data);
                if (country.PublicHolidays != null)
                {
                    foreach (var publicHoliday in country.PublicHolidays)
                    {
                        var publicHolidayData = new PublicHoliday()
                        {
                            Id = ++publicHolidayId,
                            CountryId = countryId,
                            Name = publicHoliday.Name,
                            Date = publicHoliday.Date
                        };

                        modelBuilder.Entity<PublicHoliday>().HasData(publicHolidayData);
                    }
                }

                if (country.States != null)
                {
                    foreach (var state in country.States)
                    {
                        var countryStateData = new CountryState()
                        {
                            Id = ++countryStateId,
                            CountryId = countryId,
                            Name = state.Name,
                            Iso2Code = state.Iso2Code
                        };

                        modelBuilder.Entity<CountryState>().HasData(countryStateData);

                        if (state.PublicHolidays == null)
                        {
                            continue;
                        }

                        foreach (var publicHoliday in state.PublicHolidays)
                        {
                            var publicHolidayData = new PublicHoliday()
                            {
                                Id = ++publicHolidayId,
                                CountryStateId = countryStateId,
                                Name = publicHoliday.Name,
                                Date = publicHoliday.Date
                            };

                            modelBuilder.Entity<PublicHoliday>().HasData(publicHolidayData);
                        }
                    }
                }
            }
        }

        private List<Type> GetEnumsNamesForAssembly(string assemblyName, params string[] namespaceFilter)
        {
            var asmList = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var asm = asmList.Single(a => a.FullName.StartsWith(assemblyName));
            var query = asm.GetTypes().Where(x => x.IsEnum);
            if (namespaceFilter != null && namespaceFilter.Length > 0)
            {
                query = query.Where(x => namespaceFilter.Contains(x.Namespace));
            }

            return query.ToList();

            // Example for full info
            // var result = enums.Select(x => $"{x.Name},{string.Join(",", Enum.GetNames(x).Select(e => $"{e}={(int) Enum.Parse(x, e)}"))}");
        }

        public static byte[] GetTemplate(string path)
        {
            var basePath = System.IO.Path.GetDirectoryName(typeof(OlmaDbContext).Assembly.Location);
            return System.IO.File.ReadAllBytes(System.IO.Path.Combine(basePath, path));
        }

        public Expression<Func<TEntity, bool>> GetQueryFilterSecurity<TEntity>(Func<TEntity, bool> defaultFilter)
        {
            var isDplEmloyee = this._authData.GetUserRole() == UserRole.DplEmployee;
            if (isDplEmloyee)
            {
                return PredicateBuilder.New<TEntity>(true);
            }

            return PredicateBuilder.New<TEntity>(i => defaultFilter(i));
        }

        public void AddQueryFilterSecurity<TEntity>(ModelBuilder builder, Expression<Func<TEntity, bool>> defaultFilter)
            where TEntity : class
        {
            // employee filter
            var employeeFilter = PredicateBuilder.New<TEntity>(i => this._authData.GetUserRole() == UserRole.DplEmployee);
            var combined = employeeFilter.Or(defaultFilter);

            builder.Entity<TEntity>().AddQueryFilter(combined); ;
        }
    }

    public static class EntityBuilderExtensions
    {
        internal static void AddQueryFilter<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, Expression<Func<TEntity, bool>> expression)
            where TEntity : class
        {
            var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
            var expressionFilter = Microsoft.EntityFrameworkCore.Query.ReplacingExpressionVisitor.Replace(
                expression.Parameters.Single(), parameterType, expression.Body);

            var currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();
            if (currentQueryFilter != null)
            {
                var currentExpressionFilter = Microsoft.EntityFrameworkCore.Query.ReplacingExpressionVisitor.Replace(
                    currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
                expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
            }

            var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
            entityTypeBuilder.HasQueryFilter(lambdaExpression);
        }
    }

    public class LtmsAccoutingRecord
    {
        public string Id { get; set; }
        public int? BookingId { get; set; }
        public virtual Ltms.Bookings Booking { get; set; }

        public int? PostingRequestId { get; set; }
        public virtual PostingRequest PostingRequest { get; set; }
    }
}