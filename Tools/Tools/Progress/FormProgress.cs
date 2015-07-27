using System;
using System.Windows.Forms;

namespace Tools.Progress
{
    public partial class FormProgress : Form
    {
        #region Declaraciones

        private bool _canCancel;

        private int _lastValueProcess;

        private int _lastValueAction;

        #endregion

        #region Propiedades

        public bool Cancel { get; private set; }

        public bool CanCancel
        {
            get
            {
                return _canCancel;
            }
            set
            {
                _canCancel = value;
                TheCancelButton.Enabled = (!Cancel && value);
            }
        }

        public string Process
        {
            get { return ProcessLabel.Text; }
            set { ProcessLabel.Text = value; }
        }

        public string Action
        {
            get { return ActionLabel.Text; }
            set { ActionLabel.Text = value; }
        }

        public int MaxValueProcess
        {
            get { return ProcessProgressBar.Maximum; }
            set { ProcessProgressBar.Maximum = value <= 0 ? 1 : value; }
        }

        public int MaxValueAction
        {
            get { return ActionProgressBar.Maximum; }
            set { ActionProgressBar.Maximum = value <= 0 ? 1 : value; }
        }

        public int ValueProcess
        {
            get { return ProcessProgressBar.Value; }
            set
            {
                ProcessProgressBar.Value = (ProcessProgressBar.Maximum > value ? value : ProcessProgressBar.Maximum);

                var progreso = (ProcessProgressBar.Maximum > 0 ? value/(double) ProcessProgressBar.Maximum : 0.0)*100.0;

                ProcessValorLabel.Text = progreso.ToString("#0") + @"%";

                if (value < _lastValueProcess)
                    _lastValueProcess = 0;
                else if (((value - _lastValueProcess)/(double) ProcessProgressBar.Maximum) > 0.01)
                    _lastValueProcess = ProcessProgressBar.Value;

                this.Refresh();
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public int ValueAction
        {
            get { return ActionProgressBar.Value; }
            set
            {
                ActionProgressBar.Value = ActionProgressBar.Maximum > value ? value : ActionProgressBar.Maximum;

                double progreso = (ActionProgressBar.Maximum > 0 ? value/(double) ActionProgressBar.Maximum : 0.0)*100.0;

                ActionValorLabel.Text = progreso.ToString("#0") + @"%";

                if (value < _lastValueAction)
                    _lastValueAction = 0;
                else if (((value - _lastValueAction) / (double)ActionProgressBar.Maximum) > 0.01)
                    _lastValueAction = ActionProgressBar.Value;

                this.Refresh();
                System.Windows.Forms.Application.DoEvents();
            }
        }

        #endregion

        #region Eventos

        private void TheCancelButton_Click(object sender, EventArgs e)
        {
            this.Cancel = true;
            this.TheCancelButton.Enabled = false;
        }

        #endregion

        #region Constructores

        public FormProgress()
        {
            InitializeComponent();
        }

        #endregion

        #region Metodos

        public void IncrementProcess()
        {
            this.ValueProcess++;
        }

        public void IncrementAction()
        {
            this.ValueAction++;
        }

        #endregion
    }
}
