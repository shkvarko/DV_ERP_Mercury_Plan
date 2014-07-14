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

namespace ERPMercuryPlan
{
    public partial class frmAddPartSubtypeInPriceListCalc : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CSettingForCalcPrice> m_objSettingForCalcPriceList;
        private List<CProductSubType> m_objProductSubTypeList;
        private List<CPriceType> m_objPriceTypeList;
        private System.String m_strFileCalcPriceName;
        #endregion

        public frmAddPartSubtypeInPriceListCalc(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objSettingForCalcPriceList = null;
            m_objProductSubTypeList = null;
            m_objPriceTypeList = null;
            m_strFileCalcPriceName = "";
        }

        #region Список с расчётами цен
        /// <summary>
        /// Загружает список расчётоа цен
        /// </summary>
        private void LoadCalcPriceList()
        {
            try
            {
                memoEditLog.Text = "";
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

        #endregion

        #region Настройки для расчёта цен
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

                CalcPriceForPartSubType((CPriceListCalc)treeListCalcPriceList.FocusedNode.Tag);
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
                    if (System.IO.File.Exists(strFileName) == true) { System.IO.File.Delete(strFileName); }

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

        #region OpenForm
        public void OpenForm( List<CProductSubType> objProductSubTypeList, List<CPriceType> objPriceTypeList )
        {
            try
            {
                m_objProductSubTypeList = objProductSubTypeList;
                m_objPriceTypeList = objPriceTypeList;

                LoadCalcPriceList();

                ShowDialog();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("OpenForm. Текст ошибки: " + f.Message);
            }

            return;

        }
        #endregion

        #region Выгрузить данные в расчёт
        private void btnCalcPrice_Click(object sender, EventArgs e)
        {
            try
            {
                if ((clstSheets.SelectedIndex < 0) || (treeListSettings.Tag == null))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Укажите, пожалуйста, лист с расчётом цен.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {

                    AddPartSubTypeListToExcel2(m_objProductSubTypeList, m_strFileCalcPriceName, (clstSheets.SelectedIndex + 1),
                        System.Convert.ToDouble(calcCurRatePrice.Value), (CSettingForCalcPrice)treeListSettings.Tag, m_objPriceTypeList);
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

        private void AddPartSubTypeListToExcel2(List<CProductSubType> objSubTypeList,
            System.String strFileName, System.Int32 iSheetNum, System.Double dblCurrentPriceCreateRate,
            CSettingForCalcPrice objSettingForCalcPrice, List<ERP_Mercury.Common.CPriceType> objPriceTypeList)
        {
            if ((strFileName == "") || (iSheetNum < 1)) { return; }
            if( (objSubTypeList == null) || (objSubTypeList.Count == 0 )) { return; }
            treeListPartSubType.Nodes.Clear();

            Excel.Application oXL = null;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            System.Int32 iStartRow = objSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameStartRow);
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;
            CPriceListCalcItem objCPriceListCalcItem = null;
            CPrice objPrice = null;
            memoEditLog.Text = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                oSheet = (Excel._Worksheet)oWB.Worksheets[iSheetNum];

                System.String strProductOwnerSearch = "";
                System.String strProductTypeSearch = "";

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

                memoEditLog.Text += ("\r\n" + "на обработку: " + objSubTypeList.Count.ToString() + " записей");
                System.Int32 iCurrentIndex = 0;

                foreach (CProductSubType objProductSubType in objSubTypeList)
                {
                    iCurrentIndex++;
                    memoEditLog.Text += ("\r\n" + "обрабатывается запись: " + iCurrentIndex.ToString() );

                    bFindProductGroup = false;
                    strTmp = "";
                    bStopRead = false;
                    bDoubling = false;
                    iNumOfRowForCopy = 0;
                    bNeedAdd = false;
                    bPartSubTypeExistInSheet = false;
                    strProductTypeSearch = objProductSubType.ProductType;
                    strProductOwnerSearch = objProductSubType.ProductOwner;
                    strProductTypeCurrent = "";
                    strProductOwnerCurrent = "";

                    iCurrentRow = iStartRow;

                    if ((objProductSubType.VendorTariff == 0) || (dblCurrentPriceCreateRate == 0) || ( objProductSubType.NDS == 0 ) )
                    {
                        memoEditLog.Text += ("\r\n" + objProductSubType.Name + " - позиция в расчёт не включена (нулевое значение тарифа, либо курса ценообразования, либо ставки НДС.)");
                        continue;
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
                            if ((strProductTypeCurrent != strProductTypeSearch) && (bFindProductGroup == false))
                            {
                                // мы прошли весь список и нужной группы не нашли
                                if (System.Convert.ToString(oSheet.get_Range(oSheet.Cells[iStartRow, iColumnProductOwner], oSheet.Cells[iStartRow, iColumnProductOwner]).Value2) == "")
                                {
                                    oSheet.Cells[iStartRow, iColumnProductOwner] = strProductOwnerSearch;
                                    oSheet.Cells[iStartRow, iColumnProductSubType] = strProductTypeSearch;

                                    oSheet.get_Range(oSheet.Cells[iStartRow, iColumnProductOwner], oSheet.Cells[iStartRow, iColumnProductSubType]).Font.Underline = true;
                                    oSheet.get_Range(oSheet.Cells[iStartRow, iColumnProductOwner], oSheet.Cells[iStartRow, iColumnProductSubType]).Font.Italic = true;
                                    oSheet.get_Range(oSheet.Cells[iStartRow, iColumnProductOwner], oSheet.Cells[iStartRow, iColumnProductSubType]).Font.Size = 14;
                                    oSheet.get_Range(oSheet.Cells[iStartRow, iColumnProductOwner], oSheet.Cells[iStartRow, iColumnProductSubType]).Font.Bold = true;
                                    oSheet.get_Range(oSheet.Cells[iStartRow, iColumnProductOwner], oSheet.Cells[iStartRow, iColumnProductSubType]).Interior.Pattern = Excel.XlPattern.xlPatternSolid;
                                    oSheet.get_Range(oSheet.Cells[iStartRow, iColumnProductOwner], oSheet.Cells[iStartRow, iColumnProductSubType]).Interior.PatternColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                                    oSheet.get_Range(oSheet.Cells[iStartRow, iColumnProductOwner], oSheet.Cells[iStartRow, iColumnProductSubType]).Interior.Color = 12632256;

                                    iCurrentRow = (iStartRow + 1);
                                    oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.Name;
                                    oSheet.Cells[iCurrentRow, iColumnProductSubTypeId] = objProductSubType.ID_Ib;
                                    oSheet.Cells[iCurrentRow, iColumnProductSubTypeState] = objProductSubType.SubTypeStateName;

                                    // тариф
                                    oSheet.Cells[iCurrentRow, iColumnVendorTarif] = objProductSubType.VendorTariff;
                                    // % расходов на транспорт
                                    oSheet.Cells[iCurrentRow, iColumnTransportTarif] = objProductSubType.TransportTariff;
                                    // Таможенная пошлина, %
                                    oSheet.Cells[iCurrentRow, iColumnCustomTarif] = objProductSubType.CustomsTariff;
                                    // Маржа, %
                                    oSheet.Cells[iCurrentRow, iColumnMargin] = objProductSubType.Margin;
                                    // Ставка НДС, %
                                    oSheet.Cells[iCurrentRow, iColumnNDS] = objProductSubType.NDS;
                                    // Курс ценообразования (текущий)
                                    oSheet.Cells[iCurrentRow, iColumnCurrRate] = dblCurrentPriceCreateRate;
                                    // Дисконт, %
                                    oSheet.Cells[iCurrentRow, iColumnDiscont] = objProductSubType.Discont;
                                    // Требуемая наценка, %
                                    oSheet.Cells[iCurrentRow, iColumnMarkUpReqiured] = objProductSubType.MarkUpRequired;

                                    oSheet.Calculate();

                                    objCPriceListCalcItem = new CPriceListCalcItem();
                                    objCPriceListCalcItem.objProductSubType = objProductSubType;
                                    objCPriceListCalcItem.PriceCurrencyRate = dblCurrentPriceCreateRate;
                                    objCPriceListCalcItem.PriceList = new List<CPrice>();
                                    foreach (ERP_Mercury.Common.CPriceType objPriceType in objPriceTypeList)
                                    {
                                        objPrice = new CPrice();
                                        objPrice.PriceType = objPriceType;
                                        try
                                        {
                                            objPrice.PriceValue = System.Convert.ToDouble(oSheet.get_Range(oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID], oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID]).Value2);
                                        }
                                        catch (System.Exception f)
                                        {
                                            objPrice.PriceValue = 0;
                                            SendMessageToLog( objProductSubType.Name + " " + objPriceType.Name + " Текст ошибки: " + f.Message);
                                        }
                                        objCPriceListCalcItem.PriceList.Add(objPrice);
                                    }

                                    treeListPartSubType.AppendNode(new object[] { objProductSubType.Name }, null).Tag = objCPriceListCalcItem;
                                    bStopRead = true;
                                    continue;
                                }
                                else
                                {
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
                                }
                                

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
                            objCPriceListCalcItem.objProductSubType = objProductSubType;
                            objCPriceListCalcItem.PriceCurrencyRate = dblCurrentPriceCreateRate;
                            objCPriceListCalcItem.PriceList = new List<CPrice>();
                            foreach (ERP_Mercury.Common.CPriceType objPriceType in objPriceTypeList)
                            {
                                objPrice = new CPrice();
                                objPrice.PriceType = objPriceType;
                                try
                                {
                                    objPrice.PriceValue = System.Convert.ToDouble(oSheet.get_Range(oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID], oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID]).Value2);
                                }
                                catch (System.Exception f)
                                {
                                    objPrice.PriceValue = 0;
                                    SendMessageToLog(objProductSubType.Name + " " + objPriceType.Name + " Текст ошибки: " + f.Message);
                                }
                                objCPriceListCalcItem.PriceList.Add(objPrice);
                            }

                            treeListPartSubType.AppendNode(new object[] { objProductSubType.Name }, null).Tag = objCPriceListCalcItem;

                        }
                        else if (bPartSubTypeExistInSheet == true)
                        {
                            oSheet.Cells[iCurrentRow, iColumnProductSubType] = objProductSubType.Name;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeId] = objProductSubType.ID_Ib;
                            oSheet.Cells[iCurrentRow, iColumnProductSubTypeState] = objProductSubType.SubTypeStateName;
                            // тариф
                            oSheet.Cells[iCurrentRow, iColumnVendorTarif] = objProductSubType.VendorTariff;
                            // % расходов на транспорт
                            oSheet.Cells[iCurrentRow, iColumnTransportTarif] = objProductSubType.TransportTariff;
                            // Таможенная пошлина, %
                            oSheet.Cells[iCurrentRow, iColumnCustomTarif] = objProductSubType.CustomsTariff;
                            // Маржа, %
                            oSheet.Cells[iCurrentRow, iColumnMargin] = objProductSubType.Margin;
                            // Ставка НДС, %
                            oSheet.Cells[iCurrentRow, iColumnNDS] = objProductSubType.NDS;
                            // Курс ценообразования (текущий)
                            oSheet.Cells[iCurrentRow, iColumnCurrRate] = dblCurrentPriceCreateRate;
                            // Дисконт, %
                            oSheet.Cells[iCurrentRow, iColumnDiscont] = objProductSubType.Discont;
                            // Требуемая наценка, %
                            oSheet.Cells[iCurrentRow, iColumnMarkUpReqiured] = objProductSubType.MarkUpRequired;

                            oSheet.Calculate();

                            objCPriceListCalcItem = new CPriceListCalcItem();
                            objCPriceListCalcItem.objProductSubType = objProductSubType;
                            objCPriceListCalcItem.PriceCurrencyRate = dblCurrentPriceCreateRate;
                            objCPriceListCalcItem.PriceList = new List<CPrice>();
                            foreach (ERP_Mercury.Common.CPriceType objPriceType in objPriceTypeList)
                            {
                                objPrice = new CPrice();
                                objPrice.PriceType = objPriceType;
                                try
                                {
                                    objPrice.PriceValue = System.Convert.ToDouble(oSheet.get_Range(oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID], oSheet.Cells[iCurrentRow, objPrice.PriceType.ColumnID]).Value2);
                                }
                                catch (System.Exception f)
                                {
                                    objPrice.PriceValue = 0;
                                    SendMessageToLog(objProductSubType.Name + " " + objPriceType.Name + " Текст ошибки: " + f.Message);
                                }
                                objCPriceListCalcItem.PriceList.Add(objPrice);
                            }

                            treeListPartSubType.AppendNode(new object[] { objProductSubType.Name }, null).Tag = objCPriceListCalcItem;

                        }

                        iCurrentRow++;
                    }
                }

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

                if (treeListPartSubType.Nodes.Count > 0)
                {
                    tabControl.SelectedTabPage = tabPagePrices;
                    treeListPartSubType.FocusedNode = treeListPartSubType.Nodes[0];
                    ShowPriceListCalcItem((CPriceListCalcItem)treeListPartSubType.FocusedNode.Tag);
                }

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
                this.tableLayoutPanel4.SuspendLayout();
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

                this.tableLayoutPanel4.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListCalcPrice)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отображения цен для подгруппы. Текст ошибки: " + f.Message);
            }
            return;
        }

        private void treeListPartSubType_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (e.Node == null) { return; }
                if (e.Node.Tag == null) { return; }
                {
                    ShowPriceListCalcItem((CPriceListCalcItem)e.Node.Tag);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отображения цен для подгруппы. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Сохраняет цены в расчёте и в прайсе
        /// </summary>
        /// <param name="objPriceListCalc">расчёт цен</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        private System.Boolean SavePartSubtypePricesInPriceList(CPriceListCalc objPriceListCalc)
        {
            System.Boolean bRet = false;
            try
            {
                if (treeListCalcPrice.Nodes.Count == 0) { return bRet; }
                if (treeListCalcPrice.Tag == null) { return bRet; }
                if (clstSheets.Tag == null) { return bRet; }

                System.String strErr = "";

                // сохраняем файл MS Excel с расчетом в БД
                objPriceListCalc.UploadFileXlsToDatabase(m_strFileCalcPriceName, m_objProfile, null, ref strErr);

                // в расчет нужно добавить строку с нашей подгруппой и ценами
                List<CPriceListCalcItem> objPriceListCalcItemList = new List<CPriceListCalcItem>();
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPartSubType.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objPriceListCalcItemList.Add((CPriceListCalcItem)objNode.Tag);
                }

                if (CPriceListCalc.AddPriceListCalcItem(objPriceListCalc.ID, objPriceListCalcItemList, m_objProfile, null, ref strErr) == true)
                {
                    // теперь сохраняем цены в прайс
                    // сперва в "Контракт"
                    if (CProductSubTypePriceList.SavePriceListToIB(objPriceListCalcItemList, m_objPriceTypeList, m_objProfile, ref strErr) == true)
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
                if(treeListPartSubType.Nodes.Count == 0 ) { return; }

                Cursor = Cursors.WaitCursor;

                if (SavePartSubtypePricesInPriceList((CPriceListCalc)clstSheets.Tag) == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Расчёт изменен и цены успешно переданы в прайс-лист.", "Внимание!",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка btnClose_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
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
