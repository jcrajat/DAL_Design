using System;
using System.Collections.Generic;
using System.Text;
using Data.Schemas;

namespace CMData.Utils
{
    /// <summary>
    /// Tipo de enlace a utilizar en la relacion
    /// </summary>
    public enum SqlJoinType
    {
        Inner,
        Left
    }

    /// <summary>
    /// Representa una lista de relaciones entre dos o mas tablas
    /// </summary>
    public class TableRelationCollection : List<TableRelation> , ICloneable
    {

        /// <summary>
        /// Busca y devuelve las relaciones en la que participa una tabla principal
        /// </summary>
        /// <param name="nDataBaseName">Identificador de la base de datos de la tabla</param>
        /// <param name="nSchemaName">Esquema de la tabla</param>
        /// <param name="nTableName">Nombre de la tabla</param>
        /// <returns></returns>
        public TableRelation FindRelation(string nDataBaseCod, string nSchemaName , string nTableName )
        {                                   
            for (int i = 0; i < Count; i++)
            {
                if (base[i].ForeignDataBaseCod == nDataBaseCod && base[i].ForeignSchemaName == nSchemaName && base[i].ForeignTableName == nTableName)
                {
                    return base[i];
                }
            }
            return null;
        }

        #region Miembros de ICloneable

        public object Clone()
        {
            var newSqlJoinTableCollection = new TableRelationCollection();
            for (int i = 0; i < this.Count; i++ )
            {
                newSqlJoinTableCollection.Add((TableRelation)this[i].Clone());
            }
            return newSqlJoinTableCollection;
        }

        #endregion
    }

    /// <summary>
    /// Parametros de configuracion entre dos tablas
    /// </summary>
    public class TableRelation : ICloneable
    {
        /// <summary>
        /// Alias de la tabla que interviene en las relacion, se utiliza en los casos en los que la misma tabla se utiliza como dos relaciones diferentes
        /// </summary>
        public string MainRelationAlias = "";

        /// <summary>
        /// Cadena de conexion de la base de datos de la tabla principal
        /// </summary>
        public string MainDataBaseConnectionString = "";

        /// <summary>
        /// Identificador de la base de datos de la tabla principal
        /// </summary>
        public string MainDataBaseCod = "";

        /// <summary>
        /// Esquema de la tabla principal
        /// </summary>
        public string MainSchemaName = "";

        /// <summary>
        /// Tabla principal
        /// </summary>
        public string MainTableName = "";

        /// <summary>
        /// Columnas de la tabla principal que se enviaran al resultado de la consulta, deben estar separados por (coma)
        /// </summary>
        public string MainColumnsToResult = "";

        /// <summary>
        /// Columna de la tabla principal que interviene en la relacion
        /// </summary>
        public string MainColumnName = "";

        /// <summary>
        /// Alias de la tabla que interviene en las relacion, se utiliza en los casos en los que la misma tabla se utiliza como dos relaciones diferentes
        /// </summary>
        public string ForeignRelationAlias = "";

        /// <summary>
        /// Cadena de conexion de la base de datos de la tabla foranea
        /// </summary>
        public string ForeignDataBaseConnectionString = "";        
        
        /// <summary>
        /// Identificador de la base de datos de la tabla foranea
        /// </summary>
        public string ForeignDataBaseCod = "";

        /// <summary>
        /// Esquema de la tabla foranea
        /// </summary>
        public string ForeignSchemaName = "";

        /// <summary>
        /// Tabla foranea
        /// </summary>
        public string ForeignTableName = "";

        /// <summary>
        /// Columna de la tabla foranea que interviene en la relacion
        /// </summary>
        public string ForeignColumnName = "";

        /// <summary>
        /// Columnas de la tabla foranea que se enviaran al resultado de la consulta, deben estar separados por (coma)
        /// </summary>
        public string ForeignColumnsToResult = "";

        /// <summary>
        /// Valor de la tabla foranea que se puede agregar como filtro, este campo invalida los campos de configuracion de la tabla primaria
        /// </summary>
        public string ForeignFilterTextValue = "";

        /// <summary>
        /// Tipo de relacion a realizar, si hay varias relaciones entre las mismas tablas, se utiliza el de la primera relacion creada
        /// </summary>
        public SqlJoinType SqlJoinType = SqlJoinType.Inner;

        /// <summary>
        /// Devulve el texto que representa el join en ansi sql
        /// </summary>
        /// <returns>Texto join en ansi sql</returns>
        public string GetJoinString()
        {
            switch (SqlJoinType)
            {
                case SqlJoinType.Inner: return "Inner Join";
                case SqlJoinType.Left: return "Left Join";
            }
            return "Join";
        }

        #region Miembros de ICloneable

        public object Clone()
        {
            return new TableRelation()
            {
                ForeignRelationAlias = this.ForeignRelationAlias,
                ForeignDataBaseCod = this.ForeignDataBaseCod,
                ForeignDataBaseConnectionString = this.ForeignDataBaseConnectionString,
                ForeignSchemaName = this.ForeignSchemaName,
                ForeignTableName = this.ForeignTableName,
                ForeignColumnName = this.ForeignColumnName,
                ForeignColumnsToResult = this.ForeignColumnsToResult,
                MainRelationAlias = this.MainRelationAlias,
                MainDataBaseCod = this.ForeignDataBaseCod,
                MainDataBaseConnectionString = this.MainDataBaseConnectionString,
                MainSchemaName = this.MainSchemaName,
                MainTableName = this.MainTableName,
                MainColumnName = this.MainColumnName,
                MainColumnsToResult = this.MainColumnsToResult,
                ForeignFilterTextValue = this.ForeignFilterTextValue,
                SqlJoinType = this.SqlJoinType
            };
        }

        #endregion
    }
}
