namespace Dpl.B2b.Dal.Models
{
    public class PartnerDirectoryAccess : OlmaAuditable
    {
        public int Id { get; set; }

        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; }

        public int DirectoryId { get; set; }
        public virtual PartnerDirectory Directory { get; set; }
    }
}