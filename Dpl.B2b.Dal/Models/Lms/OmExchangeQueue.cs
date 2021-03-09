using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class OmExchangeQueue
    {
        public int Id { get; set; }
        public int Omtype { get; set; }
        public int Omtask { get; set; }
        public int Lmsstatus { get; set; }
        public int Omstatus { get; set; }
        public int? AvailabilityId { get; set; }
        public int? DeliveryId { get; set; }
        public int? Avail2DeliId { get; set; }
        public string OrderManagementId { get; set; }
        public string Comment { get; set; }
        public int ArticleId { get; set; }
        public int QualityId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public string PreferredPlace { get; set; }
        public string PreferredTime { get; set; }
        public string Abnumber { get; set; }
        public string Lsnumber { get; set; }
        public string Aunumber { get; set; }
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime? ExecutionPeriodBegin { get; set; }
        public DateTime? ExecutionPeriodEnd { get; set; }
    }
}
