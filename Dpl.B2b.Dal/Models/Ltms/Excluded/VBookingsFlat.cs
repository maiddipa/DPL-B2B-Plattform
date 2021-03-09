using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VBookingsFlat
    {
        public int Pid { get; set; }
        public int Tid { get; set; }
        public int Bid { get; set; }
        public short Lid { get; set; }
        public int Aid { get; set; }
        public string PRef { get; set; }
        public string TRef { get; set; }
        public string BRef { get; set; }
        public string PName { get; set; }
        public short PState { get; set; }
        public short TState { get; set; }
        public short PType { get; set; }
        public string TType { get; set; }
        public string BType { get; set; }
        public string PIntText { get; set; }
        public string PExtText { get; set; }
        public string ANumber { get; set; }
        public string AType { get; set; }
        public string AName { get; set; }
        public string CustomerNumber { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime Valuta { get; set; }
        public string IntDescription { get; set; }
        public int? CancellationId { get; set; }
        public DateTime BookingDate { get; set; }
        public string ExtDescription { get; set; }
        public string Lt { get; set; }
        public int? Quantity { get; set; }
        public bool IncludeInBalance { get; set; }
        public bool Matched { get; set; }
        public string MatchedUser { get; set; }
        public DateTime? MatchedTime { get; set; }
        public string AccountDirection { get; set; }
        public DateTime RowModified { get; set; }
        public string PCreateUser { get; set; }
        public DateTime PCreateTime { get; set; }
        public string TCreateUser { get; set; }
        public DateTime TCreateTime { get; set; }
        public string BCreateUser { get; set; }
        public DateTime BCreateTime { get; set; }
        public string PUpdateUser { get; set; }
        public DateTime? PUpdateTime { get; set; }
        public string TUpdateUser { get; set; }
        public DateTime? TUpdateTime { get; set; }
        public string BUpdateUser { get; set; }
        public DateTime? BUpdateTime { get; set; }
        public DateTime? PChangedTime { get; set; }
        public DateTime? TChangedTime { get; set; }
        public DateTime? BChangedTime { get; set; }
        public int POptimisticLock { get; set; }
        public int TOptimisticLock { get; set; }
        public int BOptimisticLock { get; set; }
    }
}
