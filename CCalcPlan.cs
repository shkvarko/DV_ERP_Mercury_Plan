using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPMercuryPlan
{
    public enum enumAction
    {
        Unkown = -1,
        RefreshBtnClick = 0,
        AddBtnClick = 1,
        EditBtnClick = 2,
        DeleteBtnClick = 3,
        SaveBtnClick = 4,
        CancelBtnClick = 5,
        ObjectDeleted = 6,
        ObjectSelected = 7,
        ObjectChanged = 8
    }


    #region Месяцы
    public enum enMonth
    {
        UnKown = 0,
        January = 1,
        Febrary = 2,
        March = 3,
        April = 4, 
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        Ocntober = 10,
        November = 11,
        December = 12,
        Total = 13
    }
    #endregion

    public class CCalcPlanItem
    {
        #region Свойства
        /// <summary>
        /// Месяц
        /// </summary>
        private enMonth m_iMonth;
        /// <summary>
        /// Месяц
        /// </summary>
        public enMonth Month
        {
            get { return m_iMonth; }
            set { m_iMonth = value; }
        }
        /// <summary>
        /// Количество
        /// </summary>
        private System.Int32 m_iQuantity;
        /// <summary>
        /// Количество
        /// </summary>
        public System.Int32 Quantity
        {
            get { return m_iQuantity; }
            set { m_iQuantity = value; }
        }
        /// <summary>
        /// Сумма
        /// </summary>
        private System.Decimal m_AllMoney;
        /// <summary>
        /// Сумма
        /// </summary>
        public System.Decimal AllMoney
        {
            get { return m_AllMoney; }
            set { m_AllMoney = value; }
        }
        #endregion

        #region Конструктор
        public CCalcPlanItem()
        {
            m_iMonth = enMonth.UnKown;
            m_iQuantity = 0;
            m_AllMoney = 0;
        }
        public CCalcPlanItem(enMonth iMonth, System.Int32 iQuantity, System.Decimal dcmlAllMoney)
        {
            m_iMonth = iMonth;
            m_iQuantity = iQuantity;
            m_AllMoney = dcmlAllMoney;
        }
        #endregion
    }

    public class CCalcPlanItemProductType
    {
        #region Свойства
        /// <summary>
        /// Товарная группа
        /// </summary>
        private ERP_Mercury.Common.CProductType m_objProductType;
        /// <summary>
        /// Товарная группа
        /// </summary>
        public ERP_Mercury.Common.CProductType ProductType
        {
            get { return m_objProductType; }
            set { m_objProductType = value; }
        }
        /// <summary>
        /// Средняя цена EXW
        /// </summary>
        private System.Decimal m_PriceAvgEXW;
        /// <summary>
        /// Средняя цена EXW
        /// </summary>
        public System.Decimal PriceAvgEXW
        {
            get { return m_PriceAvgEXW; }
            set { m_PriceAvgEXW = value; }
        }
        /// <summary>
        /// Средняя цена EXW
        /// </summary>
        private System.Decimal m_KoeffPlan;
        /// <summary>
        /// Средняя цена EXW
        /// </summary>
        public System.Decimal KoeffPlan
        {
            get { return m_KoeffPlan; }
            set { m_KoeffPlan = value; }
        }
        /// <summary>
        /// Список расшифровок по группе
        /// </summary>
        private List<CCalcPlanItem> m_objCalcPlanItemList;
        /// <summary>
        /// Список расшифровок по группе
        /// </summary>
        public List<CCalcPlanItem> CalcPlanItemList
        {
            get { return m_objCalcPlanItemList; }
            set { m_objCalcPlanItemList = value; }
        }
        /// <summary>
        /// Количество по группе
        /// </summary>
        public System.Int32 Quantity
        {
            get { return GetQuantity(); }
        }
        private System.Int32 GetQuantity()
        {
            System.Int32 iRet = 0;
            try
            {
                if( m_objCalcPlanItemList != null )
                {
                    foreach (CCalcPlanItem objItem in m_objCalcPlanItemList)
                    {
                        if (objItem.Month == enMonth.Total)
                        {
                            iRet = objItem.Quantity;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить количество для группы.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return iRet;
        }
        /// <summary>
        /// Сумма по группе
        /// </summary>
        public System.Decimal AllMoney
        {
            get { return GetAllMoney(); }
        }
        private System.Decimal GetAllMoney()
        {
            System.Decimal dcmlRet = 0;
            try
            {
                if (m_objCalcPlanItemList != null)
                {
                    foreach (CCalcPlanItem objItem in m_objCalcPlanItemList)
                    {
                        if (objItem.Month == enMonth.Total)
                        {
                            dcmlRet = objItem.AllMoney;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить сумму для группы.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dcmlRet;
        }
        /// <summary>
        /// Возвращает запланированное количество для указанного месяца
        /// </summary>
        /// <param name="iMonth">номер месяца</param>
        /// <returns>запланированное количество</returns>
        public System.Int32 GetPlanQty(enMonth iMonth)
        {
            System.Int32 iQty = 0;
            try
            {
                if (m_objCalcPlanItemList != null)
                {
                    foreach (CCalcPlanItem objItem in m_objCalcPlanItemList)
                    {
                        if (objItem.Month == iMonth)
                        {
                            iQty = objItem.Quantity;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить количество.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return iQty;
        }
        /// <summary>
        /// Возвращает запланированную сумму для указанного месяца
        /// </summary>
        /// <param name="iMonth">номер месяца</param>
        /// <returns>запланированная сумма</returns>
        public System.Decimal GetPlanAllMoney(enMonth iMonth)
        {
            System.Decimal dcmlAllMoney = 0;
            try
            {
                if (m_objCalcPlanItemList != null)
                {
                    foreach (CCalcPlanItem objItem in m_objCalcPlanItemList)
                    {
                        if (objItem.Month == iMonth)
                        {
                            dcmlAllMoney = objItem.AllMoney;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить сумму.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dcmlAllMoney;
        }
        #endregion

        #region Конструктор
        public CCalcPlanItemProductType()
        {
            m_objProductType = null;
            m_KoeffPlan = 0;
            m_PriceAvgEXW = 0;
            m_objCalcPlanItemList = null;
        }
        public CCalcPlanItemProductType(ERP_Mercury.Common.CProductType objProductType, System.Decimal dcmlKoeffPlan,
           System.Decimal dcmlPriceAvgEXW)
        {
            m_objProductType = objProductType;
            m_KoeffPlan = dcmlKoeffPlan;
            m_PriceAvgEXW = dcmlPriceAvgEXW;
            m_objCalcPlanItemList = null;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товарных групп для товарной марки
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список товарных групп для товарной марки</returns>
        public static List<CCalcPlanItemProductType> GetCalcPlanItemProductTypeList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CCalcPlan objCalcPlan, ERP_Mercury.Common.CProductOwner objProductOwner)
        {
            List<CCalcPlanItemProductType> objList = new List<CCalcPlanItemProductType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCalcPlanItem]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlan_Guid"].Value = objCalcPlan.ID;
                cmd.Parameters["@Owner_Guid"].Value = objProductOwner.uuidID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCalcPlanItemProductType objCalcPlanItemProductType = null;
                    System.Int32 iPartType = 0;

                    while (rs.Read())
                    {
                        if (System.Convert.ToInt32(rs["Parttype_Id"]) != iPartType)
                        {
                            objCalcPlanItemProductType = new CCalcPlanItemProductType();
                            objCalcPlanItemProductType.m_objProductType = new ERP_Mercury.Common.CProductType((System.Guid)rs["PartType_Guid"], System.Convert.ToString(rs["Parttype_Name"]), System.Convert.ToInt32(rs["Parttype_Id"]), System.Convert.ToString(rs["Parttype_DemandsName"]), System.Convert.ToDouble(rs["Parttype_NDSRate"]), "", System.Convert.ToBoolean(rs["Parttype_IsActive"]));
                            objCalcPlanItemProductType.m_KoeffPlan = System.Convert.ToDecimal(rs["CalcPlan_Koeff"]);
                            objCalcPlanItemProductType.m_PriceAvgEXW = System.Convert.ToDecimal(rs["CalcPlan_PriceAvgEXW"]);
                            objCalcPlanItemProductType.m_objCalcPlanItemList = new List<CCalcPlanItem>();

                            iPartType = System.Convert.ToInt32(rs["Parttype_Id"]);

                            objList.Add(objCalcPlanItemProductType);
                        }

                        objCalcPlanItemProductType.m_objCalcPlanItemList.Add(new CCalcPlanItem((enMonth)rs["MonthId"], System.Convert.ToInt32(rs["CalcPlan_Quantity"]), System.Convert.ToDecimal(rs["CalcPlan_AllPrice"])));
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
                "Не удалось получить список товарных групп для товарной марки.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

    }

    public class CCalcPlanItemProductOwner
    {
        #region Свойства
        /// <summary>
        /// Товарная марка
        /// </summary>
        private ERP_Mercury.Common.CProductOwner m_objProductOwner;
        /// <summary>
        /// Товарная марка
        /// </summary>
        public ERP_Mercury.Common.CProductOwner ProductOwner
        {
            get { return m_objProductOwner; }
            set { m_objProductOwner = value; }
        }
        /// <summary>
        /// Товарные группы с расшифровками
        /// </summary>
        private List<CCalcPlanItemProductType> m_objCalcPlanItemProductTypeList;
        /// <summary>
        /// Товарные группы с расшифровками
        /// </summary>
        public List<CCalcPlanItemProductType> CalcPlanItemProductTypeList
        {
            get { return m_objCalcPlanItemProductTypeList; }
            set { m_objCalcPlanItemProductTypeList = value; }
        }
        /// <summary>
        /// Количество по товарной марке
        /// </summary>
        private System.Int32 m_iQuantity;
        /// <summary>
        /// Количество по товарной марке
        /// </summary>
        public System.Int32 Quantity
        {
            get { return m_iQuantity; }
            set { m_iQuantity = value; }
        }
        /// <summary>
        /// Сумма по товарной марке
        /// </summary>
        private System.Decimal m_AllMoney;
        /// <summary>
        /// Сумма по товарной марке
        /// </summary>
        public System.Decimal AllMoney
        {
            get { return m_AllMoney; }
            set { m_AllMoney = value; }
        }
        /// <summary>
        /// Пересчет итоговых сумм
        /// </summary>
        public void RecalcQuantityAndMoney()
        {
            System.Int32 iRet = 0;
            System.Decimal dcmlRet = 0;
            try
            {
                if (m_objCalcPlanItemProductTypeList != null)
                {
                    foreach (CCalcPlanItemProductType objItem in m_objCalcPlanItemProductTypeList)
                    {
                        iRet += objItem.Quantity;
                        dcmlRet += objItem.AllMoney;
                    }
                }
                m_iQuantity = iRet;
                m_AllMoney = dcmlRet;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить количество для товарной марки.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return ;
        }
        #endregion

        #region Конструктор
        public CCalcPlanItemProductOwner()
        {
            m_objProductOwner = null;
            m_objCalcPlanItemProductTypeList = null;
            m_iQuantity = 0;
            m_AllMoney = 0;
        }
        public CCalcPlanItemProductOwner(ERP_Mercury.Common.CProductOwner objProductOwner, System.Int32 iQuantity, 
            System.Decimal dcmlAllMoney )
        {
            m_objProductOwner = objProductOwner;
            m_objCalcPlanItemProductTypeList = null;
            m_iQuantity = iQuantity;
            m_AllMoney = dcmlAllMoney;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товарных марок для плана
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список товарных марок для плана</returns>
        public static List<CCalcPlanItemProductOwner> GetCalcPlanProductOwnerList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CCalcPlan objCalcPlan )
        {
            List<CCalcPlanItemProductOwner> objList = new List<CCalcPlanItemProductOwner>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCalcPlanOwner]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlan_Guid"].Value = objCalcPlan.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCalcPlanItemProductOwner objCalcPlanItemProductOwner = null;

                    while (rs.Read())
                    {

                        objCalcPlanItemProductOwner = new CCalcPlanItemProductOwner();
                        objCalcPlanItemProductOwner.m_objProductOwner = new ERP_Mercury.Common.CProductOwner((System.Guid)rs["Owner_Guid"], (System.String)rs["Owner_Name"], System.Convert.ToBoolean(rs["Owner_IsActive"]));
                        objCalcPlanItemProductOwner.m_objProductOwner.Ib_Id = System.Convert.ToInt32(rs["Owner_Id"]); 
                        objCalcPlanItemProductOwner.m_iQuantity = System.Convert.ToInt32(rs["PlanOwnerQuantity"]);
                        objCalcPlanItemProductOwner.m_AllMoney = System.Convert.ToDecimal(rs["PlanOwnerAllMoney"]);

                        objList.Add(objCalcPlanItemProductOwner);
                    }
                }
                rs.Close();
                rs.Dispose();

                foreach (CCalcPlanItemProductOwner objItem in objList)
                {
                    objItem.m_objCalcPlanItemProductTypeList = CCalcPlanItemProductType.GetCalcPlanItemProductTypeList(objProfile, cmd, objCalcPlan, objItem.ProductOwner);
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товарных марок для плана.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Привязка расшифровок к плану в БД
        /// <summary>
        /// Сохраняет в БД информацию о списке водителей у перевозчика
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCalcPlan">план</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean SaveCCalcPlanItemProductTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            CCalcPlan objCalcPlan, ref System.String strErr)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("PartType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("MonthId", typeof(System.Data.SqlTypes.SqlInt32)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CalcPlan_PriceAvgEXW", typeof(System.Data.SqlTypes.SqlMoney)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CalcPlan_Koeff", typeof(System.Data.SqlTypes.SqlMoney)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CalcPlan_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CalcPlan_AllPrice", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRow = null;
                if ((m_objCalcPlanItemProductTypeList != null) && (m_objCalcPlanItemProductTypeList.Count > 0))
                {
                    foreach (CCalcPlanItemProductType objTypeItem in m_objCalcPlanItemProductTypeList)
                    {
                        if ((objTypeItem.CalcPlanItemList == null) || (objTypeItem.CalcPlanItemList.Count == 0)) { continue; }
                        foreach (CCalcPlanItem objItem in objTypeItem.CalcPlanItemList)
                        {
                            if ((objItem.Month == enMonth.UnKown) || (objItem.Month == enMonth.Total)) { continue; }
                            newRow = addedCategories.NewRow();
                            newRow["PartType_Guid"] = objTypeItem.ProductType.ID;
                            newRow["MonthId"] = objItem.Month;
                            newRow["CalcPlan_PriceAvgEXW"] = objTypeItem.PriceAvgEXW;
                            newRow["CalcPlan_Koeff"] = objTypeItem.KoeffPlan;
                            newRow["CalcPlan_Quantity"] = objItem.Quantity;
                            newRow["CalcPlan_AllPrice"] = objItem.AllMoney;
                            addedCategories.Rows.Add(newRow);
                        }
                    }
                }
                addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCalcPlanItem]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tCalcPlanItem", addedCategories);
                cmd.Parameters["@tCalcPlanItem"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tCalcPlanItem"].TypeName = "dbo.udt_CalcPlanItem";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlan_Guid"].Value = objCalcPlan.ID;
                cmd.Parameters["@Owner_Guid"].Value = this.ProductOwner.uuidID;
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

        #region Удаление расшифровки из БД
        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidID">уникальный идентификатор объекта</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(UniXP.Common.CProfile objProfile, CCalcPlan objCalcPlan)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCalcPlan]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlan_Guid"].Value = objCalcPlan.ID;
                cmd.Parameters["@Owner_Guid"].Value = this.ProductOwner.uuidID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления расшифровок для товарной марки.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить расшифровки для товарной марки.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    public class CCalcPlan
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_ID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        /// <summary>
        /// Номер
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// Номер
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        /// <summary>
        /// Дата
        /// </summary>
        private System.DateTime m_Date;
        /// <summary>
        /// Дата
        /// </summary>
        public System.DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }
        /// <summary>
        /// Год
        /// </summary>
        private System.Int32 m_iYear;
        /// <summary>
        /// Год
        /// </summary>
        public System.Int32 Year
        {
            get { return m_iYear; }
            set { m_iYear = value; }
        }
        /// <summary>
        /// Использвать в отчетах
        /// </summary>
        private System.Boolean m_bUseForReport;
        /// <summary>
        /// Использвать в отчетах
        /// </summary>
        public System.Boolean UseForReport
        {
            get { return m_bUseForReport; }
            set { m_bUseForReport = value; }
        }
        /// <summary>
        /// Количество по году
        /// </summary>
        private System.Int32 m_iQuantity;
        /// <summary>
        /// Количество по году
        /// </summary>
        public System.Int32 Quantity
        {
            get { return m_iQuantity; }
            set { m_iQuantity = value; }
        }
        /// <summary>
        /// Сумма по году
        /// </summary>
        private System.Decimal m_AllMoney;
        /// <summary>
        /// Сумма по году
        /// </summary>
        public System.Decimal AllMoney
        {
            get { return m_AllMoney; }
            set { m_AllMoney = value; }
        }
        /// <summary>
        /// Список расшифровок по товарным маркам
        /// </summary>
        private List<CCalcPlanItemProductOwner> m_objCalcPlanItemProductOwner;
        /// <summary>
        /// Список расшифровок по товарным маркам
        /// </summary>
        public List<CCalcPlanItemProductOwner> CalcPlanItemProductOwner
        {
            get { return m_objCalcPlanItemProductOwner; }
            set { m_objCalcPlanItemProductOwner = value; }
        }
        /// <summary>
        /// Пересчет итоговых сумм
        /// </summary>
        public void RecalcQuantityAndMoney()
        {
            System.Int32 iRet = 0;
            System.Decimal dcmlRet = 0;
            try
            {
                if (m_objCalcPlanItemProductOwner != null)
                {
                    foreach (CCalcPlanItemProductOwner objItem in m_objCalcPlanItemProductOwner)
                    {
                        iRet += objItem.Quantity;
                        dcmlRet += objItem.AllMoney;
                    }
                }
                m_iQuantity = iRet;
                m_AllMoney = dcmlRet;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось пересчитать итоговые суммы для года.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Валюта
        /// </summary>
        private ERP_Mercury.Common.CCurrency m_objCurrency;
        /// <summary>
        /// Валюта
        /// </summary>
        public ERP_Mercury.Common.CCurrency Currency
        {
            get { return m_objCurrency; }
            set { m_objCurrency = value; }
        }
        public System.String CurrencyCode
        {
            get { return (m_objCurrency == null) ? "" : m_objCurrency.CurrencyAbbr; }
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
        #endregion

        #region Конструктор
        public CCalcPlan()
        {
            m_ID = System.Guid.Empty;
            m_Date = System.DateTime.MinValue;
            m_strName = "";
            m_iYear = System.DateTime.MinValue.Year;
            m_iQuantity = 0;
            m_AllMoney = 0;
            m_bUseForReport = false;
            m_objCalcPlanItemProductOwner = null;
            m_objCurrency = null;
            m_strDescription = "";
        }
        public CCalcPlan(System.Guid mID, System.DateTime mDate, System.String strName, System.Int32 iYear,
            System.Int32 iQuantity, System.Decimal mAllMoney, System.Boolean bUseForReport, ERP_Mercury.Common.CCurrency objCurrency,
            System.String strDescription)
        {
            m_ID = mID;
            m_Date = mDate;
            m_strName = strName;
            m_iYear = iYear;
            m_iQuantity = iQuantity;
            m_AllMoney = mAllMoney;
            m_bUseForReport = bUseForReport;
            m_objCalcPlanItemProductOwner = null;
            m_objCurrency = objCurrency;
            m_strDescription = strDescription;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список планов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список планов</returns>
        public static List<CCalcPlan> GetCalcPlanList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCalcPlan> objList = new List<CCalcPlan>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCalcPlan]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCalcPlan objCalcPlan = null;

                    while (rs.Read())
                    {

                        objCalcPlan = new CCalcPlan();
                        objCalcPlan.m_ID = (System.Guid)rs["CalcPlan_Guid"];
                        objCalcPlan.m_strName = (System.String)rs["CalcPlan_Name"];
                        objCalcPlan.m_Date = System.Convert.ToDateTime(rs["CalcPlan_Date"]);
                        objCalcPlan.m_iYear = System.Convert.ToInt32(rs["CalcPlan_Year"]);
                        objCalcPlan.m_bUseForReport = System.Convert.ToBoolean(rs["CalcPlan_IsUseForReport"]);
                        objCalcPlan.m_iQuantity = System.Convert.ToInt32(rs["CalcPlanQuantity"]);
                        objCalcPlan.m_AllMoney = System.Convert.ToDecimal(rs["CalcPlanAllMoney"]);
                        objCalcPlan.m_objCurrency = new ERP_Mercury.Common.CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"], (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        objCalcPlan.Description = ((rs["CalcPlan_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["CalcPlan_Description"]));
                        objList.Add(objCalcPlan);
                    }
                }
                rs.Close();
                rs.Dispose();

                foreach (CCalcPlan objItem in objList)
                {
                    objItem.m_objCalcPlanItemProductOwner = CCalcPlanItemProductOwner.GetCalcPlanProductOwnerList(objProfile, cmd, objItem);
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список планов.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
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
                    "Необходимо указать номер!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Currency == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать валюту!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_iYear < System.DateTime.Today.Year )
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать год!", "Внимание",
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
        public System.Boolean Add(UniXP.Common.CProfile objProfile)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCalcPlan]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Year", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_IsUseForReport", System.Data.SqlDbType.Bit));
                if (this.m_strDescription != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Description", System.Data.DbType.String));
                    cmd.Parameters["@CalcPlan_Description"].Value = this.m_strDescription;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlan_Name"].Value = this.Name;
                cmd.Parameters["@Currency_Guid"].Value = this.m_objCurrency.ID;
                cmd.Parameters["@CalcPlan_Year"].Value = this.m_iYear;
                cmd.Parameters["@CalcPlan_Date"].Value = this.m_Date;
                cmd.Parameters["@CalcPlan_IsUseForReport"].Value = this.m_bUseForReport;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@CalcPlan_Guid"].Value;

                    if ((this.m_objCalcPlanItemProductOwner != null) && (this.m_objCalcPlanItemProductOwner.Count > 0))
                    {
                        // собственно сохраняем расшифровки
                        foreach (CCalcPlanItemProductOwner objItem in this.m_objCalcPlanItemProductOwner)
                        {
                            if (objItem.SaveCCalcPlanItemProductTypeList(objProfile, cmd, this, ref strErr) == false)
                            {
                                iRes = -1;
                                break;
                            }
                        }
                    }

                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания записи о расчете плана.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать запись о расчете плана.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
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
        public System.Boolean Update(UniXP.Common.CProfile objProfile)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCalcPlan]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Year", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_IsUseForReport", System.Data.SqlDbType.Bit));
                if (this.m_strDescription != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Description", System.Data.DbType.String));
                    cmd.Parameters["@CalcPlan_Description"].Value = this.m_strDescription;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlan_Guid"].Value = this.ID;
                cmd.Parameters["@CalcPlan_Name"].Value = this.Name;
                cmd.Parameters["@Currency_Guid"].Value = this.m_objCurrency.ID;
                cmd.Parameters["@CalcPlan_Year"].Value = this.m_iYear;
                cmd.Parameters["@CalcPlan_Date"].Value = this.m_Date;
                cmd.Parameters["@CalcPlan_IsUseForReport"].Value = this.m_bUseForReport;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    if ((this.m_objCalcPlanItemProductOwner != null) && (this.m_objCalcPlanItemProductOwner.Count > 0))
                    {
                        // собственно сохраняем расшифровки
                        foreach (CCalcPlanItemProductOwner objItem in this.m_objCalcPlanItemProductOwner)
                        {
                            if (objItem.SaveCCalcPlanItemProductTypeList(objProfile, cmd, this, ref strErr) == false)
                            {
                                iRes = -1;
                                break;
                            }
                        }
                    }
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств записи о расчете плана.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства записи о расчете плана.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCalcPlan]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CalcPlan_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления записи о расчете плана.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить запись о расчете плана.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Копирование
        /// <summary>
        /// Копирование расчета плана
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objSRCPlan">исходный расчет плана</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean Copy(UniXP.Common.CProfile objProfile, CCalcPlan objSRCPlan )
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CopyCalcPlan]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SRCCalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SRCCalcPlan_Guid"].Value = objSRCPlan.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось скопировать расчет плана.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось скопировать расчет плана.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            return (String.Format("{0} год {1} {2}", Name, Year, ((UseForReport == true) ? " для отчетов" : "")));
        }

    }
}
