using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP_Mercury.Common;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System.Threading;

namespace ERPMercuryPlan
{
    public partial class frmPriceList : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CPriceListCalcItem> m_objPriceListCalcItemList;
        private List<CPriceType> m_objPriceTypeList;
        private CProductSubTypePriceList m_objProductSubTypePriceList;

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
        private CPriceListCalcItem m_objSelectedPriceListCalcItem;
        private System.Boolean m_bIsNew;
        System.Boolean m_bIsPriceListEditor;
        private const System.String m_strReportsDirectory = "templates";
        private const System.String m_strReportPriceF1 = "Прайс магазины.xlsx";
        private const System.String m_strReportPriceF2 = "Прайс опт.xlsx";
        private const System.String strWaitLoadProductSubTypeList = "идёт загрузка списка...";
        private System.Guid m_iPaymentType1;
        private System.Guid m_iPaymentType2;
        private const System.String m_strPaymentType1 = "58636EC5-F64A-462C-90B1-7686ADFE70F9";
        private const System.String m_strPaymentType2 = "E872B5E3-83FF-4B1A-925D-0F1B3C4D5C85";

        // потоки
        public System.Threading.Thread ThreadProductSubtypeList {get; set;}
        public System.Threading.Thread ThreadPriceList { get; set; }
        public System.Threading.ManualResetEvent EventStopThread { get; set; }
        public System.Threading.ManualResetEvent EventThreadStopped {get; set;}
        public delegate void LoadProductSubtypeListDelegate(List<CProductSubType> objProductSubtypeList);
        public LoadProductSubtypeListDelegate m_LoadProductSubtypeListDelegate;

        public delegate void LoadPriceListDelegate(List<CPriceListCalcItem> objPriceListCalcItemList);
        public LoadPriceListDelegate m_LoadPriceListDelegate;
        
        private const System.Int32 iThreadSleepTime = 1000;
        private System.Boolean m_bThreadFinishJob;
        private const System.Int32 ipanelWarningHeight = 30;

        #endregion

        #region Коструктор
        public frmPriceList(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objPriceListCalcItemList = null;
            m_objProductSubTypePriceList = new CProductSubTypePriceList();
            m_bIsComboBoxFill = false;
            m_bIsNew = false;
            m_objPriceTypeList = null;
            m_iPaymentType1 = new Guid(m_strPaymentType1);
            m_iPaymentType2 = new Guid(m_strPaymentType2);
            m_bDisableEvents = false;
            m_bIsChanged = false;

        }
        #endregion

        #region Загрузка формы
        private void frmPriceList_Shown(object sender, EventArgs e)
        {
            try
            {
                // настройка грида для прайс-листа
                AddGridColumns();
                // загрузка данных в выпадающие списки
                LoadComboBoxItems();
                // загрузка информации в закладку "Печать прайсов"
                InitTabPrintPrices();
                // проверка на наличие динамических прав
                m_bIsPriceListEditor = m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_PriceListEditor);
                if( m_bIsPriceListEditor == false )
                {
                    splitContainerControl2.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                    splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                    tabControlExportToIB.TabPages[1].PageVisible = false;
                    btnAdd.Visible = false;
                    btnDelete.Visible = false;
                    gridControlPriceList.DoubleClick -= new EventHandler(gridControlPriceList_DoubleClick);
                }

                // отключаем кнопки
                SetEnabledControlsButtons(false);
                this.Cursor = Cursors.WaitCursor;
                // запуск потоков
                StartThreadWithLoadData();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("frmPriceList_Shown().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Потоки
        public void StartThreadWithLoadData()
        {
            try
            {
                // инициализируем события
                this.EventStopThread = new System.Threading.ManualResetEvent(false);
                this.EventThreadStopped = new System.Threading.ManualResetEvent(false);

                // инициализируем делегаты
                m_LoadProductSubtypeListDelegate = new LoadProductSubtypeListDelegate(LoadProductSubTypeList);
                m_LoadPriceListDelegate = new LoadPriceListDelegate(LoadPriceList);

                // запуск потока
                StartThreadComboBox();
                StartThreadPriceList();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadWithLoadData().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void StartThreadComboBox()
        {
            try
            {
                // делаем событиям reset
                this.EventStopThread.Reset();
                this.EventThreadStopped.Reset();

                this.ThreadProductSubtypeList = new System.Threading.Thread(WorkerThreadFunction);
                this.ThreadProductSubtypeList.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadWithLoadData().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void StartThreadPriceList()
        {
            try
            {
                this.ThreadPriceList = new System.Threading.Thread(LoadPriceListInThread);
                this.ThreadPriceList.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadWithLoadData().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        
        public void WorkerThreadFunction()
        {
            try
            {
                LoadProductSubTypeListInThread();

                // пока заполняется список товаров будем проверять, не было ли сигнала прекратить все это
                while (this.m_bThreadFinishJob == false)
                {
                    // проверим, а не попросили ли нас закрыться
                    if (this.EventStopThread.WaitOne(iThreadSleepTime, true))
                    {
                        this.EventThreadStopped.Set();
                        break;
                    }
                }
            }
            catch (System.Exception e)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("WorkerThreadFunction\n" + e.Message, "Ошибка",
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

        public void LoadProductSubTypeListInThread()
        {
            try
            {
                List<CProductSubType> objProductSubtypeList = CProductSubType.GetProductSubTypeList(m_objProfile, null, false);
                List<CProductSubType> objAddProductSubtypeList = new List<CProductSubType>();

                if ((objProductSubtypeList != null) && (objProductSubtypeList.Count > 0))
                    {

                        System.Int32 iRecCount = 0;
                        foreach (CProductSubType objProductSubType in objProductSubtypeList)
                        {
                            objAddProductSubtypeList.Add(objProductSubType);
                            iRecCount++;

                            if (iRecCount == 1000)
                            {
                                iRecCount = 0;
                                Thread.Sleep(1000);
                                this.Invoke(m_LoadProductSubtypeListDelegate, new Object[] { objAddProductSubtypeList });
                            }

                        }
                        if (iRecCount != 1000)
                        {
                            iRecCount = 0;
                            this.Invoke(m_LoadProductSubtypeListDelegate, new Object[] { objAddProductSubtypeList });
                        }

                    }

                    //this.Invoke(m_LoadCustomerListDelegate, new Object[] { objCustomerList });
                    //return;
                objProductSubtypeList = null;
                objAddProductSubtypeList = null;
                this.Invoke(m_LoadProductSubtypeListDelegate, new Object[] { null });
                this.m_bThreadFinishJob = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadCustomerListInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void LoadProductSubTypeList(List<CProductSubType> objProductSubTypeList)
        {
            try
            {
                cboxProductSubType.Text = strWaitLoadProductSubTypeList;
                if ((objProductSubTypeList != null) && (objProductSubTypeList.Count > 0))
                {
                    cboxProductSubType.Properties.Items.AddRange(objProductSubTypeList);
                }
                else
                {
                    cboxProductSubType.Text = "";
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadProductSubTypeList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }


        public void LoadPriceListInThread()
        {
            try
            {
                if (m_objProductSubTypePriceList.LoadPriceList(m_objProfile, null, System.Guid.Empty) == true)
                {
                    this.Invoke(m_LoadPriceListDelegate, new Object[] { m_objProductSubTypePriceList.PriceItemmList });
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadPriceListInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        
        
        private void LoadPriceList(List<CPriceListCalcItem> objPriceListCalcItemList)
        {
            try
            {
                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).BeginInit();

                gridControlPriceList.DataSource = null;
                m_objPriceListCalcItemList = objPriceListCalcItemList;

                if (m_objPriceListCalcItemList != null)
                {
                    gridControlPriceList.DataSource = m_objPriceListCalcItemList;

                }


                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).EndInit();
                tableLayoutPanel1.Refresh();

                // отключаем информационную панель
                tableLayoutPanelPriceList.RowStyles[0].Height = 0;
                // включаем кнопки управления
                SetEnabledControlsButtons(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadProductSubTypeList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                btnRefresh.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Настройки грида
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();
            AddGridColumn(ColumnView, "ID", "Код товарной подгруппы");
            AddGridColumn(ColumnView, "SubTypeStateName", "Состояние");
            AddGridColumn(ColumnView, "PartSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnView, "PartSubTypeID_Ib", "Код");
            AddGridColumn(ColumnView, "ProductOwner", "Товарная марка");
            AddGridColumn(ColumnView, "ProductType", "Товарная группа");
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
                    if ((objColumn.FieldName == "PartSubTypeName") || (objColumn.FieldName == "ProductOwner") || (objColumn.FieldName == "ProductType") || (objColumn.FieldName == "ProductLineName") || (objColumn.FieldName == "Name"))
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
        private void gridViewPriceList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "SubTypeStateName")
                {
                    System.Drawing.Image img = ERPMercuryPlan.Properties.Resources.warning;
                    if ((e.CellValue != null) && (System.Convert.ToString(e.CellValue) != "")
                        && (gridViewPriceList.GetRowCellValue(e.RowHandle, gridViewPriceList.Columns["SubTypeStateName"]) != null)
                        && (System.Convert.ToString(gridViewPriceList.GetRowCellValue(e.RowHandle, gridViewPriceList.Columns["SubTypeStateName"])) == ERP_Mercury.Global.Consts.strWarningProductSubTypeStateSate))
                    {
                        Rectangle rImg = new Rectangle(e.Bounds.X, e.Bounds.Y + (e.Bounds.Height - img.Size.Height) / 2, img.Width, img.Height);
                        e.Graphics.DrawImage(img, rImg);
                        //                        Rectangle r = new Rectangle((e.Bounds.X + img.Width), e.Bounds.Y, e.Bounds.Width - img.Width, e.Bounds.Height);
                        Rectangle r = e.Bounds;
                        r.Inflate(-img.Width, 0);
                        e.Appearance.DrawString(e.Cache, System.Convert.ToString(e.CellValue), r);
                        e.Handled = true;
                    }
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewPriceList_CustomDrawCell. " + f.Message);
            }
            return;

        }

        private void gridViewPriceList_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                System.Boolean bIsWarningState = (System.Convert.ToString(gridViewPriceList.GetRowCellValue(e.RowHandle, gridViewPriceList.Columns["SubTypeStateName"])) == ERP_Mercury.Global.Consts.strWarningProductSubTypeStateSate);

                if (bIsWarningState == true)
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewPriceList_RowStyle. " + f.Message);
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
                //// выпадающий список с товарными подгруппами
                //cboxProductSubType.Properties.Items.Clear();
                //List<CProductSubType> objProductSubtypeList = CProductSubType.GetProductSubTypeList(m_objProfile, null, false);
                //if (objProductSubtypeList != null)
                //{
                //    foreach (CProductSubType objItem in objProductSubtypeList)
                //    {
                //        cboxProductSubType.Properties.Items.Add(objItem);
                //    }
                //}
                //objProductSubtypeList = null;

                // список типов цен
                treeListPriceEditor.Nodes.Clear();
                treeListPriceType.Nodes.Clear();
                treeListSetPriceAuto.Nodes.Clear();

                m_objPriceTypeList = CPriceType.GetPriceTypeList(m_objProfile, null);
                if (m_objPriceTypeList != null)
                {
                    foreach (CPriceType objItem in m_objPriceTypeList)
                    {
                        this.treeListPriceEditor.AppendNode(new object[] { true, objItem.Name, System.String.Format("{0:### ### ##0.000}", 0) }, null).Tag = objItem;
                        this.treeListPriceType.AppendNode(new object[] { false, objItem.Name }, null).Tag = objItem;
                        this.treeListSetPriceAuto.AppendNode(new object[] { false, objItem.Name, System.String.Format("{0:### ### ##0.000}", 0) }, null).Tag = objItem;
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

        #region Содержимое прайс-листа
        private void SetEnabledControlsButtons( System.Boolean bEnable )
        {
            btnRefresh.Enabled = bEnable;
            btnPrintPriceF1.Enabled = bEnable;
            btnPrintPriceF2.Enabled = bEnable;
            btnExportForCalcPrice.Enabled = bEnable;
            btnAdd.Enabled = bEnable;
            btnDelete.Enabled = bEnable;
            btnEdit.Enabled = bEnable;
        }

        /// <summary>
        /// Загружает содержимое прайс-листа
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public void LoadPriceList()
        {
            m_bDisableEvents = true;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // включаем информационную панель
                tableLayoutPanelPriceList.RowStyles[0].Height = ipanelWarningHeight;
                // отключаем кнопки
                SetEnabledControlsButtons( false );

                StartThreadPriceList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return ;
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

                    if ((m_objPriceListCalcItemList != null) && (m_objPriceListCalcItemList != null) && (m_objPriceListCalcItemList.Count > 0) && (Ib_ID != 0))
                    {
                        foreach (CPriceListCalcItem objCalcItem in m_objPriceListCalcItemList )
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
        private void ShowPriceListCalcItem(CPriceListCalcItem objPriceListCalcItem)
        {
            try
            {
                this.splitContainerControl1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).BeginInit();

                treeListPrices.Nodes.Clear();

                if ((objPriceListCalcItem != null) && (objPriceListCalcItem.PriceList != null))
                {
                    foreach (CPrice objPrice in objPriceListCalcItem.PriceList)
                    {
                        if( (objPrice.PriceType.IsShowInPrice == false) && ( m_bIsPriceListEditor == false ) ) { continue; }

                        this.treeListPrices.AppendNode(new object[] { objPrice.PriceType.Name, System.String.Format("{0:### ### ##0.000}", objPrice.PriceValue) }, null);
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
                bRet = (cboxProductSubType.SelectedItem != null);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки заполнения обязательных свойств позиции прайс-листа. Текст ошибки: " + f.Message);
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
                cboxProductSubType.Properties.Appearance.BackColor = ((cboxProductSubType.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
            }
            catch
            {
            }
            return;
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
        private void SetReadOnlyPropertiesControls(System.Boolean bReadOnly)
        {
            try
            {
                cboxProductSubType.Properties.ReadOnly = bReadOnly;
                colPriceValue.OptionsColumn.ReadOnly = bReadOnly;
                colPriceValue.OptionsColumn.AllowEdit = !bReadOnly;
                colCheckForExport.OptionsColumn.ReadOnly = bReadOnly;
                colCheckForExport.OptionsColumn.AllowEdit = !bReadOnly;
                colPriceCheck.OptionsColumn.ReadOnly = bReadOnly;
                colPriceCheck.OptionsColumn.AllowEdit = !bReadOnly;

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
                //btnSetToPartList.Enabled = (!bEnable) && (m_objSelectedProduct.ID.CompareTo(System.Guid.Empty) != 0);
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
        private void LoadPriceListItemInEditor( CPriceListCalcItem objPriceListItem )
        {
            try
            {
                if (objPriceListItem == null) { return; }
                m_bDisableEvents = true;
                if (m_bIsComboBoxFill == false) { LoadComboBoxItems(); }

                m_objSelectedPriceListCalcItem = objPriceListItem;
                lblCustomerIfo.Text = (m_objSelectedPriceListCalcItem.ProductOwner + " " + m_objSelectedPriceListCalcItem.ProductType);
                cboxProductSubType.SelectedItem = null;
                if( ( m_objSelectedPriceListCalcItem.objProductSubType != null ) && ( cboxProductSubType.Properties.Items.Count > 0 ) )
                {
                    foreach (object objItem in cboxProductSubType.Properties.Items)
                    {
                        if (((CProductSubType)objItem).ID.CompareTo(m_objSelectedPriceListCalcItem.objProductSubType.ID) == 0)
                        {
                            cboxProductSubType.SelectedItem = objItem;
                            break;
                        }
                    }
                }

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
                SendMessageToLog("Ошибка редактирования расчета цен. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                btnCancel.Enabled = true;
            }
            return;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddPriceListItem();
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления строки в прайс-лист. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void AddPriceListItem()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                m_bIsNew = true;
                LoadPriceListItemInEditor( new CPriceListCalcItem() );
                SetReadOnlyPropertiesControls(false);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления строки в прайс-лист. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void gridControlPriceList_DoubleClick(object sender, EventArgs e)
        {
            if (m_bIsPriceListEditor == false) { return; }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CPriceListCalcItem objPriceListItem = GetSelectedPriceListCalcItem();
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
                        CPriceListCalcItem objPriceListItemForSave = new CPriceListCalcItem();
                        objPriceListItemForSave.objProductSubType = (CProductSubType)cboxProductSubType.SelectedItem;
                        objPriceListItemForSave.PriceList = new List<CPrice>();
                        CPriceType objPriceType = null;
                        List<CPriceType> objPriceTypeCheckedList = new List<CPriceType>();
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes)
                        {
                            if (objNode.Tag == null) { continue; }
                            objPriceType = ( CPriceType )objNode.Tag;

                            if (System.Convert.ToBoolean(objNode.GetValue(colPriceCheck)) == true)
                            { 
                                objPriceTypeCheckedList.Add(objPriceType);
                                objPriceListItemForSave.PriceList.Add(new CPrice(objPriceType, System.Convert.ToDouble(objNode.GetValue(colPriceValue))));
                            }
                        }

                        objPriceType = null;

                        List<CPriceListCalcItem> objPriceListItemsList = new List<CPriceListCalcItem>();
                        objPriceListItemsList.Add( objPriceListItemForSave );

                        // сперва в InterBase
                        System.Boolean bIsOkSave = CProductSubTypePriceList.SavePriceListToIB(objPriceListItemsList, objPriceTypeCheckedList, m_objProfile, ref strErr);
                        if (bIsOkSave == true)
                        {
                            // теперь в прайсе по подгруппам и товарам
                            bIsOkSave = CProductSubTypePriceList.SaveCalcItemList(objPriceListItemsList, m_objProfile, null, ref strErr);

                            if (bIsOkSave == true)
                            {
                                m_objSelectedPriceListCalcItem.objProductSubType = objPriceListItemForSave.objProductSubType;
                                m_objSelectedPriceListCalcItem.PriceList = objPriceListItemForSave.PriceList;

                                if (m_bIsNew == true)
                                {
                                    // новая запись в прайсе
                                    m_objPriceListCalcItemList.Add(m_objSelectedPriceListCalcItem);
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
                        }
                        objPriceListItemForSave = null;
                        objPriceTypeCheckedList = null;
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

                List<CPriceListCalcItem> objPriceListCalcItemList = new List<CPriceListCalcItem>();

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
                if (CProductSubTypePriceList.DeleteCalcItemList(objPriceListCalcItemList, m_objProfile, null, ref strErr) == true)
                {
                    this.tableLayoutPanel1.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.gridControlPriceList)).BeginInit();

                    // в БД мы всё удалили, теперь нужно удалить в списке
                    foreach (CPriceListCalcItem objDeleted in objPriceListCalcItemList)
                    {
                        iDeletedObjIndx = -1;
                        for (System.Int32 i = 0; i < m_objPriceListCalcItemList.Count; i++)
                        {
                            if (m_objPriceListCalcItemList[i].objProductSubType.ID.CompareTo(objDeleted.objProductSubType.ID) == 0)
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
                    SendMessageToLog("Выбранные записи  удалить не удалось.");
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
            List<CPriceListCalcItem> objPriceList = new List<CPriceListCalcItem>();
            try
            {
                for (System.Int32 i = 0; i < gridViewPriceList.RowCount; i++)
                {
                    objPriceList.Add(m_objPriceListCalcItemList[gridViewPriceList.GetDataSourceRowIndex(i)]);
                }
                CProductSubTypePriceList.ExportToExcel(objPriceList, m_objPriceTypeList, m_bIsPriceListEditor);
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

        #region Выбор типов цен для обновления в Контракте
        private void SelectPriceTypeList(System.Boolean bSelect)
        {
            try
            {
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes)
                {
                    objNode.SetValue(colPriceCheck, bSelect);
                }

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("SelectPriceTypeList. Текст ошибки: " + f.Message);
                
            }
            finally
            {
            }

            return;
        }

        private void mitemSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectPriceTypeList(true);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("mitemSelectAll_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void mitemDeselectAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectPriceTypeList(false);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("mitemDeselectAll_Click. Текст ошибки: " + f.Message); 
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Сохранение цен в "Контракте"
        /// </summary>
        private void ExportPriceListToIB()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                List<CPriceType> objPriceTypeCheckedList = new List<CPriceType>();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceType.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    if (System.Convert.ToBoolean(objNode.GetValue(colCheckForExport)) == false) { continue; }
                    objPriceTypeCheckedList.Add((CPriceType)objNode.Tag);
                }

                if (objPriceTypeCheckedList.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Необходимо выброть хотя бы одну цену.", "Предупреждение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                List<CPriceListCalcItem> objPriceItemsList = new List<CPriceListCalcItem>();
                if (radioGroupSet.SelectedIndex == 0)
                {
                    // все записи
                    for (System.Int32 i = 0; i < gridViewPriceList.RowCount; i++)
                    {
                        objPriceItemsList.Add(m_objPriceListCalcItemList[gridViewPriceList.GetDataSourceRowIndex(i)]);
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
                            objPriceItemsList.Add(m_objPriceListCalcItemList[gridViewPriceList.GetDataSourceRowIndex(arr[i])]);
                        }
                    }
                }

                System.String strErr = "";
                if (objPriceItemsList.Count > 0)
                {
                    if( CProductSubTypePriceList.SavePriceListToIB( objPriceItemsList, objPriceTypeCheckedList, m_objProfile, ref strErr ) == true )
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Цены успешно переданы в \"Контракт\".", "Информация",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Во время сохранения цен в \"Контракт\" возникла ошибка.\nТекст ошибки: " + strErr, "Информация",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        SendMessageToLog("Во время сохранения цен в \"Контракт\" возникла ошибка: " + strErr);
                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Во время сохранения цен в \"Контракт\" возникла ошибка.\nТекст ошибки: " + f.Message, "Информация",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                SendMessageToLog("Во время сохранения цен в \"Контракт\" возникла ошибка: " + f.Message);
            }
            finally
            {
                 Cursor = Cursors.Default;
           }
            return;
        }
        private void btnExportToIB_Click(object sender, EventArgs e)
        {
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("Подтвердите передачу цен в \"Контракт\".", "Подтверждение",
                     System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    ExportPriceListToIB();
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnExportToIB_Click. Ошибка: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;

        }

        #endregion 

        #region Назначение цены списку подгрупп
        /// <summary>
        /// Автоматическое назначение списку подгрупп указанных цен
        /// </summary>
        private void SetPricesToproductSubtypeList()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                List<CPrice> objPriceCheckedList = new List<CPrice>();
                List<CPriceType> objPriceTypeCheckedList = new List<CPriceType>();

                // предварительная проверка на то, выбраны ли цены в списке
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSetPriceAuto.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    if (System.Convert.ToBoolean(objNode.GetValue(colSetPriceAutoCheck)) == false) { continue; }
                    objPriceCheckedList.Add( new CPrice( ( (CPriceType)objNode.Tag ), System.Convert.ToDouble( objNode.GetValue( colSetPriceAutoPriceValue ) ) ) );
                    objPriceTypeCheckedList.Add((CPriceType)objNode.Tag);
                }

                if (objPriceCheckedList.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Необходимо выброть хотя бы одну цену.", "Предупреждение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                List<CPriceListCalcItem> objPriceItemsListForUpdate = new List<CPriceListCalcItem>();
                CPriceListCalcItem objPriceListCalcItem = null;
                if (radioGroupSet.SelectedIndex == 0)
                {
                    // все записи
                    for (System.Int32 i = 0; i < gridViewPriceList.RowCount; i++)
                    {
                        objPriceListCalcItem = new CPriceListCalcItem();
                        objPriceListCalcItem.objProductSubType = m_objPriceListCalcItemList[gridViewPriceList.GetDataSourceRowIndex(i)].objProductSubType;
                        objPriceItemsListForUpdate.Add( objPriceListCalcItem );
                        
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
                            objPriceListCalcItem = new CPriceListCalcItem();
                            objPriceListCalcItem.objProductSubType = m_objPriceListCalcItemList[gridViewPriceList.GetDataSourceRowIndex(arr[i])].objProductSubType;
                            objPriceItemsListForUpdate.Add(objPriceListCalcItem);
                        }
                    }
                }

                // у нас есть список строк из прайса для изменения и у нас есть выбранные цены
                // тепрь мы пройдем по списку подгрупп и назначим цены 
                foreach (CPriceListCalcItem objPriceItem in objPriceItemsListForUpdate)
                {
                    if (objPriceItem.PriceList == null) { objPriceItem.PriceList = new List<CPrice>(); }
                    else { objPriceItem.PriceList.Clear(); }

                    objPriceItem.PriceList = objPriceCheckedList;
                }


                System.String strErr = "";
                if (objPriceItemsListForUpdate.Count > 0)
                {
                    // сперва в InterBase
                    System.Boolean bIsOkSave = CProductSubTypePriceList.SavePriceListToIB(objPriceItemsListForUpdate, objPriceTypeCheckedList, m_objProfile, ref strErr);

                    if (bIsOkSave == true)
                    {
                        if (CProductSubTypePriceList.SaveCalcItemList(objPriceItemsListForUpdate, m_objProfile, null, ref strErr) == true)
                        {
                            // если у нас всё получилось, то чтобы не делать запрос в БД на обновление, перепишем цены в списке
                            System.Boolean bPriceExists = false;
                            foreach (CPriceListCalcItem objPriceItemSaved in objPriceItemsListForUpdate)
                            {
                                foreach (CPriceListCalcItem objPriceItem in m_objPriceListCalcItemList)
                                {
                                    if (objPriceItemSaved.objProductSubType.ID.CompareTo(objPriceItem.objProductSubType.ID) == 0)
                                    {
                                        // нашли нужную подгруппу
                                        if (objPriceItem.PriceList == null) { objPriceItem.PriceList = new List<CPrice>(); }
                                        bPriceExists = false;
                                        foreach (CPrice objPriceSaved in objPriceItemSaved.PriceList)
                                        {
                                            foreach (CPrice objPrice in objPriceItem.PriceList)
                                            {
                                                if (objPrice.PriceType.ID.CompareTo(objPriceSaved.PriceType.ID) == 0)
                                                {
                                                    bPriceExists = true;
                                                    objPrice.PriceValue = objPriceSaved.PriceValue;
                                                    break;
                                                }
                                            }
                                            if (bPriceExists == false)
                                            {
                                                objPriceItem.PriceList.Add(new CPrice(objPriceSaved.PriceType, objPriceSaved.PriceValue));
                                            }
                                        }
                                    }
                                }
                            }

                            gridControlPriceList.RefreshDataSource();

                            DevExpress.XtraEditors.XtraMessageBox.Show("Цены успешно изменены.", "Информация",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                        else
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Во время изменения цен возникла ошибка.\nТекст ошибки: " + strErr, "Информация",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                            SendMessageToLog("Во время изменения цен возникла ошибка: " + strErr);
                        }
                    }

                }

                objPriceCheckedList = null;
                objPriceItemsListForUpdate = null;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Во время изменения цен возникла ошибка.\nТекст ошибки: " + f.Message, "Информация",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                SendMessageToLog("Во время изменения цен возникла ошибка: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }
        private void btnSetPriceListAuto_Click(object sender, EventArgs e)
        {
            try
            {
                SetPricesToproductSubtypeList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ВbtnSetPriceListAuto_Click. Ошибка: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Экспорт прайсов в MS Excel
        /// <summary>
        /// загружает необходимые данные в закладку "Печать прайсов"
        /// </summary>
        private void InitTabPrintPrices()
        {
            try
            {
                // типы цен
                cboxPriceType.Properties.Items.Clear();
                cboxPriceType.Properties.Items.AddRange(CPriceType.GetPriceTypeList(m_objProfile, null));
                cboxPriceType.SelectedItem = cboxPriceType.Properties.Items[0];

                // дата
                dtBeginDate.DateTime = System.DateTime.Today;

                // курс
                calcCurrencyRate.Value = 0;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ВInitTabPrintPrices. Ошибка: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }
        /// <summary>
        /// Выделить/снять выделение всего списка товарных марок
        /// </summary>
        /// <param name="bSelect"></param>
        private void SelectProductOwnerList(System.Boolean bSelect)
        {
            try
            {
                this.tableLayoutPanel8.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListProductOwner)).BeginInit();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListProductOwner.Nodes)
                {
                    objNode.SetValue(colProductOwnerNameCheck , bSelect);
                }

                this.tableLayoutPanel8.ResumeLayout(false);
                this.tableLayoutPanel8.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListProductOwner)).EndInit();

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("SelectProductOwnerList. Текст ошибки: " + f.Message);

            }
            finally
            {
            }

            return;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                SelectProductOwnerList(true);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("mitemSelectAll_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void mitmsDeSelectAllProductOwner_Click(object sender, EventArgs e)
        {
            try
            {
                SelectProductOwnerList(false);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("mitmsDeSelectAllProductOwner_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void PrintPriceList(System.String strFileName)
        {
            if (strFileName == "") { return; }
            if (System.IO.File.Exists(strFileName) == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show( "файл \"" + strFileName + "\" не найден.", "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            System.String strErr = "";
            Excel.Application oXL = null;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet = null;

            System.Int32 iStartRow = 8;
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<CPriceListCalcItem> objPriceListForPrint = GetPriceListForPrint((CPriceType)cboxPriceType.SelectedItem);

                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                CProductOwner objProductOwner = null;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListProductOwner.Nodes)
                {
                    if (System.Convert.ToBoolean(objNode.GetValue(colProductOwnerNameCheck)) == false) { continue; }
                    if (System.Convert.ToString(objNode.GetValue(colProductOwnerSheet)) == "") { continue; }
                    if (objNode.Tag == null) { continue; }

                    objProductOwner = (CProductOwner)objNode.Tag;
                    oSheet = null;

                    foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                    {
                        if (objSheet.Name == objProductOwner.Name)
                        {
                            oSheet = objSheet;
                            break;
                        }
                    }

                    if (oSheet != null)
                    {
                        // наконец-то печать
                        oSheet.Cells[1, 3] = calcCurrencyRate.Value;
                        oSheet.Cells[4, 2] = "c " + dtBeginDate.DateTime.ToLongDateString();

                        CProductSubTypePriceList.ExportToExcel2(objPriceListForPrint, (CPriceType)cboxPriceType.SelectedItem,
                            objProductOwner, 9, 2, 3, oSheet, ref strErr );
                    }

                    oSheet = null;
                }

                ((Excel._Worksheet)oWB.Worksheets[1]).Activate();
                oXL.Visible = true;

            }
            catch (System.Exception f)
            {
                oXL = null;
                oWB = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oSheet = null;
                oWB = null;
                oXL = null;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }

        /// <summary>
        /// Загружает список листов из файла эксель
        /// </summary>
        /// <param name="strFileName">полный путь к файлу</param>
        private void LoadToolsFromTemplate(System.String strFileName)
        {
            if (strFileName == "") { return; }
            if (System.IO.File.Exists(strFileName) == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("файл \"" + strFileName + "\" не найден.", "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            Excel.Application oXL = null;
            Excel._Workbook oWB;

            System.Int32 iStartRow = 8;
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                treeListProductOwner.Nodes.Clear();

                // товарные марки
                List<CProductOwner> objProductOwnerList = CProductOwner.GetProductOwnerList(m_objProfile);

                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));

                // список листов
                List<System.String> objSheetList = new List<string>();
                foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                {
                    if (objSheet.Name == "Tools") { continue; }
                    objSheetList.Add(objSheet.Name);
                }

                oWB = null;
                oXL.Quit();
                oXL = null;

                if (objProductOwnerList != null)
                {
                    System.String strOwnerSheet = "";
                    foreach (CProductOwner objProductOwner in objProductOwnerList)
                    {
                        strOwnerSheet = "";
                        foreach (System.String strSheetName in objSheetList)
                        {
                            if (strSheetName == objProductOwner.Name)
                            {
                                strOwnerSheet = strSheetName;
                                this.treeListProductOwner.AppendNode(new object[] { (strOwnerSheet != ""), objProductOwner.Name, strOwnerSheet }, null).Tag = objProductOwner;
                                break;
                            }
                        }

                    }
                }

                objProductOwnerList = null;

            }
            catch (System.Exception f)
            {
                oXL = null;
                oWB = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oWB = null;
                oXL = null;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void btnSelectPrintFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(openFileDialog.FileName) == true)
                    {
                        txtPrintFileName.Text = openFileDialog.FileName;
                        LoadToolsFromTemplate(txtPrintFileName.Text);
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Файл не найден, либо у Вас недостаточно прав.\n" +
                                "Файл: " + txtPrintFileName.Text, "Ошибка",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnSelectPrintFile_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }

        private void btnPrintPriceList_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboxPriceType.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Укажите, пожалуйста, какой именно прайс необходимо распечатать", "Сообщение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }


                if (txtPrintFileName.Text != "")
                {
                    if (System.IO.File.Exists(openFileDialog.FileName) == true)
                    {
                        PrintPriceList(txtPrintFileName.Text);
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Файл не найден, либо у Вас недостаточно прав.\n" +
                             "Файл: " + txtPrintFileName.Text, "Ошибка",
                             System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                         "Необходимо указать файл с шаблоном прайс-листа.", "Ошибка",
                         System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnFileOpenDialog_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        /// <summary>
        /// Возвращает отсортированный список содержимого прайса
        /// </summary>
        /// <returns>позиции из прйс-листа</returns>
        private List<CPriceListCalcItem> GetPriceListForPrint( CPriceType objPriceType )
        {
            List<CPriceListCalcItem> objPriceListForPrint = new List<CPriceListCalcItem>();
            List<CPriceListCalcItem> objPriceListForPrintSorted = new List<CPriceListCalcItem>();
            try
            {
                CPriceListCalcItem objPriceListCalcItem = null;
                List<System.String> objProductOwnerName = new List<string>();
                List<System.String> objProductTypeName = new List<string>();
                


                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListProductOwner.Nodes)
                {
                    if( objNode.Tag == null ){continue;}
                    if (System.Convert.ToBoolean(objNode.GetValue(colProductOwnerNameCheck)) == true)
                    {
                        objProductOwnerName.Add( System.Convert.ToString( objNode.GetValue(colProductOwnerName) ));
                    }
                }

                for (System.Int32 i = 0; i < gridViewPriceList.RowCount; i++)
                {
                    objPriceListCalcItem = m_objPriceListCalcItemList[gridViewPriceList.GetDataSourceRowIndex(i)];
                    if (objProductOwnerName.Contains(objPriceListCalcItem.ProductOwner))
                    {
                        objPriceListForPrint.Add(objPriceListCalcItem);
                        objProductTypeName.Add(objPriceListCalcItem.ProductType);
//                        objProductOwnerName.Add(objPriceListCalcItem.ProductOwner);
                    }

                }

                IEnumerable<System.String> objProductOwnerNameDistinct = objProductOwnerName.Distinct();
                IEnumerable<System.String> objProductTypeNameDistinct = objProductTypeName.Distinct();

                foreach (System.String strProductOwnerName in objProductOwnerNameDistinct)
                {
                    foreach (System.String strProductTypeName in objProductTypeNameDistinct)
                    {
                        foreach (CPriceListCalcItem objItem in objPriceListForPrint)
                        {
                            if ((objItem.ProductOwner == strProductOwnerName) && (objItem.ProductType == strProductTypeName) && (objItem.GetPriceValueByType(objPriceType) != 0))
                            objPriceListForPrintSorted.Add(objItem);
                        }
                    }
                }

                
            }
            catch (System.Exception f)
            {
                
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnFileOpenDialog_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return objPriceListForPrintSorted;
        }


        #endregion

        #region Прайс-лист (редакция Радюк Лены)
        private void mitmspriceF2_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPrice(m_iPaymentType2);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel (mitmspriceF1_Click).\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }


        }
        private void mitmspriceF1_Click(object sender, EventArgs e)
        {
            try
            {
                PrintPrice(m_iPaymentType1);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel (mitmspriceF1_Click).\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }

        /// <summary>
        /// Печать прайса для указанной формы оплаты
        /// </summary>
        /// <param name="objPaymentType">форма оплаты</param>
        private void PrintPrice( System.Guid PaymentTypeId )
        {
            try
            {
                System.String strFileName = "";
                System.String strDLLPath = Application.StartupPath;
                strDLLPath += ("\\" + m_strReportsDirectory + "\\");

                if (PaymentTypeId.CompareTo(m_iPaymentType2) == 0)
                {
                    strFileName = strDLLPath + m_strReportPriceF2;
                }
                else if (PaymentTypeId.CompareTo(m_iPaymentType1) == 0)
                {
                    strFileName = strDLLPath + m_strReportPriceF1;
                }

                if (System.IO.File.Exists(strFileName) == false)
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.Refresh();
                        if ((openFileDialog.FileName != "") && (System.IO.File.Exists(openFileDialog.FileName) == true))
                        {
                            strFileName = openFileDialog.FileName;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                SendMessageToLog("Идёт экспорт данных в MS Excel... ");
                this.Cursor = Cursors.WaitCursor;

                PrintPriceForProductOwner2(strFileName, PaymentTypeId);

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel (PrintPrice).\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Выгружает в лист MS Excel информацию о ценах
        /// </summary>
        /// <param name="strFileName">путь к файлу</param>
        /// <param name="uuidjPaymentTypeID">уи формы оплаты</param>
        private void PrintPriceForProductOwner2(System.String strFileName, System.Guid uuidjPaymentTypeID)
        {
            if (strFileName == "") { return; }

            try
            {
                List<ERP_Mercury.Common.CProductTradeMark> objProductTradeMarkList = ERP_Mercury.Common.CProductTradeMark.GetProductTradeMarkList(m_objProfile, null);
                if ((objProductTradeMarkList == null) || (objProductTradeMarkList.Count == 0))
                {
                    this.Cursor = Cursors.Default;
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                                        "Не удалось получить список товарных марок.", "Внимание",
                                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                System.String strTmpPath = System.IO.Path.GetTempPath();
                System.IO.FileInfo newFileSrc = new System.IO.FileInfo(strFileName);

                System.String strFileNameCopy = (strTmpPath + newFileSrc.Name.Replace(newFileSrc.Extension, ( "(2)" + newFileSrc.Extension)) );
                System.IO.FileInfo newFile = new System.IO.FileInfo(strFileNameCopy);

                if (newFile.Exists == true)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new System.IO.FileInfo(strFileNameCopy);
                }

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    using (ExcelPackage packageSrc = new ExcelPackage(newFileSrc))
                    {
                        ExcelWorksheet worksheet = null;
                        ExcelWorksheet worksheetSrc = null;


                        List<CProductSubTypePriceListExport> objPriceList = null;
                        foreach (ERP_Mercury.Common.CProductTradeMark objProductTradeMark in objProductTradeMarkList)
                        {
                            objPriceList = CProductSubTypePriceListExport.LoadPriceListForProduct(m_objProfile, null, objProductTradeMark.ID, uuidjPaymentTypeID);

                            if ((objPriceList == null) || (objPriceList.Count == 0)) { continue; }

                            worksheetSrc = packageSrc.Workbook.Worksheets[objPriceList[0].SheetNum];

                            worksheet = package.Workbook.Worksheets.Add(worksheetSrc.Name, worksheetSrc);

                            PrintPriceForProductOwner2(worksheet, objPriceList);

                        }

                        objPriceList = null;
                        worksheet = null;
                        worksheetSrc = null;

                        package.Save();
                    }
                }

                try
                {
                    using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                    {
                        process.StartInfo.FileName = strFileNameCopy;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        process.Start();
                    }
                }
                catch
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(this, "Cannot find an application on your system suitable for openning the file with exported data.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Выгружает в лист MS Excel информацию о ценах
        /// </summary>
        /// <param name="oSheet">лист MS Excel</param>
        /// <param name="objPriceList">список с ценами</param>
        private void PrintPriceForProductOwner2(ExcelWorksheet worksheet, List<CProductSubTypePriceListExport> objPriceList)
        {
            if (worksheet == null) { return; }
            if ((objPriceList == null) || (objPriceList.Count == 0)) { return; }

            System.Int32 iStartRow = 7;
            System.Int32 iColumnProductSubType = 2;
            System.Int32 iColumnPrice = 3;
            System.String strProductTypeCurrent = "";

            System.Int32 iCurrentRow = iStartRow;
            System.Int32 iOrderNum = 0;
            System.String strFormulaR1C1 = worksheet.Cells[(iStartRow + 1), 4].FormulaR1C1;
            System.String strExcelNumberFormat = worksheet.Cells[(iStartRow + 1), 3].Style.Numberformat.Format;
            System.String strExcelNumberFormat2 = worksheet.Cells[(iStartRow + 1), 4].Style.Numberformat.Format;
            System.DateTime maxDateUdatedRecord = System.DateTime.MinValue;
            try
            {

                foreach (CProductSubTypePriceListExport objProductSubType in objPriceList)
                {
                    if (System.DateTime.Compare(maxDateUdatedRecord, objProductSubType.DateUpdated) < 0)
                    {
                        maxDateUdatedRecord = objProductSubType.DateUpdated;
                    }
                    if ((objProductSubType.ProductType.Name != "") && (objProductSubType.ProductType.Name != strProductTypeCurrent))
                    {
                        // информация о товарной группе
                        using (var range = worksheet.Cells[iCurrentRow, iColumnProductSubType, iCurrentRow, ( iColumnProductSubType + 2 ) ])
                        {
                            //range.Merge = true;
                            range.Style.Font.Bold = true;
                            range.Style.Font.Size = 10;
                            range.Style.Font.Color.SetColor(Color.Black);
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor( Color.FromArgb( 145, 238, 243 ) );
                            range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;

                            range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        worksheet.Cells[iCurrentRow, iColumnProductSubType].Value = objProductSubType.ProductType.Name.ToUpper();

                        strProductTypeCurrent = objProductSubType.ProductType.Name;
                        iCurrentRow++;
                        iOrderNum++;

                    }

                    using (var range = worksheet.Cells[iCurrentRow, iColumnProductSubType, iCurrentRow, (iColumnProductSubType + 2)])
                    {
                        range.Style.Font.Color.SetColor(Color.Black);
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                    }
                    // подгруппа
                    using (var range = worksheet.Cells[iCurrentRow, iColumnProductSubType, iCurrentRow, iColumnProductSubType])
                    {
                        range.Style.Font.Bold = false;
                        range.Style.Font.Size = 10;
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    }
                    // цены
                    using (var range = worksheet.Cells[iCurrentRow, iColumnPrice, iCurrentRow, iColumnPrice])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Size = 10;
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Numberformat.Format = strExcelNumberFormat;
                        range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }

                    using (var range = worksheet.Cells[iCurrentRow, (iColumnPrice + 1), iCurrentRow, (iColumnPrice + 1)])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Size = 10;
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Numberformat.Format = strExcelNumberFormat2;
                        range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }

                    worksheet.Cells[iCurrentRow, iColumnProductSubType].Value = objProductSubType.ProductSubType.Name;
                    worksheet.Cells[iCurrentRow, iColumnPrice].Value = objProductSubType.Price.PriceValue;
                    worksheet.Cells[iCurrentRow, (iColumnPrice + 1)].FormulaR1C1 = strFormulaR1C1;

                    iCurrentRow++;
                }

                iCurrentRow--;
                worksheet.Cells[iStartRow, iColumnProductSubType, iCurrentRow, iColumnProductSubType].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[iStartRow, (iColumnPrice + 1), iCurrentRow, (iColumnPrice + 1)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[iCurrentRow, iColumnProductSubType, iCurrentRow, (iColumnPrice + 1)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells[3, 2].Value = ("с " + maxDateUdatedRecord.ToLongDateString());
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

    }

    public class ViewPriceList : PlugIn.IClassTypeView
    {
        public override void Run(UniXP.Common.MENUITEM objMenuItem, System.String strCaption)
        {
            frmPriceList obj = new frmPriceList(objMenuItem.objProfile, objMenuItem);
            obj.Text = strCaption;
            obj.MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent;
            obj.Visible = true;
        }

    }

}
