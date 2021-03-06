USE [ERP_Mercury]
GO
/****** Object:  StoredProcedure [dbo].[usp_CalcSalePlanQuotaItemDecode]    Script Date: 09.12.2013 11:17:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Расчёт долей продаж для плана продаж
--
-- Входные параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@BeginDate											дата начала периода продаж
--		@EndDate												дата окончания периода продаж
--		@t_ProductTradeMarkList					список УИ товарных марок
--		@t_ProductTypeList							список УИ товарных групп
--
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_CalcSalePlanQuotaItemDecode] 
	@SalesPlanQuota_Guid				D_GUID = NULL,
	@BeginDate									D_DATE,
	@EndDate										D_DATE,
  @t_ProductTradeMarkList			dbo.udt_GuidList READONLY,
  @t_ProductTypeList					dbo.udt_GuidList READONLY,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		CREATE TABLE #PLANSALEDEPART( OWNER_ID int, PARTTYPE_ID int, DEPART_CODE nvarchar(3), PARTSUBTYPE_ID int, CUSTOMER_ID int, SHIP_QTY float, SHIP_MONEY money, 
			Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, DepartTeam_Guid uniqueidentifier, Depart_Guid uniqueidentifier, 
			Customer_Guid uniqueidentifier, PartSubType_Guid  uniqueidentifier);

		INSERT INTO #PLANSALEDEPART( OWNER_ID, PARTTYPE_ID, DEPART_CODE, PARTSUBTYPE_ID, CUSTOMER_ID, SHIP_QTY, SHIP_MONEY )
		SELECT OWNER_ID, PARTTYPE_ID, DEPART_CODE, PARTSUBTYPE_ID, CUSTOMER_ID, SHIP_QTY_BURST, SHIP_MONEY
			 FROM [DB02].[ERP_Report].[dbo].[T_ERP_CSM_OLAP_PLANSALEDEPART]
			 WHERE [SHIPDATE] BETWEEN @BeginDate AND @EndDate
				AND [COUNTRY_ID] = 1
				AND [SHIP_QTY_BURST] > 0;

		-- удаление рекламы и тестеров
		DELETE FROM #PLANSALEDEPART WHERE PARTTYPE_ID BETWEEN 26 AND 29;

		-- обновление значений уникальных идентификаторов
		UPDATE #PLANSALEDEPART SET Owner_Guid = [dbo].[GetOwnerGuidForOwnerId]( OWNER_ID ), 
			PartType_Guid = [dbo].[GetParttypeGuidForParttypeId]( PARTTYPE_ID ), 
			PartSubType_Guid = [dbo].[GetPartsubtypeGuidForPartsubtypeId]( PARTSUBTYPE_ID ), 
			Customer_Guid =	[dbo].[GetCustomerGuidForCustomerId]( CUSTOMER_ID ), 
			Depart_Guid = [dbo].[GetDepartGuidByDepartCode]( DEPART_CODE ),
			DepartTeam_Guid = [dbo].[GetDepartTeamGuidByDepartCode]( DEPART_CODE ); 

		DELETE FROM #PLANSALEDEPART WHERE Depart_Guid IS NULL;

		--SELECT * FROM	#PLANSALEDEPART;


		CREATE TABLE #SalesPlanQuotaItem( ObjectType_Id int, SalesPlanQuotaItem_Guid uniqueidentifier, SalesPlanQuota_Guid uniqueidentifier, 
			ProductOwner_Guid uniqueidentifier, ProductType_Guid uniqueidentifier, 
			SalesPlanQuotaItem_Quantity float, SalesPlanQuotaItem_Money money, 
			SalesPlanQuotaItemDecode_Guid uniqueidentifier, ObjectDecode_Guid uniqueidentifier, ObjectDecode_Id int, ObjectDecode_Name nvarchar(128), 
			SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, 
			SalesPlanQuotaItemDecode_CalcQuota numeric( 18, 5 ), SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ) );

		-- Подразделение
		WITH SHIP_BY_DEPART ( Owner_Guid, PartType_Guid, Depart_Guid, SHIP_QTY, SHIP_QTY_GROUP, SHIP_MONEY, SHIP_MONEY_GROUP )
		 AS
		 (
			 SELECT Owner_Guid, PartType_Guid, Depart_Guid, 
				SUM([SHIP_QTY])  OVER( PARTITION BY Owner_Guid, PartType_Guid, Depart_Guid ),
				SUM([SHIP_QTY])  OVER( PARTITION BY Owner_Guid, PartType_Guid ),
				SUM([SHIP_MONEY])  OVER( PARTITION BY Owner_Guid, PartType_Guid, Depart_Guid ),
				SUM([SHIP_MONEY])  OVER( PARTITION BY Owner_Guid, PartType_Guid )
			 FROM #PLANSALEDEPART	
		 )
		 INSERT INTO #SalesPlanQuotaItem( ObjectType_Id, SalesPlanQuota_Guid, 
			ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, 
			ObjectDecode_Guid, ObjectDecode_Id, ObjectDecode_Name, 
			SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money )
		 SELECT DISTINCT cast( 1 as int ), @SalesPlanQuota_Guid, Owner_Guid, PartType_Guid, 
		  SHIP_QTY_GROUP, SHIP_MONEY_GROUP, 
			SHIP_BY_DEPART.Depart_Guid, cast( 0 as int), [dbo].[T_Depart].Depart_Code, 
			SHIP_QTY, SHIP_MONEY
		 FROM SHIP_BY_DEPART INNER JOIN [dbo].[T_Depart] ON SHIP_BY_DEPART.Depart_Guid = [dbo].[T_Depart].Depart_Guid;	 

		-- Команда
		WITH SHIP_BY_DEPARTTEAM ( Owner_Guid, PartType_Guid, DepartTeam_Guid, SHIP_QTY, SHIP_QTY_GROUP, SHIP_MONEY, SHIP_MONEY_GROUP )
		 AS
		 (
			 SELECT Owner_Guid, PartType_Guid, DepartTeam_Guid, 
				SUM([SHIP_QTY])  OVER( PARTITION BY Owner_Guid, PartType_Guid, DepartTeam_Guid ),
				SUM([SHIP_QTY])  OVER( PARTITION BY Owner_Guid, PartType_Guid ),
				SUM([SHIP_MONEY])  OVER( PARTITION BY Owner_Guid, PartType_Guid, DepartTeam_Guid ),
				SUM([SHIP_MONEY])  OVER( PARTITION BY Owner_Guid, PartType_Guid )
			 FROM #PLANSALEDEPART	
		 )
		 INSERT INTO #SalesPlanQuotaItem( ObjectType_Id, SalesPlanQuota_Guid, 
			ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, 
			ObjectDecode_Guid, ObjectDecode_Id, ObjectDecode_Name, 
			SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money )
		 SELECT DISTINCT cast( 0 as int ), @SalesPlanQuota_Guid, Owner_Guid, PartType_Guid, 
		  SHIP_QTY_GROUP, SHIP_MONEY_GROUP, 
			SHIP_BY_DEPARTTEAM.DepartTeam_Guid, cast( 0 as int), [dbo].[T_DepartTeam].DepartTeam_Name, 
			SHIP_QTY, SHIP_MONEY
		 FROM SHIP_BY_DEPARTTEAM INNER JOIN [dbo].[T_DepartTeam] ON SHIP_BY_DEPARTTEAM.DepartTeam_Guid = [dbo].[T_DepartTeam].DepartTeam_Guid;	 

		-- Клиент
		WITH SHIP_BY_CUSTOMER ( Owner_Guid, PartType_Guid, Customer_Guid, SHIP_QTY, SHIP_QTY_GROUP, SHIP_MONEY, SHIP_MONEY_GROUP )
		 AS
		 (
			 SELECT Owner_Guid, PartType_Guid, Customer_Guid, 
				SUM([SHIP_QTY])  OVER( PARTITION BY Owner_Guid, PartType_Guid, Customer_Guid ),
				SUM([SHIP_QTY])  OVER( PARTITION BY Owner_Guid, PartType_Guid ),
				SUM([SHIP_MONEY])  OVER( PARTITION BY Owner_Guid, PartType_Guid, Customer_Guid ),
				SUM([SHIP_MONEY])  OVER( PARTITION BY Owner_Guid, PartType_Guid )
			 FROM #PLANSALEDEPART	
		 )
		 INSERT INTO #SalesPlanQuotaItem( ObjectType_Id, SalesPlanQuota_Guid, 
			ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, 
			ObjectDecode_Guid, ObjectDecode_Id, ObjectDecode_Name, 
			SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money )
		 SELECT DISTINCT cast( 2 as int ), @SalesPlanQuota_Guid, Owner_Guid, PartType_Guid, 
		  SHIP_QTY_GROUP, SHIP_MONEY_GROUP, 
			SHIP_BY_CUSTOMER.Customer_Guid, [dbo].[T_Customer].Customer_Id, [dbo].[T_Customer].Customer_Name,
			SHIP_QTY, SHIP_MONEY
		 FROM SHIP_BY_CUSTOMER INNER JOIN [dbo].[T_Customer] ON SHIP_BY_CUSTOMER.Customer_Guid = [dbo].[T_Customer].Customer_Guid;	 


		-- Подгруппа
		WITH SHIP_BY_SUBTYPE ( Owner_Guid, PartType_Guid, PartSubType_Guid, SHIP_QTY, SHIP_QTY_GROUP, SHIP_MONEY, SHIP_MONEY_GROUP )
		 AS
		 (
			 SELECT Owner_Guid, PartType_Guid, PartSubType_Guid, 
				SUM([SHIP_QTY])  OVER( PARTITION BY Owner_Guid, PartType_Guid, PartSubType_Guid ),
				SUM([SHIP_QTY])  OVER( PARTITION BY Owner_Guid, PartType_Guid ),
				SUM([SHIP_MONEY])  OVER( PARTITION BY Owner_Guid, PartType_Guid, PartSubType_Guid ),
				SUM([SHIP_MONEY])  OVER( PARTITION BY Owner_Guid, PartType_Guid )
			 FROM #PLANSALEDEPART	
		 )
		 INSERT INTO #SalesPlanQuotaItem( ObjectType_Id, SalesPlanQuota_Guid, 
			ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, 
			ObjectDecode_Guid, ObjectDecode_Id, ObjectDecode_Name, 
			SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money )
		 SELECT DISTINCT cast( 3 as int ), @SalesPlanQuota_Guid, Owner_Guid, PartType_Guid, 
		  SHIP_QTY_GROUP, SHIP_MONEY_GROUP, 
			SHIP_BY_SUBTYPE.PartSubType_Guid, [dbo].[T_PartSubType].Partsubtype_Id, [dbo].[T_PartSubType].Partsubtype_Name,
			SHIP_QTY, SHIP_MONEY
		 FROM SHIP_BY_SUBTYPE INNER JOIN [dbo].[T_PartSubType] ON SHIP_BY_SUBTYPE.PartSubType_Guid = [dbo].[T_PartSubType].PartSubType_Guid;	 

		 UPDATE #SalesPlanQuotaItem SET SalesPlanQuotaItemDecode_CalcQuota = 0
		 WHERE SalesPlanQuotaItem_Quantity <= 0;

		 UPDATE #SalesPlanQuotaItem SET SalesPlanQuotaItemDecode_CalcQuota = (SalesPlanQuotaItemDecode_Quantity/SalesPlanQuotaItem_Quantity)
		 WHERE SalesPlanQuotaItem_Quantity > 0;

		 UPDATE #SalesPlanQuotaItem SET SalesPlanQuotaItemDecode_Quota = SalesPlanQuotaItemDecode_CalcQuota;

		 SELECT ObjectType_Id, SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, 
			ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, 
			SalesPlanQuotaItemDecode_Guid, ObjectDecode_Guid, ObjectDecode_Id, ObjectDecode_Name, 
			SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
			[dbo].[T_Owner].Owner_Id, [dbo].[T_Owner].Owner_Name,
			[dbo].[T_Parttype].Parttype_Id, [dbo].[T_Parttype].Parttype_Name
		 FROM #SalesPlanQuotaItem INNER JOIN [dbo].[T_Owner] ON #SalesPlanQuotaItem.ProductOwner_Guid = [dbo].[T_Owner].Owner_Guid
			INNER JOIN [dbo].[T_Parttype] ON #SalesPlanQuotaItem.ProductType_Guid = [dbo].[T_Parttype].Parttype_Guid
		 WHERE ( ProductOwner_Guid IN ( SELECT [Item_Guid] FROM @t_ProductTradeMarkList ) ) AND ( ProductType_Guid IN ( SELECT [Item_Guid] FROM @t_ProductTypeList ) )
		 ORDER BY [dbo].[T_Owner].Owner_Name, [dbo].[T_Parttype].Parttype_Name;	

		DROP TABLE #SalesPlanQuotaItem;
		DROP TABLE #PLANSALEDEPART;


	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();

		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';

	RETURN @ERROR_NUM;
END
