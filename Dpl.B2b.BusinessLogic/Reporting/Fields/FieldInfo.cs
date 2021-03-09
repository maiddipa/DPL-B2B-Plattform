using System;
using System.Data;
using System.Text.RegularExpressions;
using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;

namespace Dpl.B2b.BusinessLogic.Reporting.Fields
{
    public abstract class FieldInfo<T>
        : IFieldInfo<T> where T : XRFieldEmbeddableControl
    {
        #region Constructor
        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public FieldInfo(T control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            Control = control;
        }

        /// <summary>
        /// Initialisiert den LocalizableReportLabelBinder.
        /// </summary>
        public FieldInfo(
            T control,
            LocalizedReportDataSet dataSource)
            : this(control)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }
            DataSource = dataSource;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Liefert, oder setzt das Label.
        /// </summary>
        public T Control
        {
            get;
            set;
        }

        /// <summary>
        /// Liefert, oder setzt die DataSource.
        /// </summary>
        public LocalizedReportDataSet DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// Liefert das DataBinding des Labels.
        /// </summary>
        public XRBinding LabelDataBinding
        {
            get
            {
                XRBinding dataBinding = Control.DataBindings[
                    FieldBindingInfo.ValuePropertyName];
                if (dataBinding != null)
                {
                    return dataBinding;
                }
                return Control.DataBindings[
                   FieldBindingInfo.ValueBufferPropertyName];
            }
        }

        /// <summary>
        /// Prüft, ob das Label an die ReportLabel-Tabelle gebunden ist.
        /// </summary>
        public bool IsPropertyBoundOnThisDataTable
        {
            get
            {
                return (IsPropertyBoundOnDataTable(
                        FieldBindingInfo.ValuePropertyName)
                    || IsPropertyBoundOnDataTable(
                        FieldBindingInfo.ValueBufferPropertyName));
            }
        }

        /// <summary>
        /// Prüft, ob das Label an eien Tabelle gebunden ist.
        /// </summary>
        public bool IsUnbound
        {
            get
            {
                if (Control.DataBindings == null
                || Control.DataBindings.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Liefert, die FieldBindingInfo, an die das Label gebunden wird/ist.
        /// </summary>
        protected abstract FieldBindingInfo FieldBindingInfo
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Bindet das Label an die DataTable.
        /// </summary>
        public void Rebind()
        {
            if (Control == null)
            {
                throw new ArgumentNullException("Control");
            }

            XRBinding binding = Control.DataBindings[
                FieldBindingInfo.ValuePropertyName];
            if (binding != null)
            {
                Control.DataBindings.Remove(binding);
            }

            Control.DataBindings.Add(
                new XRBinding(
                    FieldBindingInfo.ValuePropertyName,
                    null,
                    string.Format(
                        "{0}.{1}",
                        FieldBindingInfo.DataTableName,
                        Control.Name)));
        }

        /// <summary>
        /// Lädt den Label-Text in das übergeben Label.
        /// </summary>
        public void LoadValueIntoField()
        {
            if (Control == null)
            {
                throw new ArgumentNullException("Control");
            }
            if (DataSource == null)
            {
                throw new ArgumentNullException("DataSource");
            }
            if (!FieldBindingInfo.ValueBufferPropertyNameExists)
            {
                throw new ArgumentNullException("ValueBufferPropertyName");
            }

            // Prüfen, ob eine Bindung an die virtuelle multilange table besteht.
            if (IsPropertyBoundOnThisDataTable)
            {
                XRBinding binding = LabelDataBinding;

                string dataMember = binding.DataMember;
                BindValueProperty(GetDataBindingValue(dataMember));
                
                // Umwandeln des Text-Binding in ein Tag-Binding
                Control.DataBindings.Remove(binding);
                Control.DataBindings.Add(
                    FieldBindingInfo.ValueBufferPropertyName,
                    null,
                    dataMember);
            }
        }

        /// <summary>
        /// Liefert den Wert des übergebenen DataMember.
        /// </summary>
        protected object GetDataBindingValue(string dataMember)
        {
            if (Control == null)
            {
                throw new ArgumentNullException("Label");
            }
            if (DataSource == null)
            {
                throw new ArgumentNullException("DataSource");
            }

            if (!string.IsNullOrEmpty(dataMember))
            {
                string tableName = ExtractTableNameFromDataMember(dataMember);
                string columnName = ExtractColumnNameFromDataMember(dataMember);

                DataTable dataTable = DataSource.Tables[tableName];
                if (dataTable.Columns.Count > 0)
                {
                    DataColumn column = dataTable.Columns[columnName];
                    if (column != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow row = dataTable.Rows[0];
                            object cell = row[column];
                            if (cell != null)
                            {
                                return GetCellDataBindingValue(cell);
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Prüft, ob die Property des Label gebunden ist.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool IsPropertyBoundOnDataTable(string propertyName)
        {
            XRBinding propertyDataBinding
                = Control.DataBindings[propertyName];
            // Ist ein DataBinding an die Text-Property vorhanden?
            if (propertyDataBinding != null)
            {
                string dataMember = propertyDataBinding.DataMember;
                // Handelt es sich um eine Datenbindung an die 
                // virtuelle ReportLabelDataTable?
                if (!string.IsNullOrEmpty(dataMember)
                    && dataMember.StartsWith(FieldBindingInfo.DataTableName))
                {
                    return true;
                }
            }
            return false;
        }

        protected static string ExtractColumnNameFromDataMember(string dataMember)
        {
            return Regex.Replace("" + dataMember, @"^.*\.", string.Empty);
        }

        protected static string ExtractTableNameFromDataMember(string dataMember)
        {
            return Regex.Replace("" + dataMember, @"\..*$", string.Empty);
        }

        protected abstract void BindValueProperty(object value);

        protected abstract object GetCellDataBindingValue(object cell);

        #endregion
    }
}
