using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.Fields.StandardFields;

namespace Dpl.B2b.BusinessLogic.Reporting.Extractors
{
    public class StandardFieldExtractor
        : FieldExtractor<StandardFieldInfo,XRLabel>
    {
        #region Constructors
        /// <summary>
        /// Initialisiert den LocalizableReportLabelExtractor.
        /// </summary>
        public StandardFieldExtractor(
            XtraReport report)
            : base(report)
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Erzeugt eine neue FieldInfo für das spezifizierte Label.
        /// </summary>
        protected override StandardFieldInfo CreateFieldInfo(XRLabel label)
        {
            return new StandardFieldInfo(label);
        }
        #endregion
    }
}
