using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.Fields.AdditionalFields;

namespace Dpl.B2b.BusinessLogic.Reporting.Extractors
{
    public class AdditionalFieldLabelExtractor
        : FieldExtractor<AdditionalFieldLabelInfo,XRLabel>
    {
        #region Constructors
        /// <summary>
        /// Initialisiert den LocalizableReportLabelExtractor.
        /// </summary>
        public AdditionalFieldLabelExtractor(
            XtraReport report)
            : base(report)
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Erzeugt eine neue FieldInfo für das spezifizierte Label.
        /// </summary>
        protected override AdditionalFieldLabelInfo CreateFieldInfo(XRLabel label)
        {
            return new AdditionalFieldLabelInfo(label);
        }
        #endregion
    }
}
