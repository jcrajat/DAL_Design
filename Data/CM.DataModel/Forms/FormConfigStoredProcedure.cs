using System;
using System.Data;
using System.Windows.Forms;
using CM.DataModel.Schemas;
using CM.Tools.Misellaneous;
using Tools;


namespace CM.DataModel.Forms
{
    public partial class FormConfigStoredProcedure : Form
    {
        #region Declaraciones

        private XsdDataBaseDesign _dtsDataBase;
        private CMData.Schemas.XsdDataBase.TBL_ConnectionRow _selectedConnection;
        private CMData.Schemas.XsdDataBase.TBL_ObjectRow _selectedObject;
        private CMData.Schemas.XsdDataBase.TBL_SP_ReturnRow _returnRow = null;
        private bool IgnoreSelectedIndexChanged = false;

        #endregion

        #region Propiedades

        public XsdDataBaseDesign DtsDataBase
        {
            get { return _dtsDataBase; }
        }

        public CMData.Schemas.XsdDataBase.TBL_ConnectionRow SelectedConnection
        {
            get { return _selectedConnection; }
            set { _selectedConnection = value; }
        }

        public CMData.Schemas.XsdDataBase.TBL_ObjectRow SelectedObject
        {
            get { return _selectedObject; }
            set { _selectedObject = value; }
        }

        #endregion

        #region Eventos

        private void FormConfigStoredProcedure_Load(object sender, EventArgs e)
        {
            if (_dtsDataBase == null)
            {
                MessageBox.Show("No es posible configurar los filtros del objeto, utilice la función SetDataSource ", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
                Close();
                return;
            }

            if (_selectedObject == null)
            {
                MessageBox.Show("Seleccione primero el procedimiento almacenado ", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
                Close();
                return;
            }

            IgnoreSelectedIndexChanged = true;

            // ReSharper disable CoVariantArrayConversion
            lstReturnType.Items.AddRange(Enum.GetNames(typeof(CMData.Schemas.ReturnType)));
            // ReSharper restore CoVariantArrayConversion

            var spRows = (CMData.Schemas.XsdDataBase.TBL_SP_ReturnRow[])(DtsDataBase.TBL_SP_Return.Select("fk_Object = " + _selectedObject.id_Object));

            if (spRows.Length > 0)
            {
                _returnRow = spRows[0];

                lstReturnType.SelectedItem = _returnRow.Return_Type;

                if (_returnRow.IsObject_Name_ReturnedNull())
                {
                    FillReturnDataType(_returnRow.Data_Type_Returned, "", "");
                }
                else
                {
                    FillReturnDataType(_returnRow.Data_Type_Returned, _returnRow.Schema_Name_Returned, _returnRow.Object_Name_Returned);
                }
            }
            else
            {
                lstReturnType.SelectedItem = CMData.Schemas.ReturnType.TablaGenerica.ToString();
                FillReturnDataType();
            }

            IgnoreSelectedIndexChanged = false;
        }

        private void CerrarButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AceptarButton_Click(object sender, EventArgs e)
        {
            if (lstReturnType.SelectedIndex == -1)
            {
                MessageBox.Show("Se debe seleccionar el tipo retorno", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ReturnDataTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Se debe seleccionar el tipo de dato a retornar", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_returnRow == null)
            {
                _returnRow = DtsDataBase.TBL_SP_Return.NewTBL_SP_ReturnRow();
                _returnRow.fk_Object = _selectedObject.id_Object;
                DtsDataBase.TBL_SP_Return.AddTBL_SP_ReturnRow(_returnRow);
            }

            _returnRow.Return_Type = lstReturnType.SelectedItem.ToString();

            switch ((CMData.Schemas.ReturnType)(Enum.Parse(typeof(CMData.Schemas.ReturnType), lstReturnType.SelectedItem.ToString())))
            {
                case CMData.Schemas.ReturnType.TablaGenerica:
                    DtsDataBase.TBL_SP_Return.RemoveTBL_SP_ReturnRow(_returnRow);
                    break;

                case CMData.Schemas.ReturnType.Escalar:
                    _returnRow.SetSchema_Name_ReturnedNull();
                    _returnRow.SetObject_Name_ReturnedNull();
                    _returnRow.Data_Type_Returned = ReturnDataTypeComboBox.SelectedItem.ToString();
                    break;

                case CMData.Schemas.ReturnType.TablaTipada:
                    var idObj = (int)(((ComboBoxListItem)(ReturnDataTypeComboBox.SelectedItem)).Value);
                    var returns = (CMData.Schemas.XsdDataBase.TBL_ObjectRow[])(DtsDataBase.TBL_Object.Select("id_Object = " + idObj));

                    _returnRow.Schema_Name_Returned = returns[0].Schema_Name;
                    _returnRow.Object_Name_Returned = returns[0].Object_Name;
                    _returnRow.Data_Type_Returned = DbType.Object.ToString();
                    break;

                case CMData.Schemas.ReturnType.Nada:
                    _returnRow.SetSchema_Name_ReturnedNull();
                    _returnRow.SetObject_Name_ReturnedNull();
                    _returnRow.Data_Type_Returned = DbType.Object.ToString();
                    break;
            }


            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lstReturnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IgnoreSelectedIndexChanged)
            {
                FillReturnDataType();
            }
        }

        #endregion

        #region Constructores

        public FormConfigStoredProcedure()
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

        private void FillReturnDataType()
        {
            FillReturnDataType("", "", "");
        }

        private void FillReturnDataType(string nSelect_Data_Type_Returned, string nSchema_Name_Returned, string nObject_Name_Returned)
        {
            ReturnDataTypeComboBox.Items.Clear();

            if (lstReturnType.SelectedIndex != -1)
            {
                switch ((CMData.Schemas.ReturnType)(Enum.Parse(typeof(CMData.Schemas.ReturnType), lstReturnType.SelectedItem.ToString())))
                {
                    case CMData.Schemas.ReturnType.TablaGenerica:
                        ReturnDataTypeComboBox.Items.Add("DataTable");
                        ReturnDataTypeComboBox.SelectedIndex = 0;
                        break;

                    case CMData.Schemas.ReturnType.Escalar:
                        // ReSharper disable CoVariantArrayConversion
                        ReturnDataTypeComboBox.Items.AddRange(Enum.GetNames(typeof(DbType)));
                        // ReSharper restore CoVariantArrayConversion

                        if (nSelect_Data_Type_Returned != "")
                        {
                            ReturnDataTypeComboBox.SelectedItem = nSelect_Data_Type_Returned;
                        }
                        else
                        {
                            ReturnDataTypeComboBox.SelectedIndex = 0;
                        }

                        break;

                    case CMData.Schemas.ReturnType.TablaTipada:
                        var objects = (CMData.Schemas.XsdDataBase.TBL_ObjectRow[])(DtsDataBase.TBL_Object.Select("Generic_Type IN ('Table','View')", "Schema_Name,Object_Name"));

                        foreach (var obj in objects)
                        {
                            var item = new ComboBoxListItem(obj.Schema_Name + "." + obj.Object_Name, obj.id_Object);

                            ReturnDataTypeComboBox.Items.Add(item);

                            if (obj.Schema_Name == nSchema_Name_Returned && obj.Object_Name == nObject_Name_Returned)
                            {
                                ReturnDataTypeComboBox.SelectedItem = item;
                            }
                        }

                        if (nObject_Name_Returned == "")
                        {
                            ReturnDataTypeComboBox.SelectedIndex = 0;
                        }

                        break;

                    case CMData.Schemas.ReturnType.Nada:
                        ReturnDataTypeComboBox.Items.Add("Vacio");
                        ReturnDataTypeComboBox.SelectedIndex = 0;
                        break;
                }

            }
        }

        #endregion
    }
}
