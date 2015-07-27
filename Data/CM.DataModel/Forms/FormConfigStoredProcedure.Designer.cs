namespace CM.DataModel.Forms
{
    partial class FormConfigStoredProcedure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigStoredProcedure));
            this.dtsMap = new CM.DataModel.Schemas.XsdDefault();
            this.BotonesPanel = new System.Windows.Forms.Panel();
            this.AceptarButton = new System.Windows.Forms.Button();
            this.CerrarButton = new System.Windows.Forms.Button();
            this.TextBoxObjectName = new System.Windows.Forms.TextBox();
            this.ObjectNameLabel = new System.Windows.Forms.Label();
            this.ObjectGroupBox = new System.Windows.Forms.GroupBox();
            this.SchemaNameTextBox = new System.Windows.Forms.TextBox();
            this.SchemaNameLabel = new System.Windows.Forms.Label();
            this.CatalogNameTextBox = new System.Windows.Forms.TextBox();
            this.CatalogNameLabel = new System.Windows.Forms.Label();
            this.ObjectTypeTextBox = new System.Windows.Forms.TextBox();
            this.ObjectTypeLabel = new System.Windows.Forms.Label();
            this.RetornaLabel = new System.Windows.Forms.Label();
            this.lstReturnType = new System.Windows.Forms.ComboBox();
            this.RetornaGroupBox = new System.Windows.Forms.GroupBox();
            this.ReturnDataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.TipoDatoLabel = new System.Windows.Forms.Label();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dtsMap)).BeginInit();
            this.BotonesPanel.SuspendLayout();
            this.ObjectGroupBox.SuspendLayout();
            this.RetornaGroupBox.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtsMap
            // 
            this.dtsMap.DataSetName = "XsdDefault";
            this.dtsMap.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // BotonesPanel
            // 
            this.BotonesPanel.Controls.Add(this.AceptarButton);
            this.BotonesPanel.Controls.Add(this.CerrarButton);
            this.BotonesPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BotonesPanel.Location = new System.Drawing.Point(155, 0);
            this.BotonesPanel.Name = "BotonesPanel";
            this.BotonesPanel.Size = new System.Drawing.Size(259, 40);
            this.BotonesPanel.TabIndex = 13;
            // 
            // AceptarButton
            // 
            this.AceptarButton.Location = new System.Drawing.Point(154, 6);
            this.AceptarButton.Name = "AceptarButton";
            this.AceptarButton.Size = new System.Drawing.Size(94, 23);
            this.AceptarButton.TabIndex = 11;
            this.AceptarButton.Text = "Aceptar";
            this.AceptarButton.UseVisualStyleBackColor = true;
            this.AceptarButton.Click += new System.EventHandler(this.AceptarButton_Click);
            // 
            // CerrarButton
            // 
            this.CerrarButton.Location = new System.Drawing.Point(47, 6);
            this.CerrarButton.Name = "CerrarButton";
            this.CerrarButton.Size = new System.Drawing.Size(89, 23);
            this.CerrarButton.TabIndex = 12;
            this.CerrarButton.Text = "Cancelar";
            this.CerrarButton.UseVisualStyleBackColor = true;
            this.CerrarButton.Click += new System.EventHandler(this.CerrarButton_Click);
            // 
            // TextBoxObjectName
            // 
            this.TextBoxObjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxObjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxObjectName.Location = new System.Drawing.Point(84, 72);
            this.TextBoxObjectName.Name = "TextBoxObjectName";
            this.TextBoxObjectName.ReadOnly = true;
            this.TextBoxObjectName.Size = new System.Drawing.Size(290, 20);
            this.TextBoxObjectName.TabIndex = 7;
            this.TextBoxObjectName.Text = "Nombre";
            // 
            // ObjectNameLabel
            // 
            this.ObjectNameLabel.AutoSize = true;
            this.ObjectNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectNameLabel.Location = new System.Drawing.Point(15, 74);
            this.ObjectNameLabel.Name = "ObjectNameLabel";
            this.ObjectNameLabel.Size = new System.Drawing.Size(50, 13);
            this.ObjectNameLabel.TabIndex = 6;
            this.ObjectNameLabel.Text = "Nombre";
            // 
            // ObjectGroupBox
            // 
            this.ObjectGroupBox.Controls.Add(this.TextBoxObjectName);
            this.ObjectGroupBox.Controls.Add(this.ObjectNameLabel);
            this.ObjectGroupBox.Controls.Add(this.SchemaNameTextBox);
            this.ObjectGroupBox.Controls.Add(this.SchemaNameLabel);
            this.ObjectGroupBox.Controls.Add(this.CatalogNameTextBox);
            this.ObjectGroupBox.Controls.Add(this.CatalogNameLabel);
            this.ObjectGroupBox.Controls.Add(this.ObjectTypeTextBox);
            this.ObjectGroupBox.Controls.Add(this.ObjectTypeLabel);
            this.ObjectGroupBox.Location = new System.Drawing.Point(14, 8);
            this.ObjectGroupBox.Name = "ObjectGroupBox";
            this.ObjectGroupBox.Size = new System.Drawing.Size(387, 131);
            this.ObjectGroupBox.TabIndex = 0;
            this.ObjectGroupBox.TabStop = false;
            this.ObjectGroupBox.Text = "Objeto";
            // 
            // SchemaNameTextBox
            // 
            this.SchemaNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SchemaNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SchemaNameTextBox.Location = new System.Drawing.Point(84, 98);
            this.SchemaNameTextBox.Name = "SchemaNameTextBox";
            this.SchemaNameTextBox.ReadOnly = true;
            this.SchemaNameTextBox.Size = new System.Drawing.Size(290, 20);
            this.SchemaNameTextBox.TabIndex = 5;
            this.SchemaNameTextBox.Text = "Esquema";
            // 
            // SchemaNameLabel
            // 
            this.SchemaNameLabel.AutoSize = true;
            this.SchemaNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SchemaNameLabel.Location = new System.Drawing.Point(15, 100);
            this.SchemaNameLabel.Name = "SchemaNameLabel";
            this.SchemaNameLabel.Size = new System.Drawing.Size(58, 13);
            this.SchemaNameLabel.TabIndex = 4;
            this.SchemaNameLabel.Text = "Esquema";
            // 
            // CatalogNameTextBox
            // 
            this.CatalogNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CatalogNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CatalogNameTextBox.Location = new System.Drawing.Point(84, 45);
            this.CatalogNameTextBox.Name = "CatalogNameTextBox";
            this.CatalogNameTextBox.ReadOnly = true;
            this.CatalogNameTextBox.Size = new System.Drawing.Size(290, 20);
            this.CatalogNameTextBox.TabIndex = 3;
            this.CatalogNameTextBox.Text = "Nombre";
            // 
            // CatalogNameLabel
            // 
            this.CatalogNameLabel.AutoSize = true;
            this.CatalogNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CatalogNameLabel.Location = new System.Drawing.Point(15, 47);
            this.CatalogNameLabel.Name = "CatalogNameLabel";
            this.CatalogNameLabel.Size = new System.Drawing.Size(57, 13);
            this.CatalogNameLabel.TabIndex = 2;
            this.CatalogNameLabel.Text = "Catalogo";
            // 
            // ObjectTypeTextBox
            // 
            this.ObjectTypeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ObjectTypeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectTypeTextBox.Location = new System.Drawing.Point(84, 19);
            this.ObjectTypeTextBox.Name = "ObjectTypeTextBox";
            this.ObjectTypeTextBox.ReadOnly = true;
            this.ObjectTypeTextBox.Size = new System.Drawing.Size(290, 20);
            this.ObjectTypeTextBox.TabIndex = 1;
            this.ObjectTypeTextBox.Text = "Tipo";
            // 
            // ObjectTypeLabel
            // 
            this.ObjectTypeLabel.AutoSize = true;
            this.ObjectTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectTypeLabel.Location = new System.Drawing.Point(15, 21);
            this.ObjectTypeLabel.Name = "ObjectTypeLabel";
            this.ObjectTypeLabel.Size = new System.Drawing.Size(32, 13);
            this.ObjectTypeLabel.TabIndex = 0;
            this.ObjectTypeLabel.Text = "Tipo";
            // 
            // RetornaLabel
            // 
            this.RetornaLabel.AutoSize = true;
            this.RetornaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RetornaLabel.Location = new System.Drawing.Point(6, 16);
            this.RetornaLabel.Name = "RetornaLabel";
            this.RetornaLabel.Size = new System.Drawing.Size(52, 13);
            this.RetornaLabel.TabIndex = 1;
            this.RetornaLabel.Text = "Retorna";
            // 
            // lstReturnType
            // 
            this.lstReturnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstReturnType.FormattingEnabled = true;
            this.lstReturnType.Location = new System.Drawing.Point(84, 13);
            this.lstReturnType.Name = "lstReturnType";
            this.lstReturnType.Size = new System.Drawing.Size(290, 21);
            this.lstReturnType.TabIndex = 2;
            this.lstReturnType.SelectedIndexChanged += new System.EventHandler(this.lstReturnType_SelectedIndexChanged);
            // 
            // RetornaGroupBox
            // 
            this.RetornaGroupBox.Controls.Add(this.ReturnDataTypeComboBox);
            this.RetornaGroupBox.Controls.Add(this.TipoDatoLabel);
            this.RetornaGroupBox.Controls.Add(this.lstReturnType);
            this.RetornaGroupBox.Controls.Add(this.RetornaLabel);
            this.RetornaGroupBox.Location = new System.Drawing.Point(14, 145);
            this.RetornaGroupBox.Name = "RetornaGroupBox";
            this.RetornaGroupBox.Size = new System.Drawing.Size(387, 73);
            this.RetornaGroupBox.TabIndex = 1;
            this.RetornaGroupBox.TabStop = false;
            // 
            // ReturnDataTypeComboBox
            // 
            this.ReturnDataTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReturnDataTypeComboBox.FormattingEnabled = true;
            this.ReturnDataTypeComboBox.Location = new System.Drawing.Point(84, 40);
            this.ReturnDataTypeComboBox.Name = "ReturnDataTypeComboBox";
            this.ReturnDataTypeComboBox.Size = new System.Drawing.Size(290, 21);
            this.ReturnDataTypeComboBox.Sorted = true;
            this.ReturnDataTypeComboBox.TabIndex = 4;
            // 
            // TipoDatoLabel
            // 
            this.TipoDatoLabel.AutoSize = true;
            this.TipoDatoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TipoDatoLabel.Location = new System.Drawing.Point(6, 43);
            this.TipoDatoLabel.Name = "TipoDatoLabel";
            this.TipoDatoLabel.Size = new System.Drawing.Size(61, 13);
            this.TipoDatoLabel.TabIndex = 3;
            this.TipoDatoLabel.Text = "Tipo dato";
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.RetornaGroupBox);
            this.MainPanel.Controls.Add(this.ObjectGroupBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Padding = new System.Windows.Forms.Padding(5);
            this.MainPanel.Size = new System.Drawing.Size(414, 228);
            this.MainPanel.TabIndex = 11;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.BotonesPanel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 228);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.BottomPanel.Size = new System.Drawing.Size(414, 42);
            this.BottomPanel.TabIndex = 10;
            // 
            // FormConfigStoredProcedure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 270);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.BottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigStoredProcedure";
            this.Text = "Configuración de procedimientos almacenados";
            this.Load += new System.EventHandler(this.FormConfigStoredProcedure_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtsMap)).EndInit();
            this.BotonesPanel.ResumeLayout(false);
            this.ObjectGroupBox.ResumeLayout(false);
            this.ObjectGroupBox.PerformLayout();
            this.RetornaGroupBox.ResumeLayout(false);
            this.RetornaGroupBox.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Schemas.XsdDefault dtsMap;
        internal System.Windows.Forms.Panel BotonesPanel;
        internal System.Windows.Forms.Button AceptarButton;
        internal System.Windows.Forms.Button CerrarButton;
        internal System.Windows.Forms.TextBox TextBoxObjectName;
        internal System.Windows.Forms.Label ObjectNameLabel;
        internal System.Windows.Forms.GroupBox ObjectGroupBox;
        internal System.Windows.Forms.TextBox SchemaNameTextBox;
        internal System.Windows.Forms.Label SchemaNameLabel;
        internal System.Windows.Forms.TextBox CatalogNameTextBox;
        internal System.Windows.Forms.Label CatalogNameLabel;
        internal System.Windows.Forms.TextBox ObjectTypeTextBox;
        internal System.Windows.Forms.Label ObjectTypeLabel;
        internal System.Windows.Forms.Label RetornaLabel;
        internal System.Windows.Forms.ComboBox lstReturnType;
        internal System.Windows.Forms.GroupBox RetornaGroupBox;
        internal System.Windows.Forms.ComboBox ReturnDataTypeComboBox;
        internal System.Windows.Forms.Label TipoDatoLabel;
        internal System.Windows.Forms.Panel MainPanel;
        internal System.Windows.Forms.Panel BottomPanel;
    }
}