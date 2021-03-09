using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{
    public class VoucherSyncRequest
    {
        public VoucherCreateSyncRequest VoucherCreateSyncRequest { get; set; }

        public VoucherCancelSyncRequest VoucherCancelSyncRequest { get; set; }

        public VoucherRollbackSyncRequest VoucherRollbackSyncRequest { get; set; }
    }

    public abstract class VoucherSyncRequestBase
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
    }

    public class VoucherCreateSyncRequest : VoucherSyncRequestBase
    {
        public int Type { get; set; }
        public string VoucherReason { get; set; }
        public string Recipient { get; set; }
        public string Note { get; set; }

        public int RefLtmsIssuerAccountId { get; set; }

        public int? RefLtmsRecipientAccountId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DateOfExpiry { get; set; }

        public bool ProcurementLogistics { get; set; }

        public Guid? RefLtmsTransactionRowGuid { get; set; }

        public virtual ICollection<VoucherRequestPosition> Positions { get; set; }
    }

    public class VoucherRequestPosition
    {
        public int RefLtmsPalletId { get; set; }
        public int Quantity { get; set; }
    }

    public class VoucherCancelSyncRequest :VoucherSyncRequestBase
    {
        public string CancellationReason { get; set; }
    }

    public class VoucherRollbackSyncRequest : VoucherSyncRequestBase
    {
    }
}
