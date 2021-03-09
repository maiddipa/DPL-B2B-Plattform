using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{
    public class DplEmployeeCustomer
    {
        public DplEmployeeCustomerSelectionEntryType Type { get; set; }

        public string DisplayName { get; set; }
        public string DisplayNameLong { get; set; }
        public int CustomerId { get; set; }
        public int? DivisionId { get; set; }
        public int? PostingAccountId { get; set; }

        public Address Address { get; set; }
    }

    public enum DplEmployeeCustomerSelectionEntryType
    {
        Customer = 0,
        Division = 1,
        PostingAccount = 2
    }
}
