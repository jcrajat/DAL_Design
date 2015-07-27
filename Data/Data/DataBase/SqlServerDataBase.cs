using System;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using CM.Tools.Misellaneous;
using CMData.Schemas;
using Tools;
using CMData.Manager;

namespace CMData.DataBase
{
    /// <summary>
    /// Clase base de conexión a bases de datos SQL Server
    /// </summary>
    internal class SqlServerDataBase : DataBase
    {
        #region Constructores

        public SqlServerDataBase(string nConnectionString)
        {
            DefaultFactory = System.Data.SqlClient.SqlClientFactory.Instance;
            base.Initialize(nConnectionString);
        }

        #endregion

        #region Propiedades

        public override DataBaseType DataBaseType
        {
            get
            {
                return DataBaseType.SqlServer;
            }
        }

        public override string Identifier_Table_Prefix
        {
            get
            {
                return "[";
            }
        }

        public override string Identifier_Table_Postfix
        {
            get
            {
                return "]";
            }
        }

        #endregion

        #region Metodos

        public override void DeriveParameters(ref DbCommand cmd)
        {
            var SqlCnn = new SqlConnection(this.ConnectionString);
            SqlCnn.Open();
            SqlCommandBuilder.DeriveParameters((SqlCommand)cmd);
            SqlCnn.Close();
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
                string sql = @"Select * From (" + ControlChars.CrLf +
                            "SELECT col.column_name , col.is_nullable , col.data_type , col.character_maximum_length, col.numeric_precision , col.numeric_scale , col.ordinal_position , col.table_catalog , col.table_schema , col.table_name , k.ordinal_position pk_position " + ControlChars.CrLf +
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
                        field = nFieldTable.AddTBL_FieldRow(nTable, row["column_name"].ToString(), GetGenericParameterType(row["data_type"].ToString()).ToString(), row["data_type"].ToString(), (row["is_nullable"].ToString().ToUpper() != "NO"), ToInt(row["character_maximum_length"]), ToByte(row["numeric_precision"]), ToByte(row["numeric_scale"]), row["pk_position"].ToString(), GetGenericParameterDirection("In"));
                        col = row["column_name"].ToString();
                    }

                    if (!row.IsNull("FRG_CONSTRAINT_NAME"))
                    {
                        nRelationTable.AddTBL_RelationRow(field, row["FRG_CONSTRAINT_NAME"].ToString(), row["PRI_TABLE_NAME"].ToString(), row["PRI_COLUMN_NAME"].ToString());
                    }

                }
                //}
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
                var sql = new StringBuilder();
                sql.AppendLine(@"SELECT r.routine_catalog , r.routine_schema , r.routine_name , r.routine_type , 'StoredProcedure' AS generic_type");
                sql.AppendLine(@"FROM information_schema.routines r");
                sql.AppendLine(@"WHERE r.routine_type = 'PROCEDURE' AND");
                sql.AppendLine(@"r.specific_schema NOT IN('information_schema', 'pg_catalog')");
                
                if (nSchemaFilter != null && nSchemaFilter.Count > 0)
                {
                    string CadenaFiltro = "";
                    foreach (var Filtro in nSchemaFilter)
                    {
                        if (CadenaFiltro != "")
                            CadenaFiltro += ", ";

                        CadenaFiltro += "'" + Filtro + "'";
                    }

                    sql.AppendLine(@"AND r.routine_schema IN(" + CadenaFiltro + ")");
                }

                sql.AppendLine(@"ORDER BY r.routine_schema , r.routine_name");

                DataTable table = ExecuteQueryGet(sql.ToString());

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
                string sql = @"SELECT p.parameter_name ,  'NO' is_nullable , p.data_type , p.character_maximum_length, p.numeric_precision , p.numeric_scale , p.ordinal_position , p.parameter_mode" + ControlChars.CrLf +
                            "FROM information_schema.routines r INNER JOIN information_schema.parameters p ON p.specific_catalog = r.specific_catalog AND p.specific_schema = r.specific_schema AND p.specific_name = r.specific_name" + ControlChars.CrLf +
                            "WHERE r.routine_type = 'PROCEDURE' AND r.specific_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                            "AND r.routine_catalog = '" + nTable.Catalog_Name + "' AND r.routine_schema = '" + nTable.Schema_Name + "' AND r.routine_name = '" + nTable.Object_Name + "'" + ControlChars.CrLf +
                            "ORDER BY p.ordinal_position";

                DataTable table = ExecuteQueryGet(sql);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];
                    nFieldTable.AddTBL_FieldRow(nTable, row["parameter_name"].ToString(), GetGenericParameterType(row["data_type"].ToString()).ToString(), row["data_type"].ToString(), (row["is_nullable"].ToString().ToUpper() != "NO"), ConvertToInt(row["character_maximum_length"]), ConvertToByte(row["numeric_precision"]), ConvertToByte(row["numeric_scale"]), row["ordinal_position"].ToString(), GetGenericParameterDirection(row["parameter_mode"].ToString()));
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

                nFilter = nFilter.Replace("*", "");
                nFilter += "%";

                string sql = @"SELECT Distinct c.table_catalog, c.table_schema FROM information_schema.tables c" + ControlChars.CrLf +
                            "WHERE c.table_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                            "AND c.table_schema LIKE '" + nFilter + "'" + ControlChars.CrLf +
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

                nFilterTableName = nFilterTableName.Replace("*", "");
                nFilterTableName += "%";

                string sql = "";

                if (nSchema_Name != "")
                {
                    sql = @"SELECT c.table_catalog , c.table_schema, c.table_name, c.table_type FROM information_schema.tables c" + ControlChars.CrLf +
                            "WHERE c.table_type = 'BASE TABLE' AND c.table_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                            "AND  EXISTS (SELECT cu.table_name FROM information_schema.key_column_usage cu WHERE cu.table_schema = c.table_schema AND cu.table_name = c.table_name)" + ControlChars.CrLf +
                        //"AND c.table_catalog = '" + nCatalog_Name + "'" + ControlChars.CrLf +
                            "AND c.table_schema = '" + nSchema_Name + "'" + ControlChars.CrLf +
                            "AND c.table_name LIKE '" + nFilterTableName + "'" + ControlChars.CrLf +
                            "ORDER BY c.table_name";
                }
                else
                {
                    sql = @"SELECT c.table_catalog , c.table_schema, c.table_name, c.table_type FROM information_schema.tables c" + ControlChars.CrLf +
                            "WHERE c.table_type = 'BASE TABLE' AND c.table_schema NOT IN('information_schema', 'pg_catalog')" + ControlChars.CrLf +
                            "AND  EXISTS (SELECT cu.table_name FROM information_schema.key_column_usage cu WHERE cu.table_schema = c.table_schema AND cu.table_name = c.table_name)" + ControlChars.CrLf +
                        //"AND c.table_catalog = '" + nCatalog_Name + "'" + ControlChars.CrLf +
                            "AND c.table_name LIKE '" + nFilterTableName + "'" + ControlChars.CrLf +
                            "ORDER BY c.table_name";
                }

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

                //sql = sql.Replace(';', ' ').Replace("--", " ");

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

        public override XsdDataBaseObjects.dbfieldDataTable GetDataBaseFields(string nCatalog_Name, string nSchema_Name, string nObject_Name)
        {
            try
            {
                var dbfields = new XsdDataBaseObjects.dbfieldDataTable();

                string sql = @"Select * From (" + ControlChars.CrLf +
                            "SELECT col.column_name , col.is_nullable , col.data_type , col.character_maximum_length, col.numeric_precision , col.numeric_scale , col.ordinal_position , col.table_catalog , col.table_schema , col.table_name , k.ordinal_position pk_position " + ControlChars.CrLf +
                            "FROM information_schema.key_column_usage k INNER JOIN information_schema.table_constraints tc ON tc.constraint_type = 'PRIMARY KEY' AND tc.constraint_name = k.constraint_name" + ControlChars.CrLf +
                            "RIGHT JOIN information_schema.columns col ON col.column_name = k.column_name  AND tc.table_name = col.table_name" + ControlChars.CrLf +
                            "WHERE col.table_schema = '" + nSchema_Name + "'" +
                            " AND col.table_name = '" + nObject_Name + "'" + ControlChars.CrLf +
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
                            " WHERE FRG_TBL.CONSTRAINT_SCHEMA = '" + nSchema_Name + "' AND" + ControlChars.CrLf +
                                        "FRG_TBL.TABLE_NAME = '" + nObject_Name + "' " + ControlChars.CrLf +
                            ") Rel" + ControlChars.CrLf +
                            "On Rel.FRG_COLUMN_NAME = Tab.column_name" + ControlChars.CrLf +
                            "Order by table_catalog , table_schema , table_name , ordinal_position";

                //sql = sql.Replace(';', ' ').Replace("--", " ");
                DataTable table = ExecuteQueryGet(sql);

                string col = "";
                bool is_foreign_key = false;

                XsdDataBaseObjects.dbfieldRow field = null;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];

                    is_foreign_key = false;
                    if (col != row["column_name"].ToString())
                    {
                        if (!row.IsNull("FRG_CONSTRAINT_NAME"))
                            is_foreign_key = true;

                        field = dbfields.AdddbfieldRow(nCatalog_Name, nSchema_Name, nObject_Name, row["column_name"].ToString(), GetGenericParameterType(row["data_type"].ToString()).ToString(), row["data_type"].ToString(), (row["is_nullable"].ToString().ToUpper() != "NO"), ToInt(row["character_maximum_length"]), ToByte(row["numeric_precision"]), ToByte(row["numeric_scale"]), row["pk_position"].ToString(), GetGenericParameterDirection("In"), "", "", is_foreign_key);
                        col = row["column_name"].ToString();
                    }


                    //if (!row.IsNull("FRG_CONSTRAINT_NAME"))
                    //{
                    //    nRelationTable.AddTBL_RelationRow(field, row["FRG_CONSTRAINT_NAME"].ToString(), row["PRI_TABLE_NAME"].ToString(), row["PRI_COLUMN_NAME"].ToString());
                    //}

                }

                return dbfields;

            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el listado de columnas " + base.Connection.DataSource + " Tabla = " + nObject_Name + ", " + ex.Message, ex);
            }
        }

        public override string BuildParameterName(string name)
        {
            return "@" + name.Replace("@", "");
        }

        public int ToInt(object nValue)
        {
            try
            {
                return Convert.ToInt32(nValue);
            }
            catch
            {
                return 0;
            }
        }

        public byte ToByte(object nValue)
        {
            try
            {
                return Convert.ToByte(nValue);
            }
            catch
            {
                return 0;
            }
        }

        public override DbType GetGenericParameterType(string nDataBaseParameterType)
        {
            switch (nDataBaseParameterType.ToLower())
            {
                case "bit":
                    return DbType.Boolean;

                case "text":
                case "nvarchar":
                case "varchar":
                case "xml":
                case "nchar":
                case "char":
                    return DbType.String;

                case "smalldatetime":
                    return DbType.DateTime;

                case "datetime":
                    return DbType.DateTime;

                case "int":
                    return DbType.Int32;

                case "bigint":
                    return DbType.Int64;

                case "numeric":
                case "money":
                    return DbType.Decimal;

                case "smallint":
                    return DbType.Int16;

                case "tinyint":
                    return DbType.Byte;

                case "uniqueidentifier":
                    return DbType.Guid;

                case "image":
                case "varbinary":
                    return DbType.Binary;

                case "float":
                    return DbType.Single;

                case "decimal":
                    return DbType.Decimal;

                case "sql_variant":
                    return DbType.Object;

                default:
                    throw new Exception("Tipo de dato no reconocido por asistente de SqlServer, " + nDataBaseParameterType);
            }
        }

        public override string GetGenericParameterDirection(string nDataBaseParameterDirection)
        {
            switch (nDataBaseParameterDirection.ToUpper())
            {
                case "IN":
                    return System.Data.ParameterDirection.Input.ToString();

                case "OUT":
                    return System.Data.ParameterDirection.Output.ToString();

                default:
                    throw new Exception("Tipo de dirección no reconocido por asistente de SqlServer, " + nDataBaseParameterDirection);
            }
        }

        public override DbParameter ConfigureParameter(DbParameter param, Parameter nParameter)
        {
            SqlParameter p = (SqlParameter)param;

            switch (p.DbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                    p.SqlDbType = SqlDbType.VarChar;
                    break;

                case DbType.Binary:
                    p.SqlDbType = SqlDbType.VarBinary;
                    break;

                case DbType.Boolean:
                    p.SqlDbType = SqlDbType.Bit;
                    break;

                case DbType.Byte:
                    p.SqlDbType = SqlDbType.TinyInt;
                    break;

                case DbType.Currency:
                    p.SqlDbType = SqlDbType.Money;
                    break;

                case DbType.DateTime:
                case DbType.Date:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    p.SqlDbType = SqlDbType.DateTime;
                    break;

                case DbType.Decimal:
                    p.SqlDbType = SqlDbType.Decimal;
                    break;

                case DbType.Guid:
                    p.SqlDbType = SqlDbType.VarChar;
                    break;

                case DbType.Int16:
                    p.SqlDbType = SqlDbType.SmallInt;
                    break;

                case DbType.Int32:
                    p.SqlDbType = SqlDbType.Int;
                    break;

                case DbType.Int64:
                    p.SqlDbType = SqlDbType.BigInt;
                    break;

                case DbType.Object:
                    p.SqlDbType = SqlDbType.Binary;
                    break;

                case DbType.SByte:
                    p.SqlDbType = SqlDbType.SmallInt;
                    break;

                case DbType.Single:
                    p.SqlDbType = SqlDbType.Float;
                    break;

                case DbType.String:
                    p.SqlDbType = SqlDbType.VarChar;
                    break;

                case DbType.StringFixedLength:
                    p.SqlDbType = SqlDbType.VarChar;
                    break;

                case DbType.Time:
                    p.SqlDbType = SqlDbType.Time;
                    break;

                case DbType.UInt16:
                    p.SqlDbType = SqlDbType.SmallInt;
                    break;

                case DbType.UInt32:
                    p.SqlDbType = SqlDbType.Int;
                    break;

                case DbType.UInt64:
                    p.SqlDbType = SqlDbType.BigInt;
                    break;

                case DbType.VarNumeric:
                    p.SqlDbType = SqlDbType.Binary;
                    break;

                case DbType.Xml:
                    p.SqlDbType = SqlDbType.Xml;
                    break;
            }
            return p;
        }

        public override string ConvertSqlSelectToMaxRows(string sql, int nMaxRows)
        {
            if (nMaxRows > 0)
            {
                string strSentence = sql.Substring(6, sql.Length - 6);
                return "SELECT TOP " + nMaxRows + " " + strSentence;
            }
            else
            {
                return sql;
            }
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
                case DataFilterType.AD: return "[A-D]**";
                case DataFilterType.EH: return "[E-H]**";
                case DataFilterType.IL: return "[I-L]**";
                case DataFilterType.MP: return "[M-P]**";
                case DataFilterType.QT: return "[Q-T]**";
                case DataFilterType.UZ: return "[U-Z]**";
                case DataFilterType._All: return "*";
                default:
                    return "";
            }
        }

        public override string ConvertToBinaryString(object nValue)
        {
            return "0x" + System.BitConverter.ToString((byte[])nValue).Replace("-", "");
        }

        public override string FormatToDatabaseEspecialValue(EspecialescargomasterNullable Value)
        {
            switch (Value)
            {
                case EspecialescargomasterNullable.DBUser:
                    return "CURRENT_USER";

                case EspecialescargomasterNullable.SysDate:
                    return "GETDATE()";

                default:
                    return "";
            }
        }

        public override string GetConnectionStringCatalogName(string nConnectionString)
        {
            try
            {
                string[] stringParts = nConnectionString.Split(';');

                foreach (var part in stringParts)
                {
                    string[] pairValue = part.Split('=');

                    if (pairValue[0].Trim().ToUpper() == "INITIAL CATALOG")
                        return pairValue[1].Trim();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la url de remoting, " + ex.Message, ex);
            }

            return "";
        }

        #endregion
    }
}