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
    public partial class frmTradeMarkQuota : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private CProfile m_objProfile;
        private List<CProductTradeMark> m_objProductTradeMarkList;
        public List<CSalesPlanQuotaItemForGrid> DepartTeamList { get; set; }
        public List<CSalesPlanQuotaItemForGrid> m_SrcSalesPlanQuotaItemList { get; set; }
        public CProductTradeMark ProductTradeMark { get; set; }
        public System.Boolean UseEquableDistribution { get; set; }
        #endregion

        #region Конструктор
        public frmTradeMarkQuota(CProfile objProfile)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            AppDomain currentDomain = AppDomain.CurrentDomain;

            InitializeComponent();

            m_objProfile = objProfile;
            m_objProductTradeMarkList = null;
            ProductTradeMark = null;
            DepartTeamList = null;
            m_SrcSalesPlanQuotaItemList = null;
            UseEquableDistribution = false;
        }
        #endregion

        #region Загрузка содержимого выпадающих списков
        public void LoadComboBox( List<CProductTradeMark> objProductTradeMarkList)
        {
            try
            {
                m_objProductTradeMarkList = objProductTradeMarkList;

                cboxProductTradeMark.Properties.Items.Clear();
                cboxProductTradeMark.Properties.Items.AddRange(m_objProductTradeMarkList);

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

        #region Открытие формы
        /// <summary>
        /// Открытие формы
        /// </summary>
        public void OpenTradeMarkQuota(List<CSalesPlanQuotaItemForGrid> SrcSalesPlanQuotaItemList)
        {
            try
            {
                m_SrcSalesPlanQuotaItemList = SrcSalesPlanQuotaItemList;
                treeList.Nodes.Clear();
                DialogResult = System.Windows.Forms.DialogResult.None;
                DepartTeamList = null;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("OpenTradeMarkQuota.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Выбор товарной марки
        private void cboxProductTradeMark_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                treeList.Nodes.Clear();
                if (cboxProductTradeMark.SelectedItem != null)
                {
                    List<CDepartTeam> objDepartTeamList = new List<CDepartTeam>();
                    System.String strErr = System.String.Empty;
                    System.Int32 iRes = 0;
                    CProductTradeMark objProductTradeMark = (CProductTradeMark)cboxProductTradeMark.SelectedItem;

                    CSalesPlanQuota.GetDepartTeamListForProductOwner(m_objProfile, ref objDepartTeamList,
                        objProductTradeMark.ID, ref strErr, ref iRes);

                    if ((objDepartTeamList != null) && (objDepartTeamList.Count > 0))
                    {
                        CSalesPlanQuotaItemForGrid objSrcDepart = null;

                        foreach (CDepartTeam objDepartTeam in objDepartTeamList)
                        {
                            objSrcDepart = null;

                            if (m_SrcSalesPlanQuotaItemList != null)
                            {
                                objSrcDepart = m_SrcSalesPlanQuotaItemList.FirstOrDefault<CSalesPlanQuotaItemForGrid>(x => ((x.Object_ID.CompareTo(objDepartTeam.uuidID) == 0) && (x.ProductTradeMarkID.CompareTo(objProductTradeMark.ID) == 0)));
                            }
                            treeList.AppendNode(new object[] { objDepartTeam.DepartTeamName, 
                                ((objSrcDepart != null) ? ( objSrcDepart.Object_Quota * System.Convert.ToDecimal(100) ) : 0) }, null).Tag = objDepartTeam;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("cboxProductTradeMark_SelectedValueChanged.\n\nТекст ошибки: " + f.Message, "Ошибка",
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
                if (cboxProductTradeMark.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, товарную марку.", "Внимание!",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                if (treeList.Nodes.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Необходимо указать список команд.", "Внимание!",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                System.Decimal dcmlSum = System.Convert.ToDecimal(treeList.GetSummaryValue(colDepartTeamQuota));

                if (dcmlSum != 100)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Сумма долей команд в рамках марки должна равняться ста процентам.\nВнесите, пожалуйста, изменения и повторите попытку.", "Внимание!",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                // заполняем список долей продаж
                DepartTeamList = new List<CSalesPlanQuotaItemForGrid>();
                CSalesPlanQuotaItemForGrid objItem = null;
                CDepartTeam objDepartTeam = null;
                ProductTradeMark = (CProductTradeMark)cboxProductTradeMark.SelectedItem;
                UseEquableDistribution = checkEditEquable.Checked;

                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes )
                {
                    if (System.Convert.ToDecimal(objNode.GetValue(colDepartTeamQuota)) == 0) { continue; }

                    objDepartTeam = (CDepartTeam)objNode.Tag;

                    objItem = new CSalesPlanQuotaItemForGrid() 
                    { 
                        Object_CalcQuota = ( System.Convert.ToDecimal( objNode.GetValue( colDepartTeamQuota ) )/100 ),
                        Object_ID = objDepartTeam.uuidID,
                        Object_Quota = 0,
                        Object_Name = objDepartTeam.DepartTeamName,
                        Object_QuotaObjectType = enQuotaObjectType.DepartTeam,
                        ProductTradeMark = ProductTradeMark 
                    };

                    DepartTeamList.Add( objItem );
                }


                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();

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
