using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class AccountHierarchy
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public Geometry Hid { get; set; }
        public string TreeText { get; set; }
        public string PathById { get; set; }
        public short? TreeLevel { get; set; }
        public int? RowNumber { get; set; }
        public int? Lft { get; set; }
        public int? Rgt { get; set; }
        public bool HasChildren { get; set; }

        public virtual Accounts IdNavigation { get; set; }
    }
}
