using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsSight
    {
        public int SightId { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string Sort { get; set; }
        public byte SightType { get; set; }
        public bool? Global { get; set; }
        public bool? Gruppe { get; set; }
        public string CreatedBy { get; set; }
        public int? CreatedByGroup { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
