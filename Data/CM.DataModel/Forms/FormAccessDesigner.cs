using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CM.DataModel.Schemas;
using CM.Tools.Misellaneous;
using CMData.Schemas;
using Data.Mapping.Writer;
using Data.Mapping.Mapper;
using Tools;
using System.IO;
using Tools.Progress;

namespace CM.DataModel.Forms
{
    public partial class FormAccessDesigner : Form
    {
        #region Declaraciones

        private bool IgnoreEvents;
        private bool IgnoreSetConfig;
        private FormProgress Progreso;

        #endregion

        #region Constructores

        public FormAccessDesigner()
        {
            InitializeComponent();
        }

        #endregion

        #region Propiedades

        public string FileName { get; set; }

        private XsdDataBase.TBL_ConnectionRow SelectedConnection
        {
            get
            {
                if (ConnectionsListBox.SelectedIndex != -1)
                    return (XsdDataBase.TBL_ConnectionRow)((DataRowView)(ConnectionsListBox.SelectedItem)).Row;

                return null;
            }
        }

        protected override bool ShowKeyboardCues
        {
            get { return base.ShowKeyboardCues; }
        }

        public Main MDIForm
        {
            get { return (Main)this.MdiParent; }
            set { this.MdiParent = value; }
        }

        public bool ConexionesChecked
        {
            get { return !ContainerMainSplitContainer.Panel1Collapsed; }
            set { ContainerMainSplitContainer.Panel1Collapsed = !value; }
        }

        #endregion

        #region Eventos

        private void FormAccessDesigner_Load(object sender, EventArgs e)
        {
            this.ConnectionInfoPanel.Enabled = false;
            this.ConnectionStringTextBox.Enabled = false;

            Application.DoEvents();

            LenguajeComboBox.SelectedIndex = 0;

            GetConfig();
        }

        private void FormAccessDesigner_Enter(object sender, EventArgs e)
        {
            this.MDIForm.FormAccessDesignerActive = this;
            this.MDIForm.ActivateControls(true);
        }

        private void FormAccessDesigner_Leave(object sender, EventArgs e)
        {
            this.MDIForm.FormAccessDesignerActive = null;
            this.MDIForm.ActivateControls(false);
        }

        private void FormAccessDesigner_EnabledChanged(object sender, EventArgs e)
        {
            this.MDIForm.ActivateControls(this.Enabled);
        }

        private void ConexionesCloseLabel_MouseEnter(object sender, EventArgs e)
        {
            ConexionesCloseLabel.BackColor = SystemColors.ActiveCaption;
            ConexionesCloseLabel.BorderStyle = BorderStyle.FixedSingle;
        }

        private void ConexionesCloseLabel_MouseLeave(object sender, EventArgs e)
        {
            ConexionesCloseLabel.BackColor = SystemColors.ControlDark;
            ConexionesCloseLabel.BorderStyle = BorderStyle.None;
        }

        private void ConexionesCloseLabel_Click(object sender, EventArgs e)
        {
            this.ConexionesChecked = false;
        }

        private void AcceptConnectionButton_Click(object sender, EventArgs e)
        {
            IgnoreEvents = true;

            if (SelectedConnection == null)
            {
                var cnnRow = DataBaseDataSet.TBL_Connection.NewTBL_ConnectionRow();

                cnnRow.Connection_Name = ConnectionNameTextBox.Text;
                cnnRow.Connection_Type = ConnectionTypeComboBox.SelectedItem.ToString();
                cnnRow.Connection_String = ConnectionStringTextBox.Text;

                DataBaseDataSet.TBL_Connection.AddTBL_ConnectionRow(cnnRow);

                DataBaseDataSet.TBL_Generic_Type.AddTBL_Generic_TypeRow(cnnRow, "Table", "Tabla");
                DataBaseDataSet.TBL_Generic_Type.AddTBL_Generic_TypeRow(cnnRow, "View", "Vista");
                DataBaseDataSet.TBL_Generic_Type.AddTBL_Generic_TypeRow(cnnRow, "StoredProcedure", "Procedimiento almacenado");

                ConnectionTypeComboBox.SelectedIndex = -1;
            }
            else
            {
                SelectedConnection.Connection_Name = ConnectionNameTextBox.Text;
                SelectedConnection.Connection_Type = ConnectionTypeComboBox.SelectedItem.ToString();
                SelectedConnection.Connection_String = ConnectionStringTextBox.Text;
            }

            this.ConnectionInfoPanel.Enabled = false;
            this.ConnectionStringTextBox.Enabled = false;

            IgnoreEvents = false;
        }

        private void DeleteConnectionButton_Click(object sender, EventArgs e)
        {
            if (SelectedConnection != null)
            {
                if (MessageBox.Show(@"Esta acción eliminará permanentemente toda la configuracion realizada en la conexión " + SelectedConnection.Connection_Name + ControlChars.CrLf + "¿Esta seguro de eliminar dicha conexión?", Program.AssemblyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    DataBaseDataSet.TBL_Connection.Rows.Remove(SelectedConnection);
            }
        }

        private void ConnectionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IgnoreEvents)
            {
                if (ConnectionsListBox.SelectedIndex != -1)
                {
                    ConnectionNameTextBox.Text = SelectedConnection.Connection_Name;
                    ConnectionStringTextBox.Text = SelectedConnection.Connection_String;
                    ConnectionTypeComboBox.SelectedItem = SelectedConnection.Connection_Type;
                }
            }

            ShowTypes();
        }

        public void ProgressProcessChanged(string nText, int nMaxValue, int nValue)
        {
            Progreso.Process = nText;
            Progreso.MaxValueProcess = nMaxValue;
            Progreso.ValueProcess = nValue;
        }

        public void ProgressActionChanged(string nText, int nMaxValue, int nValue)
        {
            Progreso.Action = nText;
            Progreso.MaxValueAction = nMaxValue;
            Progreso.ValueAction = nValue;
        }

        public void ProgressIncrementProcess(ref bool nCancel)
        {
            nCancel = Progreso.Cancel;
            Progreso.IncrementProcess();
        }

        public void ProgressIncrementAction(ref bool nCancel)
        {
            nCancel = Progreso.Cancel;
            Progreso.IncrementAction();
        }

        private void EditConnectionButton_Click(object sender, EventArgs e)
        {
            ConnectionInfoPanel.Enabled = true;
            ConnectionStringTextBox.Enabled = true;
        }

        private void CancelConnectionButton_Click(object sender, EventArgs e)
        {
            ConnectionInfoPanel.Enabled = false;
            ConnectionStringTextBox.Enabled = false;
        }

        private void ConnectionInfoPanel_EnabledChanged(object sender, EventArgs e)
        {
            EditConnectionButton.Visible = !ConnectionInfoPanel.Enabled;
            CancelConnectionButton.Visible = ConnectionInfoPanel.Enabled;
            ButtonDeleteConnection.Visible = !ConnectionInfoPanel.Enabled;
        }

        private void ObjectsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (SelectedConnection != null)
                {
                    var obj = (XsdDataBase.TBL_ObjectRow)((DataRowView)(ObjectsDataGridView.Rows[e.RowIndex].DataBoundItem)).Row;
                    if (obj.IsMappedNull() || !obj.Mapped)
                        RefreshObject(obj);

                    ShowObjectConfig(obj);
                }
            }
            if (e.ColumnIndex == 4)
            {
                if (SelectedConnection != null)
                {
                    var obj = (XsdDataBase.TBL_ObjectRow)((DataRowView)(ObjectsDataGridView.Rows[e.RowIndex].DataBoundItem)).Row;
                    if (obj.IsMappedNull() || !obj.Mapped)
                        RefreshObject(obj);

                    ShowObjectTest(obj);
                }
            }
        }

        private void ObjectsDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hti = ((DataGridView)sender).HitTest(e.X, e.Y);

                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    if (!ObjectsDataGridView.Rows[hti.RowIndex].Selected)
                    {
                        ObjectsDataGridView.ClearSelection();
                        ObjectsDataGridView.Rows[hti.RowIndex].Selected = true;

                        ObjectsDataGridView.Rows[hti.RowIndex].ContextMenuStrip = ObjectContextMenuStrip;
                        ObjectContextMenuStrip.Show();
                        Application.DoEvents();
                    }
                }
            }
        }

        private void ObjectsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            ShowFields();
        }

        private void BrowseDestinyPathButton_Click(object sender, EventArgs e)
        {
            var dlgBrowseFoler = new FolderBrowserDialog();

            if (FolderDestinyTextBox.Text == "")
                dlgBrowseFoler.SelectedPath = FolderDestinyTextBox.Text;

            if (dlgBrowseFoler.ShowDialog() == DialogResult.OK)
                FolderDestinyTextBox.Text = dlgBrowseFoler.SelectedPath;
        }

        private void LoadObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedConnection != null)
            {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (ObjectsDataGridView.SelectedRows != null && ObjectsDataGridView.SelectedRows.Count > 0)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                {
                    var newObject = (XsdDataBase.TBL_ObjectRow)((DataRowView)ObjectsDataGridView.SelectedRows[0].DataBoundItem).Row;
                    RefreshObject(newObject);
                }
            }
        }

        private void SetupObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedConnection != null)
            {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (ObjectsDataGridView.SelectedRows != null && ObjectsDataGridView.SelectedRows.Count > 0)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                {
                    var obj = (XsdDataBase.TBL_ObjectRow)((DataRowView)ObjectsDataGridView.SelectedRows[0].DataBoundItem).Row;

                    if (obj.IsMappedNull() || !obj.Mapped)
                        RefreshObject(obj);

                    ShowObjectConfig(obj);
                }
            }
        }

        private void RunObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedConnection != null)
            {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (ObjectsDataGridView.SelectedRows != null && ObjectsDataGridView.SelectedRows.Count > 0)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                {
                    var obj = (XsdDataBase.TBL_ObjectRow)((DataRowView)ObjectsDataGridView.SelectedRows[0].DataBoundItem).Row;

                    if (obj.IsMappedNull() || !obj.Mapped)
                        RefreshObject(obj);

                    ShowObjectTest(obj);
                }
            }
        }

        private void DbObjectTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (DbObjectTypeComboBox.SelectedIndex)
            {
                case 0:
                    FieldsForeignsSplitContainer.Panel2Collapsed = false;
                    break;

                case 1:
                    FieldsForeignsSplitContainer.Panel2Collapsed = true;
                    break;

                case 2:
                    FieldsForeignsSplitContainer.Panel2Collapsed = true;
                    break;
            }

            ShowObjects();
        }

        private void FilterObjectButton_Click(object sender, EventArgs e)
        {
            ShowObjects();
        }
        
        private void RemoveFilterObjectButton_Click(object sender, EventArgs e)
        {
            FilterObjectTextBox.Text = "";
            ShowObjects();
        }

        private void FilterObjectTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterObjectButton.Enabled = (FilterObjectTextBox.Text != "");
            RemoveFilterObjectButton.Enabled = FilterObjectButton.Enabled;
        }
        
        private void FilterObjectTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ShowObjects();
        }

        private void FilterSchemaButton_Click(object sender, EventArgs e)
        {
            ShowObjects();
        }
       
        private void RemoveFilterSchemaButton_Click(object sender, EventArgs e)
        {
            FilterSchemaTextBox.Text = "";
            ShowObjects();
        }

        private void FilterSchemaTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterSchemaButton.Enabled = (FilterSchemaTextBox.Text != "");
            RemoveFilterSchemaButton.Enabled = FilterSchemaButton.Enabled;
        }

        private void FilterSchemaTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ShowObjects();
        }

        private void CamposDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            ShowRelations();
        }

        #endregion

        #region Metodos

        public void LoadConfiguration(string nFileName)
        {
            IgnoreEvents = true;
            FileName = nFileName;

            this.Text = System.IO.Path.GetFileName(nFileName);

            DataBaseDataSet.TBL_Connection.Rows.Clear();
            DataBaseDataSet.ReadXml(FileName, XmlReadMode.Auto);

            if (ConnectionsListBox.Items.Count > 0)
            {
                ConnectionsListBox.SelectedIndex = 0;
                IgnoreEvents = false;
                ConnectionsListBox_SelectedIndexChanged(this, null);
                IgnoreEvents = true;
            }
            else
            {
                ConnectionsListBox.SelectedIndex = -1;
            }

            GetConfig();

            IgnoreEvents = false;
        }

        public void SaveConfiguration()
        {
            if (FileName == "")
                throw new Exception("No se ha seleccionado el nombre del archivo");

            SaveConfiguration(FileName);
        }

        public void SaveConfiguration(string nFileName)
        {
            SetConfig(false, FolderDestinyTextBox.Text, NamespaceMapTextBox.Text, LenguajeComboBox.SelectedIndex, this.ConexionesChecked, IsMobileCheckBox.Checked, UseFramework2CheckBox.Checked);

            FileName = nFileName;
            DataBaseDataSet.WriteXml(FileName);

            this.Text = System.IO.Path.GetFileName(nFileName);
        }

        private void RefreshObjects(bool AddNewsOnly)
        {
            try
            {
                var map = new DataBaseMapper(DataBaseDataSet, SelectedConnection);

                try
                {
                    Progreso = new FormProgress();
                    Progreso.Show();

                    map.ProgressProcessChanged += ProgressProcessChanged;
                    map.ProgressActionChanged += ProgressActionChanged;
                    map.ProgressIncrementProcess += ProgressIncrementProcess;
                    map.ProgressIncrementAction += ProgressIncrementAction;

                    map.LoadAndRefreshAllObjects(true, AddNewsOnly);

                    Progreso.Close();
                    Application.DoEvents();

                    if (map.Log.ToString() != "")
                    {
                        var log = new FormLog();

                        log.AppendText(map.Log.ToString());
                        log.ShowDialog();
                    }
                }
                catch
                {
                    Progreso.Close();
                    Application.DoEvents();
                    throw;
                }
                finally
                {
                    map.ProgressProcessChanged -= ProgressProcessChanged;
                    map.ProgressActionChanged -= ProgressActionChanged;
                    map.ProgressIncrementProcess -= ProgressIncrementProcess;
                    map.ProgressIncrementAction -= ProgressIncrementAction;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Show();
        }

        private void SetConfig(bool MultipleFile, string FolderDestiny, string NamespaceMap, int Languaje, bool ShowConnections, bool IsMobile, bool UseFramework2)
        {
            if (!IgnoreSetConfig)
            {
                if (DataBaseDataSet.TBL_Config.Rows.Count == 0)
                {
                    DataBaseDataSet.TBL_Config.AddTBL_ConfigRow(true, FolderDestiny, NamespaceMap, Languaje, ShowConnections, IsMobileCheckBox.Checked, UseFramework2CheckBox.Checked);
                }
                else
                {
                    var Fila = ((XsdDefault.TBL_ConfigRow)DataBaseDataSet.TBL_Config.Rows[0]);

                    Fila.Multiple_File = MultipleFile;
                    Fila.Destiny_Path = FolderDestiny;
                    Fila.NamespaceMap = NamespaceMap;
                    Fila.Languaje = Languaje;
                    Fila.ShowConnections = ShowConnections;
                    Fila.IsMobile = IsMobile;
                    Fila.UseFramework2 = UseFramework2;
                }
            }
        }

        private void GetConfig()
        {
            IgnoreSetConfig = true;

            if (DataBaseDataSet.TBL_Config.Rows.Count > 0)
            {
                var Fila = ((XsdDefault.TBL_ConfigRow)DataBaseDataSet.TBL_Config.Rows[0]);

                FolderDestinyTextBox.Text = Fila.Destiny_Path;
                NamespaceMapTextBox.Text = Fila.NamespaceMap;
                LenguajeComboBox.SelectedIndex = Fila.Languaje;
                this.ConexionesChecked = Fila.ShowConnections;
                IsMobileCheckBox.Checked = Fila.IsMobile;
                UseFramework2CheckBox.Checked = Fila.UseFramework2;
            }

            IgnoreSetConfig = false;
        }

        private void ShowObjectConfig(XsdDataBase.TBL_ObjectRow obj)
        {
            if (obj.Generic_Type == "StoredProcedure")
            {
                var dlg = new FormConfigStoredProcedure();

                dlg.SetDataSource(DataBaseDataSet, SelectedConnection, obj);
                dlg.ShowDialog();
            }
            else
            {
                var dlg = new FormConfigTableFilter();

                dlg.SetDataSource(DataBaseDataSet, obj.id_Object);
                dlg.ShowDialog();
            }
        }

        private void ShowObjectTest(XsdDataBase.TBL_ObjectRow obj)
        {
            var dlg = new FormTestObject();
            dlg.SetDataSource(DataBaseDataSet, SelectedConnection, obj);
            dlg.ShowDialog();
        }

        private void RefreshObject(XsdDataBase.TBL_ObjectRow originalObject)
        {
            try
            {
                var map = new DataBaseMapper(DataBaseDataSet, SelectedConnection);
                var log = new StringBuilder();

                if (originalObject.Generic_Type == "StoredProcedure")
                    map.RefreshStoredProcedure(ref originalObject);
                else
                    map.RefreshObjectTable(ref originalObject); // No aplica para modo por demanda

                originalObject.Mapped = true;

                if (log.ToString() != "")
                {
                    var dlg = new FormLog();

                    dlg.AppendText(log.ToString());
                    dlg.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                if (originalObject != null)
                    originalObject.Mapped = false;

                MessageBox.Show(ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddNewConnection()
        {
            ConnectionNameTextBox.Text = "";
            ConnectionStringTextBox.Text = "";
            ConnectionTypeComboBox.SelectedIndex = -1;

            ConnectionsListBox.SelectedIndex = -1;
            this.ConnectionInfoPanel.Enabled = true;
            this.ConnectionStringTextBox.Enabled = true;
        }

        public void RefreshDbObjets(bool AddNewsOnly)
        {
            MainPanel.Enabled = false;
            Application.DoEvents();
            RefreshObjects(AddNewsOnly);
            MainPanel.Enabled = true;
        }

        public void CatalogConfig()
        {
            if (ConnectionsListBox.SelectedItem != null)
            {
                var ConnectionRow = (CMData.Schemas.XsdDataBase.TBL_ConnectionRow)((DataRowView)ConnectionsListBox.SelectedItem).Row;

                RefreshCatalog(ConnectionRow.id_Connection);

                var ConfigForm = new FormCatalogConfig(DataBaseDataSet);

                ConfigForm.ShowDialog();
            }
            else
            {
                MessageBox.Show(@"Debe seleccionar una conexión", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void SchemaConfig()
        {
            if (ConnectionsListBox.SelectedItem != null)
            {
                var ConnectionRow = (CMData.Schemas.XsdDataBase.TBL_ConnectionRow)((DataRowView)ConnectionsListBox.SelectedItem).Row;

                RefreshSchema(ConnectionRow.id_Connection);

                var ConfigForm = new FormSchemaConfig(DataBaseDataSet);

                ConfigForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una conexión", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshCatalog(int nidConnection)
        {
            var NewCatalogosDataTable = CMData.DataBase.DataBase.GetDistinctRows(DataBaseDataSet.TBL_Object.Select("fk_Connection = " + nidConnection), "Catalog_Name");

            //Eliminar registros sobrantes
            foreach (XsdDataBase.TBL_CatalogRow Item in DataBaseDataSet.TBL_Catalog.Select())
            {
                if (!ExistRow(NewCatalogosDataTable, "Catalog_Name", Item.Catalog_Name))
                    Item.Delete();
            }

            // Insertar nuevos registros
            foreach (DataRow Item in NewCatalogosDataTable.Rows)
            {
                if (DataBaseDataSet.TBL_Catalog.Select("Catalog_Name = '" + Item["Catalog_Name"] + "'").Length == 0)
                {
                    string Identificador = FormatCode.ToIdentifier(Item["Catalog_Name"].ToString());
                    DataBaseDataSet.TBL_Catalog.AddTBL_CatalogRow(nidConnection, Item["Catalog_Name"].ToString(), Identificador, Identificador);
                }
            }

            DataBaseDataSet.TBL_Catalog.AcceptChanges();
        }

        private void RefreshSchema(int nidConnection)
        {
            var NewSchemasDataTable = CMData.DataBase.DataBase.GetDistinctRows(DataBaseDataSet.TBL_Object.Select("fk_Connection = " + nidConnection), "Schema_Name");

            //Eliminar registros sobrantes
            foreach (XsdDataBase.TBL_SchemaRow Item in DataBaseDataSet.TBL_Schema.Select())
            {
                if (!ExistRow(NewSchemasDataTable, "Schema_Name", Item.Schema_Name))
                    Item.Delete();
            }

            // Insertar nuevos registros
            foreach (DataRow Item in NewSchemasDataTable.Rows)
            {
                if (DataBaseDataSet.TBL_Schema.Select("Schema_Name = '" + Item["Schema_Name"] + "'").Length == 0)
                {
                    string Identificador = FormatCode.ToIdentifier(Item["Schema_Name"].ToString());
                    DataBaseDataSet.TBL_Schema.AddTBL_SchemaRow(nidConnection, Item["Schema_Name"].ToString(), Identificador, false);
                }
            }

            DataBaseDataSet.TBL_Schema.AcceptChanges();
        }

        public void GenerateAccessClass()
        {
            MainPanel.Enabled = false;

            SetConfig(false, FolderDestinyTextBox.Text, NamespaceMapTextBox.Text, LenguajeComboBox.SelectedIndex, this.ConexionesChecked, this.IsMobileCheckBox.Checked, this.UseFramework2CheckBox.Checked);

            var unMappedSelectedObjects = (XsdDataBase.TBL_ObjectRow[])(DataBaseDataSet.TBL_Object.Select("Selected = 1 AND Mapped = 0"));

            foreach (var unMappedSelectedObject in unMappedSelectedObjects)
            {
                RefreshObject(unMappedSelectedObject);
            }

            try
            {
                if (this.FileName == "")
                {
                    MessageBox.Show(@"Para poder generar la clase de acceso a datos primero debe almacenar el archivo de configuración", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var FolderDestiny = FolderDestinyTextBox.Text;

                    if (FolderDestiny == "")
                    {
                        var directoryName = Path.GetDirectoryName(this.FileName);
                        if (directoryName == null)
                            throw new ArgumentOutOfRangeException(this.FileName);

                        FolderDestiny = directoryName.TrimEnd('\\') + '\\';
                    }

                    ((Main)MdiParent).AnimateProgressBar();

                    var codeCreator = new DataBaseClassWriter(DataBaseDataSet);
                    var ConnectionRow = (CMData.Schemas.XsdDataBase.TBL_ConnectionRow)((DataRowView)ConnectionsListBox.SelectedItem).Row;

                    RefreshCatalog(ConnectionRow.id_Connection);
                    RefreshSchema(ConnectionRow.id_Connection);

                    codeCreator.GenerateClassCode(ConnectionRow, FolderDestiny, NamespaceMapTextBox.Text, LenguajeComboBox.SelectedIndex == 0 ? LanguajeType.VB : LanguajeType.CSharp, IsMobileCheckBox.Checked, UseFramework2CheckBox.Checked);
                    codeCreator.GenerateSchemaXML(ConnectionRow,DataBaseDataSet, FolderDestiny);

                    if (codeCreator.Log.ToString() != "")
                        throw new Exception(codeCreator.Log.ToString());

                    MessageBox.Show(@"Las clases de acceso a datos se generaron exitosamente", Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error al generar las clases de conexión, " + ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainPanel.Enabled = true;
            }
        }

        private void ShowTypes()
        {
            if (ConnectionsListBox.SelectedItem != null)
            {
                var ConnectionRow = (CMData.Schemas.XsdDataBase.TBL_ConnectionRow)((DataRowView)ConnectionsListBox.SelectedItem).Row;

                TiposBindingSource.Filter = "fk_Connection = " + ConnectionRow.id_Connection;
            }
            else
            {
                ObjetosBindingSource.Filter = "fk_Connection = -1";
            }

            ShowObjects();
        }

        private void ShowObjects()
        {
            if (DbObjectTypeComboBox.SelectedItem != null)
            {
                var TipoRow = (CMData.Schemas.XsdDataBase.TBL_Generic_TypeRow)((DataRowView)DbObjectTypeComboBox.SelectedItem).Row;

                ObjetosBindingSource.Filter = "fk_Connection = " + TipoRow.fk_Connection + " AND Generic_Type = '" + TipoRow.Generic_Type + "' AND Object_Name LIKE '%" + FilterObjectTextBox.Text + "%' AND Schema_Name LIKE '%" + FilterSchemaTextBox.Text + "%'";
            }
            else
            {
                ObjetosBindingSource.Filter = "id_Object = -1";
            }

            ShowFields();
        }

        private void ShowFields()
        {
            if (ObjectsDataGridView.CurrentRow != null)
            {
                var ObjectRow = (CMData.Schemas.XsdDataBase.TBL_ObjectRow)((DataRowView)ObjectsDataGridView.CurrentRow.DataBoundItem).Row;

                CamposBindingSource.Filter = "fk_Object = " + ObjectRow.id_Object;
            }
            else
            {
                CamposBindingSource.Filter = "fk_Object = -1";
            }

            ShowRelations();
        }

        private void ShowRelations()
        {
            if (CamposDataGridView.CurrentRow != null)
            {
                try
                {
                    var FieldRow = (CMData.Schemas.XsdDataBase.TBL_FieldRow)((DataRowView)CamposDataGridView.CurrentRow.DataBoundItem).Row;

                    RelacionesBindingSource.Filter = "fk_Field = " + FieldRow.id_Field;
                }
                catch
                { }
            }
            else
            {
                RelacionesBindingSource.Filter = "fk_Field = -1";
            }
        }

        #endregion

        #region Funciones

        private bool ExistRow(DataTable nDataTable, string nColumName, string nValue)
        {
            return nDataTable.Select(nColumName + "= '" + nValue + "'").Length > 0;
        }

        #endregion
    }
}
