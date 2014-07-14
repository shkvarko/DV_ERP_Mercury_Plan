using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPMercuryPlan
{
    class CSalesPlanQuotaDataBaseModel
    {
        /// <summary>
        /// Возвращает таблицу с описанием расчёта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="dtBeginDate">начало периода для даты расчёта</param>
        /// <param name="dtEndDate">конец периода для даты расчёта</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>таблица</returns>
        public static System.Data.DataTable GetSalesPlanQuotaList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime dtBeginDate, System.DateTime dtEndDate,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Data.DataTable dtSalesPlanQuota = new System.Data.DataTable();

            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Date", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_BeginDate", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_EndDate", typeof(System.Data.SqlTypes.SqlDateTime)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Description", typeof(System.Data.SqlTypes.SqlString)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Condition", typeof(System.Data.SqlTypes.SqlString)));


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
                        return dtSalesPlanQuota;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSalesPlanQuota]", objProfile.GetOptionsDllDBName());
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

                        newRow = dtSalesPlanQuota.NewRow();
                        newRow["SalesPlanQuota_Guid"] = (System.Guid)rs["SalesPlanQuota_Guid"];
                        newRow["SalesPlanQuota_Name"] = ((rs["SalesPlanQuota_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["SalesPlanQuota_Name"]));
                        newRow["SalesPlanQuota_Date"] = System.Convert.ToDateTime(rs["SalesPlanQuota_Date"]);
                        newRow["SalesPlanQuota_BeginDate"] = System.Convert.ToDateTime(rs["SalesPlanQuota_BeginDate"]);
                        newRow["SalesPlanQuota_EndDate"] = System.Convert.ToDateTime(rs["SalesPlanQuota_EndDate"]);
                        newRow["SalesPlanQuota_Description"] = ((rs["SalesPlanQuota_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["SalesPlanQuota_Description"]));
                        newRow["SalesPlanQuota_Condition"] = rs.GetSqlXml(5).Value; // (System.Data.SqlTypes.SqlXml)rs["SalesPlanQuota_Condition"];

                        dtSalesPlanQuota.Rows.Add(newRow);

                    }

                    dtSalesPlanQuota.AcceptChanges();
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
                strErr += ("Не удалось получить список расчётов.\n\nТекст ошибки :" + f.Message);
            }
            return dtSalesPlanQuota;
        }
        /// <summary>
        /// Возвращает бланк xml-структуры для записи условий расчёта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="NodesCount">количество элементов</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>xml-документ</returns>
        public static System.Xml.XmlDocument GetSalesPlanQuotaConditionBlank(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL,  
            System.Int32 NodesCount, ref System.String strErr, ref System.Int32 iRes)
        {
            System.Xml.XmlDocument XMLView = new System.Xml.XmlDocument();
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
                        return XMLView;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSalePlanQuotaConditionBlank]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@NodesCount", System.Data.DbType.Int32));
                cmd.Parameters["@NodesCount"].Value = NodesCount;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    
                    XMLView.LoadXml(System.Convert.ToString(rs.GetSqlXml(0).Value));
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
                strErr += ("Не удалось получить xml-структуру для условий расчёта.\n\nТекст ошибки :" + f.Message);
            }

            return XMLView;
        }

        /// <summary>
        /// Возвращает таблицу с приложением к расчёту
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="SalesPlanQuota_Guid"></param>
        /// <param name="iObjectTypeId">тип объекта в приложении</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>таблица</returns>
        public static System.Data.DataTable GetSalesPlanQuotaItemList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid SalesPlanQuota_Guid, System.Int32 iObjectTypeId,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Data.DataTable dtSalesPlanQuota = new System.Data.DataTable();

            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ProductOwner_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ProductType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("Parttype_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("Parttype_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("Owner_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("Owner_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Money", typeof(System.Data.SqlTypes.SqlMoney)));

            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectDecode_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectDecode_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectDecode_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Money", typeof(System.Data.SqlTypes.SqlMoney)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_CalcQuota", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Quota", typeof(System.Data.SqlTypes.SqlDecimal)));

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
                        return dtSalesPlanQuota;
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

                switch (iObjectTypeId)
                {
                    case 0:
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSalePlanQuotaItemDecodeDepartTeam]", objProfile.GetOptionsDllDBName());
                        break;
                    case 1:
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSalePlanQuotaItemDecodeDepart]", objProfile.GetOptionsDllDBName());
                        break;
                    case 2:
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSalePlanQuotaItemDecodeCustomer]", objProfile.GetOptionsDllDBName());
                        break;
                    case 3:
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSalePlanQuotaItemDecodePartSubType]", objProfile.GetOptionsDllDBName());
                        break;
                    default:
                        break;

                }
                
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@SalesPlanQuota_Guid"].Value = SalesPlanQuota_Guid;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;

                    while (rs.Read())
                    {

                        newRow = dtSalesPlanQuota.NewRow();
                        newRow["SalesPlanQuotaItem_Guid"] = (System.Guid)rs["SalesPlanQuotaItem_Guid"];
                        newRow["SalesPlanQuota_Guid"] = (System.Guid)rs["SalesPlanQuota_Guid"];
                        newRow["ProductOwner_Guid"] = (System.Guid)rs["ProductOwner_Guid"];
                        newRow["ProductType_Guid"] = (System.Guid)rs["ProductType_Guid"];
                        newRow["Parttype_Id"] = System.Convert.ToInt32( rs["Parttype_Id"] );
                        newRow["Parttype_Name"] = ((rs["Parttype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Parttype_Name"]));
                        newRow["Owner_Id"] = System.Convert.ToInt32(rs["Owner_Id"]);
                        newRow["Owner_Name"] = ((rs["Owner_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Owner_Name"]));
                        newRow["SalesPlanQuotaItem_Quantity"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItem_Quantity"]);
                        newRow["SalesPlanQuotaItem_Money"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItem_Money"]);

                        newRow["SalesPlanQuotaItemDecode_Guid"] = (System.Guid)rs["SalesPlanQuotaItemDecode_Guid"];
                        newRow["SalesPlanQuotaItemDecode_Quantity"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItemDecode_Quantity"]);
                        newRow["SalesPlanQuotaItemDecode_Money"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItemDecode_Money"]);
                        newRow["SalesPlanQuotaItemDecode_CalcQuota"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItemDecode_CalcQuota"]);
                        newRow["SalesPlanQuotaItemDecode_Quota"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItemDecode_Quota"]);

                        newRow["ObjectDecode_Guid"] = (System.Guid)rs["ObjectDecode_Guid"];
                        newRow["ObjectDecode_Id"] = System.Convert.ToInt32(rs["ObjectDecode_Id"]);
                        newRow["ObjectDecode_Name"] = ((rs["ObjectDecode_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["ObjectDecode_Name"]));

                        dtSalesPlanQuota.Rows.Add(newRow);
                    }

                    dtSalesPlanQuota.AcceptChanges();
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
                "Не удалось получить список приложения к расчёту.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dtSalesPlanQuota;
        }

        /// <summary>
        /// Добавляет в БД информацию о расчёте
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="SalesPlanQuota_Guid">УИ расчёта</param>
        /// <param name="SalesPlanQuota_Name">наименование расчёта</param>
        /// <param name="SalesPlanQuota_Date">дата регистрации</param>
        /// <param name="SalesPlanQuota_BeginDate">начало периода продаж для расчёта</param>
        /// <param name="SalesPlanQuota_EndDate">конец периода продаж для расчёта</param>
        /// <param name="SalesPlanQuota_Description">описание</param>
        /// <param name="SalesPlanQuota_Condition">условия расчёта в формате xml</param>
        /// <param name="dtSalesPlanQuotaItemListAdded">список добавленных расшифровок</param>
        /// <param name="dtSalesPlanQuotaItemListEdit">список изменённых расшифровок</param>
        /// <param name="dtSalesPlanQuotaItemListDeleted">список удалённых расшифровок</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        /// <param name="objectIDinDB">УИ расчёта в БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddSalesPlanQuotaToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.Guid SalesPlanQuota_Guid, System.String SalesPlanQuota_Name, System.DateTime SalesPlanQuota_Date,
            System.DateTime SalesPlanQuota_BeginDate, System.DateTime SalesPlanQuota_EndDate,
            System.String SalesPlanQuota_Description,
            System.String SalesPlanQuota_Condition, 
            System.Data.DataTable dtSalesPlanQuotaItemList,
            ref System.String strErr, ref System.Int32 iRes, ref System.Guid objectIDinDB)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if ((SalesPlanQuota_Condition.Length == 0)) 
                {
                    strErr += " Список условий расчёта пуст.";
                    return bRet; 
                }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddSalesPlanQuota]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Date", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_BeginDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_EndDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Condition", System.Data.SqlDbType.Xml));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@SalesPlanQuota_Name"].Value = SalesPlanQuota_Name;
                cmd.Parameters["@SalesPlanQuota_Date"].Value = SalesPlanQuota_Date;
                cmd.Parameters["@SalesPlanQuota_BeginDate"].Value = SalesPlanQuota_BeginDate;
                cmd.Parameters["@SalesPlanQuota_EndDate"].Value = SalesPlanQuota_EndDate;
                cmd.Parameters["@SalesPlanQuota_Description"].Value = SalesPlanQuota_Description;
                cmd.Parameters["@SalesPlanQuota_Condition"].Value = SalesPlanQuota_Condition;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    objectIDinDB = ( System.Guid )cmd.Parameters["@SalesPlanQuota_Guid"].Value;

                    if( AddSalePlanQuotaItemDecodeToDB( objProfile, cmd, objectIDinDB, dtSalesPlanQuotaItemList, ref strErr, ref iRes ) == true )
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
        /// Редактирует информацию о расчёте в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="SalesPlanQuota_Guid">УИ расчёта</param>
        /// <param name="SalesPlanQuota_Name">наименование расчёта</param>
        /// <param name="SalesPlanQuota_Date">дата регистрации</param>
        /// <param name="SalesPlanQuota_BeginDate">начало периода продаж для расчёта</param>
        /// <param name="SalesPlanQuota_EndDate">конец периода продаж для расчёта</param>
        /// <param name="SalesPlanQuota_Description">описание</param>
        /// <param name="SalesPlanQuota_Condition">условия расчёта в формате xml</param>
        /// <param name="dtSalesPlanQuotaItemListAdded">список добавленных расшифровок</param>
        /// <param name="dtSalesPlanQuotaItemListEdit">список изменённых расшифровок</param>
        /// <param name="dtSalesPlanQuotaItemListDeleted">список удалённых расшифровок</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        /// <param name="objectIDinDB">УИ расчёта в БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditSalesPlanQuotaToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid SalesPlanQuota_Guid, System.String SalesPlanQuota_Name, System.DateTime SalesPlanQuota_Date,
            System.DateTime SalesPlanQuota_BeginDate, System.DateTime SalesPlanQuota_EndDate,
            System.String SalesPlanQuota_Description,
            System.String SalesPlanQuota_Condition,
            System.Data.DataTable dtSalesPlanQuotaItemList,
            ref System.String strErr, ref System.Int32 iRes, ref System.Guid objectIDinDB)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if ((SalesPlanQuota_Condition.Length == 0))
                {
                    strErr += " Список условий расчёта пуст.";
                    return bRet;
                }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditSalesPlanQuota]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Date", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_BeginDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_EndDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Condition", System.Data.SqlDbType.Xml));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@SalesPlanQuota_Guid"].Value = SalesPlanQuota_Guid;
                cmd.Parameters["@SalesPlanQuota_Name"].Value = SalesPlanQuota_Name;
                cmd.Parameters["@SalesPlanQuota_Date"].Value = SalesPlanQuota_Date;
                cmd.Parameters["@SalesPlanQuota_BeginDate"].Value = SalesPlanQuota_BeginDate;
                cmd.Parameters["@SalesPlanQuota_EndDate"].Value = SalesPlanQuota_EndDate;
                cmd.Parameters["@SalesPlanQuota_Description"].Value = SalesPlanQuota_Description;
                cmd.Parameters["@SalesPlanQuota_Condition"].Value = SalesPlanQuota_Condition;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    objectIDinDB = (System.Guid)cmd.Parameters["@SalesPlanQuota_Guid"].Value;

                    iRes = -1;
                    AddSalePlanQuotaItemDecodeToDB( objProfile, cmd, SalesPlanQuota_Guid, dtSalesPlanQuotaItemList, ref strErr, ref iRes );
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
        /// Удаляет расчёт из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="SalesPlanQuota_Guid">УИ расчёта</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        /// <param name="objectIDinDB">УИ расчёта в БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean DeleteSalesPlanQuotaToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid SalesPlanQuota_Guid,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteSalesPlanQuota]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@SalesPlanQuota_Guid"].Value = SalesPlanQuota_Guid;
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
        /// Сохраняет в БД приложение к расчёту
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="SalesPlanQuota_Guid">УИ расчёта</param>
        /// <param name="dtSalesPlanQuotaItemList">таблица с приложением к расчёту</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddSalePlanQuotaItemDecodeToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Guid SalesPlanQuota_Guid, System.Data.DataTable dtSalesPlanQuotaItemList, 
            ref System.String strErr, ref System.Int32 iRes )
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (dtSalesPlanQuotaItemList == null) { return bRet; }

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddSalePlanQuotaItemDecode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@t_SalesPlanQuotaItemDecode", dtSalesPlanQuotaItemList);
                cmd.Parameters["@t_SalesPlanQuotaItemDecode"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@t_SalesPlanQuotaItemDecode"].TypeName = "dbo.udt_SalesPlanQuotaItemDecode";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@SalesPlanQuota_Guid"].Value = SalesPlanQuota_Guid;
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
        /// Расчёт долей продаж
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="SalesPlanQuota_Guid">УИ расчёта</param>
        /// <param name="BeginDate">дата начала периода продаж</param>
        /// <param name="EndDate">дата окончания периода продаж</param>
        /// <param name="dtProductTradeMarkList">список товарных марок</param>
        /// <param name="dtProductTypeList">список товарных групп</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        /// <returns>приложение к расчёту</returns>
        public static System.Data.DataTable CalcSalesPlanQuotaItemList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid SalesPlanQuota_Guid, System.DateTime BeginDate, System.DateTime EndDate,
            System.Data.DataTable dtProductTradeMarkList, System.Data.DataTable dtProductTypeList,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Data.DataTable dtSalesPlanQuota = new System.Data.DataTable();

            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ProductOwner_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ProductType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("Parttype_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("Parttype_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("Owner_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("Owner_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Money", typeof(System.Data.SqlTypes.SqlMoney)));

            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectType_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectDecode_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectDecode_Id", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectDecode_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Money", typeof(System.Data.SqlTypes.SqlMoney)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_CalcQuota", typeof(System.Data.SqlTypes.SqlDecimal)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Quota", typeof(System.Data.SqlTypes.SqlDecimal)));

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
                        return dtSalesPlanQuota;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CalcSalePlanQuotaItemDecode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SalesPlanQuota_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.DateTime));

                cmd.Parameters.AddWithValue("@t_ProductTradeMarkList", dtProductTradeMarkList);
                cmd.Parameters["@t_ProductTradeMarkList"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@t_ProductTradeMarkList"].TypeName = "dbo.udt_GuidList";

                cmd.Parameters.AddWithValue("@t_ProductTypeList", dtProductTypeList);
                cmd.Parameters["@t_ProductTypeList"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@t_ProductTypeList"].TypeName = "dbo.udt_GuidList";
                
                cmd.Parameters["@SalesPlanQuota_Guid"].Value = SalesPlanQuota_Guid;
                cmd.Parameters["@BeginDate"].Value = BeginDate;
                cmd.Parameters["@EndDate"].Value = EndDate;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;

                    while (rs.Read())
                    {
                        newRow = dtSalesPlanQuota.NewRow();
                        if (rs["SalesPlanQuotaItem_Guid"] != System.DBNull.Value)
                        {
                            newRow["SalesPlanQuotaItem_Guid"] = (System.Guid)rs["SalesPlanQuotaItem_Guid"];
                        }
                        newRow["SalesPlanQuota_Guid"] = (System.Guid)rs["SalesPlanQuota_Guid"];
                        newRow["ProductOwner_Guid"] = (System.Guid)rs["ProductOwner_Guid"];
                        newRow["ProductType_Guid"] = (System.Guid)rs["ProductType_Guid"];
                        newRow["Parttype_Id"] = System.Convert.ToInt32(rs["Parttype_Id"]);
                        newRow["Parttype_Name"] = ((rs["Parttype_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Parttype_Name"]));
                        newRow["Owner_Id"] = System.Convert.ToInt32(rs["Owner_Id"]);
                        newRow["Owner_Name"] = ((rs["Owner_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Owner_Name"]));
                        newRow["SalesPlanQuotaItem_Quantity"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItem_Quantity"]);
                        newRow["SalesPlanQuotaItem_Money"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItem_Money"]);
                        if (rs["SalesPlanQuotaItemDecode_Guid"] != System.DBNull.Value)
                        {
                            newRow["SalesPlanQuotaItemDecode_Guid"] = (System.Guid)rs["SalesPlanQuotaItemDecode_Guid"];
                        }
                        newRow["SalesPlanQuotaItemDecode_Quantity"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItemDecode_Quantity"]);
                        newRow["SalesPlanQuotaItemDecode_Money"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItemDecode_Money"]);
                        newRow["SalesPlanQuotaItemDecode_CalcQuota"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItemDecode_CalcQuota"]);
                        newRow["SalesPlanQuotaItemDecode_Quota"] = System.Convert.ToDecimal(rs["SalesPlanQuotaItemDecode_Quota"]);

                        newRow["ObjectType_Id"] = System.Convert.ToInt32(rs["ObjectType_Id"]);
                        newRow["ObjectDecode_Guid"] = (System.Guid)rs["ObjectDecode_Guid"];

                        newRow["ObjectDecode_Id"] = System.Convert.ToInt32(rs["ObjectDecode_Id"]);
                        newRow["ObjectDecode_Name"] = ((rs["ObjectDecode_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["ObjectDecode_Name"]));

                        dtSalesPlanQuota.Rows.Add(newRow);
                    }

                    dtSalesPlanQuota.AcceptChanges();
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
                "Не удалось получить список приложения к расчёту.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dtSalesPlanQuota;
        }

        /// <summary>
        /// Возвращает таблицу со списком твоарных марок и групп для расчёта плана
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        /// <returns>таблица</returns>
        public static System.Data.DataTable GetProductTradeMarkProductTypeListForActiveCalcPlan(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Data.DataTable dtSalesPlanQuota = new System.Data.DataTable();

            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectTypeId", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectGuid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectId", typeof(System.Data.SqlTypes.SqlInt32)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectName", typeof(System.Data.SqlTypes.SqlString)));

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
                        return dtSalesPlanQuota;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetProductTradeMarkProductTypeForActiveCalcPlan]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;

                    while (rs.Read())
                    {
                        newRow = dtSalesPlanQuota.NewRow();

                        newRow["ObjectTypeId"] = System.Convert.ToInt32(rs["ObjectTypeId"]);
                        newRow["ObjectGuid"] = (System.Guid)rs["ObjectGuid"];
                        newRow["ObjectId"] = System.Convert.ToInt32(rs["ObjectId"]);
                        newRow["ObjectName"] = ((rs["ObjectName"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["ObjectName"]));

                        dtSalesPlanQuota.Rows.Add(newRow);
                    }

                    dtSalesPlanQuota.AcceptChanges();
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
                "Не удалось получить список товарных марок и групп.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dtSalesPlanQuota;
        }

        /// <summary>
        /// Возвращает список подразделений для указанной ТМ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="ProductOwner_Guid">УИ ТМ</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>таблица со списком команд</returns>
        public static System.Data.DataTable GetDepartTeamListForTradeMark(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid ProductOwner_Guid,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Data.DataTable dtSalesPlanQuota = new System.Data.DataTable();

            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectGuid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtSalesPlanQuota.Columns.Add(new System.Data.DataColumn("ObjectName", typeof(System.Data.SqlTypes.SqlString)));

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
                        return dtSalesPlanQuota;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetDepartTeamsForProductOwner]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductOwner_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ProductOwner_Guid"].Value = ProductOwner_Guid;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;

                    while (rs.Read())
                    {
                        newRow = dtSalesPlanQuota.NewRow();

                        newRow["ObjectGuid"] = (System.Guid)rs["DepartTeam_Guid"];
                        newRow["ObjectName"] = ((rs["DepartTeam_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["DepartTeam_Name"]));

                        dtSalesPlanQuota.Rows.Add(newRow);
                    }

                    dtSalesPlanQuota.AcceptChanges();
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
                "Не удалось получить список команд для товарной марки.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dtSalesPlanQuota;
        }

    }


}
