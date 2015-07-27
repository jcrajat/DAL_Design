using System;
using System.Windows.Forms;
using CM.Tools.Misellaneous;
using CMData.Schemas;
using Tools;

namespace CM.DataModel.Forms
{
    public partial class FormCatalogConfig : Form
    {
        #region Constructores

        public FormCatalogConfig(XsdDataBase nData)
        {
            InitializeComponent();

            DataBaseDataSet = nData;
            CatalogDataGridView.DataSource = DataBaseDataSet;

            DataBaseDataSet.TBL_Catalog.AcceptChanges();
        }

        #endregion

        #region Eventos

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            DataBaseDataSet.TBL_Catalog.RejectChanges();

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AceptarButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        #endregion

        #region Metodos

        private void Save()
        {
            if (Validar())
            {
                DataBaseDataSet.TBL_Catalog.AcceptChanges();

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        #endregion

        #region Funciones

        private bool Validar()
        {
            CatalogDataGridView.EndEdit();

            foreach (XsdDataBase.TBL_CatalogRow Item in DataBaseDataSet.TBL_Catalog)
            {
                // Validar que las casilla no se encuentre vacias
                if (Item.Class_Name == "")
                {
                    DialogResult Respuesta =
                        MessageBox.Show(
                            @"El nombre de clase del catalogo: " + Item.Catalog_Name +
                            @" no puede quedar vacio, ¿desea usar el nombre del catalogo como nombre de clase?",
                            Program.AssemblyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (Respuesta == DialogResult.Yes)
                        Item.Class_Name = FormatCode.ToIdentifier(Item["Catalog_Name"].ToString());
                    else
                        return false;
                }
                else if (Item.Class_File_Name == "")
                {
                    DialogResult Respuesta =
                        MessageBox.Show(
                            @"El nombre de archivo del catalogo: " + Item.Catalog_Name +
                            @" no puede quedar vacio, ¿desea usar el nombre del catalogo como nombre de archivo?",
                            Program.AssemblyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (Respuesta == DialogResult.Yes)
                        Item.Class_File_Name = FormatCode.ToIdentifier(Item["Catalog_Name"].ToString());
                    else
                        return false;
                }
            }

            return true;
        }

        #endregion
    }
}