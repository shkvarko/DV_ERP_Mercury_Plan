namespace ERPMercuryPlan
{
    partial class ctrlCalcKoeffItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.gridControlProductList = new DevExpress.XtraGrid.GridControl();
            this.gridViewProductList = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductList)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlProductList
            // 
            this.gridControlProductList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlProductList.EmbeddedNavigator.Name = "";
            this.gridControlProductList.Location = new System.Drawing.Point(0, 0);
            this.gridControlProductList.MainView = this.gridViewProductList;
            this.gridControlProductList.Name = "gridControlProductList";
            this.gridControlProductList.Size = new System.Drawing.Size(589, 380);
            this.gridControlProductList.TabIndex = 1;
            this.gridControlProductList.ToolTipController = this.toolTipController;
            this.gridControlProductList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewProductList});
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
            // ctrlCalcOrderItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlProductList);
            this.Name = "ctrlCalcOrderItem";
            this.Size = new System.Drawing.Size(589, 380);
            this.toolTipController.SetSuperTip(this, null);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ToolTipController toolTipController;
        private DevExpress.XtraGrid.GridControl gridControlProductList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductList;
    }
}
