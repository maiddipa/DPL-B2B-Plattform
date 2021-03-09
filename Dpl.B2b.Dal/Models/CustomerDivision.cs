using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dpl.B2b.Dal.Models
{
    public class CustomerDivision : OlmaAuditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public int? PostingAccountId { get; set; }

        public virtual PostingAccount PostingAccount { get; set; }

        public int? DefaultLoadingLocationId { get; set; }
        public virtual LoadingLocation DefaultLoadingLocation { get; set; }

        public virtual ICollection<LoadingLocation> LoadingLocations { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<CustomerDivisionDocumentSetting> DocumentSettings { get; set; }
    }
}