namespace ERPMercuryPlan
{
    partial class frmExportToExcelDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.calcSheetNum = new DevExpress.XtraEditors.CalcEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFileOpenDialog = new DevExpress.XtraEditors.SimpleButton();
            this.txtID_Ib = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.calcCurRatePrice = new DevExpress.XtraEditors.CalcEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.treeListSettings = new DevExpress.XtraTreeList.TreeList();
            this.colSettingsName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colSettingsColumnNum = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemCalc = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.repItemMemoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.reptemCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btnSaveSettings = new DevExpress.XtraEditors.SimpleButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.calcSheetNum.Properties)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Ib.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcCurRatePrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCalc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reptemCheckEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.calcSheetNum, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelControl17, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelControl2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.calcCurRatePrice, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelControl3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.treeListSettings, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveSettings, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(490, 401);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // calcSheetNum
            // 
            this.calcSheetNum.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.calcSheetNum.Location = new System.Drawing.Point(149, 28);
            this.calcSheetNum.Name = "calcSheetNum";
            this.calcSheetNum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.calcSheetNum.Properties.DisplayFormat.FormatString = "{0:### ### ##0}";
            this.calcSheetNum.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcSheetNum.Properties.EditFormat.FormatString = "{0:### ### ##0}";
            this.calcSheetNum.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcSheetNum.Size = new System.Drawing.Size(91, 20);
            this.calcSheetNum.TabIndex = 1;
            this.calcSheetNum.ToolTip = "Ставка НДС";
            this.calcSheetNum.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.calcSheetNum.EditValueChanged += new System.EventHandler(this.calcSheetNum_EditValueChanged);
            this.calcSheetNum.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.calcSheetNum_EditValueChanging);
            // 
            // labelControl17
            // 
            this.labelControl17.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl17.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl17.Appearance.Options.UseFont = true;
            this.labelControl17.Location = new System.Drawing.Point(3, 6);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(84, 13);
            this.labelControl17.TabIndex = 1;
            this.labelControl17.Text = "Файл MS Excel:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(146, 369);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(344, 32);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::ERPMercuryPlan.Properties.Resources.delete2;
            this.btnCancel.Location = new System.Drawing.Point(264, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 22);
            this.btnCancel.TabIndex = 48;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.ToolTip = "Отменить выбор";
            this.btnCancel.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Image = global::ERPMercuryPlan.Properties.Resources.check2;
            this.btnOk.Location = new System.Drawing.Point(175, 7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(83, 22);
            this.btnOk.TabIndex = 47;
            this.btnOk.Text = "ОК";
            this.btnOk.ToolTip = "Подтвердить выбор файла";
            this.btnOk.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(3, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(53, 13);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "№ листа:";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.btnFileOpenDialog, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtID_Ib, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(146, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(344, 25);
            this.tableLayoutPanel3.TabIndex = 34;
            // 
            // btnFileOpenDialog
            // 
            this.btnFileOpenDialog.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnFileOpenDialog.Location = new System.Drawing.Point(318, 3);
            this.btnFileOpenDialog.Name = "btnFileOpenDialog";
            this.btnFileOpenDialog.Size = new System.Drawing.Size(22, 19);
            this.btnFileOpenDialog.TabIndex = 1;
            this.btnFileOpenDialog.Text = "...";
            this.btnFileOpenDialog.Click += new System.EventHandler(this.btnFileOpenDialog_Click);
            // 
            // txtID_Ib
            // 
            this.txtID_Ib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtID_Ib.Location = new System.Drawing.Point(3, 3);
            this.txtID_Ib.Name = "txtID_Ib";
            this.txtID_Ib.Properties.ReadOnly = true;
            this.txtID_Ib.Size = new System.Drawing.Size(309, 20);
            this.txtID_Ib.TabIndex = 0;
            this.txtID_Ib.EditValueChanged += new System.EventHandler(this.txtID_Ib_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(3, 56);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(137, 13);
            this.labelControl2.TabIndex = 35;
            this.labelControl2.Text = "Курс ценообразования:";
            // 
            // calcCurRatePrice
            // 
            this.calcCurRatePrice.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.calcCurRatePrice.Location = new System.Drawing.Point(149, 53);
            this.calcCurRatePrice.Name = "calcCurRatePrice";
            this.calcCurRatePrice.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.calcCurRatePrice.Properties.DisplayFormat.FormatString = "{0:### ### ##0}";
            this.calcCurRatePrice.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcCurRatePrice.Properties.EditFormat.FormatString = "{0:### ### ##0}";
            this.calcCurRatePrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcCurRatePrice.Size = new System.Drawing.Size(91, 20);
            this.calcCurRatePrice.TabIndex = 36;
            this.calcCurRatePrice.ToolTip = "Ставка НДС";
            this.calcCurRatePrice.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(3, 215);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(64, 13);
            this.labelControl3.TabIndex = 37;
            this.labelControl3.Text = "Настройки:";
            // 
            // treeListSettings
            // 
            this.treeListSettings.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colSettingsName,
            this.colSettingsColumnNum});
            this.treeListSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListSettings.Location = new System.Drawing.Point(149, 78);
            this.treeListSettings.Name = "treeListSettings";
            this.treeListSettings.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItemMemoEdit,
            this.reptemCheckEdit,
            this.repItemCalc});
            this.treeListSettings.Size = new System.Drawing.Size(338, 288);
            this.treeListSettings.TabIndex = 39;
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
            this.colSettingsColumnNum.ColumnEdit = this.repItemCalc;
            this.colSettingsColumnNum.FieldName = "№ столбца";
            this.colSettingsColumnNum.MinWidth = 50;
            this.colSettingsColumnNum.Name = "colSettingsColumnNum";
            this.colSettingsColumnNum.OptionsColumn.AllowMove = false;
            this.colSettingsColumnNum.OptionsColumn.AllowSort = false;
            this.colSettingsColumnNum.Visible = true;
            this.colSettingsColumnNum.VisibleIndex = 1;
            this.colSettingsColumnNum.Width = 114;
            // 
            // repItemCalc
            // 
            this.repItemCalc.AutoHeight = false;
            this.repItemCalc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemCalc.Name = "repItemCalc";
            this.repItemCalc.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.repItemCalc_EditValueChanging);
            // 
            // repItemMemoEdit
            // 
            this.repItemMemoEdit.Name = "repItemMemoEdit";
            // 
            // reptemCheckEdit
            // 
            this.reptemCheckEdit.AutoHeight = false;
            this.reptemCheckEdit.Name = "reptemCheckEdit";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Image = global::ERPMercuryPlan.Properties.Resources.check2;
            this.btnSaveSettings.Location = new System.Drawing.Point(3, 376);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(140, 22);
            this.btnSaveSettings.TabIndex = 48;
            this.btnSaveSettings.Text = "Сохранить настройки";
            this.btnSaveSettings.ToolTip = "Подтвердить выбор файла";
            this.btnSaveSettings.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "MS Excel 2003 files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*" +
    "";
            // 
            // frmExportToExcelDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 401);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "frmExportToExcelDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Экспорт данных в MS Excel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.calcSheetNum.Properties)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Ib.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcCurRatePrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCalc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reptemCheckEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtID_Ib;
        private DevExpress.XtraEditors.CalcEdit calcSheetNum;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.SimpleButton btnFileOpenDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CalcEdit calcCurRatePrice;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraTreeList.TreeList treeListSettings;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSettingsName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSettingsColumnNum;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repItemCalc;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repItemMemoEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit reptemCheckEdit;
        private DevExpress.XtraEditors.SimpleButton btnSaveSettings;
    }
}