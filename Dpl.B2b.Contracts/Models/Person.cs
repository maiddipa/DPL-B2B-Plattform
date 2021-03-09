using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{
    public class Person
    {
        public int Id { get; set; }

        public PersonGender Gender { get; set; }

        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
    }
}
