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
    public partial class frmSalesPlanQuota : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private System.Boolean m_bOnlyView;
        private System.Boolean m_bIsChanged;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewObject;

        private List<CSalesPlanQuota> m_objSalesPlanQuotaList;
        private List<CSalesPlanQuotaItemForGrid> m_objQuotaItemDepartTeamForGridList;
        private List<CSalesPlanQuotaItemForGrid> m_objQuotaItemDepartForGridList;
        private List<CSalesPlanQuotaItemForGrid> m_objQuotaItemCustomerForGridList;
        private List<CSalesPlanQuotaItemForGrid> m_objQuotaItemProductSubTypeForGridList;

        private List<CProductTradeMark> m_objProductTradeMarkList;
        private List<CProductType> m_objProductTypeList;
        private List<CDepartTeam> m_objDepartTeamList;
        private List<CDepart> m_objDepartList;
        private List<CProductSubType> m_objProductSubTypeList;
        private List<CCustomer> m_objCustomerList;
        private frmSalesPlanQuotaItemEditor SalesPlanQuotaItemEditor;
        private frmTradeMarkQuota TradeMarkQuotaEditor;
        private frmTradeMarkDepartTeamQuota TradeMarkDepartTeamQuota;

        private CSalesPlanQuota SelectedSalesPlanQuota
        {
            get
            {
                CSalesPlanQuota objRet = null;
                try
                {
                    if ((gridViewSalesPlanQuotaList.RowCount > 0) && (gridViewSalesPlanQuotaList.FocusedRowHandle >= 0) && (m_objSalesPlanQuotaList != null))
                    {
                        System.Guid uuidID = (System.Guid)(gridViewSalesPlanQuotaList.GetFocusedRowCellValue("ID"));

                        objRet = m_objSalesPlanQuotaList.Single<CSalesPlanQuota>(x => x.ID.CompareTo(uuidID) == 0);
                    }
                }//try
                catch (System.Exception f)
                {
                    SendMessageToLog("Ошибка поиска выбранного расчёта. Текст ошибки: " + f.Message);
                }
                finally
                {
                }

                return objRet;
            }
        }

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlSalesPlanQuotaList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewDecodeDepart
        {
            get { return gridControlSalePlanQuotaItemDecodeDepart.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewDecodeDepartTeam
        {
            get { return gridControlSalePlanQuotaItemDecodeDepartTeam.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewDecodeCustomer
        {
            get { return gridControlSalePlanQuotaItemDecodeCustomer.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }

        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewDecodePartSubType
        {
            get { return gridControlSalePlanQuotaItemDecodePartSubType.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }

        // потоки
        public System.Threading.Thread ThreadCalcSalesPlanQuota { get; set; }

        public System.Threading.ManualResetEvent EventStopThread { get; set; }
        public System.Threading.ManualResetEvent EventThreadStopped { get; set; }

        public delegate void LoadCalcSalesPlanQuotaDelegate(List<CSalesPlanQuotaItemForGrid> objSalesPlanQuotaItemList, System.Int32 iRowCountInLis);
        public LoadCalcSalesPlanQuotaDelegate m_LoadCalcSalesPlanQuotaDelegate;

        public delegate void LoadComboBoxForDecodeEditorDelegate(List<CDepartTeam> objDepartTeamList, 
           List<CDepart> objDepartList, List<CCustomer> objCustomerList,
           List<CProductSubType> objProductSubTypeList, List<CProductTradeMark> objProductTradeMarkList,
           List<CProductType> objProductTypeList);
        public LoadComboBoxForDecodeEditorDelegate m_LoadComboBoxForDecodeEditorDelegate;
        public System.Threading.Thread ThreadComboBoxForDecodeEditor { get; set; }

        private const System.Int32 iThreadSleepTime = 1000;
        private const System.String strWaitCustomer = "ждите... идет выполнение расчёта";
        private System.Boolean m_bThreadFinishJob;
        private const System.String strRegistryTools = "\\SalesPlanQuotaListTools\\";
        private const System.Int32 iWaitingpanelIndex = 0;
        private const System.Int32 iWaitingpanelHeight = 35;
        private const System.String m_strModeReadOnly = "Режим просмотра";
        private const System.String m_strModeEdit = "Режим редактирования";


        #endregion

        #region Конструктор
        public frmSalesPlanQuota(UniXP.Common.MENUITEM objMenuItem)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            InitializeComponent();

            m_objMenuItem = objMenuItem;
            m_objProfile = objMenuItem.objProfile;
            m_bThreadFinishJob = false;
            m_objSalesPlanQuotaList = new List<CSalesPlanQuota>();
            m_objQuotaItemDepartTeamForGridList = null;
            m_objQuotaItemDepartForGridList = null;
            m_objQuotaItemCustomerForGridList = null;
            m_objQuotaItemProductSubTypeForGridList = null;

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
            tabControlEditor.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            m_bOnlyView = false;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewObject = false;

            SearchProcessWoring.Visible = false;

            SalesPlanQuotaItemEditor = new frmSalesPlanQuotaItemEditor(m_objProfile);
            TradeMarkQuotaEditor = new frmTradeMarkQuota(m_objProfile);
            TradeMarkDepartTeamQuota = new frmTradeMarkDepartTeamQuota(m_objProfile);
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
            AddGridColumn(ColumnView, "Date", "Дата");
            AddGridColumn(ColumnView, "CalcPeriod", "Период для расчёта");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                if( objColumn.FieldName == "ID" )
                {
                    objColumn.Visible = false;
                }
            }

            // доля продаж в разрезе команд
            ColumnViewDecodeDepartTeam.Columns.Clear();

            AddGridColumn(ColumnViewDecodeDepartTeam, "ID", "Идентификатор");
            AddGridColumn(ColumnViewDecodeDepartTeam, "ProductTradeMarkID", "УИ ТМ");
            AddGridColumn(ColumnViewDecodeDepartTeam, "ProductTypeID", "УИ ТГр");
            AddGridColumn(ColumnViewDecodeDepartTeam, "ProductTradeMarkName", "Товарная марка");
            AddGridColumn(ColumnViewDecodeDepartTeam, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnViewDecodeDepartTeam, "SalesQuantity", "К-во");
            AddGridColumn(ColumnViewDecodeDepartTeam, "SalesMoney", "Сумма");
            AddGridColumn(ColumnViewDecodeDepartTeam, "Object_ID", "Идентификатор");
            AddGridColumn(ColumnViewDecodeDepartTeam, "Object_Name", "Команда");
            AddGridColumn(ColumnViewDecodeDepartTeam, "Object_SalesQuantity", "К-во");
            AddGridColumn(ColumnViewDecodeDepartTeam, "Object_SalesMoney", "Сумма");
            AddGridColumn(ColumnViewDecodeDepartTeam, "Object_CalcQuota", "Доля (расчёт)");
            AddGridColumn(ColumnViewDecodeDepartTeam, "Object_Quota", "Доля");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewDecodeDepartTeam.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;

                if ((objColumn.FieldName == "ID") || (objColumn.FieldName == "Object_ID") ||
                    (objColumn.FieldName == "ProductTradeMarkID") || (objColumn.FieldName == "ProductTypeID"))
                {
                    objColumn.Visible = false;
                }

                if ((objColumn.FieldName == "SalesQuantity") || (objColumn.FieldName == "SalesMoney") )
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0";

                    if ((objColumn.FieldName == "Object_SalesQuantity") || (objColumn.FieldName == "Object_SalesMoney"))
                    {
                        objColumn.SummaryItem.FieldName = objColumn.FieldName;
                        objColumn.SummaryItem.DisplayFormat = "Итого: {0:### ### ##0}";
                        objColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    }
                }

                if ((objColumn.FieldName == "Object_CalcQuota") || (objColumn.FieldName == "Object_Quota"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0.0000";
                }
            }

            // доля продаж в разрезе подразделений
            ColumnViewDecodeDepart.Columns.Clear();

            AddGridColumn(ColumnViewDecodeDepart, "ID", "Идентификатор");
            AddGridColumn(ColumnViewDecodeDepart, "ProductTradeMarkID", "УИ ТМ");
            AddGridColumn(ColumnViewDecodeDepart, "ProductTypeID", "УИ ТГр");
            AddGridColumn(ColumnViewDecodeDepart, "ProductTradeMarkName", "Товарная марка");
            AddGridColumn(ColumnViewDecodeDepart, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnViewDecodeDepart, "SalesQuantity", "К-во");
            AddGridColumn(ColumnViewDecodeDepart, "SalesMoney", "Сумма");
            AddGridColumn(ColumnViewDecodeDepart, "Object_ID", "Идентификатор");
            AddGridColumn(ColumnViewDecodeDepart, "Object_Name", "Подр-е");
            AddGridColumn(ColumnViewDecodeDepart, "Object_SalesQuantity", "К-во");
            AddGridColumn(ColumnViewDecodeDepart, "Object_SalesMoney", "Сумма");
            AddGridColumn(ColumnViewDecodeDepart, "Object_CalcQuota", "Доля (расчёт)");
            AddGridColumn(ColumnViewDecodeDepart, "Object_Quota", "Доля");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewDecodeDepart.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;

                if ((objColumn.FieldName == "ID") || (objColumn.FieldName == "Object_ID") ||
                    (objColumn.FieldName == "ProductTradeMarkID") || (objColumn.FieldName == "ProductTypeID"))
                {
                    objColumn.Visible = false;
                }

                if ((objColumn.FieldName == "Object_SalesQuantity") || (objColumn.FieldName == "Object_SalesMoney"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0";

                    objColumn.SummaryItem.FieldName = objColumn.FieldName;
                    objColumn.SummaryItem.DisplayFormat = "Итого: {0:### ### ##0}";
                    objColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                }

                if ((objColumn.FieldName == "Object_CalcQuota") || (objColumn.FieldName == "Object_Quota"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0.0000";

                    objColumn.SummaryItem.FieldName = objColumn.FieldName;
                    objColumn.SummaryItem.DisplayFormat = "Итого: {0:### ### ##0.0000}";
                    objColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                }
            }

            // доля продаж в разрезе клиентов
            ColumnViewDecodeCustomer.Columns.Clear();

            AddGridColumn(ColumnViewDecodeCustomer, "ID", "Идентификатор");
            AddGridColumn(ColumnViewDecodeCustomer, "ProductTradeMarkID", "УИ ТМ");
            AddGridColumn(ColumnViewDecodeCustomer, "ProductTypeID", "УИ ТГр");
            AddGridColumn(ColumnViewDecodeCustomer, "ProductTradeMarkName", "Товарная марка");
            AddGridColumn(ColumnViewDecodeCustomer, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnViewDecodeCustomer, "SalesQuantity", "К-во");
            AddGridColumn(ColumnViewDecodeCustomer, "SalesMoney", "Сумма");
            AddGridColumn(ColumnViewDecodeCustomer, "Object_ID", "Идентификатор");
            AddGridColumn(ColumnViewDecodeCustomer, "Object_Name", "Клиент");
            AddGridColumn(ColumnViewDecodeCustomer, "Object_SalesQuantity", "К-во");
            AddGridColumn(ColumnViewDecodeCustomer, "Object_SalesMoney", "Сумма");
            AddGridColumn(ColumnViewDecodeCustomer, "Object_CalcQuota", "Доля (расчёт)");
            AddGridColumn(ColumnViewDecodeCustomer, "Object_Quota", "Доля");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewDecodeCustomer.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;

                if ((objColumn.FieldName == "ID") || (objColumn.FieldName == "Object_ID") ||
                    (objColumn.FieldName == "ProductTradeMarkID") || (objColumn.FieldName == "ProductTypeID"))
                {
                    objColumn.Visible = false;
                }

                if ((objColumn.FieldName == "SalesQuantity") || (objColumn.FieldName == "SalesMoney"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0";

                    if ((objColumn.FieldName == "Object_SalesQuantity") || (objColumn.FieldName == "Object_SalesMoney"))
                    {
                        objColumn.SummaryItem.FieldName = objColumn.FieldName;
                        objColumn.SummaryItem.DisplayFormat = "Итого: {0:### ### ##0}";
                        objColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    }
                }

                if ((objColumn.FieldName == "Object_CalcQuota") || (objColumn.FieldName == "Object_Quota"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0.0000";
                }
            }

            // доля продаж в разрезе товарных подгрупп
            ColumnViewDecodePartSubType.Columns.Clear();

            AddGridColumn(ColumnViewDecodePartSubType, "ID", "Идентификатор");
            AddGridColumn(ColumnViewDecodePartSubType, "ProductTradeMarkID", "УИ ТМ");
            AddGridColumn(ColumnViewDecodePartSubType, "ProductTypeID", "УИ ТГр");
            AddGridColumn(ColumnViewDecodePartSubType, "ProductTradeMarkName", "Товарная марка");
            AddGridColumn(ColumnViewDecodePartSubType, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnViewDecodePartSubType, "SalesQuantity", "К-во");
            AddGridColumn(ColumnViewDecodePartSubType, "SalesMoney", "Сумма");
            AddGridColumn(ColumnViewDecodePartSubType, "Object_ID", "Идентификатор");
            AddGridColumn(ColumnViewDecodePartSubType, "Object_Name", "Товарная подгруппа");
            AddGridColumn(ColumnViewDecodePartSubType, "Object_SalesQuantity", "К-во");
            AddGridColumn(ColumnViewDecodePartSubType, "Object_SalesMoney", "Сумма");
            AddGridColumn(ColumnViewDecodePartSubType, "Object_CalcQuota", "Доля (расчёт)");
            AddGridColumn(ColumnViewDecodePartSubType, "Object_Quota", "Доля");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewDecodePartSubType.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;

                if ((objColumn.FieldName == "ID") || (objColumn.FieldName == "Object_ID") ||
                    (objColumn.FieldName == "ProductTradeMarkID") || (objColumn.FieldName == "ProductTypeID"))
                {
                    objColumn.Visible = false;
                }

                if ((objColumn.FieldName == "SalesQuantity") || (objColumn.FieldName == "SalesMoney"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0";

                    if ((objColumn.FieldName == "Object_SalesQuantity") || (objColumn.FieldName == "Object_SalesMoney"))
                    {
                        objColumn.SummaryItem.FieldName = objColumn.FieldName;
                        objColumn.SummaryItem.DisplayFormat = "Итого: {0:### ### ##0}";
                        objColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    }
                }

                if ((objColumn.FieldName == "Object_CalcQuota") || (objColumn.FieldName == "Object_Quota"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0.0000";
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
                gridViewSalesPlanQuotaList.RestoreLayoutFromRegistry(strReestrPath + gridViewSalesPlanQuotaList.Name);
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
                gridViewSalesPlanQuotaList.SaveLayoutToRegistry(strReestrPath + gridViewSalesPlanQuotaList.Name);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка записи настроек журнала расчётов.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        #endregion

        #endregion

        #region Список товарных марок и групп
        /// <summary>
        /// загружает список товарных марок
        /// </summary>
        private void LoadProductTrademarkList()
        {
            try
            {
                editorSalesPlanQuotaConditionProductOwner.Items.Clear();

                List<CProductTradeMark> objProductTradeMarkList = CProductTradeMark.GetProductTradeMarkList(m_objProfile, null);
                if ((objProductTradeMarkList != null) && (objProductTradeMarkList.Count > 0))
                {
                    foreach (CProductTradeMark objItem in objProductTradeMarkList) 
                    {
                        editorSalesPlanQuotaConditionProductOwner.Items.Add(objItem, false );
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadProductTrademarkList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// загружает список товарных групп
        /// </summary>
        private void LoadProductPartTypeList()
        {
            try
            {
                editorSalesPlanQuotaConditionProductGroup.Items.Clear();

                List<CProductType> objProductTypeList = CProductType.GetProductTypeList(m_objProfile, null);
                if ((objProductTypeList != null) && (objProductTypeList.Count > 0))
                {
                    foreach (CProductType objItem in objProductTypeList)
                    {
                        editorSalesPlanQuotaConditionProductGroup.Items.Add(objItem, false);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadProductPartTypeList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Редактор приложения к расчёту
        /// <summary>
        /// Загружает выпадающие списки в редактор приложения к расчёту
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
                    new Object[] { null, null, null, null, null, null});
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
                    if (SalesPlanQuotaItemEditor != null)
                    {
                        SalesPlanQuotaItemEditor.LoadComboBox(objDepartTeamList, objDepartList,
                            objCustomerList, objProductSubTypeList, objProductTradeMarkList, objProductTypeList);
                    }

                    if (TradeMarkQuotaEditor != null)
                    {
                        TradeMarkQuotaEditor.LoadComboBox(objProductTradeMarkList);
                    }
                    if (TradeMarkDepartTeamQuota != null)
                    {
                        TradeMarkDepartTeamQuota.LoadComboBox(objProductTradeMarkList, objDepartList);
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
            NewSalesPlanQuotaItemForGrid();
        }

        /// <summary>
        /// Добавление нового элемента в приложение к расчёту
        /// </summary>
        private void NewSalesPlanQuotaItemForGrid()
        {
            try
            {
                enQuotaObjectType enumQuotaObjectType = enQuotaObjectType.Unkown;

                if (tabControlSalePlanQuotaItem.SelectedTabPage == tabPageDepartTeam)
                {
                    enumQuotaObjectType = enQuotaObjectType.DepartTeam;
                }
                if (tabControlSalePlanQuotaItem.SelectedTabPage == tabPageDepart)
                {
                    enumQuotaObjectType = enQuotaObjectType.Depart;
                }
                if (tabControlSalePlanQuotaItem.SelectedTabPage == tabPageCustomer)
                {
                    enumQuotaObjectType = enQuotaObjectType.Customer;
                }
                if (tabControlSalePlanQuotaItem.SelectedTabPage == tabPageProductSubType)
                {
                    enumQuotaObjectType = enQuotaObjectType.ProductSubType;
                }

                if (SalesPlanQuotaItemEditor != null)
                {
                    SalesPlanQuotaItemEditor.AddSalesPlanQuotaItemForGrid(enumQuotaObjectType);
                    SalesPlanQuotaItemEditor.ShowDialog();

                    DialogResult dlgRes = SalesPlanQuotaItemEditor.DialogResult;

                    if (dlgRes == System.Windows.Forms.DialogResult.OK)
                    {
                        switch (enumQuotaObjectType)
                        {
                            case enQuotaObjectType.DepartTeam:
                                AddSalesPlanQuotaItemForGridToList(SalesPlanQuotaItemEditor.SalesPlanQuotaItemForGrid, 
                                    m_objQuotaItemDepartTeamForGridList);
                                gridControlSalePlanQuotaItemDecodeDepartTeam.RefreshDataSource();
                                break;
                            case enQuotaObjectType.Depart:
                                AddSalesPlanQuotaItemForGridToList(SalesPlanQuotaItemEditor.SalesPlanQuotaItemForGrid,
                                    m_objQuotaItemDepartForGridList);
                                gridControlSalePlanQuotaItemDecodeDepart.RefreshDataSource();
                                break;
                            case enQuotaObjectType.Customer:
                                AddSalesPlanQuotaItemForGridToList(SalesPlanQuotaItemEditor.SalesPlanQuotaItemForGrid,
                                    m_objQuotaItemCustomerForGridList);
                                gridControlSalePlanQuotaItemDecodeCustomer.RefreshDataSource();
                                break;
                            case enQuotaObjectType.ProductSubType:
                                AddSalesPlanQuotaItemForGridToList(SalesPlanQuotaItemEditor.SalesPlanQuotaItemForGrid,
                                    m_objQuotaItemProductSubTypeForGridList);
                                gridControlSalePlanQuotaItemDecodePartSubType.RefreshDataSource();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("NewSalesPlanQuotaItemForGrid.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Добавление нового элемента в приложение к расчёту
        /// </summary>
        /// <param name="objAdded">новый элемент</param>
        /// <param name="gridSrcList">приложение к расчёту</param>
        private void AddSalesPlanQuotaItemForGridToList(CSalesPlanQuotaItemForGrid objAdded, 
            List<CSalesPlanQuotaItemForGrid> gridSrcList )
        {
            try
            {
                if (objAdded.Object_Quota < 0) { objAdded.Object_Quota = 0; }
                if (objAdded.Object_Quota > 1) { objAdded.Object_Quota = 1; }
                
                // проверка на то, имеется ли в списке точно такой же элемент
                CSalesPlanQuotaItemForGrid objTwin = gridSrcList.SingleOrDefault<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(objAdded.ProductTradeMark.ID) == 0) &&
                    (x.ProductTypeID.CompareTo(objAdded.ProductType.ID) == 0) &&
                    (x.Object_ID.CompareTo(objAdded.Object_ID) == 0));

                if (objTwin != null)
                {
                    // редактируется только доля продаж
                    objTwin.Object_Quota = objAdded.Object_Quota;
                }
                else
                {
                    gridSrcList.Add(objAdded);
                }

                // пересчёт долей элементов группы
                RecalcQuotaInGroupSalePlanQuotaItem(objAdded, gridSrcList);
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
        /// <summary>
        /// Пересчёт долей продаж элементов группы
        /// </summary>
        /// <param name="objMain">элемент, у которого была изменена доля продаж</param>
        /// <param name="objList">список со всеми элементами</param>
        private void RecalcQuotaInGroupSalePlanQuotaItem(CSalesPlanQuotaItemForGrid objMain,
            List<CSalesPlanQuotaItemForGrid> objList)
        {
            try
            {
                // поиск "соседей" - элементов с той же маркой и группой
                List<CSalesPlanQuotaItemForGrid> objNeighbors = objList.Where<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(objMain.ProductTradeMark.ID) == 0) &&
                (x.ProductTypeID.CompareTo(objMain.ProductType.ID) == 0) &&
                (x.Object_ID.CompareTo(objMain.Object_ID) != 0)).ToList<CSalesPlanQuotaItemForGrid>();

                if( (objNeighbors != null) && ( objNeighbors.Count > 0 ))
                {
                    // элементы той же группы, что и objMain, за исключением objMain
                    System.Decimal dcmlNeighborsCurrentQuota = objNeighbors.Sum(x => x.Object_Quota);
                    System.Decimal dcmlNeighborsNewQuota = (1 - objMain.Object_Quota);
                    System.Decimal dcmlDiffQuota = (dcmlNeighborsNewQuota - dcmlNeighborsCurrentQuota );
                    
                    // корректировка доли элементов
                    foreach (CSalesPlanQuotaItemForGrid objItem in objNeighbors)
                    {
                        if ((objItem.Object_Quota != 0) && ((objItem.Object_Quota * (1 + dcmlDiffQuota)) >0))
                        {
                            objItem.Object_Quota = (objItem.Object_Quota * (1 + dcmlDiffQuota));
                        }
                    }

                    // корректировка погрешности округдения
                    System.Decimal dcmlError = (dcmlNeighborsNewQuota - objNeighbors.Sum(x => x.Object_Quota));
                    foreach (CSalesPlanQuotaItemForGrid objItem in objNeighbors)
                    {
                        if ((objItem.Object_Quota != 0) && ((objItem.Object_Quota + dcmlError) > 0))
                        {
                            objItem.Object_Quota = (objItem.Object_Quota + dcmlError);
                            break;
                        }
                    }

                }

                if (objMain.Object_QuotaObjectType == enQuotaObjectType.DepartTeam)
                {
                    // в случае исправления доли продаж у команды, необходимо пересчитать доли продаж у подразделений
                    
                    // список команда с долями продаж для заданной марки и группы
                    List<CSalesPlanQuotaItemForGrid> objDepartTeamList = objList.Where<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(objMain.ProductTradeMark.ID) == 0) &&
                                    (x.ProductTypeID.CompareTo(objMain.ProductType.ID) == 0)).ToList<CSalesPlanQuotaItemForGrid>();

                    // список подразделений для заданной марки и группы
                    List<CSalesPlanQuotaItemForGrid> objDepartListSrc = m_objQuotaItemDepartForGridList.Where<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(objMain.ProductTradeMark.ID) == 0) &&
                                    (x.ProductTypeID.CompareTo(objMain.ProductType.ID) == 0)).ToList<CSalesPlanQuotaItemForGrid>();

                    List<CSalesPlanQuotaItemForGrid> objDepartTeamListOld = new List<CSalesPlanQuotaItemForGrid>();
                    System.Int32 iDepartCount = 0;
                    System.Decimal dcmlDepartGroupQuota = 0;
                    CDepartTeam objTeam = null;
                    foreach (CSalesPlanQuotaItemForGrid objDepartTeam in objDepartTeamList)
                    {
                        iDepartCount = 0;
                        dcmlDepartGroupQuota = 0;

                        foreach (CSalesPlanQuotaItemForGrid objItem in objDepartListSrc)
                        {
                            if (m_objDepartList.SingleOrDefault<CDepart>(x => x.uuidID.CompareTo(objItem.Object_ID) == 0) != null)
                            {
                                objTeam = m_objDepartList.Single<CDepart>(x => x.uuidID.CompareTo(objItem.Object_ID) == 0).DepartTeam;

                                if (objTeam.uuidID.CompareTo(objDepartTeam.Object_ID) == 0)
                                {
                                    iDepartCount++;
                                    dcmlDepartGroupQuota += (objItem.Object_Quota);
                                }
                            }
                        }

                        objDepartTeamListOld.Add(new CSalesPlanQuotaItemForGrid() { Object_ID = objDepartTeam.Object_ID, Object_Quota = dcmlDepartGroupQuota, Object_SalesQuantity = iDepartCount });
                    }

                    // есть список команд с новыми долями и список со старыми долями
                    CDepartTeam objDepartTeamCurrent = null;
                    CDepart objDepartCurrent = null;
                    System.Decimal dcmlDepartGroupNewQuota = 0;
                    System.Decimal dcmlDepartGroupOldQuota = 0;
                    System.Decimal iDepartCountInGroup = 0;

                    foreach (CSalesPlanQuotaItemForGrid objQuotaItem in objDepartListSrc)
                    {
                        // для записи нужно найти новую и старую сумму по команде, куда входит это подразделение
                        objDepartTeamCurrent = null;
                        objDepartCurrent = null;
                        
                        dcmlDepartGroupNewQuota = 0;
                        dcmlDepartGroupOldQuota = 0;
                        iDepartCountInGroup = 0;

                        objDepartCurrent = m_objDepartList.SingleOrDefault<CDepart>(x => x.uuidID.CompareTo(objQuotaItem.Object_ID) == 0);
                        if (objDepartCurrent != null)
                        {
                            objDepartTeamCurrent = objDepartCurrent.DepartTeam;

                            dcmlDepartGroupNewQuota = objDepartTeamList.Single<CSalesPlanQuotaItemForGrid>(x => x.Object_ID.CompareTo(objDepartTeamCurrent.uuidID) == 0).Object_Quota;
                            dcmlDepartGroupOldQuota = objDepartTeamListOld.Single<CSalesPlanQuotaItemForGrid>(x => x.Object_ID.CompareTo(objDepartTeamCurrent.uuidID) == 0).Object_Quota;
                            iDepartCountInGroup = objDepartTeamListOld.Single<CSalesPlanQuotaItemForGrid>(x => x.Object_ID.CompareTo(objDepartTeamCurrent.uuidID) == 0).Object_SalesQuantity;
                            if (iDepartCountInGroup == 0) { iDepartCountInGroup = 1; }

                            objQuotaItem.Object_Quota = (objQuotaItem.Object_Quota + (dcmlDepartGroupNewQuota - dcmlDepartGroupOldQuota) / iDepartCountInGroup);
                        }
                    }

                    objDepartTeamList = null;
                    objDepartListSrc = null;
                    objDepartTeamListOld = null;
                    objDepartTeamCurrent = null;
                    objDepartCurrent = null;

                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("RecalcQuotaInGroupSalePlanQuotaItem.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void mitemEditSalePlanQuotaItem_Click(object sender, EventArgs e)
        {
            EditSalesPlanQuotaItemForGrid();
        }
        /// <summary>
        /// Возвращает выделенные элемент приложения к расчёту
        /// </summary>
        /// <returns>элемент приложения к расчёту</returns>
        private CSalesPlanQuotaItemForGrid GetSelectedQuotaItem()
        {
            CSalesPlanQuotaItemForGrid objQuotaItem = null;
            try
            {
                System.Guid ProductTradeMarkID = System.Guid.Empty;
                System.Guid ProductTypeID = System.Guid.Empty;
                System.Guid ObjectID = System.Guid.Empty;

                if (tabControlSalePlanQuotaItem.SelectedTabPage == tabPageDepartTeam)
                {
                    if ((gridViewSalePlanQuotaItemDecodeDepartTeam.RowCount > 0) &&
                        (gridViewSalePlanQuotaItemDecodeDepartTeam.FocusedRowHandle >= 0))
                    {
                        ProductTradeMarkID = (System.Guid)gridViewSalePlanQuotaItemDecodeDepartTeam.GetFocusedRowCellValue("ProductTradeMarkID");
                        ProductTypeID = (System.Guid)gridViewSalePlanQuotaItemDecodeDepartTeam.GetFocusedRowCellValue("ProductTypeID");
                        ObjectID = (System.Guid)gridViewSalePlanQuotaItemDecodeDepartTeam.GetFocusedRowCellValue("Object_ID");

                        objQuotaItem = m_objQuotaItemDepartTeamForGridList.SingleOrDefault<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(ProductTradeMarkID) == 0) && (x.ProductTypeID.CompareTo(ProductTypeID) == 0) && (x.Object_ID.CompareTo(ObjectID) == 0));
                    }
                }
                if (tabControlSalePlanQuotaItem.SelectedTabPage == tabPageDepart)
                {
                    if ((gridViewSalePlanQuotaItemDecodeDepart.RowCount > 0) &&
                        (gridViewSalePlanQuotaItemDecodeDepart.FocusedRowHandle >= 0))
                    {
                        ProductTradeMarkID = (System.Guid)gridViewSalePlanQuotaItemDecodeDepart.GetFocusedRowCellValue("ProductTradeMarkID");
                        ProductTypeID = (System.Guid)gridViewSalePlanQuotaItemDecodeDepart.GetFocusedRowCellValue("ProductTypeID");
                        ObjectID = (System.Guid)gridViewSalePlanQuotaItemDecodeDepart.GetFocusedRowCellValue("Object_ID");

                        objQuotaItem = m_objQuotaItemDepartForGridList.SingleOrDefault<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(ProductTradeMarkID) == 0) && (x.ProductTypeID.CompareTo(ProductTypeID) == 0) && (x.Object_ID.CompareTo(ObjectID) == 0));
                    }
                }
                if (tabControlSalePlanQuotaItem.SelectedTabPage == tabPageCustomer)
                {
                    if ((gridViewSalePlanQuotaItemDecodeCustomer.RowCount > 0) &&
                        (gridViewSalePlanQuotaItemDecodeCustomer.FocusedRowHandle >= 0))
                    {
                        ProductTradeMarkID = (System.Guid)gridViewSalePlanQuotaItemDecodeCustomer.GetFocusedRowCellValue("ProductTradeMarkID");
                        ProductTypeID = (System.Guid)gridViewSalePlanQuotaItemDecodeCustomer.GetFocusedRowCellValue("ProductTypeID");
                        ObjectID = (System.Guid)gridViewSalePlanQuotaItemDecodeCustomer.GetFocusedRowCellValue("Object_ID");

                        objQuotaItem = m_objQuotaItemCustomerForGridList.SingleOrDefault<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(ProductTradeMarkID) == 0) && (x.ProductTypeID.CompareTo(ProductTypeID) == 0) && (x.Object_ID.CompareTo(ObjectID) == 0));
                    }
                }
                if (tabControlSalePlanQuotaItem.SelectedTabPage == tabPageProductSubType)
                {
                    if ((gridViewSalePlanQuotaItemDecodePartSubType.RowCount > 0) &&
                        (gridViewSalePlanQuotaItemDecodePartSubType.FocusedRowHandle >= 0))
                    {
                        ProductTradeMarkID = (System.Guid)gridViewSalePlanQuotaItemDecodePartSubType.GetFocusedRowCellValue("ProductTradeMarkID");
                        ProductTypeID = (System.Guid)gridViewSalePlanQuotaItemDecodePartSubType.GetFocusedRowCellValue("ProductTypeID");
                        ObjectID = (System.Guid)gridViewSalePlanQuotaItemDecodePartSubType.GetFocusedRowCellValue("Object_ID");

                        objQuotaItem = m_objQuotaItemProductSubTypeForGridList.SingleOrDefault<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(ProductTradeMarkID) == 0) && (x.ProductTypeID.CompareTo(ProductTypeID) == 0) && (x.Object_ID.CompareTo(ObjectID) == 0));
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("GetSelectedQuotaItem.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return objQuotaItem;
        }
        /// <summary>
        /// Редактирование элемента в приложении к расчёту
        /// </summary>
        private void EditSalesPlanQuotaItemForGrid()
        {
            try
            {
                if (SalesPlanQuotaItemEditor != null)
                {
                    CSalesPlanQuotaItemForGrid objQuotaItem = GetSelectedQuotaItem();

                    if (objQuotaItem != null)
                    {
                        SalesPlanQuotaItemEditor.EditSalesPlanQuotaItemForGrid(objQuotaItem);
                        SalesPlanQuotaItemEditor.ShowDialog();

                        DialogResult dlgRes = SalesPlanQuotaItemEditor.DialogResult;

                        if (dlgRes == System.Windows.Forms.DialogResult.OK)
                        {
                            switch (objQuotaItem.Object_QuotaObjectType)
                            {
                                case enQuotaObjectType.DepartTeam:
                                    // пересчёт долей элементов группы
                                    RecalcQuotaInGroupSalePlanQuotaItem(objQuotaItem, m_objQuotaItemDepartTeamForGridList );
                                    gridControlSalePlanQuotaItemDecodeDepartTeam.RefreshDataSource();
                                    break;
                                case enQuotaObjectType.Depart:
                                    RecalcQuotaInGroupSalePlanQuotaItem(objQuotaItem, m_objQuotaItemDepartForGridList );
                                    gridControlSalePlanQuotaItemDecodeDepart.RefreshDataSource();
                                    break;
                                case enQuotaObjectType.Customer:
                                    RecalcQuotaInGroupSalePlanQuotaItem(objQuotaItem, m_objQuotaItemCustomerForGridList );
                                    gridControlSalePlanQuotaItemDecodeCustomer.RefreshDataSource();
                                    break;
                                case enQuotaObjectType.ProductSubType:
                                    RecalcQuotaInGroupSalePlanQuotaItem(objQuotaItem, m_objQuotaItemProductSubTypeForGridList );
                                    gridControlSalePlanQuotaItemDecodePartSubType.RefreshDataSource();
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("EditSalesPlanQuotaItemForGrid.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void DeleteSalesPlanQuotaItemForGrid(CSalesPlanQuotaItemForGrid objQuotaItem)
        {
            if (objQuotaItem == null) { return; }
            try
            {
                switch (objQuotaItem.Object_QuotaObjectType)
                {
                    case enQuotaObjectType.DepartTeam:
                        m_objQuotaItemDepartTeamForGridList.Remove(objQuotaItem);
                        gridControlSalePlanQuotaItemDecodeDepartTeam.RefreshDataSource();
                        break;
                    case enQuotaObjectType.Depart:
                        m_objQuotaItemDepartForGridList.Remove(objQuotaItem);
                        gridControlSalePlanQuotaItemDecodeDepart.RefreshDataSource();
                        break;
                    case enQuotaObjectType.Customer:
                        m_objQuotaItemCustomerForGridList.Remove(objQuotaItem);
                        gridControlSalePlanQuotaItemDecodeCustomer.RefreshDataSource();
                        break;
                    case enQuotaObjectType.ProductSubType:
                        m_objQuotaItemProductSubTypeForGridList.Remove(objQuotaItem);
                        gridControlSalePlanQuotaItemDecodePartSubType.RefreshDataSource();
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("DeleteSalesPlanQuotaItemForGrid.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void mitemDeleteSalePlanQuotaItem_Click(object sender, EventArgs e)
        {
            DeleteSalesPlanQuotaItemForGrid( GetSelectedQuotaItem() );
        }

        private void contextMenuSalePlanQuotaItem_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                CSalesPlanQuotaItemForGrid objQuotaItem = GetSelectedQuotaItem();
                if (objQuotaItem == null)
                {
                    mitemDeleteSalePlanQuotaItem.Enabled = false;
                    mitemEditSalePlanQuotaItem.Enabled = false;
                }
                else
                {
                    mitemDeleteSalePlanQuotaItem.Enabled = (objQuotaItem.Object_CalcQuota == 0);
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

        private void OpenTradeMarkDepartTeamQuota()
        {
            try
            {
                if (TradeMarkQuotaEditor != null)
                {
                    CSalesPlanQuotaItemForGrid objQuotaItem = GetSelectedQuotaItem();
                    TradeMarkQuotaEditor.OpenTradeMarkQuota(m_objQuotaItemDepartTeamForGridList);
                    TradeMarkQuotaEditor.ShowDialog();

                    DialogResult dlgRes = TradeMarkQuotaEditor.DialogResult;

                    if (dlgRes == System.Windows.Forms.DialogResult.OK)
                    {
                        if ((TradeMarkQuotaEditor.DepartTeamList != null) && (TradeMarkQuotaEditor.DepartTeamList.Count > 0) &&
                            (TradeMarkQuotaEditor.ProductTradeMark != null))
                        {
                            if (DevExpress.XtraEditors.XtraMessageBox.Show("Для выбранной марки будет произведен пересчет долей для команд и подразделений.\nОперация может занять длительное время.\nПодтвердите, пожалуйста, начало операции.", "Подтверждение",
                               System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                               System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                Cursor = Cursors.WaitCursor;

                                RecalcQuotaInDepartTeamList(TradeMarkQuotaEditor.DepartTeamList,
                                    TradeMarkQuotaEditor.ProductTradeMark, TradeMarkQuotaEditor.UseEquableDistribution);

                                Cursor = Cursors.Default;
                            }

                        }
                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("mitemSetQuotaForProductOwner_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }

        private void mitemSetQuotaForProductOwner_Click(object sender, EventArgs e)
        {
            OpenTradeMarkDepartTeamQuota();
        }
        /// <summary>
        /// Пересчет долей продаж для марки
        /// </summary>
        /// <param name="objMain"></param>
        /// <param name="objDepartTeamQuotaList"></param>
        /// <param name="UseEquableDistribution">признак "использовать равномерное распределение"</param> 
        private void RecalcQuotaInDepartTeamList(List<CSalesPlanQuotaItemForGrid> objDepartTeamQuotaList,
            CProductTradeMark objProductTradeMark, System.Boolean UseEquableDistribution)
        {
            try
            {
                // получаем список пар "Товарная марка - Товарная группа" для заданной товарной марки
                // для этого списка вносим правки для долей команд
                // если какой-то команды нет в текущем списке, то добавляем её
                // если в текущем списке присутствует команда, а в новом её нет, то команду необходимо удалить

                List<CSalesPlanQuotaItemForGrid> objCurrentDepartTeamQuotaList = m_objQuotaItemDepartTeamForGridList.Where<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(objProductTradeMark.ID) == 0)).ToList<CSalesPlanQuotaItemForGrid>();
                List<CProductType> objCurrentProductTypeList = new List<CProductType>();

                foreach (CSalesPlanQuotaItemForGrid objItem in objCurrentDepartTeamQuotaList)
                {
                    if (objCurrentProductTypeList.SingleOrDefault<CProductType>(x => x.ID.CompareTo(objItem.ProductTypeID) == 0) == null)
                    {
                        objCurrentProductTypeList.Add(objItem.ProductType);
                    }
                }

                List<CSalesPlanQuotaItemForGrid> objNewDepartTeamQuotaList = new List<CSalesPlanQuotaItemForGrid>();
                CSalesPlanQuotaItemForGrid objNewSalesPlanQuotaItem = null;
                System.Decimal dcmlSalesMoney = 0;
                System.Decimal dcmlSalesQuantity = 0;
                CSalesPlanQuotaItemForGrid objQuotaItem = null;

                foreach (CProductType objProductType in objCurrentProductTypeList)
                {
                    // для товарной группы
                    dcmlSalesMoney = 0;
                    dcmlSalesQuantity = 0;

                    objQuotaItem = objCurrentDepartTeamQuotaList.FirstOrDefault<CSalesPlanQuotaItemForGrid>(x => x.ProductTradeMarkID.CompareTo(objProductType.ID) == 0);
                    if (objQuotaItem != null)
                    {
                        dcmlSalesMoney = objQuotaItem.SalesMoney;
                        dcmlSalesQuantity = objQuotaItem.SalesQuantity;
                    }

                    foreach (CSalesPlanQuotaItemForGrid objSalesPlanQuotaItem in objDepartTeamQuotaList)
                    {
                        // для команды
                        objNewSalesPlanQuotaItem = new CSalesPlanQuotaItemForGrid()
                        {
                            Object_CalcQuota = 0,
                            Object_ID = objSalesPlanQuotaItem.Object_ID,
                            Object_Quota = objSalesPlanQuotaItem.Object_CalcQuota,
                            Object_Name = objSalesPlanQuotaItem.Object_Name,
                            Object_QuotaObjectType = objSalesPlanQuotaItem.Object_QuotaObjectType,
                            ProductTradeMark = objProductTradeMark, 
                            ProductType = objProductType,
                            SalesMoney = dcmlSalesMoney,
                            SalesQuantity = dcmlSalesQuantity
                        };

                        objNewDepartTeamQuotaList.Add(objNewSalesPlanQuotaItem);
                    }
                }

                // удаляем прежний список расшифровок с указанной маркой
                foreach (CSalesPlanQuotaItemForGrid objItemForDelete in objCurrentDepartTeamQuotaList)
                {
                    m_objQuotaItemDepartTeamForGridList.Remove(objItemForDelete);
                }
                // добавляем новый список расшифровок
                m_objQuotaItemDepartTeamForGridList.AddRange(objNewDepartTeamQuotaList);

                // пересчет долей на закладке с подразделениями
                // прежняя информация по подразделениям указанной марки
                List<CSalesPlanQuotaItemForGrid> objDepartListSrc = m_objQuotaItemDepartForGridList.Where<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(objProductTradeMark.ID) == 0)).ToList<CSalesPlanQuotaItemForGrid>();

                if ((objDepartListSrc != null) && (objDepartListSrc.Count > 0))
                {
                    foreach (CSalesPlanQuotaItemForGrid objDepartItemQuota in objDepartListSrc)
                    {
                        m_objQuotaItemDepartForGridList.Remove(objDepartItemQuota);
                    }
                }

                // пересчет долей для подразделений
                foreach (CSalesPlanQuotaItemForGrid objItem in objNewDepartTeamQuotaList)
                {
                    RecalcQuotaInDepartList(objItem, UseEquableDistribution, objDepartListSrc);
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("RecalcQuotaInGroupSalePlanQuotaItem.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                gridControlSalePlanQuotaItemDecodeDepartTeam.RefreshDataSource();
                gridControlSalePlanQuotaItemDecodeDepart.RefreshDataSource();
            }
            return;
        }
        /// <summary>
        /// Пересчет долей продаж в закладке с подразделениями
        /// </summary>
        /// <param name="objDepartTeamQuota">"ТМ-Тгр-Команда-доля продаж"</param>
        /// <param name="UseEquableDistribution">признак "использовать равномерное распределение"</param>
        /// <param name="objDepartListSrc">список долей ДО изменения</param>
        private void RecalcQuotaInDepartList(CSalesPlanQuotaItemForGrid objDepartTeamQuota, System.Boolean UseEquableDistribution,
            List<CSalesPlanQuotaItemForGrid> objDepartListSrc)
        {
            try
            {
                if (objDepartTeamQuota.Object_QuotaObjectType == enQuotaObjectType.DepartTeam)
                {
                    // в случае исправления доли продаж у команды, необходимо пересчитать доли продаж у подразделений
                    // запрашивается список активных подразделений команды и её доля распределяется между подразделениями поровну
                    System.String strErr = System.String.Empty;

                    List<CDepart> objDepartListForTeam = CDepart.GetDepartListForTeam(m_objProfile, null, objDepartTeamQuota.Object_ID, ref strErr, true);

                    if ((objDepartListForTeam != null) && (objDepartListForTeam.Count > 0))
                    {
                        List<CSalesPlanQuotaItemForGrid> objNewQuotaDepartList = new List<CSalesPlanQuotaItemForGrid>();
                        System.Int32 iDepartInTeamCount = objDepartListForTeam.Count;
                        System.Decimal dcmlOldQuota = 0;
                        System.Decimal dcmlOldQuotaAll = 0;
                        CSalesPlanQuotaItemForGrid SrcQuotaDepart = null;

                        // необходимо подсчитать сумму всех долей 
                        foreach (CDepart objDepartItem in objDepartListForTeam)
                        {
                            dcmlOldQuota = 0;
                            SrcQuotaDepart = objDepartListSrc.SingleOrDefault<CSalesPlanQuotaItemForGrid>(x => ((x.ProductTradeMarkID.CompareTo(objDepartTeamQuota.ProductTradeMarkID) == 0) &&
                                (x.ProductTypeID.CompareTo(objDepartTeamQuota.ProductTypeID) == 0) &&
                                (x.Object_ID.CompareTo(objDepartItem.uuidID) == 0)));

                            if (SrcQuotaDepart != null)
                            {
                                dcmlOldQuotaAll += SrcQuotaDepart.Object_CalcQuota;
                            }

                        }


                        foreach (CDepart objDepart in objDepartListForTeam)
                        {
                            dcmlOldQuota = 0;
                            SrcQuotaDepart = objDepartListSrc.SingleOrDefault<CSalesPlanQuotaItemForGrid>(x => ((x.ProductTradeMarkID.CompareTo(objDepartTeamQuota.ProductTradeMarkID) == 0) &&
                                (x.ProductTypeID.CompareTo(objDepartTeamQuota.ProductTypeID) == 0) &&
                                (x.Object_ID.CompareTo(objDepart.uuidID) == 0)));

                            if (SrcQuotaDepart != null)
                            {
                                dcmlOldQuota = SrcQuotaDepart.Object_CalcQuota;
                            }

                            objNewQuotaDepartList.Add(new CSalesPlanQuotaItemForGrid()
                                {
                                    Object_ID = objDepart.uuidID,
                                    SalesMoney = objDepartTeamQuota.SalesMoney,
                                    SalesQuantity = objDepartTeamQuota.SalesQuantity,
                                    ProductTradeMark = objDepartTeamQuota.ProductTradeMark,
                                    ProductType = objDepartTeamQuota.ProductType,
                                    Object_QuotaObjectType = enQuotaObjectType.Depart,
                                    Object_Name = objDepart.DepartCode,
                                    Object_CalcQuota = 0,
                                    Object_Quota = (((UseEquableDistribution == true ) || ( dcmlOldQuotaAll == 0 ) ) ? (objDepartTeamQuota.Object_Quota / iDepartInTeamCount) : (objDepartTeamQuota.Object_Quota * (dcmlOldQuota / dcmlOldQuotaAll)))
                                });
                        }


                        if ((objNewQuotaDepartList != null) && (objNewQuotaDepartList.Count > 0))
                        {
                            m_objQuotaItemDepartForGridList.AddRange(objNewQuotaDepartList);
                        }


                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("RecalcQuotaInDepartList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
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
                    "SendMessageToLog.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Журнал расчётов
        /// <summary>
        /// Загружает журнал расчётов
        /// </summary>
        public void LoadSalesPlanQuota()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (m_objSalesPlanQuotaList != null)
                { m_objSalesPlanQuotaList.Clear(); }

                barBtnAdd.Enabled = false;
                barBtnEdit.Enabled = false;
                barBtnDelete.Enabled = false;
                barBtnRefresh.Enabled = false;
                btnPrint.Enabled = false;
                dtBeginDate.Enabled = false;
                dtEndDate.Enabled = false;

                gridControlSalesPlanQuotaList.DataSource = null;

                System.DateTime Begindate = dtBeginDate.DateTime;
                System.DateTime Enddate = dtEndDate.DateTime;
                System.String strErr = "";
                System.Int32 iRes = 0;

                m_objSalesPlanQuotaList = CSalesPlanQuota.GetSalesPlanQuotaList(m_objProfile, Begindate, Enddate, ref strErr, ref iRes);
                if (gridControlSalesPlanQuotaList.DataSource == null)
                {
                    gridControlSalesPlanQuotaList.DataSource = m_objSalesPlanQuotaList;
                }
                gridControlSalesPlanQuotaList.RefreshDataSource();

                barBtnAdd.Enabled = true;
                barBtnEdit.Enabled = true;
                barBtnDelete.Enabled = true;
                barBtnRefresh.Enabled = true;
                btnPrint.Enabled = true;
                dtBeginDate.Enabled = true;
                dtEndDate.Enabled = true;

                if (m_objSalesPlanQuotaList.Count > 0)
                {
                    gridViewSalesPlanQuotaList.FocusedRowHandle = 0;
                }

                gridViewSalesPlanQuotaList.BestFitColumns();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadSalesPlanQuota.\n\nТекст ошибки: " + f.Message, "Ошибка",
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
                    LoadSalesPlanQuota();
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
        private void frmSalesPlanQuota_Shown(object sender, EventArgs e)
        {
            try
            {
                LoadProductTrademarkList();

                LoadProductPartTypeList();

                RestoreLayoutFromRegistry();

                tabControl.SelectedTabPage = tabPageViewer;

                LoadSalesPlanQuota();

                StartThreadComboBoxForDecodeEditor();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("frmSalesPlanQuota_Shown().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void barBtnRefresh_Click(object sender, EventArgs e)
        {
            LoadSalesPlanQuota();
        }

        #endregion

        #region Свойства расчёта

        private void gridViewSalesPlanQuotaList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                FocusedSalesPlanQuotaChanged();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewSalesPlanQuotaList_FocusedRowChanged. Текст ошибки: " + f.Message);
            }

            return;
        }

        private void gridViewSalesPlanQuotaList_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                FocusedSalesPlanQuotaChanged();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewSalesPlanQuotaList_RowCountChanged. Текст ошибки: " + f.Message);
            }

            return;
        }

        /// <summary>
        /// Определяет, какой платёж расчёт в журнале и отображает его свойства
        /// </summary>
        private void FocusedSalesPlanQuotaChanged()
        {
            try
            {
                ShowSalesPlanQuotaProperties(SelectedSalesPlanQuota);

                barBtnAdd.Enabled = !m_bOnlyView;
                barBtnEdit.Enabled = ( gridViewSalesPlanQuotaList.FocusedRowHandle >= 0);
                barBtnDelete.Enabled = ((!m_bOnlyView) && (gridViewSalesPlanQuotaList.FocusedRowHandle >= 0));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Отображение свойств расчёта. Текст ошибки: " + f.Message);
            }

            return;
        }

        /// <summary>
        /// Отображает свойства расчёта
        /// </summary>
        /// <param name="objSalesPlanQuota">расчёт</param>
        private void ShowSalesPlanQuotaProperties(CSalesPlanQuota objSalesPlanQuota)
        {
            try
            {
                this.tableLayoutPanelSalesPlanQuotaProperties.SuspendLayout();

                txtSalesPlanQuotaName.Text = System.String.Empty;
                txtSalesPlanQuotaDate.Text = System.String.Empty;
                txtSalesPlanQuotaCondition.Text = System.String.Empty;
                txtSalesPlanQuotaDescription.Text = System.String.Empty;

                if (objSalesPlanQuota != null)
                {
                    txtSalesPlanQuotaName.Text = objSalesPlanQuota.Name;
                    txtSalesPlanQuotaDate.Text = objSalesPlanQuota.Date.ToShortDateString();
                    txtSalesPlanQuotaCondition.Text = objSalesPlanQuota.CalcPeriod;
                    txtSalesPlanQuotaDescription.Text = objSalesPlanQuota.Description;

                }

                this.tableLayoutPanelSalesPlanQuotaProperties.ResumeLayout(false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Отображение свойств расчёта. Текст ошибки: " + f.Message);
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
                editorSalesPlanQuotaName.Properties.ReadOnly = bModeReadOnly;
                editorSalesPlanQuotaDate.Properties.ReadOnly = bModeReadOnly;
                editorSalesPlanQuotaBeginDate.Properties.ReadOnly = bModeReadOnly;
                editorSalesPlanQuotaEndDate.Properties.ReadOnly = bModeReadOnly;
                editorSalesPlanQuotaConditionProductOwner.Enabled = (bModeReadOnly == false);
                editorSalesPlanQuotaConditionProductGroup.Enabled = (bModeReadOnly == false);
                editorSalesPlanQuotaDescription.Properties.ReadOnly = bModeReadOnly;
                btnStartCalc.Enabled = (bModeReadOnly == false);

                gridControlSalePlanQuotaItemDecodeCustomer.ContextMenuStrip = ((bModeReadOnly == false) ? contextMenuSalePlanQuotaItem : null);
                gridControlSalePlanQuotaItemDecodeDepart.ContextMenuStrip = ((bModeReadOnly == false) ? contextMenuSalePlanQuotaItem : null);
                gridControlSalePlanQuotaItemDecodeDepartTeam.ContextMenuStrip = ((bModeReadOnly == false) ? contextMenuSalePlanQuotaItem : null);
                gridControlSalePlanQuotaItemDecodePartSubType.ContextMenuStrip = ((bModeReadOnly == false) ? contextMenuSalePlanQuotaItem : null);

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
                editorSalesPlanQuotaName.Properties.Appearance.BackColor = ((editorSalesPlanQuotaName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorSalesPlanQuotaDate.Properties.Appearance.BackColor = ((editorSalesPlanQuotaDate.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorSalesPlanQuotaBeginDate.Properties.Appearance.BackColor = ((editorSalesPlanQuotaBeginDate.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorSalesPlanQuotaEndDate.Properties.Appearance.BackColor = ((editorSalesPlanQuotaEndDate.EditValue == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorSalesPlanQuotaConditionProductOwner.Appearance.BackColor = ((editorSalesPlanQuotaConditionProductOwner.CheckedItems.Count == 0) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorSalesPlanQuotaConditionProductGroup.Appearance.BackColor = ((editorSalesPlanQuotaConditionProductGroup.CheckedItems.Count == 0) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                bRet = ((editorSalesPlanQuotaName.Text != "") && (editorSalesPlanQuotaDate.EditValue != null) &&
                    (editorSalesPlanQuotaBeginDate.EditValue != null) &&
                    (editorSalesPlanQuotaEndDate.EditValue != null) && 
                    (editorSalesPlanQuotaConditionProductOwner.CheckedItems.Count > 0) &&
                    (editorSalesPlanQuotaConditionProductGroup.CheckedItems.Count > 0)
                    );

            }
            catch (System.Exception f)
            {
                SendMessageToLog("ValidateProperties. Текст ошибки: " + f.Message);
            }

            return bRet;
        }

        private void txtSalesPlanQuotaPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
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
                SendMessageToLog(String.Format("Ошибка изменения свойств расчёта. Текст ошибки: {0}", f.Message));
            }
            finally
            {
            }

            return;
        }

        private void editorSalesPlanQuotaConditionProductOwner_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }

                SetPropertiesModified(true);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(String.Format("Ошибка изменения свойств расчёта. Текст ошибки: {0}", f.Message));
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Редактировать расчёт
        private void barBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                EditSalesPlanQuota(SelectedSalesPlanQuota, false);

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

        private void gridSalesPlanQuotaList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                EditSalesPlanQuota(SelectedSalesPlanQuota, false);

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
                editorSalesPlanQuotaName.Text = "";
                editorSalesPlanQuotaDate.EditValue = null;
                editorSalesPlanQuotaBeginDate.EditValue = null;
                editorSalesPlanQuotaEndDate.EditValue = null;
                editorSalesPlanQuotaDescription.Text = "";

                gridControlSalePlanQuotaItemDecodeCustomer.DataSource = null;
                gridControlSalePlanQuotaItemDecodeDepart.DataSource = null;
                gridControlSalePlanQuotaItemDecodeDepartTeam.DataSource = null;
                gridControlSalePlanQuotaItemDecodePartSubType.DataSource = null;

                if (m_objQuotaItemCustomerForGridList != null)
                {
                    m_objQuotaItemCustomerForGridList.Clear();
                }
                if (m_objQuotaItemDepartForGridList != null)
                {
                    m_objQuotaItemDepartForGridList.Clear();
                }
                if (m_objQuotaItemDepartTeamForGridList != null)
                {
                    m_objQuotaItemDepartTeamForGridList.Clear();
                }
                if (m_objQuotaItemProductSubTypeForGridList != null)
                {
                    m_objQuotaItemProductSubTypeForGridList.Clear();
                }

                foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductOwner.Items)
                {
                    objListBoxItem.CheckState = CheckState.Unchecked;
                }
                foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductGroup.Items)
                {
                    objListBoxItem.CheckState = CheckState.Unchecked;
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
        /// Загружает свойства расчёта для редактирования
        /// </summary>
        /// <param name="objSalesPlanQuota">расчёт</param>
        /// <param name="bNewObject">признак "новый расчёт"</param>
        public void EditSalesPlanQuota(CSalesPlanQuota objSalesPlanQuota, System.Boolean bNewObject)
        {
            if (objSalesPlanQuota == null) { return; }
            
            m_bDisableEvents = true;
            m_bNewObject = bNewObject;
            try
            {
                this.tableLayoutPanelBackground.SuspendLayout();

                ClearControls();

                editorSalesPlanQuotaName.Text = objSalesPlanQuota.Name;
                editorSalesPlanQuotaDate.EditValue = objSalesPlanQuota.Date;
                editorSalesPlanQuotaDescription.Text = objSalesPlanQuota.Description;

                if (objSalesPlanQuota.CalculationConditions != null)
                {
                    editorSalesPlanQuotaBeginDate.EditValue = objSalesPlanQuota.CalculationConditions.CalcPeriodBeginDate;
                    editorSalesPlanQuotaEndDate.EditValue = objSalesPlanQuota.CalculationConditions.CalcPeriodEndDate;

                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductOwner.Items )
                    {
                        if (objSalesPlanQuota.CalculationConditions.ProductTradeMarkList.SingleOrDefault<CProductTradeMark>(x => x.ID.CompareTo(((ERP_Mercury.Common.CProductTradeMark)objListBoxItem.Value).ID) == 0) != null)
                        {
                            objListBoxItem.CheckState = CheckState.Checked;
                        }
                    }
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductGroup.Items)
                    {
                        if (objSalesPlanQuota.CalculationConditions.ProductTypeList.SingleOrDefault<CProductType>(x => x.ID.CompareTo(((ERP_Mercury.Common.CProductType)objListBoxItem.Value).ID) == 0) != null)
                        {
                            objListBoxItem.CheckState = CheckState.Checked;
                        }
                    }

                }

                System.String strErr = "";
                System.Int32 iRes = 0;

                objSalesPlanQuota.SalesPlanQuotaItemList = CSalesPlanQuota.GetSalesPlanQuotaItemList( m_objProfile, objSalesPlanQuota.ID, ref strErr, ref iRes );
                SendMessageToLog("Строк в расчёте: " + objSalesPlanQuota.SalesPlanQuotaItemList.Count.ToString());

                if ((objSalesPlanQuota.SalesPlanQuotaItemList != null) && (objSalesPlanQuota.SalesPlanQuotaItemList.Count > 0))
                {
                    m_objQuotaItemCustomerForGridList = CSalesPlanQuota.TransformSalesPlanQuotaItemListForGrid(objSalesPlanQuota.SalesPlanQuotaItemList, enQuotaObjectType.Customer, ref strErr);
                    m_objQuotaItemDepartForGridList = CSalesPlanQuota.TransformSalesPlanQuotaItemListForGrid(objSalesPlanQuota.SalesPlanQuotaItemList, enQuotaObjectType.Depart, ref strErr);
                    m_objQuotaItemDepartTeamForGridList = CSalesPlanQuota.TransformSalesPlanQuotaItemListForGrid(objSalesPlanQuota.SalesPlanQuotaItemList, enQuotaObjectType.DepartTeam, ref strErr);
                    m_objQuotaItemProductSubTypeForGridList = CSalesPlanQuota.TransformSalesPlanQuotaItemListForGrid(objSalesPlanQuota.SalesPlanQuotaItemList, enQuotaObjectType.ProductSubType, ref strErr);
                }
                else
                {
                    SendMessageToLog("Ошибка редактирования платежа. Текст ошибки: " + strErr);
                }

                gridControlSalePlanQuotaItemDecodeCustomer.DataSource = m_objQuotaItemCustomerForGridList;
                gridControlSalePlanQuotaItemDecodeDepart.DataSource = m_objQuotaItemDepartForGridList;
                gridControlSalePlanQuotaItemDecodeDepartTeam.DataSource = m_objQuotaItemDepartTeamForGridList;
                gridControlSalePlanQuotaItemDecodePartSubType.DataSource = m_objQuotaItemProductSubTypeForGridList;

                gridControlSalePlanQuotaItemDecodeCustomer.RefreshDataSource();
                gridControlSalePlanQuotaItemDecodeDepart.RefreshDataSource();
                gridControlSalePlanQuotaItemDecodeDepartTeam.RefreshDataSource();
                gridControlSalePlanQuotaItemDecodePartSubType.RefreshDataSource();

                SetPropertiesModified(false);
                ValidateProperties();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования платежа. Текст ошибки: " + f.Message);
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

        #region Новый расчёт
        private void barBtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                NewSalesPlanQuota();

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
        /// Новый расчёт
        /// </summary>
        public void NewSalesPlanQuota()
        {
            try
            {
                m_bNewObject = true;
                m_bDisableEvents = true;
                ClearControls();

                if (m_objQuotaItemCustomerForGridList == null)
                {
                    m_objQuotaItemCustomerForGridList = new List<CSalesPlanQuotaItemForGrid>();
                }
                if (m_objQuotaItemDepartForGridList == null)
                {
                    m_objQuotaItemDepartForGridList = new List<CSalesPlanQuotaItemForGrid>();
                }
                if (m_objQuotaItemDepartTeamForGridList == null)
                {
                    m_objQuotaItemDepartTeamForGridList = new List<CSalesPlanQuotaItemForGrid>();
                }
                if (m_objQuotaItemProductSubTypeForGridList == null)
                {
                    m_objQuotaItemProductSubTypeForGridList = new List<CSalesPlanQuotaItemForGrid>();
                }

                gridControlSalePlanQuotaItemDecodeCustomer.DataSource = m_objQuotaItemCustomerForGridList;
                gridControlSalePlanQuotaItemDecodeDepart.DataSource = m_objQuotaItemDepartForGridList;
                gridControlSalePlanQuotaItemDecodeDepartTeam.DataSource = m_objQuotaItemDepartTeamForGridList;
                gridControlSalePlanQuotaItemDecodePartSubType.DataSource = m_objQuotaItemProductSubTypeForGridList;

                m_objSalesPlanQuotaList.Add(new CSalesPlanQuota() 
                { 
                    ID = System.Guid.Empty, Date = System.DateTime.Today, Name = CSalesPlanQuota.GetNewName(), 
                    CalculationConditions = new CSalesPlanQuotaCalculationConditions() 
                    { 
                        CalcPeriodBeginDate = System.DateTime.Today.AddMonths(-3), CalcPeriodEndDate = System.DateTime.Today 
                    } 
                });

                gridControlSalesPlanQuotaList.RefreshDataSource();
                gridViewSalesPlanQuotaList.FocusedRowHandle = ( m_objSalesPlanQuotaList.Count - 1 );

                this.tableLayoutPanelBackground.SuspendLayout();

                editorSalesPlanQuotaName.Text = SelectedSalesPlanQuota.Name;
                editorSalesPlanQuotaDate.EditValue = SelectedSalesPlanQuota.Date;
                editorSalesPlanQuotaDescription.Text = SelectedSalesPlanQuota.Description;
                editorSalesPlanQuotaBeginDate.DateTime = SelectedSalesPlanQuota.CalculationConditions.CalcPeriodBeginDate;
                editorSalesPlanQuotaEndDate.DateTime = SelectedSalesPlanQuota.CalculationConditions.CalcPeriodEndDate;

                btnEdit.Enabled = false;
                btnCancel.Enabled = true;

                // запрашивается список марок и групп для расчёта
                System.String strErr = "";
                System.Int32 iRes = 0;
                List<CProductTradeMark> objProductTradeMarkList = new List<CProductTradeMark>();
                List<CProductType> objProductTypeList = new List<CProductType>();

                CSalesPlanQuota.GetProductTradeMarkProductTypeListForActiveCalcPlan(m_objProfile, ref objProductTradeMarkList, ref objProductTypeList,
                    ref strErr, ref iRes);
                if (iRes == 0)
                {
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductOwner.Items)
                    {
                        if (objProductTradeMarkList.SingleOrDefault<CProductTradeMark>(x => x.ID.CompareTo(((ERP_Mercury.Common.CProductTradeMark)objListBoxItem.Value).ID) == 0) != null)
                        {
                            objListBoxItem.CheckState = CheckState.Checked;
                        }
                    }
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductGroup.Items)
                    {
                        if (objProductTypeList.SingleOrDefault<CProductType>(x => x.ID.CompareTo(((ERP_Mercury.Common.CProductType)objListBoxItem.Value).ID) == 0) != null)
                        {
                            objListBoxItem.CheckState = CheckState.Checked;
                        }
                    }
                }

                SetModeReadOnly(false);
                SetPropertiesModified(true);


            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания платежа. Текст ошибки: " + f.Message);
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

        #region Удалить расчёт
        /// <summary>
        /// Удаляет расчёт
        /// </summary>
        /// <param name="objSalesPlanQuota">объект класса "Расчёт"</param>
        private void DeleteSalesPlanQuota(CSalesPlanQuota objSalesPlanQuota)
        {
            if (objSalesPlanQuota == null) { return; }
            System.String strErr = "";
            System.Int32 iRes = 0;

            try
            {
                System.Int32 iFocusedRowHandle = gridViewSalesPlanQuotaList.FocusedRowHandle;
                if (DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Подтвердите, пожалуйста, удаление расчёта.\n\n№: {0}\n\nДата: {1}", objSalesPlanQuota.Name, objSalesPlanQuota.Date.ToShortDateString()), "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.No) { return; }

                if ( CSalesPlanQuota.DeleteSalesPlanQuota( m_objProfile, objSalesPlanQuota.ID, ref strErr, ref iRes) == true)
                {
                    LoadSalesPlanQuota();
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Предупреждение",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    SendMessageToLog("Удаление расчёта. Текст ошибки: " + strErr);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Удаление расчёта. Текст ошибки: " + f.Message);
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
                DeleteSalesPlanQuota(SelectedSalesPlanQuota);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Удаление расчёта. Текст ошибки: " + f.Message);
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
                        "Выйти из редактора расчёта без сохранения изменений?", "Подтверждение",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                }

                tabControl.SelectedTabPage = tabPageViewer;
                if (SelectedSalesPlanQuota != null)
                {
                    if (SelectedSalesPlanQuota.ID.CompareTo(System.Guid.Empty) == 0)
                    {
                        // новый не сохраненный отчёт
                        m_objSalesPlanQuotaList.Remove(SelectedSalesPlanQuota);
                        gridControlSalesPlanQuotaList.RefreshDataSource();
                    }
                    else
                    {
                        System.Int32 iIndxSelectedObject = m_objSalesPlanQuotaList.IndexOf(m_objSalesPlanQuotaList.SingleOrDefault<CSalesPlanQuota>(x => x.ID.CompareTo(SelectedSalesPlanQuota.ID) == 0));
                        if (iIndxSelectedObject >= 0)
                        {
                            gridViewSalesPlanQuotaList.FocusedRowHandle = gridViewSalesPlanQuotaList.GetRowHandle(iIndxSelectedObject);
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
        private System.Boolean SaveSalesPlanQuotaInDataBase(ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;
            System.Boolean bOkSave = false;

            Cursor = Cursors.WaitCursor;
            try
            {
                // из элементов управления зачитываются значения свойств расчёта для последующего сохранения в БД
                System.Guid ID = SelectedSalesPlanQuota.ID;
                System.String Name = editorSalesPlanQuotaName.Text;
                System.DateTime Date = editorSalesPlanQuotaDate.DateTime;
                System.String Description = editorSalesPlanQuotaDescription.Text;
                
                // Условия расчёта: период продаж
                CSalesPlanQuotaCalculationConditions CalculationConditions = new CSalesPlanQuotaCalculationConditions() 
                { 
                    CalcPeriodBeginDate = editorSalesPlanQuotaBeginDate.DateTime, CalcPeriodEndDate = editorSalesPlanQuotaEndDate.DateTime, 
                    ProductTradeMarkList = new List<CProductTradeMark>(), ProductTypeList = new List<CProductType>() 
                };
                // Условия расчёта: товарные марки и группы
                foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductOwner.CheckedItems)
                {
                    if (objListBoxItem.CheckState == CheckState.Checked)
                    {
                        CalculationConditions.ProductTradeMarkList.Add((CProductTradeMark)objListBoxItem.Value);
                    }
                }
                foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductGroup.CheckedItems)
                {
                    if (objListBoxItem.CheckState == CheckState.Checked)
                    {
                        CalculationConditions.ProductTypeList.Add((CProductType)objListBoxItem.Value);
                    }
                }
                // запись списка товарных марок и групп в xml
                CalculationConditions.XMLView = CSalesPlanQuota.GetSalesPlanQuotaConditionBlank( m_objProfile, 
                    ( CalculationConditions.ProductTradeMarkList.Count + CalculationConditions.ProductTypeList.Count ), 
                    ref strErr, ref iRes);

                System.Xml.XmlNodeList nodeList = CalculationConditions.XMLView.GetElementsByTagName("SalesPlanQuotaConditionItem");
                if (nodeList != null)
                {
                    System.Int32 iNodeIndx = 0;

                    foreach (CProductTradeMark objItem in CalculationConditions.ProductTradeMarkList)
                    {
                        nodeList[iNodeIndx].Attributes["ItemTypeId"].InnerText = "0";
                        nodeList[iNodeIndx].Attributes["ItemGuid"].InnerText = objItem.ID.ToString();
                        nodeList[iNodeIndx].Attributes["ItemName"].InnerText = objItem.Name;

                        iNodeIndx++;
                    }

                    foreach (CProductType objItem in CalculationConditions.ProductTypeList)
                    {
                        nodeList[iNodeIndx].Attributes["ItemTypeId"].InnerText = "1";
                        nodeList[iNodeIndx].Attributes["ItemGuid"].InnerText = objItem.ID.ToString();
                        nodeList[iNodeIndx].Attributes["ItemName"].InnerText = objItem.Name;

                        iNodeIndx++;
                    }
                }

                // Приложение к расчёту
                List<CSalesPlanQuotaItem> SalesPlanQuotaItemList = new List<CSalesPlanQuotaItem>();

                if (((m_objQuotaItemDepartTeamForGridList != null) && (m_objQuotaItemDepartTeamForGridList.Count > 0)) ||
                    ((m_objQuotaItemDepartForGridList != null) && (m_objQuotaItemDepartForGridList.Count > 0)) ||
                    ((m_objQuotaItemCustomerForGridList != null) && (m_objQuotaItemCustomerForGridList.Count > 0)) ||
                    ((m_objQuotaItemProductSubTypeForGridList != null) && (m_objQuotaItemProductSubTypeForGridList.Count > 0)))
                {
                    // хотя бы в одной из таблиц есть значения
                    List<CProductTradeMark> objProductTradeMarkList = new List<CProductTradeMark>();
                    List<CProductType> objProductTypeList = new List<CProductType>();
                    List<CSalesPlanQuotaItemForGrid> objSalesPlanQuotaItemForGridList = new List<CSalesPlanQuotaItemForGrid>();
                    List<CSalesPlanQuotaItemForGrid> objGroupPlanQuotaItemList = new List<CSalesPlanQuotaItemForGrid>();
                    CSalesPlanQuotaItem objSalesPlanQuotaItem = null;

                    objSalesPlanQuotaItemForGridList.AddRange(m_objQuotaItemDepartTeamForGridList);
                    objSalesPlanQuotaItemForGridList.AddRange(m_objQuotaItemDepartForGridList);
                    objSalesPlanQuotaItemForGridList.AddRange(m_objQuotaItemCustomerForGridList);
                    objSalesPlanQuotaItemForGridList.AddRange(m_objQuotaItemProductSubTypeForGridList);

                    foreach (CSalesPlanQuotaItemForGrid objForGrid in objSalesPlanQuotaItemForGridList)
                    {
                        if (objProductTradeMarkList.SingleOrDefault<CProductTradeMark>(x => x.ID.CompareTo(objForGrid.ProductTradeMarkID) == 0) == null)
                        {
                            objProductTradeMarkList.Add(objForGrid.ProductTradeMark);
                        }
                        if (objProductTypeList.SingleOrDefault<CProductType>(x => x.ID.CompareTo(objForGrid.ProductTypeID) == 0) == null)
                        {
                            objProductTypeList.Add(objForGrid.ProductType);
                        }
                    }

                    // данные из таблиц необходимо преобразовать к списку элементов класса "CSalesPlanQuotaItem"
                    if ((objProductTradeMarkList != null) && (objProductTradeMarkList.Count > 0) &&
                        (objProductTypeList != null) && (objProductTypeList.Count > 0))
                    {
                        objProductTradeMarkList = objProductTradeMarkList.Distinct<CProductTradeMark>( ).ToList<CProductTradeMark>();
                        objProductTypeList = objProductTypeList.Distinct<CProductType>().ToList<CProductType>();

                        foreach (CProductTradeMark objProductTradeMark in objProductTradeMarkList)
                        {
                            foreach (CProductType objProductType in objProductTypeList)
                            {
                                objGroupPlanQuotaItemList = objSalesPlanQuotaItemForGridList.Where<CSalesPlanQuotaItemForGrid>(x => ((x.ProductTradeMark.ID.CompareTo(objProductTradeMark.ID) == 0) && (x.ProductType.ID.CompareTo(objProductType.ID) == 0))).ToList<CSalesPlanQuotaItemForGrid>();
                                if ((objGroupPlanQuotaItemList != null) && (objGroupPlanQuotaItemList.Count > 0))
                                {
                                    objSalesPlanQuotaItem = new CSalesPlanQuotaItem() { ID = objGroupPlanQuotaItemList[0].ID, ProductTradeMark = objProductTradeMark, ProductType = objProductType, SalesMoney = objGroupPlanQuotaItemList[0].SalesMoney, SalesQuantity = objGroupPlanQuotaItemList[0].SalesQuantity, QuotaList = new List<CQuotaItemObject>() };

                                    foreach (CSalesPlanQuotaItemForGrid objItem in objGroupPlanQuotaItemList)
                                    {
                                        objSalesPlanQuotaItem.QuotaList.Add(new CQuotaItemObject() { ID = objItem.Object_ID, Name = objItem.Object_Name, SalesMoney = objItem.Object_SalesMoney, SalesQuantity = objItem.Object_SalesQuantity, QuotaObjectType = objItem.Object_QuotaObjectType, CalcQuota = objItem.Object_CalcQuota, Quota = objItem.Object_Quota });
                                    }

                                    SalesPlanQuotaItemList.Add(objSalesPlanQuotaItem);
                                }
                            }
                        }
                    }
                }

                if ((SalesPlanQuotaItemList != null) && (SalesPlanQuotaItemList.Count > 0))
                {
                    // проверка значений
                    if (CSalesPlanQuota.IsAllParametersValid(Name, Date, CalculationConditions, SalesPlanQuotaItemList, ref strErr) == true)
                    {
                        System.Guid objectIDinDB = System.Guid.Empty;

                        bOkSave = CSalesPlanQuota.SaveToDB(m_bNewObject, m_objProfile, ID, Name, Date,
                            CalculationConditions.CalcPeriodBeginDate, CalculationConditions.CalcPeriodEndDate,
                            Description, CalculationConditions, SalesPlanQuotaItemList,
                            ref strErr, ref iRes, ref objectIDinDB);

                        if (bOkSave == true)
                        {
                            if (m_bNewObject == true)
                            {
                                SelectedSalesPlanQuota.ID = objectIDinDB;
                            }
                            SelectedSalesPlanQuota.Name = Name;
                            SelectedSalesPlanQuota.Date = Date;
                            SelectedSalesPlanQuota.Description = Description;
                            SelectedSalesPlanQuota.CalculationConditions = CalculationConditions;
                            SelectedSalesPlanQuota.SalesPlanQuotaItemList = SalesPlanQuotaItemList;

                            gridControlSalesPlanQuotaList.RefreshDataSource();
                        }

                    }
                }
                else
                {
                    strErr = "В приложении к расчёту отстутствуют данные.\nПожалуйста, произведите расчёт, либо добавьте данные вручную.";
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

                if (SaveSalesPlanQuotaInDataBase(ref strErr, ref iRes) == true)
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
                SendMessageToLog("Ошибка сохранения изменений в расчёте. Текст ошибки: " + f.Message);
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
        private void ReadInfoFromGrid( DevExpress.XtraGrid.Views.Grid.GridView objGridView,  ExcelWorksheet worksheet )
        {
            try
            {
                foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in objGridView.Columns)
                {
                    if (objColumn.Visible == false) { continue; }

                    worksheet.Cells[1, objColumn.VisibleIndex + 1].Value = objColumn.Caption;
                }

                using (var range = worksheet.Cells[1, 1, 1, objGridView.Columns.Count])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 14;
                }

                System.Int32 iCurrentRow = 2;
                System.Int32 iRowsCount = objGridView.RowCount;
                for (System.Int32 i = 0; i < iRowsCount; i++)
                {
                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in objGridView.Columns)
                    {
                        if (objColumn.Visible == false) { continue; }

                        worksheet.Cells[iCurrentRow, objColumn.VisibleIndex + 1].Value = objGridView.GetRowCellValue(i, objColumn);
                        if (objColumn.FieldName == "Date")
                        {
                            worksheet.Cells[iCurrentRow, objColumn.VisibleIndex + 1].Style.Numberformat.Format = "DD.MM.YYYY";
                        }
                    }
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
        /// экспорт в файл MS Excel содержимого приложения к расчёту
        /// </summary>
        /// <param name="strFileName">путь к файлу</param>
        private void ExportToExcelSalesPlanQuotaItemList(string strFileName)
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
                    ExcelWorksheet worksheet = worksheet = package.Workbook.Worksheets.Add("Команды");
                    ReadInfoFromGrid(gridViewSalePlanQuotaItemDecodeDepartTeam, worksheet);

                    worksheet = package.Workbook.Worksheets.Add("Подразделения");
                    ReadInfoFromGrid(gridViewSalePlanQuotaItemDecodeDepart, worksheet);

                    worksheet = package.Workbook.Worksheets.Add("Клиенты");
                    ReadInfoFromGrid(gridViewSalePlanQuotaItemDecodeCustomer, worksheet);

                    worksheet = package.Workbook.Worksheets.Add("Подгруппы");
                    ReadInfoFromGrid( gridViewSalePlanQuotaItemDecodePartSubType,  worksheet);

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
        private void btnPrintSalesPlanQuotaItemList_Click(object sender, EventArgs e)
        {
            try
            {
                ExportToExcelSalesPlanQuotaItemList(String.Format("{0}{1}.xlsx", System.IO.Path.GetTempPath(), "Расчёт"));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnPrintSalesPlanQuotaItemList_Click. Текст ошибки: " + f.Message);
            }

            return;
        }
        /// <summary>
        /// Выгружает в файл MS Excel список расчётов
        /// </summary>
        /// <param name="strFileName">полный путь к файлу (каталог + имя)</param>
        private void ExportToExcelSalesPlanQuotaList(string strFileName)
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

                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewSalesPlanQuotaList.Columns)
                    {
                        if (objColumn.Visible == false) { continue; }

                        worksheet.Cells[1, objColumn.VisibleIndex + 1].Value = objColumn.Caption;
                    }

                    using (var range = worksheet.Cells[1, 1, 1, gridViewSalesPlanQuotaList.Columns.Count])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Size = 14;
                    }

                    System.Int32 iCurrentRow = 2;
                    System.Int32 iRowsCount = gridViewSalesPlanQuotaList.RowCount;
                    for (System.Int32 i = 0; i < iRowsCount; i++)
                    {
                        foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewSalesPlanQuotaList.Columns)
                        {
                            if (objColumn.Visible == false) { continue; }

                            worksheet.Cells[iCurrentRow, objColumn.VisibleIndex + 1].Value = gridViewSalesPlanQuotaList.GetRowCellValue(i, objColumn);
                            if (objColumn.FieldName == "Date")
                            {
                                worksheet.Cells[iCurrentRow, objColumn.VisibleIndex + 1].Style.Numberformat.Format = "DD.MM.YYYY";
                            }
                        }
                        iCurrentRow++;
                    }

                    iCurrentRow--;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewSalesPlanQuotaList.Columns.Count].AutoFitColumns(0);
                    worksheet.Cells[1, 1, iCurrentRow, gridViewSalesPlanQuotaList.Columns.Count].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewSalesPlanQuotaList.Columns.Count].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewSalesPlanQuotaList.Columns.Count].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewSalesPlanQuotaList.Columns.Count].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

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
                ExportToExcelSalesPlanQuotaList(String.Format("{0}{1}.xlsx", System.IO.Path.GetTempPath(), this.Text));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnPrint_Click. Текст ошибки: " + f.Message);
            }

            return;
        }
        #endregion

        #region Расчёт коэффициентов
        private void btnStartCalc_Click(object sender, EventArgs e)
        {
            StartCalcSalesPlanQuota();
        }
        /// <summary>
        /// Стартует поток, в котором производится расчёт
        /// </summary>
        public void StartCalcSalesPlanQuota()
        {
            try
            {
                if ((editorSalesPlanQuotaBeginDate.EditValue == null) || (editorSalesPlanQuotaEndDate.EditValue == null) ||
                    (editorSalesPlanQuotaBeginDate.DateTime.CompareTo(editorSalesPlanQuotaEndDate.DateTime) > 0))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Для расчёта необходимо задать корректный период дат.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                if ((editorSalesPlanQuotaConditionProductOwner.CheckedItems.Count == 0) || (editorSalesPlanQuotaConditionProductGroup.CheckedItems.Count == 0))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Необходимо указать хотя бы одну товарную марку и группу.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                
                // инициализируем делегаты
                m_LoadCalcSalesPlanQuotaDelegate = new LoadCalcSalesPlanQuotaDelegate(LoadSalesPlanQuotaItemListInGrids);
                m_objQuotaItemCustomerForGridList.Clear();
                m_objQuotaItemDepartForGridList.Clear();
                m_objQuotaItemDepartTeamForGridList.Clear();
                m_objQuotaItemProductSubTypeForGridList.Clear();

                btnStartCalc.Enabled = false;
                editorSalesPlanQuotaBeginDate.Enabled = false;
                editorSalesPlanQuotaEndDate.Enabled = false;
                editorSalesPlanQuotaConditionProductOwner.Enabled = false;
                editorSalesPlanQuotaConditionProductGroup.Enabled = false;

                gridControlSalePlanQuotaItemDecodeCustomer.DataSource = null;
                gridControlSalePlanQuotaItemDecodeDepart.DataSource = null;
                gridControlSalePlanQuotaItemDecodeDepartTeam.DataSource = null;
                gridControlSalePlanQuotaItemDecodePartSubType.DataSource = null;

                gridControlSalePlanQuotaItemDecodeCustomer.ContextMenuStrip = null;
                gridControlSalePlanQuotaItemDecodeDepart.ContextMenuStrip = null;
                gridControlSalePlanQuotaItemDecodeDepartTeam.ContextMenuStrip = null;
                gridControlSalePlanQuotaItemDecodePartSubType.ContextMenuStrip = null;

                SearchProcessWoring.Visible = true;
                SearchProcessWoring.Refresh();

                btnPrintSalesPlanQuotaItemList.Enabled = false;

                // запуск потока
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
                ci.NumberFormat.CurrencyDecimalSeparator = ".";
                ci.NumberFormat.NumberDecimalSeparator = ".";
                this.ThreadCalcSalesPlanQuota = new System.Threading.Thread(CalcSalesPlanQuotaInThread);
                this.ThreadCalcSalesPlanQuota.CurrentCulture = ci;
                this.ThreadCalcSalesPlanQuota.Start();
                Thread.Sleep(1000);

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadLoadEarningList().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        /// <summary>
        /// Вызов расчёта коэффициентов и передача данных в гриды
        /// </summary>
        public void CalcSalesPlanQuotaInThread()
        {
            try
            {
                CSalesPlanQuotaCalculationConditions objSalesPlanQuotaCalculationConditions = new CSalesPlanQuotaCalculationConditions() 
                { 
                    CalcPeriodBeginDate = editorSalesPlanQuotaBeginDate.DateTime,
                    CalcPeriodEndDate = editorSalesPlanQuotaEndDate.DateTime, 
                    ProductTradeMarkList = new List<CProductTradeMark>(), 
                    ProductTypeList = new List<CProductType>() 
                };

                foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductOwner.CheckedItems)
                {
                    if (objListBoxItem.CheckState == CheckState.Checked)
                    {
                        objSalesPlanQuotaCalculationConditions.ProductTradeMarkList.Add((CProductTradeMark)objListBoxItem.Value);
                    }
                }
                foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in editorSalesPlanQuotaConditionProductGroup.CheckedItems)
                {
                    if (objListBoxItem.CheckState == CheckState.Checked)
                    {
                        objSalesPlanQuotaCalculationConditions.ProductTypeList.Add(( CProductType )objListBoxItem.Value);
                    }
                }


                System.String strErr = "";
                System.Int32 iRes = -1;
                List<CSalesPlanQuotaItemForGrid> objSalesPlanQuotaItemForGridList = CSalesPlanQuota.CalcSalesPlanQuota(m_objProfile, SelectedSalesPlanQuota.ID, 
                    objSalesPlanQuotaCalculationConditions, ref strErr, ref iRes);

                if ((objSalesPlanQuotaItemForGridList != null) && (objSalesPlanQuotaItemForGridList.Count > 0))
                {
                    //List<CSalesPlanQuotaItemForGrid> objSalesPlanQuotaItemForGridList = CSalesPlanQuota.TransformSalesPlanQuotaItemListForGrid(objSalesPlanQuotaItemList, ref strErr);
                    List<CSalesPlanQuotaItemForGrid> objAddSalesPlanQuotaItemForGridList = new List<CSalesPlanQuotaItemForGrid>();

                    if ((objSalesPlanQuotaItemForGridList != null) && (objSalesPlanQuotaItemForGridList.Count > 0))
                    {
                        System.Int32 iRecCount = 0;
                        System.Int32 iRecAllCount = 0;

                        foreach (CSalesPlanQuotaItemForGrid objSalesPlanQuotaItemForGrid in objSalesPlanQuotaItemForGridList)
                        {
                            objAddSalesPlanQuotaItemForGridList.Add(objSalesPlanQuotaItemForGrid);
                            iRecCount++;
                            iRecAllCount++;

                            if (iRecCount == 1000)
                            {
                                iRecCount = 0;
                                Thread.Sleep(1000);
                                this.Invoke(m_LoadCalcSalesPlanQuotaDelegate, new Object[] { objAddSalesPlanQuotaItemForGridList, iRecAllCount });
                                objAddSalesPlanQuotaItemForGridList.Clear();
                            }

                        }
                        if (iRecCount != 1000)
                        {
                            iRecCount = 0;
                            this.Invoke(m_LoadCalcSalesPlanQuotaDelegate, new Object[] { objAddSalesPlanQuotaItemForGridList, iRecAllCount });
                            objAddSalesPlanQuotaItemForGridList.Clear();
                        }

                    }                
                }

                this.Invoke(m_LoadCalcSalesPlanQuotaDelegate, new Object[] { null, 0 });
                this.m_bThreadFinishJob = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("CalcSalesPlanQuotaInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// загружает в журнал приложение к расчёту
        /// </summary>
        /// <param name="objSalesPlanQuotaItemForGridList">приложение к расчёту</param>
        /// <param name="iRowCountInList">количество строк, которые требуется загрузить в журнал</param>
        private void LoadSalesPlanQuotaItemListInGrids(List<CSalesPlanQuotaItemForGrid> objSalesPlanQuotaItemForGridList, System.Int32 iRowCountInList)
        {
            try
            {
                if ((objSalesPlanQuotaItemForGridList != null) && (objSalesPlanQuotaItemForGridList.Count > 0) && 
                    ( ( gridViewSalePlanQuotaItemDecodeCustomer.RowCount + gridViewSalePlanQuotaItemDecodeDepart.RowCount + 
                        gridViewSalePlanQuotaItemDecodeDepartTeam.RowCount + gridViewSalePlanQuotaItemDecodePartSubType.RowCount ) < iRowCountInList) )
                {
                    m_objQuotaItemCustomerForGridList.AddRange(objSalesPlanQuotaItemForGridList.Where<CSalesPlanQuotaItemForGrid>(x => x.Object_QuotaObjectType == enQuotaObjectType.Customer).ToList<CSalesPlanQuotaItemForGrid>());
                    m_objQuotaItemDepartForGridList.AddRange(objSalesPlanQuotaItemForGridList.Where<CSalesPlanQuotaItemForGrid>(x => x.Object_QuotaObjectType == enQuotaObjectType.Depart).ToList<CSalesPlanQuotaItemForGrid>());
                    m_objQuotaItemDepartTeamForGridList.AddRange(objSalesPlanQuotaItemForGridList.Where<CSalesPlanQuotaItemForGrid>(x => x.Object_QuotaObjectType == enQuotaObjectType.DepartTeam).ToList<CSalesPlanQuotaItemForGrid>());
                    m_objQuotaItemProductSubTypeForGridList.AddRange(objSalesPlanQuotaItemForGridList.Where<CSalesPlanQuotaItemForGrid>(x => x.Object_QuotaObjectType == enQuotaObjectType.ProductSubType).ToList<CSalesPlanQuotaItemForGrid>());

                    if (gridControlSalePlanQuotaItemDecodeCustomer.DataSource == null)
                    {
                        gridControlSalePlanQuotaItemDecodeCustomer.DataSource = m_objQuotaItemCustomerForGridList;
                    }
                    gridControlSalePlanQuotaItemDecodeCustomer.RefreshDataSource();

                    if ( gridControlSalePlanQuotaItemDecodeDepart.DataSource == null)
                    {
                        gridControlSalePlanQuotaItemDecodeDepart.DataSource = m_objQuotaItemDepartForGridList;
                    }
                    gridControlSalePlanQuotaItemDecodeDepart.RefreshDataSource();

                    if ( gridControlSalePlanQuotaItemDecodeDepartTeam.DataSource == null)
                    {
                        gridControlSalePlanQuotaItemDecodeDepartTeam.DataSource = m_objQuotaItemDepartTeamForGridList;
                    }
                    gridControlSalePlanQuotaItemDecodeDepartTeam.RefreshDataSource();

                    if ( gridControlSalePlanQuotaItemDecodePartSubType.DataSource == null )
                    {
                        gridControlSalePlanQuotaItemDecodePartSubType.DataSource = m_objQuotaItemProductSubTypeForGridList;
                    }
                    gridControlSalePlanQuotaItemDecodePartSubType.RefreshDataSource();
                }
                else
                {
                    Thread.Sleep(1000);
                    SearchProcessWoring.Visible = false;

                    btnStartCalc.Enabled = true;
                    btnPrintSalesPlanQuotaItemList.Enabled = true;

                    editorSalesPlanQuotaBeginDate.Enabled = true;
                    editorSalesPlanQuotaEndDate.Enabled = true;
                    editorSalesPlanQuotaConditionProductOwner.Enabled = true;
                    editorSalesPlanQuotaConditionProductGroup.Enabled = true;

                    gridControlSalePlanQuotaItemDecodeCustomer.RefreshDataSource();
                    gridControlSalePlanQuotaItemDecodeDepart.RefreshDataSource();
                    gridControlSalePlanQuotaItemDecodeDepartTeam.RefreshDataSource();
                    gridControlSalePlanQuotaItemDecodePartSubType.RefreshDataSource();

                    gridControlSalePlanQuotaItemDecodeCustomer.ContextMenuStrip = contextMenuSalePlanQuotaItem;
                    gridControlSalePlanQuotaItemDecodeDepart.ContextMenuStrip = contextMenuSalePlanQuotaItem;
                    gridControlSalePlanQuotaItemDecodeDepartTeam.ContextMenuStrip = contextMenuSalePlanQuotaItem;
                    gridControlSalePlanQuotaItemDecodePartSubType.ContextMenuStrip = contextMenuSalePlanQuotaItem;

                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewSalePlanQuotaItemDecodeCustomer.Columns)
                    {
                        if (objColumn.Visible == true)
                        {
                            objColumn.BestFit();
                        }
                    }

                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewSalePlanQuotaItemDecodeDepart.Columns)
                    {
                        if (objColumn.Visible == true)
                        {
                            objColumn.BestFit();
                        }
                    }

                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewSalePlanQuotaItemDecodeDepartTeam.Columns)
                    {
                        if (objColumn.Visible == true)
                        {
                            objColumn.BestFit();
                        }
                    }

                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewSalePlanQuotaItemDecodePartSubType.Columns)
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
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadSalesPlanQuotaItemListInGrids.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Ручная корректировка долей продаж для подразделений команды
        /// <summary>
        /// Установка долей продаж для подразделений
        /// </summary>
        /// <param name="objDepartQuotaList">список подразделений с долями продаж</param>
        /// <param name="objProductTradeMark">товарная марка</param>
        private void RecalcQuotaInDepartList(List<CSalesPlanQuotaItemForGrid> objDepartQuotaList,
            CProductTradeMark objProductTradeMark)
        {
            try
            {
                // для каждой пары "Товарная марка - Товарная группа" для товарной марки objProductTradeMark
                // необходимо найти подразделения, входящие в команду objDepartTeam
                // найденные подразделения удаляем, а вместо них прописываем данные из objDepartQuotaList

                List<CSalesPlanQuotaItemForGrid> objCurrentDepartQuotaList = m_objQuotaItemDepartForGridList.Where<CSalesPlanQuotaItemForGrid>(x => (x.ProductTradeMarkID.CompareTo(objProductTradeMark.ID) == 0)).ToList<CSalesPlanQuotaItemForGrid>();

                List<CProductType> objCurrentProductTypeList = new List<CProductType>();

                foreach (CSalesPlanQuotaItemForGrid objItem in objCurrentDepartQuotaList)
                {
                    if (objCurrentProductTypeList.SingleOrDefault<CProductType>(x => x.ID.CompareTo(objItem.ProductTypeID) == 0) == null)
                    {
                        objCurrentProductTypeList.Add(objItem.ProductType);
                    }
                }

                List<CSalesPlanQuotaItemForGrid> objNewDepartQuotaList = new List<CSalesPlanQuotaItemForGrid>();
                CSalesPlanQuotaItemForGrid objNewSalesPlanQuotaItem = null;
                System.Decimal dcmlSalesMoney = 0;
                System.Decimal dcmlSalesQuantity = 0;
                CSalesPlanQuotaItemForGrid objQuotaItem = null;

                foreach (CProductType objProductType in objCurrentProductTypeList)
                {
                    // для товарной группы
                    dcmlSalesMoney = 0;
                    dcmlSalesQuantity = 0;

                    objQuotaItem = objCurrentDepartQuotaList.FirstOrDefault<CSalesPlanQuotaItemForGrid>(x => x.ProductTypeID.CompareTo(objProductType.ID) == 0);
                    if (objQuotaItem != null)
                    {
                        dcmlSalesMoney = objQuotaItem.SalesMoney;
                        dcmlSalesQuantity = objQuotaItem.SalesQuantity;
                    }

                    foreach (CSalesPlanQuotaItemForGrid objSalesPlanQuotaItem in objDepartQuotaList)
                    {
                        // подразделения
                        objNewSalesPlanQuotaItem = new CSalesPlanQuotaItemForGrid()
                        {
                            Object_CalcQuota = 0,
                            Object_ID = objSalesPlanQuotaItem.Object_ID,
                            Object_Quota = objSalesPlanQuotaItem.Object_CalcQuota,
                            Object_Name = objSalesPlanQuotaItem.Object_Name,
                            Object_QuotaObjectType = objSalesPlanQuotaItem.Object_QuotaObjectType,
                            ProductTradeMark = objProductTradeMark,
                            ProductType = objProductType,
                            SalesMoney = dcmlSalesMoney,
                            SalesQuantity = dcmlSalesQuantity
                        };

                        objNewDepartQuotaList.Add(objNewSalesPlanQuotaItem);
                    }
                }

                // удаляем прежний список расшифровок с указанной маркой
                foreach (CSalesPlanQuotaItemForGrid objItemForDelete in objCurrentDepartQuotaList)
                {
                    m_objQuotaItemDepartForGridList.Remove(objItemForDelete);
                }
                // добавляем новый список расшифровок
                m_objQuotaItemDepartForGridList.AddRange(objNewDepartQuotaList);

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("RecalcQuotaInDepartList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                gridControlSalePlanQuotaItemDecodeDepart.RefreshDataSource();
            }
            return;
        }

        /// <summary>
        /// Открывает редактор, в котором доли продаж распределяются в рамках марки по подразделениям команды
        /// </summary>
        private void OpenTradeMarkDepartQuota(  )
        {
            try
            {
                if (TradeMarkDepartTeamQuota != null)
                {
                    CSalesPlanQuotaItemForGrid objQuotaItem = GetSelectedQuotaItem();
                    TradeMarkDepartTeamQuota.OpenTradeMarkDepartTeamQuota(m_objQuotaItemDepartForGridList);
                    TradeMarkDepartTeamQuota.ShowDialog();

                    DialogResult dlgRes = TradeMarkDepartTeamQuota.DialogResult;

                    if (dlgRes == System.Windows.Forms.DialogResult.OK)
                    {
                        if ((TradeMarkDepartTeamQuota.SalesPlanQuotaItemList != null) && (TradeMarkDepartTeamQuota.SalesPlanQuotaItemList.Count > 0) &&
                            (TradeMarkDepartTeamQuota.ProductTradeMark != null) )
                        {
                            if (DevExpress.XtraEditors.XtraMessageBox.Show("Для выбранной марки будет произведен пересчет долей для подразделений." +
                                "\nОперация может занять длительное время.\nПодтвердите, пожалуйста, начало операции.", "Подтверждение",
                               System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                               System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                Cursor = Cursors.WaitCursor;

                                RecalcQuotaInDepartList( TradeMarkDepartTeamQuota.SalesPlanQuotaItemList, 
                                    TradeMarkDepartTeamQuota.ProductTradeMark );

                                Cursor = Cursors.Default;
                            }

                        }
                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("mitemSetQuotaForProductOwnerAndDepartTeam_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }

        private void mitemSetQuotaForProductOwnerAndDepartTeam_Click(object sender, EventArgs e)
        {
            OpenTradeMarkDepartQuota();
        }
        #endregion


    }

    public class SalesPlanQuotaEditor : PlugIn.IClassTypeView
    {
        public override void Run(UniXP.Common.MENUITEM objMenuItem, System.String strCaption)
        {
            frmSalesPlanQuota obj = new frmSalesPlanQuota(objMenuItem) { Text = strCaption, MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent, Visible = true };
        }
    }

}
