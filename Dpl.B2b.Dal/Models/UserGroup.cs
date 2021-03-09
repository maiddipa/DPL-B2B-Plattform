using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public interface IUserGroup
    {
         int Id { get; set; }

         UserGroupType Type { get; set; }

         string Name { get; set; }

         bool IsSystemGroup { get; set; }

         bool IsHidden { get; set; }

         ICollection<UserUserGroupRelation> Users { get; set; }

         ICollection<Permission> Permissions { get; set; }
    }

    public abstract class UserGroup : OlmaAuditable, IUserGroup
    {
        public int Id { get; set; }

        public UserGroupType Type { get; set; }

        public string Name { get; set; }

        public bool IsSystemGroup { get; set; }

        public bool IsHidden { get; set; }

        public virtual ICollection<UserUserGroupRelation> Users { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }

    public enum UserGroupType
    {
        Organization = 0,
        Customer = 1,
        CustomerDivision = 2
    }

    public class OrganizationUserGroup : UserGroup
    {
        public OrganizationUserGroup()
        {
            Type = UserGroupType.Organization;
        }
        public int OrganizationId { get; set; }
        public virtual Organization Customer { get; set; }
    }

    public class CustomerUserGroup : UserGroup
    {
        public CustomerUserGroup()
        {
            Type = UserGroupType.Customer;
        }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public class CustomerDivisionUserGroup : UserGroup
    {
        public CustomerDivisionUserGroup()
        {
            Type = UserGroupType.CustomerDivision;
        }
        public int CustomerDivisionId { get; set; }
        public virtual CustomerDivision CustomerDivisionDivision { get; set; }
    }

    ////Many-to-many relationships need an entity class for join table. See:  https://docs.microsoft.com/de-de/ef/core/modeling/relationships?tabs=fluent-api
    //public class UserGroupPermissionRelation : OlmaAuditable
    //{
    //    public int Id { get; set; }

    //    public int UserGroupId { get; set; }
    //    public UserGroup UserGroup { get; set; }

    //    public int PermissionId { get; set; }
    //    public Permission Permission { get; set; }

    //}
}