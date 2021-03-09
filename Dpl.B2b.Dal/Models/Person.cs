using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public class Person : OlmaAuditable
    {
        public int Id { get; set; }

        public PersonGender Gender { get; set; }

        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }

        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}