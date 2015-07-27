using System;
using System.Text;
using CMData.Schemas;
using System.Collections.Generic;
using System.Collections.Specialized;
using CMData.Manager;

namespace CMData.Utils
{
    /// <summary>
    /// Clase que construye las sentencias SQL
    /// </summary>
    public class SqlBuilder
    {
        #region Propiedades

        public DataBase.DataBase DataBase { get; private set; }
        
        #endregion

        #region Constructores

        /// <summary>
        /// Crea una nueva instancia de la clase
        /// </summary>
        /// <param name="nDataBase">Base de datos para la cual se construyen las sentencias</param>
        public SqlBuilder(DataBase.DataBase nDataBase)
        {
            this.DataBase = nDataBase;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Construye la sentencia SQL para calcular el NextId de una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <param name="nIdColumn">Campo a calcular</param>
        /// <returns>Sentencia SQL</returns>
        public string SqlNextId(string nSchemaName, string nDataTableName, List<Parameter> nKeys, string nIdColumn)
        {
            try
            {
                string filters = "";
                string sql = "";

                for (int i = 0; i < nKeys.Count; i++)
                {
                    if (filters == "")
                        filters = " Where ";
                    else
                        filters += " And ";

                    filters += this.DataBase.FormatToDatabaseColumnName(nKeys[i].Name) + " = " + this.DataBase.FormatToDatabaseStringValue(nKeys[i], true);
                }

                sql = "Select Max ( " + this.DataBase.FormatToDatabaseColumnName(nIdColumn) + " ) From " + this.DataBase.FormatToDatabaseTableName(nSchemaName, nDataTableName) + filters;

                this.DataBase.LastQuery = sql;
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la sentencia de enumeracion, " + ex.Message);
            }
        }

        /// <summary>
        /// Construye la sentencia SQL para insertar un nuevo registro a una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nInParams"></param>
        /// <returns>Sentencia SQL</returns>
        public string SqlInsert(string nSchemaName, string nDataTableName, List<Parameter> nInParams, bool IsBulkQuery)
        {
            try
            {
                string columns = "";
                string values = "";
                string sql = "";

                for (int i = 0; i < nInParams.Count; i++)
                {
                    //if (!DBNulls.IsNull(nInParams[i].Value))
                    if (nInParams[i].Value != null && !DBNulls.IsNull(nInParams[i].Value) && !DBNulls.IgnoreInsert(nInParams[i].Value))
                    {
                        if (columns != "")
                        {
                            columns += ", ";
                            values += ", ";
                        }

                        columns += this.DataBase.FormatToDatabaseColumnName(nInParams[i].Name);
                        values += this.DataBase.FormatToDatabaseStringValue(nInParams[i], false);
                    }
                }

                if (IsBulkQuery)
                    sql = "Insert Into " + this.DataBase.FormatToDatabaseTableName(nSchemaName, nDataTableName) + " Values ( " + values + " )";
                else
                    sql = "Insert Into " + this.DataBase.FormatToDatabaseTableName(nSchemaName, nDataTableName) + " ( " + columns + " ) Values ( " + values + " )";

                this.DataBase.LastQuery = sql;
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la sentencia de insercion, " + ex.Message);
            }
        }

        /// <summary>
        /// Construye la sentencia SQL para actualizar la data de un registro de una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <param name="nInParams"></param>
        /// <returns>Sentencia SQL</returns>
        public string SqlUpdate(string nSchemaName, string nDataTableName, List<Parameter> nKeys, List<Parameter> nInParams)
        {
            try
            {
                string filters = "";
                string sets = "";
                string sql = "";

                for (int i = 0; i < nInParams.Count; i++)
                {
                    if (nInParams[i].Value != null && !DBNulls.IsNull(nInParams[i].Value) && !DBNulls.IgnoreUpdate(nInParams[i].Value))
                    {
                        if (sets != "")
                            sets += ", ";

                        sets += this.DataBase.FormatToDatabaseColumnName(nInParams[i].Name) + " = " + this.DataBase.FormatToDatabaseStringValue(nInParams[i], false);
                    }
                }

                if (sets == "")
                    throw new Exception("No hay datos para actualizar");

                for (int i = 0; i < nKeys.Count; i++)
                {
                    if (nKeys[i].Value != null && !DBNulls.IsNull(nKeys[i].Value))
                    {
                        if (filters == "")
                            filters = " Where ";
                        else
                            filters += " And ";

                        filters += this.DataBase.FormatToDatabaseColumnName(nKeys[i].Name) + " = " + this.DataBase.FormatToDatabaseStringValue(nKeys[i], true);
                    }
                }

                sql = "Update " + this.DataBase.FormatToDatabaseTableName(nSchemaName, nDataTableName) + " set " + sets + filters;

                this.DataBase.LastQuery = sql;
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la sentencia de actualizacion, " + nDataTableName + " , " + ex.Message);
            }
        }

        /// <summary>
        /// Construye la sentencia SQL para eliminar un registro de una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <returns>Sentencia SQL</returns>
        public string SqlDelete(string nSchemaName, string nDataTableName, List<Parameter> nKeys)
        {
            try
            {
                string filters = "";
                string sql = "";

                for (int i = 0; i < nKeys.Count; i++)
                {
                    if (nKeys[i].Value != null && !DBNulls.IsNull(nKeys[i].Value))
                    {
                        if (filters == "")
                            filters = " Where ";
                        else
                            filters += " And ";

                        filters += this.DataBase.FormatToDatabaseColumnName(nKeys[i].Name) + " = " + this.DataBase.FormatToDatabaseStringValue(nKeys[i], true);
                    }
                }

                sql = "Delete From " + this.DataBase.FormatToDatabaseTableName(nSchemaName, nDataTableName) + filters;

                this.DataBase.LastQuery = sql;
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la sentencia de eliminacion, " + nDataTableName + " , " + ex.Message);
            }
        }

        /// <summary>
        /// Construye la sentencia SQL para obtener los registro de una tabla filtrando por la llave primaria
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Sentencia SQL</returns>
        public string SqlGet(string nSchemaName, string nDataTableName, List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            try
            {
                string filters = "";
                string sql = "";

                for (int i = 0; i < nKeys.Count; i++)
                {
                    if (nKeys[i].Value != null && !DBNulls.IsNull(nKeys[i].Value))
                    {
                        if (filters == "")
                            filters += " Where ";
                        else
                            filters += " And ";

                        filters += this.DataBase.FormatToDatabaseColumnName(nKeys[i].Name) + " " + this.DataBase.FormatToComparisonOperator(nKeys[i]) + " " + this.DataBase.FormatToDatabaseStringValue(nKeys[i], true);
                    }
                }

                sql = "Select * From " + this.DataBase.FormatToDatabaseTableName(nSchemaName, nDataTableName) + filters;

                sql = this.DataBase.ConvertSqlSelectToMaxRows(sql, nMaxRows);
                sql = this.DataBase.ConvertSqlSelectToOrderBy(sql, nOrderByParams);

                this.DataBase.LastQuery = sql;
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la sentencia de consulta, " + ex.Message);
            }
        }

        /// <summary>
        /// Construye la sentencia SQL para obtener los registro de una tabla filtrando por diferentes campos
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Sentencia SQL</returns>
        public string SqlFilter(string nSchemaName, string nDataTableName, List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            try
            {
                string filters = "";
                string filterGroupString = "";
                string sql = "";
                int itemsByGroup = 0;

                int groupCount = GetGroupCount(nKeys);
                for (int group = 1; group <= groupCount; group++)
                {
                    filterGroupString = "";
                    itemsByGroup = 0;

                    for (int i = 0; i < nKeys.Count; i++)
                    {
                        if (nKeys[i].FilterGroup == group)
                        {
                            if (nKeys[i].Value != null && !DBNulls.IsNull(nKeys[i].Value))
                            {
                                if (filterGroupString == "")
                                {
                                    if (filters != "")
                                        filterGroupString = " And (";
                                    else
                                        filterGroupString = "Where (";

                                    itemsByGroup++;
                                }
                                else
                                {
                                    filterGroupString += " " + nKeys[i].FilterOption.ToString() + " ";
                                }

                                filterGroupString += this.DataBase.FormatToDatabaseColumnName(nKeys[i].Name) + " " + this.DataBase.FormatToComparisonOperator(nKeys[i]) + " " + this.DataBase.FormatToDatabaseStringValue(nKeys[i], true);
                            }
                        }
                    }

                    if (filterGroupString != "") filters = filters + filterGroupString + ")";
                }

                sql = "Select * From " + this.DataBase.FormatToDatabaseTableName(nSchemaName, nDataTableName) + filters;

                sql = this.DataBase.ConvertSqlSelectToMaxRows(sql, nMaxRows);
                sql = this.DataBase.ConvertSqlSelectToOrderBy(sql, nOrderByParams);

                this.DataBase.LastQuery = sql;
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la sentencia de consulta, " + ex.Message);
            }
        }

        /// <summary>
        /// Construye la sentencia SQL para obtener los registros de varias tablas filtrando por diferentes campos
        /// </summary>
        /// <param name="nTableAlias">Alias usado para la tabla principal</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <param name="nTableRelationList">Lista de relaciones con otras tablas</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Sentencia SQL</returns>
        public string SqlQuery(string nTableAlias, string nSchemaName, string nDataTableName, List<Parameter> nKeys, TableRelationCollection nTableRelationList, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            try
            {
                string filters = "";
                string sql = "";

                for (int i = 0; i < nKeys.Count; i++)
                {
                    if (nKeys[i].Value != null && !DBNulls.IsNull(nKeys[i].Value))
                    {
                        if (filters == "")
                            filters += " Where ";
                        else
                            filters += " And ";

                        filters += "main." + this.DataBase.FormatToDatabaseColumnName(nKeys[i].Name) + " " + this.DataBase.FormatToComparisonOperator(nKeys[i]) + " " + this.DataBase.FormatToDatabaseStringValue(nKeys[i], true);
                    }
                }

                //Crear Joins
                var tablesToJoin = new TableRelationCollection();
                if (nTableRelationList != null)
                    tablesToJoin = nTableRelationList;

                var aliasList = new NameValueCollection();
                int aliasIndex = 0;

                //Crear las lista de alias
                aliasList.Set(nTableAlias + nSchemaName + nDataTableName, "main");
                foreach (var columnToJoin in nTableRelationList)
                {
                    string alias = aliasList.Get(columnToJoin.ForeignRelationAlias + columnToJoin.ForeignSchemaName + columnToJoin.ForeignTableName);
                    if (alias == null)
                    {
                        alias = " tab" + (++aliasIndex);
                        aliasList.Set(columnToJoin.ForeignRelationAlias + columnToJoin.ForeignSchemaName + columnToJoin.ForeignTableName, alias);
                    }
                }

                var joinList = new NameValueCollection();
                var selectColumnList = new StringCollection();
                selectColumnList.Add("main.*");

                foreach (var columnToJoin in tablesToJoin)
                {
                    string alias = aliasList.Get(columnToJoin.ForeignRelationAlias + columnToJoin.ForeignSchemaName + columnToJoin.ForeignTableName);
                    string pairJoinAlias = aliasList.Get(columnToJoin.MainRelationAlias + columnToJoin.MainSchemaName + columnToJoin.MainTableName);
                    if (pairJoinAlias == null)
                    {
                        throw new Exception("No se encontró la tabla para crear el join, " + columnToJoin.MainSchemaName + "." + columnToJoin.MainTableName);
                    }

                    string strJoin = joinList.Get(columnToJoin.ForeignRelationAlias + columnToJoin.ForeignSchemaName + columnToJoin.ForeignTableName);
                    if (strJoin == null)
                    {
                        strJoin = " " + columnToJoin.GetJoinString() + " " + this.DataBase.FormatToDatabaseTableName(columnToJoin.ForeignSchemaName, columnToJoin.ForeignTableName) + " as " + alias +
                            " on " + alias + "." + this.DataBase.FormatToDatabaseColumnName(columnToJoin.ForeignColumnName) + " = " + pairJoinAlias + "." + this.DataBase.FormatToDatabaseColumnName(columnToJoin.MainColumnName);
                    }
                    else
                    {
                        strJoin += " and " + alias + "." + this.DataBase.FormatToDatabaseColumnName(columnToJoin.ForeignColumnName) + " = " + pairJoinAlias + "." + this.DataBase.FormatToDatabaseColumnName(columnToJoin.MainColumnName);
                    }

                    if (columnToJoin.ForeignFilterTextValue != "")
                    {
                        var columnsToFilter = columnToJoin.ForeignColumnsToResult.Split(',');
                        foreach (string textColumn in columnsToFilter)
                        {
                            strJoin += " and " + alias + "." + this.DataBase.FormatToDatabaseColumnName(textColumn) + " " + this.DataBase.FormatToComparisonOperator(columnToJoin.ForeignFilterTextValue) + " " + this.DataBase.FormatStringToDatabaseStringValue(columnToJoin.ForeignFilterTextValue, true);
                        }
                    }

                    joinList.Set(columnToJoin.ForeignRelationAlias + columnToJoin.ForeignSchemaName + columnToJoin.ForeignTableName, strJoin);

                    var columnsToSelect = columnToJoin.MainColumnsToResult.Split(',');
                    foreach (string textColumn in columnsToSelect)
                    {
                        if (pairJoinAlias != "main" && textColumn.Trim() != "")
                        {
                            string colName = pairJoinAlias + "." + this.DataBase.FormatToDatabaseColumnName(textColumn);
                            if (!selectColumnList.Contains(", " + colName))
                                selectColumnList.Add(", " + colName);
                        }
                    }

                    columnsToSelect = columnToJoin.ForeignColumnsToResult.Split(',');
                    foreach (string textColumn in columnsToSelect)
                    {
                        if (textColumn.Trim() != "")
                        {
                            string colName = alias + "." + this.DataBase.FormatToDatabaseColumnName(textColumn);
                            if (!selectColumnList.Contains(", " + colName))
                                selectColumnList.Add(", " + colName);
                        }
                    }
                }

                string joinString = "";
                foreach (var join in joinList)
                {
                    joinString += joinList[join.ToString()];
                }

                string selectString = "";
                foreach (var sel in selectColumnList)
                {
                    selectString += sel;
                }


                sql = "Select " + selectString + " From " + this.DataBase.FormatToDatabaseTableName(nSchemaName, nDataTableName) + " As main " + joinString + filters;

                sql = this.DataBase.ConvertSqlSelectToMaxRows(sql, nMaxRows);
                sql = this.DataBase.ConvertSqlSelectToOrderBy(sql, nOrderByParams);

                this.DataBase.LastQuery = sql;
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la sentencia de consulta, " + ex.Message);
            }
        }

        /// <summary>
        /// Calcula el número de agrupaciones
        /// </summary>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <returns>Número de grupos</returns>
        public int GetGroupCount(List<Parameter> nKeys)
        {
            int groupCount = 1;
            for (int i = 0; i < nKeys.Count; i++)
            {
                if (nKeys[i].FilterGroup > groupCount)
                    groupCount = nKeys[i].FilterGroup;
            }

            return groupCount;
        }

        #endregion
    }
}
