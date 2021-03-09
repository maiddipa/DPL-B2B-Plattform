using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.Fields.AdditionalFields;

namespace Dpl.B2b.BusinessLogic.Reporting.Extractors
{
    public class AdditionalFieldValueCheckboxExtractor
        : FieldExtractor<AdditionalFieldValueCheckboxInfo,XRCheckBox>
    {
        #region Constructors
        /// <summary>
        /// Initialisiert den LocalizableReportLabelExtractor.
        /// </summary>
        public AdditionalFieldValueCheckboxExtractor(
            XtraReport report)
            : base(report)
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Erzeugt eine neue FieldInfo für das spezifizierte Label.
        /// </summary>
        protected override AdditionalFieldValueCheckboxInfo CreateFieldInfo(XRCheckBox checkbox)
        {
            return new AdditionalFieldValueCheckboxInfo(checkbox);
        }
        #endregion
    }
}
