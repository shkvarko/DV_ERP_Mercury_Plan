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
    public partial class frmSalesPlanQuotaItemEditor : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private CProfile m_objProfile;
        private List<CProductTradeMark> m_objProductTradeMarkList;
        private List<CProductType> m_objProductTypeList;
        private List<CDepartTeam> m_objDepartTeamList;
        private List<CDepart> m_objDepartList;
        private List<CProductSubType> m_objProductSubTypeList;
        private List<CCustomer> m_objCustomerList;
        public CSalesPlanQuotaItemForGrid SalesPlanQuotaItemForGrid {get; set;}
        private System.Boolean m_bNewObject;
        #endregion

        #region Конструктор
        public frmSalesPlanQuotaItemEditor(CProfile objProfile)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objProductTradeMarkList = null;
            m_objProductTypeList = null;
            m_objDepartTeamList = null;
            m_objDepartList = null;
            m_objProductSubTypeList = null;
            m_objCustomerList = null;
            SalesPlanQuotaItemForGrid = null;
            m_bNewObject = false;

            tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
        }
        #endregion

        #region Загрузка содержимого выпадающих списков
        public void LoadComboBox(List<CDepartTeam> objDepartTeamList,
           List<CDepart> objDepartList, List<CCustomer> objCustomerList,
           List<CProductSubType> objProductSubTypeList, List<CProductTradeMark> objProductTradeMarkList,
           List<CProductType> objProductTypeList)
        {
            try
            {
                m_objProductTradeMarkList = objProductTradeMarkList;
                m_objProductTypeList = objProductTypeList;
                m_objDepartTeamList = objDepartTeamList;
                m_objDepartList = objDepartList;
                m_objProductSubTypeList = objProductSubTypeList;
                m_objCustomerList = objCustomerList;

                cboxProductTradeMark.Properties.Items.Clear();
                cboxProductType.Properties.Items.Clear();
                cboxDepartTeam.Properties.Items.Clear();
                cboxDepart.Properties.Items.Clear();
                cboxCustomer.Properties.Items.Clear();
                cboxProductSubType.Properties.Items.Clear();

                cboxProductTradeMark.Properties.Items.AddRange(m_objProductTradeMarkList);
                cboxProductType.Properties.Items.AddRange(m_objProductTypeList);
                cboxDepartTeam.Properties.Items.AddRange(m_objDepartTeamList);
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

        #region Новый элемент в приложении к расчёту
        /// <summary>
        /// Добавление нового элемента к приложению в расчёте
        /// </summary>
        /// <param name="enumQuotaObjectType">тип объекта</param>
        public void AddSalesPlanQuotaItemForGrid( enQuotaObjectType enumQuotaObjectType )
        {
            try
            {
                m_bNewObject = true;
                SalesPlanQuotaItemForGrid = new CSalesPlanQuotaItemForGrid();
                SalesPlanQuotaItemForGrid.Object_QuotaObjectType = enumQuotaObjectType;

                LoadPropertiesSalesPlanQuotaItemInControls();

                DialogResult = System.Windows.Forms.DialogResult.None;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("AddSalesPlanQuotaItemForGrid.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Редактировать элемент в приложении к расчёту
        /// <summary>
        /// Отображает свойства элемента приложения к расчёту
        /// </summary>
        private void LoadPropertiesSalesPlanQuotaItemInControls()
        {
            try
            {
                cboxProductTradeMark.SelectedItem = null;
                cboxProductType.SelectedItem = null;
                cboxDepartTeam.SelectedItem = null;
                cboxDepart.SelectedItem = null;
                cboxCustomer.SelectedItem = null;
                cboxProductSubType.SelectedItem = null;
                calcObject_Quota.Value = 0;
                calcSalesQuantity.Value = 0;
                calcSalesMoney.Value = 0;
                calcObject_SalesQuantity.Value = 0;
                calcObject_SalesMoney.Value = 0;
                calcObject_CalcQuota.Value = 0;
                lblObjectType.Text = "";

                if( SalesPlanQuotaItemForGrid != null )
                {
                    cboxProductTradeMark.SelectedItem = (SalesPlanQuotaItemForGrid.ProductTradeMark == null) ? null : cboxProductTradeMark.Properties.Items.Cast<CProductTradeMark>().SingleOrDefault<CProductTradeMark>(x => x.ID.CompareTo( SalesPlanQuotaItemForGrid.ProductTradeMark.ID) == 0);
                    cboxProductType.SelectedItem = (SalesPlanQuotaItemForGrid.ProductType == null) ? null : cboxProductType.Properties.Items.Cast<CProductType>().SingleOrDefault<CProductType>(x => x.ID.CompareTo( SalesPlanQuotaItemForGrid.ProductType.ID) == 0);
                
                    switch( SalesPlanQuotaItemForGrid.Object_QuotaObjectType )
                    {
                        case enQuotaObjectType.DepartTeam:
                            cboxDepartTeam.SelectedItem = (SalesPlanQuotaItemForGrid.Object_ID.CompareTo(System.Guid.Empty) == 0) ? null : cboxDepartTeam.Properties.Items.Cast<CDepartTeam>().SingleOrDefault<CDepartTeam>(x => x.uuidID.CompareTo( SalesPlanQuotaItemForGrid.Object_ID) == 0);
                            lblObjectType.Text = "Команда:";
                            tabControl.SelectedTabPage = tabPageDepartTeam;
                            break;
                        case enQuotaObjectType.Depart:
                            cboxDepart.SelectedItem = (SalesPlanQuotaItemForGrid.Object_ID.CompareTo(System.Guid.Empty) == 0) ? null : cboxDepart.Properties.Items.Cast<CDepart>().SingleOrDefault<CDepart>(x => x.uuidID.CompareTo( SalesPlanQuotaItemForGrid.Object_ID) == 0);
                            lblObjectType.Text = "Подр-е:";
                            tabControl.SelectedTabPage = tabPageDepart;
                            break;
                        case enQuotaObjectType.Customer:
                            cboxCustomer.SelectedItem = (SalesPlanQuotaItemForGrid.Object_ID.CompareTo(System.Guid.Empty) == 0) ? null : cboxCustomer.Properties.Items.Cast<CCustomer>().SingleOrDefault<CCustomer>(x => x.ID.CompareTo( SalesPlanQuotaItemForGrid.Object_ID) == 0);
                            lblObjectType.Text = "Клиент:";
                            tabControl.SelectedTabPage = tabPageCustomer;
                            break;
                        case enQuotaObjectType.ProductSubType:
                            cboxProductSubType.SelectedItem = (SalesPlanQuotaItemForGrid.Object_ID.CompareTo(System.Guid.Empty) == 0) ? null : cboxProductSubType.Properties.Items.Cast<CProductSubType>().SingleOrDefault<CProductSubType>(x => x.ID.CompareTo( SalesPlanQuotaItemForGrid.Object_ID) == 0);
                            lblObjectType.Text = "Подгруппа:";
                            tabControl.SelectedTabPage = tabPageProductSubType;
                            break;
                        default:
                            break;
                    }

                    calcObject_Quota.Value = SalesPlanQuotaItemForGrid.Object_Quota;
                    calcSalesQuantity.Value = SalesPlanQuotaItemForGrid.SalesQuantity;
                    calcSalesMoney.Value = SalesPlanQuotaItemForGrid.SalesMoney;
                    calcObject_SalesQuantity.Value = SalesPlanQuotaItemForGrid.Object_SalesQuantity;
                    calcObject_SalesMoney.Value = SalesPlanQuotaItemForGrid.Object_SalesMoney;
                    calcObject_CalcQuota.Value = SalesPlanQuotaItemForGrid.Object_CalcQuota;

                    cboxProductTradeMark.Properties.ReadOnly = !(m_bNewObject);
                    cboxProductType.Properties.ReadOnly = !(m_bNewObject);
                    cboxCustomer.Properties.ReadOnly = !(m_bNewObject);
                    cboxDepart.Properties.ReadOnly = !(m_bNewObject);
                    cboxDepartTeam.Properties.ReadOnly = !(m_bNewObject);
                    cboxProductSubType.Properties.ReadOnly = !(m_bNewObject);

                    calcObject_Quota.SelectAll();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadPropertiesSalesPlanQuotaItemInControls.\n\nТекст ошибки: " + f.Message, "Ошибка",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Редактирование свойств элемента приложения к расчёту
        /// </summary>
        /// <param name="objSalesPlanQuotaItemForGrid">элемент приложения к расчёту</param>
        public void EditSalesPlanQuotaItemForGrid( CSalesPlanQuotaItemForGrid objSalesPlanQuotaItemForGrid )
        {
            try
            {
                m_bNewObject = false;

                SalesPlanQuotaItemForGrid = objSalesPlanQuotaItemForGrid;

                LoadPropertiesSalesPlanQuotaItemInControls();

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
        /// Подтверждает изменения в элементе приложения к расчёту
        /// </summary>
        private void Save()
        {
            try
            {
                if (SalesPlanQuotaItemForGrid != null)
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
                    if (SalesPlanQuotaItemForGrid.Object_QuotaObjectType == enQuotaObjectType.ProductSubType)
                    {
                        if (cboxProductSubType.SelectedItem == null)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, товарную подгруппу.", "Внимание!",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                            return;
                        }
                    }
                    if (SalesPlanQuotaItemForGrid.Object_QuotaObjectType == enQuotaObjectType.DepartTeam)
                    {
                        if (cboxDepartTeam.SelectedItem == null)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, команду.", "Внимание!",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                            return;
                        }
                    }
                    if (SalesPlanQuotaItemForGrid.Object_QuotaObjectType == enQuotaObjectType.Depart)
                    {
                        if (cboxDepart.SelectedItem == null)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, подразделение.", "Внимание!",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                            return;
                        }
                    }
                    if (SalesPlanQuotaItemForGrid.Object_QuotaObjectType == enQuotaObjectType.Customer)
                    {
                        if (cboxCustomer.SelectedItem == null)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, клиента.", "Внимание!",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                            return;
                        }
                    }
                    if (calcObject_Quota.Value < 0)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Значение доли в продажах не должно быть меньше нуля.\nИсправьте, пожалуйста.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }
                    if (calcObject_Quota.Value > 1)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Значение доли в продажах не должно превышать единицу.\nИсправьте, пожалуйста.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                        return;
                    }

                    if (m_bNewObject == true)
                    {
                        SalesPlanQuotaItemForGrid.ProductTradeMark = (CProductTradeMark)cboxProductTradeMark.SelectedItem;
                        SalesPlanQuotaItemForGrid.ProductType = (CProductType)cboxProductType.SelectedItem;
                        if (SalesPlanQuotaItemForGrid.Object_QuotaObjectType == enQuotaObjectType.ProductSubType)
                        {
                            SalesPlanQuotaItemForGrid.Object_ID = ((CProductSubType)cboxProductSubType.SelectedItem).ID;
                            SalesPlanQuotaItemForGrid.Object_Name = ((CProductSubType)cboxProductSubType.SelectedItem).Name;
                        }
                        if (SalesPlanQuotaItemForGrid.Object_QuotaObjectType == enQuotaObjectType.DepartTeam)
                        {
                            SalesPlanQuotaItemForGrid.Object_ID = ((CDepartTeam)cboxDepartTeam.SelectedItem).uuidID;
                            SalesPlanQuotaItemForGrid.Object_Name = ((CDepartTeam)cboxDepartTeam.SelectedItem).DepartTeamName;
                        }
                        if (SalesPlanQuotaItemForGrid.Object_QuotaObjectType == enQuotaObjectType.Depart)
                        {
                            SalesPlanQuotaItemForGrid.Object_ID = ((CDepart)cboxDepart.SelectedItem).uuidID;
                            SalesPlanQuotaItemForGrid.Object_Name = ((CDepart)cboxDepart.SelectedItem).DepartCode;
                        }
                        if (SalesPlanQuotaItemForGrid.Object_QuotaObjectType == enQuotaObjectType.Customer)
                        {
                            SalesPlanQuotaItemForGrid.Object_ID = ((CCustomer)cboxCustomer.SelectedItem).ID;
                            SalesPlanQuotaItemForGrid.Object_Name = ((CCustomer)cboxCustomer.SelectedItem).FullName;
                        }
                    }

                    SalesPlanQuotaItemForGrid.Object_Quota = calcObject_Quota.Value;

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
