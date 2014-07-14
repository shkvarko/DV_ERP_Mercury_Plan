using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP_Mercury.Common;
using System.ComponentModel;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERPMercuryPlan
{
    /// <summary>
    /// Класс "Цена"
    /// </summary>
    public class CPrice
    {
        #region Свойства
        /// <summary>
        /// Тип цены
        /// </summary>
        private CPriceType m_objPriceType;
        /// <summary>
        /// Тип цены
        /// </summary>
        public CPriceType PriceType
        {
            get { return m_objPriceType; }
            set { m_objPriceType = value; }
        }
        /// <summary>
        /// Значение
        /// </summary>
        private System.Double m_dblPriceValue;
        /// <summary>
        /// Значение
        /// </summary>
        public System.Double PriceValue
        {
            get { return m_dblPriceValue; }
            set { m_dblPriceValue = value; }
        }

        #endregion

        #region Конструктор
        public CPrice()
        {
            m_objPriceType = null;
            m_dblPriceValue = 0;
        }
        public CPrice( CPriceType objPriceType, System.Double dblValue )
        {
            m_objPriceType = objPriceType;
            m_dblPriceValue = dblValue;
        }
        #endregion

        public override string ToString()
        {
            return m_objPriceType.Name;
        }
    }
    /// <summary>
    /// Класс "Строка расчета для прайс-листа"
    /// </summary>
    public class CPriceListCalcItem
    {
        #region Свойства
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        private CProductSubType m_objProductSubType;
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        public CProductSubType objProductSubType
        {
            get { return m_objProductSubType; }
            set { m_objProductSubType = value; }
        }
        /// <summary>
        /// Затраты на транспорт
        /// </summary>
        private System.Double m_dblTransportCost;
        /// <summary>
        /// Затраты на транспорт
        /// </summary>
        public System.Double TransportCost
        {
            get { return m_dblTransportCost; }
            set { m_dblTransportCost = value; }
        }
        /// <summary>
        /// Затраты на таможню
        /// </summary>
        private System.Double m_dblCustomCost;
        /// <summary>
        /// Затраты на таможню
        /// </summary>
        public System.Double CustomCost
        {
            get { return m_dblCustomCost; }
            set { m_dblCustomCost = value; }
        }
        /// <summary>
        /// Сумма с НДС
        /// </summary>
        private System.Double m_dblNDSCost;
        /// <summary>
        /// Сумма с НДС
        /// </summary>
        public System.Double NDSCost
        {
            get { return m_dblNDSCost; }
            set { m_dblNDSCost = value; }
        }
        /// <summary>
        /// Курс ценообразования
        /// </summary>
        private System.Double m_dblPriceCurrencyRate;
        /// <summary>
        /// Курс ценообразования
        /// </summary>
        public System.Double PriceCurrencyRate
        {
            get { return m_dblPriceCurrencyRate; }
            set { m_dblPriceCurrencyRate = value; }
        }
        /// <summary>
        /// Список цен
        /// </summary>
        private List<CPrice> m_objPriceList;
        /// <summary>
        /// Список цен
        /// </summary>
        public List<CPrice> PriceList
        {
            get { return m_objPriceList; }
            set { m_objPriceList = value; }
        }
        /// <summary>
        /// Возвращает значение цены по заданному типу цены
        /// </summary>
        /// <param name="objPriceType">тип цены</param>
        /// <returns>значение цены</returns>
        public System.Double GetPriceValueByType(CPriceType objPriceType)
        {
            System.Double dblRet = 0;
            try
            {
                if ((PriceList != null) && (PriceList.Count > 0))
                {
                    foreach (CPrice objPrice in PriceList)
                    {
                        if (objPrice.PriceType.ID.CompareTo(objPriceType.ID) == 0)
                        {
                            dblRet = objPrice.PriceValue;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "PriceValueByType. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return dblRet;

        }

        public System.Int32 PartSubTypeID_Ib
        {
            get { return ((m_objProductSubType == null) ? 0 : m_objProductSubType.ID_Ib); }
        }
        public System.String PartSubTypeName
        {
            get { return ((m_objProductSubType == null) ? "" : m_objProductSubType.Name); }
        }
        public System.Double VendorTariff
        {
            get { return ((m_objProductSubType == null) ? 0 : m_objProductSubType.VendorTariff); }
        }
        public System.Double TransportTariff
        {
            get { return ((m_objProductSubType == null) ? 0 : m_objProductSubType.TransportTariff); }
        }
        public System.Double CustomsTariff
        {
            get { return ((m_objProductSubType == null) ? 0 : m_objProductSubType.CustomsTariff); }
        }
        public System.Double Margin
        {
            get { return ((m_objProductSubType == null) ? 0 : m_objProductSubType.Margin); }
        }
        public System.Double NDS
        {
            get { return ((m_objProductSubType == null) ? 0 : m_objProductSubType.NDS); }
        }
        public System.Double Discont
        {
            get { return ((m_objProductSubType == null) ? 0 : m_objProductSubType.Discont); }
        }
        public System.Double MarkUpRequired
        {
            get { return ((m_objProductSubType == null) ? 0 : m_objProductSubType.MarkUpRequired); }
        }
        public System.String ProductOwner
        {
            get { return ((m_objProductSubType == null) ? "" : m_objProductSubType.ProductOwner); }
        }
        public System.String ProductType
        {
            get { return ((m_objProductSubType == null) ? "" : m_objProductSubType.ProductType); }
        }
        public System.String ProductLineName
        {
            get { return ((m_objProductSubType == null) ? "" : m_objProductSubType.ProductLineName); }
        }
        public System.String SubTypeStateName
        {
            get { return ((m_objProductSubType == null) ? "" : m_objProductSubType.SubTypeStateName); }
        }

        #endregion

        #region Конструктор
        public CPriceListCalcItem()
        {
            m_objProductSubType = null;
            m_dblNDSCost = 0;
            m_dblCustomCost = 0;
            m_dblTransportCost = 0;
            m_objPriceList = null;
            m_dblPriceCurrencyRate = 0;
        }
        public CPriceListCalcItem(CProductSubType objProductSubType, System.Double dblNDSCost, System.Double dblCustomCost,
            System.Double dblTransportCost, System.Double dblPriceCurrencyRate)
        {
            m_objProductSubType = objProductSubType;
            m_dblNDSCost = dblNDSCost;
            m_dblCustomCost = dblCustomCost;
            m_dblTransportCost = dblTransportCost;
            m_objPriceList = null;
            m_dblPriceCurrencyRate = dblPriceCurrencyRate;
        }
        #endregion
    }

    /// <summary>
    /// Класс "Расчет цен для прайс-листа"
    /// </summary>
    public class CPriceListCalc
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID
        {
            get { return m_uidID; }
            set { m_uidID = value; }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// Наименование
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescrpn;
        /// <summary>
        /// Примечание
        /// </summary>
        public System.String Description
        {
            get { return m_strDescrpn; }
            set { m_strDescrpn = value; }
        }
        /// <summary>
        /// Дата
        /// </summary>
        private System.DateTime m_dtDocDate;
        /// <summary>
        /// Дата
        /// </summary>
        public System.DateTime DocDate
        {
            get { return m_dtDocDate; }
            set { m_dtDocDate = value; }
        }
        /// <summary>
        /// Признак "документ активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "документ активен"
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        /// <summary>
        /// Приложение к расчету
        /// </summary>
        private List<CPriceListCalcItem> m_objCalcItemList;
        /// <summary>
        /// Приложение к расчету
        /// </summary>
        public List<CPriceListCalcItem> CalcItemList
        {
            get { return m_objCalcItemList; }
            set { m_objCalcItemList = value; }
        }
        /// <summary>
        /// Файл расчета
        /// </summary>
        private System.String m_strFileNameXLS;
        /// <summary>
        /// Файл расчета
        /// </summary>
        public System.String FileNameXLS
        {
            get { return m_strFileNameXLS; }
            set { m_strFileNameXLS = value; }
        }
        #endregion

        #region Конструктор
        public CPriceListCalc()
        {
            m_uidID = System.Guid.Empty;
            m_strName = "";
            m_strDescrpn = "";
            m_bIsActive = false;
            m_dtDocDate = System.DateTime.MinValue;
            m_objCalcItemList = null;
            m_strFileNameXLS = "";
        }
        public CPriceListCalc(System.Guid uidID, System.String strName, System.String strDescrpn, System.DateTime dtDocDate,
            System.Boolean bIsActive, System.String strFileNameXLS )
        {
            m_uidID = uidID;
            m_strName = strName;
            m_strDescrpn = strDescrpn;
            m_bIsActive = bIsActive;
            m_dtDocDate = dtDocDate;
            m_objCalcItemList = null;
            m_strFileNameXLS = strFileNameXLS;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список расчетов цен
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список расчетов цен</returns>
        public static List<CPriceListCalc> GetPriceListCalcList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CPriceListCalc> objList = new List<CPriceListCalc>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objList;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPriceListCalc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CPriceListCalc(
                          (System.Guid)rs["PriceListCalc_Guid"],
                          (System.String)rs["PriceListCalc_Name"],
                          ((rs["PriceListCalc_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PriceListCalc_Description"]),
                          System.Convert.ToDateTime(rs["PriceListCalc_Date"]),
                          System.Convert.ToBoolean(rs["PriceListCalc_IsActive"]),
                          ((rs["PriceListCalc_XLSFileName"] == System.DBNull.Value) ? "" : (System.String)rs["PriceListCalc_XLSFileName"])
                          ));
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список расчетов цен.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает приложение к расчету
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadCalcItemList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPriceListCalcItems]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = this.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Int32 iPartSubTypeId = 0;
                    CProductSubType objProductSubType = null;
                    CPriceType objPriceType = null;
                    CPriceListCalcItem objPriceListCalcItem = null;
                    CPrice objPrice = null;
                    this.m_objCalcItemList = new List<CPriceListCalcItem>();
                    while (rs.Read())
                    {
                        objPriceType = new CPriceType(
                            (System.Guid)rs["PartsubtypePriceType_Guid"],
                            (System.String)rs["PartsubtypePriceType_Name"],
                            (System.String)rs["PartsubtypePriceType_Abbr"],
                            ((rs["PartsubtypePriceType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsubtypePriceType_Description"]),
                            System.Convert.ToBoolean(rs["PartsubtypePriceType_IsActive"]),
                            new CCurrency(
                            (System.Guid)rs["Currency_Guid"],
                            (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"],
                            (System.String)rs["Currency_Code"]
                            ),
                            System.Convert.ToInt32(rs["PartsubtypePriceType_ColumnIdDefault"]),
                            System.Convert.ToBoolean(rs["PartsubtypePriceType_ShowInPriceList"])
                            );

                        objPrice = new CPrice(objPriceType, System.Convert.ToDouble(rs["Price_Value"]));

                        if (System.Convert.ToInt32(rs["Partsubtype_Id"]) != iPartSubTypeId)
                        {
                            // новая подгруппа, и как следствие новая запись в приложении к расчету
                            objProductSubType = new CProductSubType();
                            objProductSubType.ID = (System.Guid)rs["Partsubtype_Guid"];
                            objProductSubType.Name = (System.String)rs["Partsubtype_Name"];
                            objProductSubType.ID_Ib = System.Convert.ToInt32(rs["Partsubtype_Id"]);
                            objProductSubType.VendorTariff =(  ( rs["Partsubtype_VendorTariff"] == System.DBNull.Value ) ? 0 :  System.Convert.ToDouble(rs["Partsubtype_VendorTariff"]) ); 
                            objProductSubType.CustomsTariff = (  ( rs["Partsubtype_CustomsTariff"] == System.DBNull.Value ) ? 0 :  System.Convert.ToDouble(rs["Partsubtype_CustomsTariff"]));
                            objProductSubType.TransportTariff = (  ( rs["Partsubtype_TransportTariff"] == System.DBNull.Value ) ? 0 :  System.Convert.ToDouble(rs["Partsubtype_TransportTariff"]));
                            objProductSubType.Discont = (  ( rs["Partsubtype_Discont"] == System.DBNull.Value ) ? 0 :  System.Convert.ToDouble(rs["Partsubtype_Discont"]));
                            objProductSubType.Margin = (  ( rs["Partsubtype_Margin"] == System.DBNull.Value ) ? 0 :  System.Convert.ToDouble(rs["Partsubtype_Margin"]));
                            objProductSubType.NDS = (  ( rs["Partsubtype_NDS"] == System.DBNull.Value ) ? 0 :  System.Convert.ToDouble(rs["Partsubtype_NDS"]));
                            objProductSubType.ProductOwner = ( ( rs["Owner_Name"] == System.DBNull.Value ) ? "" : System.Convert.ToString(rs["Owner_Name"]) );
                            objProductSubType.MarkUpRequired = ((rs["Partsubtype_MarkUpRequired"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Partsubtype_MarkUpRequired"]));

                            objPriceListCalcItem = new CPriceListCalcItem(objProductSubType, System.Convert.ToDouble(rs["Partsubtype_NDSSum"]), System.Convert.ToDouble(rs["Partsubtype_CustomsSum"]), System.Convert.ToDouble(rs["Partsubtype_TransportSum"]), System.Convert.ToDouble(rs["Partsubtype_CurRatePricing"]));

                            objPriceListCalcItem.PriceList = new List<CPrice>();

                            this.m_objCalcItemList.Add(objPriceListCalcItem);

                            iPartSubTypeId = objProductSubType.ID_Ib;
                        }

                        if (objPriceListCalcItem != null)
                        { objPriceListCalcItem.PriceList.Add(objPrice); }
                    }
                }
                bRet = true;
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список расчетов цен.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }

        /// <summary>
        /// Загружает файл из БД на диск
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strDirectory">каталог на диске</param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public System.Boolean LoadReportFileFromDBOnDisk( UniXP.Common.CProfile objProfile, System.String strDirectory, ref System.String strErr )
        {
            System.Boolean bRet = false;
            //Создаем соединение с базой данных
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не удалось получить соединение с БД.", "Внимание",
                 System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return bRet;
            }
            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPriceListCalcXLSData]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = this.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    // набор данных непустой
                    rs.Read();

                    // проверим наличие папки, в которую запишем отчет
                    if (System.IO.Directory.Exists(strDirectory) == false)
                    {
                        strDirectory = (Environment.GetEnvironmentVariable("TMP") + @"\" + this.ID.ToString());
                        System.IO.Directory.CreateDirectory( strDirectory );
                    }

                    System.String strXlsFileName = (System.String)rs["PriceListCalc_XLSFileName"];
                    System.String strFileFullName = strDirectory + @"\" + strXlsFileName;
                    if (System.IO.File.Exists(strFileFullName) == true)
                    {
                        System.IO.File.Delete(strFileFullName);
                    }
                    object oMissing = System.Reflection.Missing.Value;

                    if (rs["PriceListCalc_XLSData"] == System.DBNull.Value)
                    {
                        // для данного отчета не сформировано представление
                        Excel.ApplicationClass oExcel = new Excel.ApplicationClass();
                        Excel._Workbook oBook = oExcel.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                        oBook.SaveAs(strFileFullName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                            null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                            null, null, null, null, null);

                        // Quit Excel and clean up.
                        oBook.Close(false, oMissing, oMissing);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oBook);
                        oBook = null;
                        oExcel.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject( oExcel );
                        oExcel = null;
                    }
                    else
                    {
                        byte[] arAttach = (byte[])rs["PriceListCalc_XLSData"];
                        //m_objReport.XlsShema = arAttach;

                        int lung = Convert.ToInt32(arAttach.Length);
                        // создаем файловый поток
                        System.IO.FileStream fs = new System.IO.FileStream( strFileFullName, System.IO.FileMode.Create );
                        // Считываем содержимое вложения в файл
                        fs.Write(arAttach, 0, lung);
                        fs.Close();
                        fs.Dispose();
                        fs = null;
                        arAttach = null;

                        Excel.ApplicationClass oXL = new Excel.ApplicationClass();
                        Excel._Workbook oBook = (Excel._Workbook)(oXL.Workbooks.Open(strFileFullName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));

                        oXL.Visible = true;

                        oBook = null;
                        oXL = null;
                    }
                }
                rs.Close();
                rs.Dispose();
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось загрузить файл из БД на диск.\n\nТекст ошибки: " + f.Message;
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Возвращает содержимое файла расчёта из базы данных в виде массива байтов
        /// </summary>
        /// <param name="uuidID">УИ расчёта</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>массив байтов</returns>
        public static byte[] GetReportFile( System.Guid uuidID, UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            byte[] arAttach = null;
            //Создаем соединение с базой данных
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не удалось получить соединение с БД.", "Внимание",
                 System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return arAttach;
            }
            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPriceListCalcXLSData]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = uuidID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    // набор данных непустой
                    rs.Read();
                    arAttach = (byte[])rs["PriceListCalc_XLSData"];
                }
                rs.Close();
                rs.Dispose();
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось загрузить содержимое файла из БД.\n\nТекст ошибки: " + f.Message;
            }
            finally
            {
                DBConnection.Close();
            }
            return arAttach;
        }

        #endregion

        #region IsAllParametersValid
        /// <summary>
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать название!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка проверки свойств.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        #region Add
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, System.Boolean bNeedSaveFile )
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPriceListCalc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Name"].Value = this.Name;
                cmd.Parameters["@PriceListCalc_IsActive"].Value = this.IsActive;
                cmd.Parameters["@PriceListCalc_Description"].Value = this.Description;
                cmd.Parameters["@PriceListCalc_Date"].Value = this.DocDate;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@PriceListCalc_Guid"].Value;
                    if (bNeedSaveFile == true)
                    {
                        iRes = ((this.UploadFileXlsToDatabase(this.FileNameXLS, objProfile, cmd, ref strErr) == true) ? 0 : -1);
                    }
                    if (iRes == 0)
                    {
                        iRes = ((this.SaveCalcItemList( objProfile, cmd, ref strErr) == true) ? 0 : -1);
                        if (iRes == 0)
                        {
                            // подтверждаем транзакцию
                            DBTransaction.Commit();
                        }
                        else
                        {
                            DBTransaction.Rollback();
                            DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания расчета цен.\n\nТекст ошибки: " + strErr, "Ошибка",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        DBTransaction.Rollback();
                        DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания расчета цен.\n\nТекст ошибки: " + strErr, "Ошибка",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания расчета цен.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать описание расчета цен.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        /// <summary>
        /// сохраняет файл отчета в БД
        /// </summary>
        /// <param name="strCopyFileName">путь к сохраняемому файлу</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - успешное завершение; false - ошибка</returns>
        public System.Boolean UploadFileXlsToDatabase(System.String strCopyFileName, 
                UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr )
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;

            try
            {
                
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPriceListCalcXLSData]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = this.ID;
                if (strCopyFileName != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_XLSData", System.Data.SqlDbType.Image));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_XLSFileName", System.Data.DbType.String));

                    //System.String strCopyFileName = m_strDirectory + "\\tmp_" + Report_axPivotTable.DocumentName;
                    //System.IO.File.Copy(Report_axPivotTable.DocumentFullName, strCopyFileName, true);
                    System.IO.FileStream fs = new System.IO.FileStream(strCopyFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    System.IO.FileInfo fi = new System.IO.FileInfo(strCopyFileName);
                    System.String strShortName = fi.Name;
                    System.String strExtension = fi.Extension;
                    int lung = Convert.ToInt32(fi.Length);
                    // Считываем содержимое файла в массив байт.
                    byte[] arAttach = new byte[lung];
                    fs.Read(arAttach, 0, lung);
                    fs.Close();
                    fs = null;
                    //if (System.IO.File.Exists(strCopyFileName) == true)
                    //{ System.IO.File.Delete(strCopyFileName); }

                    cmd.Parameters["@PriceListCalc_XLSFileName"].Value = fi.Name;
                    cmd.Parameters["@PriceListCalc_XLSData"].Value = arAttach;

                    fi = null;
                }

                cmd.ExecuteNonQuery();

                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = "Не удалось сохранить файл в БД. Текст ошибки: " + f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }

            return bRet;
        }

        #endregion

        #region Сохранение содержимого расчета в БД
        /// <summary>
        /// Сохраняет в БД содержимого расчета цен
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        private System.Boolean SaveCalcItemList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItems = new System.Data.DataTable();
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_VendorTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_TransportTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_TransportSum", typeof(System.Data.SqlTypes.SqlDecimal)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CustomsTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CustomsSum", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Margin", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_NDS", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_NDSSum", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Discont", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CurRatePricing", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_MarkUpRequired", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItems = null;
                System.Data.DataRow newRowCalcItemsPrice = null;
                if ((m_objCalcItemList != null) && (m_objCalcItemList.Count > 0))
                {
                    foreach (CPriceListCalcItem objItem in m_objCalcItemList)
                    {
                        newRowCalcItems = tPriceListCalcItems.NewRow();
                        newRowCalcItems["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                        newRowCalcItems["Partsubtype_VendorTariff"] = System.Convert.ToDecimal( objItem.VendorTariff );
                        newRowCalcItems["Partsubtype_TransportTariff"] = System.Convert.ToDecimal( objItem.TransportTariff );
                        newRowCalcItems["Partsubtype_TransportSum"] = System.Convert.ToDecimal( objItem.TransportCost );
                        newRowCalcItems["Partsubtype_CustomsTariff"] = System.Convert.ToDecimal( objItem.CustomsTariff );
                        newRowCalcItems["Partsubtype_CustomsSum"] = System.Convert.ToDecimal( objItem.CustomCost );
                        newRowCalcItems["Partsubtype_Margin"] = System.Convert.ToDecimal( objItem.Margin );
                        newRowCalcItems["Partsubtype_NDS"] = System.Convert.ToDecimal( objItem.NDS );
                        newRowCalcItems["Partsubtype_NDSSum"] = System.Convert.ToDecimal( objItem.NDSCost );
                        newRowCalcItems["Partsubtype_Discont"] = System.Convert.ToDecimal( objItem.Discont );
                        newRowCalcItems["Partsubtype_CurRatePricing"] = System.Convert.ToDecimal(objItem.PriceCurrencyRate);
                        newRowCalcItems["Partsubtype_MarkUpRequired"] = System.Convert.ToDecimal(objItem.MarkUpRequired);
                        tPriceListCalcItems.Rows.Add(newRowCalcItems);

                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                                newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal( objPrice.PriceValue );
                                tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                            }
                        }

                    }
                }
                tPriceListCalcItems.AcceptChanges();
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignPartsubtypeWithPriceInCalc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItems", tPriceListCalcItems);
                cmd.Parameters["@tPriceListCalcItems"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItems"].TypeName = "dbo.udt_PriceListCalcItems";
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        /// <summary>
        /// Сохраняет изменения в расчёте цен
        /// </summary>
        /// <param name="PriceListCalc_Guid">уи расчёта цен</param>
        /// <param name="objCalcItemList">список строк расчёта</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddPriceListCalcItem(System.Guid PriceListCalc_Guid, List<CPriceListCalcItem> objCalcItemList, 
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItems = new System.Data.DataTable();
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_VendorTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_TransportTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_TransportSum", typeof(System.Data.SqlTypes.SqlDecimal)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CustomsTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CustomsSum", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Margin", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_NDS", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_NDSSum", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Discont", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CurRatePricing", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_MarkUpRequired", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItems = null;
                System.Data.DataRow newRowCalcItemsPrice = null;
                if ((objCalcItemList != null) && (objCalcItemList.Count > 0))
                {
                    foreach (CPriceListCalcItem objItem in objCalcItemList)
                    {
                        newRowCalcItems = tPriceListCalcItems.NewRow();
                        newRowCalcItems["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                        newRowCalcItems["Partsubtype_VendorTariff"] = System.Convert.ToDecimal(objItem.VendorTariff);
                        newRowCalcItems["Partsubtype_TransportTariff"] = System.Convert.ToDecimal(objItem.TransportTariff);
                        newRowCalcItems["Partsubtype_TransportSum"] = System.Convert.ToDecimal(objItem.TransportCost);
                        newRowCalcItems["Partsubtype_CustomsTariff"] = System.Convert.ToDecimal(objItem.CustomsTariff);
                        newRowCalcItems["Partsubtype_CustomsSum"] = System.Convert.ToDecimal(objItem.CustomCost);
                        newRowCalcItems["Partsubtype_Margin"] = System.Convert.ToDecimal(objItem.Margin);
                        newRowCalcItems["Partsubtype_NDS"] = System.Convert.ToDecimal(objItem.NDS);
                        newRowCalcItems["Partsubtype_NDSSum"] = System.Convert.ToDecimal(objItem.NDSCost);
                        newRowCalcItems["Partsubtype_Discont"] = System.Convert.ToDecimal(objItem.Discont);
                        newRowCalcItems["Partsubtype_CurRatePricing"] = System.Convert.ToDecimal(objItem.PriceCurrencyRate);
                        newRowCalcItems["Partsubtype_MarkUpRequired"] = System.Convert.ToDecimal(objItem.MarkUpRequired);
                        tPriceListCalcItems.Rows.Add(newRowCalcItems);

                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                                newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                            }
                        }
                    }
                }
                tPriceListCalcItems.AcceptChanges();
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPriceListCalcItem]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItems", tPriceListCalcItems);
                cmd.Parameters["@tPriceListCalcItems"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItems"].TypeName = "dbo.udt_PriceListCalcItems";
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = PriceListCalc_Guid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        /// <summary>
        /// Добавляет запись в расчёт цен
        /// </summary>
        /// <param name="PriceListCalc_Guid">УИ расчёта цен</param>
        /// <param name="objItem">Строка с расчётом цен</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddPriceListCalcItem( System.Guid PriceListCalc_Guid, CPriceListCalcItem objItem, 
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItems = new System.Data.DataTable();
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_VendorTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_TransportTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_TransportSum", typeof(System.Data.SqlTypes.SqlDecimal)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CustomsTariff", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CustomsSum", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Margin", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_NDS", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_NDSSum", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_Discont", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_CurRatePricing", typeof(System.Data.SqlTypes.SqlMoney)));
                tPriceListCalcItems.Columns.Add(new System.Data.DataColumn("Partsubtype_MarkUpRequired", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItems = null;
                System.Data.DataRow newRowCalcItemsPrice = null;

                newRowCalcItems = tPriceListCalcItems.NewRow();
                newRowCalcItems["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                newRowCalcItems["Partsubtype_VendorTariff"] = System.Convert.ToDecimal(objItem.VendorTariff);
                newRowCalcItems["Partsubtype_TransportTariff"] = System.Convert.ToDecimal(objItem.TransportTariff);
                newRowCalcItems["Partsubtype_TransportSum"] = System.Convert.ToDecimal(objItem.TransportCost);
                newRowCalcItems["Partsubtype_CustomsTariff"] = System.Convert.ToDecimal(objItem.CustomsTariff);
                newRowCalcItems["Partsubtype_CustomsSum"] = System.Convert.ToDecimal(objItem.CustomCost);
                newRowCalcItems["Partsubtype_Margin"] = System.Convert.ToDecimal(objItem.Margin);
                newRowCalcItems["Partsubtype_NDS"] = System.Convert.ToDecimal(objItem.NDS);
                newRowCalcItems["Partsubtype_NDSSum"] = System.Convert.ToDecimal(objItem.NDSCost);
                newRowCalcItems["Partsubtype_Discont"] = System.Convert.ToDecimal(objItem.Discont);
                newRowCalcItems["Partsubtype_CurRatePricing"] = System.Convert.ToDecimal(objItem.PriceCurrencyRate);
                newRowCalcItems["Partsubtype_MarkUpRequired"] = System.Convert.ToDecimal(objItem.MarkUpRequired);
                tPriceListCalcItems.Rows.Add(newRowCalcItems);

                if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                {
                    foreach (CPrice objPrice in objItem.PriceList)
                    {
                        newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                        newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                        newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                        newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                        tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                    }
                }

                tPriceListCalcItems.AcceptChanges();
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPriceListCalcItem]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItems", tPriceListCalcItems);
                cmd.Parameters["@tPriceListCalcItems"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItems"].TypeName = "dbo.udt_PriceListCalcItems";
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = PriceListCalc_Guid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region Update
        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(UniXP.Common.CProfile objProfile, System.Boolean bNeedSaveFile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPriceListCalc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = this.ID;
                cmd.Parameters["@PriceListCalc_Name"].Value = this.Name;
                cmd.Parameters["@PriceListCalc_IsActive"].Value = this.IsActive;
                cmd.Parameters["@PriceListCalc_Description"].Value = this.Description;
                cmd.Parameters["@PriceListCalc_Date"].Value = this.DocDate;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                bRet = (iRes == 0);

                if (bRet == true)
                {
                    if (bNeedSaveFile == true)
                    {
                        bRet = this.UploadFileXlsToDatabase(this.FileNameXLS, objProfile, cmd, ref strErr);
                        if (bRet == true)
                        {
                            bRet = this.SaveCalcItemList(objProfile, cmd, ref strErr);
                        }
                    }
                    if (bRet == true)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        DBTransaction.Rollback();
                        DevExpress.XtraEditors.XtraMessageBox.Show("Ошибкаи зменения описания расчета цен.\n\nТекст ошибки: " + strErr, "Ошибка",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения описания расчета цен.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось измененить описание расчета цен.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePriceListCalc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceListCalc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceListCalc_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                bRet = (iRes == 0);


                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления расчета цен.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить расчет цен.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

    }

    public class CSettingItemForCalcPrice
    {
        #region Свойства
        /// <summary>
        /// Название параметра
        /// </summary>
        private System.String m_strParamName;
        /// <summary>
        /// Название параметра
        /// </summary>
        public System.String ParamName
        {
            get { return m_strParamName; }
            set { m_strParamName = value; }
        }
        /// <summary>
        /// Номер столбца
        /// </summary>
        private System.Int32 m_iColumnID;
        /// <summary>
        /// Номер столбца
        /// </summary>
        public System.Int32 ColumnID
        {
            get { return m_iColumnID; }
            set { m_iColumnID = value; }
        }

        #endregion

        #region Конструктор
        public CSettingItemForCalcPrice()
        {
            m_iColumnID = 0;
            m_strParamName = "";
        }
        public CSettingItemForCalcPrice(System.Int32 iColumnID, System.String strParamName)
        {
            m_iColumnID = iColumnID;
            m_strParamName = strParamName;
        }
        #endregion
    }
    /// <summary>
    /// Класс "Настройки для расчета цен"
    /// </summary>
    public class CSettingForCalcPrice
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// Номер листа
        /// </summary>
        private System.Int32 m_iSheetID;
        /// <summary>
        /// Номер листа
        /// </summary>
        public System.Int32 SheetID
        {
            get { return m_iSheetID; }
            set { m_iSheetID = value; }
        }
        /// <summary>
        /// Список настроек
        /// </summary>
        private List<CSettingItemForCalcPrice> m_objSettingsList;
        /// <summary>
        /// Список настроек
        /// </summary>
        public List<CSettingItemForCalcPrice> SettingsList
        {
            get { return m_objSettingsList; }
            set { m_objSettingsList = value; }
        }
        /// <summary>
        /// настройки в виде xml
        /// </summary>
        private System.Xml.XmlDocument m_objXMLSettings;
        /// <summary>
        /// настройки в виде xml
        /// </summary>
        public System.Xml.XmlDocument XMLSettings
        {
            get { return m_objXMLSettings; }
            set { m_objXMLSettings = value; }
        }
        /// <summary>
        /// Возвращает номер столбца для параметра
        /// </summary>
        /// <param name="strParamName">Имя параметра</param>
        /// <returns>номер столбца</returns>
        public System.Int32 GetColumnNumForParam(System.String strParamName)
        {
            System.Int32 iRes = 0;
            try
            {
                if (m_objSettingsList == null) { return iRes; }
                foreach (CSettingItemForCalcPrice objSettingItem in m_objSettingsList)
                {
                    if (objSettingItem.ParamName == strParamName)
                    {
                        iRes = objSettingItem.ColumnID;
                        break;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "GetColumnNumForParam.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return iRes;
        }
        /// <summary>
        /// Возвращает номер столбца для параметра
        /// </summary>
        /// <param name="strParamName">Имя параметра</param>
        /// <param name="objTreeList">дерево со списком параметров</param>
        /// <param name="objParamNameColumn">столбец с названиями параметров</param>
        /// <param name="objColumnId">столбец с номерами столбцов Excel</param>
        /// <returns>номер столбца</returns>
        public static System.Int32 GetColumnNumForParam(System.String strParamName, 
            DevExpress.XtraTreeList.TreeList objTreeList, 
            DevExpress.XtraTreeList.Columns.TreeListColumn objParamNameColumn, 
            DevExpress.XtraTreeList.Columns.TreeListColumn objColumnId
             )
        {
            System.Int32 iRes = 0;
            try
            {
                if (objTreeList == null) { return iRes; }
                if (objParamNameColumn == null) { return iRes; }
                if (objColumnId == null) { return iRes; }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in objTreeList.Nodes)
                {
                    if ( System.Convert.ToString( objNode.GetValue(objParamNameColumn) ) == strParamName)
                    {
                        iRes = System.Convert.ToInt32(objNode.GetValue(objColumnId));
                        break;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "GetColumnNumForParam.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return iRes;
        }

        public static readonly System.String strParamNameStartRow = "Начальная строка";
        public static readonly System.String strParamNameProductOwner = "Товарная марка";
        public static readonly System.String strParamNameProductSubType = "Товарная подгруппа";
        public static readonly System.String strParamNameProductSubTypeID = "Код подгруппы";
        public static readonly System.String strParamNameProductSubTypeState = "Состояние подгруппы";
        public static readonly System.String strParamNameVendorTarif = "Тариф поставщика";
        public static readonly System.String strParamNameTransportTarif = "Транспортные расходы, %";
        public static readonly System.String strParamNameTransportTarifSum = "Сумма расходов на транспорт";
        public static readonly System.String strParamNameCustomTarif = "Таможенные расходы, %";
        public static readonly System.String strParamNameCustomTarifSum = "Сумма расходов на таможню";
        public static readonly System.String strParamNameMargin = "Наценка базовая, %";
        public static readonly System.String strParamNameNDS = "НДС, %";
        public static readonly System.String strParamNameNDSSum = "Сумма с НДС";
        public static readonly System.String strParamNameCurRate = "Курс ценообразования";
        public static readonly System.String strParamNameDiscont = "Наценка, компенсирующая постоянную скидку, %";
        public static readonly System.String strParamNameMarkUpReqiured = "Требуемая наценка, %";

        #endregion

        #region Конструктор
        public CSettingForCalcPrice()
        {
            m_uuidID = System.Guid.Empty;
            m_iSheetID = 0;
            m_objSettingsList = null;
            m_objXMLSettings = null;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список настроек
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список настроек</returns>
        public static List<CSettingForCalcPrice> GetSettingForCalcPriceList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CSettingForCalcPrice> objList = new List<CSettingForCalcPrice>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objList;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPriceListCalcSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CSettingForCalcPrice objSettingForCalcPrice = null;
                    while (rs.Read())
                    {
                        objSettingForCalcPrice = new CSettingForCalcPrice();
                        objSettingForCalcPrice.ID = (System.Guid)rs["Settings_Guid"];
                        objSettingForCalcPrice.SheetID = System.Convert.ToInt32(rs["Settings_SheetId"]);
                        objSettingForCalcPrice.SettingsList = new List<CSettingItemForCalcPrice>();

                        objSettingForCalcPrice.XMLSettings = new System.Xml.XmlDocument();
                        objSettingForCalcPrice.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objSettingForCalcPrice.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objSettingForCalcPrice.SettingsList.Add(new CSettingItemForCalcPrice(System.Convert.ToInt32(objChildNode.Attributes["ColumnNum"].Value), objChildNode.Attributes["CalcParamName"].Value));
                            }
                        }

                        objList.Add(objSettingForCalcPrice);

                    }

                    objSettingForCalcPrice = null;
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список настроек.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Сохранение настроек в базе данных
        /// <summary>
        /// Сохраняет в БД настройки для экспорта тарифов товарных подгрупп в MS Excel
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>true - успешное завершение операции; false - ошибка</returns>
        public System.Boolean SaveExportSettingForCalcPriceList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPriceListCalcSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Settings_SheetId", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Settings_XML", System.Data.SqlDbType.Xml));
                cmd.Parameters["@Settings_SheetId"].Value = this.SheetID;
                cmd.Parameters["@Settings_XML"].Value = this.XMLSettings.InnerXml;
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion
    }
    /// <summary>
    /// Прайс-лист по подгруппам
    /// </summary>
    public class CProductSubTypePriceList
    {
        #region Свойства
        /// <summary>
        /// Список "Товарная подгруппа - Цены"
        /// </summary>
        private List<CPriceListCalcItem> m_objPriceListCalcItemList;
        /// <summary>
        /// Список "Товарная подгруппа - Цены"
        /// </summary>
        public List<CPriceListCalcItem> PriceItemmList
        {
            get { return m_objPriceListCalcItemList; }
            set { m_objPriceListCalcItemList = value; }
        }
        #endregion

        #region Конструктор
        public CProductSubTypePriceList()
        {
            m_objPriceListCalcItemList = null;
        }
        #endregion

        #region Содержимое прайс листа
        /// <summary>
        /// Загружает содержимое прайс-листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadPriceList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidProductSubTypeId)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubTypePriceList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                if (uuidProductSubTypeId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Partsubtype_Guid"].Value = uuidProductSubTypeId;
                }

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    if( this.m_objPriceListCalcItemList == null ) { this.m_objPriceListCalcItemList = new List<CPriceListCalcItem>(); }
                    else { this.m_objPriceListCalcItemList.Clear(); }

                    System.Int32 iPartSubTypeId = 0;
                    CProductSubType objProductSubType = null;
                    CPriceType objPriceType = null;
                    CPriceListCalcItem objPriceListCalcItem = null;
                    CPrice objPrice = null;
                    while (rs.Read())
                    {
                        objPriceType = new CPriceType(
                            (System.Guid)rs["PartsubtypePriceType_Guid"],
                            (System.String)rs["PartsubtypePriceType_Name"],
                            (System.String)rs["PartsubtypePriceType_Abbr"],
                            ((rs["PartsubtypePriceType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsubtypePriceType_Description"]),
                            System.Convert.ToBoolean(rs["PartsubtypePriceType_IsActive"]),
                            new CCurrency(
                            (System.Guid)rs["Currency_Guid"],
                            (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"],
                            (System.String)rs["Currency_Code"]
                            ),
                            System.Convert.ToInt32(rs["PartsubtypePriceType_ColumnIdDefault"]),
                            System.Convert.ToBoolean(rs["PartsubtypePriceType_ShowInPriceList"])
                            );

                        objPrice = new CPrice(objPriceType, System.Convert.ToDouble(rs["Price_Value"]));

                        if (System.Convert.ToInt32(rs["Partsubtype_Id"]) != iPartSubTypeId)
                        {
                            // новая подгруппа, и как следствие новая запись в приложении к расчету
                            objProductSubType = new CProductSubType();
                            objProductSubType.ID = (System.Guid)rs["Partsubtype_Guid"];
                            objProductSubType.Name = (System.String)rs["Partsubtype_Name"];
                            objProductSubType.ID_Ib = System.Convert.ToInt32(rs["Partsubtype_Id"]);

                            if( rs["Partline_Guid"] != System.DBNull.Value )
                            {
                              objProductSubType.ProductLine = new CProductLine(
                                  (System.Guid)rs["Partline_Guid"],
                                  (System.String)rs["Partline_Name"],
                                  System.Convert.ToInt32(rs["Partline_Id"]),
                                  ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                                  System.Convert.ToBoolean(rs["Partline_IsActive"])                              
                                  );
                            }
                            if( rs["PartsubtypeState_Guid"] != System.DBNull.Value )
                            {
                              objProductSubType.SubTypeState =  new CProductSubTypeState(
                                  (System.Guid)rs["PartsubtypeState_Guid"],
                                  (System.String)rs["PartsubtypeState_Name"],
                                  ((rs["PartsubtypeState_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsubtypeState_Description"]),
                                  System.Convert.ToBoolean(rs["PartsubtypeState_IsActive"])
                                  );
                            }
                            if (rs["Owner_Name"] != System.DBNull.Value)
                            {
                                objProductSubType.ProductOwner = (System.String)rs["Owner_Name"];
                            }
                            if (rs["Parttype_Name"] != System.DBNull.Value)
                            {
                                objProductSubType.ProductType = (System.String)rs["Parttype_Name"];
                            }


                            //objProductSubType.VendorTariff = System.Convert.ToDouble(rs["Partsubtype_VendorTariff"]);
                            //objProductSubType.CustomsTariff = System.Convert.ToDouble(rs["Partsubtype_CustomsTariff"]);
                            //objProductSubType.TransportTariff = System.Convert.ToDouble(rs["Partsubtype_TransportTariff"]);
                            //objProductSubType.Discont = System.Convert.ToDouble(rs["Partsubtype_Discont"]);
                            //objProductSubType.Margin = System.Convert.ToDouble(rs["Partsubtype_Margin"]);
                            //objProductSubType.NDS = System.Convert.ToDouble(rs["Partsubtype_NDS"]);

                            objPriceListCalcItem = new CPriceListCalcItem( objProductSubType, 0, 0, 0, 0 );

                            objPriceListCalcItem.PriceList = new List<CPrice>();

                            this.m_objPriceListCalcItemList.Add( objPriceListCalcItem );

                            iPartSubTypeId = objProductSubType.ID_Ib;
                        }

                        objPriceListCalcItem.PriceList.Add(objPrice);
                    }
                }
                bRet = true;
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товарных подгрупп с ценами.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        #region Сохранение содержимого прайс-листа в БД
        /// <summary>
        /// Сохраняет в БД содержимое прайс-листа
        /// </summary>
        /// <param name="objPriceListCalcItemList">список цен</param>
        /// <param name="objPriceTypeCheckedList">список типов цен, которые необходимо сохранить</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveCalcItemListCheck(List<CPriceListCalcItem> objPriceListCalcItemList, List<CPriceType> objPriceTypeCheckedList,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                System.Boolean bNeedAddPrice = false;
                if ((objPriceListCalcItemList != null) && (objPriceListCalcItemList.Count > 0))
                {
                    foreach (CPriceListCalcItem objItem in objPriceListCalcItemList)
                    {
                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                bNeedAddPrice = false;
                                foreach (CPriceType objPriceType in objPriceTypeCheckedList)
                                {
                                    if (objPriceType.ID.CompareTo(objPrice.PriceType.ID) == 0)
                                    {
                                        bNeedAddPrice = true;
                                        break;
                                    }
                                }
                                if (bNeedAddPrice == true)
                                {
                                    newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                    newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                                    newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                    newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                    tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                                }
                            }
                        }

                    }

                }
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.CommandTimeout = 600;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignPartsubtypeWithPriceList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        /// <summary>
        /// Сохраняет в БД содержимое прайс-листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveCalcItemList(List<CPriceListCalcItem> objPriceListCalcItemList, 
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                if ((objPriceListCalcItemList != null) && (objPriceListCalcItemList.Count > 0))
                {
                    foreach (CPriceListCalcItem objItem in objPriceListCalcItemList)
                    {
                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                                newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                            }
                        }

                    }

                }
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.CommandTimeout = 600;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignPartsubtypeWithPriceList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region Сохранение содержимого прайс-листа в InterBase
        /// <summary>
        /// Сохраняет в БД содержимое прайс-листа в InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SavePriceListToIB(List<CPriceListCalcItem> objPriceListCalcItemList, List<CPriceType> objPriceTypeCheckedList,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iCommandTimeOut = 600;
            try
            {

                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                System.Boolean bNeedAddPrice = false;
                if ((objPriceListCalcItemList != null) && (objPriceListCalcItemList.Count > 0) && ( objPriceTypeCheckedList != null ) && ( objPriceTypeCheckedList.Count > 0 ) )
                {
                    foreach (CPriceListCalcItem objItem in objPriceListCalcItemList)
                    {
                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                bNeedAddPrice = false;
                                foreach (CPriceType objPriceType in objPriceTypeCheckedList)
                                {
                                    if (objPriceType.ID.CompareTo(objPrice.PriceType.ID) == 0)
                                    {
                                        bNeedAddPrice = true;
                                        break;
                                    }
                                }
                                if (bNeedAddPrice == true)
                                {
                                    newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                    newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                                    newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                    newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                    tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                                }
                            }
                        }

                    }
                }
                tPriceListCalcItemsPrice.AcceptChanges();

                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandTimeout = iCommandTimeOut;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignPartsubtypeWithPriceListInIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                 strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region Удаление записей из прайс-листа
        /// <summary>
        /// Удаление записей из прайс-листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean DeleteCalcItemList(List<CPriceListCalcItem> objPriceListCalcItemList,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                if ((objPriceListCalcItemList != null) && (objPriceListCalcItemList.Count > 0))
                {
                    foreach (CPriceListCalcItem objItem in objPriceListCalcItemList)
                    {
                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.objProductSubType.ID;
                                newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                            }
                        }

                    }
                }
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartsubtypeWithPriceList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region экспорт в MS Excel
         /// <summary>
        /// Экспорт содержимого прайс-листа в MS Ecel
        /// </summary>
        /// <param name="objCPriceListCalcItemList">список строк прайса</param>
        /// <param name="objPriceTypeList">список типов цен</param>
        /// <param name="bIsPriceListEditor">список типов цен</param>
        public static void ExportToExcel(List<CPriceListCalcItem> objCPriceListCalcItemList, List<CPriceType> objPriceTypeList,
            System.Boolean bIsPriceListEditor )
        {
            if ((objCPriceListCalcItemList == null) || (objPriceTypeList == null)) { return; }
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            try
            {
                //Start Excel and get Application object.
                List<System.String> strProductOwnerList = new List<string>();
                System.Boolean bProductOwnerExistsInList = false;
                foreach (CPriceListCalcItem objItem in objCPriceListCalcItemList)
                {
                    bProductOwnerExistsInList = false;
                    foreach (System.String strProductOwner in strProductOwnerList)
                    {
                        if (strProductOwner == objItem.ProductOwner)
                        {
                            bProductOwnerExistsInList = true;
                            break;
                        }
                    }
                    if (bProductOwnerExistsInList == false)
                    {
                        strProductOwnerList.Add(objItem.ProductOwner);
                    }
                }

                oXL = new Excel.Application();

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                if (oWB.Sheets.Count < strProductOwnerList.Count)
                {
                    System.Int32 iDiffSheets = strProductOwnerList.Count - oWB.Sheets.Count;
                    for (System.Int32 i = 0; i < iDiffSheets; i++)
                    {
                        oWB.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    }
                }

                System.Int32 iStartPriceColNum = 6;
                System.Int32 iColNum = 0;
                System.Int32 iLastIndxRowForPrint = 2;


                foreach (System.String strProductOwner in strProductOwnerList)
                {
                    oSheet = (Excel._Worksheet)oWB.Worksheets[strProductOwnerList.IndexOf(strProductOwner) + 1];

                    oSheet.Name = strProductOwner;
                    oSheet.Cells[1, 1] = "Товарная марка";
                    oSheet.Cells[1, 2] = "Товарная группа";
                    oSheet.Cells[1, 3] = "Товарная подгруппа";
                    oSheet.Cells[1, 4] = "Состояние";
                    oSheet.Cells[1, 5] = "Код подгруппы";

                    iStartPriceColNum = 6;
                    iColNum = 0;
                    iColNum = iStartPriceColNum;

                    foreach (CPriceType objPriceType in objPriceTypeList)
                    {
                        if ((objPriceType.IsShowInPrice == false) && (bIsPriceListEditor == false)) { continue; }
                        oSheet.Cells[1, iColNum] = objPriceType.Name;
                        iColNum++;
                    }

                    iLastIndxRowForPrint = 2;

                    foreach (CPriceListCalcItem objPriceListItem in objCPriceListCalcItemList)
                    {
                        if (objPriceListItem.ProductOwner != strProductOwner) { continue; }
                        oSheet.Cells[iLastIndxRowForPrint, 1] = objPriceListItem.ProductOwner;
                        oSheet.Cells[iLastIndxRowForPrint, 2] = objPriceListItem.ProductType;
                        oSheet.Cells[iLastIndxRowForPrint, 3] = objPriceListItem.PartSubTypeName;
                        oSheet.Cells[iLastIndxRowForPrint, 4] = objPriceListItem.SubTypeStateName;

                        if (objPriceListItem.SubTypeStateName == ERP_Mercury.Global.Consts.strWarningProductSubTypeStateSate)
                        {
                            oSheet.get_Range(oSheet.Cells[iLastIndxRowForPrint, 1], oSheet.Cells[iLastIndxRowForPrint, 5]).Interior.Color = 255;
                        }

                        oSheet.Cells[iLastIndxRowForPrint, 5] = objPriceListItem.PartSubTypeID_Ib;

                        if (objPriceListItem.PriceList != null)
                        {
                            iColNum = iStartPriceColNum;
                            foreach (CPriceType objPriceType in objPriceTypeList)
                            {
                                if ((objPriceType.IsShowInPrice == false) && (bIsPriceListEditor == false)) { continue; }

                                foreach (CPrice objPrice in objPriceListItem.PriceList)
                                {
                                    if ((objPrice.PriceType.IsShowInPrice == false) && (bIsPriceListEditor == false)) { continue; }

                                    if (objPrice.PriceType.ID.CompareTo(objPriceType.ID) == 0)
                                    {
                                        oSheet.Cells[iLastIndxRowForPrint, iColNum] = objPrice.PriceValue;
                                        break;
                                    }
                                }
                                iColNum++;
                            }
                        }

                        iLastIndxRowForPrint++;
                    }

                    oSheet.get_Range("A1", "Z1").Font.Size = 12;
                    oSheet.get_Range("A1", "Z1").Font.Bold = true;
                    oSheet.get_Range("A1", "Z1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                    oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();

                    oSheet.get_Range("A1", "A1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                    oSheet.get_Range("F1", "Z10000").NumberFormat = "# ##0,000";

                }

                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oSheet = null;
                oWB = null;
                oXL = null;
            }
        }

        public static void ExportToExcel2(List<CPriceListCalcItem> objCPriceListCalcItemList, CPriceType objPriceType, 
            CProductOwner objProductOwner, System.Int32 iStartRow, System.Int32 iColumnPartType, System.Int32 iColumnPrice,
            Excel._Worksheet oSheet, ref System.String strErr )
        {
            if ((objCPriceListCalcItemList == null) || (objPriceType == null)) { return; }

            try
            {

                System.Int32 iLastIndxRowForPrint = iStartRow;

                System.String strFormula = System.Convert.ToString( oSheet.Cells[iLastIndxRowForPrint, iColumnPartType] );
                System.String strCurrentPartTypeName = "";
                foreach (CPriceListCalcItem objPriceListItem in objCPriceListCalcItemList)
                {
                    if (objPriceListItem.ProductOwner != objProductOwner.Name) { continue; }

                    if (strCurrentPartTypeName != objPriceListItem.ProductType)
                    {
                        strCurrentPartTypeName = objPriceListItem.ProductType;
                        oSheet.Cells[iLastIndxRowForPrint, iColumnPartType] = strCurrentPartTypeName;
                        oSheet.get_Range(oSheet.Cells[iLastIndxRowForPrint, iColumnPartType], oSheet.Cells[iLastIndxRowForPrint, iColumnPartType]).Interior.Color = 12895428;
                        oSheet.Cells[iLastIndxRowForPrint, iColumnPrice] = "";
                        oSheet.Cells[iLastIndxRowForPrint, iColumnPrice + 1] = "";
                        iLastIndxRowForPrint++;
                    }

                    oSheet.Cells[iLastIndxRowForPrint, iColumnPartType] = objPriceListItem.PartSubTypeName;
                    oSheet.Cells[iLastIndxRowForPrint, iColumnPrice] = objPriceListItem.GetPriceValueByType(objPriceType);
                    oSheet.get_Range(oSheet.Cells[iLastIndxRowForPrint, iColumnPrice + 1], oSheet.Cells[iLastIndxRowForPrint, iColumnPrice + 1]).Font.ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                    if (objPriceListItem.SubTypeStateName.ToUpper() != "ТОВАР") { oSheet.Cells[iLastIndxRowForPrint, iColumnPrice + 3] = objPriceListItem.SubTypeStateName; }
                    iLastIndxRowForPrint++;

                }

                oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка экспорта в MS Excel. Текст ошибки: " + f.Message;
            }
            finally
            {
            }
        }
        
        #endregion

    }



    public class CSettingItemForImportData
    {
        #region Свойства
        /// <summary>
        /// Идентификатор параметра
        /// </summary>
        public System.Int32 TOOLS_ID { get; set; }
        /// <summary>
        /// Параметр (наименование)
        /// </summary>
        public System.String TOOLS_NAME { get; set; }
        /// <summary>
        /// Параметр (наименование для отображения пользователю)
        /// </summary>
        public System.String TOOLS_USERNAME { get; set; }
        /// <summary>
        /// Описание параметра
        /// </summary>
        public System.String TOOLS_DESCRIPTION { get; set; }
        /// <summary>
        /// Значение параметра
        /// </summary>
        public System.Int32 TOOLS_VALUE { get; set; }
        /// <summary>
        /// УИ
        /// </summary>
        public System.Guid TOOLSTYPE_GUID { get; set; }
        #endregion

        #region Конструктор
        public CSettingItemForImportData()
        {
            TOOLS_ID = 0;
            TOOLS_NAME = "";
            TOOLS_USERNAME = "";
            TOOLS_DESCRIPTION = "";
            TOOLS_VALUE = 0;
            TOOLSTYPE_GUID = System.Guid.Empty;
        }
        #endregion
    }

    public class CSettingForImportData
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public System.String Name { get; set; }
        /// <summary>
        /// Список параметров в xml-виде
        /// </summary>
        public System.Xml.XmlDocument XMLSettings { get; set; }
        /// <summary>
        /// Список параметров
        /// </summary>
        public List<CSettingItemForImportData> SettingsList { get; set; }

        public static readonly System.String strFieldNameSTARTROW = "STARTROW";
        public static readonly System.String strFieldNameARTICLE = "ARTICLE";
        public static readonly System.String strFieldNameNAME2 = "NAME2";
        public static readonly System.String strFieldNameQUANTITY = "QUANTITY";
        public static readonly System.String strFieldNamePRICE = "PRICE";
        public static readonly System.String strFieldNameMARKUP = "MARKUP";
        public static readonly System.String strFieldNameDEPART_CODE = "DEPART_CODE";
        public static readonly System.String strFieldNameCUSTOMER_ID = "CUSTOMER_ID";
        public static readonly System.String strFieldNameRTT_CODE = "RTT_CODE";
        public static readonly System.String strFieldNameFULLNAME = "FULLNAME";
        public static readonly System.String strFieldNamePARTS_ID = "PARTS_ID";

        #endregion

        #region Конструктор
        public CSettingForImportData()
        {
            ID = System.Guid.Empty;
            Name = "";
            SettingsList = null;
            XMLSettings = null;
        }
        #endregion

        #region Список параметров
        /// <summary>
        /// Возвращает настройки для импорта данных в заказ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список настроек</returns>
        public static CSettingForImportData GetSettingForImportData(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            CSettingForImportData objRet = null;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetImportDataInProductPriceListSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    rs.Read();
                    {
                        objRet = new CSettingForImportData();
                        objRet.ID = (System.Guid)rs["Settings_Guid"];
                        objRet.Name = System.Convert.ToString(rs["Settings_Name"]);
                        objRet.SettingsList = new List<CSettingItemForImportData>();

                        objRet.XMLSettings = new System.Xml.XmlDocument();
                        objRet.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objRet.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objRet.SettingsList.Add(new CSettingItemForImportData()
                                {
                                    TOOLS_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_ID"].Value),
                                    TOOLS_NAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_NAME"].Value),
                                    TOOLS_USERNAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_USERNAME"].Value),
                                    TOOLS_DESCRIPTION = System.Convert.ToString(objChildNode.Attributes["TOOLS_DESCRIPTION"].Value),
                                    TOOLS_VALUE = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_VALUE"].Value),
                                    TOOLSTYPE_GUID = ((System.Convert.ToString(objChildNode.Attributes["TOOLSTYPE_GUID"].Value) == "") ? System.Guid.Empty : new System.Guid(System.Convert.ToString(objChildNode.Attributes["TOOLSTYPE_GUID"].Value)))
                                });
                            }
                        }
                    }

                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список настроек.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
        }

        #endregion

        #region Сохранение настроек в базе данных
        /// <summary>
        /// Сохраняет в БД настройки для импорта данных в заказ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>true - успешное завершение операции; false - ошибка</returns>
        public System.Boolean SaveExportSetting(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = CSetting.SaveSettingInDB(this.ID, this.XMLSettings.InnerXml, objProfile, cmdSQL, ref strErr);
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }
            finally
            {
            }
            return bRet;
        }
        #endregion

    }



    /// <summary>
    ///  строка прайс-листа по товарам
    /// </summary>
    public class CPartsPriceListItem
    {
        #region Свойства
        /// <summary>
        /// Товар
        /// </summary>
        private ERP_Mercury.Common.CProduct m_objProduct;
        /// <summary>
        /// Товар
        /// </summary>
        public ERP_Mercury.Common.CProduct Product
        {
            get { return m_objProduct; }
            set { m_objProduct = value; }
        }
        /// <summary>
        /// Список цен
        /// </summary>
        private List<CPrice> m_objPriceList;
        /// <summary>
        /// Список цен
        /// </summary>
        public List<CPrice> PriceList
        {
            get { return m_objPriceList; }
            set { m_objPriceList = value; }
        }
        /// <summary>
        /// Возвращает значение цены по заданному типу цены
        /// </summary>
        /// <param name="objPriceType">тип цены</param>
        /// <returns>значение цены</returns>
        public System.Double GetPriceValueByType(CPriceType objPriceType)
        {
            System.Double dblRet = 0;
            try
            {
                if ((PriceList != null) && (PriceList.Count > 0))
                {
                    foreach (CPrice objPrice in PriceList)
                    {
                        if (objPrice.PriceType.ID.CompareTo(objPriceType.ID) == 0)
                        {
                            dblRet = objPrice.PriceValue;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "PriceValueByType. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return dblRet;

        }

        public System.Int32 PartsID_Ib
        {
            get { return ((m_objProduct == null) ? 0 : m_objProduct.ID_Ib); }
        }
        public System.String PartsName
        {
            get { return ((m_objProduct == null) ? "" : m_objProduct.ProductFullName); }
        }
        public System.String ProductOwner
        {
            get { return ((m_objProduct == null) ? "" : m_objProduct.ProductTradeMarkName); }
        }
        public System.String ProductType
        {
            get { return ((m_objProduct == null) ? "" : m_objProduct.ProductTypeName); }
        }
        public System.String ProductLineName
        {
            get { return ((m_objProduct == null) ? "" : m_objProduct.ProductLineName); }
        }
        public System.String ProductSubTypeName
        {
            get { return ((m_objProduct == null) ? "" : m_objProduct.ProductSubTypeName); }
        }

        #endregion

        #region Конструктор
        public CPartsPriceListItem()
        {
            m_objProduct = null;
            m_objPriceList = null;
        }
        public CPartsPriceListItem(CProduct objProduct)
        {
            m_objProduct = objProduct;
            m_objPriceList = null;
        }
        #endregion

        #region Список цен для товара
        /// <summary>
        /// Возвращает список цен для товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>true- удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadPriceListForProduct( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (this.m_objPriceList == null) { this.m_objPriceList = new List<CPrice>(); }
                else { this.m_objPriceList.Clear(); }

                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsPriceList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = this.m_objProduct.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    CPriceType objPriceType = null;
                    CPrice objPrice = null;
                    while (rs.Read())
                    {
                        objPriceType = new CPriceType(
                            (System.Guid)rs["PartsubtypePriceType_Guid"],
                            (System.String)rs["PartsubtypePriceType_Name"],
                            (System.String)rs["PartsubtypePriceType_Abbr"],
                            ((rs["PartsubtypePriceType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsubtypePriceType_Description"]),
                            System.Convert.ToBoolean(rs["PartsubtypePriceType_IsActive"]),
                            new CCurrency(
                            (System.Guid)rs["Currency_Guid"],
                            (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"],
                            (System.String)rs["Currency_Code"]
                            ),
                            System.Convert.ToInt32(rs["PartsubtypePriceType_ColumnIdDefault"]),
                            System.Convert.ToBoolean(rs["PartsubtypePriceType_ShowInPriceList"])
                            );

                        objPrice = new CPrice(objPriceType, System.Convert.ToDouble(rs["Price_Value"]));

                        this.PriceList.Add(objPrice);

                        }
                }
                bRet = true;
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список цен для товара.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion
 
    }



    /// <summary>
    /// Прайс-лист по товарам
    /// </summary>
    public class CProductPriceList
    {
        #region Свойства
        /// <summary>
        /// Список "Товар - Цены"
        /// </summary>
        private List<CPartsPriceListItem> m_objPriceListCalcItemList;
        /// <summary>
        /// Список "Товар - Цены"
        /// </summary>
        public List<CPartsPriceListItem> PriceItemmList
        {
            get { return m_objPriceListCalcItemList; }
            set { m_objPriceListCalcItemList = value; }
        }
        #endregion

        #region Конструктор
        public CProductPriceList()
        {
            m_objPriceListCalcItemList = null;
        }
        #endregion

        #region Содержимое прайс листа
        /// <summary>
        /// Загружает содержимое прайс-листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadPriceList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            System.Boolean bRet = false;
            try
            {
                if( this.m_objPriceListCalcItemList == null )
                {
                    this.m_objPriceListCalcItemList = new List<CPartsPriceListItem>();
                }
                else
                {
                    this.m_objPriceListCalcItemList.Clear();
                }
                List<CProduct> objProductList = CProduct.GetProductListForPriceList(objProfile, cmdSQL);

                if (objProductList != null)
                {
                    foreach (CProduct objProduct in objProductList)
                    {
                        this.m_objPriceListCalcItemList.Add(new CPartsPriceListItem(objProduct));
                    }
                }
                objProductList = null;

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товаров из прайса.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }



        #endregion

        #region Сохранение содержимого прайс-листа в БД
        /// <summary>
        /// Сохраняет в БД содержимое прайс-листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SavePriceList(List<CPartsPriceListItem> objPriceListCalcItemList,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                if ((objPriceListCalcItemList != null) && (objPriceListCalcItemList.Count > 0))
                {
                    foreach (CPartsPriceListItem objItem in objPriceListCalcItemList)
                    {
                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.Product.ID;
                                newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                            }
                        }

                    }
                }
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignPartsWithPriceList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region Сохранение содержимого прайс-листа в InterBase
        /// <summary>
        /// Сохраняет в БД содержимое прайс-листа в InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SavePriceListToIB(List<CPartsPriceListItem> objPriceListCalcItemList, List<CPriceType> objPriceTypeCheckedList,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iCommandTimeOut = 600;
            try
            {

                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                System.Boolean bNeedAddPrice = false;
                if ((objPriceListCalcItemList != null) && (objPriceListCalcItemList.Count > 0) && (objPriceTypeCheckedList != null) && (objPriceTypeCheckedList.Count > 0))
                {
                    foreach (CPartsPriceListItem objItem in objPriceListCalcItemList)
                    {
                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                bNeedAddPrice = false;
                                foreach (CPriceType objPriceType in objPriceTypeCheckedList)
                                {
                                    if (objPriceType.ID.CompareTo(objPrice.PriceType.ID) == 0)
                                    {
                                        bNeedAddPrice = true;
                                        break;
                                    }
                                }
                                if (bNeedAddPrice == true)
                                {
                                    newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                    newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.Product.ID;
                                    newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                    newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                    tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                                }
                            }
                        }

                    }
                }
                tPriceListCalcItemsPrice.AcceptChanges();

                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandTimeout = iCommandTimeOut;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignPartsWithPriceListInIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region Удаление записей из прайс-листа
        /// <summary>
        /// Удаление записей из прайс-листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean DeletePriceListItems(List<CPartsPriceListItem> objPriceListCalcItemList,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                if ((objPriceListCalcItemList != null) && (objPriceListCalcItemList.Count > 0))
                {
                    foreach (CPartsPriceListItem objItem in objPriceListCalcItemList)
                    {
                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.Product.ID;
                                newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                            }
                        }

                    }
                }
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartsPriceList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region Удаление позиций из прайс-листа в InterBase
        /// <summary>
        /// Удаление позиций из прайс-листа в InterBase
        /// </summary>
        /// <param name="objPriceListCalcItemList">список позиций на удаление</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean DeletePriceListFromIB(List<CPartsPriceListItem> objPriceListCalcItemList, 
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {

            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iCommandTimeOut = 600;
            try
            {

                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Partsubtype_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("PartsubtypePriceType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Price_Value", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                if ((objPriceListCalcItemList != null) && (objPriceListCalcItemList.Count > 0) )
                {
                    foreach (CPartsPriceListItem objItem in objPriceListCalcItemList)
                    {
                        if ((objItem.PriceList != null) && (objItem.PriceList.Count > 0))
                        {
                            foreach (CPrice objPrice in objItem.PriceList)
                            {
                                newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                                newRowCalcItemsPrice["Partsubtype_Guid"] = objItem.Product.ID;
                                newRowCalcItemsPrice["PartsubtypePriceType_Guid"] = objPrice.PriceType.ID;
                                newRowCalcItemsPrice["Price_Value"] = System.Convert.ToDecimal(objPrice.PriceValue);
                                tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);
                            }
                        }

                    }
                }
                tPriceListCalcItemsPrice.AcceptChanges();

                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandTimeout = iCommandTimeOut;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartsPriceListFromIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tPriceListCalcItemsPrice", tPriceListCalcItemsPrice);
                cmd.Parameters["@tPriceListCalcItemsPrice"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tPriceListCalcItemsPrice"].TypeName = "dbo.udt_PriceListCalcItemsPrice";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region Добавление позиций в прайс-лист
        /// <summary>
        /// Добавление новых позиций в прайс-лист (с нулевыми ценами)
        /// </summary>
        /// <param name="objProductGuidList">список товаров</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddProductListToPartsPriceList(List<CProduct> objProductList,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {

            System.Boolean bRet = false;
            if (objProductList == null) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable tPriceListCalcItemsPrice = new System.Data.DataTable();
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Product_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                tPriceListCalcItemsPrice.Columns.Add(new System.Data.DataColumn("Product_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRowCalcItemsPrice = null;
                if ((objProductList != null) && (objProductList.Count > 0))
                {
                    foreach (CProduct objItem in objProductList)
                    {
                        newRowCalcItemsPrice = tPriceListCalcItemsPrice.NewRow();
                        newRowCalcItemsPrice["Product_Guid"] = objItem.ID;
                        newRowCalcItemsPrice["Product_Id"] = objItem.ID_Ib;
                        tPriceListCalcItemsPrice.Rows.Add(newRowCalcItemsPrice);

                    }
                }
                tPriceListCalcItemsPrice.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartsPriceItem]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tProduct", tPriceListCalcItemsPrice);
                cmd.Parameters["@tProduct"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProduct"].TypeName = "dbo.udt_Product";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        #endregion

        #region экспорт в MS Excel
        /// <summary>
        /// Экспорт содержимого прайс-листа в MS Ecel
        /// </summary>
        /// <param name="objCPriceListCalcItemList">список строк прайса</param>
        /// <param name="objPriceTypeList">список типов цен</param>
        /// <param name="bIsPriceListEditor">список типов цен</param>
        public static void ExportToExcel(List<CPartsPriceListItem> objCPriceListCalcItemList, List<CPriceType> objPriceTypeList,
            System.Boolean bIsPriceListEditor, UniXP.Common.CProfile objProfile)
        {
            if ((objCPriceListCalcItemList == null) || (objPriceTypeList == null)) { return; }
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            try
            {
                //Start Excel and get Application object.
                List<System.String> strProductOwnerList = new List<string>();
                System.Boolean bProductOwnerExistsInList = false;
                foreach (CPartsPriceListItem objItem in objCPriceListCalcItemList)
                {
                    bProductOwnerExistsInList = false;
                    foreach (System.String strProductOwner in strProductOwnerList)
                    {
                        if (strProductOwner == objItem.ProductOwner)
                        {
                            bProductOwnerExistsInList = true;
                            break;
                        }
                    }
                    if (bProductOwnerExistsInList == false)
                    {
                        strProductOwnerList.Add(objItem.ProductOwner);
                    }
                }

                oXL = new Excel.Application();

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                if (oWB.Sheets.Count < strProductOwnerList.Count)
                {
                    System.Int32 iDiffSheets = strProductOwnerList.Count - oWB.Sheets.Count;
                    for (System.Int32 i = 0; i < iDiffSheets; i++)
                    {
                        oWB.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    }
                }

                System.Int32 iStartPriceColNum = 6;
                System.Int32 iColNum = 0;
                System.Int32 iLastIndxRowForPrint = 2;


                foreach (System.String strProductOwner in strProductOwnerList)
                {
                    oSheet = (Excel._Worksheet)oWB.Worksheets[strProductOwnerList.IndexOf(strProductOwner) + 1];

                    oSheet.Name = strProductOwner.Replace("/", " ");
                    oSheet.Cells[1, 1] = "Товарная марка";
                    oSheet.Cells[1, 2] = "Товарная группа";
                    oSheet.Cells[1, 3] = "Товарная подгруппа";
                    oSheet.Cells[1, 4] = "Товар";
                    oSheet.Cells[1, 5] = "Код товара";

                    iStartPriceColNum = 6;
                    iColNum = 0;
                    iColNum = iStartPriceColNum;

                    foreach (CPriceType objPriceType in objPriceTypeList)
                    {
                        if ((objPriceType.IsShowInPrice == false) && (bIsPriceListEditor == false)) { continue; }
                        oSheet.Cells[1, iColNum] = objPriceType.Name;
                        iColNum++;
                    }

                    iLastIndxRowForPrint = 2;

                    foreach (CPartsPriceListItem objPriceListItem in objCPriceListCalcItemList)
                    {
                        if (objPriceListItem.ProductOwner != strProductOwner) { continue; }
                        oSheet.Cells[iLastIndxRowForPrint, 1] = objPriceListItem.ProductOwner;
                        oSheet.Cells[iLastIndxRowForPrint, 2] = objPriceListItem.ProductType;
                        oSheet.Cells[iLastIndxRowForPrint, 3] = objPriceListItem.ProductSubTypeName;
                        oSheet.Cells[iLastIndxRowForPrint, 4] = objPriceListItem.Product.ProductFullName;
                        oSheet.Cells[iLastIndxRowForPrint, 5] = objPriceListItem.Product.ID_Ib;

                        if( (objPriceListItem.PriceList == null) || ( objPriceListItem.PriceList.Count == 0 ))
                        {
                            objPriceListItem.LoadPriceListForProduct(objProfile, null);
                        }
                        
                        if (objPriceListItem.PriceList != null)
                        {
                            iColNum = iStartPriceColNum;
                            foreach (CPriceType objPriceType in objPriceTypeList)
                            {
                                if ((objPriceType.IsShowInPrice == false) && (bIsPriceListEditor == false)) { continue; }

                                foreach (CPrice objPrice in objPriceListItem.PriceList)
                                {
                                    if ((objPrice.PriceType.IsShowInPrice == false) && (bIsPriceListEditor == false)) { continue; }

                                    if (objPrice.PriceType.ID.CompareTo(objPriceType.ID) == 0)
                                    {
                                        oSheet.Cells[iLastIndxRowForPrint, iColNum] = objPrice.PriceValue;
                                        break;
                                    }
                                }
                                iColNum++;
                            }
                        }

                        iLastIndxRowForPrint++;
                    }

                    oSheet.get_Range("A1", "Z1").Font.Size = 12;
                    oSheet.get_Range("A1", "Z1").Font.Bold = true;
                    oSheet.get_Range("A1", "Z1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                    oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();

                    oSheet.get_Range("A1", "A1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                    oSheet.get_Range("F1", "Z10000").NumberFormat = "# ##0,000";

                }

                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oSheet = null;
                oWB = null;
                oXL = null;
            }
        }

        public static void ExportToExcel2(List<CPartsPriceListItem> objCPriceListCalcItemList, CPriceType objPriceType,
            CProductOwner objProductOwner, System.Int32 iStartRow, System.Int32 iColumnPartType, System.Int32 iColumnPrice,
            Excel._Worksheet oSheet, ref System.String strErr)
        {
            if ((objCPriceListCalcItemList == null) || (objPriceType == null)) { return; }

            try
            {

                System.Int32 iLastIndxRowForPrint = iStartRow;

                System.String strFormula = System.Convert.ToString(oSheet.Cells[iLastIndxRowForPrint, iColumnPartType]);
                System.String strCurrentPartTypeName = "";
                foreach (CPartsPriceListItem objPriceListItem in objCPriceListCalcItemList)
                {
                    if (objPriceListItem.ProductOwner != objProductOwner.Name) { continue; }

                    if (strCurrentPartTypeName != objPriceListItem.ProductType)
                    {
                        strCurrentPartTypeName = objPriceListItem.ProductType;
                        oSheet.Cells[iLastIndxRowForPrint, iColumnPartType] = strCurrentPartTypeName;
                        oSheet.get_Range(oSheet.Cells[iLastIndxRowForPrint, iColumnPartType], oSheet.Cells[iLastIndxRowForPrint, iColumnPartType]).Interior.Color = 12895428;
                        oSheet.Cells[iLastIndxRowForPrint, iColumnPrice] = "";
                        oSheet.Cells[iLastIndxRowForPrint, iColumnPrice + 1] = "";
                        iLastIndxRowForPrint++;
                    }

                    oSheet.Cells[iLastIndxRowForPrint, iColumnPartType] = objPriceListItem.Product.ProductFullName;
                    oSheet.Cells[iLastIndxRowForPrint, iColumnPrice] = objPriceListItem.GetPriceValueByType(objPriceType);
                    oSheet.get_Range(oSheet.Cells[iLastIndxRowForPrint, iColumnPrice + 1], oSheet.Cells[iLastIndxRowForPrint, iColumnPrice + 1]).Font.ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                    iLastIndxRowForPrint++;

                }

                oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка экспорта в MS Excel. Текст ошибки: " + f.Message;
            }
            finally
            {
            }
        }

        #endregion

    }



    /// <summary>
    /// Строка вида "Товар - Цены из прайса Контракта"
    /// </summary>
    public class CProductPriceListItemIB
    {
        #region Свойства

        public CProduct Product { get; set; }
        public System.String ProductFullName { get { return Product.ProductFullName; } }
        public System.Int32 ProductIB_Id { get { return Product.ID_Ib; } }
        public double Price0 { get; set; }
        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
        public double Price4 { get; set; }
        public double Price5 { get; set; }
        public double Price6 { get; set; }
        public double Price7 { get; set; }
        public double Price8 { get; set; }
        public double Price9 { get; set; }
        public double Price10 { get; set; }
        public double Price11 { get; set; }
        public double Price12 { get; set; }
        public double Price0_1 { get; set; }
        public double Price0_2 { get; set; }
        public double Price0_3 { get; set; }
        public double Price0_10 { get; set; }
        public double Price0_11 { get; set; }
        public double Price12_1 { get; set; }
        public double Price12_2 { get; set; }

        #endregion

        #region Конструктор
        public CProductPriceListItemIB()
        {
            Product = null;
            Price0 = 0;
            Price1 = 0;
            Price2 = 0;
            Price3 = 0;
            Price4 = 0;
            Price5 = 0;
            Price6 = 0;
            Price7 = 0;
            Price8 = 0;
            Price9 = 0;
            Price10 = 0;
            Price11 = 0;
            Price12 = 0;
            Price12_1 = 0;
            Price12_2 = 0;
            Price0_1 = 0;
            Price0_2 = 0;
            Price0_3 = 0;
            Price0_10 = 0;
            Price0_11 = 0;
        }
        #endregion

        #region Список цен из Контракта
        /// <summary>
        /// возвращает список товаров с ценами из прайса для подгруппы
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL команда</param>
        /// <param name="uuidPartsubtypeGuid">уи подгруппы</param>
        /// <returns>список товаров с ценами из прайса для подгруппы</returns>
        public static List<CProductPriceListItemIB> GetPricesFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid uuidPartsubtypeGuid)
        {
            List<CProductPriceListItemIB> objList = new List<CProductPriceListItemIB>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objList;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPricesFromIBForPartsubtype]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Partsubtype_Guid"].Value = uuidPartsubtypeGuid;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CProductPriceListItemIB objCProductPriceListItemIB = null;
                    while (rs.Read())
                    {
                        objCProductPriceListItemIB = new CProductPriceListItemIB();
                        objCProductPriceListItemIB.Product = new CProduct() { ID = (System.Guid)rs["Parts_Guid"], ID_Ib = System.Convert.ToInt32(rs["Parts_Id"]), Name = System.Convert.ToString(rs["Parts_Name"]), Article = System.Convert.ToString(rs["Parts_Article"]) };
                        objCProductPriceListItemIB.Price0 = System.Convert.ToDouble( rs["PRICE0"] );
                        objCProductPriceListItemIB.Price1 = System.Convert.ToDouble(rs["PRICE1"]);
                        objCProductPriceListItemIB.Price2 = System.Convert.ToDouble(rs["PRICE2"]);
                        objCProductPriceListItemIB.Price3 = System.Convert.ToDouble(rs["PRICE3"]);
                        objCProductPriceListItemIB.Price4 = System.Convert.ToDouble(rs["PRICE4"]);
                        objCProductPriceListItemIB.Price5 = System.Convert.ToDouble(rs["PRICE5"]);
                        objCProductPriceListItemIB.Price6 = System.Convert.ToDouble(rs["PRICE6"]);
                        objCProductPriceListItemIB.Price7 = System.Convert.ToDouble(rs["PRICE7"]);
                        objCProductPriceListItemIB.Price8 = System.Convert.ToDouble(rs["PRICE8"]);
                        objCProductPriceListItemIB.Price9 = System.Convert.ToDouble(rs["PRICE9"]);
                        objCProductPriceListItemIB.Price10 = System.Convert.ToDouble(rs["PRICE10"]);
                        objCProductPriceListItemIB.Price11 = System.Convert.ToDouble(rs["PRICE11"]);
                        objCProductPriceListItemIB.Price12 = System.Convert.ToDouble(rs["PRICE12"]);
                        objCProductPriceListItemIB.Price0_3 = System.Convert.ToDouble(rs["PRICE0_3"]);
                        objCProductPriceListItemIB.Price0_1 = System.Convert.ToDouble(rs["PRICE0_1"]);
                        objCProductPriceListItemIB.Price0_2 = System.Convert.ToDouble(rs["PRICE0_2"]);
                        objCProductPriceListItemIB.Price0_10 = System.Convert.ToDouble(rs["PRICE0_10"]);
                        objCProductPriceListItemIB.Price0_11 = System.Convert.ToDouble(rs["PRICE0_11"]);
                        objCProductPriceListItemIB.Price12_1 = System.Convert.ToDouble(rs["PRICE12_1"]);
                        objCProductPriceListItemIB.Price12_2 = System.Convert.ToDouble(rs["PRICE12_2"]);

                        objList.Add(objCProductPriceListItemIB);

                    }

                    objCProductPriceListItemIB = null;
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товаров и цен из прайса \"Контракта\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

    }

    public class CProductSubTypePriceListExport
    {
        #region Свойства
        public ERP_Mercury.Common.CProductType ProductType { get; set; }
        public ERP_Mercury.Common.CProductSubType ProductSubType { get; set; }
        public CPrice Price { get; set; }
        public System.Int32 SheetNum { get; set; }
        public System.DateTime DateUpdated { get; set; } 
        #endregion

        #region Конструктор
        public CProductSubTypePriceListExport()
        {
            ProductType = null;
            ProductSubType = null;
            Price = null;
            SheetNum = 0;
        }
        #endregion

        #region Список цен
        /// <summary>
        /// Возвращает прайс
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Owner_Guid">уи товарной марки</param>
        /// <param name="PaymentType_Guid">уи формы оплаты</param>
        /// <returns></returns>
        public static List<CProductSubTypePriceListExport> LoadPriceListForProduct(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Owner_Guid, System.Guid PaymentType_Guid)
        {
            List<CProductSubTypePriceListExport> objRet = null;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubTypePriceList2]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Owner_Guid"].Value = Owner_Guid;
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    objRet = new List<CProductSubTypePriceListExport>();
                    while (rs.Read())
                    {
                        objRet.Add(new CProductSubTypePriceListExport()
                        {
                            ProductType = new CProductType() { ID = (System.Guid)rs["Parttype_Guid"], ID_Ib = (System.Convert.ToInt32(rs["Parttype_Id"])), Name = System.Convert.ToString(rs["Parttype_Name"]) },
                            ProductSubType = new ERP_Mercury.Common.CProductSubType() { ID = (System.Guid)rs["Partsubtype_Guid"], ID_Ib = (System.Convert.ToInt32(rs["Partsubtype_Id"])), Name = System.Convert.ToString(rs["Partsubtype_Name"]) },
                            Price = new CPrice() { PriceValue = System.Convert.ToDouble(rs["Price_Value"]) },
                            SheetNum = System.Convert.ToInt32(rs["Settings_SheetId"]),
                            DateUpdated = ((rs["Record_Updated"] == System.DBNull.Value) ? System.DateTime.MinValue : System.Convert.ToDateTime(rs["Record_Updated"]))
                        });

                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список цен для товарной марки.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
        }
        #endregion
    }

}
