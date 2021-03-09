using System;

namespace Dpl.B2b.Contracts
{
    public interface IAuditable
    {
        int? CreatedById { get; set; }
        DateTime? CreatedAt { get; set; }

        int? ChangedById { get; set; }
        DateTime? ChangedAt { get; set; }
        
        int? DeletedById { get; set; }
        DateTime? DeletedAt { get; set; }

        bool IsDeleted { get; set; }
    }

    public interface IAuditable<TUser> : IAuditable
    {
        TUser CreatedBy { get; set; }

        TUser ChangedBy { get; set; }

        TUser DeletedBy { get; set; }
    }

    public class Auditable<TUser> : IAuditable<TUser>
    {
        public int? CreatedById { get; set; }
        public virtual TUser CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int? ChangedById { get; set; }        
        public virtual TUser ChangedBy { get; set; }
        public DateTime? ChangedAt { get; set; }

        public int? DeletedById { get; set; }
        public virtual TUser DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
