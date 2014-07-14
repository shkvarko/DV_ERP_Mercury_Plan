USE [ERP_Mercury]
GO

UPDATE [dbo].[T_Partsubtype] SET Partsubtype_MarkUpRequired = 0
GO


/****** Object:  View [dbo].[PartSubtypeView]    Script Date: 20.03.2013 11:07:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[PartSubtypeView]
AS
WITH Partsubtype AS (SELECT     Partsubtype_Guid, Partsubtype_Id, Partsubtype_Name, Partsubtype_Description, Partsubtype_Image, Partsubtype_IsActive, 
                                                                       dbo.GetPartLineGuidForPartSubType(Partsubtype_Guid) AS PartLine_Guid, Partsubtype_VendorTariff, Partsubtype_TransportTariff, 
                                                                       Partsubtype_CustomsTariff, Partsubtype_Margin, Partsubtype_NDS, Partsubtype_Discont, Partsubtype_MarkUpRequired
                                                 FROM         dbo.T_Partsubtype)
    SELECT     Partsubtype_1.Partsubtype_Guid, Partsubtype_1.Partsubtype_Id, Partsubtype_1.Partsubtype_Name, Partsubtype_1.Partsubtype_Description, 
                            Partsubtype_1.Partsubtype_Image, Partsubtype_1.Partsubtype_IsActive, Partsubtype_1.Partsubtype_VendorTariff, Partsubtype_1.Partsubtype_TransportTariff, 
                            Partsubtype_1.Partsubtype_CustomsTariff, Partsubtype_1.Partsubtype_Margin, Partsubtype_1.Partsubtype_NDS, Partsubtype_1.Partsubtype_Discont, 
                            Partsubtype_1.PartLine_Guid, Partline.Partline_Id, Partline.Partline_Name, Partline.Partline_Description, Partline.Partline_IsActive, 
                            Partsubtype_1.Partsubtype_MarkUpRequired
     FROM         Partsubtype AS Partsubtype_1 LEFT OUTER JOIN
                            dbo.T_Partline AS Partline ON Partsubtype_1.PartLine_Guid = Partline.Partline_Guid

GO

/****** Object:  StoredProcedure [dbo].[usp_AddPartSubType]    Script Date: 20.03.2013 11:08:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.T_Partsubtype
--
-- Входящие параметры:
-- @InPartsubtype_Id							- код в InterBase в стороннем "Контракте"
--	 @Partsubtype_Name						- наименование
--	 @Partsubtype_Description			- примечание
--	 @Partsubtype_IsActive				- признак активности
--	 @Partline_Guid								- уникальный идентификатор товарной линии
-- @Partsubtype_VendorTariff			- тариф поставщика
-- @Partsubtype_TransportTariff - затраты на транспорт, %
-- @Partsubtype_CustomsTariff		- таможенная пошлина, %
-- @Partsubtype_Margin						- наценка базовая, %
-- @Partsubtype_NDS							- ставка НДС, %
-- @Partsubtype_Discont					- наценка, компенсирующая постоянную скидку, %
-- @PartsubtypeState_Guid				- уи состояния подгруппы
-- @Partsubtype_MarkUpRequired		- требуемая наценка
--
--
-- Выходные параметры:
--  @Partsubtype_Guid						- уникальный идентификатор записи
--  @Partsubtype_Id							- уникальный идентификатор записи в IB
--  @ERROR_NUM										- номер ошибки
--  @ERROR_MES										- текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_AddPartSubType] 
	@InPartsubtype_Id D_ID = NULL,
	@Partsubtype_Name D_NAME,
	@Partsubtype_Description D_DESCRIPTION = NULL,
	@Partsubtype_IsActive D_ISACTIVE,
	@Partline_Guid D_GUID,
	@Partsubtype_VendorTariff D_MONEY,
	@Partsubtype_TransportTariff D_MONEY,
	@Partsubtype_CustomsTariff D_MONEY,
	@Partsubtype_Margin D_MONEY,
	@Partsubtype_NDS D_MONEY,
	@Partsubtype_Discont D_MONEY,
	@PartsubtypeState_Guid D_GUID,
	@Partsubtype_MarkUpRequired	D_MONEY = 0,

	@Partsubtype_Id D_ID output,
  @Partsubtype_Guid D_GUID output,
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = NULL;
    DECLARE @tmpERROR_NUM int;
    DECLARE @tmpERROR_MES nvarchar(4000);
    SET @tmpERROR_NUM = 0;
    SET @tmpERROR_MES = '';

		-- 2012.08.17
		-- Проверим, зарегистрирован ли эта товарная подгруппа в системе, как поступившая из другой системы
		IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )		
			BEGIN
				DECLARE @ExistsPartsubtype_Guid D_GUID = NULL;
				SELECT @ExistsPartsubtype_Guid = Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id;
				
				IF( @ExistsPartsubtype_Guid IS NOT NULL )
					BEGIN
						-- товарная подгруппа была внесена в систему ранее
						IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name AND Partsubtype_Guid <> @ExistsPartsubtype_Guid )
							BEGIN
								DECLARE @CurrentPartsubtype_Name	D_NAME;
								SELECT @CurrentPartsubtype_Name = Partsubtype_Name FROM dbo.T_Partsubtype WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid;

								IF( @CurrentPartsubtype_Name <> @Partsubtype_Name )
									BEGIN
										UPDATE dbo.T_Partsubtype SET Partsubtype_Name = @Partsubtype_Name
										WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid;
										
										-- редактируем подгруппу в InterBase
										EXEC dbo.usp_EditPartsubtypeToIB @Partsubtype_Guid = @ExistsPartsubtype_Guid, @Partsubtype_Name = @Partsubtype_Name, 
											@Partline_Guid = @Partline_Guid, @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;
									END
								
								IF( @ERROR_NUM = 0 )
									BEGIN
										SET @ERROR_NUM = @tmpERROR_NUM;
										SET @ERROR_MES = @tmpERROR_MES;

										SET @Partsubtype_Guid = @ExistsPartsubtype_Guid;
										SET @Partsubtype_Id = @InPartsubtype_Id;
									END
							END
							
					END
				
			END
		
		IF( @ERROR_NUM <> 0 ) RETURN @ERROR_NUM;

    -- Проверяем наличие записи с заданным именем
    IF EXISTS ( SELECT * FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name )
      BEGIN
				SELECT Top 1 @Partsubtype_Guid = Partsubtype_Guid, @Partsubtype_Id = Partsubtype_Id 
				FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name;

				IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )
					BEGIN
						IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id )
							INSERT INTO dbo.T_Partsubtype_AdvCode( Partsubtype_Guid, Partsubtype_Id )	VALUES( @Partsubtype_Guid, @InPartsubtype_Id);
					END

        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует товарная подгруппа с указанным именем.' + Char(13) + 
          'Имя: ' + Char(9) + @Partsubtype_Name + Char(13) + 'Код подгруппы: ' + CAST( @Partsubtype_Id as nvarchar( 10 ) );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной линии с указанным идентификатором
    IF NOT EXISTS ( SELECT * FROM dbo.T_Partline WHERE Partline_Guid = @Partline_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найдена товарная линия с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @Partline_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END
      
    -- Проверяем наличие состояния подгруппы с указанным идентификатором
    IF NOT EXISTS ( SELECT PartsubtypeState_Guid FROM dbo.T_PartsubtypeState WHERE PartsubtypeState_Guid = @PartsubtypeState_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найдено состояние подгруппы с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @PartsubtypeState_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    DECLARE @NewID D_GUID;
    SET @NewID = NEWID ( );	
    EXEC @Partsubtype_Id = SP_GetGeneratorID @strTableName = 'T_Partsubtype';
    
    INSERT INTO dbo.T_Partsubtype( Partsubtype_Guid, Partsubtype_Id, Partsubtype_Name, Partsubtype_Description, Partsubtype_IsActive,
			 Partsubtype_VendorTariff, Partsubtype_TransportTariff, Partsubtype_CustomsTariff, Partsubtype_Margin, Partsubtype_NDS, Partsubtype_Discont, 
			 PartsubtypeState_Guid, Partsubtype_MarkUpRequired )
    VALUES( @NewID, @Partsubtype_Id, @Partsubtype_Name, @Partsubtype_Description, @Partsubtype_IsActive, 
			@Partsubtype_VendorTariff, @Partsubtype_TransportTariff, @Partsubtype_CustomsTariff, @Partsubtype_Margin, @Partsubtype_NDS, @Partsubtype_Discont, 
			@PartsubtypeState_Guid, @Partsubtype_MarkUpRequired );

    SET @Partsubtype_Guid = @NewID;
    
    IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )
			BEGIN
				IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id )
					INSERT INTO dbo.T_Partsubtype_AdvCode( Partsubtype_Guid, Partsubtype_Id )	VALUES( @Partsubtype_Guid, @InPartsubtype_Id);
			END

    IF NOT EXISTS ( SELECT PartsubtypePartline_Guid FROM dbo.T_PartsubtypePartline WHERE Partsubtype_Guid = @Partsubtype_Guid )
			INSERT INTO dbo.T_PartsubtypePartline( PartsubtypePartline_Guid, Partsubtype_Guid, Partline_Guid )
			VALUES( NEWID(), @Partsubtype_Guid, @Partline_Guid );
		ELSE
			UPDATE 	dbo.T_PartsubtypePartline SET Partline_Guid = @Partline_Guid WHERE Partsubtype_Guid = @Partsubtype_Guid;
    

	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();
		RETURN @ERROR_NUM;
	END CATCH;

	SET @ERROR_NUM = 0;
	SET @ERROR_MES = 'Успешное завершение операции.';
	RETURN @ERROR_NUM;
END
GO

/****** Object:  StoredProcedure [dbo].[usp_AddPartSubType2]    Script Date: 20.03.2013 11:08:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.T_Partsubtype  одновременно в Контракт
--
-- Входящие параметры:
--	 @InPartsubtype_Id						- код в InterBase в стороннем "Контракте"
--	 @Partsubtype_Name						- наименование
--	 @Partsubtype_Description			- примечание
--	 @Partsubtype_IsActive				- признак активности
--	 @Partline_Guid								- уникальный идентификатор товарной линии
-- @Partsubtype_VendorTariff			- тариф поставщика
-- @Partsubtype_TransportTariff - затраты на транспорт, %
-- @Partsubtype_CustomsTariff		- таможенная пошлина, %
-- @Partsubtype_Margin						- наценка базовая, %
-- @Partsubtype_NDS							- ставка НДС, %
-- @Partsubtype_Discont					- наценка, компенсирующая постоянную скидку, %
-- @PartsubtypeState_Guid				- уи состояния подгруппы
-- @Partsubtype_MarkUpRequired		- требуемая наценка
--
--
-- Выходные параметры:
--  @Partsubtype_Guid						- уникальный идентификатор записи
--  @Partsubtype_Id							- уникальный идентификатор записи в IB
--  @ERROR_NUM										- номер ошибки
--  @ERROR_MES										- текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_AddPartSubType2] 
	@InPartsubtype_Id D_ID = NULL,
	@Partsubtype_Name D_NAME,
	@Partsubtype_Description D_DESCRIPTION = NULL,
	@Partsubtype_IsActive D_ISACTIVE,
	@Partline_Guid D_GUID,
	@Partsubtype_VendorTariff D_MONEY = 0,
	@Partsubtype_TransportTariff D_MONEY = 0,
	@Partsubtype_CustomsTariff D_MONEY = 0,
	@Partsubtype_Margin D_MONEY = 0,
	@Partsubtype_NDS D_MONEY = 0,
	@Partsubtype_Discont D_MONEY = 0,
	@PartsubtypeState_Guid D_GUID = NULL,
	@Partsubtype_MarkUpRequired	D_MONEY = 0,

	@Partsubtype_Id D_ID output,
  @Partsubtype_Guid D_GUID output,
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = NULL;
    DECLARE @tmpERROR_NUM int;
    DECLARE @tmpERROR_MES nvarchar(4000);
    SET @tmpERROR_NUM = 0;
    SET @tmpERROR_MES = '';
    
    IF( @PartsubtypeState_Guid IS NULL ) SET @PartsubtypeState_Guid = '387C0CE7-2F38-4E1C-820F-6988C5764778'; -- Новинка

		-- 2012.08.17
		-- Проверим, зарегистрирован ли эта товарная подгруппа в системе, как поступившая из другой системы
		IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )		
			BEGIN
				DECLARE @ExistsPartsubtype_Guid D_GUID = NULL;
				SELECT @ExistsPartsubtype_Guid = Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id;
				
				IF( @ExistsPartsubtype_Guid IS NOT NULL )
					BEGIN
						-- товарная подгруппа была внесена в систему ранее
						
						-- редактируем тариф и ставку НДС
						UPDATE dbo.T_Partsubtype SET Partsubtype_VendorTariff = @Partsubtype_VendorTariff, Partsubtype_NDS = @Partsubtype_NDS
						WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid;
						
						IF NOT EXISTS( SELECT Partsubtype_Guid FROM dbo.T_PartsubtypePrice WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid)
							INSERT INTO dbo.T_PartsubtypePrice( Partsubtype_Guid, Partsubtype_PriceEXW, Record_Updated )
							VALUES( @ExistsPartsubtype_Guid, @Partsubtype_VendorTariff, sysutcdatetime() );
						ELSE
							UPDATE  dbo.T_PartsubtypePrice SET Partsubtype_PriceEXW = @Partsubtype_VendorTariff
							WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid;

						-- теперь проверим названия
						IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name AND Partsubtype_Guid <> @ExistsPartsubtype_Guid )
							BEGIN
								DECLARE @CurrentPartsubtype_Name	D_NAME;
								SELECT @CurrentPartsubtype_Name = Partsubtype_Name FROM dbo.T_Partsubtype WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid;

								IF( @CurrentPartsubtype_Name <> @Partsubtype_Name )
									BEGIN
										UPDATE dbo.T_Partsubtype SET Partsubtype_Name = @Partsubtype_Name
										WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid;
										
										-- редактируем подгруппу в InterBase
										EXEC dbo.usp_EditPartsubtypeToIB @Partsubtype_Guid = @ExistsPartsubtype_Guid, @Partsubtype_Name = @Partsubtype_Name, 
											@Partline_Guid = @Partline_Guid, @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;
									END
								
								IF( @ERROR_NUM = 0 )
									BEGIN
										SET @ERROR_NUM = @tmpERROR_NUM;
										SET @ERROR_MES = @tmpERROR_MES;

										SET @Partsubtype_Guid = @ExistsPartsubtype_Guid;
										SET @Partsubtype_Id = @InPartsubtype_Id;
									END
							END
							
					END
				
			END
		
		IF( @ERROR_NUM <> 0 ) RETURN @ERROR_NUM;

    -- Проверяем наличие записи с заданным именем
    IF EXISTS ( SELECT * FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name )
      BEGIN
				SELECT Top 1 @Partsubtype_Guid = Partsubtype_Guid, @Partsubtype_Id = Partsubtype_Id 
				FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name;

				IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )
					BEGIN
						IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id )
							INSERT INTO dbo.T_Partsubtype_AdvCode( Partsubtype_Guid, Partsubtype_Id )	VALUES( @Partsubtype_Guid, @InPartsubtype_Id);
					END

        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует товарная подгруппа с указанным именем.' + Char(13) + 
          'Имя: ' + Char(9) + @Partsubtype_Name + Char(13) + 'Код подгруппы: ' + CAST( @Partsubtype_Id as nvarchar( 10 ) );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной линии с указанным идентификатором
    IF NOT EXISTS ( SELECT * FROM dbo.T_Partline WHERE Partline_Guid = @Partline_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найдена товарная линия с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @Partline_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END
      
    -- Проверяем наличие состояния подгруппы с указанным идентификатором
    IF NOT EXISTS ( SELECT PartsubtypeState_Guid FROM dbo.T_PartsubtypeState WHERE PartsubtypeState_Guid = @PartsubtypeState_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найдено состояние подгруппы с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @PartsubtypeState_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    DECLARE @NewID D_GUID;
    SET @NewID = NEWID ( );	
    EXEC @Partsubtype_Id = SP_GetGeneratorID @strTableName = 'T_Partsubtype';
    
    BEGIN TRANSACTION UpdateData;
    
    INSERT INTO dbo.T_Partsubtype( Partsubtype_Guid, Partsubtype_Id, Partsubtype_Name, Partsubtype_Description, Partsubtype_IsActive,
			 Partsubtype_VendorTariff, Partsubtype_TransportTariff, Partsubtype_CustomsTariff, Partsubtype_Margin, Partsubtype_NDS, Partsubtype_Discont, 
			 PartsubtypeState_Guid, Partsubtype_MarkUpRequired )
    VALUES( @NewID, @Partsubtype_Id, @Partsubtype_Name, @Partsubtype_Description, @Partsubtype_IsActive, 
			@Partsubtype_VendorTariff, @Partsubtype_TransportTariff, @Partsubtype_CustomsTariff, @Partsubtype_Margin, @Partsubtype_NDS, @Partsubtype_Discont, 
			@PartsubtypeState_Guid, @Partsubtype_MarkUpRequired );

    SET @Partsubtype_Guid = @NewID;
    
    IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )
			BEGIN
				IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id )
					INSERT INTO dbo.T_Partsubtype_AdvCode( Partsubtype_Guid, Partsubtype_Id )	VALUES( @Partsubtype_Guid, @InPartsubtype_Id);
			END
		-- товарная линия
    IF NOT EXISTS ( SELECT PartsubtypePartline_Guid FROM dbo.T_PartsubtypePartline WHERE Partsubtype_Guid = @Partsubtype_Guid )
			INSERT INTO dbo.T_PartsubtypePartline( PartsubtypePartline_Guid, Partsubtype_Guid, Partline_Guid )
			VALUES( NEWID(), @Partsubtype_Guid, @Partline_Guid );
		ELSE
			UPDATE 	dbo.T_PartsubtypePartline SET Partline_Guid = @Partline_Guid WHERE Partsubtype_Guid = @Partsubtype_Guid;
    -- цена exw
		IF NOT EXISTS( SELECT Partsubtype_Guid FROM dbo.T_PartsubtypePrice WHERE Partsubtype_Guid = @Partsubtype_Guid )
			INSERT INTO dbo.T_PartsubtypePrice( Partsubtype_Guid, Partsubtype_PriceEXW, Record_Updated )
			VALUES( @Partsubtype_Guid, @Partsubtype_VendorTariff, sysutcdatetime() );
		ELSE
			UPDATE  dbo.T_PartsubtypePrice SET Partsubtype_PriceEXW = @Partsubtype_VendorTariff
			WHERE Partsubtype_Guid = @Partsubtype_Guid;

    EXEC dbo.usp_AddPartSubTypeToIB @Partsubtype_Guid = @Partsubtype_Guid, @IBLINKEDSERVERNAME = NULL, 
			@ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;

		IF( @ERROR_NUM = 0 ) COMMIT TRANSACTION UpdateData
			ELSE ROLLBACK TRANSACTION UpdateData;
    

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION UpdateData;

    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();
		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';
		
	RETURN @ERROR_NUM;
END
GO

/****** Object:  StoredProcedure [dbo].[usp_EditPartSubTypeList]    Script Date: 20.03.2013 14:33:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Изменяет свойства записи в таблице dbo.T_Partsubtype
--
-- Входящие параметры:
--  @tProductSubType - таблица со списком уникальных идентификаторов записей
--	@Partsubtype_Name - наименование
--	@Partsubtype_Description - примечание
--	@Partsubtype_IsActive - признак активности
--	@Partline_Guid - уникальный идентификатор товарной линии
	--@Partsubtype_VendorTariff- тариф поставщика
	--@Partsubtype_TransportTariff - величина (пройент) расходов на транспорт
	--@Partsubtype_CustomsTariff - размер (процент) таможенной пошлины
	--@Partsubtype_Margin - маржа (процент)
	--@Partsubtype_NDS - ставка НДС (процент)
	--@Partsubtype_Discont - дисконт (средняя сложившаяся скидка)
-- @Partsubtype_MarkUpRequired		- требуемая наценка
--
--
-- Выходные параметры:
--  @ERROR_NUM - номер ошибки
--  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_EditPartSubTypeList] 
  @tProductSubType dbo.udt_ProductSubType READONLY,
	@Partline_Guid D_GUID = NULL,
	@Partsubtype_VendorTariff D_MONEY = NULL,
	@Partsubtype_TransportTariff D_MONEY = NULL,
	@Partsubtype_CustomsTariff D_MONEY = NULL,
	@Partsubtype_Margin D_MONEY = NULL,
	@Partsubtype_NDS D_MONEY = NULL,
	@Partsubtype_Discont D_MONEY = NULL,
	@Partsubtype_PriceEXW D_MONEY = NULL,
	@PartsubtypeState_Guid D_GUID = NULL,
	@Partsubtype_MarkUpRequired	D_MONEY = NULL,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = NULL;

    DECLARE @Partsubtype_Guid D_GUID;
    
    IF( @Partline_Guid IS NOT NULL )
      BEGIN
        UPDATE dbo.T_PartsubtypePartline SET Partline_Guid = @Partline_Guid WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
        
        INSERT INTO dbo.T_PartsubtypePartline( PartsubtypePartline_Guid, Partsubtype_Guid, Partline_Guid )
        SELECT NEWID(), ProductSubType_Guid, @Partline_Guid FROM @tProductSubType
        WHERE ProductSubType_Guid NOT IN ( SELECT Partsubtype_Guid FROM dbo.T_PartsubtypePartline );
        
      END

    IF( @PartsubtypeState_Guid IS NOT NULL )
      BEGIN
        UPDATE dbo.T_Partsubtype SET PartsubtypeState_Guid = @PartsubtypeState_Guid
        WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
      END

    IF( @Partsubtype_VendorTariff IS NOT NULL )
      BEGIN
        UPDATE dbo.T_Partsubtype SET Partsubtype_VendorTariff = @Partsubtype_VendorTariff
        WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
      END

    IF( @Partsubtype_TransportTariff IS NOT NULL )
      BEGIN
        UPDATE dbo.T_Partsubtype SET Partsubtype_TransportTariff = @Partsubtype_TransportTariff
        WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
      END

    IF( @Partsubtype_CustomsTariff IS NOT NULL )
      BEGIN
        UPDATE dbo.T_Partsubtype SET Partsubtype_CustomsTariff = @Partsubtype_CustomsTariff
        WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
      END

    IF( @Partsubtype_Margin IS NOT NULL )
      BEGIN
        UPDATE dbo.T_Partsubtype SET Partsubtype_Margin = @Partsubtype_Margin
        WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
      END

    IF( @Partsubtype_NDS IS NOT NULL )
      BEGIN
        UPDATE dbo.T_Partsubtype SET Partsubtype_NDS = @Partsubtype_NDS
        WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
      END

    IF( @Partsubtype_Discont IS NOT NULL )
      BEGIN
        UPDATE dbo.T_Partsubtype SET Partsubtype_Discont = @Partsubtype_Discont
        WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
      END

    IF( @Partsubtype_PriceEXW IS NOT NULL )
      BEGIN
        UPDATE dbo.T_PartsubtypePrice SET Partsubtype_PriceEXW = @Partsubtype_PriceEXW WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
        
        INSERT INTO dbo.T_PartsubtypePrice( Partsubtype_Guid, Partsubtype_PriceEXW, Record_Updated )
        SELECT ProductSubType_Guid, @Partsubtype_PriceEXW, sysutcdatetime() FROM @tProductSubType
        WHERE ProductSubType_Guid NOT IN ( SELECT Partsubtype_Guid FROM dbo.T_PartsubtypePrice );
        
      END

    IF( @Partsubtype_MarkUpRequired IS NOT NULL )
      BEGIN
        UPDATE dbo.T_Partsubtype SET Partsubtype_MarkUpRequired = @Partsubtype_MarkUpRequired
        WHERE Partsubtype_Guid IN ( SELECT ProductSubType_Guid FROM @tProductSubType );
      END

	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();
		RETURN @ERROR_NUM;
	END CATCH;

	SET @ERROR_NUM = 0;
	SET @ERROR_MES = 'Успешное завершение операции.';
	RETURN @ERROR_NUM;
END
GO

/****** Object:  StoredProcedure [dbo].[usp_EditPartSubType]    Script Date: 20.03.2013 14:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Изменяет свойства записи в таблице dbo.T_Partsubtype
--
-- Входящие параметры:
--  @Partsubtype_Guid - уникальный идентификатор записи
--	@Partsubtype_Name - наименование
--	@Partsubtype_Description - примечание
--	@Partsubtype_IsActive - признак активности
--	@Partline_Guid - уникальный идентификатор товарной линии
	--@Partsubtype_VendorTariff- тариф поставщика
	--@Partsubtype_TransportTariff - величина (пройент) расходов на транспорт
	--@Partsubtype_CustomsTariff - размер (процент) таможенной пошлины
	--@Partsubtype_Margin - маржа (процент)
	--@Partsubtype_NDS - ставка НДС (процент)
	--@Partsubtype_Discont - дисконт (средняя сложившаяся скидка)
-- @Partsubtype_MarkUpRequired		- требуемая наценка
--
--
-- Выходные параметры:
--  @ERROR_NUM - номер ошибки
--  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_EditPartSubType] 
  @Partsubtype_Guid D_GUID,
	@Partsubtype_Name D_NAME,
	@Partsubtype_Description D_DESCRIPTION = NULL,
	@Partsubtype_IsActive D_ISACTIVE,
	@Partline_Guid D_GUID,
	@Partsubtype_VendorTariff D_MONEY,
	@Partsubtype_TransportTariff D_MONEY,
	@Partsubtype_CustomsTariff D_MONEY,
	@Partsubtype_Margin D_MONEY,
	@Partsubtype_NDS D_MONEY,
	@Partsubtype_Discont D_MONEY,
	@PartsubtypeState_Guid D_GUID,
	@Partsubtype_MarkUpRequired	D_MONEY = NULL,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие записи с заданным идентификатором
    IF NOT EXISTS ( SELECT * FROM dbo.T_Partsubtype WHERE Partsubtype_Guid = @Partsubtype_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена товарная подгруппа с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @Partsubtype_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие записи с заданным именем
    IF EXISTS ( SELECT * FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name AND Partsubtype_Guid <> @Partsubtype_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных уже присутствует товарная подгруппа с указанным именем.' + Char(13) + 
          'Имя: ' + Char(9) + @Partsubtype_Name;
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной линии с указанным идентификатором
    IF NOT EXISTS ( SELECT * FROM dbo.T_Partline WHERE Partline_Guid = @Partline_Guid )
      BEGIN
        SET @ERROR_NUM = 3;
        SET @ERROR_MES = 'В базе данных не найдена товарная линия с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @Partline_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие состояния подгруппы с указанным идентификатором
    IF NOT EXISTS ( SELECT PartsubtypeState_Guid FROM dbo.T_PartsubtypeState WHERE PartsubtypeState_Guid = @PartsubtypeState_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найдено состояние подгруппы с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @PartsubtypeState_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    UPDATE dbo.T_Partsubtype SET Partsubtype_Name = @Partsubtype_Name, Partsubtype_Description = @Partsubtype_Description, 
			Partsubtype_IsActive = @Partsubtype_IsActive, Partsubtype_VendorTariff = @Partsubtype_VendorTariff, 
			Partsubtype_TransportTariff = @Partsubtype_TransportTariff, Partsubtype_CustomsTariff = @Partsubtype_CustomsTariff, 
			Partsubtype_Margin = @Partsubtype_Margin, Partsubtype_NDS = @Partsubtype_NDS, Partsubtype_Discont = @Partsubtype_Discont, 
			PartsubtypeState_Guid = @PartsubtypeState_Guid
		WHERE Partsubtype_Guid = @Partsubtype_Guid;

		IF( @Partsubtype_MarkUpRequired IS NOT NULL )
			UPDATE dbo.T_Partsubtype SET Partsubtype_MarkUpRequired = @Partsubtype_MarkUpRequired
			WHERE Partsubtype_Guid = @Partsubtype_Guid;
    
    IF NOT EXISTS ( SELECT PartsubtypePartline_Guid FROM dbo.T_PartsubtypePartline WHERE Partsubtype_Guid = @Partsubtype_Guid )
			INSERT INTO dbo.T_PartsubtypePartline( PartsubtypePartline_Guid, Partsubtype_Guid, Partline_Guid )
			VALUES( NEWID(), @Partsubtype_Guid, @Partline_Guid );
		ELSE
			UPDATE 	dbo.T_PartsubtypePartline SET Partline_Guid = @Partline_Guid WHERE Partsubtype_Guid = @Partsubtype_Guid;
    

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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Creation date:	
-- Author:			

CREATE FUNCTION [dbo].[GetCurrentCurrencyMainGuid] ()
RETURNS D_GUID
WITH EXECUTE AS caller
AS
BEGIN
  
	DECLARE @Currency_Guid D_GUID = NULL;
	DECLARE @Currency_Code D_CURRENCYCODE = 'EUR';
	

	SELECT Top 1 @Currency_Guid = Currency_Guid FROM dbo.T_Currency WHERE Currency_Abbr = @Currency_Code;

	RETURN @Currency_Guid;

end

GO
GRANT EXECUTE ON [dbo].[GetCurrentCurrencyMainGuid] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Creation date:	
-- Author:			

CREATE FUNCTION [dbo].[GetNationalCurrencyMainGuid] ()
RETURNS D_GUID
WITH EXECUTE AS caller
AS
BEGIN
  
	DECLARE @Currency_Guid D_GUID = NULL;
	DECLARE @Currency_Code D_CURRENCYCODE = 'BYB';
	

	SELECT Top 1 @Currency_Guid = Currency_Guid FROM dbo.T_Currency WHERE Currency_Abbr = @Currency_Code;

	RETURN @Currency_Guid;

end

GO
GRANT EXECUTE ON [dbo].[GetNationalCurrencyMainGuid] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает текущее значение курса ценообразования
--
-- Входящие параметры:
--
-- Выходные параметры:
--
-- @CurrencyRate_Value			- курс ценообразования
-- @ERROR_NUM							- номер ошибки
-- @ERROR_MES							- сообщение об ошибке
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetCurrentCurrencyRatePricing] 
	@CurrencyRate_Value	float output,
  @ERROR_NUM					int output,
  @ERROR_MES					nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';
	SET @CurrencyRate_Value = 0;

  BEGIN TRY

		DECLARE @CurrencyRate_Guid				D_GUID = NULL;
		DECLARE @CurrencyRate_Date				D_DATE = NULL;
		DECLARE @NationalCurrency_Guid		D_GUID = NULL;
		DECLARE @CurrentMainCurrency_Guid	D_GUID = NULL;

		SET @NationalCurrency_Guid = ( SELECT dbo.GetNationalCurrencyMainGuid() );
		SET @CurrentMainCurrency_Guid = ( SELECT dbo.GetCurrentCurrencyMainGuid() );


		SELECT @CurrencyRate_Date = MAX( [CurrencyRate_Date] ) FROM [dbo].[T_CurrencyRate]
		WHERE [Currency_In_Guid] = @CurrentMainCurrency_Guid
			AND [Currency_Out_Guid] = @NationalCurrency_Guid
			AND [CurrencyRate_Pricing] = 1;

		IF( @CurrencyRate_Date IS NOT NULL )
			SELECT TOP 1 @CurrencyRate_Guid = [CurrencyRate_Guid]
			FROM [dbo].[T_CurrencyRate]
			WHERE [CurrencyRate_Date] = @CurrencyRate_Date
				AND [Currency_In_Guid] = @CurrentMainCurrency_Guid
				AND [Currency_Out_Guid] = @NationalCurrency_Guid
				AND [CurrencyRate_Pricing] = 1;
		
		IF( @CurrencyRate_Guid IS NOT NULL )
			SELECT @CurrencyRate_Value = [CurrencyRate_Value]
			FROM [dbo].[T_CurrencyRate]
			WHERE [CurrencyRate_Guid] = @CurrencyRate_Guid;
  
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
GRANT EXECUTE ON [dbo].[usp_GetCurrentCurrencyRatePricing] TO [public]
GO
