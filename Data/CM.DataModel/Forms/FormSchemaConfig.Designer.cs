namespace CM.DataModel.Forms
{
    partial class FormSchemaConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSchemaConfig));
            this.SchemaDataGridView = new System.Windows.Forms.DataGridView();
            this.DataBaseDataSet = new CMData.Schemas.XsdDataBase();
            this.CancelarButton = new System.Windows.Forms.Button();
            this.BotonesPanel = new System.Windows.Forms.Panel();
            this.AceptarButton = new System.Windows.Forms.Button();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.schemaNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.schemaAliasDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Maping_Schema = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.SchemaDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataBaseDataSet)).BeginInit();
            this.BotonesPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // SchemaDataGridView
            // 
            this.SchemaDataGridView.AllowUserToAddRows = false;
            this.SchemaDataGridView.AllowUserToDeleteRows = false;
            this.SchemaDataGridView.AutoGenerateColumns = false;
            this.SchemaDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SchemaDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.schemaNameDataGridViewTextBoxColumn,
            this.schemaAliasDataGridViewTextBoxColumn,
            this.Maping_Schema});
            this.SchemaDataGridView.DataMember = "TBL_Schema";
            this.SchemaDataGridView.DataSource = this.DataBaseDataSet;
            this.SchemaDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SchemaDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.SchemaDataGridView.Location = new System.Drawing.Point(10, 10);
            this.SchemaDataGridView.Name = "SchemaDataGridView";
            this.SchemaDataGridView.Size = new System.Drawing.Size(446, 205);
            this.SchemaDataGridView.TabIndex = 14;
            // 
            // DataBaseDataSet
            // 
            this.DataBaseDataSet.DataSetName = "XsdDataBase";
            this.DataBaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // BotonesPanel
            // 
            this.BotonesPanel.Controls.Add(this.AceptarButton);
            this.BotonesPanel.Controls.Add(this.CancelarButton);
            this.BotonesPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BotonesPanel.Location = new System.Drawing.Point(187, 0);
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
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.BotonesPanel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(10, 215);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.BottomPanel.Size = new System.Drawing.Size(446, 42);
            this.BottomPanel.TabIndex = 13;
            // 
            // schemaNameDataGridViewTextBoxColumn
            // 
            this.schemaNameDataGridViewTextBoxColumn.DataPropertyName = "Schema_Name";
            this.schemaNameDataGridViewTextBoxColumn.HeaderText = "Esquema";
            this.schemaNameDataGridViewTextBoxColumn.Name = "schemaNameDataGridViewTextBoxColumn";
            this.schemaNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.schemaNameDataGridViewTextBoxColumn.ToolTipText = "Nombre del esquema";
            this.schemaNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // schemaAliasDataGridViewTextBoxColumn
            // 
            this.schemaAliasDataGridViewTextBoxColumn.DataPropertyName = "Schema_Alias";
            this.schemaAliasDataGridViewTextBoxColumn.HeaderText = "Alias";
            this.schemaAliasDataGridViewTextBoxColumn.Name = "schemaAliasDataGridViewTextBoxColumn";
            this.schemaAliasDataGridViewTextBoxColumn.Width = 200;
            // 
            // Maping_Schema
            // 
            this.Maping_Schema.DataPropertyName = "Maping_Schema";
            this.Maping_Schema.HeaderText = "Usar";
            this.Maping_Schema.Name = "Maping_Schema";
            this.Maping_Schema.Width = 40;
            // 
            // FormSchemaConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 262);
            this.Controls.Add(this.SchemaDataGridView);
            this.Controls.Add(this.BottomPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSchemaConfig";
            this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuración de esquemas";
            ((System.ComponentModel.ISupportInitialize)(this.SchemaDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataBaseDataSet)).EndInit();
            this.BotonesPanel.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView SchemaDataGridView;
        private CMData.Schemas.XsdDataBase DataBaseDataSet;
        internal System.Windows.Forms.Button CancelarButton;
        internal System.Windows.Forms.Panel BotonesPanel;
        internal System.Windows.Forms.Button AceptarButton;
        internal System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn schemaNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn schemaAliasDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Maping_Schema;
    }
}