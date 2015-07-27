using System;
using System.IO;
using System.Windows.Forms;
using CM.Tools.Misellaneous;
using Tools;

namespace CM.DataModel.Forms
{
    public partial class FormLog : Form
    {
        #region Propiedades

        public string LogName
        {
            get { return Text; }
            set { Text = value; }
        }

        public string Content
        {
            get { return ContentTextBox.Text; }
            set { ContentTextBox.Text = value; }
        }

        #endregion

        #region Eventos

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();

            dlg.Filter = "Archivo de texto *.txt|*.txt";
            dlg.FileName = LogName + "_" + DateTime.Now.ToString("yyyyMMddHHmm");

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var sw = new StreamWriter(dlg.FileName);

                sw.Write(ContentTextBox.Text);
                sw.Flush();
                sw.Close();
            }
        }

        private void CerrarButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Constructores

        public FormLog()
        {
            InitializeComponent();
        }

        #endregion

        #region Metodos

        public void AppendText(string text)
        {
            ContentTextBox.AppendText(text);
        }

        public void AppendLine(string text)
        {
            ContentTextBox.AppendText(text + ControlChars.CrLf);
        }

        #endregion
    }
}
