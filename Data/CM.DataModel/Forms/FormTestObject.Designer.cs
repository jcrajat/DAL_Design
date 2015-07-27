namespace CM.DataModel.Forms
{
    partial class FormTestObject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestObject));
            this.DefaultDataSet = new CM.DataModel.Schemas.XsdDefault();
            this.ResultDataGridView = new System.Windows.Forms.DataGridView();
            this.ObjectNameTextBox = new System.Windows.Forms.TextBox();
            this.EjecutarButton = new System.Windows.Forms.Button();
            this.ObjectNameLabel = new System.Windows.Forms.Label();
            this.CerrarButton = new System.Windows.Forms.Button();
            this.BotonesPanel = new System.Windows.Forms.Panel();
            this.GroupBoxObject = new System.Windows.Forms.GroupBox();
            this.SchemaNameTextBox = new System.Windows.Forms.TextBox();
            this.SchemaNameLabel = new System.Windows.Forms.Label();
            this.CatalogNameTextBox = new System.Windows.Forms.TextBox();
            this.CatalogNameLabel = new System.Windows.Forms.Label();
            this.ObjectTypeTextBox = new System.Windows.Forms.TextBox();
            this.ObjectTypeLabel = new System.Windows.Forms.Label();
            this.ValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsNullMDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ConnectionTextBox = new System.Windows.Forms.TextBox();
            this.ConexionGroupBox = new System.Windows.Forms.GroupBox();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.ParametrosGroupBox = new System.Windows.Forms.GroupBox();
            this.ParametrosDataGridView = new System.Windows.Forms.DataGridView();
            this.NameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BottomPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultDataGridView)).BeginInit();
            this.BotonesPanel.SuspendLayout();
            this.GroupBoxObject.SuspendLayout();
            this.ConexionGroupBox.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.ParametrosGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParametrosDataGridView)).BeginInit();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DefaultDataSet
            // 
            this.DefaultDataSet.DataSetName = "XsdDefault";
            this.DefaultDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ResultDataGridView
            // 
            this.ResultDataGridView.AllowUserToAddRows = false;
            this.ResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultDataGridView.Location = new System.Drawing.Point(0, 235);
            this.ResultDataGridView.Name = "ResultDataGridView";
            this.ResultDataGridView.Size = new System.Drawing.Size(765, 242);
            this.ResultDataGridView.TabIndex = 0;
            // 
            // ObjectNameTextBox
            // 
            this.ObjectNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ObjectNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectNameTextBox.Location = new System.Drawing.Point(75, 100);
            this.ObjectNameTextBox.Name = "ObjectNameTextBox";
            this.ObjectNameTextBox.ReadOnly = true;
            this.ObjectNameTextBox.Size = new System.Drawing.Size(290, 20);
            this.ObjectNameTextBox.TabIndex = 7;
            this.ObjectNameTextBox.Text = "Nombre";
            // 
            // EjecutarButton
            // 
            this.EjecutarButton.Image = global::CM.DataModel.Properties.Resources.bullet_go;
            this.EjecutarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.EjecutarButton.Location = new System.Drawing.Point(8, 206);
            this.EjecutarButton.Name = "EjecutarButton";
            this.EjecutarButton.Size = new System.Drawing.Size(94, 23);
            this.EjecutarButton.TabIndex = 12;
            this.EjecutarButton.Text = "Ejecutar";
            this.EjecutarButton.UseVisualStyleBackColor = true;
            this.EjecutarButton.Click += new System.EventHandler(this.EjecutarButton_Click);
            // 
            // ObjectNameLabel
            // 
            this.ObjectNameLabel.AutoSize = true;
            this.ObjectNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectNameLabel.Location = new System.Drawing.Point(8, 102);
            this.ObjectNameLabel.Name = "ObjectNameLabel";
            this.ObjectNameLabel.Size = new System.Drawing.Size(50, 13);
            this.ObjectNameLabel.TabIndex = 6;
            this.ObjectNameLabel.Text = "Nombre";
            // 
            // CerrarButton
            // 
            this.CerrarButton.Image = global::CM.DataModel.Properties.Resources.door_out;
            this.CerrarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CerrarButton.Location = new System.Drawing.Point(3, 0);
            this.CerrarButton.Name = "CerrarButton";
            this.CerrarButton.Size = new System.Drawing.Size(94, 23);
            this.CerrarButton.TabIndex = 11;
            this.CerrarButton.Text = "Cerrar";
            this.CerrarButton.UseVisualStyleBackColor = true;
            this.CerrarButton.Click += new System.EventHandler(this.CerrarButton_Click);
            // 
            // BotonesPanel
            // 
            this.BotonesPanel.Controls.Add(this.CerrarButton);
            this.BotonesPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BotonesPanel.Location = new System.Drawing.Point(656, 5);
            this.BotonesPanel.Name = "BotonesPanel";
            this.BotonesPanel.Size = new System.Drawing.Size(104, 27);
            this.BotonesPanel.TabIndex = 13;
            // 
            // GroupBoxObject
            // 
            this.GroupBoxObject.Controls.Add(this.ObjectNameTextBox);
            this.GroupBoxObject.Controls.Add(this.ObjectNameLabel);
            this.GroupBoxObject.Controls.Add(this.SchemaNameTextBox);
            this.GroupBoxObject.Controls.Add(this.SchemaNameLabel);
            this.GroupBoxObject.Controls.Add(this.CatalogNameTextBox);
            this.GroupBoxObject.Controls.Add(this.CatalogNameLabel);
            this.GroupBoxObject.Controls.Add(this.ObjectTypeTextBox);
            this.GroupBoxObject.Controls.Add(this.ObjectTypeLabel);
            this.GroupBoxObject.Location = new System.Drawing.Point(8, 58);
            this.GroupBoxObject.Name = "GroupBoxObject";
            this.GroupBoxObject.Size = new System.Drawing.Size(377, 142);
            this.GroupBoxObject.TabIndex = 14;
            this.GroupBoxObject.TabStop = false;
            this.GroupBoxObject.Text = "Objeto";
            // 
            // SchemaNameTextBox
            // 
            this.SchemaNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SchemaNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SchemaNameTextBox.Location = new System.Drawing.Point(75, 48);
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
            this.SchemaNameLabel.Location = new System.Drawing.Point(8, 50);
            this.SchemaNameLabel.Name = "SchemaNameLabel";
            this.SchemaNameLabel.Size = new System.Drawing.Size(58, 13);
            this.SchemaNameLabel.TabIndex = 4;
            this.SchemaNameLabel.Text = "Esquema";
            // 
            // CatalogNameTextBox
            // 
            this.CatalogNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CatalogNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CatalogNameTextBox.Location = new System.Drawing.Point(75, 74);
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
            this.CatalogNameLabel.Location = new System.Drawing.Point(8, 76);
            this.CatalogNameLabel.Name = "CatalogNameLabel";
            this.CatalogNameLabel.Size = new System.Drawing.Size(57, 13);
            this.CatalogNameLabel.TabIndex = 2;
            this.CatalogNameLabel.Text = "Catalogo";
            // 
            // ObjectTypeTextBox
            // 
            this.ObjectTypeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ObjectTypeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectTypeTextBox.Location = new System.Drawing.Point(75, 19);
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
            this.ObjectTypeLabel.Location = new System.Drawing.Point(7, 21);
            this.ObjectTypeLabel.Name = "ObjectTypeLabel";
            this.ObjectTypeLabel.Size = new System.Drawing.Size(32, 13);
            this.ObjectTypeLabel.TabIndex = 0;
            this.ObjectTypeLabel.Text = "Tipo";
            // 
            // ValueDataGridViewTextBoxColumn
            // 
            this.ValueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.ValueDataGridViewTextBoxColumn.HeaderText = "Valor";
            this.ValueDataGridViewTextBoxColumn.Name = "ValueDataGridViewTextBoxColumn";
            // 
            // IsNullMDataGridViewCheckBoxColumn
            // 
            this.IsNullMDataGridViewCheckBoxColumn.DataPropertyName = "IsNullM";
            this.IsNullMDataGridViewCheckBoxColumn.HeaderText = "Nulo";
            this.IsNullMDataGridViewCheckBoxColumn.Name = "IsNullMDataGridViewCheckBoxColumn";
            // 
            // ConnectionTextBox
            // 
            this.ConnectionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ConnectionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionTextBox.Location = new System.Drawing.Point(11, 17);
            this.ConnectionTextBox.Name = "ConnectionTextBox";
            this.ConnectionTextBox.ReadOnly = true;
            this.ConnectionTextBox.Size = new System.Drawing.Size(356, 20);
            this.ConnectionTextBox.TabIndex = 3;
            // 
            // ConexionGroupBox
            // 
            this.ConexionGroupBox.Controls.Add(this.ConnectionTextBox);
            this.ConexionGroupBox.Location = new System.Drawing.Point(8, 8);
            this.ConexionGroupBox.Name = "ConexionGroupBox";
            this.ConexionGroupBox.Size = new System.Drawing.Size(377, 44);
            this.ConexionGroupBox.TabIndex = 16;
            this.ConexionGroupBox.TabStop = false;
            this.ConexionGroupBox.Text = "Conección";
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.ConexionGroupBox);
            this.TopPanel.Controls.Add(this.ParametrosGroupBox);
            this.TopPanel.Controls.Add(this.GroupBoxObject);
            this.TopPanel.Controls.Add(this.EjecutarButton);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(765, 235);
            this.TopPanel.TabIndex = 16;
            // 
            // ParametrosGroupBox
            // 
            this.ParametrosGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParametrosGroupBox.Controls.Add(this.ParametrosDataGridView);
            this.ParametrosGroupBox.Location = new System.Drawing.Point(391, 8);
            this.ParametrosGroupBox.Name = "ParametrosGroupBox";
            this.ParametrosGroupBox.Size = new System.Drawing.Size(365, 192);
            this.ParametrosGroupBox.TabIndex = 15;
            this.ParametrosGroupBox.TabStop = false;
            this.ParametrosGroupBox.Text = "Parametros";
            // 
            // ParametrosDataGridView
            // 
            this.ParametrosDataGridView.AllowUserToAddRows = false;
            this.ParametrosDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParametrosDataGridView.AutoGenerateColumns = false;
            this.ParametrosDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ParametrosDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameDataGridViewTextBoxColumn,
            this.IsNullMDataGridViewCheckBoxColumn,
            this.ValueDataGridViewTextBoxColumn});
            this.ParametrosDataGridView.DataMember = "TBL_Parameter";
            this.ParametrosDataGridView.DataSource = this.DefaultDataSet;
            this.ParametrosDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ParametrosDataGridView.Location = new System.Drawing.Point(6, 19);
            this.ParametrosDataGridView.Name = "ParametrosDataGridView";
            this.ParametrosDataGridView.Size = new System.Drawing.Size(352, 163);
            this.ParametrosDataGridView.TabIndex = 0;
            // 
            // NameDataGridViewTextBoxColumn
            // 
            this.NameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.NameDataGridViewTextBoxColumn.HeaderText = "Nombre";
            this.NameDataGridViewTextBoxColumn.Name = "NameDataGridViewTextBoxColumn";
            this.NameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.BotonesPanel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 477);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(5);
            this.BottomPanel.Size = new System.Drawing.Size(765, 37);
            this.BottomPanel.TabIndex = 18;
            // 
            // FormTestObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 514);
            this.Controls.Add(this.ResultDataGridView);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.TopPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTestObject";
            this.Text = "Datos del objeto";
            this.Load += new System.EventHandler(this.FormTestObject_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DefaultDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultDataGridView)).EndInit();
            this.BotonesPanel.ResumeLayout(false);
            this.GroupBoxObject.ResumeLayout(false);
            this.GroupBoxObject.PerformLayout();
            this.ConexionGroupBox.ResumeLayout(false);
            this.ConexionGroupBox.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.ParametrosGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ParametrosDataGridView)).EndInit();
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Schemas.XsdDefault DefaultDataSet;
        internal System.Windows.Forms.DataGridView ResultDataGridView;
        internal System.Windows.Forms.TextBox ObjectNameTextBox;
        internal System.Windows.Forms.Button EjecutarButton;
        internal System.Windows.Forms.Label ObjectNameLabel;
        internal System.Windows.Forms.Button CerrarButton;
        internal System.Windows.Forms.Panel BotonesPanel;
        internal System.Windows.Forms.GroupBox GroupBoxObject;
        internal System.Windows.Forms.TextBox SchemaNameTextBox;
        internal System.Windows.Forms.Label SchemaNameLabel;
        internal System.Windows.Forms.TextBox CatalogNameTextBox;
        internal System.Windows.Forms.Label CatalogNameLabel;
        internal System.Windows.Forms.TextBox ObjectTypeTextBox;
        internal System.Windows.Forms.Label ObjectTypeLabel;
        internal System.Windows.Forms.DataGridViewTextBoxColumn ValueDataGridViewTextBoxColumn;
        internal System.Windows.Forms.DataGridViewCheckBoxColumn IsNullMDataGridViewCheckBoxColumn;
        internal System.Windows.Forms.TextBox ConnectionTextBox;
        internal System.Windows.Forms.GroupBox ConexionGroupBox;
        internal System.Windows.Forms.Panel TopPanel;
        internal System.Windows.Forms.GroupBox ParametrosGroupBox;
        internal System.Windows.Forms.DataGridView ParametrosDataGridView;
        internal System.Windows.Forms.DataGridViewTextBoxColumn NameDataGridViewTextBoxColumn;
        internal System.Windows.Forms.Panel BottomPanel;
    }
}