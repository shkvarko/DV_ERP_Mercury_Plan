using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERPMercuryPlan
{
    public partial class frmCalcOrder : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private System.Boolean m_bPlanIsChanged;
        private List<CCalcPlanKoef> m_objCalcOrderList;
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlProductList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        private const System.Int32 m_iStartSplitterPosition = 180;
        #endregion

        #region Конструктор
        public frmCalcOrder(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bPlanIsChanged = false;
            m_objCalcOrderList = null;

            AddGridColumns();
            LoadCalcOrderList();
            Text = m_objMenuItem.strName;
        }
        #endregion

        #region Список расчетов
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();
            AddGridColumn(ColumnView, "ID", "УИ расчета");
            AddGridColumn(ColumnView, "Date", "Дата");
            AddGridColumn(ColumnView, "Num", "Номер");
            AddGridColumn(ColumnView, "ShipPeriod", "Период продаж");
            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                objColumn.Visible = (objColumn.FieldName != "ID");
            }
        }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption) { AddGridColumn(view, fieldName, caption, null); }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption, DevExpress.XtraEditors.Repository.RepositoryItem item) { AddGridColumn(view, fieldName, caption, item, "", DevExpress.Utils.FormatType.None); }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption, DevExpress.XtraEditors.Repository.RepositoryItem item, string format, DevExpress.Utils.FormatType type)
        {
            DevExpress.XtraGrid.Columns.GridColumn column = view.Columns.AddField(fieldName);
            column.Caption = caption;
            column.ColumnEdit = item;
            column.DisplayFormat.FormatType = type;
            column.DisplayFormat.FormatString = format;
            column.VisibleIndex = view.VisibleColumns.Count;
        }

        /// <summary>
        /// Запрашивает список расчетов
        /// </summary>
        private void LoadCalcOrderList()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.splitContainerControl.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                gridControlProductList.DataSource = null;

                m_objCalcOrderList = CCalcPlanKoef.GetCalcPlanKoefList( m_objProfile, null );

                if (m_objCalcOrderList != null)
                {
                    gridControlProductList.DataSource = m_objCalcOrderList;

                }

                this.splitContainerControl.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();
                splitContainerControl.SplitterPosition = m_iStartSplitterPosition;
                //splitContainerControl.Panel2.Size = new Size(200, splitContainerControl.Panel2.Size.Height);
                splitContainerControl.Refresh();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return ;
        }
        private void frmCalcOrder_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCalcOrderList();
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка открытия формы.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadCalcOrderList();
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка обновления списка расчетов.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        #endregion

        #region Загрузить содержимое расчета
        /// <summary>
        /// Возвращает ссылку на выбранный в списке расчет
        /// </summary>
        /// <returns>ссылка на товар</returns>
        private CCalcPlanKoef GetSelectedCalcOrder()
        {
            CCalcPlanKoef objRet = null;
            try
            {
                if ((((DevExpress.XtraGrid.Views.Grid.GridView)gridControlProductList.MainView).RowCount > 0) &&
                    (((DevExpress.XtraGrid.Views.Grid.GridView)gridControlProductList.MainView).FocusedRowHandle >= 0))
                {
                    System.Guid uuidID = (System.Guid)(((DevExpress.XtraGrid.Views.Grid.GridView)gridControlProductList.MainView)).GetFocusedRowCellValue("ID");

                    if ((m_objCalcOrderList != null) && (m_objCalcOrderList.Count > 0) && (uuidID.CompareTo(System.Guid.Empty) != 0))
                    {
                        foreach (CCalcPlanKoef objCalcOrder in m_objCalcOrderList)
                        {
                            if (objCalcOrder.ID.CompareTo(uuidID) == 0)
                            {
                                objRet = objCalcOrder;
                                break;
                            }
                        }
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка поиска выбранного расчета. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return objRet;
        }
        private void gridControlProductList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CCalcPlanKoef objSelectedCalcOrder = GetSelectedCalcOrder();
                LoadData(objSelectedCalcOrder, false);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка просмотра приложения к расчету. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }


        private void LoadData(CCalcPlanKoef objCalcOrder, System.Boolean bNewCalc)
        {
            try
            {
                //System.String strStartProcess = "идет загрузка данных...";
                if (objCalcOrder == null) { return; }
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Boolean bPageExsits = false;
                System.String strTabName = objCalcOrder.Num;
                foreach (DevExpress.XtraTab.XtraTabPage tabPageItem in tabControl.TabPages)
                {
                    if (tabPageItem.Name == strTabName)
                    {
                        bPageExsits = true;
                        tabControl.SelectedTabPage = tabPageItem;
                        break;
                    }
                }
                if (bPageExsits == true) { return; }

                DevExpress.XtraTab.XtraTabPage tabPage = new DevExpress.XtraTab.XtraTabPage();
                if (tabPage != null)
                {
                    tabPage.Name = strTabName;
                    tabControl.TabPages.Add(tabPage);
                    tabControl.SelectedTabPage = tabPage;
                    if (bNewCalc == true)
                    {
                        tabPage.Image = ERPMercuryPlan.Properties.Resources.check2;
                        tabPage.Refresh();
                    }

                    ctrlCalcKoeffItem ViewerCalcOrderItem = new ctrlCalcKoeffItem(m_objProfile, objCalcOrder);
                    if (ViewerCalcOrderItem != null)
                    {
                        tabPage.Controls.Add(ViewerCalcOrderItem);
                        tabPage.Text = strTabName;
                        ViewerCalcOrderItem.Dock = DockStyle.Fill;

                        this.Refresh();

                        ViewerCalcOrderItem.LoadCalcOrderItems();
                        ViewerCalcOrderItem.ChangeCalcOrderItem += this.OnChangeCalcOrderItem;
                    }
                    this.Refresh();
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка загрузки плана закупок для ." + objCalcOrder.Num + "\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return;
        }
        private void SetPropertiesModified(System.Boolean bModified)
        {
            if (bModified == m_bPlanIsChanged) { return; }

            m_bPlanIsChanged = bModified;
        }

        private void OnChangeCalcOrderItem(Object sender, ChangeCalcOrderItemEventArgs e)
        {
            try
            {
                if (e.IsChanged == true)
                {
                    SetPropertiesModified(true);
                }
                if (tabControl.TabPages.Count > 0)
                {
                    foreach (DevExpress.XtraTab.XtraTabPage tabPage in tabControl.TabPages)
                    {
                        if (tabPage.Name == e.CalcOrder.Num)
                        {
                            tabPage.Image = (e.IsChanged == true) ? ERPMercuryPlan.Properties.Resources.warning : ERPMercuryPlan.Properties.Resources.check2;
                            tabPage.Refresh();
                            break;
                        }
                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangePlanVariable.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        #endregion

        #region Закрыть закладки
        private void tabControl_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                if ((tabControl.TabPages.Count == 0) || (tabControl.SelectedTabPage == null)) { return; }
                // закрываем закладку
                CloseTabPage(tabControl.SelectedTabPage, true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("tabControl_CloseButtonClick.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Закрывает закладку вместе с работающим в ней потоком
        /// </summary>
        /// <param name="tPage">закладка</param>
        /// <param name="bConfirm">Признак "Выдавать сообщение"</param>
        private void CloseTabPage(DevExpress.XtraTab.XtraTabPage tabPage, System.Boolean bConfirm)
        {
            System.String tPageText = "";
            try
            {
                if (tabControl.TabPages.Count == 0) { return; }
                if (tabPage == null) { return; }
                if (tabPage.Controls.Count == 0) { return; }
                tPageText = tabPage.Text;

                ctrlCalcKoeffItem ViewerPlanProductOwner = (ctrlCalcKoeffItem)tabPage.Controls[0];
                if (ViewerPlanProductOwner == null) { return; }

                if (ViewerPlanProductOwner.PlanIsChanged == true)
                {
                    DialogResult resDlg = DevExpress.XtraEditors.XtraMessageBox.Show("Сохранить изменения в расчете?", "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                    switch (resDlg)
                    {
                        case DialogResult.Yes:
                            {
                                //// попробуем сохранить
                                //if (bSavePlanOnlyOneProductOwner(tabPage) == true)
                                //{
                                    ViewerPlanProductOwner = null;
                                    tabControl.TabPages.Remove(tabPage);
                                    tabPage = null;
                                    this.Refresh();
                                //}
                                break;
                            }
                        case DialogResult.No:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    ViewerPlanProductOwner = null;
                    tabControl.TabPages.Remove(tabPage);
                    tabPage = null;
                    this.Refresh();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(tPageText + "\nCloseTabPage.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
            finally
            {
            }
            return;
        }
        private void frmCalcOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (tabControl.TabPages.Count > 0)
                {
                    System.Boolean bFindChanges = false;
                    foreach (DevExpress.XtraTab.XtraTabPage tabPage in tabControl.TabPages)
                    {
                        if (((ctrlCalcKoeffItem)tabPage.Controls[0]).PlanIsChanged == true)
                        {
                            bFindChanges = true;
                            break;
                        }
                    }
                    if (bFindChanges == true)
                    {
                        DialogResult resDlg = DevExpress.XtraEditors.XtraMessageBox.Show(
                        "План заказа поставщику был изменен.\nСохранить изменения в плане?", "Подтверждение",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);

                        switch (resDlg)
                        {
                            case DialogResult.Yes:
                                {
                                    //e.Cancel = (bSavePlan() == true) ? false : true;
                                    break;
                                }
                            case DialogResult.No:
                                break;
                            case DialogResult.Cancel:
                                {
                                    e.Cancel = true;
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка закрытия формы.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.frmCalcOrder_FormClosing);
                this.Close();
                this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCalcOrder_FormClosing);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка отмены изменений.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,  System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        #endregion

        #region Печать
        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (tabControl.TabPages.Count == 0) { return; }
            if (tabControl.SelectedTabPage == null) { return; }
            if (tabControl.SelectedTabPage.Controls.Count == 0) { return; }

            ctrlCalcKoeffItem ViewerPlanProductOwner = (ctrlCalcKoeffItem)tabControl.SelectedTabPage.Controls[0];
            if (ViewerPlanProductOwner == null) { return; }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SendMessageToLog("идет экспорт данных в Microsoft Excel...");
                ViewerPlanProductOwner.PrintCalcOrderItem();
                SendMessageToLog("завершен экспорт данных в Microsoft Excel");
                Cursor.Current = Cursors.Default;

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка печати\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Журнал сообщений
        private void SendMessageToLog(System.String strMessage)
        {
            try
            {
                m_objMenuItem.SimulateNewMessage(strMessage);
                this.Refresh();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SendMessageToLog.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Расчет заказа
        /// <summary>
        /// Расчет заказа
        /// </summary>
        private void CalcOrder()
        {
            try
            {
                frmCalcPlanKoefproperties objfrmProductOwnerList = new frmCalcPlanKoefproperties(m_objProfile);
                if (objfrmProductOwnerList != null)
                {
                    objfrmProductOwnerList.LoadCalcPlanKoefProperties(null);
                    if ((objfrmProductOwnerList.ShowDialog() == DialogResult.OK) &&
                        (objfrmProductOwnerList.SelectedCalcPlanKoef != null))
                    {
                        System.String strErr = "";
                        if (DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Выбран период для расчета: " + objfrmProductOwnerList.SelectedCalcPlanKoef.BeginDate.ToShortDateString() + " - " + 
                            objfrmProductOwnerList.SelectedCalcPlanKoef.EndDate.ToShortDateString() +  ".\n\nРасчет заказа может занять несколько минут.\nНачать расчет?", "Подтверждение",
                            System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            SendMessageToLog("идет расчет коэффициента для плана продаж ...");
                            this.Refresh();
                            this.Cursor = Cursors.WaitCursor;

                            List<ERP_Mercury.Common.CProductOwner> objProductOwnerList = objfrmProductOwnerList.CheckProductOwnerList;
                            List<ERP_Mercury.Common.CProductType> objProductTypeList = objfrmProductOwnerList.CheckProductTypeList;
                            if (objfrmProductOwnerList.SelectedCalcPlanKoef.CalcOrder(m_objProfile, null, objProductOwnerList, objProductTypeList, ref strErr) == true)
                            {
                                this.Cursor = Cursors.Default;
                                SendMessageToLog("расчет завершен");

                                this.splitContainerControl.SuspendLayout();
                                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                                m_objCalcOrderList.Add(objfrmProductOwnerList.SelectedCalcPlanKoef);

                                gridControlProductList.RefreshDataSource();

                                gridViewProductList.FocusedRowHandle = (gridViewProductList.RowCount - 1);

                                LoadData(objfrmProductOwnerList.SelectedCalcPlanKoef, true);

                                this.splitContainerControl.ResumeLayout(false);
                                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();

                                //DevExpress.XtraEditors.XtraMessageBox.Show(
                                //    "Коэффициенты успешно расчитаны. Нажмите кнопку \"Обновить\"", "Информация",
                                //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
                                SendMessageToLog("ошибка расчета коэффициента. " + strErr);
                                DevExpress.XtraEditors.XtraMessageBox.Show(
                                   "Ошибка расчета коэффициента.\n\nТекст ошибки: " + strErr, "Ошибка",
                                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                        }
                    }

                }
                objfrmProductOwnerList = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ошибка расчета коэффициента. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        private void barBtnCalcOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CalcOrder();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "barBtnCalcOrder_ItemClick.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Удаление расчета
        private void DeleteCalcPlanKoef (CCalcPlanKoef objSelectedCalcOrder)
        {
            if (objSelectedCalcOrder == null) { return; }
            try
            {
                if (objSelectedCalcOrder.Remove(m_objProfile) == true)
                {
                    m_objCalcOrderList.Remove(objSelectedCalcOrder);
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка просмотра приложения к расчету. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        private void barBtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CCalcPlanKoef objSelectedCalcOrder = GetSelectedCalcOrder();

                if (objSelectedCalcOrder != null)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show("Подтвердите удаление расчета: " + objSelectedCalcOrder.Num + "  " + objSelectedCalcOrder.ShipPeriod + ".", "Подтверждение",
                        System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        DeleteCalcPlanKoef(objSelectedCalcOrder);

                        gridControlProductList.RefreshDataSource();
                        this.Cursor = Cursors.Default;
                    }
                }
                
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления расчета. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;

        }

        #endregion

    }

    public class ViewCalcOrder : PlugIn.IClassTypeView
    {
        public override void Run(UniXP.Common.MENUITEM objMenuItem, System.String strCaption)
        {
            frmCalcOrder obj = new frmCalcOrder(objMenuItem.objProfile, objMenuItem);
            obj.Text = strCaption;
            obj.MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent;
            obj.Visible = true;
        }

    }

}
