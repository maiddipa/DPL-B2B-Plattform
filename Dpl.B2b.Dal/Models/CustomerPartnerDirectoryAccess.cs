namespace Dpl.B2b.Dal.Models
{
    public class CustomerPartnerDirectoryAccess : OlmaAuditable
    {
        public int Id { get; set; }

        public int CustomerPartnerId { get; set; }
        public virtual CustomerPartner Partner { get; set; }

        public int DirectoryId { get; set; }
        public virtual PartnerDirectory Directory { get; set; }
    }
}