using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.Fields.AdditionalFields;

namespace Dpl.B2b.BusinessLogic.Reporting.Extractors
{
    public class AdditionalFieldValueExtractor
        : FieldExtractor<AdditionalFieldValueInfo,XRLabel>
    {
        #region Constructors
        /// <summary>
        /// Initialisiert den LocalizableReportLabelExtractor.
        /// </summary>
        public AdditionalFieldValueExtractor(
            XtraReport report)
            : base(report)
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Erzeugt eine neue FieldInfo für das spezifizierte Label.
        /// </summary>
        protected override AdditionalFieldValueInfo CreateFieldInfo(XRLabel label)
        {
            return new AdditionalFieldValueInfo(label);
        }
        #endregion
    }
}
