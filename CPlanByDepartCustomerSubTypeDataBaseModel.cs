using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPMercuryPlan
{
    class CPlanByDepartCustomerSubTypeDataBaseModel
    {
        /// <summary>
        /// Возвращает таблицу с планами продаж
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="dtBeginDate">начало периода плана</param>
        /// <param name="dtEndDate">конец периода плана</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>таблица</returns>
        public static System.Data.DataTable GetPlanByDepartCustomerSubTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime dtBeginDate, System.DateTime dtEndDate,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Data.DataTable dtPlan = new System.Data.DataTable();

            dtPlan.Columns.Add(new System.Data.DataColumn("CalcPlanByDepartCustomerSubType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Plan_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Plan_Date", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Plan_BeginDate", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Plan_EndDate", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Plan_Description", typeof(System.Data.SqlTypes.SqlString)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Plan_IsUseForReport", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtPlan.Columns.Add(new System.Data.DataColumn("CalcPlan_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtPlan.Columns.Add(new System.Data.DataColumn("CalcPlan_AllPrice", typeof(System.Data.SqlTypes.SqlMoney)));

            dtPlan.Columns.Add(new System.Data.DataColumn("Currency_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Currency_Abbr", typeof(System.Data.SqlTypes.SqlString)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Currency_Code", typeof(System.Data.SqlTypes.SqlString)));
            dtPlan.Columns.Add(new System.Data.DataColumn("Currency_ShortName", typeof(System.Data.SqlTypes.SqlString)));

            dtPlan.Columns.Add(new System.Data.DataColumn("CalcPlan_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlan.Columns.Add(new System.Data.DataColumn("CalcPlan_Year", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlan.Columns.Add(new System.Data.DataColumn("CalcPlan_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtPlan.Columns.Add(new System.Data.DataColumn("CalcPlan_Date", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtPlan.Columns.Add(new System.Data.DataColumn("CalcPlan_IsUseForReport", typeof(System.Data.SqlTypes.SqlBoolean)));

            dtPlan.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlan.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtPlan.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Date", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtPlan.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_BeginDate", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtPlan.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_EndDate", typeof(System.Data.SqlTypes.SqlDateTime)));

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
                        return dtPlan;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCalcPlanByDepartCustomerSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                cmd.Parameters["@EndDate"].Value = dtEndDate;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;

                    while (rs.Read())
                    {

                        newRow = dtPlan.NewRow();
                        newRow["CalcPlanByDepartCustomerSubType_Guid"] = (System.Guid)rs["CalcPlanByDepartCustomerSubType_Guid"];
                        newRow["Plan_Name"] = ((rs["Plan_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Plan_Name"]));
                        if (rs["Plan_Date"] != System.DBNull.Value)
                        {
                            newRow["Plan_Date"] = System.Convert.ToDateTime(rs["Plan_Date"]); 
                        }
                        if (rs["Plan_BeginDate"] != System.DBNull.Value)
                        {
                            newRow["Plan_BeginDate"] = System.Convert.ToDateTime(rs["Plan_BeginDate"]);
                        }
                        if (rs["Plan_EndDate"] != System.DBNull.Value)
                        {
                            newRow["Plan_EndDate"] = System.Convert.ToDateTime(rs["Plan_EndDate"]);
                        }
                        newRow["Plan_Description"] = ((rs["Plan_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Plan_Description"]));
                        newRow["Plan_IsUseForReport"] = ((rs["Plan_IsUseForReport"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Plan_IsUseForReport"]));
                        if (rs["Currency_Guid"] != System.DBNull.Value)
                        {
                            newRow["Currency_Guid"] = (System.Guid)rs["Currency_Guid"];
                        }
                        newRow["Currency_Abbr"] = ((rs["Currency_Abbr"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Currency_Abbr"]));
                        newRow["Currency_Code"] = ((rs["Currency_Code"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Currency_Code"]));
                        newRow["Currency_ShortName"] = ((rs["Currency_ShortName"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Currency_ShortName"]));

                        if (rs["CalcPlan_Guid"] != System.DBNull.Value)
                        {
                            newRow["CalcPlan_Guid"] = (System.Guid)rs["CalcPlan_Guid"];
                        }
                        newRow["CalcPlan_Year"] = ((rs["CalcPlan_Year"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["CalcPlan_Year"]));
                        newRow["CalcPlan_Name"] = ((rs["CalcPlan_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["CalcPlan_Name"]));
                        if (rs["CalcPlan_Date"] != System.DBNull.Value)
                        {
                            newRow["CalcPlan_Date"] = System.Convert.ToDateTime(rs["CalcPlan_Date"]);
                        }
                        newRow["CalcPlan_IsUseForReport"] = ((rs["CalcPlan_IsUseForReport"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["CalcPlan_IsUseForReport"]));


                        if (rs["SalesPlanQuota_Guid"] != System.DBNull.Value)
                        {
                            newRow["SalesPlanQuota_Guid"] = (System.Guid)rs["SalesPlanQuota_Guid"];
                        }
                        newRow["SalesPlanQuota_Name"] = ((rs["SalesPlanQuota_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["SalesPlanQuota_Name"]));
                        if (rs["SalesPlanQuota_Date"] != System.DBNull.Value)
                        {
                            newRow["SalesPlanQuota_Date"] = System.Convert.ToDateTime(rs["SalesPlanQuota_Date"]);
                        }
                        if (rs["SalesPlanQuota_BeginDate"] != System.DBNull.Value)
                        {
                            newRow["SalesPlanQuota_BeginDate"] = System.Convert.ToDateTime(rs["SalesPlanQuota_BeginDate"]);
                        }
                        if (rs["SalesPlanQuota_EndDate"] != System.DBNull.Value)
                        {
                            newRow["SalesPlanQuota_EndDate"] = System.Convert.ToDateTime(rs["SalesPlanQuota_EndDate"]);
                        }
                        newRow["CalcPlan_Quantity"] = System.Convert.ToDecimal(rs["CalcPlan_Quantity"]);
                        newRow["CalcPlan_AllPrice"] = System.Convert.ToDecimal(rs["CalcPlan_AllPrice"]);

                        dtPlan.Rows.Add(newRow);
                    }

                    dtPlan.AcceptChanges();
                }

                strErr = System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value);
                iRes = System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value);

                rs.Close();
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось получить список планов.\n\nТекст ошибки :" + f.Message);
            }
            return dtPlan;
        }

        /// <summary>
        /// Возвращает таблицу с приложением к плану продаж
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="PlanID">УИ плана продаж</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>таблица</returns>
        public static System.Data.DataTable GetPlanByDepartCustomerSubTypeItemList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL,  System.Guid PlanID, 
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Data.DataTable dtPlanItem = new System.Data.DataTable();

            dtPlanItem.Columns.Add(new System.Data.DataColumn("CalcPlanByDepartCustomerSubTypeItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("CalcPlanByDepartCustomerSubType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Owner_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Owner_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Owner_Name", typeof(System.Data.SqlTypes.SqlString)));

            dtPlanItem.Columns.Add(new System.Data.DataColumn("PartType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Parttype_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Parttype_Name", typeof(System.Data.SqlTypes.SqlString)));

            dtPlanItem.Columns.Add(new System.Data.DataColumn("DepartTeam_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("DepartTeam_Name", typeof(System.Data.SqlTypes.SqlString)));

            dtPlanItem.Columns.Add(new System.Data.DataColumn("Depart_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Depart_Code", typeof(System.Data.SqlTypes.SqlString)));

            dtPlanItem.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Customer_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Customer_Name", typeof(System.Data.SqlTypes.SqlString)));

            dtPlanItem.Columns.Add(new System.Data.DataColumn("PartSubType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Partsubtype_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Partsubtype_Name", typeof(System.Data.SqlTypes.SqlString)));

            dtPlanItem.Columns.Add(new System.Data.DataColumn("CalcPlan_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("CalcPlan_AllPrice", typeof(System.Data.SqlTypes.SqlMoney)));

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
                        return dtPlanItem;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCalcPlanByDepartCustomerSubTypeItem]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanByDepartCustomerSubType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@CalcPlanByDepartCustomerSubType_Guid"].Value = PlanID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;

                    while (rs.Read())
                    {

                        newRow = dtPlanItem.NewRow();
                        newRow["CalcPlanByDepartCustomerSubTypeItem_Guid"] = (System.Guid)rs["CalcPlanByDepartCustomerSubTypeItem_Guid"];
                        newRow["CalcPlanByDepartCustomerSubType_Guid"] = (System.Guid)rs["CalcPlanByDepartCustomerSubType_Guid"];
                        if (rs["Owner_Guid"] != System.DBNull.Value)
                        {
                            newRow["Owner_Guid"] = (System.Guid)rs["Owner_Guid"];
                            newRow["Owner_Id"] = System.Convert.ToInt32(rs["Owner_Id"]);
                            newRow["Owner_Name"] = ((rs["Owner_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Owner_Name"]));
                        }
                        if (rs["PartType_Guid"] != System.DBNull.Value)
                        {
                            newRow["PartType_Guid"] = (System.Guid)rs["PartType_Guid"];
                            newRow["Parttype_Id"] = System.Convert.ToInt32(rs["Parttype_Id"]);
                            newRow["Parttype_Name"] = ((rs["Parttype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Parttype_Name"]));
                        }
                        newRow["CalcPlan_Quantity"] = System.Convert.ToDecimal(rs["CalcPlan_Quantity"]);
                        newRow["CalcPlan_AllPrice"] = System.Convert.ToDecimal(rs["CalcPlan_AllPrice"]);
                        if (rs["DepartTeam_Guid"] != System.DBNull.Value)
                        {
                            newRow["DepartTeam_Guid"] = (System.Guid)rs["DepartTeam_Guid"];
                            newRow["DepartTeam_Name"] = ((rs["DepartTeam_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["DepartTeam_Name"]));
                        }
                        if (rs["Depart_Guid"] != System.DBNull.Value)
                        {
                            newRow["Depart_Guid"] = (System.Guid)rs["Depart_Guid"];
                            newRow["Depart_Code"] = ((rs["Depart_Code"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Depart_Code"]));
                        }
                        if (rs["Customer_Guid"] != System.DBNull.Value)
                        {
                            newRow["Customer_Guid"] = (System.Guid)rs["Customer_Guid"];
                            newRow["Customer_Id"] = System.Convert.ToInt32(rs["Customer_Id"]);
                            newRow["Customer_Name"] = ((rs["Customer_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Customer_Name"]));
                        }
                        if (rs["PartSubType_Guid"] != System.DBNull.Value)
                        {
                            newRow["PartSubType_Guid"] = (System.Guid)rs["PartSubType_Guid"];
                            newRow["Partsubtype_Id"] = System.Convert.ToInt32(rs["Partsubtype_Id"]);
                            newRow["Partsubtype_Name"] = ((rs["Partsubtype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Partsubtype_Name"]));
                        }

                        dtPlanItem.Rows.Add(newRow);
                    }

                    dtPlanItem.AcceptChanges();
                }

                strErr = System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value);
                iRes = System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value);

                rs.Close();
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
                "Не удалось получить список приложения к плану.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dtPlanItem;
        }

        /// <summary>
        /// Сохраняет в БД приложение к плану
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="PlanID">УИ плана</param>
        /// <param name="dtPlanItemList">таблица с приложением к плану</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddPlanItemDecodeToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid PlanID, System.Data.DataTable dtPlanItemList,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (dtPlanItemList == null) { return bRet; }

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

                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCalcPlanByDepartCustomerSubTypeItemList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanByDepartCustomerSubType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@t_CalcPlanByDepartCustomerSubTypeItem", dtPlanItemList);
                cmd.Parameters["@t_CalcPlanByDepartCustomerSubTypeItem"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@t_CalcPlanByDepartCustomerSubTypeItem"].TypeName = "dbo.udt_CalcPlanByDepartCustomerSubTypeItem";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@CalcPlanByDepartCustomerSubType_Guid"].Value = PlanID;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
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
        /// Добавляет в БД информацию о плане продаж
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CalcPlanByDepartCustomerSubType_Guid">УИ доли продаж</param>
        /// <param name="CalcPlan_Guid">уи плана по марке и группе</param>
        /// <param name="Plan_Name">наименование</param>
        /// <param name="Plan_BeginDate">начало периода плана</param>
        /// <param name="Plan_EndDate">конец периода плана</param>
        /// <param name="Plan_Date">дата регистрации плана</param>
        /// <param name="Currency_Guid">УИ валюты</param>
        /// <param name="Plan_Description">примечание</param>
        /// <param name="Plan_IsUseForReport">признак "использовать для отчётов"</param>
        /// <param name="dtPlanItemList">расшифровка плана</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        /// <param name="objectIDinDB">УИ плана в БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddPlanToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.Guid CalcPlanByDepartCustomerSubType_Guid, System.Guid CalcPlan_Guid, System.Guid SalesPlanQuota_Guid, 
            System.String Plan_Name, System.DateTime Plan_BeginDate,
            System.DateTime Plan_EndDate, System.DateTime Plan_Date, System.Guid Currency_Guid, 
            System.String Plan_Description, System.Boolean Plan_IsUseForReport,
            System.Data.DataTable dtPlanItemList,
            ref System.String strErr, ref System.Int32 iRes, ref System.Guid objectIDinDB)
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
                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCalcPlanByDepartCustomerSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanByDepartCustomerSubType_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_Date", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_BeginDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_EndDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_IsUseForReport", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@CalcPlan_Guid"].Value = CalcPlan_Guid;
                cmd.Parameters["@SalesPlanQuota_Guid"].Value = SalesPlanQuota_Guid;
                cmd.Parameters["@Currency_Guid"].Value = Currency_Guid;
                cmd.Parameters["@Plan_Name"].Value = Plan_Name;
                cmd.Parameters["@Plan_Date"].Value = Plan_Date;
                cmd.Parameters["@Plan_BeginDate"].Value = Plan_BeginDate;
                cmd.Parameters["@Plan_EndDate"].Value = Plan_EndDate;
                cmd.Parameters["@Plan_Description"].Value = Plan_Description;
                cmd.Parameters["@Plan_IsUseForReport"].Value = Plan_IsUseForReport;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    objectIDinDB = (System.Guid)cmd.Parameters["@CalcPlanByDepartCustomerSubType_Guid"].Value;

                    if (AddPlanItemDecodeToDB(objProfile, cmd, objectIDinDB, dtPlanItemList, ref strErr, ref iRes) == true)
                    {
                        SalesPlanQuota_Guid = objectIDinDB;
                    }
                }
                else
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
        /// редактирует в БД информацию о плане продаж
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CalcPlanByDepartCustomerSubType_Guid">УИ доли продаж</param>
        /// <param name="CalcPlan_Guid">уи плана по марке и группе</param>
        /// <param name="Plan_Name">наименование</param>
        /// <param name="Plan_BeginDate">начало периода плана</param>
        /// <param name="Plan_EndDate">конец периода плана</param>
        /// <param name="Plan_Date">дата регистрации плана</param>
        /// <param name="Currency_Guid">УИ валюты</param>
        /// <param name="Plan_Description">примечание</param>
        /// <param name="Plan_IsUseForReport">признак "использовать для отчётов"</param>
        /// <param name="dtPlanItemList">расшифровка плана</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        /// <param name="objectIDinDB">УИ плана в БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditPlanToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid CalcPlanByDepartCustomerSubType_Guid, System.Guid CalcPlan_Guid, System.Guid SalesPlanQuota_Guid,
            System.String Plan_Name, System.DateTime Plan_BeginDate,
            System.DateTime Plan_EndDate, System.DateTime Plan_Date, System.Guid Currency_Guid,
            System.String Plan_Description, System.Boolean Plan_IsUseForReport,
            System.Data.DataTable dtPlanItemList,
            ref System.String strErr, ref System.Int32 iRes, ref System.Guid objectIDinDB)
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
                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCalcPlanByDepartCustomerSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanByDepartCustomerSubType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_Date", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_BeginDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_EndDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Plan_IsUseForReport", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@CalcPlanByDepartCustomerSubType_Guid"].Value = CalcPlanByDepartCustomerSubType_Guid;
                cmd.Parameters["@CalcPlan_Guid"].Value = CalcPlan_Guid;
                cmd.Parameters["@SalesPlanQuota_Guid"].Value = SalesPlanQuota_Guid;
                cmd.Parameters["@Currency_Guid"].Value = Currency_Guid;
                cmd.Parameters["@Plan_Name"].Value = Plan_Name;
                cmd.Parameters["@Plan_Date"].Value = Plan_Date;
                cmd.Parameters["@Plan_BeginDate"].Value = Plan_BeginDate;
                cmd.Parameters["@Plan_EndDate"].Value = Plan_EndDate;
                cmd.Parameters["@Plan_Description"].Value = Plan_Description;
                cmd.Parameters["@Plan_IsUseForReport"].Value = Plan_IsUseForReport;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    objectIDinDB = (System.Guid)cmd.Parameters["@CalcPlanByDepartCustomerSubType_Guid"].Value;

                    if (AddPlanItemDecodeToDB(objProfile, cmd, objectIDinDB, dtPlanItemList, ref strErr, ref iRes) == true)
                    {
                        SalesPlanQuota_Guid = objectIDinDB;
                    }
                }
                else
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
        /// Удаляет план из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CalcPlanByDepartCustomerSubType_Guid">УИ плана</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        /// <param name="objectIDinDB">УИ расчёта в БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean DeletePlanFromDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid CalcPlanByDepartCustomerSubType_Guid,
            ref System.String strErr, ref System.Int32 iRes)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCalcPlanByDepartCustomerSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlanByDepartCustomerSubType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@CalcPlanByDepartCustomerSubType_Guid"].Value = CalcPlanByDepartCustomerSubType_Guid;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
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
        /// Расчёт плана продаж
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="SalesPlanQuota_Guid">уи расчёта долей продаж</param>
        /// <param name="CalcPlan_Guid">уи плана по маркам и группам</param>
        /// <param name="MonthId">№ месяца</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>таблица с приложением к плану продаж</returns>
        public static System.Data.DataTable CalcPlan(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid SalesPlanQuota_Guid, System.Guid CalcPlan_Guid,
            System.Int32 MonthId,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Data.DataTable dtPlanItem = new System.Data.DataTable();

            dtPlanItem.Columns.Add(new System.Data.DataColumn("Owner_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Owner_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Owner_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("PartType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Parttype_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Parttype_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Depart_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Depart_Code", typeof(System.Data.SqlTypes.SqlString)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("DepartTeam_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("DepartTeam_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Customer_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Customer_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("PartSubType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Partsubtype_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Partsubtype_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("CalcPlan_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("CalcPlan_AllPrice", typeof(System.Data.SqlTypes.SqlMoney)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Plan_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtPlanItem.Columns.Add(new System.Data.DataColumn("Plan_AllPrice", typeof(System.Data.SqlTypes.SqlMoney)));

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
                        return dtPlanItem;
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

                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CalcPlanDepartCustomerProductSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CalcPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MonthId", System.Data.SqlDbType.Int));

                cmd.Parameters["@SalesPlanQuota_Guid"].Value = SalesPlanQuota_Guid;
                cmd.Parameters["@CalcPlan_Guid"].Value = CalcPlan_Guid;
                cmd.Parameters["@MonthId"].Value = MonthId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;

                    while (rs.Read())
                    {
                        newRow = dtPlanItem.NewRow();
                        if (rs["Owner_Guid"] != System.DBNull.Value)
                        {
                            newRow["Owner_Guid"] = (System.Guid)rs["Owner_Guid"];
                            newRow["Owner_Id"] = System.Convert.ToInt32(rs["Owner_Id"]);
                            newRow["Owner_Name"] = ((rs["Owner_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Owner_Name"]));
                        }
                        if (rs["PartType_Guid"] != System.DBNull.Value)
                        {
                            newRow["PartType_Guid"] = (System.Guid)rs["PartType_Guid"];
                            newRow["Parttype_Id"] = System.Convert.ToInt32(rs["Parttype_Id"]);
                            newRow["Parttype_Name"] = ((rs["Parttype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Parttype_Name"]));
                        }
                        if (rs["Depart_Guid"] != System.DBNull.Value)
                        {
                            newRow["Depart_Guid"] = (System.Guid)rs["Depart_Guid"];
                            newRow["Depart_Code"] = ((rs["Depart_Code"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Depart_Code"]));
                        }
                        if (rs["DepartTeam_Guid"] != System.DBNull.Value)
                        {
                            newRow["DepartTeam_Guid"] = (System.Guid)rs["DepartTeam_Guid"];
                            newRow["DepartTeam_Name"] = ((rs["DepartTeam_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["DepartTeam_Name"]));
                        }
                        if (rs["Customer_Guid"] != System.DBNull.Value)
                        {
                            newRow["Customer_Guid"] = (System.Guid)rs["Customer_Guid"];
                            newRow["Customer_Id"] = System.Convert.ToInt32(rs["Customer_Id"]);
                            newRow["Customer_Name"] = ((rs["Customer_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Customer_Name"]));
                        }
                        if (rs["PartSubType_Guid"] != System.DBNull.Value)
                        {
                            newRow["PartSubType_Guid"] = (System.Guid)rs["PartSubType_Guid"];
                            newRow["Partsubtype_Id"] = System.Convert.ToInt32(rs["Partsubtype_Id"]);
                            newRow["Partsubtype_Name"] = ((rs["Partsubtype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Partsubtype_Name"]));
                        }

                        newRow["CalcPlan_Quantity"] = System.Convert.ToDecimal(rs["CalcPlan_Quantity"]);
                        newRow["CalcPlan_AllPrice"] = System.Convert.ToDecimal(rs["CalcPlan_AllPrice"]);

                        newRow["Plan_Quantity"] = System.Convert.ToDecimal(rs["Plan_Quantity"]);
                        newRow["Plan_AllPrice"] = System.Convert.ToDecimal(rs["Plan_AllPrice"]);


                        dtPlanItem.Rows.Add(newRow);
                    }

                    dtPlanItem.AcceptChanges();
                }

                strErr = System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value);
                iRes = System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value);

                rs.Close();
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
                "Не удалось получить список приложения к плану продаж.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dtPlanItem;
        }


    }
}
