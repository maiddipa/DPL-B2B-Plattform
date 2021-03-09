using Dpl.B2b.Common.Enumerations;


namespace Dpl.B2b.Dal.Models
{
    // TODO Need to think about generalizing permissions instead of boolean flags
    public class Permission : OlmaAuditable
    {
        public int Id { get; set; }

        public PermissionType Type { get; set; }

        public PermissionScope Scope { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public int? UserGroupId { get; set; }
        public virtual UserGroup UserGroup { get; set; }

        public PermissionResourceType Resource { get; set; }

        public int? ReferenceId { get; set; }

        // CanCreateOrder
        public string Action { get; set; }
    }

    public enum PermissionType
    {
        Deny = 0,
        Allow = 1
    }

    public enum PermissionScope
    {
        User = 0,
        Group = 1,
        Collection = 2
    }
}