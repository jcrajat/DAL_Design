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
    public abstract class ObjectManager : MarshalByRefObject
    {
        #region Declaraciones

        private SchemaManager _SchemaManager;
        protected string _ObjectName = "";

        #endregion

        #region Constructores

        /// <summary>
        /// Crea una nueva instancia de la clase
        /// </summary>
        /// <param name="nSchemaManager">SchemaManager que administra la tabla</param>
        public ObjectManager(SchemaManager nSchemaManager)
        {
            this._SchemaManager = nSchemaManager;
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// SchemaManager que administra el objeto
        /// </summary>
        public SchemaManager SchemaManager
        {
            get { return _SchemaManager; }
        }

        /// <summary>
        /// Nombre del objeto administrado
        /// </summary>
        public string ObjectName
        {
            get { return _ObjectName; }
        }

        #endregion
    }
}