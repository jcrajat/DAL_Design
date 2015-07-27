using System;
using System.Collections.Specialized;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using CM.Tools.Cryptographic;

namespace CMData.DataBase
{
    public enum DataBaseType
    {
        SqlServer,
        Postgres,
        Oracle
    }

    public class DataBaseFactory
    {
        #region Funciones

        public static DataBase CreateDatabase(string nConnectionString)
        {
            return CreateDatabase(GetDataBaseType(nConnectionString), nConnectionString);
        }

        public static DataBase CreateDatabase(DataBaseType nType, string nConnectionString)
        {
            string remotingString = GetRemotingUrl(nConnectionString);

            if (remotingString != "")
            {
                try
                {
                    string remotingPassword = GetRemotingPassword(nConnectionString);
                    bool remotingTrusted = GetRemotingTrusted(nConnectionString);

                    //Inicializar remoting
                    TcpChannel CanalTCP = ChannelServices.GetChannel("tcp") as TcpChannel;
                    if (CanalTCP != null)
                    {
                        if (CanalTCP.IsSecured != remotingTrusted)
                        {
                            try { ChannelServices.UnregisterChannel(CanalTCP); }
                            catch { }

                            RegisterChannel(remotingTrusted);
                        }
                    }
                    else
                    {
                        RegisterChannel(remotingTrusted);
                    }                    

                    //  "tcp://localhost:8085/nAppName"
                    var instance = (DataBaseProxy)Activator.GetObject(typeof(DataBaseProxy), remotingString);
                    var ServerPublicKey = instance.getServerPublicKey();
                    var remotingEncriptedPassword = Crypto.RSAEncryptText(remotingPassword, ServerPublicKey);
                    
                    DataBase db;
                    string Message;

                    if (!instance.CreateDataBase(nType, nConnectionString, remotingEncriptedPassword, out db, out Message))
                    {
                        throw new Exception(Message);
                    }

                    return db;
                }
                catch (Exception ex)
                {
                    throw new Exception("No fue posible conectarse con el servidor remoto, " + ex.Message, ex);
                }
            }
            else
            {
                try
                {
                    switch (nType)
                    {
                        case DataBaseType.Postgres:
                            return new PostgresDataBase(GetInnerConnectionString(nConnectionString));

                        case DataBaseType.SqlServer:
                            return new SqlServerDataBase(GetInnerConnectionString(nConnectionString));

                        case DataBaseType.Oracle:
                            return new OracleDataBase(GetInnerConnectionString(nConnectionString));

                        default:
                            throw new Exception("Dase de datos no permitida " + nType.ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("No fue posible inicializar el control de base de datos, " + ex.Message, ex);
                }
            }
        }

        public static void RegisterChannel(bool nRemotingTrusted)
        {
            //var Port = GetRemotingPort(nRemotingString);
            var CanalTCP = new TcpChannel(0);
            CanalTCP.IsSecured = nRemotingTrusted;

            try { ChannelServices.RegisterChannel(CanalTCP, nRemotingTrusted); }
            catch { }
        }

        public static DataBaseType GetDataBaseType(string nConnectionString)
        {
            string[] providerNames = Enum.GetNames(typeof(DataBaseType));
            string providers = "";

            foreach (var provider in providerNames)
            {
                providers += provider + "|";
            }

            try
            {
                string[] stringParts = nConnectionString.Split(';');

                foreach (var part in stringParts)
                {
                    string[] pairValue = part.Split('=');

                    if ((pairValue[0].ToUpper() == "CARGOMASTERPROVIDER"))
                    {
                        foreach (var provider in providerNames)
                        {
                            if (pairValue[1].ToUpper() == provider.ToUpper())
                            {
                                var nType = (DataBaseType)(Enum.Parse(typeof(DataBaseType), provider));
                                return nType;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener el proveedor de acceso a datos, utilice la llave cargomasterProvider=[" + providers + "]; Message: " + ex.Message, ex);
            }

            throw new Exception("No fue posible obtener el proveedor de acceso a datos, utilice la llave cargomasterProvider=[" + providers + "];");
        }

        public static string GetRemotingUrl(string nConnectionString)
        {
            try
            {
                string[] stringParts = nConnectionString.Split(';');

                foreach (var part in stringParts)
                {
                    string[] pairValue = part.Split('=');

                    if ((pairValue[0].Trim().ToUpper() == "REMOTINGURL"))
                    {
                        return pairValue[1].Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la url de remoting, " + ex.Message, ex);
            }

            return "";
        }

        public static int GetRemotingPort(string nRemotingUrl)
        {
            try
            {
                string[] stringParts = nRemotingUrl.Split(':');
                string[] stringParts2 = stringParts[stringParts.Length - 1].Split('/');

                return int.Parse(stringParts2[0]);
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la url de remoting, " + ex.Message, ex);
            }
        }

        public static string GetRemotingPassword(string nConnectionString)
        {
            try
            {
                string[] stringParts = nConnectionString.Split(';');

                foreach (var part in stringParts)
                {
                    string[] pairValue = part.Split('=');

                    if ((pairValue[0].Trim().ToUpper() == "REMOTINGPWD"))
                    {
                        return pairValue[1].Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la url de remoting, " + ex.Message, ex);
            }

            return "";
        }

        public static bool GetRemotingTrusted(string nConnectionString)
        {
            try
            {
                string[] stringParts = nConnectionString.Split(';');

                foreach (var part in stringParts)
                {
                    string[] pairValue = part.Split('=');

                    if ((pairValue[0].Trim().ToUpper() == "REMOTINGTRUSTED"))
                    {
                        switch (pairValue[1].Trim().ToUpper())
                        {
                            case "TRUE":
                            case "YES":
                            case "SI":
                                return true;

                            case "FALSE":
                            case "NO":
                                return false;

                            default:
                                throw new Exception("Parámetro no válido");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtener la configuración de uso de conexión segura por Remoting, " + ex.Message, ex);
            }

            return true;
        }

        public static string GetInnerConnectionString(string nConnectionString)
        {
            string innerConnectionString = "";
            var cargomasterKeys = new StringCollection();
            cargomasterKeys.AddRange(new string[] { "CARGOMASTERPROVIDER", "REMOTINGURL", "REMOTINGPWD", "REMOTINGTRUSTED" });

            try
            {
                string[] stringParts = nConnectionString.Split(';');

                foreach (var part in stringParts)
                {
                    string[] pairValue = part.Split('=');

                    if (!cargomasterKeys.Contains(pairValue[0].Trim().ToUpper()))
                    {
                        if (innerConnectionString != "") innerConnectionString += ";";
                        innerConnectionString += part.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible procesar la cadena de conexion , " + ex.Message, ex);
            }

            if (innerConnectionString == "")
            {
                throw new Exception("No fue posible procesar la cadena de conexion");
            }

            return innerConnectionString;
        }

        #endregion
    }
}