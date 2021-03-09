using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Bookings
    {

        public int Id { get; set; }
        public string BookingTypeId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string ExtDescription { get; set; }
        public int? Quantity { get; set; }
        public short ArticleId { get; set; }
        public short QualityId { get; set; }
        public bool IncludeInBalance { get; set; }
        public bool Matched { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string MatchedUser { get; set; }
        public DateTime? MatchedTime { get; set; }
        public string AccountDirection { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int OptimisticLockField { get; set; }
        public DateTime? ChangedTime { get; set; }
        public DateTime RowModified { get; set; }
        public bool Computed { get; set; }
        public Guid RowGuid { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Articles Article { get; set; }
        public virtual BookingTypes BookingType { get; set; }
        public virtual Qualities Quality { get; set; }
        public virtual Transactions Transaction { get; set; }
        public virtual ICollection<Fees> Fees { get; set; }
        public virtual ICollection<ReportBookings> ReportBookings { get; set; }

        public virtual Dal.Models.PostingAccount OlmaPostingAccount { get; set; }
    }
}
