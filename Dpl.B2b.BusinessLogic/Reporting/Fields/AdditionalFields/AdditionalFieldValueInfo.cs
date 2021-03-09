using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;

namespace Dpl.B2b.BusinessLogic.Reporting.Fields.AdditionalFields
{
    public class AdditionalFieldValueInfo
        : AdditionalFieldInfo<XRLabel>
    {
        #region Fields
        private static FieldBindingInfo _fieldBindingInfo;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public AdditionalFieldValueInfo(XRLabel label)
            : base(label)
        { }

        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public AdditionalFieldValueInfo(
            XRLabel label,
            LocalizedReportDataSet dataSource)
            : base(label, dataSource)
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
                        "Text",
                        "Tag");
                }
                return _fieldBindingInfo;
            }
        }
        #endregion

        #region Methods

        protected override void BindValueProperty(object value)
        {
            Control.Text = (string)value;
        }

        protected override object GetCellDataBindingValue(object cell)
        {
            return cell.ToString();
        }

        /// <summary>
        /// Legt das DataBinding der Text-Eigenschaft des Feldes fest,
        /// unter nutzung der SourceTable, dem FieldName udn der FieldID.
        /// </summary>
        public override void SetDataBinding(
            string sourceTable,
            string fieldName)
        {
            // Text-Property-Binding hinzufügen
            Control.DataBindings.Add(
                "Text",
                null,
                string.Format(
                    "{0}.{1}{2}",
                    sourceTable,
                    fieldName,
                    FieldId));

            // Default Text setzen
            Control.Text = fieldName + @"Value";
        }

        #endregion
    }
}
