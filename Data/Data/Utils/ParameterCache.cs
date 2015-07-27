using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using CMData;

namespace CMData.Helpers
{
    public class ParameterCache
    {
        #region Declaraciones

        private CachingMechanism cache = new CachingMechanism();

        #endregion

        #region Metodos

        public void SetParameters(DbCommand command, DataBase.DataBase database)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (database == null)
                throw new ArgumentNullException("database");

            if (this.AlreadyCached(command, database))
            {
                this.AddParametersFromCache(command, database);
            }
            else
            {
                database.DeriveParameters(ref command);
                IDataParameter[] parameters = CreateParameterCopy(command);
                this.cache.AddParameterSetToCache(database.ConnectionString, command, parameters);
            }
        }

        protected void AddParametersFromCache(DbCommand command, DataBase.DataBase database)
        {
            IDataParameter[] cachedParameterSet = this.cache.GetCachedParameterSet(database.ConnectionString, command);

            foreach (IDataParameter parameter in cachedParameterSet)
            {
                command.Parameters.Add(parameter);
            }
        }

        protected internal void Clear()
        {
            this.cache.Clear();
        }

        #endregion

        #region Funciones

        private bool AlreadyCached(IDbCommand command, DataBase.DataBase database)
        {
            return this.cache.IsParameterSetCached(database.ConnectionString, command);
        }

        private static IDataParameter[] CreateParameterCopy(DbCommand command)
        {
            IDataParameterCollection parameters = command.Parameters;
            IDataParameter[] array = new IDataParameter[parameters.Count - 1];
            parameters.CopyTo(array, 0);
            return CachingMechanism.CloneParameters(array);
        }

        #endregion
    }
}