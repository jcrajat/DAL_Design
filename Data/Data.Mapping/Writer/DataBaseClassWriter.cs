using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using CM.Tools.Misellaneous;
using CMData.Schemas;
using Tools;
using CMData.Utils;

namespace Data.Mapping.Writer
{
    /// <summary>
    /// 
    /// </summary>
    public enum LanguajeType
    {
        VB,
        CSharp
    }

    public class DataBaseClassWriter
    {
        #region Declaraciones

        private XsdDataBase _DataBaseDataSet;
        private StringBuilder _Log = new StringBuilder();
        private static StringCollection _InvalidCharacters = null;

        #endregion

        #region Propiedades

        public StringBuilder Log
        {
            get { return _Log; }
        }

        public static StringCollection InvalidCharacters
        {
            get
            {
                if (_InvalidCharacters == null)
                {
                    _InvalidCharacters = new StringCollection();
                    _InvalidCharacters.AddRange(new[] { "@", ".", "$" });
                }

                return _InvalidCharacters;
            }
        }

        #endregion

        #region Constructores

        public DataBaseClassWriter()
        {
        }
        public DataBaseClassWriter(XsdDataBase nDataBaseDataSet)
        {
            _DataBaseDataSet = nDataBaseDataSet;
        }

        #endregion

        #region Metodos

        public void GenerateClassCode(XsdDataBase.TBL_ConnectionRow nConnection, string nPath, string nNamespaceMap, LanguajeType Languaje, bool IsMobile, bool UseFramework2)
        {
            //Crear el Database Manager
            var catalogos = CMData.DataBase.DataBase.GetDistinctRows(_DataBaseDataSet.TBL_Object.Select("fk_Connection = " + nConnection.id_Connection), "Catalog_Name");

            foreach (DataRow catalogo in catalogos.Rows)
            {
                try
                {
                    var CatalogConfig = _DataBaseDataSet.TBL_Catalog.FindByfk_ConnectionCatalog_Name(nConnection.id_Connection, catalogo["Catalog_Name"].ToString());

                    var fileName = nPath.TrimEnd('\\') + "\\" + CatalogConfig.Class_File_Name + "DBManager";
                    var fileNameDesigner = nPath.TrimEnd('\\') + "\\" + CatalogConfig.Class_File_Name + "DBManager.Designer";

                    switch (Languaje)
                    {
                        case LanguajeType.VB:
                            fileName += ".vb";
                            fileNameDesigner += ".vb";
                            break;

                        case LanguajeType.CSharp:
                            fileName += ".cs";
                            fileNameDesigner += ".cs";
                            break;
                    }

                    var importsCode = getImports(Languaje);
                    var SourceCode = new StringBuilder();
                    var SourceCodeDesigner = new StringBuilder();

                    switch (Languaje)
                    {
                        case LanguajeType.VB:
                            SourceCode = VisualBasicWriter.WriteDataBaseManager(nConnection, CatalogConfig.Class_Name, nNamespaceMap);
                            SourceCodeDesigner = VisualBasicWriter.WriteDataBaseManagerDesigner(nConnection, CatalogConfig.Class_Name, CatalogConfig.Catalog_Name, nNamespaceMap, _DataBaseDataSet, IsMobile, UseFramework2);
                            break;

                        case LanguajeType.CSharp:
                            SourceCode = CSharpWriter.WriteDataBaseManager(nConnection, CatalogConfig.Class_Name, nNamespaceMap);
                            SourceCodeDesigner = CSharpWriter.WriteDataBaseManagerDesigner(nConnection, CatalogConfig.Class_Name, CatalogConfig.Catalog_Name, nNamespaceMap, _DataBaseDataSet, IsMobile, UseFramework2);
                            break;
                    }

                    StreamWriter sw = null;

                    // Diseñador
                    sw = new System.IO.StreamWriter(fileNameDesigner);

                    foreach (var imp in importsCode)
                    {
                        sw.WriteLine(imp);
                    }

                    sw.WriteLine();

                    sw.Write(SourceCodeDesigner.ToString());
                    sw.Flush();
                    sw.Close();

                    // Codigo usuario
                    if (!File.Exists(fileName))
                    {
                        sw = new System.IO.StreamWriter(fileName);

                        foreach (var imp in importsCode)
                        {
                            sw.Write(imp + ControlChars.CrLf);
                        }

                        sw.Write(SourceCode.ToString());
                        sw.Flush();
                        sw.Close();
                    }
                }
                catch (Exception ex)
                {
                    _Log.AppendLine(ex.Message);
                }
            }
        }

        public StringBuilder GenerateClassCode(LanguajeType Languaje, bool IsMobile, bool UseFramework2)
        {
            try
            {
                var cnn = _DataBaseDataSet.TBL_Connection[0];
                var catalogos = CMData.DataBase.DataBase.GetDistinctRows(_DataBaseDataSet.TBL_Object.Select("fk_Connection = " + cnn.id_Connection), "Catalog_Name");
                var SourceCode = new StringBuilder();

                string Catalog = catalogos.Rows[0]["Catalog_Name"].ToString();
                string CatalogClassName = FormatCode.ToIdentifier(Catalog);

                switch (Languaje)
                {
                    case LanguajeType.VB:
                        SourceCode = VisualBasicWriter.WriteDataBaseManagerDesigner(cnn, CatalogClassName, Catalog, "", _DataBaseDataSet, IsMobile, UseFramework2);
                        break;

                    case LanguajeType.CSharp:
                        SourceCode = CSharpWriter.WriteDataBaseManagerDesigner(cnn, CatalogClassName, Catalog, "", _DataBaseDataSet, IsMobile, UseFramework2);
                        break;
                }

                return SourceCode;
            }
            catch (Exception ex)
            {
                _Log.AppendLine(ex.Message);
            }

            return new StringBuilder();
        }

        public void GenerateSchemaXML(XsdDataBase.TBL_ConnectionRow nConnection, XsdDataBase nDataBaseMap, string nPath)
        {
            var catalogos = CMData.DataBase.DataBase.GetDistinctRows(_DataBaseDataSet.TBL_Object.Select("fk_Connection = " + nConnection.id_Connection), "Catalog_Name");

            foreach (DataRow catalogo in catalogos.Rows)
            {
                try
                {
                    var CatalogConfig = _DataBaseDataSet.TBL_Catalog.FindByfk_ConnectionCatalog_Name(nConnection.id_Connection, catalogo["Catalog_Name"].ToString());

                    var fileName = nPath.TrimEnd('\\') + "\\" + CatalogConfig.Class_Name + "DBManager.map";

                    var schemas = CMData.DataBase.DataBase.GetDistinctRows(nDataBaseMap.TBL_Object.Select("fk_Connection = " + nConnection.id_Connection + " AND Catalog_Name = '" + catalogo["Catalog_Name"] + "' AND Selected = 1"), "Schema_Name");
                    var Maping = new SchemaMapingManager();

                    foreach (DataRow schema in schemas.Rows)
                    {
                        var SchemaConfig = nDataBaseMap.TBL_Schema.FindByfk_ConnectionSchema_Name(nConnection.id_Connection, schema["Schema_Name"].ToString());

                        Maping.Schemas.Add(SchemaConfig.Schema_Alias, SchemaConfig.Schema_Name);
                    }

                    SchemaMapingManager.Serialize(Maping, fileName);
                }
                catch (Exception ex)
                {
                    _Log.AppendLine(ex.Message);
                }
            }
        }

        public static void Write(StringBuilder writer, int nIndentTab, string text)
        {
            for (int i = 0; i < nIndentTab; i++)
            {
                writer.Append(ControlChars.Tab);
            }

            writer.Append(text + ControlChars.CrLf);
        }

        #endregion

        #region Funciones

        public List<string> getImports(LanguajeType Languaje)
        {
            var Lista = new List<string>();

            switch (Languaje)
            {
                case LanguajeType.VB:
                    Lista.Add("Imports System");
                    Lista.Add("Imports System.Data");
                    Lista.Add("Imports CMData");
                    Lista.Add("Imports System.Collections");
                    Lista.Add("Imports System.Collections.Generic");
                    Lista.Add("Imports CMData.Manager");
                    Lista.Add("Imports CMData.DataBase");
                    Lista.Add("Imports CMData.Schemas");
                    Lista.Add("Imports CM.Tools");
                    Lista.Add("Imports CM.Tools.Misellaneous");
                    break;

                case LanguajeType.CSharp:
                    Lista.Add("using System;");
                    Lista.Add("using System.Data;");
                    Lista.Add("using CMData;");
                    Lista.Add("using System.Collections;");
                    Lista.Add("using System.Collections.Generic;");
                    Lista.Add("using CMData.Manager;");
                    Lista.Add("using CMData.DataBase;");
                    Lista.Add("using CMData.Schemas;");
                    Lista.Add("using CM.Tools;");
                    Lista.Add("using CM.Tools.Misellaneous;");
                    break;
            }

            return Lista;
        }

        #endregion

        #region Auxiliares

        internal static string GetApplicationStringType(string nType, LanguajeType nLanguaje)
        {
            return GetApplicationStringType(nType, nLanguaje, false);
        }

        internal static string GetApplicationStringType(string nType, LanguajeType nLanguaje, bool IsNullable)
        {
            return GetApplicationStringType((DbType)(Enum.Parse(typeof(DbType), nType)), nLanguaje, IsNullable);
        }

        internal static string GetApplicationStringType(System.Data.DbType nType, LanguajeType nLanguaje)
        {
            return GetApplicationStringType(nType, nLanguaje, false);
        }

        internal static string GetApplicationStringType(System.Data.DbType nType, LanguajeType nLanguaje, bool IsNullable)
        {
            if (IsNullable)
            {
                switch (nType)
                {
                    case DbType.Boolean:
                    case DbType.Byte:
                    case DbType.Decimal:
                    case DbType.Int32:
                    case DbType.Int64:
                    case DbType.Int16:
                    case DbType.SByte:
                    case DbType.Single:
                    case DbType.UInt16:
                    case DbType.UInt32:
                    case DbType.UInt64:
                    case DbType.VarNumeric:
                    case DbType.Time:
                    case DbType.DateTime:
                    case DbType.Date:
                    case DbType.DateTime2:
                    case DbType.DateTimeOffset:
                    case DbType.Guid:
                        return "cargomasterNullable" + getPlantillaNullableIni(nLanguaje) + getLanguajeStringType(nType, nLanguaje) + getPlantillaNullableFin(nLanguaje);

                    case DbType.Binary:
                    case DbType.Currency:
                    case DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                    case DbType.String:
                    case DbType.StringFixedLength:
                    case DbType.Xml:
                        return "cargomasterNullable" + getPlantillaNullableIni(nLanguaje) + getLanguajeStringType(nType, nLanguaje) + getPlantillaNullableFin(nLanguaje);

                    case DbType.Object:
                        return getLanguajeStringType(nType, nLanguaje);
                }
            }
            else
            {
                return getLanguajeStringType(nType, nLanguaje);
            }

            return "object";
        }

        internal static string getLanguajeStringType(System.Data.DbType nType, LanguajeType nLanguaje)
        {
            switch (nLanguaje)
            {
                case LanguajeType.VB:
                    switch (nType)
                    {
                        case DbType.AnsiString:
                        case DbType.AnsiStringFixedLength:
                        case DbType.String:
                        case DbType.StringFixedLength:
                        case DbType.Xml:
                            return "String";

                        case DbType.Binary:
                            return "Byte()";

                        case DbType.Boolean:
                            return "Boolean";

                        case DbType.Byte:
                            return "Byte";

                        case DbType.Currency:
                            return "Double";

                        case DbType.Time:
                        case DbType.DateTime:
                        case DbType.Date:
                        case DbType.DateTime2:
                        case DbType.DateTimeOffset:
                            return "DateTime";

                        case DbType.Decimal:
                            return "Decimal";

                        case DbType.Double:
                        case DbType.VarNumeric:
                            return "Double";

                        case DbType.Guid:
                            return "Guid";

                        case DbType.Int16:
                            return "Short";

                        case DbType.Int32:
                            return "Integer";

                        case DbType.Int64:
                            return "Long";

                        case DbType.Object:
                            return "Object";

                        case DbType.SByte:
                            return "SByte";

                        case DbType.Single:
                            return "Single";

                        case DbType.UInt16:
                            return "UInt16";

                        case DbType.UInt32:
                            return "UInt32";

                        case DbType.UInt64:
                            return "UInt64";

                        default:
                            throw new Exception("El tipo de dato " + nType.ToString() + " no se pudo mapear");
                    }

                case LanguajeType.CSharp:
                    switch (nType)
                    {
                        case DbType.AnsiString:
                        case DbType.AnsiStringFixedLength:
                        case DbType.String:
                        case DbType.StringFixedLength:
                        case DbType.Xml:
                            return "string";

                        case DbType.Binary:
                            return "byte[]";

                        case DbType.Boolean:
                            return "bool";

                        case DbType.Byte:
                            return "byte";

                        case DbType.Currency:
                            return "double";

                        case DbType.Time:
                        case DbType.DateTime:
                        case DbType.Date:
                        case DbType.DateTime2:
                        case DbType.DateTimeOffset:
                            return "DateTime";

                        case DbType.Decimal:
                            return "decimal";

                        case DbType.Double:
                        case DbType.VarNumeric:
                            return "double";

                        case DbType.Guid:
                            return "Guid";

                        case DbType.Int16:
                            return "short";

                        case DbType.Int32:
                            return "int";

                        case DbType.Int64:
                            return "long";

                        case DbType.Object:
                            return "object";

                        case DbType.SByte:
                            return "sbyte";

                        case DbType.Single:
                            return "float";

                        case DbType.UInt16:
                            return "UInt16";

                        case DbType.UInt32:
                            return "UInt32";

                        case DbType.UInt64:
                            return "UInt64";

                        default:
                            throw new Exception("El tipo de dato " + nType.ToString() + " no se pudo mapear");
                    }
            }

            return "";
        }

        private static string getArrayIdentifier(LanguajeType nLanguaje)
        {
            switch (nLanguaje)
            {
                case LanguajeType.VB:
                    return "()";

                case LanguajeType.CSharp:
                    return "[]";
            }

            return "";
        }

        private static string getPlantillaNullableIni(LanguajeType nLanguaje)
        {
            switch (nLanguaje)
            {
                case LanguajeType.VB: return "(Of ";
                case LanguajeType.CSharp: return "<";
                default: return "";
            }
        }

        private static string getPlantillaNullableFin(LanguajeType nLanguaje)
        {
            switch (nLanguaje)
            {
                case LanguajeType.VB: return ")";
                case LanguajeType.CSharp: return ">";
                default: return "";
            }
        }

        internal static string getBoolean(bool nValue, LanguajeType nLanguaje)
        {
            switch (nLanguaje)
            {
                case LanguajeType.VB:
                    return nValue ? "True" : "False";
                case LanguajeType.CSharp:
                    return nValue ? "true" : "false";
                default:
                    return "";
            }
        }

        #endregion
    }
}