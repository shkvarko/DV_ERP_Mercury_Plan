using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraExport;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraEditors;
using ERP_Mercury.Common;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERPMercuryPlan
{
    public partial class frmPartsPriceList : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CPartsPriceListItem> m_objPriceListCalcItemList;
        private List<CPriceType> m_objPriceTypeList;
        private CProductPriceList m_objProductPriceList;
        private List<CProduct> m_objPartsList;
        private frmPartsList m_objFrmPartsList;

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlPriceList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }


        private System.Boolean m_bIsComboBoxFill;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bIsChanged;
        public System.Boolean IsChanged
        {
            get { return m_bIsChanged; }
        }
        private CPartsPriceListItem m_objSelectedPriceListCalcItem;
        private System.Boolean m_bIsNew;
        System.Boolean m_bIsPriceListEditor;

        // потоки
        private System.Threading.Thread thrAddress;
        public System.Threading.Thread ThreadAddress
        {
            get { return thrAddress; }
        }
        private System.Threading.ManualResetEvent m_EventStopThread;
        public System.Threading.ManualResetEvent EventStopThread
        {
            get { return m_EventStopThread; }
        }
        private System.Threading.ManualResetEvent m_EventThreadStopped;
        public System.Threading.ManualResetEvent EventThreadStopped
        {
            get { return m_EventThreadStopped; }
        }
        public delegate void LoadPartsListDelegate();
        public LoadPartsListDelegate m_LoadPartsListDelegate;

        public delegate void SendMessageToLogDelegate(System.String strMessage);
        public SendMessageToLogDelegate m_SendMessageToLogDelegate;

        public delegate void SetProductNewListToGridDelegate(List<CProduct> objProductNewList);
        public SetProductNewListToGridDelegate m_SetProductNewListToGridDelegate;

        public delegate void SetProductListToFormDelegate(List<CProduct> objProductNewList);
        public SetProductListToFormDelegate m_SetProductListToFormDelegate;

        private const System.Int32 iThreadSleepTime = 1000;
        private System.Boolean m_bThreadFinishJob;
        

        #endregion

        #region Коструктор
        public frmPartsPriceList(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objPriceListCalcItemList = null;
            m_objProductPriceList = new CProductPriceList();
            m_bIsComboBoxFill = false;
            m_bIsNew = false;
            m_objPriceTypeList = null;
            m_objFrmPartsList = null;
            AddGridColumns();

            LoadComboBoxItems();

            LoadPriceList();

            m_bIsPriceListEditor = m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_PriceListEditor);
            if( m_bIsPriceListEditor == false )
            {
                btnAdd.Visible = false;
                btnDelete.Visible = false;
            }

            m_bDisableEvents = false;
            m_bIsChanged = false;
            StartThreadWithLoadData();
        }
        #endregion

        #region Настройки грида
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();
            ColumnView.Columns.Clear();
            AddGridColumn(ColumnView, "PartsID_Ib", "Код товара");
            AddGridColumn(ColumnView, "PartsName", "Товар");
            AddGridColumn(ColumnView, "ProductOwner", "Товарная марка");
            AddGridColumn(ColumnView, "ProductType", "Товарная группа");
            AddGridColumn(ColumnView, "ProductSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnView, "ProductLineName", "Товарная линия");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                objColumn.Visible = (objColumn.FieldName != "ID");

            }

               foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
                {
                    if ((objColumn.FieldName == "PartsName") || (objColumn.FieldName == "ProductOwner") || (objColumn.FieldName == "ProductType") || (objColumn.FieldName == "ProductLineName") || (objColumn.FieldName == "ProductSubTypeName"))
                    {
                        objColumn.BestFit();
                    }
                    //objColumn.Fixed = (((objColumn.FieldName == "IsSetToPart") || (objColumn.FieldName == "SubTypeStateName") || (objColumn.FieldName == "Name")) ? DevExpress.XtraGrid.Columns.FixedStyle.Left : DevExpress.XtraGrid.Columns.FixedStyle.None);
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
        #endregion

        #region Содержимое прайс-листа

        #region Выпадающие списки
        /// <summary>
        /// загружает информацию в выпадающие списки
        /// </summary>
        private void LoadComboBoxItems()
        {
            if (m_bIsComboBoxFill == true) { return; }
            m_bDisableEvents = true;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // список типов цен
                treeListPriceEditor.Nodes.Clear();

                m_objPriceTypeList = CPriceType.GetPriceTypeList(m_objProfile, null);
                if (m_objPriceTypeList != null)
                {
                    foreach (CPriceType objItem in m_objPriceTypeList)
                    {
                        this.treeListPriceEditor.AppendNode(new object[] { objItem.Name, System.String.Format("{0:### ### ##0.000}", 0) }, null).Tag = objItem;
                    }
                }


                m_bIsComboBoxFill = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления выпадающих списков. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                this.Cursor = Cursors.Default;
            }

            return;
        }
        #endregion

        /// <summary>
        /// Загружает содержимое прайс-листа
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadPriceList()
        {
            System.Boolean bRet = false;
            m_bDisableEvents = true;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).BeginInit();

                gridControlPriceList.DataSource = null;

                if(m_objProductPriceList.LoadPriceList( m_objProfile, null ) == true )
                {
                    m_objPriceListCalcItemList = m_objProductPriceList.PriceItemmList;
                }

                if (m_objPriceListCalcItemList != null)
                {
                    gridControlPriceList.DataSource = m_objPriceListCalcItemList;

                }


                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).EndInit();
                tableLayoutPanel1.Refresh();
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                this.Cursor = Cursors.Default;
            }

            return bRet;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPriceList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Список цен
        /// <summary>
        /// Возвращает ссылку на выбранную позицию в гриде
        /// </summary>
        /// <returns>ссылка на товар</returns>
        private CPartsPriceListItem GetSelectedPriceListCalcItem()
        {
            CPartsPriceListItem objRet = null;
            try
            {
                if ((((DevExpress.XtraGrid.Views.Grid.GridView)gridControlPriceList.MainView).RowCount > 0) &&
                    (((DevExpress.XtraGrid.Views.Grid.GridView)gridControlPriceList.MainView).FocusedRowHandle >= 0))
                {
                    System.Int32 Ib_ID = (System.Int32)(((DevExpress.XtraGrid.Views.Grid.GridView)gridControlPriceList.MainView)).GetFocusedRowCellValue("PartsID_Ib");

                    objRet = m_objPriceListCalcItemList.Single<CPartsPriceListItem>(x => x.PartsID_Ib.CompareTo(Ib_ID) == 0);
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка поиска строки прайс-листа. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return objRet;
        }
        /// <summary>
        /// Отображает список цен для строки расчета
        /// </summary>
        /// <param name="objPriceListCalcItem">строка расчета</param>
        private void ShowPriceListCalcItem(CPartsPriceListItem objPriceListCalcItem)
        {
            try
            {
                this.splitContainerControl1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).BeginInit();

                treeListPrices.Nodes.Clear();
                if (objPriceListCalcItem != null)
                {
                    if ((objPriceListCalcItem.PriceList == null) || (objPriceListCalcItem.PriceList.Count == 0))
                    {
                        objPriceListCalcItem.LoadPriceListForProduct(m_objProfile, null);
                    }

                    if (objPriceListCalcItem.PriceList != null)
                    {
                        foreach (CPrice objPrice in objPriceListCalcItem.PriceList)
                        {
                            if ((objPrice.PriceType.IsShowInPrice == false) && (m_bIsPriceListEditor == false)) { continue; }

                            this.treeListPrices.AppendNode(new object[] { objPrice.PriceType.Name, System.String.Format("{0:### ### ##0.000}", objPrice.PriceValue) }, null);
                        }
                    }
                }

                this.splitContainerControl1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отображения списка цен для строки прайс-листа. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void gridViewPriceList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                ShowPriceListCalcItem(GetSelectedPriceListCalcItem());
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка смены записи в списке. Текст ошибки: " + f.Message);
            }
        }
        #endregion

        #region Журнал сообщений
        private void SendMessageToLog(System.String strMessage)
        {
            try
            {
                m_objMenuItem.SimulateNewMessage(strMessage);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SendMessageToLog. Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Индикация изменений
        /// Проверяет все ли обязательные поля заполнены
        /// </summary>
        /// <returns>true - все обязательные поля заполнены; false - не все полязаполнены</returns>
        private System.Boolean IsAllParamFill()
        {
            System.Boolean bRet = false;
            try
            {
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки заполнения обязательных свойств позиции прайс-листа. Текст ошибки: " + f.Message);
            }
            return bRet;
        }
        private void treeListPriceEditor_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("treeListPriceEditor_CellValueChanging. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void SetPropertiesModified(System.Boolean bModified)
        {
            m_bIsChanged = bModified;
            if (m_bIsChanged == true)
            {
                btnSave.Enabled = IsAllParamFill();
            }
            else
            {
                btnSave.Enabled = m_bIsChanged;
            }

            btnCancel.Enabled = bModified;
        }
        /// <summary>
        /// Включает/отключает элементы управления для отображения свойств 
        /// </summary>
        /// <param name="bEnable">признак "включить/выключить"</param>
        private void SetReadOnlyPropertiesControls(System.Boolean bReadOnly)
        {
            try
            {
                colPriceValue.OptionsColumn.ReadOnly = bReadOnly;
                colPriceValue.OptionsColumn.AllowEdit = !bReadOnly;

                if (bReadOnly == true)
                {
                    // включен режим "только просмотр"
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                }
                else
                {
                    // включен режим "редактирование"
                    btnSave.Enabled = m_bIsChanged;
                    btnCancel.Enabled = true;
                }
                btnEdit.Enabled = bReadOnly;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetReadOnlyPropertiesControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void PriceList_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("PriceList_EditValueChanging. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }

        private void PriceList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("PriceList_SelectedValueChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }
        #endregion
        
        #region Редактирование прайс-листа
        /// <summary>
        /// загружает свойства строки прайс-листа в элементы управления
        /// </summary>
        /// <param name="objPriceListItem">строка прайс-листа</param>
        private void LoadPriceListItemInEditor(CPartsPriceListItem objPriceListItem)
        {
            try
            {
                if (objPriceListItem == null) { return; }
                m_bDisableEvents = true;

                m_objSelectedPriceListCalcItem = objPriceListItem;
                m_objSelectedPriceListCalcItem.LoadPriceListForProduct(m_objProfile, null);
                lblCustomerIfo.Text = (m_objSelectedPriceListCalcItem.ProductOwner + " " + m_objSelectedPriceListCalcItem.ProductType);
                txtSubType.Text = m_objSelectedPriceListCalcItem.ProductSubTypeName;

                CPriceType objPriceType = null;
                System.Boolean bExists = false;
                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes )
                {
                    bExists = false;
                    if (objNode.Tag != null)
                    {
                        objPriceType = ( CPriceType )objNode.Tag;

                        if(m_objSelectedPriceListCalcItem.PriceList != null)
                        {
                            foreach (CPrice objPrice in m_objSelectedPriceListCalcItem.PriceList)
                            {
                                if (objPrice.PriceType.ID.CompareTo(objPriceType.ID) == 0)
                                {
                                    bExists = true;
                                    objNode.SetValue(colPriceValue, objPrice.PriceValue);
                                    break;
                                }
                            }
                        }
                    }
                    if (bExists == false)
                    {
                        objNode.SetValue(colPriceValue, 0);
                    }
                }

                SetReadOnlyPropertiesControls(true);
                SetPropertiesModified(false);

                tabControl.SelectedTabPage = tabDetail;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования позиции в прайс-листе. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                btnCancel.Enabled = true;
            }
            return;
        }

        private void gridControlPriceList_DoubleClick(object sender, EventArgs e)
        {
            if (m_bIsPriceListEditor == false) { return; }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CPartsPriceListItem objPriceListItem = GetSelectedPriceListCalcItem();
                m_bIsNew = false;
                LoadPriceListItemInEditor( objPriceListItem );
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования строки прайс-листа. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Установка режима просмотра/редактирования расчета цен
        /// </summary>
        /// <param name="bReadOnly"></param>
        private void SetModeReadOnlyForProductDetail(System.Boolean bReadOnly)
        {
            try
            {
                btnEdit.Enabled = bReadOnly;
                btnSave.Enabled = !bReadOnly;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetModeReadOnlyForProductDetail. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                SetReadOnlyPropertiesControls(false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnEdit_Click. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Сохранение/отмена изменений
        /// <summary>
        /// Отменяет изменения в составе набора
        /// </summary>
        private void CancelChanges()
        {
            try
            {
                tabControl.SelectedTabPage = tabView;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                CancelChanges();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnCancel_Click. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmPriceList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSave_Click. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Сохраняет изменения в прайс-листе
        /// </summary>
        private void ConfirmPriceList()
        {
            try
            {
                if (m_objSelectedPriceListCalcItem != null)
                {
                    System.String strErr = "";

                    if (IsAllParamFill() == true)
                    {
                        CPartsPriceListItem objPriceListItemForSave = new CPartsPriceListItem();
                        objPriceListItemForSave.Product = m_objSelectedPriceListCalcItem.Product;
                        objPriceListItemForSave.PriceList = new List<CPrice>();
                        CPriceType objPriceType = null;
                         List<CPriceType> objPriceTypeCheckedList = new List<CPriceType>();
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes)
                        {
                            if (objNode.Tag == null) { continue; }
                            objPriceType = ( CPriceType )objNode.Tag;
                            objPriceTypeCheckedList.Add( objPriceType );

                            objPriceListItemForSave.PriceList.Add(new CPrice(objPriceType, System.Convert.ToDouble( objNode.GetValue( colPriceValue ))));

                        }

                        objPriceType = null;

                        List<CPartsPriceListItem> objPriceListItemsList = new List<CPartsPriceListItem>();
                        objPriceListItemsList.Add( objPriceListItemForSave );
                        
                        // сперва пишем в IB
                        System.Boolean bIsOkSave = CProductPriceList.SavePriceListToIB(objPriceListItemsList, objPriceTypeCheckedList, m_objProfile, ref strErr);
                        if (bIsOkSave == true)
                        {
                            bIsOkSave = CProductPriceList.SavePriceList(objPriceListItemsList, m_objProfile, null, ref strErr);
                        }

                        if (bIsOkSave == true)
                        {
                            m_objSelectedPriceListCalcItem.PriceList = objPriceListItemForSave.PriceList;

                            if (m_bIsNew == true)
                            {
                                // новая запись в прайсе
                                m_objPriceListCalcItemList.Add( m_objSelectedPriceListCalcItem );
                                gridControlPriceList.RefreshDataSource();
                            }
                            else
                            {
                                gridControlPriceList.RefreshDataSource();
                            }
                            tabControl.SelectedTabPage = tabView;
                        }
                        else
                        {
                            SendMessageToLog(strErr);
                        }
                        objPriceListItemForSave = null;
                    }

                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в прайс-листе. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Удаление строк в прайсе
        /// <summary>
        /// Удаление строк в прайсе
        /// </summary>
        private void DeletePriceListItem( )
        {
            System.String strErr = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<CPartsPriceListItem> objPriceListCalcItemList = new List<CPartsPriceListItem>();

                int[] arr = gridViewPriceList.GetSelectedRows();
                if (arr.Length > 0)
                {
                    for (System.Int32 i = 0; i < arr.Length; i++)
                    {
                        objPriceListCalcItemList.Add(m_objPriceListCalcItemList[gridViewPriceList.GetDataSourceRowIndex(arr[i])]);
                    }
                    if (DevExpress.XtraEditors.XtraMessageBox.Show("Подтвердите удаление выбранных записей. ", "Подтверждение",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("В списке необходимо выделить записи.", "Сообщение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return ;
                }

                System.Int32 iDeletedObjIndx = -1;

                System.Boolean bDeleteOK = CProductPriceList.DeletePriceListFromIB(objPriceListCalcItemList, m_objProfile, ref strErr);
                if (bDeleteOK == true)
                {
                    if (CProductPriceList.DeletePriceListItems(objPriceListCalcItemList, m_objProfile, null, ref strErr) == true)
                    {
                        this.tableLayoutPanel1.SuspendLayout();
                        ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).BeginInit();

                        // в БД мы всё удалили, теперь нужно удалить в списке
                        foreach (CPartsPriceListItem objDeleted in objPriceListCalcItemList)
                        {
                            iDeletedObjIndx = -1;
                            for (System.Int32 i = 0; i < m_objPriceListCalcItemList.Count; i++)
                            {
                                if (m_objPriceListCalcItemList[i].Product.ID.CompareTo(objDeleted.Product.ID) == 0)
                                {
                                    iDeletedObjIndx = i;
                                    break;
                                }
                            }
                            if (iDeletedObjIndx >= 0) { m_objPriceListCalcItemList.RemoveAt(iDeletedObjIndx); }
                        }


                        gridControlPriceList.RefreshDataSource();
                        if (((iDeletedObjIndx - 1) < 0) && (m_objPriceListCalcItemList.Count > 0)) { iDeletedObjIndx = 0; }
                        else
                        {
                            if (((iDeletedObjIndx - 1) >= 0) && (m_objPriceListCalcItemList.Count > 0)) { iDeletedObjIndx = iDeletedObjIndx--; }
                        }

                        gridViewPriceList.FocusedRowHandle = iDeletedObjIndx;

                        this.tableLayoutPanel1.ResumeLayout(false);
                        ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).EndInit();

                        SendMessageToLog("Выбранные записи удалены из прайс-листа.");
                        DevExpress.XtraEditors.XtraMessageBox.Show("Выбранные записи удалены из прайс-листа.", "Информация",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                    }
                    else
                    {
                        SendMessageToLog(strErr);
                    }
                }
                else
                {
                    SendMessageToLog(strErr);
                }


            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления записей из прайс-листа. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeletePriceListItem();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления строк в прайс-листе. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }


        #endregion

        #region Печать
        private void btnExportForCalcPrice_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ExportPriceListToExcel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка экспорта прайс-листа в MS Excel. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void ExportPriceListToExcel()
        {
            List<CPartsPriceListItem> objPriceList = new List<CPartsPriceListItem>();
            try
            {
                for (System.Int32 i = 0; i < gridViewPriceList.RowCount; i++)
                {
                    objPriceList.Add(m_objPriceListCalcItemList[gridViewPriceList.GetDataSourceRowIndex(i)]);
                }
                CProductPriceList.ExportToExcel(objPriceList, m_objPriceTypeList, m_bIsPriceListEditor, m_objProfile);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка экспорта прайс-листа в MS Excel. Текст ошибки: " + f.Message);
            }
            finally
            {
                objPriceList = null;
            }

            return;

        }
        #endregion

        #region Экспорт в MS Excel
        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            //string name = Application.ProductName;
            string name = "Прайс-лист (товары)";
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Экспорт списка товаров " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }
        private void OpenFile(string fileName)
        {
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("Вы хотите открыть этот файл?", "Экспорт в MS Excel...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = fileName;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        process.Start();
                    }
                    catch
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(this, "Cannot find an application on your system suitable for openning the file with exported data.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка метода OpenFile.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        //<sbExportToHTML>
        private void ExportTo(DevExpress.XtraExport.IExportProvider provider,
            DevExpress.XtraGrid.Views.Grid.GridView objGridView)
        {
            try
            {
                Cursor currentCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                this.FindForm().Refresh();

                BaseExportLink link = objGridView.CreateExportLink(provider);
                (link as GridViewExportLink).ExpandAll = false;
                //link.Progress += new DevExpress.XtraGrid.Export.ProgressEventHandler(Export_Progress);
                link.ExportTo(true);
                provider.Dispose();
                //link.Progress -= new DevExpress.XtraGrid.Export.ProgressEventHandler(Export_Progress);

                Cursor.Current = currentCursor;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка метода OpenFile.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void sbExportToXLS_Click(object sender, System.EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView objGridView = gridViewPriceList;

                if ((objGridView == null) || (objGridView.RowCount == 0))
                {
                    return;
                }
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                if (fileName != "")
                {
                    ExportTo(new ExportXlsProvider(fileName), objGridView);
                    OpenFile(fileName);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка метода sbExportToXLS_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Потоки
        /// <summary>
        /// Создает форму со списком товара
        /// </summary>
        /// <param name="objPartsList">списком товара</param>
        private void SetProductListToForm(List<CProduct> objPartsList)
        {
            try
            {
                if (m_objFrmPartsList == null)
                {
                    m_objFrmPartsList = new frmPartsList(m_objProfile, m_objMenuItem, objPartsList);
                    btnAdd.Visible = true;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetProductListToForm. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        /// <summary>
        /// загружает список товаров и список новых товаров
        /// </summary>
        private void LoadPartssList()
        {
            try
            {
                // товары
                m_objPartsList = CProduct.GetProductList(m_objProfile, null, false);
                if (m_objPartsList != null)
                {
                    this.Invoke(m_SetProductListToFormDelegate, new Object[] { m_objPartsList });
                }
            }
            catch (System.Exception f)
            {
                this.Invoke(m_SendMessageToLogDelegate, new Object[] { ("Ошибка обновления списка новинок. Текст ошибки: " + f.Message) });
            }
            finally
            {
                EventStopThread.Set();
            }

            return;
        }

        public void StartThreadWithLoadData()
        {
            try
            {
                // инициализируем события
                this.m_EventStopThread = new System.Threading.ManualResetEvent(false);
                this.m_EventThreadStopped = new System.Threading.ManualResetEvent(false);

                // инициализируем делегаты
                m_LoadPartsListDelegate = new LoadPartsListDelegate(LoadPartssList);
                m_SetProductListToFormDelegate = new SetProductListToFormDelegate(SetProductListToForm);
                m_SendMessageToLogDelegate = new SendMessageToLogDelegate(SendMessageToLog);

                // запуск потока
                StartThread();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadWithLoadData().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void StartThread()
        {
            try
            {
                // делаем событиям reset
                this.m_EventStopThread.Reset();
                this.m_EventThreadStopped.Reset();

                this.thrAddress = new System.Threading.Thread(WorkerThreadFunction);
                this.thrAddress.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThread().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        public void WorkerThreadFunction()
        {
            try
            {
                Run();
            }
            catch (System.Exception e)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("WorkerThreadFunction\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }

        public void Run()
        {
            try
            {
                LoadPartssList();

                // пока заполняется список товаров будем проверять, не было ли сигнала прекратить все это
                while (this.m_bThreadFinishJob == false)
                {
                    // проверим, а не попросили ли нас закрыться
                    if (this.m_EventStopThread.WaitOne(iThreadSleepTime, true))
                    {
                        this.m_EventThreadStopped.Set();
                        break;
                    }
                }

            }
            catch (System.Exception e)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Run\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }
        /// <summary>
        /// Делает пометку о необходимости остановить поток
        /// </summary>
        public void TreadIsFree()
        {
            try
            {
                this.m_bThreadFinishJob = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StopPleaseTread() " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }

        private System.Boolean bIsThreadsActive()
        {
            System.Boolean bRet = false;
            try
            {
                bRet = (
                    ((ThreadAddress != null) && (ThreadAddress.IsAlive == true))
                    );
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("bIsThreadsActive.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }

        private void CloseThreadInAddressEditor()
        {
            try
            {
                if (bIsThreadsActive() == true)
                {
                    if ((ThreadAddress != null) && (ThreadAddress.IsAlive == true))
                    {
                        EventStopThread.Set();
                    }
                }
                while (bIsThreadsActive() == true)
                {
                    if (System.Threading.WaitHandle.WaitAll((new System.Threading.ManualResetEvent[] { EventThreadStopped }), 100, true))
                    {
                        break;
                    }
                    Application.DoEvents();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("bIsThreadsActive.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Добавление позиций в прайс
        /// <summary>
        /// добавление позиций в парйс
        /// </summary>
        private void AddpartsListToPriceList()
        {
            try
            {
                if (m_objFrmPartsList == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Выполняется запрос списка товаров. Пожалуйста, подождите несколько секунд и повторите попытку.", "Информация",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                if (m_objFrmPartsList != null)
                {
                    DialogResult dlgRes = m_objFrmPartsList.ShowDialog();
                    if ((dlgRes == DialogResult.OK) && (m_objFrmPartsList.SelectedProductList != null) && (m_objFrmPartsList.SelectedProductList.Count > 0))
                    {
                        // делаем дополнительный запрос
                        if (m_objFrmPartsList.SelectedProductList.Count >= 2)
                        {
                            if (DevExpress.XtraEditors.XtraMessageBox.Show("Внимание!\nСписок товаров, которые Вы хотите добавить в прайс, содержит " + m_objFrmPartsList.SelectedProductList.Count.ToString() + " позиций.\n" +
                                "Пожалуйста, подтвердите начало операции.", "Информация", System.Windows.Forms.MessageBoxButtons.YesNo,
                                System.Windows.Forms.MessageBoxIcon.Warning) == DialogResult.No)
                            {
                                return;
                            }
                        }

                        // сперва сохраним в IB
                        System.String strErr = "";
                        System.Boolean bSaveOk = CProductPriceList.AddProductListToPartsPriceList(m_objFrmPartsList.SelectedProductList, m_objProfile, null, ref strErr);
                        if (bSaveOk == true)
                        {
                            SendMessageToLog("Товары добавлены в прайс-лист с нулевыми ценами.");
                            DevExpress.XtraEditors.XtraMessageBox.Show(
                                "Товары добавлены в прайс-лист с нулевыми ценами.\nЧтобы увидеть их, обновите, пожалуйста, прайс-лист.", "Информация",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                        else
                        {
                            SendMessageToLog(strErr);
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка AddpartsListToPriceList. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddpartsListToPriceList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка btnAdd_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        private void gridViewPriceList_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                ShowPriceListCalcItem(GetSelectedPriceListCalcItem());
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewPriceList_RowCountChanged. Текст ошибки: " + f.Message);
            }

        }

        #region Импорт цен в прайс-лист
        private void mitemImportPrices_Click(object sender, EventArgs e)
        {
            try
            {
                ImportPrices();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewPriceList_RowCountChanged. Текст ошибки: " + f.Message);
            }
        }

        private void ImportPrices()
        {
            try
            {
                frmImportInPartsPriceList objFrmImportInPartsPriceList = new frmImportInPartsPriceList(m_objProfile);
                if (objFrmImportInPartsPriceList != null)
                {
                    objFrmImportInPartsPriceList.OpenForm();
                    objFrmImportInPartsPriceList.ShowDialog();
                    
                    objFrmImportInPartsPriceList.Dispose();
                    objFrmImportInPartsPriceList = null;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ImportPrices. Текст ошибки: " + f.Message);
            }
        }

        #endregion

    }

    public class ViewPartsPriceList : PlugIn.IClassTypeView
    {
        public override void Run(UniXP.Common.MENUITEM objMenuItem, System.String strCaption)
        {
            frmPartsPriceList obj = new frmPartsPriceList(objMenuItem.objProfile, objMenuItem);
            obj.Text = strCaption;
            obj.MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent;
            obj.Visible = true;
        }

    }

}
