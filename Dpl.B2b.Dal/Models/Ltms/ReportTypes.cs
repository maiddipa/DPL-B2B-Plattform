using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ReportTypes
    {
        public string Id { get; set; }
        public string ReportName { get; set; }
        public string NamePattern { get; set; }
        public string FileNamePattern { get; set; }
        public string DropDownText { get; set; }
        public string Description { get; set; }
        public string BarcodePrefix { get; set; }
        public bool HasToBeReconcilied { get; set; }
        public bool CanBeStored { get; set; }
        public bool IsInvoice { get; set; }
        public bool? DefaultMatchedFilter { get; set; }
        public string DefaultIncludeAssociationsOfType { get; set; }
        public bool? ArticleIntrinsic { get; set; }
        public bool? IncludeAllArticles { get; set; }
        public string FileFormat { get; set; }
        public virtual ICollection<Reports> Reports { get; set; }
    }
}
