using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsPallettype
    {
        public LmsPallettype()
        {
            LmsQuali2pallet = new HashSet<LmsQuali2pallet>();
        }

        public int PalletTypeId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string ArtNrShortName { get; set; }
        public bool IsMixType { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }

        public virtual ICollection<LmsQuali2pallet> LmsQuali2pallet { get; set; }
    }
}
