namespace CM.DataModel.Forms
{
    partial class FormLog
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof (FormLog));
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ContentTextBox = new System.Windows.Forms.RichTextBox();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.GuardarButton = new System.Windows.Forms.Button();
            this.CerrarButton = new System.Windows.Forms.Button();
            this.MainPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ContentTextBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 5);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Padding = new System.Windows.Forms.Padding(5);
            this.MainPanel.Size = new System.Drawing.Size(733, 379);
            this.MainPanel.TabIndex = 4;
            // 
            // ContentTextBox
            // 
            this.ContentTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F,
                                                               System.Drawing.FontStyle.Regular,
                                                               System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.ContentTextBox.Location = new System.Drawing.Point(5, 5);
            this.ContentTextBox.Name = "ContentTextBox";
            this.ContentTextBox.Size = new System.Drawing.Size(723, 369);
            this.ContentTextBox.TabIndex = 0;
            this.ContentTextBox.Text = "";
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.ButtonsPanel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 384);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(733, 33);
            this.BottomPanel.TabIndex = 3;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.GuardarButton);
            this.ButtonsPanel.Controls.Add(this.CerrarButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonsPanel.Location = new System.Drawing.Point(468, 0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(265, 33);
            this.ButtonsPanel.TabIndex = 0;
            // 
            // GuardarButton
            // 
            this.GuardarButton.Location = new System.Drawing.Point(39, 7);
            this.GuardarButton.Name = "GuardarButton";
            this.GuardarButton.Size = new System.Drawing.Size(94, 23);
            this.GuardarButton.TabIndex = 13;
            this.GuardarButton.Text = "Guardar";
            this.GuardarButton.UseVisualStyleBackColor = true;
            this.GuardarButton.Click += new System.EventHandler(this.GuardarButton_Click);
            // 
            // CerrarButton
            // 
            this.CerrarButton.Location = new System.Drawing.Point(159, 6);
            this.CerrarButton.Name = "CerrarButton";
            this.CerrarButton.Size = new System.Drawing.Size(94, 23);
            this.CerrarButton.TabIndex = 12;
            this.CerrarButton.Text = "Cerrar";
            this.CerrarButton.UseVisualStyleBackColor = true;
            this.CerrarButton.Click += new System.EventHandler(this.CerrarButton_Click);
            // 
            // FormLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CerrarButton;
            this.ClientSize = new System.Drawing.Size(733, 417);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.BottomPanel);
            this.Name = "FormLog";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Text = "Log";
            this.MainPanel.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel MainPanel;
        internal System.Windows.Forms.RichTextBox ContentTextBox;
        internal System.Windows.Forms.Panel BottomPanel;
        internal System.Windows.Forms.Panel ButtonsPanel;
        internal System.Windows.Forms.Button GuardarButton;
        internal System.Windows.Forms.Button CerrarButton;

    }
}