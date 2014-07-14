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
    public partial class frmImportInPartsPriceList : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства

        private UniXP.Common.CProfile m_objProfile;
        private List<CPriceType> m_objPriceTypeList;
        private CSettingForImportData m_objImportSetting;
        private List<CPartsPriceListItem> m_objPartsPriceList;
        private System.Boolean m_bSettingsForImportIsOK;

        private const System.String m_srtSettingImportPriceInPatrsPriceListName = "ImportPriceInProductPriceList";
        private const System.String m_strImportMode_1 = "Поиск товара происходит по коду, указанному в файле.\r\nКод товара соотвествует коду в справочнике товаров \"UniXP\".";
        private const System.String m_strImportMode_2 = "Поиск товара происходит по коду, указанному в файле.\r\nКод товара соотвествует коду в справочнике товаров поставщика.";
        private const System.String m_strImportMode_Error = "Внимание! Цена, указанная в настройках не зарегистрирована в \"UniXP\"";
        private const System.Int32 m_iImportMode_2 = 2;
        private const System.String m_strPricePrefix = "PRICE";
        private const System.String m_strWarningPriceNotFound = "цена не найдена в справочнике";
        private const System.String m_strWarningProductIdUnDefined = "значение не удалось преобразовать в число";
        private const System.String m_strWarningProductNotFound = "товар с указанным кодом не найден в справочнике";
        private const System.String m_strTools_PARTS_ID = "PARTS_ID";
        private const System.String m_strTools_PARTS_NAME = "PARTS_NAME";
        private const System.String m_strTools_PARTS_ARTICLE = "PARTS_ARTICLE";
        private const System.String m_strTools_OWNER_NAME = "OWNER_NAME";
        private const System.String m_strTools_STARTROW = "STARTROW";

        private const System.Int32 m_iNodeImgIndexOK = 0;
        private const System.Int32 m_iNodeImgIndexFalse = 1;
        #endregion

        #region Конструктор
        public frmImportInPartsPriceList(UniXP.Common.CProfile objProfile)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objPriceTypeList = null;
            m_objImportSetting = null;
            m_objPartsPriceList = null;
            radioGroupImportMode.SelectedIndex = 0;
            m_bSettingsForImportIsOK = false;
        }
        #endregion

        #region Загрузка формы

        public void OpenForm()
        {
            try
            {
                SetInitialParams();

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "OpenForm.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Первоначальные установки
        private void SetInitialParams()
        {
            try
            {
                // список типов цен
                m_objPriceTypeList = CPriceType.GetPriceTypeList(m_objProfile, null);

                m_bSettingsForImportIsOK = true;

                txtID_Ib.Text = "";
                cboxSheetList.Properties.Items.Clear();
                treeListSettings.Nodes.Clear();
                treeListPriceEditor.Nodes.Clear();
                treeListProducts.Nodes.Clear();

                btnImport.Enabled = false;
                memoEditLog.Text = "";


                m_objImportSetting = CSettingForImportData.GetSettingForImportData( m_objProfile, null );

                if (m_objImportSetting != null)
                {
                    // заполняем список настроек
                    foreach (CSettingItemForImportData objSettingItem in m_objImportSetting.SettingsList)
                    {
                        if ((objSettingItem.TOOLS_NAME.Contains(m_strPricePrefix) == true) && (objSettingItem.TOOLSTYPE_GUID.CompareTo(System.Guid.Empty) != 0))
                        {
                            // элемент насройки относится к цене
                            // необходимо проверить, зарегистрирован ли в системе тип цены с указанным идентификатором
                            if (m_objPriceTypeList.SingleOrDefault<CPriceType>(x => x.ID.CompareTo(objSettingItem.TOOLSTYPE_GUID) == 0) != null)
                            {
                                treeListSettings.AppendNode(new object[] { objSettingItem.TOOLS_USERNAME, objSettingItem.TOOLS_VALUE }, null).Tag = objSettingItem;
                            }
                            else
                            {
                                treeListSettings.AppendNode(new object[] { objSettingItem.TOOLS_USERNAME, m_strWarningPriceNotFound }, null).Tag = null;
                                m_bSettingsForImportIsOK = false;
                            }
                        }
                        else
                        {
                            treeListSettings.AppendNode(new object[] { objSettingItem.TOOLS_USERNAME, objSettingItem.TOOLS_VALUE }, null).Tag = objSettingItem;
                        }
                    }
                }

                SetModeImportData();
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

        private void SetModeImportData()
        {
            try
            {
                this.LblInfoImportMode.Text = "";

                if (m_bSettingsForImportIsOK == true)
                {
                    switch (radioGroupImportMode.SelectedIndex)
                    {
                        case 0:
                            LblInfoImportMode.Text = m_strImportMode_1;
                            break;
                        case 1:
                            LblInfoImportMode.Text = m_strImportMode_2;
                            break;
                        default:
                            break;
                    }

                    lblSelectedImportMode.Text = (" №" + (radioGroupImportMode.SelectedIndex + 1).ToString() + " " + radioGroupImportMode.Properties.Items[radioGroupImportMode.SelectedIndex].Description);
                }
                else
                {
                    lblSelectedImportMode.Text = m_strImportMode_Error;
                }
                btnImport.Enabled = m_bSettingsForImportIsOK;
                btnOk.Enabled = m_bSettingsForImportIsOK;
                
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SetModeImportData.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void radioGroupImportMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetModeImportData();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "radioGroupImportMode_SelectedIndexChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

                        btnImport.Enabled = true;
                        memoEditLog.Text = "";
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
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                cboxSheetList.Properties.Items.Clear();

                foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                {
                    cboxSheetList.Properties.Items.Add(objSheet.Name);
                }

                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oXL.Quit();

                m = null;
                oWB = null;
                oXL = null;
            }
            catch (System.Exception f)
            {
                oXL = null;
                oWB = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не удалось получить список листов в файле.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (cboxSheetList.Properties.Items.Count > 0) { cboxSheetList.SelectedIndex = 0; }
                treeListProducts.Nodes.Clear();
                treeListPriceEditor.Nodes.Clear();

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
            memoEditLog.Text = "";

            this.tableLayoutPanel2.SuspendLayout();

            Excel.Application oXL = null;
            Excel._Workbook oWB;

            System.Int32 iStartRow = m_objImportSetting.SettingsList.SingleOrDefault<CSettingItemForImportData>(x => x.TOOLS_NAME == m_strTools_STARTROW).TOOLS_VALUE;
            System.Int32 iColumnProductId = m_objImportSetting.SettingsList.SingleOrDefault<CSettingItemForImportData>(x => x.TOOLS_NAME == m_strTools_PARTS_ID).TOOLS_VALUE;
            System.Int32 iclmnProductName = m_objImportSetting.SettingsList.Single<CSettingItemForImportData>(x => x.TOOLS_NAME.ToUpper().Trim().Contains( m_strTools_PARTS_NAME) == true).TOOLS_VALUE;
            System.Int32 iclmnProductArticle = m_objImportSetting.SettingsList.SingleOrDefault<CSettingItemForImportData>(x => x.TOOLS_NAME == m_strTools_PARTS_ARTICLE).TOOLS_VALUE;
            System.Int32 iclmnOwnerName = m_objImportSetting.SettingsList.SingleOrDefault<CSettingItemForImportData>(x => x.TOOLS_NAME == m_strTools_OWNER_NAME).TOOLS_VALUE;
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (m_objPartsPriceList == null) { m_objPartsPriceList = new List<CPartsPriceListItem>(); }
                else { m_objPartsPriceList.Clear(); }

                treeListProducts.Nodes.Clear();

                System.Int32 iPartsId = 0;
                System.Int32 iColumnPrice = 0;

                foreach (CSettingItemForImportData objItem in m_objImportSetting.SettingsList)
                {
                    if (objItem.TOOLS_NAME == m_strTools_PARTS_NAME)
                    {
                        iclmnProductName = objItem.TOOLS_VALUE;
                    }
                    if (objItem.TOOLS_NAME == m_strTools_PARTS_ARTICLE)
                    {
                        iclmnProductArticle = objItem.TOOLS_VALUE;
                    }
                    if (objItem.TOOLS_NAME == m_strTools_OWNER_NAME)
                    {
                        iclmnOwnerName = objItem.TOOLS_VALUE;
                    }
                }

                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));

                System.Boolean bStopRead = false;
                System.String strTmp = "";
                System.String strPriceValue = "";
                System.String strErr = "";
                System.Boolean bIsAdvCode = ( radioGroupImportMode.SelectedIndex == 1 );

                Excel._Worksheet objSheet = (Excel._Worksheet)oWB.Worksheets[this.cboxSheetList.SelectedIndex + 1];
                CPartsPriceListItem objPartsPriceListItem = null;
                CProduct objProduct = null;
                CPrice objPrice = null;

                List<CPriceType> objPriceTypeList = new List<CPriceType>(); 
                List<CSettingItemForImportData> objSettingsList =  m_objImportSetting.SettingsList.Where( x=>x.TOOLSTYPE_GUID != System.Guid.Empty ).ToList<CSettingItemForImportData>();
                foreach( CSettingItemForImportData objItem in objSettingsList )
                {
                    objPriceTypeList.Add( m_objPriceTypeList.SingleOrDefault<CPriceType>(x=>x.ID == objItem.TOOLSTYPE_GUID ) );
                }
                objSettingsList = null;

                iCurrentRow = iStartRow;
                bStopRead = false;
                //strTmp = System.Convert.ToString(objSheet2.get_Range(objSheet2.Cells[iCurrentRow, 2], objSheet2.Cells[iCurrentRow, 2]).Value2);
                while (bStopRead == false)
                {
                    // пробежим по строкам и считаем информацию
                    objProduct = null;
                    objPrice = null;
                    objPartsPriceListItem = null;
                    iPartsId = 0;
                    strPriceValue = "";

                    strTmp = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnProductId], objSheet.Cells[iCurrentRow, iColumnProductId]).Value2);
                    if (strTmp == "")
                    {
                        bStopRead = true;
                    }
                    else
                    {
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnProductId], objSheet.Cells[iCurrentRow, iColumnProductId]).Value2) == "")
                        {
                            iCurrentRow++;
                            continue;
                        }

                        // преобразуем строку в код товара
                        try
                        {
                            iPartsId = System.Convert.ToInt32(strTmp);
                        }
                        catch
                        {
                            iPartsId = 0;
                        }

                        if (iPartsId == 0)
                        {
                            treeListProducts.AppendNode(new object[] { null, strTmp, m_strWarningProductIdUnDefined, null }, null).Tag = null;
                            memoEditLog.Text += "\r\n" + strTmp + " " + m_strWarningProductIdUnDefined;
                            iCurrentRow++;
                            continue;
                        }

                        // запрос информации о товаре по коду товара
                        objProduct = ERP_Mercury.Common.CProduct.FindProductByPartsId(m_objProfile, null, iPartsId, bIsAdvCode, ref strErr);
                        if (objProduct == null)
                        {
                            treeListProducts.AppendNode(new object[] { null, strTmp, m_strWarningProductNotFound, null }, null).Tag = null;
                            memoEditLog.Text += "\r\n" + strTmp + " " + m_strWarningProductNotFound;
                            iCurrentRow++;
                            continue;
                        }

                        objPartsPriceListItem = new CPartsPriceListItem();
                        objPartsPriceListItem.Product = objProduct;
                        objPartsPriceListItem.PriceList = new List<CPrice>();
                        iColumnPrice = 0;

                        // список прайсов (типов цен)
                        foreach (CPriceType objPriceType in objPriceTypeList)
                        {
                            objPrice = new CPrice();
                            objPrice.PriceType = objPriceType;
                            iColumnPrice = m_objImportSetting.SettingsList.Single<CSettingItemForImportData>( x=>x.TOOLSTYPE_GUID == objPriceType.ID ).TOOLS_VALUE;
                            strPriceValue = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPrice], objSheet.Cells[iCurrentRow, iColumnPrice]).Value2);
                            try
                            {
                                objPrice.PriceValue = System.Convert.ToDouble(strPriceValue);
                                objPartsPriceListItem.PriceList.Add(objPrice);
                            }
                            catch
                            {
                                objPrice.PriceValue = 0;
                            }
                        }

                        treeListProducts.AppendNode(new object[] { objPartsPriceListItem.Product.ProductTradeMarkName, 
                            objPartsPriceListItem.Product.ID_Ib, objPartsPriceListItem.Product.Name, objPartsPriceListItem.Product.Article }, null).Tag = objPartsPriceListItem;

                        iCurrentRow++;
                    }

                }

                objSheet = null;
                objPartsPriceListItem = null;
                objProduct = null;
                objPrice = null;
                objPriceTypeList = null; 

                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oXL.Quit();

                m = null;
                oWB = null;
                oXL = null;
            
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
                this.tableLayoutPanel2.ResumeLayout(false);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                treeListProducts.FocusedNode = ((treeListProducts.Nodes.Count == 0) ? null : treeListProducts.Nodes[0]);
            }


            return;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID_Ib.Text != "")
                {
                    treeListPriceEditor.Nodes.Clear();
                    ReadDataFromXLSFile(txtID_Ib.Text);

                    if ((treeListProducts.Nodes.Count > 0) && (treeListProducts.FocusedNode != null) &&
                        (treeListProducts.FocusedNode.Tag != null) && (treeListProducts.FocusedNode.Tag.GetType().Name == "CPartsPriceListItem"))
                    {
                        ShowPricesForProduct((CPartsPriceListItem)treeListProducts.FocusedNode.Tag);
                    }
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnImport_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void gridViewProductList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "IsCheck")
                {
                    System.Drawing.Image img = ERPMercuryPlan.Properties.Resources.warning;
                    if ((e.CellValue != null) && (System.Convert.ToBoolean(e.CellValue) == false))
                    {
                        Rectangle rImg = new Rectangle(e.Bounds.X - 6 + e.Bounds.Width / 2, e.Bounds.Y + (e.Bounds.Height - img.Size.Height) / 2, img.Width, img.Height);
                        e.Graphics.DrawImage(img, rImg);
                        Rectangle r = e.Bounds;
                        e.Handled = true;
                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("gridViewProductList_CustomDrawCell\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Загружает в дерево список цен для товара
        /// </summary>
        /// <param name="objPartsPriceListItem">объект класса "Строка в прайс-листе по товарам"</param>
        private void ShowPricesForProduct(CPartsPriceListItem objPartsPriceListItem)
        {
            try
            {
                treeListPriceEditor.Nodes.Clear();
                if (objPartsPriceListItem == null) { return; }
                if (objPartsPriceListItem.PriceList == null) { return; }

                foreach (CPrice objPrice in objPartsPriceListItem.PriceList)
                {
                    treeListPriceEditor.AppendNode(new object[] { objPrice.PriceType.Name, objPrice.PriceValue }, null).Tag = objPrice;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("ShowPricesForProduct\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        private void treeListProducts_AfterFocusNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            try
            {
                if( (e.Node == null) || (e.Node.Tag == null) ) { return; }

                ShowPricesForProduct((CPartsPriceListItem)e.Node.Tag);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("treeListProducts_AfterFocusNode\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        private void treeListProducts_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            try
            {
                e.SelectImageIndex = (((e.Node == null) || (e.Node.Tag == null)) ? m_iNodeImgIndexFalse : m_iNodeImgIndexOK);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("treeListProducts_CustomDrawNodeImages\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        #endregion

        #region Закрыть форму
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("btnCancel_Click\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        private void CloseForm()
        {
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("CloseForm\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        #endregion

        #region Импорт цен в базу данных
        /// <summary>
        /// Сохраняет изменения в прайс-листе
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean ImportPriceListToDB( ref System.String strErr )
        {
            System.Boolean bRet = false;
            if (treeListProducts.Nodes.Count == 0) { return bRet; }
            try
            {
                List<CPartsPriceListItem> objPriceListItemsList = new List<CPartsPriceListItem>();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListProducts.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objPriceListItemsList.Add((CPartsPriceListItem)objNode.Tag);
                }

                List<CPriceType> objPriceTypeList = new List<CPriceType>();
                List<CSettingItemForImportData> objSettingsList = m_objImportSetting.SettingsList.Where(x => x.TOOLSTYPE_GUID != System.Guid.Empty).ToList<CSettingItemForImportData>();
                foreach (CSettingItemForImportData objItem in objSettingsList)
                {
                    objPriceTypeList.Add(m_objPriceTypeList.SingleOrDefault<CPriceType>(x => x.ID == objItem.TOOLSTYPE_GUID));
                }
                objSettingsList = null;
                    
                // сперва пишем в IB
                System.Boolean bIsOkSave = CProductPriceList.SavePriceListToIB(objPriceListItemsList, objPriceTypeList, m_objProfile, ref strErr);
                if (bIsOkSave == true)
                {
                    bIsOkSave = CProductPriceList.SavePriceList(objPriceListItemsList, m_objProfile, null, ref strErr);
                }

                objPriceTypeList = null;
                objPriceListItemsList = null;

                bRet = bIsOkSave;

            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }
            return bRet;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {

                if (DevExpress.XtraEditors.XtraMessageBox.Show("В прайс-листе будут сохранены только те позиции, \nдля которых был найден товар в справочнике.\n\nПодтвердите, пожалуйста, начало операции.", "Сохранение цен в прайс-листе",
                   System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                
                System.String strErr = "";
                if (ImportPriceListToDB(ref strErr) == true)
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                else
                {
                    memoEditLog.Text += "\r\n" + strErr;

                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Ошибка",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("btnOk_Click\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        #endregion


    }
}
