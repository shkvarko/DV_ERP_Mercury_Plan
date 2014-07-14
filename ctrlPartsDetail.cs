using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraExport;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraEditors;
using ERP_Mercury.Common;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace ERPMercuryPlan
{
    enum enTabProperties
    {
        All = 0,
        Properties = 1,
        ProductList = 2,
        Price = 3,
        Image = 4
    }
    public partial class ctrlPartsDetail : UserControl
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CProductSubType> m_objProductList;
        private List<CProduct> m_objProductNewList;
        private List<CProduct> m_objPartsList;
        private List<CSettingForCalcPrice> m_objSettingForCalcPriceList;
        private CProductSubTypePriceList m_objProductSubTypePriceList;
        private frmPartsList m_objFrmPartsList;
        private System.String m_strFileCalcPriceName;
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlProductList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewPartsNew
        {
            get { return gridControlPartsNewList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewProduct
        {
            get { return gridControlProduct.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnViewPricesContract
        {
            get { return gridControlPricesContract.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }

        private System.Boolean m_bIsComboBoxFill;
        private System.Boolean m_bDisableEvents;
        public System.Boolean IsSubtypePropertiesChanged {get; set;}
        public System.Boolean IsSubtypeProductListChanged { get; set; }
        public System.Boolean IsSubtypePriceChanged { get; set; }
        public System.Boolean IsChangedImage { get; set; }
        
        private CProductSubType m_objSelectedProduct;
        private List<CProductSubTypeStock> m_objProductSubTypeStockList;
        private const System.String m_strBtnExitName = "Выход";
        private const System.String m_strBtnCancelName = "Отмена";

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

        private System.Boolean m_bExistsDREditProductSubType;
        private System.Boolean m_bExistsDREditOnlyImageProductSubType;

        #endregion

        #region Конструктор
        public ctrlPartsDetail(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objProductList = null;
            m_objPartsList = null;
            m_objProductNewList = null;
            m_objSelectedProduct = null;
            m_bIsComboBoxFill = false;
            btnSetToPartList.Visible = false;
            m_objFrmPartsList = null;
            m_objSettingForCalcPriceList = null;
            m_strFileCalcPriceName = "";
            m_objProductSubTypePriceList = new CProductSubTypePriceList();

            m_bDisableEvents = false;
            IsSubtypePropertiesChanged = false;
            IsSubtypeProductListChanged = false;
            IsSubtypePriceChanged = false;
            IsChangedImage = false;
            btnCancel.Text = m_strBtnExitName;
            m_bExistsDREditProductSubType = false;
            m_bExistsDREditOnlyImageProductSubType = false;

            OpenForm();
        }
        #endregion

        #region OpenForm
        public void OpenForm()
        {
            try
            {
                CheckDRForEditProductSubtype();

                AddGridColumns();

                AddGridColumnsToPricesContract();

                RestoreLayoutFromRegistry();

                AddGridColumnsToPartsNew();

                AddGridColumnsToProduct();

                //LoadProductNewList();

                m_objProductSubTypeStockList = CProductSubTypeStock.GetProductSubTypeStock(m_objProfile, null);

                tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

                //StartThreadWithLoadData();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("OpenForm. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        private void ctrlPartsDetail_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCalcPriceList();

                LoadProductList();

                StartThreadWithLoadData();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ctrlPartsDetail_Load. Текст ошибки : " + f.Message);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }

        #endregion

        #region Проверка прав на редактирование подгрупп
        /// <summary>
        /// Проверка динамических прав
        /// </summary>
        private void CheckDRForEditProductSubtype()
        {
            try
            {
                m_bExistsDREditProductSubType = m_objProfile.GetClientsRight().GetState(Consts.strDRForEditProductSubType);
                m_bExistsDREditOnlyImageProductSubType = m_objProfile.GetClientsRight().GetState(Consts.strDRForEditOnlyProductSubTypeImage);

                btnAdd.Visible = m_bExistsDREditProductSubType;
                btnDelete.Visible = m_bExistsDREditProductSubType;
                btnExportForCalcPrice.Visible = m_bExistsDREditProductSubType;
                btnAddToPriceCalcInExcel.Visible = m_bExistsDREditProductSubType;
                tabPageSetValues.PageVisible = m_bExistsDREditProductSubType;
                btnEdit.Visible = ((m_bExistsDREditProductSubType == true) || (m_bExistsDREditOnlyImageProductSubType == true));
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки динамических прав пользователя. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        #endregion

        #region Список подгрупп
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();
            AddGridColumn(ColumnView, "IsSetToPart", "Подгруппа содержит товары");
            //AddGridColumn(ColumnView, "IsContainingOnlyPartsActual", "Содержит только актуальные товары");
            AddGridColumn(ColumnView, "IsContainingOnlyPartsValid", "Подгруппа содержит актуальные товары");
            AddGridColumn(ColumnView, "QuantityInWarehouseForShipping", "Остаток на складах отгрузки");
            AddGridColumn(ColumnView, "SubTypeStateName", "Состояние");
            AddGridColumn(ColumnView, "Name", "Товарная подгруппа");
            AddGridColumn(ColumnView, "ID", "Код товарной подгруппы");
            AddGridColumn(ColumnView, "ID_Ib", "Код");
            AddGridColumn(ColumnView, "ProductOwner", "Товарная марка");
            AddGridColumn(ColumnView, "ProductType", "Товарная группа");
            AddGridColumn(ColumnView, "ProductLineName", "Товарная линия");
            AddGridColumn(ColumnView, "PriceEXW", "Цена exw");
            AddGridColumn(ColumnView, "VendorTariff", "Тариф поставщика");
            AddGridColumn(ColumnView, "TransportTariff", "Тариф транспортный");
            AddGridColumn(ColumnView, "CustomsTariff", "Тариф таможенный");
            AddGridColumn(ColumnView, "Margin", "Наценка базовая");
            AddGridColumn(ColumnView, "NDS", "НДС");
            AddGridColumn(ColumnView, "Discont", "Наценка, компенс. пост. скидку");
            AddGridColumn(ColumnView, "MarkUpRequired", "Требуемая наценка, %");
            
            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                objColumn.Visible = (objColumn.FieldName != "ID");
            }
        }
        private void AddGridColumnsToPartsNew()
        {
            ColumnViewPartsNew.Columns.Clear();
            AddGridColumn(ColumnViewPartsNew, "ID", "Код товара");
            AddGridColumn(ColumnViewPartsNew, "ID_Ib", "Код");
            AddGridColumn(ColumnViewPartsNew, "ProductTradeMarkName", "Товарная марка");
            AddGridColumn(ColumnViewPartsNew, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnViewPartsNew, "ProductSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnViewPartsNew, "ProductLineName", "Товарная линия");
            AddGridColumn(ColumnViewPartsNew, "ProductFullName", "Товар");
            AddGridColumn(ColumnViewPartsNew, "ProductCategoryName", "Категория товара");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewPartsNew.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                objColumn.Visible = (objColumn.FieldName != "ID");
            }
        }
        private void AddGridColumnsToPricesContract()
        {
            ColumnViewPricesContract.Columns.Clear();
            AddGridColumn(ColumnViewPricesContract, "ProductIB_Id", "Код товара");
            AddGridColumn(ColumnViewPricesContract, "ProductFullName", "Товар");
            AddGridColumn(ColumnViewPricesContract, "Price0", "Импортёр");
            AddGridColumn(ColumnViewPricesContract, "Price2", "Магазины");
            AddGridColumn(ColumnViewPricesContract, "Price0_2", "Магазины, у.е.");
            AddGridColumn(ColumnViewPricesContract, "Price3", "Опт-Центр");
            AddGridColumn(ColumnViewPricesContract, "Price0_3", "Опт-Центр, у.е");
            AddGridColumn(ColumnViewPricesContract, "Price4", "Цена импортёра без НДС, руб.");
            AddGridColumn(ColumnViewPricesContract, "Price5", "Розница с налогом");
            AddGridColumn(ColumnViewPricesContract, "Price6", "Магазин-склад");
            AddGridColumn(ColumnViewPricesContract, "Price7", "Переброска");
            AddGridColumn(ColumnViewPricesContract, "Price8", "C&C приход");
            AddGridColumn(ColumnViewPricesContract, "Price10", "Экспорт");
            AddGridColumn(ColumnViewPricesContract, "Price0_10", "Экспорт, у.е.");
            AddGridColumn(ColumnViewPricesContract, "Price11", "Регионы спец.");
            AddGridColumn(ColumnViewPricesContract, "Price0_11", "Регионы спец. у.е.");
            AddGridColumn(ColumnViewPricesContract, "Price12", "Импортёр (C&C)");
            AddGridColumn(ColumnViewPricesContract, "Price12_1", "Розница-брак C&C без налога");
            AddGridColumn(ColumnViewPricesContract, "Price12_2", "Розница-брак C&C с налогом");
            AddGridColumn(ColumnViewPricesContract, "Price9", "Розница C&C с налогом");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewPricesContract.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                objColumn.Visible = (objColumn.FieldName != "ID");
                if (objColumn.FieldName == "ProductFullName")
                {
                    objColumn.Width = objColumn.GetBestWidth();
                }
                else if (objColumn.FieldName != "ProductIB_Id")
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0.00";
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


        private void AddGridColumnsToProduct()
        {
            ColumnViewProduct.Columns.Clear();
            AddGridColumn(ColumnViewProduct, "ID", "Код товара");
            AddGridColumn(ColumnViewProduct, "ID_Ib", "Код");
            AddGridColumn(ColumnViewProduct, "ProductTradeMarkName", "Товарная марка");
            AddGridColumn(ColumnViewProduct, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnViewProduct, "ProductFullName", "Товар");
            AddGridColumn(ColumnViewProduct, "ProductSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnViewProduct, "ProductLineName", "Товарная линия");
            AddGridColumn(ColumnViewProduct, "ProductCategoryName", "Категория товара");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewProduct.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                objColumn.Visible = (objColumn.FieldName != "ID");
                if (objColumn.FieldName == "ProductFullName")
                {
                    objColumn.Width = objColumn.GetBestWidth();
                }
            }
        }


        public void SetSplitter()
        {
            splitContainerControl.SplitterPosition = splitContainerControl.Size.Width - treeListStock.Size.Width;
        }

        /// <summary>
        /// Загружает список товарных подгрупп
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadProductList()
        {
            System.Boolean bRet = false;
            m_bDisableEvents = true; 
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.splitContainerControl.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                gridControlProductList.DataSource = null;

                m_objProductList = CProductSubType.GetProductSubTypeList(m_objProfile, null, false);

                if (m_objProductList != null)
                {
                    gridControlProductList.DataSource = m_objProductList;

                }

                foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
                {
                    if ((objColumn.FieldName == "ProductOwner") || (objColumn.FieldName == "ProductType") || (objColumn.FieldName == "ProductLineName") || (objColumn.FieldName == "Name"))
                    {
                        objColumn.BestFit();
                    }
                    objColumn.Fixed = (((objColumn.FieldName == "IsSetToPart") || (objColumn.FieldName == "IsContainingOnlyPartsValid") || (objColumn.FieldName == "QuantityInWarehouseForShipping") || (objColumn.FieldName == "SubTypeStateName") || (objColumn.FieldName == "Name")) ? DevExpress.XtraGrid.Columns.FixedStyle.Left : DevExpress.XtraGrid.Columns.FixedStyle.None);
                }

                LoadComboBoxItems();

                SetPrimaryPropertiesForSetPnl();

                this.splitContainerControl.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();

                splitContainerControl.Refresh();
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
                LoadProductList();

                LoadProductNewList();

                StartThreadWithLoadData();
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

        private void LoadProductNewList()
        {
            try
            {
                gridControlPartsNewList.DataSource = null;

                m_objProductNewList = CProduct.GetProductList(m_objProfile, null, true);

                if (m_objProductNewList != null)
                {
                    gridControlPartsNewList.DataSource = m_objProductNewList;

                    tabPagePartsNew.Image = ((m_objProductNewList.Count > 0) ? ERPMercuryPlan.Properties.Resources.warning : null);
                }

                foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewPartsNew.Columns)
                {
                    objColumn.BestFit();
                }

//                SaveLayoutToRegistry();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка новинок. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return ;
        }
        /// <summary>
        /// Считывает настройки журналов из реестра
        /// </summary>
        private void RestoreLayoutFromRegistry()
        {
            System.String strReestrPath = this.m_objProfile.GetRegKeyBase();
            strReestrPath += ("\\Tools\\");
            try
            {
                gridViewProductList.RestoreLayoutFromRegistry(strReestrPath + gridViewProductList.Name);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка загрузки настроек списка товарных подгрупп. Текст ошибки : " + f.Message);
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка загрузки настроек списка товарных подгрупп.\n\nТекст ошибки : " + f.Message, "Внимание",
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
            strReestrPath += ("\\Tools\\");
            try
            {
                gridViewProductList.SaveLayoutToRegistry(strReestrPath + gridViewProductList.Name);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SaveLayoutToRegistry. Текст ошибки : " + f.Message);
            }
            finally // очищаем занимаемые ресурсы
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
                    "SendMessageToLog. Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Свойства подгруппы
        /// <summary>
        /// Возвращает ссылку на выбранную в списке подгруппу
        /// </summary>
        /// <returns>ссылка на товар</returns>
        private CProductSubType GetSelectedProduct()
        {
            CProductSubType objRet = null;
            try
            {
                System.Guid uuidID = (System.Guid)gridViewProductList.GetFocusedRowCellValue("ID");
                objRet = m_objProductList.Single<CProductSubType>(x => x.ID.CompareTo(uuidID) == 0);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка поиска выбранного товара. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return objRet;
        }
        /// <summary>
        /// Отображает свойства товара
        /// </summary>
        /// <param name="objProduct">товар</param>
        private void ShowProductProperties(CProductSubType objProduct)
        {
            try
            {
                this.splitContainerControl.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPartsDetail)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.treeListStock)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPartsList)).BeginInit();

                treeListPartsDetail.Nodes.Clear();
                treeListStock.Nodes.Clear();
                treeListPartsList.Nodes.Clear();

                if (objProduct != null)
                {
                    treeListPartsDetail.AppendNode(new object[] { "Код подгруппы", objProduct.ID_Ib }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Товарная подгруппа", objProduct.Name }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Товарная марка", objProduct.ProductOwner }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Товарная группа", objProduct.ProductType }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Товарная линия", objProduct.ProductLineName }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Состояние", objProduct.SubTypeStateName }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Цена exw", System.String.Format("{0:### ### ##0.000}", objProduct.PriceEXW) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Тариф поставщика", System.String.Format("{0:### ### ##0.000}", objProduct.VendorTariff) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Тариф транспортный, %", System.String.Format("{0:### ### ##0.000}", objProduct.TransportTariff) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Тариф таможенный, %", System.String.Format("{0:### ### ##0.000}", objProduct.CustomsTariff) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Наценка базовая, %", System.String.Format("{0:### ### ##0.000}", objProduct.Margin) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "НДС, %", System.String.Format("{0:### ### ##0}", objProduct.NDS) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Наценка, компенсирующая постоянную скидку, %", System.String.Format("{0:### ### ##0.000}", objProduct.Discont) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Требуемая наценка, %", System.String.Format("{0:### ### ##0.000}", objProduct.MarkUpRequired) }, null);

                    // остатки по складам
                    if (objProduct.ProductSubTypeStockList == null)
                    {
                        objProduct.ProductSubTypeStockList = new List<CProductSubTypeStock>();

                        objProduct.ProductSubTypeStockList = (from Stock in m_objProductSubTypeStockList
                                                              where Stock.ProductSubTypeId.CompareTo(objProduct.ID) == 0
                                                              select Stock).ToList<CProductSubTypeStock>();
                    }
                    if (objProduct.ProductSubTypeStockList != null)
                    {
                        foreach (CProductSubTypeStock objSubTypeStock in objProduct.ProductSubTypeStockList)
                        {
                            treeListStock.AppendNode(new object[] { objSubTypeStock.IsForShipping, objSubTypeStock.Name, System.String.Format("{0:### ### ##0}", objSubTypeStock.Quantity) }, null);
                        }
                    }
                }

                // товары из подгруппы
                if (m_objPartsList != null)
                {
                    var ProductList =
                    from Product in m_objPartsList
                    where Product.ProductSubTypeIbID == objProduct.ID_Ib
                    select Product;

                    foreach (CProduct objItem in ProductList)
                    {
                        treeListPartsList.AppendNode(new object[] { objItem.ProductFullName }, null);
                    }

                }


                this.splitContainerControl.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListPartsDetail)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.treeListStock)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPartsList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отображения свойств товарной подгруппы. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void gridViewProductList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                ShowProductProperties(GetSelectedProduct());
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка смены записи в списке. Текст ошибки: " + f.Message);
            }
        }
        private void gridViewProductList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "IsSetToPart")
                {
                    
                    if (e.CellValue != null)
                    {
                        System.Drawing.Image img = ((System.Convert.ToString(e.CellValue) == "Нет") ? ERPMercuryPlan.Properties.Resources.delete2 : ERPMercuryPlan.Properties.Resources.check2 );
                        Rectangle rImg = new Rectangle(e.Bounds.X - 6 + e.Bounds.Width / 2, e.Bounds.Y + (e.Bounds.Height - img.Size.Height) / 2, img.Width, img.Height);
                        e.Graphics.DrawImage(img, rImg);
                        Rectangle r = e.Bounds;
                        e.Handled = true;
                    }
                }
                if (e.Column.FieldName == "IsContainingOnlyPartsValid")
                {
                    if (e.CellValue != null)
                    {
                        System.Drawing.Image img = ((System.Convert.ToString(e.CellValue) == "Нет") ? ERPMercuryPlan.Properties.Resources.delete2 : ERPMercuryPlan.Properties.Resources.check2);
                        Rectangle rImg = new Rectangle(e.Bounds.X - 6 + e.Bounds.Width / 2, e.Bounds.Y + (e.Bounds.Height - img.Size.Height) / 2, img.Width, img.Height);
                        e.Graphics.DrawImage(img, rImg);
                        Rectangle r = e.Bounds;
                        e.Handled = true;
                    }
                }
                if (e.Column.FieldName == "SubTypeStateName")
                {
                    System.Drawing.Image img = ERPMercuryPlan.Properties.Resources.warning;
                    if ((e.CellValue != null) && (System.Convert.ToString(e.CellValue) != "")
                        && (gridViewProductList.GetRowCellValue(e.RowHandle, gridViewProductList.Columns["SubTypeStateName"]) != null)
                        && (System.Convert.ToString(gridViewProductList.GetRowCellValue(e.RowHandle, gridViewProductList.Columns["SubTypeStateName"])) == ERP_Mercury.Global.Consts.strWarningProductSubTypeStateSate))
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
                SendMessageToLog("gridViewProductList_CustomDrawCell. " + f.Message);
            }
            return;

        }
        private void gridViewProductList_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                System.Int32 iQty = System.Convert.ToInt32(gridViewProductList.GetRowCellValue(e.RowHandle, gridViewProductList.Columns["QuantityInWarehouseForShipping"]));
                System.String bIsOnlyActiveParts = System.Convert.ToString(gridViewProductList.GetRowCellValue(e.RowHandle, gridViewProductList.Columns["IsContainingOnlyPartsValid"]));
                System.Boolean bIsWarningState = ( (System.Convert.ToString(gridViewProductList.GetRowCellValue(e.RowHandle, gridViewProductList.Columns["SubTypeStateName"])) == ERP_Mercury.Global.Consts.strWarningProductSubTypeStateSate) ||
                    ((iQty == 0) && (bIsOnlyActiveParts == "Да")));

            if (bIsWarningState == true)
            {
                e.Appearance.BackColor = Color.LightPink;
            }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewProductList_RowStyle. " + f.Message);
            }
            return;

        }

        #endregion

        #region Редактирование товарной подгруппы
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddProductSubType();
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания товарной подгруппы. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void AddProductSubType()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                m_objSelectedProduct = new CProductSubType();
                LoadProductDetailInEditor(m_objSelectedProduct);

                // товары из подгруппы
                treeListProductInSubtype.Nodes.Clear();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes)
                {
                    objNode.SetValue(colPriceValue, 0);
                }


                SetReadOnlyPropertiesControls(false);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания товарной подгруппы. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void gridControlProductList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CProductSubType objSelectedProduct = GetSelectedProduct();
                LoadProductDetailInEditor(objSelectedProduct);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования товарной подгруппы. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Редактирование подгруппы
        /// </summary>
        /// <param name="objProduct">подгруппа</param>
        private void LoadProductDetailInEditor(CProductSubType objProduct)
        {
            try
            {
                if (objProduct == null) { return; }
                m_bDisableEvents = true;
                if (m_bIsComboBoxFill == false) { LoadComboBoxItems(); }

                m_objSelectedProduct = objProduct;
                lblCustomerIfo.Text = m_objSelectedProduct.Name;

                this.tableLayoutPanelDetail.SuspendLayout();

                

                txtID_Ib.Text = m_objSelectedProduct.ID_Ib.ToString();
                txtName.Text = m_objSelectedProduct.Name;
                txtProductType.Text = m_objSelectedProduct.ProductType;
                txtProductOwner.Text = m_objSelectedProduct.ProductOwner;
                calcPriceEXW.Value = System.Convert.ToDecimal( m_objSelectedProduct.PriceEXW );
                calcMargin.Value = System.Convert.ToDecimal(m_objSelectedProduct.Margin);
                calcNDS.Value = System.Convert.ToDecimal(m_objSelectedProduct.NDS);
                calcTransportTariff.Value = System.Convert.ToDecimal(m_objSelectedProduct.TransportTariff);
                calcVendorTariff.Value = System.Convert.ToDecimal(m_objSelectedProduct.VendorTariff);
                calcCustomTariff.Value = System.Convert.ToDecimal(m_objSelectedProduct.CustomsTariff);
                calcDiscont.Value = System.Convert.ToDecimal(m_objSelectedProduct.Discont);
                calcMarkUpReqiured.Value = System.Convert.ToDecimal(m_objSelectedProduct.MarkUpRequired);

                clstSheets.Items.Clear();
                clstSheets.Tag = null;
                treeListSettings.ClearNodes();
                treeListCalcPrice.ClearNodes();
                treeListCalcPrice.Tag = null;

                // товарная линия
                cboxPartLine.SelectedItem = (m_objSelectedProduct.ProductLine == null) ? null : cboxPartLine.Properties.Items.Cast<ERP_Mercury.Common.CProductLine>().Single<ERP_Mercury.Common.CProductLine>(x => x.ID.CompareTo(m_objSelectedProduct.ProductLine.ID) == 0);
                // состояние
                cboxPartSubTypeState.SelectedItem = (m_objSelectedProduct.SubTypeState == null) ? null : cboxPartSubTypeState.Properties.Items.Cast<CProductSubTypeState>().Single<CProductSubTypeState>(x => x.ID.CompareTo(m_objSelectedProduct.SubTypeState.ID) == 0);


                // товары из подгруппы
                treeListProductInSubtype.Nodes.Clear();

                if (m_objPartsList != null)
                {
                    var ProductList =
                    from Product in m_objPartsList
                    where Product.ProductSubTypeIbID == objProduct.ID_Ib
                    select Product;

                    foreach (CProduct objItem in ProductList)
                    {
                        treeListProductInSubtype.AppendNode(new object[] { objItem.ProductFullName }, null).Tag = objItem;
                    }

                }

                // цены
                m_objProductSubTypePriceList.PriceItemmList = null;
                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes )
                {
                    objNode.SetValue(colPriceValue, 0);
                }

                if (m_objSelectedProduct.ID.CompareTo(System.Guid.Empty) != 0)
                {
                    if( (m_objProductSubTypePriceList.LoadPriceList(m_objProfile, null, m_objSelectedProduct.ID) == true) &&
                        (m_objProductSubTypePriceList.PriceItemmList != null) &&
                        (m_objProductSubTypePriceList.PriceItemmList.Count > 0 ))
                    {
                        CPriceType objPriceType = null;
                        System.Boolean bExists = false;
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes)
                        {
                            bExists = false;
                            if (objNode.Tag != null)
                            {
                                objPriceType = (CPriceType)objNode.Tag;
                                foreach (CPrice objPrice in m_objProductSubTypePriceList.PriceItemmList[0].PriceList)
                                {
                                    if (objPrice.PriceType.ID.CompareTo(objPriceType.ID) == 0)
                                    {
                                        bExists = true;
                                        objNode.SetValue(colPriceValue, objPrice.PriceValue);
                                        break;
                                    }
                                }
                            }
                            if (bExists == false)
                            {
                                objNode.SetValue(colPriceValue, 0);
                            }
                        }

                        objPriceType = null;
                    }
                }

                // цены в Контракте
                gridControlPricesContract.DataSource = null;
                gridControlPricesContract.DataSource = CProductPriceListItemIB.GetPricesFromIB(m_objProfile, null, m_objSelectedProduct.ID);

                // картинка
                pictureBoxCertificate.Image = null;
                btnCertificateImageView.Enabled = false;

                pictureBoxCertificate.Image = ((m_objSelectedProduct.ExistImage == true) ? ERPMercuryPlan.Properties.Resources.Document_2_Check : ERPMercuryPlan.Properties.Resources.Document_2_Warning);
                btnCertificateImageView.Enabled = m_objSelectedProduct.ExistImage;

                this.tableLayoutPanelDetail.ResumeLayout(false);

                SetReadOnlyPropertiesControls(true);
                SetPropertiesModified(false, 0);

                tabControl.SelectedTabPage = tabDetail;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования товарной группы. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                btnCancel.Enabled = true;
            }
            return;
        }
        /// <summary>
        /// Установка режима просмотра/редактирования состава набора
        /// </summary>
        /// <param name="bReadOnly"></param>
        private void SetModeReadOnlyForProductDetail( System.Boolean bReadOnly )
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_bExistsDREditOnlyImageProductSubType == true)
                {
                    SaveOnlyImage();
                }
                else
                {
                    ConfirmProductDetail();
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSave_Click. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Сохраняет изменения в подгруппе
        /// </summary>
        private void ConfirmProductDetail()
        {
            System.Boolean bIsNew = false;
            try
            {
                if (m_objSelectedProduct != null)
                {
                    System.String strErr = "";

                    if (IsAllParamFill() == true)
                    {
                        CProductSubType objProductForSave = new CProductSubType();
                        objProductForSave.ID = m_objSelectedProduct.ID;
                        objProductForSave.ID_Ib = m_objSelectedProduct.ID_Ib;
                        objProductForSave.Name = txtName.Text;
                        objProductForSave.PriceEXW = System.Convert.ToDouble( calcPriceEXW.Value );
                        objProductForSave.ProductLine = ((cboxPartLine.SelectedItem == null) ? null : (ERP_Mercury.Common.CProductLine)cboxPartLine.SelectedItem);
                        objProductForSave.SubTypeState = (( cboxPartSubTypeState.SelectedItem == null) ? null : (CProductSubTypeState)cboxPartSubTypeState.SelectedItem);
                        objProductForSave.VendorTariff = System.Convert.ToDouble( calcVendorTariff.Value);
                        objProductForSave.TransportTariff = System.Convert.ToDouble(  calcTransportTariff.Value);
                        objProductForSave.CustomsTariff = System.Convert.ToDouble( calcCustomTariff.Value);
                        objProductForSave.Margin = System.Convert.ToDouble( calcMargin.Value);
                        objProductForSave.NDS = System.Convert.ToDouble( calcNDS.Value);
                        objProductForSave.Discont = System.Convert.ToDouble( calcDiscont.Value);
                        objProductForSave.MarkUpRequired = System.Convert.ToDouble( calcMarkUpReqiured.Value);

                        System.Boolean bIsOkSaveGeneralProperties = false;
                        if (objProductForSave.ID.CompareTo(System.Guid.Empty) == 0)
                        {
                            // новая подгруппа
                            bIsOkSaveGeneralProperties = objProductForSave.Add(m_objProfile);
                            if (bIsOkSaveGeneralProperties == true)
                            {
                                // теперь нужно добавить в IB
                                bIsOkSaveGeneralProperties = objProductForSave.AddToIB(m_objProfile, ref strErr);
                                bIsNew = true;
                                if (bIsOkSaveGeneralProperties == false)
                                {
                                    // в IB добавить не удалось, поэтому в SQL Server нужно удалить
                                    SendMessageToLog(strErr);

                                    if (objProductForSave.Remove(m_objProfile) == false)
                                    {
                                        SendMessageToLog("Ошибка сохранения изменений в свойствах товарной группы. В IB сохранить не удалось и в UniXP удалить тоже не удалось.");
                                    }

                                    objProductForSave.ID = System.Guid.Empty;
                                    objProductForSave.ID_Ib = 0;
                                }
                               
                            }
                        }
                        else
                        {
                            // уже существующая подгруппа
                            bIsOkSaveGeneralProperties = objProductForSave.UpdateInIB(m_objProfile, ref strErr );
                            if (bIsOkSaveGeneralProperties == true)
                            {
                                bIsOkSaveGeneralProperties = objProductForSave.Update(m_objProfile);
                            }
                            else
                            {
                                SendMessageToLog(strErr);
                            }
                        }
                        if (bIsOkSaveGeneralProperties == true)
                        {
                            // дополнительные свойства
                            if (objProductForSave.EditPrices(m_objProfile, null, ref strErr) == true)
                            {
                                m_objSelectedProduct.ID = objProductForSave.ID;
                                m_objSelectedProduct.ID_Ib = objProductForSave.ID_Ib;
                                m_objSelectedProduct.Name = objProductForSave.Name;
                                m_objSelectedProduct.PriceEXW = objProductForSave.PriceEXW;
                                m_objSelectedProduct.ProductLine = objProductForSave.ProductLine;

                                m_objSelectedProduct.SubTypeState = objProductForSave.SubTypeState;
                                m_objSelectedProduct.VendorTariff = objProductForSave.VendorTariff;
                                m_objSelectedProduct.TransportTariff = objProductForSave.TransportTariff;
                                m_objSelectedProduct.CustomsTariff = objProductForSave.CustomsTariff;
                                m_objSelectedProduct.Margin = objProductForSave.Margin;
                                m_objSelectedProduct.NDS = objProductForSave.NDS;
                                m_objSelectedProduct.Discont = objProductForSave.Discont;
                                m_objSelectedProduct.MarkUpRequired = objProductForSave.MarkUpRequired;

                                if (bIsNew == true)
                                {
                                    // новая подгруппа
                                    m_objProductList.Add(m_objSelectedProduct);
                                    gridControlProductList.RefreshDataSource();
                                    gridViewProductList.FocusedRowHandle = (gridViewProductList.RowCount - 1);
                                }
                                else
                                {
                                    ShowProductProperties(m_objSelectedProduct);
                                }
                                // помечаем, что основные свойства подгруппы мы сохранили
                                SetPropertiesModified(false, enTabProperties.Properties);
                                //tabControl.SelectedTabPage = tabView;
                            }
                            else
                            {
                                SendMessageToLog(strErr);
                            }


                        }

                        //проверим, не нужно ли нам сохранить изменения в ценах
                        if (IsSubtypePriceChanged == true)
                        {
                            if (SavePriceList() == true)
                            {
                                // цены тоже успешно сохранены
                                SetPropertiesModified(false, enTabProperties.Price);
                            }
                        }
                        // изображение товарной подгруппы
                        if (IsChangedImage == true)
                        {
                            System.Int32 iRes = 0;
                            if (CProductSubType.SetImageToProductSubType(m_objProfile, null, m_objSelectedProduct.ID,
                                m_objSelectedProduct.ImageProductSubType, m_objSelectedProduct.ImageProductSubTypeFileName,
                                (System.Int32)m_objSelectedProduct.ActionType, ref strErr, ref iRes) == true)
                            {
                                SetPropertiesModified(false, enTabProperties.Image);
                            }
                            else
                            {
                                SendMessageToLog(strErr);
                            }
                        }
                        // проверим список назначенных товаров
                        if (IsSubtypeProductListChanged == true)
                        {
                            if (SaveProductlist() == true)
                            {
                                SetPropertiesModified(false, enTabProperties.ProductList);
                                // 2012.06.11
                                // так как перемещение товара между группами должно привести и к изменении цен в прайсе на этот товар,
                                // то принудительно сохраняем цены
                                if (SavePriceList() == true)
                                {
                                    // цены тоже успешно сохранены
                                    SetPropertiesModified(false, enTabProperties.Price);
                                }

                                StartThreadWithLoadData();
                            }
                        }

                        objProductForSave = null;
                    }

                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в свойствах товарной группы. Текст ошибки: " + f.Message);
            }
            return;
        }

        private void SaveOnlyImage()
        {
            try
            {
                // изображение товарной подгруппы
                if ((IsChangedImage == true) && (m_objSelectedProduct != null))
                {
                    System.Int32 iRes = 0;
                    System.String strErr = System.String.Empty;

                    if (CProductSubType.SetImageToProductSubType(m_objProfile, null, m_objSelectedProduct.ID,
                        m_objSelectedProduct.ImageProductSubType, m_objSelectedProduct.ImageProductSubTypeFileName,
                        (System.Int32)m_objSelectedProduct.ActionType, ref strErr, ref iRes) == true)
                    {
                        SetPropertiesModified(false, enTabProperties.Image);
                    }
                    else
                    {
                        SendMessageToLog(strErr);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в свойствах товарной группы. Текст ошибки: " + f.Message);
            }
            return;

        }

        /// <summary>
        /// Проверяет все ли обязательные поля заполнены
        /// </summary>
        /// <returns>true - все обязательные поля заполнены; false - не все полязаполнены</returns>
        private System.Boolean IsAllParamFill()
        {
            System.Boolean bRet = false;
            try
            {
                bRet = ((cboxPartLine.SelectedItem != null) && (txtName.Text != "") && ( cboxPartSubTypeState.SelectedItem != null )  );
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки заполнения обязательных свойств товарной подгруппы. Текст ошибки: " + f.Message);
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
                cboxPartLine.Properties.Appearance.BackColor = ((cboxPartLine.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxPartSubTypeState.Properties.Appearance.BackColor = ((cboxPartSubTypeState.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtName.Properties.Appearance.BackColor = ((txtName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                
                //txtProductType.Properties.Appearance.BackColor = ((txtProductType.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                //txtProductOwner.Properties.Appearance.BackColor = ((txtProductOwner.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                
                calcPriceEXW.Properties.Appearance.BackColor = ((calcPriceEXW.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcCustomTariff.Properties.Appearance.BackColor = ((calcCustomTariff.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcDiscont.Properties.Appearance.BackColor = ((calcDiscont.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcMargin.Properties.Appearance.BackColor = ((calcMargin.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcNDS.Properties.Appearance.BackColor = ((calcNDS.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcTransportTariff.Properties.Appearance.BackColor = ((calcTransportTariff.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcVendorTariff.Properties.Appearance.BackColor = ((calcVendorTariff.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcMarkUpReqiured.Properties.Appearance.BackColor = ((calcMarkUpReqiured.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
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
        /// <summary>
        /// Запрашивает тарифы для указанной подгруппы
        /// </summary>
        /// <param name="objProductSubType">товарная подгруппа</param>
        private void ImportTarifForPartSubType(CProductSubType objProductSubType)
        {
            try
            {
                if (objProductSubType == null) { return; }
                if ((objProductSubType.ProductOwner == "") || (objProductSubType.ProductType == ""))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Для импорта тарифов необходимо, \nчтобы подгруппе был назначен хотя бы один товар.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    
                    return;
                }
                IEnumerable<CProductSubType> SubTypeList =
                from SubType in m_objProductList
                where (SubType.ProductOwner == objProductSubType.ProductOwner) && (SubType.ProductType == objProductSubType.ProductType) && (SubType.TransportTariff != 0 )
                select SubType;

                if (SubTypeList.Count<CProductSubType>() > 0)
                {
                    CProductSubType objItem = SubTypeList.First<CProductSubType>();

                    calcCustomTariff.Value = System.Convert.ToDecimal(objItem.CustomsTariff);
                    calcTransportTariff.Value = System.Convert.ToDecimal(objItem.TransportTariff);
                    calcMargin.Value = System.Convert.ToDecimal(objItem.Margin);
                    calcNDS.Value = System.Convert.ToDecimal(objItem.NDS);
                    calcDiscont.Value = System.Convert.ToDecimal(objItem.Discont);
                    calcMarkUpReqiured.Value = System.Convert.ToDecimal(objItem.MarkUpRequired);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("ImportTarifForPartSubType. Текст ошибки: " + f.Message);
            }
            return;

        }
        private void btnImportTariff_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_objSelectedProduct.ID.CompareTo(System.Guid.Empty) != 0)
                {
                    ImportTarifForPartSubType(m_objSelectedProduct);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка btnImportTariff_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        #endregion

        #region Удаление товарной подгруппы
        /// <summary>
        /// Удаление товарной подгруппы
        /// </summary>
        /// <param name="objProductSubType">Товарная подгруппа</param>
        private void DeleteProductSubType(CProductSubType objProductSubType)
        {
            if (objProductSubType == null) { return; }
            if (DevExpress.XtraEditors.XtraMessageBox.Show( "Подтвердите удаление товарной подгруппы \"" + objProductSubType.Name + "\". ", "Подтверждение",
                System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            System.String strErr = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.Int32 iDeletedObjIndx = -1;
                for( System.Int32 i=0; i < m_objProductList.Count; i++ )
                {
                   if( m_objProductList[i].ID.CompareTo(objProductSubType.ID) == 0 )
                   {
                       iDeletedObjIndx = i;
                       break;
                   }
                }
                if (iDeletedObjIndx >= 0)
                {
                    if (objProductSubType.RemoveFromIB(m_objProfile, ref strErr) == true)
                    {
                        // в IB удалить удалось, это самое сложное, теперь удалим в SQL Server
                        if (objProductSubType.Remove(m_objProfile) == true)
                        {
                            this.splitContainerControl.SuspendLayout();
                            ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                            // в БД мы всё удалили, теперь нужно удалить в списке
                            m_objProductList.RemoveAt(iDeletedObjIndx);
                            gridControlPartsNewList.RefreshDataSource();
                            if (((iDeletedObjIndx - 1) < 0) && (m_objProductList.Count > 0)) { iDeletedObjIndx = 0; }
                            else
                            {
                                if (((iDeletedObjIndx - 1) >= 0) && (m_objProductList.Count > 0)) { iDeletedObjIndx = iDeletedObjIndx--; }
                            }

                            gridViewProductList.FocusedRowHandle = iDeletedObjIndx;

                            this.splitContainerControl.ResumeLayout(false);
                            ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();

                            SendMessageToLog("Товарная подгруппа удалена.");
                            DevExpress.XtraEditors.XtraMessageBox.Show("Товарная подгруппа удалена.", "Информация",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                        }
                        else
                        {
                            SendMessageToLog("Подгруппу удалить не удалось.");
                        }

                    }
                    else
                    {
                        SendMessageToLog(strErr);
                    }
                }
                

            }
            catch (System.Exception f)
            {
                SendMessageToLog( "Ошибка удаления товарной подгруппы. Текст ошибки: " + f.Message );
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
                CProductSubType objSubType = GetSelectedProduct();
                if (objSubType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста подгруппу для удаления.", "Предупреждение",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    DeleteProductSubType(objSubType);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog( "Ошибка удаления товарной подгруппы. Текст ошибки: " + f.Message );
            }
            finally
            {
                this.Cursor = Cursors.Default;
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
                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.cboxPartLine.Properties)).BeginInit();

                cboxPartLine.Properties.Items.Clear();
                cboxPartSubTypeState.Properties.Items.Clear();
                cboxSetPartLine.Properties.Items.Clear();
                cboxSetState.Properties.Items.Clear();

                List<ERP_Mercury.Common.CProductLine> objProductLineList = ERP_Mercury.Common.CProductLine.GetProductLineList(m_objProfile, null);
                if (objProductLineList != null)
                {
                    cboxPartLine.Properties.Items.AddRange( objProductLineList );
                    cboxSetPartLine.Properties.Items.AddRange( objProductLineList );
                }

                List<CProductSubTypeState> objProductSubTypeState = CProductSubTypeState.GetProductSubTypeStateList(m_objProfile, null);
                if (objProductSubTypeState != null)
                {
                    cboxPartSubTypeState.Properties.Items.AddRange(objProductSubTypeState);
                    cboxSetState.Properties.Items.AddRange(objProductSubTypeState);
                }


                // список типов цен
                treeListPriceEditor.Nodes.Clear();
                List<CPriceType> objPriceTypeList = CPriceType.GetPriceTypeList(m_objProfile, null);
                if (objPriceTypeList != null)
                {
                    foreach (CPriceType objItem in objPriceTypeList)
                    {
                        this.treeListPriceEditor.AppendNode(new object[] { true, objItem.Name, System.String.Format("{0:### ### ##0.000}", 0) }, null).Tag = objItem;
                    }
                }
                objPriceTypeList = null;

                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.cboxPartLine.Properties)).EndInit();
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

            return ;
        }
        #endregion

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified, enTabProperties iModifiedType)
        {
            ValidateProperties();

            switch (iModifiedType)
            {
                case enTabProperties.Properties :
                    {
                        IsSubtypePropertiesChanged = bModified;
                        break;
                    }
                case enTabProperties.ProductList:
                    {
                        IsSubtypeProductListChanged = bModified;
                        break;
                    }
                case enTabProperties.Price:
                    {
                        IsSubtypePriceChanged = bModified;
                        break;
                    }
                case enTabProperties.Image:
                    {
                        IsChangedImage = bModified;
                        break;
                    }
                default:
                    {
                        IsSubtypePropertiesChanged = bModified;
                        IsSubtypeProductListChanged = bModified;
                        IsSubtypePriceChanged = bModified;
                        IsChangedImage = bModified;
                        break;
                    }
            }
            // в том случае, если хоть одна закладка изменялась, делаем проверкку на возможность сохранения
            if (IsSubtypePriceChanged || IsSubtypeProductListChanged || IsSubtypePropertiesChanged || IsChangedImage)
            {
                btnSave.Enabled = IsAllParamFill();
                btnCancel.Text = m_strBtnCancelName;
                btnCancel.Enabled = true;
            }
            else if ((IsSubtypePriceChanged == false) && (IsSubtypeProductListChanged == false) && (IsSubtypePropertiesChanged == false) && (IsChangedImage == false))
            {
                btnSave.Enabled = false;
                btnCancel.Text = m_strBtnExitName;
                btnCancel.Enabled = true;
            }
            tabSubtypeProperties.Image = (((IsSubtypePropertiesChanged == true) || (IsChangedImage == true)) ? ERPMercuryPlan.Properties.Resources.warning : null);
            tabSubtypeProductList.Image = ((IsSubtypeProductListChanged == true) ? ERPMercuryPlan.Properties.Resources.warning : null);
            tabPriceList.Image = ((IsSubtypePriceChanged == true) ? ERPMercuryPlan.Properties.Resources.warning : null);

            //btnCancel.Enabled = (IsSubtypePriceChanged || IsSubtypeProductListChanged || IsSubtypePropertiesChanged);
        }
        /// <summary>
        /// Включает/отключает элементы управления для отображения свойств адреса
        /// </summary>
        /// <param name="bSetReadOnlyMode">признак "включить/выключить" режим "только просмотр"</param>
        private void SetReadOnlyPropertiesControls(System.Boolean bSetReadOnlyMode)
        {
            try
            {
                cboxPartLine.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ( ( m_bExistsDREditProductSubType == true ) ? false : true ));
                cboxPartSubTypeState.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));
                txtID_Ib.Properties.ReadOnly = true;
                txtName.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));
                txtProductType.Properties.ReadOnly = true;
                txtProductOwner.Properties.ReadOnly = true;
                calcPriceEXW.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));

                calcCustomTariff.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));
                calcDiscont.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));
                calcMargin.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));
                calcNDS.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));
                calcVendorTariff.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));
                calcTransportTariff.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));
                calcMarkUpReqiured.Properties.ReadOnly = ((bSetReadOnlyMode == true) ? true : ((m_bExistsDREditProductSubType == true) ? false : true));

                btnDeleteProduct.Enabled = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));
                btnAddProduct.Enabled = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));
                treeListPriceEditor.OptionsBehavior.Editable = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));
                treeListCalcPriceList.Enabled = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));
                treeListCalcPrice.Enabled = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));
                btnCalcPrice.Enabled = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));
                btnSavePriceList.Enabled = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));

                clstSheets.Enabled = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));
                treeListSettings.Enabled = ((bSetReadOnlyMode == true) ? false : ((m_bExistsDREditProductSubType == true) ? true : false));

                btnCertificateImageView.Enabled = ((m_objSelectedProduct != null) && (m_objSelectedProduct.ExistImage == true));
                btnCertificateImageLoad.Enabled = ((bSetReadOnlyMode == true) ? false : (((m_bExistsDREditProductSubType == true) || (m_bExistsDREditOnlyImageProductSubType == true)) ? true : false));
                btnCertificateImageClear.Enabled = ((bSetReadOnlyMode == true) ? false : (((m_bExistsDREditProductSubType == true) || (m_bExistsDREditOnlyImageProductSubType == true)) ? true : false));

                if (bSetReadOnlyMode == true)
                {
                    // включен режим "только просмотр"
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                }
                else
                {
                    // включен режим "редактирование"
                    btnSave.Enabled = IsAllParamFill();
                    btnCancel.Enabled = true;
                }

                btnEdit.Enabled = bSetReadOnlyMode;
                btnSetToPartList.Enabled = ((bSetReadOnlyMode == true) ? false : (((m_bExistsDREditProductSubType == true) && (m_objSelectedProduct.ID.CompareTo(System.Guid.Empty) != 0)) ? true : false));// (!bSetReadOnlyMode) && (m_objSelectedProduct.ID.CompareTo(System.Guid.Empty) != 0);
                btnImportTariff.Enabled = (btnSetToPartList.Enabled && (m_objSelectedProduct.ProductOwner != "") && (m_objSelectedProduct.ProductType != "") && (m_bExistsDREditProductSubType == true));
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
                if (e.NewValue != null)
                {
                    if ((sender == calcPriceEXW) ) 
                    {
                        if ((sender == calcPriceEXW) || (sender == calcVendorTariff) || 
                            (sender == calcCustomTariff) || (sender == calcDiscont) || 
                            (sender == calcMargin) || (sender == calcNDS) ||
                            (sender == calcTransportTariff) || (sender == calcMarkUpReqiured))
                        {
                            if( (System.Convert.ToDecimal(e.NewValue) < 0) || (System.Convert.ToDecimal(e.NewValue) > 100 ) )
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            if (System.Convert.ToDecimal(e.NewValue) < 0)
                            {
                                e.Cancel = true;
                            }
                        }
                    }

                    SetPropertiesModified(true, enTabProperties.Properties);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств товарной подгруппы.\nТекст ошибки: " + f.Message, "Ошибка",
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
                SetPropertiesModified(true, enTabProperties.Properties);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств товарной подгруппы.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Экспорт в MS Excel
        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            //string name = Application.ProductName;
            string name = "Товарные подгруппы";
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Экспорт списка товарных подгрупп " + title;
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
                DevExpress.XtraGrid.Views.Grid.GridView objGridView = gridViewProductList;

                if ((objGridView == null) || (objGridView.RowCount == 0))
                {
                    return;
                }
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xls");
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

        #region Назначение свойств списку подгрупп
        private void checkPartLine_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender == checkCustomTariff) { calcSetCustomTarif.Enabled = (checkCustomTariff.CheckState == CheckState.Checked); }
                if (sender == checkDiscont) { calcSetDiscont.Enabled = (checkDiscont.CheckState == CheckState.Checked); }
                if (sender == checkMargin) { calcSetMargin.Enabled = (checkMargin.CheckState == CheckState.Checked); }
                if (sender == checkNDS) { calcSetNDS.Enabled = (checkNDS.CheckState == CheckState.Checked); }
                if (sender == checkPartLine) { cboxSetPartLine.Enabled = (checkPartLine.CheckState == CheckState.Checked); }
                if (sender == checkState) { cboxSetState.Enabled = (checkState.CheckState == CheckState.Checked); }
                if (sender == checkPriceExW) { calcSetPriceExw.Enabled = (checkPriceExW.CheckState == CheckState.Checked); }
                if (sender == checkTransportTariff) { calcSetTransportTarif.Enabled = (checkTransportTariff.CheckState == CheckState.Checked); }
                if (sender == checkVendorTariff) { calcSetVendorTarif.Enabled = (checkVendorTariff.CheckState == CheckState.Checked); }
                if (sender == checkMarkUpRequierd) { calcMarkUpReqiured.Enabled = (checkMarkUpRequierd.CheckState == CheckState.Checked); }

                btnSetPrprtsToList.Enabled = IsSelectedParamForList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("CheckStateChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private System.Boolean IsSelectedParamForList()
        {
            System.Boolean bRet = false;
            try
            {
                bRet = (
                    (checkCustomTariff.CheckState == CheckState.Checked) || (checkDiscont.CheckState == CheckState.Checked) ||
                    (checkMargin.CheckState == CheckState.Checked) || (checkNDS.CheckState == CheckState.Checked) ||
                    (checkPartLine.CheckState == CheckState.Checked) || (checkState.CheckState == CheckState.Checked) ||
                    (checkPriceExW.CheckState == CheckState.Checked) || (checkTransportTariff.CheckState == CheckState.Checked) ||
                    (checkVendorTariff.CheckState == CheckState.Checked) || ( checkMarkUpRequierd.CheckState == CheckState.Checked)
                    );
            }
            catch (System.Exception f)
            {
                SendMessageToLog("IsSelectedParamForList. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;

        }

        private void SetPrimaryPropertiesForSetPnl()
        {
            try
            {
                checkCustomTariff.CheckState = CheckState.Unchecked;
                checkDiscont.CheckState = CheckState.Unchecked;
                checkMargin.CheckState = CheckState.Unchecked;
                checkNDS.CheckState = CheckState.Unchecked;
                checkPartLine.CheckState = CheckState.Unchecked;
                checkState.CheckState = CheckState.Unchecked;
                checkPriceExW.CheckState  = CheckState.Unchecked;
                checkTransportTariff.CheckState  = CheckState.Unchecked;
                checkVendorTariff.CheckState = CheckState.Unchecked;
                checkMarkUpRequierd.CheckState = CheckState.Unchecked;

                calcSetCustomTarif.Value = 0;
                calcSetDiscont.Value = 0;
                calcSetMargin.Value = 0;
                calcSetNDS.Value = 0;
                cboxSetPartLine.SelectedItem = null;
                cboxSetState.SelectedItem = null;
                calcSetPriceExw.Value = 0;
                calcSetTransportTarif.Value = 0;
                calcSetVendorTarif.Value = 0;
                calcMarkUpReqiured.Value = 0;

                calcSetCustomTarif.Enabled = (checkCustomTariff.CheckState == CheckState.Checked);
                calcSetDiscont.Enabled = (checkDiscont.CheckState == CheckState.Checked);
                calcSetMargin.Enabled = (checkMargin.CheckState == CheckState.Checked);
                calcSetNDS.Enabled = (checkNDS.CheckState == CheckState.Checked);
                cboxSetPartLine.Enabled = (checkPartLine.CheckState == CheckState.Checked);
                cboxSetState.Enabled = (checkState.CheckState == CheckState.Checked);
                calcSetPriceExw.Enabled = (checkPriceExW.CheckState == CheckState.Checked);
                calcSetTransportTarif.Enabled = (checkTransportTariff.CheckState == CheckState.Checked);
                calcSetVendorTarif.Enabled = (checkVendorTariff.CheckState == CheckState.Checked);
                calcSetMarkUpRequired.Enabled = (checkMarkUpRequierd.CheckState == CheckState.Checked);


                btnSetPrprtsToList.Enabled = IsSelectedParamForList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("CheckStateChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private System.Boolean bSetPropertiesToSubTypeList()
        {
            System.Boolean bRet = false;
            System.String strErr = "";
            if (IsSelectedParamForList() == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                     "Необходимо выбрать хотя бы один параметр, который будет назначен списку подгрупп.", "Внимание",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return bRet;

            }
            try
            {
                List<CProductSubType> objSubTypeList = new List<CProductSubType>();
                if (radioGroupSet.SelectedIndex == 0)
                {
                    // все записи
                    for (System.Int32 i = 0; i < gridViewProductList.RowCount; i++)
                    {
                        objSubTypeList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(i)]);
                    }
                }
                else
                {
                    // только выделенные
                    int[] arr = gridViewProductList.GetSelectedRows();
                    if (arr.Length > 0)
                    {
                        for (System.Int32 i = 0; i < arr.Length; i++)
                        {
                            objSubTypeList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(arr[i])]);
                        }
                    }
                }
                if (objSubTypeList.Count > 0)
                {
                    // список на изменение не пустой
                    CProductSubTypeState objProductSubTypeStateSave = (((checkState.CheckState == CheckState.Checked) && (cboxSetState.SelectedItem != null)) ? (CProductSubTypeState)cboxSetState.SelectedItem : null);
                    CProductLine objProductLineSave = (( ( checkPartLine.CheckState == CheckState.Checked ) && (cboxSetPartLine.SelectedItem != null) ) ? (CProductLine)cboxSetPartLine.SelectedItem : null );
                    System.Double dblVendorTariffSave = (((checkVendorTariff.CheckState == CheckState.Checked) && (calcSetVendorTarif.Value >= 0)) ? (System.Convert.ToDouble(calcSetVendorTarif.Value)) : -1);
                    System.Double dblTransportTariffSave = (((checkTransportTariff.CheckState == CheckState.Checked) && (calcSetTransportTarif.Value >= 0)) ? (System.Convert.ToDouble(calcSetTransportTarif.Value)) : -1);
                    System.Double dblCustomsTariffSave = (((checkCustomTariff.CheckState == CheckState.Checked) && (calcSetCustomTarif.Value >= 0)) ? (System.Convert.ToDouble(calcSetCustomTarif.Value)) : -1);
                    System.Double dblMarginSave = (((checkMargin.CheckState == CheckState.Checked) && (calcSetMargin.Value >= 0)) ? (System.Convert.ToDouble(calcSetMargin.Value)) : -1);
                    System.Double dblNDSSave = (((checkNDS.CheckState == CheckState.Checked) && (calcSetNDS.Value >= 0)) ? (System.Convert.ToDouble(calcSetNDS.Value)) : -1);
                    System.Double dblDiscontSave = (((checkDiscont.CheckState == CheckState.Checked) && (calcSetDiscont.Value >= 0)) ? (System.Convert.ToDouble(calcSetDiscont.Value)) : -1);
                    System.Double dblPriceEXWSave = (((checkPriceExW.CheckState == CheckState.Checked) && (calcSetPriceExw.Value >= 0)) ? (System.Convert.ToDouble(calcSetPriceExw.Value)) : -1);
                    System.Double dblMarkUprequired = ((( checkMarkUpRequierd.CheckState == CheckState.Checked) && (calcSetMarkUpRequired.Value >= 0)) ? (System.Convert.ToDouble(calcSetMarkUpRequired.Value)) : -1);

                    System.Boolean bOkSaveIB = true;
                    if (objProductLineSave != null)
                    {
                        // в IB массово меняется только товарная линия, больше в справочник подгрупп передавать нечего 
                        // поэтому мы сперва попробуем сохранить информацию в IB, а потом уже в SQL Server
                        bOkSaveIB = CProductSubType.SetPropertiesToSubTypeListInIB(m_objProfile, objSubTypeList, objProductLineSave, ref strErr);
                        if (bOkSaveIB == false)
                        {
                            SendMessageToLog(strErr);
                        }
                    }
                    System.Boolean bOkSave = false;
                    if (bOkSaveIB == true)
                    {
                        CProductSubType.SetPropertiesToSubTypeList(m_objProfile, objSubTypeList, objProductLineSave, objProductSubTypeStateSave,
                            dblVendorTariffSave, dblTransportTariffSave, dblCustomsTariffSave, dblMarginSave, dblNDSSave, dblDiscontSave, 
                            dblPriceEXWSave, dblMarkUprequired);
                    }

                    if (bOkSave == true)
                    {
                        this.splitContainerControl.SuspendLayout();
                        ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                        foreach (CProductSubType objSave in objSubTypeList)
                        {
                            if (objProductSubTypeStateSave != null) { objSave.SubTypeState = objProductSubTypeStateSave; }
                            if (objProductLineSave != null) { objSave.ProductLine = objProductLineSave; }
                            if (dblVendorTariffSave >= 0) { objSave.VendorTariff = dblVendorTariffSave; }
                            if (dblTransportTariffSave >= 0) { objSave.TransportTariff = dblTransportTariffSave; }
                            if (dblCustomsTariffSave >= 0) { objSave.CustomsTariff = dblCustomsTariffSave; }
                            if (dblMarginSave >= 0) { objSave.Margin = dblMarginSave; }
                            if (dblNDSSave >= 0) { objSave.NDS = dblNDSSave; }
                            if (dblDiscontSave >= 0) { objSave.Discont = dblDiscontSave; }
                            if (dblPriceEXWSave >= 0) { objSave.PriceEXW = dblPriceEXWSave; }
                            if (dblMarkUprequired >= 0) { objSave.MarkUpRequired = dblMarkUprequired; }
                        }

                        gridControlProductList.RefreshDataSource();

                        this.splitContainerControl.ResumeLayout(false);
                        ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();

                        bRet = true;
                    }

                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("bSetPropertiesToSubTypeList. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;

        }

        private void btnSetPrprtsToList_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewProductList.RowCount == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                         "Список подгрупп не должен быть пустым.", "Внимание",
                         System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return ;
                }
                int[] arr = gridViewProductList.GetSelectedRows();
                if ((radioGroupSet.SelectedIndex > 0) && (arr.Length == 0))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                         "Для заданного режима нужно выделить хотя бы одну запись.", "Внимание",
                         System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                if (bSetPropertiesToSubTypeList() == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                         "Операция завершена успешно!", "Внимание",
                         System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("IsSelectedParamForList. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return ;

        }

        #endregion

        #region Присвоить подгруппу списку товаров
        private void SetProductSubTypeToPartList( CProductSubType objProductSubType )
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
                            if (DevExpress.XtraEditors.XtraMessageBox.Show( "Внимание!\nСписок товаров, у которых будет изменена подгруппа, содержит " + m_objFrmPartsList.SelectedProductList.Count.ToString() + " позиций.\n" +
                                "Пожалуйста, подтвердите начало операции.", "Информация",  System.Windows.Forms.MessageBoxButtons.YesNo, 
                                System.Windows.Forms.MessageBoxIcon.Warning) == DialogResult.No)
                            {
                                return;
                            }
                        }

                        // теперь добавляем позиции в прайс-лист
                         System.String strErr = "";
                         System.Boolean bSaveOkIB = CProductSubType.SetPartSubTypeToPartsInIB(m_objProfile, m_objFrmPartsList.SelectedProductList, objProductSubType, ref strErr);
                         if (bSaveOkIB == true)
                         {
                             if (CProductSubType.SetPartSubTypeToParts(m_objProfile, m_objFrmPartsList.SelectedProductList, objProductSubType) == true)
                             {
                                 SendMessageToLog("Товарная подгруппа успешно присвоена списку товаров");
                                 DevExpress.XtraEditors.XtraMessageBox.Show(
                                     "Товарная подгруппа успешно присвоена списку товаров.", "Информация",
                                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                             }
                         }
                         else
                         {
                             SendMessageToLog( strErr );
                         }
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка SetProductSubTypeToPartList. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        private void btnSetToPartList_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_objSelectedProduct.ID.CompareTo(System.Guid.Empty) != 0)
                {
                    SetProductSubTypeToPartList(m_objSelectedProduct);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка btnSetToPartList_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Выгрузить данные в шаблон MS Excel
        private void btnAddToPriceCalcInExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                //AddPartSubTypeListToExcel();

                AddPartSubTypeListPriceListCalc();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка экспорта в MS Excel. Текст ошибки: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }
        private void AddPartSubTypeListToExcel()
        {
            CSettingForCalcPrice objSettingForCalcPrice = null;
            List<CProductSubType> objSubTypeList = null;

            try
            {
                System.String strFileName = "";
                System.Int32 iSheetNum = 0;
                System.Double dblCurRatePricing = 0;
                

                frmExportToExcelDialog objFrmExportToExcelDialog = new frmExportToExcelDialog(m_objProfile);
                DialogResult dlgRes = objFrmExportToExcelDialog.ShowDialog();
                if (dlgRes == DialogResult.OK)
                {
                    strFileName = objFrmExportToExcelDialog.FileFullName;
                    iSheetNum = objFrmExportToExcelDialog.SheetNumber;
                    dblCurRatePricing = objFrmExportToExcelDialog.CurrentPriceCreateRate;
                    objSettingForCalcPrice = objFrmExportToExcelDialog.SelectedSettingForCalcPrice;
                }
                objFrmExportToExcelDialog.Dispose();
                objFrmExportToExcelDialog = null;

                if ((strFileName != "") && (iSheetNum >= 1))
                {

                    objSubTypeList = new List<CProductSubType>();
                    int[] arr = gridViewProductList.GetSelectedRows();
                    if (arr.Length >= 1)
                    {
                        // выделено больше одной записи, так что считаем, что в Excel передаем только выбранный список
                        for (System.Int32 i = 0; i < arr.Length; i++)
                        {
                            objSubTypeList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(arr[i])]);
                        }
                    }
                    else
                    {
                        // все видимые записи
                        for (System.Int32 i = 0; i < gridViewProductList.RowCount; i++)
                        {
                            objSubTypeList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(i)]);
                        }
                    }
                    if (objSubTypeList.Count == 0)
                    {
                        this.Cursor = Cursors.Default;
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                                            "Не удалось получить список записей для экспорта в MS Excel.", "Внимание",
                                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return;
                    }

                    AddPartSubTypeListToExcel(objSubTypeList, strFileName, iSheetNum, dblCurRatePricing, objSettingForCalcPrice);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка экспорта в MS Excel. Текст ошибки: " + f.Message);
            }
            finally
            {
                objSettingForCalcPrice = null;
                objSubTypeList = null;
            }
            return;
        }

        private void btnExportForCalcPrice_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewProductList.RowCount == 0) { return; }
                StartExportToExcel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка экспорта в MS Excel. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }
        /// <summary>
        /// Выбор файла и листа
        /// </summary>
        private void StartExportToExcel()
        {
            try
            {
                System.String strFileName = "";
                System.Int32 iSheetNum = 0;
                System.Double dblCurRatePricing = 0;
                CSettingForCalcPrice objSettingForCalcPrice = null;

                frmExportToExcelDialog objFrmExportToExcelDialog = new frmExportToExcelDialog( m_objProfile );
                DialogResult dlgRes = objFrmExportToExcelDialog.ShowDialog();
                if (dlgRes == DialogResult.OK)
                {
                    strFileName = objFrmExportToExcelDialog.FileFullName;
                    iSheetNum = objFrmExportToExcelDialog.SheetNumber;
                    dblCurRatePricing = objFrmExportToExcelDialog.CurrentPriceCreateRate;
                    objSettingForCalcPrice = objFrmExportToExcelDialog.SelectedSettingForCalcPrice;
                }
                objFrmExportToExcelDialog.Dispose();
                objFrmExportToExcelDialog = null;

                if ((strFileName != "") && (iSheetNum >= 1))
                {
                    ExportPartSubTypeListToExcel(strFileName, iSheetNum, dblCurRatePricing, objSettingForCalcPrice);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка экспорта в MS Excel. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Экспорт подгрупп в файл расчета цен
        /// </summary>
        /// <param name="strFileName">Полное имя файла</param>
        /// <param name="iSheetNum">№ листа</param>
        /// <param name="dblCurrentPriceCreateRate">Курс ценообразования</param>
        /// <param name="objSettingForCalcPrice">Настройки</param>
        private void ExportPartSubTypeListToExcel(System.String strFileName, System.Int32 iSheetNum, System.Double dblCurrentPriceCreateRate, CSettingForCalcPrice objSettingForCalcPrice)
        {
            if ((strFileName == "") || (iSheetNum < 1)) { return; }
            
            Excel.Application oXL = null;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            System.Int32 iStartRow = objSettingForCalcPrice.GetColumnNumForParam( CSettingForCalcPrice.strParamNameStartRow );
            System.Int32 iCurrentRow = iStartRow;
            System.Int32 iOrderNum = 0;
            object m = Type.Missing;
            
            
            List<CProductSubType> objSubTypeList = new List<CProductSubType>();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                int[] arr = gridViewProductList.GetSelectedRows();
                if (arr.Length >= 1)
                {
                    // выделено больше одной записи, так что считаем, что в Excel передаем только выбранный список
                    for (System.Int32 i = 0; i < arr.Length; i++)
                    {
                        objSubTypeList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(arr[i])]);
                    }
                }
                else
                {
                    // все видимые записи
                    for (System.Int32 i = 0; i < gridViewProductList.RowCount; i++)
                    {
                        objSubTypeList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(i)]);
                    }
                }
                if (objSubTypeList.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                                        "Не удалось получить список записей для экспорта в MS Excel.", "Внимание",
                                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning); 
                    return;
                }
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                oSheet = (Excel._Worksheet)oWB.Worksheets[ iSheetNum ];

                System.String strProductOwnerCurrent = "";
                System.String strProductTypeCurrent = "";

                System.Int32 iColumnProductOwner = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductOwner);
                System.Int32 iColumnProductSubType = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubType);
                System.Int32 iColumnProductSubTypeId = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubTypeID);
                System.Int32 iColumnProductSubTypeState = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubTypeState);
                System.Int32 iColumnVendorTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameVendorTarif);
                System.Int32 iColumnTransportTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameTransportTarif);
                System.Int32 iColumnTransportTarifSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameTransportTarifSum);
                System.Int32 iColumnCustomTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCustomTarif);
                System.Int32 iColumnCustomTarifSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCustomTarifSum);
                System.Int32 iColumnMargin = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameMargin);
                System.Int32 iColumnNDS = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameNDS);
                System.Int32 iColumnNDSSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameNDSSum);
                System.Int32 iColumnDiscont = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameDiscont);
                System.Int32 iColumnCurrRate = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCurRate);
                System.Int32 iColumnMarkUpReqiured = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameMarkUpReqiured);

                foreach (CProductSubType objProductSubType in objSubTypeList)
                {
                    if (((objProductSubType.ProductOwner != "") && (objProductSubType.ProductOwner != strProductOwnerCurrent)) ||
                        ((objProductSubType.ProductType != "") && (objProductSubType.ProductType != strProductTypeCurrent)))
                    {
                        oSheet.Cells[iCurrentRow, iColumnProductOwner] = objProductSubType.ProductOwner;
                        oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.ProductType;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Underline = true;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Italic = true;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Size = 14;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Bold = true;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.PatternColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.Color = 12632256;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, 3], oSheet.Cells[iCurrentRow, 100]).ClearContents();
                        strProductOwnerCurrent = objProductSubType.ProductOwner;
                        strProductTypeCurrent = objProductSubType.ProductType;
                        //oSheet.get_Range(oSheet.Cells[iCurrentRow + 1, 1], oSheet.Cells[iCurrentRow + 1, 1]).EntireRow.Insert(null, null);
                        iCurrentRow++;
                        iOrderNum++;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Font.Underline = false;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Font.Italic = false;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Font.Size = 12;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Font.Bold = false;
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Interior.Pattern = Excel.XlPattern.xlPatternNone;
                    }

                    oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.Name;
                    oSheet.Cells[iCurrentRow, iColumnProductSubTypeId] = objProductSubType.ID_Ib;
                    oSheet.Cells[iCurrentRow, iColumnProductSubTypeState] = objProductSubType.SubTypeStateName;
                    // тариф
                    oSheet.Cells[iCurrentRow, iColumnVendorTarif] = objProductSubType.VendorTariff;
                    // % расходов на транспорт
                    oSheet.Cells[iCurrentRow, iColumnTransportTarif] = objProductSubType.TransportTariff;
                    //Сумма расходов на транспорт
                    //oSheet.Cells[iCurrentRow, 7] = oSheet.Cells[iCurrentRow-1, 7]; 
                    // Таможенная пошлина, %
                    oSheet.Cells[iCurrentRow, iColumnCustomTarif] = objProductSubType.CustomsTariff;
                    // Сумма расходов на таможню
                    //oSheet.Cells[iCurrentRow, 9] = oSheet.Cells[iCurrentRow-1, 9]; 
                    // Маржа, %
                    oSheet.Cells[iCurrentRow, iColumnMargin] = objProductSubType.Margin;
                    // Ставка НДС, %
                    oSheet.Cells[iCurrentRow, iColumnNDS] = objProductSubType.NDS;
                    // Сумма НДС
                    //oSheet.Cells[iCurrentRow, 12] = oSheet.Cells[iCurrentRow-1, 12];
                    // Курс ценообразования (текущий)
                    oSheet.Cells[iCurrentRow, iColumnCurrRate] = dblCurrentPriceCreateRate;
                    // Дисконт, %
                    oSheet.Cells[iCurrentRow, iColumnDiscont] = objProductSubType.Discont;
                    // требуемая наценка, %
                    oSheet.Cells[iCurrentRow, iColumnMarkUpReqiured] = objProductSubType.MarkUpRequired;
                    

                    iCurrentRow++;
                }

                System.Int32 iRowCount = oSheet.Cells.Rows.Count;
                oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iRowCount - iCurrentRow, 100]).ClearContents();

                ((Excel._Worksheet)oWB.Worksheets[1]).Activate();
                oXL.Visible = true;
                //oXL.UserControl = true;
            }
            catch (System.Exception f)
            {
                oXL = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                objSubTypeList = null;
                oSheet = null;
                oWB = null;
                oXL = null;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Добавление списка подгрупп в уже существующий файл расчета
        /// </summary>
        /// <param name="objSubTypeList">список подгрупп</param>
        /// <param name="strFileName">файл расчета</param>
        /// <param name="iSheetNum">номер листа</param>
        /// <param name="dblCurrentPriceCreateRate">курс ценообразования</param>
        /// <param name="objSettingForCalcPrice">настройки</param>
        private void AddPartSubTypeListToExcel(List<CProductSubType> objSubTypeList, 
            System.String strFileName, System.Int32 iSheetNum, System.Double dblCurrentPriceCreateRate, CSettingForCalcPrice objSettingForCalcPrice)
        {
            if ((strFileName == "") || (iSheetNum < 1)) { return; }
            if ((objSubTypeList == null) || (objSubTypeList.Count == 0)) { return; }

            Excel.Application oXL = null;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            System.Int32 iStartRow = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameStartRow);
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;


            try
            {
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                oSheet = (Excel._Worksheet)oWB.Worksheets[iSheetNum];

                System.String strProductOwnerCurrent = "";
                System.String strProductTypeCurrent = "";
                System.Boolean bStopRead = false;
                System.String strTmp = "";
                System.String strPartSubTypeId = "";
                System.Boolean bFindProductGroup = false;
                System.Boolean bDoubling = false;
                System.Int32 iNumOfRowForCopy = 0;
                System.Boolean bNeedAdd = false;
                System.Boolean bPartSubTypeExistInSheet = false;

                System.Int32 iColumnProductOwner = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductOwner);
                System.Int32 iColumnProductSubType = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubType);
                System.Int32 iColumnProductSubTypeId = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubTypeID);
                System.Int32 iColumnProductSubTypeState = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubTypeState);
                System.Int32 iColumnVendorTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameVendorTarif);
                System.Int32 iColumnTransportTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameTransportTarif);
                System.Int32 iColumnTransportTarifSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameTransportTarifSum);
                System.Int32 iColumnCustomTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCustomTarif);
                System.Int32 iColumnCustomTarifSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCustomTarifSum);
                System.Int32 iColumnMargin = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameMargin);
                System.Int32 iColumnNDS = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameNDS);
                System.Int32 iColumnNDSSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameNDSSum);
                System.Int32 iColumnDiscont = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameDiscont);
                System.Int32 iColumnCurrRate = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCurRate);
                System.Int32 iColumnMarkUpReqiured = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameMarkUpReqiured);

                foreach (CProductSubType objProductSubType in objSubTypeList)
                {
                    bFindProductGroup = false;
                    strTmp = "";
                    bStopRead = false;
                    bDoubling = false;
                    iNumOfRowForCopy = 0;
                    bNeedAdd = false;
                    bPartSubTypeExistInSheet = false;

                    iCurrentRow = iStartRow;
                    while (bStopRead == false)
                    {
                        strTmp = System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubType], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Value2);
                        strPartSubTypeId = System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubTypeId], oSheet.Cells[iCurrentRow, iColumnProductSubTypeId]).Value2);

                        if (strPartSubTypeId == objProductSubType.ID_Ib.ToString())
                        {
                            bStopRead = true;
                            bNeedAdd = false;
                            bPartSubTypeExistInSheet = true;
                        }
                        if (strTmp == "")
                        {
                            if ((strProductTypeCurrent != objProductSubType.ProductType) && (bFindProductGroup == false))
                            {
                                // мы прошли весь список и нужной группы не нашли
                                oSheet.Cells[iCurrentRow, iColumnProductOwner] = objProductSubType.ProductOwner;
                                oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.ProductType;

                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Underline = true;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Italic = true;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Size = 14;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Bold = true;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.PatternColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.Color = 12632256;

                                strProductTypeCurrent = objProductSubType.ProductType;
                                bFindProductGroup = true;
                                iCurrentRow++;
                                iNumOfRowForCopy = iCurrentRow - 2;
                                bNeedAdd = true;

                                //iCurrentRow = iStartRow;
                            }
                            else 
                            {
                               // мы прошли весь список
                                bStopRead = true;
                            }

                        }
                        else
                        {
                            //в одном столбце с подгруппами идет название группы (сверху)
                            if (System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubTypeId], oSheet.Cells[iCurrentRow, iColumnProductSubTypeId]).Value2) == "")
                            {
                                // в ячейке название группы
                                strProductTypeCurrent = strTmp;
                                if (System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductOwner]).Value2) != strProductOwnerCurrent)
                                {
                                    strProductOwnerCurrent = System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductOwner]).Value2);
                                }
                            }

                            if (strProductTypeCurrent == objProductSubType.ProductType)
                            {
                                // группу нашли
                                bFindProductGroup = true;
                            }
                            else
                            {
                                if ((strProductTypeCurrent != objProductSubType.ProductType) && (bFindProductGroup == true) && (bDoubling == false))
                                {
                                    // получается, что мы только что закончили просматривать подгруппы нужной нам группы и не нашли искомой подгруппы
                                    // самое время добавлять строку
                                    iNumOfRowForCopy = iCurrentRow - 1;
                                    bNeedAdd = true;
                                }
                            }
                            if (strTmp == objProductSubType.Name) { bDoubling = true; }
                        }


                        if (bStopRead == true)
                        {
                            // мы прошли все записи
                            if (bDoubling == false)
                            {
                                iNumOfRowForCopy = iCurrentRow - 1;
                                bNeedAdd = true;
                                // дублирующихся записей нет
                                if (bFindProductGroup == false)
                                {
                                    iNumOfRowForCopy = iCurrentRow - 1;
                                    // нужная группа не найдена,  придется добавить название группы
                                    if (strProductOwnerCurrent != objProductSubType.ProductOwner)
                                    {
                                        oSheet.Cells[iCurrentRow, iColumnProductOwner] = objProductSubType.ProductOwner;
                                        oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.ProductType;
                                    }
                                    iCurrentRow++;
                                }
                            }
                        }

                        if (bNeedAdd == true)
                        {
                            oSheet.get_Range(oSheet.Cells[iNumOfRowForCopy, 1], oSheet.Cells[iNumOfRowForCopy, 200]).Copy(Missing.Value);
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 1]).Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubType], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.ColorIndex = 3;
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnVendorTarif], oSheet.Cells[iCurrentRow, iColumnVendorTarif]).Interior.Pattern = Excel.XlPattern.xlPatternNone;
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnTransportTarif], oSheet.Cells[iCurrentRow, iColumnTransportTarif]).Interior.Pattern = Excel.XlPattern.xlPatternNone;
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnCustomTarif], oSheet.Cells[iCurrentRow, iColumnCustomTarif]).Interior.Pattern = Excel.XlPattern.xlPatternNone;

                            oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.Name;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeId] = objProductSubType.ID_Ib;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeState] = objProductSubType.SubTypeStateName;
                            // тариф
                            oSheet.Cells[iCurrentRow, iColumnVendorTarif] = objProductSubType.VendorTariff;
                            if (objProductSubType.VendorTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnVendorTarif], oSheet.Cells[iCurrentRow, iColumnVendorTarif]).Interior.ColorIndex = 3;
                            }
                            // % расходов на транспорт
                            oSheet.Cells[iCurrentRow, iColumnTransportTarif] = objProductSubType.TransportTariff;
                            if (objProductSubType.TransportTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnTransportTarif], oSheet.Cells[iCurrentRow, iColumnTransportTarif]).Interior.ColorIndex = 3;
                            }
                            //Сумма расходов на транспорт
                            //oSheet.Cells[iCurrentRow, 7] = oSheet.Cells[iCurrentRow-1, 7]; 
                            // Таможенная пошлина, %
                            oSheet.Cells[iCurrentRow, iColumnCustomTarif] = objProductSubType.CustomsTariff;
                            if (objProductSubType.CustomsTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnCustomTarif], oSheet.Cells[iCurrentRow, iColumnCustomTarif]).Interior.ColorIndex = 3;
                            }
                            // Сумма расходов на таможню
                            //oSheet.Cells[iCurrentRow, 9] = oSheet.Cells[iCurrentRow-1, 9]; 
                            // Маржа, %
                            oSheet.Cells[iCurrentRow, iColumnMargin] = objProductSubType.Margin;
                            // Ставка НДС, %
                            oSheet.Cells[iCurrentRow, iColumnNDS] = objProductSubType.NDS;
                            // Сумма НДС
                            //oSheet.Cells[iCurrentRow, 12] = oSheet.Cells[iCurrentRow-1, 12];
                            // Курс ценообразования (текущий)
                            oSheet.Cells[iCurrentRow, iColumnCurrRate] = dblCurrentPriceCreateRate;
                            // Дисконт, %
                            oSheet.Cells[iCurrentRow, iColumnDiscont] = objProductSubType.Discont;
                            // требуемая наценка, %
                            oSheet.Cells[iCurrentRow, iColumnMarkUpReqiured] = objProductSubType.MarkUpRequired;

                            bStopRead = true;
                        }
                        else if (bPartSubTypeExistInSheet == true)
                        {
                            oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.Name;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeId] = objProductSubType.ID_Ib;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeState] = objProductSubType.SubTypeStateName;
                            // тариф
                            oSheet.Cells[iCurrentRow, iColumnVendorTarif] = objProductSubType.VendorTariff;
                            if (objProductSubType.VendorTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnVendorTarif], oSheet.Cells[iCurrentRow, iColumnVendorTarif]).Interior.ColorIndex = 3;
                            }
                            // % расходов на транспорт
                            oSheet.Cells[iCurrentRow, iColumnTransportTarif] = objProductSubType.TransportTariff;
                            if (objProductSubType.TransportTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnTransportTarif], oSheet.Cells[iCurrentRow, iColumnTransportTarif]).Interior.ColorIndex = 3;
                            }
                            //Сумма расходов на транспорт
                            //oSheet.Cells[iCurrentRow, 7] = oSheet.Cells[iCurrentRow-1, 7]; 
                            // Таможенная пошлина, %
                            oSheet.Cells[iCurrentRow, iColumnCustomTarif] = objProductSubType.CustomsTariff;
                            if (objProductSubType.CustomsTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnCustomTarif], oSheet.Cells[iCurrentRow, iColumnCustomTarif]).Interior.ColorIndex = 3;
                            }
                            // Сумма расходов на таможню
                            //oSheet.Cells[iCurrentRow, 9] = oSheet.Cells[iCurrentRow-1, 9]; 
                            // Маржа, %
                            oSheet.Cells[iCurrentRow, iColumnMargin] = objProductSubType.Margin;
                            // Ставка НДС, %
                            oSheet.Cells[iCurrentRow, iColumnNDS] = objProductSubType.NDS;
                            // Сумма НДС
                            //oSheet.Cells[iCurrentRow, 12] = oSheet.Cells[iCurrentRow-1, 12];
                            // Курс ценообразования (текущий)
                            oSheet.Cells[iCurrentRow, iColumnCurrRate] = dblCurrentPriceCreateRate;
                            // Дисконт, %
                            oSheet.Cells[iCurrentRow, iColumnDiscont] = objProductSubType.Discont;
                            // требуемая наценка, %
                            oSheet.Cells[iCurrentRow, iColumnMarkUpReqiured] = objProductSubType.MarkUpRequired;

                            oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Select();
                        }

                        iCurrentRow++;
                    }
                }

                //System.Int32 iRowCount = oSheet.Cells.Rows.Count;
                //oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iRowCount - iCurrentRow, 100]).ClearContents();

                ((Excel._Worksheet)oWB.Worksheets[iSheetNum]).Activate();
                oXL.Visible = true;
                //oXL.UserControl = true;
            }
            catch (System.Exception f)
            {
                oXL = null;
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

        #endregion

        #region Потоки
        /// <summary>
        /// Подключает список новинок гриду
        /// </summary>
        /// <param name="objProductNewList">список новинок</param>
        private void SetProductNewListToGrid( List<CProduct> objProductNewList)
        {
            try
            {
                gridControlPartsNewList.DataSource = null;

                if (objProductNewList != null)
                {
                    gridControlPartsNewList.DataSource = objProductNewList;

                    tabPagePartsNew.Image = ((m_objProductNewList.Count > 0) ? ERPMercuryPlan.Properties.Resources.warning : null);
                }

                foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnViewPartsNew.Columns)
                {
                    objColumn.BestFit();
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetProductNewListToGrid. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
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
                    //m_objFrmPartsList = new frmPartsList(m_objProfile, m_objMenuItem, objPartsList);
                    //btnSetToPartList.Visible = true;
                }

                gridControlProduct.DataSource = objPartsList;
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
                // новинки
                m_objProductNewList = CProduct.GetProductList(m_objProfile, null, true);
                if (m_objProductNewList != null)
                {
                    this.Invoke(m_SetProductNewListToGridDelegate, new Object[] { m_objProductNewList });
                }

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
                m_SetProductNewListToGridDelegate = new SetProductNewListToGridDelegate(SetProductNewListToGrid);
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

        #region Добавление/удаление товаров в подгруппу
        /// <summary>
        /// Прописывает значения марки и группы в зависимости от того, какие товары включены в подгруппу
        /// </summary>
        private void RefreshInfoAboutProductOwner()
        {
            try
            {
                if (treeListProductInSubtype.Nodes.Count == 0)
                {
                    txtProductOwner.Text = "";
                    txtProductType.Text = "";
                }
                else if( ( treeListProductInSubtype.FocusedNode != null ) && (treeListProductInSubtype.FocusedNode.Tag != null) )
                {
                    CProduct objProduct = (CProduct)treeListProductInSubtype.FocusedNode.Tag;
                    txtProductOwner.Text = objProduct.ProductTradeMarkName;
                    txtProductType.Text = objProduct.ProductTypeName;
                    objProduct = null;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("RefreshInfoAboutProductOwner. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Добавляет товары в товарную подгруппу
        /// </summary>
        private void AddproductToSubType()
        {
            System.Boolean bAdd = false;
            try
            {
                int[] arr = gridViewProduct.GetSelectedRows();
                

                if (arr.Length > 0)
                {
                    System.Boolean bProductExists = false;
                    CProduct objProduct = null;
                    for (System.Int32 i = 0; i < arr.Length; i++)
                    {
                        bProductExists = false;
                        objProduct = m_objPartsList[gridViewProduct.GetDataSourceRowIndex(arr[i])];

                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListProductInSubtype.Nodes)
                        {
                            if (objNode.Tag == null) { continue; }
                            if (((CProduct)objNode.Tag).ID.CompareTo(objProduct.ID) == 0)
                            {
                                bProductExists = true;
                                break;
                            }
                        }

                        if (bProductExists == false)
                        {
                            treeListProductInSubtype.AppendNode(new object[] { objProduct.ProductFullName }, null).Tag = objProduct;
                            bAdd = true;
                        }

                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка AddproductToSubType. Текст ошибки: " + f.Message);
            }
            finally
            {
                if (bAdd == true) { SetPropertiesModified( true, enTabProperties.ProductList ); }
                RefreshInfoAboutProductOwner();
            }
            return;
        }
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                AddproductToSubType();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка btnAddProduct_Click. Текст ошибки: " + f.Message);
            }
            finally
            {

            }

            return;
        }
        private void DeleteProductFromSubType()
        {
            try
            {
                if (treeListProductInSubtype.FocusedNode != null )
                {
                    treeListProductInSubtype.Nodes.Remove(treeListProductInSubtype.FocusedNode);

                    SetPropertiesModified(true, enTabProperties.ProductList);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка DeleteProductFromSubType. Текст ошибки: " + f.Message);
            }
            finally
            {
                RefreshInfoAboutProductOwner();
            }

            return;
        }
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteProductFromSubType();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка DeleteProductFromSubType. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Сохраняет список товаров для подгруппы
        /// </summary>
        /// <returns></returns>
        private System.Boolean SaveProductlist()
        {
            System.Boolean bRet = false;
            try
            {
                System.String strErr = "";

                List<CProduct> objSelectedProductList = new List<CProduct>();
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListProductInSubtype.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objSelectedProductList.Add((CProduct)objNode.Tag);
                }

                System.Boolean bSaveOkIB = CProductSubType.SetPartSubTypeToPartsInIB(m_objProfile, objSelectedProductList, m_objSelectedProduct, ref strErr);
                if (bSaveOkIB == true)
                {
                    bRet = bSaveOkIB;
                    if (CProductSubType.SetPartSubTypeToParts(m_objProfile, objSelectedProductList, m_objSelectedProduct) == true)
                    {
                        SendMessageToLog("Товарная подгруппа успешно присвоена списку товаров");
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Товарная подгруппа успешно присвоена списку товаров.", "Информация",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
                else
                {
                    SendMessageToLog(strErr);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка SaveProductlist. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;

        }
        #endregion

        #region Прайс-лист
        private void treeListPriceEditor_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true, enTabProperties.Price);
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
        /// <summary>
        /// Сохраняет изменения в прайс-листе
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean SavePriceList()
        {
            System.Boolean bRet = false;
            try
            {
                System.String strErr = "";

                CPriceListCalcItem objPriceListItemForSave = new CPriceListCalcItem();
                objPriceListItemForSave.objProductSubType = m_objSelectedProduct;
                objPriceListItemForSave.PriceList = new List<CPrice>();

                CPriceType objPriceType = null;
                List<CPriceType> objPriceTypeCheckedList = new List<CPriceType>();
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objPriceType = (CPriceType)objNode.Tag;

                    objPriceTypeCheckedList.Add(objPriceType);
                    objPriceListItemForSave.PriceList.Add(new CPrice(objPriceType, System.Convert.ToDouble(objNode.GetValue(colPriceValue))));
                }

                objPriceType = null;

                List<CPriceListCalcItem> objPriceListItemsList = new List<CPriceListCalcItem>();
                objPriceListItemsList.Add(objPriceListItemForSave);

                // сперва в InterBase
                System.Boolean bIsOkSave = CProductSubTypePriceList.SavePriceListToIB(objPriceListItemsList, objPriceTypeCheckedList, m_objProfile, ref strErr);
                if (bIsOkSave == true)
                {
                    // теперь в прайсе по подгруппам и товарам
                    bIsOkSave = CProductSubTypePriceList.SaveCalcItemList(objPriceListItemsList, m_objProfile, null, ref strErr);

                    if( bIsOkSave == false )
                    {
                        SendMessageToLog(strErr);
                    }
                }
                objPriceListItemForSave = null;
                objPriceTypeCheckedList = null;

                if (bIsOkSave == true)
                {
                    gridControlPricesContract.DataSource = null;
                    gridControlPricesContract.DataSource = CProductPriceListItemIB.GetPricesFromIB(m_objProfile, null, m_objSelectedProduct.ID);
                }

                bRet = bIsOkSave;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в прайс-листе. Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        #endregion

        #region Расчёт цен
        /// <summary>
        /// Загружает список расчётоа цен
        /// </summary>
        private void LoadCalcPriceList()
        {
            try
            {
                treeListCalcPriceList.Nodes.Clear();
                List<CPriceListCalc> objPriceListCalcList = CPriceListCalc.GetPriceListCalcList(m_objProfile, null);
                // попробовать через using !!!
                if (objPriceListCalcList != null)
                {
                    foreach (CPriceListCalc objItem in objPriceListCalcList)
                    {
                        treeListCalcPriceList.AppendNode(new object[] { objItem.IsActive, objItem.DocDate.ToShortDateString(), objItem.Name, objItem.FileNameXLS }, null).Tag = objItem;
                    }
                }

                objPriceListCalcList = null;

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("LoadCalcPriceList. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void CalcPriceForPartSubType(CPriceListCalc objPriceListCalc)
        {
            try
            {
                m_strFileCalcPriceName = "";
                if (objPriceListCalc == null) { return; }
                if (objPriceListCalc.FileNameXLS == "") 
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Для выбранного расчёта не указан файл MS Excel.\n Укажите, пожалуйста, в карточке расчёта файл с шаблоном для расчёта цен.", "Внимание!",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return; 
                }

                // загружаем из базы файл на диск
                System.String strFileName = "C:\\Temp\\" + objPriceListCalc.FileNameXLS;
                System.IO.FileInfo fi = new System.IO.FileInfo(strFileName);
                System.String strDirectory = fi.DirectoryName;
                fi = null;

                System.String strErr = "";

                byte[] arAttach = CPriceListCalc.GetReportFile(objPriceListCalc.ID, m_objProfile, ref strErr);
                if (arAttach != null)
                {
                    if (System.IO.File.Exists(strFileName) == true)  { System.IO.File.Delete(strFileName); }

                    int lung = Convert.ToInt32(arAttach.Length);
                    // создаем файловый поток
                    System.IO.FileStream fs = new System.IO.FileStream(strFileName, System.IO.FileMode.Create);
                    // Считываем содержимое вложения в файл
                    fs.Write(arAttach, 0, lung);
                    fs.Close();
                    fs.Dispose();
                    fs = null;

                    Excel.Application oXL = null;
                    Excel._Workbook oWB;
                    object m = Type.Missing;

                    oXL = new Excel.Application();
                    oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));

                    clstSheets.Items.Clear();
                    foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                    {
                        clstSheets.Items.Add(objSheet.Name, false);
                    }
                   

                    oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                    oXL.Quit();

                    oWB = null;
                    oXL = null;

                    calcCurRatePrice.Value = System.Convert.ToDecimal(CProductSubType.GetCurrentPriceCreateRate( m_objProfile, ref strErr ));

                    m_objSettingForCalcPriceList = CSettingForCalcPrice.GetSettingForCalcPriceList(m_objProfile, null);

                    System.Int32 iSelectedSheet = -1;
                    if(txtProductOwner.Text != "")
                    {
                        for( System.Int32 i=0; i < clstSheets.ItemCount; i++  )
                        {
                            if( clstSheets.GetItemText( i ) == txtProductOwner.Text )
                            {
                                iSelectedSheet = i;
                                break;
                            }
                        }
                    }

                    if (iSelectedSheet >= 0) { clstSheets.SelectedIndex = iSelectedSheet; }

                    m_strFileCalcPriceName = strFileName;
                    clstSheets.Tag = objPriceListCalc;
                }
                else
                {
                    SendMessageToLog("GetReportFile. Текст ошибки: " + strErr);
                    return;
                }
                arAttach = null;

                // у нас есть файл на диске с расчётом, нужно загрузить в него информацию о подгруппе


            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadCalcPriceList. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void LoadSettingsForSheet(System.Int32 iSheetNum)
        {

            try
            {
                treeListSettings.Nodes.Clear();

                if (iSheetNum <= 0) { return; }
                if (m_objSettingForCalcPriceList == null) { return; }
                if (m_objSettingForCalcPriceList.Count < 0) { return; }

                foreach (CSettingForCalcPrice objItem in m_objSettingForCalcPriceList)
                {
                    if (objItem.SheetID == iSheetNum)
                    {
                        treeListSettings.Tag = objItem;

                        foreach (CSettingItemForCalcPrice objSetting in objItem.SettingsList)
                        {
                            treeListSettings.AppendNode(new object[] { objSetting.ParamName, System.String.Format("{0:### ### ##0}", objSetting.ColumnID) }, null).Tag = objSetting;
                        }

                        break;
                    }
                }

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("LoadSettingsForSheet. Текст ошибки: " + f.Message); 
            }
            finally
            {
            }

            return;
        }

        private void clstSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((clstSheets.SelectedIndex >= 0) && (m_objSettingForCalcPriceList != null) &&
                    (m_objSettingForCalcPriceList.Count > clstSheets.SelectedIndex))
                {
                    LoadSettingsForSheet(clstSheets.SelectedIndex + 1);
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("clstSheets_SelectedIndexChanged. Текст ошибки: " + f.Message); 
            }
            finally
            {
            }

            return;
        }

        private void treeListCalcPriceList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (treeListCalcPriceList.Nodes.Count == 0) { return; }
                if (treeListCalcPriceList.FocusedNode == null) { return; }
                if (treeListCalcPriceList.FocusedNode.Tag == null) { return; }

                CalcPriceForPartSubType( ( CPriceListCalc )treeListCalcPriceList.FocusedNode.Tag );
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "treeListCalcPriceList_MouseDoubleClick.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }


        private void btnCalcPrice_Click(object sender, EventArgs e)
        {
            try
            {
                if( (clstSheets.SelectedIndex < 0) || (treeListSettings.Tag == null) )
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Укажите, пожалуйста, лист с расчётом цен.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {

                    AddPartSubTypeListToExcel2(m_objSelectedProduct, m_strFileCalcPriceName, (clstSheets.SelectedIndex + 1),
                        System.Convert.ToDouble(calcCurRatePrice.Value), (CSettingForCalcPrice)treeListSettings.Tag);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnCalcPrice_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void AddPartSubTypeListToExcel2(CProductSubType objProductSubType,
            System.String strFileName, System.Int32 iSheetNum, System.Double dblCurrentPriceCreateRate,
            CSettingForCalcPrice objSettingForCalcPrice )
        {
            if ((strFileName == "") || (iSheetNum < 1)) { return; }
            if (objProductSubType == null) { return; }

            Excel.Application oXL = null;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            System.Int32 iStartRow = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameStartRow);
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;
            CPriceListCalcItem objCPriceListCalcItem = null;
            CPrice objPrice = null;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                oSheet = (Excel._Worksheet)oWB.Worksheets[iSheetNum];

                System.String strProductOwnerSearch = txtProductOwner.Text;
                System.String strProductTypeSearch = txtProductType.Text;

                System.String strProductOwnerCurrent = "";
                System.String strProductTypeCurrent = "";
                System.Boolean bStopRead = false;
                System.String strTmp = "";
                System.String strPartSubTypeId = "";
                System.Boolean bFindProductGroup = false;
                System.Boolean bDoubling = false;
                System.Int32 iNumOfRowForCopy = 0;
                System.Boolean bNeedAdd = false;
                System.Boolean bPartSubTypeExistInSheet = false;

                System.Int32 iColumnProductOwner = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductOwner);
                System.Int32 iColumnProductSubType = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubType);
                System.Int32 iColumnProductSubTypeId = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubTypeID);
                System.Int32 iColumnProductSubTypeState = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubTypeState);
                System.Int32 iColumnVendorTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameVendorTarif);
                System.Int32 iColumnTransportTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameTransportTarif);
                System.Int32 iColumnTransportTarifSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameTransportTarifSum);
                System.Int32 iColumnCustomTarif = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCustomTarif);
                System.Int32 iColumnCustomTarifSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCustomTarifSum);
                System.Int32 iColumnMargin = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameMargin);
                System.Int32 iColumnNDS = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameNDS);
                System.Int32 iColumnNDSSum = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameNDSSum);
                System.Int32 iColumnDiscont = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameDiscont);
                System.Int32 iColumnCurrRate = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCurRate);
                System.Int32 iColumnMarkUpReqiured = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameMarkUpReqiured);

                    bFindProductGroup = false;
                    strTmp = "";
                    bStopRead = false;
                    bDoubling = false;
                    iNumOfRowForCopy = 0;
                    bNeedAdd = false;
                    bPartSubTypeExistInSheet = false;

                    if ((System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubType], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Value2) != "") &&
                        (System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubType], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Value2) != "2"))
                    {
                        iCurrentRow = iStartRow - 1;
                    }
                    else
                    {
                        iCurrentRow = iStartRow;
                    }

                    while (bStopRead == false)
                    {
                        strTmp = System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubType], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Value2);
                        strPartSubTypeId = System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubTypeId], oSheet.Cells[iCurrentRow, iColumnProductSubTypeId]).Value2);

                        if (strPartSubTypeId == objProductSubType.ID_Ib.ToString())
                        {
                            bStopRead = true;
                            bNeedAdd = false;
                            bPartSubTypeExistInSheet = true;
                        }
                        if (strTmp == "")
                        {
                            if ((strProductTypeCurrent != strProductTypeSearch ) && (bFindProductGroup == false))
                            {
                                // мы прошли весь список и нужной группы не нашли
                                oSheet.Cells[iCurrentRow, iColumnProductOwner] = strProductOwnerSearch;
                                oSheet.Cells[iCurrentRow, iColumnProductSubType] = strProductTypeSearch;

                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Underline = true;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Italic = true;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Size = 14;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Font.Bold = true;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.PatternColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.Color = 12632256;

                                strProductTypeCurrent = strProductTypeSearch;
                                bFindProductGroup = true;
                                iCurrentRow++;
                                iNumOfRowForCopy = iCurrentRow - 2;
                                bNeedAdd = true;

                                //iCurrentRow = iStartRow;
                            }
                            else
                            {
                                // мы прошли весь список
                                bStopRead = true;
                            }

                        }
                        else
                        {
                            //в одном столбце с подгруппами идет название группы (сверху)
                            if (System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubTypeId], oSheet.Cells[iCurrentRow, iColumnProductSubTypeId]).Value2) == "")
                            {
                                // в ячейке название группы
                                strProductTypeCurrent = strTmp;
                                if (System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductOwner]).Value2) != strProductOwnerCurrent)
                                {
                                    strProductOwnerCurrent = System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductOwner], oSheet.Cells[iCurrentRow, iColumnProductOwner]).Value2);
                                }
                            }

                            if (strProductTypeCurrent == strProductTypeSearch)
                            {
                                // группу нашли
                                bFindProductGroup = true;
                            }
                            else
                            {
                                if ((strProductTypeCurrent != strProductTypeSearch) && (bFindProductGroup == true) && (bDoubling == false))
                                {
                                    // получается, что мы только что закончили просматривать подгруппы нужной нам группы и не нашли искомой подгруппы
                                    // самое время добавлять строку
                                    iNumOfRowForCopy = iCurrentRow - 1;
                                    bNeedAdd = true;
                                }
                            }
                            if (strTmp == objProductSubType.Name) { bDoubling = true; }
                        }


                        if (bStopRead == true)
                        {
                            // мы прошли все записи
                            if (bDoubling == false)
                            {
                                iNumOfRowForCopy = iCurrentRow - 1;
                                bNeedAdd = true;
                                // дублирующихся записей нет
                                if (bFindProductGroup == false)
                                {
                                    iNumOfRowForCopy = iCurrentRow - 1;
                                    // нужная группа не найдена,  придется добавить название группы
                                    if (strProductOwnerCurrent != strProductOwnerSearch)
                                    {
                                        oSheet.Cells[iCurrentRow, iColumnProductOwner] = strProductOwnerSearch;
                                        oSheet.Cells[iCurrentRow, iColumnProductSubType] = strProductTypeSearch;
                                    }
                                    iCurrentRow++;
                                }
                            }
                        }

                        if (bNeedAdd == true)
                        {
                            oSheet.get_Range(oSheet.Cells[iNumOfRowForCopy, 1], oSheet.Cells[iNumOfRowForCopy, 200]).Copy(Missing.Value);
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 1]).Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnProductSubType], oSheet.Cells[iCurrentRow, iColumnProductSubType]).Interior.ColorIndex = 3;
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnVendorTarif], oSheet.Cells[iCurrentRow, iColumnVendorTarif]).Interior.Pattern = Excel.XlPattern.xlPatternNone;
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnTransportTarif], oSheet.Cells[iCurrentRow, iColumnTransportTarif]).Interior.Pattern = Excel.XlPattern.xlPatternNone;
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnCustomTarif], oSheet.Cells[iCurrentRow, iColumnCustomTarif]).Interior.Pattern = Excel.XlPattern.xlPatternNone;

                            oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.Name;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeId] = objProductSubType.ID_Ib;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeState] = objProductSubType.SubTypeStateName;
                            // тариф
                            oSheet.Cells[iCurrentRow, iColumnVendorTarif] = objProductSubType.VendorTariff;
                            if (objProductSubType.VendorTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnVendorTarif], oSheet.Cells[iCurrentRow, iColumnVendorTarif]).Interior.ColorIndex = 3;
                            }
                            // % расходов на транспорт
                            oSheet.Cells[iCurrentRow, iColumnTransportTarif] = objProductSubType.TransportTariff;
                            if (objProductSubType.TransportTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnTransportTarif], oSheet.Cells[iCurrentRow, iColumnTransportTarif]).Interior.ColorIndex = 3;
                            }
                            //Сумма расходов на транспорт
                            //oSheet.Cells[iCurrentRow, 7] = oSheet.Cells[iCurrentRow-1, 7]; 
                            // Таможенная пошлина, %
                            oSheet.Cells[iCurrentRow, iColumnCustomTarif] = objProductSubType.CustomsTariff;
                            if (objProductSubType.CustomsTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnCustomTarif], oSheet.Cells[iCurrentRow, iColumnCustomTarif]).Interior.ColorIndex = 3;
                            }
                            // Сумма расходов на таможню
                            //oSheet.Cells[iCurrentRow, 9] = oSheet.Cells[iCurrentRow-1, 9]; 
                            // Маржа, %
                            oSheet.Cells[iCurrentRow, iColumnMargin] = objProductSubType.Margin;
                            // Ставка НДС, %
                            oSheet.Cells[iCurrentRow, iColumnNDS] = objProductSubType.NDS;
                            // Сумма НДС
                            //oSheet.Cells[iCurrentRow, 12] = oSheet.Cells[iCurrentRow-1, 12];
                            // Курс ценообразования (текущий)
                            oSheet.Cells[iCurrentRow, iColumnCurrRate] = dblCurrentPriceCreateRate;
                            // Дисконт, %
                            oSheet.Cells[iCurrentRow, iColumnDiscont] = objProductSubType.Discont;
                            // Требуемая наценка, %
                            oSheet.Cells[iCurrentRow, iColumnMarkUpReqiured] = objProductSubType.MarkUpRequired;

                            bStopRead = true;
                            oSheet.Calculate();

                            objCPriceListCalcItem = new CPriceListCalcItem();
                            objCPriceListCalcItem.objProductSubType = m_objSelectedProduct;
                            objCPriceListCalcItem.PriceCurrencyRate = dblCurrentPriceCreateRate;
                            objCPriceListCalcItem.PriceList = new List<CPrice>();


                            // теперь цены                                
                            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes)
                            {
                                objPrice = null;
                                if ( ( System.Convert.ToBoolean(objNode.GetValue(colPriceCheck)) == true ) && ( objNode.Tag != null ) )
                                {
                                    if (objCPriceListCalcItem.PriceList == null) { objCPriceListCalcItem.PriceList = new List<CPrice>(); }
                                    objPrice = new CPrice();
                                    objPrice.PriceType = (CPriceType)objNode.Tag;
                                    objPrice.PriceValue = System.Convert.ToDouble(oSheet.get_Range(oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID], oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID]).Value2);
                                    objCPriceListCalcItem.PriceList.Add(objPrice);
                                }
                            }

                        }
                        else if (bPartSubTypeExistInSheet == true)
                        {
                            oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.Name;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeId] = objProductSubType.ID_Ib;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeState] = objProductSubType.SubTypeStateName;
                            // тариф
                            oSheet.Cells[iCurrentRow, iColumnVendorTarif] = objProductSubType.VendorTariff;
                            if (objProductSubType.VendorTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnVendorTarif], oSheet.Cells[iCurrentRow, iColumnVendorTarif]).Interior.ColorIndex = 3;
                            }
                            // % расходов на транспорт
                            oSheet.Cells[iCurrentRow, iColumnTransportTarif] = objProductSubType.TransportTariff;
                            if (objProductSubType.TransportTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnTransportTarif], oSheet.Cells[iCurrentRow, iColumnTransportTarif]).Interior.ColorIndex = 3;
                            }
                            //Сумма расходов на транспорт
                            //oSheet.Cells[iCurrentRow, 7] = oSheet.Cells[iCurrentRow-1, 7]; 
                            // Таможенная пошлина, %
                            oSheet.Cells[iCurrentRow, iColumnCustomTarif] = objProductSubType.CustomsTariff;
                            if (objProductSubType.CustomsTariff == 0)
                            {
                                oSheet.get_Range(oSheet.Cells[iCurrentRow, iColumnCustomTarif], oSheet.Cells[iCurrentRow, iColumnCustomTarif]).Interior.ColorIndex = 3;
                            }
                            // Сумма расходов на таможню
                            //oSheet.Cells[iCurrentRow, 9] = oSheet.Cells[iCurrentRow-1, 9]; 
                            // Маржа, %
                            oSheet.Cells[iCurrentRow, iColumnMargin] = objProductSubType.Margin;
                            // Ставка НДС, %
                            oSheet.Cells[iCurrentRow, iColumnNDS] = objProductSubType.NDS;
                            // Сумма НДС
                            //oSheet.Cells[iCurrentRow, 12] = oSheet.Cells[iCurrentRow-1, 12];
                            // Курс ценообразования (текущий)
                            oSheet.Cells[iCurrentRow, iColumnCurrRate] = dblCurrentPriceCreateRate;
                            // Дисконт, %
                            oSheet.Cells[iCurrentRow, iColumnDiscont] = objProductSubType.Discont;
                            // Требуемая наценка, %
                            oSheet.Cells[iCurrentRow, iColumnMarkUpReqiured] = objProductSubType.MarkUpRequired;

                            //oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Select();
                            oSheet.Calculate();

                            objCPriceListCalcItem = new CPriceListCalcItem();
                            objCPriceListCalcItem.objProductSubType = m_objSelectedProduct;
                            objCPriceListCalcItem.PriceCurrencyRate = dblCurrentPriceCreateRate;
                            objCPriceListCalcItem.PriceList = new List<CPrice>();

                            // теперь цены                                
                            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPriceEditor.Nodes)
                            {
                                objPrice = null;
                                if ((System.Convert.ToBoolean(objNode.GetValue(colPriceCheck)) == true) && (objNode.Tag != null))
                                {
                                    if (objCPriceListCalcItem.PriceList == null) { objCPriceListCalcItem.PriceList = new List<CPrice>(); }
                                    objPrice = new CPrice();
                                    objPrice.PriceType = (CPriceType)objNode.Tag;
                                    objPrice.PriceValue = System.Convert.ToDouble(oSheet.get_Range(oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID], oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID]).Value2);
                                    objCPriceListCalcItem.PriceList.Add(objPrice);
                                }
                            }

                        }

                        iCurrentRow++;
                    }


                ShowPriceListCalcItem(objCPriceListCalcItem);

                ((Excel._Worksheet)oWB.Worksheets[iSheetNum]).Activate();
                oWB.Save();
                oXL.Visible = true;
            }
            catch (System.Exception f)
            {
                oXL = null;
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
        /// Отображает список цен для строки расчета
        /// </summary>
        /// <param name="objPriceListCalcItem">строка расчета</param>
        private void ShowPriceListCalcItem(CPriceListCalcItem objPriceListCalcItem)
        {
            try
            {
                this.splitContainerControl.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListCalcPrice)).BeginInit();

                treeListCalcPrice.Nodes.Clear();

                if ((objPriceListCalcItem != null) && (objPriceListCalcItem.PriceList != null))
                {
                    foreach (CPrice objPrice in objPriceListCalcItem.PriceList)
                    {
                        treeListCalcPrice.AppendNode(new object[] { objPrice.PriceType.Name, objPrice.PriceValue }, null).Tag = objPrice;
                    }
                }
                treeListCalcPrice.Tag = objPriceListCalcItem;

                this.splitContainerControl.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListCalcPrice)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отображения цен для подгруппы. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Сохраняет рассчитанные цены впрайсе
        /// </summary>
        /// <param name="objPriceListCalc">объект "расчет цен"</param>
        /// <returns>true - цены переданы в прайс-лист; false - ошибка</returns>
        private System.Boolean SavePartSubtypePricesInPriceList(CPriceListCalc objPriceListCalc)
        {
            System.Boolean bRet = false;
            try
            {
                if (treeListCalcPrice.Nodes.Count == 0) { return bRet; }
                if (treeListCalcPrice.Tag == null) { return bRet; }
                if(clstSheets.Tag == null) { return bRet; }

                System.String strErr = "";

                // сохраняем файл MS Excel с расчетом в БД
                objPriceListCalc.UploadFileXlsToDatabase( m_strFileCalcPriceName, m_objProfile, null, ref strErr); 
                
                // в расчет нужно добавить строку с нашей подгруппой и ценами
                CPriceListCalcItem objPriceListCalcItem = (CPriceListCalcItem)treeListCalcPrice.Tag;
                List<CPriceListCalcItem> objPriceListCalcItemList = null;

                CPrice objPrice = null;
                List<CPriceType> objPriceTypeCheckedList = new List<CPriceType>();
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListCalcPrice.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objPrice = (CPrice)objNode.Tag;
                    objPrice.PriceValue = System.Convert.ToDouble(objNode.GetValue(coltreelistCalcPriceValue));
                    objPriceListCalcItem.PriceList.Add(objPrice);
                    objPriceTypeCheckedList.Add(objPrice.PriceType);
                }

                if (CPriceListCalc.AddPriceListCalcItem(objPriceListCalc.ID, objPriceListCalcItem, m_objProfile, null, ref strErr) == true)
                {
                    // теперь сохраняем цены в прайс
                    objPriceListCalcItemList = new List<CPriceListCalcItem>();
                    objPriceListCalcItemList.Add(objPriceListCalcItem);

                    // сперва в "Контракт"
                    if (CProductSubTypePriceList.SavePriceListToIB(objPriceListCalcItemList, objPriceTypeCheckedList, m_objProfile, ref strErr) == true)
                    {

                        bRet = CProductSubTypePriceList.SaveCalcItemList(objPriceListCalcItemList, m_objProfile, null, ref strErr);
                        if (bRet == true)
                        {
                            SendMessageToLog("Цены успешно переданы в прайс-лист.");
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
                else
                {
                    SendMessageToLog("Ошибка сохранения строки расчёта: " + strErr);
                }

                objPriceListCalcItemList = null;
                objPriceTypeCheckedList = null;
                objPrice = null;

            }
            catch (System.Exception f)
            {
                SendMessageToLog("SavePriceList. Текст ошибки: " + f.Message);
            }
            finally
            {
                
            }

            return bRet;
        }

        private void btnSavePriceList_Click(object sender, EventArgs e)
        {
            try
            {
                if (clstSheets.Tag == null) { return; }

                Cursor = Cursors.WaitCursor;

                if (SavePartSubtypePricesInPriceList((CPriceListCalc)clstSheets.Tag) == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Расчёт изменен и цены успешно переданы в прайс-лист.", "Внимание!",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка btnSavePriceList_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }


        private void AddPartSubTypeListPriceListCalc()
        {
            List<CProductSubType> objSubTypeList = null;

            try
            {
                objSubTypeList = new List<CProductSubType>();
                int[] arr = gridViewProductList.GetSelectedRows();
                if (arr.Length >= 1)
                {
                    // выделено больше одной записи, так что считаем, что в Excel передаем только выбранный список
                    for (System.Int32 i = 0; i < arr.Length; i++)
                    {
                        objSubTypeList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(arr[i])]);
                    }
                }
                else
                {
                    // все видимые записи
                    for (System.Int32 i = 0; i < gridViewProductList.RowCount; i++)
                    {
                        objSubTypeList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(i)]);
                    }
                }
                if (objSubTypeList.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                                        "Не удалось получить список записей для экспорта в MS Excel.", "Внимание",
                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                frmAddPartSubtypeInPriceListCalc objFrmAddPartSubtypeInPriceListCalc = new frmAddPartSubtypeInPriceListCalc(m_objProfile, m_objMenuItem);
                if (objFrmAddPartSubtypeInPriceListCalc != null)
                {
                    List<CPriceType> objPriceTypeList = CPriceType.GetPriceTypeList( m_objProfile, null );
                    objFrmAddPartSubtypeInPriceListCalc.OpenForm(objSubTypeList, objPriceTypeList);

                    objFrmAddPartSubtypeInPriceListCalc.Dispose();
                    objFrmAddPartSubtypeInPriceListCalc = null;
                    objPriceTypeList = null;
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования расчёта. Текст ошибки: " + f.Message);
            }
            finally
            {
                objSubTypeList = null;
            }
            return;
        }


        #endregion

        #region Изображение товарной подгруппы
        /// <summary>
        /// Загружает изображение товарной подгруппы
        /// </summary>
        /// <param name="objProductSubType">товарная подгруппа</param>
        private void LoadImageForProductSubType(CProductSubType objProductSubType)
        {
            if (objProductSubType == null) { return; }
            openFileDialog.Filter = "JPG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp|All files (*.*)|*.*";

            if ((openFileDialog.ShowDialog() == DialogResult.OK) && (openFileDialog.FileName != ""))
            {
                try
                {

                    // проверяем, существует ли указанный файл
                    if (System.IO.File.Exists(openFileDialog.FileName) == false)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            String.Format("Указанный файл не найден.\n{0}", openFileDialog.FileName), "Ошибка",
                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }

                    // создаем файловый поток
                    using (System.IO.FileStream fs = new System.IO.FileStream(openFileDialog.FileName, System.IO.FileMode.Open))
                    {
                        // FileInfo закачиваемого файла
                        System.IO.FileInfo fi = new System.IO.FileInfo(openFileDialog.FileName);
                        System.String strShortName = fi.Name;
                        System.String strExtension = fi.Extension;
                        int lung = Convert.ToInt32(fi.Length);
                        // Считываем содержимое файла в массив байт.
                        objProductSubType.ImageProductSubType = new byte[lung];
                        fs.Read(objProductSubType.ImageProductSubType, 0, lung);
                        fs.Close();

                        objProductSubType.ImageProductSubTypeFileName = fi.Name;
                        objProductSubType.ExistImage = true;
                        objProductSubType.ActionType = enumProductSubTypeActionTypeWithImage.EditImage;
                    }

                    pictureBoxCertificate.Image = ((objProductSubType.ExistImage == true) ? ERPMercuryPlan.Properties.Resources.Document_2_Check : ERPMercuryPlan.Properties.Resources.Document_2_Warning);

                    btnCertificateImageView.Enabled = (objProductSubType.ExistImage == true);
                    btnCertificateImageClear.Enabled = (objProductSubType.ExistImage == true);
                    btnCertificateImageLoad.Enabled = (objProductSubType != null);
                    
                    SetPropertiesModified(true, enTabProperties.Image);
                }
                catch (System.Exception f)
                {
                    SendMessageToLog("Ошибка загрузки картинки. Текст ошибки: " + f.Message);
                }
            }

            return;
        }

        private void btnCertificateImageLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadImageForProductSubType( m_objSelectedProduct );
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка загрузки картинки.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Открывает изображение товарной подгруппы на просмотр
        /// </summary>
        /// <param name="objProductSubType">товарная подгруппа</param>
        private void ShowProductSubTypeImage(CProductSubType objProductSubType)
        {
            if (objProductSubType == null) { return; }
            if (objProductSubType.ExistImage == false) { return; }
            try
            {
                System.String strImageFileName = System.String.Empty;
                System.String strErr = System.String.Empty;

                if (objProductSubType.ImageProductSubType == null)
                {
                    byte[] arImageBytes = null;
                    System.String strCertificateFileName = System.String.Empty;

                    if (CProductSubType.LoadImageFromDB(m_objProfile, objProductSubType.ID, ref arImageBytes,
                        ref strCertificateFileName, ref strErr) == false)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось загрузить картинку из БД.\n" + strErr, "Внимание",
                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        objProductSubType.ImageProductSubType = arImageBytes;
                        objProductSubType.ImageProductSubTypeFileName = strCertificateFileName;
                    }
                }

                // Получить путь к системной папке.
                System.String sysFolder = Environment.GetFolderPath(Environment.SpecialFolder.System);
                System.String tmpFolder = Environment.GetEnvironmentVariable("TMP");

                Random rand = new Random();
                int temp = rand.Next(100);
                rand = null;

                System.String strFileFullName = String.Format(@"{0}\{1}", tmpFolder, objProductSubType.ImageProductSubTypeFileName);
                // Удаляем файл с таким именем
                if (System.IO.File.Exists(strFileFullName) == true)
                {
                    System.IO.File.Delete(strFileFullName);
                }
                if (System.IO.File.Exists(strFileFullName) == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Файл с указанным именем либо уже открыт,\nлибо у Вас нет соотвествующих прав доступа.\nОбратитесь к системному администратору.\nФайл: " + strFileFullName, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                // создаем файловый поток
                using (System.IO.FileStream fs = new System.IO.FileStream(strFileFullName, System.IO.FileMode.Create))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(strFileFullName);
                    int lung = Convert.ToInt32(objProductSubType.ImageProductSubType.Length);
                    // Считываем содержимое вложения в файл
                    fs.Write(objProductSubType.ImageProductSubType, 0, lung);
                    fs.Close();
                }

                ProcessStartInfo pInfo = new ProcessStartInfo(strFileFullName) { UseShellExecute = true, FileName = strFileFullName };
                Process p = Process.Start(pInfo);

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка просмотра изображения.\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void btnCertificateImageView_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_objSelectedProduct != null)
                {
                    ShowProductSubTypeImage(m_objSelectedProduct);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка просмотра картинки.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Удаление изображения товарной подгруппы
        /// </summary>
        /// <param name="objProductSubType">товарная подгруппа</param>
        private void RemoveImageForProductSubType(CProductSubType objProductSubType)
        {
            if (objProductSubType == null) { return; }

            try
            {
                objProductSubType.ImageProductSubType = null;
                objProductSubType.ExistImage = false;
                objProductSubType.ImageProductSubTypeFileName = System.String.Empty;
                objProductSubType.ActionType = enumProductSubTypeActionTypeWithImage.DeleteImage;

                pictureBoxCertificate.Image = ((objProductSubType.ExistImage == true) ? ERPMercuryPlan.Properties.Resources.Document_2_Check : ERPMercuryPlan.Properties.Resources.Document_2_Warning);

                btnCertificateImageView.Enabled = ( objProductSubType.ExistImage == true );
                btnCertificateImageClear.Enabled = ( objProductSubType.ExistImage == true);
                btnCertificateImageLoad.Enabled = ( objProductSubType != null );

                SetPropertiesModified(true, enTabProperties.Image);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления картинки. Текст ошибки: " + f.Message);
            }

            return;
        }

        private void btnCertificateImageClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_objSelectedProduct != null)
                {
                    RemoveImageForProductSubType(m_objSelectedProduct);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка удаления картинки.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        private void clstSheets_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if ((clstSheets.SelectedIndex >= 0) && (m_objSettingForCalcPriceList != null) &&
                    (m_objSettingForCalcPriceList.Count > clstSheets.SelectedIndex))
                {
                    LoadSettingsForSheet(clstSheets.SelectedIndex + 1);
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("clstSheets_ItemCheck. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

    }
}
