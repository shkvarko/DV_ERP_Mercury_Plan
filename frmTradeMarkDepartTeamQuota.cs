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
    public partial class frmTradeMarkDepartTeamQuota : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private CProfile m_objProfile;
        private List<CProductTradeMark> m_objProductTradeMarkList;
        private List<CDepart> m_objDepartList;

        public List<CSalesPlanQuotaItemForGrid> SalesPlanQuotaItemList { get; set; }
        public List<CSalesPlanQuotaItemForGrid> m_SrcSalesPlanQuotaItemList { get; set; }
        public CProductTradeMark ProductTradeMark { get; set; }
        #endregion

        #region Конструктор
        public frmTradeMarkDepartTeamQuota(CProfile objProfile)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            AppDomain currentDomain = AppDomain.CurrentDomain;

            InitializeComponent();

            m_objProfile = objProfile;
            m_objProductTradeMarkList = null;
            m_objDepartList = null;

            ProductTradeMark = null;
            SalesPlanQuotaItemList = null;
            m_SrcSalesPlanQuotaItemList = null;
        }
        #endregion

        #region Загрузка содержимого выпадающих списков
        /// <summary>
        /// Заполняет выпадающие списки товарных марок и команд
        /// </summary>
        /// <param name="objProductTradeMarkList">список товарных марок</param>
        /// <param name="objDepartList">список торговых подразделений</param>
        public void LoadComboBox(List<CProductTradeMark> objProductTradeMarkList, List<CDepart> objDepartList)
        {
            try
            {
                m_objProductTradeMarkList = objProductTradeMarkList;
                m_objDepartList = objDepartList;

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
        public void OpenTradeMarkDepartTeamQuota(List<CSalesPlanQuotaItemForGrid> SrcSalesPlanQuotaItemList)
        {
            try
            {
                m_SrcSalesPlanQuotaItemList = SrcSalesPlanQuotaItemList;

                treeList.Nodes.Clear();
                cboxProductTradeMark.SelectedItem = null;
                checkEditEquable.Checked = false;

                DialogResult = System.Windows.Forms.DialogResult.None;
                SalesPlanQuotaItemList = null;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("OpenTradeMarkDepartTeamQuota.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Выбор товарной марки
        /// <summary>
        /// загружает в выпадающий список команды, работающие с заданной товарной маркой
        /// </summary>
        /// <param name="objProductTradeMark">товарная марка</param>
        private void LoadDepartTeamListForProductTradeMark(CProductTradeMark objProductTradeMark)
        {
            try
            {
                treeList.Nodes.Clear();

                if (objProductTradeMark == null) { return; }

                List<CDepartTeam> objDepartTeamList = new List<CDepartTeam>();
                List<CDepart> objDepartList = null;
                CSalesPlanQuotaItemForGrid objSrcDepart = null;
                System.Boolean bEqualQuota = checkEditEquable.Checked;

                System.String strErr = System.String.Empty;
                System.Int32 iRes = 0;

                CSalesPlanQuota.GetDepartTeamListForProductOwner(m_objProfile, ref objDepartTeamList,
                    objProductTradeMark.ID, ref strErr, ref iRes);

                if ((objDepartTeamList != null) && (objDepartTeamList.Count > 0))
                {
                    foreach (CDepartTeam objDepartTeam in objDepartTeamList)
                    {
                        objDepartList = m_objDepartList.Where<CDepart>(x => x.DepartTeam.uuidID.CompareTo(objDepartTeam.uuidID) == 0 && x.DepartIsActive == true).ToList<CDepart>();

                        if ((objDepartList != null) && (objDepartList.Count > 0))
                        {
                            foreach (CDepart objItem in objDepartList)
                            {
                                objSrcDepart = null;

                                if (m_SrcSalesPlanQuotaItemList != null)
                                {
                                    objSrcDepart = m_SrcSalesPlanQuotaItemList.FirstOrDefault<CSalesPlanQuotaItemForGrid>(x => ((x.Object_ID.CompareTo(objItem.uuidID) == 0) && (x.ProductTradeMarkID.CompareTo(objProductTradeMark.ID) == 0 )));
                                }

                                treeList.AppendNode(new object[] { objDepartTeam.DepartTeamName, objItem.DepartCode, 
                                    ((objSrcDepart != null) ? ( objSrcDepart.Object_Quota * System.Convert.ToDecimal(100) ) : 0) }, null).Tag = objItem;
                            }
                        }
                    }
                
                }

                if ((bEqualQuota == true) && (treeList.Nodes.Count > 0))
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        objNode.SetValue(colDepartQuota, System.Convert.ToDecimal(System.Convert.ToDecimal(100) / System.Convert.ToDecimal(treeList.Nodes.Count)));
                    }

                    System.Decimal dcmlSum = System.Convert.ToDecimal(treeList.GetSummaryValue(colDepartQuota));
                    if (dcmlSum != System.Convert.ToDecimal(100))
                    {
                        System.Decimal dcmlSumItem = System.Convert.ToDecimal(treeList.Nodes[0].GetValue(colDepartQuota));
                        dcmlSumItem = (dcmlSumItem - (dcmlSum - System.Convert.ToDecimal(100)));

                        treeList.Nodes[0].SetValue(colDepartQuota, dcmlSumItem);
                    }
                }


            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadDepartTeamListForProductTradeMark.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void cboxProductTradeMark_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDepartTeamListForProductTradeMark((cboxProductTradeMark.SelectedItem != null) ? (CProductTradeMark)cboxProductTradeMark.SelectedItem : null);
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

        private void checkEditEquable_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDepartTeamListForProductTradeMark((cboxProductTradeMark.SelectedItem != null) ? (CProductTradeMark)cboxProductTradeMark.SelectedItem : null);
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

        private void cboxDepartTeam_SelectedValueChanged(object sender, EventArgs e)
        {
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Необходимо указать список активных подразделений.", "Внимание!",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                System.Decimal dcmlSum = System.Convert.ToDecimal(treeList.GetSummaryValue(colDepartQuota));

                if (dcmlSum != 100)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Сумма долей подразделений в рамках марки-команды должна равняться 100%.\nВнесите, пожалуйста, изменения и повторите попытку.", "Внимание!",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                // заполняем список долей продаж
                SalesPlanQuotaItemList = new List<CSalesPlanQuotaItemForGrid>();
                CSalesPlanQuotaItemForGrid objItem = null;
                CDepart objDepart = null;

                ProductTradeMark = (CProductTradeMark)cboxProductTradeMark.SelectedItem;

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (System.Convert.ToDecimal(objNode.GetValue(colDepartQuota)) == 0) { continue; }

                    objDepart = (CDepart)objNode.Tag;

                    objItem = new CSalesPlanQuotaItemForGrid()
                    {
                        Object_CalcQuota = (System.Convert.ToDecimal(objNode.GetValue(colDepartQuota)) / 100),
                        Object_ID = objDepart.uuidID,
                        Object_Quota = 0,
                        Object_Name = objDepart.DepartCode,
                        Object_QuotaObjectType = enQuotaObjectType.Depart,
                        ProductTradeMark = ProductTradeMark 
                    };

                    SalesPlanQuotaItemList.Add(objItem);
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
