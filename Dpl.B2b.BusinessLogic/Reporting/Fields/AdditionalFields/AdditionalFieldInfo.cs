using System;
using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;

namespace Dpl.B2b.BusinessLogic.Reporting.Fields.AdditionalFields
{
    public abstract class AdditionalFieldInfo<T>
        : FieldInfo<T> where T : XRFieldEmbeddableControl
    {
        #region Constructor
        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public AdditionalFieldInfo(T control)
            : base(control)
        { }

        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public AdditionalFieldInfo(
            T control,
            LocalizedReportDataSet dataSource)
            : base(control, dataSource)
        { }
        #endregion

        #region Properties
        /// <summary>
        /// Liefert, oder setzt die Datenbank-ID des Feldes.
        /// </summary>
        public long FieldId
        {
            get
            {
                object tag = Control.Tag;
                if (tag != null)
                {
                    var fieldId = tag.ToString();
                    if (!string.Empty.Equals(fieldId))
                    {
                        return Convert.ToInt64(fieldId);
                    }
                }
                return 0;
            }
            set
            {
                Control.Tag = value;
            }
        }

        /// <summary>
        /// Liefert, ob die FieldID gültig ist.
        /// </summary>
        public bool IsValidFieldId => (FieldId != 0);

        protected override FieldBindingInfo FieldBindingInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Legt das DataBinding der Text-Eigenschaft des Feldes fest,
        /// unter nutzung der SourceTable, dem FieldName udn der FieldID.
        /// </summary>
        public virtual void SetDataBinding(
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

            // Defautl Text setzen
            Control.Text = fieldName;
        }

        #endregion
    }
}
