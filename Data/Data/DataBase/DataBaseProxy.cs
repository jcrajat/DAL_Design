using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using CM.Tools.Cryptographic;

namespace CMData.DataBase
{
    public class DataBaseProxy : MarshalByRefObject
    {
        #region Declaraciones

        private Dictionary<Guid, DataBase> _DataBaseList;

        private string ServerPublicKey;        
        private string ServerPrivateKey;
        private string ServerPassword;

        #endregion

        #region Constructores

        public DataBaseProxy(string nServerPassword): this()   
        {
            ServerPassword = nServerPassword;
        }

        public DataBaseProxy()
        {
            this._DataBaseList = new Dictionary<Guid, DataBase>();

            Crypto.RSACrearKeys(out ServerPrivateKey, out ServerPublicKey);
        }

        #endregion

        #region Metodos

        public void DisconetDataBase(Guid nToken)
        {
            try
            {
                var instance = this._DataBaseList[nToken];

                if (instance != null)
                {
                    this._DataBaseList.Remove(nToken);
                    instance = null;
                }
            }
            catch { }
        }

        public void FreeDataBase(int nLifeTime)
        {
            try
            {
                var ToDisconectList = new List<DataBase>();

                foreach (var Conexion in this._DataBaseList.Values)
                {
                    if (Conexion.LastAccess.AddMilliseconds(nLifeTime) > DateTime.Now)
                        ToDisconectList.Add(Conexion);
                }

                foreach (var Conexion in ToDisconectList)
                {
                    Conexion.Disconect();
                }
            }
            catch { }
        }

        #endregion

        #region Funciones

        public bool CreateDataBase(DataBaseType nType, string nConnectionString, byte[] nRemotingPassword, out DataBase nDataBase, out string nMessage)
        {
            nMessage = "";
            nDataBase = null;

            try
            {
                if (this.ServerPassword == Crypto.RSADecryptText(nRemotingPassword, this.ServerPrivateKey))
                {
                    nDataBase = DataBaseFactory.CreateDatabase(nType, DataBaseFactory.GetInnerConnectionString(nConnectionString));

                    nDataBase._proxy = this;

                    this._DataBaseList.Add(nDataBase._token, nDataBase);

                    return true;
                }
                else
                {
                    throw new Exception("La contraseña de acceso al servidor remoting no es válida");
                }
            }
            catch (Exception ex)
            {
                nMessage = ex.Message;                
            }

            return false;
        }

        public string getServerPublicKey()
        {
            return this.ServerPublicKey;
        }

        public override object InitializeLifetimeService()
        {
            var lease = (ILease)base.InitializeLifetimeService();

            if (lease.CurrentState == LeaseState.Initial)
                lease.InitialLeaseTime = TimeSpan.Zero;

            return lease;
        }

        #endregion
    }
}
