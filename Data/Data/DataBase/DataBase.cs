using System;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CM.Tools.Cryptographic;
using CM.Tools.Misellaneous;
using CMData.Schemas;
using CMData.Utils;
using CMData.Manager;
using Tools;

namespace CMData.DataBase
{
    /// <summary>
    /// Define las opciones de filtrado rapido
    /// </summary>
    /// 
    public enum DataFilterType
    {
        // ReSharper disable InconsistentNaming
        // ReSharper disable UnusedMember.Global
        AD,
        EH,
        IL,
        MP,
        QT,
        UZ,
        _All,

        _None,
        _Custom
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }

    /// <summary>
    /// Define los operadores AND y OR a usar en las consultas
    /// </summary>
    public enum FilterOption
    {
        And,
        Or
    }

    /// <summary>
    /// Clase base de conexión a bases de datos
    /// </summary>
    public abstract class DataBase : MarshalByRefObject
    {
        #region Declaraciones

        internal Guid _token = Guid.NewGuid();
        internal DataBaseProxy _proxy;
        private DateTime _lastAccess = DateTime.Now;
        private DbConnection _connection;
        private DbTransaction _transaction;
        protected DbProviderFactory DefaultFactory;
        //private static ParameterCache _parameterCache = new ParameterCache();

        private string _identifierDateFormat = "yyyy-MM-dd hh:mm:ss";
        private string _identifierDateDataBaseFormat = "yyyy/mm/dd hh24:mi:ss";

        public const string Identifier_Comodin_Like = "*";
        public const string Identifier_Comodin_Similar = "**";

        private string _serverPublicKey;
        private string _serverPrivateKey;

        private bool _isBullkQuery;
        private readonly List<string> _bullkQuery = new List<string>();

        private string _lastQuery = "";

        //Version Beta: Indica que se debe utilizar una connecion heredada por lo que algunas acciones no estarán permitidas
        public bool IsInheritDataBase;
        //Version Beta: Indica el catalogo sobre el que se realizaran las acciones
        public string CurrentDataBaseCatalogName = "";
        //Version Beta: Obtiene el catalogo sobre el que se realizaran las acciones
        public abstract string GetConnectionStringCatalogName(string nConnectionString);
        //Version Beta: Establece la base de datos sobre la que se realizaran las acciones
        public DBManager InheritDbManager;

        #endregion

        #region Propiedades

        #region Identificadores

        /// <summary>
        /// Cadena usada para identificar la posición de la fecha en una consulta SQL
        /// </summary>
        protected virtual string Identifier_Date_Mask
        {
            get { return "'$$DATE$$'"; }
        }

        /// <summary>
        /// Identificador de cadenas de texto
        /// </summary>
        public virtual string Identifier_String_Symbol
        {
            get { return "'"; }
        }

        /// <summary>
        /// Identificador de fin de línea
        /// </summary>
        public virtual string Identifier_EndLine_Symbol
        {
            get { return ";" + "\n"; }
        }

        /// <summary>
        /// Prefijo de identificación de tablas
        /// </summary>
        public virtual string Identifier_Table_Prefix
        {
            get { return ""; }
        }

        /// <summary>
        /// Postfijo de identificación de tablas
        /// </summary>
        public virtual string Identifier_Table_Postfix
        {
            get { return ""; }
        }

        /// <summary>
        /// Prefijo de identificación de procedimientos almacenados
        /// </summary>
        public virtual string Identifier_Procedure_Prefix
        {
            get { return ""; }
        }

        /// <summary>
        /// Postfijo de identificación de procedimientos almacenados
        /// </summary>
        public virtual string Identifier_Procedure_Postfix
        {
            get { return ""; }
        }

        /// <summary>
        /// Identificador de punto flotante
        /// </summary>
        public virtual string Identifier_Decimal_Separator
        {
            get { return "."; }
        }

        /// <summary>
        /// Formato de fecha
        /// </summary>
        public virtual string Identifier_Date_Format
        {
            get { return _identifierDateFormat; }
            set { _identifierDateFormat = value; }
        }

        /// <summary>
        /// Formato de fecha para las funciones de base de datos de converción de fecha
        /// </summary>
        public virtual string Identifier_Date_DataBase_Format
        {
            get { return _identifierDateDataBaseFormat; }
            set { _identifierDateDataBaseFormat = value; }
        }

        /// <summary>
        /// Codificación de texto usada
        /// </summary>
        public virtual Encoding Identifier_Text_Encoding
        {
            get { return Encoding.Unicode; }
        }

        /// <summary>
        /// Separador de esquemas
        /// </summary>
        public virtual string Identifier_Schema_Separator
        {
            get { return "."; }
        }

        /// <summary>
        /// Operador de igualdad
        /// </summary>
        public virtual string Identifier_Operator_Equal
        {
            get { return "="; }
        }

        /// <summary>
        /// Operador de comparación con NULL
        /// </summary>
        public virtual string Identifier_Operator_Null
        {
            get { return "IS"; }
        }

        /// <summary>
        /// Operador de coincidencias parciales
        /// </summary>
        public virtual string Identifier_Operator_Like
        {
            get { return "LIKE"; }
        }

        /// <summary>
        /// Caracter comodín para búsquedas aproximadas
        /// </summary>
        public virtual string Identifier_Symbol_Like
        {
            get { return "%"; }
        }

        /// <summary>
        /// Caracter comodín para búsquedas similares
        /// </summary>
        public virtual string Identifier_Symbol_Similar
        {
            get { return "%"; }
        }

        /// <summary>
        /// Operador de coincidencias similares
        /// </summary>
        public virtual string Identifier_Operator_Similar
        {
            get { return "LIKE"; }
        }

        /// <summary>
        /// Operador de ordenación ascendente
        /// </summary>
        public virtual string Identifier_OrderBy_ASC
        {
            get { return "ASC"; }
        }

        /// <summary>
        /// Operador de ordenación descendente
        /// </summary>
        public virtual string Identifier_OrderBy_DESC
        {
            get { return "DESC"; }
        }

        #endregion

        /// <summary>
        /// Código de identificación único de instancia para Remoting
        /// </summary>
        internal Guid Token
        {
            get { return _token; }
        }

        /// <summary>
        /// Proxy usado en la comunicación con remoting
        /// </summary>
        internal DataBaseProxy Proxy
        {
            get { return _proxy; }
        }

        /// <summary>
        /// Tipo de la instancia de base de datos
        /// </summary>
        public abstract DataBaseType DataBaseType { get; }

        /// <summary>
        /// Caadena de conexión a la base de datos
        /// </summary>
        public string ConnectionString
        {
            get { return Connection.ConnectionString; }
        }

        /// <summary>
        /// Define si existe una transacción activa
        /// </summary>
        public bool ExistsTransaction
        {
            get { return (Transaction != null); }
        }

        /// <summary>
        /// Objeto de conexión a la base de datos
        /// </summary>
        public DbConnection Connection
        {
            get { return _connection; }
            internal set { _connection = value; }
        }

        /// <summary>
        /// Objeto de identificación de la transacción activa
        /// </summary>
        public DbTransaction Transaction
        {
            get { return _transaction; }
            internal set { _transaction = value; }
        }

        /// <summary>
        /// Fecha de último acceso a la base de datos
        /// </summary>
        internal DateTime LastAccess
        {
            get { return _lastAccess; }
        }

        public bool IsBullkQuery
        {
            get { return _isBullkQuery; }
        }

        public int BullkQueryLines
        {
            get { return _bullkQuery.Count; }
        }

        public string LastQuery
        {
            get { return _lastQuery; }
            internal set { _lastQuery = value; }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Inicializa las variables del objeto
        /// </summary>
        /// <param name="nConnectionString">Cadena de conexión a la base de datos</param>
        internal void Initialize(string nConnectionString)
        {
            _connection = DefaultFactory.CreateConnection();

            if (_connection != null) _connection.ConnectionString = nConnectionString;

            Crypto.RSACrearKeys(out _serverPrivateKey, out _serverPublicKey);
        }

        /// <summary>
        /// Abre la conexión a la base de datos
        /// </summary>
        public void Connection_Open()
        {
            UpdateLastAccess();

            if (IsInheritDataBase)
            {
                _connection = InheritDbManager.DataBase.Connection;
            }
            else
            {
                _connection.Open();

                _transaction = null;
            }
        }

        /// <summary>
        /// Cierra la conexión a la base de datos
        /// </summary>
        public void Connection_Close()
        {
            UpdateLastAccess();

            if (IsInheritDataBase)
            {
                InheritDbManager.DataBase.Connection_Close();
            }
            else
            {
                if (ExistsTransaction) Transaction_Rollback();

                _bullkQuery.Clear();

                if (_connection.State != ConnectionState.Closed) _connection.Close();
            }
        }

        /// <summary>
        /// Inicia una nueva transacción
        /// </summary>
        public void Transaction_Begin()
        {
            UpdateLastAccess();

            if (IsInheritDataBase)
            {
                if (!InheritDbManager.DataBase.ExistsTransaction)
                    InheritDbManager.DataBase.Transaction_Begin();
                _transaction = InheritDbManager.DataBase.Transaction;
            }
            else
            {
                if (!ExistsTransaction)
                {
                    var cnn = GetOpenConnection();
                    _transaction = cnn.BeginTransaction();
                }
                else
                {
                    throw new Exception("Ya existe una transacción activa");
                }
            }
        }

        /// <summary>
        /// Inicia una nueva transacción
        /// </summary>
        /// <param name="nIsolationLevel">NIvel de aislamiento usado por la transacción</param>
        public void Transaction_Begin(IsolationLevel nIsolationLevel)
        {
            UpdateLastAccess();

            if (IsInheritDataBase)
            {
                if (!InheritDbManager.DataBase.ExistsTransaction)
                    InheritDbManager.DataBase.Transaction_Begin(nIsolationLevel);
                _transaction = InheritDbManager.DataBase.Transaction;
            }
            else
            {
                if (!ExistsTransaction)
                {
                    var cnn = GetOpenConnection();
                    _transaction = cnn.BeginTransaction(nIsolationLevel);
                }
                else
                {
                    throw new Exception("Ya existe una transacción activa");
                }
            }
        }

        /// <summary>
        /// Acepta los cambios realizados durante la transacción
        /// </summary>
        public void Transaction_Commit()
        {
            UpdateLastAccess();

            if (IsInheritDataBase)
                throw new Exception(
                    "Solo se permite finalizar la transaccion desde la conexion principal, cuando es una conexion enlazada");

            if (!ExistsTransaction)
                throw new Exception("No existe una transacción activa");

            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// Descarta los cambios realizados durante la transacción
        /// </summary>
        public void Transaction_Rollback()
        {
            UpdateLastAccess();

            if (IsInheritDataBase)
            {
                InheritDbManager.DataBase.Transaction_Rollback();
            }
            else
            {
                if (ExistsTransaction)
                {
                    _transaction.Rollback();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Ejecuta una sentencia SQL sin retornar datos
        /// </summary>
        /// <param name="nSql">Sentencia SQL a ejecutar</param>
        public void ExecuteNonQuery(string nSql)
        {
            if (Connection.State != ConnectionState.Open)
                throw new Exception("Debe existir una conexión abierta para poder ejecutar esta instrucción");

            UpdateLastAccess();

            var cmd = GetSqlCommand(nSql);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Ejectuta una sentencia SQL y almacena el resultado de la consulta en un DataTable
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se almacena la información</param>
        /// <param name="nSql">Sentencia SQL a ejecutar</param>
        public void ExecuteQueryFill(ref DataTable nDataTable, string nSql)
        {
            if (Connection.State != ConnectionState.Open)
                throw new Exception("Debe existir una conexión abierta para poder ejecutar esta instrucción");

            UpdateLastAccess();

            var cmd = GetSqlCommand(nSql);
            var da = DefaultFactory.CreateDataAdapter();

            if (da == null) return;

            da.SelectCommand = cmd;
            da.Fill(nDataTable);
        }

        /// <summary>
        /// Ejectuta un procedimiento almacenado que no devuelve datos
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        public void ExecuteStoredProcedure(string nSchemaName, string nStoredProcedure)
        {
            ExecuteStoredProcedure(nSchemaName, nStoredProcedure, null);
        }

        /// <summary>
        /// Ejectuta un procedimiento almacenado que no devuelve datos
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nParameters">Parametros del procedimiento almacenado</param>
        public void ExecuteStoredProcedure(string nSchemaName, string nStoredProcedure, List<Parameter> nParameters)
        {
            if (Connection.State != ConnectionState.Open)
                throw new Exception("Debe existir una conexión abierta para poder ejecutar esta instrucción");

            var cmd = GetStoredProcedureCommand(nSchemaName, nStoredProcedure);

            if (nParameters != null)
            {
                foreach (var par in nParameters)
                {
                    var parameter = CreateParameter(par);
                    cmd.Parameters.Add(parameter);
                }
            }

            UpdateLastAccess();
            UpdateLastQuery(nSchemaName, nStoredProcedure, nParameters);

            DbTransaction LocalTransaction = null;

            try
            {
                if (_transaction == null)
                    LocalTransaction = _connection.BeginTransaction();

                cmd.Connection = _connection;
                cmd.Transaction = LocalTransaction ?? _transaction;

                cmd.ExecuteNonQuery();

                if (LocalTransaction != null)
                    LocalTransaction.Commit();
            }
            catch
            {
                if (LocalTransaction != null)
                    LocalTransaction.Rollback();

                throw;
            }
        }

        /// <summary>
        /// Ejectuta un procedimiento almacenado que devuelve datos
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se almacenan los datos</param>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        public void ExecuteStoredProcedureFill(ref DataTable nDataTable, string nSchemaName, string nStoredProcedure)
        {
            ExecuteStoredProcedureFill(ref nDataTable, nSchemaName, nStoredProcedure, null);
        }

        /// <summary>
        /// Ejectuta un procedimiento almacenado que devuelve datos
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se almacenan los datos</param>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nParameters">Parametros del procedimiento almacenado</param>
        public void ExecuteStoredProcedureFill(ref DataTable nDataTable, string nSchemaName, string nStoredProcedure,
                                               List<Parameter> nParameters)
        {
            if (Connection.State != ConnectionState.Open)
                throw new Exception("Debe existir una conexión abierta para poder ejecutar esta instrucción");

            var cmd = GetStoredProcedureCommand(nSchemaName, nStoredProcedure, false);
            var da = DefaultFactory.CreateDataAdapter();
            if (da == null) throw new Exception("No se pudo crear el DataAdapter");
            da.SelectCommand = cmd;

            if (nParameters != null)
            {
                foreach (var par in nParameters)
                {
                    var parameter = CreateParameter(par);
                    cmd.Parameters.Add(parameter);
                }
            }

            UpdateLastAccess();
            UpdateLastQuery(nSchemaName, nStoredProcedure, nParameters);

            DbTransaction LocalTransaction = null;

            try
            {
                if (_transaction == null)
                    LocalTransaction = _connection.BeginTransaction();

                cmd.Connection = _connection;
                cmd.Transaction = LocalTransaction ?? _transaction;

                da.Fill(nDataTable);

                if (LocalTransaction != null)
                    LocalTransaction.Commit();
            }
            catch (Exception ex)
            {
                if (LocalTransaction != null)
                    LocalTransaction.Rollback();

                throw new Exception("Error al ejecutar el procedimiento " + nStoredProcedure + ". " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza la variable LastAccess con la fecha actual del sistema
        /// </summary>
        private void UpdateLastAccess()
        {
            _lastAccess = DateTime.Now;
        }

        /// <summary>
        /// Le indica al proxy que elimine la conexión remota
        /// </summary>
        public void Disconect()
        {
            if (_proxy != null)
            {
                try
                {
                    _proxy.DisconetDataBase(_token);
                }
                catch
                {
                }
            }
        }

        public void BeginBullkQuery()
        {
            UpdateLastAccess();

            _isBullkQuery = true;
        }

        public void EndBullkQuery()
        {
            _isBullkQuery = false;

            // Ejecutar
            var SQL = new StringBuilder("");

            while (_bullkQuery.Count > 0)
            {
                SQL.Append(_bullkQuery[0] + Identifier_EndLine_Symbol);
                _bullkQuery.RemoveAt(0);

                if (SQL.Length > 1000000)
                {
                    ExecuteNonQuery(SQL.ToString());
                    SQL = new StringBuilder("");
                }
            }

            if (SQL.Length > 0)
                ExecuteNonQuery(SQL.ToString());

            // Liberar memoria
            _bullkQuery.Clear();
            GC.Collect();
        }

        public void SendBullkQuery(int nLines)
        {
            // Ejecutar
            var SQL = new StringBuilder("");
            int Line = 0;

            while (_bullkQuery.Count > 0 && Line < nLines)
            {
                Line++;

                SQL.Append(_bullkQuery[0] + Identifier_EndLine_Symbol);
                _bullkQuery.RemoveAt(0);

                if (SQL.Length > 1000000)
                {
                    ExecuteNonQuery(SQL.ToString());
                    SQL = new StringBuilder("");
                }
            }

            if (SQL.Length > 0)
                ExecuteNonQuery(SQL.ToString());
        }

        public void CancelBullkQuery()
        {
            _isBullkQuery = false;

            // Liberar memoria
            _bullkQuery.Clear();
        }

        #endregion

        #region Funciones

        /// <summary>
        /// Ejecuta la instrucción SQL y retorna un DataTable con los datos devueltos por el servidor
        /// </summary>
        /// <param name="nSql">Sentencia SQL a ejecutar</param>
        /// <returns>Datos devueltos por el servidor</returns>
        public DataTable ExecuteQueryGet(string nSql)
        {
            UpdateLastAccess();

            var cmd = GetSqlCommand(nSql);
            var da = DefaultFactory.CreateDataAdapter();
            if (da == null) throw new Exception("No se pudo crear el DataAdapter");

            da.SelectCommand = cmd;
            var TempDataTable = new DataTable();
            da.Fill(TempDataTable);

            return TempDataTable;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna un DataTable con los datos devueltos por el servidor
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Procedimiento almacenado a ejecutar</param>
        /// <returns>Datos devueltos por el servidor</returns>
        public DataTable ExecuteStoredProcedureGet(string nSchemaName, string nStoredProcedure)
        {
            return ExecuteStoredProcedureGet(nSchemaName, nStoredProcedure, null);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna un DataTable con los datos devueltos por el servidor
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Procedimiento almacenado a ejecutar</param>
        /// <param name="nParameters">Parámetros del procedimiento almacenado</param>
        /// <returns>Datos devueltos por el servidor</returns>
        public DataTable ExecuteStoredProcedureGet(string nSchemaName, string nStoredProcedure,
                                                   List<Parameter> nParameters)
        {
            var TempDataTable = new DataTable();

            ExecuteStoredProcedureFill(ref TempDataTable, nSchemaName, nStoredProcedure, nParameters);

            return TempDataTable;
        }

        /// <summary>
        /// Obtiene el objeto de conexión a base de datos, si esta cerrada la conexión la abre antes de devolverla
        /// </summary>        
        /// <returns>Objeto de conexión a base de datos</returns>
        public DbConnection GetOpenConnection()
        {
            if (_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
                Connection_Open();

            return _connection;
        }

        /// <summary>
        /// Crea un nuevo objeto comando para una sentencia SQL
        /// </summary>
        /// <param name="nSqlString">Sentencia SQL del comando</param>
        /// <returns>Comando</returns>
        public DbCommand GetSqlCommand(string nSqlString)
        {
            var NewSqlCnd = CreateCommandByCommandType(CommandType.Text, nSqlString);
            return NewSqlCnd;
        }

        /// <summary>
        /// Crea un nuevo objeto comando para un procedimiento almacenado
        /// </summary>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <returns>Comando</returns>
        public DbCommand GetStoredProcedureCommand(string nStoredProcedure)
        {
            return GetStoredProcedureCommand("", nStoredProcedure, false);
        }

        /// <summary>
        /// Crea un nuevo objeto comando para un procedimiento almacenado
        /// </summary>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nDeriveParameters">Define si se derivan los parametros de la base de datos</param>
        /// <returns>Comando</returns>
        public DbCommand GetStoredProcedureCommand(string nStoredProcedure, bool nDeriveParameters)
        {
            return GetStoredProcedureCommand("", nStoredProcedure, nDeriveParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <returns>Comando</returns>
        public DbCommand GetStoredProcedureCommand(string nSchemaName, string nStoredProcedure)
        {
            return GetStoredProcedureCommand(nSchemaName, nStoredProcedure, false);
        }

        /// <summary>
        /// Crea un nuevo objeto comando para un procedimiento almacenado
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nDeriveParameters">Define si se derivan los parametros de la base de datos</param>
        /// <returns>Comando</returns>
        public DbCommand GetStoredProcedureCommand(string nSchemaName, string nStoredProcedure, bool nDeriveParameters)
        {
            var NewSqlCnd = CreateCommandByCommandType(CommandType.StoredProcedure,
                                                       FormatToDatabaseStoredProcedureName(nSchemaName, nStoredProcedure));

            if (nDeriveParameters)
                DeriveParameters(ref NewSqlCnd);

            return NewSqlCnd;
        }

        /// <summary>
        /// Crea un nuevo parametro
        /// </summary>
        /// <param name="nParameter">Datos de configuración del parámetro</param>
        /// <returns>Parámetro</returns>
        public DbParameter CreateParameter(Parameter nParameter)
        {
            var param = DefaultFactory.CreateParameter();
            if (param == null) throw new Exception("No se pudo crear el parametro");

            param.ParameterName = nParameter.Name;
            param.DbType = nParameter.Type;

            if (nParameter.Type == DbType.Guid)
            {
                var Valor = DBNulls.GetPrimitiveObjectValue(nParameter.Value);

                if (Valor == DBNull.Value)
                    param.Value = DBNull.Value;
                else
                    param.Value = Valor.ToString();
            }
            else
            {
                param.Value = DBNulls.GetPrimitiveObjectValue(nParameter.Value);
            }

            param.Size = nParameter.MaxLength;
            param.Direction = nParameter.Direction;

            return ConfigureParameter(param, nParameter);
        }

        /// <summary>
        /// Crea un nuevo comando de a cuerdo al tipo definido
        /// </summary>
        /// <param name="nCommandType">Tipo de comando a crear</param>
        /// <param name="nCommandText">Sentencia SQL del comando</param>
        /// <returns>Comando</returns>
        public DbCommand CreateCommandByCommandType(CommandType nCommandType, string nCommandText)
        {
            var NewSqlCnd = DefaultFactory.CreateCommand();
            if (NewSqlCnd == null) throw new Exception("No se pudo crear el comando");

            NewSqlCnd.Connection = _connection;

            if (_transaction != null)
                NewSqlCnd.Transaction = _transaction;

            NewSqlCnd.CommandText = nCommandText;
            NewSqlCnd.CommandTimeout = 0;
            NewSqlCnd.CommandType = nCommandType;

            return NewSqlCnd;
        }

        #endregion

        #region Scripts

        /// <summary>
        /// Construye la sentencia SQL para calcular la siguiente llave primaria de una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nParams"></param>
        /// <param name="nIdColumn">Campo a calcular</param>
        /// <returns>Sentencia SQL</returns>
        public string BuildSqlNextId(string nSchemaName, string nDataTableName, List<Parameter> nParams,
                                     string nIdColumn)
        {
            return new SqlBuilder(this).SqlNextId(nSchemaName, nDataTableName, nParams, nIdColumn);
        }

        /// <summary>
        /// Construye la sentencia SQL de inserción de un registro a una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nInParams">Datos a insertar</param>
        /// <returns>Sentencia SQL</returns>
        public string BuildSqlInsert(string nSchemaName, string nDataTableName, List<Parameter> nInParams)
        {
            return new SqlBuilder(this).SqlInsert(nSchemaName, nDataTableName, nInParams, _isBullkQuery);
        }

        /// <summary>
        /// Construye la sentencia SQL de actualización de datos de un registro en una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <param name="nInParams">Campos a actualizar</param>
        /// <returns>Sentencia SQL</returns>
        public string BuildSqlUpdate(string nSchemaName, string nDataTableName, List<Parameter> nKeys,
                                     List<Parameter> nInParams)
        {
            return new SqlBuilder(this).SqlUpdate(nSchemaName, nDataTableName, nKeys, nInParams);
        }

        /// <summary>
        /// Construye la sentencia SQL de eliminación de un registro en una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <returns>Sentencia SQL</returns>
        public string BuildSqlDelete(string nSchemaName, string nDataTableName, List<Parameter> nKeys)
        {
            return new SqlBuilder(this).SqlDelete(nSchemaName, nDataTableName, nKeys);
        }

        /// <summary>
        /// Construye la sentencia SQL de consulta por las llaves primarias de una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Sentencia SQL</returns>
        public string BuildSqlGet(string nSchemaName, string nDataTableName, List<Parameter> nKeys, int nMaxRows,
                                  ColumnEnumList nOrderByParams)
        {
            return new SqlBuilder(this).SqlGet(nSchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams);
        }

        /// <summary>
        /// Construye la sentencia SQL de consulta a una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nParams"></param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Sentencia SQL</returns>
        public string BuildSqlFilter(string nSchemaName, string nDataTableName, List<Parameter> nParams, int nMaxRows,
                                     ColumnEnumList nOrderByParams)
        {
            return new SqlBuilder(this).SqlFilter(nSchemaName, nDataTableName, nParams, nMaxRows, nOrderByParams);
        }

        /// <summary>
        /// Construye la sentencia SQL Avanzada a una serie de tablas
        /// </summary>
        /// <param name="nTableAlias">Alias de la tabla</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Campos de filtrado</param>
        /// <param name="nTableRelationList">Lista de relaciones con otras tablas</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <returns>Sentencia SQL</returns>
        public string BuildSqlQuery(string nTableAlias, string nSchemaName, string nDataTableName, List<Parameter> nKeys,
                                    TableRelationCollection nTableRelationList, int nMaxRows,
                                    ColumnEnumList nOrderByParams)
        {
            return new SqlBuilder(this).SqlQuery(nTableAlias, nSchemaName, nDataTableName, nKeys, nTableRelationList,
                                                 nMaxRows, nOrderByParams);
        }

        #endregion

        #region Sentencias de tabla

        /// <summary>
        /// Permite calcular la siguiente llave primaria para una determinada tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nInParams">Columnas de la llave principal sin incluir la columna calculada</param>
        /// <param name="nIdColumn">Columna a la que se le va a calcular el NextId</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Siguiente id</returns>
        public int DBNextId(string nSchemaName, string nDataTableName, List<Parameter> nInParams, string nIdColumn,
                            out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                if (_isBullkQuery)
                    throw new Exception("La instrucción NextId no es permitida durante un proceso BullkQuery");

                if (_transaction == null)
                    throw new Exception("La función NextId requiere una transacción activa, " + nDataTableName);

                var table = ExecuteQueryGet(BuildSqlNextId(nSchemaName, nDataTableName, nInParams, nIdColumn));

                if (table.Rows.Count == 0)
                    return 1;
                
                if (table.Rows[0].IsNull(0))
                    return 1;
                
                return int.Parse(table.Rows[0][0].ToString()) + 1;
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al generar el NextId de la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
                return 0;
            }
        }


        /// <summary>
        /// Permite insertar un nuevo registro en una tabla determinada
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nInParams">Valores a insertar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBInsert(string nSchemaName, string nDataTableName, List<Parameter> nInParams, out bool nResult,
                             out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                var SQL = BuildSqlInsert(nSchemaName, nDataTableName, nInParams);

                if (_isBullkQuery)
                    _bullkQuery.Add(SQL);
                else
                    ExecuteNonQuery(SQL);
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al insertar el registro en la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite insertar un nuevo registro en una tabla determinada generando automáticamente el siguiente id
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nInParams">datos a insertar</param>
        /// <param name="nPrimaryKeysMapping">Columnas de la llave principal sin incluir la columna calculada</param>
        /// <param name="nIdColumn">Columna a la que se le va a calcular el NextId</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBInsertAutoNextId(string nSchemaName, string nDataTableName, List<Parameter> nInParams,
                                       List<Parameter> nPrimaryKeysMapping, string nIdColumn, out bool nResult,
                                       out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                if (_isBullkQuery)
                    throw new Exception("La instrucción NextId no es permitida durante un proceso BullkQuery");

                var primaryForeingKeys = new List<Parameter>();

                foreach (var key in nPrimaryKeysMapping)
                {
                    if (key.Name != nIdColumn)
                    {
                        Parameter par = key.Clone();
                        par.Value = Parameter.Find(nInParams, key.Name).Value;
                    }
                }

                var NextId = DBNextId(nSchemaName, nDataTableName, primaryForeingKeys, nIdColumn, out nResult,
                                      out nException);
                if (!nResult) return;

                var insertParams = new List<Parameter>();

                foreach (var par in nInParams)
                {
                    Parameter tableParam = par.Clone();

                    if (tableParam.Name == nIdColumn)
                        tableParam.Value = NextId;

                    insertParams.Add(par);
                }

                ExecuteNonQuery(BuildSqlInsert(nSchemaName, nDataTableName, insertParams));
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al insertar el registro en la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite actualizar la data de un registro de una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nInParams">Campos a actualizar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBUpdate(string nSchemaName, string nDataTableName, List<Parameter> nKeys, List<Parameter> nInParams,
                             out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                var SQL = BuildSqlUpdate(nSchemaName, nDataTableName, nKeys, nInParams);

                if (_isBullkQuery)
                    _bullkQuery.Add(SQL);
                else
                    ExecuteNonQuery(SQL);
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al actualizar el registro en la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite eliminar un registro de una tabla
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBDelete(string nSchemaName, string nDataTableName, List<Parameter> nKeys, out bool nResult,
                             out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                var SQL = BuildSqlDelete(nSchemaName, nDataTableName, nKeys);

                if (_isBullkQuery)
                    _bullkQuery.Add(SQL);
                else
                    ExecuteNonQuery(SQL);
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al eliminar el registro en la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por la llave primaria
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBGet(string nSchemaName, string nDataTableName, List<Parameter> nKeys, out bool nResult,
                               out Exception nException)
        {
            return DBGet(nSchemaName, nDataTableName, nKeys, 0, null, out nResult, out nException);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por la llave primaria
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBGet(string nSchemaName, string nDataTableName, List<Parameter> nKeys, int nMaxRows,
                               ColumnEnumList nOrderByParams, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                return ExecuteQueryGet(BuildSqlGet(nSchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams));
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al recuperar los registros de la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
                return null;
            }
        }


        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de una tabla existente, filtrando por la llave primaria
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBFill(ref DataTable nDataTable, string nSchemaName, string nDataTableName, List<Parameter> nKeys,
                           out bool nResult, out Exception nException)
        {
            DBFill(ref nDataTable, nSchemaName, nDataTableName, nKeys, 0, null, out nResult, out nException);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de una tabla existente, filtrando por la llave primaria
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBFill(ref DataTable nDataTable, string nSchemaName, string nDataTableName, List<Parameter> nKeys,
                           int nMaxRows, ColumnEnumList nOrderByParams, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                ExecuteQueryFill(ref nDataTable,
                                 BuildSqlGet(nSchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams));
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al recuperar los registros de la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por diferentes campos
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBFilterGet(string nSchemaName, string nDataTableName, List<Parameter> nKeys, out bool nResult,
                                     out Exception nException)
        {
            return DBFilterGet(nSchemaName, nDataTableName, nKeys, 0, null, out nResult, out nException);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla filtrando por diferentes campos
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBFilterGet(string nSchemaName, string nDataTableName, List<Parameter> nKeys, int nMaxRows,
                                     ColumnEnumList nOrderByParams, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                return ExecuteQueryGet(BuildSqlFilter(nSchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams));
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al recuperar los registros de la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
                return null;
            }
        }


        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de un DataTable existente, filtrando por diferentes campos 
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBFilterFill(ref DataTable nDataTable, string nSchemaName, string nDataTableName,
                                 List<Parameter> nKeys, out bool nResult, out Exception nException)
        {
            DBFilterFill(ref nDataTable, nSchemaName, nDataTableName, nKeys, 0, null, out nResult, out nException);
        }

        /// <summary>
        /// Permite obtener los registro de una tabla, dentro de un DataTable existente, filtrando por diferentes campos 
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBFilterFill(ref DataTable nDataTable, string nSchemaName, string nDataTableName,
                                 List<Parameter> nKeys, int nMaxRows, ColumnEnumList nOrderByParams, out bool nResult,
                                 out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                ExecuteQueryFill(ref nDataTable,
                                 BuildSqlFilter(nSchemaName, nDataTableName, nKeys, nMaxRows, nOrderByParams));
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al recuperar los registros de la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener los registros de varias tablas filtrando por diferentes campos
        /// </summary>
        /// <param name="nTableAlias"></param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nTableRelationList">Lista de relaciones con otras tablas</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBQueryGet(string nTableAlias, string nSchemaName, string nDataTableName, List<Parameter> nKeys,
                                    TableRelationCollection nTableRelationList, out bool nResult,
                                    out Exception nException)
        {
            return DBQueryGet(nTableAlias, nSchemaName, nDataTableName, nKeys, nTableRelationList, 0, null, out nResult,
                              out nException);
        }

        /// <summary>
        /// Permite obtener los registros de varias tablas filtrando por diferentes campos
        /// </summary>
        /// <param name="nTableAlias"></param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nTableRelationList">Lista de relaciones con otras tablas</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBQueryGet(string nTableAlias, string nSchemaName, string nDataTableName, List<Parameter> nKeys,
                                    TableRelationCollection nTableRelationList, int nMaxRows,
                                    ColumnEnumList nOrderByParams, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                return
                    ExecuteQueryGet(BuildSqlQuery(nTableAlias, nSchemaName, nDataTableName, nKeys, nTableRelationList,
                                                  nMaxRows, nOrderByParams));
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al recuperar los registros de la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
                return null;
            }
        }


        /// <summary>
        /// Ejecuta una instruccion nativa SQL en el motor de base de datos y devuelve los resultados en un DataTable
        /// </summary>
        /// <param name="nSql">Instruccion Select a ejecutar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBExecuteSqlGet(string nSql, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                return ExecuteQueryGet(nSql);
            }
            catch (Exception ex)
            {
                nResult = false;
                nException = new Exception("Error al ejecutar la consulta, " + ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Ejecuta una instruccion nativa SQL en el motor de base de datos y devuelve los resultados en un DataTable
        /// </summary>
        /// <param name="nDataTable">Tabla en la que se poblaran los datos</param>
        /// <param name="nSql">Instruccion Select a ejecutar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public void DBExecuteSqlFill(ref DataTable nDataTable, string nSql, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                ExecuteQueryFill(ref nDataTable, nSql);
            }
            catch (Exception ex)
            {
                nResult = false;
                nException = new Exception("Error al ejecutar la consulta, " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los registros de varias tablas filtrando por diferentes campos
        /// </summary>
        /// <param name="nDataTable">Resultado de la consulta</param>
        /// <param name="nTableAlias"></param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nTableRelationList">Lista de relaciones con otras tablas</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBQueryFill(ref DataTable nDataTable, string nTableAlias, string nSchemaName, string nDataTableName,
                                List<Parameter> nKeys, TableRelationCollection nTableRelationList, out bool nResult,
                                out Exception nException)
        {
            DBQueryFill(ref nDataTable, nTableAlias, nSchemaName, nDataTableName, nKeys, nTableRelationList, 0, null,
                        out nResult, out nException);
        }

        /// <summary>
        /// Permite obtener los registros de varias tablas filtrando por diferentes campos
        /// </summary>
        /// <param name="nDataTable">Resultado de la consulta</param>
        /// <param name="nTableAlias"></param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nKeys">Llave primaria del registro</param>
        /// <param name="nTableRelationList">Lista de relaciones con otras tablas</param>
        /// <param name="nMaxRows">Número máximo de registros a retornar, 0 para devolver todas la filas</param>
        /// <param name="nOrderByParams">Parametros de ordenación, null para no rodenar</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBQueryFill(ref DataTable nDataTable, string nTableAlias, string nSchemaName, string nDataTableName,
                                List<Parameter> nKeys, TableRelationCollection nTableRelationList, int nMaxRows,
                                ColumnEnumList nOrderByParams, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                ExecuteQueryFill(ref nDataTable,
                                 BuildSqlQuery(nTableAlias, nSchemaName, nDataTableName, nKeys, nTableRelationList,
                                               nMaxRows, nOrderByParams));
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al recuperar los registros de la tabla " + nSchemaName + "." + nDataTableName + ", " +
                        ex.Message, ex);
            }
        }


        /// <summary>
        /// Ejecuta un procedimiento almacenado que no devuelve registros
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBExecuteStoredProcedure(string nSchemaName, string nStoredProcedure, out bool nResult,
                                             out Exception nException)
        {
            DBExecuteStoredProcedure(nSchemaName, nStoredProcedure, null, out nResult, out nException);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que no devuelve registros
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nParameters">Parámetros del procedimiento almacenado</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBExecuteStoredProcedure(string nSchemaName, string nStoredProcedure, List<Parameter> nParameters,
                                             out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                ExecuteStoredProcedure(nSchemaName, nStoredProcedure, nParameters);
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al ejecutar el procedimiento almacenado " + nSchemaName + "." + nStoredProcedure + ", " +
                        ex.Message, ex);
            }
        }


        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve registros
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBExecuteStoredProcedureGet(string nSchemaName, string nStoredProcedure, out bool nResult,
                                                     out Exception nException)
        {
            return DBExecuteStoredProcedureGet(nSchemaName, nStoredProcedure, null, out nResult, out nException);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve registros
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nParameters">Parametros del procedimiento almacenado</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        /// <returns>Resultado de la consulta</returns>
        public DataTable DBExecuteStoredProcedureGet(string nSchemaName, string nStoredProcedure,
                                                     List<Parameter> nParameters, out bool nResult,
                                                     out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                return ExecuteStoredProcedureGet(nSchemaName, nStoredProcedure, nParameters);
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al ejecutar el procedimiento almacenado " + nSchemaName + "." + nStoredProcedure + ", " +
                        ex.Message, ex);
                return null;
            }
        }


        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve registros
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBExecuteStoredProcedureFill(ref DataTable nDataTable, string nSchemaName, string nStoredProcedure,
                                                 out bool nResult, out Exception nException)
        {
            DBExecuteStoredProcedureFill(ref nDataTable, nSchemaName, nStoredProcedure, null, out nResult,
                                         out nException);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve registros
        /// </summary>
        /// <param name="nDataTable">DataTable en el que se devolveran los registros</param>
        /// <param name="nSchemaName">Esquema al que pertenece el procedimiento almacenado</param>
        /// <param name="nStoredProcedure">Nombre del procedimiento almacenado</param>
        /// <param name="nParameters">Parametros del procedimiento almacenado</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBExecuteStoredProcedureFill(ref DataTable nDataTable, string nSchemaName, string nStoredProcedure,
                                                 List<Parameter> nParameters, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                ExecuteStoredProcedureFill(ref nDataTable, nSchemaName, nStoredProcedure, nParameters);
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al ejecutar el procedimiento almacenado " + nSchemaName + "." + nStoredProcedure + ", " +
                        ex.Message, ex);
            }
        }


        /// <summary>
        /// Aplica las modificaciones realizadas a los registros de un DataTable, calculando automáticamente el id siguiente, a la tabla en la base de datos
        /// </summary>
        /// <param name="nDataTableData">DataTable con las modificaciones</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nColumnsMapping">Campos de la tabla</param>
        /// <param name="nPrimaryKeysMapping">Campos de la llave primaria sin incluir el campo a calcular el NextId</param>
        /// <param name="nIdColumn">Campo a calcular el NextId</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBSaveTableAutoNextId(ref DataTable nDataTableData, string nSchemaName, string nDataTableName,
                                          List<Parameter> nColumnsMapping, List<Parameter> nPrimaryKeysMapping,
                                          string nIdColumn, out bool nResult, out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                foreach (DataRow row in nDataTableData.Select())
                {
                    try
                    {
                        switch (row.RowState)
                        {
                            case DataRowState.Added:
                                var insertParams = new List<Parameter>();

                                foreach (var par in nColumnsMapping)
                                {
                                    insertParams.Add(new Parameter(par.Name, par.Type, par.SpecificType, row[par.Name],
                                                                   par.IsNullable, par.MaxLength, par.Precision,
                                                                   par.Scale, par.Direction));
                                }

                                DBInsertAutoNextId(nSchemaName, nDataTableName, insertParams, nPrimaryKeysMapping,
                                                   nIdColumn, out nResult, out nException);
                                if (!nResult) return;

                                break;

                            case DataRowState.Deleted:
                                var deleteParams = new List<Parameter>();

                                foreach (var par in nPrimaryKeysMapping)
                                {
                                    deleteParams.Add(new Parameter(par.Name, par.Type, par.SpecificType,
                                                                   row[par.Name, DataRowVersion.Original],
                                                                   par.IsNullable, par.MaxLength, par.Precision,
                                                                   par.Scale, par.Direction));
                                }

                                DBDelete(nSchemaName, nDataTableName, deleteParams, out nResult, out nException);
                                if (!nResult) return;

                                break;

                            case DataRowState.Detached:
                                break;

                            case DataRowState.Modified:
                                var keyParams = new List<Parameter>();

                                foreach (var par in nPrimaryKeysMapping)
                                {
                                    keyParams.Add(new Parameter(par.Name, par.Type, par.SpecificType,
                                                                row[par.Name, DataRowVersion.Original], par.IsNullable,
                                                                par.MaxLength, par.Precision, par.Scale, par.Direction));
                                }

                                var columnsParams = new List<Parameter>();

                                foreach (var par in nColumnsMapping)
                                {
                                    columnsParams.Add(new Parameter(par.Name, par.Type, par.SpecificType, row[par.Name],
                                                                    par.IsNullable, par.MaxLength, par.Precision,
                                                                    par.Scale, par.Direction));
                                }

                                DBUpdate(nSchemaName, nDataTableName, keyParams, columnsParams, out nResult,
                                         out nException);
                                if (!nResult) return;

                                break;

                            case DataRowState.Unchanged:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            "No fue posible actualizar el registro " + row[0] + ", " + ex.Message, ex);
                    }
                }

                nDataTableData.AcceptChanges();
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al guardar la información de la tabla " + nDataTableData.TableName + ", " + ex.Message,
                        ex);
            }
        }


        /// <summary>
        /// Aplica las modificaciones realizadas a los registros de un DataTable, a la tabla en la base de datos
        /// </summary>
        /// <param name="nDataTableData">DataTable con las modificaciones</param>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <param name="nColumnsMapping">Campos de la tabla</param>
        /// <param name="nPrimaryKeysMapping">Campos de la llave primaria</param>
        /// <param name="nResult">Define si la instrucción se ejecutó correctamente o no</param>
        /// <param name="nException">Exepción ocurrida al ejecutar la instrucción</param>
        public void DBSaveTable(ref DataTable nDataTableData, string nSchemaName, string nDataTableName,
                                List<Parameter> nColumnsMapping, List<Parameter> nPrimaryKeysMapping, out bool nResult,
                                out Exception nException)
        {
            nResult = true;
            nException = null;

            try
            {
                foreach (DataRow row in nDataTableData.Rows)
                {
                    try
                    {
                        switch (row.RowState)
                        {
                            case DataRowState.Added:
                                var insertParams = new List<Parameter>();

                                foreach (var par in nColumnsMapping)
                                {
                                    insertParams.Add(new Parameter(par.Name, par.Type, par.SpecificType, row[par.Name],
                                                                   par.IsNullable, par.MaxLength, par.Precision,
                                                                   par.Scale, par.Direction));
                                }

                                DBInsert(nSchemaName, nDataTableName, insertParams, out nResult, out nException);
                                if (!nResult) return;

                                break;

                            case DataRowState.Deleted:
                                var deleteParams = new List<Parameter>();

                                foreach (var par in nPrimaryKeysMapping)
                                {
                                    deleteParams.Add(new Parameter(par.Name, par.Type, par.SpecificType,
                                                                   row[par.Name, DataRowVersion.Original],
                                                                   par.IsNullable, par.MaxLength, par.Precision,
                                                                   par.Scale, par.Direction));
                                }

                                DBDelete(nSchemaName, nDataTableName, deleteParams, out nResult, out nException);
                                if (!nResult) return;

                                break;

                            case DataRowState.Detached:
                                break;

                            case DataRowState.Modified:
                                var keyParams = new List<Parameter>();

                                foreach (var par in nPrimaryKeysMapping)
                                {
                                    keyParams.Add(new Parameter(par.Name, par.Type, par.SpecificType,
                                                                row[par.Name, DataRowVersion.Original], par.IsNullable,
                                                                par.MaxLength, par.Precision, par.Scale, par.Direction));
                                }

                                var columnsParams = new List<Parameter>();

                                foreach (var par in nColumnsMapping)
                                {
                                    columnsParams.Add(new Parameter(par.Name, par.Type, par.SpecificType, row[par.Name],
                                                                    par.IsNullable, par.MaxLength, par.Precision,
                                                                    par.Scale, par.Direction));
                                }

                                DBUpdate(nSchemaName, nDataTableName, keyParams, columnsParams, out nResult,
                                         out nException);
                                if (!nResult) return;

                                break;

                            case DataRowState.Unchanged:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            "No fue posible actualizar el registro " + row[0] + ", " + ex.Message, ex);
                    }
                }

                nDataTableData.AcceptChanges();
            }
            catch (Exception ex)
            {
                nResult = false;
                nException =
                    new Exception(
                        "Error al guardar la información de la tabla " + nDataTableData.TableName + ", " + ex.Message,
                        ex);
            }
        }

        #endregion

        #region Abstractas

        /// <summary>
        /// Deriva los parametros de un procedimiento almacenado
        /// </summary>
        /// <param name="cmd">Comando en el que se almacenarán los parámetros</param>
        public abstract void DeriveParameters(ref DbCommand cmd);

        /// <summary>
        /// Crea un parámetro por nombre
        /// </summary>
        /// <param name="name">Nombre del parámetro</param>
        /// <returns>Nuevo parámetro</returns>
        public abstract string BuildParameterName(string name);

        /// <summary>
        /// Configura un parámetro
        /// </summary>
        /// <param name="param">Parametro a configurar</param>
        /// <param name="nParameter">Configuración a aplicar</param>
        /// <returns>Parámetro configurado</returns>
        public abstract DbParameter ConfigureParameter(DbParameter param, Parameter nParameter);

        /// <summary>
        /// Obtiene la definición de las tablas de una base de datos
        /// </summary>
        /// <param name="nObjectTable">DataTable en el que se almacenan las definiciones</param>
        /// <param name="nConnection">Conexión a usar para obtener los datos</param>
        /// <param name="nSchemaFilter"></param>
        public abstract void FillDataBaseTables(XsdDataBase.TBL_ObjectDataTable nObjectTable,
                                                XsdDataBase.TBL_ConnectionRow nConnection, List<string> nSchemaFilter);

        /// <summary>
        /// Obtiene la definición de las columnas de una tabla
        /// </summary>
        /// <param name="nFieldTable">DataTable en el que se almacenan las definiciones</param>
        /// <param name="nRelationDataTable">Tablas relacionadas</param>
        /// <param name="nDataTable">Tabla a la que pertenecen las columnas</param>
        public abstract void FillDataTableColumns(XsdDataBase.TBL_FieldDataTable nFieldTable,
                                                  XsdDataBase.TBL_RelationDataTable nRelationDataTable,
                                                  XsdDataBase.TBL_ObjectRow nDataTable);

        /// <summary>
        /// Obtiene la definición de las vistas de una base de datos
        /// </summary>
        /// <param name="nObjectTable"></param>
        /// <param name="nConnection"></param>
        /// <param name="nSchemaFilter"></param>
        public abstract void FillDataBaseViews(XsdDataBase.TBL_ObjectDataTable nObjectTable,
                                               XsdDataBase.TBL_ConnectionRow nConnection, List<string> nSchemaFilter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nObjectTable"></param>
        /// <param name="nConnection"></param>
        /// <param name="nSchemaFilter"></param>
        public abstract void FillDataBaseStoredProcedures(XsdDataBase.TBL_ObjectDataTable nObjectTable,
                                                          XsdDataBase.TBL_ConnectionRow nConnection,
                                                          List<string> nSchemaFilter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nFieldTable"></param>
        /// <param name="nDataTable"></param>
        public abstract void FillDataBaseParameters(XsdDataBase.TBL_FieldDataTable nFieldTable,
                                                    XsdDataBase.TBL_ObjectRow nDataTable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nFilter"></param>
        /// <returns></returns>
        public abstract XsdDataBaseObjects.dbschemaDataTable GetDataBaseSchemas(string nFilter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCatalog_Name"></param>
        /// <param name="nSchema_Name"></param>
        /// <param name="nFilterTableName"></param>
        /// <returns></returns>
        public abstract XsdDataBaseObjects.dbobjectDataTable GetDataBaseTables(string nCatalog_Name, string nSchema_Name,
                                                                               string nFilterTableName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCatalog_Name"></param>
        /// <param name="nSchema_Name"></param>
        /// <param name="nFilterViewName"></param>
        /// <returns></returns>
        public abstract XsdDataBaseObjects.dbobjectDataTable GetDataBaseViews(string nCatalog_Name, string nSchema_Name,
                                                                              string nFilterViewName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCatalog_Name"></param>
        /// <param name="nSchema_Name"></param>
        /// <param name="nObjectName"></param>
        /// <returns></returns>
        public abstract XsdDataBaseObjects.dbfieldDataTable GetDataBaseFields(string nCatalog_Name, string nSchema_Name,
                                                                              string nObjectName);

        /// <summary>
        /// Transforma el tipo de parametro de base en un tipo de datos genérico para la aplicación
        /// </summary>
        /// <param name="nDataBaseParameterType">Nombre del tipo de párametro de base de datos</param>
        /// <returns>Tipo de parametro genérico</returns>
        /// <remarks>Transforma el tipo de parametro de base en un tipo de datos genérico para la aplicación</remarks>
        public abstract System.Data.DbType GetGenericParameterType(string nDataBaseParameterType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nDataBaseParameterDirection"></param>
        /// <returns></returns>
        public abstract string GetGenericParameterDirection(string nDataBaseParameterDirection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="nMaxRows">Número máximo de registros a retornar</param>
        /// <returns></returns>
        public abstract string ConvertSqlSelectToMaxRows(string sql, int nMaxRows);

        public abstract string ConvertSqlSelectToOrderBy(string sql, ColumnEnumList GroupByParams);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract string GetDataFilterString(DataFilterType type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public abstract string ConvertToBinaryString(object nValue);

        public abstract string FormatToDatabaseEspecialValue(EspecialescargomasterNullable Value);

        #endregion

        #region Auxiliares

        /// <summary>
        /// Actualiza la variable LastQuery para la ejecución de procedimientos almacenados
        /// </summary>        
        /// <param name="nSchemaName"></param>
        /// <param name="nDataTableName"></param>
        /// <param name="nParameters"></param>
        private void UpdateLastQuery(string nSchemaName, string nDataTableName, List<Parameter> nParameters)
        {
            var query = new StringBuilder("");

            query.Append(FormatToDatabaseStoredProcedureName(nSchemaName, nDataTableName));

            for (var i = 0; i < nParameters.Count; i++)
            {
                query.Append(i == 0 ? " " : ", ");
                query.Append(nParameters[i].Name + "=" + FormatToDatabaseStringValue(nParameters[i], false));
            }

            LastQuery = query.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public static bool IsNull(object nValue)
        {
            return DBNulls.IsNull(nValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public static bool IsDbNull(object nValue)
        {
            return DBNulls.IsDbNull(nValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEspecial(object value)
        {
            return DBNulls.IsEspecial(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EspecialescargomasterNullable getEspecial(object value)
        {
            return DBNulls.GetEspecial(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nParam"></param>
        /// <param name="nIsFilter"></param>
        /// <returns></returns>
        public string FormatToDatabaseStringValue(Parameter nParam, bool nIsFilter)
        {
            if (nParam.Value == null) return "Null";
            if (IsNull(nParam.Value)) return "Null";
            if (IsDbNull(nParam.Value)) return "Null";

            if (IsEspecial(nParam.Value)) return FormatToDatabaseEspecialValue(getEspecial(nParam.Value));

            object primitiveValue = DBNulls.GetPrimitiveObjectValue(nParam.Value);

            switch (nParam.Type)
            {
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                    return Identifier_String_Symbol +
                           FormatToComodinSymbolStringValue(primitiveValue.ToString(), nIsFilter)
                               .Replace(Identifier_String_Symbol, Identifier_String_Symbol + Identifier_String_Symbol) +
                           Identifier_String_Symbol;

                case DbType.Binary:
                    //return "0x" + System.BitConverter.ToString((byte[])primitiveValue).Replace("-", "");
                    return ConvertToBinaryString(primitiveValue);

                case DbType.Boolean:
                    return AppTypes.GetDbDefaultBoolean((bool) primitiveValue);

                case DbType.Currency:
                case DbType.Double:
                case DbType.Single:
                case DbType.Decimal:
                    return primitiveValue.ToString().Replace(',', '.');

                case DbType.DateTime:
                case DbType.Date:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.Time:
                    return Identifier_Date_Mask.Replace("$$DATE$$",
                                                        DBNulls.GetDateTime(nParam.Value)
                                                               .ToString(Identifier_Date_Format));

                case DbType.Byte:
                    //return ((byte)primitiveValue).ToString();
                    return primitiveValue.ToString();

                case DbType.Guid:
                    return Identifier_String_Symbol + primitiveValue + Identifier_String_Symbol;

                case DbType.Int16:
                    //return ((short)primitiveValue).ToString();
                    return primitiveValue.ToString();

                case DbType.Int32:
                    //return ((int)primitiveValue).ToString();
                    return primitiveValue.ToString();

                case DbType.Int64:
                    //return ((long)primitiveValue).ToString();
                    return primitiveValue.ToString();

                case DbType.Object:
                    switch (primitiveValue.GetType().FullName)
                    {
                        case "System.String":
                        case "System.Guid":
                        case "System.Char":
                            return Identifier_String_Symbol +
                                   FormatToComodinSymbolStringValue(primitiveValue.ToString(), nIsFilter)
                                       .Replace(Identifier_String_Symbol,
                                                Identifier_String_Symbol + Identifier_String_Symbol) +
                                   Identifier_String_Symbol;

                        case "System.Boolean":
                            return AppTypes.GetDbDefaultBoolean((bool) primitiveValue);

                        case "System.DateTime":
                            return Identifier_Date_Mask.Replace("$$DATE$$",
                                                                DBNulls.GetDateTime(nParam.Value)
                                                                       .ToString(Identifier_Date_Format));

                        case "System.Decimal":
                        case "System.Double":
                        case "System.Single":
                            return primitiveValue.ToString().Replace(',', '.');

                        case "System.Byte":
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.UInt16":
                        case "System.UInt32":
                        case "System.UInt64":
                            return primitiveValue.ToString();

                        default:
                            throw new Exception("El tipo de dato " + primitiveValue.GetType().FullName +
                                                " no esta soportado por la DAL");
                    }

                case DbType.SByte:
                    //return ((sbyte)(primitiveValue)).ToString();
                    return primitiveValue.ToString();

                case DbType.UInt16:
                    //return ((ushort)primitiveValue).ToString();
                    return primitiveValue.ToString();

                case DbType.UInt32:
                    //return ((uint)primitiveValue).ToString();
                    return primitiveValue.ToString();

                case DbType.UInt64:
                    //return ((ulong)primitiveValue).ToString();
                    return primitiveValue.ToString();

                case DbType.VarNumeric:
                    //return ((decimal)primitiveValue).ToString();
                    return primitiveValue.ToString();

                case DbType.Xml:
                    return Identifier_String_Symbol + primitiveValue + Identifier_String_Symbol;


                default:
                    return primitiveValue.ToString();
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="nValue"></param>
        ///// <returns></returns>
        public string FormatStringToDatabaseStringValue(string nValue, bool nIsFilter)
        {
            return Identifier_String_Symbol +
                   FormatToComodinSymbolStringValue(nValue, nIsFilter)
                       .Replace(Identifier_String_Symbol, Identifier_String_Symbol + Identifier_String_Symbol) +
                   Identifier_String_Symbol;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="nIsFilter"></param>
        /// <returns></returns>
        public string FormatToComodinSymbolStringValue(string Value, bool nIsFilter)
        {
            if (!nIsFilter)
                return Value;

            if (Value.IndexOf(Identifier_Comodin_Similar) > -1)
                return Value.Replace(Identifier_Comodin_Similar, Identifier_Symbol_Similar);

            if (Value.IndexOf(Identifier_Comodin_Like) > -1)
                return Value.Replace(Identifier_Comodin_Like, Identifier_Symbol_Like);

            return Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nName"></param>
        /// <returns></returns>
        public string FormatToDatabaseColumnName(string nName)
        {
            return Identifier_Table_Prefix + nName + Identifier_Table_Postfix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <returns></returns>
        public string FormatToDatabaseTableName(string nSchemaName, string nDataTableName)
        {
            return FormatCatalogIdentifier() + Identifier_Table_Prefix + nSchemaName + Identifier_Table_Postfix +
                   Identifier_Schema_Separator + Identifier_Table_Prefix + nDataTableName + Identifier_Table_Postfix;
        }

        public string FormatCatalogIdentifier()
        {
            if (CurrentDataBaseCatalogName == "") return CurrentDataBaseCatalogName;

            return Identifier_Table_Prefix + CurrentDataBaseCatalogName + Identifier_Table_Postfix +
                   Identifier_Schema_Separator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nSchemaName">Esquema al que pertenece la tabla</param>
        /// <param name="nDataTableName">Nombre de la tabla</param>
        /// <returns></returns>
        public virtual string FormatToDatabaseStoredProcedureName(string nSchemaName, string nDataTableName)
        {
            if (nSchemaName != "")
                return FormatCatalogIdentifier() + Identifier_Procedure_Prefix + nSchemaName +
                       Identifier_Procedure_Postfix + Identifier_Schema_Separator + Identifier_Procedure_Prefix +
                       nDataTableName + Identifier_Procedure_Postfix;            
            
            return FormatCatalogIdentifier() + Identifier_Procedure_Prefix + nDataTableName +
                       Identifier_Procedure_Postfix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nParam"></param>
        /// <returns></returns>
        public string FormatToComparisonOperator(Parameter nParam)
        {
            if (IsNull(nParam.Value) || IsDbNull(nParam.Value))
                return Identifier_Operator_Null;
            
            if (nParam.Value.ToString().IndexOf(Identifier_Comodin_Similar) > -1)
                return Identifier_Operator_Similar;
            if (nParam.Value.ToString().IndexOf(Identifier_Comodin_Like) > -1)
                return Identifier_Operator_Like;
            
            return Identifier_Operator_Equal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public string FormatToComparisonOperator(string nValue)
        {
            if (nValue.IndexOf(Identifier_Comodin_Similar) > -1)
                return Identifier_Operator_Similar;
            
            if (nValue.IndexOf(Identifier_Comodin_Like) > -1)
                return Identifier_Operator_Like;
            
            return Identifier_Operator_Equal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int ConvertToInt(object Value)
        {
            return Value == DBNull.Value ? 0 : int.Parse(Value.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static byte ConvertToByte(object Value)
        {
            return Value == DBNull.Value ? (byte)0 : byte.Parse(Value.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nKeyParam"></param>
        /// <returns></returns>
        public static bool IsNumericType(Parameter nKeyParam)
        {
            switch (nKeyParam.Type)
            {
                case DbType.Byte:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Single:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nKeyParam"></param>
        /// <returns></returns>
        public static bool IsStringType(Parameter nKeyParam)
        {
            switch (nKeyParam.Type)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nKeyParam"></param>
        /// <returns></returns>
        public static bool IsDateType(Parameter nKeyParam)
        {
            switch (nKeyParam.Type)
            {
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Devuleve los registros no repetidos de un conjunto de registros para una columna determinada
        /// </summary>
        /// <param name="nRows">Conjunto de registros</param>
        /// <param name="nColumnName">Nombre de la columna</param>
        /// <returns>Registros unicos</returns>
        public static DataTable GetDistinctRows(DataRow[] nRows, string nColumnName)
        {
            var dt = new DataTable();
            dt.Columns.Add(nColumnName);
            dt.Columns.Add("Count", typeof (int));

            foreach (var row in nRows)
            {
                string s = row[nColumnName].ToString();
                DataRow[] drx = dt.Select(nColumnName + "= '" + s + "'");

                if (drx.Length == 0)
                    dt.Rows.Add(new object[] {s, 1});
                else
                {
                    drx[0]["Count"] = (int) drx[0]["Count"] + 1;
                }
            }

            return dt;
        }

        /// <summary>
        /// Devuleve los registros no repetidos de un conjunto de registros para varias columnas determinadas
        /// </summary>
        /// <param name="nRows">Conjunto de registros</param>
        /// <param name="nColumnNames">Nombre de las columnas</param>
        /// <returns>Registros unicos</returns>
        public static DataTable GetDistinctRows(DataRow[] nRows, string[] nColumnNames)
        {
            var dt = new DataTable();
            foreach (string columnName in nColumnNames)
                dt.Columns.Add(columnName);

            foreach (var row in nRows)
            {
                string distictFilter = "";
                var rowValues = new ArrayList();

                foreach (string columnName in nColumnNames)
                {
                    if (distictFilter != "") distictFilter += " AND ";
                    distictFilter += columnName + " = '" + row[columnName] + "'";
                    rowValues.Add(row[columnName].ToString());
                }

                DataRow[] drx = dt.Select(distictFilter);

                if (drx.Length == 0)
                    dt.Rows.Add(rowValues.ToArray());
            }

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nKeys">Campos que componen la llave primaria del registro</param>
        /// <returns></returns>
        public static int GetGroupCount(List<Parameter> nKeys)
        {
            int groupCount = 1;

            for (int i = 0; i < nKeys.Count; i++)
            {
                if (nKeys[i].FilterGroup > groupCount)
                    groupCount = nKeys[i].FilterGroup;
            }

            return groupCount;
        }

        #endregion
    }
}