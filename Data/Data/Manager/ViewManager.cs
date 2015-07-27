using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CMData.Manager;
using CMData.Schemas;
using CMData.DataBase;

namespace CMData.Manager
{
    /// <summary>
    /// Clase base de las funcionalidades de acceso a tabla de base de datos
    /// </summary>
    public abstract class ViewManager : ObjectManager
    {
        #region Constructores

        /// <summary>
        /// Crea una nueva instancia de la clase
        /// </summary>
        /// <param name="nSchemaManager">SchemaManager que administra la tabla</param>
        protected ViewManager(SchemaManager nSchemaManager)
            : base(nSchemaManager)
        {
        }

        #endregion

        #region Metodos

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

        #endregion

        #region Funciones

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

        #endregion
    }
}