namespace Tools.Progress
{
    public partial class FormProgress
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgress));
            this.BaseGroupBox = new System.Windows.Forms.GroupBox();
            this.ProcessValorLabel = new System.Windows.Forms.Label();
            this.ProcessProgressBar = new System.Windows.Forms.ProgressBar();
            this.ActionValorLabel = new System.Windows.Forms.Label();
            this.ActionLabel = new System.Windows.Forms.Label();
            this.ProcessLabel = new System.Windows.Forms.Label();
            this.ActionProgressBar = new System.Windows.Forms.ProgressBar();
            this.TheCancelButton = new System.Windows.Forms.Button();
            this.BaseGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BaseGroupBox
            // 
            this.BaseGroupBox.Controls.Add(this.ProcessValorLabel);
            this.BaseGroupBox.Controls.Add(this.ProcessProgressBar);
            this.BaseGroupBox.Controls.Add(this.ActionValorLabel);
            this.BaseGroupBox.Controls.Add(this.ActionLabel);
            this.BaseGroupBox.Controls.Add(this.ProcessLabel);
            this.BaseGroupBox.Controls.Add(this.ActionProgressBar);
            this.BaseGroupBox.Location = new System.Drawing.Point(6, 2);
            this.BaseGroupBox.Name = "BaseGroupBox";
            this.BaseGroupBox.Size = new System.Drawing.Size(371, 113);
            this.BaseGroupBox.TabIndex = 5;
            this.BaseGroupBox.TabStop = false;
            // 
            // ProcessValorLabel
            // 
            this.ProcessValorLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProcessValorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessValorLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ProcessValorLabel.Location = new System.Drawing.Point(169, 37);
            this.ProcessValorLabel.Name = "ProcessValorLabel";
            this.ProcessValorLabel.Size = new System.Drawing.Size(40, 15);
            this.ProcessValorLabel.TabIndex = 8;
            this.ProcessValorLabel.Text = "100%";
            this.ProcessValorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProcessProgressBar
            // 
            this.ProcessProgressBar.Location = new System.Drawing.Point(5, 32);
            this.ProcessProgressBar.Name = "ProcessProgressBar";
            this.ProcessProgressBar.Size = new System.Drawing.Size(360, 22);
            this.ProcessProgressBar.TabIndex = 7;
            this.ProcessProgressBar.Value = 100;
            // 
            // ActionValorLabel
            // 
            this.ActionValorLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActionValorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionValorLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ActionValorLabel.Location = new System.Drawing.Point(169, 87);
            this.ActionValorLabel.Name = "ActionValorLabel";
            this.ActionValorLabel.Size = new System.Drawing.Size(40, 15);
            this.ActionValorLabel.TabIndex = 6;
            this.ActionValorLabel.Text = "100%";
            this.ActionValorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ActionLabel
            // 
            this.ActionLabel.AutoSize = true;
            this.ActionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionLabel.Location = new System.Drawing.Point(6, 61);
            this.ActionLabel.Name = "ActionLabel";
            this.ActionLabel.Size = new System.Drawing.Size(43, 15);
            this.ActionLabel.TabIndex = 5;
            this.ActionLabel.Text = "Acción";
            // 
            // ProcessLabel
            // 
            this.ProcessLabel.AutoSize = true;
            this.ProcessLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessLabel.Location = new System.Drawing.Point(6, 11);
            this.ProcessLabel.Name = "ProcessLabel";
            this.ProcessLabel.Size = new System.Drawing.Size(58, 15);
            this.ProcessLabel.TabIndex = 4;
            this.ProcessLabel.Text = "Process";
            // 
            // ActionProgressBar
            // 
            this.ActionProgressBar.Location = new System.Drawing.Point(6, 82);
            this.ActionProgressBar.Name = "ActionProgressBar";
            this.ActionProgressBar.Size = new System.Drawing.Size(359, 22);
            this.ActionProgressBar.TabIndex = 0;
            this.ActionProgressBar.Value = 100;
            // 
            // TheCancelButton
            // 
            this.TheCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.TheCancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TheCancelButton.Image = ((System.Drawing.Image)(resources.GetObject("TheCancelButton.Image")));
            this.TheCancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TheCancelButton.Location = new System.Drawing.Point(158, 121);
            this.TheCancelButton.Name = "TheCancelButton";
            this.TheCancelButton.Size = new System.Drawing.Size(72, 24);
            this.TheCancelButton.TabIndex = 4;
            this.TheCancelButton.Text = "&Cancelar";
            this.TheCancelButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TheCancelButton.Click += new System.EventHandler(this.TheCancelButton_Click);
            // 
            // FormProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 154);
            this.ControlBox = false;
            this.Controls.Add(this.BaseGroupBox);
            this.Controls.Add(this.TheCancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Progreso";
            this.BaseGroupBox.ResumeLayout(false);
            this.BaseGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox BaseGroupBox;
        internal System.Windows.Forms.Label ProcessValorLabel;
        internal System.Windows.Forms.ProgressBar ProcessProgressBar;
        internal System.Windows.Forms.Label ActionValorLabel;
        internal System.Windows.Forms.Label ActionLabel;
        internal System.Windows.Forms.Label ProcessLabel;
        internal System.Windows.Forms.ProgressBar ActionProgressBar;
        internal System.Windows.Forms.Button TheCancelButton;
    }
}