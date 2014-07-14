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

namespace ERPMercuryPlan
{
    public partial class frmSalesPlanItemEditor : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private CProfile m_objProfile;
        private List<CProductTradeMark> m_objProductTradeMarkList;
        private List<CProductType> m_objProductTypeList;
        private List<CDepart> m_objDepartList;
        private List<CProductSubType> m_objProductSubTypeList;
        private List<CCustomer> m_objCustomerList;
        public CPlanByDepartCustomerSubTypeItem PlanItem { get; set; }
        //private System.Boolean m_bNewObject;
        #endregion

        #region Конструктор
        public frmSalesPlanItemEditor(CProfile objProfile)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objProductTradeMarkList = null;
            m_objProductTypeList = null;
            m_objDepartList = null;
            m_objProductSubTypeList = null;
            m_objCustomerList = null;
            PlanItem = null;
            //m_bNewObject = false;

        }
        #endregion

        #region Загрузка содержимого выпадающих списков
        public void LoadComboBox( List<CDepart> objDepartList, List<CCustomer> objCustomerList,
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

                cboxProductTradeMark.Properties.Items.Clear();
                cboxProductType.Properties.Items.Clear();
                cboxDepart.Properties.Items.Clear();
                cboxCustomer.Properties.Items.Clear();
                cboxProductSubType.Properties.Items.Clear();

                cboxProductTradeMark.Properties.Items.AddRange(m_objProductTradeMarkList);
                cboxProductType.Properties.Items.AddRange(m_objProductTypeList);
                cboxDepart.Properties.Items.AddRange(m_objDepartList);
                cboxCustomer.Properties.Items.AddRange(m_objCustomerList);
                cboxProductSubType.Properties.Items.AddRange(m_objProductSubTypeList);

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

        #region Новый элемент в приложении к плану
        /// <summary>
        /// Добавление нового элемента к приложению в плане
        /// </summary>
        public void AddPlanItemForGrid()
        {
            try
            {
                //m_bNewObject = true;
                PlanItem = new CPlanByDepartCustomerSubTypeItem();

                LoadPropertiesPlanItemInControls();

                DialogResult = System.Windows.Forms.DialogResult.None;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("AddsPlanItemForGrid.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Редактировать элемент в приложении к плану
        /// <summary>
        /// Отображает свойства элемента приложения к плану
        /// </summary>
        private void LoadPropertiesPlanItemInControls()
        {
            try
            {
                cboxProductTradeMark.SelectedItem = null;
                cboxProductType.SelectedItem = null;
                cboxDepart.SelectedItem = null;
                cboxCustomer.SelectedItem = null;
                cboxProductSubType.SelectedItem = null;
                calcPlanQuantity.Value = 0;
                calcPlanMoney.Value = 0;

                if ( PlanItem != null)
                {
                    cboxProductTradeMark.SelectedItem = (PlanItem.ProductOwner == null) ? null : cboxProductTradeMark.Properties.Items.Cast<CProductTradeMark>().SingleOrDefault<CProductTradeMark>(x => x.ID.CompareTo(PlanItem.ProductOwner.ID) == 0);
                    cboxProductType.SelectedItem = (PlanItem.ProductType == null) ? null : cboxProductType.Properties.Items.Cast<CProductType>().SingleOrDefault<CProductType>(x => x.ID.CompareTo(PlanItem.ProductType.ID) == 0);
                    cboxDepart.SelectedItem = (PlanItem.Depart == null) ? null : cboxDepart.Properties.Items.Cast<CDepart>().SingleOrDefault<CDepart>(x => x.uuidID.CompareTo(PlanItem.Depart.uuidID) == 0);
                    cboxCustomer.SelectedItem = (PlanItem.Customer == null) ? null : cboxCustomer.Properties.Items.Cast<CCustomer>().SingleOrDefault<CCustomer>(x => x.ID.CompareTo(PlanItem.Customer.ID) == 0);
                    cboxProductSubType.SelectedItem = (PlanItem.ProductSubType == null) ? null : cboxProductSubType.Properties.Items.Cast<CProductSubType>().SingleOrDefault<CProductSubType>(x => x.ID.CompareTo(PlanItem.ProductSubType.ID) == 0);

                    calcPlanQuantity.Value = PlanItem.Plan_Quantity;
                    calcPlanMoney.Value = PlanItem.Plan_AllPrice;

                    //cboxProductTradeMark.Properties.ReadOnly = !(m_bNewObject);
                    //cboxProductType.Properties.ReadOnly = !(m_bNewObject);
                    //cboxCustomer.Properties.ReadOnly = !(m_bNewObject);
                    //cboxDepart.Properties.ReadOnly = !(m_bNewObject);
                    //cboxProductSubType.Properties.ReadOnly = !(m_bNewObject);

                    calcPlanQuantity.SelectAll();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadPropertiesPlanItemInControls.\n\nТекст ошибки: " + f.Message, "Ошибка",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Редактирование свойств элемента приложения к плану
        /// </summary>
        /// <param name="objPlanItem">элемент приложения к плану</param>
        public void EditPlanItem(CPlanByDepartCustomerSubTypeItem objPlanItem)
        {
            try
            {
                //m_bNewObject = false;

                PlanItem = objPlanItem;

                LoadPropertiesPlanItemInControls();

                DialogResult = System.Windows.Forms.DialogResult.None;
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
        #endregion

        #region Отмена изменений
        /// <summary>
        /// Закрывает форму
        /// </summary>
        private void Cancel()
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        #endregion

        #region Подтверждение изменений
        /// <summary>
        /// Подтверждает изменения в элементе приложения к плану
        /// </summary>
        private void Save()
        {
            try
            {
                if (PlanItem != null)
                {
                    if (cboxProductTradeMark.SelectedItem == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, товарную марку.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }
                    if (cboxProductType.SelectedItem == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, товарную группу.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }
                    if (cboxProductSubType.SelectedItem == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, товарную подгруппу.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }
                    if (cboxDepart.SelectedItem == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, подразделение.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }
                    if (cboxCustomer.SelectedItem == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, клиента.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }
                    if (calcPlanMoney.Value <= 0)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Сумма плана должна быть больше нуля.\nИсправьте, пожалуйста.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }
                    if ( calcPlanQuantity.Value <= 0)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Количество должно быть больше нуля.\nИсправьте, пожалуйста.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }

                    PlanItem.ProductOwner = (CProductTradeMark)cboxProductTradeMark.SelectedItem;
                    PlanItem.ProductType = (CProductType)cboxProductType.SelectedItem;
                    PlanItem.Depart = (CDepart)cboxDepart.SelectedItem;
                    PlanItem.Customer = (CCustomer)cboxCustomer.SelectedItem;
                    PlanItem.ProductSubType = (CProductSubType)cboxProductSubType.SelectedItem;
                    PlanItem.Plan_Quantity = calcPlanQuantity.Value;
                    PlanItem.Plan_AllPrice = calcPlanMoney.Value;

                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                else
                {
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Save.\n\nТекст ошибки: " + f.Message, "Ошибка",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
        #endregion

    }
}
