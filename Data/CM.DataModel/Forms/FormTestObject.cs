using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CM.DataModel.Schemas;

namespace CM.DataModel.Forms
{
    public partial class FormTestObject : Form
    {
        #region Declaraciones

        private XsdDataBaseDesign _dtsDataBase;
        private CMData.Schemas.XsdDataBase.TBL_ConnectionRow _selectedConnection;
        private CMData.Schemas.XsdDataBase.TBL_ObjectRow _selectedObject;
        private List<CMData.Schemas.Parameter> _Fields;

        #endregion

        #region Propiedades

        public XsdDataBaseDesign DtsDataBase
        {
            get
            {
                return _dtsDataBase;
            }
        }

        public CMData.Schemas.XsdDataBase.TBL_ConnectionRow SelectedConnection
        {
            get
            {
                return _selectedConnection;
            }
            set
            {
                _selectedConnection = value;
            }
        }

        public CMData.Schemas.XsdDataBase.TBL_ObjectRow SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                _selectedObject = value;
            }
        }

        #endregion

        #region Eventos

        private void FormTestObject_Load(object sender, EventArgs e)
        {
            if (_selectedConnection == null)
            {
                MessageBox.Show("Seleccione primero una conexion", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
                return;
            }

            if (DtsDataBase == null)
            {
                MessageBox.Show("No se ha seleccionado la fuente de datos, primero utilice SetDataSource", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
                return;
            }

            if (_selectedObject == null)
            {
                MessageBox.Show("No se ha seleccionado un objeto válido", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
                return;
            }

            ConnectionTextBox.Text = _selectedConnection.Connection_Name;
            CatalogNameTextBox.Text = _selectedObject.Catalog_Name;
            ObjectTypeTextBox.Text = _selectedObject.Generic_Type;
            SchemaNameTextBox.Text = _selectedObject.Schema_Name;
            ObjectNameTextBox.Text = _selectedObject.Object_Name;

            DefaultDataSet.TBL_Parameter.Rows.Clear();
            _Fields = DtsDataBase.GetParameters(_selectedObject);

            foreach (var field in _Fields)
            {
                DefaultDataSet.TBL_Parameter.AddTBL_ParameterRow(field.Name, "", true);
            }
        }

        private void EjecutarButton_Click(object sender, EventArgs e)
        {
            TestObject();
        }

        private void CerrarButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Constructores

        public FormTestObject()
        {
            InitializeComponent();
        }

        #endregion

        #region Metodos

        public void SetDataSource(XsdDataBaseDesign nDataBase, CMData.Schemas.XsdDataBase.TBL_ConnectionRow nSelectedConnection, CMData.Schemas.XsdDataBase.TBL_ObjectRow nSelectedObject)
        {
            _dtsDataBase = nDataBase;
            _selectedConnection = nSelectedConnection;
            _selectedObject = nSelectedObject;
        }

        private void TestObject()
        {
            var dbType = ((CMData.DataBase.DataBaseType)Enum.Parse(typeof(CMData.DataBase.DataBaseType), _selectedConnection.Connection_Type));
            var db = CMData.DataBase.DataBaseFactory.CreateDatabase(dbType, SelectedConnection.Connection_String);

            try
            {
                db.Connection_Open();

                foreach (var field in _Fields)
                {
                    var rows = (XsdDefault.TBL_ParameterRow[])(DefaultDataSet.TBL_Parameter.Select("Name = '" + field.Name + "'"));

                    field.Value = !rows[0].IsNullM ? rows[0].Value : null;
                }

                var tableData = new DataTable();

                bool Result;
                Exception Exception;
                if (_selectedObject.Generic_Type == "Table" || _selectedObject.Generic_Type == "View")
                {
                    db.DBFill(ref tableData, _selectedObject.Schema_Name, _selectedObject.Object_Name, _Fields, 100, null, out Result, out Exception);
                    if (!Result) throw Exception;
                }
                else
                {
                    db.DBExecuteStoredProcedureFill(ref tableData, _selectedObject.Schema_Name, _selectedObject.Object_Name, _Fields, out Result, out Exception);
                    if (!Result) throw Exception;
                }

                ResultDataGridView.DataSource = tableData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.Connection_Close();
            }
        }

        #endregion
    }
}
