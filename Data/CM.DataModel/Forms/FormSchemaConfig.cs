using System;
using System.Windows.Forms;
using CM.Tools.Misellaneous;
using Tools;

namespace CM.DataModel.Forms
{
    public partial class FormSchemaConfig : Form
    {
        #region Constructores

        public FormSchemaConfig(CM.DataModel.Schemas.XsdDataBaseDesign nData)
        {
            InitializeComponent();

            DataBaseDataSet = nData;
            SchemaDataGridView.DataSource = DataBaseDataSet;

            DataBaseDataSet.TBL_Schema.AcceptChanges();
        }

        #endregion

        #region Eventos

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            DataBaseDataSet.TBL_Schema.RejectChanges();

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
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
                DataBaseDataSet.TBL_Schema.AcceptChanges();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        #endregion

        #region Funciones

        private bool Validar()
        {
            SchemaDataGridView.EndEdit();

            foreach (CMData.Schemas.XsdDataBase.TBL_SchemaRow Item in DataBaseDataSet.TBL_Schema)
            {
                // Validar que las casilla no se encuentre vacias
                if (Item.Schema_Alias == "")
                {
                    var Respuesta = MessageBox.Show("El nombre de clase del Schemao: " + Item.Schema_Name + " no puede quedar vacio, ¿desea usar el nombre del Esquema como alias de clase?", Program.AssemblyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (Respuesta == System.Windows.Forms.DialogResult.Yes)
                        Item.Schema_Alias = FormatCode.ToIdentifier(Item["Schema_Name"].ToString());
                    else
                        return false;
                }
            }

            return true;
        }

        #endregion
    }
}
