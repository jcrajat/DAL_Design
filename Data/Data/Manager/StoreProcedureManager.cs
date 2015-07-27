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
    public abstract class StoreProcedureManager : ObjectManager
    {
        #region Constructores

        /// <summary>
        /// Crea una nueva instancia de la clase
        /// </summary>
        /// <param name="nSchemaManager">SchemaManager que administra la tabla</param>
        protected StoreProcedureManager(SchemaManager nSchemaManager)
            : base(nSchemaManager)
        {
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Ejecuta un procedimiento almacenado que no devuelve registros
        /// </summary>
        protected virtual void DBExecuteSp()
        {
            this.SchemaManager.DBExecute(this._ObjectName, null);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que no devuelve registros
        /// </summary>
        /// <param name="nParameters">Parametros del procedimiento almacenado</param>
        protected virtual void DBExecuteSp(List<Parameter> nParameters)
        {
            this.SchemaManager.DBExecute(this._ObjectName, nParameters);
        }


        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve registros
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        protected virtual void DBExecuteSp(DataTable nDataTable)
        {
            DBExecuteSp(nDataTable, null);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve registros
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nParameters">Parametros del procedimiento almacenado</param>
        protected virtual void DBExecuteSp(DataTable nDataTable, List<Parameter> nParameters)
        {
            this.SchemaManager.DBExecute(nDataTable, this._ObjectName, nParameters);
        }

        #endregion
    }
}