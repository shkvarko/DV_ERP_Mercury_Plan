using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP_Mercury.Common;
using System.ComponentModel;

namespace ERPMercuryPlan
{
    public enum enumProductSubTypeActionTypeWithImage
    {
        Nothing = 0,
        EditImage = 1,
        DeleteImage = 2
    }

    /// <summary>
    /// Класс "Состояние товарной подгруппы"
    /// </summary>
    public class CProductSubTypeState : CBusinessObject
    {
        #region Свойства
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
        /// признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// признак "Активен"
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        #endregion

        #region Конструктор
        public CProductSubTypeState()
            : base()
        {
            m_strDescription = "";
            m_bIsActive = false;
        }
        public CProductSubTypeState(System.Guid uuidId, System.String strName, System.String strDesrpn, System.Boolean bActive)
        {
            ID = uuidId;
            Name = strName;
            m_strDescription = strDesrpn;
            m_bIsActive = bActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список состояний товарных подгрупп
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список состояний товарных подгрупп</returns>
        public static List<CProductSubTypeState> GetProductSubTypeStateList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductSubTypeState> objList = new List<CProductSubTypeState>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsubtypeState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductSubTypeState(
                              (System.Guid)rs["PartsubtypeState_Guid"],
                              (System.String)rs["PartsubtypeState_Name"],
                              ((rs["PartsubtypeState_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsubtypeState_Description"]),
                              System.Convert.ToBoolean(rs["PartsubtypeState_IsActive"])
                              ));
                    }
                }
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
                "Не удалось получить список состояний товарных подгрупп.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Add
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Add(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            try
            {
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать запись.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
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
        public override System.Boolean Remove(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            try
            {
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить запись.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
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
        public override System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            try
            {
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства записи. Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return bRet;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }

    }
    /// <summary>
    /// Класс "Товарная подгруппа"
    /// </summary>
    public class CProductSubType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// TypeConverter для списка товарных линий
        /// </summary>
        class ProductLineTypeConverter : TypeConverter
        {
            /// <summary>
            /// Будем предоставлять выбор из списка
            /// </summary>
            public override bool GetStandardValuesSupported(
              ITypeDescriptorContext context)
            {
                return true;
            }
            /// <summary>
            /// ... и только из списка
            /// </summary>
            public override bool GetStandardValuesExclusive(
              ITypeDescriptorContext context)
            {
                // false - можно вводить вручную
                // true - только выбор из списка
                return true;
            }

            /// <summary>
            /// А вот и список
            /// </summary>
            public override StandardValuesCollection GetStandardValues(
              ITypeDescriptorContext context)
            {
                // возвращаем список строк из настроек программы
                // (базы данных, интернет и т.д.)

                CProductSubType objProductSubType = (CProductSubType)context.Instance;
                System.Collections.Generic.List<CProductLine> objList = objProductSubType.GetAllProductLineList();

                return new StandardValuesCollection(objList);
            }
        }

        /// <summary>
        /// УИ в InterBase
        /// </summary>
        private System.Int32 m_ID_Ib;
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 ID_Ib
        {
            get { return m_ID_Ib; }
            set { m_ID_Ib = value; }
        }
        /// <summary>
        /// Товарная линия
        /// </summary>
        private CProductLine m_objProductLine;
        /// <summary>
        /// Товарная линия
        /// </summary>
        [DisplayName("Товарная линия")]
        [Description("Товарная линия")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(ProductLineTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CProductLine ProductLine
        {
            get { return m_objProductLine; }
            set { m_objProductLine = value; }
        }
        /// <summary>
        /// Товарная линия
        /// </summary>
        [DisplayName("Товарная линия")]
        [Description("Товарная линия")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(ProductLineTypeConverter))]
        public System.String ProductLineName
        {
            get { return m_objProductLine.Name; }
            set { SetProductLineValue(value); }
        }
        private void SetProductLineValue(System.String strProductLineName)
        {
            try
            {
                if (m_objAllProductLineList == null) { m_objProductLine = null; }
                else
                {
                    foreach (CProductLine objProductLine in m_objAllProductLineList)
                    {
                        if (objProductLine.Name == strProductLineName)
                        {
                            m_objProductLine = objProductLine;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение товарной линии.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        /// <summary>
        /// Тариф поставщика
        /// </summary>
        private System.Double m_dblVendorTariff;
        /// <summary>
        /// Тариф поставщика
        /// </summary>
        [DisplayName("Тариф поставщика")]
        [Description("Тариф поставщика")]
        [Category("1. Обязательные значения")]
        public System.Double VendorTariff
        {
            get { return m_dblVendorTariff; }
            set { m_dblVendorTariff = value; }
        }
        /// <summary>
        /// Величина (процент) расходов на транспорт
        /// </summary>
        private System.Double m_dblTransportTariff;
        /// <summary>
        /// Величина (процент) расходов на транспорт
        /// </summary>
        [DisplayName("Тариф транспортный, %")]
        [Description("Величина (процент) расходов на транспорт")]
        [Category("1. Обязательные значения")]
        public System.Double TransportTariff
        {
            get { return m_dblTransportTariff; }
            set { m_dblTransportTariff = value; }
        }
        /// <summary>
        /// Тариф таможенный
        /// </summary>
        private System.Double m_dblCustomsTariff;
        /// <summary>
        /// Тариф таможенный
        /// </summary>
        [DisplayName("Тариф таможенный, %")]
        [Description("Величина (процент) расходов на транспорт")]
        [Category("1. Обязательные значения")]
        public System.Double CustomsTariff
        {
            get { return m_dblCustomsTariff; }
            set { m_dblCustomsTariff = value; }
        }
        /// <summary>
        /// Наценка базовая, %"
        /// </summary>
        private System.Double m_dblMargin;
        /// <summary>
        /// Наценка базовая, %"
        /// </summary>
        [DisplayName("Наценка базовая, %")]
        [Description("Наценка базовая (процент)")]
        [Category("1. Обязательные значения")]
        public System.Double Margin
        {
            get { return m_dblMargin; }
            set { m_dblMargin = value; }
        }
        /// <summary>
        /// Ставка НДС
        /// </summary>
        private System.Double m_dblNDS;
        /// <summary>
        /// Ставка НДС
        /// </summary>
        [DisplayName("НДС, %")]
        [Description("ставка НДС (процент)")]
        [Category("1. Обязательные значения")]
        public System.Double NDS
        {
            get { return m_dblNDS; }
            set { m_dblNDS = value; }
        }
        /// <summary>
        /// Наценка, компенсирующая постоянную скидку
        /// </summary>
        private System.Double m_dblDiscont;
        /// <summary>
        /// Наценка, компенсирующая постоянную скидку
        /// </summary>
        [DisplayName("Наценка, компенсирующая постоянную скидку, %")]
        [Description("Дисконт (средняя сложившаяся скидка)")]
        [Category("1. Обязательные значения")]
        public System.Double Discont
        {
            get { return m_dblDiscont; }
            set { m_dblDiscont = value; }
        }
        /// <summary>
        /// Требуемая наценка
        /// </summary>
        [DisplayName("Требуемая наценка, %")]
        [Description("Требуемая наценка")]
        [Category("1. Обязательные значения")]
        public System.Double MarkUpRequired { get; set; }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        private List<CProductLine> m_objAllProductLineList;
        public List<CProductLine> GetAllProductLineList()
        {
            return m_objAllProductLineList;
        }
        /// <summary>
        /// Товарная марка
        /// </summary>
        private System.String m_strProductOwner;
        /// <summary>
        /// Товарная марка
        /// </summary>
        [DisplayName("Товарная марка")]
        [Description("Товарная марка")]
        [Category("3. Структура")]
        [ReadOnly(true)]
        public System.String ProductOwner
        {
            get { return m_strProductOwner; }
            set { m_strProductOwner = value; }
        }
        /// <summary>
        /// Товарная группа
        /// </summary>
        private System.String m_strProductType;
        /// <summary>
        /// Товарная группа
        /// </summary>
        [DisplayName("Товарная группа")]
        [Description("Товарная группа")]
        [Category("3. Структура")]
        [ReadOnly(true)]
        [Browsable(false)]
        public System.String ProductType
        {
            get { return m_strProductType; }
            set { m_strProductType = value; }
        }
        /// <summary>
        /// Товарная группа
        /// </summary>
        private System.Double m_moneyPriceEXW;
        /// <summary>
        /// Товарная группа
        /// </summary>
        [DisplayName("Цена exw")]
        [Description("Цена exw")]
        [Category("3. Структура")]
        [ReadOnly(true)]
        [Browsable(false)]
        public System.Double PriceEXW
        {
            get { return m_moneyPriceEXW; }
            set { m_moneyPriceEXW = value; }
        }
        /// <summary>
        /// Состояние
        /// </summary>
        private CProductSubTypeState m_objProductSubTypeState;
        /// <summary>
        /// Состояние
        /// </summary>
        public CProductSubTypeState SubTypeState
        {
            get { return m_objProductSubTypeState; }
            set { m_objProductSubTypeState = value; }
        }
        /// <summary>
        /// Состояние
        /// </summary>
        public System.String SubTypeStateName
        {
            get { return ( ( m_objProductSubTypeState == null ) ? "" : m_objProductSubTypeState.Name ); }
        }
        /// <summary>
        /// Признак "Товарная подгруппа назначена товару"
        /// </summary>
        private System.Boolean m_bIsSetToPart;
        /// <summary>
        /// Признак "Товарная подгруппа назначена товару"
        /// </summary>
        public System.String IsSetToPart
        {
            get { return ((m_bIsSetToPart == true) ? "Да" : "Нет"); }
            //set { m_bIsSetToPart = value; }
        }
        /// <summary>
        /// Признак "В товарной подгруппе НЕТ товаров с состоянием "подтверждение неактуальности""
        /// </summary>
        private System.Boolean m_bIsContainingPartsActualNotValid;
        /// <summary>
        /// Признак "В товарной подгруппе есть товары с состоянием "подтверждение неактуальности""
        /// </summary>
        public System.String IsContainingOnlyPartsActual
        {
            get { return (( m_bIsContainingPartsActualNotValid == true ) ? "подтверждена неактуальность" : "только актуальные" ); }
            //set { m_bIsContainingPartsActualNotValid = !value; }
        }
        /// <summary>
        /// Признак "В товарной подгруппе НЕТ товаров с состоянием "неактуален""
        /// </summary>
        private System.Boolean m_bIsContainingPartsNotValid;
        private System.Boolean m_bIsContainingPartsValid;
        /// <summary>
        /// Признак "В товарной подгруппе НЕТ товаров с состоянием "неактуален""
        /// </summary>
        public System.String IsContainingOnlyPartsValid
        {
            get { return ((m_bIsContainingPartsValid == true) ? "Да" : "Нет"); }
            //set { m_bIsContainingPartsNotValid = !value; }
        }
        /// <summary>
        /// Остатки
        /// </summary>
        private List<CProductSubTypeStock> m_objProductSubTypeStockList;
        /// <summary>
        /// Остатки
        /// </summary>
        public List<CProductSubTypeStock> ProductSubTypeStockList
        {
            get { return m_objProductSubTypeStockList; }
            set { m_objProductSubTypeStockList = value; }
        }
        /// <summary>
        /// Количество остатка на складах отгрузки
        /// </summary>
        private System.Int32 m_iQuantityInWarehouseForShipping;
        /// <summary>
        /// Количество остатка на складах отгрузки
        /// </summary>
        public System.Int32 QuantityInWarehouseForShipping
        {
            get { return m_iQuantityInWarehouseForShipping; }
            set { m_iQuantityInWarehouseForShipping = value; }
        }
        /// <summary>
        /// Наименование файла с изображением
        /// </summary>
        public System.String ImageProductSubTypeFileName { get; set; }
        /// <summary>
        /// Изображение
        /// </summary>
        public byte[] ImageProductSubType { get; set; }
        /// <summary>
        /// Признак "Наличие изображения"
        /// </summary>
        public System.Boolean ExistImage { get; set; }
        /// <summary>
        /// Действие над изображением
        /// </summary>
        public enumProductSubTypeActionTypeWithImage ActionType { get; set; }
       
        public const System.Int32 iCommandTimeoutIB = 720;
        #endregion

        #region Конструктор
        public CProductSubType()
            : base()
        {
            m_ID_Ib = 0;
            m_strDescription = "";
            m_objProductLine = null;
            m_objAllProductLineList = null;
            m_bIsActive = false;
            m_moneyPriceEXW = 0;
            m_strProductOwner = "";
            m_strProductType = "";
            m_dblCustomsTariff = 0;
            m_dblDiscont = 0;
            m_dblMargin = 0;
            m_dblNDS = 0;
            m_dblTransportTariff = 0;
            m_dblVendorTariff = 0;
            m_objProductSubTypeState = null;
            m_bIsSetToPart = false;
            m_bIsContainingPartsActualNotValid = false;
            m_bIsContainingPartsNotValid = false;
            m_bIsContainingPartsValid = false;
            m_objProductSubTypeStockList = null;
            m_iQuantityInWarehouseForShipping = 0;
            MarkUpRequired = 0;
            ImageProductSubTypeFileName = System.String.Empty;
            ImageProductSubType = null;
            ExistImage = false;
            ActionType = enumProductSubTypeActionTypeWithImage.Nothing;
        }
        public CProductSubType(System.Guid uuidId, System.String strName, System.Int32 iID_Ib,
          System.String strDescription, System.Boolean bIsActive, CProductLine objProductLine,
            System.Double dblCustomsTariff, System.Double dblDiscont, System.Double dblMargin,
            System.Double dblNDS, System.Double dblTransportTariff, System.Double dblVendorTariff,
            CProductSubTypeState objProductSubTypeState )
        {
            ID = uuidId;
            Name = strName;
            m_objProductLine = objProductLine;
            m_objAllProductLineList = null;
            m_ID_Ib = iID_Ib;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
            m_moneyPriceEXW = 0;
            m_strProductOwner = "";
            m_strProductType = "";
            m_dblCustomsTariff = dblCustomsTariff;
            m_dblDiscont = dblDiscont;
            m_dblMargin = dblMargin;
            m_dblNDS = dblNDS;
            m_dblTransportTariff = dblTransportTariff;
            m_dblVendorTariff = dblVendorTariff;
            m_objProductSubTypeState = objProductSubTypeState;
            m_bIsSetToPart = false;
            m_bIsContainingPartsActualNotValid = false;
            m_bIsContainingPartsNotValid = false;
            m_objProductSubTypeStockList = null;
            MarkUpRequired = 0;
            ImageProductSubTypeFileName = System.String.Empty;
            ImageProductSubType = null;
            ExistImage = false;
            ActionType = enumProductSubTypeActionTypeWithImage.Nothing;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товарных подгрупп
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="bOnlyFullRecords">признак "возвращать только полные записи"</param>
        /// <returns>список товарных подгрупп</returns>
        public static List<CProductSubType> GetProductSubTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Boolean bOnlyFullRecords)
        {
            List<CProductSubType> objList = new List<CProductSubType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CProductSubType objProductSubType = null;
                    while (rs.Read())
                    {
                        objProductSubType = new CProductSubType();

                        objProductSubType.ID = (System.Guid)rs["Partsubtype_Guid"];
                        objProductSubType.Name = (System.String)rs["Partsubtype_Name"];
                        objProductSubType.ID_Ib = System.Convert.ToInt32(rs["Partsubtype_Id"]);
                        objProductSubType.Description = ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]);
                        objProductSubType.IsActive = System.Convert.ToBoolean(rs["Partsubtype_IsActive"]);
                        objProductSubType.ProductLine =  new CProductLine(
                              (System.Guid)rs["Partline_Guid"],
                              (System.String)rs["Partline_Name"],
                              System.Convert.ToInt32(rs["Partline_Id"]),
                              ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                              System.Convert.ToBoolean(rs["Partline_IsActive"])                              
                              );
                        objProductSubType.CustomsTariff = ( ( rs["Partsubtype_CustomsTariff"] == System.DBNull.Value ) ? 0 : System.Convert.ToDouble(rs["Partsubtype_CustomsTariff"]) );
                        objProductSubType.Discont = ((rs["Partsubtype_Discont"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Partsubtype_Discont"]));
                        objProductSubType.Margin = ((rs["Partsubtype_Margin"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Partsubtype_Margin"]));
                        objProductSubType.NDS = ((rs["Partsubtype_NDS"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Partsubtype_NDS"]));
                        objProductSubType.TransportTariff = ((rs["Partsubtype_TransportTariff"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Partsubtype_TransportTariff"]));
                        objProductSubType.VendorTariff = ((rs["Partsubtype_VendorTariff"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Partsubtype_VendorTariff"]));
                        objProductSubType.MarkUpRequired = ((rs["Partsubtype_MarkUpRequired"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Partsubtype_MarkUpRequired"]));
                        objProductSubType.SubTypeState = ((rs["PartsubtypeState_Guid"] == System.DBNull.Value) ? null : new CProductSubTypeState(
                              (System.Guid)rs["PartsubtypeState_Guid"],
                              (System.String)rs["PartsubtypeState_Name"],
                              ((rs["PartsubtypeState_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsubtypeState_Description"]),
                              System.Convert.ToBoolean(rs["PartsubtypeState_IsActive"])
                              ));
                        
                        objProductSubType.m_bIsContainingPartsActualNotValid = System.Convert.ToBoolean(rs["ContainingPartsActualNotValid"]);
                        objProductSubType.m_bIsContainingPartsNotValid = System.Convert.ToBoolean(rs["ContainingPartsNotValid"]);
                        objProductSubType.m_bIsContainingPartsValid = System.Convert.ToBoolean(rs["ContainingPartsActual"]);
                        if (rs["Owner_Name"] != System.DBNull.Value)
                        {
                            objProductSubType.m_strProductOwner = (System.String)rs["Owner_Name"];
                            objProductSubType.m_bIsSetToPart = true;
                        }
                        if (rs["Parttype_Name"] != System.DBNull.Value)
                        {
                            objProductSubType.m_strProductType = (System.String)rs["Parttype_Name"];
                        }
                        if (rs["PartSubtypePriceEXW"] != System.DBNull.Value)
                        {
                            objProductSubType.m_moneyPriceEXW = System.Convert.ToDouble(rs["PartSubtypePriceEXW"]);
                        }
                        else
                        {
                            objProductSubType.m_moneyPriceEXW = 0;
                        }
                        if (rs["StockQtyInWarehouseForShipping"] != System.DBNull.Value)
                        {
                            objProductSubType.QuantityInWarehouseForShipping = System.Convert.ToInt32(rs["StockQtyInWarehouseForShipping"]);
                        }

                        objProductSubType.ExistImage = ((rs["ExistImage"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["ExistImage"]));
                        objProductSubType.ImageProductSubTypeFileName = ((rs["Partsubtype_ImageFileFullName"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Partsubtype_ImageFileFullName"]));

                        if (bOnlyFullRecords == true)
                        {
                            if ((objProductSubType.ProductOwner != "") && (objProductSubType.ProductType != "") && (objProductSubType.Name != ""))
                            {
                                objList.Add(objProductSubType);
                            }
                        }
                        else
                        {
                            objList.Add(objProductSubType);
                        }
                    }
                }
                rs.Dispose();

                // товарные подгруппы исключены из справочника в ctrlDatabaseDirectory, поэтому выпадающий список товарных линий инициализировать нет нужды
                //List<CProductLine> objProductLineList = CProductLine.GetProductLineList(objProfile, cmd);
                //if ((objList != null) && (objProductLineList != null))
                //{
                //    foreach (CProductSubType objSubType in objList)
                //    {
                //        objSubType.m_objAllProductLineList = objProductLineList;
                //    }
                //}
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товарных подгрупп.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает в m_objAllProductLineList список товарных линий
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitProductLineList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllProductLineList = CProductLine.GetProductLineList(objProfile, cmdSQL);
                if ((this.m_objAllProductLineList != null) && (this.m_objAllProductLineList.Count > 0))
                {
                    this.ProductLine = this.m_objAllProductLineList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список товарных линий.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Загружает из БД изображение товарной подгруппы
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Partsubtype_Guid">УИ товарной подгруппы</param>
        /// <param name="arAttach">изображение в виде массива байт</param>
        /// <param name="strPartsubtypeFileName">название файла с изображением</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean LoadImageFromDB(UniXP.Common.CProfile objProfile, System.Guid Partsubtype_Guid,
            ref byte[] arAttach, ref System.String strPartsubtypeFileName,
            ref System.String strErr)
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
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand()
                {
                    Connection = DBConnection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsubtypeImage]", objProfile.GetOptionsDllDBName())
                };
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Guid"].Value = Partsubtype_Guid;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    // набор данных непустой
                    rs.Read();
                    if (rs["Partsubtype_Image"] != System.DBNull.Value)
                    {
                        strPartsubtypeFileName = ((rs["Partsubtype_ImageFileFullName"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Partsubtype_ImageFileFullName"]));
                        arAttach = (byte[])rs["Partsubtype_Image"];
                    }

                }
                rs.Close();
                rs.Dispose();
                bRet = true;

            }
            catch (System.Exception f)
            {
                strErr = "Не удалось получить изображение товарной подгруппы. Текст ошибки: " + f.Message;
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region IsAllParametersValid
        /// <summary>
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public override System.Boolean IsAllParametersValid()
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
                if (this.m_objProductLine == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать товарную линию!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.SubTypeState == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать состояние тованой подгруппы!", "Внимание",
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
        public override System.Boolean Add(UniXP.Common.CProfile objProfile)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@Partsubtype_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@Partsubtype_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@Partsubtype_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@Partsubtype_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@Partline_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@PartsubtypeState_Guid", System.Data.DbType.Guid));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_VendorTariff", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_TransportTariff", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_CustomsTariff", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Margin", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_NDS", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Discont", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_MarkUpRequired", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters[ "@ERROR_MES" ].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters[ "@Partsubtype_Name" ].Value = this.Name;
                cmd.Parameters[ "@Partsubtype_IsActive" ].Value = this.IsActive;
                cmd.Parameters[ "@Partsubtype_Description" ].Value = this.Description;
                cmd.Parameters[ "@Partline_Guid" ].Value = this.ProductLine.ID;
                cmd.Parameters[ "@PartsubtypeState_Guid" ].Value = this.SubTypeState.ID;
                cmd.Parameters["@Partsubtype_VendorTariff"].Value = this.VendorTariff;
                cmd.Parameters["@Partsubtype_TransportTariff"].Value = this.TransportTariff;
                cmd.Parameters["@Partsubtype_CustomsTariff"].Value = this.CustomsTariff;
                cmd.Parameters["@Partsubtype_Margin"].Value = this.Margin;
                cmd.Parameters["@Partsubtype_NDS"].Value = this.NDS;
                cmd.Parameters["@Partsubtype_Discont"].Value = this.Discont;
                cmd.Parameters["@Partsubtype_MarkUpRequired"].Value = this.MarkUpRequired;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Partsubtype_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Partsubtype_Id"].Value);
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания товарной подгруппы.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать товарную подгруппу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Добавить запись в БД в IB
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddToIB(UniXP.Common.CProfile objProfile, ref System.String strErr )
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
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
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandTimeout = CProductSubType.iCommandTimeoutIB; 
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartSubTypeToIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PartSubType_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if( (iRes == 0) || (iRes == 1) )
                {
                    bRet = true;
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    //DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания товарной подгруппы в IB.\n\nТекст ошибки: " + strErr, "Ошибка",
                    //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                strErr = ("Не удалось создать товарную подгруппу. Текст ошибки: " + f.Message );
                //DevExpress.XtraEditors.XtraMessageBox.Show(
                //"Не удалось создать товарную подгруппу.\n\nТекст ошибки: " + f.Message, "Внимание",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
        public override System.Boolean Remove(UniXP.Common.CProfile objProfile)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartsubtype]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления товарной подгруппы.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить товарную подгруппу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean RemoveFromIB(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
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
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandTimeout = CProductSubType.iCommandTimeoutIB;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartSubTypeToIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartSubType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PartSubType_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    // откатываем транзакцию
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    //DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления товарной подгруппы.\n\nТекст ошибки: " +
                    //(System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                    //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                strErr = ("Не удалось удалить товарную подгруппу. Текст ошибки: " + f.Message);
                //DevExpress.XtraEditors.XtraMessageBox.Show(
                //"Не удалось удалить товарную подгруппу.\n\nТекст ошибки: " + f.Message, "Внимание",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
        public override System.Boolean Update(UniXP.Common.CProfile objProfile)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypeState_Guid", System.Data.DbType.Guid));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_VendorTariff", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_TransportTariff", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_CustomsTariff", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Margin", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_NDS", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Discont", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_MarkUpRequired", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Guid"].Value = this.ID;
                cmd.Parameters["@Partline_Guid"].Value = this.ProductLine.ID;
                cmd.Parameters["@PartsubtypeState_Guid"].Value = this.SubTypeState.ID;
                cmd.Parameters["@Partsubtype_Name"].Value = this.Name;
                cmd.Parameters["@Partsubtype_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Partsubtype_Description"].Value = this.Description;

                cmd.Parameters["@Partsubtype_VendorTariff"].Value = this.VendorTariff;
                cmd.Parameters["@Partsubtype_TransportTariff"].Value = this.TransportTariff;
                cmd.Parameters["@Partsubtype_CustomsTariff"].Value = this.CustomsTariff;
                cmd.Parameters["@Partsubtype_Margin"].Value = this.Margin;
                cmd.Parameters["@Partsubtype_NDS"].Value = this.NDS;
                cmd.Parameters["@Partsubtype_Discont"].Value = this.Discont;
                cmd.Parameters["@Partsubtype_MarkUpRequired"].Value = this.MarkUpRequired;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения товарной подгруппы. Текст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства товарной подгруппы. Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        public static System.Boolean SetPropertiesToSubTypeList(UniXP.Common.CProfile objProfile, List<CProductSubType> objProductSubTypeList, CProductLine objProductLine, 
            CProductSubTypeState objProductSubtypeState, System.Double dblVendorTariff, System.Double dblTransportTariff, 
            System.Double dblCustomsTariff, System.Double dblMargin, System.Double dblNDS, System.Double dblDiscont,
            System.Double dblPriceEXW, System.Double dblMarkUpRequired)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("ProductSubType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("ProductSubType_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRow = null;
                if ((objProductSubTypeList != null) && (objProductSubTypeList.Count > 0))
                {
                    foreach (CProductSubType objItem in objProductSubTypeList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["ProductSubType_Guid"] = objItem.ID;
                        newRow["ProductSubType_Id"] = objItem.ID_Ib;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartSubTypeList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tProductSubType", addedCategories);
                cmd.Parameters["@tProductSubType"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProductSubType"].TypeName = "dbo.udt_ProductSubType";
                if (objProductLine != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@Partline_Guid"].Value = objProductLine.ID;
                }
                if (objProductSubtypeState != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypeState_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@PartsubtypeState_Guid"].Value = objProductSubtypeState.ID;
                }
                if (dblVendorTariff >= 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_VendorTariff", System.Data.SqlDbType.Money));
                    cmd.Parameters["@Partsubtype_VendorTariff"].Value = dblVendorTariff;
                }
                if (dblTransportTariff >= 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_TransportTariff", System.Data.SqlDbType.Money));
                    cmd.Parameters["@Partsubtype_TransportTariff"].Value = dblTransportTariff;
                }
                if (dblCustomsTariff >= 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_CustomsTariff", System.Data.SqlDbType.Money));
                    cmd.Parameters["@Partsubtype_CustomsTariff"].Value = dblCustomsTariff;
                }
                if( dblMargin >= 0 )
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Margin", System.Data.SqlDbType.Money));
                    cmd.Parameters["@Partsubtype_Margin"].Value = dblMargin;
                }
                if( dblNDS >= 0 )
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_NDS", System.Data.SqlDbType.Money));
                    cmd.Parameters["@Partsubtype_NDS"].Value = dblNDS;
                }
                if( dblDiscont >= 0 )
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Discont", System.Data.SqlDbType.Money));
                    cmd.Parameters["@Partsubtype_Discont"].Value = dblDiscont;
                }
                if (dblPriceEXW >= 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_PriceEXW", System.Data.SqlDbType.Money));
                    cmd.Parameters["@Partsubtype_PriceEXW"].Value = dblPriceEXW;
                }
                if (dblMarkUpRequired >= 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_MarkUpRequired", System.Data.SqlDbType.Money));
                    cmd.Parameters["@Partsubtype_MarkUpRequired"].Value = dblMarkUpRequired;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения товарных подгрупп. Текст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства товарных подгрупп. Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        public static System.Boolean SetPropertiesToSubTypeListInIB(UniXP.Common.CProfile objProfile, List<CProductSubType> objProductSubTypeList, CProductLine objProductLine,
            ref System.String strErr)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("ProductSubType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("ProductSubType_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRow = null;
                if ((objProductSubTypeList != null) && (objProductSubTypeList.Count > 0))
                {
                    foreach (CProductSubType objItem in objProductSubTypeList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["ProductSubType_Guid"] = objItem.ID;
                        newRow["ProductSubType_Id"] = objItem.ID_Ib;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandTimeout = CProductSubType.iCommandTimeoutIB;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartSubTypeListInIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tProductSubType", addedCategories);
                cmd.Parameters["@tProductSubType"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProductSubType"].TypeName = "dbo.udt_ProductSubType";
                if (objProductLine != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@Partline_Guid"].Value = objProductLine.ID;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    //DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения товарных подгрупп. Текст ошибки: " + strErr, "Ошибка",
                    //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                strErr = ("Не удалось изменить свойства товарных подгрупп в IB. Текст ошибки: " + f.Message );
                //DevExpress.XtraEditors.XtraMessageBox.Show(
                //"Не удалось изменить свойства товарных подгрупп. Текст ошибки: " + f.Message, "Внимание",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        /// <summary>
        /// Изменяет значение цены exw
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditPrices(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            //System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
                {
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
                    //DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    //cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartSubTypePrice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_PriceEXW", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Guid"].Value = this.ID;
                cmd.Parameters["@Partsubtype_PriceEXW"].Value = this.m_moneyPriceEXW;
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
                        //if (DBTransaction != null)
                        //{
                        //    DBTransaction.Commit();
                        //}
                    }
                    else
                    {
                        // откатываем транзакцию
                        //if (DBTransaction != null)
                        //{
                        //    DBTransaction.Rollback();
                        //}
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                //if ((cmdSQL == null) && (DBTransaction != null))
                //{
                //    DBTransaction.Rollback();
                //}
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

        public static System.Boolean SetPartSubTypeToParts(UniXP.Common.CProfile objProfile, List<CProduct> objProductList, CProductSubType objProductSubType)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Product_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Product_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRow = null;
                if ((objProductList != null) && (objProductList.Count > 0))
                {
                    foreach (CProduct objItem in objProductList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["Product_Guid"] = objItem.ID;
                        newRow["Product_Id"] = objItem.ID_Ib;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_SetPartSubtypeToPartList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tProduct", addedCategories);
                cmd.Parameters["@tProduct"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProduct"].TypeName = "dbo.udt_Product";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartSubType_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@PartSubType_Guid"].Value = objProductSubType.ID;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка назначения подгруппы списку товаров. Текст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось назначить подгруппу списку товаров. Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        public static System.Boolean SetPartSubTypeToPartsInIB(UniXP.Common.CProfile objProfile, List<CProduct> objProductList,
            CProductSubType objProductSubType, ref System.String strErr)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Product_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Product_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRow = null;
                if ((objProductList != null) && (objProductList.Count > 0))
                {
                    foreach (CProduct objItem in objProductList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["Product_Guid"] = objItem.ID;
                        newRow["Product_Id"] = objItem.ID_Ib;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandTimeout = CProductSubType.iCommandTimeoutIB;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_SetPartsListToPartSubTypeInIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tProduct", addedCategories);
                cmd.Parameters["@tProduct"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProduct"].TypeName = "dbo.udt_Product";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartSubType_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@PartSubType_Guid"].Value = objProductSubType.ID;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    //DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка назначения подгруппы списку товаров. Текст ошибки: " + strErr, "Ошибка",
                    //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                strErr = ("Не удалось назначить подгруппу списку товаров. Текст ошибки: " + f.Message);
                //DevExpress.XtraEditors.XtraMessageBox.Show(
                //"Не удалось назначить подгруппу списку товаров. Текст ошибки: " + f.Message, "Внимание",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        /// <summary>
        /// Сохранить изменения в БД в IB
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean UpdateInIB(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
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
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandTimeout = CProductSubType.iCommandTimeoutIB;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartSubTypeToIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Name", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Guid"].Value = this.ID;
                cmd.Parameters["@Partline_Guid"].Value = this.ProductLine.ID;
                cmd.Parameters["@Partsubtype_Name"].Value = this.Name;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    //DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения товарной подгруппы. Текст ошибки: " + strErr, "Ошибка",
                    //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = ("Не удалось изменить свойства товарной подгруппы. Текст ошибки: " + f.Message);
                //DevExpress.XtraEditors.XtraMessageBox.Show(
                //"Не удалось изменить свойства товарной подгруппы. Текст ошибки: " + f.Message, "Внимание",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Курс ценообразования
        // Пока это заглушки
        // В БД нужно сделать таблицы для курса ценообразования, а так же процедуры и функции
        public static System.Double GetCurrentPriceCreateRate(CCurrency objCurrencyIn, CCurrency objCurrencyOut)
        {
            System.Double dblRet = 0;

            return dblRet;
        }
        public static System.Double GetCurrentPriceCreateRate(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Double dblRet = 0;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return dblRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandTimeout = CProductSubType.iCommandTimeoutIB;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCurrentCurrencyRatePricing]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Value", System.Data.SqlDbType.Float));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CurrencyRate_Value"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    dblRet = System.Convert.ToDouble( cmd.Parameters["@CurrencyRate_Value"].Value );
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                strErr += ("\nНе удалось получить значение курса ценообразования. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }

            return dblRet;
        }

        #endregion

        #region Изображение подгруппы
        /// <summary>
        /// Сохраняет изображение подгруппы в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Partsubtype_Guid">УИ товарной подгруппы</param>
        /// <param name="Partsubtype_Image">изображение в двоичном виде</param>
        /// <param name="Partsubtype_ImageFileFullName">наименование файла изображения</param>
        /// <param name="Partsubtype_ImageActionTypeId">тип операции с изображением (установить/удалить)</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SetImageToProductSubType(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Partsubtype_Guid, byte[] Partsubtype_Image, System.String Partsubtype_ImageFileFullName, 
            System.Int32 Partsubtype_ImageActionTypeId, ref System.String strErr, ref System.Int32 iRes)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_SetPartSubTypeImage]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Image", System.Data.SqlDbType.Binary));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_ImageFileFullName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_ImageActionTypeId", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Partsubtype_Guid"].Value = Partsubtype_Guid;
                cmd.Parameters["@Partsubtype_Image"].Value = Partsubtype_Image;
                cmd.Parameters["@Partsubtype_ImageFileFullName"].Value = Partsubtype_ImageFileFullName;
                cmd.Parameters["@Partsubtype_ImageActionTypeId"].Value = Partsubtype_ImageActionTypeId;

                cmd.ExecuteNonQuery();
                
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;

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

        public override string ToString()
        {
            return Name;
        }
    }

    public class CProductSubTypeStock : CWarehouse
    {
        #region Свойства
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
        private System.Guid m_uuidProductSubTypeId;
        public System.Guid ProductSubTypeId
        {
            get { return m_uuidProductSubTypeId; }
            set { m_uuidProductSubTypeId = value; }
        }
        #endregion

        #region Конструктор
        public CProductSubTypeStock() : base()
        {
            m_iQuantity = 0;
        }
        #endregion

        #region Список остатка
        public static List<CProductSubTypeStock> GetProductSubTypeStock(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductSubTypeStock> objList = new List<CProductSubTypeStock>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubTypeStock]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CProductSubTypeStock objProductSubTypeStock = null;
                    while (rs.Read())
                    {
                        objProductSubTypeStock = new CProductSubTypeStock();
                        objProductSubTypeStock.m_iQuantity = System.Convert.ToInt32(rs["QUANTITY"]);
                        objProductSubTypeStock.ID = (System.Guid)rs["Warehouse_Guid"];
                        objProductSubTypeStock.Name = (System.String)rs["Warehouse_Name"];
                        objProductSubTypeStock.m_uuidProductSubTypeId = (System.Guid)rs["PartSybType_Guid"];
                        objProductSubTypeStock.IsForShipping = System.Convert.ToBoolean(rs["Warehouse_IsForShipping"]);
                        objList.Add(objProductSubTypeStock);
                    }
                }
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
                "Не удалось получить список остатка товарных подгрупп.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

    }

}
