namespace ERPMercuryPlan
{
    partial class frmProductDetail
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
            this.tableLayoutPanelBgrnd = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBgrnd
            // 
            this.tableLayoutPanelBgrnd.ColumnCount = 1;
            this.tableLayoutPanelBgrnd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBgrnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBgrnd.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBgrnd.Name = "tableLayoutPanelBgrnd";
            this.tableLayoutPanelBgrnd.RowCount = 1;
            this.tableLayoutPanelBgrnd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBgrnd.Size = new System.Drawing.Size(614, 404);
            this.tableLayoutPanelBgrnd.TabIndex = 0;
            // 
            // frmProductDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 404);
            this.Controls.Add(this.tableLayoutPanelBgrnd);
            this.Name = "frmProductDetail";
            this.Text = "Справочник товарных подгрупп";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProductDetail_FormClosing);
            this.Shown += new System.EventHandler(this.frmProductDetail_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBgrnd;
    }
}