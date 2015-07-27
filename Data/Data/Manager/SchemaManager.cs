using System;
using System.Data;
using System.Collections.Generic;
using CMData.Schemas;

namespace CMData.Manager
{
    /// <summary>
    /// Clase base de las funcionalidades de esquema de base de datos
    /// </summary>
    public abstract class SchemaManager : MarshalByRefObject
    {        
        #region Constructores

        /// <summary>
        /// Crea una nueva instancia de la clase
        /// </summary>
        /// <param name="nDbManager">DBManager que administra el esquema</param>
        protected SchemaManager(DBManager nDbManager)
        {
            this.DbManager = nDbManager;
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// DBManager que administra el esquema
        /// </summary>
        public DBManager DbManager { get; protected set; }

        /// <summary>
        /// Alias del esquema administrado
        /// </summary>
        public string SchemaAlias { get; protected set; }
        
        /// <summary>
        /// Nombre del esquema administrado
        /// </summary>
        public string SchemaName 
        {
            get { return this.DbManager.SchemaMaping[this.SchemaAlias]; }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Permite insertar un nuevo registro en una tabla determinada
        /// </summary>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nInParams">Valores a insertar</param>
        internal void DBInsert(string nDataTableName, List<Parameter> nInParams)
        {
            bool Result;
            Exception Exception;

            this.DbManager.DataBase.DBInsert(this.SchemaName, nDataTableName, nInParams, out Result, out Exception);
            if (!Result) throw Exception;
        }


        /// <summary>
        /// Permite actualizar la data de un registro de una tabla
        /// </summary>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nInParams">Campos a actualizar</param>
        internal void DBUpdate(string nDataTableName, List<Parameter> nKeys, List<Parameter> nInParams)
        {
            bool Result;
            Exception Exception;

            this.DbManager.DataBase.DBUpdate(this.SchemaName, nDataTableName, nKeys, nInParams, out Result, out Exception);
            if (!Result) throw Exception;
        }


        /// <summary>
        /// Permite eliminar un registro de una tabla
        /// </summary>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        internal void DBDelete(string nDataTableName, List<Parameter> nKeys)
        {
            bool Result;
            Exception Exception;

            this.DbManager.DataBase.DBDelete(this.SchemaName, nDataTableName, nKeys, out Result, out Exception);
            if (!Result) throw Exception;
        }


        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de un DataTable existente, filtrando por diferentes campos 
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        internal void DBFilterFill(DataTable nDataTable, string nDataTableName, List<Parameter> nKeys)
        {
            DBFilterFill(nDataTable, nDataTableName, nKeys, 0, null);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de un DataTable existente, filtrando por diferentes campos 
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        internal void DBFilterFill(DataTable nDataTable, string nDataTableName, List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            bool Result;
            Exception Exception;

            if (this.DbManager.IsRemoting)
            {
                var TempDataTable = new DataTable();

                this.DbManager.DataBase.DBFilterFill(ref TempDataTable, this.SchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams, out Result, out Exception);
                if (!Result) throw Exception;

                if (nDataTable.Columns.Count == 0)
                {
                    nDataTable.Merge(TempDataTable);
                }
                else if (TempDataTable.Columns.Count == nDataTable.Columns.Count)
                {
                    foreach (DataRow Fila in TempDataTable.Rows)
                    {
                        object[] Items = new object[nDataTable.Columns.Count];

                        for (int i = 0; i < nDataTable.Columns.Count; i++)
                        {
                            Items[i] = Fila[nDataTable.Columns[i].ColumnName];
                        }

                        nDataTable.Rows.Add(Items);
                    }
                }
                else
                {
                    throw new Exception("La estructura devuelta por la base de datos no coincide con la tabla");
                }
            }
            else
            {
                this.DbManager.DataBase.DBFilterFill(ref nDataTable, this.SchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams, out Result, out Exception);
                if (!Result) throw Exception;
            }
        }


        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de una tabla existente, filtrando por la llave primaria
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        internal void DBFill(DataTable nDataTable, string nDataTableName, List<Parameter> nKeys)
        {
            DBFill(nDataTable, nDataTableName, nKeys, 0, null);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de una tabla existente, filtrando por la llave primaria
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        internal void DBFill(DataTable nDataTable, string nDataTableName, List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            bool Result;
            Exception Exception;

            if (this.DbManager.IsRemoting)
            {
                var TempDataTable = new DataTable();
                this.DbManager.DataBase.DBFill(ref TempDataTable, this.SchemaName, nDataTableName, nKeys, out Result, out Exception);
                if (!Result) throw Exception;

                if (nDataTable.Columns.Count == 0)
                {
                    nDataTable.Merge(TempDataTable);
                }
                else if (TempDataTable.Columns.Count == nDataTable.Columns.Count)
                {
                    foreach (DataRow Fila in TempDataTable.Rows)
                    {
                        object[] Items = new object[nDataTable.Columns.Count];

                        for (int i = 0; i < nDataTable.Columns.Count; i++)
                        {
                            Items[i] = Fila[nDataTable.Columns[i].ColumnName];
                        }

                        nDataTable.Rows.Add(Items);
                    }
                }
                else
                {
                    throw new Exception("La estructura devuelta por la base de datos no coincide con la tabla");
                }
            }
            else
            {
                this.DbManager.DataBase.DBFill(ref nDataTable, this.SchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams, out Result, out Exception);
                if (!Result) throw Exception;
            }
        }


        /// <summary>
        /// Aplica las modificaciones realizadas a los registros de un DataTable, calculando automáticamente el id siguiente, a la tabla en la base de datos
        /// </summary>
        /// <param name="nDataTableData">DataTable con las modificaciones</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nColumnsMapping">Campos de la tabla</param>
        /// <param name="nPrimaryKeysMapping">Campos de la llave primaria sin incluir el campo a calcular el NextId</param>
        /// <param name="nIdColumn">Campo a calcular el NextId</param>
        internal void DBSaveTableAutoNextId(DataTable nDataTableData, string nDataTableName, List<Parameter> nColumnsMapping, List<Parameter> nPrimaryKeysMapping, string nIdColumn)
        {
            bool Result;
            Exception Exception;

            this.DbManager.DataBase.DBSaveTableAutoNextId(ref nDataTableData, this.SchemaName, nDataTableName, nColumnsMapping, nPrimaryKeysMapping, nIdColumn, out Result, out Exception);
            if (!Result) throw Exception;
        }


        /// <summary>
        /// Aplica las modificaciones realizadas a los registros de un DataTable, a la tabla en la base de datos
        /// </summary>
        /// <param name="nDataTableData">DataTable con las modificaciones</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nColumnsMapping">Campos de la tabla</param>
        /// <param name="nPrimaryKeysMapping">Campos de la llave primaria</param>
        internal void DBSaveTable(DataTable nDataTableData, string nDataTableName, List<Parameter> nColumnsMapping, List<Parameter> nPrimaryKeysMapping)
        {
            bool Result;
            Exception Exception;

            this.DbManager.DataBase.DBSaveTable(ref nDataTableData, this.SchemaName, nDataTableName, nColumnsMapping, nPrimaryKeysMapping, out Result, out Exception);
            if (!Result) throw Exception;
        }


        /// <summary>
        /// Ejecuta un procedimiento almacenado que no devuelve registros
        /// </summary>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        internal void DBExecute(string nStoredProcedure)
        {
            DBExecute(nStoredProcedure, null);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que no devuelve registros
        /// </summary>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nParameters">Parametros del procedimiento almacenado</param>
        internal void DBExecute(string nStoredProcedure, List<Parameter> nParameters)
        {
            bool Result;
            Exception Exception;

            this.DbManager.DataBase.DBExecuteStoredProcedure(this.SchemaName, nStoredProcedure, nParameters, out Result, out Exception);
            if (!Result) throw Exception;
        }


        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve registros
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        internal void DBExecute(DataTable nDataTable, string nStoredProcedure)
        {
            DBExecute(nDataTable, nStoredProcedure, null);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve registros
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nParameters">Parametros del procedimiento almacenado</param>
        internal void DBExecute(DataTable nDataTable, string nStoredProcedure, List<Parameter> nParameters)
        {
            bool Result;
            Exception Exception;

            if (this.DbManager.IsRemoting)
            {
                var TempDataTable = new DataTable();
                this.DbManager.DataBase.DBExecuteStoredProcedureFill(ref TempDataTable, this.SchemaName, nStoredProcedure, nParameters, out Result, out Exception);
                if (!Result) throw Exception;

                if (nDataTable.Columns.Count == 0)
                {
                    nDataTable.Merge(TempDataTable);
                }
                else if (TempDataTable.Columns.Count == nDataTable.Columns.Count)
                {
                    foreach (DataRow Fila in TempDataTable.Rows)
                    {
                        object[] Items = new object[nDataTable.Columns.Count];

                        for (int i = 0; i < nDataTable.Columns.Count; i++)
                        {
                            Items[i] = Fila[nDataTable.Columns[i].ColumnName];
                        }

                        nDataTable.Rows.Add(Items);
                    }

                    nDataTable.AcceptChanges();
                }
                else
                {
                    throw new Exception("La estructura devuelta por la base de datos no coincide con la tabla");
                }
            }
            else
            {
                this.DbManager.DataBase.DBExecuteStoredProcedureFill(ref nDataTable, this.SchemaName, nStoredProcedure, nParameters, out Result, out Exception);
                if (!Result) throw Exception;
            }
        }

        #endregion

        #region Funciones

        /// <summary>
        /// Permite calcular la siguiente llave primaria para una determinada tabla
        /// </summary>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nInParams">Campos a actualizar</param>
        /// <param name="nIdColumn">Columna a la que se le va a calcular el NextId</param>
        /// <returns></returns>
        internal int DBNextId(string nDataTableName, List<Parameter> nInParams, string nIdColumn)
        {
            bool Result;
            Exception Exception;

            int NextId = this.DbManager.DataBase.DBNextId(this.SchemaName, nDataTableName, nInParams, nIdColumn, out Result, out Exception);
            if (!Result) throw Exception;

            return NextId;
        }


        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por la llave primaria
        /// </summary>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <returns>Resultado de la consulta</returns>
        internal DataTable DBGet(string nDataTableName, List<Parameter> nKeys)
        {
            return DBGet(nDataTableName, nKeys, 0, null);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por la llave primaria
        /// </summary>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Resultado de la consulta</returns>
        internal DataTable DBGet(string nDataTableName, List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            bool Result;
            Exception Exception;

            DataTable Tabla = this.DbManager.DataBase.DBGet(this.SchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams, out Result, out Exception);
            if (!Result) throw Exception;

            return Tabla;
        }


        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por diferentes campos
        /// </summary>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <returns>Resultado de la consulta</returns>
        internal DataTable DBFilterGet(string nDataTableName, List<Parameter> nKeys)
        {
            return DBFilterGet(nDataTableName, nKeys, 0, null);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por diferentes campos
        /// </summary>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Resultado de la consulta</returns>
        internal DataTable DBFilterGet(string nDataTableName, List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            bool Result;
            Exception Exception;

            DataTable Tabla = this.DbManager.DataBase.DBFilterGet(this.SchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams, out Result, out Exception);
            if (!Result) throw Exception;

            return Tabla;
        }

        #endregion
    }
}