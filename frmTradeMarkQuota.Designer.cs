namespace ERPMercuryPlan
{
    partial class frmTradeMarkQuota
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTradeMarkQuota));
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cboxProductTradeMark = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblEarningCompany = new DevExpress.XtraEditors.LabelControl();
            this.lblObjectType = new DevExpress.XtraEditors.LabelControl();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.colDepartTeam = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartTeamQuota = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCalcEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.checkEditEquable = new DevExpress.XtraEditors.CheckEdit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboxProductTradeMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditEquable.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(525, 375);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 344);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(525, 31);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel2, null);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(447, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Выход";
            this.btnCancel.ToolTip = "Отменить изменения";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(365, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "ОК";
            this.btnSave.ToolTip = "Сохранить изменения";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.cboxProductTradeMark, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblEarningCompany, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblObjectType, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.treeList, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.checkEditEquable, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(519, 338);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel3, null);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // cboxProductTradeMark
            // 
            this.cboxProductTradeMark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxProductTradeMark.Location = new System.Drawing.Point(102, 3);
            this.cboxProductTradeMark.Name = "cboxProductTradeMark";
            this.cboxProductTradeMark.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxProductTradeMark.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxProductTradeMark.Size = new System.Drawing.Size(414, 20);
            this.cboxProductTradeMark.TabIndex = 0;
            this.cboxProductTradeMark.ToolTip = "товарная марка";
            this.cboxProductTradeMark.ToolTipController = this.toolTipController;
            this.cboxProductTradeMark.SelectedValueChanged += new System.EventHandler(this.cboxProductTradeMark_SelectedValueChanged);
            // 
            // lblEarningCompany
            // 
            this.lblEarningCompany.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEarningCompany.Location = new System.Drawing.Point(3, 5);
            this.lblEarningCompany.Name = "lblEarningCompany";
            this.lblEarningCompany.Size = new System.Drawing.Size(85, 13);
            this.lblEarningCompany.TabIndex = 4;
            this.lblEarningCompany.Tag = "1";
            this.lblEarningCompany.Text = "Товарная марка:";
            // 
            // lblObjectType
            // 
            this.lblObjectType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblObjectType.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblObjectType.Appearance.Options.UseFont = true;
            this.lblObjectType.Location = new System.Drawing.Point(3, 162);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(58, 13);
            this.lblObjectType.TabIndex = 6;
            this.lblObjectType.Tag = "1";
            this.lblObjectType.Text = "Команды:";
            // 
            // treeList
            // 
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colDepartTeam,
            this.colDepartTeamQuota});
            this.treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList.Location = new System.Drawing.Point(102, 27);
            this.treeList.Name = "treeList";
            this.treeList.OptionsView.ShowSummaryFooter = true;
            this.treeList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCalcEdit});
            this.treeList.Size = new System.Drawing.Size(414, 284);
            this.treeList.TabIndex = 13;
            this.treeList.ToolTipController = this.toolTipController;
            // 
            // colDepartTeam
            // 
            this.colDepartTeam.Caption = "Команда";
            this.colDepartTeam.FieldName = "Команда";
            this.colDepartTeam.Name = "colDepartTeam";
            this.colDepartTeam.OptionsColumn.AllowEdit = false;
            this.colDepartTeam.OptionsColumn.ReadOnly = true;
            this.colDepartTeam.Visible = true;
            this.colDepartTeam.VisibleIndex = 0;
            this.colDepartTeam.Width = 257;
            // 
            // colDepartTeamQuota
            // 
            this.colDepartTeamQuota.Caption = "Доля продаж, %";
            this.colDepartTeamQuota.FieldName = "Доля продаж, %";
            this.colDepartTeamQuota.Format.FormatString = "{0:#0.000}";
            this.colDepartTeamQuota.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDepartTeamQuota.Name = "colDepartTeamQuota";
            this.colDepartTeamQuota.RowFooterSummary = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colDepartTeamQuota.RowFooterSummaryStrFormat = "{0:#0.000}";
            this.colDepartTeamQuota.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colDepartTeamQuota.SummaryFooterStrFormat = "{0:#0.000}";
            this.colDepartTeamQuota.Visible = true;
            this.colDepartTeamQuota.VisibleIndex = 1;
            this.colDepartTeamQuota.Width = 114;
            // 
            // repositoryItemCalcEdit
            // 
            this.repositoryItemCalcEdit.AutoHeight = false;
            this.repositoryItemCalcEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemCalcEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemCalcEdit.Name = "repositoryItemCalcEdit";
            // 
            // checkEditEquable
            // 
            this.checkEditEquable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEditEquable.Location = new System.Drawing.Point(102, 317);
            this.checkEditEquable.Name = "checkEditEquable";
            this.checkEditEquable.Properties.Caption = "распределить долю команды по подразделениям равномерно";
            this.checkEditEquable.Size = new System.Drawing.Size(414, 19);
            this.checkEditEquable.TabIndex = 14;
            // 
            // frmTradeMarkQuota
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 375);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "frmTradeMarkQuota";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.toolTipController.SetSuperTip(this, null);
            this.Text = "Доли продаж команд в рамках товарной марки";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboxProductTradeMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditEquable.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ToolTipController toolTipController;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.ComboBoxEdit cboxProductTradeMark;
        private DevExpress.XtraEditors.LabelControl lblEarningCompany;
        private DevExpress.XtraEditors.LabelControl lblObjectType;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartTeam;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartTeamQuota;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit;
        private DevExpress.XtraEditors.CheckEdit checkEditEquable;
    }
}