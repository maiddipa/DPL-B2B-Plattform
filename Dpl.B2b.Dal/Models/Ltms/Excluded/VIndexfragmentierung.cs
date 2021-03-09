using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VIndexfragmentierung
    {
        public string TabellenName { get; set; }
        public string SchemaName { get; set; }
        public string IndexName { get; set; }
        public decimal? Fragmentierung { get; set; }
        public string Erforderlich { get; set; }
    }
}
