using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using CM.Tools.Misellaneous;
using Tools;

namespace CM.Tools.CSV
{
    public class CSVData : IDisposable
    {
        #region Declaraciones

        string[] DataString;

        private Regex regQuote = new Regex(@"^(\x22)(.*)(\x22)(\s*,)(.*)$", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
        private Regex regNormal = new Regex(@"^([^,]*)(\s*,)(.*)$", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
        private Regex regQuoteLast = new Regex(@"^(\x22)([\x22*]{2,})(\x22)$", RegexOptions.IgnoreCase);
        private Regex regNormalLast = new Regex(@"^.*$", RegexOptions.IgnoreCase);

        protected bool Disposed;

        #endregion

        #region Costructores

        public CSVData()
        {
            this.LinesToJump = 0;
            this.Separator = ",";
            this.TextQualifier = "\"";
            this.HasHeader = true;
            this.DateFormat = "yyyy-MM-dd HH:mm:ss";            
            this.LoadEncoding = null;
            this.SaveEncoding = Encoding.UTF8;
        }

        public CSVData(string nSeparator, string nTextQualifier, bool nHasHeader)
            : this()
        {
            this.Separator = nSeparator;
            this.TextQualifier = nTextQualifier;
            this.HasHeader = nHasHeader;
        }

        ~CSVData()
        {
            Dispose(false);
        }

        #endregion

        #region Implementacion - IDisposable

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region Propiedades

        public int LinesToJump { get; set; }

        public string Separator { get; set; }

        public string TextQualifier { get; set; }

        public bool HasHeader { get; set; }

        public string DateFormat { get; set; }

        public CSVTable DataTable { get; set; }

        /// <summary>
        /// Define la codificación usada para leer los archivos de texto, si es null se lee a partir de la propiedades del archivo
        /// </summary>
        public Encoding LoadEncoding { get; set; }

        /// <summary>
        /// Define la codificación usada para escribir los archivos de texto
        /// </summary>
        public Encoding SaveEncoding { get; set; }        

        #endregion

        #region Load CSV

        public void LoadCSV(string nCSVFile)
        {
            if (!File.Exists(nCSVFile))
                throw new Exception(nCSVFile + " no existe.");

            using (StreamReader sr = this.LoadEncoding == null ? new StreamReader(nCSVFile, true) : new StreamReader(nCSVFile, this.LoadEncoding))
            {
                LoadCSV(sr);
            }
        }

        public void LoadCSV(string nCSVFile, bool nHasHeader)
        {
            this.HasHeader = nHasHeader;
            LoadCSV(nCSVFile);
        }

        public void LoadCSV(string nCSVFile, bool nHasHeader, string nSeparator)
        {
            this.Separator = nSeparator;
            LoadCSV(nCSVFile, nHasHeader);
        }

        public void LoadCSV(string nCSVFile, bool nHasHeader, string nSeparator, string nTextQualifier)
        {
            this.TextQualifier = nTextQualifier;
            LoadCSV(nCSVFile, nHasHeader, nSeparator);
        }


        public void LoadCSV(StreamReader nCSVFile)
        {
            SetupRegEx();

            if (this.DataTable != null)
                this.DataTable.Clear();

            this.DataTable = new CSVTable();

            bool FirstLine = true;

            if (this.LinesToJump > 0)
            {
                for (int i = 0; i < this.LinesToJump; i++)
                {
                    nCSVFile.ReadLine();
                }
            }

            do
            {
                ProcessLine(nCSVFile.ReadLine());

                //
                // Create Columns
                //
                if (FirstLine)
                {
                    for (int idx = 0; idx <= DataString.GetUpperBound(0); idx++)
                    {
                        if (this.HasHeader)
                            this.DataTable.Columns.Add(DataString[idx]);
                        else
                            this.DataTable.Columns.Add("Column" + idx);
                    }
                }

                //
                // Add Data
                //
                if (!(FirstLine && this.HasHeader))
                {
                    var dr = this.DataTable.NewRow();

                    for (int idx = 0; idx <= DataString.GetUpperBound(0); idx++)
                    {
                        dr[idx] = DataString[idx];
                    }

                    this.DataTable.Rows.Add(dr);
                }

                FirstLine = false;
            }
            while (nCSVFile.Peek() > -1);

            nCSVFile.Close();
        }

        public void LoadCSV(StreamReader nCSVFile, bool nHasHeader)
        {
            this.HasHeader = nHasHeader;
            LoadCSV(nCSVFile);
        }
        
        public void LoadCSV(StreamReader nCSVFile, bool nHasHeader, string nSeparator)
        {
            this.Separator = nSeparator;
            LoadCSV(nCSVFile, nHasHeader);
        }

        public void LoadCSV(StreamReader nCSVFile, bool nHasHeader, string nSeparator, string nTextQualifier)
        {
            this.TextQualifier = nTextQualifier;
            LoadCSV(nCSVFile, nHasHeader, nSeparator);
        }
        
        #endregion

        #region Save As

        public void SaveAsCSV(string nCSVFile, bool nAutoTrim)
        {
            if (this.DataTable == null)
                return;

            SaveAsCSV(this.DataTable, nCSVFile, nAutoTrim);
        }

        public void SaveAsCSV(string nCSVFile, bool nAutoTrim, bool nHasHeader)
        {
            this.HasHeader = nHasHeader;
            SaveAsCSV(nCSVFile, nAutoTrim);
        }

        public void SaveAsCSV(string nCSVFile, bool nAutoTrim, bool nHasHeader, string nSeparator)
        {
            this.Separator = nSeparator;
            SaveAsCSV(nCSVFile, nAutoTrim, nHasHeader);
        }

        public void SaveAsCSV(string nCSVFile, bool nAutoTrim, bool nHasHeader, string nSeparator, string nTextQualifier)
        {            
            this.TextQualifier = nTextQualifier;
            SaveAsCSV(nCSVFile, nAutoTrim, nHasHeader, nSeparator);
        }

        
        public void SaveAsCSV(CSVTable nDataTable, string nCSVFile, bool nAutoTrim)
        {
            SetupRegEx();

            var sLine = "";
            var sw = new StreamWriter(nCSVFile, false, this.SaveEncoding);

            if (this.HasHeader)
            {
                for (var iCol = 0; iCol < nDataTable.Columns.Count; iCol++)
                {
                    if (!nDataTable.Columns[iCol].Export) continue;
                    if (sLine.Length > 0)
                        sLine += this.Separator;

                    sLine += ExportFormat(nDataTable.Columns[iCol].ColumnTitle);
                }

                sw.WriteLine(sLine);
            }

            foreach (CSVRow dr in nDataTable.Rows)
            {
                sLine = "";

                for (var iCol = 0; iCol < nDataTable.Columns.Count; iCol++)
                {
                    if (!nDataTable.Columns[iCol].Export) continue;

                    if (iCol > 0) sLine += this.Separator;

                    if (dr[iCol] == null) continue;

                    var Valor = ExportFormat(getValueFormated(dr[iCol], nDataTable.Columns[iCol].Format));
                    
                    if (Valor.IndexOf(this.Separator) > -1)
                        sLine += this.TextQualifier + (nAutoTrim ? Valor.Trim() : Valor) + this.TextQualifier;
                    else
                        sLine += (nAutoTrim ? ExportFormat(Valor.Trim()) : Valor);
                }

                sw.WriteLine(sLine);
            }

            sw.Flush();
            sw.Close();
        }

        private string ExportFormat(string nExportText)
        {
            // Eliminar el enter
            var result = nExportText.Replace('\n', ' ').Replace("\r", "");

            // Remplazar identificador de texto
            if (this.TextQualifier != "")
                result = result.Trim(this.TextQualifier.ToCharArray());
            
            return result;
        }        

        public void SaveAsCSV(CSVTable nDataTable, string nCSVFile, bool nAutoTrim, bool nHasHeader)
        {
            this.HasHeader = nHasHeader;
            SaveAsCSV(nDataTable, nCSVFile, nAutoTrim);
        }

        public void SaveAsCSV(CSVTable nDataTable, string nCSVFile, bool nAutoTrim, bool nHasHeader, string nSeparator)
        {
            this.Separator = nSeparator;
            SaveAsCSV(nDataTable, nCSVFile, nAutoTrim, nHasHeader);
        }

        public void SaveAsCSV(CSVTable nDataTable, string nCSVFile, bool nAutoTrim, bool nHasHeader, string nSeparator, string nTextQualifier)
        {
            this.TextQualifier = nTextQualifier;
            SaveAsCSV(nDataTable, nCSVFile, nAutoTrim, nHasHeader, nSeparator);
        }


        public void SaveAsCSV(CSVTable nDataTable, MemoryStream nCSVStream, bool nAutoTrim)
        {
            SetupRegEx();

            string sLine = "";

            if (this.HasHeader)
            {
                for (int iCol = 0; iCol < nDataTable.Columns.Count; iCol++)
                {
                    if (nDataTable.Columns[iCol].Export)
                    {
                        if (sLine.Length > 0)
                            sLine += this.Separator;

                        sLine += nDataTable.Columns[iCol].ColumnTitle;
                    }
                }

                var bytes = this.SaveEncoding.GetBytes(sLine + "\r\n");
                nCSVStream.Write(bytes, 0, bytes.Length);
            }

            foreach (CSVRow dr in nDataTable.Rows)
            {
                sLine = "";

                for (int iCol = 0; iCol < nDataTable.Columns.Count; iCol++)
                {
                    if (nDataTable.Columns[iCol].Export)
                    {
                        if (iCol > 0) sLine += this.Separator;

                        if (dr[iCol] != null)
                        {
                            string Valor = getValueFormated(dr[iCol], nDataTable.Columns[iCol].Format);

                            if (Valor.IndexOf(this.Separator) > -1)
                                sLine += this.TextQualifier + (nAutoTrim ? Valor.Trim() : Valor) + this.TextQualifier;
                            else
                                sLine += (nAutoTrim ? Valor.Trim() : Valor);
                        }
                    }
                }

                var bytes = this.SaveEncoding.GetBytes(sLine + "\r\n");
                nCSVStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void SaveAsCSV(CSVTable nDataTable, MemoryStream nCSVStream, bool nAutoTrim, bool nHasHeader)
        {
            this.HasHeader = nHasHeader;
            SaveAsCSV(nDataTable, nCSVStream, nAutoTrim);
        }

        public void SaveAsCSV(CSVTable nDataTable, MemoryStream nCSVStream, bool nAutoTrim, bool nHasHeader, string nSeparator)
        {
            this.Separator = nSeparator;
            SaveAsCSV(nDataTable, nCSVStream, nAutoTrim, nHasHeader);
        }

        public void SaveAsCSV(CSVTable nDataTable, MemoryStream nCSVStream, bool nAutoTrim, bool nHasHeader, string nSeparator, string nTextQualifier)
        {
            this.TextQualifier = nTextQualifier;
            SaveAsCSV(nDataTable, nCSVStream, nAutoTrim, nHasHeader, nSeparator);
        }
        
        #endregion

        #region Metodos

        private void ProcessLine(string sLine)
        {
            DataString = null;

            if (this.Separator != ControlChars.Tab)
                sLine = sLine.Replace(ControlChars.Tab, "    "); //Replace tab with 4 spaces

            do
            {
                string sData;

                Match m;
                if (regQuote.IsMatch(sLine))
                {
                    regQuote.Matches(sLine);
                    //
                    // "text",<rest of the line>
                    //
                    m = regQuote.Match(sLine);
                    sData = m.Groups[2].Value;
                    sLine = m.Groups[5].Value;
                }
                else if (regQuoteLast.IsMatch(sLine))
                {
                    //
                    // "text"
                    //
                    m = regQuoteLast.Match(sLine);
                    sData = m.Groups[2].Value;
                    sLine = "";
                }
                else if (regNormal.IsMatch(sLine))
                {
                    //
                    // text,<rest of the line>
                    //
                    m = regNormal.Match(sLine);
                    sData = m.Groups[1].Value;
                    sLine = m.Groups[3].Value;
                }
                else if (regNormalLast.IsMatch(sLine))
                {
                    //
                    // text
                    //
                    m = regNormalLast.Match(sLine);
                    sData = m.Groups[0].Value;
                    sLine = "";
                }
                else
                {
                    //
                    // ERROR!!!!!
                    //
                    sData = "";
                    sLine = "";
                }

                sData = sData.Trim();

                if (this.Separator != ControlChars.Tab)
                    sLine = sLine.Trim();
                
                int idx;
                if (DataString == null)
                {
                    DataString = new string[1];
                    idx = 0;
                }
                else
                {
                    idx = DataString.GetUpperBound(0) + 1;
                    var tmp = new string[idx + 1];

                    DataString.CopyTo(tmp, 0);
                    DataString = tmp;
                }

                DataString[idx] = sData;
            }
            while (sLine.Length > 0);
        }

        private void SetupRegEx()
        {
            var sQuote = @"^(%Q)(.*)(%Q)(\s*%S)(.*)$";
            var sNormal = @"^([^%S]*)(\s*%S)(.*)$";
            var sQuoteLast = @"^(%Q)(.*)(%Q$)";
            const string sNormalLast = @"^.*$";

            regQuote = null;
            regNormal = null;
            regQuoteLast = null;
            regNormalLast = null;

            var sSep = this.Separator;
            var sQual = this.TextQualifier;

            const string Caracteres = @".$^{[(|)]}*+?\";

            if (Caracteres.IndexOf(sSep) > -1) sSep = @"\" + sSep;
            if (Caracteres.IndexOf(sQual) > 0) sQual = @"\" + sQual;

            sQuote = sQuote.Replace(@"%S", sSep);
            sQuote = sQuote.Replace(@"%Q", sQual);
            sNormal = sNormal.Replace(@"%S", sSep);
            sQuoteLast = sQuoteLast.Replace(@"%Q", sQual);

            regQuote = new Regex(sQuote, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            regNormal = new Regex(sNormal, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            regQuoteLast = new Regex(sQuoteLast, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            regNormalLast = new Regex(sNormalLast, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
        }

        protected void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                Disposed = true;
                GC.SuppressFinalize(this);
            }

            if (this.DataTable != null)
            {
                this.DataTable.Clear();
                this.DataTable = null;
            }
        }

        #endregion

        #region Funciones

        private string getValueFormated(object nValue, string nFormat)
        {
            if (nFormat != "")
            {
                switch (nValue.GetType().FullName)
                {
                    case "System.DateTime": return ((DateTime)nValue).ToString(nFormat);
                    case "System.Single": return ((Single)nValue).ToString(nFormat);
                    case "System.Double": return ((Double)nValue).ToString(nFormat);
                    case "System.Decimal": return ((Decimal)nValue).ToString(nFormat);
                    case "System.Byte": return ((Byte)nValue).ToString(nFormat);
                    case "System.Int16": return ((Int16)nValue).ToString(nFormat);
                    case "System.Int32": return ((Int32)nValue).ToString(nFormat);
                    case "System.Int64": return ((Int64)nValue).ToString(nFormat);
                    default: return nValue.ToString();
                }
            }

            switch (nValue.GetType().FullName)
            {
                case "System.DateTime":
                    return ((DateTime)nValue).ToString(this.DateFormat);

                case "System.Decimal":
                case "System.Double":
                case "System.Single":
                    return nValue.ToString().Replace(',', '.');

                default:
                    return nValue.ToString();
            }
        }

        #endregion
    }

    public class CSVTable : IEnumerable
    {
        #region Declaraciones

        public delegate void OnColumnAddDelegate(CSVColumn nColumn);
        public event OnColumnAddDelegate OnColumnAdd;

        public delegate void OnColumnRemoveDelegate(int nColumnIndex);
        public event OnColumnRemoveDelegate OnColumnRemove;

        public delegate void OnColumnsClearDelegate();
        public event OnColumnsClearDelegate OnColumnsClear;

        #endregion

        #region Propiedades

        public CSVRow this[int nRowIndex]
        {
            get { return this.Rows[nRowIndex]; }
        }

        public CSVColumnList Columns { get; private set; }

        public CSVRowList Rows { get; private set; }

        public int Count
        {
            get { return this.Rows.Count; }
        }

        #endregion

        #region Constructores

        public CSVTable()
        {
            this.Columns = new CSVColumnList(this);
            this.Rows = new CSVRowList(this);
        }

        public CSVTable(DataTable nDataTable)
            : this()
        {
            foreach (DataColumn Column in nDataTable.Columns)
            {
                this.Columns.Add(Column.ColumnName, Column.DataType);
            }

            foreach (DataRow Row in nDataTable.Rows)
            {
                var NewRow = this.NewRow();

                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (!Row.IsNull(i))
                        NewRow[i] = Row[i];
                }

                this.Rows.Add(NewRow);
            }
        }

        ~CSVTable()
        {
            this.Columns.Clear();
            this.Columns = null;

            this.Rows.Clear();
            this.Rows = null;
        }

        #endregion

        #region Implementacion - IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        #endregion

        #region Metodos

        internal void ColumnAdd(CSVColumn NewColumn)
        {
            if (this.OnColumnAdd != null) this.OnColumnAdd(NewColumn);
        }

        internal void ColumnRemove(int nColumnIndex)
        {
            if (this.OnColumnRemove != null) this.OnColumnRemove(nColumnIndex);
        }

        internal void ColumnsClear()
        {
            if (this.OnColumnsClear != null) this.OnColumnsClear();
        }

        public void Clear()
        {
            this.Rows.Clear();
        }

        #endregion

        #region Funciones

        public DataTable ToDataTable()
        {
            return ToDataTable("Table1");
        }

        public DataTable ToDataTable(string nTableName)
        {
            var NewDataTable = new DataTable();

            if (nTableName != "") NewDataTable.TableName = nTableName;

            foreach (CSVColumn Column in this.Columns)
            {
                NewDataTable.Columns.Add(Column.ColumnName, Column.ColumnType);
            }

            foreach (CSVRow Row in this.Rows)
            {
                var NewRow = NewDataTable.NewRow();

                for (var i = 0; i < this.Columns.Count; i++)
                {
                    if (Row[i] != null)
                        NewRow[i] = Row[i];
                }

                NewDataTable.Rows.Add(NewRow);
            }

            return NewDataTable;
        }

        public CSVRow NewRow()
        {
            return new CSVRow(this);
        }

        #endregion
    }

    public class CSVColumnList : IEnumerable
    {
        #region Declaraciones

        private Dictionary<string, CSVColumn> ColumnsDictionary;

        private List<CSVColumn> ColumnsList;

        #endregion

        #region Propiedades

        public CSVColumn this[int nColumnIndex]
        {
            get { return this.ColumnsList[nColumnIndex]; }
        }

        public CSVColumn this[string nColumnName]
        {
            get { return this.ColumnsDictionary[nColumnName]; }
        }

        public CSVTable Table { get; internal set; }

        public int Count
        {
            get { return this.ColumnsList.Count; }
        }

        #endregion

        #region Constructores

        internal CSVColumnList(CSVTable nTable)
        {
            this.ColumnsDictionary = new Dictionary<string, CSVColumn>();
            this.ColumnsList = new List<CSVColumn>();
            this.Table = nTable;
        }

        ~CSVColumnList()
        {
            this.Table = null;
            this.ColumnsDictionary.Clear();
            this.ColumnsList.Clear();
        }

        #endregion

        #region Implementacion - IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ColumnsList.GetEnumerator();
        }

        #endregion

        #region Metodos

        public void Add(CSVColumn nColumn)
        {
            nColumn.Table = this.Table;
            this.ColumnsDictionary.Add(nColumn.ColumnName, nColumn);
            this.ColumnsList.Add(nColumn);
            this.Table.ColumnAdd(nColumn);

            // Reportar cambio en la colección de columnas
            this.Table.ColumnAdd(nColumn);
        }

        public void Remove(CSVColumn nColumn)
        {
            var ColumnIndex = this.ColumnsList.IndexOf(nColumn);
            this.ColumnsDictionary.Remove(nColumn.ColumnName);
            this.ColumnsList.Remove(nColumn);

            // Reportar cambio en la colección de columnas
            this.Table.ColumnRemove(ColumnIndex);
        }

        public void Clear()
        {
            this.ColumnsDictionary.Clear();
            this.ColumnsList.Clear();
            if (this.Table != null) this.Table.ColumnsClear();
        }

        #endregion

        #region Funciones

        public CSVColumn Add(string nColumnName)
        {
            return Add(nColumnName, typeof(string));
        }

        public CSVColumn Add(string nColumnName, Type nColumnType)
        {
            var NewColumn = new CSVColumn(nColumnName, nColumnType);
            Add(NewColumn);
            return NewColumn;
        }

        public CSVColumn Remove(string nColumnName)
        {
            var RemoveColumn = this.ColumnsDictionary[nColumnName];
            Remove(RemoveColumn);
            return RemoveColumn;
        }

        public CSVColumn Remove(int nColumnIndex)
        {
            var RemoveColumn = this.ColumnsList[nColumnIndex];
            this.ColumnsDictionary.Remove(RemoveColumn.ColumnName);
            this.ColumnsList.Remove(RemoveColumn);
            return RemoveColumn;
        }

        internal int IndexOf(string nColumnName)
        {
            return this.ColumnsList.IndexOf(this.ColumnsDictionary[nColumnName]);
        }

        #endregion
    }

    public class CSVColumn
    {
        #region Propiedades

        public CSVTable Table { get; internal set; }

        public string ColumnName { get; private set; }

        public string ColumnTitle { get; set; }

        public Type ColumnType { get; private set; }

        public int ColumnIndex
        {
            get { return this.Table != null ? this.Table.Columns.IndexOf(this.ColumnName) : -1; }
        }

        public bool Export { get; set; }

        public string Format { get; set; }

        #endregion

        #region Constructores

        public CSVColumn(string nColumnName)
            : this(nColumnName, nColumnName, typeof(string))
        { }

        public CSVColumn(string nColumnName, string nColumnTitle)
            : this(nColumnName, nColumnTitle, typeof(string))
        { }

        public CSVColumn(string nColumnName, Type nColumnType)
            : this(nColumnName, nColumnName, nColumnType)
        { }

        public CSVColumn(string nColumnName, string nColumnTitle, Type nColumnType)
        {
            this.ColumnName = nColumnName;
            this.ColumnTitle = nColumnTitle;
            this.ColumnType = nColumnType;
            this.Export = true;
        }

        #endregion
    }

    public class CSVRowList : IEnumerable
    {
        #region Declaraciones

        private List<CSVRow> Rows;

        #endregion

        #region Propiedades

        public CSVRow this[int nRowIndex]
        {
            get { return this.Rows[nRowIndex]; }
        }

        public CSVTable Table { get; internal set; }

        public int Count
        {
            get { return this.Rows.Count; }
        }

        #endregion

        #region Constructores

        internal CSVRowList(CSVTable nTable)
        {
            this.Rows = new List<CSVRow>();
            this.Table = nTable;
        }

        ~CSVRowList()
        {
            this.Table = null;
            this.Rows.Clear();
        }

        #endregion

        #region Implementacion - IEnumerable

        public IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        #endregion

        #region Metodos

        public void Add(CSVRow nRow)
        {
            if (nRow.Table == this.Table)
                this.Rows.Add(nRow);
            else
                throw new Exception("El Row que intenta agregar no pertenece a la tabla");
        }

        public void Remove(CSVRow nRow)
        {
            this.Rows.Remove(nRow);
        }

        public void Remove(int nRowIndex)
        {
            this.Rows.RemoveAt(nRowIndex);
        }

        public void Clear()
        {
            this.Rows.Clear();
        }

        #endregion
    }

    public class CSVRow : IEnumerable
    {
        #region Declaraciones

        private List<object> Items;

        #endregion

        #region Propiedades

        public object this[int nColumnIndex]
        {
            get { return this.Items[nColumnIndex]; }
            set { this.Items[nColumnIndex] = Convert.ChangeType(value, this.Table.Columns[nColumnIndex].ColumnType); }
        }

        public object this[string nColumnName]
        {
            get { return this.Items[this.Table.Columns.IndexOf(nColumnName)]; }
            set
            {
                int ColumnIndex = this.Table.Columns.IndexOf(nColumnName);
                this.Items[ColumnIndex] = Convert.ChangeType(value, this.Table.Columns[ColumnIndex].ColumnType);
            }
        }

        public CSVTable Table { get; internal set; }

        public int Count
        {
            get { return this.Items.Count; }
        }

        #endregion

        #region Constructores

        internal CSVRow(CSVTable nTable)
        {
            this.Table = nTable;
            this.Items = new List<object>();

            for (int i = 0; i < this.Table.Columns.Count; i++)
            {
                this.Items.Add(null);
            }

            this.Table.OnColumnAdd += Table_OnColumnAdd;
            this.Table.OnColumnRemove += Table_OnColumnRemove;
            this.Table.OnColumnsClear += Table_OnColumnsClear;
        }

        ~CSVRow()
        {
            this.Items.Clear();
            this.Items = null;
        }

        #endregion

        #region Eventos

        void Table_OnColumnsClear()
        {
            if (this.Items != null)
                this.Items.Clear();
        }

        void Table_OnColumnRemove(int nColumnIndex)
        {
            this.Items.RemoveAt(nColumnIndex);
        }

        void Table_OnColumnAdd(CSVColumn nColumn)
        {
            this.Items.Add(null);
        }

        #endregion

        #region Implementacion - IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion
    }
}