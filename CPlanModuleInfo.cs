using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPMercuryPlan
{
    public class CPlanModuleClassInfo : UniXP.Common.CModuleClassInfo
    {
        public CPlanModuleClassInfo()
        {
            UniXP.Common.CLASSINFO objClassInfo;

            objClassInfo = new UniXP.Common.CLASSINFO();
            objClassInfo.enClassType = UniXP.Common.EnumClassType.mcView;
            objClassInfo.strClassName = "ERPMercuryPlan.EditorPartSubType";
            objClassInfo.strName = "Товарные подгруппы";
            objClassInfo.strDescription = "Справочник товарных подгрупп";
            objClassInfo.lID = 0;
            objClassInfo.nImage = 1;
            objClassInfo.strResourceName = "IMAGES_PLANORDER";
            m_arClassInfo.Add(objClassInfo);

            objClassInfo = new UniXP.Common.CLASSINFO();
            objClassInfo.enClassType = UniXP.Common.EnumClassType.mcView;
            objClassInfo.strClassName = "ERPMercuryPlan.ViewCalcOrder";
            objClassInfo.strName = "Коэффициенты для плана продаж";
            objClassInfo.strDescription = "Расчеты коэффициентов для плана продаж";
            objClassInfo.lID = 1;
            objClassInfo.nImage = 1;
            objClassInfo.strResourceName = "IMAGES_PLANORDER";
            m_arClassInfo.Add(objClassInfo);

            objClassInfo = new UniXP.Common.CLASSINFO();
            objClassInfo.enClassType = UniXP.Common.EnumClassType.mcView;
            objClassInfo.strClassName = "ERPMercuryPlan.ViewCalcPlanList";
            objClassInfo.strName = "План продаж (ТМ)";
            objClassInfo.strDescription = "План продаж";
            objClassInfo.lID = 2;
            objClassInfo.nImage = 1;
            objClassInfo.strResourceName = "IMAGES_PLANORDER";
            m_arClassInfo.Add(objClassInfo);

            objClassInfo = new UniXP.Common.CLASSINFO();
            objClassInfo.enClassType = UniXP.Common.EnumClassType.mcView;
            objClassInfo.strClassName = "ERPMercuryPlan.ViewPriceListCalc";
            objClassInfo.strName = "Расчет цен";
            objClassInfo.strDescription = "Расчет цен";
            objClassInfo.lID = 3;
            objClassInfo.nImage = 1;
            objClassInfo.strResourceName = "IMAGES_PLANORDER";
            m_arClassInfo.Add(objClassInfo);

            objClassInfo = new UniXP.Common.CLASSINFO();
            objClassInfo.enClassType = UniXP.Common.EnumClassType.mcView;
            objClassInfo.strClassName = "ERPMercuryPlan.ViewPriceList";
            objClassInfo.strName = "Прайс-лист";
            objClassInfo.strDescription = "Прайс-лист";
            objClassInfo.lID = 4;
            objClassInfo.nImage = 1;
            objClassInfo.strResourceName = "IMAGES_PLANORDER";
            m_arClassInfo.Add(objClassInfo);

            objClassInfo = new UniXP.Common.CLASSINFO();
            objClassInfo.enClassType = UniXP.Common.EnumClassType.mcView;
            objClassInfo.strClassName = "ERPMercuryPlan.ViewPartsPriceList";
            objClassInfo.strName = "Прайс-лист (товары)";
            objClassInfo.strDescription = "Прайс-лист (товары)";
            objClassInfo.lID = 5;
            objClassInfo.nImage = 1;
            objClassInfo.strResourceName = "IMAGES_PLANORDER";
            m_arClassInfo.Add(objClassInfo);

            objClassInfo = new UniXP.Common.CLASSINFO();
            objClassInfo.enClassType = UniXP.Common.EnumClassType.mcView;
            objClassInfo.strClassName = "ERPMercuryPlan.SalesPlanQuotaEditor";
            objClassInfo.strName = "Продажи за период";
            objClassInfo.strDescription = "Доли продаж марок, групп, подгрупп, а также клиентов и подразделений в общем объёме продаж";
            objClassInfo.lID = 6;
            objClassInfo.nImage = 1;
            objClassInfo.strResourceName = "IMAGES_PLANORDER";
            m_arClassInfo.Add(objClassInfo);

            objClassInfo = new UniXP.Common.CLASSINFO();
            objClassInfo.enClassType = UniXP.Common.EnumClassType.mcView;
            objClassInfo.strClassName = "ERPMercuryPlan.PlanByDepartCustomerSubtypeEditor";
            objClassInfo.strName = "План продаж (ТМ, Клиенты)";
            objClassInfo.strDescription = "План продаж марок, групп, подгрупп, а также клиентов и подразделений в общем объёме продаж";
            objClassInfo.lID = 7;
            objClassInfo.nImage = 1;
            objClassInfo.strResourceName = "IMAGES_PLANORDER";
            m_arClassInfo.Add(objClassInfo);

        }
    }

    public class CPlanModuleInfo : UniXP.Common.CClientModuleInfo
    {
        public CPlanModuleInfo()
            : base(Assembly.GetExecutingAssembly(),
                UniXP.Common.EnumDLLType.typeItem,
                new System.Guid("{D7294768-8111-4772-AAF1-4C653830E414}"),
                new System.Guid("{A6319AD0-08C0-49ED-B25B-659BAB622B15}"),
                ERPMercuryPlan.Properties.Resources.IMAGES_PLANORDER,
                ERPMercuryPlan.Properties.Resources.IMAGES_PLANORDER)
        {
        }

        /// <summary>
        /// Выполняет операции по проверке правильности установки модуля в системе.
        /// </summary>
        /// <param name="objProfile">Профиль пользователя.</param>
        public override System.Boolean Check(UniXP.Common.CProfile objProfile)
        {
            return true;
        }
        /// <summary>
        /// Выполняет операции по установке модуля в систему.
        /// </summary>
        /// <param name="objProfile">Профиль пользователя.</param>
        public override System.Boolean Install(UniXP.Common.CProfile objProfile)
        {
            return true;
        }
        /// <summary>
        /// Выполняет операции по удалению модуля из системы.
        /// </summary>
        /// <param name="objProfile">Профиль пользователя.</param>
        public override System.Boolean UnInstall(UniXP.Common.CProfile objProfile)
        {
            return true;
        }
        /// <summary>
        /// Производит действия по обновлению при установке новой версии подключаемого модуля.
        /// </summary>
        /// <param name="objProfile">Профиль пользователя.</param>
        public override System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            return true;
        }
        /// <summary>
        /// Возвращает список доступных классов в данном модуле.
        /// </summary>
        public override UniXP.Common.CModuleClassInfo GetClassInfo()
        {
            return new CPlanModuleClassInfo();
        }
    }

    public class ModuleInfo : PlugIn.IModuleInfo
    {
        public UniXP.Common.CClientModuleInfo GetModuleInfo()
        {
            return new CPlanModuleInfo();
        }
    }
}
