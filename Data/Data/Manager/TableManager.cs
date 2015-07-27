using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CMData.Schemas;
using CMData.Manager;
using CMData.Schemas;
using CMData.DataBase;

namespace CMData.Manager
{
    /// <summary>
    /// Clase base de las funcionalidades de acceso a tabla de base de datos
    /// </summary>
    public abstract class TableManager : ObjectManager
    {
        #region Constructores

        /// <summary>
        /// Crea una nueva instancia de la clase
        /// </summary>
        /// <param name="nSchemaManager">SchemaManager que administra la tabla</param>
        protected TableManager(SchemaManager nSchemaManager)
            : base(nSchemaManager)
        {
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Permite insertar un nuevo registro en una tabla determinada
        /// </summary>
        /// <param name="nInParams">Valores a insertar</param>
        protected virtual void DBInsert(List<Parameter> nInParams)
        {
            this.SchemaManager.DBInsert(this._ObjectName, nInParams);
        }
        

        /// <summary>
        /// Permite actualizar la data de un registro de una tabla
        /// </summary>        
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nInParams">Campos a actualizar</param>
        protected virtual void DBUpdate(List<Parameter> nKeys, List<Parameter> nInParams)
        {
            this.SchemaManager.DBUpdate(this._ObjectName, nKeys, nInParams);
        }


        /// <summary>
        /// Permite eliminar un registro de una tabla
        /// </summary>
        /// <param name="nKeys">Llave primaria del registro</param>
        protected virtual void DBDelete(List<Parameter> nKeys)
        {
            this.SchemaManager.DBDelete(this._ObjectName, nKeys);
        }


        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de un DataTable existente, filtrando por diferentes campos 
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>        
        /// <param name="nKeys">Llave primaria del registro</param>
        protected virtual void DBFilterFill(DataTable nDataTable, List<Parameter> nKeys)
        {
            DBFilterFill(nDataTable, nKeys, 0, null);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de un DataTable existente, filtrando por diferentes campos 
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>        
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        protected virtual void DBFilterFill(DataTable nDataTable, List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            this.SchemaManager.DBFilterFill(nDataTable, this._ObjectName, nKeys, nMaxRows, nOrderByParams);
        }


        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de una tabla existente, filtrando por la llave primaria
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>        
        /// <param name="nKeys">Llave primaria del registro</param>
        protected virtual void DBFill(DataTable nDataTable, List<Parameter> nKeys)
        {
            DBFill(nDataTable, nKeys, 0, null);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de una tabla existente, filtrando por la llave primaria
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>        
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        protected virtual void DBFill(DataTable nDataTable, List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            this.SchemaManager.DBFill(nDataTable, this._ObjectName, nKeys, nMaxRows, nOrderByParams);
        }


        /// <summary>
        /// Aplica las modificaciones realizadas a los registros de un DataTable, calculando automáticamente el id siguiente, a la tabla en la base de datos
        /// </summary>
        /// <param name="nDataTableData">DataTable con las modificaciones</param>        
        /// <param name="nColumnsMapping">Campos de la tabla</param>
        /// <param name="nPrimaryKeysMapping">Campos de la llave primaria sin incluir el campo a calcular el NextId</param>
        /// <param name="nIdColumn">Campo a calcular el NextId</param>
        protected virtual void DBSaveTableAutoNextId(DataTable nDataTableData, List<Parameter> nColumnsMapping, List<Parameter> nPrimaryKeysMapping, string nIdColumn)
        {
            this.SchemaManager.DBSaveTableAutoNextId(nDataTableData, this._ObjectName, nColumnsMapping, nPrimaryKeysMapping, nIdColumn);
        }


        /// <summary>
        /// Aplica las modificaciones realizadas a los registros de un DataTable, a la tabla en la base de datos
        /// </summary>
        /// <param name="nDataTableData">DataTable con las modificaciones</param>        
        /// <param name="nColumnsMapping">Campos de la tabla</param>
        /// <param name="nPrimaryKeysMapping">Campos de la llave primaria</param>
        protected virtual void DBSaveTable(DataTable nDataTableData, List<Parameter> nColumnsMapping, List<Parameter> nPrimaryKeysMapping)
        {
            this.SchemaManager.DBSaveTable(nDataTableData, this._ObjectName, nColumnsMapping, nPrimaryKeysMapping);
        }

        #endregion

        #region Funciones

        /// <summary>
        /// Permite calcular la siguiente llave primaria para una determinada tabla
        /// </summary>        
        /// <param name="nInParams">Campos a actualizar</param>
        /// <param name="nIdColumn">Columna a la que se le va a calcular el NextId</param>
        /// <returns></returns>
        protected virtual int DBNextId(List<Parameter> nInParams, string nIdColumn)
        {
            return this.SchemaManager.DBNextId(this._ObjectName, nInParams, nIdColumn);
        }


        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por la llave primaria
        /// </summary>        
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <returns>Resultado de la consulta</returns>
        protected virtual DataTable DBGet(List<Parameter> nKeys)
        {
            return DBGet(nKeys, 0, null);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por la llave primaria
        /// </summary>        
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Resultado de la consulta</returns>
        protected virtual DataTable DBGet(List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            return this.SchemaManager.DBGet(this._ObjectName, nKeys, nMaxRows, nOrderByParams);
        }


        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por diferentes campos
        /// </summary>        
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <returns>Resultado de la consulta</returns>
        protected virtual DataTable DBFilterGet(List<Parameter> nKeys)
        {
            return DBFilterGet(nKeys, 0, null);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por diferentes campos
        /// </summary>        
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Resultado de la consulta</returns>
        protected virtual DataTable DBFilterGet(List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            return this.SchemaManager.DBFilterGet(this._ObjectName, nKeys, nMaxRows, nOrderByParams);
        }

        /// <summary>
        /// Permite obtener el valor de la columna de una fila sin importar si este es nulo
        /// </summary>
        /// <param name="nRow">Objeto DataRow que contiene el dato</param>
        /// <param name="nColumName">Nombre de la columna que contiene el dato en el DataRow</param>
        /// <returns></returns>
        protected object getColumnValue(DataRow nRow, string nColumName)
        {
            if (nRow.IsNull(nColumName))
                return null;
            else
                return nRow[nColumName];
        }

        #endregion
    }

    //public class DefaultTableManager : TableManager
    //{
    //}
}