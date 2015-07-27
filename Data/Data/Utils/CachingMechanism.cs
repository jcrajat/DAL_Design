using System;
using System.Collections;
using System.Data;

namespace CMData.Helpers
{
    internal class CachingMechanism
    {
        #region Declaraciones

        private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        #endregion

        #region Metodos

        public void AddParameterSetToCache(string connectionString, IDbCommand command, IDataParameter[] parameters)
        {
            string commandText = command.CommandText;
            string str2 = CreateHashKey(connectionString, commandText);
            this.paramCache[str2] = parameters;
        }

        public void Clear()
        {
            this.paramCache.Clear();
        }

        #endregion

        #region Funciones

        public static IDataParameter[] CloneParameters(IDataParameter[] originalParameters)
        {
            IDataParameter[] parameterArray = new IDataParameter[originalParameters.Length - 1];
            int index = 0;
            int length = originalParameters.Length;

            do
            {
                parameterArray[index] = (IDataParameter)((ICloneable)originalParameters[index]).Clone();
                index++;
            } while (index < length);

            return parameterArray;
        }

        private static string CreateHashKey(string connectionString, string storedProcedure)
        {
            return (connectionString + ":" + storedProcedure);
        }

        public IDataParameter[] GetCachedParameterSet(string connectionString, IDbCommand command)
        {
            string commandText = command.CommandText;
            string str2 = CreateHashKey(connectionString, commandText);
            IDataParameter[] originalParameters = (IDataParameter[])(this.paramCache[str2]);
            return CachingMechanism.CloneParameters(originalParameters);
        }

        public bool IsParameterSetCached(string connectionString, IDbCommand command)
        {
            string str = CreateHashKey(connectionString, command.CommandText);
            return (this.paramCache[str] != null);
        }

        #endregion
    }
}