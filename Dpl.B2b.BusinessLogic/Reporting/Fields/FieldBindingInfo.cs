namespace Dpl.B2b.BusinessLogic.Reporting.Fields
{
    public class FieldBindingInfo
    {
        #region Constructor
        /// <summary>
        /// Initialisiert die FieldBindingInfo.
        /// </summary>
        public FieldBindingInfo(
            string dataTableName,
            string valuePropertyName,
            string valueBufferPropertyName)
        {
            DataTableName = dataTableName;
            ValuePropertyName = valuePropertyName;
            ValueBufferPropertyName = valueBufferPropertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Liefert, oder setzt die DataTable, an die 
        /// das ReportField gebunden ist.
        /// </summary>
        public string DataTableName
        {
            get;
            set;
        }

        /// <summary>
        /// Liefert, den Namen der Wert-Property, die an die Datenquelle 
        /// des ReportFields gebunden werden. 
        /// </summary>
        public string ValuePropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Liefert, den Namen der Wert-Puffer-Property, die an die Datenquelle 
        /// des ReportFields gebunden werden. 
        /// </summary>
        public string ValueBufferPropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Prüft, ob das Wert-Property definiert wurde.
        /// </summary>
        public bool ValuePropertyNameExists
        {
            get
            {
                return !string.IsNullOrEmpty(ValuePropertyName);
            }
        }

        /// <summary>
        /// Prüft, ob das Wert-Puffer-Property definiert wurde.
        /// </summary>
        public bool ValueBufferPropertyNameExists
        {
            get
            {
                return !string.IsNullOrEmpty(ValueBufferPropertyName);
            }
        }
        #endregion
    }
}
