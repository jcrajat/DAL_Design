using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CMData.DataBase;
using CMData.Utils;

namespace CMData.Manager
{
    public class DBManager : MarshalByRefObject
    {
        #region Declaraciones

        private DataBase.DataBase _DataBase;
        private List<SchemaManager> _Schemas = new List<SchemaManager>();
        private bool _IsRemoting = false;
        private bool _IsTrusted = true;

        public event System.EventHandler OnConnectionOpening;
        public event System.EventHandler OnConnectionOpened;
        public event System.EventHandler OnConnectionClosing;
        public event System.EventHandler OnConnectionClosed;

        #endregion

        #region Constructores

        public DBManager(string nConnectionString)
        {
            InitializeConnection(nConnectionString);
            InitializeSchemaMaping();
            DataBase.IsInheritDataBase = false;
        }

        public DBManager(DataBase.DataBaseType nType, string nConnectionString)
        {
            InitializeConnection(nType, nConnectionString);
            InitializeSchemaMaping();
            DataBase.IsInheritDataBase = false;
        }

        public DBManager(string nConnectionString, DBManager nInheritDbManager)
        {
            InitializeConnection(nConnectionString);
            InitializeSchemaMaping();
            LinkDataBaseManager(nInheritDbManager);
        }

        public DBManager(DataBase.DataBaseType nType, string nConnectionString, DBManager nInheritDbManager)
        {
            InitializeConnection(nType, nConnectionString);
            InitializeSchemaMaping();
            LinkDataBaseManager(nInheritDbManager);
        }

        public void LinkDataBaseManager(DBManager nInheritDbManager)
        {
            DataBase.IsInheritDataBase = true;
            DataBase.InheritDbManager = nInheritDbManager;
            DataBase.CurrentDataBaseCatalogName = DataBase.GetConnectionStringCatalogName(this.DataBase.ConnectionString);
            DataBase.Connection = nInheritDbManager.DataBase.Connection;
            DataBase.Transaction = nInheritDbManager._DataBase.Transaction;
        }

        ~DBManager()
        {
            try { this._DataBase.Disconect(); }
            catch { }

            this._DataBase = null;
            this._Schemas = null;
        }
        
        #endregion

        #region Propiedades

        public DataBase.DataBase DataBase
        {
            get { return _DataBase; }
        }

        public List<SchemaManager> Schemas
        {
            get { return _Schemas; }
        }

        public bool IsRemoting
        {
            get { return this._IsRemoting; }
        }

        public bool IsTrusted
        {
            get { return this._IsTrusted; }
        }

        public SchemaMapingManager SchemaMaping { get; private set; }

        public virtual string ClassFileName
        {
            get { return this.GetType().Name; }
        }

        #endregion

        #region Metodos

        private void InitializeConnection(string nConnectionString)
        {
            var type = DataBaseFactory.GetDataBaseType(nConnectionString);
            InitializeConnection(type, nConnectionString);
        }

        private void InitializeConnection(DataBase.DataBaseType nType, string nConnectionString)
        {
            this._DataBase = DataBaseFactory.CreateDatabase(nType, nConnectionString);
            this._IsRemoting = (DataBaseFactory.GetRemotingUrl(nConnectionString) != "");
            this._IsTrusted = DataBaseFactory.GetRemotingTrusted(nConnectionString);
        }

        private void InitializeSchemaMaping()
        {
            string MapFileName = ClassFileName + ".map";

            if (File.Exists(MapFileName))
                this.SchemaMaping = SchemaMapingManager.Deserialize(MapFileName);
            else
                this.SchemaMaping = new SchemaMapingManager();
        }

        public void Connection_Open()
        {
            if (OnConnectionOpening != null) OnConnectionOpening(this, null);

            DataBase.Connection_Open();

            if (OnConnectionOpened != null) OnConnectionOpened(this, null);
        }

        public void Connection_Close()
        {
            if (OnConnectionClosing != null) OnConnectionClosing(this, null);

            DataBase.Connection_Close();

            if (OnConnectionClosed != null) OnConnectionClosed(this, null);
        }

        public void Transaction_Begin()
        {
            DataBase.Transaction_Begin();
        }

        public void Transaction_Begin(IsolationLevel nIsolationLevel)
        {
            DataBase.Transaction_Begin(nIsolationLevel);
        }

        public void Transaction_Commit()
        {
            DataBase.Transaction_Commit();
        }

        public void Transaction_Rollback()
        {
            try
            {
                DataBase.Transaction_Rollback();
            }
            catch 
            {
                //Tragarse la excepcion para poder continuar con los procesos
            }
        }

        private void _DataBase_OnConnectionOpening(object sender, System.EventArgs e)
        {
            if (OnConnectionOpening != null) OnConnectionOpening(sender, e);
        }

        private void _DataBase_OnConnectionOpened(object sender, System.EventArgs e)
        {
            if (OnConnectionOpened != null) OnConnectionOpened(sender, e);
        }

        private void _DataBase_OnConnectionClosing(object sender, System.EventArgs e)
        {
            if (OnConnectionClosing != null) OnConnectionClosing(sender, e);
        }

        private void _DataBase_OnConnectionClosed(object sender, System.EventArgs e)
        {
            if (OnConnectionClosed != null) OnConnectionClosed(sender, e);
        }

        #endregion
    }
}