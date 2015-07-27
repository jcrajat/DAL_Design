using System;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using CM.Tools.Misellaneous;
using CMData.Schemas;
using Tools;
using CMData.Manager;

namespace CMData.DataBase
{
    /// <summary>
    /// Clase base de conexión a bases de datos Postgres
    /// </summary>
    internal class PostgresDataBase : DataBase
    {
        #region Declaraciones

        private string _Identifier_Date_Format = "yyyy-MM-dd HH:mm:ss";

        #endregion

        #region Constructores

        public PostgresDataBase(string nConnectionString)
        {
            DefaultFactory = Npgsql.NpgsqlFactory.Instance;
            base.Initialize(nConnectionString);
        }

        #endregion

        #region Propiedades

        public override DataBaseType DataBaseType
        {
            get
            {
                return DataBaseType.Postgres;
            }
        }

        public override string Identifier_Table_Prefix
        {
            get
            {
                return "\"";
            }
        }

        public override string Identifier_Table_Postfix
        {
            get
            {
                return "\"";
            }
        }

        public override string Identifier_Procedure_Prefix
        {
            get
            {
                return "";
            }
        }

        public override string Identifier_Procedure_Postfix
        {
            get
            {
                return "";
            }
        }

        public override string Identifier_Date_Format
        {
            get
            {
                return _Identifier_Date_Format; //"yyyy-MM-dd HH:mm:ss"
            }
            set
            {
                _Identifier_Date_Format = value;
            }
        }

        public override string Identifier_Operator_Like
        {
            get
            {
                return "ILIKE";
            }
        }

        public override string Identifier_Operator_Similar
        {
            get
            {
                return "SIMILAR TO"; //"~*"
            }
        }

        public override string Identifier_Symbol_Similar
        {
            get
            {
                return "%"; //".*"
            }
        }

        #endregion

        #region Metodos

        public override void DeriveParameters(ref DbCommand cmd)
        {
        }

        public override void FillDataBaseTables(XsdDataBase.TBL_ObjectDataTable nObjectTable, XsdDataBase.TBL_ConnectionRow nConnection, List<string> nSchemaFilter)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendLine(@"SELECT c.table_catalog , c.table_schema, c.table_name, c.table_type FROM information_schema.tables c");
                sql.AppendLine(@"WHERE c.table_type = 'BASE TABLE' AND c.table_schema NOT IN('information_schema', 'pg_catalog')");
                sql.AppendLine(@"AND  EXISTS (SELECT cu.table_name FROM information_schema.key_column_usage cu WHERE cu.table_schema = c.table_schema AND cu.table_name = c.table_name)");
                
                if (nSchemaFilter != null && nSchemaFilter.Count > 0)
                {
                    string CadenaFiltro = "";
                    foreach (var Filtro in nSchemaFilter)
                    {
                        if (CadenaFiltro != "")
                            CadenaFiltro += ", ";

                        CadenaFiltro += "'" + Filtro + "'";
                    }

                    sql.AppendLine(@"AND c.table_schema IN(" + CadenaFiltro + ")");
                }

                sql.AppendLine(@"ORDER BY c.table_schema, c.table_name");

                DataTable table = ExecuteQueryGet(sql.ToString());

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];

                    var newObjectRow = nObjectTable.NewTBL_ObjectRow();

                    newObjectRow.fk_Connection = nConnection.id_Connection;
                    newObjectRow.Catalog_Name = row["table_catalog"].ToString();
                    newObjectRow.Schema_Name = row["table_schema"].ToString();
                    newObjectRow.Object_Type = row["table_type"].ToString();
                    newObjectRow.Generic_Type = "Table";
                    newObjectRow.Object_Name = row["table_name"].ToString();
                    newObjectRow.Selected = false;

                    nObjectTable.AddTBL_ObjectRow(newObjectRow);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de tablas " + base.Connection.DataSource + " , " + ex.Message, ex);
            }
        }

        public override void FillDataTableColumns(XsdDataBase.TBL_FieldDataTable nFieldTable, XsdDataBase.TBL_RelationDataTable nRelationTable, XsdDataBase.TBL_ObjectRow nTable)
        {
            try
            {
                if (nTable.Generic_Type == "View")
                {
                    string sql = @"select column_name , nullable is_nullable , data_type , data_length character_maximum_length , data_precision numeric_precision , data_scale numeric_scale , data_type udt_name , column_id ordinal_position " + ControlChars.CrLf +
                                "from all_tab_columns av where owner = '" + nTable.Schema_Name + "' and table_name = '" + nTable.Object_Name + "'";

                    DataTable table = ExecuteQueryGet(sql);

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        nFieldTable.AddTBL_FieldRow(nTable, row["column_name"].ToString(), GetGenericParameterType(row["udt_name"].ToString()).ToString(), row["data_type"].ToString(), (row["is_nullable"].ToString().ToUpper() != "NO"), ConvertToInt(row["character_maximum_length"]), ConvertToByte(row["numeric_precision"]), ConvertToByte(row["numeric_scale"]), row["ordinal_position"].ToString(), GetGenericParameterDirection("In"));
                    }
                }
                else // Table
                {
                    string sql = @"Select * From (" + ControlChars.CrLf +
                                "SELECT col.column_name , col.is_nullable , col.data_type , col.character_maximum_length, col.numeric_precision , col.numeric_scale , col.ordinal_position , col.table_catalog , col.table_schema , col.table_name , k.ordinal_position pk_position , col.udt_name" + ControlChars.CrLf +
                                "FROM information_schema.key_column_usage k INNER JOIN information_schema.table_constraints tc ON tc.constraint_type = 'PRIMARY KEY' AND tc.constraint_name = k.constraint_name" + ControlChars.CrLf +
                                "RIGHT JOIN information_schema.columns col ON col.column_name = k.column_name  AND tc.table_name = col.table_name" + ControlChars.CrLf +
                                "WHERE col.table_catalog = '" + nTable.Catalog_Name + "' AND col.table_schema = '" + nTable.Schema_Name + "' AND col.table_name = '" + nTable.Object_Name + "'" + ControlChars.CrLf +
                                ") Tab Left Join (" + ControlChars.CrLf +
                                "SELECT    FRG_TBL.CONSTRAINT_NAME AS FRG_CONSTRAINT_NAME , FRG_TBL.TABLE_CATALOG AS FRG_TABLE_CATALOG , FRG_TBL.TABLE_SCHEMA AS FRG_TABLE_SCHEMA" + ControlChars.CrLf +
                                  ", FRG_TBL.TABLE_NAME AS FRG_TABLE_NAME , FRG_TBL.COLUMN_NAME AS FRG_COLUMN_NAME , FRG_TBL.ORDINAL_POSITION AS FRG_ORDINAL_POSITION" + ControlChars.CrLf +
                                  ", PRI_TBL.CONSTRAINT_NAME AS PRI_CONSTRAINT_NAME , PRI_TBL.CONSTRAINT_CATALOG AS PRI_CONSTRAINT_CATALOG , PRI_TBL.CONSTRAINT_SCHEMA AS PRI_CONSTRAINT_SCHEMA" + ControlChars.CrLf +
                                  ", PRI_TBL.TABLE_NAME AS PRI_TABLE_NAME , PRI_TBL.COLUMN_NAME AS PRI_COLUMN_NAME , PRI_TBL.ORDINAL_POSITION AS PRI_ORDINAL_POSITION" + ControlChars.CrLf +
                                "FROM	INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC" + ControlChars.CrLf +
                                "JOIN	INFORMATION_SCHEMA.KEY_COLUMN_USAGE FRG_TBL " + ControlChars.CrLf +
                                 "ON	FRG_TBL.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG AND FRG_TBL.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA AND FRG_TBL.CONSTRAINT_NAME = RC.CONSTRAINT_NAME" + ControlChars.CrLf +
                                "JOIN	INFORMATION_SCHEMA.KEY_COLUMN_USAGE PRI_TBL" + ControlChars.CrLf +
                                 "ON	PRI_TBL.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG " + ControlChars.CrLf +
                                   "AND	PRI_TBL.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA" + ControlChars.CrLf +
                                   "AND	PRI_TBL.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME" + ControlChars.CrLf +
                                   "AND	PRI_TBL.ORDINAL_POSITION = FRG_TBL.ORDINAL_POSITION" + ControlChars.CrLf +
                                "WHERE       FRG_TBL.CONSTRAINT_CATALOG = '" + nTable.Catalog_Name + "' AND FRG_TBL.CONSTRAINT_SCHEMA = '" + nTable.Schema_Name + "' AND" + ControlChars.CrLf +
                                            "FRG_TBL.TABLE_NAME = '" + nTable.Object_Name + "' " + ControlChars.CrLf +
                                ") Rel" + ControlChars.CrLf +
                                "On Rel.FRG_COLUMN_NAME = Tab.column_name" + ControlChars.CrLf +
                                "Order by table_catalog , table_schema , table_name , ordinal_position";

                    DataTable table = ExecuteQueryGet(sql);

                    string col = "";
                    XsdDataBase.TBL_FieldRow field = null;

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];

                        if (col != row["column_name"].ToString())
                        {
                            field = nFieldTable.AddTBL_FieldRow(nTable, row["column_name"].ToString(), GetGenericParameterType(row["udt_name"].ToString()).ToString(), row["data_type"].ToString(), (row["is_nullable"].ToString().ToUpper() != "NO"), ConvertToInt(row["character_maximum_length"]), ConvertToByte(row["numeric_precision"]), ConvertToByte(row["numeric_scale"]), row["pk_position"].ToString(), GetGenericParameterDirection("In"));
                            col = row["column_name"].ToString();
                        }

                        if (!row.IsNull("FRG_CONSTRAINT_NAME"))
                        {
                            nRelationTable.AddTBL_RelationRow(field, row["FRG_CONSTRAINT_NAME"].ToString(), row["PRI_TABLE_NAME"].ToString(), row["PRI_COLUMN_NAME"].ToString());
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de columnas " + base.Connection.DataSource + " Tabla = " + nTable.Object_Name + ", " + ex.Message, ex);
            }
        }

        public override void FillDataBaseViews(XsdDataBase.TBL_ObjectDataTable nObjectTable, XsdDataBase.TBL_ConnectionRow nConnection, List<string> nSchemaFilter)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendLine(@"SELECT c.table_catalog , c.table_schema, c.table_name, c.table_type FROM information_schema.tables c");
                sql.AppendLine(@"WHERE c.table_type = 'VIEW' AND c.table_schema NOT IN('information_schema', 'pg_catalog')");

                if (nSchemaFilter != null && nSchemaFilter.Count > 0)
                {
                    string CadenaFiltro = "";
                    foreach (var Filtro in nSchemaFilter)
                    {
                        if (CadenaFiltro != "")
                            CadenaFiltro += ", ";

                        CadenaFiltro += "'" + Filtro + "'";
                    }

                    sql.AppendLine(@"AND c.table_schema IN(" + CadenaFiltro + ")");
                }
                
                sql.AppendLine(@"ORDER BY c.table_schema, c.table_name");

                DataTable table = ExecuteQueryGet(sql.ToString());

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];

                    var newObjectRow = nObjectTable.NewTBL_ObjectRow();

                    newObjectRow.fk_Connection = nConnection.id_Connection;
                    newObjectRow.Catalog_Name = row["table_catalog"].ToString();
                    newObjectRow.Schema_Name = row["table_schema"].ToString();
                    newObjectRow.Object_Type = row["table_type"].ToString();
                    newObjectRow.Generic_Type = "View";
                    newObjectRow.Object_Name = row["table_name"].ToString();
                    newObjectRow.Selected = false;

                    nObjectTable.AddTBL_ObjectRow(newObjectRow);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de tablas " + base.Connection.DataSource + " , " + ex.Message, ex);
            }
        }

        public override void FillDataBaseStoredProcedures(XsdDataBase.TBL_ObjectDataTable nObjectTable, XsdDataBase.TBL_ConnectionRow nConnection, List<string> nSchemaFilter)
        {
            try
            {
                string sql = @"SELECT r.routine_catalog , r.routine_schema , r.routine_name , r.routine_type , 'StoredProcedure' AS generic_type" + ControlChars.CrLf +
                            "FROM information_schema.routines r" + ControlChars.CrLf +
                            "WHERE r.routine_type = 'FUNCTION' AND" + ControlChars.CrLf +
                            "r.specific_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                            "ORDER BY r.routine_schema , r.routine_name";

                DataTable table = ExecuteQueryGet(sql);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];

                    var newObjectRow = nObjectTable.NewTBL_ObjectRow();

                    newObjectRow.fk_Connection = nConnection.id_Connection;
                    newObjectRow.Catalog_Name = row["routine_catalog"].ToString();
                    newObjectRow.Schema_Name = row["routine_schema"].ToString();
                    newObjectRow.Object_Type = row["routine_type"].ToString();
                    newObjectRow.Generic_Type = "StoredProcedure";
                    newObjectRow.Object_Name = row["routine_name"].ToString();
                    newObjectRow.Selected = false;

                    nObjectTable.AddTBL_ObjectRow(newObjectRow);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de tablas " + base.Connection.DataSource + " , " + ex.Message, ex);
            }
        }

        public override void FillDataBaseParameters(XsdDataBase.TBL_FieldDataTable nFieldTable, XsdDataBase.TBL_ObjectRow nTable)
        {
            try
            {
                string sql = @"SELECT p.parameter_name ,  'NO' is_nullable , p.data_type , p.character_maximum_length, p.numeric_precision , p.numeric_scale , p.udt_name , p.ordinal_position , p.parameter_mode" + ControlChars.CrLf +
                            "FROM information_schema.routines r INNER JOIN information_schema.parameters p ON p.specific_catalog = r.specific_catalog AND p.specific_schema = r.specific_schema AND p.specific_name = r.specific_name" + ControlChars.CrLf +
                            "WHERE r.routine_type = 'FUNCTION' AND r.specific_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                            "AND r.routine_catalog = '" + nTable.Catalog_Name + "' AND r.routine_schema = '" + nTable.Schema_Name + "' AND r.routine_name = '" + nTable.Object_Name + "'" + ControlChars.CrLf +
                            "ORDER BY p.ordinal_position";

                DataTable table = ExecuteQueryGet(sql);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];
                    nFieldTable.AddTBL_FieldRow(nTable, row["parameter_name"].ToString(), GetGenericParameterType(row["udt_name"].ToString()).ToString(), row["data_type"].ToString(), (row["is_nullable"].ToString().ToUpper() != "NO"), ConvertToInt(row["character_maximum_length"]), ConvertToByte(row["numeric_precision"]), ConvertToByte(row["numeric_scale"]), row["ordinal_position"].ToString(), GetGenericParameterDirection(row["parameter_mode"].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de columnas " + base.Connection.DataSource + " Tabla = " + nTable.Object_Name + ", " + ex.Message, ex);
            }
        }

        #endregion

        #region Funciones

        public override XsdDataBaseObjects.dbschemaDataTable GetDataBaseSchemas(string nFilter)
        {
            try
            {
                var schemas = new XsdDataBaseObjects.dbschemaDataTable();

                string sql = @"SELECT c.table_catalog, c.table_schema FROM information_schema.tables c" + ControlChars.CrLf +
                            "WHERE c.table_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                            "ORDER BY c.table_schema";

                DataTable table = ExecuteQueryGet(sql);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];
                    schemas.AdddbschemaRow(row["table_catalog"].ToString(), row["table_schema"].ToString());
                }
                return schemas;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de tablas " + base.Connection.DataSource + " , " + ex.Message, ex);
            }
        }

        public override XsdDataBaseObjects.dbobjectDataTable GetDataBaseTables(string nCatalog_Name, string nSchema_Name, string nFilterTableName)
        {
            try
            {
                var dbobjects = new XsdDataBaseObjects.dbobjectDataTable();

                nFilterTableName += "%";

                string sql = @"SELECT c.table_catalog , c.table_schema, c.table_name, c.table_type FROM information_schema.tables c" + ControlChars.CrLf +
                            "WHERE c.table_type = 'BASE TABLE' AND c.table_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                            "AND  EXISTS (SELECT cu.table_name FROM information_schema.key_column_usage cu WHERE cu.table_schema = c.table_schema AND cu.table_name = c.table_name)" + ControlChars.CrLf +
                            "AND c.table_catalog = '" + nCatalog_Name + "' AND c.table_schema = '" + nSchema_Name + "'" + ControlChars.CrLf +
                            "AND c.table_name LIKE '" + nFilterTableName + "'" + ControlChars.CrLf +
                            "ORDER BY c.table_name";

                DataTable table = ExecuteQueryGet(sql);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];

                    var newdbobjectRow = dbobjects.NewdbobjectRow();

                    newdbobjectRow.generic_type = "Table";
                    newdbobjectRow.catalog_name = row["table_catalog"].ToString();
                    newdbobjectRow.schema_name = row["table_schema"].ToString();
                    newdbobjectRow.object_type = row["table_type"].ToString();
                    newdbobjectRow.object_name = row["table_name"].ToString();
                    newdbobjectRow.selected = false;
                    newdbobjectRow.mapped = false;

                    dbobjects.AdddbobjectRow(newdbobjectRow);
                }

                return dbobjects;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de tablas " + base.Connection.DataSource + " , " + ex.Message, ex);
            }
        }

        public override XsdDataBaseObjects.dbobjectDataTable GetDataBaseViews(string nCatalog_Name, string nSchema_Name, string nFilterViewName)
        {
            try
            {
                var dbobjects = new XsdDataBaseObjects.dbobjectDataTable();

                nFilterViewName = nFilterViewName.Replace("*", "");
                nFilterViewName += "%";

                string sql = @"SELECT c.table_catalog , c.table_schema, c.table_name, c.table_type FROM information_schema.tables c" + ControlChars.CrLf +
                            "WHERE c.table_type = 'VIEW' AND c.table_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                    //"AND c.table_catalog = '" + nCatalog_Name + "'" + ControlChars.CrLf +
                            "AND c.table_schema = '" + nSchema_Name + "'" + ControlChars.CrLf +
                            "AND c.table_name LIKE '" + nFilterViewName + "'" + ControlChars.CrLf +
                            "ORDER BY c.table_name";

                DataTable table = ExecuteQueryGet(sql);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];

                    var newdbobjectRow = dbobjects.NewdbobjectRow();

                    newdbobjectRow.generic_type = "Table";
                    newdbobjectRow.catalog_name = row["table_catalog"].ToString();
                    newdbobjectRow.schema_name = row["table_schema"].ToString();
                    newdbobjectRow.object_type = row["table_type"].ToString();
                    newdbobjectRow.object_name = row["table_name"].ToString();
                    newdbobjectRow.selected = false;
                    newdbobjectRow.mapped = false;

                    dbobjects.AdddbobjectRow(newdbobjectRow);
                }

                return dbobjects;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de tablas " + base.Connection.DataSource + " , " + ex.Message, ex);
            }
        }

        public override XsdDataBaseObjects.dbfieldDataTable GetDataBaseFields(string nCatalog_Name, string nSchema_Name, string nObjectName)
        {
            return null;
        }

        public override string BuildParameterName(string name)
        {
            return name;
        }

        public override DbType GetGenericParameterType(string nDataBaseParameterType)
        {
            //TODO Realizar proceso de conversion de parametro de postgres a generico
            switch (nDataBaseParameterType.ToUpper())
            {
                case "INTEGER":
                    return DbType.Int32;

                case "INT2":
                    return DbType.Int16;

                case "INT4":
                    return DbType.Int32;

                case "INT8":
                    return DbType.Int64;

                case "BOOL":
                    return DbType.Boolean;

                case "TIMESTAMP":
                    return DbType.DateTime;

                case "VARCHAR":
                case "VARCHAR2":
                case "TEXT":
                case "CHARACTER VARYING":
                    return DbType.String;

                default:
                    throw new Exception("Tipo de dato no reconocido por asistente de Postgress, " + nDataBaseParameterType);
            }
        }

        public override string GetGenericParameterDirection(string nDataBaseParameterDirection)
        {
            //TODO Realizar proceso de conversion de parametro de postgres a generico
            switch (nDataBaseParameterDirection.ToUpper())
            {
                case "IN":
                    return System.Data.ParameterDirection.Input.ToString();

                case "OUT":
                    return System.Data.ParameterDirection.Output.ToString();

                default:
                    throw new Exception("Tipo de dirección no reconocido por asistente de Postgress, " + nDataBaseParameterDirection);
            }
        }

        public override DbParameter ConfigureParameter(DbParameter param, Parameter nParameter)
        {
            var p = (Npgsql.NpgsqlParameter)param;

            switch (p.DbType)
            {
                case DbType.Binary:
                case DbType.Byte:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                    break;

                case DbType.Boolean:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean;
                    break;

                case DbType.Currency:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Money;
                    break;

                case DbType.DateTime:
                case DbType.Date:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Timestamp;
                    break;

                case DbType.VarNumeric:
                case DbType.Decimal:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Numeric;
                    break;

                case DbType.Int32:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer;
                    break;

                case DbType.UInt64:
                case DbType.Int64:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bigint;
                    break;

                case DbType.Object:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Array;
                    break;

                case DbType.Int16:
                case DbType.SByte:
                case DbType.Single:
                case DbType.UInt16:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Smallint;
                    break;

                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Guid:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
                    break;

                case DbType.Time:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Time;
                    break;

                case DbType.Xml:
                    p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Xml;
                    break;
            }

            return p;
        }

        public override string FormatToDatabaseStoredProcedureName(string nSchemaName, string nTableName)
        {
            if (nSchemaName != "")
            {
                return Identifier_Procedure_Prefix + nSchemaName + Identifier_Procedure_Postfix + Identifier_Schema_Separator + Identifier_Procedure_Prefix + nTableName + Identifier_Procedure_Postfix;
            }
            else
            {
                return Identifier_Procedure_Prefix + nTableName + Identifier_Procedure_Postfix;
            }
        }

        public override string ConvertSqlSelectToMaxRows(string sql, int nMaxRows)
        {
            if (nMaxRows > 0)
                return sql + "  LIMIT " + nMaxRows;
            else
                return sql;
        }

        public override string ConvertSqlSelectToOrderBy(string sql, ColumnEnumList GroupByParams)
        {
            if (GroupByParams != null && GroupByParams.Count > 0)
                return sql + " ORDER BY " + GroupByParams.getOrderByParams(this);
            else
                return sql;
        }

        public override string GetDataFilterString(DataFilterType type)
        {
            switch (type)
            {
                case DataFilterType.AD: return "(A|B|C|D|a|b|c|d)**";
                case DataFilterType.EH: return "(E|F|G|H|e|f|g|h)**";
                case DataFilterType.IL: return "(I|J|K|L|i|j|k|l)**";
                case DataFilterType.MP: return "(M|N|O|P|m|n|o|p)**";
                case DataFilterType.QT: return "(Q|R|S|T|q|r|s|t)**";
                case DataFilterType.UZ: return "(U|V|W|X|Y|Z|u|v|w|x|y|z)**";
                case DataFilterType._All: return "*";
                default:
                    return "";
            }
        }

        public override string ConvertToBinaryString(object nValue)
        {
            return Identifier_String_Symbol + System.BitConverter.ToString((byte[])nValue).Replace("-", "") + Identifier_String_Symbol;
        }

        public override string FormatToDatabaseEspecialValue(EspecialescargomasterNullable Value)
        {
            switch (Value)
            {
                case EspecialescargomasterNullable.DBUser:
                    return "current_user";

                case EspecialescargomasterNullable.SysDate:
                    return "now";

                default:
                    return "";
            }
        }

        public override string GetConnectionStringCatalogName(string nConnectionString)
        {
            throw new Exception("Funcionalidad no disponible para postgres driver");
        }

        #endregion
    }
}