namespace ERPMercuryPlan
{
    partial class frmTradeMarkDepartTeamQuota
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTradeMarkDepartTeamQuota));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cboxProductTradeMark = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblEarningCompany = new DevExpress.XtraEditors.LabelControl();
            this.lblObjectType = new DevExpress.XtraEditors.LabelControl();
            this.checkEditEquable = new DevExpress.XtraEditors.CheckEdit();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.colDepartTeam = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepart = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartQuota = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCalcEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboxProductTradeMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditEquable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit)).BeginInit();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(524, 377);
            this.tableLayoutPanel1.TabIndex = 2;
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 346);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(524, 31);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(446, 3);
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
            this.btnSave.Location = new System.Drawing.Point(364, 3);
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
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.cboxProductTradeMark, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblEarningCompany, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblObjectType, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.checkEditEquable, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.treeList, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(518, 340);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // cboxProductTradeMark
            // 
            this.cboxProductTradeMark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxProductTradeMark.Location = new System.Drawing.Point(107, 3);
            this.cboxProductTradeMark.Name = "cboxProductTradeMark";
            this.cboxProductTradeMark.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxProductTradeMark.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxProductTradeMark.Size = new System.Drawing.Size(408, 20);
            this.cboxProductTradeMark.TabIndex = 0;
            this.cboxProductTradeMark.ToolTip = "товарная марка";
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
            this.lblObjectType.Location = new System.Drawing.Point(3, 187);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(97, 13);
            this.lblObjectType.TabIndex = 6;
            this.lblObjectType.Tag = "1";
            this.lblObjectType.Text = "Подразделения:";
            // 
            // checkEditEquable
            // 
            this.checkEditEquable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEditEquable.Location = new System.Drawing.Point(107, 27);
            this.checkEditEquable.Name = "checkEditEquable";
            this.checkEditEquable.Properties.Caption = "распределить доли равномерно по подразделениям";
            this.checkEditEquable.Size = new System.Drawing.Size(408, 19);
            this.checkEditEquable.TabIndex = 1;
            this.checkEditEquable.CheckedChanged += new System.EventHandler(this.checkEditEquable_CheckedChanged);
            // 
            // treeList
            // 
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colDepartTeam,
            this.colDepart,
            this.colDepartQuota});
            this.treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList.Location = new System.Drawing.Point(107, 51);
            this.treeList.Name = "treeList";
            this.treeList.OptionsView.ShowSummaryFooter = true;
            this.treeList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCalcEdit});
            this.treeList.Size = new System.Drawing.Size(408, 286);
            this.treeList.TabIndex = 2;
            // 
            // colDepartTeam
            // 
            this.colDepartTeam.Caption = "Команда";
            this.colDepartTeam.FieldName = "Команда";
            this.colDepartTeam.MinWidth = 75;
            this.colDepartTeam.Name = "colDepartTeam";
            this.colDepartTeam.OptionsColumn.AllowEdit = false;
            this.colDepartTeam.OptionsColumn.ReadOnly = true;
            this.colDepartTeam.Visible = true;
            this.colDepartTeam.VisibleIndex = 0;
            this.colDepartTeam.Width = 165;
            // 
            // colDepart
            // 
            this.colDepart.Caption = "Подразделение";
            this.colDepart.FieldName = "Подразделение";
            this.colDepart.Name = "colDepart";
            this.colDepart.OptionsColumn.AllowEdit = false;
            this.colDepart.OptionsColumn.ReadOnly = true;
            this.colDepart.Visible = true;
            this.colDepart.VisibleIndex = 1;
            this.colDepart.Width = 97;
            // 
            // colDepartQuota
            // 
            this.colDepartQuota.Caption = "Доля продаж, %";
            this.colDepartQuota.FieldName = "Доля продаж, %";
            this.colDepartQuota.Format.FormatString = "{0:#0.0000}";
            this.colDepartQuota.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDepartQuota.Name = "colDepartQuota";
            this.colDepartQuota.RowFooterSummary = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colDepartQuota.RowFooterSummaryStrFormat = "{0:#0.0000}";
            this.colDepartQuota.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colDepartQuota.SummaryFooterStrFormat = "{0:#0.0000}";
            this.colDepartQuota.Visible = true;
            this.colDepartQuota.VisibleIndex = 2;
            this.colDepartQuota.Width = 125;
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
            // frmTradeMarkDepartTeamQuota
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 377);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "frmTradeMarkDepartTeamQuota";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Доли продаж подразделений в рамках команды";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboxProductTradeMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditEquable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.ComboBoxEdit cboxProductTradeMark;
        private DevExpress.XtraEditors.LabelControl lblEarningCompany;
        private DevExpress.XtraEditors.LabelControl lblObjectType;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepart;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartQuota;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit;
        private DevExpress.XtraEditors.CheckEdit checkEditEquable;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartTeam;
    }
}