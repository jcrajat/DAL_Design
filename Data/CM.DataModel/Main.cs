using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CM.DataModel.Forms;

namespace CM.DataModel
{
    public partial class Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region Declaraciones

        private int _ChildFormNumber;

        private List<string> _DropFileNames = new List<string>();

        #endregion

        #region Constructores

        public Main()
        {
            InitializeComponent();
        }

        #endregion

        #region Propiedades

        public FormAccessDesigner FormAccessDesignerActive { get; set; }

        #endregion

        #region Eventos

        private void FormMDI_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("FileDrop") != null)
            {
                var FileNames = (string[])e.Data.GetData("FileDrop");
                this._DropFileNames.Clear();

                foreach (var FileName in FileNames)
                {
                    var extension = System.IO.Path.GetExtension(FileName);
                    if (extension != null && extension.ToUpper() == ".SQLDESIGN")
                        this._DropFileNames.Add(FileName);
                }

                if (this._DropFileNames.Count > 0)
                {
                    e.Effect = DragDropEffects.Move;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void FormMDI_DragDrop(object sender, DragEventArgs e)
        {
            foreach (var FileName in this._DropFileNames)
            {
                Cargar(FileName);
            }
        }

        private void FormMDI_DragLeave(object sender, EventArgs e)
        {
            this._DropFileNames.Clear();
        }

        private void SaveToolStripButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new FormAbout();
            dlg.ShowDialog();
        }

        private void NewToolStripButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Cree una nueva instancia del formulario secundario.
                var ChildForm = new FormAccessDesigner();

                // Conviértalo en un elemento secundario de este formulario MDI antes de mostrarlo.
                ChildForm.MdiParent = this;

                _ChildFormNumber += 1;
                ChildForm.Text = "Diseñador " + _ChildFormNumber;

                ChildForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el diseñador, " + ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenToolStripButton_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var OpenFileDialog = new OpenFileDialog();

                OpenFileDialog.Filter = "Archivos de diseño de acceso sql (*.sqldesign)|*.sqldesign";

                if (OpenFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string FileName = OpenFileDialog.FileName;

                    // Cree una nueva instancia del formulario secundario.
                    var ChildForm = new FormAccessDesigner();

                    // Conviértalo en un elemento secundario de este formulario MDI antes de mostrarlo.
                    ChildForm.MdiParent = this;

                    _ChildFormNumber += 1;
                    ChildForm.Text = System.IO.Path.GetFileName(FileName);

                    ChildForm.LoadConfiguration(FileName);

                    ChildForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el diseñador, " + ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void MyBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            for (int i = 0; i <= 10; i++)
            {
                worker.ReportProgress(i * 10);
                System.Threading.Thread.Sleep(50);
            }

            worker.ReportProgress(0);
        }

        private void MyBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SetProgressBarValue(e.ProgressPercentage);
        }

        private void AddConnectionToolStripButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.FormAccessDesignerActive != null)
            {
                this.FormAccessDesignerActive.AddNewConnection();
            }
        }
        private void RefreshtoolStripButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.FormAccessDesignerActive != null)
            {
                this.FormAccessDesignerActive.RefreshDbObjets(false);
                this.FormAccessDesignerActive.Focus();
            }

        }

        private void RefreshOnlyNewsToolStripButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.FormAccessDesignerActive != null)
            {
                this.FormAccessDesignerActive.RefreshDbObjets(true);
                this.FormAccessDesignerActive.Focus();
            }
        }

        private void GenerateCodeToolStripButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.FormAccessDesignerActive != null)
            {
                this.FormAccessDesignerActive.GenerateAccessClass();
                this.FormAccessDesignerActive.Focus();
            }
        }

        private void CatalogToolStripButton_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.FormAccessDesignerActive != null)
            {
                this.FormAccessDesignerActive.CatalogConfig();
                this.FormAccessDesignerActive.Focus();
            }
        }

        private void ConexionesToolStripButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.FormAccessDesignerActive != null)
            {
                //this.ConexionesChecked = ConexionesToolStripButton.Checked;
                //this.FormAccessDesignerActive.ConexionesChecked = ConexionesToolStripButton.Checked;
            }
        }
        private void SchemaToolStripButton_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.FormAccessDesignerActive != null)
            {
                this.FormAccessDesignerActive.SchemaConfig();
                this.FormAccessDesignerActive.Focus();
            }
        }

        #endregion

        #region Metodos

        public void AnimateProgressBar()
        {
            MyBackgroundWorker.RunWorkerAsync();
        }

        public void SetProgressBarValue(int Value)
        {
            MyProgressBar.Value = Value;
        }

        public void Cargar(string nFileName)
        {
            // Cree una nueva instancia del formulario secundario.
            var ChildForm = new FormAccessDesigner();

            // Conviértalo en un elemento secundario de este formulario MDI antes de mostrarlo.
            ChildForm.MdiParent = this;
            ChildForm.LoadConfiguration(nFileName);

            ChildForm.Show();
        }

        private void Save()
        {
            try
            {
                if (this.ActiveMdiChild == null) return;

                AnimateProgressBar();

                var dlg = (FormAccessDesigner)(this.ActiveMdiChild);

                if (string.IsNullOrEmpty(dlg.FileName))
                    SaveAs();
                else
                    dlg.SaveConfiguration();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios, " + ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAs()
        {
            try
            {
                if (this.ActiveMdiChild == null) return;

                var SaveFileDialog = new SaveFileDialog();

                SaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                SaveFileDialog.Filter = "Archivos de diseño de acceso sql (*.sqldesign)|*.sqldesign|Todos los archivos (*.*)|*.*";

                var dlg = ((FormAccessDesigner)this.ActiveMdiChild);

                SaveFileDialog.FileName = dlg.FileName == "" ? "DataBaseMapping.sqldesign" : dlg.FileName;

                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                    dlg.SaveConfiguration(SaveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios, " + ex.Message, Program.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void ActivateControls(bool nActivar)
        {
            SaveToolStripButton.Enabled = nActivar;

            AddConnectionToolStripButton.Enabled = nActivar;

            RefreshtoolStripButton.Enabled = nActivar;
            RefreshOnlyNewsToolStripButton.Enabled = nActivar;

            GenerateCodeToolStripButton.Enabled = nActivar;

            CatalogToolStripButton.Enabled = nActivar;
            SchemaToolStripButton.Enabled = nActivar;

        }

        #endregion























    }
}
