namespace ERPMercuryPlan
{
    partial class frmImportXLSData
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFileOpenDialog = new DevExpress.XtraEditors.SimpleButton();
            this.txtID_Ib = new DevExpress.XtraEditors.TextEdit();
            this.clstSheets = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mitemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.treeListSettings = new DevExpress.XtraTreeList.TreeList();
            this.colSettingsName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colSettingsColumnNum = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCalcEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.treeListPrices = new DevExpress.XtraTreeList.TreeList();
            this.colCheck = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.reptemCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colPriceTypeName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colColumnPriceNum = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemCalc = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.contextMenuStripPrices = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mitemSelectAllPrices = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemDeselectAllPrices = new System.Windows.Forms.ToolStripMenuItem();
            this.repItemMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Ib.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clstSheets)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reptemCheckEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCalc)).BeginInit();
            this.contextMenuStripPrices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "MS Excel 2003 files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*" +
    "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.clstSheets, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelControl3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelControl17, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelControl1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(592, 601);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 571);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(592, 30);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel2, null);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Image = global::ERPMercuryPlan.Properties.Resources.delete2;
            this.btnCancel.Location = new System.Drawing.Point(512, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 24);
            this.btnCancel.TabIndex = 48;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.ToolTip = "Отменить выбор";
            this.btnCancel.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOk.Image = global::ERPMercuryPlan.Properties.Resources.check2;
            this.btnOk.Location = new System.Drawing.Point(423, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(83, 24);
            this.btnOk.TabIndex = 47;
            this.btnOk.Text = "ОК";
            this.btnOk.ToolTip = "Подтвердить выбор файла";
            this.btnOk.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.Controls.Add(this.btnFileOpenDialog, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtID_Ib, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(592, 25);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel3, null);
            this.tableLayoutPanel3.TabIndex = 34;
            // 
            // btnFileOpenDialog
            // 
            this.btnFileOpenDialog.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnFileOpenDialog.Location = new System.Drawing.Point(567, 3);
            this.btnFileOpenDialog.Name = "btnFileOpenDialog";
            this.btnFileOpenDialog.Size = new System.Drawing.Size(22, 19);
            this.btnFileOpenDialog.TabIndex = 1;
            this.btnFileOpenDialog.Text = "...";
            this.btnFileOpenDialog.ToolTipController = this.toolTipController;
            this.btnFileOpenDialog.Click += new System.EventHandler(this.btnFileOpenDialog_Click);
            // 
            // txtID_Ib
            // 
            this.txtID_Ib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtID_Ib.Location = new System.Drawing.Point(3, 3);
            this.txtID_Ib.Name = "txtID_Ib";
            this.txtID_Ib.Properties.ReadOnly = true;
            this.txtID_Ib.Size = new System.Drawing.Size(558, 20);
            this.txtID_Ib.TabIndex = 0;
            this.txtID_Ib.ToolTipController = this.toolTipController;
            // 
            // clstSheets
            // 
            this.clstSheets.ContextMenuStrip = this.contextMenuStrip;
            this.clstSheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clstSheets.Location = new System.Drawing.Point(3, 68);
            this.clstSheets.MultiColumn = true;
            this.clstSheets.Name = "clstSheets";
            this.clstSheets.Size = new System.Drawing.Size(586, 77);
            this.clstSheets.TabIndex = 36;
            this.clstSheets.ToolTipController = this.toolTipController;
            this.clstSheets.SelectedIndexChanged += new System.EventHandler(this.clstSheets_SelectedIndexChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitemSelectAll,
            this.mitemDeselectAll});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(146, 48);
            this.toolTipController.SetSuperTip(this.contextMenuStrip, null);
            // 
            // mitemSelectAll
            // 
            this.mitemSelectAll.Name = "mitemSelectAll";
            this.mitemSelectAll.Size = new System.Drawing.Size(145, 22);
            this.mitemSelectAll.Text = "Выделить все";
            this.mitemSelectAll.Click += new System.EventHandler(this.mitemSelectAll_Click);
            // 
            // mitemDeselectAll
            // 
            this.mitemDeselectAll.Name = "mitemDeselectAll";
            this.mitemDeselectAll.Size = new System.Drawing.Size(145, 22);
            this.mitemDeselectAll.Text = "Отменить все";
            this.mitemDeselectAll.Click += new System.EventHandler(this.mitemDeselectAll_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(3, 152);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(285, 13);
            this.labelControl3.TabIndex = 38;
            this.labelControl3.Text = "Привязка данных MS Excel к свойствам расчета:";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.treeListSettings, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.treeListPrices, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 172);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(586, 396);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel5, null);
            this.tableLayoutPanel5.TabIndex = 39;
            // 
            // treeListSettings
            // 
            this.treeListSettings.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colSettingsName,
            this.colSettingsColumnNum});
            this.treeListSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListSettings.Location = new System.Drawing.Point(3, 3);
            this.treeListSettings.Name = "treeListSettings";
            this.treeListSettings.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemCheckEdit1,
            this.repositoryItemCalcEdit1});
            this.treeListSettings.Size = new System.Drawing.Size(274, 390);
            this.treeListSettings.TabIndex = 40;
            // 
            // colSettingsName
            // 
            this.colSettingsName.Caption = "Параметр";
            this.colSettingsName.FieldName = "Цена";
            this.colSettingsName.MinWidth = 80;
            this.colSettingsName.Name = "colSettingsName";
            this.colSettingsName.OptionsColumn.AllowEdit = false;
            this.colSettingsName.OptionsColumn.AllowFocus = false;
            this.colSettingsName.OptionsColumn.ReadOnly = true;
            this.colSettingsName.Visible = true;
            this.colSettingsName.VisibleIndex = 0;
            this.colSettingsName.Width = 158;
            // 
            // colSettingsColumnNum
            // 
            this.colSettingsColumnNum.Caption = "№ столбца";
            this.colSettingsColumnNum.ColumnEdit = this.repositoryItemCalcEdit1;
            this.colSettingsColumnNum.FieldName = "№ столбца";
            this.colSettingsColumnNum.MinWidth = 50;
            this.colSettingsColumnNum.Name = "colSettingsColumnNum";
            this.colSettingsColumnNum.OptionsColumn.AllowMove = false;
            this.colSettingsColumnNum.OptionsColumn.AllowSort = false;
            this.colSettingsColumnNum.Visible = true;
            this.colSettingsColumnNum.VisibleIndex = 1;
            this.colSettingsColumnNum.Width = 114;
            // 
            // repositoryItemCalcEdit1
            // 
            this.repositoryItemCalcEdit1.AutoHeight = false;
            this.repositoryItemCalcEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEdit1.Name = "repositoryItemCalcEdit1";
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // treeListPrices
            // 
            this.treeListPrices.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colCheck,
            this.colPriceTypeName,
            this.colColumnPriceNum});
            this.treeListPrices.ContextMenuStrip = this.contextMenuStripPrices;
            this.treeListPrices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListPrices.Location = new System.Drawing.Point(283, 3);
            this.treeListPrices.Name = "treeListPrices";
            this.treeListPrices.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItemMemoEdit,
            this.reptemCheckEdit,
            this.repItemCalc});
            this.treeListPrices.Size = new System.Drawing.Size(300, 390);
            this.treeListPrices.TabIndex = 38;
            this.treeListPrices.ToolTipController = this.toolTipController;
            // 
            // colCheck
            // 
            this.colCheck.Caption = "Вкл";
            this.colCheck.ColumnEdit = this.reptemCheckEdit;
            this.colCheck.FieldName = "Вкл";
            this.colCheck.MinWidth = 30;
            this.colCheck.Name = "colCheck";
            this.colCheck.OptionsColumn.AllowMove = false;
            this.colCheck.OptionsColumn.FixedWidth = true;
            this.colCheck.Visible = true;
            this.colCheck.VisibleIndex = 0;
            this.colCheck.Width = 57;
            // 
            // reptemCheckEdit
            // 
            this.reptemCheckEdit.AutoHeight = false;
            this.reptemCheckEdit.Name = "reptemCheckEdit";
            // 
            // colPriceTypeName
            // 
            this.colPriceTypeName.Caption = "Цена";
            this.colPriceTypeName.FieldName = "Цена";
            this.colPriceTypeName.MinWidth = 80;
            this.colPriceTypeName.Name = "colPriceTypeName";
            this.colPriceTypeName.OptionsColumn.AllowEdit = false;
            this.colPriceTypeName.OptionsColumn.AllowFocus = false;
            this.colPriceTypeName.OptionsColumn.ReadOnly = true;
            this.colPriceTypeName.Visible = true;
            this.colPriceTypeName.VisibleIndex = 1;
            this.colPriceTypeName.Width = 158;
            // 
            // colColumnPriceNum
            // 
            this.colColumnPriceNum.Caption = "№ столбца";
            this.colColumnPriceNum.ColumnEdit = this.repItemCalc;
            this.colColumnPriceNum.FieldName = "№ столбца";
            this.colColumnPriceNum.MinWidth = 50;
            this.colColumnPriceNum.Name = "colColumnPriceNum";
            this.colColumnPriceNum.Visible = true;
            this.colColumnPriceNum.VisibleIndex = 2;
            this.colColumnPriceNum.Width = 114;
            // 
            // repItemCalc
            // 
            this.repItemCalc.AutoHeight = false;
            this.repItemCalc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemCalc.Name = "repItemCalc";
            // 
            // contextMenuStripPrices
            // 
            this.contextMenuStripPrices.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitemSelectAllPrices,
            this.mitemDeselectAllPrices});
            this.contextMenuStripPrices.Name = "contextMenuStrip";
            this.contextMenuStripPrices.Size = new System.Drawing.Size(146, 48);
            this.toolTipController.SetSuperTip(this.contextMenuStripPrices, null);
            // 
            // mitemSelectAllPrices
            // 
            this.mitemSelectAllPrices.Name = "mitemSelectAllPrices";
            this.mitemSelectAllPrices.Size = new System.Drawing.Size(145, 22);
            this.mitemSelectAllPrices.Text = "Выделить все";
            this.mitemSelectAllPrices.Click += new System.EventHandler(this.mitemSelectAllPrices_Click);
            // 
            // mitemDeselectAllPrices
            // 
            this.mitemDeselectAllPrices.Name = "mitemDeselectAllPrices";
            this.mitemDeselectAllPrices.Size = new System.Drawing.Size(145, 22);
            this.mitemDeselectAllPrices.Text = "Отменить все";
            this.mitemDeselectAllPrices.Click += new System.EventHandler(this.mitemDeselectAllPrices_Click);
            // 
            // repItemMemoEdit
            // 
            this.repItemMemoEdit.Name = "repItemMemoEdit";
            // 
            // labelControl17
            // 
            this.labelControl17.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl17.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl17.Appearance.Options.UseFont = true;
            this.labelControl17.Location = new System.Drawing.Point(3, 3);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(84, 13);
            this.labelControl17.TabIndex = 1;
            this.labelControl17.Text = "Файл MS Excel:";
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(3, 48);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(82, 13);
            this.labelControl1.TabIndex = 35;
            this.labelControl1.Text = "Лист MS Excel:";
            // 
            // frmImportXLSData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 601);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "frmImportXLSData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.toolTipController.SetSuperTip(this, null);
            this.Text = "Импорт расчета цен из MS Excel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Ib.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clstSheets)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reptemCheckEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCalc)).EndInit();
            this.contextMenuStripPrices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.Utils.ToolTipController toolTipController;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.SimpleButton btnFileOpenDialog;
        private DevExpress.XtraEditors.TextEdit txtID_Ib;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckedListBoxControl clstSheets;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private DevExpress.XtraTreeList.TreeList treeListPrices;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit reptemCheckEdit;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colPriceTypeName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colColumnPriceNum;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repItemMemoEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repItemCalc;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mitemSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mitemDeselectAll;
        private DevExpress.XtraTreeList.TreeList treeListSettings;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSettingsName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSettingsColumnNum;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPrices;
        private System.Windows.Forms.ToolStripMenuItem mitemSelectAllPrices;
        private System.Windows.Forms.ToolStripMenuItem mitemDeselectAllPrices;
    }
}