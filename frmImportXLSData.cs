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
namespace ERPMercuryPlan
{
    public partial class frmImportXLSData : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CPriceListCalcItem> m_objCPriceListCalcItemList;
        /// <summary>
        /// Список приложения к расчету
        /// </summary>
        public List<CPriceListCalcItem> PriceListCalcItemList
        {
            get { return m_objCPriceListCalcItemList; }
        }
        /// <summary>
        /// Список настроек
        /// </summary>
        private List<CSettingForCalcPrice> m_objSettingForCalcPriceList;
        private List<CPriceType> m_objPriceTypeList;
        private List<CProductSubType> m_objProductSubTypeList;
        private CPriceListCalc m_objPriceListCalc;
        private System.String m_strFileFullName;
        /// <summary>
        /// Имя файла расчета
        /// </summary>
        public System.String FileFullName
        {
            get { return m_strFileFullName; }
        }

        #endregion

        #region Конструктор
        public frmImportXLSData(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem, CPriceListCalc objPriceListCalc, 
            List<CProductSubType> objProductSubTypeList) 
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objPriceListCalc = objPriceListCalc;
            m_objProductSubTypeList = objProductSubTypeList;
            m_objCPriceListCalcItemList = null;
            m_objPriceTypeList = null;
            m_strFileFullName = "";
            m_objSettingForCalcPriceList = null;

            SetInitialParams();
        }
        #endregion

        #region Первоначальные установки
        private void SetInitialParams()
        {
            try
            {
                txtID_Ib.Text = "";
                clstSheets.Items.Clear();
                treeListPrices.Nodes.Clear();
                treeListSettings.Nodes.Clear();

                m_objSettingForCalcPriceList = CSettingForCalcPrice.GetSettingForCalcPriceList(m_objProfile, null);

                m_objPriceTypeList = CPriceType.GetPriceTypeList(m_objProfile, null);
                if( m_objPriceTypeList != null )
                {
                    foreach (CPriceType objPriceType in m_objPriceTypeList)
                    {
                        if (objPriceType.ColumnID != 1)
                        {
                            treeListPrices.AppendNode(new object[] { true, objPriceType.Name, objPriceType.ColumnID }, null).Tag = objPriceType;
                        }
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
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "clstSheets_SelectedIndexChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "LoadSettings.\nТекст ошибки: " + f.Message, "Ошибка",
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
        private void ReadSheetListFromXLSFile( System.String strFileName )
        {
            if (strFileName == "") { return; }
            if (System.IO.File.Exists(strFileName) == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                     String.Format("файл \"{0}\" не найден.", strFileName), "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            Excel.Application oXL = null;
            Excel._Workbook oWB;

            object m = Type.Missing;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));

                foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                {
                    clstSheets.Items.Add(objSheet.Name, true);
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
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Считывает информацию из фала MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ReadDataFromXLSFile(System.String strFileName)
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

            System.Int32 iStartRow = CSettingForCalcPrice.GetColumnNumForParam( CSettingForCalcPrice.strParamNameStartRow, treeListSettings, colSettingsName, colSettingsColumnNum );
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;

            System.String strCaption = this.Text;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.Int32 iColumnProductOwner = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductOwner, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnProductSubType = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubType, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPartTypeId = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubTypeID, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnProductSubTypeState = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameProductSubTypeState, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnVendorTarif = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameVendorTarif, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnTransportTarif = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameTransportTarif, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnTransportTarifSum = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameTransportTarifSum, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnCustomTarif = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCustomTarif, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnCustomTarifSum = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCustomTarifSum, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnMargin = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameMargin, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnNDS = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameNDS, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnNDSSum = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameNDSSum, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnDiscont = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameDiscont, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnCurrRate = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameCurRate, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnMarkUpReqiured = CSettingForCalcPrice.GetColumnNumForParam(CSettingForCalcPrice.strParamNameMarkUpReqiured, treeListSettings, colSettingsName, colSettingsColumnNum);

                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                if (m_objCPriceListCalcItemList == null) { m_objCPriceListCalcItemList = new List<CPriceListCalcItem>(); }
                else { m_objCPriceListCalcItemList.Clear(); }
                

                System.Boolean bNeedreadData = false;
                System.Boolean bStopRead = false;
                CPriceListCalcItem objCPriceListCalcItem = null;
                CPrice objPrice = null;
                System.String strTmp = "";
                

                foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                {
                    //Excel._Worksheet objSheet2 = (Excel._Worksheet)oWB.Worksheets[1];
                    this.Text = "обрабатывается лист " + objSheet.Name + "...";
                    this.Refresh();
                    bNeedreadData = false;
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in clstSheets.CheckedItems)
                    {
                        if ( ( (System.String)objListBoxItem.Value) == objSheet.Name)
                        {
                            bNeedreadData = true;
                            break;
                        }
                    }
                    if (bNeedreadData == true)
                    {
                        // этот лист есть в списке выбранных, поэтому можносчитывать с него данные
                        iCurrentRow = iStartRow;
                        bStopRead = false;
                        //strTmp = System.Convert.ToString(objSheet2.get_Range(objSheet2.Cells[iCurrentRow, 2], objSheet2.Cells[iCurrentRow, 2]).Value2);
                        while (bStopRead == false)
                        {
                            // пробежим по строкам и считаем информацию
                            strTmp = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, 2], objSheet.Cells[iCurrentRow, 2]).Value2);
                            if (strTmp == "")
                            {
                                bStopRead = true;
                            }
                            else
                            {
                                if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartTypeId], objSheet.Cells[iCurrentRow, iColumnPartTypeId]).Value2) == "")
                                {
                                    iCurrentRow++;
                                    continue;
                                }

                                objCPriceListCalcItem = new CPriceListCalcItem();
                                objCPriceListCalcItem.objProductSubType = new CProductSubType();
                                objCPriceListCalcItem.objProductSubType.ID_Ib = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartTypeId], objSheet.Cells[iCurrentRow, iColumnPartTypeId]).Value2);

                                foreach (CProductSubType objProductSubType in m_objProductSubTypeList)
                                {
                                    if (objProductSubType.ID_Ib == objCPriceListCalcItem.objProductSubType.ID_Ib)
                                    {
                                        objCPriceListCalcItem.objProductSubType.ID = objProductSubType.ID;
                                        objCPriceListCalcItem.objProductSubType.ProductLine = objProductSubType.ProductLine;
                                        objCPriceListCalcItem.objProductSubType.ProductOwner = objProductSubType.ProductOwner;
                                        break;
                                    }
                                }

                                objCPriceListCalcItem.objProductSubType.Name = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnProductSubType], objSheet.Cells[iCurrentRow, iColumnProductSubType]).Value2);
                                objCPriceListCalcItem.objProductSubType.VendorTariff = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnVendorTarif], objSheet.Cells[iCurrentRow, iColumnVendorTarif]).Value2);
                                objCPriceListCalcItem.objProductSubType.TransportTariff = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnTransportTarif], objSheet.Cells[iCurrentRow, iColumnTransportTarif]).Value2);
                                objCPriceListCalcItem.TransportCost = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnTransportTarifSum], objSheet.Cells[iCurrentRow, iColumnTransportTarifSum]).Value2);
                                objCPriceListCalcItem.objProductSubType.CustomsTariff = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnCustomTarif], objSheet.Cells[iCurrentRow, iColumnCustomTarif]).Value2);
                                objCPriceListCalcItem.CustomCost = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnCustomTarifSum], objSheet.Cells[iCurrentRow, iColumnCustomTarifSum]).Value2);
                                objCPriceListCalcItem.objProductSubType.Margin = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnMargin], objSheet.Cells[iCurrentRow, iColumnMargin]).Value2);
                                objCPriceListCalcItem.objProductSubType.NDS = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnNDS], objSheet.Cells[iCurrentRow, iColumnNDS]).Value2);
                                objCPriceListCalcItem.NDSCost = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnNDSSum], objSheet.Cells[iCurrentRow, iColumnNDSSum]).Value2);
                                objCPriceListCalcItem.objProductSubType.Discont = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnDiscont], objSheet.Cells[iCurrentRow, iColumnDiscont]).Value2);
                                objCPriceListCalcItem.PriceCurrencyRate = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnCurrRate], objSheet.Cells[iCurrentRow, iColumnCurrRate]).Value2);
                                objCPriceListCalcItem.objProductSubType.MarkUpRequired = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnMarkUpReqiured], objSheet.Cells[iCurrentRow, iColumnMarkUpReqiured]).Value2);
                                // теперь цены                                
                                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPrices.Nodes)
                                {
                                    objPrice = null;
                                    if ( ( System.Convert.ToBoolean(objNode.GetValue(colCheck)) == true ) && ( objNode.Tag != null ) )
                                    {
                                        if (objCPriceListCalcItem.PriceList == null) { objCPriceListCalcItem.PriceList = new List<CPrice>(); }
                                        objPrice = new CPrice();
                                        objPrice.PriceType = (CPriceType)objNode.Tag;
                                        objPrice.PriceValue = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, System.Convert.ToInt32( objNode.GetValue(colColumnPriceNum) )], objSheet.Cells[iCurrentRow, System.Convert.ToInt32( objNode.GetValue(colColumnPriceNum) )]).Value2);
                                        objCPriceListCalcItem.PriceList.Add(objPrice);
                                    }
                                }

                                iCurrentRow++;
                                m_objCPriceListCalcItemList.Add(objCPriceListCalcItem);
                            }

                        }
                    }

                }

                oWB.Close( false, Missing.Value, Missing.Value);
                oXL.Quit();
            }
            catch (System.Exception f)
            {
                oWB = null;
                oXL = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oWB = null;
                oXL = null;
                this.Text = strCaption;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        #endregion

        #region Подтвердить выбор
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID_Ib.Text == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Укажите файл шаблона MS Excel.", "Предупреждение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                m_strFileFullName = txtID_Ib.Text;

                ReadDataFromXLSFile(m_strFileFullName);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnOk_Click.\nТекст ошибки: " + f.Message, "Ошибка",
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

        #region Выделить всё/ Отменить всё
        private void mitemSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (System.Int32 i = 0; i < clstSheets.Items.Count; i++)
                {
                    clstSheets.Items[i].CheckState = CheckState.Checked;
                }

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "mitemSelectAll_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                for (System.Int32 i = 0; i < clstSheets.Items.Count; i++)
                {
                    clstSheets.Items[i].CheckState = CheckState.Unchecked;
                }

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "mitemDeselectAll_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }

        private void SelectDeselectNodes(System.Boolean bSelect)
        {
            try
            {
                tableLayoutPanel5.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).BeginInit();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPrices.Nodes)
                {
                    if (System.Convert.ToBoolean(objNode.GetValue(colCheck)) != bSelect)
                    {
                        objNode.SetValue(colCheck, bSelect);
                    }
                }

                ((System.ComponentModel.ISupportInitialize)(this.treeListPrices)).EndInit();
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

    }
}
