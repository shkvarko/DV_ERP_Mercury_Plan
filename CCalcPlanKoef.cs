using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPMercuryPlan
{
    public class CCalcPlanKoef
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
        /// Дата
        /// </summary>
        private System.DateTime m_dtDate;
        /// <summary>
        /// Дата
        /// </summary>
        public System.DateTime Date
        {
            get { return m_dtDate; }
            set { m_dtDate = value; }
        }
        /// <summary>
        /// Номер
        /// </summary>
        private System.String m_strNum;
        /// <summary>
        /// Номер
        /// </summary>
        public System.String Num
        {
            get { return m_strNum; }
            set { m_strNum = value; }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
        /// </summary>
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        /// <summary>
        /// Период расчета (начало)
        /// </summary>
        private System.DateTime m_dtBeginDate;
        /// <summary>
        /// Период расчета (начало)
        /// </summary>
        public System.DateTime BeginDate
        {
            get { return m_dtBeginDate; }
            set { m_dtBeginDate = value; }
        }
        /// <summary>
        /// Период расчета (окончание)
        /// </summary>
        private System.DateTime m_dtEndDate;
        /// <summary>
        /// Период расчета (окончание)
        /// </summary>
        public System.DateTime EndDate
        {
            get { return m_dtEndDate; }
            set { m_dtEndDate = value; }
        }
        /// <summary>
        /// Приложение к расчету 
        /// </summary>
        private List<CCalcPlanKoefItem> m_objCalcPlanKoefItemList;
        /// <summary>
        /// Приложение к расчету заказа
        /// </summary>
        public List<CCalcPlanKoefItem> CalcPlanKoefItemList
        {
            get { return m_objCalcPlanKoefItemList; }
            set { m_objCalcPlanKoefItemList = value; }
        }
        public System.String ShipPeriod
        {
            get
            {
                return ("[" + m_dtBeginDate.ToShortDateString() + " - " +  m_dtEndDate.ToShortDateString() + "]");
            }
        }
        #endregion

        #region Конструктор
        public CCalcPlanKoef()
        {
            m_uuidID = System.Guid.Empty;
            m_strNum = "";
            m_dtDate = System.DateTime.MinValue;
            m_dtBeginDate = System.DateTime.MinValue;
            m_dtEndDate = System.DateTime.MinValue;
            m_strDescription = "";
            m_objCalcPlanKoefItemList = null;
        }
        public CCalcPlanKoef(System.Guid uuidID, System.String strNum, System.DateTime dtDate,
            System.DateTime dtBeginDate, System.DateTime dtEndDate, System.String strDescription)
        {
            m_uuidID = uuidID;
            m_strNum = strNum;
            m_dtDate = dtDate;
            m_dtBeginDate = dtBeginDate;
            m_dtEndDate = dtEndDate;
            m_strDescription = strDescription;
            m_objCalcPlanKoefItemList = null;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список расчетов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список расчетов</returns>
        public static List<CCalcPlanKoef> GetCalcPlanKoefList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCalcPlanKoef> objList = new List<CCalcPlanKoef>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCalcPlanKoef]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCalcPlanKoef objCalcPlanKoef = null;

                    while (rs.Read())
                    {

                        objCalcPlanKoef = new CCalcPlanKoef();
                        objCalcPlanKoef.m_uuidID = (System.Guid)rs["CalcPlanKoef_Guid"];
                        objCalcPlanKoef.m_strNum = (System.String)rs["CalcPlanKoef_Name"];
                        objCalcPlanKoef.m_dtDate = System.Convert.ToDateTime(rs["CalcPlanKoef_Date"]);
                        objCalcPlanKoef.m_dtBeginDate = System.Convert.ToDateTime(rs["CalcPlanKoef_BeginDate"]);
                        objCalcPlanKoef.m_dtEndDate = System.Convert.ToDateTime(rs["CalcPlanKoef_EndDate"]);
                        objCalcPlanKoef.m_strDescription = ((rs["CalcPlanKoef_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["CalcPlanKoef_Description"]));

                        objList.Add(objCalcPlanKoef);
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
                "Не удалось получить список расчетов.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Расчет заказа
        /// <summary>
        /// Запускает хранимую процедуру по расчету коэффициента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - успешное завершение операции; false - ошибка</returns>
        public System.Boolean CalcOrder(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, List<ERP_Mercury.Common.CProductOwner> objProductOwnerList, 
            List<ERP_Mercury.Common.CProductType> objProductTypeList, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Int32 iCmdTimeOut = 600;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
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
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                System.Data.DataTable addedProductOwner = new System.Data.DataTable();
                addedProductOwner.Columns.Add(new System.Data.DataColumn("ProductOwner_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedProductOwner.Columns.Add(new System.Data.DataColumn("ProductOwner_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRow = null;
                if ((objProductOwnerList != null) && (objProductOwnerList.Count > 0))
                {
                    foreach (ERP_Mercury.Common.CProductOwner objItem in objProductOwnerList)
                    {
                        if (objItem.uuidID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        newRow = addedProductOwner.NewRow();
                        newRow["ProductOwner_Guid"] = objItem.uuidID;
                        newRow["ProductOwner_Id"] = objItem.Ib_Id;
                        addedProductOwner.Rows.Add(newRow);
                    }
                    addedProductOwner.AcceptChanges();
                }

                System.Data.DataTable addedProductType = new System.Data.DataTable();
                addedProductType.Columns.Add(new System.Data.DataColumn("ProductType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedProductType.Columns.Add(new System.Data.DataColumn("ProductType_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRow2 = null;
                if ((objProductTypeList != null) && (objProductTypeList.Count > 0))
                {
                    foreach (ERP_Mercury.Common.CProductType objItem in objProductTypeList)
                    {
                        if (objItem.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        newRow2 = addedProductType.NewRow();
                        newRow2["ProductType_Guid"] = objItem.ID;
                        newRow2["ProductType_Id"] = objItem.ID_Ib;
                        addedProductType.Rows.Add(newRow2);
                    }
                    addedProductType.AcceptChanges();
                }


                cmd.CommandTimeout = iCmdTimeOut;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetKoefForPlan]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanKoef_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanKoef_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BEGINDATE", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ENDDATE", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanKoef_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanKoef_Description", System.Data.DbType.String));
                cmd.Parameters.AddWithValue("@tProductOwner", addedProductOwner);
                cmd.Parameters["@tProductOwner"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProductOwner"].TypeName = "dbo.udt_ProductOwner";
                cmd.Parameters.AddWithValue("@tProductPartType", addedProductType);
                cmd.Parameters["@tProductPartType"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProductPartType"].TypeName = "dbo.udt_ProductType";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@CalcPlanKoef_Date"].Value = this.m_dtDate;
                cmd.Parameters["@BEGINDATE"].Value = this.m_dtBeginDate;
                cmd.Parameters["@ENDDATE"].Value = this.m_dtEndDate;
                cmd.Parameters["@CalcPlanKoef_Name"].Value = this.m_strNum;
                cmd.Parameters["@CalcPlanKoef_Description"].Value = this.m_strDescription;
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@ERROR_NUM"].Value;
                if (iRes == 0) { this.m_uuidID = ( System.Guid )cmd.Parameters["@CalcPlanKoef_Guid"].Value; }
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                bRet = (iRes == 0);

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось получить сформировать расчет. Текст ошибки : " + f.Message;
            }
            return bRet;
        }

        #endregion

        #region Remove
        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidID">уникальный идентификатор объекта</param>
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCalcPlanKoef]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanKoef_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlanKoef_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления расчета.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить расчет.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion


        public override string ToString()
        {
            return ( m_dtDate.Year.ToString() + " " + m_dtDate.ToShortDateString() + " " + m_strNum + " [" + m_dtBeginDate.ToShortDateString() + " - " +
                m_dtEndDate.ToShortDateString() + "]");
        }


    }

    /// <summary>
    /// Класс "Приложение к расчету коэффициета для плана продаж"
    /// </summary>
    public class CCalcPlanKoefItem
    {
        #region Свойства
        /// <summary>
        /// Товарая марка (код IB)
        /// </summary>
        private System.Int32 m_iProductOwnerIB_Id;
        /// <summary>
        /// Товарая марка (код IB)
        /// </summary>
        public System.Int32 ProductOwnerIB_Id
        {
            get { return m_iProductOwnerIB_Id; }
            set { m_iProductOwnerIB_Id = value; }
        }
        /// <summary>
        /// Товарая марка (наименование)
        /// </summary>
        private System.String m_strProductOwnerName;
        /// <summary>
        /// Товарая марка (наименование)
        /// </summary>
        public System.String ProductOwnerName
        {
            get { return m_strProductOwnerName; }
            set { m_strProductOwnerName = value; }
        }
        /// <summary>
        /// Товарая группа (УИ)
        /// </summary>
        private System.Guid m_iProductTypeId;
        /// <summary>
        /// Товарая группа (УИ)
        /// </summary>
        public System.Guid ProductTypeId
        {
            get { return m_iProductTypeId; }
            set { m_iProductTypeId = value; }
        }
        /// <summary>
        /// Товарая группа (код IB)
        /// </summary>
        private System.Int32 m_iProductTypeIB_Id;
        /// <summary>
        /// Товарая группа (код IB)
        /// </summary>
        public System.Int32 ProductTypeIB_Id
        {
            get { return m_iProductTypeIB_Id; }
            set { m_iProductTypeIB_Id = value; }
        }
        /// <summary>
        /// Товарая группа (наименование)
        /// </summary>
        private System.String m_strProductTypeName;
        /// <summary>
        /// Товарая группа (наименование)
        /// </summary>
        public System.String ProductTypeName
        {
            get { return m_strProductTypeName; }
            set { m_strProductTypeName = value; }
        }
        /// <summary>
        /// Товарая подгруппа (код IB)
        /// </summary>
        private System.Int32 m_iProductSubTypeIB_Id;
        /// <summary>
        /// Товарая подгруппа (код IB)
        /// </summary>
        public System.Int32 ProductSubTypeIB_Id
        {
            get { return m_iProductSubTypeIB_Id; }
            set { m_iProductSubTypeIB_Id = value; }
        }
        /// <summary>
        /// Товарая подгруппа (наименование)
        /// </summary>
        private System.String m_strProductSubTypeName;
        /// <summary>
        /// Товарая подгруппа (наименование)
        /// </summary>
        public System.String ProductSubTypeName
        {
            get { return m_strProductSubTypeName; }
            set { m_strProductSubTypeName = value; }
        }
        /// <summary>
        /// Цена exw
        /// </summary>
        private System.Decimal m_EW_PRICE;
        /// <summary>
        /// Цена exw
        /// </summary>
        public System.Decimal EW_PRICE
        {
            get { return m_EW_PRICE; }
            set { m_EW_PRICE = value; }
        }
        /// <summary>
        /// Сумма exw
        /// </summary>
        private System.Decimal m_SHIP_EW_PRICE;
        /// <summary>
        /// Сумма exw
        /// </summary>
        public System.Decimal SHIP_EW_PRICE
        {
            get { return m_SHIP_EW_PRICE; }
            set { m_SHIP_EW_PRICE = value; }
        }
        /// <summary>
        /// Цена средняя exw
        /// </summary>
        private System.Decimal m_AVG_EW_PRICE;
        /// <summary>
        /// Цена средняя exw
        /// </summary>
        public System.Decimal AVG_EW_PRICE
        {
            get { return m_AVG_EW_PRICE; }
            set { m_AVG_EW_PRICE = value; }
        }
        /// <summary>
        /// Цена опт
        /// </summary>
        private System.Decimal m_PRICE02;
        /// <summary>
        /// Цена опт
        /// </summary>
        public System.Decimal PRICE02
        {
            get { return m_PRICE02; }
            set { m_PRICE02 = value; }
        }
        /// <summary>
        /// Сумма опт
        /// </summary>
        private System.Decimal m_SHIP_PRICE02;
        /// <summary>
        /// Сумма опт
        /// </summary>
        public System.Decimal SHIP_PRICE02
        {
            get { return m_SHIP_PRICE02; }
            set { m_SHIP_PRICE02 = value; }
        }
        /// <summary>
        /// Цена со скидкой
        /// </summary>
        private System.Decimal m_PRICE;
        /// <summary>
        /// Цена со скидкой
        /// </summary>
        public System.Decimal PRICE
        {
            get { return m_PRICE; }
            set { m_PRICE = value; }
        }
        /// <summary>
        /// Сумма реализации
        /// </summary>
        private System.Decimal m_SHIP_PRICE;
        /// <summary>
        /// Сумма реализации
        /// </summary>
        public System.Decimal SHIP_PRICE
        {
            get { return m_SHIP_PRICE; }
            set { m_SHIP_PRICE = value; }
        }
        /// <summary>
        /// Цена реализации средняя
        /// </summary>
        private System.Decimal m_AVG_PRICE;
        /// <summary>
        /// Цена реализации средняя
        /// </summary>
        public System.Decimal AVG_PRICE
        {
            get { return m_AVG_PRICE; }
            set { m_AVG_PRICE = value; }
        }
        /// <summary>
        /// Коэффициент
        /// </summary>
        private System.Decimal m_KOEFF;
        /// <summary>
        /// Коэффициент
        /// </summary>
        public System.Decimal KOEFF
        {
            get { return m_KOEFF; }
            set { m_KOEFF = value; }
        }
        /// <summary>
        /// Количество
        /// </summary>
        private System.Int32 m_QUANTITY;
        /// <summary>
        /// Количество
        /// </summary>
        public System.Int32 QUANTITY
        {
            get { return m_QUANTITY; }
            set { m_QUANTITY = value; }
        }
        #endregion

        #region Конструктор
        public CCalcPlanKoefItem()
        {
            m_AVG_EW_PRICE = 0;
            m_AVG_PRICE = 0;
            m_EW_PRICE = 0;
            m_iProductOwnerIB_Id = 0;
            m_iProductSubTypeIB_Id = 0;
            m_iProductTypeIB_Id = 0;
            m_KOEFF = 0;
            m_PRICE = 0;
            m_PRICE02 = 0;
            m_QUANTITY = 0;
            m_SHIP_EW_PRICE = 0;
            m_SHIP_PRICE = 0;
            m_SHIP_PRICE02 = 0;
            m_strProductOwnerName = "";
            m_strProductSubTypeName = "";
            m_strProductTypeName = "";
            m_iProductTypeId = System.Guid.Empty;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает приложение к расчету коэффициента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список расчетов</returns>
        public static List<CCalcPlanKoefItem> GetCalcOrderItemList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid CalcPlanID, ref System.String strErr )
        {
            List<CCalcPlanKoefItem> objList = new List<CCalcPlanKoefItem>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCalcPlanKoefItems]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanKoef_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlanKoef_Guid"].Value = CalcPlanID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCalcPlanKoefItem objItem = null;

                    while (rs.Read())
                    {
                        objItem = new CCalcPlanKoefItem();
                        objItem.m_strProductOwnerName = ((rs["Owner_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Owner_Name"]));
                        objItem.m_strProductTypeName = ((rs["Parttype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Parttype_Name"]));
                        objItem.m_strProductSubTypeName = ((rs["Partsubtype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Partsubtype_Name"]));
                        objItem.m_iProductOwnerIB_Id = ((rs["Owner_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_Id"]));
                        objItem.m_iProductTypeIB_Id = ((rs["Parttype_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parttype_Id"]));
                        objItem.m_iProductSubTypeIB_Id = ((rs["Partsubtype_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Partsubtype_Id"]));

                        objItem.m_AVG_EW_PRICE = ((rs["AVG_EW_PRICE"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["AVG_EW_PRICE"]));
                        objItem.m_AVG_PRICE = ((rs["AVG_PRICE"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["AVG_PRICE"]));
                        objItem.m_EW_PRICE = ((rs["EW_PRICE"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["EW_PRICE"]));
                        objItem.m_KOEFF = ((rs["KOEFF"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["KOEFF"]));
                        objItem.m_PRICE = ((rs["PRICE"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["PRICE"]));
                        objItem.m_PRICE02 = ((rs["PRICE02"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["PRICE02"]));
                        objItem.m_QUANTITY = ((rs["QUANTITY"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["QUANTITY"]));
                        objItem.m_SHIP_EW_PRICE = ((rs["SHIP_EW_PRICE"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["SHIP_EW_PRICE"]));
                        objItem.m_SHIP_PRICE = ((rs["SHIP_PRICE"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["SHIP_PRICE"]));
                        objItem.m_SHIP_PRICE02 = ((rs["SHIP_PRICE02"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["SHIP_PRICE02"]));

                        objList.Add(objItem);
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
                strErr = System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value);
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить приложение к расчету.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает приложение к расчету коэффициента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CalcPlanID">уи расчета</param>
        /// <param name="ProductOwnerId">уи ТМ</param>
        /// <returns>список расчетов</returns>
        public static List<CCalcPlanKoefItem> GetCalcOrderItemListForProductOwner(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid CalcPlanID, System.Guid ProductOwnerId,  ref System.String strErr)
        {
            List<CCalcPlanKoefItem> objList = new List<CCalcPlanKoefItem>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCalcPlanKoefItemsForProductOwner]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanKoef_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductOwner_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlanKoef_Guid"].Value = CalcPlanID;
                cmd.Parameters["@ProductOwner_Guid"].Value = ProductOwnerId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCalcPlanKoefItem objItem = null;

                    while (rs.Read())
                    {
                        objItem = new CCalcPlanKoefItem();

                        objItem.m_iProductOwnerIB_Id = ((rs["Owner_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_Id"]));
                        objItem.m_strProductTypeName = ((rs["Parttype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Parttype_Name"]));
                        objItem.m_iProductTypeIB_Id = ((rs["Parttype_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parttype_Id"]));
                        objItem.m_iProductTypeId = ((rs["PartType_Guid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["PartType_Guid"] );

                        objItem.m_AVG_EW_PRICE = ((rs["AVG_EW_PRICE"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["AVG_EW_PRICE"]));
                        objItem.m_KOEFF = ((rs["KOEFF"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["KOEFF"]));

                        objList.Add(objItem);
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
                strErr = System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value);
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить приложение к расчету.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

    }

}
