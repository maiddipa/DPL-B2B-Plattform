using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Upn { get; set; }
        public UserRole Role { get; set; }
        public Person Person { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<PostingAccount> PostingAccounts { get; set; }
        public IEnumerable<UserPermission> Permissions { get; set; }

        public Newtonsoft.Json.Linq.JObject Settings { get; set; }
    }

    // this class is used to store user information in the distributed cache
    public class CachedUser
    {
        public int Id { get; set; }
        public string Upn { get; set; }

        public IEnumerable<UserPermission> Permissions { get; set; }

        public IEnumerable<int> OrganizationIds { get; set; }
        public IEnumerable<int> CustomerIds { get; set; }
        public IEnumerable<int> CustomerDivisionIds { get; set; }
        public IEnumerable<int> PostingAccountIds { get; set; }
    }

    public class UserPermission : IEquatable<UserPermission>
    {
        public PermissionResourceType Resource { get; set; }
        public int ReferenceId { get; set; }
        public ResourceAction Action { get; set; }

        #region Equality

        public bool Equals(UserPermission other)
        {
            if (other == null)
            {
                return false;
            }


            return Resource == other.Resource &&
                   ReferenceId == other.ReferenceId &&
                   string.Equals(Action, other.Action);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as UserPermission);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = ((hashCode * 397) ^ ReferenceId);
                var ActionHashCode = Action.GetHashCode();
                hashCode = (hashCode * 397) ^ ActionHashCode;
                hashCode =
                    (hashCode * 397) ^ Resource.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }

    public class UserCreateRequest
    {
        // User Properties
        public int CustomerId { get; set; }
        public string Upn { get; set; }
        public UserRole Role { get; set; }

        //Person Properties
        public PersonGender Gender { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
    }
    
    public class UserUpdateRequest
    {
        public int? Id { get; set; }
        public string Values { get; set; }
    }
    
    public class UserListItem
    {
        public int Id { get; set; }
        public string Upn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ChangedAt { get; set; }
        public bool Locked { get; set; }
        public DateTime? FirstLoginDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string Role { get; set; }
        public int PersonId { get; set; }
        public string Gender { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FirstLoginPassword { get; set; }
    }
    
    public class UserResetPasswordResult
    {
        public string NewPassword { get; set; }
    }
    
    public class UserResetPasswordRequest
    {
        public int UserId { get; set; }
    }

    public class UserAddToCustomerRequest
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }
    }
    public class RemoveFromCustomerRequest
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }
    }

    public class UsersSearchRequest 
    {
        public int? CustomerId { get; set; }

        public int? OrganizationId { get; set; }

        public int? DivisionId { get; set; }

    }
    public class UsersByOrganizationRequest 
    {
        public int OrganizationId { get; set; }

        public int? ExceptCustomerId { get; set; }
    }

    public class UsersLockRequest
    {
        public int? Id { get; set; }
        public bool? Locked { get; set; }
    }

    public class UserListRequest : PaginationRequest
    {
        public int? CustomerId { get; set; }
        public int? OrganizationId { get; set; }

    }
}
