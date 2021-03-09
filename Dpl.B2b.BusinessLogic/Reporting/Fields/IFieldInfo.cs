using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;

namespace Dpl.B2b.BusinessLogic.Reporting.Fields
{
    public interface IFieldInfo<T> where T : XRFieldEmbeddableControl
    {
        #region Properties
        /// <summary>
        /// Liefert, oder setzt das Control.
        /// </summary>
        T Control
        {
            get;
            set;
        }

        /// <summary>
        /// Liefert die DataSource.
        /// </summary>
        LocalizedReportDataSet DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// Prüft, ob das Label an keine Datenquelle gebunden ist.
        /// </summary>
        bool IsUnbound
        {
            get;
        }

        /// <summary>
        /// Prüft, ob das Label an die Datenquelle gebunden ist.
        /// </summary>
        bool IsPropertyBoundOnThisDataTable
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Bindet das Label an die Datenquelle.
        /// </summary>
        void Rebind();

        /// <summary>
        /// Lädt den Datenquell-Text in das übergeben Label.
        /// </summary>
        void LoadValueIntoField();
        #endregion
    }
}
