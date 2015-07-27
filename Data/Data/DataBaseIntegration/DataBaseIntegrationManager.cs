using System;
using System.Collections.Generic;
using System.Text;
using CMData.Manager;
using System.Data;
using CMData.Schemas;

namespace CMData.Utils
{
    public class DataBaseIntegrationManager
    {
        public Dictionary<string, DBManager> ManagerPool = new Dictionary<string, DBManager>();

        public DBManager AddManager(string nDataBaseName, DBManager nDbManager)
        {
            if (!ManagerPool.ContainsKey(nDataBaseName))
                ManagerPool.Add(nDataBaseName, nDbManager);
            return nDbManager;
        }

        public DBManager AddManager(string nDataBaseName, string nConnectionString, bool AutoOpen)
        {
            if (ManagerPool.ContainsKey(nDataBaseName))
                return ManagerPool[nDataBaseName];

            var dbNewManager = new DBManager(nConnectionString);
            if( AutoOpen)
                dbNewManager.Connection_Open();

            ManagerPool.Add(nDataBaseName, dbNewManager);
            return dbNewManager;
        }

        public void Connection_Open_Managers()
        {
            string strExceptions = "";
            foreach (var dbManager in ManagerPool)
            {
                try
                {
                    if (dbManager.Value.DataBase.Connection.State == ConnectionState.Closed)
                        dbManager.Value.Connection_Open();
                }
                catch (Exception ex)
                {
                    strExceptions += "Error al abrir el manager de base de datos " + dbManager.Key + "," + ex.Message + ". ";
                }
            }

            if( strExceptions != "")
                throw new Exception("No fue posible abrir todos los managers de base de datos," + strExceptions);
        }


        public void Connection_Close_Managers()
        {
            string strExceptions = "";
            foreach (var dbManager in ManagerPool)
            {
                try
                {
                    if (dbManager.Value.DataBase.Connection.State != ConnectionState.Closed)
                        dbManager.Value.Connection_Close();
                }
                catch (Exception ex)
                {
                    strExceptions += "Error al cerrar el manager de base de datos " + dbManager.Key + "," + ex.Message + ". ";
                }
            }
            if (strExceptions != "")
                throw new Exception("No fue posible abrir todos los managers de base de datos," + strExceptions);
        }

        public void Connection_Commit_Managers()
        {
            string strExceptions = "";
            foreach (var dbManager in ManagerPool)
            {
                try
                {
                    dbManager.Value.Transaction_Commit();
                }
                catch (Exception ex)
                {
                    strExceptions += "Error al ejecutar el commit del manager de base de datos " + dbManager.Key + "," + ex.Message + ". ";
                }
            }
            if (strExceptions != "")
                throw new Exception("No fue posible abrir todos los managers de base de datos," + strExceptions);
        }

        public void Connection_Rollback_Managers()
        {
            string strExceptions = "";
            foreach (var dbManager in ManagerPool)
            {
                try
                {
                    dbManager.Value.Transaction_Rollback();
                }
                catch (Exception ex)
                {
                    strExceptions += "Error al ejecutar el rollback del manager de base de datos " + dbManager.Key + "," + ex.Message + ". ";
                }
            }
            if (strExceptions != "")
                throw new Exception("No fue posible abrir todos los managers de base de datos," + strExceptions);
        }

        public void CommitAndClose()
        {
            Connection_Commit_Managers();
            Connection_Close_Managers();
        }

        public void RollbackAndClose()
        {
            Connection_Rollback_Managers();
            Connection_Close_Managers();
        }


        /// <summary>
        /// Permite obtener los registros de varias tablas filtrando por diferentes campos
        /// </summary>
        /// <param name="nDataTable">Resultado de la consulta</param>
        /// <param name="nDataBaseCod">Base de datos a la que pertenece la tabla</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nTableRelationList">Lista de relaciones con otras tablas</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBQueryFill(ref DataTable nDataTable, string nDataBaseCod, string nSchemaName, string nDataTableName, List<Parameter> nKeys, TableRelationCollection nTableRelationList, int nMaxRows, ColumnEnumList nOrderByParams)
        {
            try
            {
                bool nResult = true;
                Exception nException = null;
                int index = 0;

                //Ejecutar la consulta principal
                var primaryRelations = new TableRelationCollection();
                while (index < nTableRelationList.Count)
                {
                    if (nTableRelationList[index].ForeignDataBaseCod != nDataBaseCod)
                        break;
                    primaryRelations.Add((TableRelation)nTableRelationList[index].Clone());
                    index++;
                }

                ManagerPool[nDataBaseCod].DataBase.DBQueryFill(ref nDataTable, "form", nSchemaName, nDataTableName, nKeys, primaryRelations, nMaxRows, nOrderByParams, out nResult, out nException);
                if (!nResult) throw nException;

                //TODO: Agregar consultas a relaciones a base de datos externas
                while (index < nTableRelationList.Count)
                {


                    index++;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar los registros de la tabla [" + nDataBaseCod + "] " + nSchemaName + "." + nDataTableName + ", " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los registros de varias tablas filtrando por diferentes campos
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nTableRelationList">Lista de relaciones con otras tablas</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBQueryGet(string nSchemaName, string nDataTableName, List<Parameter> nKeys, TableRelationCollection nTableRelationList, int nMaxRows)
        {
            return null;
        }

    }
}
