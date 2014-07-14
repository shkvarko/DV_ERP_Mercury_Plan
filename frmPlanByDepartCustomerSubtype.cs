using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP_Mercury.Common;
using UniXP.Common;
using OfficeOpenXml;
using System.Threading;

namespace ERPMercuryPlan
{
    public partial class frmPlanByDepartCustomerSubtype : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private System.Boolean m_bOnlyView;
        private System.Boolean m_bIsChanged;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewObject;

        private List<CPlanByDepartCustomerSubType> m_objPlanList;
        private List<CPlanByDepartCustomerSubTypeItem> m_objPlanItemList;

        private List<CProductTradeMark> m_objProductTradeMarkList;
        private List<CProductType> m_objProductTypeList;
        private List<CDepartTeam> m_objDepartTeamList;
        private List<CDepart> m_objDepartList;
        private List<CProductSubType> m_objProductSubTypeList;
        private List<CCustomer> m_objCustomerList;
        private frmSalesPlanItemEditor PlanItemEditor;
        private frmImportPlanByDepartCustomerSubtype PlanImportEditor;

        private CPlanByDepartCustomerSubType SelectedSalesPlan
        {
            get
            {
                CPlanByDepartCustomerSubType objRet = null;
                try
                {
                    if ((gridViewPlanList.RowCount > 0) && (gridViewPlanList.FocusedRowHandle >= 0) && (m_objPlanList != null))
                    {
                        System.Guid uuidID = (System.Guid)(gridViewPlanList.GetFocusedRowCellValue("ID"));

                        objRet = m_objPlanList.Single<CPlanByDepartCustomerSubType>(x => x.ID.CompareTo(uuidID) == 0);
                    }
                }//try
                catch (System.Exception f)
                {
                    SendMessageToLog("Ошибка поиска идентификатора выбранного плана. Текст ошибки: " + f.Message);
                }
                finally
                {
                }

                return objRet;
            }
        }

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlPlanList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewPlanItem
        {
            get { return gridControlPlanItem.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }


        // потоки
        public System.Threading.Thread ThreadCalcPlan { get; set; }

        public System.Threading.ManualResetEvent EventStopThread { get; set; }
        public System.Threading.ManualResetEvent EventThreadStopped { get; set; }

        public delegate void LoadCalcPlanDelegate(List<CPlanByDepartCustomerSubTypeItem> objPlanItemList, System.Int32 iRowCountInLis);
        public LoadCalcPlanDelegate m_LoadCalcPlanDelegate;

        public delegate void LoadComboBoxForDecodeEditorDelegate(List<CDepartTeam> objDepartTeamList,
           List<CDepart> objDepartList, List<CCustomer> objCustomerList,
           List<CProductSubType> objProductSubTypeList, List<CProductTradeMark> objProductTradeMarkList,
           List<CProductType> objProductTypeList);
        public LoadComboBoxForDecodeEditorDelegate m_LoadComboBoxForDecodeEditorDelegate;
        public System.Threading.Thread ThreadComboBoxForDecodeEditor { get; set; }

        private const System.Int32 iThreadSleepTime = 1000;
        private const System.String strWaitCustomer = "ждите... идет выполнение расчёта";
        private System.Boolean m_bThreadFinishJob;
        private const System.String strRegistryTools = "\\PlanByDepartCustomerSubtypeListTools\\";
        private const System.Int32 iWaitingpanelIndex = 0;
        private const System.Int32 iWaitingpanelHeight = 35;
        private const System.String m_strModeReadOnly = "Режим просмотра";
        private const System.String m_strModeEdit = "Режим редактирования";
        private System.String m_strXLSImportFilePath;
        private System.Int32 m_iXLSSheetImport;
        private List<System.String> m_SheetList;


        #endregion

        #region Конструктор
        public frmPlanByDepartCustomerSubtype(UniXP.Common.MENUITEM objMenuItem)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            InitializeComponent();

            m_objMenuItem = objMenuItem;
            m_objProfile = objMenuItem.objProfile;
            m_bThreadFinishJob = false;
            m_objPlanList = new List<CPlanByDepartCustomerSubType>();
            m_objPlanItemList = null;

            m_objProductTradeMarkList = null;
            m_objProductTypeList = null;
            m_objDepartTeamList = null;
            m_objDepartList = null;
            m_objProductSubTypeList = null;
            m_objCustomerList = null;

            AddGridColumns();

            dtBeginDate.DateTime = System.DateTime.Today.AddMonths(-1);
            dtEndDate.DateTime = System.DateTime.Today;

            tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            m_bOnlyView = false;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewObject = false;

            SearchProcessWoring.Visible = false;
            m_strXLSImportFilePath = "";
            m_iXLSSheetImport = 0;
            m_SheetList = new List<string>();

            PlanItemEditor = new frmSalesPlanItemEditor( m_objProfile );
            PlanImportEditor = new frmImportPlanByDepartCustomerSubtype(m_objProfile, m_objMenuItem);
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
                    "SendMessageToLog.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Настройки грида
        /// <summary>
        /// Настройка внешнего вида гридов
        /// </summary>
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();

            AddGridColumn(ColumnView, "ID", "Идентификатор");
            AddGridColumn(ColumnView, "Name", "Наименование");
            AddGridColumn(ColumnView, "CreateDate", "Дата создания");
            AddGridColumn(ColumnView, "BeginDate", "Период с");
            AddGridColumn(ColumnView, "EndDate", "по");
            AddGridColumn(ColumnView, "PlanQuantity", "Кол-во");
            AddGridColumn(ColumnView, "PlanAllPrice", "Сумма");
            AddGridColumn(ColumnView, "IsUseForReport", "Вкл.");
            AddGridColumn(ColumnView, "CalcPlanName", "План по маркам и группам");
            AddGridColumn(ColumnView, "SalesPlanQuotaName", "расчёт долей продаж");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;

                if (objColumn.FieldName == "ID")
                {
                    objColumn.Visible = false;
                }
                if ((objColumn.FieldName == "PlanQuantity") || (objColumn.FieldName == "PlanAllPrice"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0";

                    objColumn.SummaryItem.FieldName = objColumn.FieldName;
                    objColumn.SummaryItem.DisplayFormat = "Итого: {0:### ### ##0}";
                    objColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                }
            }

            // расшифровка плана
            ColumnViewPlanItem.Columns.Clear();

            AddGridColumn(ColumnViewPlanItem, "ID", "Идентификатор");
            AddGridColumn(ColumnViewPlanItem, "ProductOwnerName", "Товарная марка");
            AddGridColumn(ColumnViewPlanItem, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnViewPlanItem, "DepartTeamName", "Команда");
            AddGridColumn(ColumnViewPlanItem, "DepartCode", "Подразделение");
            AddGridColumn(ColumnViewPlanItem, "CustomerName", "Клиент");
            AddGridColumn(ColumnViewPlanItem, "ProductSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnViewPlanItem, "Plan_Quantity", "План (количество)");
            AddGridColumn(ColumnViewPlanItem, "Plan_AllPrice", "План (сумма)");

            AddGridColumn(ColumnViewPlanItem, "ProductOwnerID", "Товарная марка (код)");
            AddGridColumn(ColumnViewPlanItem, "ProductTypeID", "Товарная группа (код)");
            AddGridColumn(ColumnViewPlanItem, "ProductSubTypeID", "Товарная подгруппа (код)");
            AddGridColumn(ColumnViewPlanItem, "DepartID", "Подразделение (код)");
            AddGridColumn(ColumnViewPlanItem, "CustomerID", "Клиент (код)");

            AddGridColumn(ColumnViewPlanItem, "ProductOwnerIbID", "Товарная марка (код)");
            AddGridColumn(ColumnViewPlanItem, "ProductTypeIbID", "Товарная группа (код)");
            AddGridColumn(ColumnViewPlanItem, "ProductSubTypeIbID", "Товарная подгруппа (код)");
            AddGridColumn(ColumnViewPlanItem, "CustomerIbID", "Клиент (код)");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewPlanItem.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;

                if ((objColumn.FieldName == "ID") || (objColumn.FieldName == "ProductOwnerID") ||
                    (objColumn.FieldName == "ProductTypeID") || (objColumn.FieldName == "ProductSubTypeID") ||
                    (objColumn.FieldName == "ProductOwnerIbID") || (objColumn.FieldName == "ProductTypeIbID") ||
                    (objColumn.FieldName == "ProductSubTypeIbID") || (objColumn.FieldName == "CustomerIbID") ||
                    (objColumn.FieldName == "DepartID") || (objColumn.FieldName == "CustomerID"))
                {
                    objColumn.Visible = false;
                }

                if ((objColumn.FieldName == "Plan_Quantity") || (objColumn.FieldName == "Plan_AllPrice"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = ((objColumn.FieldName == "Plan_Quantity") ? "### ### ##0" : "### ### ##0.00");

                    objColumn.SummaryItem.FieldName = objColumn.FieldName;
                    objColumn.SummaryItem.DisplayFormat = "Итого: {0:### ### ##0}";
                    objColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                }
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

        #region Настройки внешнего вида журналов
        /// <summary>
        /// Считывает настройки журналов из реестра
        /// </summary>
        public void RestoreLayoutFromRegistry()
        {
            System.String strReestrPath = this.m_objProfile.GetRegKeyBase();
            strReestrPath += (strRegistryTools);
            try
            {
                gridViewPlanList.RestoreLayoutFromRegistry(strReestrPath + gridViewPlanList.Name);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка загрузки настроек журнала расчётов.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        /// <summary>
        /// Записывает настройки журналов в реестр
        /// </summary>
        public void SaveLayoutToRegistry()
        {
            System.String strReestrPath = this.m_objProfile.GetRegKeyBase();
            strReestrPath += (strRegistryTools);
            try
            {
                gridViewPlanList.SaveLayoutToRegistry(strReestrPath + gridViewPlanList.Name);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка записи настроек журнала планов продаж.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        #endregion

        #endregion

        #region Журнал планов
        /// <summary>
        /// Загружает журнал планов
        /// </summary>
        public void LoadPlanList()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (m_objPlanList != null)
                { m_objPlanList.Clear(); }

                barBtnAdd.Enabled = false;
                barBtnEdit.Enabled = false;
                barBtnDelete.Enabled = false;
                barBtnRefresh.Enabled = false;
                btnPrint.Enabled = false;
                dtBeginDate.Enabled = false;
                dtEndDate.Enabled = false;

                gridControlPlanList.DataSource = null;

                System.DateTime Begindate = dtBeginDate.DateTime;
                System.DateTime Enddate = dtEndDate.DateTime;
                System.String strErr = "";
                System.Int32 iRes = 0;

                m_objPlanList = CPlanByDepartCustomerSubType.GetPlanByDepartCustomerSubTypeList(m_objProfile, Begindate, Enddate, ref strErr, ref iRes);
                if (gridControlPlanList.DataSource == null)
                {
                    gridControlPlanList.DataSource = m_objPlanList;
                }
                gridControlPlanList.RefreshDataSource();

                barBtnAdd.Enabled = true;
                barBtnEdit.Enabled = true;
                barBtnDelete.Enabled = true;
                barBtnRefresh.Enabled = true;
                btnPrint.Enabled = true;
                dtBeginDate.Enabled = true;
                dtEndDate.Enabled = true;

                if (m_objPlanList.Count > 0)
                {
                    gridViewPlanList.FocusedRowHandle = 0;
                }

                gridViewPlanList.BestFitColumns();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadPlanList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }

        private void dtBeginDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar == (char)Keys.Enter) && (barBtnRefresh.Visible == true))
                {
                    LoadPlanList();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("dtBeginDate_KeyPress.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Загружает выпадающие списки для редактора плана продаж
        /// </summary>
        private void LoadCombobox()
        {
            try
            {

                editorCurrency.Properties.Items.Clear();
                editorSalesPlanQuota.Properties.Items.Clear();
                editorCalcPlan.Properties.Items.Clear();

                System.String strErr = System.String.Empty;
                System.Int32 iRes = 0;

                List<CCurrency> objCurrencyList = CCurrency.GetCurrencyList(m_objProfile, null);
                List<CSalesPlanQuota> objSalesPlanQuotaList = CSalesPlanQuota.GetSalesPlanQuotaList(m_objProfile, System.DateTime.Today.AddYears(-1), System.DateTime.Today.AddYears(1), ref strErr, ref iRes);
                List<CCalcPlan> objCalcPlanList = CCalcPlan.GetCalcPlanList(m_objProfile, null);

                if( (objCurrencyList != null) && (objCurrencyList.Count > 0))
                {
                    editorCurrency.Properties.Items.AddRange(objCurrencyList);
                }
                if ((objSalesPlanQuotaList != null) && (objSalesPlanQuotaList.Count > 0))
                {
                    editorSalesPlanQuota.Properties.Items.AddRange(objSalesPlanQuotaList);
                }
                if ((objCalcPlanList != null) && (objCalcPlanList.Count > 0))
                {
                    editorCalcPlan.Properties.Items.AddRange(objCalcPlanList);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadCombobox().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void frmPlanByDepartCustomerSubtype_Shown(object sender, EventArgs e)
        {
            try
            {

                RestoreLayoutFromRegistry();

                tabControl.SelectedTabPage = tabPageViewer;

                LoadCombobox();

                LoadPlanList();

                StartThreadComboBoxForDecodeEditor();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("frmPlanByDepartCustomerSubtype_Shown().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void barBtnRefresh_Click(object sender, EventArgs e)
        {
            LoadPlanList();
        }

        #endregion

        #region Свойства плана

        private void gridViewPlanList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                FocusedPlanChanged();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewPlanList_FocusedRowChanged. Текст ошибки: " + f.Message);
            }

            return;
        }

        private void gridViewPlanList_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                FocusedPlanChanged();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewPlanList_RowCountChanged. Текст ошибки: " + f.Message);
            }

            return;
        }

        /// <summary>
        /// Отображает информацию о плане на панели свойств
        /// </summary>
        private void FocusedPlanChanged()
        {
            try
            {
                ShowPlanProperties( SelectedSalesPlan);

                barBtnAdd.Enabled = !m_bOnlyView;
                barBtnEdit.Enabled = (gridViewPlanList.FocusedRowHandle >= 0);
                barBtnDelete.Enabled = ((!m_bOnlyView) && (gridViewPlanList.FocusedRowHandle >= 0));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Отображение свойств плана. Текст ошибки: " + f.Message);
            }

            return;
        }

        /// <summary>
        /// Отображает свойства плана
        /// </summary>
        /// <param name="objPlan">план</param>
        private void ShowPlanProperties(CPlanByDepartCustomerSubType objPlan)
        {
            try
            {
                this.tableLayoutPanelPlanProperties.SuspendLayout();

                txtName.Text = System.String.Empty;
                txtCreateDate.Text = System.String.Empty;
                txtBeginEndDate.Text = System.String.Empty;
                txtCurrency.Text = System.String.Empty;
                txtCalcPlan.Text = System.String.Empty;
                txtSalesPlanQuota.Text = System.String.Empty;
                txtDescription.Text = System.String.Empty;

                if(objPlan != null)
                {
                    txtName.Text = objPlan.Name;
                    txtCreateDate.Text = objPlan.CreateDate.ToShortDateString();
                    txtBeginEndDate.Text = String.Format("{0} - {1}", objPlan.BeginDate.ToShortDateString(), objPlan.EndDate.ToShortDateString());
                    txtCurrency.Text = ((objPlan.Currency == null) ? System.String.Empty : objPlan.Currency.CurrencyAbbr);
                    txtCalcPlan.Text = objPlan.CalcPlanName;
                    txtSalesPlanQuota.Text = objPlan.SalesPlanQuotaName;
                    txtDescription.Text = objPlan.Description;
                }

                this.tableLayoutPanelPlanProperties.ResumeLayout(false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Отображение свойств плана. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Редактор приложения к плану
        /// <summary>
        /// Загружает выпадающие списки в редактор приложения к плану
        /// </summary>
        public void StartThreadComboBoxForDecodeEditor()
        {
            try
            {
                // инициализируем делегаты
                m_LoadComboBoxForDecodeEditorDelegate = new LoadComboBoxForDecodeEditorDelegate(LoadComboBoxForDecodeEditor);

                // запуск потока
                this.ThreadComboBoxForDecodeEditor = new System.Threading.Thread(LoadComboBoxForDecodeEditorInThread);
                this.ThreadComboBoxForDecodeEditor.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadComboBoxForDecodeEditor().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        /// <summary>
        /// Загружает выпадающие списки в редактор приложения к расчёту (метод, выполняемый в потоке)
        /// </summary>
        public void LoadComboBoxForDecodeEditorInThread()
        {
            try
            {
                m_objDepartList = CDepart.GetDepartList(m_objProfile);
                m_objDepartTeamList = CDepartTeam.GetDepartTeamList(m_objProfile, null, true);
                m_objProductTradeMarkList = CProductTradeMark.GetProductTradeMarkList(m_objProfile, null);
                m_objProductTypeList = CProductType.GetProductTypeList(m_objProfile, null);
                m_objCustomerList = CCustomer.GetCustomerListWithoutAdvancedProperties(m_objProfile, null, null);
                m_objProductSubTypeList = CProductSubType.GetProductSubTypeList(m_objProfile, null, true);

                this.Invoke(m_LoadComboBoxForDecodeEditorDelegate,
                    new Object[] {
                                    m_objDepartTeamList, m_objDepartList, m_objCustomerList,
                                    m_objProductSubTypeList, m_objProductTradeMarkList,
                                    m_objProductTypeList 
                                  });

                this.Invoke(m_LoadComboBoxForDecodeEditorDelegate,
                    new Object[] { null, null, null, null, null, null });
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadComboBoxForDecodeEditorInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Загрузка выпадающих списков значений в редактор приложения к расчёту
        /// </summary>
        /// <param name="objDepartTeamList">список команд</param>
        /// <param name="objDepartList">список подразделений</param>
        /// <param name="objCustomerList">список клиентов</param>
        /// <param name="objProductSubTypeList">список подгрупп</param>
        /// <param name="objProductTradeMarkList">список товарных марок</param>
        /// <param name="objProductTypeList">список товарных групп</param>
        private void LoadComboBoxForDecodeEditor(List<CDepartTeam> objDepartTeamList,
           List<CDepart> objDepartList, List<CCustomer> objCustomerList,
           List<CProductSubType> objProductSubTypeList, List<CProductTradeMark> objProductTradeMarkList,
           List<CProductType> objProductTypeList)
        {
            try
            {
                if (
                    (objDepartTeamList != null) && (objDepartList.Count > 0) &&
                    (objDepartList != null) && (objDepartList.Count > 0) &&
                    (objCustomerList != null) && (objCustomerList.Count > 0) &&
                    (objProductSubTypeList != null) && (objProductSubTypeList.Count > 0) &&
                    (objProductTradeMarkList != null) && (objProductTradeMarkList.Count > 0) &&
                    (objProductTypeList != null) && (objProductTypeList.Count > 0)
                    )
                {
                    if (PlanItemEditor != null)
                    {
                        PlanItemEditor.LoadComboBox( objDepartList,
                            objCustomerList, objProductSubTypeList, objProductTradeMarkList, objProductTypeList);
                    }
                    if (PlanImportEditor != null)
                    {
                        PlanImportEditor.LoadComboBox(objDepartList,
                            objCustomerList, objProductSubTypeList, objProductTradeMarkList, objProductTypeList);
                    }
                }
                else
                {
                    // процесс загрузки данных завершён
                    Thread.Sleep(iThreadSleepTime);
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadComboBoxForDecodeEditor.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void mitemAddSalePlanQuotaItem_Click(object sender, EventArgs e)
        {
            NewPlanItemForGrid();
        }

        /// <summary>
        /// Добавление нового элемента в приложение к плану
        /// </summary>
        private void NewPlanItemForGrid()
        {
            try
            {

                if (PlanItemEditor != null)
                {
                    PlanItemEditor.AddPlanItemForGrid();
                    PlanItemEditor.ShowDialog();

                    DialogResult dlgRes = PlanItemEditor.DialogResult;

                    if (dlgRes == System.Windows.Forms.DialogResult.OK)
                    {
                        AddPlanItemToList(PlanItemEditor.PlanItem);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("NewPlanItemForGrid.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Добавление нового элемента в приложение к плану
        /// </summary>
        /// <param name="objAdded">новый элемент</param>
        private void AddPlanItemToList(CPlanByDepartCustomerSubTypeItem objAdded)
        {
            try
            {
                if (objAdded.Plan_Quantity <= 0) { objAdded.Plan_Quantity = 1; }
                if (objAdded.Plan_AllPrice <= 0) { objAdded.Plan_AllPrice = 1; }

                // проверка на то, имеется ли в списке точно такой же элемент
                CPlanByDepartCustomerSubTypeItem objTwin = m_objPlanItemList.SingleOrDefault<CPlanByDepartCustomerSubTypeItem>(x => (x.ProductOwnerID.CompareTo(objAdded.ProductOwnerID) == 0) &&
                    (x.ProductTypeID.CompareTo(objAdded.ProductTypeID) == 0) &&
                    (x.DepartID.CompareTo(objAdded.DepartID) == 0) &&
                    (x.CustomerID.CompareTo(objAdded.CustomerID) == 0) &&
                    (x.ProductSubTypeID.CompareTo(objAdded.ProductSubTypeID) == 0) );

                if (objTwin != null)
                {
                    // редактируется количество и сумма
                    objTwin.Plan_Quantity += objAdded.Plan_Quantity;
                    objTwin.Plan_AllPrice += objAdded.Plan_AllPrice;
                }
                else
                {
                    m_objPlanItemList.Add(objAdded);
                }

                gridControlPlanItem.RefreshDataSource();
                if (m_objPlanItemList.Count == 1)
                {
                    gridViewPlanItem.BestFitColumns();
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("AddSalesPlanQuotaItemForGridToList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void mitemEditSalePlanQuotaItem_Click(object sender, EventArgs e)
        {
            EditPlanItem();
        }
        /// <summary>
        /// Возвращает выделенные элемент приложения к плану
        /// </summary>
        /// <returns>элемент приложения к плану</returns>
        private CPlanByDepartCustomerSubTypeItem GetSelectedPlanItem()
        {
            CPlanByDepartCustomerSubTypeItem objItem = null;
            try
            {
                System.Guid ProductTradeMarkID = System.Guid.Empty;
                System.Guid ProductTypeID = System.Guid.Empty;
                System.Guid ProductSubTypeID = System.Guid.Empty;
                System.Guid DepartID = System.Guid.Empty;
                System.Guid CustomerID = System.Guid.Empty;

                if ((gridViewPlanItem.RowCount > 0) &&
                    (gridViewPlanItem.FocusedRowHandle >= 0))
                {
                    ProductTradeMarkID = (System.Guid)gridViewPlanItem.GetFocusedRowCellValue("ProductOwnerID");
                    ProductTypeID = (System.Guid)gridViewPlanItem.GetFocusedRowCellValue("ProductTypeID");
                    ProductSubTypeID = (System.Guid)gridViewPlanItem.GetFocusedRowCellValue("ProductSubTypeID");
                    DepartID = (System.Guid)gridViewPlanItem.GetFocusedRowCellValue("DepartID");
                    CustomerID = (System.Guid)gridViewPlanItem.GetFocusedRowCellValue("CustomerID");

                    objItem = m_objPlanItemList.SingleOrDefault<CPlanByDepartCustomerSubTypeItem>(x => (x.ProductOwnerID.CompareTo(ProductTradeMarkID) == 0) &&
                    (x.ProductTypeID.CompareTo(ProductTypeID) == 0) &&
                    (x.DepartID.CompareTo(DepartID) == 0) &&
                    (x.CustomerID.CompareTo(CustomerID) == 0) &&
                    (x.ProductSubTypeID.CompareTo(ProductSubTypeID) == 0));
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("GetSelectedItem.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return objItem;
        }
        /// <summary>
        /// Редактирование элемента в приложении к плану
        /// </summary>
        private void EditPlanItem()
        {
            try
            {
                if (PlanItemEditor != null)
                {
                    CPlanByDepartCustomerSubTypeItem objPlanItem = GetSelectedPlanItem();

                    if (objPlanItem != null)
                    {
                        PlanItemEditor.EditPlanItem(objPlanItem);
                        PlanItemEditor.ShowDialog();

                        DialogResult dlgRes = PlanItemEditor.DialogResult;

                        if (dlgRes == System.Windows.Forms.DialogResult.OK)
                        {
                            gridControlPlanItem.RefreshDataSource();
                        }

                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("EditPlanItem.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Удаление элемента в приложении к плану
        /// </summary>
        /// <param name="objPlanItem">элемент в приложении к плану продаж</param>
        private void DeletePlanItem(CPlanByDepartCustomerSubTypeItem objPlanItem)
        {
            if (objPlanItem == null) { return; }
            try
            {
                m_objPlanItemList.Remove(objPlanItem);
                gridControlPlanItem.RefreshDataSource();

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("DeletePlanItem.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Удаление всех записей в приложении к расчёту
        /// </summary>
        private void DeleteAllPlanItem()
        {
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("Подтвердите, пожалуйста, удаление всех записей в приложении к плану продаж.", "Подтверждение",
                   System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes )
                {
                    m_objPlanItemList.Clear();
                    gridControlPlanItem.RefreshDataSource();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("DeleteAllPlanItem.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void mitemDeleteSalePlanQuotaItem_Click(object sender, EventArgs e)
        {
            DeletePlanItem(GetSelectedPlanItem());
        }
        private void mitmsDeleteAllRecorde_Click(object sender, EventArgs e)
        {
            DeleteAllPlanItem();
        }

        private void contextMenuSalePlanQuotaItem_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                CPlanByDepartCustomerSubTypeItem objPlanItem = GetSelectedPlanItem();
                if (objPlanItem == null)
                {
                    mitemDeleteSalePlanQuotaItem.Enabled = false;
                    mitemEditSalePlanQuotaItem.Enabled = false;
                }
                else
                {
                    mitemDeleteSalePlanQuotaItem.Enabled = true;
                    mitemEditSalePlanQuotaItem.Enabled = true;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("contextMenuSalePlanQuotaItem_Opening.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Режим просмотра/редактирования
        /// <summary>
        /// Устанавливает режим просмотра/редактирования
        /// </summary>
        /// <param name="bSet">true - режим просмотра; false - режим редактирования</param>
        private void SetModeReadOnly(System.Boolean bModeReadOnly)
        {
            try
            {

                editorBeginDate.Properties.ReadOnly = bModeReadOnly;
                editorCalcPlan.Properties.ReadOnly = bModeReadOnly;
                editorCreateDate.Properties.ReadOnly = bModeReadOnly;
                editorCurrency.Properties.ReadOnly = bModeReadOnly;
                editorDescription.Properties.ReadOnly = bModeReadOnly;
                editorIsUseForReport.Properties.ReadOnly = bModeReadOnly;
                editorEndDate.Properties.ReadOnly = bModeReadOnly;
                editorName.Properties.ReadOnly = bModeReadOnly;
                editorSalesPlanQuota.Properties.ReadOnly = bModeReadOnly;

                btnStartCalc.Enabled = (bModeReadOnly == false);

                gridControlPlanItem.ContextMenuStrip = ((bModeReadOnly == false) ? contextMenuSalePlanQuotaItem : null);

                btnEdit.Enabled = bModeReadOnly;

                lblEditMode.Text = ((bModeReadOnly == true) ? m_strModeReadOnly : m_strModeEdit);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetModeReadOnly. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                SetModeReadOnly(false);
                btnEdit.Enabled = false;
                m_bNewObject = false;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnEdit_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void SetPropertiesModified(System.Boolean bModified)
        {
            try
            {
                m_bIsChanged = bModified;
                btnSave.Enabled = (m_bIsChanged && (ValidateProperties() == true));
                btnCancel.Enabled = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetPropertiesModified. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Индикация изменений
        /// <summary>
        /// Проверяет содержимое элементов управления
        /// </summary>
        private System.Boolean ValidateProperties()
        {
            System.Boolean bRet = true;
            try
            {
                editorName.Properties.Appearance.BackColor = ((editorName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorCalcPlan.Properties.Appearance.BackColor = ((editorCalcPlan.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorCurrency.Properties.Appearance.BackColor = ((editorCurrency.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorSalesPlanQuota.Properties.Appearance.BackColor = ((editorSalesPlanQuota.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorBeginDate.Properties.Appearance.BackColor = ((editorBeginDate.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorEndDate.Properties.Appearance.BackColor = ((editorEndDate.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorCreateDate.Properties.Appearance.BackColor = ((editorCreateDate.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                bRet = ((editorName.Text != "") && (editorCreateDate.EditValue != null) &&
                    (editorBeginDate.EditValue != null) && (editorEndDate.EditValue != null) &&
                    (editorCalcPlan.EditValue != null) && (editorCurrency.EditValue != null) &&
                    (editorSalesPlanQuota.EditValue != null) 
                    );

            }
            catch (System.Exception f)
            {
                SendMessageToLog("ValidateProperties. Текст ошибки: " + f.Message);
            }

            return bRet;
        }

        private void txtPlanPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (e.NewValue != null)
                {
                    SetPropertiesModified(true);
                    if ((sender.GetType().Name == "TextEdit") &&
                        (((DevExpress.XtraEditors.TextEdit)sender).Properties.ReadOnly == false))
                    {
                        System.String strValue = (System.String)e.NewValue;
                        ((DevExpress.XtraEditors.TextEdit)sender).Properties.Appearance.BackColor = (strValue == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(String.Format("Ошибка изменения свойств плана. Текст ошибки: {0}", f.Message));
            }
            finally
            {
            }

            return;
        }

        private void editorCurrency_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(String.Format("Ошибка изменения свойств плана. Текст ошибки: {0}", f.Message));
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Редактировать план
        private void barBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                EditPlan(SelectedSalesPlan, false);

                SetModeReadOnly(false);
                btnEdit.Enabled = false;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("barBtnEdit_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void gridPlanList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                EditPlan(SelectedSalesPlan, false);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// очистка содержимого элементов управления
        /// </summary>
        private void ClearControls()
        {
            try
            {
                editorBeginDate.EditValue = null;
                editorCalcPlan.SelectedItem = null;
                editorCreateDate.EditValue = null;
                editorCurrency.SelectedItem = null;
                editorDescription.Text = "";
                editorIsUseForReport.CheckState = CheckState.Unchecked;
                editorEndDate.EditValue = null;
                editorName.Text = "";
                editorSalesPlanQuota.SelectedItem = null;

                gridControlPlanItem.DataSource = null;
                if ( m_objPlanItemList != null)
                {
                    m_objPlanItemList.Clear();
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ClearControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Загружает свойства плана для редактирования
        /// </summary>
        /// <param name="objPlan">план</param>
        /// <param name="bNewObject">признак "новый расчёт"</param>
        public void EditPlan(CPlanByDepartCustomerSubType objPlan, System.Boolean bNewObject)
        {
            if (objPlan == null) { return; }

            m_bDisableEvents = true;
            m_bNewObject = bNewObject;
            try
            {
                this.tableLayoutPanelBackground.SuspendLayout();

                ClearControls();

                editorBeginDate.EditValue = objPlan.BeginDate;
                editorCreateDate.EditValue = objPlan.CreateDate;
                editorDescription.Text = objPlan.Description;
                editorIsUseForReport.Checked = objPlan.IsUseForReport;
                editorEndDate.EditValue = objPlan.EndDate;
                editorName.Text = objPlan.Name;

                editorCalcPlan.SelectedItem = (editorCalcPlan.Properties.Items.Count == 0) ? null : editorCalcPlan.Properties.Items.Cast<CCalcPlan>().SingleOrDefault<CCalcPlan>(x => x.ID.CompareTo(objPlan.CalcPlan.ID) == 0);
                editorCurrency.SelectedItem = (editorCurrency.Properties.Items.Count == 0) ? null : editorCurrency.Properties.Items.Cast<CCurrency>().SingleOrDefault<CCurrency>(x => x.ID.CompareTo(objPlan.Currency.ID) == 0);
                editorSalesPlanQuota.SelectedItem = (editorSalesPlanQuota.Properties.Items.Count == 0) ? null : editorSalesPlanQuota.Properties.Items.Cast<CSalesPlanQuota>().SingleOrDefault<CSalesPlanQuota>(x => x.ID.CompareTo(objPlan.SalesPlanQuota.ID) == 0);

                System.String strErr = "";
                System.Int32 iRes = 0;

                objPlan.ContentsPlan = CPlanByDepartCustomerSubType.GetPlanItemList(m_objProfile, objPlan.ID, ref strErr, ref iRes);
                if ((objPlan.ContentsPlan != null) && (objPlan.ContentsPlan.Count > 0))
                {
                    m_objPlanItemList = objPlan.ContentsPlan;
                }

                gridControlPlanItem.DataSource = m_objPlanItemList;
                gridControlPlanItem.RefreshDataSource();

                SetPropertiesModified(false);
                ValidateProperties();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования плана. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.tableLayoutPanelBackground.ResumeLayout(false);
                m_bDisableEvents = false;
                btnCancel.Enabled = true;
                tabControl.SelectedTabPage = tabPageEditor;
            }
            return;
        }
        #endregion

        #region Новый план
        private void barBtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                NewPlan();

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Новый план
        /// </summary>
        public void NewPlan()
        {
            try
            {
                m_bNewObject = true;
                m_bDisableEvents = true;
                ClearControls();

                if (m_objPlanItemList == null)
                {
                    m_objPlanItemList = new List<CPlanByDepartCustomerSubTypeItem>();
                }
                gridControlPlanItem.DataSource = m_objPlanItemList;

                m_objPlanList.Add( new CPlanByDepartCustomerSubType()
                {
                    ID = System.Guid.Empty,
                    CreateDate = System.DateTime.Today,
                    Name = CPlanByDepartCustomerSubType.GetNewName(),
                    BeginDate = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, 1),
                    EndDate = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)
                });

                gridControlPlanList.RefreshDataSource();
                gridViewPlanList.FocusedRowHandle = (m_objPlanList.Count - 1);

                this.tableLayoutPanelBackground.SuspendLayout();

                editorBeginDate.EditValue = SelectedSalesPlan.BeginDate;
                editorCreateDate.EditValue = SelectedSalesPlan.CreateDate;
                editorDescription.Text = SelectedSalesPlan.Description;
                editorIsUseForReport.Checked = SelectedSalesPlan.IsUseForReport;
                editorEndDate.EditValue = SelectedSalesPlan.EndDate;
                editorName.Text = SelectedSalesPlan.Name;
                editorSalesPlanQuota.SelectedItem = null;

                btnEdit.Enabled = false;
                btnCancel.Enabled = true;

                SetModeReadOnly(false);
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания плана. Текст ошибки: " + f.Message);
            }
            finally
            {
                tableLayoutPanelBackground.ResumeLayout(false);
                m_bDisableEvents = false;

                if (tabControl.SelectedTabPage != tabPageEditor)
                {
                    tabControl.SelectedTabPage = tabPageEditor;
                }
            }
            return;
        }

        #endregion

        #region Удалить план
        /// <summary>
        /// Удаляет план
        /// </summary>
        /// <param name="objPlan">объект класса "План"</param>
        private void DeletePlan(CPlanByDepartCustomerSubType objPlan)
        {
            if (objPlan == null) { return; }
            System.String strErr = "";
            System.Int32 iRes = 0;

            try
            {
                System.Int32 iFocusedRowHandle = gridViewPlanList.FocusedRowHandle;
                if (DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Подтвердите, пожалуйста, удаление плана продаж.\n\n№: {0}\n\nПериод: {1}-{2}", objPlan.Name, objPlan.BeginDate.ToShortDateString(), objPlan.EndDate.ToShortDateString()), "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.No) { return; }

                if ( CPlanByDepartCustomerSubType.DeletePlan(m_objProfile, objPlan.ID, ref strErr, ref iRes) == true)
                {
                    LoadPlanList();
                    if ((m_objPlanList != null) && (m_objPlanList.Count > 0))
                    {
                        iFocusedRowHandle--;
                        if ((iFocusedRowHandle < 0) || (iFocusedRowHandle > (m_objPlanList.Count - 1)))
                        {
                            iFocusedRowHandle = (m_objPlanList.Count - 1);
                        }
                        gridViewPlanList.FocusedRowHandle = iFocusedRowHandle;
                    }
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Предупреждение",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    SendMessageToLog("Удаление плана. Текст ошибки: " + strErr);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Удаление плана. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void barBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeletePlan(SelectedSalesPlan);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Удаление плана. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Отмена изменений
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cancel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены внесенных изменений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Отмена внесенных изменений
        /// </summary>
        private void Cancel()
        {
            try
            {
                if (m_bIsChanged == true)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Выйти из редактора плана без сохранения изменений?", "Подтверждение",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                }

                tabControl.SelectedTabPage = tabPageViewer;
                if (SelectedSalesPlan != null)
                {
                    if (SelectedSalesPlan.ID.CompareTo(System.Guid.Empty) == 0)
                    {
                        // новый не сохраненный план
                        m_objPlanList.Remove(SelectedSalesPlan);
                        gridControlPlanList.RefreshDataSource();
                    }
                    else
                    {
                        System.Int32 iIndxSelectedObject = m_objPlanList.IndexOf(m_objPlanList.SingleOrDefault<CPlanByDepartCustomerSubType>(x => x.ID.CompareTo(SelectedSalesPlan.ID) == 0));
                        if (iIndxSelectedObject >= 0)
                        {
                            gridViewPlanList.FocusedRowHandle = gridViewPlanList.GetRowHandle(iIndxSelectedObject);
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }

        #endregion

        #region Сохранение изменений в БД
        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        /// <returns>true - удачное завершение операции;false - ошибка</returns>
        private System.Boolean SavePlanInDataBase(ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;
            System.Boolean bOkSave = false;

            Cursor = Cursors.WaitCursor;
            try
            {
                // из элементов управления зачитываются значения свойств расчёта для последующего сохранения в БД
                System.Guid CalcPlanByDepartCustomerSubType_Guid = SelectedSalesPlan.ID;
                
                System.Guid CalcPlan_Guid = ((editorCalcPlan.SelectedItem == null) ? System.Guid.Empty : ((CCalcPlan)editorCalcPlan.SelectedItem).ID);
                System.Guid SalesPlanQuota_Guid = ((editorSalesPlanQuota.SelectedItem == null) ? System.Guid.Empty : ((CSalesPlanQuota)editorSalesPlanQuota.SelectedItem).ID);
                System.Guid Currency_Guid = ((editorCurrency.SelectedItem == null) ? System.Guid.Empty : ((CCurrency)editorCurrency.SelectedItem).ID);
                
                System.String Plan_Name = editorName.Text;
                System.DateTime Plan_BeginDate = editorBeginDate.DateTime;
                System.DateTime Plan_EndDate = editorEndDate.DateTime;
                System.DateTime Plan_Date = editorCreateDate.DateTime;
                System.Boolean Plan_IsUseForReport = editorIsUseForReport.Checked;
                System.String Plan_Description = editorDescription.Text;

                // Приложение к плану
                List<CPlanByDepartCustomerSubTypeItem> PlanByDepartCustomerSubTypeItemList = new List<CPlanByDepartCustomerSubTypeItem>();
                PlanByDepartCustomerSubTypeItemList.AddRange(m_objPlanItemList);

                if ((PlanByDepartCustomerSubTypeItemList != null) && (PlanByDepartCustomerSubTypeItemList.Count > 0))
                {
                    // проверка значений
                    if ( CPlanByDepartCustomerSubType.IsAllParametersValid( Plan_Name, Plan_Date,
                            Plan_BeginDate, Plan_EndDate, CalcPlan_Guid, SalesPlanQuota_Guid, Currency_Guid,
                            PlanByDepartCustomerSubTypeItemList, ref strErr ) == true)
                    {
                        System.Guid objectIDinDB = System.Guid.Empty;

                        bOkSave = CPlanByDepartCustomerSubType.SaveToDB( m_bNewObject, m_objProfile, 
                            CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid, 
                            Plan_Name, Plan_BeginDate, Plan_EndDate, Plan_Date, 
                            Currency_Guid, Plan_IsUseForReport, Plan_Description,
                            PlanByDepartCustomerSubTypeItemList,
                            ref strErr, ref iRes, ref objectIDinDB);

                        if (bOkSave == true)
                        {
                            if (m_bNewObject == true)
                            {
                                SelectedSalesPlan.ID = objectIDinDB;
                            }
                            SelectedSalesPlan.Name = Plan_Name;
                            SelectedSalesPlan.CreateDate = Plan_Date;
                            SelectedSalesPlan.BeginDate = Plan_BeginDate;
                            SelectedSalesPlan.EndDate = Plan_EndDate;
                            SelectedSalesPlan.IsUseForReport = Plan_IsUseForReport;
                            SelectedSalesPlan.Description = Plan_Description;
                            SelectedSalesPlan.ContentsPlan = PlanByDepartCustomerSubTypeItemList;
                            SelectedSalesPlan.CalcPlan = editorCalcPlan.Properties.Items.Cast<CCalcPlan>().Single<CCalcPlan>(x => x.ID.CompareTo(CalcPlan_Guid) == 0);
                            SelectedSalesPlan.SalesPlanQuota = editorSalesPlanQuota.Properties.Items.Cast<CSalesPlanQuota>().Single<CSalesPlanQuota>(x => x.ID.CompareTo(SalesPlanQuota_Guid) == 0);
                            SelectedSalesPlan.Currency = editorCurrency.Properties.Items.Cast<CCurrency>().Single<CCurrency>(x => x.ID.CompareTo(Currency_Guid) == 0);

                            gridControlPlanList.RefreshDataSource();
                        }

                    }
                }
                else
                {
                    strErr = "В приложении к плану отстутствуют данные.\nПожалуйста, произведите расчёт, либо добавьте данные вручную.";
                }


                bRet = bOkSave;
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
                SendMessageToLog("Ошибка сохранения изменений в расчёте. Текст ошибки: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return bRet;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.String strErr = "";
                System.Int32 iRes = -1;

                if (SavePlanInDataBase(ref strErr, ref iRes) == true)
                {
                    SetPropertiesModified(false);
                    SetModeReadOnly(true);
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в плане. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Экспорт в MS Excel
        /// <summary>
        /// Считывает данные из грида и записывает в лист файла MS Excel
        /// </summary>
        /// <param name="objGridView">представление грида</param>
        /// <param name="worksheet">лист файла MS Excel</param>
        private void ReadInfoFromGrid(DevExpress.XtraGrid.Views.Grid.GridView objGridView, ExcelWorksheet worksheet)
        {
            try
            {
                worksheet.Cells[1, 1].Value = objGridView.Columns["ProductOwnerIbID"].Caption;
                worksheet.Cells[1, 2].Value = objGridView.Columns["ProductOwnerName"].Caption;
                worksheet.Cells[1, 3].Value = objGridView.Columns["ProductTypeIbID"].Caption;
                worksheet.Cells[1, 4].Value = objGridView.Columns["ProductTypeName"].Caption;
                worksheet.Cells[1, 5].Value = objGridView.Columns["DepartTeamName"].Caption;
                worksheet.Cells[1, 6].Value = objGridView.Columns["DepartCode"].Caption;
                worksheet.Cells[1, 7].Value = objGridView.Columns["CustomerIbID"].Caption;
                worksheet.Cells[1, 8].Value = objGridView.Columns["CustomerName"].Caption;
                worksheet.Cells[1, 9].Value = objGridView.Columns["ProductSubTypeIbID"].Caption;
                worksheet.Cells[1, 10].Value = objGridView.Columns["ProductSubTypeName"].Caption;
                worksheet.Cells[1, 11].Value = objGridView.Columns["Plan_Quantity"].Caption;
                worksheet.Cells[1, 12].Value = objGridView.Columns["Plan_AllPrice"].Caption;

                using (var range = worksheet.Cells[1, 1, 1, 12])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 14;
                }

                System.Int32 iCurrentRow = 2;
                System.Int32 iRowsCount = objGridView.RowCount;
                for (System.Int32 i = 0; i < iRowsCount; i++)
                {
                    worksheet.Cells[iCurrentRow, 1].Value = objGridView.GetRowCellValue(i, objGridView.Columns["ProductOwnerIbID"]);
                    worksheet.Cells[iCurrentRow, 2].Value = objGridView.GetRowCellValue(i, objGridView.Columns["ProductOwnerName"]);
                    worksheet.Cells[iCurrentRow, 3].Value = objGridView.GetRowCellValue(i, objGridView.Columns["ProductTypeIbID"]);
                    worksheet.Cells[iCurrentRow, 4].Value = objGridView.GetRowCellValue(i, objGridView.Columns["ProductTypeName"]);
                    worksheet.Cells[iCurrentRow, 5].Value = objGridView.GetRowCellValue(i, objGridView.Columns["DepartTeamName"]);
                    worksheet.Cells[iCurrentRow, 6].Value = objGridView.GetRowCellValue(i, objGridView.Columns["DepartCode"]);
                    worksheet.Cells[iCurrentRow, 7].Value = objGridView.GetRowCellValue(i, objGridView.Columns["CustomerIbID"]);
                    worksheet.Cells[iCurrentRow, 8].Value = objGridView.GetRowCellValue(i, objGridView.Columns["CustomerName"]);
                    worksheet.Cells[iCurrentRow, 9].Value = objGridView.GetRowCellValue(i, objGridView.Columns["ProductSubTypeIbID"]);
                    worksheet.Cells[iCurrentRow, 10].Value = objGridView.GetRowCellValue(i, objGridView.Columns["ProductSubTypeName"]);
                    worksheet.Cells[iCurrentRow, 11].Value = objGridView.GetRowCellValue(i, objGridView.Columns["Plan_Quantity"]);
                    worksheet.Cells[iCurrentRow, 12].Value = objGridView.GetRowCellValue(i, objGridView.Columns["Plan_AllPrice"]);

                    worksheet.Cells[iCurrentRow, 11].Style.Numberformat.Format = "### ### ##0";
                    worksheet.Cells[iCurrentRow, 12].Style.Numberformat.Format = "### ### ##0.00";
                    
                    iCurrentRow++;
                }

                iCurrentRow--;
                worksheet.Cells[1, 1, iCurrentRow, objGridView.Columns.Count].AutoFitColumns(0);
                worksheet.Cells[1, 1, iCurrentRow, objGridView.Columns.Count].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, iCurrentRow, objGridView.Columns.Count].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, iCurrentRow, objGridView.Columns.Count].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, iCurrentRow, objGridView.Columns.Count].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.PrinterSettings.FitToWidth = 1;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "ReadInfoFromGrid.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// экспорт в файл MS Excel содержимого приложения к плану
        /// </summary>
        /// <param name="strFileName">путь к файлу</param>
        private void ExportToExcelSalesPlanItemList(string strFileName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.IO.FileInfo newFile = new System.IO.FileInfo(strFileName);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new System.IO.FileInfo(strFileName);
                }

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = worksheet = package.Workbook.Worksheets.Add("План продаж");
                    ReadInfoFromGrid(gridViewPlanItem, worksheet);

                    worksheet = null;

                    package.Save();

                    try
                    {
                        using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                        {
                            process.StartInfo.FileName = strFileName;
                            process.StartInfo.Verb = "Open";
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                            process.Start();
                        }
                    }
                    catch
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(this, "Системе не удалось найти приложение, чтобы открыть файл.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

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
        }
        private void btnPrintPlanItemList_Click(object sender, EventArgs e)
        {
            try
            {
                ExportToExcelSalesPlanItemList(String.Format("{0}{1}.xlsx", System.IO.Path.GetTempPath(), "Расчёт"));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnPrintPlanItemList_Click. Текст ошибки: " + f.Message);
            }

            return;
        }
        /// <summary>
        /// Выгружает в файл MS Excel список планов
        /// </summary>
        /// <param name="strFileName">полный путь к файлу (каталог + имя)</param>
        private void ExportToExcelPlanList(string strFileName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.IO.FileInfo newFile = new System.IO.FileInfo(strFileName);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new System.IO.FileInfo(strFileName);
                }

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(this.Text);

                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewPlanList.Columns)
                    {
                        if (objColumn.Visible == false) { continue; }

                        worksheet.Cells[1, objColumn.VisibleIndex + 1].Value = objColumn.Caption;
                    }

                    using (var range = worksheet.Cells[1, 1, 1, gridViewPlanList.Columns.Count])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Size = 14;
                    }

                    System.Int32 iCurrentRow = 2;
                    System.Int32 iRowsCount = gridViewPlanList.RowCount;
                    for (System.Int32 i = 0; i < iRowsCount; i++)
                    {
                        foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewPlanList.Columns)
                        {
                            if (objColumn.Visible == false) { continue; }

                            worksheet.Cells[iCurrentRow, objColumn.VisibleIndex + 1].Value = gridViewPlanList.GetRowCellValue(i, objColumn);
                            if (objColumn.FieldName == "Date")
                            {
                                worksheet.Cells[iCurrentRow, objColumn.VisibleIndex + 1].Style.Numberformat.Format = "DD.MM.YYYY";
                            }
                        }
                        iCurrentRow++;
                    }

                    iCurrentRow--;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewPlanList.Columns.Count].AutoFitColumns(0);
                    worksheet.Cells[1, 1, iCurrentRow, gridViewPlanList.Columns.Count].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewPlanList.Columns.Count].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewPlanList.Columns.Count].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewPlanList.Columns.Count].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    worksheet.PrinterSettings.FitToWidth = 1;

                    worksheet = null;

                    package.Save();

                    try
                    {
                        using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                        {
                            process.StartInfo.FileName = strFileName;
                            process.StartInfo.Verb = "Open";
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                            process.Start();
                        }
                    }
                    catch
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(this, "Системе не удалось найти приложение, чтобы открыть файл.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

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
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ExportToExcelPlanList(String.Format("{0}{1}.xlsx", System.IO.Path.GetTempPath(), this.Text));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnPrint_Click. Текст ошибки: " + f.Message);
            }

            return;
        }
        #endregion

        #region Расчёт плана продаж
        private void btnStartCalc_Click(object sender, EventArgs e)
        {
            StartCalcPlan();
        }
        /// <summary>
        /// Стартует поток, в котором производится расчёт
        /// </summary>
        public void StartCalcPlan()
        {
            try
            {
                if ((editorBeginDate.EditValue == null) || ( editorEndDate.EditValue == null) ||
                    (editorBeginDate.DateTime.CompareTo(editorEndDate.DateTime) > 0))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Для расчёта необходимо задать корректный период дат.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                if ((editorCalcPlan.SelectedItem == null) || (editorSalesPlanQuota.SelectedItem == null))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Необходимо указать план продаж по маркам и группам, \nа также вариант расчёта долей продаж.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                // инициализируем делегаты
                m_LoadCalcPlanDelegate = new LoadCalcPlanDelegate(LoadPlanItemListInGrids);
                m_objPlanItemList.Clear();

                btnStartCalc.Enabled = false;
                editorCalcPlan.Enabled = false;
                editorSalesPlanQuota.Enabled = false;
                editorCurrency.Enabled = false;
                editorBeginDate.Enabled = false;
                editorEndDate.Enabled = false;

                btnCancel.Enabled = false;
                btnSave.Enabled = false;

                gridControlPlanItem.DataSource = null;
                gridControlPlanItem.ContextMenuStrip = null;

                SearchProcessWoring.Visible = true;
                SearchProcessWoring.Refresh();

                btnPrintPlanItemList.Enabled = false;

                // запуск потока
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
                ci.NumberFormat.CurrencyDecimalSeparator = ".";
                ci.NumberFormat.NumberDecimalSeparator = ".";
                this.ThreadCalcPlan = new System.Threading.Thread(CalcPlanInThread);
                this.ThreadCalcPlan.CurrentCulture = ci;
                this.ThreadCalcPlan.Start();
                Thread.Sleep(1000);

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartCalcPlan().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        /// <summary>
        /// Вызов расчёта плана продаж и передача данных в гриды
        /// </summary>
        public void CalcPlanInThread()
        {
            try
            {


                System.String strErr = "";
                System.Int32 iRes = -1;
                System.Guid CalcPlan_Guid = ((editorCalcPlan.SelectedItem == null) ? System.Guid.Empty : ((CCalcPlan)editorCalcPlan.SelectedItem).ID);
                System.Guid SalesPlanQuota_Guid = ((editorSalesPlanQuota.SelectedItem == null) ? System.Guid.Empty : ((CSalesPlanQuota)editorSalesPlanQuota.SelectedItem).ID);
                System.Int32 iMonth = editorBeginDate.DateTime.Month;
                System.DateTime Plan_EndDate = editorEndDate.DateTime;

                List<CPlanByDepartCustomerSubTypeItem> objPlanItemList = CPlanByDepartCustomerSubType.CalculationPlan(m_objProfile,
                    SalesPlanQuota_Guid, CalcPlan_Guid, iMonth, ref strErr, ref iRes);

                if ((objPlanItemList != null) && (objPlanItemList.Count > 0))
                {
                    List<CPlanByDepartCustomerSubTypeItem> objAddPlanItemList = new List<CPlanByDepartCustomerSubTypeItem>();

                    if ((objAddPlanItemList != null) && (objPlanItemList.Count > 0))
                    {
                        System.Int32 iRecCount = 0;
                        System.Int32 iRecAllCount = 0;

                        foreach (CPlanByDepartCustomerSubTypeItem objPlanItem in objPlanItemList)
                        {
                            objAddPlanItemList.Add(objPlanItem);
                            iRecCount++;
                            iRecAllCount++;

                            if (iRecCount == 1000)
                            {
                                iRecCount = 0;
                                Thread.Sleep(1000);
                                this.Invoke(m_LoadCalcPlanDelegate, new Object[] { objAddPlanItemList, iRecAllCount });
                                objAddPlanItemList.Clear();
                            }

                        }
                        if (iRecCount != 1000)
                        {
                            iRecCount = 0;
                            this.Invoke(m_LoadCalcPlanDelegate, new Object[] { objAddPlanItemList, iRecAllCount });
                            objAddPlanItemList.Clear();
                        }

                    }
                }

                this.Invoke(m_LoadCalcPlanDelegate, new Object[] { null, 0 });
                this.m_bThreadFinishJob = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("CalcPlanInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// загружает в журнал приложение к плану
        /// </summary>
        /// <param name="objPlanItemList">приложение к плану</param>
        /// <param name="iRowCountInList">количество строк, которые требуется загрузить в журнал</param>
        private void LoadPlanItemListInGrids(List<CPlanByDepartCustomerSubTypeItem> objPlanItemList, System.Int32 iRowCountInList)
        {
            try
            {
                if ((objPlanItemList != null) && (objPlanItemList.Count > 0) &&
                    (gridViewPlanItem.RowCount < iRowCountInList))
                {
                    m_objPlanItemList.AddRange(objPlanItemList);

                    if( gridControlPlanItem.DataSource == null)
                    {
                        gridControlPlanItem.DataSource = m_objPlanItemList;
                    }
                    gridControlPlanItem.RefreshDataSource();

                }
                else
                {
                    Thread.Sleep(1000);
                    SearchProcessWoring.Visible = false;

                    btnStartCalc.Enabled = true;
                    btnPrintPlanItemList.Enabled = true;

                    btnStartCalc.Enabled = true;
                    editorCalcPlan.Enabled = true;
                    editorSalesPlanQuota.Enabled = true;
                    editorCurrency.Enabled = true;
                    editorBeginDate.Enabled = true;
                    editorEndDate.Enabled = true;

                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;

                    gridControlPlanItem.RefreshDataSource();
                    gridControlPlanItem.ContextMenuStrip = contextMenuSalePlanQuotaItem;

                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewPlanItem.Columns)
                    {
                        if (objColumn.Visible == true)
                        {
                            objColumn.BestFit();
                        }
                    }

                    Cursor = Cursors.Default;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadPlanItemListInGrids.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Импорт плана продаж
        /// <summary>
        /// импорт данных в приложение к плану продаж
        /// </summary>
        private void ImportPlanItemList()
        {
            try
            {
                if (PlanImportEditor != null)
                {
                    PlanImportEditor.OpenForImportDataInPlan(m_objPlanItemList, m_strXLSImportFilePath, m_iXLSSheetImport, m_SheetList);
                  
                    //PlanImportEditor.ShowDialog();

                    DialogResult dlgRes = PlanItemEditor.DialogResult;

                    m_strXLSImportFilePath = PlanImportEditor.FileFullName;
                    m_iXLSSheetImport = PlanImportEditor.SelectedSheetId;
                    m_SheetList = PlanImportEditor.SheetList;

                    gridControlPlanItem.RefreshDataSource();

                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("ImportPlanItemList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void mitemsImport_Click(object sender, EventArgs e)
        {
            ImportPlanItemList();
        }
        #endregion

    }

    public class PlanByDepartCustomerSubtypeEditor : PlugIn.IClassTypeView
    {
        public override void Run(UniXP.Common.MENUITEM objMenuItem, System.String strCaption)
        {
            frmPlanByDepartCustomerSubtype obj = new frmPlanByDepartCustomerSubtype(objMenuItem) { Text = strCaption, MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent, Visible = true };
        }
    }

}
