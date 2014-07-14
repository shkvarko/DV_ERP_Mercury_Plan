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
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;


namespace ERPMercuryPlan
{
    public partial class frmImportPlanByDepartCustomerSubtype : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.MENUITEM m_objMenuItem;
        private CProfile m_objProfile;
        private List<CProductTradeMark> m_objProductTradeMarkList;
        private List<CProductType> m_objProductTypeList;
        private List<CDepart> m_objDepartList;
        private List<CProductSubType> m_objProductSubTypeList;
        private List<CCustomer> m_objCustomerList;
        private List<CPlanByDepartCustomerSubTypeItem> m_objPlanItemList;
        private System.Data.DataTable m_objdtPlanItems;

        /// <summary>
        /// Список настроек
        /// </summary>
        private ERP_Mercury.Common.CSettingForImportData m_objSettingForImportData;
        private System.String m_strFileFullName;
        /// <summary>
        /// Имя файла
        /// </summary>
        public System.String FileFullName
        {
            get { return m_strFileFullName; }
        }
        public System.Int32 SelectedSheetId { get; set; }
        public List<System.String> SheetList;

        private const System.String strNodeSettingname = "ColumnItem";
        private const string strCaptionTextForImportInPlan = "Импорт данных в приложение к плану продаж";

        private static readonly System.String strFieldNameSTARTROW = "STARTROW";
        private static readonly System.String strFieldNameOWNER_ID = "OWNER_ID";
        private static readonly System.String strFieldNamePARTTYPE_ID = "PARTTYPE_ID";
        private static readonly System.String strFieldNameDEPART_CODE = "DEPART_CODE";
        private static readonly System.String strFieldNameCUSTOMER_ID = "CUSTOMER_ID";
        private static readonly System.String strFieldNamePARTSUBTYPE_ID = "PARTSUBTYPE_ID";
        private static readonly System.String strFieldNameQUANTITY = "QUANTITY";
        private static readonly System.String strFieldNameALLPRICE = "ALLPRICE";
        #endregion

        #region Конструктор
        public frmImportPlanByDepartCustomerSubtype(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objProductTradeMarkList = null;
            m_objProductTypeList = null;
            m_objDepartList = null;
            m_objProductSubTypeList = null;
            m_objCustomerList = null;
            m_strFileFullName = "";
            m_objSettingForImportData = null;
            btnLoadDataFromFile.Enabled = false;
            SelectedSheetId = 0;
            SheetList = null;
            m_objPlanItemList = null;

        }
        #endregion

        #region Загрузка списков для проверки импортируемых значений
        public void LoadComboBox(List<CDepart> objDepartList, List<CCustomer> objCustomerList,
           List<CProductSubType> objProductSubTypeList,
           List<CProductTradeMark> objProductTradeMarkList,
           List<CProductType> objProductTypeList)
        {
            try
            {
                m_objProductTradeMarkList = objProductTradeMarkList;
                m_objProductTypeList = objProductTypeList;
                m_objDepartList = objDepartList;
                m_objProductSubTypeList = objProductSubTypeList;
                m_objCustomerList = objCustomerList;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadComboBox.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Открыть форму с настройками для импорта приложения в заказ
        /// <summary>
        /// Открывает форму в режиме импорта данных в приложение к заказу
        /// </summary>
        /// <param name="objPlanItemList">приложение к плану продаж</param>
        /// <param name="strFileName">путь к файлу MS Excel</param>
        /// <param name="iSelectedSheetId">номер листа в файле</param>
        /// <param name="SheetList">список листов в файле</param>
        public void OpenForImportDataInPlan(
            List<CPlanByDepartCustomerSubTypeItem> objPlanItemList, System.String strFileName,
            System.Int32 iSelectedSheetId, List<System.String> SheetList
            )
        {
            try
            {
                m_objPlanItemList = objPlanItemList;

                SetInitialParams();

                txtID_Ib.Text = strFileName;
                cboxSheet.Properties.Items.Clear();
                if (SheetList != null)
                {
                    cboxSheet.Properties.Items.AddRange(SheetList);
                    cboxSheet.SelectedIndex = iSelectedSheetId;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "OpenForImportPartsInSuppl.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                ShowDialog();
            }

            return;

        }
        #endregion

        #region Первоначальные установки
        private void SetInitialParams()
        {
            try
            {
                txtID_Ib.Text = "";
                cboxSheet.Properties.Items.Clear();
                treeListSettings.Nodes.Clear();

                m_objSettingForImportData =  ERP_Mercury.Common.CSettingForImportData.GetSettingForImportDataInPlanByDepartCustomerSubtype(m_objProfile, null);
                tabControlTreeList.SelectedTabPage = tabPageImportOrder;
                Text = strCaptionTextForImportInPlan;

                if (m_objSettingForImportData != null)
                {
                    foreach (ERP_Mercury.Common.CSettingItemForImportData objSetting in m_objSettingForImportData.SettingsList)
                    {
                        treeListSettings.AppendNode(new object[] { true, objSetting.TOOLS_USERNAME, System.String.Format("{0:### ### ##0}", objSetting.TOOLS_VALUE), objSetting.TOOLS_DESCRIPTION }, null).Tag = objSetting;
                    }
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SetInitialParams.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Выбор файла
        private void btnFileOpenDialog_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Refresh();
                    if ((openFileDialog.FileName != "") && (System.IO.File.Exists(openFileDialog.FileName) == true))
                    {
                        txtID_Ib.Text = openFileDialog.FileName;
                        ReadSheetListFromXLSFile(txtID_Ib.Text);
                    }
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
        /// Считывает коллекцию листов в файле MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ReadSheetListFromXLSFile(System.String strFileName)
        {
            if (strFileName == "") { return; }
            if (System.IO.File.Exists(strFileName) == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                     "файл \"" + strFileName + "\" не найден.", "Ошибка",
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
                cboxSheet.Properties.Items.Clear();
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));

                foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                {
                    cboxSheet.Properties.Items.Add(objSheet.Name);
                }

                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oXL.Quit();

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
                cboxSheet.SelectedItem = ((cboxSheet.Properties.Items.Count > 0) ? cboxSheet.Properties.Items[0] : null);
                btnLoadDataFromFile.Enabled = (cboxSheet.SelectedItem != null);

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Возвращает значение параметра настройки по его наименованию
        /// </summary>
        /// <param name="strSettingName">имя параметра</param>
        /// <param name="bIsCheck">признак того, включена ли настройка</param>
        /// <returns>значение параметра настройки</returns>
        private System.Int32 GetSettingValueByName(System.String strSettingName, ref System.Boolean bIsCheck)
        {
            System.Int32 iRet = 0;
            try
            {
                ERP_Mercury.Common.CSettingItemForImportData objSetting = null;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSettings.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objSetting = (ERP_Mercury.Common.CSettingItemForImportData)objNode.Tag;
                    foreach (ERP_Mercury.Common.CSettingItemForImportData objItem in m_objSettingForImportData.SettingsList)
                    {
                        if (objSetting.TOOLS_NAME == strSettingName)
                        {
                            iRet = System.Convert.ToInt32(objNode.GetValue(colSettingsColumnNum));
                            bIsCheck = System.Convert.ToBoolean(objNode.GetValue(colCheck));
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "GetSettingValueByName.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return iRet;
        }

        /// <summary>
        /// Считывает информацию из фала MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ReadDataFromXLSFileForImportInPlan(System.String strFileName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                System.Int32 i = 1;

                treeListImportPlan.CellValueChanged -= new DevExpress.XtraTreeList.CellValueChangedEventHandler(treeListImportPlan_CellValueChanged);
                treeListImportPlan.Nodes.Clear();
                ((System.ComponentModel.ISupportInitialize)(this.treeListImportPlan)).BeginInit();

                System.IO.FileInfo newFile = new System.IO.FileInfo(strFileName);
                if (newFile.Exists == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Ошибка экспорта в MS Excel.\n\nНе найден файл: " + strFileName, "Ошибка",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }

                this.Text = "идет импорт записей...";
                listEditLog.Items.Clear();

                object objToolsItemValue = null;
                System.Int32 iColumnProductOwnerIbId = 0;
                System.Int32 iColumnProductSubTypeIbId = 0;
                System.Int32 iColumnProductTypeIbId = 0;
                System.Int32 iColumnCustomerIbId = 0;
                System.Int32 iColumnDepartCode = 0;
                System.Int32 iColumnPlanQuantity = 0;
                System.Int32 iColumnPlanAllPrice = 0;
                System.Int32 iStartRowForImport = 0;

                objToolsItemValue = m_objSettingForImportData.SettingsList.SingleOrDefault<ERP_Mercury.Common.CSettingItemForImportData>(x => x.TOOLS_NAME == strFieldNameOWNER_ID).TOOLS_VALUE;
                if (objToolsItemValue != null)
                {
                    iColumnProductOwnerIbId = System.Convert.ToInt32(objToolsItemValue);
                }
                objToolsItemValue = m_objSettingForImportData.SettingsList.SingleOrDefault<ERP_Mercury.Common.CSettingItemForImportData>(x => x.TOOLS_NAME == strFieldNamePARTSUBTYPE_ID).TOOLS_VALUE;
                if (objToolsItemValue != null)
                {
                    iColumnProductSubTypeIbId = System.Convert.ToInt32(objToolsItemValue);
                }
                objToolsItemValue = m_objSettingForImportData.SettingsList.SingleOrDefault<ERP_Mercury.Common.CSettingItemForImportData>(x => x.TOOLS_NAME == strFieldNamePARTTYPE_ID).TOOLS_VALUE;
                if (objToolsItemValue != null)
                {
                    iColumnProductTypeIbId = System.Convert.ToInt32(objToolsItemValue);
                }
                objToolsItemValue = m_objSettingForImportData.SettingsList.SingleOrDefault<ERP_Mercury.Common.CSettingItemForImportData>(x => x.TOOLS_NAME == strFieldNameCUSTOMER_ID).TOOLS_VALUE;
                if (objToolsItemValue != null)
                {
                    iColumnCustomerIbId = System.Convert.ToInt32(objToolsItemValue);
                }
                objToolsItemValue = m_objSettingForImportData.SettingsList.SingleOrDefault<ERP_Mercury.Common.CSettingItemForImportData>(x => x.TOOLS_NAME == strFieldNameDEPART_CODE).TOOLS_VALUE;
                if (objToolsItemValue != null)
                {
                    iColumnDepartCode = System.Convert.ToInt32(objToolsItemValue);
                }
                objToolsItemValue = m_objSettingForImportData.SettingsList.SingleOrDefault<ERP_Mercury.Common.CSettingItemForImportData>(x => x.TOOLS_NAME == strFieldNameQUANTITY).TOOLS_VALUE;
                if (objToolsItemValue != null)
                {
                    iColumnPlanQuantity = System.Convert.ToInt32(objToolsItemValue);
                }
                objToolsItemValue = m_objSettingForImportData.SettingsList.SingleOrDefault<ERP_Mercury.Common.CSettingItemForImportData>(x => x.TOOLS_NAME == strFieldNameALLPRICE).TOOLS_VALUE;
                if (objToolsItemValue != null)
                {
                    iColumnPlanAllPrice = System.Convert.ToInt32(objToolsItemValue);
                }
                objToolsItemValue = m_objSettingForImportData.SettingsList.SingleOrDefault<ERP_Mercury.Common.CSettingItemForImportData>(x => x.TOOLS_NAME == strFieldNameSTARTROW).TOOLS_VALUE;
                if (objToolsItemValue != null)
                {
                    iStartRowForImport = System.Convert.ToInt32(objToolsItemValue);
                }

                System.String strProductOwnerIbId = System.String.Empty;
                System.String strProductSubTypeIbId = System.String.Empty;
                System.String strProductTypeIbId = System.String.Empty;
                System.String strCustomerIbId = System.String.Empty;
                System.String strDepartCode = System.String.Empty;
                System.String strPlanQuantity = System.String.Empty;
                System.String strPlanAllPrice = System.String.Empty;

                System.Int32 iProductOwnerIbId = 0;
                System.Int32 iProductSubTypeIbId = 0;
                System.Int32 iProductTypeIbId = 0;
                System.Int32 iCustomerIbId = 0;

                System.Decimal dblPlanQuantity = 0;
                System.Decimal dblPlanAllPrice = 0;

                System.Decimal dblAllPlanQuantity = 0;
                System.Decimal dblAllPlanAllPrice = 0;

                System.Int32 iCurrentRow = iStartRowForImport;

                CProductTradeMark objProductTradeMark = null;
                CProductType objProductType = null;
                CProductSubType objProductSubType = null;
                CCustomer objCustomer = null;
                CDepart objDepart = null;

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[cboxSheet.Text];
                    if (worksheet != null)
                    {

                        System.Boolean bStopRead = false;
                        System.Boolean bErrExists = false;
                        System.String strFrstColumn = "";

                        while (bStopRead == false)
                        {
                            bErrExists = false;

                            // пробежим по строкам и считаем информацию
                            strFrstColumn = System.Convert.ToString(worksheet.Cells[iCurrentRow, 1].Value);
                            if (strFrstColumn == "")
                            {
                                bStopRead = true;
                            }
                            else
                            {
                                strProductOwnerIbId = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnProductOwnerIbId].Value);
                                strProductTypeIbId = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnProductTypeIbId].Value);
                                strProductSubTypeIbId = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnProductSubTypeIbId].Value);
                                strCustomerIbId = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnCustomerIbId].Value);
                                strDepartCode = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnDepartCode].Value);
                                strPlanQuantity = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnPlanQuantity].Value);
                                strPlanAllPrice = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnPlanAllPrice].Value);

                                dblPlanQuantity = 0;
                                dblPlanAllPrice = 0;
                                iProductOwnerIbId = 0;
                                iProductSubTypeIbId = 0;
                                iProductTypeIbId = 0;
                                iCustomerIbId = 0;

                                objProductTradeMark = null;
                                objProductType = null;
                                objProductSubType = null;
                                objCustomer = null;
                                objDepart = null;

                                // код ТМ
                                try
                                {
                                    iProductOwnerIbId = System.Math.Abs(System.Convert.ToInt32(strProductOwnerIbId));
                                }
                                catch
                                {
                                    bErrExists = true;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования кода марки товара в числовой формат.", i));
                                    listEditLog.Refresh();
                                }
                                // код Группы
                                try
                                {
                                    iProductTypeIbId = System.Math.Abs(System.Convert.ToInt32(strProductTypeIbId));
                                }
                                catch
                                {
                                    bErrExists = true;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования кода группы в числовой формат.", i));
                                    listEditLog.Refresh();
                                }
                                // код Подгруппы
                                try
                                {
                                    iProductSubTypeIbId = System.Math.Abs(System.Convert.ToInt32(strProductSubTypeIbId));
                                }
                                catch
                                {
                                    bErrExists = true;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования кода подгруппы в числовой формат.", i));
                                    listEditLog.Refresh();
                                }
                                // код Клиента
                                try
                                {
                                    iCustomerIbId = System.Math.Abs(System.Convert.ToInt32(strCustomerIbId));
                                }
                                catch
                                {
                                    bErrExists = true;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования кода клиента в числовой формат.", i));
                                    listEditLog.Refresh();
                                }
                                // количество
                                try
                                {
                                    dblPlanQuantity = System.Math.Abs(System.Convert.ToDecimal(strPlanQuantity));
                                    dblAllPlanQuantity += (dblPlanQuantity);
                                }
                                catch
                                {
                                    bErrExists = true;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования количества в числовой формат.", i));
                                    listEditLog.Refresh();
                                }
                                // сумма
                                try
                                {
                                    dblPlanAllPrice = System.Convert.ToDecimal(strPlanAllPrice);
                                    dblAllPlanAllPrice += (dblPlanAllPrice);
                                }
                                catch
                                {
                                    bErrExists = true;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования суммы в числовой формат.", i));
                                    listEditLog.Refresh();
                                }
                                
                                // поиск товарной марки, группы, подгруппы, клиента и подразделения
                                objProductTradeMark = m_objProductTradeMarkList.SingleOrDefault<CProductTradeMark>(x => x.ID_Ib.Equals(iProductOwnerIbId) == true);
                                objProductType = m_objProductTypeList.SingleOrDefault<CProductType>(x => x.ID_Ib.Equals(iProductTypeIbId) == true);
                                objProductSubType = m_objProductSubTypeList.SingleOrDefault<CProductSubType>(x => x.ID_Ib.Equals(iProductSubTypeIbId) == true);
                                objCustomer = m_objCustomerList.SingleOrDefault<CCustomer>(x => x.InterBaseID.Equals(iCustomerIbId) == true);
                                objDepart = m_objDepartList.SingleOrDefault<CDepart>(x => x.DepartCode == strDepartCode);

                                if ((bErrExists == false) && (bStopRead == false) && (objProductTradeMark != null) &&
                                    (objProductType != null) && (objProductSubType != null) &&
                                    (objCustomer != null) && (objDepart != null))
                                {
                                    treeListImportPlan.AppendNode(new object[] { true, objProductTradeMark.ID_Ib, objProductTradeMark.Name, 
                                        true, objProductType.ID_Ib, objProductType.Name, 
                                        true, objDepart.DepartCode, 
                                        true, objCustomer.InterBaseID, objCustomer.FullName, 
                                        true, objProductSubType.ID_Ib, objProductSubType.Name, 
                                        dblPlanQuantity, dblPlanAllPrice}, null).Tag = new CPlanByDepartCustomerSubTypeItem() 
                                                                                        {
                                                                                            ProductOwner = objProductTradeMark,
                                                                                            ProductType = objProductType,
                                                                                            Depart = objDepart,
                                                                                            Customer = objCustomer,
                                                                                            ProductSubType = objProductSubType,
                                                                                            Plan_Quantity = dblPlanQuantity,
                                                                                            Plan_AllPrice = dblPlanAllPrice
                                                                                        };
                                    treeListImportPlan.Refresh();

                                    listEditLog.Items.Add(String.Format("{0} OK ", i));
                                    listEditLog.Refresh();
                                }
                                else if (bStopRead == false) //((bErrExists == true) && (bStopRead == false))
                                {
                                    treeListImportPlan.AppendNode(new object[] {  ( objProductTradeMark != null ), strProductOwnerIbId, ( ( objProductTradeMark == null ) ? "ТМ НЕ найдена" : objProductTradeMark.Name ), 
                                        ( objProductType != null ), strProductTypeIbId, ( ( objProductType == null ) ? "Группа НЕ найдена" :  objProductType.Name),  
                                        ( objDepart != null ), objDepart.DepartCode, 
                                        ( objCustomer != null ), strCustomerIbId, ( ( objCustomer == null ) ? "Клиент НЕ найден" : objCustomer.FullName ), 
                                        (objProductSubType != null), strProductSubTypeIbId, ( (objProductSubType == null) ? "Подгруппа НЕ найдена" : objProductSubType.Name ), 
                                        dblPlanQuantity, dblPlanAllPrice }, null).Tag = null;

                                    treeListImportPlan.Refresh();
                                }
                                iCurrentRow++;
                                i++;
                                strFrstColumn = System.Convert.ToString(worksheet.Cells[iCurrentRow, 1].Value);
                                //listEditLog.Refresh();

                                //this.Text = String.Format("обрабатывается запись №{0}", i);
                                //this.Refresh();
                            }


                        } //while (bStopRead == false)

                        listEditLog.Items.Add(String.Format("обработано {0} записей, количество, шт. : {1},  сумма : {2} ", i, dblAllPlanQuantity, dblAllPlanAllPrice));
                    }
                    worksheet = null;
                }


            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка импорта данных из файла MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                ((System.ComponentModel.ISupportInitialize)(this.treeListImportPlan)).EndInit();
                treeListImportPlan.CellValueChanged += new DevExpress.XtraTreeList.CellValueChangedEventHandler(treeListImportPlan_CellValueChanged);
                treeListImportPlan.BestFitColumns();

                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.Text = strCaptionTextForImportInPlan;

            }
        }

        private void treeListImportPlan_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (( e.Value != null ) && (treeListImportPlan.FocusedNode != null) && (treeListImportPlan.FocusedNode.Tag != null))
                {
                    if (e.Column == colPlanQuantity)
                    {
                        ((CPlanByDepartCustomerSubTypeItem)treeListImportPlan.FocusedNode.Tag).Plan_Quantity = System.Convert.ToDecimal(e.Value);
                    }
                    if (e.Column == colPlanAllPrice)
                    {
                        ((CPlanByDepartCustomerSubTypeItem)treeListImportPlan.FocusedNode.Tag).Plan_AllPrice = System.Convert.ToDecimal(e.Value);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "treeListImportPlan_CellValueChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnLoadDataFromFile_Click(object sender, EventArgs e)
        {
            try
            {
                ReadDataFromXLSFileForImportInPlan(txtID_Ib.Text);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnLoadDataFromFile_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void cboxSheet_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                btnLoadDataFromFile.Enabled = (cboxSheet.SelectedItem != null);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "cboxSheet_SelectedValueChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Подтвердить выбор
        private void treeListSettings_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if ((e.Node == null) || (e.Node.Tag == null)) { return; }

                if (e.Column == colCheck)
                {
                    System.String strSettingName = ((ERP_Mercury.Common.CSettingItemForImportData)e.Node.Tag).TOOLS_NAME;

                    if (((strSettingName == ERP_Mercury.Common.CSettingForImportData.strFieldNamePARTS_ID) ||
                        (strSettingName == ERP_Mercury.Common.CSettingForImportData.strFieldNameARTICLE) ||
                        (strSettingName == ERP_Mercury.Common.CSettingForImportData.strFieldNameNAME2) || (strSettingName == CSettingForImportData.strFieldNameSTARTROW) ||
                        (strSettingName == ERP_Mercury.Common.CSettingForImportData.strFieldNameFULLNAME)) && (System.Convert.ToBoolean(e.Value) == false))
                    {
                        e.Node.SetValue(colCheck, true);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "treeListSettings_CellValueChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }


        #endregion

        #region Отмена
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnCancel_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Сохранить настройки в БД

        private void SaveSettings()
        {
            try
            {
                if ((m_objSettingForImportData != null) && (m_objSettingForImportData.SettingsList != null))
                {
                    ERP_Mercury.Common.CSettingItemForImportData objSetting = null;
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSettings.Nodes)
                    {
                        if (objNode.Tag == null) { continue; }
                        objSetting = (ERP_Mercury.Common.CSettingItemForImportData)objNode.Tag;
                        foreach (ERP_Mercury.Common.CSettingItemForImportData objItem in m_objSettingForImportData.SettingsList)
                        {
                            if (objSetting.TOOLS_ID == objItem.TOOLS_ID)
                            {
                                objItem.TOOLS_VALUE = objSetting.TOOLS_VALUE;
                                objItem.TOOLS_DESCRIPTION = objSetting.TOOLS_DESCRIPTION;
                                break;
                            }
                        }
                    }
                }

                System.String strErr = "";
                if (SaveXMLSettings(ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Ошибка сохранения настроек в базе данных.\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SaveSettings.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private System.Boolean SaveXMLSettings(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                System.Xml.XmlNodeList nodeList = m_objSettingForImportData.XMLSettings.GetElementsByTagName(strNodeSettingname);
                if (nodeList != null)
                {
                    ERP_Mercury.Common.CSettingItemForImportData objSetting = null;
                    foreach (System.Xml.XmlNode xmlNode in nodeList)
                    {
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSettings.Nodes)
                        {
                            if (objNode.Tag == null) { continue; }
                            objSetting = (ERP_Mercury.Common.CSettingItemForImportData)objNode.Tag;

                            if (objSetting.TOOLS_ID.ToString() == xmlNode.Attributes[0].Value)
                            {
                                xmlNode.Attributes[3].InnerText = System.Convert.ToString(objNode.GetValue(colSettingsDescription));
                                xmlNode.Attributes[4].InnerText = System.Convert.ToString(objNode.GetValue(colSettingsColumnNum));
                            }
                        }
                    }
                    // теперь и в Базе данных
                    bRet = m_objSettingForImportData.SaveExportSetting(m_objProfile, null, ref strErr);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SaveXMLSettingsForSheet.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;

        }

        private void btnSaveSetings_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettings();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка сохранения настроек.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }
        #endregion

        #region Закрытие формы
        private void frmImportPlanByDepartCustomerSubtype_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                m_strFileFullName = txtID_Ib.Text;
                SelectedSheetId = cboxSheet.SelectedIndex;
                SheetList = new List<string>();
                foreach (object objItem in cboxSheet.Properties.Items)
                {
                    SheetList.Add(System.Convert.ToString(objItem));
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "frmImportXLSData_FormClosed.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Отрисовка ячеек дерева
        private void treeListImportPlan_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            try
            {
                System.Double dblPlanAllPrice = System.Convert.ToDouble(e.Node.GetValue(colPlanAllPrice));
                System.Double dblPlanQuantity = System.Convert.ToDouble(e.Node.GetValue(colPlanQuantity));

                if ((dblPlanAllPrice == 0) || (dblPlanQuantity == 0))
                {
                    if (e.Column == colPlanAllPrice)
                    {
                        e.Appearance.DrawString(e.Cache, e.CellText,
                                    new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y,
                                    e.Bounds.Size.Width - 3, e.Bounds.Size.Height), new System.Drawing.SolidBrush(Color.Red));
                        e.Handled = true;
                    }
                    if (e.Column == colPlanQuantity)
                    {
                        e.Appearance.DrawString(e.Cache, e.CellText,
                                    new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y,
                                    e.Bounds.Size.Width - 3, e.Bounds.Size.Height), new System.Drawing.SolidBrush(Color.Red));
                        e.Handled = true;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "treeListImportOrder_CustomDrawNodeCell.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void treeListImportPlan_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            try
            {
                if (treeListImportPlan.Nodes.Count == 0) { return; }
                if (e.Node == null) { return; }

                System.Boolean bProductOwnerCheckCheck = System.Convert.ToBoolean(e.Node.GetValue(colProductOwnerCheck));
                System.Boolean bProductTypeCheck = System.Convert.ToBoolean(e.Node.GetValue(colProductTypeCheck));
                System.Boolean bCustomerCheck = System.Convert.ToBoolean(e.Node.GetValue(colCustomerCheck));
                System.Boolean bDepartCheck = System.Convert.ToBoolean(e.Node.GetValue(colDepartCheck));
                System.Boolean bProductSubTypeCheck = System.Convert.ToBoolean(e.Node.GetValue(colProductSubTypeCheck));

                int Y = e.SelectRect.Top + (e.SelectRect.Height - imglNodes.Images[0].Height) / 2;

                if ((bProductOwnerCheckCheck == false) || (bProductTypeCheck == false) ||
                    (bCustomerCheck == false) || (bDepartCheck == false) || (bProductSubTypeCheck == false))
                {
                    try
                    {
                        //ControlPaint.DrawImageDisabled(e.Graphics, imglNodes.Images[1], e.SelectRect.X, Y, Color.Black);
                        e.Graphics.DrawImage(imglNodes.Images[1], new Point(e.SelectRect.X, Y));
                        e.Handled = true;
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        e.Graphics.DrawImage(imglNodes.Images[0], new Point(e.SelectRect.X, Y));
                        e.Handled = true;
                    }
                    catch { }
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "treeListImportPlan_CustomDrawNodeImages\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Импорт данных в приложение к плану
        /// <summary>
        /// Импорт данных в приложение к плану продаж
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все записи импортированы; false - не все записи импортированы</returns>
        private System.Boolean ImportDataToPlanItemList(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (m_objPlanItemList == null) { m_objPlanItemList = new List<CPlanByDepartCustomerSubTypeItem>(); }
                //m_objPlanItemList.Clear();

                List<DevExpress.XtraTreeList.Nodes.TreeListNode> objNodesNeedRemoveList = new List<DevExpress.XtraTreeList.Nodes.TreeListNode>();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListImportPlan.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    
                    m_objPlanItemList.Add((CPlanByDepartCustomerSubTypeItem)objNode.Tag);
                    objNodesNeedRemoveList.Add( objNode );
                }

                this.tableLayoutPanel1.SuspendLayout();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNodeRemoved in objNodesNeedRemoveList)
                {
                    treeListImportPlan.Nodes.Remove(objNodeRemoved);
                }
                this.tableLayoutPanel1.ResumeLayout(false);

                if (treeListImportPlan.Nodes.Count > 0)
                {
                    strErr += ( "Не все записи импортированы.\nПроверьте данные и повторите попытку." );
                }
                else
                {
                    strErr += ("Все записи успешно импортированы.");
                    bRet = true;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "ImportDataToPlanItemList.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return bRet;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (treeListImportPlan.Nodes.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Список для импорта пуст.\nОперация отменена.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    
                    return;
                }

                System.String strErr = System.String.Empty;

                if (ImportDataToPlanItemList(ref strErr) == true)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnOk_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            return;
        }

        #endregion 

    }
}
