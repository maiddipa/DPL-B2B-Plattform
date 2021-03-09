using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;
using Dpl.B2b.BusinessLogic.Reporting.Extractors;
using Dpl.B2b.BusinessLogic.Reporting.Fields.StandardFields;

namespace Dpl.B2b.BusinessLogic.Reporting
{
    public class CustomReport : XtraReport
    {
        private StandardFieldExtractor _standardFieldExtractor;
        private AdditionalFieldLabelExtractor _additionalFieldLabelExtractor;
        private AdditionalFieldValueExtractor _additionalFieldValueExtractor;
        private AdditionalFieldValueCheckboxExtractor _additionalFieldValueCheckboxExtractor;


        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LocalizedReportDataSet ReportDataSet { get; set; }
        

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected StandardFieldExtractor StandardFieldExtractor
        {
            get
            {
                if (_standardFieldExtractor == null)
                {
                    _standardFieldExtractor
                        = new StandardFieldExtractor(
                            this);
                }
                return _standardFieldExtractor;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected AdditionalFieldLabelExtractor AdditionalFieldLabelExtractor
        {
            get
            {
                if (_additionalFieldLabelExtractor == null)
                {
                    _additionalFieldLabelExtractor
                        = new AdditionalFieldLabelExtractor(
                            this);
                }
                return _additionalFieldLabelExtractor;
            }
        }

        /// <summary>
        /// Liefert den AdditionalFieldValueExtractor.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected AdditionalFieldValueExtractor AdditionalFieldValueExtractor
        {
            get
            {
                if (_additionalFieldValueExtractor == null)
                {
                    _additionalFieldValueExtractor
                        = new AdditionalFieldValueExtractor(
                            this);
                }
                return _additionalFieldValueExtractor;
            }
        }

        /// <summary>
        /// Liefert den AdditionalFieldValueExtractor.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected AdditionalFieldValueCheckboxExtractor AdditionalFieldValueCheckboxExtractor
        {
            get
            {
                if (_additionalFieldValueCheckboxExtractor == null)
                {
                    _additionalFieldValueCheckboxExtractor
                        = new AdditionalFieldValueCheckboxExtractor(
                            this);
                }
                return _additionalFieldValueCheckboxExtractor;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void RebindLocalizedFields()
        {
            foreach (StandardFieldInfo reportField
                in StandardFieldExtractor.LocalizedFields)
            {
                reportField.DataSource = ReportDataSet;
                reportField.Rebind();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void LoadBoundLabelTexts()
        {
            foreach (StandardFieldInfo reportField
                in StandardFieldExtractor.BoundLocalizedFields)
            {
                reportField.DataSource = ReportDataSet;
                reportField.LoadValueIntoField();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<StandardFieldInfo> GetLocalizedFields()
        {
            return StandardFieldExtractor.LocalizedFields;
        }
    }
}
