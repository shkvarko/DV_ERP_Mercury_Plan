using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ERPMercuryPlan
{
    public partial class frmCalcPlanKoefproperties : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private CCalcPlanKoef m_objSelectedCalcPlanKoef;
        private List<ERP_Mercury.Common.CProductOwner> m_objProductOwnerList;
        private List<ERP_Mercury.Common.CProductType> m_objProductTypeList;
        private List<ERP_Mercury.Common.CProductOwner> m_objCheckProductOwnerList;
        public List<ERP_Mercury.Common.CProductOwner> CheckProductOwnerList
        {
            get { return m_objCheckProductOwnerList; }
        }
        private List<ERP_Mercury.Common.CProductType> m_objCheckProductTypeList;
        public List<ERP_Mercury.Common.CProductType> CheckProductTypeList
        {
            get { return m_objCheckProductTypeList; }
        }
        public CCalcPlanKoef SelectedCalcPlanKoef
        {
            get { return m_objSelectedCalcPlanKoef; }
        }
        private const System.String strAllName = " Все";
        #endregion

        #region Конструктор
        public frmCalcPlanKoefproperties( UniXP.Common.CProfile objProfile )
        {
            m_objProfile = objProfile;
            m_objSelectedCalcPlanKoef = null;

            InitializeComponent();

            m_objProductOwnerList = ERP_Mercury.Common.CProductOwner.GetProductOwnerList(m_objProfile);
            m_objProductTypeList = ERP_Mercury.Common.CProductType.GetProductTypeList(m_objProfile, null);

            m_objCheckProductOwnerList = null;
            m_objCheckProductTypeList = null;

            LoadListsFoFilter();

            EnableFilter(false);

            txtName.Text = "";
            txtDscrpn.Text = "";
            dtBeginDate.DateTime = System.DateTime.Today;
            dtEndDate.DateTime = System.DateTime.Today;

        }
        #endregion

        #region Список для фильтра
        private void LoadListsFoFilter()
        {
            try
            {
                this.tableLayoutPanelFilter.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.lstPartType)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.lstOwner)).BeginInit();

                lstOwner.Items.Clear();
                lstPartType.Items.Clear();

                if ((m_objProductOwnerList != null) && (m_objProductOwnerList.Count > 0))
                {
                    lstOwner.Items.Add(new ERP_Mercury.Common.CProductOwner(System.Guid.Empty, strAllName, true), true);
                    foreach (ERP_Mercury.Common.CProductOwner objOwner in m_objProductOwnerList)
                    {
                        lstOwner.Items.Add(objOwner, true);
                    }
                }

                if ((m_objProductTypeList != null) && (m_objProductTypeList.Count > 0))
                {
                    lstPartType.Items.Add(new ERP_Mercury.Common.CProductType(System.Guid.Empty, strAllName, 0, "", 0, "", true), true);
                    foreach (ERP_Mercury.Common.CProductType objOwner in m_objProductTypeList)
                    {
                        lstPartType.Items.Add(objOwner, true);
                    }
                }

                //lstOwner.DataSource = m_objProductOwnerList;
                //lstPartType.DataSource = m_objProductTypeList;
 
                this.tableLayoutPanelFilter.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.lstPartType)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.lstOwner)).EndInit();
                tableLayoutPanelFilter.Refresh();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка обновления списка для фильтрации. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        private void EnableFilter(System.Boolean bEnable)
        {
            try
            {
                lstOwner.Enabled = bEnable;
                lstPartType.Enabled = bEnable;
                lstPartType.Appearance.BackColor = ((bEnable) ? Color.White : Color.LightGray);
                lstOwner.Appearance.BackColor = ((bEnable) ? Color.White : Color.LightGray);
                checkFilter.Font = (bEnable == true) ? new Font(checkFilter.Font, FontStyle.Bold) : new Font(checkFilter.Font, FontStyle.Regular);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка включения/выключения фиьлтра. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private void checkFilter_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                EnableFilter(checkFilter.Checked);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка checkFilter_EditValueChanged. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private void lstOwner_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (e.Index == 0)
                {
                    CheckState bCheck = e.State;
                    DevExpress.XtraEditors.CheckedListBoxControl objCheckList = (DevExpress.XtraEditors.CheckedListBoxControl)sender;
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in objCheckList.Items)
                    {
                        if (objListBoxItem.CheckState != bCheck)
                        {
                            objListBoxItem.CheckState = bCheck;
                        }

                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "lstOwner_ItemCheck. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        #endregion

        #region Свойства объекта
        /// <summary>
        /// Загружает в элементы управления свойства расчета
        /// </summary>
        /// <param name="objSelectedCalcPlanKoef">объект "расчет"</param>
        public void LoadCalcPlanKoefProperties(CCalcPlanKoef objSelectedCalcPlanKoef)
        {
            try
            {
                m_objSelectedCalcPlanKoef = objSelectedCalcPlanKoef;

                txtName.Text = "";
                txtDscrpn.Text = "";
                dtBeginDate.DateTime = System.DateTime.Today;
                dtEndDate.DateTime = System.DateTime.Today;

                if (m_objSelectedCalcPlanKoef != null)
                {
                    txtName.Text = m_objSelectedCalcPlanKoef.Num;
                    txtDscrpn.Text = m_objSelectedCalcPlanKoef.Description;
                    dtBeginDate.DateTime = m_objSelectedCalcPlanKoef.BeginDate;
                    dtEndDate.DateTime = m_objSelectedCalcPlanKoef.EndDate;
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка отображения свойств расчета. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Отменить выбор
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                m_objSelectedCalcPlanKoef = null;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnCancel_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Подтвердить выбор
        /// <summary>
        /// Подтвердить выбор
        /// </summary>
        private System.Boolean SaveChanges()
        {
            System.Boolean bRet = false;
            try
            {
                if (txtName.Text == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Укажате, пожалуйста, номер.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (dtEndDate.DateTime.CompareTo( dtBeginDate.DateTime ) < 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Дата окончания периода должна быть больше начала периода продаж.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                if (m_objSelectedCalcPlanKoef == null)
                {
                    m_objSelectedCalcPlanKoef = new CCalcPlanKoef();
                    m_objSelectedCalcPlanKoef.Date = System.DateTime.Today;
                    m_objSelectedCalcPlanKoef.BeginDate = dtBeginDate.DateTime;
                    m_objSelectedCalcPlanKoef.EndDate = dtEndDate.DateTime;
                    m_objSelectedCalcPlanKoef.Num = txtName.Text;
                    m_objSelectedCalcPlanKoef.Description = txtDscrpn.Text;
                }
                if (checkFilter.Checked == true)
                {
                    m_objCheckProductOwnerList = new List<ERP_Mercury.Common.CProductOwner>();
                    m_objCheckProductTypeList = new List<ERP_Mercury.Common.CProductType>();

                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in lstOwner.CheckedItems)
                    {
                        m_objCheckProductOwnerList.Add((ERP_Mercury.Common.CProductOwner)objListBoxItem.Value);
                    }

                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem objListBoxItem in lstPartType.CheckedItems)
                    {
                        m_objCheckProductTypeList.Add((ERP_Mercury.Common.CProductType)objListBoxItem.Value);
                    }
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SaveChanges. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return bRet;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges() == true)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnSave_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
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
