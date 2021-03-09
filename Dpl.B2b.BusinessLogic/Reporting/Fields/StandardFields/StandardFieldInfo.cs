using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;

namespace Dpl.B2b.BusinessLogic.Reporting.Fields.StandardFields
{
    public class StandardFieldInfo
        : FieldInfo<XRLabel>
    {
        #region Fields
        private static FieldBindingInfo _fieldBindingInfo;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public StandardFieldInfo(XRLabel label)
            : base(label)
        { }

        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public StandardFieldInfo(
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
                        "ReportLabel",
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
            Control.Text = (string) value;
        }

        protected override object GetCellDataBindingValue(object cell)
        {
            return cell.ToString();
        }

        #endregion
    }
}
