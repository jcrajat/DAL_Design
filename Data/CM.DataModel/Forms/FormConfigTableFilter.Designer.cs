namespace CM.DataModel.Forms
{
    partial class FormConfigTableFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigTableFilter));
            this.dtsMap = new DataModel.Schemas.XsdDefault();
            this.GrillaPanel = new System.Windows.Forms.Panel();
            this.FilterColumnsDataGridView = new System.Windows.Forms.DataGridView();
            this.DataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FilterOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpDownPanel = new System.Windows.Forms.Panel();
            this.DownRowButton = new System.Windows.Forms.Button();
            this.UpRowButton = new System.Windows.Forms.Button();
            this.FiltersListBox = new System.Windows.Forms.ListBox();
            this.AceptarButton = new System.Windows.Forms.Button();
            this.FilterDetailNamePanel = new System.Windows.Forms.Panel();
            this.FilterNameTextBox = new System.Windows.Forms.TextBox();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.FiltroDetalleGroupBox = new System.Windows.Forms.GroupBox();
            this.FilterColumnsPanel = new System.Windows.Forms.Panel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.FilterGroupBox = new System.Windows.Forms.GroupBox();
            this.DeleteFilterButton = new System.Windows.Forms.Button();
            this.AddNewFilterButton = new System.Windows.Forms.Button();
            this.ObjectNameTextBox = new System.Windows.Forms.TextBox();
            this.ObjectNameLabel = new System.Windows.Forms.Label();
            this.SchemaNameTextBox = new System.Windows.Forms.TextBox();
            this.CatalogNameTextBox = new System.Windows.Forms.TextBox();
            this.CerrarButton = new System.Windows.Forms.Button();
            this.SchemaNameLabel = new System.Windows.Forms.Label();
            this.CatalogNameLabel = new System.Windows.Forms.Label();
            this.ObjectGroupBox = new System.Windows.Forms.GroupBox();
            this.ObjectTypeTextBox = new System.Windows.Forms.TextBox();
            this.ObjectTypeLabel = new System.Windows.Forms.Label();
            this.BotonesPanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.LeftPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dtsMap)).BeginInit();
            this.GrillaPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilterColumnsDataGridView)).BeginInit();
            this.UpDownPanel.SuspendLayout();
            this.FilterDetailNamePanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.FiltroDetalleGroupBox.SuspendLayout();
            this.FilterColumnsPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.FilterGroupBox.SuspendLayout();
            this.ObjectGroupBox.SuspendLayout();
            this.BotonesPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtsMap
            // 
            this.dtsMap.DataSetName = "XsdDefault";
            this.dtsMap.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GrillaPanel
            // 
            this.GrillaPanel.Controls.Add(this.FilterColumnsDataGridView);
            this.GrillaPanel.Controls.Add(this.UpDownPanel);
            this.GrillaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrillaPanel.Location = new System.Drawing.Point(15, 43);
            this.GrillaPanel.Name = "GrillaPanel";
            this.GrillaPanel.Size = new System.Drawing.Size(535, 302);
            this.GrillaPanel.TabIndex = 2;
            // 
            // FilterColumnsDataGridView
            // 
            this.FilterColumnsDataGridView.AllowUserToAddRows = false;
            this.FilterColumnsDataGridView.AutoGenerateColumns = false;
            this.FilterColumnsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FilterColumnsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataGridViewCheckBoxColumn4,
            this.FilterOrder,
            this.DataGridViewTextBoxColumn13,
            this.DataGridViewTextBoxColumn14,
            this.DataGridViewCheckBoxColumn3,
            this.DataGridViewTextBoxColumn19});
            this.FilterColumnsDataGridView.DataMember = "TBL_Map_Filter.FK_TBL_Map_Filter_TBL_Map_Filter_Column";
            this.FilterColumnsDataGridView.DataSource = this.dtsMap;
            this.FilterColumnsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterColumnsDataGridView.Location = new System.Drawing.Point(33, 0);
            this.FilterColumnsDataGridView.Name = "FilterColumnsDataGridView";
            this.FilterColumnsDataGridView.Size = new System.Drawing.Size(502, 302);
            this.FilterColumnsDataGridView.TabIndex = 1;
            this.FilterColumnsDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.FilterColumnsDataGridView_CellEndEdit);
            // 
            // DataGridViewCheckBoxColumn4
            // 
            this.DataGridViewCheckBoxColumn4.DataPropertyName = "Selected";
            this.DataGridViewCheckBoxColumn4.Frozen = true;
            this.DataGridViewCheckBoxColumn4.HeaderText = "";
            this.DataGridViewCheckBoxColumn4.Name = "DataGridViewCheckBoxColumn4";
            this.DataGridViewCheckBoxColumn4.Width = 30;
            // 
            // FilterOrder
            // 
            this.FilterOrder.DataPropertyName = "Filter_Order";
            this.FilterOrder.HeaderText = "Orden";
            this.FilterOrder.Name = "FilterOrder";
            this.FilterOrder.ReadOnly = true;
            this.FilterOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.FilterOrder.Visible = false;
            this.FilterOrder.Width = 40;
            // 
            // DataGridViewTextBoxColumn13
            // 
            this.DataGridViewTextBoxColumn13.DataPropertyName = "Field_Name";
            this.DataGridViewTextBoxColumn13.HeaderText = "Nombre";
            this.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13";
            this.DataGridViewTextBoxColumn13.ReadOnly = true;
            this.DataGridViewTextBoxColumn13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DataGridViewTextBoxColumn14
            // 
            this.DataGridViewTextBoxColumn14.DataPropertyName = "Field_Type";
            this.DataGridViewTextBoxColumn14.HeaderText = "Tipo";
            this.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14";
            this.DataGridViewTextBoxColumn14.ReadOnly = true;
            this.DataGridViewTextBoxColumn14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DataGridViewCheckBoxColumn3
            // 
            this.DataGridViewCheckBoxColumn3.DataPropertyName = "Is_Nullable";
            this.DataGridViewCheckBoxColumn3.HeaderText = "Admite Nulo";
            this.DataGridViewCheckBoxColumn3.Name = "DataGridViewCheckBoxColumn3";
            this.DataGridViewCheckBoxColumn3.ReadOnly = true;
            // 
            // DataGridViewTextBoxColumn19
            // 
            this.DataGridViewTextBoxColumn19.DataPropertyName = "PrimaryKey_Order";
            this.DataGridViewTextBoxColumn19.HeaderText = "Key";
            this.DataGridViewTextBoxColumn19.Name = "DataGridViewTextBoxColumn19";
            this.DataGridViewTextBoxColumn19.ReadOnly = true;
            this.DataGridViewTextBoxColumn19.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UpDownPanel
            // 
            this.UpDownPanel.Controls.Add(this.DownRowButton);
            this.UpDownPanel.Controls.Add(this.UpRowButton);
            this.UpDownPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.UpDownPanel.Location = new System.Drawing.Point(0, 0);
            this.UpDownPanel.Name = "UpDownPanel";
            this.UpDownPanel.Size = new System.Drawing.Size(33, 302);
            this.UpDownPanel.TabIndex = 2;
            // 
            // DownRowButton
            // 
            this.DownRowButton.Location = new System.Drawing.Point(3, 143);
            this.DownRowButton.Name = "DownRowButton";
            this.DownRowButton.Size = new System.Drawing.Size(26, 23);
            this.DownRowButton.TabIndex = 1;
            this.DownRowButton.UseVisualStyleBackColor = true;
            this.DownRowButton.Click += new System.EventHandler(this.DownRowButton_Click);
            // 
            // UpRowButton
            // 
            this.UpRowButton.Location = new System.Drawing.Point(3, 114);
            this.UpRowButton.Name = "UpRowButton";
            this.UpRowButton.Size = new System.Drawing.Size(26, 23);
            this.UpRowButton.TabIndex = 0;
            this.UpRowButton.UseVisualStyleBackColor = true;
            this.UpRowButton.Click += new System.EventHandler(this.UpRowButton_Click);
            // 
            // FiltersListBox
            // 
            this.FiltersListBox.DataSource = this.dtsMap;
            this.FiltersListBox.DisplayMember = "TBL_Map_Filter.Name";
            this.FiltersListBox.FormattingEnabled = true;
            this.FiltersListBox.Location = new System.Drawing.Point(6, 57);
            this.FiltersListBox.Name = "FiltersListBox";
            this.FiltersListBox.Size = new System.Drawing.Size(230, 277);
            this.FiltersListBox.TabIndex = 0;
            this.FiltersListBox.ValueMember = "TBL_Map_Filter.Name";
            this.FiltersListBox.SelectedIndexChanged += new System.EventHandler(this.FiltersListBox_SelectedIndexChanged);
            // 
            // AceptarButton
            // 
            this.AceptarButton.Location = new System.Drawing.Point(146, 6);
            this.AceptarButton.Name = "AceptarButton";
            this.AceptarButton.Size = new System.Drawing.Size(94, 23);
            this.AceptarButton.TabIndex = 11;
            this.AceptarButton.Text = "Aceptar";
            this.AceptarButton.UseVisualStyleBackColor = true;
            this.AceptarButton.Click += new System.EventHandler(this.AceptarButton_Click);
            // 
            // FilterDetailNamePanel
            // 
            this.FilterDetailNamePanel.Controls.Add(this.FilterNameTextBox);
            this.FilterDetailNamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FilterDetailNamePanel.Location = new System.Drawing.Point(15, 15);
            this.FilterDetailNamePanel.Name = "FilterDetailNamePanel";
            this.FilterDetailNamePanel.Size = new System.Drawing.Size(535, 28);
            this.FilterDetailNamePanel.TabIndex = 2;
            // 
            // FilterNameTextBox
            // 
            this.FilterNameTextBox.Location = new System.Drawing.Point(33, 5);
            this.FilterNameTextBox.Name = "FilterNameTextBox";
            this.FilterNameTextBox.ReadOnly = true;
            this.FilterNameTextBox.Size = new System.Drawing.Size(495, 20);
            this.FilterNameTextBox.TabIndex = 14;
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.FiltroDetalleGroupBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(253, 75);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Padding = new System.Windows.Forms.Padding(5);
            this.MainPanel.Size = new System.Drawing.Size(581, 389);
            this.MainPanel.TabIndex = 8;
            // 
            // FiltroDetalleGroupBox
            // 
            this.FiltroDetalleGroupBox.Controls.Add(this.FilterColumnsPanel);
            this.FiltroDetalleGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FiltroDetalleGroupBox.Location = new System.Drawing.Point(5, 5);
            this.FiltroDetalleGroupBox.Name = "FiltroDetalleGroupBox";
            this.FiltroDetalleGroupBox.Size = new System.Drawing.Size(571, 379);
            this.FiltroDetalleGroupBox.TabIndex = 0;
            this.FiltroDetalleGroupBox.TabStop = false;
            this.FiltroDetalleGroupBox.Text = "Filtro";
            // 
            // FilterColumnsPanel
            // 
            this.FilterColumnsPanel.Controls.Add(this.GrillaPanel);
            this.FilterColumnsPanel.Controls.Add(this.FilterDetailNamePanel);
            this.FilterColumnsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterColumnsPanel.Location = new System.Drawing.Point(3, 16);
            this.FilterColumnsPanel.Name = "FilterColumnsPanel";
            this.FilterColumnsPanel.Padding = new System.Windows.Forms.Padding(15);
            this.FilterColumnsPanel.Size = new System.Drawing.Size(565, 360);
            this.FilterColumnsPanel.TabIndex = 3;
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.FilterGroupBox);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.RightPanel.Location = new System.Drawing.Point(0, 75);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(253, 389);
            this.RightPanel.TabIndex = 6;
            // 
            // FilterGroupBox
            // 
            this.FilterGroupBox.Controls.Add(this.DeleteFilterButton);
            this.FilterGroupBox.Controls.Add(this.AddNewFilterButton);
            this.FilterGroupBox.Controls.Add(this.FiltersListBox);
            this.FilterGroupBox.Location = new System.Drawing.Point(5, 6);
            this.FilterGroupBox.Name = "FilterGroupBox";
            this.FilterGroupBox.Size = new System.Drawing.Size(242, 377);
            this.FilterGroupBox.TabIndex = 1;
            this.FilterGroupBox.TabStop = false;
            this.FilterGroupBox.Text = "Filtros";
            // 
            // DeleteFilterButton
            // 
            this.DeleteFilterButton.Location = new System.Drawing.Point(10, 348);
            this.DeleteFilterButton.Name = "DeleteFilterButton";
            this.DeleteFilterButton.Size = new System.Drawing.Size(89, 23);
            this.DeleteFilterButton.TabIndex = 13;
            this.DeleteFilterButton.Text = "Eliminar";
            this.DeleteFilterButton.UseVisualStyleBackColor = true;
            this.DeleteFilterButton.Click += new System.EventHandler(this.DeleteFilterButton_Click);
            // 
            // AddNewFilterButton
            // 
            this.AddNewFilterButton.Location = new System.Drawing.Point(10, 19);
            this.AddNewFilterButton.Name = "AddNewFilterButton";
            this.AddNewFilterButton.Size = new System.Drawing.Size(138, 23);
            this.AddNewFilterButton.TabIndex = 2;
            this.AddNewFilterButton.Text = "Agregar filtro";
            this.AddNewFilterButton.UseVisualStyleBackColor = true;
            this.AddNewFilterButton.Click += new System.EventHandler(this.AddNewFilterButton_Click);
            // 
            // ObjectNameTextBox
            // 
            this.ObjectNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ObjectNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectNameTextBox.Location = new System.Drawing.Point(455, 43);
            this.ObjectNameTextBox.Name = "ObjectNameTextBox";
            this.ObjectNameTextBox.ReadOnly = true;
            this.ObjectNameTextBox.Size = new System.Drawing.Size(290, 20);
            this.ObjectNameTextBox.TabIndex = 7;
            this.ObjectNameTextBox.Text = "Nombre";
            // 
            // ObjectNameLabel
            // 
            this.ObjectNameLabel.AutoSize = true;
            this.ObjectNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectNameLabel.Location = new System.Drawing.Point(393, 43);
            this.ObjectNameLabel.Name = "ObjectNameLabel";
            this.ObjectNameLabel.Size = new System.Drawing.Size(50, 13);
            this.ObjectNameLabel.TabIndex = 6;
            this.ObjectNameLabel.Text = "Nombre";
            // 
            // SchemaNameTextBox
            // 
            this.SchemaNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SchemaNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SchemaNameTextBox.Location = new System.Drawing.Point(75, 43);
            this.SchemaNameTextBox.Name = "SchemaNameTextBox";
            this.SchemaNameTextBox.ReadOnly = true;
            this.SchemaNameTextBox.Size = new System.Drawing.Size(290, 20);
            this.SchemaNameTextBox.TabIndex = 5;
            this.SchemaNameTextBox.Text = "Esquema";
            // 
            // CatalogNameTextBox
            // 
            this.CatalogNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CatalogNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CatalogNameTextBox.Location = new System.Drawing.Point(455, 19);
            this.CatalogNameTextBox.Name = "CatalogNameTextBox";
            this.CatalogNameTextBox.ReadOnly = true;
            this.CatalogNameTextBox.Size = new System.Drawing.Size(290, 20);
            this.CatalogNameTextBox.TabIndex = 3;
            this.CatalogNameTextBox.Text = "Nombre";
            // 
            // CerrarButton
            // 
            this.CerrarButton.Location = new System.Drawing.Point(24, 6);
            this.CerrarButton.Name = "CerrarButton";
            this.CerrarButton.Size = new System.Drawing.Size(89, 23);
            this.CerrarButton.TabIndex = 12;
            this.CerrarButton.Text = "Cancel";
            this.CerrarButton.UseVisualStyleBackColor = true;
            this.CerrarButton.Click += new System.EventHandler(this.CerrarButton_Click);
            // 
            // SchemaNameLabel
            // 
            this.SchemaNameLabel.AutoSize = true;
            this.SchemaNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SchemaNameLabel.Location = new System.Drawing.Point(7, 43);
            this.SchemaNameLabel.Name = "SchemaNameLabel";
            this.SchemaNameLabel.Size = new System.Drawing.Size(58, 13);
            this.SchemaNameLabel.TabIndex = 4;
            this.SchemaNameLabel.Text = "Esquema";
            // 
            // CatalogNameLabel
            // 
            this.CatalogNameLabel.AutoSize = true;
            this.CatalogNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CatalogNameLabel.Location = new System.Drawing.Point(393, 19);
            this.CatalogNameLabel.Name = "CatalogNameLabel";
            this.CatalogNameLabel.Size = new System.Drawing.Size(57, 13);
            this.CatalogNameLabel.TabIndex = 2;
            this.CatalogNameLabel.Text = "Catalogo";
            // 
            // ObjectGroupBox
            // 
            this.ObjectGroupBox.Controls.Add(this.ObjectNameTextBox);
            this.ObjectGroupBox.Controls.Add(this.ObjectNameLabel);
            this.ObjectGroupBox.Controls.Add(this.SchemaNameTextBox);
            this.ObjectGroupBox.Controls.Add(this.SchemaNameLabel);
            this.ObjectGroupBox.Controls.Add(this.CatalogNameTextBox);
            this.ObjectGroupBox.Controls.Add(this.CatalogNameLabel);
            this.ObjectGroupBox.Controls.Add(this.ObjectTypeTextBox);
            this.ObjectGroupBox.Controls.Add(this.ObjectTypeLabel);
            this.ObjectGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectGroupBox.Location = new System.Drawing.Point(5, 5);
            this.ObjectGroupBox.Name = "ObjectGroupBox";
            this.ObjectGroupBox.Size = new System.Drawing.Size(824, 65);
            this.ObjectGroupBox.TabIndex = 0;
            this.ObjectGroupBox.TabStop = false;
            this.ObjectGroupBox.Text = "Objeto";
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
            this.ObjectTypeLabel.Location = new System.Drawing.Point(7, 19);
            this.ObjectTypeLabel.Name = "ObjectTypeLabel";
            this.ObjectTypeLabel.Size = new System.Drawing.Size(32, 13);
            this.ObjectTypeLabel.TabIndex = 0;
            this.ObjectTypeLabel.Text = "Tipo";
            // 
            // BotonesPanel
            // 
            this.BotonesPanel.Controls.Add(this.AceptarButton);
            this.BotonesPanel.Controls.Add(this.CerrarButton);
            this.BotonesPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BotonesPanel.Location = new System.Drawing.Point(575, 0);
            this.BotonesPanel.Name = "BotonesPanel";
            this.BotonesPanel.Size = new System.Drawing.Size(259, 38);
            this.BotonesPanel.TabIndex = 13;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.BotonesPanel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 464);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.BottomPanel.Size = new System.Drawing.Size(834, 40);
            this.BottomPanel.TabIndex = 7;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.ObjectGroupBox);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Padding = new System.Windows.Forms.Padding(5);
            this.LeftPanel.Size = new System.Drawing.Size(834, 75);
            this.LeftPanel.TabIndex = 5;
            // 
            // FormConfigTableFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 504);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.LeftPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigTableFilter";
            this.Text = "Configuración de filtros especializados";
            this.Load += new System.EventHandler(this.FormConfigObjectFilter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtsMap)).EndInit();
            this.GrillaPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FilterColumnsDataGridView)).EndInit();
            this.UpDownPanel.ResumeLayout(false);
            this.FilterDetailNamePanel.ResumeLayout(false);
            this.FilterDetailNamePanel.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.FiltroDetalleGroupBox.ResumeLayout(false);
            this.FilterColumnsPanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.FilterGroupBox.ResumeLayout(false);
            this.ObjectGroupBox.ResumeLayout(false);
            this.ObjectGroupBox.PerformLayout();
            this.BotonesPanel.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Schemas.XsdDefault dtsMap;
        internal System.Windows.Forms.Panel GrillaPanel;
        internal System.Windows.Forms.DataGridView FilterColumnsDataGridView;
        internal System.Windows.Forms.DataGridViewCheckBoxColumn DataGridViewCheckBoxColumn4;
        internal System.Windows.Forms.DataGridViewTextBoxColumn FilterOrder;
        internal System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumn13;
        internal System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumn14;
        internal System.Windows.Forms.DataGridViewCheckBoxColumn DataGridViewCheckBoxColumn3;
        internal System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewTextBoxColumn19;
        internal System.Windows.Forms.Panel UpDownPanel;
        internal System.Windows.Forms.Button DownRowButton;
        internal System.Windows.Forms.Button UpRowButton;
        internal System.Windows.Forms.ListBox FiltersListBox;
        internal System.Windows.Forms.Button AceptarButton;
        internal System.Windows.Forms.Panel FilterDetailNamePanel;
        internal System.Windows.Forms.TextBox FilterNameTextBox;
        internal System.Windows.Forms.Panel MainPanel;
        internal System.Windows.Forms.GroupBox FiltroDetalleGroupBox;
        internal System.Windows.Forms.Panel FilterColumnsPanel;
        internal System.Windows.Forms.Panel RightPanel;
        internal System.Windows.Forms.GroupBox FilterGroupBox;
        internal System.Windows.Forms.Button DeleteFilterButton;
        internal System.Windows.Forms.Button AddNewFilterButton;
        internal System.Windows.Forms.TextBox ObjectNameTextBox;
        internal System.Windows.Forms.Label ObjectNameLabel;
        internal System.Windows.Forms.TextBox SchemaNameTextBox;
        internal System.Windows.Forms.TextBox CatalogNameTextBox;
        internal System.Windows.Forms.Button CerrarButton;
        internal System.Windows.Forms.Label SchemaNameLabel;
        internal System.Windows.Forms.Label CatalogNameLabel;
        internal System.Windows.Forms.GroupBox ObjectGroupBox;
        internal System.Windows.Forms.TextBox ObjectTypeTextBox;
        internal System.Windows.Forms.Label ObjectTypeLabel;
        internal System.Windows.Forms.Panel BotonesPanel;
        internal System.Windows.Forms.Panel BottomPanel;
        internal System.Windows.Forms.Panel LeftPanel;
    }
}