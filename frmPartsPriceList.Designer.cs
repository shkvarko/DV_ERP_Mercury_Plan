namespace ERPMercuryPlan
{
    partial class frmPartsPriceList
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPartsPriceList));
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.tabView = new DevExpress.XtraTab.XtraTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnExportForCalcPrice = new DevExpress.XtraEditors.SimpleButton();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControlPriceList = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mitemImportPrices = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewPriceList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.tableLayoutPanelDetail = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.btnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.lblCustomerIfo = new DevExpress.XtraEditors.LabelControl();
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.treeListPriceEditor = new DevExpress.XtraTreeList.TreeList();
            this.colPriceName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colPriceValue = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemCalcEditPrice = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.repItemMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtSubType = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.treeListPrices = new DevExpress.XtraTreeList.TreeList();
            this.colPriceListPriceType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colPriceListPriceValue = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.contextMenuStripPrintPrices = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mitmsSelectAllProductOwner = new System.Windows.Forms.ToolStripMenuItem();
            this.mitmsDeSelectAllProductOwner = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlExportToIB = new DevExpress.XtraTab.XtraTabControl();
            this.tabPagePriceProperties = new DevExpress.XtraTab.XtraTabPage();
            this.tabDetail = new DevExpress.XtraTab.XtraTabPage();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabView.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPriceList)).BeginInit();
            this.tableLayoutPanelDetail.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.xtraScrollableControl1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListPriceEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCalcEditPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubType.Properties)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            this.contextMenuStripPrintPrices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlExportToIB)).BeginInit();
            this.tabControlExportToIB.SuspendLayout();
            this.tabPagePriceProperties.SuspendLayout();
            this.tabDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabPage = this.tabView;
            this.tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.tabControl.Size = new System.Drawing.Size(921, 556);
            this.tabControl.TabIndex = 1;
            this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabView,
            this.tabDetail});
            this.tabControl.Text = "xtraTabControl1";
            // 
            // tabView
            // 
            this.tabView.Controls.Add(this.tableLayoutPanel1);
            this.tabView.Name = "tabView";
            this.tabView.Size = new System.Drawing.Size(912, 547);
            this.tabView.Text = "xtraTabPage1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainerControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(912, 547);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 6;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.btnRefresh, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnDelete, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnAdd, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnExportForCalcPrice, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(912, 27);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel4, null);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(1, 3);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(21, 21);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.ToolTip = "Обновить список";
            this.btnRefresh.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDelete.Image = global::ERPMercuryPlan.Properties.Resources.delete2;
            this.btnDelete.Location = new System.Drawing.Point(49, 3);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(21, 21);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.ToolTip = "Удалить записи из прайса";
            this.btnDelete.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.Image = global::ERPMercuryPlan.Properties.Resources.add2;
            this.btnAdd.Location = new System.Drawing.Point(73, 3);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(21, 21);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.ToolTip = "Добавить записи в прайс лист";
            this.btnAdd.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnExportForCalcPrice
            // 
            this.btnExportForCalcPrice.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnExportForCalcPrice.Image = global::ERPMercuryPlan.Properties.Resources.printer2;
            this.btnExportForCalcPrice.Location = new System.Drawing.Point(25, 3);
            this.btnExportForCalcPrice.Margin = new System.Windows.Forms.Padding(1);
            this.btnExportForCalcPrice.Name = "btnExportForCalcPrice";
            this.btnExportForCalcPrice.Size = new System.Drawing.Size(21, 21);
            this.btnExportForCalcPrice.TabIndex = 12;
            this.btnExportForCalcPrice.ToolTip = "Экспорт прайс-листа в MS Excel";
            this.btnExportForCalcPrice.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnExportForCalcPrice.Click += new System.EventHandler(this.btnExportForCalcPrice_Click);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(3, 30);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControlPriceList);
            this.splitContainerControl1.Panel1.MinSize = 200;
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.tabControlExportToIB);
            this.splitContainerControl1.Panel2.MinSize = 200;
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(906, 514);
            this.splitContainerControl1.SplitterPosition = 584;
            this.toolTipController.SetSuperTip(this.splitContainerControl1, null);
            this.splitContainerControl1.TabIndex = 3;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridControlPriceList
            // 
            this.gridControlPriceList.ContextMenuStrip = this.contextMenuStrip;
            this.gridControlPriceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlPriceList.EmbeddedNavigator.Name = "";
            this.gridControlPriceList.Location = new System.Drawing.Point(0, 0);
            this.gridControlPriceList.MainView = this.gridViewPriceList;
            this.gridControlPriceList.Name = "gridControlPriceList";
            this.gridControlPriceList.Size = new System.Drawing.Size(580, 510);
            this.gridControlPriceList.TabIndex = 4;
            this.gridControlPriceList.ToolTipController = this.toolTipController;
            this.gridControlPriceList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPriceList});
            this.gridControlPriceList.DoubleClick += new System.EventHandler(this.gridControlPriceList_DoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitemImportPrices});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(201, 26);
            this.toolTipController.SetSuperTip(this.contextMenuStrip, null);
            // 
            // mitemImportPrices
            // 
            this.mitemImportPrices.Name = "mitemImportPrices";
            this.mitemImportPrices.Size = new System.Drawing.Size(200, 22);
            this.mitemImportPrices.Text = "Импорт цен в прайс-лист";
            this.mitemImportPrices.Click += new System.EventHandler(this.mitemImportPrices_Click);
            // 
            // gridViewPriceList
            // 
            this.gridViewPriceList.GridControl = this.gridControlPriceList;
            this.gridViewPriceList.Name = "gridViewPriceList";
            this.gridViewPriceList.OptionsBehavior.Editable = false;
            this.gridViewPriceList.OptionsSelection.MultiSelect = true;
            this.gridViewPriceList.OptionsView.ColumnAutoWidth = false;
            this.gridViewPriceList.OptionsView.ShowDetailButtons = false;
            this.gridViewPriceList.OptionsView.ShowGroupPanel = false;
            this.gridViewPriceList.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewPriceList_FocusedRowChanged);
            this.gridViewPriceList.RowCountChanged += new System.EventHandler(this.gridViewPriceList_RowCountChanged);
            // 
            // tableLayoutPanelDetail
            // 
            this.tableLayoutPanelDetail.ColumnCount = 1;
            this.tableLayoutPanelDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDetail.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanelDetail.Controls.Add(this.lblCustomerIfo, 0, 0);
            this.tableLayoutPanelDetail.Controls.Add(this.xtraScrollableControl1, 0, 1);
            this.tableLayoutPanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDetail.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDetail.Name = "tableLayoutPanelDetail";
            this.tableLayoutPanelDetail.RowCount = 3;
            this.tableLayoutPanelDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDetail.Size = new System.Drawing.Size(912, 547);
            this.toolTipController.SetSuperTip(this.tableLayoutPanelDetail, null);
            this.tableLayoutPanelDetail.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnPrint, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnEdit, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 517);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(912, 30);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel2, null);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(834, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.ToolTip = "Отменить изменения";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(752, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "ОК";
            this.btnSave.ToolTip = "Сохранить изменения";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.Location = new System.Drawing.Point(220, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(23, 23);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Visible = false;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.Image = global::ERPMercuryPlan.Properties.Resources.document_edit;
            this.btnEdit.Location = new System.Drawing.Point(3, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(112, 23);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.ToolTip = "Разрешить редактирование";
            this.btnEdit.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // lblCustomerIfo
            // 
            this.lblCustomerIfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomerIfo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCustomerIfo.Appearance.Options.UseFont = true;
            this.lblCustomerIfo.Location = new System.Drawing.Point(3, 5);
            this.lblCustomerIfo.Name = "lblCustomerIfo";
            this.lblCustomerIfo.Size = new System.Drawing.Size(75, 13);
            this.lblCustomerIfo.TabIndex = 6;
            this.lblCustomerIfo.Text = "labelControl1";
            // 
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Controls.Add(this.tableLayoutPanel3);
            this.xtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraScrollableControl1.Location = new System.Drawing.Point(3, 26);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Size = new System.Drawing.Size(906, 488);
            this.toolTipController.SetSuperTip(this.xtraScrollableControl1, null);
            this.xtraScrollableControl1.TabIndex = 7;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.treeListPriceEditor, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelControl2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelControl4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtSubType, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(906, 488);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel3, null);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // treeListPriceEditor
            // 
            this.treeListPriceEditor.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colPriceName,
            this.colPriceValue});
            this.treeListPriceEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListPriceEditor.Location = new System.Drawing.Point(138, 28);
            this.treeListPriceEditor.Name = "treeListPriceEditor";
            this.treeListPriceEditor.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItemMemoEdit,
            this.repItemCalcEditPrice});
            this.treeListPriceEditor.Size = new System.Drawing.Size(765, 457);
            this.treeListPriceEditor.TabIndex = 2;
            this.treeListPriceEditor.ToolTipController = this.toolTipController;
            this.treeListPriceEditor.CellValueChanging += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.treeListPriceEditor_CellValueChanging);
            // 
            // colPriceName
            // 
            this.colPriceName.Caption = "Цена";
            this.colPriceName.FieldName = "Цена";
            this.colPriceName.MinWidth = 80;
            this.colPriceName.Name = "colPriceName";
            this.colPriceName.OptionsColumn.AllowEdit = false;
            this.colPriceName.OptionsColumn.AllowFocus = false;
            this.colPriceName.OptionsColumn.ReadOnly = true;
            this.colPriceName.Visible = true;
            this.colPriceName.VisibleIndex = 0;
            this.colPriceName.Width = 180;
            // 
            // colPriceValue
            // 
            this.colPriceValue.Caption = "Значение";
            this.colPriceValue.ColumnEdit = this.repItemCalcEditPrice;
            this.colPriceValue.FieldName = "Значение";
            this.colPriceValue.Format.FormatString = "{0:### ### ##0.000}";
            this.colPriceValue.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPriceValue.MinWidth = 50;
            this.colPriceValue.Name = "colPriceValue";
            this.colPriceValue.Visible = true;
            this.colPriceValue.VisibleIndex = 1;
            this.colPriceValue.Width = 293;
            // 
            // repItemCalcEditPrice
            // 
            this.repItemCalcEditPrice.AutoHeight = false;
            this.repItemCalcEditPrice.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemCalcEditPrice.DisplayFormat.FormatString = "{0:### ### ##0.000}";
            this.repItemCalcEditPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repItemCalcEditPrice.Name = "repItemCalcEditPrice";
            // 
            // repItemMemoEdit
            // 
            this.repItemMemoEdit.Name = "repItemMemoEdit";
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(3, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(126, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Товарная подгруппа:";
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(3, 250);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 13);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "Цены:";
            // 
            // txtSubType
            // 
            this.txtSubType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubType.Location = new System.Drawing.Point(138, 3);
            this.txtSubType.Name = "txtSubType";
            this.txtSubType.Properties.ReadOnly = true;
            this.txtSubType.Size = new System.Drawing.Size(765, 20);
            this.txtSubType.TabIndex = 4;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.treeListPrices, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 479F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 479F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 479F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 479F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(303, 479);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel6, null);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // treeListPrices
            // 
            this.treeListPrices.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colPriceListPriceType,
            this.colPriceListPriceValue});
            this.treeListPrices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListPrices.Location = new System.Drawing.Point(3, 3);
            this.treeListPrices.Name = "treeListPrices";
            this.treeListPrices.OptionsBehavior.Editable = false;
            this.treeListPrices.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.treeListPrices.Size = new System.Drawing.Size(297, 473);
            this.treeListPrices.TabIndex = 5;
            this.treeListPrices.ToolTipController = this.toolTipController;
            // 
            // colPriceListPriceType
            // 
            this.colPriceListPriceType.Caption = "Цена";
            this.colPriceListPriceType.FieldName = "Цена";
            this.colPriceListPriceType.MinWidth = 80;
            this.colPriceListPriceType.Name = "colPriceListPriceType";
            this.colPriceListPriceType.OptionsColumn.AllowEdit = false;
            this.colPriceListPriceType.OptionsColumn.AllowFocus = false;
            this.colPriceListPriceType.OptionsColumn.ReadOnly = true;
            this.colPriceListPriceType.Visible = true;
            this.colPriceListPriceType.VisibleIndex = 0;
            this.colPriceListPriceType.Width = 135;
            // 
            // colPriceListPriceValue
            // 
            this.colPriceListPriceValue.Caption = "Значение";
            this.colPriceListPriceValue.FieldName = "Значение";
            this.colPriceListPriceValue.MinWidth = 50;
            this.colPriceListPriceValue.Name = "colPriceListPriceValue";
            this.colPriceListPriceValue.OptionsColumn.AllowEdit = false;
            this.colPriceListPriceValue.OptionsColumn.AllowFocus = false;
            this.colPriceListPriceValue.OptionsColumn.ReadOnly = true;
            this.colPriceListPriceValue.Visible = true;
            this.colPriceListPriceValue.VisibleIndex = 1;
            this.colPriceListPriceValue.Width = 90;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // contextMenuStripPrintPrices
            // 
            this.contextMenuStripPrintPrices.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitmsSelectAllProductOwner,
            this.mitmsDeSelectAllProductOwner});
            this.contextMenuStripPrintPrices.Name = "contextMenuStrip";
            this.contextMenuStripPrintPrices.Size = new System.Drawing.Size(146, 48);
            this.toolTipController.SetSuperTip(this.contextMenuStripPrintPrices, null);
            // 
            // mitmsSelectAllProductOwner
            // 
            this.mitmsSelectAllProductOwner.Name = "mitmsSelectAllProductOwner";
            this.mitmsSelectAllProductOwner.Size = new System.Drawing.Size(145, 22);
            this.mitmsSelectAllProductOwner.Text = "Выделить все";
            // 
            // mitmsDeSelectAllProductOwner
            // 
            this.mitmsDeSelectAllProductOwner.Name = "mitmsDeSelectAllProductOwner";
            this.mitmsDeSelectAllProductOwner.Size = new System.Drawing.Size(145, 22);
            this.mitmsDeSelectAllProductOwner.Text = "Отменить все";
            // 
            // tabControlExportToIB
            // 
            this.tabControlExportToIB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlExportToIB.Location = new System.Drawing.Point(0, 0);
            this.tabControlExportToIB.Name = "tabControlExportToIB";
            this.tabControlExportToIB.SelectedTabPage = this.tabPagePriceProperties;
            this.tabControlExportToIB.Size = new System.Drawing.Size(312, 510);
            this.tabControlExportToIB.TabIndex = 0;
            this.tabControlExportToIB.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPagePriceProperties});
            this.tabControlExportToIB.Text = "xtraTabControl1";
            // 
            // tabPagePriceProperties
            // 
            this.tabPagePriceProperties.Controls.Add(this.tableLayoutPanel6);
            this.tabPagePriceProperties.Name = "tabPagePriceProperties";
            this.tabPagePriceProperties.Size = new System.Drawing.Size(303, 479);
            this.tabPagePriceProperties.Text = "Цены";
            // 
            // tabDetail
            // 
            this.tabDetail.Controls.Add(this.tableLayoutPanelDetail);
            this.tabDetail.Name = "tabDetail";
            this.tabDetail.Size = new System.Drawing.Size(912, 547);
            this.tabDetail.Text = "xtraTabPage2";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "MS Excel 2003 files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*" +
    "";
            // 
            // frmPartsPriceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 556);
            this.Controls.Add(this.tabControl);
            this.Name = "frmPartsPriceList";
            this.toolTipController.SetSuperTip(this, null);
            this.Text = "frmPartsPriceList";
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabView.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPriceList)).EndInit();
            this.tableLayoutPanelDetail.ResumeLayout(false);
            this.tableLayoutPanelDetail.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.xtraScrollableControl1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListPriceEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCalcEditPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubType.Properties)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            this.contextMenuStripPrintPrices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlExportToIB)).EndInit();
            this.tabControlExportToIB.ResumeLayout(false);
            this.tabPagePriceProperties.ResumeLayout(false);
            this.tabDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraTab.XtraTabPage tabView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnExportForCalcPrice;
        private DevExpress.XtraTab.XtraTabPage tabDetail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDetail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.SimpleButton btnEdit;
        private DevExpress.XtraEditors.LabelControl lblCustomerIfo;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraTreeList.TreeList treeListPriceEditor;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colPriceName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colPriceValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repItemMemoEdit;
        private DevExpress.Utils.ToolTipController toolTipController;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridControlPriceList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewPriceList;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repItemCalcEditPrice;
        private DevExpress.XtraTab.XtraTabControl tabControlExportToIB;
        private DevExpress.XtraTab.XtraTabPage tabPagePriceProperties;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPrintPrices;
        private System.Windows.Forms.ToolStripMenuItem mitmsSelectAllProductOwner;
        private System.Windows.Forms.ToolStripMenuItem mitmsDeSelectAllProductOwner;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.XtraTreeList.TreeList treeListPrices;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colPriceListPriceType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colPriceListPriceValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.TextEdit txtSubType;
        private System.Windows.Forms.ToolStripMenuItem mitemImportPrices;
    }
}