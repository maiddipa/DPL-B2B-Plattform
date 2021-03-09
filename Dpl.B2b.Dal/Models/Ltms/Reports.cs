using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Reports
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReportName { get; set; }
        public string FileName { get; set; }
        public byte[] ReportFile { get; set; }
        public string FilePath { get; set; }
        public string ReportUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Quarter { get; set; }
        public int? Month { get; set; }
        public int? AddressId { get; set; }
        public string CustomerNumber { get; set; }
        public string AdditionalParams { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string ReportStateId { get; set; }
        public int? Year { get; set; }
        public string MatchedUser { get; set; }
        public DateTime? MatchedTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int OptimisticLockField { get; set; }
        public string ReportTypeId { get; set; }
        public int PrimaryAccountId { get; set; }
        public long? DocId { get; set; }
        public long? GdocId { get; set; }

        public virtual Accounts PrimaryAccount { get; set; }
        public virtual ReportTypes ReportType { get; set; }
        public virtual ICollection<ReportBookings> ReportBookings { get; set; }
    }
}
