using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP_Mercury.Common;


namespace ERPMercuryPlan
{
    public partial class frmPriceListCalc : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CPriceListCalc> m_objPriceListCalcList;
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControl.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewPriceList
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
        private CPriceListCalc m_objSelectedPriceListCalc;
        private System.Boolean m_bNeedSaveFileXLS;
        private List<CProductSubType> m_objProductSubTypeList;
        private List<CPriceType> m_objPriceTypeList;

        #endregion

        #region Конструктор
        public frmPriceListCalc(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objPriceListCalcList = null;
            m_objSelectedPriceListCalc = null;
            m_bIsComboBoxFill = false;
            m_objPriceTypeList = null;

            m_objProductSubTypeList = CProductSubType.GetProductSubTypeList(m_objProfile, null, false);

            AddGridColumns();
            AddGridColumnsToPriceList();

            LoadPriceListCalc();

            m_bDisableEvents = false;
            m_bIsChanged = false;
            m_bNeedSaveFileXLS = false;
            tabControl.SelectedTabPage = tabView;

        }
        #endregion
 
        #region Настройки грида
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();
            AddGridColumn(ColumnView, "ID", "Код");
            AddGridColumn(ColumnView, "IsActive", "Активен");
            AddGridColumn(ColumnView, "Name", "Наименование");
            AddGridColumn(ColumnView, "DocDate", "Дата");
            AddGridColumn(ColumnView, "FileNameXLS", "Файл расчета");
            AddGridColumn(ColumnView, "Description", "Описание");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                objColumn.Visible = (objColumn.FieldName != "ID");
                if (objColumn.FieldName == "IsActive") { objColumn.Width = 30; }
            }
        }
        private void AddGridColumnsToPriceList()
        {
            ColumnViewPriceList.Columns.Clear();
            AddGridColumn(ColumnViewPriceList, "ProductOwner", "Товарная марка");
            AddGridColumn(ColumnViewPriceList, "PartSubTypeID_Ib", "Код подгруппы");
            AddGridColumn(ColumnViewPriceList, "PartSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnViewPriceList, "VendorTariff", "Тариф поставщика");
            AddGridColumn(ColumnViewPriceList, "TransportTariff", "Тариф транспортный");
            AddGridColumn(ColumnViewPriceList, "TransportCost", "Затраты на транспорт");
            AddGridColumn(ColumnViewPriceList, "CustomsTariff", "Тариф таможенный");
            AddGridColumn(ColumnViewPriceList, "CustomCost", "Затраты на таможню");
            AddGridColumn(ColumnViewPriceList, "Margin", "Наценка базовая");
            AddGridColumn(ColumnViewPriceList, "NDS", "НДС");
            AddGridColumn(ColumnViewPriceList, "NDSCost", "Сумма с НДС");
            AddGridColumn(ColumnViewPriceList, "PriceCurrencyRate", "Курс ценообразования");
            AddGridColumn(ColumnViewPriceList, "Discont", "Наценка, компенс-я скидку");
            AddGridColumn(ColumnViewPriceList, "MarkUpRequired", "Требуемая наценка");
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

        #region Список расчетов цен
        /// <summary>
        /// Загружает список расчетов цен
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadPriceListCalc()
        {
            System.Boolean bRet = false;
            m_bDisableEvents = true;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();

                gridControl.DataSource = null;

                m_objPriceListCalcList = CPriceListCalc.GetPriceListCalcList( m_objProfile, null );

                if (m_objPriceListCalcList != null)
                {
                    gridControl.DataSource = m_objPriceListCalcList;

                }


                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
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
                LoadPriceListCalc();
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
    //            this.tableLayoutPanel1.SuspendLayout();

                // список типов цен
                treeListPriceType.Nodes.Clear();

                m_objPriceTypeList = CPriceType.GetPriceTypeList(m_objProfile, null);
                if (m_objPriceTypeList != null)
                {
                    foreach (CPriceType objItem in m_objPriceTypeList)
                    {
                        this.treeListPriceType.AppendNode(new object[] { false, objItem.Name }, null).Tag = objItem;
                    }
                }

   //             this.tableLayoutPanel1.ResumeLayout(false);
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

        #region Редактирование расчета цен
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddPriceListCalc();
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания расчета цен. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void AddPriceListCalc()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                m_objSelectedPriceListCalc = new CPriceListCalc();
                m_objSelectedPriceListCalc.DocDate = System.DateTime.Today;
                m_objSelectedPriceListCalc.IsActive = true;
                m_objSelectedPriceListCalc.CalcItemList = new List<CPriceListCalcItem>();
                LoadPriceListCalcInEditor( m_objSelectedPriceListCalc );
                SetReadOnlyPropertiesControls( false );
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания расчета цен. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Возвращает ссылку на выбранный в списке расчет цен
        /// </summary>
        /// <returns>ссылка на расчет цен</returns>
        private CPriceListCalc GetSelectedPriceListCalc()
        {
            CPriceListCalc objRet = null;
            try
            {
                if ((((DevExpress.XtraGrid.Views.Grid.GridView)gridControl.MainView).RowCount > 0) &&
                    (((DevExpress.XtraGrid.Views.Grid.GridView)gridControl.MainView).FocusedRowHandle >= 0))
                {
                    System.Guid uuidID = (System.Guid)(((DevExpress.XtraGrid.Views.Grid.GridView)gridControl.MainView)).GetFocusedRowCellValue("ID");

                    if ((m_objPriceListCalcList != null) && (m_objPriceListCalcList.Count > 0) && (uuidID.CompareTo(System.Guid.Empty) != 0))
                    {
                        foreach (CPriceListCalc objProduct in m_objPriceListCalcList)
                        {
                            if (objProduct.ID.CompareTo(uuidID) == 0)
                            {
                                objRet = objProduct;
                                break;
                            }
                        }
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка поиска выбранного расчета цен. Текст ошибки: " + f.Message);
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
                CPriceListCalc objPriceListCalc = GetSelectedPriceListCalc();
                LoadPriceListCalcInEditor(objPriceListCalc);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования расчета цен. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Редактирование расчета цен
        /// </summary>
        /// <param name="objPriceListCalc">выбранный расчет цен</param>
        private void LoadPriceListCalcInEditor( CPriceListCalc objPriceListCalc )
        {
            try
            {
                if (objPriceListCalc == null) { return; }
                m_bDisableEvents = true;
                if (m_bIsComboBoxFill == false) { LoadComboBoxItems(); }

                m_objSelectedPriceListCalc = objPriceListCalc;
                lblCustomerIfo.Text = objPriceListCalc.Name;

                this.tableLayoutPanelDetail.SuspendLayout();

                txtName.Text = objPriceListCalc.Name;
                dateBeginDate.DateTime = objPriceListCalc.DocDate;
                txtDscrpn.Text = objPriceListCalc.Description;
                checkActive.Checked = objPriceListCalc.IsActive;
                txtFileName.Text = objPriceListCalc.FileNameXLS;

                gridControlPriceList.DataSource = null;
                treeListPartsDetail.ClearNodes();


                //if ((m_objSelectedPriceListCalc.CalcItemList == null) || (m_objSelectedPriceListCalc.CalcItemList.Count == 0))
                //{
                    if (m_objSelectedPriceListCalc.LoadCalcItemList(m_objProfile, null) == true)
                    {
                        if( (m_objSelectedPriceListCalc.CalcItemList != null) && (m_objSelectedPriceListCalc.CalcItemList.Count > 0) )
                        {
                            LoadPriceListCalcItems(m_objSelectedPriceListCalc);
                            gridViewPriceListCalc.FocusedRowHandle = 0;
                        }
                    }
                //}

                this.tableLayoutPanelDetail.ResumeLayout(false);

                SetReadOnlyPropertiesControls(true);
                SetPropertiesModified(false);

                tabControl.SelectedTabPage = tabDetail;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования расчета цен. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                btnCancel.Enabled = true;
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
        /// <summary>
        /// Загружает в таблицу содержимое расчета цен
        /// </summary>
        /// <param name="objPriceListCalc">Расчет цен</param>
        private void LoadPriceListCalcItems(CPriceListCalc objPriceListCalc)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.tableLayoutPanelDetail.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).BeginInit();

                gridControlPriceList.DataSource = objPriceListCalc.CalcItemList;

                this.tableLayoutPanelDetail.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).EndInit();
                tableLayoutPanelDetail.Refresh();

            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadPriceListCalcItems. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        /// <summary>
        /// Возвращает ссылку на выбранную позицию в приложении к расчету
        /// </summary>
        /// <returns>ссылка на товар</returns>
        private CPriceListCalcItem GetSelectedPriceListCalcItem()
        {
            CPriceListCalcItem objRet = null;
            try
            {
                if ((((DevExpress.XtraGrid.Views.Grid.GridView)gridControlPriceList.MainView).RowCount > 0) &&
                    (((DevExpress.XtraGrid.Views.Grid.GridView)gridControlPriceList.MainView).FocusedRowHandle >= 0))
                {
                    System.Int32 Ib_ID = (System.Int32)(((DevExpress.XtraGrid.Views.Grid.GridView)gridControlPriceList.MainView)).GetFocusedRowCellValue("PartSubTypeID_Ib");

                    if ((m_objSelectedPriceListCalc != null) && (m_objSelectedPriceListCalc.CalcItemList != null) && (m_objSelectedPriceListCalc.CalcItemList.Count > 0) && (Ib_ID != 0))
                    {
                        foreach (CPriceListCalcItem objCalcItem in m_objSelectedPriceListCalc.CalcItemList)
                        {
                            if (objCalcItem.PartSubTypeID_Ib == Ib_ID)
                            {
                                objRet = objCalcItem;
                                break;
                            }
                        }
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка поиска строки расчета. Текст ошибки: " + f.Message);
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
        private void ShowPriceListCalcItem(CPriceListCalcItem objPriceListCalcItem)
        {
            try
            {
                this.splitContainerControl.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPartsDetail)).BeginInit();

                treeListPartsDetail.Nodes.Clear();

                if( (objPriceListCalcItem != null) && (objPriceListCalcItem.PriceList != null) )
                {
                    foreach (CPrice objPrice in objPriceListCalcItem.PriceList)
                    {
                        treeListPartsDetail.AppendNode(new object[] { objPrice.PriceType.Name, System.String.Format("{0:### ### ##0.000}", objPrice.PriceValue) }, null);
                    }
                }

                this.splitContainerControl.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListPartsDetail)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отображения списка цен для строки расчета. Текст ошибки: " + f.Message);
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

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            ValidateProperties();
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
        private void SetReadOnlyPropertiesControls(System.Boolean bEnable)
        {
            try
            {
                txtName.Properties.ReadOnly = bEnable;
                dateBeginDate.Properties.ReadOnly = true;
                txtDscrpn.Properties.ReadOnly = bEnable;
                checkActive.Properties.ReadOnly = bEnable;
                btnLoadFile.Enabled = bEnable;
                btnClearFile.Enabled = bEnable;
                btnLoadFileToDisk.Enabled = bEnable && (m_objSelectedPriceListCalc.ID.CompareTo(System.Guid.Empty) != 0);

                if (bEnable == true)
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
                btnEdit.Enabled = bEnable;
                //btnSetToPartList.Enabled = (!bEnable) && (m_objSelectedProduct.ID.CompareTo(System.Guid.Empty) != 0);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("DisableEnablePropertiesControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void ProductProperties_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }

        private void ProductProperties_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }
        #endregion

        #region Сохранение/отмена изменений
        /// <summary>
        /// Проверяет все ли обязательные поля заполнены
        /// </summary>
        /// <returns>true - все обязательные поля заполнены; false - не все полязаполнены</returns>
        private System.Boolean IsAllParamFill()
        {
            System.Boolean bRet = false;
            try
            {
                bRet = (txtName.Text != "");
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки заполнения обязательных свойств расчета цен. Текст ошибки: " + f.Message);
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет содержимое элементов управления
        /// </summary>
        private void ValidateProperties()
        {
            try
            {
                txtName.Properties.Appearance.BackColor = ((txtName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                ////txtProductType.Properties.Appearance.BackColor = ((txtProductType.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                ////txtProductOwner.Properties.Appearance.BackColor = ((txtProductOwner.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                //calcPriceEXW.Properties.Appearance.BackColor = ((calcPriceEXW.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                //calcCustomTariff.Properties.Appearance.BackColor = ((calcPriceEXW.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                //calcDiscont.Properties.Appearance.BackColor = ((calcPriceEXW.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                //calcMargin.Properties.Appearance.BackColor = ((calcPriceEXW.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                //calcNDS.Properties.Appearance.BackColor = ((calcPriceEXW.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                //calcTransportTariff.Properties.Appearance.BackColor = ((calcPriceEXW.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                //calcVendorTariff.Properties.Appearance.BackColor = ((calcPriceEXW.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
            }
            catch
            {
            }
            return;
        }
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
                ConfirmPriceListCalc();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSave_Click. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Сохраняет изменения в расчете цен
        /// </summary>
        private void ConfirmPriceListCalc()
        {
            System.Boolean bIsNew = false;
            try
            {
                if (m_objSelectedPriceListCalc != null)
                {
                    System.String strErr = "";

                    if (IsAllParamFill() == true)
                    {
                        CPriceListCalc objPriceListCalcForSave = new CPriceListCalc();
                        objPriceListCalcForSave.ID = m_objSelectedPriceListCalc.ID;
                        objPriceListCalcForSave.Name = txtName.Text;
                        objPriceListCalcForSave.DocDate = dateBeginDate.DateTime;
                        objPriceListCalcForSave.Description = txtDscrpn.Text;
                        objPriceListCalcForSave.IsActive = checkActive.Checked;
                        objPriceListCalcForSave.FileNameXLS = txtFileName.Text;
                        objPriceListCalcForSave.CalcItemList = m_objSelectedPriceListCalc.CalcItemList;

                        System.Boolean bIsOkSaveGeneralProperties = false;
                        if (objPriceListCalcForSave.ID.CompareTo(System.Guid.Empty) == 0)
                        {
                            // новый расчет цен
                            bIsOkSaveGeneralProperties = objPriceListCalcForSave.Add(m_objProfile, m_bNeedSaveFileXLS);
                            if (bIsOkSaveGeneralProperties == true)
                            {
                                if (bIsOkSaveGeneralProperties == false)
                                {
                                    // в IB добавить не удалось, поэтому в SQL Server нужно удалить
                                    SendMessageToLog(strErr);
                                    objPriceListCalcForSave.ID = System.Guid.Empty;
                                }

                            }
                        }
                        else
                        {
                            // уже существующий расчет цен
                            bIsOkSaveGeneralProperties = objPriceListCalcForSave.Update(m_objProfile, m_bNeedSaveFileXLS);
                        }
                        if (bIsOkSaveGeneralProperties == true)
                        {
                            m_objSelectedPriceListCalc.ID = objPriceListCalcForSave.ID;
                            m_objSelectedPriceListCalc.Name = objPriceListCalcForSave.Name;
                            m_objSelectedPriceListCalc.DocDate = objPriceListCalcForSave.DocDate;
                            m_objSelectedPriceListCalc.Description = objPriceListCalcForSave.Description;
                            m_objSelectedPriceListCalc.IsActive = objPriceListCalcForSave.IsActive;
                            m_objSelectedPriceListCalc.FileNameXLS = objPriceListCalcForSave.FileNameXLS;
                            m_objSelectedPriceListCalc.CalcItemList = objPriceListCalcForSave.CalcItemList;

                            if (bIsNew == true)
                            {
                                // новый расчет цен
                                m_objPriceListCalcList.Add( m_objSelectedPriceListCalc );
                                gridControl.RefreshDataSource();
                                gridViewPriceListCalc.FocusedRowHandle = (gridViewPriceListCalc.RowCount - 1);
                            }
                            else
                            {
                                gridControl.RefreshDataSource();
                            }
                            tabControl.SelectedTabPage = tabView;
                        }
                        else
                        {
                            SendMessageToLog(strErr);
                        }
                        objPriceListCalcForSave = null;
                    }

                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в свойствах расчета цен. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Удаление расчета цен
        /// <summary>
        /// Удаление расчета цен
        /// </summary>
        /// <param name="objPriceListCalc">Расчет цен</param>
        private void DeletePriceListCalc(CPriceListCalc objPriceListCalc)
        {
            if (objPriceListCalc == null) { return; }
            if (DevExpress.XtraEditors.XtraMessageBox.Show("Подтвердите удаление расчета цен \"" + objPriceListCalc.Name + "\". ", "Подтверждение",
                System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
//            System.String strErr = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.Int32 iDeletedObjIndx = -1;
                for (System.Int32 i = 0; i < m_objPriceListCalcList.Count; i++)
                {
                    if (m_objPriceListCalcList[i].ID.CompareTo( objPriceListCalc.ID ) == 0)
                    {
                        iDeletedObjIndx = i;
                        break;
                    }
                }
                if (iDeletedObjIndx >= 0)
                {
                    // в IB удалить удалось, это самое сложное, теперь удалим в SQL Server
                    if (objPriceListCalc.Remove(m_objProfile) == true)
                    {
                        this.tableLayoutPanel1.SuspendLayout();
                        ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();

                        // в БД мы всё удалили, теперь нужно удалить в списке
                        m_objPriceListCalcList.RemoveAt(iDeletedObjIndx);
                        gridControl.RefreshDataSource();
                        if (((iDeletedObjIndx - 1) < 0) && ( m_objPriceListCalcList.Count > 0 )) { iDeletedObjIndx = 0; }
                        else
                        {
                            if (((iDeletedObjIndx - 1) >= 0) && (m_objPriceListCalcList.Count > 0)) { iDeletedObjIndx = iDeletedObjIndx--; }
                        }

                        gridViewPriceListCalc.FocusedRowHandle = iDeletedObjIndx;

                        this.tableLayoutPanel1.ResumeLayout(false);
                        ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();

                        SendMessageToLog("Расчет цен удален.");
                        DevExpress.XtraEditors.XtraMessageBox.Show("Расчет цен удален.", "Информация",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                    }
                    else
                    {
                        SendMessageToLog("Расчет цен удалить не удалось.");
                    }

                }


            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления расчета цен. Текст ошибки: " + f.Message);
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
                CPriceListCalc objPriceListCalc = GetSelectedPriceListCalc();
                if (objPriceListCalc == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, расчет для удаления.", "Предупреждение",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    DeletePriceListCalc(objPriceListCalc);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления товарной подгруппы. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }


        #endregion

        #region Сохранение и открытие файла расчета
        private void btnClearFile_Click(object sender, EventArgs e)
        {
            try
            {
                txtFileName.Text = "";
                m_bNeedSaveFileXLS = true;
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnClearFile_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        /// <summary>
        /// Выбирает файл для сохрагнения в БД
        /// </summary>
        private void SelectFileXLS()
        {
            try
            {
                System.String strFileName = "";
                //System.Int32 iSheetNum = 0;

                frmImportXLSData objFrmImportXLSDataDialog = new frmImportXLSData(m_objProfile, m_objMenuItem, m_objSelectedPriceListCalc, m_objProductSubTypeList);
                DialogResult dlgRes = objFrmImportXLSDataDialog.ShowDialog();
                if (dlgRes == DialogResult.OK)
                {
                    strFileName = objFrmImportXLSDataDialog.FileFullName;
                    if (objFrmImportXLSDataDialog.PriceListCalcItemList != null)
                    {
                        m_objSelectedPriceListCalc.CalcItemList = objFrmImportXLSDataDialog.PriceListCalcItemList;
                        LoadPriceListCalcItems(m_objSelectedPriceListCalc);
                    }
                }
                objFrmImportXLSDataDialog.Dispose();
                objFrmImportXLSDataDialog = null;

                //frmExportToExcelDialog objFrmExportToExcelDialog = new frmExportToExcelDialog();
                //DialogResult dlgRes = objFrmExportToExcelDialog.ShowDialog();
                //if (dlgRes == DialogResult.OK)
                //{
                //    strFileName = objFrmExportToExcelDialog.FileFullName;
                //    iSheetNum = objFrmExportToExcelDialog.SheetNumber;
                //}
                //objFrmExportToExcelDialog.Dispose();
                //objFrmExportToExcelDialog = null;
                
                if (strFileName != "")
                {
                    txtFileName.Text = strFileName;
                    m_bNeedSaveFileXLS = true;
                    SetPropertiesModified(true);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("SelectFileXLS. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                SelectFileXLS();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnLoadFile_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// загружает файл из БД на диск
        /// </summary>
        private void OpenFileMSExcel()
        {
            if (m_objSelectedPriceListCalc == null) { return; }
            if (m_objSelectedPriceListCalc.ID.CompareTo(System.Guid.Empty) == 0) { return; }
            if (m_objSelectedPriceListCalc.FileNameXLS == "") { return; }
            System.String strErr = "";
            try
            {
                // нужен путь к папке, куда будем выгружать отчет
                System.String strFileName = "";

                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    strFileName = folderBrowserDialog.SelectedPath + "\\" + m_objSelectedPriceListCalc.FileNameXLS;
                }

                //System.Int32 iSheetNum = 0;

                //frmExportToExcelDialog objFrmExportToExcelDialog = new frmExportToExcelDialog();
                //DialogResult dlgRes = objFrmExportToExcelDialog.ShowDialog();
                //if (dlgRes == DialogResult.OK)
                //{
                //    strFileName = objFrmExportToExcelDialog.FileFullName;
                //    iSheetNum = objFrmExportToExcelDialog.SheetNumber;
                //}
                //objFrmExportToExcelDialog.Dispose();
                //objFrmExportToExcelDialog = null;

                if (strFileName != "")
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(strFileName);
                    System.String strDirectory = fi.DirectoryName;
                    fi = null;

                    if (m_objSelectedPriceListCalc.LoadReportFileFromDBOnDisk(m_objProfile, strDirectory, ref strErr) == false)
                    {
                        SendMessageToLog("OpenFileMSExcel. Текст ошибки: " + strErr);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("OpenFileMSExcel. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnLoadFileToDisk_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileMSExcel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnLoadFileToDisk_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Передать данные в прайс-лист

        private List<CPriceListCalcItem> GetRecordListForPriceList( System.Boolean bAllRecords )
        {
            List<CPriceListCalcItem> objPriceListCalcItemList = null;

            if (gridViewPriceList.RowCount == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Список с ценами пуст.", "Сообщение",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return objPriceListCalcItemList;
            }
            try
            {

                
                objPriceListCalcItemList = new List<CPriceListCalcItem>();
                if (bAllRecords == true)
                {
                    // все записи
                    for (System.Int32 i = 0; i < gridViewPriceList.RowCount; i++)
                    {
                        objPriceListCalcItemList.Add(m_objSelectedPriceListCalc.CalcItemList[gridViewPriceList.GetDataSourceRowIndex(i)]);
                    }
                }
                else
                {
                    // только выделенные
                    int[] arr = gridViewPriceList.GetSelectedRows();
                    if (arr.Length > 0)
                    {
                        for (System.Int32 i = 0; i < arr.Length; i++)
                        {
                            objPriceListCalcItemList.Add(m_objSelectedPriceListCalc.CalcItemList[gridViewPriceList.GetDataSourceRowIndex(arr[i])]);
                        }
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("В списке необходимо выделить записи.", "Сообщение",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        return objPriceListCalcItemList;
                    }
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadAllRecordsInPriceList. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return objPriceListCalcItemList;
        }

        private System.Boolean LoadPricesInPriceList(List<CPriceListCalcItem> objPriceListCalcItemList)
        {
            System.Boolean bRet = false;
            if( objPriceListCalcItemList == null ) { return bRet; }
            try
            {
                System.String strErr = "";

                List<CPriceType> objPriceTypeCheckedList = new List<CPriceType>();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceType.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    if (System.Convert.ToBoolean(objNode.GetValue(colCheckForExport)) == false) { continue; }
                    objPriceTypeCheckedList.Add((CPriceType)objNode.Tag);
                }

                if (objPriceTypeCheckedList.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Необходимо выброть хотя бы один прайс (цену).", "Предупреждение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                // загрузить цены в прайс...  что это значит
                // в ERP_Mercury есть прайс и в "Контракте" тоже есть прайс
                // нужно записать изменения в ценах одновременно в оба эти прайса

                // сперва в "Контракт"
                if (CProductSubTypePriceList.SavePriceListToIB(objPriceListCalcItemList, objPriceTypeCheckedList, m_objProfile, ref strErr) == true)
                {

                    bRet = CProductSubTypePriceList.SaveCalcItemListCheck(objPriceListCalcItemList, objPriceTypeCheckedList, m_objProfile, null, ref strErr);
                    if (bRet == true)
                    {
                        SendMessageToLog( "Цены успешно переданы в прайс-лист." );
                    }
                    else
                    {
                        SendMessageToLog("Ошибка загрузки цен в прайс-лист: " + strErr);
                    }

                }
                else
                {
                    SendMessageToLog("Во время сохранения цен в \"Контракт\" возникла ошибка: " + strErr);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadPricesInpriceList. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return bRet;

        }

        private void mitemAddToPriceAllRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                List<CPriceListCalcItem> objPriceListCalcItemList = GetRecordListForPriceList(true);
                if (objPriceListCalcItemList != null)
                {
                    LoadPricesInPriceList(objPriceListCalcItemList);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddToPriceAllRecordsToolStripMenuItem_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return ;

        }
        private void mitemAddToPriceSelectedRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                List<CPriceListCalcItem> objPriceListCalcItemList = GetRecordListForPriceList(false);
                if (objPriceListCalcItemList != null)
                {
                    LoadPricesInPriceList(objPriceListCalcItemList);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddToPriceSelectedRecordsToolStripMenuItem_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        private void btnExportToIB_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Boolean bAllRecords = (radioGroupSet.SelectedIndex == 0);
                List<CPriceListCalcItem> objPriceListCalcItemList = GetRecordListForPriceList(bAllRecords);
                if (objPriceListCalcItemList != null)
                {
                    LoadPricesInPriceList(objPriceListCalcItemList);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddToPriceSelectedRecordsToolStripMenuItem_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        #region Выделить всё/ Отменить всё

        private void SelectDeselectNodes(System.Boolean bSelect)
        {
            try
            {
                tableLayoutPanel5.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPriceType)).BeginInit();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceType.Nodes)
                {
                    if (System.Convert.ToBoolean(objNode.GetValue(colCheckForExport)) != bSelect)
                    {
                        objNode.SetValue(colCheckForExport, bSelect);
                    }
                }

                ((System.ComponentModel.ISupportInitialize)(this.treeListPriceType)).EndInit();
                tableLayoutPanel5.ResumeLayout(false);

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SelectDeselectNodes.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void mitemSelectAllPrices_Click(object sender, EventArgs e)
        {
            try
            {
                SelectDeselectNodes(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "mitemSelectAllPrices_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void mitemDeselectAllPrices_Click(object sender, EventArgs e)
        {
            try
            {
                SelectDeselectNodes(false);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "mitemDeselectAllPrices_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion


        #endregion

    }

    public class ViewPriceListCalc : PlugIn.IClassTypeView
    {
        public override void Run(UniXP.Common.MENUITEM objMenuItem, System.String strCaption)
        {
            frmPriceListCalc obj = new frmPriceListCalc(objMenuItem.objProfile, objMenuItem);
            obj.Text = strCaption;
            obj.MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent;
            obj.Visible = true;
        }

    }

}
