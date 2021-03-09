using Microsoft.EntityFrameworkCore;

namespace Dpl.B2b.Dal.Models
{
   
    public class PartnerPreset
    {
        public int Id { get; set; }
        public ExpressCodePresetType Type { get; set; }

        public int ExpressCodeId { get; set; }
        public virtual ExpressCode ExpressCode { get; set; }

        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
    }

    public enum ExpressCodePresetType
    {
        Default = 0,
        Recipient = 1,
        Supplier = 2,
        Shipper = 3,
        SubShipper = 4
    }
}