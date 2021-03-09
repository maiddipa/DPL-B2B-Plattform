using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;

namespace Dpl.B2b.BusinessLogic.Reporting.Fields.AdditionalFields
{
    public class AdditionalFieldValueCheckboxInfo
        : AdditionalFieldInfo<XRCheckBox>
    {
        #region Fields
        private static FieldBindingInfo _fieldBindingInfo;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public AdditionalFieldValueCheckboxInfo(XRCheckBox checkbox)
            : base(checkbox)
        { }

        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public AdditionalFieldValueCheckboxInfo(
            XRCheckBox checkbox,
            LocalizedReportDataSet dataSource)
            : base(checkbox, dataSource)
        { }
        #endregion

        #region Properties
        protected override FieldBindingInfo FieldBindingInfo
        {
            get
            {
                if (_fieldBindingInfo == null)
                {
                    _fieldBindingInfo = new FieldBindingInfo(
                        "AdditionalFieldValue",
                        "Checked",
                        "Tag");
                }
                return _fieldBindingInfo;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Legt das DataBinding der Text-Eigenschaft des Feldes fest,
        /// unter nutzung der SourceTable, dem FieldName udn der FieldID.
        /// </summary>
        public override void SetDataBinding(
            string sourceTable,
            string fieldName)
        {
            // Property-Binding hinzufügen
            Control.DataBindings.Add(
                "Checked",
                null,
                string.Format(
                    "{0}.{1}{2}",
                    sourceTable,
                    fieldName,
                    FieldId));

            // Default Werte setzen
            Control.Checked = false;
            Control.Text = "";
        }

        protected override void BindValueProperty(object value)
        {
            if (((string)value).Trim() == "1")
            {
                Control.Checked = true;
            }
            else
            {
                Control.Checked = false;
            }
        }

        protected override object GetCellDataBindingValue(object cell)
        {
            if (cell.ToString().Trim() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
