using System.Collections.Generic;
using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.Fields;

namespace Dpl.B2b.BusinessLogic.Reporting.Extractors
{
    public abstract class FieldExtractor<T,TControl>
        where T : IFieldInfo<TControl> where TControl : XRFieldEmbeddableControl
    {
        #region Constructors
        /// <summary>
        /// Initialisiert den LocalizableReportLabelExtractor.
        /// </summary>
        public FieldExtractor(
            XtraReport report)
        {
            Report = report;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Lieferrt den ReportFieldManager.
        /// </summary>
        public XtraReport Report
        {
            get;
            set;
        }

        /// <summary>
        /// Liefert eine Enumeration mit allen Labels mit DataBindings
        /// an die virtuelle Tabelle ReprotLabel.
        /// </summary>
        public List<T> BoundLocalizedFields
        {
            get
            {
                List<T> boundLocalizedFields = new List<T>();

                foreach (T reportField in AllFields)
                {
                    // Ist das Label an die ReportLabel-Tabelle gebunden?
                    if (reportField.IsPropertyBoundOnThisDataTable)
                    {
                        boundLocalizedFields.Add(reportField);
                    }
                }

                return boundLocalizedFields;
            }
        }

        /// <summary>
        /// Extrahiert alle gebundenen und ungebundenen Labels, die mehrsprachig sind.
        /// </summary>
        public List<T> LocalizedFields
        {
            get
            {
                List<T> localizedFields = new List<T>();

                foreach (T reportField in AllFields)
                {
                    // Ist das Label an die ReportLabel-Tabelle gebunden?
                    if (reportField.IsUnbound
                        || reportField.IsPropertyBoundOnThisDataTable)
                    {
                        localizedFields.Add(reportField);
                    }
                }

                return localizedFields;
            }
        }

        /// <summary>
        /// Extrahiert alle Labels.
        /// </summary>
        protected List<T> AllFields
        {
            get
            {
                List<T> extractedFields = new List<T>();

                foreach (Band band in Report.Bands)
                {
                    ExtractFields(band, extractedFields);
                }

                return extractedFields;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Erzeugt eine neue FieldInfo für das spezifizierte Label.
        /// </summary>
        protected abstract T CreateFieldInfo(TControl label);

        /// <summary>
        /// Extrahiert alle Labels des übergeben Containers.
        /// </summary>
        private void ExtractFields(XRControl container, List<T> extractedFields)
        {
            // Ist das Element bereits ein Label?
            TControl control = container as TControl;
            if (control != null)
            {
                extractedFields.Add(CreateFieldInfo(control));
            }
            else
            {
                // Alle Unterelemente nach Labels durchsuchen...
                foreach (XRControl possibleContainer in container.Controls)
                {
                    ExtractFields(possibleContainer, extractedFields);
                }
            }
        }
        #endregion
    }
}
