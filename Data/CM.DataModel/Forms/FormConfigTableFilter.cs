using System;
using System.Collections.Specialized;
using System.Data;
using System.Windows.Forms;
using CM.DataModel.Schemas;


namespace CM.DataModel.Forms
{
    public partial class FormConfigTableFilter : Form
    {
        #region Declaraciones

        private int _Id_Object = 0;
        private XsdDataBaseDesign _DtsDataBase = null;
        private int _FindNumber = 1;

        #endregion

        #region Propiedades

        public int Id_object
        {
            get { return _Id_Object; }
            set { _Id_Object = value; }
        }

        public XsdDataBaseDesign DtsDataBase
        {
            get { return _DtsDataBase; }
            set { _DtsDataBase = value; }
        }

        public XsdDefault DataSourceMap
        {
            get { return dtsMap; }
        }

        #endregion

        #region Eventos

        private void FormConfigObjectFilter_Load(object sender, EventArgs e)
        {
            if (_DtsDataBase == null)
            {
                MessageBox.Show("No es posible configurar los filtros del objeto, utilice la función set{DataSource ",
                                Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void AddNewFilterButton_Click(object sender, EventArgs e)
        {
            if (validateCurrentFilter())
            {
                var newMapFilter = dtsMap.TBL_Map_Filter.AddTBL_Map_FilterRow(0, _Id_Object, "Find_" + _FindNumber);
                _FindNumber += 1;

                var fieldRows =
                    (CMData.Schemas.XsdDataBase.TBL_FieldRow[])(_DtsDataBase.TBL_Field.Select("fk_Object = " + _Id_Object));

                // Agregar los campos incluyendo los previamente mapeados
                int order = 1;

                foreach (var f in fieldRows)
                {
                    // Agregar la columna para mapear
                    var newMapFilterColumn = dtsMap.TBL_Map_Filter_Column.NewTBL_Map_Filter_ColumnRow();

                    newMapFilterColumn.fk_Map_Filter = newMapFilter.id_Map_Filter;
                    newMapFilterColumn.fk_Object = f.fk_Object;
                    newMapFilterColumn.Field_Name = f.Field_Name;
                    newMapFilterColumn.Field_Type = f.Field_Type;
                    newMapFilterColumn.Specific_Type = f.Specific_Type;
                    newMapFilterColumn.Is_Nullable = f.Is_Nullable;
                    newMapFilterColumn.Max_Length = f.Max_Length;
                    newMapFilterColumn.Precision = f.Precision;
                    newMapFilterColumn.Scale = f.Scale;
                    newMapFilterColumn.PrimaryKey_Order = f.PrimaryKey_Order;
                    newMapFilterColumn.Filter_Order = order;
                    newMapFilterColumn.Selected = false;

                    order += 1;

                    dtsMap.TBL_Map_Filter_Column.AddTBL_Map_Filter_ColumnRow(newMapFilterColumn);
                }

                FiltersListBox.SelectedIndex = FiltersListBox.Items.Count - 1;
            }
        }

        private void AceptarButton_Click(object sender, EventArgs e)
        {
            var createdFilters = new StringCollection();

            // Guardar los cambios en el datasource de origen
            foreach (XsdDefault.TBL_Map_FilterRow mapFilter in dtsMap.TBL_Map_Filter.Rows)
            {
                var cols =
                    (XsdDefault.TBL_Map_Filter_ColumnRow[])
                    (dtsMap.TBL_Map_Filter_Column.Select(
                        "fk_Map_Filter = " + mapFilter.id_Map_Filter + " AND Selected = 1", "Filter_Order"));

                if (cols.Length > 0)
                {
                    string strFilterCols = "";

                    foreach (var col in cols)
                    {
                        strFilterCols += col.Field_Name;
                    }

                    if (createdFilters.Contains(strFilterCols))
                    {
                        MessageBox.Show("No se permite agregar dos filtros con la misma estructura",
                                        Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    createdFilters.Add(strFilterCols);
                }
            }

            var rowObject = DtsDataBase.TBL_Object.FindByid_Object(Id_object);
            var filterRows = rowObject.GetTBL_FilterRows();

            foreach (var flt in filterRows)
            {
                DtsDataBase.TBL_Filter.RemoveTBL_FilterRow(flt);
            }

            // Guardar los cambios en el datasource de origen
            foreach (XsdDefault.TBL_Map_FilterRow mapFilter in dtsMap.TBL_Map_Filter.Rows)
            {
                var newFilter = DtsDataBase.TBL_Filter.NewTBL_FilterRow();

                newFilter.fk_Object = Id_object;
                newFilter.Name = mapFilter.Name;

                var cols =
                    (XsdDefault.TBL_Map_Filter_ColumnRow[])
                    (dtsMap.TBL_Map_Filter_Column.Select(
                        "fk_Map_Filter = " + mapFilter.id_Map_Filter + " AND Selected = 1", "Filter_Order"));
                var order = 1;

                if (cols.Length > 0)
                {
                    DtsDataBase.TBL_Filter.AddTBL_FilterRow(newFilter);

                    foreach (var col in cols)
                    {
                        var newFilter_Field = DtsDataBase.TBL_Filter_Field.NewTBL_Filter_FieldRow();

                        newFilter_Field.fk_Filter = newFilter.id_Filter;
                        newFilter_Field.Field_Name = col.Field_Name;
                        newFilter_Field.Filter_Order = order;

                        DtsDataBase.TBL_Filter_Field.AddTBL_Filter_FieldRow(newFilter_Field);
                        order += 1;
                    }
                }
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CerrarButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void UpRowButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (FilterColumnsDataGridView.CurrentRow != null)
                {
                    var filterColumn =
                        (XsdDefault.TBL_Map_Filter_ColumnRow)
                        ((DataRowView)(FilterColumnsDataGridView.CurrentRow.DataBoundItem)).Row;
                    int curRow_id_Map_Filter_Column = filterColumn.id_Map_Filter_Column;
                    int curOrder = filterColumn.Filter_Order;

                    var pos = FindFilterColumnIndex(filterColumn);

                    if (pos > 0)
                    {
                        var cols =
                            (XsdDefault.TBL_Map_Filter_ColumnRow[])
                            (dtsMap.TBL_Map_Filter_Column.Select("fk_Map_Filter = " + filterColumn.fk_Map_Filter,
                                                                 "Filter_Order"));

                        var prevOrder = cols[pos - 1].Filter_Order;
                        var prevRow_id_Map_Filter_Column = cols[pos - 1].id_Map_Filter_Column;

                        try
                        {
                            dtsMap.TBL_Map_Filter_Column.Select("id_Map_Filter_Column = " + prevRow_id_Map_Filter_Column)
                                [0]["Filter_Order"] = curOrder;
                            dtsMap.TBL_Map_Filter_Column.Select("id_Map_Filter_Column = " + curRow_id_Map_Filter_Column)
                                [0]["Filter_Order"] = prevOrder;
                        }
                        catch
                        {
                        }

                        RenameFilter();
                    }
                }
            }
            catch
            {
            }
        }

        private void DownRowButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (FilterColumnsDataGridView.CurrentRow != null)
                {
                    var filterColumn =
                        (XsdDefault.TBL_Map_Filter_ColumnRow)
                        ((DataRowView)(FilterColumnsDataGridView.CurrentRow.DataBoundItem)).Row;
                    var cols =
                        (XsdDefault.TBL_Map_Filter_ColumnRow[])
                        (dtsMap.TBL_Map_Filter_Column.Select("fk_Map_Filter = " + filterColumn.fk_Map_Filter,
                                                             "Filter_Order"));

                    if (FilterColumnsDataGridView.CurrentRow.Index < cols.Length - 1)
                    {
                        int curRow_id_Map_Filter_Column = filterColumn.id_Map_Filter_Column;
                        int curOrder = filterColumn.Filter_Order;

                        var pos = FindFilterColumnIndex(filterColumn);

                        if (pos < cols.Length - 1)
                        {
                            var postOrder = cols[pos + 1].Filter_Order;
                            var postRow_id_Map_Filter_Column = cols[pos + 1].id_Map_Filter_Column;

                            try
                            {
                                dtsMap.TBL_Map_Filter_Column.Select("id_Map_Filter_Column = " +
                                                                    postRow_id_Map_Filter_Column)[0]["Filter_Order"] =
                                    curOrder;
                                dtsMap.TBL_Map_Filter_Column.Select("id_Map_Filter_Column = " +
                                                                    curRow_id_Map_Filter_Column)[0]["Filter_Order"] =
                                    postOrder;
                            }
                            catch
                            {
                            }

                            RenameFilter();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void DeleteFilterButton_Click(object sender, EventArgs e)
        {
            if (FiltersListBox.SelectedIndex != -1)
            {
                var currentFilter = (XsdDefault.TBL_Map_FilterRow)((DataRowView)(FiltersListBox.SelectedItem)).Row;
                dtsMap.TBL_Map_Filter.Rows.Remove(currentFilter);
            }
        }

        private void FiltersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FiltersListBox.SelectedIndex != -1)
            {
                RenameFilter();
            }
        }

        private void FilterColumnsDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            RenameFilter();
        }

        #endregion

        #region Constructores

        public FormConfigTableFilter()
        {
            InitializeComponent();
        }

        #endregion

        #region Metodos

        private void RenameFilter()
        {
            if (FiltersListBox.SelectedIndex != -1)
            {
                var currentFilter = (XsdDefault.TBL_Map_FilterRow)((DataRowView)(FiltersListBox.SelectedItem)).Row;
                var dtvCols = new DataView(dtsMap.TBL_Map_Filter_Column);

                dtvCols.RowFilter = "fk_Map_Filter = " + currentFilter.id_Map_Filter + " AND Selected = 1";
                dtvCols.Sort = "Filter_Order";

                string newName = "FindBy_";

                foreach (DataRowView col in dtvCols)
                {
                    if (newName != "FindBy_") newName += "_";
                    newName += ((XsdDefault.TBL_Map_Filter_ColumnRow)(col.Row)).Field_Name;
                }

                FilterNameTextBox.Text = newName;
            }
        }

        public void SetDataSource(XsdDataBaseDesign nDtsDataBase, int nId_Object)
        {
            try
            {
                _Id_Object = nId_Object;
                _DtsDataBase = nDtsDataBase;

                var rowObject = nDtsDataBase.TBL_Object.FindByid_Object(nId_Object);
                CatalogNameTextBox.Text = rowObject.Catalog_Name;
                ObjectTypeTextBox.Text = rowObject.Generic_Type;
                SchemaNameTextBox.Text = rowObject.Schema_Name;
                ObjectNameTextBox.Text = rowObject.Object_Name;

                var fieldRows =
                    (CMData.Schemas.XsdDataBase.TBL_FieldRow[])(nDtsDataBase.TBL_Field.Select("fk_Object = " + nId_Object));

                // Agregar los filtros
                var filterRows = rowObject.GetTBL_FilterRows();
                foreach (var filterRow in filterRows)
                {
                    var newMapFilter = dtsMap.TBL_Map_Filter.AddTBL_Map_FilterRow(filterRow.id_Filter,
                                                                                  filterRow.fk_Object,
                                                                                  "Find_" + _FindNumber);
                    _FindNumber += 1;

                    // Agregar los campos incluyendo los previamente mapeados
                    foreach (var f in fieldRows)
                    {
                        // Buscar si ya existe el campo mapeado
                        var filterField =
                            ((CMData.Schemas.XsdDataBase.TBL_Filter_FieldRow[])
                             (nDtsDataBase.TBL_Filter_Field.Select("fk_Filter = " + filterRow.id_Filter +
                                                                   " AND Field_Name = '" + f.Field_Name + "'")));

                        // Agregar la columna para mapear
                        var newMapFilterColumn = dtsMap.TBL_Map_Filter_Column.NewTBL_Map_Filter_ColumnRow();

                        newMapFilterColumn.fk_Map_Filter = newMapFilter.id_Map_Filter;
                        newMapFilterColumn.fk_Object = f.fk_Object;
                        newMapFilterColumn.Field_Name = f.Field_Name;
                        newMapFilterColumn.Field_Type = f.Field_Type;
                        newMapFilterColumn.Specific_Type = f.Specific_Type;
                        newMapFilterColumn.Is_Nullable = f.Is_Nullable;
                        newMapFilterColumn.Max_Length = f.Max_Length;
                        newMapFilterColumn.Precision = f.Precision;
                        newMapFilterColumn.Scale = f.Scale;
                        newMapFilterColumn.PrimaryKey_Order = f.PrimaryKey_Order;

                        if (filterField.Length > 0)
                        {
                            newMapFilterColumn.Filter_Order = filterField[0].Filter_Order;
                            newMapFilterColumn.Selected = true;
                        }
                        else
                        {
                            newMapFilterColumn.Filter_Order = 100;
                            newMapFilterColumn.Selected = false;
                        }

                        dtsMap.TBL_Map_Filter_Column.AddTBL_Map_Filter_ColumnRow(newMapFilterColumn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enlazar los datos, " + ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Funciones

        private bool validateCurrentFilter()
        {
            return true;
        }

        private int FindFilterColumnIndex(XsdDefault.TBL_Map_Filter_ColumnRow nFilter_Column)
        {
            var cols =
                ((XsdDefault.TBL_Map_Filter_ColumnRow[])
                 (dtsMap.TBL_Map_Filter_Column.Select("fk_Map_Filter = " + nFilter_Column.fk_Map_Filter, "Filter_Order")));

            for (int i = 0; i <= cols.Length; i++)
            {
                var col = cols[i];
                if (nFilter_Column.id_Map_Filter_Column == col.id_Map_Filter_Column)
                {
                    return i;
                }
            }
            return 0;
        }

        #endregion
    }
}
