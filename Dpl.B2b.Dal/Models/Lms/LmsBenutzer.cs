using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsBenutzer
    {
        public int Id { get; set; }
        public string Benutzername { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public int GruppeId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
        public string Email { get; set; }
    }
}
