using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ERPMercuryPlan
{
    public partial class ctrlCalcPlanItem : UserControl
    {

        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private CCalcPlan m_objCalcPlan;
        private List<ERP_Mercury.Common.CProductType> m_objProductTypeList;
        private CCalcPlanItemProductOwner m_objCalcPlanProductOwner;
        public CCalcPlanItemProductOwner CalcPlanProductOwner
        {
            get { return m_objCalcPlanProductOwner; }
        }
        private CCalcPlanItemProductOwner m_objCalcPlanProductOwnreForSave;
        public CCalcPlanItemProductOwner CalcPlanProductOwnreForSave
        {
            get { return m_objCalcPlanProductOwnreForSave; }
        }
        private System.Boolean m_bIsChanged;
        /// <summary>
        /// Признак "план по ТМ изменен"
        /// </summary>
        public System.Boolean IsChanged
        {
            get { return m_bIsChanged; }
            set { m_bIsChanged = value; }
        }
        private System.Boolean m_bDisableEvents;
        private List<CCalcPlanKoef> m_objCalcPlanKoefList;
        private ERP_Mercury.Common.CProductType m_objProductTypeDefault;
        private const System.String m_strAllItems = "\"ВСЕ\"";
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeCalcPlanItemEventArgs> m_ChangeSetting;
        // Создаем в классе член-событие
        public event EventHandler<ChangeCalcPlanItemEventArgs> ChangeSetting
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeSetting += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeSetting -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeSetting(ChangeCalcPlanItemEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeCalcPlanItemEventArgs> temp = m_ChangeSetting;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeSetting(enumAction enActionType, CCalcPlanItemProductOwner objCalcPlanItemProductOwner)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeCalcPlanItemEventArgs e = new ChangeCalcPlanItemEventArgs(enActionType, objCalcPlanItemProductOwner);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeSetting(e);
        }
        #endregion

        #region Конструктор
        public ctrlCalcPlanItem(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objProductTypeList = ERP_Mercury.Common.CProductType.GetProductTypeList(m_objProfile, null);
            m_objProductTypeDefault = ERP_Mercury.Common.CProductType.GetProductTypeDefaultForCalcPlan(m_objProfile, null);
            m_objCalcPlan = null;
            m_objCalcPlanProductOwner = null;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_objCalcPlanProductOwnreForSave = null;
            m_objCalcPlanKoefList = null;

            InitializeComponent();
            btnReCalc.Enabled = false;

            foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
            {
                objColumn.Tag = objColumn.VisibleIndex;
            }
            
            LoadComboBox();
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

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            m_bIsChanged = bModified;

            if (bModified == true)
            {
                SimulateChangeSetting(enumAction.ObjectChanged, m_objCalcPlanProductOwner);
            }
        }
        private void EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка изменения свойств. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void treeList_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }

                if (IsColumnForEdit( e.Column ) == true)
                {
                    RecalcSumInColMoneyForChangedQuantity(e.Node, e.Column);
                    RecalcItog(e.Node);
                }

                RecalcTotalItog();
                SetPropertiesModified(true);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка изменения свойств. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void mitemRecalcSumRight_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeList.FocusedNode == null) { return; }
                if (treeList.FocusedColumn == null) { return; }

                RecalcColumnsRight(treeList.FocusedNode, treeList.FocusedColumn);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка mitemRecalcSumRight_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }

        private void RecalcColumnsRight(DevExpress.XtraTreeList.Nodes.TreeListNode objNode, DevExpress.XtraTreeList.Columns.TreeListColumn objColumn)
        {
            try
            {
                System.Int32 iVisibleIndx = objColumn.VisibleIndex;

                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objItemColumn in treeList.Columns)
                {
                    if ((objItemColumn.VisibleIndex >= iVisibleIndx) && ( IsColumnForEdit( objItemColumn ) == true) && ( IsColumnForEditQty( objItemColumn ) == true))
                    {
                        RecalcSumInColMoneyForChangedQuantity( objNode, objItemColumn );
                    }
                }

                RecalcItog( objNode );
                RecalcTotalItog();
                SetPropertiesModified(true);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка пересчета сумм вправо. Текст ошибки: " + f.Message);
            }
            finally
            {

            }

            return;
        }

        /// <summary>
        /// Проверка, является ли столбец столбцом для редактирования (январь - декабрь)
        /// </summary>
        /// <param name="objColumn">столбец</param>
        /// <returns>true - является; false - нет</returns>
        private System.Boolean IsColumnForEdit(DevExpress.XtraTreeList.Columns.TreeListColumn objColumn)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = ((objColumn == colMonth1Quantity) || (objColumn == colMonth2Quantity) || (objColumn == colMotnh3Quantity) ||
                    (objColumn == colMonth4Quantity) || (objColumn == colMonth5Quantity) || (objColumn == colMonth6Quantity) ||
                    (objColumn == colMonth7Quantity) || (objColumn == colMonth8Quantity) || (objColumn == colMonth9Quantity) ||
                    (objColumn == colMonth10Quantity) || (objColumn == colMonth11Quantity) || (objColumn == colMonth12Quantity) ||
                    (objColumn == colMonth1AllPrice) || (objColumn == colMonth2AllPrice) || (objColumn == colMonth3AllPrice) ||
                    (objColumn == colMonth4AllPrice) || (objColumn == colMonth5AllPrice) || (objColumn == colMonth6AllPrice) ||
                    (objColumn == colMonth7AllPrice) || (objColumn == colMonth8AllPrice) || (objColumn == colMonth9AllPrice) ||
                    (objColumn == colMonth9AllPrice) || (objColumn == colMonth10AllPrice) || (objColumn == colMonth12AllPrice) 
                    );
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка IsColumnForEdit. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
        }
        private System.Boolean IsColumnForEditQty(DevExpress.XtraTreeList.Columns.TreeListColumn objColumn)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = ((objColumn == colMonth1Quantity) || (objColumn == colMonth2Quantity) || (objColumn == colMotnh3Quantity) ||
                    (objColumn == colMonth4Quantity) || (objColumn == colMonth5Quantity) || (objColumn == colMonth6Quantity) ||
                    (objColumn == colMonth7Quantity) || (objColumn == colMonth8Quantity) || (objColumn == colMonth9Quantity) ||
                    (objColumn == colMonth10Quantity) || (objColumn == colMonth11Quantity) || (objColumn == colMonth12Quantity)
                    );
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка IsColumnForEditQty. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
        }
        private System.Boolean IsColumnForEditMoney(DevExpress.XtraTreeList.Columns.TreeListColumn objColumn)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = ((objColumn == colMonth1AllPrice) || (objColumn == colMonth2AllPrice) || (objColumn == colMonth3AllPrice) ||
                    (objColumn == colMonth4AllPrice) || (objColumn == colMonth5AllPrice) || (objColumn == colMonth6AllPrice) ||
                    (objColumn == colMonth7AllPrice) || (objColumn == colMonth8AllPrice) || (objColumn == colMonth9AllPrice) ||
                    (objColumn == colMonth9AllPrice) || (objColumn == colMonth10AllPrice) || (objColumn == colMonth11AllPrice) || (objColumn == colMonth12AllPrice)
                    );
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка IsColumnForEditMoney. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
        }

        /// <summary>
        /// Пересчет итогов по узлу
        /// </summary>
        /// <param name="objNode">узел</param>
        private void RecalcItog( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            try
            {
                //this.tableLayoutPanel1.SuspendLayout();
                //((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                
                System.Int32 iQty = 0;

                    iQty = (((objNode.GetValue(colMonth1Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth1Quantity))) +
                        ((objNode.GetValue(colMonth2Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth2Quantity))) +
                        ((objNode.GetValue(colMotnh3Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMotnh3Quantity))) +
                        ((objNode.GetValue(colMonth4Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth4Quantity))) +
                        ((objNode.GetValue(colMonth5Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth5Quantity))) +
                        ((objNode.GetValue(colMonth6Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth6Quantity))) +
                        ((objNode.GetValue(colMonth7Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth7Quantity))) +
                        ((objNode.GetValue(colMonth8Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth8Quantity))) +
                        ((objNode.GetValue(colMonth9Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth9Quantity))) +
                        ((objNode.GetValue(colMonth10Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth10Quantity))) +
                        ((objNode.GetValue(colMonth11Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth11Quantity))) +
                        ((objNode.GetValue(colMonth12Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth12Quantity))));

                    objNode.SetValue(colMonthTotalQuantity, iQty);

                    System.Decimal dcmlAllMoney = 0;
                    dcmlAllMoney = (((objNode.GetValue(colMonth1AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth1AllPrice))) +
                        ((objNode.GetValue(colMonth12AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth12AllPrice))) +
                        ((objNode.GetValue(colMonth2AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth2AllPrice))) +
                        ((objNode.GetValue(colMonth3AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth3AllPrice))) +
                        ((objNode.GetValue(colMonth4AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth4AllPrice))) +
                        ((objNode.GetValue(colMonth5AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth5AllPrice))) +
                        ((objNode.GetValue(colMonth6AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth6AllPrice))) +
                        ((objNode.GetValue(colMonth7AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth7AllPrice))) +
                        ((objNode.GetValue(colMonth8AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth8AllPrice))) +
                        ((objNode.GetValue(colMonth9AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth9AllPrice))) +
                        ((objNode.GetValue(colMonth10AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth10AllPrice))) +
                        ((objNode.GetValue(colMonth11AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth11AllPrice))));

                    objNode.SetValue(colMonthTotalAllMoney, dcmlAllMoney);
                    //this.tableLayoutPanel1.ResumeLayout(false);
                    //((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
                    
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка пересчета итоговых сумм. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void RecalcSumInColMoneyForChangedQuantity( DevExpress.XtraTreeList.Nodes.TreeListNode objNode, 
            DevExpress.XtraTreeList.Columns.TreeListColumn objColQty )
        {
            if (IsColumnForEditQty( objColQty ) == false) { return; }
            try
            {

                System.Decimal PriceAvgEXW = ( objNode.GetValue(colPriceAvgEXW) == null ) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colPriceAvgEXW));
                System.Decimal KoeffPlan = ( objNode.GetValue(colPlanKoeff) == null ) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colPlanKoeff));

                System.Int32 iQty = System.Convert.ToInt32(objNode.GetValue(objColQty));
                System.Decimal dcmlAllMoney = 0;
                DevExpress.XtraTreeList.Columns.TreeListColumn objColMoney = null;

                if (objColQty == colMonth1Quantity) { objColMoney = colMonth1AllPrice; }
                if (objColQty == colMonth2Quantity) { objColMoney = colMonth2AllPrice; }
                if (objColQty == colMotnh3Quantity) { objColMoney = colMonth3AllPrice; }
                if (objColQty == colMonth4Quantity) { objColMoney = colMonth4AllPrice; }
                if (objColQty == colMonth5Quantity) { objColMoney = colMonth5AllPrice; }
                if (objColQty == colMonth6Quantity) { objColMoney = colMonth6AllPrice; }
                if (objColQty == colMonth7Quantity) { objColMoney = colMonth7AllPrice; }
                if (objColQty == colMonth8Quantity) { objColMoney = colMonth8AllPrice; }
                if (objColQty == colMonth9Quantity) { objColMoney = colMonth9AllPrice; }
                if (objColQty == colMonth10Quantity) { objColMoney = colMonth10AllPrice; }
                if (objColQty == colMonth11Quantity) { objColMoney = colMonth11AllPrice; }
                if (objColQty == colMonth12Quantity) { objColMoney = colMonth12AllPrice; }

                if( (objColMoney != null) && ( PriceAvgEXW > 0 ) && ( KoeffPlan > 0 ) )
                {
                    dcmlAllMoney = RoundToTen( PriceAvgEXW * KoeffPlan * iQty );
                    objNode.SetValue(objColMoney, dcmlAllMoney);
                }

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка пересчета суммы в столбце. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void RecalcTotalItog()
        {
            try
            {
                //this.tableLayoutPanel1.SuspendLayout();
                //((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                System.Int32 iQty = 0;
                System.Decimal dcmlAllMoney = 0;

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    iQty +=( (objNode.GetValue(colMonthTotalQuantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonthTotalQuantity)));
                    dcmlAllMoney += ((objNode.GetValue(colMonthTotalAllMoney) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonthTotalAllMoney)));
                }

                calcEditQty.Value = iQty;
                calcEditMoney.Value = dcmlAllMoney;

                //this.tableLayoutPanel1.ResumeLayout(false);
                //((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка пересчета итоговых сумм. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void treeList_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка изменения свойств. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }


        #endregion

        #region Очистка содержимого элементов управления
        /// <summary>
        /// Очищает данные в элементах управления
        /// </summary>
        private void ClearPropertiesEditors()
        {
            try
            {
                checkedListBoxProductType.Items.Clear();
                checkedListBoxCalcProductType.Items.Clear();
                treeList.Nodes.Clear();
                lblProductOwner.Text = "";
                cboxCalcPlanKoef.Properties.Items.Clear();
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка очистки элементов управления. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Загружает выпадающие списки
        /// </summary>
        private void LoadComboBox()
        {
            try
            {
                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).BeginInit();

                checkedListBoxMonth.Items.Clear();
                checkedListBoxMonth.Items.Add(m_strAllItems, true);
                checkedListBoxMonth.Items.Add("Январь", true);
                checkedListBoxMonth.Items.Add("Февраль", true);
                checkedListBoxMonth.Items.Add("Март", true);
                checkedListBoxMonth.Items.Add("Апрель", true);
                checkedListBoxMonth.Items.Add("Май", true);
                checkedListBoxMonth.Items.Add("Июнь", true);
                checkedListBoxMonth.Items.Add("Июль", true);
                checkedListBoxMonth.Items.Add("Август", true);
                checkedListBoxMonth.Items.Add("Сентябрь", true);
                checkedListBoxMonth.Items.Add("Октябрь", true);
                checkedListBoxMonth.Items.Add("Ноябрь", true);
                checkedListBoxMonth.Items.Add("Декабрь", true);

                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).EndInit();

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Заполнение выпадающих списков. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Редактирование плана по товарной марке
        /// <summary>
        /// Редактирование существующего плана по товарной марке
        /// </summary>
        /// <param name="objCalcPlanItemProductOwner">объект "Расштфровка по ТМ"</param>
        /// <param name="objCalcPlan">объект "План"</param>
        public void EditCalcPlanItemProductOwner(CCalcPlanItemProductOwner objCalcPlanItemProductOwner, CCalcPlan objCalcPlan,
            List<CCalcPlanKoef> objCalcPlanKoefList)
        {
            if (objCalcPlanItemProductOwner == null) { return; }
            if (objCalcPlan == null) { return; }
            try
            {
                m_objCalcPlanProductOwner = objCalcPlanItemProductOwner;
                m_objCalcPlan = objCalcPlan;
                m_objCalcPlanKoefList = objCalcPlanKoefList;
                

                m_bDisableEvents = true;
                ClearPropertiesEditors();

                lblProductOwner.Text = m_objCalcPlanProductOwner.ProductOwner.Name;
                tabPageCalcItems.Text = m_objCalcPlanProductOwner.ProductOwner.Name;

                if (m_objCalcPlanKoefList != null)
                {
                    cboxCalcPlanKoef.Properties.Items.AddRange(m_objCalcPlanKoefList );
                    //foreach (CCalcPlanKoef objCalcPlanKoef in m_objCalcPlanKoefList)
                    //{
                    //    cboxCalcPlanKoef.Properties.Items.Add(objCalcPlanKoef);
                    //}
                }

                // сперва заполним список незадействованных товарных групп
                System.Boolean bExists = false;
                if ((m_objProductTypeList != null) && (m_objProductTypeList.Count > 0))
                {
                    checkedListBoxCalcProductType.Items.Add(new ERP_Mercury.Common.CProductType(System.Guid.Empty, "\"ВСЕ\"", 0, "", 0, "", false), false);
                    foreach (ERP_Mercury.Common.CProductType objProductType in m_objProductTypeList)
                    {
                        checkedListBoxCalcProductType.Items.Add( objProductType, true );

                        bExists = false;
                        if (m_objCalcPlanProductOwner.CalcPlanItemProductTypeList != null)
                        {
                            foreach (CCalcPlanItemProductType objCalcPlanItemProductType in m_objCalcPlanProductOwner.CalcPlanItemProductTypeList)
                            {
                                if (objCalcPlanItemProductType.ProductType.ID.CompareTo(objProductType.ID) == 0)
                                {
                                    bExists = true;
                                    break;
                                }
                            }
                        }
                        if (bExists == false)
                        {
                            checkedListBoxProductType.Items.Add(objProductType, false);
                        }
                    }

                    if (checkedListBoxProductType.Items.Count > 0)
                    {
                        checkedListBoxProductType.Items.Add(new ERP_Mercury.Common.CProductType(System.Guid.Empty, "\"ВСЕ\"", 0, "", 0, "", false), false);
                    }
                }

                // теперь нужно разобраться с расшифровками
                if( objCalcPlanItemProductOwner.CalcPlanItemProductTypeList != null )
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode objAppendNode = null;
                    foreach (CCalcPlanItemProductType objCalcPlanItemProductType in m_objCalcPlanProductOwner.CalcPlanItemProductTypeList)
                    {
                        objAppendNode = treeList.AppendNode(new object[] { objCalcPlanItemProductType.ProductType.Name, objCalcPlanItemProductType.PriceAvgEXW, objCalcPlanItemProductType.KoeffPlan,
                          objCalcPlanItemProductType.GetPlanQty(enMonth.January), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.January),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.Febrary), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.Febrary),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.March), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.March),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.April), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.April),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.May), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.May),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.June), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.June),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.July), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.July),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.August), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.August),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.September), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.September),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.Ocntober), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.Ocntober),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.November), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.November),
                          objCalcPlanItemProductType.GetPlanQty(enMonth.December), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.December),
                          objCalcPlanItemProductType.Quantity, objCalcPlanItemProductType.AllMoney
                        }, null);
                        objAppendNode.Tag = objCalcPlanItemProductType;

                        RecalcItog(objAppendNode);
                    }

                    RecalcTotalItog();
                }


            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования  плана по товарной марке. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
            }

            return;
        }
        private void treeList_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if ( ( e.Node == null )  || ( e.Column == null ) || (e.Node == treeList.FocusedNode && e.Column != treeList.FocusedColumn) ) return;

            try
            {
                if (IsColumnForEditQty(e.Column) == true)
                {
                    e.Appearance.ForeColor = Color.Green;
                }
                else
                {
                    if (IsColumnForEditMoney(e.Column) == true)
                    {
                        e.Appearance.ForeColor = Color.RoyalBlue;
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка treeList_CustomDrawNodeCell. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }
        #endregion

        #region Новый план по товарной марке
        /// <summary>
        /// Создание нового плана по товарной марке
        /// </summary>
        public void NewRoteShet(ERP_Mercury.Common.CProductOwner objProductOwner, CCalcPlan objCalcPlan)
        {
            try
            {
                m_objCalcPlanProductOwner = new CCalcPlanItemProductOwner(objProductOwner, 0, 0);
                m_objCalcPlanProductOwner.CalcPlanItemProductTypeList = new List<CCalcPlanItemProductType>();
                EditCalcPlanItemProductOwner(m_objCalcPlanProductOwner, objCalcPlan, m_objCalcPlanKoefList);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания плана по товарной марке. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Сохранить изменения
        /// <summary>
        /// Прописывает свойства объекта m_objCalcPlanProductOwner из элементов управления
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean bSaveChanges()
        {
            System.Boolean bRet = false;
            try
            {
                if (m_objCalcPlanProductOwner == null) { return bRet; }

                m_objCalcPlanProductOwnreForSave = new CCalcPlanItemProductOwner(m_objCalcPlanProductOwner.ProductOwner, 0, 0);
                m_objCalcPlanProductOwnreForSave.CalcPlanItemProductTypeList = new List<CCalcPlanItemProductType>();


                CCalcPlanItemProductType objCalcPlanItemProductType = null;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCalcPlanItemProductType = (CCalcPlanItemProductType)objNode.Tag;
                    objCalcPlanItemProductType.PriceAvgEXW = System.Convert.ToDecimal(objNode.GetValue(colPriceAvgEXW));
                    objCalcPlanItemProductType.KoeffPlan = System.Convert.ToDecimal(objNode.GetValue(colPlanKoeff));
                    objCalcPlanItemProductType.CalcPlanItemList = new List<CCalcPlanItem>();
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.January, ((objNode.GetValue(colMonth1Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth1Quantity))), ((objNode.GetValue(colMonth1AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth1AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.Febrary, ((objNode.GetValue(colMonth2Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth2Quantity))), ((objNode.GetValue(colMonth2AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth2AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.March, ((objNode.GetValue(colMotnh3Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMotnh3Quantity))), ((objNode.GetValue(colMonth3AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth3AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.April, ((objNode.GetValue(colMonth4Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth4Quantity))), ((objNode.GetValue(colMonth4AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth4AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.May, ((objNode.GetValue(colMonth5Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth5Quantity))), ((objNode.GetValue(colMonth5AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth5AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.June, ((objNode.GetValue(colMonth6Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth6Quantity))), ((objNode.GetValue(colMonth6AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth6AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.July, ((objNode.GetValue(colMonth7Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth7Quantity))), ((objNode.GetValue(colMonth7AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth7AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.August, ((objNode.GetValue(colMonth8Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth8Quantity))), ((objNode.GetValue(colMonth8AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth8AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.September, ((objNode.GetValue(colMonth9Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth9Quantity))), ((objNode.GetValue(colMonth9AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth9AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.Ocntober, ((objNode.GetValue(colMonth10Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth10Quantity))), ((objNode.GetValue(colMonth10AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth10AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.November, ((objNode.GetValue(colMonth11Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth11Quantity))), ((objNode.GetValue(colMonth11AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth11AllPrice)))));
                    objCalcPlanItemProductType.CalcPlanItemList.Add(new CCalcPlanItem(enMonth.December, ((objNode.GetValue(colMonth12Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth12Quantity))), ((objNode.GetValue(colMonth12AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth12AllPrice)))));
                    m_objCalcPlanProductOwnreForSave.CalcPlanItemProductTypeList.Add(objCalcPlanItemProductType );
                }

                //SimulateChangeSetting(enumAction.SaveBtnClick, m_objCalcPlanProductOwnreForSave);

                bRet = true;

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
        }

        public CCalcPlanItemProductOwner GetCalcPlanProductOwnreForSave()
        {
            CCalcPlanItemProductOwner objRet = null;
            try
            {
                if (bSaveChanges() == true)
                {
                    objRet = m_objCalcPlanProductOwnreForSave;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return objRet;
        }
        #endregion

        #region Добавление товарных групп
        private void checkedListBoxProductType_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                ERP_Mercury.Common.CProductType objItem =  ( ERP_Mercury.Common.CProductType )checkedListBoxProductType.Items[ e.Index ].Value;
                if( objItem.ID.CompareTo( System.Guid.Empty ) != 0 ){return;}

                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                if (checkedListBoxProductType.Items.Count > 1)
                {
                    for (System.Int32 i = 1; i < checkedListBoxProductType.Items.Count; i++)
                    {
                        if ((m_objProductTypeDefault != null) && (((ERP_Mercury.Common.CProductType)checkedListBoxProductType.Items[i].Value).ID.CompareTo(m_objProductTypeDefault.ID) == 0))
                        {
                            // в этой строчке товарная группа "-", она используется, когда план вносится только по марке
                            // если мы ткнули в строку "ВСЕ", то эту группу "-" выделять не нужно
                            if (((checkedListBoxProductType.Items[i].CheckState != e.State)) && (e.State == CheckState.Unchecked))
                            {
                                checkedListBoxProductType.Items[i].CheckState = e.State;
                            }
                        }
                        else
                        {
                            if (checkedListBoxProductType.Items[i].CheckState != e.State)
                            {
                                checkedListBoxProductType.Items[i].CheckState = e.State;
                            }
                        }
                    }
                }

                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("checkedListBoxProductType_ItemCheck. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Удаляет строку из плана продаж
        /// </summary>
        /// <param name="objNode">узел</param>
        private void RemoveProductTypeFromPlan(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if (objNode.Tag == null) { return; }
                CCalcPlanItemProductType objCalcPlanItemProductType = (CCalcPlanItemProductType)objNode.Tag;

                //this.tableLayoutPanel1.SuspendLayout();
                //((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                //((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).BeginInit();

                treeList.Nodes.Remove(objNode);
                System.Boolean bExist = false;

                for (System.Int32 i = 0; i < checkedListBoxProductType.Items.Count; i++)
                {
                    if (((ERP_Mercury.Common.CProductType)checkedListBoxProductType.Items[i].Value).ID.CompareTo(objCalcPlanItemProductType.ProductType.ID) == 0 )
                    {
                        checkedListBoxProductType.Items[i].CheckState = CheckState.Unchecked;
                        bExist = true;
                        break;
                    }
                }
                if (bExist == false)
                {
                    checkedListBoxProductType.Items.Add(objCalcPlanItemProductType.ProductType, false);
                }

                SetPropertiesModified(true);

                //this.tableLayoutPanel1.ResumeLayout(false);
                //((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
                //((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("RemoveProductTypeFromPlan. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeList.FocusedNode == null) { return; }
                RemoveProductTypeFromPlan(treeList.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnRemove_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        /// <summary>
        /// Добавляет строки в план продаж
        /// </summary>
        private void AddPartTypeToPlan()
        {
            try
            {
                if (checkedListBoxProductType.Items.Count <= 1) { return; }
                if (checkedListBoxProductType.CheckedItems.Count == 0) { return; }
                //this.tableLayoutPanel1.SuspendLayout();
                //((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                //((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).BeginInit();

                System.Boolean bExist = false;

                for (System.Int32 i = 0; i < checkedListBoxProductType.Items.Count; i++)
                {
                    if (((ERP_Mercury.Common.CProductType)checkedListBoxProductType.Items[i].Value).Name == m_strAllItems) { continue; }
                    if (checkedListBoxProductType.Items[i].CheckState == CheckState.Checked)
                    {
                        bExist = false;
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                        {
                            if (objNode.Tag == null) { continue; }
                            if (((CCalcPlanItemProductType)objNode.Tag).ProductType.ID.CompareTo(((ERP_Mercury.Common.CProductType)checkedListBoxProductType.Items[i].Value).ID) == 0)
                            {
                                bExist = true;
                                break;
                            }
                        }

                        if (bExist == false)
                        {
                            // добавляем строку в план
                            CCalcPlanItemProductType objCalcPlanItemProductType = new CCalcPlanItemProductType((ERP_Mercury.Common.CProductType)checkedListBoxProductType.Items[i].Value, 0, 0);
                            treeList.AppendNode(new object[] { objCalcPlanItemProductType.ProductType.Name, objCalcPlanItemProductType.PriceAvgEXW, objCalcPlanItemProductType.KoeffPlan,
                              objCalcPlanItemProductType.GetPlanQty(enMonth.January), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.January),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.Febrary), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.Febrary),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.March), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.March),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.April), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.April),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.May), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.May),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.June), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.June),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.July), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.July),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.August), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.August),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.September), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.September),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.Ocntober), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.Ocntober),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.November), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.November),
                              objCalcPlanItemProductType.GetPlanQty(enMonth.December), objCalcPlanItemProductType.GetPlanAllMoney(enMonth.December),
                              objCalcPlanItemProductType.Quantity, objCalcPlanItemProductType.AllMoney
                            }, null).Tag = objCalcPlanItemProductType;
                        }
                    }
                }

                SetPropertiesModified(true);

                //this.tableLayoutPanel1.ResumeLayout(false);
                //((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
                //((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("AddPartTypeToPlan. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddPartTypeToPlan();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnAdd_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Печать
        /// <summary>
        /// Экспорт плана в MS Excel
        /// </summary>
        public void ExportToExcel()
        {
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
                    oSheet.Cells[1, 1] = m_objCalcPlan.Year;
                    oSheet.Cells[2, 1] = m_objCalcPlan.Name;
                    oSheet.Cells[3, 1] = m_objCalcPlanProductOwner.ProductOwner.Name;
                    // Имена столбцов
                    oSheet.Cells[5, 1] = "Товарная группа";
                    oSheet.Cells[5, 2] = "Цена exw (средняя)";
                    oSheet.Cells[5, 3] = "Коэффициент";
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
                CCalcPlanItemProductType objCalcPlanItemProductType = null;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }

                    objCalcPlanItemProductType = (CCalcPlanItemProductType)objNode.Tag;


                    oSheet.Cells[iStartRowNum, 1] = objCalcPlanItemProductType.ProductType.Name;
                    oSheet.Cells[iStartRowNum, 2] = System.Convert.ToDecimal(objNode.GetValue(colPriceAvgEXW));
                    oSheet.Cells[iStartRowNum, 3] = System.Convert.ToDecimal(objNode.GetValue(colPlanKoeff));

                    oSheet.Cells[iStartRowNum, 4] = ((objNode.GetValue(colMonth1Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth1Quantity)));
                    oSheet.Cells[iStartRowNum, 5] = ((objNode.GetValue(colMonth2Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth2Quantity)));
                    oSheet.Cells[iStartRowNum, 6] = ((objNode.GetValue(colMotnh3Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMotnh3Quantity)));
                    oSheet.Cells[iStartRowNum, 7] = ((objNode.GetValue(colMonth4Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth4Quantity)));
                    oSheet.Cells[iStartRowNum, 8] = ((objNode.GetValue(colMonth5Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth5Quantity)));
                    oSheet.Cells[iStartRowNum, 9] = ((objNode.GetValue(colMonth6Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth6Quantity)));
                    oSheet.Cells[iStartRowNum, 10] = ((objNode.GetValue(colMonth7Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth7Quantity)));
                    oSheet.Cells[iStartRowNum, 11] = ((objNode.GetValue(colMonth8Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth8Quantity)));
                    oSheet.Cells[iStartRowNum, 12] = ((objNode.GetValue(colMonth9Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth9Quantity)));
                    oSheet.Cells[iStartRowNum, 13] = ((objNode.GetValue(colMonth10Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth10Quantity)));
                    oSheet.Cells[iStartRowNum, 14] = ((objNode.GetValue(colMonth11Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth11Quantity)));
                    oSheet.Cells[iStartRowNum, 15] = ((objNode.GetValue(colMonth12Quantity) == null) ? 0 : System.Convert.ToInt32(objNode.GetValue(colMonth12Quantity)));
                    oSheet.get_Range(oSheet.Cells[iStartRowNum, 16], oSheet.Cells[iStartRowNum, 16]).Formula = "=СУММ(RC[-12]:RC[-1])";

                    oSheet2.Cells[iStartRowNum, 1] = objCalcPlanItemProductType.ProductType.Name;
                    oSheet2.Cells[iStartRowNum, 2] = System.Convert.ToDecimal(objNode.GetValue(colPriceAvgEXW));
                    oSheet2.Cells[iStartRowNum, 3] = System.Convert.ToDecimal(objNode.GetValue(colPlanKoeff));

                    oSheet2.Cells[iStartRowNum, 4] = ((objNode.GetValue(colMonth1AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth1AllPrice)));
                    oSheet2.Cells[iStartRowNum, 5] = ((objNode.GetValue(colMonth2AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth2AllPrice)));
                    oSheet2.Cells[iStartRowNum, 6] = ((objNode.GetValue(colMonth3AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth3AllPrice)));
                    oSheet2.Cells[iStartRowNum, 7] = ((objNode.GetValue(colMonth4AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth4AllPrice)));
                    oSheet2.Cells[iStartRowNum, 8] = ((objNode.GetValue(colMonth5AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth5AllPrice)));
                    oSheet2.Cells[iStartRowNum, 9] = ((objNode.GetValue(colMonth6AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth6AllPrice)));
                    oSheet2.Cells[iStartRowNum, 10] = ((objNode.GetValue(colMonth7AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth7AllPrice)));
                    oSheet2.Cells[iStartRowNum, 11] = ((objNode.GetValue(colMonth8AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth8AllPrice)));
                    oSheet2.Cells[iStartRowNum, 12] = ((objNode.GetValue(colMonth9AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth9AllPrice)));
                    oSheet2.Cells[iStartRowNum, 13] = ((objNode.GetValue(colMonth10AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth10AllPrice)));
                    oSheet2.Cells[iStartRowNum, 14] = ((objNode.GetValue(colMonth11AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth11AllPrice)));
                    oSheet2.Cells[iStartRowNum, 15] = ((objNode.GetValue(colMonth12AllPrice) == null) ? 0 : System.Convert.ToDecimal(objNode.GetValue(colMonth12AllPrice)));

                    oSheet2.get_Range(oSheet2.Cells[iStartRowNum, 16], oSheet2.Cells[iStartRowNum, 16]).Formula = "=СУММ(RC[-12]:RC[-1])";

                    iStartRowNum++;
                }

                for (System.Int32 i2 = 1; i2 <= 2; i2++)
                {
                    oSheet = (Excel._Worksheet)oWB.Worksheets[i2];
                    if (oSheet != null)
                    {
                        for (System.Int32 i = 3; i <= 15; i++)
                        {
                            oSheet.get_Range(oSheet.Cells[iStartRowNum, i], oSheet.Cells[iStartRowNum, i]).Formula = "=СУММ(R[-" + (iStartRowNum - 6).ToString() + "]C:R[-1]C)";

                            oSheet.get_Range(oSheet.Cells[iStartRowNum, i], oSheet.Cells[iStartRowNum, i]).NumberFormat = ( ( i2 == 1 ) ? "# ##0" : "# ##0,00" );
                            oSheet.get_Range(oSheet.Cells[iStartRowNum, i], oSheet.Cells[iStartRowNum, i]).Font.Bold = true;
                            oSheet.get_Range(oSheet.Cells[iStartRowNum, i], oSheet.Cells[iStartRowNum, i]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        }
                        oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();
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
        #endregion

        #region Пересчет плана

        private decimal RoundToTen(decimal dcmlValue)
        {
            decimal dRet = dcmlValue;
            try
            {
                dRet = System.Math.Round((dcmlValue / 10), 0) * 10;
            }
            catch (System.Exception f)
            {
                SendMessageToLog( dcmlValue.ToString() + "RoundToTen. Текст ошибки: " + f.Message);
            }
            return dRet;
        }

        private void RecalcPlanWithEXW(CCalcPlanKoef objCalcPlanKoef)
        {
            try
            {
                if (objCalcPlanKoef == null) { return; }

                System.String strErr = "";
                objCalcPlanKoef.CalcPlanKoefItemList = CCalcPlanKoefItem.GetCalcOrderItemListForProductOwner(m_objProfile, null, objCalcPlanKoef.ID, m_objCalcPlanProductOwner.ProductOwner.uuidID, ref strErr);

                //if ((objCalcPlanKoef.CalcPlanKoefItemList == null) || (objCalcPlanKoef.CalcPlanKoefItemList.Count == 0))
                //{
                //    System.String strErr = "";
                //    objCalcPlanKoef.CalcPlanKoefItemList = CCalcPlanKoefItem.GetCalcOrderItemListForProductOwner(m_objProfile, null, objCalcPlanKoef.ID, m_objCalcPlanProductOwner.ProductOwner.uuidID, ref strErr);
                //}
                if ((objCalcPlanKoef.CalcPlanKoefItemList != null) && (objCalcPlanKoef.CalcPlanKoefItemList.Count > 0))
                {
                    this.tableLayoutPanel1.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                     CCalcPlanItemProductType objCalcPlanItemProductType = null;

                     System.Boolean bProductTypeExists = false;

                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        if (objNode.Tag == null) { continue; }
                        objCalcPlanItemProductType = (CCalcPlanItemProductType)objNode.Tag;

                        bProductTypeExists = false;
                        for (System.Int32 i2 = 0; i2 < checkedListBoxCalcProductType.Items.Count; i2++)
                        {
                            if (checkedListBoxCalcProductType.Items[i2].CheckState == CheckState.Checked)
                            {
                                if (((ERP_Mercury.Common.CProductType)checkedListBoxCalcProductType.Items[i2].Value).ID.CompareTo(objCalcPlanItemProductType.ProductType.ID) == 0)
                                {
                                    bProductTypeExists = true;
                                    break;
                                }
                            }
                        }

                        if (bProductTypeExists == false) { continue; }

                        foreach (CCalcPlanKoefItem objKoef in objCalcPlanKoef.CalcPlanKoefItemList)
                        {
                            if( ( objKoef.ProductOwnerIB_Id.CompareTo( m_objCalcPlanProductOwner.ProductOwner.Ib_Id ) == 0 ) && 
                                ( (objKoef.ProductTypeId.CompareTo(objCalcPlanItemProductType.ProductType.ID) == 0) ) )
                            {
                                // для строки в плане мы нашли строку с коэффициентом и средней ценой exw
                                objNode.SetValue(colPriceAvgEXW, objKoef.AVG_EW_PRICE);
                                objNode.SetValue(colPlanKoeff, objKoef.KOEFF);

                                if (bIsNeedRecalcMonth(colMonth1AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth1AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth1Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth2AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth2AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth2Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth3AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth3AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMotnh3Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth4AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth4AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth4Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth5AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth5AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth5Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth6AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth6AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth6Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth7AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth7AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth7Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth8AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth8AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth8Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth9AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth9AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth9Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth10AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth10AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth10Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth11AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth11AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth11Quantity)))));
                                }
                                if (bIsNeedRecalcMonth(colMonth12AllPrice) == true)
                                {
                                    objNode.SetValue(colMonth12AllPrice, RoundToTen((objKoef.KOEFF * objKoef.AVG_EW_PRICE * System.Convert.ToInt32(objNode.GetValue(colMonth12Quantity)))));
                                }

                                RecalcItog(objNode);
                            }
                        }
                    }
                    objCalcPlanItemProductType = null;
                    RecalcTotalItog();

                    this.tableLayoutPanel1.ResumeLayout(false);
                    ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                    SetPropertiesModified(true);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("RecalcPlanWithEXW. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void cboxCalcPlanKoef_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                btnReCalc.Enabled = (cboxCalcPlanKoef.SelectedItem != null) && ( treeList.Nodes.Count > 0 );                
            }
            catch (System.Exception f)
            {
                SendMessageToLog("cboxCalcPlanKoef_SelectedValueChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void btnReCalc_Click(object sender, EventArgs e)
        {
            try
            {
                if ((cboxCalcPlanKoef.SelectedItem != null) && (treeList.Nodes.Count > 0))
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show( "Подтвердите начало операции пересчета сумм.", "Подтверждение",
                     System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes )
                    {
                        SendMessageToLog("идет пересчет сумм плана...");
                        this.Refresh();
                        this.Cursor = Cursors.WaitCursor;

                        RecalcPlanWithEXW((CCalcPlanKoef)cboxCalcPlanKoef.SelectedItem);

                        tabControlCalcItems.SelectedTabPage = tabPageCalcItems;

                        SendMessageToLog("завершен пересчет сумм плана");
                        this.Refresh();
                        this.Cursor = Cursors.Default;
                        treeList.Refresh();
                    }

                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnReCalc_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void checkedListBoxMonth_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                System.String objItem = (System.String)checkedListBoxMonth.Items[e.Index].Value;
                if (objItem != m_strAllItems) { return; }

                this.tableLayoutPanel4.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).BeginInit();

                if (checkedListBoxMonth.Items.Count > 1)
                {
                    for (System.Int32 i = 1; i < checkedListBoxMonth.Items.Count; i++)
                    {
                        if (checkedListBoxMonth.Items[i].CheckState != e.State)
                        {
                            checkedListBoxMonth.Items[i].CheckState = e.State;
                        }
                    }
                }

                this.tableLayoutPanel4.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxProductType)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("checkedListBoxMonth_ItemCheck. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        System.Boolean bIsNeedRecalcMonth(DevExpress.XtraTreeList.Columns.TreeListColumn objColumn)
        {
            System.Boolean bRet = false;
            try
            {
                enMonth iMonth = enMonth.UnKown;
                if (objColumn == colMonth1AllPrice) { iMonth = enMonth.January; }
                if (objColumn == colMonth2AllPrice) { iMonth = enMonth.Febrary; }
                if (objColumn == colMonth3AllPrice) { iMonth = enMonth.March; }
                if (objColumn == colMonth4AllPrice) { iMonth = enMonth.April; }
                if (objColumn == colMonth5AllPrice) { iMonth = enMonth.May; }
                if (objColumn == colMonth6AllPrice) { iMonth = enMonth.June; }
                if (objColumn == colMonth7AllPrice) { iMonth = enMonth.July; }
                if (objColumn == colMonth8AllPrice) { iMonth = enMonth.August; }
                if (objColumn == colMonth9AllPrice) { iMonth = enMonth.September; }
                if (objColumn == colMonth10AllPrice) { iMonth = enMonth.Ocntober; }
                if (objColumn == colMonth11AllPrice) { iMonth = enMonth.November; }
                if (objColumn == colMonth12AllPrice) { iMonth = enMonth.December; }

                if (checkedListBoxMonth.Items.Count >= 13)
                {
                    bRet = (checkedListBoxMonth.Items[ System.Convert.ToInt32( iMonth ) ].CheckState == CheckState.Checked);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("bIsNeedRecalcMonth. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
        }

        #endregion

        private void checkedListBoxCalcProductType_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                ERP_Mercury.Common.CProductType objItem = (ERP_Mercury.Common.CProductType)checkedListBoxCalcProductType.Items[e.Index].Value;
                if (objItem.Name != m_strAllItems) { return; }

                this.tableLayoutPanel4.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxCalcProductType)).BeginInit();

                if (checkedListBoxCalcProductType.Items.Count > 1)
                {
                    for (System.Int32 i = 1; i < checkedListBoxCalcProductType.Items.Count; i++)
                    {
                        if (checkedListBoxCalcProductType.Items[i].CheckState != e.State)
                        {
                            checkedListBoxCalcProductType.Items[i].CheckState = e.State;
                        }
                    }
                }

                this.tableLayoutPanel4.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxCalcProductType)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("checkedListBoxCalcProductType_ItemCheck. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void mitemShowColumnsMoney_Click(object sender, EventArgs e)
        {
            try
            {
                mitemShowColumnsMoney.Checked = !mitemShowColumnsMoney.Checked;

                ShowColumnsMoney(mitemShowColumnsMoney.Checked);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemShowColumnsMoney_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void ShowColumnsMoney(System.Boolean bShow)
        {
            try
            {
                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                {
                    if (IsColumnForEditMoney(objColumn) == true)
                    {
                        if (objColumn.Visible != bShow)
                        {
                            objColumn.Visible = bShow;
                            //if (bShow == true)
                            //{
                            //    objColumn.VisibleIndex = System.Convert.ToInt32( objColumn.Tag );
                            //}
                        }
                    }
                }

                if (bShow == true)
                {
                    foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                    {
                        objColumn.VisibleIndex = System.Convert.ToInt32(objColumn.Tag);
                    }
                }


            }
            catch (System.Exception f)
            {
                SendMessageToLog("ShowColumnsMoney. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void contextMenuStrip_Click(object sender, EventArgs e)
        {

        }


    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public class ChangeCalcPlanItemEventArgs : EventArgs
    {
        private readonly enumAction m_enActionType;
        public enumAction ActionType
        { get { return m_enActionType; } }

        private readonly CCalcPlanItemProductOwner m_objCalcPlanItemProductOwner;
        public CCalcPlanItemProductOwner CalcPlanItemProductOwner
        { get { return m_objCalcPlanItemProductOwner; } }


        public ChangeCalcPlanItemEventArgs(enumAction enActionType, CCalcPlanItemProductOwner objCalcPlanItemProductOwner)
        {
            m_enActionType = enActionType;
            m_objCalcPlanItemProductOwner = objCalcPlanItemProductOwner;
        }
    }

}
