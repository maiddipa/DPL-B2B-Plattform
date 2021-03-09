using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class TransactionDetails
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public int OptimisticLockField { get; set; }
        public DateTime? FieldDate { get; set; }
        public int? FieldInt { get; set; }
        public decimal? FieldDecimal { get; set; }
        public bool? FieldBool { get; set; }
        public Guid? FieldGuid { get; set; }
        public string FieldText { get; set; }
        public string FieldType { get; set; }

        public virtual Transactions Transaction { get; set; }
    }
}
