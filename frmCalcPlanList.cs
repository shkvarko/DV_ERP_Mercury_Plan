using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP_Mercury.Common;
using DevExpress.XtraExport;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraEditors;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ERPMercuryPlan
{
    public partial class frmCalcPlanList : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CCalcPlan> m_objCalcPlanList;
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        private System.Boolean m_bIsComboBoxFill;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bIsChanged;
        public System.Boolean IsChanged
        {
            get { return m_bIsChanged; }
        }
        private CCalcPlan m_objSelectedCalcPlan;
        private List<CProductOwner> m_objProductOwnerList;
        private List<CCalcPlanItemProductOwner> m_objDeletedCalcPlanItemProductOwnerList;
        private List<CCalcPlanKoef> m_objCalcPlanKoefList;
        private System.Int32 m_iRowHandle;
             
        #endregion

        #region Конструктор
        public frmCalcPlanList(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;


            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objCalcPlanList = null;
            m_objSelectedCalcPlan = null;
            m_objDeletedCalcPlanItemProductOwnerList = null;
            m_bIsComboBoxFill = false;
            m_objProductOwnerList = CProductOwner.GetProductOwnerList(m_objProfile);
            m_objCalcPlanKoefList = CCalcPlanKoef.GetCalcPlanKoefList(m_objProfile, null);
            m_iRowHandle = 0;

            //txtYear.Properties.MinValue = System.DateTime.Today.Year;

            AddGridColumns();
            LoadCalcPlanList();
            m_bDisableEvents = false;
            m_bIsChanged = false;

            Text = m_objMenuItem.strName;
        }

        private System.Reflection.Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            System.Reflection.Assembly MyAssembly = null;
            System.Reflection.Assembly objExecutingAssemblies = System.Reflection.Assembly.GetExecutingAssembly();

            System.Reflection.AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            System.String strDllName = "";
            foreach (System.Reflection.AssemblyName strAssmbName in arrReferencedAssmbNames)
            {

                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    strDllName = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    break;
                }

            }
            if (strDllName != "")
            {
                System.String strFileFullName = "";
                System.Boolean bError = false;
                foreach (System.String strPath in this.m_objProfile.ResourcePathList)
                {
                    //Load the assembly from the specified path. 
                    strFileFullName = strPath + "\\" + strDllName;
                    if (System.IO.File.Exists(strFileFullName))
                    {
                        try
                        {
                            MyAssembly = System.Reflection.Assembly.LoadFrom(strFileFullName);
                            break;
                        }
                        catch (System.Exception f)
                        {
                            bError = true;
                            DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка загрузки библиотеки: " +
                                strFileFullName + "\n\nТекст ошибки: " + f.Message, "Ошибка",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                    if (bError) { break; }
                }
            }

            //Return the loaded assembly.
            if (MyAssembly == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось найти библиотеку: " +
                                strDllName, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

            }
            return MyAssembly;
        }

        #endregion

        #region Журнал сообщений
        private void SendMessageToLog(System.String strMessage)
        {
            try
            {
                m_objMenuItem.SimulateNewMessage(strMessage);
                this.Refresh();
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
                this.tableLayoutPanelCalcPlanProperties.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.cboxCurrency.Properties)).BeginInit();

                cboxCurrency.Properties.Items.Clear();
                List<CCurrency> objCurrencyList = CCurrency.GetCurrencyList(m_objProfile, null);
                if (objCurrencyList != null)
                {
                    foreach (CCurrency objCurrency in objCurrencyList)
                    {
                        cboxCurrency.Properties.Items.Add(objCurrency);
                    }
                }
                objCurrencyList = null;

                this.tableLayoutPanelCalcPlanProperties.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.cboxCurrency.Properties)).EndInit();
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

        #region Список заказов
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();
            AddGridColumn(ColumnView, "ID", "УИ плана");
            AddGridColumn(ColumnView, "Year", "Год");
            AddGridColumn(ColumnView, "Name", "Номер расчета плана");
            AddGridColumn(ColumnView, "CurrencyCode", "Валюта");
            AddGridColumn(ColumnView, "Date", "Дата");
            AddGridColumn(ColumnView, "Quantity", "Количество");
            AddGridColumn(ColumnView, "AllMoney", "Сумма");
            AddGridColumn(ColumnView, "UseForReport", "Для отчетов");
            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                objColumn.Visible = (objColumn.FieldName != "ID");
                if (objColumn.FieldName == "AllMoney")
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "{0:### ### ##0.00}";
                }
                if (objColumn.FieldName == "Quantity")
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "{0:### ### ##0}";
                }
                //if (objColumn.FieldName == "IsProductComposite")
                //{
                //    objColumn.Width = icolIsProductCompositeWidth;
                //    objColumn.OptionsColumn.FixedWidth = true;
                //}
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
        /// <summary>
        /// Загружает список планов продаж
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadCalcPlanList()
        {
            System.Boolean bRet = false;
            m_bDisableEvents = true;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Int32 iRowHandle = 0;
                if ((((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).RowCount > 0) &&
                     (((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).FocusedRowHandle >= 0))
                {
                    iRowHandle = ((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).FocusedRowHandle;
                }
                    
                ((System.ComponentModel.ISupportInitialize)(this.gridControlList)).BeginInit();

                gridControlList.DataSource = null;

                m_objCalcPlanList = CCalcPlan.GetCalcPlanList(m_objProfile, null);

                if (m_objCalcPlanList != null)
                {
                    gridControlList.DataSource = m_objCalcPlanList;

                    if (iRowHandle > 0)
                    {
                         ((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).FocusedRowHandle = iRowHandle;
                    }
                }

                ((System.ComponentModel.ISupportInitialize)(this.gridControlList)).EndInit();
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка планов продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                this.Cursor = Cursors.Default;
            }

            return bRet;
        }
        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadCalcPlanList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка. Текст ошибки: " + f.Message);
            }
        }

        #endregion

        #region Редактирование плана продаж
        /// <summary>
        /// Возвращает ссылку на выбранный в списке план продаж
        /// </summary>
        /// <returns>ссылка на заказ</returns>
        private CCalcPlan GetSelectedCalcPlan()
        {
            CCalcPlan objRet = null;
            try
            {
                if ((((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).RowCount > 0) &&
                    (((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).FocusedRowHandle >= 0))
                {
                    m_iRowHandle = ((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).FocusedRowHandle;
                    System.Guid uuidID = (System.Guid)(((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView)).GetFocusedRowCellValue("ID");

                    if ((m_objCalcPlanList != null) && (m_objCalcPlanList.Count > 0) && (uuidID.CompareTo(System.Guid.Empty) != 0))
                    {
                        foreach( CCalcPlan objCalcPlan in m_objCalcPlanList )
                        {
                            if (objCalcPlan.ID.CompareTo(uuidID) == 0)
                            {
                                objRet = objCalcPlan;
                                break;
                            }
                        }
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка поиска выбранного плана продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return objRet;
        }
        private void barButtonAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CCalcPlan objSelectedCalcPlan = new CCalcPlan();
                objSelectedCalcPlan.Year = System.DateTime.Today.Year;
                objSelectedCalcPlan.Date = System.DateTime.Today;

                LoadCalcPlanDetailInEditor(objSelectedCalcPlan);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания плана продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void gridControlList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CCalcPlan objSelectedCalcPlan = GetSelectedCalcPlan();
                LoadCalcPlanDetailInEditor(objSelectedCalcPlan);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования плана продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void barButtonEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CCalcPlan objSelectedCalcPlan = GetSelectedCalcPlan();
                LoadCalcPlanDetailInEditor(objSelectedCalcPlan);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования плана продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        /// <summary>
        /// Редактирование плана продаж
        /// </summary>
        /// <param name="objCalcPlan">выбранный план продаж</param>
        private void LoadCalcPlanDetailInEditor(CCalcPlan objCalcPlan)
        {
            try
            {
                if (objCalcPlan == null) { return; }
                m_bDisableEvents = true;
                if (m_bIsComboBoxFill == false) { LoadComboBoxItems(); }

                m_objSelectedCalcPlan = objCalcPlan;

                if (m_objDeletedCalcPlanItemProductOwnerList == null) { m_objDeletedCalcPlanItemProductOwnerList = new List<CCalcPlanItemProductOwner>(); }
                else { m_objDeletedCalcPlanItemProductOwnerList.Clear(); }

                //this.tableLayoutPanelDetail.SuspendLayout();

                txtName.Text = m_objSelectedCalcPlan.Name;
                dtBeginDate.DateTime = m_objSelectedCalcPlan.Date;
                txtYear.Value = m_objSelectedCalcPlan.Year;
                checkUseInReport.Checked = m_objSelectedCalcPlan.UseForReport;
                txtYear.Properties.ReadOnly = (m_objSelectedCalcPlan.ID.CompareTo(System.Guid.Empty) != 0);

                cboxCurrency.SelectedItem = null;
                // валюта
                if ((m_objSelectedCalcPlan.Currency != null) && (cboxCurrency.Properties.Items.Count > 0))
                {
                    foreach (object objItem in cboxCurrency.Properties.Items)
                    {
                        if (((CCurrency)objItem).ID.CompareTo(m_objSelectedCalcPlan.Currency.ID) == 0)
                        {
                            cboxCurrency.SelectedItem = objItem;
                            break;
                        }
                    }
                }
                txtDscrpn.Text = m_objSelectedCalcPlan.Description;

                // теперь список товарных марок
                treeListProductOwner.Nodes.Clear();
                if (m_objSelectedCalcPlan.CalcPlanItemProductOwner != null)
                {
                    foreach (CCalcPlanItemProductOwner objItem in m_objSelectedCalcPlan.CalcPlanItemProductOwner)
                    {
                        treeListProductOwner.AppendNode(new object[] { objItem.ProductOwner.Name, objItem.Quantity, objItem.AllMoney }, null).Tag = objItem;
                    }
                }

                //  а теперь список не задействованных товарных марок
                treeListFreeProductOwner.Nodes.Clear();
                if (m_objProductOwnerList != null)
                {
                    System.Boolean bExist = false;
                    foreach (CProductOwner objItemProductOwner in m_objProductOwnerList)
                    {
                        bExist = false;
                        if (m_objSelectedCalcPlan.CalcPlanItemProductOwner != null)
                        {
                            foreach (CCalcPlanItemProductOwner objItem in m_objSelectedCalcPlan.CalcPlanItemProductOwner)
                            {
                                if (objItem.ProductOwner.uuidID.CompareTo(objItemProductOwner.uuidID) == 0)
                                {
                                    bExist = true;
                                    break;
                                }
                            }
                        }
                        if (bExist == false)
                        {
                            treeListFreeProductOwner.AppendNode(new object[] { objItemProductOwner.Name }, null).Tag = objItemProductOwner;
                        }
                    }
                }

                // теперь список расшифровок
                tabControlCalcPlanItems.TabPages.Clear();

                SetPropertiesModified(false);

                tabControl.SelectedTabPage = tabPageProperties;
                bar1.Visible = false;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования плана продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                btnCancel.Enabled = true;
            }
            return;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConfirmCalcPlan() == true)
                {
                    foreach (DevExpress.XtraTab.XtraTabPage tabPage in tabControlCalcPlanItems.TabPages)
                    {
                        if (((ctrlCalcPlanItem)tabPage.Controls[0]).IsChanged == true)
                        {
                            ((ctrlCalcPlanItem)tabPage.Controls[0]).IsChanged = false;
                        }
                    }
                    SetPropertiesModified(false);
                    btnCancel.Enabled = true;
                    //// теперь список расшифровок
                    //tabControlCalcPlanItems.TabPages.Clear();
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSave_Click. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Сохраняет изменения в плане продаж
        /// </summary>
        private System.Boolean ConfirmCalcPlan()
        {
            System.Boolean bRet = false;
            try
            {
                if (m_objSelectedCalcPlan != null)
                {
                    System.String strErr = "";

                    if (IsAllParamFill() == true)
                    {
                        System.Boolean bIsNew = (m_objSelectedCalcPlan.ID.CompareTo(System.Guid.Empty) == 0);
                        CCalcPlan objCalcPlan = new CCalcPlan();
                        objCalcPlan.ID = m_objSelectedCalcPlan.ID;
                        objCalcPlan.Name = txtName.Text;
                        objCalcPlan.Date = dtBeginDate.DateTime;
                        objCalcPlan.Year = System.Convert.ToInt32( txtYear.Value );
                        objCalcPlan.UseForReport = checkUseInReport.Checked;
                        objCalcPlan.Description = txtDscrpn.Text;
                        objCalcPlan.CalcPlanItemProductOwner = new List<CCalcPlanItemProductOwner>();
                        objCalcPlan.Currency = (CCurrency)cboxCurrency.SelectedItem;

                        ctrlCalcPlanItem objCtrlCalcPlanItem = null;
                        foreach (DevExpress.XtraTab.XtraTabPage tabPage2 in tabControlCalcPlanItems.TabPages)
                        {
                            objCtrlCalcPlanItem = (ctrlCalcPlanItem)tabPage2.Controls[0];
                            if( objCtrlCalcPlanItem.IsChanged == true )
                            {
                                CCalcPlanItemProductOwner objCalcPlanItemProductOwner = objCtrlCalcPlanItem.GetCalcPlanProductOwnreForSave();
                                if( objCalcPlanItemProductOwner != null )
                                {
                                    objCalcPlan.CalcPlanItemProductOwner.Add(objCalcPlanItemProductOwner);
                                }
                            }
                        }

                        if ((m_objDeletedCalcPlanItemProductOwnerList != null) && (m_objDeletedCalcPlanItemProductOwnerList.Count > 0))
                        {
                            System.Boolean bExist = false;
                            foreach (CCalcPlanItemProductOwner objForDelete in m_objDeletedCalcPlanItemProductOwnerList)
                            {
                                bExist = false;

                                foreach (CCalcPlanItemProductOwner objCalcPlanItemProductOwner in objCalcPlan.CalcPlanItemProductOwner)
                                {
                                    if (objCalcPlanItemProductOwner.ProductOwner.uuidID.CompareTo(objForDelete.ProductOwner.uuidID) == 0)
                                    {
                                        bExist = true;
                                        break;
                                    }
                                }

                                if (bExist == false)
                                {
                                    objForDelete.CalcPlanItemProductTypeList = new List<CCalcPlanItemProductType>();
                                    objForDelete.Quantity = 0;
                                    objForDelete.AllMoney = 0;
                                    objCalcPlan.CalcPlanItemProductOwner.Add(objForDelete);
                                }
                            }

                        }

                        
                        if (objCalcPlan.ID.CompareTo(System.Guid.Empty) == 0)
                        {
                            bRet = objCalcPlan.Add( m_objProfile );
                        }
                        else
                        {
                            bRet = objCalcPlan.Update( m_objProfile );
                        }
                        SendMessageToLog(strErr);

                        if (bRet == true)
                        {
                            m_objSelectedCalcPlan.ID = objCalcPlan.ID;

                            m_objSelectedCalcPlan.Name = objCalcPlan.Name;
                            m_objSelectedCalcPlan.Date = objCalcPlan.Date;
                            m_objSelectedCalcPlan.UseForReport = objCalcPlan.UseForReport;
                            m_objSelectedCalcPlan.Description = objCalcPlan.Description;
                            m_objSelectedCalcPlan.CalcPlanItemProductOwner = objCalcPlan.CalcPlanItemProductOwner;
                            m_objSelectedCalcPlan.Currency = objCalcPlan.Currency;
                            m_objSelectedCalcPlan.Year = objCalcPlan.Year;

                            if (bIsNew == true)
                            {
                                m_objCalcPlanList.Add( m_objSelectedCalcPlan );
                            }
                            //tabControl.SelectedTabPage = tabPageList;
                            gridViewList.RefreshData();
                            //bar1.Visible = true;
                        }
                        else
                        {
                            SendMessageToLog(strErr);
                        }
                        objCalcPlan = null;
                    }

                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в свойствах плана продаж. Текст ошибки: " + f.Message);
            }
            return bRet;
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
                bRet = ( (txtName.Text != "") && (cboxCurrency.SelectedItem != null));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки заполнения обязательных свойств плана продаж. Текст ошибки: " + f.Message);
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
                cboxCurrency.Properties.Appearance.BackColor = ((cboxCurrency.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtName.Properties.Appearance.BackColor = ((txtName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
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
                if (m_bIsChanged == true)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show( "Выйти без сохранения изменений?", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                }

                foreach (DevExpress.XtraTab.XtraTabPage tabPage in tabControlCalcPlanItems.TabPages)
                {
                    if (((ctrlCalcPlanItem)tabPage.Controls[0]).IsChanged == true)
                    {
                        ((ctrlCalcPlanItem)tabPage.Controls[0]).IsChanged = false;
                    }
                }
                // теперь список расшифровок
                tabControlCalcPlanItems.TabPages.Clear();

                LoadCalcPlanList();

                if (((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).RowCount > 0)
                {
                    if( (m_iRowHandle > 0) && (m_iRowHandle < ((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).RowCount) )
                    {

                        ((DevExpress.XtraGrid.Views.Grid.GridView)gridControlList.MainView).FocusedRowHandle = m_iRowHandle;
                    }
                }


                tabControl.SelectedTabPage = tabPageList;
                bar1.Visible = true;

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
        private void CalcPlanProperties_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if (e.NewValue != null)
                {
                    SetPropertiesModified(true);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств плана продаж.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void CalcPlanrProperties_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств  плана продаж.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Удаление плана продаж
        /// <summary>
        /// Удаление выделенного в списке плана продаж
        /// </summary>
        private void DeleteCalcPlan()
        {
            try
            {
                CCalcPlan objSelectedCalcPlan = GetSelectedCalcPlan();
                if (objSelectedCalcPlan != null)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Подтвердите удаление плана продаж.\n" + objSelectedCalcPlan.Name, "Внимание",
                            System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        System.String strErr = "";
                        if (objSelectedCalcPlan.Remove( m_objProfile ) == true)
                        {
                            m_objCalcPlanList.Remove(objSelectedCalcPlan);
                            gridViewList.RefreshData();
                        }
                        else
                        {
                            SendMessageToLog(strErr);
                        }
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления плана продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void barButtonDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DeleteCalcPlan();
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления плана продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        #endregion

        #region загрузить расшифровку по товарной марке
        private void treeListproductOwner_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((treeListProductOwner.Nodes.Count == 0) || (treeListProductOwner.FocusedNode == null) || (treeListProductOwner.FocusedNode.Tag == null)) { return; }
            try
            {
                CCalcPlanItemProductOwner objCalcPlanItemProductOwner = (CCalcPlanItemProductOwner)treeListProductOwner.FocusedNode.Tag;
                if (objCalcPlanItemProductOwner != null)
                {
                    LoadData(objCalcPlanItemProductOwner);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("treeList_MouseDoubleClick.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void LoadData(CCalcPlanItemProductOwner objCalcPlanItemProductOwner)
        {
            try
            {
                //System.String strStartProcess = "идет загрузка данных...";
                if (objCalcPlanItemProductOwner == null) { return; }
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                System.Boolean bPageExsits = false;
                System.String strTabName = objCalcPlanItemProductOwner.ProductOwner.Name;
                foreach (DevExpress.XtraTab.XtraTabPage tabPageItem in tabControlCalcPlanItems.TabPages)
                {
                    if (tabPageItem.Name == strTabName)
                    {
                        bPageExsits = true;
                        tabControlCalcPlanItems.SelectedTabPage = tabPageItem;
                        break;
                    }
                }
                if (bPageExsits == true) { return; }

                this.tabControlCalcPlanItems.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.tabControlCalcPlanItems)).BeginInit();

                DevExpress.XtraTab.XtraTabPage tabPage = new DevExpress.XtraTab.XtraTabPage();
                if (tabPage != null)
                {
                    tabPage.Name = strTabName;
                    tabControlCalcPlanItems.TabPages.Add(tabPage);
                    tabControlCalcPlanItems.SelectedTabPage = tabPage;

                    ctrlCalcPlanItem ViewerPlanProductOwner = new ctrlCalcPlanItem(m_objProfile, m_objMenuItem);
                    if (ViewerPlanProductOwner != null)
                    {
                        tabPage.Controls.Add(ViewerPlanProductOwner);
                        tabPage.Text = strTabName;
                        ViewerPlanProductOwner.Dock = DockStyle.Fill;

                        this.Refresh();

                        ViewerPlanProductOwner.EditCalcPlanItemProductOwner(objCalcPlanItemProductOwner, m_objSelectedCalcPlan, m_objCalcPlanKoefList);

                        ViewerPlanProductOwner.ChangeSetting += this.OnChangeCalcPlanVariable;
                    }
                    this.Refresh();
                }

                this.tabControlCalcPlanItems.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.tabControlCalcPlanItems)).EndInit();

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка загрузки плана продаж для ." + objCalcPlanItemProductOwner.ProductOwner.Name + "\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return;
        }
        private void OnChangeCalcPlanVariable(Object sender, ChangeCalcPlanItemEventArgs e)
        {
            try
            {
                if (e.ActionType == enumAction.ObjectChanged )
                {
                    SetPropertiesModified(true);
                }
                if (tabControl.TabPages.Count > 0)
                {
                    foreach (DevExpress.XtraTab.XtraTabPage tabPage in this.tabControlCalcPlanItems.TabPages)
                    {
                        if (tabPage.Name == e.CalcPlanItemProductOwner.ProductOwner.Name)
                        {
                            tabPage.Image = (e.ActionType == enumAction.ObjectChanged) ? ERPMercuryPlan.Properties.Resources.warning : ERPMercuryPlan.Properties.Resources.check2;
                            tabPage.Refresh();
                            break;
                        }
                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangeCalcPlanVariable.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }

        #endregion

        #region Добавить товарную марку в план

        private void AddProductOwnerToPlan()
        {
            if ((treeListFreeProductOwner.Nodes.Count == 0) || (treeListFreeProductOwner.FocusedNode == null) || (treeListFreeProductOwner.FocusedNode.Tag == null)) { return; }
            try
            {
                CCalcPlanItemProductOwner objCalcPlanItemProductOwner = new CCalcPlanItemProductOwner( ( CProductOwner )treeListFreeProductOwner.FocusedNode.Tag, 0, 0 );
                if (objCalcPlanItemProductOwner != null)
                {
                    LoadData(objCalcPlanItemProductOwner);

                    treeListFreeProductOwner.Nodes.Remove(treeListFreeProductOwner.FocusedNode);

                    treeListProductOwner.AppendNode(new object[] { objCalcPlanItemProductOwner.ProductOwner.Name, 0, 0 }, null).Tag = objCalcPlanItemProductOwner;

                    SetPropertiesModified(true);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("AddProductOwnerToPlan.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void treeListFreeProductOwner_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                AddProductOwnerToPlan();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("treeList_MouseDoubleClick.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Удалить товарную марку из плана
        private void RemoveProductOwnerFromPlan()
        {
            if ((treeListProductOwner.Nodes.Count == 0) || (treeListProductOwner.FocusedNode == null) || (treeListProductOwner.FocusedNode.Tag == null)) { return; }
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("Подтвердите удаление из плана продаж товарной марки.", "Подтверждение",
                   System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.Yes) { return; }
                CCalcPlanItemProductOwner objCalcPlanItemProductOwner = (CCalcPlanItemProductOwner)treeListProductOwner.FocusedNode.Tag;

                this.tabControlCalcPlanItems.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListFreeProductOwner)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.treeListProductOwner)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.tabControlCalcPlanItems)).BeginInit();

                
                System.String strTabName = objCalcPlanItemProductOwner.ProductOwner.Name;
                DevExpress.XtraTab.XtraTabPage objtabPageForDeleted = null;
                foreach (DevExpress.XtraTab.XtraTabPage tabPageItem in tabControlCalcPlanItems.TabPages)
                {
                    if (tabPageItem.Name == strTabName)
                    {
                        objtabPageForDeleted = tabPageItem;
                        break;
                    }
                }

                if (objtabPageForDeleted != null)
                {
                    tabControlCalcPlanItems.TabPages.Remove(objtabPageForDeleted);
                }

                if (m_objDeletedCalcPlanItemProductOwnerList == null) { m_objDeletedCalcPlanItemProductOwnerList = new List<CCalcPlanItemProductOwner>(); }
                m_objDeletedCalcPlanItemProductOwnerList.Add( objCalcPlanItemProductOwner );

                treeListProductOwner.Nodes.Remove(treeListProductOwner.FocusedNode);

                treeListFreeProductOwner.AppendNode(new object[] { objCalcPlanItemProductOwner.ProductOwner.Name }, null).Tag = objCalcPlanItemProductOwner.ProductOwner;

                SetPropertiesModified(true);

                this.tabControlCalcPlanItems.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListFreeProductOwner)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.treeListProductOwner)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.tabControlCalcPlanItems)).EndInit();

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("RemoveProductOwnerFromPlan.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void btnDeleteProductOwner_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveProductOwnerFromPlan();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("btnDeleteProductOwner_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Закрыть закладки
        private void tabControlCalcPlanItems_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                if ((tabControlCalcPlanItems.TabPages.Count == 0) || (tabControlCalcPlanItems.SelectedTabPage == null)) { return; }
                // закрываем закладку
                CloseTabPage(tabControlCalcPlanItems.SelectedTabPage, true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("tabControlCalcPlanItems_CloseButtonClick.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Закрывает закладку вместе с работающим в ней потоком
        /// </summary>
        /// <param name="tPage">закладка</param>
        /// <param name="bConfirm">Признак "Выдавать сообщение"</param>
        private void CloseTabPage(DevExpress.XtraTab.XtraTabPage tabPage, System.Boolean bConfirm)
        {
            System.String tPageText = "";
            try
            {
                if (tabControlCalcPlanItems.TabPages.Count == 0) { return; }
                if (tabPage == null) { return; }
                if (tabPage.Controls.Count == 0) { return; }
                tPageText = tabPage.Text;

                ctrlCalcPlanItem ViewerPlanProductOwner = (ctrlCalcPlanItem)tabPage.Controls[0];
                if (ViewerPlanProductOwner == null) { return; }

                if (ViewerPlanProductOwner.IsChanged == true)
                {
                    DialogResult resDlg = DevExpress.XtraEditors.XtraMessageBox.Show(
                    ViewerPlanProductOwner.CalcPlanProductOwner.ProductOwner.Name + "\nВ план были внесены изменения.\nЗакрыть без сохранения изменений?", "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                    switch (resDlg)
                    {
                        case DialogResult.Yes:
                            {
                                //// попробуем сохранить
                                //if (bSavePlanOnlyOneProductOwner(tabPage) == true)
                                //{
                                    ViewerPlanProductOwner = null;
                                    tabControlCalcPlanItems.TabPages.Remove(tabPage);
                                    tabPage = null;
                                    this.Refresh();
                                //}
                                break;
                            }
                        case DialogResult.No:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    ViewerPlanProductOwner = null;
                    tabControlCalcPlanItems.TabPages.Remove(tabPage);
                    tabPage = null;
                    this.Refresh();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(tPageText + "\nCloseTabPage.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
            finally
            {
            }
            return;
        }
        private void frmPlanCalcPlanList_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (tabControlCalcPlanItems.TabPages.Count > 0)
                {
                    System.Boolean bFindChanges = false;
                    foreach (DevExpress.XtraTab.XtraTabPage tabPage in tabControlCalcPlanItems.TabPages)
                    {
                        if (((ctrlCalcPlanItem)tabPage.Controls[0]).IsChanged == true)
                        {
                            bFindChanges = true;
                            break;
                        }
                    }
                    if (bFindChanges == true)
                    {
                        DialogResult resDlg = DevExpress.XtraEditors.XtraMessageBox.Show(
                        "План продаж был изменен.\nСохранить изменения в плане?", "Подтверждение",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);

                        switch (resDlg)
                        {
                            case DialogResult.Yes:
                                {
                                    e.Cancel = (ConfirmCalcPlan () == true) ? false : true;
                                    break;
                                }
                            case DialogResult.No:
                                break;
                            case DialogResult.Cancel:
                                {
                                    e.Cancel = true;
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка закрытия формы.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Печать
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if ((tabControlCalcPlanItems.TabPages.Count == 0) || (tabControlCalcPlanItems.SelectedTabPage == null)) { return; }
                ctrlCalcPlanItem ViewerPlanProductOwner = (ctrlCalcPlanItem)(tabControlCalcPlanItems.SelectedTabPage.Controls[0]);
                Cursor.Current = Cursors.WaitCursor;
                SendMessageToLog("идет экспорт данных в Microsoft Excel...");
                this.Refresh();
                ViewerPlanProductOwner.ExportToExcel();
                SendMessageToLog("завершен экспорт данных в Microsoft Excel");
                Cursor.Current = Cursors.Default;

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка печати\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Печать всего расчета
        /// <summary>
        /// Экспорт плана в MS Excel
        /// </summary>
        public void ExportToExcelAllSelectedCalcPlan()
        {
            if (m_objSelectedCalcPlan == null) { return; }
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel._Worksheet oSheet2;
            //Excel.Range oRng;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                object m = Type.Missing;
                //oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                if (oWB.Worksheets.Count < 2)
                {
                    oWB.Worksheets.Add(m, m, m, Excel.XlSheetType.xlWorksheet);
                }

                // теперь у нас нужное количество листов
                System.Int32 iSheetNumForQty = 1;
                System.Int32 iSheetNumForMoney = 2;
                System.Int32 iStartRowNum = 6;

                for (System.Int32 i = 1; i <= 2; i++)
                {
                    oSheet = (Excel._Worksheet)oWB.Worksheets[i];
                    oSheet.Name = ((i == 1) ? "кол-во" : "сумма");
                    oSheet.Cells[1, 3] = ( m_objSelectedCalcPlan.Year.ToString() + " год" );
                    oSheet.Cells[2, 3] = m_objSelectedCalcPlan.Name;
                    // Имена столбцов
                    oSheet.Cells[5, 3] = "Товарная марка";
                    oSheet.Cells[5, 4] = "Январь";
                    oSheet.Cells[5, 5] = "Февраль";
                    oSheet.Cells[5, 6] = "Март";
                    oSheet.Cells[5, 7] = "Апрель";
                    oSheet.Cells[5, 8] = "Май";
                    oSheet.Cells[5, 9] = "Июнь";
                    oSheet.Cells[5, 10] = "Июль";
                    oSheet.Cells[5, 11] = "Август";
                    oSheet.Cells[5, 12] = "Сентябрь";
                    oSheet.Cells[5, 13] = "Октябрь";
                    oSheet.Cells[5, 14] = "Ноябрь";
                    oSheet.Cells[5, 15] = "Декабрь";
                    oSheet.Cells[5, 16] = "Итого";
                }

                oSheet = (Excel._Worksheet)oWB.Worksheets[iSheetNumForQty];
                oSheet2 = (Excel._Worksheet)oWB.Worksheets[iSheetNumForMoney];

                oSheet.get_Range("A5", "Z5").Font.Size = 12;
                oSheet.get_Range("A5", "Z5").Font.Bold = true;
                oSheet.get_Range("A5", "Z5").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("P5", "P1000").Font.Bold = true;

                oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();
                oSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                oSheet.PageSetup.Zoom = false;
                oSheet.PageSetup.FitToPagesTall = 1;
                oSheet.PageSetup.FitToPagesTall = 1;

                oSheet2.get_Range("A5", "Z5").Font.Size = 12;
                oSheet2.get_Range("A5", "Z5").Font.Bold = true;
                oSheet2.get_Range("A5", "Z5").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet2.get_Range("P5", "P1000").Font.Bold = true;

                oSheet2.get_Range("A1", "Z1").EntireColumn.AutoFit();
                oSheet2.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                oSheet2.PageSetup.Zoom = false;
                oSheet2.PageSetup.FitToPagesTall = 1;
                oSheet2.PageSetup.FitToPagesTall = 1;


                oSheet.get_Range("C1", "Z1000").NumberFormat = "# ##0";
                oSheet2.get_Range("C1", "Z1000").NumberFormat = "# ##0,00";


                // попробуем пройти список ячеек за один проход
                System.Decimal dcmlAllMoney_1 = 0;
                System.Decimal dcmlAllMoney_2 = 0;
                System.Decimal dcmlAllMoney_3 = 0;
                System.Decimal dcmlAllMoney_4 = 0;
                System.Decimal dcmlAllMoney_5 = 0;
                System.Decimal dcmlAllMoney_6 = 0;
                System.Decimal dcmlAllMoney_7 = 0;
                System.Decimal dcmlAllMoney_8 = 0;
                System.Decimal dcmlAllMoney_9 = 0;
                System.Decimal dcmlAllMoney_10 = 0;
                System.Decimal dcmlAllMoney_11 = 0;
                System.Decimal dcmlAllMoney_12 = 0;
                System.Int32 iQty_1 = 0;
                System.Int32 iQty_2 = 0;
                System.Int32 iQty_3 = 0;
                System.Int32 iQty_4 = 0;
                System.Int32 iQty_5 = 0;
                System.Int32 iQty_6 = 0;
                System.Int32 iQty_7 = 0;
                System.Int32 iQty_8 = 0;
                System.Int32 iQty_9 = 0;
                System.Int32 iQty_10 = 0;
                System.Int32 iQty_11 = 0;
                System.Int32 iQty_12 = 0;
                foreach (CCalcPlanItemProductOwner objProductOwner in m_objSelectedCalcPlan.CalcPlanItemProductOwner)
                {
                    oSheet.Cells[iStartRowNum, 3] = objProductOwner.ProductOwner.Name;

                    if (objProductOwner.CalcPlanItemProductTypeList != null)
                    {
                        dcmlAllMoney_1 = 0;
                        dcmlAllMoney_2 = 0;
                        dcmlAllMoney_3 = 0;
                        dcmlAllMoney_4 = 0;
                        dcmlAllMoney_5 = 0;
                        dcmlAllMoney_6 = 0;
                        dcmlAllMoney_7 = 0;
                        dcmlAllMoney_8 = 0;
                        dcmlAllMoney_9 = 0;
                        dcmlAllMoney_10 = 0;
                        dcmlAllMoney_11 = 0;
                        dcmlAllMoney_12 = 0;
                        iQty_1 = 0;
                        iQty_2 = 0;
                        iQty_3 = 0;
                        iQty_4 = 0;
                        iQty_5 = 0;
                        iQty_6 = 0;
                        iQty_7 = 0;
                        iQty_8 = 0;
                        iQty_9 = 0;
                        iQty_10 = 0;
                        iQty_11 = 0;
                        iQty_12 = 0;

                        foreach (CCalcPlanItemProductType objCalcPlanItemProductType in objProductOwner.CalcPlanItemProductTypeList)
                        {
                            dcmlAllMoney_1 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.January));
                            iQty_1 += (objCalcPlanItemProductType.GetPlanQty(enMonth.January));

                            dcmlAllMoney_2 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.Febrary));
                            iQty_2 += (objCalcPlanItemProductType.GetPlanQty(enMonth.Febrary));

                            dcmlAllMoney_3 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.March));
                            iQty_3 += (objCalcPlanItemProductType.GetPlanQty(enMonth.March));

                            dcmlAllMoney_4 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.April));
                            iQty_4 += (objCalcPlanItemProductType.GetPlanQty(enMonth.April));

                            dcmlAllMoney_5 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.May));
                            iQty_5 += (objCalcPlanItemProductType.GetPlanQty(enMonth.May));

                            dcmlAllMoney_6 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.June));
                            iQty_6 += (objCalcPlanItemProductType.GetPlanQty(enMonth.June));

                            dcmlAllMoney_7 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.July));
                            iQty_7 += (objCalcPlanItemProductType.GetPlanQty(enMonth.July));

                            dcmlAllMoney_8 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.August));
                            iQty_8 += (objCalcPlanItemProductType.GetPlanQty(enMonth.August));

                            dcmlAllMoney_9 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.September));
                            iQty_9 += (objCalcPlanItemProductType.GetPlanQty(enMonth.September));

                            dcmlAllMoney_10 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.Ocntober));
                            iQty_10 += (objCalcPlanItemProductType.GetPlanQty(enMonth.Ocntober));

                            dcmlAllMoney_11 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.November));
                            iQty_11 += (objCalcPlanItemProductType.GetPlanQty(enMonth.November));

                            dcmlAllMoney_12 += (objCalcPlanItemProductType.GetPlanAllMoney(enMonth.December));
                            iQty_12 += (objCalcPlanItemProductType.GetPlanQty(enMonth.December));

                        }
                    }

                    oSheet.Cells[iStartRowNum, 4] = iQty_1;
                    oSheet.Cells[iStartRowNum, 5] = iQty_2;
                    oSheet.Cells[iStartRowNum, 6] = iQty_3;
                    oSheet.Cells[iStartRowNum, 7] = iQty_4;
                    oSheet.Cells[iStartRowNum, 8] = iQty_5;
                    oSheet.Cells[iStartRowNum, 9] = iQty_6;
                    oSheet.Cells[iStartRowNum, 10] = iQty_7;
                    oSheet.Cells[iStartRowNum, 11] = iQty_8;
                    oSheet.Cells[iStartRowNum, 12] = iQty_9;
                    oSheet.Cells[iStartRowNum, 13] = iQty_10;
                    oSheet.Cells[iStartRowNum, 14] = iQty_11;
                    oSheet.Cells[iStartRowNum, 15] = iQty_12;
                    oSheet.get_Range(oSheet.Cells[iStartRowNum, 16], oSheet.Cells[iStartRowNum, 16]).Formula = "=СУММ(RC[-12]:RC[-1])";

                    oSheet2.Cells[iStartRowNum, 3] = objProductOwner.ProductOwner.Name;

                    oSheet2.Cells[iStartRowNum, 4] = dcmlAllMoney_1;
                    oSheet2.Cells[iStartRowNum, 5] = dcmlAllMoney_2;
                    oSheet2.Cells[iStartRowNum, 6] = dcmlAllMoney_3;
                    oSheet2.Cells[iStartRowNum, 7] = dcmlAllMoney_4;
                    oSheet2.Cells[iStartRowNum, 8] = dcmlAllMoney_5;
                    oSheet2.Cells[iStartRowNum, 9] = dcmlAllMoney_6;
                    oSheet2.Cells[iStartRowNum, 10] = dcmlAllMoney_7;
                    oSheet2.Cells[iStartRowNum, 11] = dcmlAllMoney_8;
                    oSheet2.Cells[iStartRowNum, 12] = dcmlAllMoney_9;
                    oSheet2.Cells[iStartRowNum, 13] = dcmlAllMoney_10;
                    oSheet2.Cells[iStartRowNum, 14] = dcmlAllMoney_11;
                    oSheet2.Cells[iStartRowNum, 15] = dcmlAllMoney_12;

                    oSheet2.get_Range(oSheet2.Cells[iStartRowNum, 16], oSheet2.Cells[iStartRowNum, 16]).Formula = "=СУММ(RC[-12]:RC[-1])";

                    iStartRowNum++;
                }

                for (System.Int32 i2 = 1; i2 <= 2; i2++)
                {
                    oSheet = (Excel._Worksheet)oWB.Worksheets[i2];
                    if (oSheet != null)
                    {
                        for (System.Int32 i = 4; i <= 15; i++)
                        {
                            oSheet.get_Range(oSheet.Cells[iStartRowNum, i], oSheet.Cells[iStartRowNum, i]).Formula = "=СУММ(R[-" + (iStartRowNum - 6).ToString() + "]C:R[-1]C)";

                            oSheet.get_Range(oSheet.Cells[iStartRowNum, i], oSheet.Cells[iStartRowNum, i]).NumberFormat = ((i2 == 1) ? "# ##0" : "# ##0,00");
                            oSheet.get_Range(oSheet.Cells[iStartRowNum, i], oSheet.Cells[iStartRowNum, i]).Font.Bold = true;
                            oSheet.get_Range(oSheet.Cells[iStartRowNum, i], oSheet.Cells[iStartRowNum, i]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        }
                        oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();
                        oSheet.get_Range("A1", "B1").EntireColumn.Hidden = true;
                        oSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                        oSheet.PageSetup.Zoom = false;
                        oSheet.PageSetup.FitToPagesTall = 1;
                        oSheet.PageSetup.FitToPagesTall = 1;


                    }
                }



                oXL.Visible = true;
                oXL.UserControl = true;
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
                oSheet2 = null;
                oWB = null;
                oXL = null;

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void btnPrintAllPlan_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SendMessageToLog("идет экспорт данных в Microsoft Excel...");
                this.Refresh();
                ExportToExcelAllSelectedCalcPlan();
                SendMessageToLog("завершен экспорт данных в Microsoft Excel");
                Cursor.Current = Cursors.Default;

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка печати\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;

        }
        #endregion


        #region Копирование расчета
        private void CopyCalcPlan()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CCalcPlan objSelectedCalcPlan = GetSelectedCalcPlan();
                if (objSelectedCalcPlan != null)
                {
                    if (CCalcPlan.Copy(m_objProfile, objSelectedCalcPlan) == true)
                    {
                        LoadCalcPlanList();
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка копирования плана продаж. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CopyCalcPlan();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка копирования плана продаж. Текст ошибки: " + f.Message);
            }
        }
        #endregion

        private void gridControlList_Click(object sender, EventArgs e)
        {

        }



    }

    public class ViewCalcPlanList : PlugIn.IClassTypeView
    {
        public override void Run(UniXP.Common.MENUITEM objMenuItem, System.String strCaption)
        {
            frmCalcPlanList obj = new frmCalcPlanList(objMenuItem.objProfile, objMenuItem);
            obj.Text = strCaption;
            obj.MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent;
            obj.Visible = true;
        }

    }

}
