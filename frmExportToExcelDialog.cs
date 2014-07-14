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
    public partial class frmExportToExcelDialog : DevExpress.XtraEditors.XtraForm
    {
        private System.String m_strFileFullName;
        /// <summary>
        /// Файл шаблона
        /// </summary>
        public System.String FileFullName
        {
            get { return m_strFileFullName; }
        }
        private System.Int32 m_iSheetNumber;
        /// <summary>
        /// Номер листа шаблона
        /// </summary>
        public System.Int32 SheetNumber
        {
            get { return m_iSheetNumber; }
        }
        /// <summary>
        /// Курс ценообразования
        /// </summary>
        private System.Double m_dblCurrentPriceCreateRate;
        /// <summary>
        /// Курс ценообразования
        /// </summary>
        public System.Double CurrentPriceCreateRate
        {
            get { return m_dblCurrentPriceCreateRate; }
        }
        /// <summary>
        /// Список настроек
        /// </summary>
        private List<CSettingForCalcPrice> m_objSettingForCalcPriceList;

        private CSettingForCalcPrice m_objSelectedSettingForCalcPrice;
        public CSettingForCalcPrice SelectedSettingForCalcPrice
        {
            get { return m_objSelectedSettingForCalcPrice; }
        }
        private UniXP.Common.CProfile m_objProfile;
        private const System.String strNodeSettingname = "ColumnItem";


        public frmExportToExcelDialog( UniXP.Common.CProfile objProfile )
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objSettingForCalcPriceList = null;
            m_objSelectedSettingForCalcPrice = null;
            btnOk.Enabled = false;

            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                System.String strErr = "";
                m_objSettingForCalcPriceList = CSettingForCalcPrice.GetSettingForCalcPriceList(m_objProfile, null);
                treeListSettings.Nodes.Clear();

                calcSheetNum.Value = 1;
                txtID_Ib.Text = "";
                calcCurRatePrice.Value = System.Convert.ToDecimal(CProductSubType.GetCurrentPriceCreateRate(m_objProfile, ref strErr));

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

        private void LoadSettingsForSheet( System.Int32 iSheetNum )
        {
            
            try
            {
                treeListSettings.Nodes.Clear();
                m_objSelectedSettingForCalcPrice = null;

                if (iSheetNum <= 0) { return; }
                if (m_objSettingForCalcPriceList == null) { return; }
                if (m_objSettingForCalcPriceList.Count < 0) { return; }

                foreach (CSettingForCalcPrice objItem in m_objSettingForCalcPriceList)
                {
                    if (objItem.SheetID == iSheetNum)
                    {
                        m_objSelectedSettingForCalcPrice = objItem;
                        break;
                    }
                }

                if ((m_objSelectedSettingForCalcPrice != null) && (m_objSelectedSettingForCalcPrice.SettingsList != null))
                {
                    foreach (CSettingItemForCalcPrice objSetting in m_objSelectedSettingForCalcPrice.SettingsList)
                    {
                        treeListSettings.AppendNode(new object[] { objSetting.ParamName, System.String.Format("{0:### ### ##0}", objSetting.ColumnID) }, null).Tag = objSetting;
                    }
                }
                m_objSettingForCalcPriceList = CSettingForCalcPrice.GetSettingForCalcPriceList(m_objProfile, null);
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

        private void calcSheetNum_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (e.NewValue != null)
                {
                    if (System.Convert.ToDecimal(e.NewValue) < 1)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        btnOk.Enabled = true;
                    }
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "calcSheetNum_EditValueChanging.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void calcSheetNum_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadSettingsForSheet(System.Convert.ToInt32(calcSheetNum.Value));
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "calcSheetNum_EditValueChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void txtID_Ib_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ( txtID_Ib.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "txtID_Ib_EditValueChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }

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

        private System.Boolean SaveXMLSettingsForSheet(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                System.Xml.XmlNodeList nodeList = this.SelectedSettingForCalcPrice.XMLSettings.GetElementsByTagName(strNodeSettingname);
                if (nodeList != null)
                {
                    foreach (System.Xml.XmlNode xmlNode in nodeList)
                    {
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSettings.Nodes)
                        {
                            if (System.Convert.ToString(objNode.GetValue(colSettingsName)) == xmlNode.Attributes[0].Value)
                            {
                                xmlNode.Attributes[1].InnerText = System.Convert.ToString(objNode.GetValue(colSettingsColumnNum));
                            }
                        }
                    }
                    // теперь и в Базе данных
                    bRet = this.SelectedSettingForCalcPrice.SaveExportSettingForCalcPriceList(m_objProfile, null, ref strErr);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SaveXMLSettingsForSheet.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;

        }

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
                if (calcSheetNum.Value < 1)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Номер листа болжен быть больше единицы.", "Предупреждение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if ( calcCurRatePrice.Value < 1)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Проверьте, пожалуйста, курс ценообразования.", "Предупреждение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                m_iSheetNumber = System.Convert.ToInt32(calcSheetNum.Value);
                m_strFileFullName = txtID_Ib.Text;
                m_dblCurrentPriceCreateRate = System.Convert.ToDouble(calcCurRatePrice.Value);

                if ((m_objSelectedSettingForCalcPrice != null) && (m_objSelectedSettingForCalcPrice.SettingsList != null))
                {
                    CSettingItemForCalcPrice objSetting = null;
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSettings.Nodes)
                    {
                        if (objNode.Tag == null) { continue; }
                        objSetting = (CSettingItemForCalcPrice)objNode.Tag;
                        foreach (CSettingItemForCalcPrice objItem in m_objSelectedSettingForCalcPrice.SettingsList)
                        {
                            if (objSetting.ParamName == objItem.ParamName)
                            {
                                objItem.ColumnID = objSetting.ColumnID;
                                break;
                            }
                        }
                    }
                }

                System.String strErr = "";
                if( SaveXMLSettingsForSheet( ref strErr ) == false )
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Ошибка сохранения настроек в базе данных.\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }

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

        private void repItemCalc_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (e.NewValue != null)
                {
                    if (System.Convert.ToDecimal(e.NewValue) < 1)
                    {
                        e.Cancel = true;
                    }
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "repItemCalc_EditValueChanging.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                System.String strErr = "";
                if (SaveXMLSettingsForSheet(ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Ошибка сохранения настроек в базе данных.\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }
                else
                {
                    Close();
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnSaveSettings_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }



    }
}
