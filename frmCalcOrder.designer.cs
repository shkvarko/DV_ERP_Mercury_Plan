namespace ERPMercuryPlan
{
    partial class frmCalcOrder
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
            this.toolTipController = new DevExpress.Utils.ToolTipController();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControlProductList = new DevExpress.XtraGrid.GridControl();
            this.gridViewProductList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barManager = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barBtnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCalcOrder = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
            this.splitContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainerControl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 430F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(647, 430);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // splitContainerControl
            // 
            this.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl.Location = new System.Drawing.Point(3, 3);
            this.splitContainerControl.Name = "splitContainerControl";
            this.splitContainerControl.Panel1.Controls.Add(this.gridControlProductList);
            this.splitContainerControl.Panel1.MinSize = 100;
            this.splitContainerControl.Panel1.Text = "Panel1";
            this.splitContainerControl.Panel2.Controls.Add(this.tabControl);
            this.splitContainerControl.Panel2.MinSize = 200;
            this.splitContainerControl.Panel2.Text = "Panel2";
            this.splitContainerControl.Size = new System.Drawing.Size(641, 424);
            this.splitContainerControl.SplitterPosition = 282;
            this.toolTipController.SetSuperTip(this.splitContainerControl, null);
            this.splitContainerControl.TabIndex = 6;
            this.splitContainerControl.Text = "splitContainerControl1";
            // 
            // gridControlProductList
            // 
            this.gridControlProductList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlProductList.EmbeddedNavigator.Name = "";
            this.gridControlProductList.Location = new System.Drawing.Point(0, 0);
            this.gridControlProductList.MainView = this.gridViewProductList;
            this.gridControlProductList.Name = "gridControlProductList";
            this.gridControlProductList.Size = new System.Drawing.Size(278, 420);
            this.gridControlProductList.TabIndex = 1;
            this.gridControlProductList.ToolTipController = this.toolTipController;
            this.gridControlProductList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewProductList});
            this.gridControlProductList.DoubleClick += new System.EventHandler(this.gridControlProductList_DoubleClick);
            // 
            // gridViewProductList
            // 
            this.gridViewProductList.GridControl = this.gridControlProductList;
            this.gridViewProductList.Name = "gridViewProductList";
            this.gridViewProductList.OptionsBehavior.Editable = false;
            this.gridViewProductList.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewProductList.OptionsDetail.ShowDetailTabs = false;
            this.gridViewProductList.OptionsDetail.SmartDetailExpand = false;
            this.gridViewProductList.OptionsView.ShowGroupPanel = false;
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HeaderButtons = ((DevExpress.XtraTab.TabButtons)((DevExpress.XtraTab.TabButtons.Close | DevExpress.XtraTab.TabButtons.Default)));
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.MinimumSize = new System.Drawing.Size(100, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.tabControl.Size = new System.Drawing.Size(349, 420);
            this.tabControl.TabIndex = 2;
            this.tabControl.Text = "xtraTabControl1";
            this.tabControl.ToolTipController = this.toolTipController;
            this.tabControl.CloseButtonClick += new System.EventHandler(this.tabControl_CloseButtonClick);
            // 
            // barDockControlTop
            // 
            this.toolTipController.SetSuperTip(this.barDockControlTop, null);
            // 
            // barDockControlBottom
            // 
            this.toolTipController.SetSuperTip(this.barDockControlBottom, null);
            // 
            // barDockControlLeft
            // 
            this.toolTipController.SetSuperTip(this.barDockControlLeft, null);
            // 
            // barDockControlRight
            // 
            this.toolTipController.SetSuperTip(this.barDockControlRight, null);
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barBtnRefresh,
            this.barBtnPrint,
            this.barBtnCalcOrder,
            this.barBtnDelete});
            this.barManager.MaxItemId = 4;
            this.barManager.ToolTipController = this.toolTipController;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnRefresh),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnCalcOrder),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnDelete),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnPrint)});
            this.bar1.Text = "Tools";
            // 
            // barBtnRefresh
            // 
            this.barBtnRefresh.Glyph = global::ERPMercuryPlan.Properties.Resources.refresh;
            this.barBtnRefresh.Hint = "Обновить";
            this.barBtnRefresh.Id = 0;
            this.barBtnRefresh.Name = "barBtnRefresh";
            this.barBtnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnRefresh_ItemClick);
            // 
            // barBtnCalcOrder
            // 
            this.barBtnCalcOrder.Caption = "Расчет";
            this.barBtnCalcOrder.Glyph = global::ERPMercuryPlan.Properties.Resources.add2;
            this.barBtnCalcOrder.Hint = "Расчет коэффициента";
            this.barBtnCalcOrder.Id = 2;
            this.barBtnCalcOrder.Name = "barBtnCalcOrder";
            this.barBtnCalcOrder.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCalcOrder_ItemClick);
            // 
            // barBtnDelete
            // 
            this.barBtnDelete.Caption = "barBtnDelete";
            this.barBtnDelete.Glyph = global::ERPMercuryPlan.Properties.Resources.delete2;
            this.barBtnDelete.Hint = "Удалить расчет";
            this.barBtnDelete.Id = 3;
            this.barBtnDelete.Name = "barBtnDelete";
            this.barBtnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnDelete_ItemClick);
            // 
            // barBtnPrint
            // 
            this.barBtnPrint.Glyph = global::ERPMercuryPlan.Properties.Resources.printer2;
            this.barBtnPrint.Hint = "Печать";
            this.barBtnPrint.Id = 1;
            this.barBtnPrint.Name = "barBtnPrint";
            this.barBtnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnPrint_ItemClick);
            // 
            // frmCalcOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 456);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmCalcOrder";
            this.toolTipController.SetSuperTip(this, null);
            this.Text = "расчет коэффициентов для плана продаж";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCalcOrder_FormClosing);
            this.Load += new System.EventHandler(this.frmCalcOrder_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
            this.splitContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ToolTipController toolTipController;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraGrid.GridControl gridControlProductList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductList;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barBtnRefresh;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barBtnPrint;
        private DevExpress.XtraBars.BarButtonItem barBtnCalcOrder;
        private DevExpress.XtraBars.BarButtonItem barBtnDelete;
    }
}

