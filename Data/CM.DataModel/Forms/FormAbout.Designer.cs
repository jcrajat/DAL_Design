namespace CM.DataModel.Forms
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.SeparatorPanel = new System.Windows.Forms.Panel();
            this.AceptarButton = new System.Windows.Forms.Button();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.CompanyNameLabel = new System.Windows.Forms.Label();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.ProductNameLabel = new System.Windows.Forms.Label();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SeparatorPanel
            // 
            this.SeparatorPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SeparatorPanel.Location = new System.Drawing.Point(279, 198);
            this.SeparatorPanel.Name = "SeparatorPanel";
            this.SeparatorPanel.Size = new System.Drawing.Size(232, 4);
            this.SeparatorPanel.TabIndex = 23;
            // 
            // AceptarButton
            // 
            this.AceptarButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AceptarButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AceptarButton.Image = global::CM.DataModel.Properties.Resources.tick;
            this.AceptarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AceptarButton.Location = new System.Drawing.Point(430, 214);
            this.AceptarButton.Name = "AceptarButton";
            this.AceptarButton.Size = new System.Drawing.Size(77, 24);
            this.AceptarButton.TabIndex = 16;
            this.AceptarButton.Text = "&Aceptar";
            this.AceptarButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AceptarButton.Click += new System.EventHandler(this.AceptarButton_Click);
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.DescriptionTextBox.Location = new System.Drawing.Point(283, 126);
            this.DescriptionTextBox.Multiline = true;
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.ReadOnly = true;
            this.DescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DescriptionTextBox.Size = new System.Drawing.Size(224, 64);
            this.DescriptionTextBox.TabIndex = 22;
            // 
            // CompanyNameLabel
            // 
            this.CompanyNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CompanyNameLabel.Location = new System.Drawing.Point(283, 94);
            this.CompanyNameLabel.Name = "CompanyNameLabel";
            this.CompanyNameLabel.Size = new System.Drawing.Size(224, 16);
            this.CompanyNameLabel.TabIndex = 21;
            this.CompanyNameLabel.Text = "CompanyName";
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CopyrightLabel.Location = new System.Drawing.Point(283, 70);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(224, 16);
            this.CopyrightLabel.TabIndex = 20;
            this.CopyrightLabel.Text = "Copyright";
            // 
            // VersionLabel
            // 
            this.VersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.Location = new System.Drawing.Point(283, 46);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(224, 16);
            this.VersionLabel.TabIndex = 19;
            this.VersionLabel.Text = "Version";
            // 
            // ProductNameLabel
            // 
            this.ProductNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.ProductNameLabel.Location = new System.Drawing.Point(283, 14);
            this.ProductNameLabel.Name = "ProductNameLabel";
            this.ProductNameLabel.Size = new System.Drawing.Size(224, 23);
            this.ProductNameLabel.TabIndex = 17;
            this.ProductNameLabel.Text = "ProductName";
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.LogoPictureBox.Image = global::CM.DataModel.Properties.Resources.database2;
            this.LogoPictureBox.Location = new System.Drawing.Point(10, 0);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(256, 252);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.LogoPictureBox.TabIndex = 18;
            this.LogoPictureBox.TabStop = false;
            // 
            // FormAbout
            // 
            this.AcceptButton = this.AceptarButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.AceptarButton;
            this.ClientSize = new System.Drawing.Size(514, 252);
            this.Controls.Add(this.SeparatorPanel);
            this.Controls.Add(this.AceptarButton);
            this.Controls.Add(this.DescriptionTextBox);
            this.Controls.Add(this.CompanyNameLabel);
            this.Controls.Add(this.CopyrightLabel);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.ProductNameLabel);
            this.Controls.Add(this.LogoPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Panel SeparatorPanel;
        internal System.Windows.Forms.Button AceptarButton;
        internal System.Windows.Forms.TextBox DescriptionTextBox;
        internal System.Windows.Forms.Label CompanyNameLabel;
        internal System.Windows.Forms.Label CopyrightLabel;
        internal System.Windows.Forms.Label VersionLabel;
        internal System.Windows.Forms.Label ProductNameLabel;
        internal System.Windows.Forms.PictureBox LogoPictureBox;
    }
}