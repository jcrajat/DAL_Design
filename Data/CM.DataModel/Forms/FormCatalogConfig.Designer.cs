namespace CM.DataModel.Forms
{
    partial class FormCatalogConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCatalogConfig));
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.BotonesPanel = new System.Windows.Forms.Panel();
            this.AceptarButton = new System.Windows.Forms.Button();
            this.CancelarButton = new System.Windows.Forms.Button();
            this.CatalogDataGridView = new System.Windows.Forms.DataGridView();
            this.DataBaseDataSet = new CMData.Schemas.XsdDataBase();
            this.catalogNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BottomPanel.SuspendLayout();
            this.BotonesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CatalogDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataBaseDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.BotonesPanel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(10, 215);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.BottomPanel.Size = new System.Drawing.Size(567, 42);
            this.BottomPanel.TabIndex = 11;
            // 
            // BotonesPanel
            // 
            this.BotonesPanel.Controls.Add(this.AceptarButton);
            this.BotonesPanel.Controls.Add(this.CancelarButton);
            this.BotonesPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BotonesPanel.Location = new System.Drawing.Point(308, 0);
            this.BotonesPanel.Name = "BotonesPanel";
            this.BotonesPanel.Size = new System.Drawing.Size(259, 40);
            this.BotonesPanel.TabIndex = 13;
            // 
            // AceptarButton
            // 
            this.AceptarButton.Location = new System.Drawing.Point(162, 14);
            this.AceptarButton.Name = "AceptarButton";
            this.AceptarButton.Size = new System.Drawing.Size(94, 23);
            this.AceptarButton.TabIndex = 11;
            this.AceptarButton.Text = "Aceptar";
            this.AceptarButton.UseVisualStyleBackColor = true;
            this.AceptarButton.Click += new System.EventHandler(this.AceptarButton_Click);
            // 
            // CancelarButton
            // 
            this.CancelarButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelarButton.Location = new System.Drawing.Point(54, 14);
            this.CancelarButton.Name = "CancelarButton";
            this.CancelarButton.Size = new System.Drawing.Size(89, 23);
            this.CancelarButton.TabIndex = 12;
            this.CancelarButton.Text = "Cancelar";
            this.CancelarButton.UseVisualStyleBackColor = true;
            this.CancelarButton.Click += new System.EventHandler(this.CancelarButton_Click);
            // 
            // CatalogDataGridView
            // 
            this.CatalogDataGridView.AllowUserToAddRows = false;
            this.CatalogDataGridView.AllowUserToDeleteRows = false;
            this.CatalogDataGridView.AutoGenerateColumns = false;
            this.CatalogDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CatalogDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.catalogNameDataGridViewTextBoxColumn,
            this.classNameDataGridViewTextBoxColumn,
            this.classFileNameDataGridViewTextBoxColumn});
            this.CatalogDataGridView.DataMember = "TBL_Catalog";
            this.CatalogDataGridView.DataSource = this.DataBaseDataSet;
            this.CatalogDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CatalogDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.CatalogDataGridView.Location = new System.Drawing.Point(10, 10);
            this.CatalogDataGridView.Name = "CatalogDataGridView";
            this.CatalogDataGridView.Size = new System.Drawing.Size(567, 205);
            this.CatalogDataGridView.TabIndex = 12;
            // 
            // DataBaseDataSet
            // 
            this.DataBaseDataSet.DataSetName = "XsdDataBase";
            this.DataBaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // catalogNameDataGridViewTextBoxColumn
            // 
            this.catalogNameDataGridViewTextBoxColumn.DataPropertyName = "Catalog_Name";
            this.catalogNameDataGridViewTextBoxColumn.HeaderText = "Catálogo";
            this.catalogNameDataGridViewTextBoxColumn.Name = "catalogNameDataGridViewTextBoxColumn";
            this.catalogNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // classNameDataGridViewTextBoxColumn
            // 
            this.classNameDataGridViewTextBoxColumn.DataPropertyName = "Class_Name";
            this.classNameDataGridViewTextBoxColumn.HeaderText = "Clase";
            this.classNameDataGridViewTextBoxColumn.Name = "classNameDataGridViewTextBoxColumn";
            this.classNameDataGridViewTextBoxColumn.ToolTipText = "Nombre de la clase a generar";
            this.classNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // classFileNameDataGridViewTextBoxColumn
            // 
            this.classFileNameDataGridViewTextBoxColumn.DataPropertyName = "Class_File_Name";
            this.classFileNameDataGridViewTextBoxColumn.HeaderText = "Archivo";
            this.classFileNameDataGridViewTextBoxColumn.Name = "classFileNameDataGridViewTextBoxColumn";
            this.classFileNameDataGridViewTextBoxColumn.ToolTipText = "Nombre del archivo a generar";
            this.classFileNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // FormCatalogConfig
            // 
            this.AcceptButton = this.AceptarButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelarButton;
            this.ClientSize = new System.Drawing.Size(587, 262);
            this.Controls.Add(this.CatalogDataGridView);
            this.Controls.Add(this.BottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCatalogConfig";
            this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuración de catálogos";
            this.BottomPanel.ResumeLayout(false);
            this.BotonesPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CatalogDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataBaseDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel BottomPanel;
        internal System.Windows.Forms.Panel BotonesPanel;
        internal System.Windows.Forms.Button AceptarButton;
        internal System.Windows.Forms.Button CancelarButton;
        private System.Windows.Forms.DataGridView CatalogDataGridView;
        private CMData.Schemas.XsdDataBase DataBaseDataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn catalogNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn classNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn classFileNameDataGridViewTextBoxColumn;
    }
}