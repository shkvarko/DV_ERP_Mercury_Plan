using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP_Mercury.Common;

namespace ERPMercuryPlan
{
    /// <summary>
    /// Класс "План продаж по подразделенииям, клиентам, подгруппам"
    /// </summary>
    public class CPlanByDepartCustomerSubType
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Расчёт плана продаж по маркам и группам
        /// </summary>
        public CCalcPlan CalcPlan { get; set; }
        /// <summary>
        /// Плана продаж по маркам и группам (наименование)
        /// </summary>
        public System.String CalcPlanName
        {
            get { return ((CalcPlan == null) ? System.String.Empty : String.Format("{0} {1}", CalcPlan.Name, CalcPlan.Date.ToShortDateString())); }
        }
        /// <summary>
        /// Плана продаж по маркам и группам (количество)
        /// </summary>
        public System.Decimal CalcPlanQty
        {
            get { return ((CalcPlan == null) ? 0 : CalcPlan.Quantity); }
        }
        /// <summary>
        /// Плана продаж по маркам и группам (сумма)
        /// </summary>
        public System.Decimal CalcPlanMoney
        {
            get { return ((CalcPlan == null) ? 0 : CalcPlan.AllMoney); }
        }
        /// <summary>
        /// Доли продаж клиентов, подгрупп, подразделений
        /// </summary>
        public CSalesPlanQuota SalesPlanQuota { get; set; }
        /// <summary>
        /// Доли продаж клиентов, подгрупп, подразделений
        /// </summary>
        public System.String SalesPlanQuotaName
        {
            get { return ((SalesPlanQuota == null) ? System.String.Empty : String.Format("{0} {1}", SalesPlanQuota.Name, SalesPlanQuota.CalcPeriod)); }
        }
        /// <summary>
        /// Дата регистрации плана
        /// </summary>
        public System.DateTime CreateDate { get; set; }
        /// <summary>
        /// Начало периода для плана
        /// </summary>
        public System.DateTime BeginDate { get; set; }
        /// <summary>
        /// Конец периода для плана
        /// </summary>
        public System.DateTime EndDate { get; set; }
        /// <summary>
        /// Наименование плана
        /// </summary>
        public System.String Name { get; set; }
        /// <summary>
        /// Валюта плана
        /// </summary>
        public CCurrency Currency { get; set; }
        /// <summary>
        /// Признак "использовать план в отчётах"
        /// </summary>
        public System.Boolean IsUseForReport { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public System.String Description { get; set; }
        /// <summary>
        /// Расшифровка плана
        /// </summary>
        public List<CPlanByDepartCustomerSubTypeItem> ContentsPlan;
        /// <summary>
        /// План (количество)
        /// </summary>
        public System.Decimal PlanQuantity { get; set; }
        /// <summary>
        /// План (сумма)
        /// </summary>
        public System.Decimal PlanAllPrice { get; set; }

        #endregion

        #region Конструктор
        public CPlanByDepartCustomerSubType()
        {
            ID = System.Guid.Empty;
            CalcPlan = null;
            SalesPlanQuota = null;
            CreateDate = System.DateTime.Today;
            BeginDate = System.DateTime.MinValue;
            EndDate = System.DateTime.MinValue;
            Name = System.String.Empty;
            Currency = null;
            IsUseForReport = false;
            Description = System.String.Empty;
            PlanQuantity = 0;
            PlanAllPrice = 0;
        }
        #endregion

        #region Журнал расчётов
        /// <summary>
        /// Возвращает журнал планов продаж
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="dtBeginDate">начало периода плана</param>
        /// <param name="dtEndDate">конец периода плана</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата метода, работающего с БД</param>
        /// <returns>список объектов класса "CPlanByDepartCustomerSubType"</returns>
        public static List<CPlanByDepartCustomerSubType> GetPlanByDepartCustomerSubTypeList(UniXP.Common.CProfile objProfile, System.DateTime dtBeginDate,
            System.DateTime dtEndDate, ref System.String strErr, ref System.Int32 iRes)
        {
            List<CPlanByDepartCustomerSubType> objList = new List<CPlanByDepartCustomerSubType>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtPlan = CPlanByDepartCustomerSubTypeDataBaseModel.GetPlanByDepartCustomerSubTypeList(objProfile, null, 
                    dtBeginDate, dtEndDate, ref strErr, ref iRes);
                if ((dtPlan != null) && (iRes == 0))
                {
                    CPlanByDepartCustomerSubType objPlan = null;
                    foreach (System.Data.DataRow objItem in dtPlan.Rows)
                    {
                        objPlan = new CPlanByDepartCustomerSubType();
                        objPlan.ID = new Guid(System.Convert.ToString(objItem["CalcPlanByDepartCustomerSubType_Guid"]));
                        objPlan.Name = ((objItem["Plan_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["Plan_Name"]));
                        if (objItem["Plan_Date"] != System.DBNull.Value)
                        {
                            objPlan.CreateDate = System.Convert.ToDateTime(System.Convert.ToString(objItem["Plan_Date"]));
                        }
                        if (objItem["Plan_BeginDate"] != System.DBNull.Value)
                        {
                            objPlan.BeginDate = System.Convert.ToDateTime(System.Convert.ToString(objItem["Plan_BeginDate"]));
                        }
                        if (objItem["Plan_EndDate"] != System.DBNull.Value)
                        {
                            objPlan.EndDate = System.Convert.ToDateTime(System.Convert.ToString(objItem["Plan_EndDate"]));
                        }
                        objPlan.Description = ((objItem["Plan_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["Plan_Description"]));
                        objPlan.IsUseForReport =  System.Convert.ToBoolean( System.Convert.ToString(objItem["Plan_IsUseForReport"]) );

                        if (objItem["Currency_Guid"] != System.DBNull.Value)
                        {
                            objPlan.Currency = new CCurrency() 
                            {
                                ID = new Guid(System.Convert.ToString(objItem["Currency_Guid"])),
                                CurrencyAbbr = ((objItem["Currency_Abbr"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["Currency_Abbr"])),
                                CurrencyCode = ((objItem["Currency_Code"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["Currency_Code"])),
                                CurrencyShortName = ((objItem["Currency_ShortName"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["Currency_ShortName"]))
                            };
                        }
                        if (objItem["CalcPlan_Guid"] != System.DBNull.Value)
                        {
                            objPlan.CalcPlan = new CCalcPlan()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["CalcPlan_Guid"])),
                                Year = ((objItem["CalcPlan_Year"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(System.Convert.ToString(objItem["CalcPlan_Year"]))),
                                Name = ((objItem["CalcPlan_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["CalcPlan_Name"])),
                                UseForReport = ((objItem["CalcPlan_IsUseForReport"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(System.Convert.ToString(objItem["CalcPlan_IsUseForReport"])))
                            };
                            if (objItem["CalcPlan_Date"] != System.DBNull.Value)
                            {
                                objPlan.CalcPlan.Date = System.Convert.ToDateTime(System.Convert.ToString(objItem["CalcPlan_Date"]));
                            }
                        }
                        if (objItem["SalesPlanQuota_Guid"] != System.DBNull.Value)
                        {
                            objPlan.SalesPlanQuota = new CSalesPlanQuota()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["SalesPlanQuota_Guid"])), 
                                Name = ((objItem["SalesPlanQuota_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["SalesPlanQuota_Name"]))
                            };
                            if (objItem["SalesPlanQuota_Date"] != System.DBNull.Value)
                            {
                                objPlan.SalesPlanQuota.Date = System.Convert.ToDateTime(System.Convert.ToString(objItem["SalesPlanQuota_Date"]));
                            }
                            if (objItem["SalesPlanQuota_BeginDate"] != System.DBNull.Value)
                            {
                                objPlan.SalesPlanQuota.Date = System.Convert.ToDateTime(System.Convert.ToString(objItem["SalesPlanQuota_BeginDate"]));
                            }
                            if (objItem["SalesPlanQuota_EndDate"] != System.DBNull.Value)
                            {
                                objPlan.SalesPlanQuota.Date = System.Convert.ToDateTime(System.Convert.ToString(objItem["SalesPlanQuota_EndDate"]));
                            }
                            objPlan.PlanQuantity = System.Convert.ToDecimal(System.Convert.ToString(objItem["CalcPlan_Quantity"]));
                            objPlan.PlanAllPrice = System.Convert.ToDecimal(System.Convert.ToString(objItem["CalcPlan_AllPrice"]));

                        }


                        objList.Add(objPlan);
                    }
                }
        
                dtPlan = null;
            }
        
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return objList;
        }

        #endregion

        #region Генератор имени
        /// <summary>
        /// Возвращает имя нового плана
        /// </summary>
        /// <returns>Строка с новым именем</returns>
        public static System.String GetNewName()
        {
            return (String.Format("План №{0}{1}{2}_{3}", System.DateTime.Today.Year, System.DateTime.Today.Month, System.DateTime.Today.Day, System.DateTime.Today.Minute.ToString()));
        }

        #endregion

        #region Приложение к плану
        /// <summary>
        /// Возвращает приложение к плану
        /// </summary>
        /// <param name="PlanID">УИ плана</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>приложение к плану</returns>
        public static List<CPlanByDepartCustomerSubTypeItem> GetPlanItemList(UniXP.Common.CProfile objProfile, System.Guid PlanID,
            ref System.String strErr, ref System.Int32 iRes)
        {
            List<CPlanByDepartCustomerSubTypeItem> objList = new List<CPlanByDepartCustomerSubTypeItem>();

            try
            {
                System.Data.DataTable dtPlanItem = CPlanByDepartCustomerSubTypeDataBaseModel.GetPlanByDepartCustomerSubTypeItemList(objProfile, 
                    null, PlanID, ref strErr, ref iRes);

                if ((dtPlanItem != null) && (iRes == 0))
                {
                    CPlanByDepartCustomerSubTypeItem objPlanItem = null;
                    foreach (System.Data.DataRow objItem in dtPlanItem.Rows)
                    {
                        objPlanItem = new CPlanByDepartCustomerSubTypeItem();
                        objPlanItem.ID = new Guid(System.Convert.ToString(objItem["CalcPlanByDepartCustomerSubTypeItem_Guid"]) );
                        if (objItem["Owner_Guid"] != System.DBNull.Value)
                        {
                            objPlanItem.ProductOwner = new CProductTradeMark()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["Owner_Guid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Owner_Id"])),
                                Name = System.Convert.ToString(objItem["Owner_Name"])
                            };
                        }
                        if (objItem["PartType_Guid"] != System.DBNull.Value)
                        {
                            objPlanItem.ProductType = new CProductType()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["PartType_Guid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Parttype_Id"])),
                                Name = System.Convert.ToString(objItem["Parttype_Name"])
                            };
                        }
                        if (objItem["Depart_Guid"] != System.DBNull.Value)
                        {
                            objPlanItem.Depart = new CDepart()
                            {
                                uuidID = new Guid(System.Convert.ToString(objItem["Depart_Guid"])), 
                                DepartCode = System.Convert.ToString(objItem["Depart_Code"]), 
                                DepartTeam = new CDepartTeam ( System.Guid.Empty, "", "", false  )
                            };
                            if (objItem["DepartTeam_Guid"] != System.DBNull.Value)
                            {
                                objPlanItem.Depart.DepartTeam.uuidID = new Guid(System.Convert.ToString(objItem["DepartTeam_Guid"]));
                                objPlanItem.Depart.DepartTeam.DepartTeamName = System.Convert.ToString(objItem["DepartTeam_Name"]);
                            }
                        }
                        if (objItem["Customer_Guid"] != System.DBNull.Value)
                        {
                            objPlanItem.Customer = new CCustomer()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["Customer_Guid"])), 
                                InterBaseID = System.Convert.ToInt32(System.Convert.ToString(objItem["Customer_Id"])),
                                FullName = System.Convert.ToString(objItem["Customer_Name"])
                            };
                        }
                        if (objItem["PartSubType_Guid"] != System.DBNull.Value)
                        {
                            objPlanItem.ProductSubType = new CProductSubType()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["PartSubType_Guid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Partsubtype_Id"])),
                                Name = System.Convert.ToString(objItem["Partsubtype_Name"])
                            };
                        }
                        objPlanItem.Plan_Quantity = System.Convert.ToDecimal(System.Convert.ToString(objItem["CalcPlan_Quantity"]));
                        objPlanItem.Plan_AllPrice = System.Convert.ToDecimal(System.Convert.ToString(objItem["CalcPlan_AllPrice"]));

                        objList.Add(objPlanItem);
                    }
                }

                dtPlanItem = null;
            }
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return objList;
        }
        #endregion

        #region Удаление плана
        /// <summary>
        /// Удаление плана из БД
        /// </summary>
        /// <param name="ID">УИ плана</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возарата процедуры БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean DeletePlan(UniXP.Common.CProfile objProfile, System.Guid ID, ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;

            try
            {
                bRet = CPlanByDepartCustomerSubTypeDataBaseModel.DeletePlanFromDB(objProfile, null, ID, ref strErr, ref iRes);
            }
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }

            return bRet;
        }

        #endregion

        #region Проверка значений перед сохранением
        /// <summary>
        /// Проверка свойств объекта "План" перед сохранением в БД
        /// </summary>
        /// <param name="Name">наименование плана</param>
        /// <param name="CreateDate">дата регистрации плана</param>
        /// <param name="BeginDate">дата начала планового периода</param>
        /// <param name="EndDate">дата окончания планового периода</param>
        /// <param name="CalcPlan_Guid">уи плана продаж по маркам и группам</param>
        /// <param name="SalesPlanQuota_Guid">уи расчёта долей продаж</param>
        /// <param name="Currency_Guid">уи валюты</param>
        /// <param name="ContentsPlan">содрежимое плана продаж</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все свойства удовлетворяют требованиям; false - не выполняются условия к значениям свойств</returns>
        public static System.Boolean IsAllParametersValid(System.String Name, 
            System.DateTime CreateDate,  System.DateTime BeginDate, System.DateTime EndDate,
            System.Guid CalcPlan_Guid, System.Guid SalesPlanQuota_Guid, System.Guid Currency_Guid,
            List<CPlanByDepartCustomerSubTypeItem> ContentsPlan,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Int32 iWarningCount = 0;
            try
            {
                if (Name.Trim() == "")
                {
                    strErr += ("\nНеобходимо указать наименование плана");
                    iWarningCount++;
                }
                if (CreateDate.CompareTo(System.DateTime.MinValue) == 0)
                {
                    strErr += ("\nНеобходимо указать дату регистрации плана!");
                    iWarningCount++;
                }
                if (BeginDate.CompareTo(System.DateTime.MinValue) == 0)
                {
                    strErr += ("\nНеобходимо указать дату начала планового периода!");
                    iWarningCount++;
                }
                if (EndDate.CompareTo(System.DateTime.MinValue) == 0)
                {
                    strErr += ("\nНеобходимо указать дату окончания планового периода!");
                    iWarningCount++;
                }
                if (CalcPlan_Guid.CompareTo( System.Guid.Empty ) == 0)
                {
                    strErr += ("\nНе задан план продаж по маркам и группам!");
                    iWarningCount++;
                }
                if (SalesPlanQuota_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr += ("\nНе задан расчёт коэффициентов!");
                    iWarningCount++;
                }
                if (Currency_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr += ("\nНеобходимо указать, в какой валюте расчитан план!");
                    iWarningCount++;
                }
                if ((ContentsPlan == null) || (ContentsPlan.Count == 0))
                {
                    strErr += ("\nПриложение к плану не должно быть пустым!");
                    iWarningCount++;
                }

                bRet = (iWarningCount == 0);
            }
            catch (System.Exception f)
            {
                strErr += (String.Format("Ошибка проверки свойств объекта 'План'. Текст ошибки: {0}", f.Message));
            }
            return bRet;
        }
        #endregion

        #region Сохранение объекта в БД
        /// <summary>
        /// Сохранение объекта в БД
        /// </summary>
        /// <param name="bNewObject">признак "Новый объект"</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="CalcPlanByDepartCustomerSubType_Guid">уи плана</param>
        /// <param name="CalcPlan_Guid">уи плана по маркам и группам</param>
        /// <param name="SalesPlanQuota_Guid">уи долей продаж</param>
        /// <param name="Plan_Name">наименование плана</param>
        /// <param name="Plan_BeginDate">дата начала планового периода</param>
        /// <param name="Plan_EndDate">дата окончания планового периода</param>
        /// <param name="Plan_Date">дата регистрации плана</param>
        /// <param name="Currency_Guid">уи валюты плана</param>
        /// <param name="Plan_IsUseForReport">признак "использовать для отчётов"</param>
        /// <param name="Plan_Description">описание</param>
        /// <param name="PlanByDepartCustomerSubTypeItemList">содержимое плана</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <param name="objectIDinDB">УИ объекта в БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveToDB(System.Boolean bNewObject, UniXP.Common.CProfile objProfile, System.Guid CalcPlanByDepartCustomerSubType_Guid,
            System.Guid CalcPlan_Guid, System.Guid SalesPlanQuota_Guid, System.String Plan_Name, System.DateTime Plan_BeginDate,
            System.DateTime Plan_EndDate, System.DateTime Plan_Date, System.Guid Currency_Guid, System.Boolean Plan_IsUseForReport,
            System.String Plan_Description,
            List<CPlanByDepartCustomerSubTypeItem> PlanByDepartCustomerSubTypeItemList,
            ref System.String strErr, ref System.Int32 iRes, ref System.Guid objectIDinDB)
        {
            System.Boolean bRet = false;
            try
            {
                System.Data.DataTable dtPlanItemList = new System.Data.DataTable();
                System.Data.DataRow newRow = null;

                dtPlanItemList.Columns.Add(new System.Data.DataColumn("CalcPlanByDepartCustomerSubTypeItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtPlanItemList.Columns.Add(new System.Data.DataColumn("CalcPlanByDepartCustomerSubType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtPlanItemList.Columns.Add(new System.Data.DataColumn("Owner_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtPlanItemList.Columns.Add(new System.Data.DataColumn("PartType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtPlanItemList.Columns.Add(new System.Data.DataColumn("Depart_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtPlanItemList.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtPlanItemList.Columns.Add(new System.Data.DataColumn("PartSubType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtPlanItemList.Columns.Add(new System.Data.DataColumn("CalcPlan_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
                dtPlanItemList.Columns.Add(new System.Data.DataColumn("CalcPlan_AllPrice", typeof(System.Data.SqlTypes.SqlMoney)));

                if ((PlanByDepartCustomerSubTypeItemList != null) && (PlanByDepartCustomerSubTypeItemList.Count > 0))
                {
                    foreach (CPlanByDepartCustomerSubTypeItem objItem in PlanByDepartCustomerSubTypeItemList)
                    {
                        newRow = dtPlanItemList.NewRow();

                        newRow["CalcPlanByDepartCustomerSubTypeItem_Guid"] = System.Guid.Empty;
                        newRow["CalcPlanByDepartCustomerSubType_Guid"] = CalcPlanByDepartCustomerSubType_Guid;
                        newRow["Owner_Guid"] = objItem.ProductOwner.ID;
                        newRow["PartType_Guid"] = objItem.ProductType.ID;
                        newRow["Depart_Guid"] = objItem.Depart.uuidID;
                        newRow["Customer_Guid"] = objItem.Customer.ID;
                        newRow["PartSubType_Guid"] = objItem.ProductSubType.ID;
                        newRow["CalcPlan_Quantity"] = objItem.Plan_Quantity;
                        newRow["CalcPlan_AllPrice"] = objItem.Plan_AllPrice;

                        dtPlanItemList.Rows.Add(newRow);
                    }
                }

                dtPlanItemList.AcceptChanges();

                if (bNewObject == true)
                {
                    bRet = CPlanByDepartCustomerSubTypeDataBaseModel.AddPlanToDB( objProfile, null, 
                        ref CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid, 
                        Plan_Name, Plan_BeginDate,  Plan_EndDate, Plan_Date, Currency_Guid, 
                        Plan_Description, Plan_IsUseForReport,  dtPlanItemList,
                        ref strErr, ref iRes, ref objectIDinDB );
                }
                else
                {
                    bRet = CPlanByDepartCustomerSubTypeDataBaseModel.EditPlanToDB(objProfile, null, 
                        CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid,
                        Plan_Name, Plan_BeginDate, Plan_EndDate, Plan_Date, Currency_Guid,
                        Plan_Description, Plan_IsUseForReport, dtPlanItemList,
                        ref strErr, ref iRes, ref objectIDinDB);
                }

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("Ошибка сохранения объекта 'План' в базе данных. Текст ошибки: {0}", f.Message));
            }

            return bRet;
        }

        #endregion

        #region Расчёт плана продаж
        /// <summary>
        /// Расчёт плана продаж
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="SalesPlanQuota_Guid">уи расчёта долей продаж</param>
        /// <param name="CalcPlan_Guid">уи плана по маркам и группам</param>
        /// <param name="MonthId">№ месяца</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>приложение к плану продаж</returns>
        public static List<CPlanByDepartCustomerSubTypeItem> CalculationPlan( UniXP.Common.CProfile objProfile,
            System.Guid SalesPlanQuota_Guid, System.Guid CalcPlan_Guid, System.Int32 MonthId,
            ref System.String strErr, ref System.Int32 iRes )
        {
            List<CPlanByDepartCustomerSubTypeItem> objList = new List<CPlanByDepartCustomerSubTypeItem>();

            try
            {
                System.Data.DataTable dtPlanItem = CPlanByDepartCustomerSubTypeDataBaseModel.CalcPlan(objProfile, null,
                    SalesPlanQuota_Guid, CalcPlan_Guid, MonthId, ref strErr, ref iRes);

                if ((dtPlanItem != null) && (dtPlanItem.Rows.Count > 0))
                {
                    CPlanByDepartCustomerSubTypeItem objPlanItem = null;

                    foreach (System.Data.DataRow objItem in dtPlanItem.Rows)
                    {
                        
                        objPlanItem = new CPlanByDepartCustomerSubTypeItem()
                        {

                            ProductOwner = new CProductTradeMark()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["Owner_Guid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Owner_Id"])),
                                Name = System.Convert.ToString(objItem["Owner_Name"])
                            },
                            ProductType = new CProductType()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["PartType_Guid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Parttype_Id"])),
                                Name = System.Convert.ToString(objItem["Parttype_Name"])
                            }
                        };

                        if ((objItem["Depart_Guid"] != System.DBNull.Value) && ( objItem.IsNull( "Depart_Guid" ) == false ))
                        {
                            objPlanItem.Depart = new CDepart();
                            objPlanItem.Depart.uuidID = new Guid(System.Convert.ToString(objItem["Depart_Guid"]));
                            objPlanItem.Depart.DepartCode = System.Convert.ToString(objItem["Depart_Code"]);
                            if (objItem["DepartTeam_Guid"] != System.DBNull.Value)
                            {
                                objPlanItem.Depart.DepartTeam = new CDepartTeam(new Guid(System.Convert.ToString(objItem["DepartTeam_Guid"])),
                                    System.Convert.ToString(objItem["DepartTeam_Name"]), "", true);
                            }
                        }
                        if ((objItem["Customer_Guid"] != System.DBNull.Value) && (objItem.IsNull("Customer_Guid") == false))
                        {
                            objPlanItem.Customer = new CCustomer();
                            objPlanItem.Customer.ID = new Guid(System.Convert.ToString(objItem["Customer_Guid"]));
                            objPlanItem.Customer.InterBaseID = System.Convert.ToInt32(System.Convert.ToString(objItem["Customer_Id"]));
                            objPlanItem.Customer.FullName = System.Convert.ToString(objItem["Customer_Name"]);
                        }
                        if ((objItem["PartSubType_Guid"] != System.DBNull.Value) && (objItem.IsNull("PartSubType_Guid") == false))
                        {
                            objPlanItem.ProductSubType = new CProductSubType();
                            objPlanItem.ProductSubType.ID = new Guid(System.Convert.ToString(objItem["PartSubType_Guid"]));
                            objPlanItem.ProductSubType.ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Partsubtype_Id"]));
                            objPlanItem.ProductSubType.Name = System.Convert.ToString(objItem["Partsubtype_Name"]);
                        }

                        objPlanItem.Plan_Quantity = System.Convert.ToDecimal(System.Convert.ToString(objItem["Plan_Quantity"]));
                        objPlanItem.Plan_AllPrice = System.Convert.ToDecimal(System.Convert.ToString(objItem["Plan_AllPrice"]));

                        objList.Add( objPlanItem );
                    }

                }

            }
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return objList;
        }

        #endregion

    }

    public class CPlanByDepartCustomerSubTypeItem
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Товарная марка
        /// </summary>
        public CProductTradeMark ProductOwner { get; set; }
        /// <summary>
        /// Товарная марка
        /// </summary>
        public System.String ProductOwnerName
        {
            get { return ((ProductOwner == null) ? System.String.Empty : ProductOwner.Name); }
        }
        /// <summary>
        /// Товарная марка
        /// </summary>
        public System.Guid ProductOwnerID
        {
            get { return ((ProductOwner == null) ? System.Guid.Empty : ProductOwner.ID); }
        }
        /// <summary>
        /// Товарная марка
        /// </summary>
        public System.Int32 ProductOwnerIbID
        {
            get { return ((ProductOwner == null) ? 0 : ProductOwner.ID_Ib); }
        }
        /// <summary>
        /// Товарная группа
        /// </summary>
        public CProductType ProductType { get; set; }
        /// <summary>
        /// Товарная группа
        /// </summary>
        public System.String ProductTypeName
        {
            get { return ((ProductType == null) ? System.String.Empty : ProductType.Name); }
        }
        /// <summary>
        /// Товарная группа
        /// </summary>
        public System.Guid ProductTypeID
        {
            get { return ((ProductType == null) ? System.Guid.Empty : ProductType.ID); }
        }
        /// <summary>
        /// Товарная группа
        /// </summary>
        public System.Int32 ProductTypeIbID
        {
            get { return ((ProductType == null) ? 0 : ProductType.ID_Ib); }
        }
        /// <summary>
        /// Подразделение
        /// </summary>
        public CDepart Depart { get; set; }
        /// <summary>
        /// Подразделение
        /// </summary>
        public System.String DepartCode
        {
            get { return ((Depart == null) ? System.String.Empty : Depart.DepartCode); }
        }
        /// <summary>
        /// Подразделение
        /// </summary>
        public System.Guid DepartID
        {
            get { return ((Depart == null) ? System.Guid.Empty : Depart.uuidID); }
        }
        /// <summary>
        /// Команда
        /// </summary>
        public System.String DepartTeamName
        {
            get { return (((Depart == null) || (Depart.DepartTeam == null)) ? System.String.Empty : Depart.DepartTeam.DepartTeamName); }
        }
        /// <summary>
        /// Клиент
        /// </summary>
        public CCustomer Customer { get; set; }
        /// <summary>
        /// Клиент
        /// </summary>
        public System.String CustomerName
        {
            get { return ((Customer == null) ? System.String.Empty : Customer.FullName); }
        }
        /// <summary>
        /// Клиент
        /// </summary>
        public System.Guid CustomerID
        {
            get { return ((Customer == null) ? System.Guid.Empty : Customer.ID); }
        }
        /// <summary>
        /// Клиент
        /// </summary>
        public System.Int32 CustomerIbID
        {
            get { return ((Customer == null) ? 0 : Customer.InterBaseID); }
        }
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        public CProductSubType ProductSubType { get; set; }
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        public System.String ProductSubTypeName
        {
            get { return ((ProductSubType == null) ? System.String.Empty : ProductSubType.Name); }
        }
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        public System.Guid ProductSubTypeID
        {
            get { return ((ProductSubType == null) ? System.Guid.Empty : ProductSubType.ID); }
        }
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        public System.Int32 ProductSubTypeIbID
        {
            get { return ((ProductSubType == null) ? 0 : ProductSubType.ID_Ib); }
        }
        /// <summary>
        /// План (количество)
        /// </summary>
        public System.Decimal Plan_Quantity { get; set; }
        /// <summary>
        /// План (сумма)
        /// </summary>
        public System.Decimal Plan_AllPrice { get; set; }
        #endregion

        #region Конструктор
        public CPlanByDepartCustomerSubTypeItem()
        {
            ID = System.Guid.Empty;
            ProductOwner = null;
            ProductType = null;
            Depart = null;
            Customer = null;
            ProductSubType = null;
            Plan_Quantity = 0;
            Plan_AllPrice = 0;
        }
        #endregion
    }

}
