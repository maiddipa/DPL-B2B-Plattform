using System.Collections.Generic;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{

    public class UserGroupCreateRequest
    {
        public string Name { get; set; }

        public bool IsSystemGroup { get; set; }

        public bool IsHidden { get; set; }

        public int? OrganizationId { get; set; }

        public int? CustomerId { get; set; }

        public int? DivisionId { get; set; }
        public List<GroupPermission> Permissions { get; set; }
    }
    public class UserGroupListItem
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public bool IsSystemGroup { get; set; }

        public List<GroupPermission> Permissions { get; set; }

    }

    public class UserGroupMembership
    {
        public UserGroupListItem UserGroup { get; set; }
        public List<UserListItem> Users { get; set; }
    }

    public class UserGroupUpdateMembershipRequest
    {
        public List<UserListItem> Users { get; set; }
    }
    public class UserGroupUpdatePermissionsRequest
    {
        public string GroupName { get; set; }
        public List<GroupPermission> Permissions { get; set; }
    }

    public class UserGroupsSearchRequest
    {
        public int? CustomerId { get; set; }

        public int? OrganizationId { get; set; }

        public int? CustomerDivisionId { get; set; }

        public int? PostingAccountId { get; set; }
    }

    public class GroupPermission
    {
        public PermissionResourceType Resource { get; set; }
        public int ReferenceId { get; set; }
        public string Action { get; set; }
    }

}