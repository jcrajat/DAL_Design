using System;
using System.Windows.Forms;

namespace CM.DataModel.Forms
{
    public partial class FormAbout : Form
    {
        #region Eventos

        private void FormAbout_Load(object sender, EventArgs e)
        {
            ShowData();
        }

        private void AceptarButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Constructores

        public FormAbout()
        {
            InitializeComponent();
        }

        #endregion

        #region Metodos

        private void ShowData()
        {
            this.Text = String.Format("Acerca de {0}", Program.AssemblyTitle);
            this.ProductNameLabel.Text = Program.AssemblyProduct;
            this.VersionLabel.Text = String.Format("Versión {0}", Program.AssemblyVersion);
            this.CopyrightLabel.Text = Program.AssemblyCopyright;
            this.CompanyNameLabel.Text = Program.AssemblyCompany;
            this.DescriptionTextBox.Text = Program.AssemblyDescription;
        }

        #endregion
    }
}
