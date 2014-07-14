USE [ERP_Mercury]
GO

UPDATE [dbo].[T_PriceListCalcItems] SET [Partsubtype_MarkUpRequired] = 0
GO

--IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'usp_AddPriceListCalcItem')
IF OBJECT_ID('usp_AddPriceListCalcItem') IS NOT NULL
DROP PROCEDURE [dbo].[usp_AddPriceListCalcItem]

IF OBJECT_ID('usp_AssignPartsubtypeWithPriceInCalc') IS NOT NULL
--IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'usp_AssignPartsubtypeWithPriceInCalc')
DROP PROCEDURE [dbo].[usp_AssignPartsubtypeWithPriceInCalc]
GO


/****** Object:  UserDefinedTableType [dbo].[udt_PriceListCalcItems]    Script Date: 21.03.2013 11:12:50 ******/
DROP TYPE [dbo].[udt_PriceListCalcItems]
GO

/****** Object:  UserDefinedTableType [dbo].[udt_PriceListCalcItems]    Script Date: 21.03.2013 11:12:50 ******/
CREATE TYPE [dbo].[udt_PriceListCalcItems] AS TABLE(
	[Partsubtype_Guid] [dbo].[D_GUID] NULL,
	[Partsubtype_VendorTariff] [dbo].[D_MONEY] NULL,
	[Partsubtype_TransportTariff] [dbo].[D_MONEY] NULL,
	[Partsubtype_TransportSum] [dbo].[D_MONEY] NULL,
	[Partsubtype_CustomsTariff] [dbo].[D_MONEY] NULL,
	[Partsubtype_CustomsSum] [dbo].[D_MONEY] NULL,
	[Partsubtype_Margin] [dbo].[D_MONEY] NULL,
	[Partsubtype_NDS] [dbo].[D_MONEY] NULL,
	[Partsubtype_NDSSum] [dbo].[D_MONEY] NULL,
	[Partsubtype_Discont] [dbo].[D_MONEY] NULL,
	[Partsubtype_CurRatePricing] [dbo].[D_MONEY] NULL,
	[Partsubtype_MarkUpRequired] [dbo].[D_MONEY] NULL
)
GO

GRANT CONTROL ON TYPE::[dbo].[udt_PriceListCalcItems] TO [public]
GO
use [ERP_Mercury]
GO
GRANT REFERENCES ON TYPE::[dbo].[udt_PriceListCalcItems] TO [public]
GO
use [ERP_Mercury]
GO
GRANT TAKE OWNERSHIP ON TYPE::[dbo].[udt_PriceListCalcItems] TO [public]
GO
use [ERP_Mercury]
GO
GRANT VIEW DEFINITION ON TYPE::[dbo].[udt_PriceListCalcItems] TO [public]
GO

/****** Object:  StoredProcedure [dbo].[usp_AddPriceListCalcItem]    Script Date: 21.03.2013 11:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет к расчёту цен запись
--
-- Параметры:
-- [in] @PriceListCalc_Guid - идентификатор расчета цен
-- [out]  @ERROR_NUM - номер ошибки
-- [out]  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - успешное завершение
--
-- Возвращает:
--

CREATE PROCEDURE [dbo].[usp_AddPriceListCalcItem] 
	@PriceListCalc_Guid dbo.D_GUID, 
	@tPriceListCalcItems dbo.udt_PriceListCalcItems READONLY,
	@tPriceListCalcItemsPrice dbo.udt_PriceListCalcItemsPrice  READONLY,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  BEGIN TRY
    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем, существует ли расчет с заданым идентификатором
    IF NOT EXISTS ( SELECT PriceListCalc_Guid FROM dbo.T_PriceListCalc
      WHERE PriceListCalc_Guid = @PriceListCalc_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден расчет цен с указанным идентификатором.';
        RETURN @ERROR_NUM;
      END

    DECLARE @Partsubtype_Guid [dbo].[D_GUID];
    DECLARE @Partsubtype_VendorTariff [dbo].[D_MONEY];
    DECLARE @Partsubtype_TransportTariff [dbo].[D_MONEY];
    DECLARE @Partsubtype_TransportSum [dbo].[D_MONEY];
    DECLARE @Partsubtype_CustomsTariff [dbo].[D_MONEY];
    DECLARE @Partsubtype_CustomsSum [dbo].[D_MONEY];
    DECLARE @Partsubtype_Margin [dbo].[D_MONEY];
    DECLARE @Partsubtype_NDS [dbo].[D_MONEY];
    DECLARE @Partsubtype_NDSSum [dbo].[D_MONEY];
    DECLARE @Partsubtype_Discont [dbo].[D_MONEY];
    DECLARE @PriceListCalcItems_Guid [dbo].[D_GUID];
    DECLARE @Partsubtype_CurRatePricing [dbo].[D_MONEY];
    DECLARE @Partsubtype_MarkUpRequired [dbo].[D_MONEY];

	  DECLARE crCalcItem CURSOR
	  FOR SELECT Partsubtype_Guid,   
	     Partsubtype_TransportSum, Partsubtype_CustomsSum, 
	     Partsubtype_NDSSum, Partsubtype_CurRatePricing
	  FROM @tPriceListCalcItems;
	  OPEN crCalcItem;
	  FETCH NEXT FROM crCalcItem INTO @Partsubtype_Guid, @Partsubtype_TransportSum,  @Partsubtype_CustomsSum, 
	     @Partsubtype_NDSSum, @Partsubtype_CurRatePricing;
	  WHILE ( @@fetch_status = 0)
		  BEGIN

        SELECT @Partsubtype_VendorTariff = Partsubtype_VendorTariff, 
					@Partsubtype_TransportTariff = Partsubtype_TransportTariff, 
					@Partsubtype_CustomsTariff = Partsubtype_CustomsTariff,
					@Partsubtype_Margin = Partsubtype_Margin, @Partsubtype_NDS = Partsubtype_NDS, 
					@Partsubtype_Discont = Partsubtype_Discont, @Partsubtype_MarkUpRequired = Partsubtype_MarkUpRequired
				FROM dbo.T_Partsubtype
				WHERE Partsubtype_Guid = @Partsubtype_Guid;
					
        SET @PriceListCalcItems_Guid = NULL;
        IF( @Partsubtype_MarkUpRequired IS NULL ) SET @Partsubtype_MarkUpRequired = 0;
        
        SELECT @PriceListCalcItems_Guid = PriceListCalcItems_Guid 
        FROM dbo.T_PriceListCalcItems
        WHERE PriceListCalc_Guid = @PriceListCalc_Guid
					AND Partsubtype_Guid = @Partsubtype_Guid;
					
				IF( @PriceListCalcItems_Guid IS NULL ) 	
	        SET @PriceListCalcItems_Guid = NEWID();
	      ELSE
					BEGIN
						DELETE FROM dbo.T_PriceListCalcItemsPrice WHERE PriceListCalcItems_Guid = @PriceListCalcItems_Guid;
						
						DELETE FROM dbo.T_PriceListCalcItems WHERE PriceListCalcItems_Guid = @PriceListCalcItems_Guid;
					END  
        
        INSERT INTO dbo.T_PriceListCalcItems( PriceListCalcItems_Guid, PriceListCalc_Guid, Partsubtype_Guid, Partsubtype_VendorTariff, 
					Partsubtype_TransportTariff, Partsubtype_TransportSum, Partsubtype_CustomsTariff, Partsubtype_CustomsSum, Partsubtype_Margin, 
					Partsubtype_NDS, Partsubtype_NDSSum, Partsubtype_Discont, Partsubtype_CurRatePricing, Partsubtype_MarkUpRequired )
        VALUES( @PriceListCalcItems_Guid, @PriceListCalc_Guid, @Partsubtype_Guid, @Partsubtype_VendorTariff, 
					@Partsubtype_TransportTariff, @Partsubtype_TransportSum, @Partsubtype_CustomsTariff, @Partsubtype_CustomsSum, @Partsubtype_Margin, 
					@Partsubtype_NDS, @Partsubtype_NDSSum, @Partsubtype_Discont, @Partsubtype_CurRatePricing, @Partsubtype_MarkUpRequired )
        
        INSERT INTO dbo.T_PriceListCalcItemsPrice( PriceListCalcItems_Guid, PartsubtypePriceType_Guid, Price_Value )
        SELECT @PriceListCalcItems_Guid, PartsubtypePriceType_Guid, Price_Value 
        FROM @tPriceListCalcItemsPrice 
        WHERE Partsubtype_Guid = @Partsubtype_Guid;


			  FETCH NEXT FROM crCalcItem INTO @Partsubtype_Guid, @Partsubtype_TransportSum,  @Partsubtype_CustomsSum, 
	     @Partsubtype_NDSSum, @Partsubtype_CurRatePricing;
		  END -- while
  	
	  CLOSE crCalcItem;
	  DEALLOCATE crCalcItem;
      
 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = 'Ошибка заполнения содержимого расчета цен. ' + ERROR_MESSAGE();
		RETURN @ERROR_NUM;
	END CATCH;

  IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';

	RETURN @ERROR_NUM;
END

GO

/****** Object:  StoredProcedure [dbo].[usp_AssignPartsubtypeWithPriceInCalc]    Script Date: 21.03.2013 11:11:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Прописывает в БД содержимое расчета цен
--
-- Параметры:
-- [in] @PriceListCalc_Guid - идентификатор расчета цен
-- [out]  @ERROR_NUM - номер ошибки
-- [out]  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - успешное завершение
--
-- Возвращает:
--

CREATE  PROCEDURE [dbo].[usp_AssignPartsubtypeWithPriceInCalc] 
	@PriceListCalc_Guid dbo.D_GUID, 
	@tPriceListCalcItems dbo.udt_PriceListCalcItems READONLY,
	@tPriceListCalcItemsPrice dbo.udt_PriceListCalcItemsPrice  READONLY,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  BEGIN TRY
    SET @ERROR_NUM = 0;
    SET @ERROR_MES = NULL;

    -- Проверяем, существует ли расчет с заданым идентификатором
    IF NOT EXISTS ( SELECT PriceListCalc_Guid FROM dbo.T_PriceListCalc
      WHERE PriceListCalc_Guid = @PriceListCalc_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден расчет цен с указанным идентификатором.';
        RETURN @ERROR_NUM;
      END

    DELETE FROM dbo.T_PriceListCalcItemsPrice WHERE PriceListCalcItems_Guid IN ( SELECT PriceListCalcItems_Guid FROM dbo.T_PriceListCalcItems WHERE PriceListCalc_Guid = @PriceListCalc_Guid );

    DELETE FROM dbo.T_PriceListCalcItems WHERE PriceListCalc_Guid = @PriceListCalc_Guid;

    DECLARE @Partsubtype_Guid [dbo].[D_GUID];
    DECLARE @Partsubtype_VendorTariff [dbo].[D_MONEY];
    DECLARE @Partsubtype_TransportTariff [dbo].[D_MONEY];
    DECLARE @Partsubtype_TransportSum [dbo].[D_MONEY];
    DECLARE @Partsubtype_CustomsTariff [dbo].[D_MONEY];
    DECLARE @Partsubtype_CustomsSum [dbo].[D_MONEY];
    DECLARE @Partsubtype_Margin [dbo].[D_MONEY];
    DECLARE @Partsubtype_NDS [dbo].[D_MONEY];
    DECLARE @Partsubtype_NDSSum [dbo].[D_MONEY];
    DECLARE @Partsubtype_Discont [dbo].[D_MONEY];
    DECLARE @PriceListCalcItems_Guid [dbo].[D_GUID];
    DECLARE @Partsubtype_CurRatePricing [dbo].[D_MONEY];
    DECLARE @Partsubtype_MarkUpRequired [dbo].[D_MONEY];

	  DECLARE crCalcItem CURSOR
	  FOR SELECT Partsubtype_Guid, Partsubtype_VendorTariff, Partsubtype_TransportTariff, 
	     Partsubtype_TransportSum, Partsubtype_CustomsTariff, Partsubtype_CustomsSum, 
	     Partsubtype_Margin, Partsubtype_NDS, Partsubtype_NDSSum, Partsubtype_Discont, 
			 Partsubtype_CurRatePricing, Partsubtype_MarkUpRequired
	  FROM @tPriceListCalcItems;
	  OPEN crCalcItem;
	  FETCH NEXT FROM crCalcItem INTO @Partsubtype_Guid, @Partsubtype_VendorTariff, @Partsubtype_TransportTariff, 
	     @Partsubtype_TransportSum, @Partsubtype_CustomsTariff, @Partsubtype_CustomsSum, 
	     @Partsubtype_Margin, @Partsubtype_NDS, @Partsubtype_NDSSum, @Partsubtype_Discont, 
			 @Partsubtype_CurRatePricing, @Partsubtype_MarkUpRequired;
	  WHILE ( @@fetch_status = 0)
		  BEGIN

        SET @PriceListCalcItems_Guid = NEWID();
        
        INSERT INTO dbo.T_PriceListCalcItems( PriceListCalcItems_Guid, PriceListCalc_Guid, Partsubtype_Guid, Partsubtype_VendorTariff, 
					Partsubtype_TransportTariff, Partsubtype_TransportSum, Partsubtype_CustomsTariff, Partsubtype_CustomsSum, Partsubtype_Margin, 
					Partsubtype_NDS, Partsubtype_NDSSum, Partsubtype_Discont, Partsubtype_CurRatePricing, Partsubtype_MarkUpRequired )
        VALUES( @PriceListCalcItems_Guid, @PriceListCalc_Guid, @Partsubtype_Guid, @Partsubtype_VendorTariff, 
					@Partsubtype_TransportTariff, @Partsubtype_TransportSum, @Partsubtype_CustomsTariff, @Partsubtype_CustomsSum, @Partsubtype_Margin, 
					@Partsubtype_NDS, @Partsubtype_NDSSum, @Partsubtype_Discont, @Partsubtype_CurRatePricing, @Partsubtype_MarkUpRequired )
        
        IF EXISTS ( SELECT Partsubtype_Guid FROM @tPriceListCalcItemsPrice 
            WHERE Partsubtype_Guid = @Partsubtype_Guid )
          BEGIN
            INSERT INTO dbo.T_PriceListCalcItemsPrice( PriceListCalcItems_Guid, PartsubtypePriceType_Guid, Price_Value )
            SELECT @PriceListCalcItems_Guid, PartsubtypePriceType_Guid, Price_Value 
            FROM @tPriceListCalcItemsPrice 
            WHERE Partsubtype_Guid = @Partsubtype_Guid;
          END    

			  FETCH NEXT FROM crCalcItem INTO @Partsubtype_Guid, @Partsubtype_VendorTariff, @Partsubtype_TransportTariff, 
				 @Partsubtype_TransportSum, @Partsubtype_CustomsTariff, @Partsubtype_CustomsSum, 
				 @Partsubtype_Margin, @Partsubtype_NDS, @Partsubtype_NDSSum, @Partsubtype_Discont, 
				 @Partsubtype_CurRatePricing, @Partsubtype_MarkUpRequired;
		  END -- while
  	
	  CLOSE crCalcItem;
	  DEALLOCATE crCalcItem;
      
 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = 'Ошибка заполнения содержимого расчета цен. ' + ERROR_MESSAGE();
		RETURN @ERROR_NUM;
	END CATCH;

	RETURN @ERROR_NUM;
END

GO


/****** Object:  View [dbo].[PriceListCalcItemsPriceView]    Script Date: 21.03.2013 11:33:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[PriceListCalcItemsPriceView]
AS
SELECT     dbo.T_PriceListCalcItems.PriceListCalcItems_Guid, dbo.T_PriceListCalcItems.PriceListCalc_Guid, dbo.T_PriceListCalcItems.Partsubtype_Guid, 
                      dbo.T_PriceListCalcItems.Partsubtype_VendorTariff, dbo.T_PriceListCalcItems.Partsubtype_TransportTariff, dbo.T_PriceListCalcItems.Partsubtype_TransportSum, 
                      dbo.T_PriceListCalcItems.Partsubtype_CustomsTariff, dbo.T_PriceListCalcItems.Partsubtype_CustomsSum, dbo.T_PriceListCalcItems.Partsubtype_Margin, 
                      dbo.T_PriceListCalcItems.Partsubtype_NDS, dbo.T_PriceListCalcItems.Partsubtype_NDSSum, dbo.T_PriceListCalcItems.Partsubtype_Discont, 
                      dbo.PartSubtypeView.Partsubtype_Id, dbo.PartSubtypeView.Partsubtype_Name, dbo.PartSubtypeView.Partsubtype_IsActive, 
                      dbo.T_PriceListCalcItemsPrice.PartsubtypePriceType_Guid, dbo.T_PriceListCalcItemsPrice.Price_Value, dbo.T_PartsubtypePriceType.PartsubtypePriceType_Name, 
                      dbo.T_PartsubtypePriceType.PartsubtypePriceType_Abbr, dbo.T_PartsubtypePriceType.PartsubtypePriceType_Description, 
                      dbo.T_PartsubtypePriceType.Currency_Guid, dbo.T_PartsubtypePriceType.PartsubtypePriceType_IsActive, 
                      dbo.T_PartsubtypePriceType.PartsubtypePriceType_ColumnIdDefault, dbo.T_Currency.Currency_Abbr, dbo.T_Currency.Currency_Code, 
                      dbo.T_Currency.Currency_Name, dbo.T_PriceListCalcItems.Partsubtype_CurRatePricing, dbo.T_PartsubtypePriceType.PartsubtypePriceType_ShowInPriceList, 
                      dbo.GetPartOwnerGuidForPartSubType(dbo.T_PriceListCalcItems.Partsubtype_Guid) AS PartOwner_Guid, 
                      dbo.T_PriceListCalcItems.Partsubtype_MarkUpRequired
FROM         dbo.T_PriceListCalcItems INNER JOIN
                      dbo.PartSubtypeView ON dbo.T_PriceListCalcItems.Partsubtype_Guid = dbo.PartSubtypeView.Partsubtype_Guid INNER JOIN
                      dbo.T_PriceListCalcItemsPrice ON dbo.T_PriceListCalcItems.PriceListCalcItems_Guid = dbo.T_PriceListCalcItemsPrice.PriceListCalcItems_Guid INNER JOIN
                      dbo.T_PartsubtypePriceType ON dbo.T_PriceListCalcItemsPrice.PartsubtypePriceType_Guid = dbo.T_PartsubtypePriceType.PartsubtypePriceType_Guid INNER JOIN
                      dbo.T_Currency ON dbo.T_PartsubtypePriceType.Currency_Guid = dbo.T_Currency.Currency_Guid

GO


/****** Object:  View [dbo].[PriceListCalcItemsView]    Script Date: 21.03.2013 11:33:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[PriceListCalcItemsView]
AS
SELECT     dbo.T_PriceListCalcItems.PriceListCalcItems_Guid, dbo.T_PriceListCalcItems.PriceListCalc_Guid, dbo.T_PriceListCalcItems.Partsubtype_Guid, 
                      dbo.T_PriceListCalcItems.Partsubtype_VendorTariff, dbo.T_PriceListCalcItems.Partsubtype_TransportTariff, dbo.T_PriceListCalcItems.Partsubtype_TransportSum, 
                      dbo.T_PriceListCalcItems.Partsubtype_CustomsTariff, dbo.T_PriceListCalcItems.Partsubtype_CustomsSum, dbo.T_PriceListCalcItems.Partsubtype_Margin, 
                      dbo.T_PriceListCalcItems.Partsubtype_NDS, dbo.T_PriceListCalcItems.Partsubtype_NDSSum, dbo.T_PriceListCalcItems.Partsubtype_Discont, 
                      dbo.PriceListCalcView.PriceListCalc_Name, dbo.PriceListCalcView.PriceListCalc_Date, dbo.PartSubtypeView.Partsubtype_Id, dbo.PartSubtypeView.Partsubtype_Name, 
                      dbo.PartSubtypeView.Partsubtype_IsActive, dbo.T_PriceListCalcItems.Partsubtype_CurRatePricing, dbo.T_PriceListCalcItems.Partsubtype_MarkUpRequired
FROM         dbo.T_PriceListCalcItems INNER JOIN
                      dbo.PartSubtypeView ON dbo.T_PriceListCalcItems.Partsubtype_Guid = dbo.PartSubtypeView.Partsubtype_Guid INNER JOIN
                      dbo.PriceListCalcView ON dbo.T_PriceListCalcItems.PriceListCalc_Guid = dbo.PriceListCalcView.PriceListCalc_Guid

GO

/****** Object:  StoredProcedure [dbo].[usp_GetPriceListCalcItems]    Script Date: 21.03.2013 11:31:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список записей из ( dbo.PriceListCalcItemsPriceView )
--
-- Входные параметры:
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetPriceListCalcItems] 
  @PriceListCalc_Guid D_GUID,
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

  BEGIN TRY

  SELECT PriceListCalcItems_Guid, PriceListCalc_Guid, Partsubtype_Guid, PartsubtypePriceType_ShowInPriceList,
    Partsubtype_VendorTariff, Partsubtype_TransportTariff, Partsubtype_TransportSum, 
    Partsubtype_CustomsTariff, Partsubtype_CustomsSum, Partsubtype_Margin, 
    Partsubtype_NDS, Partsubtype_NDSSum, Partsubtype_CurRatePricing, Partsubtype_Discont, Partsubtype_Id, 
    Partsubtype_Name, Partsubtype_IsActive, PartsubtypePriceType_Guid, Price_Value, 
    PartsubtypePriceType_Name, PartsubtypePriceType_Abbr, PartsubtypePriceType_Description, 
    Currency_Guid, PartsubtypePriceType_IsActive, PartsubtypePriceType_ColumnIdDefault, 
    Currency_Abbr, Currency_Code, Currency_Name, 
    dbo.PriceListCalcItemsPriceView.PartOwner_Guid, 
    PartOwner.Owner_Id, PartOwner.Owner_Name, Partsubtype_MarkUpRequired
  FROM dbo.PriceListCalcItemsPriceView  LEFT OUTER JOIN dbo.T_Owner as PartOwner ON dbo.PriceListCalcItemsPriceView.PartOwner_Guid = PartOwner.Owner_Guid
  WHERE dbo.PriceListCalcItemsPriceView.PriceListCalc_Guid = @PriceListCalc_Guid
  ORDER BY dbo.PriceListCalcItemsPriceView.Partsubtype_Id;

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

GO

UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 41 WHERE PartsubtypePriceType_Guid = 'D98F44FA-0DDF-49FB-A89A-008ADCB102BB';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 27 WHERE PartsubtypePriceType_Guid = '5B08820C-A9B8-4720-954B-00B709194152';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 21 WHERE PartsubtypePriceType_Guid = '0833529F-EC68-45E9-8334-177811BC878F';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = '5EDC5074-6446-4A46-A239-29DA3904EF33';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = 'D11C1BD7-E134-41FB-95AA-31A58B4C1EB9';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 54 WHERE PartsubtypePriceType_Guid = 'EB9CD764-AD5F-42DB-808F-423E1DE0FFDA';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = '401CB9AF-E329-45E2-B45A-7394478E0A30';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 23 WHERE PartsubtypePriceType_Guid = 'C3C03EE8-ED79-4B15-8625-780692FF19C9';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 38 WHERE PartsubtypePriceType_Guid = 'CAABE4C4-02AA-4660-A490-899EDC1EA0B1';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = '5306B381-55F4-46C2-8FC3-9A2E76E482DA';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 51 WHERE PartsubtypePriceType_Guid = '4238F140-CB82-4F40-9DAC-A4671280CAA2';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = '120B75A8-B318-4986-852D-A4E9D972EDF7';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = '4C368480-BE72-4371-88B2-ABF348F7AFD2';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 28 WHERE PartsubtypePriceType_Guid = '275E114C-5248-4AE7-BBE3-B7EFCA90C802';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = '0BF23463-1045-44BE-B97F-BA2E30F2F959';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 31 WHERE PartsubtypePriceType_Guid = 'DBE05686-55A1-4094-86E8-CCE136623CE1';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 25 WHERE PartsubtypePriceType_Guid = '05B334B4-57E5-4147-A3D3-DF2741CC9BEB';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 53 WHERE PartsubtypePriceType_Guid = '76144510-261B-4D87-8C0B-E8C52CE148F9';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = 'D190122A-E472-41D3-87DE-F4BAD179DFFC';
UPDATE T_PartsubtypePriceType SET PartsubtypePriceType_ColumnIdDefault = 1 WHERE PartsubtypePriceType_Guid = 'FCD8C9D4-B96F-466B-A192-F96EEC287DC8';

DECLARE @doc xml;

SET @doc = '<SettingForPriceCalc xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <ColumnItem CalcParamName="Начальная строка" ColumnNum="  8" />
  <ColumnItem CalcParamName="Товарная марка" ColumnNum="  1" />
  <ColumnItem CalcParamName="Товарная подгруппа" ColumnNum="  2" />
  <ColumnItem CalcParamName="Код подгруппы" ColumnNum="  3" />
  <ColumnItem CalcParamName="Состояние подгруппы" ColumnNum="  4" />
  <ColumnItem CalcParamName="Тариф поставщика" ColumnNum="7" />
  <ColumnItem CalcParamName="Транспортные расходы, %" ColumnNum="8" />
  <ColumnItem CalcParamName="Сумма расходов на транспорт" ColumnNum="9" />
  <ColumnItem CalcParamName="Таможенные расходы, %" ColumnNum="10" />
  <ColumnItem CalcParamName="Сумма расходов на таможню" ColumnNum="11" />
  <ColumnItem CalcParamName="Наценка базовая, %" ColumnNum="12" />
  <ColumnItem CalcParamName="НДС, %" ColumnNum="16" />
  <ColumnItem CalcParamName="Сумма с НДС" ColumnNum="17" />
  <ColumnItem CalcParamName="Курс ценообразования" ColumnNum="18" />
  <ColumnItem CalcParamName="Наценка, компенсирующая постоянную скидку, %" ColumnNum="13" />
  <ColumnItem CalcParamName="Требуемая наценка, %" ColumnNum="14" />
</SettingForPriceCalc>';

UPDATE [dbo].[T_PriceListCalcSettings] SET [Settings_XML] = @doc;

/****** Object:  StoredProcedure [dbo].[usp_GetPartSubType]    Script Date: 21.03.2013 12:48:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список записей из ( dbo.T_Partsubtype )
--
-- Входящие параметры:
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetPartSubType] 
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

		CREATE TABLE #PartSubTypeStock( Stock_Guid uniqueidentifier, PartSybType_Guid uniqueidentifier, StockQty int );

		INSERT INTO #PartSubTypeStock( Stock_Guid, PartSybType_Guid, StockQty )
		SELECT dbo.GetStockGuidForStockId( STOCK_ID ), dbo.GetPartsubtypeGuidForPartsubtypeId( PARTSUBTYPE_ID ), QUANTITY 
		FROM [DB02].[ERP_Report].dbo.StockRestPartSubTypeView;
		
		CREATE TABLE #PartsubtypeInWarehouseForShipping( PartSybType_Guid uniqueidentifier, StockQty int );
		
		INSERT INTO #PartsubtypeInWarehouseForShipping( PartSybType_Guid, StockQty )
		
		SELECT  #PartSubTypeStock.PartSybType_Guid, SUM( #PartSubTypeStock.StockQty )
		FROM #PartSubTypeStock, dbo.T_Stock, dbo.T_Warehouse as Warehouse
		WHERE #PartSubTypeStock.Stock_Guid = dbo.T_Stock.Stock_Guid
			AND dbo.T_Stock.Warehouse_Guid = Warehouse.Warehouse_Guid
			AND Warehouse.Warehouse_IsForShipping = 1
		GROUP BY #PartSubTypeStock.PartSybType_Guid;	
		
		DROP TABLE 	#PartSubTypeStock;

		WITH Partsubtype AS
		(
			SELECT Partsubtype_Guid, Partsubtype_Id, Partsubtype_Name, Partsubtype_Description, Partsubtype_Image, Partsubtype_IsActive, 
				Partsubtype_VendorTariff, Partsubtype_TransportTariff, Partsubtype_CustomsTariff, Partsubtype_Margin, Partsubtype_NDS, Partsubtype_Discont,
				dbo.GetPartLineGuidForPartSubType( Partsubtype_Guid ) as PartLine_Guid,
				dbo.GetPartOwnerGuidForPartSubType( Partsubtype_Guid ) as PartOwner_Guid,
				dbo.GetPartTypeGuidForPartSubType( Partsubtype_Guid ) as PartType_Guid,
				PartsubtypeState_Guid, 
				dbo.GetIsContainingPartSubTypePartsActualNotValid( Partsubtype_Guid ) as ContainingPartsActualNotValid,
				dbo.GetIsContainingPartSubTypePartsNotValid( Partsubtype_Guid ) as ContainingPartsNotValid, 
				dbo.GetIsContainingPartSubTypePartsActual( Partsubtype_Guid ) as ContainingPartsActual, 
				Partsubtype_PriceEXW, 
				#PartsubtypeInWarehouseForShipping.StockQty, Partsubtype_MarkUpRequired
			FROM dbo.T_Partsubtype LEFT OUTER JOIN #PartsubtypeInWarehouseForShipping ON dbo.T_Partsubtype.Partsubtype_Guid = #PartsubtypeInWarehouseForShipping.PartSybType_Guid
		)
    SELECT Partsubtype.Partsubtype_Guid, Partsubtype.Partsubtype_Id, Partsubtype.Partsubtype_Name, Partsubtype.Partsubtype_Description, 
			Partsubtype.Partsubtype_Image, Partsubtype.Partsubtype_IsActive, 
			Partsubtype.Partsubtype_VendorTariff, Partsubtype.Partsubtype_TransportTariff, Partsubtype.Partsubtype_CustomsTariff, Partsubtype.Partsubtype_Margin, Partsubtype.Partsubtype_NDS, Partsubtype.Partsubtype_Discont,
			Partsubtype.ContainingPartsActualNotValid,
			Partsubtype.ContainingPartsNotValid,
			ContainingPartsActual,
			dbo.GetPartSubtypePriceEXW( Partsubtype.Partsubtype_Guid ) as PartSubtypePriceEXW,
			Partsubtype.PartLine_Guid, Partline.Partline_Id, Partline.Partline_Name, Partline.Partline_Description, Partline.Partline_IsActive,
			Partsubtype.PartOwner_Guid, PartsOwner.Owner_Id, PartsOwner.Owner_Name,
			Partsubtype.PartType_Guid, Parttype.Parttype_Id, Parttype.Parttype_Name, 
			Partsubtype.PartsubtypeState_Guid, PartsubtypeState.PartsubtypeState_Name, PartsubtypeState.PartsubtypeState_Description, PartsubtypeState.PartsubtypeState_IsActive,
			Partsubtype.StockQty AS StockQtyInWarehouseForShipping, Partsubtype.Partsubtype_MarkUpRequired
    FROM Partsubtype LEFT OUTER JOIN dbo.T_Partline as Partline ON Partsubtype.PartLine_Guid = Partline.Partline_Guid 
      LEFT OUTER JOIN dbo.T_Owner as PartsOwner ON Partsubtype.PartOwner_Guid = PartsOwner.Owner_Guid 
      LEFT OUTER JOIN dbo.T_Parttype as Parttype ON Partsubtype.PartType_Guid = Parttype.Parttype_Guid 
      LEFT OUTER JOIN dbo.T_PartsubtypeState as PartsubtypeState ON Partsubtype.PartsubtypeState_Guid = PartsubtypeState.PartsubtypeState_Guid
    --ORDER BY PartsOwner.Owner_Name, Parttype.Parttype_Name, Partsubtype.Partsubtype_Name;
    ORDER BY Partsubtype.Partsubtype_Name;

		DROP TABLE 	#PartsubtypeInWarehouseForShipping;
	--END TRY
	--BEGIN CATCH
	--	SET @ERROR_NUM = ERROR_NUMBER();
	--	SET @ERROR_MES = ERROR_MESSAGE();
	--	RETURN @ERROR_NUM;
	--END CATCH;

  IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';

  RETURN @ERROR_NUM;
END

GO


UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.6654936 WHERE Partsubtype_id = 3757;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.6654936 WHERE Partsubtype_id = 873;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.6654936 WHERE Partsubtype_id = 3806;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.6654936 WHERE Partsubtype_id = 2830;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 37.1406225 WHERE Partsubtype_id = 3487;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 37.1406225 WHERE Partsubtype_id = 337;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1941;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1940;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3907;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3963;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3773;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3579;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3724;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3906;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4004;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4375;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4376;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1945;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3723;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3766;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3908;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3580;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4590;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4546;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4417;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3659;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 476;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 2029;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 2112;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3722;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3721;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3805;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3804;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5362;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5364;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5363;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4573;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3256;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4720;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4574;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5017;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4572;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3479;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3498;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3775;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3480;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3470;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3776;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4418;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3801;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4419;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5273;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5272;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5265;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4846;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.6654936 WHERE Partsubtype_id = 5365;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.6654936 WHERE Partsubtype_id = 872;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.6654936 WHERE Partsubtype_id = 4857;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.6654936 WHERE Partsubtype_id = 4931;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.8296992 WHERE Partsubtype_id = 2827;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.8296992 WHERE Partsubtype_id = 2847;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.8296992 WHERE Partsubtype_id = 5382;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.8296992 WHERE Partsubtype_id = 5383;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.8296992 WHERE Partsubtype_id = 816;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 32.8296992 WHERE Partsubtype_id = 3197;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 37.1406225 WHERE Partsubtype_id = 3734;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 37.1406225 WHERE Partsubtype_id = 3735;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3628;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1570;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3158;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4561;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3034;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3159;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3458;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3473;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 2358;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 2108;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3654;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4488;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3961;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1571;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3653;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1640;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5270;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5271;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3691;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3690;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 2684;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3171;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4421;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3753;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3752;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4003;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3655;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 2020;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1862;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 2109;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4454;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 2711;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3629;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3937;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5030;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5269;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4847;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3063;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3732;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3962;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1574;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 1573;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4551;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5516;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3247;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 5268;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4875;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3903;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3619;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3902;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3687;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3621;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4481;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4480;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4482;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4923;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4922;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4925;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4926;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3917;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 3620;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 43.5293782 WHERE Partsubtype_id = 4924;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 37.1406225 WHERE Partsubtype_id = 2259;
UPDATE T_Partsubtype SET Partsubtype_MarkUpRequired = 37.1406225 WHERE Partsubtype_id = 4721;
