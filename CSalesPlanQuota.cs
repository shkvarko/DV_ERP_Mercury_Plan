using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP_Mercury.Common;

namespace ERPMercuryPlan
{
    /// <summary>
    /// Тип объекта в доли продаж
    /// </summary>
    public enum enQuotaObjectType
    {
        Unkown = -1,
        /// <summary>
        /// Команда
        /// </summary>
        DepartTeam = 0,
        /// <summary>
        /// Подразделение
        /// </summary>
        Depart = 1,    
        /// <summary>
        /// Клиент
        /// </summary>
        Customer = 2, 
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        ProductSubType = 3
    }
    
    /// <summary>
    /// Элемент доли продаж
    /// </summary>
    public class CQuotaItemObject
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public System.String Name { get; set; }
        /// <summary>
        /// Тип объекта
        /// </summary>
        public enQuotaObjectType QuotaObjectType { get; set; }
        /// <summary>
        /// Продано (шт.)
        /// </summary>
        public System.Decimal SalesQuantity { get; set; }
        /// <summary>
        /// Продано (ОВУ)
        /// </summary>
        public System.Decimal SalesMoney { get; set; }
        /// <summary>
        /// Доля в продажах рассчитанная
        /// </summary>
        public System.Decimal CalcQuota { get; set; }
        /// <summary>
        /// Доля в продажах
        /// </summary>
        public System.Decimal Quota { get; set; }

        public CQuotaItemObject()
        {
            ID = System.Guid.Empty;
            Name = System.String.Empty;
            QuotaObjectType = enQuotaObjectType.Unkown;
            SalesQuantity = 0;
            SalesMoney = 0;
            CalcQuota = 0;
            Quota = 0;
        }
    }

    /// <summary>
    /// Элемент в приложении к расчёту коэффициентов плана
    /// </summary>
    public class CSalesPlanQuotaItem
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Товарная марка
        /// </summary>
        public CProductTradeMark ProductTradeMark { get; set; }
        /// <summary>
        /// Товарная группа
        /// </summary>
        public CProductType ProductType { get; set; }
        /// <summary>
        /// Продано (шт.)
        /// </summary>
        public System.Decimal SalesQuantity { get; set; }
        /// <summary>
        /// Продано (ОВУ)
        /// </summary>
        public System.Decimal SalesMoney { get; set; }
        /// <summary>
        /// Список долей продаж
        /// </summary>
        public List<CQuotaItemObject> QuotaList { get; set; }

        public CSalesPlanQuotaItem()
        {
            ID = System.Guid.Empty;
            ProductTradeMark = null;
            ProductType = null;
            SalesQuantity = 0;
            SalesMoney = 0;
            QuotaList = null;
        }
    }

    public class CSalesPlanQuotaItemForGrid
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Товарная марка
        /// </summary>
        public CProductTradeMark ProductTradeMark { get; set; }
        /// <summary>
        /// Товарная марка (наименование)
        /// </summary>
        public System.String ProductTradeMarkName
        { get { return ( ( ProductTradeMark == null ) ? "" : ProductTradeMark.Name ); } }
        /// <summary>
        /// Товарная марка (УИ)
        /// </summary>
        public System.Guid ProductTradeMarkID
        { get { return ((ProductTradeMark == null) ? System.Guid.Empty : ProductTradeMark.ID); } }
        /// <summary>
        /// Товарная группа
        /// </summary>
        public CProductType ProductType { get; set; }
        /// <summary>
        /// Товарная группа (наименование)
        /// </summary>
        public System.String ProductTypeName
        { get { return ((ProductType == null) ? "" : ProductType.Name); } }
        /// <summary>
        /// Товарная группа (УИ)
        /// </summary>
        public System.Guid ProductTypeID
        { get { return ((ProductType == null) ? System.Guid.Empty : ProductType.ID); } }
        /// <summary>
        /// Продано (шт.)
        /// </summary>
        public System.Decimal SalesQuantity { get; set; }
        /// <summary>
        /// Продано (ОВУ)
        /// </summary>
        public System.Decimal SalesMoney { get; set; }
        /// <summary>
        /// Уникальный идентификатор объекта для расчёта
        /// </summary>
        public System.Guid Object_ID { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public System.String Object_Name { get; set; }
        /// <summary>
        /// Тип объекта
        /// </summary>
        public enQuotaObjectType Object_QuotaObjectType { get; set; }
        /// <summary>
        /// Продано (шт.)
        /// </summary>
        public System.Decimal Object_SalesQuantity { get; set; }
        /// <summary>
        /// Продано (ОВУ)
        /// </summary>
        public System.Decimal Object_SalesMoney { get; set; }
        /// <summary>
        /// Доля в продажах рассчитанная
        /// </summary>
        public System.Decimal Object_CalcQuota { get; set; }
        /// <summary>
        /// Доля в продажах
        /// </summary>
        public System.Decimal Object_Quota { get; set; }

        public CSalesPlanQuotaItemForGrid()
        {
            ID = System.Guid.Empty;
            ProductTradeMark = null;
            ProductType = null;
            SalesQuantity = 0;
            SalesMoney = 0;
            Object_ID = System.Guid.Empty;
            Object_Name = System.String.Empty;
            Object_QuotaObjectType = enQuotaObjectType.Unkown;
            Object_SalesQuantity = 0;
            Object_SalesMoney = 0;
            Object_CalcQuota = 0;
            Object_Quota = 0;
        }
    }

    /// <summary>
    /// Условия формирования расчёта
    /// </summary>
    public class CSalesPlanQuotaCalculationConditions
    {
        /// <summary>
        /// Начало периода для расчёта
        /// </summary>
        public System.DateTime CalcPeriodBeginDate { get; set; }
        /// <summary>
        /// Конец периода для расчёта
        /// </summary>
        public System.DateTime CalcPeriodEndDate { get; set; }
        /// <summary>
        /// Список товарных марок для расчёта
        /// </summary>
        public List<CProductTradeMark> ProductTradeMarkList { get; set; }
        /// <summary>
        /// Список товарных групп для расчёта
        /// </summary>
        public List<CProductType> ProductTypeList { get; set; }
        /// <summary>
        /// Представление условий в виде xml
        /// </summary>
        public System.Xml.XmlDocument XMLView { get; set; }

        public CSalesPlanQuotaCalculationConditions()
        {
            CalcPeriodBeginDate = System.DateTime.MinValue;
            CalcPeriodEndDate = System.DateTime.MinValue;
            ProductTradeMarkList = null;
            ProductTypeList = null;
            XMLView = null;
        }
    }

    /// <summary>
    /// Класс "Коэффициент для расчёта плана по подразделениям, клиентам и подгруппам"
    /// </summary>
    public class CSalesPlanQuota
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
        /// Дата создания расчёта
        /// </summary>
        public System.DateTime Date { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public System.String Description { get; set; }
        /// <summary>
        /// Условия расчёта
        /// </summary>
        public CSalesPlanQuotaCalculationConditions CalculationConditions { get; set; }
        /// <summary>
        /// Приложение к расчёту
        /// </summary>
        public List<CSalesPlanQuotaItem> SalesPlanQuotaItemList { get; set; }
        /// <summary>
        /// Период для расчёта
        /// </summary>
        public System.String CalcPeriod
        {
            get
            {
                System.String CalcPeriodBeginDate = ((CalculationConditions == null) ? System.String.Empty : CalculationConditions.CalcPeriodBeginDate.ToShortDateString());
                System.String CalcPeriodEndDate = ((CalculationConditions == null) ? System.String.Empty : CalculationConditions.CalcPeriodEndDate.ToShortDateString());

                return (String.Format("{0} - {1}", CalcPeriodBeginDate, CalcPeriodEndDate));
            }
        }
        #endregion

        #region Конструктор
        public CSalesPlanQuota()
        {
            ID = System.Guid.Empty;
            Name = System.String.Empty;
            Date = System.DateTime.MinValue;
            Description = System.String.Empty;
            CalculationConditions = null;
            SalesPlanQuotaItemList = null;
        }
        #endregion

        #region Журнал расчётов
        /// <summary>
        /// Возвращает журнал расчётов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="dtBeginDate">начало периода</param>
        /// <param name="dtEndDate">конец периода</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата метода, работающего с БД</param>
        /// <returns>список объектов класса "CSalesPlanQuota"</returns>
        public static List<CSalesPlanQuota> GetSalesPlanQuotaList( UniXP.Common.CProfile objProfile, System.DateTime dtBeginDate, 
            System.DateTime dtEndDate, ref System.String strErr, ref System.Int32 iRes )
        {
            List<CSalesPlanQuota> objList = new List<CSalesPlanQuota>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtSalesPlanQuota = CSalesPlanQuotaDataBaseModel.GetSalesPlanQuotaList(objProfile, null, dtBeginDate, dtEndDate, ref strErr, ref iRes);
                if ((dtSalesPlanQuota != null) && ( iRes == 0 ))
                {
                    CSalesPlanQuota objSalesPlanQuota = null;
                    foreach (System.Data.DataRow objItem in dtSalesPlanQuota.Rows)
                    {
                        objSalesPlanQuota = new CSalesPlanQuota();
                        objSalesPlanQuota.ID = new Guid( System.Convert.ToString( objItem["SalesPlanQuota_Guid"] ) );
                        objSalesPlanQuota.Name = System.Convert.ToString(objItem["SalesPlanQuota_Name"]);
                        objSalesPlanQuota.Date = System.Convert.ToDateTime(  System.Convert.ToString( objItem["SalesPlanQuota_Date"] ));
                        objSalesPlanQuota.Description = ((objItem["SalesPlanQuota_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["SalesPlanQuota_Description"]));
                        objSalesPlanQuota.CalculationConditions = new CSalesPlanQuotaCalculationConditions()
                        {
                            CalcPeriodBeginDate = System.Convert.ToDateTime(System.Convert.ToString(objItem["SalesPlanQuota_BeginDate"])),
                            CalcPeriodEndDate = System.Convert.ToDateTime(System.Convert.ToString(objItem["SalesPlanQuota_EndDate"])),
                            XMLView = new System.Xml.XmlDocument(), ProductTradeMarkList = new List<CProductTradeMark>(), ProductTypeList = new List<CProductType>()
                        };
                        objSalesPlanQuota.CalculationConditions.XMLView.LoadXml( System.Convert.ToString( objItem["SalesPlanQuota_Condition"] ) );

                        foreach (System.Xml.XmlNode objNode in objSalesPlanQuota.CalculationConditions.XMLView.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                switch (System.Convert.ToInt32(objChildNode.Attributes["ItemTypeId"].Value))
                                {
                                    case 0:
                                        objSalesPlanQuota.CalculationConditions.ProductTradeMarkList.Add(
                                            new CProductTradeMark() 
                                            { 
                                                ID = new Guid(System.Convert.ToString(objChildNode.Attributes["ItemGuid"].Value)), 
                                                Name = System.Convert.ToString(objChildNode.Attributes["ItemName"].Value) 
                                            });
                                        break;
                                    case 1:
                                        objSalesPlanQuota.CalculationConditions.ProductTypeList.Add(
                                            new CProductType() 
                                            { 
                                                ID = new Guid(System.Convert.ToString(objChildNode.Attributes["ItemGuid"].Value)), 
                                                Name = System.Convert.ToString(objChildNode.Attributes["ItemName"].Value) 
                                            });
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        objList.Add(objSalesPlanQuota);
                    }
                }

                dtSalesPlanQuota = null;
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
        /// Возвращает имя нового расчёта
        /// </summary>
        /// <returns></returns>
        public static System.String GetNewName()
        {
            return (String.Format("Расчёт №{0}{1}{2}{3}", System.DateTime.Today.Year, System.DateTime.Today.Month, System.DateTime.Today.Day, System.DateTime.Today.Minute.ToString()));
        }

        #endregion

        #region Условия расчёта
        /// <summary>
        /// Возвращает бланк xml-структуры для записи условий расчёта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="NodesCount">количество элементов</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>xml-документ</returns>
        public static System.Xml.XmlDocument GetSalesPlanQuotaConditionBlank(UniXP.Common.CProfile objProfile,
            System.Int32 NodesCount, ref System.String strErr, ref System.Int32 iRes)
        {
            return CSalesPlanQuotaDataBaseModel.GetSalesPlanQuotaConditionBlank(objProfile, null,
                NodesCount, ref strErr, ref iRes);
        }

        #endregion

        #region Приложение к расчёту
        /// <summary>
        /// Возвращает приложение к расчёту, отформатированное для табличного отображения
        /// </summary>
        /// <param name="SalesPlanQuotaItemList">приложение к расчёту</param>
        /// <param name="enumQuotaObjectType">тип объекта в приложении</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>приложение к расчёту для представления в гриде</returns>
        public static List<CSalesPlanQuotaItemForGrid> TransformSalesPlanQuotaItemListForGrid( List<CSalesPlanQuotaItem> SalesPlanQuotaItemList, 
            enQuotaObjectType enumQuotaObjectType, ref System.String strErr)
        {
            List<CSalesPlanQuotaItemForGrid> objList = null;

            try
            {
                if ((SalesPlanQuotaItemList != null) && (SalesPlanQuotaItemList.Count > 0))
                {
                    objList = new List<CSalesPlanQuotaItemForGrid>();
                    foreach (CSalesPlanQuotaItem objItem in SalesPlanQuotaItemList)
                    {
                        foreach (CQuotaItemObject objQuota in objItem.QuotaList)
                        {
                            if (objQuota.QuotaObjectType == enumQuotaObjectType)
                            {
                                objList.Add(new CSalesPlanQuotaItemForGrid() 
                                {
                                    ID = objItem.ID,
                                    ProductTradeMark = objItem.ProductTradeMark,
                                    ProductType = objItem.ProductType,
                                    SalesQuantity = objItem.SalesQuantity,
                                    SalesMoney = objItem.SalesMoney,
                                    Object_ID = objQuota.ID,
                                    Object_Name = objQuota.Name,
                                    Object_QuotaObjectType = objQuota.QuotaObjectType,
                                    Object_SalesQuantity = objQuota.SalesQuantity,
                                    Object_SalesMoney = objQuota.SalesMoney,
                                    Object_CalcQuota = objQuota.CalcQuota,
                                    Object_Quota = objQuota.Quota
                                }
                                );
                            }
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return objList;
        }
        /// <summary>
        /// Возвращает приложение к расчёту, отформатированное для табличного отображения
        /// </summary>
        /// <param name="SalesPlanQuotaItemList">приложение к расчёту</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>приложение к расчёту для представления в гриде</returns>
        public static List<CSalesPlanQuotaItemForGrid> TransformSalesPlanQuotaItemListForGrid(List<CSalesPlanQuotaItem> SalesPlanQuotaItemList, ref System.String strErr)
        {
            List<CSalesPlanQuotaItemForGrid> objList = null;

            try
            {
                if ((SalesPlanQuotaItemList != null) && (SalesPlanQuotaItemList.Count > 0))
                {
                    objList = new List<CSalesPlanQuotaItemForGrid>();
                    foreach (CSalesPlanQuotaItem objItem in SalesPlanQuotaItemList)
                    {
                        foreach (CQuotaItemObject objQuota in objItem.QuotaList)
                        {
                            objList.Add(new CSalesPlanQuotaItemForGrid()
                            {
                                ID = objItem.ID,
                                ProductTradeMark = objItem.ProductTradeMark,
                                ProductType = objItem.ProductType,
                                SalesQuantity = objItem.SalesQuantity,
                                SalesMoney = objItem.SalesMoney,
                                Object_ID = objQuota.ID,
                                Object_Name = objQuota.Name,
                                Object_QuotaObjectType = objQuota.QuotaObjectType,
                                Object_SalesQuantity = objQuota.SalesQuantity,
                                Object_SalesMoney = objQuota.SalesMoney,
                                Object_CalcQuota = objQuota.CalcQuota,
                                Object_Quota = objQuota.Quota
                            }
                            );
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return objList;
        }
        /// <summary>
        /// Возвращает приложение к расчёту
        /// </summary>
        /// <param name="SalesPlanQuotaID">УИ расчёта</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>приложение к расчёту</returns>
        public static List<CSalesPlanQuotaItem> GetSalesPlanQuotaItemList(UniXP.Common.CProfile objProfile, System.Guid SalesPlanQuotaID,
            ref System.String strErr, ref System.Int32 iRes)
        {
            List<CSalesPlanQuotaItem> objList = new List<CSalesPlanQuotaItem>();

            try
            {

                System.Data.DataTable dtSalesPlanItemQuota = null;

                   for (System.Int32 i = 0; i <= (System.Int32)enQuotaObjectType.ProductSubType; i++)
                    {
                    dtSalesPlanItemQuota = CSalesPlanQuotaDataBaseModel.GetSalesPlanQuotaItemList(objProfile, null, SalesPlanQuotaID,
                    i, ref strErr, ref iRes);

                    if ((dtSalesPlanItemQuota != null) && (iRes == 0))
                    {
                        CSalesPlanQuotaItem objSalesPlanQuotaItem = null;
                        System.Guid SalesPlanQuotaItem_Guid = System.Guid.Empty;
                        foreach (System.Data.DataRow objItem in dtSalesPlanItemQuota.Rows)
                        {
                            if (new Guid(System.Convert.ToString(objItem["SalesPlanQuotaItem_Guid"])).CompareTo(SalesPlanQuotaItem_Guid) != 0)
                            {
                                SalesPlanQuotaItem_Guid = new Guid(System.Convert.ToString(objItem["SalesPlanQuotaItem_Guid"]));

                                objSalesPlanQuotaItem = new CSalesPlanQuotaItem()
                                {
                                    
                                    ID = new Guid(System.Convert.ToString(objItem["SalesPlanQuotaItem_Guid"])),
                                    ProductTradeMark = new CProductTradeMark() 
                                    { 
                                        ID = new Guid(System.Convert.ToString(objItem["ProductOwner_Guid"])), 
                                        ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Owner_Id"])), 
                                        Name = System.Convert.ToString(objItem["Owner_Name"]) 
                                    },
                                    ProductType = new CProductType() 
                                    { 
                                        ID = new Guid(System.Convert.ToString(objItem["ProductType_Guid"])),
                                        ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Parttype_Id"])),
                                        Name = System.Convert.ToString(objItem["Parttype_Name"]) 
                                    },
                                    SalesMoney = System.Convert.ToDecimal(System.Convert.ToString( objItem["SalesPlanQuotaItem_Money"])),
                                    SalesQuantity = System.Convert.ToDecimal(System.Convert.ToString( objItem["SalesPlanQuotaItem_Quantity"])),
                                    QuotaList = new List<CQuotaItemObject>()
                                };

                                objList.Add(objSalesPlanQuotaItem);
                            }

                            objSalesPlanQuotaItem.QuotaList.Add(new CQuotaItemObject()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["ObjectDecode_Guid"])),
                                QuotaObjectType = (enQuotaObjectType)i,
                                Name = System.Convert.ToString(objItem["ObjectDecode_Name"]),
                                SalesQuantity = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItemDecode_Quantity"])),
                                SalesMoney = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItemDecode_Money"])),
                                CalcQuota = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItemDecode_CalcQuota"])),
                                Quota = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItemDecode_Quota"]))
                            });

                        
                        }


                    }

                }

                dtSalesPlanItemQuota = null;
            }
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return objList;
        }
        #endregion

        #region Расчёт приложения
        /// <summary>
        /// заполняет списки товарных марок и групп для расчёта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objProductTradeMarkList">список твоарных марок</param>
        /// <param name="objProductTypeList">список твоарных групп</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        public static void GetProductTradeMarkProductTypeListForActiveCalcPlan(UniXP.Common.CProfile objProfile, 
            ref List<CProductTradeMark> objProductTradeMarkList, ref List<CProductType> objProductTypeList,
            ref System.String strErr, ref System.Int32 iRes)
        {
            try
            {
                if (objProductTradeMarkList == null) { objProductTradeMarkList = new List<CProductTradeMark>(); }
                else { objProductTradeMarkList.Clear(); }

                if (objProductTypeList == null) { objProductTypeList = new List<CProductType>(); }
                else { objProductTypeList.Clear(); }

                System.Data.DataTable dtProductTradeMarkProductTypeList = CSalesPlanQuotaDataBaseModel.GetProductTradeMarkProductTypeListForActiveCalcPlan(objProfile, null,
                    ref strErr, ref iRes);


                if ((dtProductTradeMarkProductTypeList != null) && (iRes == 0))
                {
                    foreach (System.Data.DataRow objItem in dtProductTradeMarkProductTypeList.Rows)
                    {
                        if (System.Convert.ToInt32(System.Convert.ToString(objItem["ObjectTypeId"])) == 0)
                        {
                            objProductTradeMarkList.Add( new CProductTradeMark() 
                            {
                                ID = new Guid(System.Convert.ToString(objItem["ObjectGuid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["ObjectId"])),
                                Name = System.Convert.ToString(objItem["ObjectName"])
                            } );
                        }
                        else if (System.Convert.ToInt32(System.Convert.ToString(objItem["ObjectTypeId"])) == 1)
                        {
                            objProductTypeList.Add(new CProductType()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["ObjectGuid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["ObjectId"])),
                                Name = System.Convert.ToString(objItem["ObjectName"])
                            });
                        }
    
                    }
                }

                dtProductTradeMarkProductTypeList = null;
            }
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return ;
        }
        
        
        /// <summary>
        /// Расчёт приложения
        /// </summary>
        /// <param name="objSalesPlanQuotaCalculationConditions">условия для расчёта</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>приложение к расчёту</returns>
        public static List<CSalesPlanQuotaItemForGrid> CalcSalesPlanQuota(UniXP.Common.CProfile objProfile, 
            System.Guid SalesPlanQuota_Guid, 
            CSalesPlanQuotaCalculationConditions objSalesPlanQuotaCalculationConditions,
            ref System.String strErr, ref System.Int32 iRes )
        {
            List<CSalesPlanQuotaItemForGrid> objList = new List<CSalesPlanQuotaItemForGrid>();

            try
            {
                System.Data.DataTable dtProductTradeMarkList = new System.Data.DataTable();
                System.Data.DataTable dtProductTypeList = new System.Data.DataTable();
                System.Data.DataRow newRow = null;

                dtProductTradeMarkList.Columns.Add(new System.Data.DataColumn("Item_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtProductTypeList.Columns.Add(new System.Data.DataColumn("Item_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                if ((objSalesPlanQuotaCalculationConditions != null)  &&
                    (objSalesPlanQuotaCalculationConditions.ProductTradeMarkList != null) && 
                    (objSalesPlanQuotaCalculationConditions.ProductTradeMarkList.Count > 0))
                {
                    foreach (CProductTradeMark objItem in objSalesPlanQuotaCalculationConditions.ProductTradeMarkList )
                    {
                        newRow = dtProductTradeMarkList.NewRow();

                        newRow["Item_Guid"] = objItem.ID;

                        dtProductTradeMarkList.Rows.Add(newRow);
                    }
                }
                dtProductTradeMarkList.AcceptChanges();

                if ((objSalesPlanQuotaCalculationConditions != null) &&
                    (objSalesPlanQuotaCalculationConditions.ProductTypeList != null) &&
                    (objSalesPlanQuotaCalculationConditions.ProductTypeList.Count > 0))
                {
                    foreach (CProductType objItem in objSalesPlanQuotaCalculationConditions.ProductTypeList)
                    {
                        newRow = dtProductTypeList.NewRow();

                        newRow["Item_Guid"] = objItem.ID;

                        dtProductTypeList.Rows.Add(newRow);
                    }
                }
                dtProductTypeList.AcceptChanges();

                System.Data.DataTable dtSalesPlanItemQuota = CSalesPlanQuotaDataBaseModel.CalcSalesPlanQuotaItemList(objProfile, null,
                    SalesPlanQuota_Guid, objSalesPlanQuotaCalculationConditions.CalcPeriodBeginDate, objSalesPlanQuotaCalculationConditions.CalcPeriodEndDate, 
                    dtProductTradeMarkList, dtProductTypeList, ref strErr, ref iRes);

                if ((dtSalesPlanItemQuota != null) && (dtSalesPlanItemQuota.Rows.Count > 0))
                {
                    CSalesPlanQuotaItemForGrid objSalesPlanQuotaItem = null;
                    System.Guid SalesPlanQuotaItem_Guid = System.Guid.Empty;

                    foreach (System.Data.DataRow objItem in dtSalesPlanItemQuota.Rows)
                    {
                        objSalesPlanQuotaItem = new CSalesPlanQuotaItemForGrid()
                        {

                            ProductTradeMark = new CProductTradeMark()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["ProductOwner_Guid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Owner_Id"])),
                                Name = System.Convert.ToString(objItem["Owner_Name"])
                            },
                            ProductType = new CProductType()
                            {
                                ID = new Guid(System.Convert.ToString(objItem["ProductType_Guid"])),
                                ID_Ib = System.Convert.ToInt32(System.Convert.ToString(objItem["Parttype_Id"])),
                                Name = System.Convert.ToString(objItem["Parttype_Name"])
                            },
                            SalesMoney = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItem_Money"])),
                            SalesQuantity = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItem_Quantity"])),
                            Object_QuotaObjectType = (enQuotaObjectType)System.Convert.ToInt32(System.Convert.ToString(objItem["ObjectType_Id"])), 
                            Object_ID = new Guid(System.Convert.ToString(objItem["ObjectDecode_Guid"])),
                            Object_Name = System.Convert.ToString(objItem["ObjectDecode_Name"]), 
                            Object_SalesQuantity = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItemDecode_Quantity"])),
                            Object_SalesMoney = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItemDecode_Money"])), 
                            Object_CalcQuota = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItemDecode_CalcQuota"])), 
                            Object_Quota = System.Convert.ToDecimal(System.Convert.ToString(objItem["SalesPlanQuotaItemDecode_Quota"])) 

                        };

                        objList.Add(objSalesPlanQuotaItem);
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

        #region Удаление расчета
        /// <summary>
        /// Удаление расчёта из БД
        /// </summary>
        /// <param name="ID">УИ расчёта</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возарата процедуры БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean DeleteSalesPlanQuota( UniXP.Common.CProfile objProfile, System.Guid ID, ref System.String strErr, ref System.Int32 iRes )
        {
            System.Boolean bRet = false;

            try
            {
                bRet = CSalesPlanQuotaDataBaseModel.DeleteSalesPlanQuotaToDB(objProfile, null, ID, ref strErr, ref iRes);
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
        /// Проверка свойств объекта "Расчёт" перед сохранением в БД
        /// </summary>
        /// <param name="Name">Наименование</param>
        /// <param name="Date">Дата</param>
        /// <param name="CalculationConditions">Условия расчёта коэффициентов</param>
        /// <param name="SalesPlanQuotaItemList">Приложение к расчёту</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все свойства удовлетворяют требованиям; false - не выполняются условия к значениям свойств</returns>
        public static System.Boolean IsAllParametersValid(System.String Name, System.DateTime Date, 
            CSalesPlanQuotaCalculationConditions CalculationConditions, List<CSalesPlanQuotaItem> SalesPlanQuotaItemList, 
            ref System.String strErr )
        {
            System.Boolean bRet = false;
            System.Int32 iWarningCount = 0;
            try
            {
                if (Name.Trim() == "")
                {
                    strErr += ("\nНеобходимо указать наименование расчёта");
                    iWarningCount++;
                }
                if (Date.CompareTo(System.DateTime.MinValue) == 0)
                {
                    strErr += ("\nНеобходимо указать дату расчёта!");
                    iWarningCount++;
                }
                if (CalculationConditions == null)
                {
                    strErr += ("\nНе заданы условия расчёта!");
                    iWarningCount++;
                }
                else
                {
                    if (CalculationConditions.CalcPeriodBeginDate.CompareTo(System.DateTime.MinValue) == 0)
                    {
                        strErr += ("\nНеобходимо указать дату начала периода продаж!");
                        iWarningCount++;
                    }
                    if (CalculationConditions.CalcPeriodEndDate.CompareTo(System.DateTime.MinValue) == 0)
                    {
                        strErr += ("\nНеобходимо указать дату окончания периода продаж!");
                        iWarningCount++;
                    }
                    if ((CalculationConditions.ProductTradeMarkList == null) || (CalculationConditions.ProductTradeMarkList.Count == 0))
                    {
                        strErr += ("\nСписок товарных марок для расчёта не должен быть пустым!");
                        iWarningCount++;
                    }
                    if ((CalculationConditions.ProductTypeList == null) || (CalculationConditions.ProductTypeList.Count == 0))
                    {
                        strErr += ("\nСписок товарных групп для расчёта не должен быть пустым!");
                        iWarningCount++;
                    }
                }
                if ((SalesPlanQuotaItemList == null) || (SalesPlanQuotaItemList.Count == 0))
                {
                    strErr += ("\nПриложение к расчёту не должно быть пустым!");
                    iWarningCount++;
                }

                bRet = (iWarningCount == 0);
            }
            catch (System.Exception f)
            {
                strErr += (String.Format("Ошибка проверки свойств объекта 'Расчёт'. Текст ошибки: {0}", f.Message));
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
        /// <param name="SalesPlanQuota_Guid">Уникальный идентификатор</param>
        /// <param name="SalesPlanQuota_Name">Наименование</param>
        /// <param name="SalesPlanQuota_Date">Дата</param>
        /// <param name="SalesPlanQuota_BeginDate">Дата начала периода продаж</param>
        /// <param name="SalesPlanQuota_EndDate">Дата окончания периода продаж</param>
        /// <param name="SalesPlanQuota_Description">Описание</param>
        /// <param name="SalesPlanQuota_Condition">Условия расчёта коэффициентов</param>
        /// <param name="SalesPlanQuotaItemList">Приложение к расчёту</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <param name="objectIDinDB">УИ объекта в БД</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveToDB( System.Boolean bNewObject, UniXP.Common.CProfile objProfile, System.Guid SalesPlanQuota_Guid, 
            System.String SalesPlanQuota_Name, System.DateTime SalesPlanQuota_Date, 
            System.DateTime SalesPlanQuota_BeginDate, System.DateTime SalesPlanQuota_EndDate,
            System.String SalesPlanQuota_Description,
            CSalesPlanQuotaCalculationConditions SalesPlanQuota_Condition, 
            List<CSalesPlanQuotaItem> SalesPlanQuotaItemList,
            ref System.String strErr, ref System.Int32 iRes, ref System.Guid objectIDinDB)
        {
            System.Boolean bRet = false;
            try
            {
                System.Data.DataTable dtSalesPlanQuotaItemList = new System.Data.DataTable();
                System.Data.DataRow newRow = null;

                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuota_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("ProductOwner_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("ProductType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItem_Money", typeof(System.Data.SqlTypes.SqlMoney)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("ObjectType_Id", typeof(System.Data.SqlTypes.SqlInt32)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("ObjectDecode_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Quantity", typeof(System.Data.SqlTypes.SqlDecimal)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Money", typeof(System.Data.SqlTypes.SqlMoney)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_CalcQuota", typeof(System.Data.SqlTypes.SqlDecimal)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("SalesPlanQuotaItemDecode_Quota", typeof(System.Data.SqlTypes.SqlDecimal)));
                dtSalesPlanQuotaItemList.Columns.Add(new System.Data.DataColumn("ActionType_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                if ((SalesPlanQuotaItemList != null) && (SalesPlanQuotaItemList.Count > 0))
                {
                    foreach (CSalesPlanQuotaItem objItem in SalesPlanQuotaItemList)
                    {
                        if( ( objItem.QuotaList == null ) || ( objItem.QuotaList.Count == 0 ) ) { continue; }

                        foreach( CQuotaItemObject objQuotaItem in objItem.QuotaList )
                        {
                            newRow = dtSalesPlanQuotaItemList.NewRow();

                            newRow["SalesPlanQuotaItemDecode_Guid"] = System.Guid.Empty;
                            newRow["SalesPlanQuotaItem_Guid"] = objItem.ID;
                            newRow["SalesPlanQuota_Guid"] = SalesPlanQuota_Guid;
                            newRow["ProductOwner_Guid"] = objItem.ProductTradeMark.ID;
                            newRow["ProductType_Guid"] = objItem.ProductType.ID;
                            newRow["SalesPlanQuotaItem_Quantity"] = objItem.SalesQuantity;
                            newRow["SalesPlanQuotaItem_Money"] = objItem.SalesMoney;
                            newRow["ObjectType_Id"] = (System.Int32)objQuotaItem.QuotaObjectType;
                            newRow["ObjectDecode_Guid"] = objQuotaItem.ID;
                            newRow["SalesPlanQuotaItemDecode_Quantity"] = objQuotaItem.SalesQuantity;
                            newRow["SalesPlanQuotaItemDecode_Money"] = objQuotaItem.SalesMoney;
                            newRow["SalesPlanQuotaItemDecode_CalcQuota"] = objQuotaItem.CalcQuota;
                            newRow["SalesPlanQuotaItemDecode_Quota"] = objQuotaItem.Quota;
                            newRow["ActionType_Id"] = 0;

                            dtSalesPlanQuotaItemList.Rows.Add(newRow);
                        }
                    }
                }

                dtSalesPlanQuotaItemList.AcceptChanges();

                if (bNewObject == true)
                {
                    bRet = CSalesPlanQuotaDataBaseModel.AddSalesPlanQuotaToDB(objProfile, null, ref SalesPlanQuota_Guid, SalesPlanQuota_Name,
                        SalesPlanQuota_Date, SalesPlanQuota_BeginDate, SalesPlanQuota_EndDate, SalesPlanQuota_Description,
                        SalesPlanQuota_Condition.XMLView.InnerXml, dtSalesPlanQuotaItemList, ref strErr, ref iRes, ref objectIDinDB);
                }
                else
                {
                    bRet = CSalesPlanQuotaDataBaseModel.EditSalesPlanQuotaToDB(objProfile, null, SalesPlanQuota_Guid, SalesPlanQuota_Name,
                        SalesPlanQuota_Date, SalesPlanQuota_BeginDate, SalesPlanQuota_EndDate, SalesPlanQuota_Description,
                        SalesPlanQuota_Condition.XMLView.InnerXml, dtSalesPlanQuotaItemList, ref strErr, ref iRes, ref objectIDinDB);
                }

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("Ошибка сохранения объекта 'Расчёт' в базе данных. Текст ошибки: {0}", f.Message));
            }
            
            return bRet;
        }

        #endregion

        #region Список команд, назначенных товарной марке
        /// <summary>
        /// Возвращает список команд для заданной товарной марки
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objDepartTeamList">список команд</param>
        /// <param name="ProductOwnerGuid">УИ товарной марки</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата процедуры</param>
        public static void GetDepartTeamListForProductOwner(UniXP.Common.CProfile objProfile,
            ref List<CDepartTeam> objDepartTeamList, System.Guid ProductOwnerGuid,
            ref System.String strErr, ref System.Int32 iRes)
        {
            try
            {
                if (objDepartTeamList == null) { objDepartTeamList = new List<CDepartTeam>(); }
                else { objDepartTeamList.Clear(); }

                System.Data.DataTable dtDepartTeamList = CSalesPlanQuotaDataBaseModel.GetDepartTeamListForTradeMark(objProfile, null, ProductOwnerGuid,
                    ref strErr, ref iRes);


                if ((dtDepartTeamList != null) && (iRes == 0))
                {
                    foreach (System.Data.DataRow objItem in dtDepartTeamList.Rows)
                    {
                        objDepartTeamList.Add(new CDepartTeam(new Guid(System.Convert.ToString(objItem["ObjectGuid"])),
                            System.Convert.ToString(objItem["ObjectName"]), System.String.Empty, true));
                    }
                }

                dtDepartTeamList = null;
            }
            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return;
        }
        #endregion

        public override string ToString()
        {
            return (String.Format("{0} {1}", Name, CalcPeriod));
        }

    }
}
