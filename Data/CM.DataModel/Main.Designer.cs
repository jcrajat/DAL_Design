namespace CM.DataModel
{
    partial class Main
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.RefreshtoolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.RefreshOnlyNewsToolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.GenerateCodeToolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.SaveToolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.AddConnectionToolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.NewToolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.OpenToolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.CatalogToolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.SchemaToolStripButton = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageCategory1 = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.Archivo = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.backstageViewTabItem1 = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.MyBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MyProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.AccionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barSubItem1,
            this.RefreshtoolStripButton,
            this.RefreshOnlyNewsToolStripButton,
            this.GenerateCodeToolStripButton,
            this.SaveToolStripButton,
            this.AddConnectionToolStripButton,
            this.NewToolStripButton,
            this.OpenToolStripButton,
            this.CatalogToolStripButton,
            this.SchemaToolStripButton});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 7;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.PageCategories.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageCategory[] {
            this.ribbonPageCategory1});
            this.ribbonControl1.PageHeaderItemLinks.Add(this.RefreshtoolStripButton);
            this.ribbonControl1.PageHeaderItemLinks.Add(this.RefreshOnlyNewsToolStripButton);
            this.ribbonControl1.PageHeaderItemLinks.Add(this.GenerateCodeToolStripButton);
            this.ribbonControl1.PageHeaderItemLinks.Add(this.SaveToolStripButton);
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.MacOffice;
            this.ribbonControl1.Size = new System.Drawing.Size(992, 131);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.RefreshOnlyNewsToolStripButton);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.RefreshtoolStripButton);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.SaveToolStripButton);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.GenerateCodeToolStripButton);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "barSubItem1";
            this.barSubItem1.Id = 1;
            this.barSubItem1.Name = "barSubItem1";
            // 
            // RefreshtoolStripButton
            // 
            this.RefreshtoolStripButton.Caption = "Actualizar Estructura";
            this.RefreshtoolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("RefreshtoolStripButton.Glyph")));
            this.RefreshtoolStripButton.Id = 5;
            this.RefreshtoolStripButton.Name = "RefreshtoolStripButton";
            this.RefreshtoolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshtoolStripButton_ItemClick);
            // 
            // RefreshOnlyNewsToolStripButton
            // 
            this.RefreshOnlyNewsToolStripButton.Caption = "Actualizar Nuevos Items";
            this.RefreshOnlyNewsToolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("RefreshOnlyNewsToolStripButton.Glyph")));
            this.RefreshOnlyNewsToolStripButton.Id = 6;
            this.RefreshOnlyNewsToolStripButton.Name = "RefreshOnlyNewsToolStripButton";
            this.RefreshOnlyNewsToolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshOnlyNewsToolStripButton_ItemClick);
            // 
            // GenerateCodeToolStripButton
            // 
            this.GenerateCodeToolStripButton.Caption = "Generar Códido DAL";
            this.GenerateCodeToolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("GenerateCodeToolStripButton.Glyph")));
            this.GenerateCodeToolStripButton.Id = 7;
            this.GenerateCodeToolStripButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("GenerateCodeToolStripButton.LargeGlyph")));
            this.GenerateCodeToolStripButton.Name = "GenerateCodeToolStripButton";
            this.GenerateCodeToolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.GenerateCodeToolStripButton_ItemClick);
            // 
            // SaveToolStripButton
            // 
            this.SaveToolStripButton.Caption = "Guardar";
            this.SaveToolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("SaveToolStripButton.Glyph")));
            this.SaveToolStripButton.Id = 11;
            this.SaveToolStripButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("SaveToolStripButton.LargeGlyph")));
            this.SaveToolStripButton.Name = "SaveToolStripButton";
            this.SaveToolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SaveToolStripButton_ItemClick);
            // 
            // AddConnectionToolStripButton
            // 
            this.AddConnectionToolStripButton.Caption = "Agregar Conexión";
            this.AddConnectionToolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("AddConnectionToolStripButton.Glyph")));
            this.AddConnectionToolStripButton.Id = 1;
            this.AddConnectionToolStripButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("AddConnectionToolStripButton.LargeGlyph")));
            this.AddConnectionToolStripButton.Name = "AddConnectionToolStripButton";
            this.AddConnectionToolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AddConnectionToolStripButton_ItemClick);
            // 
            // NewToolStripButton
            // 
            this.NewToolStripButton.Caption = "Nuevo";
            this.NewToolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("NewToolStripButton.Glyph")));
            this.NewToolStripButton.Id = 2;
            this.NewToolStripButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("NewToolStripButton.LargeGlyph")));
            this.NewToolStripButton.Name = "NewToolStripButton";
            this.NewToolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NewToolStripButton_ItemClick);
            // 
            // OpenToolStripButton
            // 
            this.OpenToolStripButton.Caption = "Abrir";
            this.OpenToolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("OpenToolStripButton.Glyph")));
            this.OpenToolStripButton.Id = 3;
            this.OpenToolStripButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("OpenToolStripButton.LargeGlyph")));
            this.OpenToolStripButton.Name = "OpenToolStripButton";
            this.OpenToolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.OpenToolStripButton_ItemClick_1);
            // 
            // CatalogToolStripButton
            // 
            this.CatalogToolStripButton.Caption = "Catalogos";
            this.CatalogToolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("CatalogToolStripButton.Glyph")));
            this.CatalogToolStripButton.Id = 4;
            this.CatalogToolStripButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("CatalogToolStripButton.LargeGlyph")));
            this.CatalogToolStripButton.Name = "CatalogToolStripButton";
            this.CatalogToolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CatalogToolStripButton_ItemClick_1);
            // 
            // SchemaToolStripButton
            // 
            this.SchemaToolStripButton.Caption = "Esquemas";
            this.SchemaToolStripButton.Glyph = ((System.Drawing.Image)(resources.GetObject("SchemaToolStripButton.Glyph")));
            this.SchemaToolStripButton.Id = 5;
            this.SchemaToolStripButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("SchemaToolStripButton.LargeGlyph")));
            this.SchemaToolStripButton.Name = "SchemaToolStripButton";
            this.SchemaToolStripButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SchemaToolStripButton_ItemClick_1);
            // 
            // ribbonPageCategory1
            // 
            this.ribbonPageCategory1.Color = System.Drawing.Color.Empty;
            this.ribbonPageCategory1.Name = "ribbonPageCategory1";
            this.ribbonPageCategory1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage2});
            this.ribbonPageCategory1.Text = "ribbonPageCategory1";
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.Archivo,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup6,
            this.ribbonPageGroup4,
            this.ribbonPageGroup5});
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "Configuración";
            // 
            // Archivo
            // 
            this.Archivo.ItemLinks.Add(this.NewToolStripButton);
            this.Archivo.ItemLinks.Add(this.OpenToolStripButton);
            this.Archivo.Name = "Archivo";
            this.Archivo.Text = "Archivo";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.AddConnectionToolStripButton);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Conexion";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.RefreshtoolStripButton);
            this.ribbonPageGroup3.ItemLinks.Add(this.RefreshOnlyNewsToolStripButton);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Actualizar";
            // 
            // ribbonPageGroup6
            // 
            this.ribbonPageGroup6.ItemLinks.Add(this.SaveToolStripButton);
            this.ribbonPageGroup6.Name = "ribbonPageGroup6";
            this.ribbonPageGroup6.Text = "Guardar";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.GenerateCodeToolStripButton);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "Generar";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.CatalogToolStripButton);
            this.ribbonPageGroup5.ItemLinks.Add(this.SchemaToolStripButton);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.Text = "Configuracion";
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(0, 131);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 382);
            this.splitterControl1.TabIndex = 2;
            this.splitterControl1.TabStop = false;
            // 
            // backstageViewTabItem1
            // 
            this.backstageViewTabItem1.Caption = "backstageViewTabItem1";
            this.backstageViewTabItem1.Name = "backstageViewTabItem1";
            this.backstageViewTabItem1.Selected = false;
            // 
            // MyBackgroundWorker
            // 
            this.MyBackgroundWorker.WorkerReportsProgress = true;
            this.MyBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MyBackgroundWorker_DoWork);
            this.MyBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.MyBackgroundWorker_ProgressChanged);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Location = new System.Drawing.Point(5, 491);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(987, 22);
            this.StatusStrip.TabIndex = 11;
            this.StatusStrip.Text = "StatusStrip";
            // 
            // ToolStripStatusLabel
            // 
            this.ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            this.ToolStripStatusLabel.Size = new System.Drawing.Size(42, 17);
            this.ToolStripStatusLabel.Text = "Estado";
            // 
            // MyProgressBar
            // 
            this.MyProgressBar.MarqueeAnimationSpeed = 2000;
            this.MyProgressBar.Name = "MyProgressBar";
            this.MyProgressBar.Size = new System.Drawing.Size(150, 16);
            // 
            // AccionToolStripStatusLabel
            // 
            this.AccionToolStripStatusLabel.Name = "AccionToolStripStatusLabel";
            this.AccionToolStripStatusLabel.Size = new System.Drawing.Size(44, 17);
            this.AccionToolStripStatusLabel.Text = "Accion";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 513);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.ribbonControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "Main";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cargo Master Designer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMDI_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMDI_DragEnter);
            this.DragLeave += new System.EventHandler(this.FormMDI_DragLeave);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageCategory ribbonPageCategory1;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem backstageViewTabItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem RefreshtoolStripButton;
        private DevExpress.XtraBars.BarButtonItem RefreshOnlyNewsToolStripButton;
        private DevExpress.XtraBars.BarButtonItem GenerateCodeToolStripButton;
        private DevExpress.XtraBars.BarButtonItem SaveToolStripButton;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        internal System.ComponentModel.BackgroundWorker MyBackgroundWorker;
        internal System.Windows.Forms.StatusStrip StatusStrip;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        internal System.Windows.Forms.ToolStripProgressBar MyProgressBar;
        internal System.Windows.Forms.ToolStripStatusLabel AccionToolStripStatusLabel;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup Archivo;
        private DevExpress.XtraBars.BarButtonItem AddConnectionToolStripButton;
        private DevExpress.XtraBars.BarButtonItem NewToolStripButton;
        private DevExpress.XtraBars.BarButtonItem OpenToolStripButton;
        private DevExpress.XtraBars.BarButtonItem CatalogToolStripButton;
        private DevExpress.XtraBars.BarButtonItem SchemaToolStripButton;
    }
}

