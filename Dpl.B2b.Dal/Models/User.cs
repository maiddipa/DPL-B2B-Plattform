using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class User : OlmaAuditable
    {
        public int Id { get; set; }

        public string Upn { get; set; }

        public UserRole Role { get; set; }

        //TODO Add Organization
        public int? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int UserSettingId { get; set; }
        public virtual UserSetting UserSettings { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }


        public int PersonId { get; set; }
        public virtual Person Person { get; set; }


        public bool Locked { get; set; }
        public bool OrderNotificationsEnabled { get; set; }

        public DateTime? FirstLoginDate { get; set; }
        public DateTime? LastLoginDate { get; set; }


        public virtual ICollection<Submission> Submissions { get; set; }
        
        public virtual ICollection<UserUserGroupRelation> UserGroups { get; set; }

        public virtual ICollection<UserCustomerRelation> Customers { get; set; }
    }


    //Many-to-many relationships need an entity class for join table. See:  https://docs.microsoft.com/de-de/ef/core/modeling/relationships?tabs=fluent-api
    public class UserUserGroupRelation 
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int UserGroupId { get; set; }
        public virtual UserGroup UserGroup { get; set; }

    }
    
    public class UserCustomerRelation
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}