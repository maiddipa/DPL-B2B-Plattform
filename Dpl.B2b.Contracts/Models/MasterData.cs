using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{
    public class MasterData
    {
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<DocumentState> DocumentStates { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        public IEnumerable<LoadCarrier> LoadCarriers { get; set; }
        public IEnumerable<VoucherReasonType> VoucherReasonTypes { get; set; }
    }
}
