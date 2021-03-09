using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsConfig
    {
        public bool LmsGesperrt { get; set; }
        public int ProgAktuelleVersion1 { get; set; }
        public int ProgAktuelleVersion2 { get; set; }
        public int ProgAktuelleVersion3 { get; set; }
        public int ProgAktuelleVersion4 { get; set; }
        public bool ProgVersionErzwingen { get; set; }
    }
}
