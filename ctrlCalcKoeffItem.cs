using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERPMercuryPlan
{
    public partial class ctrlCalcKoeffItem : UserControl
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private ERPMercuryPlan.CCalcPlanKoef m_objCalcPlanKoef;
        private System.Boolean m_bPlanIsChanged;
        public System.Boolean PlanIsChanged
        {
            get { return m_bPlanIsChanged; }
        }
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlProductList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        #endregion

        #region События
        // Создаем закрытое экземплярное поле для блокировки синхронизации потоков
        private readonly Object m_eventLock = new Object();
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeCalcOrderItemEventArgs> m_ChangeCalcOrderItem;
        // Создаем в классе член-событие
        public event EventHandler<ChangeCalcOrderItemEventArgs> ChangeCalcOrderItem
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                lock (m_eventLock) { m_ChangeCalcOrderItem += value; }
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                lock (m_eventLock) { m_ChangeCalcOrderItem -= value; }
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeCalcOrderItem(ChangeCalcOrderItemEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeCalcOrderItemEventArgs> temp = m_ChangeCalcOrderItem;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeCalcOrderItem(CCalcPlanKoef objCalcOrder, System.Boolean bIsChanged)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeCalcOrderItemEventArgs e = new ChangeCalcOrderItemEventArgs(objCalcOrder, bIsChanged);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeCalcOrderItem(e);
        }
        #endregion

        #region Конструктор
        public ctrlCalcKoeffItem(UniXP.Common.CProfile objProfile, ERPMercuryPlan.CCalcPlanKoef objCalcPlanKoef)
        {
            m_objProfile = objProfile;

            InitializeComponent();

            m_objCalcPlanKoef = objCalcPlanKoef;
            m_bPlanIsChanged = false;

            AddGridColumns();
            LoadCalcOrderItems();
        }

        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();
            AddGridColumn(ColumnView, "ProductOwnerIB_Id", "Код ТМ");
            AddGridColumn(ColumnView, "ProductTypeIB_Id", "Код товарной группы");
            AddGridColumn(ColumnView, "ProductOwner", "Код товарной подгруппы");
            AddGridColumn(ColumnView, "ProductOwnerName", "Товарная марка");
            AddGridColumn(ColumnView, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnView, "ProductSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnView, "QUANTITY", "Количество");
            AddGridColumn(ColumnView, "EW_PRICE", "Цена exw");
            AddGridColumn(ColumnView, "SHIP_EW_PRICE", "Сумма exw");
            AddGridColumn(ColumnView, "AVG_EW_PRICE", "Цена средняя exw");
            AddGridColumn(ColumnView, "PRICE02", "Цена опт");
            AddGridColumn(ColumnView, "SHIP_PRICE02", "Сумма опт");
            AddGridColumn(ColumnView, "PRICE", "Цена со скидкой");
            AddGridColumn(ColumnView, "SHIP_PRICE", "Сумма реализации");
            AddGridColumn(ColumnView, "AVG_PRICE", "Цена реализации средняя");
            AddGridColumn(ColumnView, "KOEFF", "Коэффициент");
            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
                if (objColumn.FieldName == "QUANTITY")
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "{0:### ### ##0.00}";
                }
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
        #endregion

        #region Загрузить приложение к расчету
        /// <summary>
        /// Загружает приложение к расчету
        /// </summary>
        public void LoadCalcOrderItems()
        {
            if (m_objCalcPlanKoef == null) { return; }
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();
                this.SuspendLayout();

                gridControlProductList.DataSource = null;

                System.String strErr = "";
                m_objCalcPlanKoef.CalcPlanKoefItemList = CCalcPlanKoefItem.GetCalcOrderItemList(m_objProfile, null, m_objCalcPlanKoef.ID, ref strErr);

                if (m_objCalcPlanKoef.CalcPlanKoefItemList != null)
                {
                    gridControlProductList.DataSource = m_objCalcPlanKoef.CalcPlanKoefItemList;

                }
                else
                {
                    if (strErr != "")
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Ошибка построения содержимого расчета коэффициентов. Текст ошибки: " + strErr, "Ошибка",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка построения содержимого расчета коэффициентов.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();
                this.ResumeLayout(false);
                SetPropertiesModified(false);
            }

            return;
        }
        #endregion

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            if (bModified == m_bPlanIsChanged) { return; }

            m_bPlanIsChanged = bModified;
            SimulateChangeCalcOrderItem(m_objCalcPlanKoef, m_bPlanIsChanged);
        }
        public void ResetPlanChanges()
        {
            m_bPlanIsChanged = false;
        }
        #endregion

        #region Закрытие закладки
        /// <summary>
        /// Закрывает закладку
        /// </summary>
        public void CloseStockOrderTypeParts()
        {
            try
            {
                if (m_bPlanIsChanged == true)
                {
                    DialogResult resDlg = DevExpress.XtraEditors.XtraMessageBox.Show("\nСохранить изменения?", "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                    switch (resDlg)
                    {
                        case DialogResult.Yes:
                            //SavePartsList();
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось корректно закрыть закладку.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Печать
        public void PrintCalcOrderItem()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ExportToExcel();
                Cursor.Current = Cursors.Default;

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка печати\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        /// <summary>
        /// Экспорт бюджета в MS Excel
        /// </summary>
        private void ExportToExcel()
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            System.Int32 iStartRow = 3;
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
                if (oWB.Worksheets.Count > 0)
                {
                    oSheet = (Excel._Worksheet)oWB.Worksheets[1];
                }
                else
                {
                    oWB.Worksheets.Add(m, m, m, Excel.XlSheetType.xlWorksheet);
                    oSheet = (Excel._Worksheet)oWB.Worksheets[1];
                }
                oSheet.Cells[1, 1] = m_objCalcPlanKoef.Num;
                oSheet.Cells[1, 2] = m_objCalcPlanKoef.ShipPeriod;

                oSheet.Cells[iStartRow, 1] = "Код ТМ";
                oSheet.Cells[iStartRow, 2] = "Код Тов. Группы";
                oSheet.Cells[iStartRow, 3] = "Код Тов. подгруппы";
                oSheet.Cells[iStartRow, 4] = "Товарная марка";
                oSheet.Cells[iStartRow, 5] = "Товарная группа";
                oSheet.Cells[iStartRow, 6] = "Товарная подгруппа";
                oSheet.Cells[iStartRow, 7] = "Количество";
                oSheet.Cells[iStartRow, 8] = "Цена exw";
                oSheet.Cells[iStartRow, 9] = "Сумма exw";
                oSheet.Cells[iStartRow, 10] = "Цена средняя exw";
                oSheet.Cells[iStartRow, 11] = "Цена опт";
                oSheet.Cells[iStartRow, 12] = "Сумма опт";
                oSheet.Cells[iStartRow, 13] = "Цена со скидкой";
                oSheet.Cells[iStartRow, 14] = "Сумма реализации";
                oSheet.Cells[iStartRow, 15] = "Цена реализации средняя";
                oSheet.Cells[iStartRow, 16] = "Коэффициент";

                iStartRow++;
                oSheet.get_Range("G1", "G1000").NumberFormat = "# ##0";
                oSheet.get_Range("H1", "H1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("I1", "I1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("J1", "J1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("K1", "KO1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("L1", "L1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("M1", "M1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("N1", "N1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("O1", "O1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("P1", "P1000").NumberFormat = "# ##0,000";
                oSheet.get_Range("Q1", "Q1000").NumberFormat = "# ##0,0000";

                foreach (CCalcPlanKoefItem objNode in m_objCalcPlanKoef.CalcPlanKoefItemList)
                {

                    oSheet.Cells[iStartRow, 1] = objNode.ProductOwnerIB_Id;
                    oSheet.Cells[iStartRow, 2] = objNode.ProductTypeIB_Id;
                    oSheet.Cells[iStartRow, 3] = objNode.ProductSubTypeIB_Id;
                    oSheet.Cells[iStartRow, 4] = objNode.ProductOwnerName;
                    oSheet.Cells[iStartRow, 5] = objNode.ProductTypeName;
                    oSheet.Cells[iStartRow, 6] = objNode.ProductSubTypeName;
                    oSheet.Cells[iStartRow, 7] = objNode.QUANTITY;
                    oSheet.Cells[iStartRow, 8] = objNode.EW_PRICE;
                    oSheet.Cells[iStartRow, 9] = objNode.SHIP_EW_PRICE;
                    oSheet.Cells[iStartRow, 10] = objNode.AVG_EW_PRICE;
                    oSheet.Cells[iStartRow, 11] = objNode.PRICE02;
                    oSheet.Cells[iStartRow, 12] = objNode.SHIP_PRICE02;
                    oSheet.Cells[iStartRow, 13] = objNode.PRICE;
                    oSheet.Cells[iStartRow, 14] = objNode.SHIP_PRICE;
                    oSheet.Cells[iStartRow, 15] = objNode.AVG_PRICE;
                    oSheet.Cells[iStartRow, 16] = objNode.KOEFF;

                    iStartRow++;
                }
                oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();
                oSheet.get_Range("Z1", "AZ1").ColumnWidth = 15;

                oSheet.get_Range("A3", "AZ3").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A3", "AZ3").WrapText = true;
                oSheet.get_Range("A3", "AZ3").ColumnWidth = 20;
                //oSheet.get_Range("X1", "Y1").EntireColumn.Hidden = true;

                ((Excel._Worksheet)oWB.Worksheets[1]).Activate();

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
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }
        #endregion
    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeCalcOrderItemEventArgs : EventArgs
    {
        private readonly System.Boolean m_bIsChanged;
        public System.Boolean IsChanged
        { get { return m_bIsChanged; } }

        private readonly CCalcPlanKoef m_objCalcOrder;
        public CCalcPlanKoef CalcOrder
        { get { return m_objCalcOrder; } }

        public ChangeCalcOrderItemEventArgs(CCalcPlanKoef objCalcOrder, System.Boolean bIsChanged)
        {
            m_objCalcOrder = objCalcOrder;
            m_bIsChanged = bIsChanged;
        }
    }

}
