using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsQuali2pallet
    {
        public int QualityId { get; set; }
        public int PalletTypeId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }

        public virtual LmsPallettype PalletType { get; set; }
        public virtual LmsQuality Quality { get; set; }
    }
}
