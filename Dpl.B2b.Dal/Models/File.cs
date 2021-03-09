using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string InternalFullPathAndName { get; set; }
    }
}
