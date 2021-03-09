using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{
    public class CustomerPartnerImportRecord
    {
        public string Reference { get; set; }
        public string ReferenceOld { get; set; }
        public PartnerType Type { get; set; }
        public string CompanyName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CountryIso3Code { get; set; }
    }
}
