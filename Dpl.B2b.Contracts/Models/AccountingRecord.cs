using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class AccountingRecordProcess
    {
        public int Id { get; set; }
        public IEnumerable<AccountingRecordTransaction> Transactions { get; set; }
    }

    public class AccountingRecordTransaction
    {
        public IEnumerable<AccountingRecord> Records { get; set; }
    }

    // public class LtmsBooking
    // {
    //     public int Id { get; set; }
    //     public string ReferenceNumber { get; set; }
    //     public string Type { get; set; }
    //     public DateTime Date { get; set; }
    //     public string ExtDescription { get; set; }
    //     public int LoadCarrierId { get; set; }
    //     public int? Quantity { get; set; }
    // }
    
    public class AccountingRecord
    {
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public int ProcessId { get; set; }
        public string ReferenceNumber { get; set; }
        public int PostingAccountId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string LoadCarrierTypeName { get; set; }
        public string QualityName { get; set; }
        public DateTime Date { get; set; }
        public string ExtDescription { get; set; }
        public int LoadCarrierId { get; set; }
        public int? Quantity { get; set; }
        public int? Charge { get; set; }
        public int? Credit { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public EmployeeNote DplNote { get; set; }
        public bool HasDplNote { get; set; }
    }

    public enum AccountingRecordType
    {
        ABH = 1,
        ABHL = 2,
        AKD = 3,
        AKK = 4,
        ANL = 5,
        ANLDEP = 6,
        ANLDPK = 7,
        AUSDPG = 8,
        AUSOPG = 9,
        DIFF = 10,
        DQ = 11,
        EINOPG = 12,
        ENTSOR = 13,
        FSD = 14,
        FSI = 15,
        GUTDPG = 16,
        GUTOPG = 17,
        K = 18,
        KAUF = 19,
        KBU = 20,
        KDPG = 21,
        KKD = 22,
        KKK = 23,
        KUA = 24,
        KUABA = 25,
        KUABZ = 26,
        KUFSA = 27,
        KUFSZ = 28,
        KUZ = 29,
        KZA = 30,
        LIPAK = 31,
        MAN = 32,
        PAT = 33,
        PM = 34,
        POOL = 35,
        QT = 36,
        REIN = 37,
        REP = 38,
        REPAUF = 39,
        REPKND = 40,
        REPQU = 41,
        RETOUR = 42,        
        ROH = 43,                       
        RPT = 44,
        SCHULD = 45,
        SORT = 46,
        STORNO = 47,
        SYS = 48,
        TADIFF = 49,
        U = 50,
        VERK = 51,
        VERLUS = 52,
        VERS = 53,
        VERSA = 54,
        WMP = 55,
        GUTDDG = 56,
        AUSDDG = 57,
        GUTDBG = 58,
        AUSDBG = 59
    }

    public enum AccountingRecordStatus
    {
        // Bookings related status
        //Pending = 0,
        //Confirmed = 1
        Provisional = 0, //vorläufig
        Uncoordinated = 1, // Unkoordiniert
        InCoordination = 2, // InCoordination
        Coordinated = 3, // ? ~Abgestimmt?

        // Posting Request related status
        Pending = 10,
        Canceled = 11
    }

    public class AccountingRecordsSearchRequest
    {
        public int RefLtmsAccountId { get; set; }
        public int RefLtmsArticleId { get; set; }
        public AccountingRecordStatus? Status { get; set; }
    }

    public enum AccountingRecordsSearchRequestSortOptions
    {
        ReferenceNumber = 0,
        Date = 1,
        Status = 2,
        ExtDescription = 3,
        LoadCarrierName = 4,
        Charge = 5,
        Credit = 6,
        DplNote
    }
}
