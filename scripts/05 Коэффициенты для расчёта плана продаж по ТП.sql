USE [ERP_Mercury]
GO

CREATE TYPE [dbo].[D_QUOTA] FROM NUMERIC(18, 5) NOT NULL
GO

CREATE XML SCHEMA COLLECTION [dbo].[SalesPlanQuotaConditionSchema] AS N'<xsd:schema xmlns:xsd="http://www.w3.org/2001/XMLSchema" elementFormDefault="qualified">
	<xsd:element name="SalesPlanQuotaCondition">
		<xsd:complexType>
			<xsd:complexContent>
				<xsd:restriction base="xsd:anyType">
					<xsd:sequence minOccurs="0" maxOccurs="unbounded">
						<xsd:element name="SalesPlanQuotaConditionItem">
							<xsd:complexType>
								<xsd:complexContent>
									<xsd:restriction base="xsd:anyType">
										<xsd:sequence/>
										<xsd:attribute name="ItemTypeId" type="xsd:int" use="required"/>
										<xsd:attribute name="ItemGuid" type="xsd:string" use="required"/>
										<xsd:attribute name="ItemName" type="xsd:string" use="required"/>
									</xsd:restriction>
								</xsd:complexContent>
							</xsd:complexType>
						</xsd:element>
					</xsd:sequence>
				</xsd:restriction>
			</xsd:complexContent>
		</xsd:complexType>
	</xsd:element>
</xsd:schema>'
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_SalesPlanQuota](
	[SalesPlanQuota_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuota_Name] [dbo].[D_NAME] NOT NULL,
	[SalesPlanQuota_Date] [dbo].[D_DATE] NOT NULL,
	[SalesPlanQuota_BeginDate] [dbo].[D_DATE] NULL,
	[SalesPlanQuota_EndDate] [dbo].[D_DATE] NOT NULL,
	[SalesPlanQuota_Condition] xml NOT NULL,
	[SalesPlanQuota_Description] [dbo].[D_DESCRIPTION] NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
 CONSTRAINT [PK_T_SalesPlanQuota] PRIMARY KEY CLUSTERED 
(
	[SalesPlanQuota_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_Name] ON [dbo].[T_SalesPlanQuota]
(
	[SalesPlanQuota_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_Date] ON [dbo].[T_SalesPlanQuota]
(
	[SalesPlanQuota_Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_BeginDate] ON [dbo].[T_SalesPlanQuota]
(
	[SalesPlanQuota_BeginDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_EndDate] ON [dbo].[T_SalesPlanQuota]
(
	[SalesPlanQuota_EndDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_SalesPlanQuota_Archive](
	[SalesPlanQuota_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuota_Name] [dbo].[D_NAME] NOT NULL,
	[SalesPlanQuota_Date] [dbo].[D_DATE] NOT NULL,
	[SalesPlanQuota_BeginDate] [dbo].[D_DATE] NULL,
	[SalesPlanQuota_EndDate] [dbo].[D_DATE] NOT NULL,
	[SalesPlanQuota_Condition] [xml] NOT NULL,
	[SalesPlanQuota_Description] [dbo].[D_DESCRIPTION] NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
	[Action_TypeId] [dbo].[D_ID] NOT NULL,

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_Archive_Guid] ON [dbo].[T_SalesPlanQuota_Archive]
(
	[SalesPlanQuota_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_Archive_Name] ON [dbo].[T_SalesPlanQuota_Archive]
(
	[SalesPlanQuota_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_Archive_Date] ON [dbo].[T_SalesPlanQuota_Archive]
(
	[SalesPlanQuota_Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_Archive_BeginDate] ON [dbo].[T_SalesPlanQuota_Archive]
(
	[SalesPlanQuota_BeginDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_Archive_EndDate] ON [dbo].[T_SalesPlanQuota_Archive]
(
	[SalesPlanQuota_EndDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuota_Archive_Action_TypeId] ON [dbo].[T_SalesPlanQuota_Archive]
(
	[Action_TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер обновляет время редактирования/вставки записи
-- =============================================
CREATE TRIGGER [dbo].[TG_SalesPlanQuota_AfterUpdate]
   ON  [dbo].[T_SalesPlanQuota] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuota_Archive ( SalesPlanQuota_Guid, SalesPlanQuota_Name, SalesPlanQuota_Date,	SalesPlanQuota_BeginDate,
		SalesPlanQuota_EndDate,	SalesPlanQuota_Condition,	SalesPlanQuota_Description,
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuota_Guid, SalesPlanQuota_Name, SalesPlanQuota_Date,	SalesPlanQuota_BeginDate,
		SalesPlanQuota_EndDate,	SalesPlanQuota_Condition,	SalesPlanQuota_Description,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 0
	FROM inserted;

	UPDATE dbo.[T_SalesPlanQuota] SET Record_Updated = sysutcdatetime(), Record_UserUdpated = ( Host_Name() + ': ' + SUSER_SNAME() )
	WHERE SalesPlanQuota_Guid IN ( SELECT SalesPlanQuota_Guid FROM inserted );
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер добавляет записи в случае их удаления в таблицу удаленных записей
-- =============================================
CREATE TRIGGER [dbo].[TG_SalesPlanQuota_AfterDelete]
   ON [dbo].[T_SalesPlanQuota] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.T_SalesPlanQuota_Archive ( SalesPlanQuota_Guid, SalesPlanQuota_Name, SalesPlanQuota_Date,	SalesPlanQuota_BeginDate,
		SalesPlanQuota_EndDate,	SalesPlanQuota_Condition,	SalesPlanQuota_Description,
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuota_Guid, SalesPlanQuota_Name, SalesPlanQuota_Date,	SalesPlanQuota_BeginDate,
		SalesPlanQuota_EndDate,	SalesPlanQuota_Condition,	SalesPlanQuota_Description,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 2

	FROM deleted;
END

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Возвращает список записей из [T_SalesPlanQuota]
--
-- Входные параметры:
--		@BeginDate			- дата начала периода поиска
--		@EndDate				- дата окончания периода поиска
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetSalesPlanQuota] 
	@BeginDate			D_DATE,
	@EndDate				D_DATE,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY

    SELECT SalesPlanQuota_Guid, SalesPlanQuota_Name, SalesPlanQuota_Date, SalesPlanQuota_BeginDate, SalesPlanQuota_EndDate, 
			SalesPlanQuota_Condition, SalesPlanQuota_Description
		FROM [dbo].[T_SalesPlanQuota] 
		WHERE [SalesPlanQuota_Date] BETWEEN @BeginDate AND @EndDate
		ORDER BY SalesPlanQuota_Name;

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
GRANT EXECUTE ON [dbo].[usp_GetSalesPlanQuota] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Добавляет в базу данных описание расчёта коэффициента продаж
--
-- Входные параметры:
--
--		@SalesPlanQuota_Name				наименование
--		@SalesPlanQuota_Date				дата регистрации
--		@SalesPlanQuota_BeginDate		начало периода продаж
--		@SalesPlanQuota_EndDate			конец периода продаж
--		@SalesPlanQuota_Description	примечание
--		@SalesPlanQuota_Condition		условия расчёта
--
--		Выходные параметры
--
--		@SalesPlanQuota_Guid				- УИ записи
--		@ERROR_NUM									- номер ошибки
--		@ERROR_MES									- текст ошибки
--
-- Результат:
--    0 - успешное завершение
--		<>0 - ошибка
--
-- Возвращает:
--

CREATE PROCEDURE [dbo].[usp_AddSalesPlanQuota]
	@SalesPlanQuota_Name				D_NAME,
	@SalesPlanQuota_Date				D_DATE,
	@SalesPlanQuota_BeginDate		D_DATE,
	@SalesPlanQuota_EndDate			D_DATE,
	@SalesPlanQuota_Description	D_DESCRIPTION,
  @SalesPlanQuota_Condition		xml (DOCUMENT SalesPlanQuotaConditionSchema),

  @SalesPlanQuota_Guid				D_GUID output,
  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output
AS

BEGIN

  BEGIN TRY
    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    SET @SalesPlanQuota_Guid = NULL;
    
    -- Проверяем наличие расчёта с указанным номером
    IF EXISTS ( SELECT [SalesPlanQuota_Guid] FROM [dbo].[T_SalesPlanQuota] WHERE [SalesPlanQuota_Name] = @SalesPlanQuota_Name )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'расчёт с указанным номером уже зарегистрирован.' + Char(13) + 
          '№ ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Name  );
        RETURN @ERROR_NUM;
      END
    
    -- Проверяем наличие валюты с указанным идентификатором
    IF( DATEDIFF( day, @SalesPlanQuota_BeginDate, @SalesPlanQuota_EndDate ) < 0 )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'Неверно задан период дат для поиска: [' + CONVERT( nvarchar(10), @SalesPlanQuota_BeginDate, 104  )  + ' - ' + 
					CONVERT( nvarchar(10), @SalesPlanQuota_EndDate, 104 ) + ']';
        RETURN @ERROR_NUM;
      END


		-- Регистрируем новую запись
    DECLARE @NewID D_GUID;
    SET @NewID = NEWID( );	

		INSERT INTO [dbo].[T_SalesPlanQuota]( SalesPlanQuota_Guid, SalesPlanQuota_Name, SalesPlanQuota_Date, 
			SalesPlanQuota_BeginDate, SalesPlanQuota_EndDate, SalesPlanQuota_Condition, SalesPlanQuota_Description, 
			Record_Updated,	Record_UserUdpated )
		VALUES( @NewID, @SalesPlanQuota_Name, @SalesPlanQuota_Date, 
			@SalesPlanQuota_BeginDate, @SalesPlanQuota_EndDate, @SalesPlanQuota_Condition, @SalesPlanQuota_Description,
			sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
		
		SET @SalesPlanQuota_Guid = @NewID;

		   
 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = 'Ошибка добавления в базу данных информации о заказе. ' + ERROR_MESSAGE();
    
		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';
		
	RETURN @ERROR_NUM;
END

GO
GRANT EXECUTE ON [dbo].[usp_AddSalesPlanQuota] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Редактирует в базе данных описание расчёта коэффициента продаж
--
-- Входные параметры:
--
--		@SalesPlanQuota_Guid				УИ записи
--		@SalesPlanQuota_Name				наименование
--		@SalesPlanQuota_Date				дата регистрации
--		@SalesPlanQuota_BeginDate		начало периода продаж
--		@SalesPlanQuota_EndDate			конец периода продаж
--		@SalesPlanQuota_Description	примечание
--		@SalesPlanQuota_Condition		условия расчёта
--
--		Выходные параметры
--
--		@ERROR_NUM									номер ошибки
--		@ERROR_MES									текст ошибки
--
-- Результат:
--    0 - успешное завершение
--		<>0 - ошибка
--
-- Возвращает:
--

CREATE PROCEDURE [dbo].[usp_EditSalesPlanQuota]
  @SalesPlanQuota_Guid				D_GUID,
	@SalesPlanQuota_Name				D_NAME,
	@SalesPlanQuota_Date				D_DATE,
	@SalesPlanQuota_BeginDate		D_DATE,
	@SalesPlanQuota_EndDate			D_DATE,
	@SalesPlanQuota_Description	D_DESCRIPTION,
  @SalesPlanQuota_Condition		xml (DOCUMENT SalesPlanQuotaConditionSchema),

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output
AS

BEGIN

  BEGIN TRY
    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    
    -- Проверяем наличие расчёта с указанным номером
    IF NOT EXISTS ( SELECT [SalesPlanQuota_Guid] FROM [dbo].[T_SalesPlanQuota] WHERE SalesPlanQuota_Guid = @SalesPlanQuota_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'Расчёт с указанным идентификатором не найден.' + Char(13) + 
          '№ ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие расчёта с указанным номером
    IF EXISTS ( SELECT [SalesPlanQuota_Guid] FROM [dbo].[T_SalesPlanQuota] WHERE [SalesPlanQuota_Name] = @SalesPlanQuota_Name AND SalesPlanQuota_Guid <> @SalesPlanQuota_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'расчёт с указанным номером уже зарегистрирован.' + Char(13) + 
          '№ ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Name  );
        RETURN @ERROR_NUM;
      END
    
    -- Проверяем наличие валюты с указанным идентификатором
    IF( DATEDIFF( day, @SalesPlanQuota_BeginDate, @SalesPlanQuota_EndDate ) < 0 )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'Неверно задан период дат для поиска: [' + CONVERT( nvarchar(10), @SalesPlanQuota_BeginDate, 104  )  + ' - ' + 
					CONVERT( nvarchar(10), @SalesPlanQuota_EndDate, 104 ) + ']';
        RETURN @ERROR_NUM;
      END

		UPDATE [dbo].[T_SalesPlanQuota] SET [SalesPlanQuota_Name] = @SalesPlanQuota_Name, [SalesPlanQuota_Date] = @SalesPlanQuota_Date, 
			[SalesPlanQuota_BeginDate] = @SalesPlanQuota_BeginDate, [SalesPlanQuota_EndDate] = @SalesPlanQuota_EndDate, 
			[SalesPlanQuota_Condition] = @SalesPlanQuota_Condition, [SalesPlanQuota_Description] = @SalesPlanQuota_Description
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid;
				   
 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = 'Ошибка редактирование записи в базе данных. ' + ERROR_MESSAGE();
    
		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';
		
	RETURN @ERROR_NUM;
END

GO
GRANT EXECUTE ON [dbo].[usp_EditSalesPlanQuota] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Удаляет в базе данных описание расчёта коэффициента продаж
--
-- Входные параметры:
--
--		@SalesPlanQuota_Guid				УИ записи
--
--		Выходные параметры
--
--		@ERROR_NUM									номер ошибки
--		@ERROR_MES									текст ошибки
--
-- Результат:
--    0 - успешное завершение
--		<>0 - ошибка
--
-- Возвращает:
--

CREATE PROCEDURE [dbo].[usp_DeleteSalesPlanQuota]
  @SalesPlanQuota_Guid				D_GUID,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output
AS

BEGIN

  BEGIN TRY
    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    
    -- Проверяем наличие расчёта с указанным номером
    IF NOT EXISTS ( SELECT [SalesPlanQuota_Guid] FROM [dbo].[T_SalesPlanQuota] WHERE SalesPlanQuota_Guid = @SalesPlanQuota_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'Расчёт с указанным идентификатором не найден.' + Char(13) + 
          '№ ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END
		
		DELETE FROM [dbo].[T_SalesPlanQuota]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid;
				   
 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = 'Ошибка удаления записи в базе данных. ' + ERROR_MESSAGE();
    
		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';
		
	RETURN @ERROR_NUM;
END

GO
GRANT EXECUTE ON [dbo].[usp_DeleteSalesPlanQuota] TO [public]
GO

/****** Object:  Table [dbo].[T_SalesPlanQuotaItem]    Script Date: 15.10.2013 13:59:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItem](
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuota_Guid] [dbo].[D_GUID] NOT NULL,
	[ProductOwner_Guid] [dbo].[D_GUID] NOT NULL,
	[ProductType_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItem_Money] [dbo].[D_MONEY] NOT NULL,
 CONSTRAINT [PK_T_SalesPlanQuotaItem] PRIMARY KEY CLUSTERED 
(
	[SalesPlanQuotaItem_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItem]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItem_T_Owner] FOREIGN KEY([ProductOwner_Guid])
REFERENCES [dbo].[T_Owner] ([Owner_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItem] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItem_T_Owner]
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItem]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItem_T_Parttype] FOREIGN KEY([ProductType_Guid])
REFERENCES [dbo].[T_Parttype] ([Parttype_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItem] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItem_T_Parttype]
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItem]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItem_T_SalesPlanQuota] FOREIGN KEY([SalesPlanQuota_Guid])
REFERENCES [dbo].[T_SalesPlanQuota] ([SalesPlanQuota_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItem] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItem_T_SalesPlanQuota]
GO



/****** Object:  Table [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]    Script Date: 15.10.2013 14:06:22 ******/
IF EXISTS (SELECT * FROM sys.tables WHERE NAME = 'T_SalesPlanQuotaItemDecodeDepartTeam')
	DROP TABLE T_SalesPlanQuotaItemDecodeDepartTeam;

IF EXISTS (SELECT * FROM sys.tables WHERE NAME = 'T_SalesPlanQuotaItemDecodeDepart')
	DROP TABLE T_SalesPlanQuotaItemDecodeDepart;

IF EXISTS (SELECT * FROM sys.tables WHERE NAME = 'T_SalesPlanQuotaItemDecodeCustomer')
	DROP TABLE T_SalesPlanQuotaItemDecodeCustomer;

IF EXISTS (SELECT * FROM sys.tables WHERE NAME = 'T_SalesPlanQuotaItemDecodePartSubType')
	DROP TABLE T_SalesPlanQuotaItemDecodePartSubType;

IF EXISTS (SELECT * FROM sys.tables WHERE NAME = 'T_SalesPlanQuotaItemDecodeDepartTeam_Archive')
	DROP TABLE T_SalesPlanQuotaItemDecodeDepartTeam_Archive;

IF EXISTS (SELECT * FROM sys.tables WHERE NAME = 'T_SalesPlanQuotaItemDecodeDepart_Archive')
	DROP TABLE T_SalesPlanQuotaItemDecodeDepart_Archive;

IF EXISTS (SELECT * FROM sys.tables WHERE NAME = 'T_SalesPlanQuotaItemDecodeCustomer_Archive')
	DROP TABLE T_SalesPlanQuotaItemDecodeCustomer_Archive;

IF EXISTS (SELECT * FROM sys.tables WHERE NAME = 'T_SalesPlanQuotaItemDecodePartSubType_Archive')
	DROP TABLE T_SalesPlanQuotaItemDecodePartSubType_Archive;

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam](
	[SalesPlanQuotaItemDecode_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[DepartTeam_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItemDecode_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItemDecode_Money] [dbo].[D_MONEY] NOT NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [dbo].[D_QUOTA] NOT NULL,
	[SalesPlanQuotaItemDecode_Quota] [dbo].[D_QUOTA] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
 CONSTRAINT [PK_T_SalesPlanQuotaItemDecodeDepartTeam] PRIMARY KEY CLUSTERED 
(
	[SalesPlanQuotaItemDecode_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam_Archive](
	[SalesPlanQuotaItemDecode_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[DepartTeam_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItemDecode_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItemDecode_Money] [dbo].[D_MONEY] NOT NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [dbo].[D_QUOTA] NOT NULL,
	[SalesPlanQuotaItemDecode_Quota] [dbo].[D_QUOTA] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
	[Action_TypeId]	[dbo].[D_ID] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeDepartTeam_T_DepartTeam] FOREIGN KEY([DepartTeam_Guid])
REFERENCES [dbo].[T_DepartTeam] ([DepartTeam_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeDepartTeam_T_DepartTeam]
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeDepartTeam_T_SalesPlanQuotaItem] FOREIGN KEY([SalesPlanQuotaItem_Guid])
REFERENCES [dbo].[T_SalesPlanQuotaItem] ([SalesPlanQuotaItem_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeDepartTeam_T_SalesPlanQuotaItem]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepart](
	[SalesPlanQuotaItemDecode_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[Depart_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItemDecode_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItemDecode_Money] [dbo].[D_MONEY] NOT NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [dbo].[D_QUOTA] NOT NULL,
	[SalesPlanQuotaItemDecode_Quota] [dbo].[D_QUOTA] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
 CONSTRAINT [PK_T_SalesPlanQuotaItemDecodeDepart] PRIMARY KEY CLUSTERED 
(
	[SalesPlanQuotaItemDecode_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepart_Archive](
	[SalesPlanQuotaItemDecode_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[Depart_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItemDecode_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItemDecode_Money] [dbo].[D_MONEY] NOT NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [dbo].[D_QUOTA] NOT NULL,
	[SalesPlanQuotaItemDecode_Quota] [dbo].[D_QUOTA] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
	[Action_TypeId]	[dbo].[D_ID] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepart]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeDepart_T_Depart] FOREIGN KEY([Depart_Guid])
REFERENCES [dbo].[T_Depart] ([Depart_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepart] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeDepart_T_Depart]
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepart]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeDepart_T_SalesPlanQuotaItem] FOREIGN KEY([SalesPlanQuotaItem_Guid])
REFERENCES [dbo].[T_SalesPlanQuotaItem] ([SalesPlanQuotaItem_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeDepart] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeDepart_T_SalesPlanQuotaItem]
GO



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItemDecodeCustomer](
	[SalesPlanQuotaItemDecode_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[Customer_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItemDecode_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItemDecode_Money] [dbo].[D_MONEY] NOT NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [dbo].[D_QUOTA] NOT NULL,
	[SalesPlanQuotaItemDecode_Quota] [dbo].[D_QUOTA] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
 CONSTRAINT [PK_T_SalesPlanQuotaItemDecodeCustomer] PRIMARY KEY CLUSTERED 
(
	[SalesPlanQuotaItemDecode_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItemDecodeCustomer_Archive](
	[SalesPlanQuotaItemDecode_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[Customer_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItemDecode_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItemDecode_Money] [dbo].[D_MONEY] NOT NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [dbo].[D_QUOTA] NOT NULL,
	[SalesPlanQuotaItemDecode_Quota] [dbo].[D_QUOTA] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
	[Action_TypeId]	[dbo].[D_ID] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeCustomer]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeCustomer_T_Customer] FOREIGN KEY([Customer_Guid])
REFERENCES [dbo].[T_Customer] ([Customer_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeCustomer] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeCustomer_T_Customer]
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeCustomer]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeCustomer_T_SalesPlanQuotaItem] FOREIGN KEY([SalesPlanQuotaItem_Guid])
REFERENCES [dbo].[T_SalesPlanQuotaItem] ([SalesPlanQuotaItem_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodeCustomer] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItemDecodeCustomer_T_SalesPlanQuotaItem]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItemDecodePartSubType](
	[SalesPlanQuotaItemDecode_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[PartSubType_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItemDecode_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItemDecode_Money] [dbo].[D_MONEY] NOT NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [dbo].[D_QUOTA] NOT NULL,
	[SalesPlanQuotaItemDecode_Quota] [dbo].[D_QUOTA] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
 CONSTRAINT [PK_T_SalesPlanQuotaItemDecodePartSubType] PRIMARY KEY CLUSTERED 
(
	[SalesPlanQuotaItemDecode_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[T_SalesPlanQuotaItemDecodePartSubType_Archive](
	[SalesPlanQuotaItemDecode_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItem_Guid] [dbo].[D_GUID] NOT NULL,
	[PartSubType_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuotaItemDecode_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[SalesPlanQuotaItemDecode_Money] [dbo].[D_MONEY] NOT NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [dbo].[D_QUOTA] NOT NULL,
	[SalesPlanQuotaItemDecode_Quota] [dbo].[D_QUOTA] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NOT NULL,
	[Record_UserUdpated] [dbo].[D_NAMESHORT] NOT NULL,
	[Action_TypeId]	[dbo].[D_ID] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodePartSubType]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItemDecodePartSubType_T_PartSubType] FOREIGN KEY([PartSubType_Guid])
REFERENCES [dbo].[T_PartSubType] ([PartSubType_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodePartSubType] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItemDecodePartSubType_T_PartSubType]
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodePartSubType]  WITH CHECK ADD  CONSTRAINT [FK_T_SalesPlanQuotaItemDecodePartSubType_T_SalesPlanQuotaItem] FOREIGN KEY([SalesPlanQuotaItem_Guid])
REFERENCES [dbo].[T_SalesPlanQuotaItem] ([SalesPlanQuotaItem_Guid])
GO

ALTER TABLE [dbo].[T_SalesPlanQuotaItemDecodePartSubType] CHECK CONSTRAINT [FK_T_SalesPlanQuotaItemDecodePartSubType_T_SalesPlanQuotaItem]
GO


CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeCustomer_Archive_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeCustomer_Archive]
(
	[SalesPlanQuotaItemDecode_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeCustomer_Archive_SalesPlanQuotaItem_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeCustomer_Archive]
(
	[SalesPlanQuotaItem_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeCustomer_Archive_Customer_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeCustomer_Archive]
(
	[Customer_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeCustomer_Archive_Record_Updated] ON [dbo].[T_SalesPlanQuotaItemDecodeCustomer_Archive]
(
	[Record_Updated] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeCustomer_Archive_Action_TypeId] ON [dbo].[T_SalesPlanQuotaItemDecodeCustomer_Archive]
(
	[Action_TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepart_Archive_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeDepart_Archive]
(
	[SalesPlanQuotaItemDecode_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepart_Archive_SalesPlanQuotaItem_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeDepart_Archive]
(
	[SalesPlanQuotaItem_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepart_Archive_Customer_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeDepart_Archive]
(
	[Depart_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepart_Archive_Record_Updated] ON [dbo].[T_SalesPlanQuotaItemDecodeDepart_Archive]
(
	[Record_Updated] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepart_Archive_Action_TypeId] ON [dbo].[T_SalesPlanQuotaItemDecodeDepart_Archive]
(
	[Action_TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepartTeam_Archive_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam_Archive]
(
	[SalesPlanQuotaItemDecode_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepartTeam_Archive_SalesPlanQuotaItem_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam_Archive]
(
	[SalesPlanQuotaItem_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepartTeam_Archive_Customer_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam_Archive]
(
	[DepartTeam_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepartTeam_Archive_Record_Updated] ON [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam_Archive]
(
	[Record_Updated] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeDepartTeam_Archive_Action_TypeId] ON [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam_Archive]
(
	[Action_TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodePartSubType_Archive_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodePartSubType_Archive]
(
	[SalesPlanQuotaItemDecode_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodePartSubType_Archive_SalesPlanQuotaItem_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodePartSubType_Archive]
(
	[SalesPlanQuotaItem_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodePartSubType_Archive_Customer_Guid] ON [dbo].[T_SalesPlanQuotaItemDecodePartSubType_Archive]
(
	[PartSubType_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodePartSubType_Archive_Record_Updated] ON [dbo].[T_SalesPlanQuotaItemDecodePartSubType_Archive]
(
	[Record_Updated] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodePartSubType_Archive_Action_TypeId] ON [dbo].[T_SalesPlanQuotaItemDecodePartSubType_Archive]
(
	[Action_TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер обновляет время редактирования/вставки записи
-- =============================================
CREATE TRIGGER [dbo].[TG_T_SalesPlanQuotaItemDecodeCustomer_AfterUpdate]
   ON  [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuotaItemDecodeCustomer_Archive ( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, Customer_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, Customer_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 0
	FROM inserted;

	UPDATE dbo.[T_SalesPlanQuotaItemDecodeCustomer] SET Record_Updated = sysutcdatetime(), Record_UserUdpated = ( Host_Name() + ': ' + SUSER_SNAME() )
	WHERE SalesPlanQuotaItemDecode_Guid IN ( SELECT SalesPlanQuotaItemDecode_Guid FROM inserted );
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер обновляет время редактирования/вставки записи
-- =============================================
CREATE TRIGGER [dbo].[TG_T_SalesPlanQuotaItemDecodeDepartTeam_AfterUpdate]
   ON  [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuotaItemDecodeDepartTeam_Archive ( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, DepartTeam_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, DepartTeam_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 0
	FROM inserted;

	UPDATE dbo.[T_SalesPlanQuotaItemDecodeDepartTeam] SET Record_Updated = sysutcdatetime(), Record_UserUdpated = ( Host_Name() + ': ' + SUSER_SNAME() )
	WHERE SalesPlanQuotaItemDecode_Guid IN ( SELECT SalesPlanQuotaItemDecode_Guid FROM inserted );
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер обновляет время редактирования/вставки записи
-- =============================================
CREATE TRIGGER [dbo].[TG_T_SalesPlanQuotaItemDecodeDepart_AfterUpdate]
   ON  [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuotaItemDecodeDepart_Archive ( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, Depart_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, Depart_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 0
	FROM inserted;

	UPDATE dbo.[T_SalesPlanQuotaItemDecodeDepart] SET Record_Updated = sysutcdatetime(), Record_UserUdpated = ( Host_Name() + ': ' + SUSER_SNAME() )
	WHERE SalesPlanQuotaItemDecode_Guid IN ( SELECT SalesPlanQuotaItemDecode_Guid FROM inserted );
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер обновляет время редактирования/вставки записи
-- =============================================
CREATE TRIGGER [dbo].[TG_T_SalesPlanQuotaItemDecodePartSubType_AfterUpdate]
   ON  [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuotaItemDecodePartSubType_Archive ( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, PartSubType_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, PartSubType_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 0
	FROM inserted;

	UPDATE dbo.[T_SalesPlanQuotaItemDecodePartSubType] SET Record_Updated = sysutcdatetime(), Record_UserUdpated = ( Host_Name() + ': ' + SUSER_SNAME() )
	WHERE SalesPlanQuotaItemDecode_Guid IN ( SELECT SalesPlanQuotaItemDecode_Guid FROM inserted );
END

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер добавляет записи в случае их удаления в таблицу удаленных записей
-- =============================================
CREATE TRIGGER [dbo].[TG_T_SalesPlanQuotaItemDecodeCustomer_AfterDelete]
   ON [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuotaItemDecodeCustomer_Archive ( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, Customer_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, Customer_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 2

	FROM deleted;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер добавляет записи в случае их удаления в таблицу удаленных записей
-- =============================================
CREATE TRIGGER [dbo].[TG_T_SalesPlanQuotaItemDecodeDepartTeam_AfterDelete]
   ON [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuotaItemDecodeDepartTeam_Archive ( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, DepartTeam_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, DepartTeam_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 2

	FROM deleted;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер добавляет записи в случае их удаления в таблицу удаленных записей
-- =============================================
CREATE TRIGGER [dbo].[TG_T_SalesPlanQuotaItemDecodeDepart_AfterDelete]
   ON [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuotaItemDecodeDepart_Archive ( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, Depart_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, Depart_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 2

	FROM deleted;
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер добавляет записи в случае их удаления в таблицу удаленных записей
-- =============================================
CREATE TRIGGER [dbo].[TG_T_SalesPlanQuotaItemDecodePartSubType_AfterDelete]
   ON [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_SalesPlanQuotaItemDecodePartSubType_Archive ( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, PartSubType_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, PartSubType_Guid, 
		SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
		SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 2

	FROM deleted;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SalesPlanQuotaItemDecodeCustomerView]
AS
SELECT     dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Guid, dbo.T_SalesPlanQuotaItem.SalesPlanQuota_Guid, dbo.T_SalesPlanQuotaItem.ProductOwner_Guid, 
                      dbo.T_SalesPlanQuotaItem.ProductType_Guid, dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Quantity, dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Money, 
                      dbo.T_SalesPlanQuotaItemDecodeCustomer.SalesPlanQuotaItemDecode_Guid, dbo.T_SalesPlanQuotaItemDecodeCustomer.Customer_Guid, 
                      dbo.T_SalesPlanQuotaItemDecodeCustomer.SalesPlanQuotaItemDecode_Quantity, dbo.T_SalesPlanQuotaItemDecodeCustomer.SalesPlanQuotaItemDecode_Money, 
                      dbo.T_SalesPlanQuotaItemDecodeCustomer.SalesPlanQuotaItemDecode_CalcQuota, dbo.T_SalesPlanQuotaItemDecodeCustomer.SalesPlanQuotaItemDecode_Quota, 
                      dbo.T_Customer.Customer_Id, dbo.T_Customer.Customer_Name, dbo.T_Parttype.Parttype_Id, dbo.T_Parttype.Parttype_Name, dbo.T_Owner.Owner_Id, 
                      dbo.T_Owner.Owner_Name
FROM         dbo.T_SalesPlanQuotaItem INNER JOIN
                      dbo.T_SalesPlanQuotaItemDecodeCustomer ON 
                      dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Guid = dbo.T_SalesPlanQuotaItemDecodeCustomer.SalesPlanQuotaItem_Guid INNER JOIN
                      dbo.T_Customer ON dbo.T_SalesPlanQuotaItemDecodeCustomer.Customer_Guid = dbo.T_Customer.Customer_Guid INNER JOIN
                      dbo.T_Owner ON dbo.T_SalesPlanQuotaItem.ProductOwner_Guid = dbo.T_Owner.Owner_Guid INNER JOIN
                      dbo.T_Parttype ON dbo.T_SalesPlanQuotaItem.ProductType_Guid = dbo.T_Parttype.Parttype_Guid

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SalesPlanQuotaItemDecodeDepartTeamView]
AS
SELECT     dbo.T_DepartTeam.DepartTeam_Name, dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Guid, dbo.T_SalesPlanQuotaItem.SalesPlanQuota_Guid, 
                      dbo.T_SalesPlanQuotaItem.ProductOwner_Guid, dbo.T_SalesPlanQuotaItem.ProductType_Guid, dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Money, 
                      dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Quantity, dbo.T_SalesPlanQuotaItemDecodeDepartTeam.SalesPlanQuotaItemDecode_Guid, 
                      dbo.T_SalesPlanQuotaItemDecodeDepartTeam.DepartTeam_Guid, dbo.T_SalesPlanQuotaItemDecodeDepartTeam.SalesPlanQuotaItemDecode_Quantity, 
                      dbo.T_SalesPlanQuotaItemDecodeDepartTeam.SalesPlanQuotaItemDecode_Money, 
                      dbo.T_SalesPlanQuotaItemDecodeDepartTeam.SalesPlanQuotaItemDecode_CalcQuota, 
                      dbo.T_SalesPlanQuotaItemDecodeDepartTeam.SalesPlanQuotaItemDecode_Quota, dbo.T_Parttype.Parttype_Id, dbo.T_Parttype.Parttype_Name, 
                      dbo.T_Owner.Owner_Id, dbo.T_Owner.Owner_Name
FROM         dbo.T_DepartTeam INNER JOIN
                      dbo.T_SalesPlanQuotaItemDecodeDepartTeam ON 
                      dbo.T_DepartTeam.DepartTeam_Guid = dbo.T_SalesPlanQuotaItemDecodeDepartTeam.DepartTeam_Guid INNER JOIN
                      dbo.T_SalesPlanQuotaItem ON 
                      dbo.T_SalesPlanQuotaItemDecodeDepartTeam.SalesPlanQuotaItem_Guid = dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Guid INNER JOIN
                      dbo.T_Owner ON dbo.T_SalesPlanQuotaItem.ProductOwner_Guid = dbo.T_Owner.Owner_Guid INNER JOIN
                      dbo.T_Parttype ON dbo.T_SalesPlanQuotaItem.ProductType_Guid = dbo.T_Parttype.Parttype_Guid

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SalesPlanQuotaItemDecodeDepartView]
AS
SELECT     dbo.T_Depart.Depart_Code, dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Guid, dbo.T_SalesPlanQuotaItem.SalesPlanQuota_Guid, 
                      dbo.T_SalesPlanQuotaItem.ProductOwner_Guid, dbo.T_SalesPlanQuotaItem.ProductType_Guid, dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Quantity, 
                      dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Money, dbo.T_SalesPlanQuotaItemDecodeDepart.SalesPlanQuotaItemDecode_Guid, 
                      dbo.T_SalesPlanQuotaItemDecodeDepart.Depart_Guid, dbo.T_SalesPlanQuotaItemDecodeDepart.SalesPlanQuotaItemDecode_Quantity, 
                      dbo.T_SalesPlanQuotaItemDecodeDepart.SalesPlanQuotaItemDecode_Money, dbo.T_SalesPlanQuotaItemDecodeDepart.SalesPlanQuotaItemDecode_CalcQuota, 
                      dbo.T_SalesPlanQuotaItemDecodeDepart.SalesPlanQuotaItemDecode_Quota, dbo.T_Owner.Owner_Id, dbo.T_Owner.Owner_Name, dbo.T_Parttype.Parttype_Id, 
                      dbo.T_Parttype.Parttype_Name
FROM         dbo.T_Depart INNER JOIN
                      dbo.T_SalesPlanQuotaItemDecodeDepart ON dbo.T_Depart.Depart_Guid = dbo.T_SalesPlanQuotaItemDecodeDepart.Depart_Guid INNER JOIN
                      dbo.T_SalesPlanQuotaItem ON 
                      dbo.T_SalesPlanQuotaItemDecodeDepart.SalesPlanQuotaItem_Guid = dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Guid INNER JOIN
                      dbo.T_Owner ON dbo.T_SalesPlanQuotaItem.ProductOwner_Guid = dbo.T_Owner.Owner_Guid INNER JOIN
                      dbo.T_Parttype ON dbo.T_SalesPlanQuotaItem.ProductType_Guid = dbo.T_Parttype.Parttype_Guid

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[SalesPlanQuotaItemDecodePartSubTypeView]
AS
SELECT     dbo.T_Partsubtype.Partsubtype_Id, dbo.T_Partsubtype.Partsubtype_Name, dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Guid, 
                      dbo.T_SalesPlanQuotaItem.SalesPlanQuota_Guid, dbo.T_SalesPlanQuotaItem.ProductOwner_Guid, dbo.T_SalesPlanQuotaItem.ProductType_Guid, 
                      dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Money, dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Quantity, 
                      dbo.T_SalesPlanQuotaItemDecodePartSubType.SalesPlanQuotaItemDecode_Guid, dbo.T_SalesPlanQuotaItemDecodePartSubType.PartSubType_Guid, 
                      dbo.T_SalesPlanQuotaItemDecodePartSubType.SalesPlanQuotaItemDecode_Quantity, 
                      dbo.T_SalesPlanQuotaItemDecodePartSubType.SalesPlanQuotaItemDecode_Money, 
                      dbo.T_SalesPlanQuotaItemDecodePartSubType.SalesPlanQuotaItemDecode_CalcQuota, 
                      dbo.T_SalesPlanQuotaItemDecodePartSubType.SalesPlanQuotaItemDecode_Quota, dbo.T_Parttype.Parttype_Name, dbo.T_Parttype.Parttype_Id, 
                      dbo.T_Owner.Owner_Id, dbo.T_Owner.Owner_Name
FROM         dbo.T_Partsubtype INNER JOIN
                      dbo.T_SalesPlanQuotaItemDecodePartSubType ON 
                      dbo.T_Partsubtype.Partsubtype_Guid = dbo.T_SalesPlanQuotaItemDecodePartSubType.PartSubType_Guid INNER JOIN
                      dbo.T_SalesPlanQuotaItem ON 
                      dbo.T_SalesPlanQuotaItemDecodePartSubType.SalesPlanQuotaItem_Guid = dbo.T_SalesPlanQuotaItem.SalesPlanQuotaItem_Guid INNER JOIN
                      dbo.T_Owner ON dbo.T_SalesPlanQuotaItem.ProductOwner_Guid = dbo.T_Owner.Owner_Guid INNER JOIN
                      dbo.T_Parttype ON dbo.T_SalesPlanQuotaItem.ProductType_Guid = dbo.T_Parttype.Parttype_Guid

GO


GRANT SELECT ON [dbo].[SalesPlanQuotaItemDecodeCustomerView] TO [public]
GO

GRANT SELECT ON [dbo].[SalesPlanQuotaItemDecodeDepartTeamView] TO [public]
GO

GRANT SELECT ON [dbo].[SalesPlanQuotaItemDecodeDepartView] TO [public]
GO

GRANT SELECT ON [dbo].[SalesPlanQuotaItemDecodePartSubTypeView] TO [public]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodeDepartTeamView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodeDepartTeam] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT DepartTeam_Name, SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, SalesPlanQuotaItem_Money, SalesPlanQuotaItem_Quantity, 
			SalesPlanQuotaItemDecode_Guid, DepartTeam_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, Parttype_Id, Parttype_Name, Owner_Id, Owner_Name
		FROM [dbo].[SalesPlanQuotaItemDecodeDepartTeamView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, DepartTeam_Name;

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

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodeDepartView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodeDepart] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT Depart_Code, SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, SalesPlanQuotaItemDecode_Guid, Depart_Guid, 
			SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, Owner_Id, Owner_Name, Parttype_Id, Parttype_Name
		FROM [dbo].[SalesPlanQuotaItemDecodeDepartView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, Depart_Code;

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

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodeCustomerView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodeCustomer] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, 
			SalesPlanQuotaItemDecode_Guid, Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
			Customer_Id, Customer_Name, Parttype_Id, Parttype_Name, Owner_Id, Owner_Name
		FROM [dbo].[SalesPlanQuotaItemDecodeCustomerView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, Customer_Name;

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

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodePartSubTypeView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodePartSubType] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT Partsubtype_Id, Partsubtype_Name, SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Money, SalesPlanQuotaItem_Quantity, 
			SalesPlanQuotaItemDecode_Guid, PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, Parttype_Name, Parttype_Id, Owner_Id, Owner_Name
		FROM [dbo].[SalesPlanQuotaItemDecodePartSubTypeView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, Partsubtype_Name;

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

GRANT EXECUTE ON [dbo].[usp_GetSalePlanQuotaItemDecodeDepartTeam] TO [public]
GO

GRANT EXECUTE ON [dbo].[usp_GetSalePlanQuotaItemDecodeDepart] TO [public]
GO

GRANT EXECUTE ON [dbo].[usp_GetSalePlanQuotaItemDecodeCustomer] TO [public]
GO

GRANT EXECUTE ON [dbo].[usp_GetSalePlanQuotaItemDecodePartSubType] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.T_SalesPlanQuotaItemDecodeDepartTeam
--
-- Входящие параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@ProductOwner_Guid							УИ товарной марки
--		@ProductType_Guid								УИ товарной группы
--		@DepartTeam_Guid								УИ команды
--		@SalesPlanQuotaItemDecode_Quota	доля продаж
--
--
-- Выходные параметры:
--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_AddSalePlanQuotaItemDecodeDepartTeam] 
	@SalesPlanQuota_Guid						D_GUID,
	@ProductOwner_Guid							D_GUID,
	@ProductType_Guid								D_GUID,
	@DepartTeam_Guid								D_GUID,
	@SalesPlanQuotaItemDecode_Quota	D_QUOTA,

  @SalesPlanQuotaItemDecode_Guid	D_GUID output,
  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    SET @SalesPlanQuotaItemDecode_Quota = NULL;

    -- Проверяем наличие расчёта с указанным идентификатором
    IF NOT EXISTS ( SELECT SalesPlanQuota_Guid FROM dbo.T_SalesPlanQuota WHERE SalesPlanQuota_Guid = @SalesPlanQuota_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден расчёт  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной марки с указанным идентификатором
    IF NOT EXISTS ( SELECT Owner_Guid FROM dbo.T_Owner WHERE Owner_Guid = @ProductOwner_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найдена товарная марка  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @ProductOwner_Guid  );
        RETURN @ERROR_NUM;
      END
    
    -- Проверяем наличие товарной группы с указанным идентификатором
    IF NOT EXISTS ( SELECT Parttype_Guid FROM dbo.T_Parttype WHERE Parttype_Guid = @ProductType_Guid )
      BEGIN
        SET @ERROR_NUM = 3;
        SET @ERROR_MES = 'В базе данных не найдена товарная группа  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @ProductType_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие команды с указанным идентификатором
    IF NOT EXISTS ( SELECT DepartTeam_Guid FROM dbo.T_DepartTeam WHERE DepartTeam_Guid = @DepartTeam_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найдена команда  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @DepartTeam_Guid );
        RETURN @ERROR_NUM;
      END

    DECLARE @SalesPlanQuotaItem_Guid	D_GUID;
		SELECT Top 1 @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
			AND [ProductOwner_Guid] = @ProductOwner_Guid
			AND [ProductType_Guid] = @ProductType_Guid;

		IF( @SalesPlanQuotaItem_Guid IS NULL )
			BEGIN
				SET @SalesPlanQuotaItem_Guid = NEWID();
				INSERT INTO [dbo].[T_SalesPlanQuotaItem]( SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, 
					ProductOwner_Guid, ProductType_Guid, SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money )
				VALUES( @SalesPlanQuotaItem_Guid,  @SalesPlanQuota_Guid, @ProductOwner_Guid, @ProductType_Guid, 0, 0 );
			END
		
		IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
										WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid 
											AND [DepartTeam_Guid] = @DepartTeam_Guid )
			BEGIN									
				DECLARE @NEWID D_GUID;
				SET @NEWID = NEWID();

				INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
					DepartTeam_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
					SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
					Record_Updated, Record_UserUdpated )
				VALUES( @NEWID, @SalesPlanQuotaItem_Guid, @DepartTeam_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
					sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );

				SET @SalesPlanQuotaItemDecode_Guid = @NEWID;
			END
		ELSE
			BEGIN
				SELECT @SalesPlanQuotaItemDecode_Guid = [SalesPlanQuotaItemDecode_Guid] 
				FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
				WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid 
					AND [DepartTeam_Guid] = @DepartTeam_Guid;
				
				IF( @SalesPlanQuotaItemDecode_Guid IS NOT NULL )
					UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
					WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
			END
    
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

GRANT EXECUTE ON [dbo].[usp_AddSalePlanQuotaItemDecodeDepartTeam] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.T_SalesPlanQuotaItemDecodeDepart
--
-- Входящие параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@ProductOwner_Guid							УИ товарной марки
--		@ProductType_Guid								УИ товарной группы
--		@Depart_Guid										УИ подразделения
--		@SalesPlanQuotaItemDecode_Quota	доля продаж
--
--
-- Выходные параметры:
--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_AddSalePlanQuotaItemDecodeDepart] 
	@SalesPlanQuota_Guid						D_GUID,
	@ProductOwner_Guid							D_GUID,
	@ProductType_Guid								D_GUID,
	@Depart_Guid										D_GUID,
	@SalesPlanQuotaItemDecode_Quota	D_QUOTA,

  @SalesPlanQuotaItemDecode_Guid	D_GUID output,
  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    SET @SalesPlanQuotaItemDecode_Quota = NULL;

    -- Проверяем наличие расчёта с указанным идентификатором
    IF NOT EXISTS ( SELECT SalesPlanQuota_Guid FROM dbo.T_SalesPlanQuota WHERE SalesPlanQuota_Guid = @SalesPlanQuota_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден расчёт  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной марки с указанным идентификатором
    IF NOT EXISTS ( SELECT Owner_Guid FROM dbo.T_Owner WHERE Owner_Guid = @ProductOwner_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найдена товарная марка  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @ProductOwner_Guid  );
        RETURN @ERROR_NUM;
      END
    
    -- Проверяем наличие товарной группы с указанным идентификатором
    IF NOT EXISTS ( SELECT Parttype_Guid FROM dbo.T_Parttype WHERE Parttype_Guid = @ProductType_Guid )
      BEGIN
        SET @ERROR_NUM = 3;
        SET @ERROR_MES = 'В базе данных не найдена товарная группа  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @ProductType_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие команды с указанным идентификатором
    IF NOT EXISTS ( SELECT Depart_Guid FROM dbo.T_Depart WHERE Depart_Guid = @Depart_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найдено подразделение с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @Depart_Guid );
        RETURN @ERROR_NUM;
      END

    DECLARE @SalesPlanQuotaItem_Guid	D_GUID;
		SELECT Top 1 @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
			AND [ProductOwner_Guid] = @ProductOwner_Guid
			AND [ProductType_Guid] = @ProductType_Guid;

		IF( @SalesPlanQuotaItem_Guid IS NULL )
			BEGIN
				SET @SalesPlanQuotaItem_Guid = NEWID();
				INSERT INTO [dbo].[T_SalesPlanQuotaItem]( SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, 
					ProductOwner_Guid, ProductType_Guid, SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money )
				VALUES( @SalesPlanQuotaItem_Guid,  @SalesPlanQuota_Guid, @ProductOwner_Guid, @ProductType_Guid, 0, 0 );
			END
		
		IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
										WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid 
											AND [Depart_Guid] = @Depart_Guid )
			BEGIN									
				DECLARE @NEWID D_GUID;
				SET @NEWID = NEWID();

				INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeDepart]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
					Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
					SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
					Record_Updated, Record_UserUdpated )
				VALUES( @NEWID, @SalesPlanQuotaItem_Guid, @Depart_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
					sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );

				SET @SalesPlanQuotaItemDecode_Guid = @NEWID;
			END
		ELSE
			BEGIN
				SELECT @SalesPlanQuotaItemDecode_Guid = [SalesPlanQuotaItemDecode_Guid] 
				FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
				WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid 
					AND [Depart_Guid] = @Depart_Guid;
				
				IF( @SalesPlanQuotaItemDecode_Guid IS NOT NULL )
					UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepart] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
					WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
			END
    
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

GRANT EXECUTE ON [dbo].[usp_AddSalePlanQuotaItemDecodeDepart] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.T_SalesPlanQuotaItemDecodeCustomer
--
-- Входящие параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@ProductOwner_Guid							УИ товарной марки
--		@ProductType_Guid								УИ товарной группы
--		@Customer_Guid									УИ клиента
--		@SalesPlanQuotaItemDecode_Quota	доля продаж
--
--
-- Выходные параметры:
--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_AddSalePlanQuotaItemDecodeCustomer] 
	@SalesPlanQuota_Guid						D_GUID,
	@ProductOwner_Guid							D_GUID,
	@ProductType_Guid								D_GUID,
	@Customer_Guid									D_GUID,
	@SalesPlanQuotaItemDecode_Quota	D_QUOTA,

  @SalesPlanQuotaItemDecode_Guid	D_GUID output,
  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    SET @SalesPlanQuotaItemDecode_Quota = NULL;

    -- Проверяем наличие расчёта с указанным идентификатором
    IF NOT EXISTS ( SELECT SalesPlanQuota_Guid FROM dbo.T_SalesPlanQuota WHERE SalesPlanQuota_Guid = @SalesPlanQuota_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден расчёт  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной марки с указанным идентификатором
    IF NOT EXISTS ( SELECT Owner_Guid FROM dbo.T_Owner WHERE Owner_Guid = @ProductOwner_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найдена товарная марка  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @ProductOwner_Guid  );
        RETURN @ERROR_NUM;
      END
    
    -- Проверяем наличие товарной группы с указанным идентификатором
    IF NOT EXISTS ( SELECT Parttype_Guid FROM dbo.T_Parttype WHERE Parttype_Guid = @ProductType_Guid )
      BEGIN
        SET @ERROR_NUM = 3;
        SET @ERROR_MES = 'В базе данных не найдена товарная группа  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @ProductType_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие клиента с указанным идентификатором
    IF NOT EXISTS ( SELECT Customer_Guid FROM dbo.T_Customer WHERE Customer_Guid = @Customer_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найден клиент с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @Customer_Guid );
        RETURN @ERROR_NUM;
      END

    DECLARE @SalesPlanQuotaItem_Guid	D_GUID;
		SELECT Top 1 @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
			AND [ProductOwner_Guid] = @ProductOwner_Guid
			AND [ProductType_Guid] = @ProductType_Guid;

		IF( @SalesPlanQuotaItem_Guid IS NULL )
			BEGIN
				SET @SalesPlanQuotaItem_Guid = NEWID();
				INSERT INTO [dbo].[T_SalesPlanQuotaItem]( SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, 
					ProductOwner_Guid, ProductType_Guid, SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money )
				VALUES( @SalesPlanQuotaItem_Guid,  @SalesPlanQuota_Guid, @ProductOwner_Guid, @ProductType_Guid, 0, 0 );
			END
		
		IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
										WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid 
											AND [Customer_Guid] = @Customer_Guid )
			BEGIN									
				DECLARE @NEWID D_GUID;
				SET @NEWID = NEWID();

				INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeCustomer]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
					Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
					SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
					Record_Updated, Record_UserUdpated )
				VALUES( @NEWID, @SalesPlanQuotaItem_Guid, @Customer_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
					sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );

				SET @SalesPlanQuotaItemDecode_Guid = @NEWID;
			END
		ELSE
			BEGIN
				SELECT @SalesPlanQuotaItemDecode_Guid = [SalesPlanQuotaItemDecode_Guid] 
				FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
				WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid 
					AND [Customer_Guid] = @Customer_Guid;
				
				IF( @SalesPlanQuotaItemDecode_Guid IS NOT NULL )
					UPDATE [dbo].[T_SalesPlanQuotaItemDecodeCustomer] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
					WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
			END
    
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

GRANT EXECUTE ON [dbo].[usp_AddSalePlanQuotaItemDecodeCustomer] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.T_SalesPlanQuotaItemDecodePartSubType
--
-- Входящие параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@ProductOwner_Guid							УИ товарной марки
--		@ProductType_Guid								УИ товарной группы
--		@PartSubType_Guid								УИ товарной подгруппы
--		@SalesPlanQuotaItemDecode_Quota	доля продаж
--
--
-- Выходные параметры:
--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_AddSalePlanQuotaItemDecodePartSubType] 
	@SalesPlanQuota_Guid						D_GUID,
	@ProductOwner_Guid							D_GUID,
	@ProductType_Guid								D_GUID,
	@PartSubType_Guid								D_GUID,
	@SalesPlanQuotaItemDecode_Quota	D_QUOTA,

  @SalesPlanQuotaItemDecode_Guid	D_GUID output,
  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    SET @SalesPlanQuotaItemDecode_Quota = NULL;

    -- Проверяем наличие расчёта с указанным идентификатором
    IF NOT EXISTS ( SELECT SalesPlanQuota_Guid FROM dbo.T_SalesPlanQuota WHERE SalesPlanQuota_Guid = @SalesPlanQuota_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден расчёт  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной марки с указанным идентификатором
    IF NOT EXISTS ( SELECT Owner_Guid FROM dbo.T_Owner WHERE Owner_Guid = @ProductOwner_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найдена товарная марка  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @ProductOwner_Guid  );
        RETURN @ERROR_NUM;
      END
    
    -- Проверяем наличие товарной группы с указанным идентификатором
    IF NOT EXISTS ( SELECT Parttype_Guid FROM dbo.T_Parttype WHERE Parttype_Guid = @ProductType_Guid )
      BEGIN
        SET @ERROR_NUM = 3;
        SET @ERROR_MES = 'В базе данных не найдена товарная группа  с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @ProductType_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие подгруппы с указанным идентификатором
    IF NOT EXISTS ( SELECT PartSubType_Guid FROM dbo.T_PartSubType WHERE PartSubType_Guid = @PartSubType_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найдена подгруппа с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @PartSubType_Guid );
        RETURN @ERROR_NUM;
      END

    DECLARE @SalesPlanQuotaItem_Guid	D_GUID;
		SELECT Top 1 @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
			AND [ProductOwner_Guid] = @ProductOwner_Guid
			AND [ProductType_Guid] = @ProductType_Guid;

		IF( @SalesPlanQuotaItem_Guid IS NULL )
			BEGIN
				SET @SalesPlanQuotaItem_Guid = NEWID();
				INSERT INTO [dbo].[T_SalesPlanQuotaItem]( SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, 
					ProductOwner_Guid, ProductType_Guid, SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money )
				VALUES( @SalesPlanQuotaItem_Guid,  @SalesPlanQuota_Guid, @ProductOwner_Guid, @ProductType_Guid, 0, 0 );
			END
		
		IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
										WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid 
											AND [PartSubType_Guid] = @PartSubType_Guid )
			BEGIN									
				DECLARE @NEWID D_GUID;
				SET @NEWID = NEWID();

				INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodePartSubType]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
					PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
					SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
					Record_Updated, Record_UserUdpated )
				VALUES( @NEWID, @SalesPlanQuotaItem_Guid, @PartSubType_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
					sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );

				SET @SalesPlanQuotaItemDecode_Guid = @NEWID;
			END
		ELSE
			BEGIN
				SELECT @SalesPlanQuotaItemDecode_Guid = [SalesPlanQuotaItemDecode_Guid] 
				FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
				WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid 
					AND [PartSubType_Guid] = @PartSubType_Guid;
				
				IF( @SalesPlanQuotaItemDecode_Guid IS NOT NULL )
					UPDATE [dbo].[T_SalesPlanQuotaItemDecodePartSubType] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
					WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
			END
    
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

GRANT EXECUTE ON [dbo].[usp_AddSalePlanQuotaItemDecodePartSubType] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- редактирует значение доли продаж в таблице dbo.T_SalesPlanQuotaItemDecodeCustomer
--
-- Входящие параметры:

--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--		@SalesPlanQuotaItemDecode_Quota	доля продаж
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_EditSalePlanQuotaItemDecodeCustomer] 
  @SalesPlanQuotaItemDecode_Guid	D_GUID,
	@SalesPlanQuotaItemDecode_Quota	D_QUOTA,

  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие расшифровки с указанным идентификатором
    IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
										WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена расшифровка с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
        RETURN @ERROR_NUM;
      END

		UPDATE [dbo].[T_SalesPlanQuotaItemDecodeCustomer] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
		WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
    
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

GRANT EXECUTE ON [dbo].[usp_EditSalePlanQuotaItemDecodeCustomer] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- редактирует значение доли продаж в таблице dbo.T_SalesPlanQuotaItemDecodePartSubType
--
-- Входящие параметры:

--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--		@SalesPlanQuotaItemDecode_Quota	доля продаж
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_EditSalePlanQuotaItemDecodePartSubType] 
  @SalesPlanQuotaItemDecode_Guid	D_GUID,
	@SalesPlanQuotaItemDecode_Quota	D_QUOTA,

  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие расшифровки с указанным идентификатором
    IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
										WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена расшифровка с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
        RETURN @ERROR_NUM;
      END

		UPDATE [dbo].[T_SalesPlanQuotaItemDecodePartSubType] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
		WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
    
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

GRANT EXECUTE ON [dbo].[usp_EditSalePlanQuotaItemDecodePartSubType] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- редактирует значение доли продаж в таблице dbo.T_SalesPlanQuotaItemDecodeDepartTeam
--
-- Входящие параметры:

--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--		@SalesPlanQuotaItemDecode_Quota	доля продаж
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_EditSalePlanQuotaItemDecodeDepartTeam] 
  @SalesPlanQuotaItemDecode_Guid	D_GUID,
	@SalesPlanQuotaItemDecode_Quota	D_QUOTA,

  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие расшифровки с указанным идентификатором
    IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
										WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена расшифровка с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
        RETURN @ERROR_NUM;
      END

		UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
		WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
    
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

GRANT EXECUTE ON [dbo].[usp_EditSalePlanQuotaItemDecodeDepartTeam] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- редактирует значение доли продаж в таблице dbo.T_SalesPlanQuotaItemDecodeDepart
--
-- Входящие параметры:

--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--		@SalesPlanQuotaItemDecode_Quota	доля продаж
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_EditSalePlanQuotaItemDecodeDepart] 
  @SalesPlanQuotaItemDecode_Guid	D_GUID,
	@SalesPlanQuotaItemDecode_Quota	D_QUOTA,

  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие расшифровки с указанным идентификатором
    IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
										WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена расшифровка с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
        RETURN @ERROR_NUM;
      END

		UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepart] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
		WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
    
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

GRANT EXECUTE ON [dbo].[usp_EditSalePlanQuotaItemDecodeDepart] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Удаляет значение доли продаж в таблице dbo.T_SalesPlanQuotaItemDecodeCustomer
--
-- Входящие параметры:

--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_DeleteSalePlanQuotaItemDecodeCustomer] 
  @SalesPlanQuotaItemDecode_Guid	D_GUID,

  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие расшифровки с указанным идентификатором
    IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
										WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена расшифровка с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
        RETURN @ERROR_NUM;
      END

		DECLARE @SalesPlanQuotaItemDecode_CalcQuota	D_QUOTA;
		
		SELECT @SalesPlanQuotaItemDecode_CalcQuota = [SalesPlanQuotaItemDecode_CalcQuota]
		FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer]
		WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;

		IF( @SalesPlanQuotaItemDecode_CalcQuota <> 0 )
			BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'Нельзя удалить строку с  расчётной долей продаж не равной нулю.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
			END
		ELSE	
			DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;

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

GRANT EXECUTE ON [dbo].[usp_DeleteSalePlanQuotaItemDecodeCustomer] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Удаляет значение доли продаж в таблице dbo.T_SalesPlanQuotaItemDecodePartSubType
--
-- Входящие параметры:

--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_DeleteSalePlanQuotaItemDecodePartSubType] 
  @SalesPlanQuotaItemDecode_Guid	D_GUID,

  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие расшифровки с указанным идентификатором
    IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
										WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена расшифровка с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
        RETURN @ERROR_NUM;
      END

		DECLARE @SalesPlanQuotaItemDecode_CalcQuota	D_QUOTA;
		
		SELECT @SalesPlanQuotaItemDecode_CalcQuota = [SalesPlanQuotaItemDecode_CalcQuota]
		FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType]
		WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;

		IF( @SalesPlanQuotaItemDecode_CalcQuota <> 0 )
			BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'Нельзя удалить строку с  расчётной долей продаж не равной нулю.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
			END
		ELSE	
			DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
    
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

GRANT EXECUTE ON [dbo].[usp_DeleteSalePlanQuotaItemDecodePartSubType] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Удаляет значение доли продаж в таблице dbo.T_SalesPlanQuotaItemDecodeDepartTeam
--
-- Входящие параметры:

--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_DeleteSalePlanQuotaItemDecodeDepartTeam] 
  @SalesPlanQuotaItemDecode_Guid	D_GUID,

  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие расшифровки с указанным идентификатором
    IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
										WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена расшифровка с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
        RETURN @ERROR_NUM;
      END

		DECLARE @SalesPlanQuotaItemDecode_CalcQuota	D_QUOTA;
		
		SELECT @SalesPlanQuotaItemDecode_CalcQuota = [SalesPlanQuotaItemDecode_CalcQuota]
		FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]
		WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;

		IF( @SalesPlanQuotaItemDecode_CalcQuota <> 0 )
			BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'Нельзя удалить строку с  расчётной долей продаж не равной нулю.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
			END
		ELSE	
			DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;
    
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

GRANT EXECUTE ON [dbo].[usp_DeleteSalePlanQuotaItemDecodeDepartTeam] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Удаляет значение доли продаж в таблице dbo.T_SalesPlanQuotaItemDecodeDepart
--
-- Входящие параметры:

--  @SalesPlanQuotaItemDecode_Guid		УИ записи
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_DeleteSalePlanQuotaItemDecodeDepart] 
  @SalesPlanQuotaItemDecode_Guid	D_GUID,

  @ERROR_NUM											int output,
  @ERROR_MES											nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие расшифровки с указанным идентификатором
    IF NOT EXISTS ( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
										WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена расшифровка с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
        RETURN @ERROR_NUM;
      END

		DECLARE @SalesPlanQuotaItemDecode_CalcQuota	D_QUOTA;
		
		SELECT @SalesPlanQuotaItemDecode_CalcQuota = [SalesPlanQuotaItemDecode_CalcQuota]
		FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart]
		WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;

		IF( @SalesPlanQuotaItemDecode_CalcQuota <> 0 )
			BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'Нельзя удалить строку с  расчётной долей продаж не равной нулю.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuotaItemDecode_Guid );
			END
		ELSE	
			DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] WHERE [SalesPlanQuotaItemDecode_Guid] = @SalesPlanQuotaItemDecode_Guid;

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

GRANT EXECUTE ON [dbo].[usp_DeleteSalePlanQuotaItemDecodeDepart] TO [public]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetSalePlanQuotaItemDecodePartSubType]    Script Date: 17.10.2013 11:57:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodePartSubTypeView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodePartSubType] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT Partsubtype_Id AS ObjectDecode_Id, Partsubtype_Name AS ObjectDecode_Name, SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Money, SalesPlanQuotaItem_Quantity, 
			SalesPlanQuotaItemDecode_Guid, PartSubType_Guid AS ObjectDecode_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, Parttype_Name, Parttype_Id, Owner_Id, Owner_Name
		FROM [dbo].[SalesPlanQuotaItemDecodePartSubTypeView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, Partsubtype_Name;

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

/****** Object:  StoredProcedure [dbo].[usp_GetSalePlanQuotaItemDecodeCustomer]    Script Date: 17.10.2013 11:56:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodeCustomerView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodeCustomer] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, 
			SalesPlanQuotaItemDecode_Guid, Customer_Guid AS ObjectDecode_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
			Customer_Id AS ObjectDecode_Id, Customer_Name AS ObjectDecode_Name, Parttype_Id, Parttype_Name, Owner_Id, Owner_Name
		FROM [dbo].[SalesPlanQuotaItemDecodeCustomerView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, Customer_Name;

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

/****** Object:  StoredProcedure [dbo].[usp_GetSalePlanQuotaItemDecodeDepart]    Script Date: 17.10.2013 11:35:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodeDepartView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodeDepart] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT Depart_Code AS ObjectDecode_Name, SalesPlanQuotaItem_Guid AS ObjectDecode_Id, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, SalesPlanQuotaItemDecode_Guid, Depart_Guid, 
			SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, Owner_Id, Owner_Name, Parttype_Id, Parttype_Name, cast( 0 as int ) AS ObjectDecode_Id
		FROM [dbo].[SalesPlanQuotaItemDecodeDepartView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, Depart_Code;

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

/****** Object:  StoredProcedure [dbo].[usp_GetSalePlanQuotaItemDecodeDepartTeam]    Script Date: 17.10.2013 11:55:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodeDepartTeamView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodeDepartTeam] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT DepartTeam_Name AS ObjectDecode_Name, SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, SalesPlanQuotaItem_Money, SalesPlanQuotaItem_Quantity, 
			SalesPlanQuotaItemDecode_Guid, DepartTeam_Guid AS ObjectDecode_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, Parttype_Id, Parttype_Name, Owner_Id, Owner_Name, cast( 0 as int ) AS ObjectDecode_Id
		FROM [dbo].[SalesPlanQuotaItemDecodeDepartTeamView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, DepartTeam_Name;

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

/****** Object:  StoredProcedure [dbo].[usp_DeleteSalesPlanQuota]    Script Date: 17.10.2013 17:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Удаляет в базе данных описание расчёта коэффициента продаж
--
-- Входные параметры:
--
--		@SalesPlanQuota_Guid				УИ записи
--
--		Выходные параметры
--
--		@ERROR_NUM									номер ошибки
--		@ERROR_MES									текст ошибки
--
-- Результат:
--    0 - успешное завершение
--		<>0 - ошибка
--
-- Возвращает:
--

ALTER PROCEDURE [dbo].[usp_DeleteSalesPlanQuota]
  @SalesPlanQuota_Guid				D_GUID,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output
AS

BEGIN

  BEGIN TRY
    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    
    -- Проверяем наличие расчёта с указанным номером
    IF NOT EXISTS ( SELECT [SalesPlanQuota_Guid] FROM [dbo].[T_SalesPlanQuota] WHERE SalesPlanQuota_Guid = @SalesPlanQuota_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'Расчёт с указанным идентификатором не найден.' + Char(13) + 
          '№ ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END
		
		DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer]
		WHERE [SalesPlanQuotaItem_Guid] IN ( SELECT [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid );

		DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart]
		WHERE [SalesPlanQuotaItem_Guid] IN ( SELECT [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid );

		DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]
		WHERE [SalesPlanQuotaItem_Guid] IN ( SELECT [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid );

		DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType]
		WHERE [SalesPlanQuotaItem_Guid] IN ( SELECT [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid );

		DELETE FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid;

		DELETE FROM [dbo].[T_SalesPlanQuota]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid;
				   
 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = 'Ошибка удаления записи в базе данных. ' + ERROR_MESSAGE();
    
		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';
		
	RETURN @ERROR_NUM;
END

GO


CREATE TYPE [dbo].[udt_SalesPlanQuotaItemDecode] AS TABLE(
	[SalesPlanQuotaItemDecode_Guid] [uniqueidentifier]  NULL,
	[SalesPlanQuotaItem_Guid] [uniqueidentifier]  NULL,
	[SalesPlanQuota_Guid] [uniqueidentifier]  NULL,
	[ProductOwner_Guid] [uniqueidentifier]  NULL,
	[ProductType_Guid] [uniqueidentifier]  NULL,
	[ObjectType_Id] [int] NULL,
	[ObjectDecode_Guid] [uniqueidentifier]  NULL,
	[SalesPlanQuotaItemDecode_Quantity] [float] NULL,
	[SalesPlanQuotaItemDecode_Money] [money] NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] numeric(18,5) NULL,
	[SalesPlanQuotaItemDecode_Quota] numeric(18,5) NULL,
	[ActionType_Id] int NULL
)
GO

GRANT CONTROL ON TYPE::[dbo].[udt_SalesPlanQuotaItemDecode] TO [public]
GO
use [ERP_Mercury]
GO
GRANT REFERENCES ON TYPE::[dbo].[udt_SalesPlanQuotaItemDecode] TO [public]
GO
use [ERP_Mercury]
GO
GRANT TAKE OWNERSHIP ON TYPE::[dbo].[udt_SalesPlanQuotaItemDecode] TO [public]
GO
use [ERP_Mercury]
GO
GRANT VIEW DEFINITION ON TYPE::[dbo].[udt_SalesPlanQuotaItemDecode] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- редактирует информацию о приложении к расчёту
--
-- Входящие параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@t_SalesPlanQuotaItemDecode			приложение к расчёту
--
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_AddSalePlanQuotaItemDecode] 
	@SalesPlanQuota_Guid				D_GUID,
  @t_SalesPlanQuotaItemDecode	dbo.udt_SalesPlanQuotaItemDecode READONLY,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		DECLARE @SalesPlanQuotaItemDecode_Guid	uniqueidentifier;
		DECLARE @SalesPlanQuotaItem_Guid				uniqueidentifier;
		DECLARE @ProductOwner_Guid							uniqueidentifier;
		DECLARE @ProductType_Guid								uniqueidentifier;
		DECLARE @ObjectType_Id									int;
		DECLARE @ObjectDecode_Guid							uniqueidentifier;
		DECLARE @SalesPlanQuotaItemDecode_Quantity	float;
		DECLARE @SalesPlanQuotaItemDecode_Money			money;
		DECLARE @SalesPlanQuotaItemDecode_CalcQuota	numeric(18,5);
		DECLARE @SalesPlanQuotaItemDecode_Quota			numeric(18,5);
		DECLARE @ActionType_Id											int;

		DECLARE @ObjectTypeDepartTeamId	int = 0;
		DECLARE @ObjectTypeDepartId	int = 1;
		DECLARE @ObjectTypeCustomerId	int = 2;
		DECLARE @ObjectTypePartSubTypeId	int = 3;

		DECLARE @ActionTypeAdd_Id	int = 0;
		DECLARE @ActionTypeEdit_Id	int = 1;
		DECLARE @ActionTypeDelete_Id	int = 2;

		DECLARE crUpdate CURSOR FOR SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid,
			SalesPlanQuota_Guid,	ProductOwner_Guid,	ProductType_Guid, ObjectType_Id, ObjectDecode_Guid,
			SalesPlanQuotaItemDecode_Quantity,	SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota,	SalesPlanQuotaItemDecode_Quota,
			ActionType_Id
		FROM @t_SalesPlanQuotaItemDecode;
		OPEN crUpdate;
		fetch next from crUpdate into @SalesPlanQuotaItemDecode_Guid, @SalesPlanQuotaItem_Guid,
			@SalesPlanQuota_Guid,	@ProductOwner_Guid,	@ProductType_Guid, @ObjectType_Id, @ObjectDecode_Guid,
			@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
			@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota,
			@ActionType_Id;
		while @@fetch_status = 0
			begin
				
				IF( @ObjectType_Id = @ObjectTypeCustomerId )
					BEGIN
						-- Клиент
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [Customer_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeCustomer]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeCustomer] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [Customer_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypeDepartTeamId )
					BEGIN
						-- Команда
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [DepartTeam_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								DepartTeam_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [DepartTeam_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypeDepartId )
					BEGIN
						-- Подразделение
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [Depart_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeDepart]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepart] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [Depart_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypePartSubTypeId )
					BEGIN
						-- Подгруппа
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [PartSubType_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodePartSubType]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodePartSubType] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [PartSubType_Guid] = @ObjectDecode_Guid;
					END

				fetch next from crUpdate into @SalesPlanQuotaItemDecode_Guid, @SalesPlanQuotaItem_Guid,
					@SalesPlanQuota_Guid,	@ProductOwner_Guid,	@ProductType_Guid, @ObjectType_Id, @ObjectDecode_Guid,
					@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
					@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota,
					@ActionType_Id;
			end -- while @@fetch_status = 0

		close crUpdate;
		deallocate crUpdate;

		-- Удаление
    
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

GRANT EXECUTE ON [dbo].[usp_AddSalePlanQuotaItemDecode] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--	 Возвращает пустой xml документ для условий расчёта
--
-- Входные параметры:
--		@NodesCount			количество узлов
--
-- Выходные параметры:
--
--		@ERROR_NUM			номер ошибки
--		@ERROR_MES			текст ошибки
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetSalePlanQuotaConditionBlank] 
	@NodesCount			int,
  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
	
	DECLARE @doc xml ( DOCUMENT SalesPlanQuotaConditionSchema );
	DECLARE @NodeString nvarchar(max);
	SET @NodeString = N'<SalesPlanQuotaConditionItem ItemTypeId="0" ItemGuid="00000000-0000-0000-0000-000000000000" ItemName=""/>';
	IF( @NodesCount > 1 )
		SET @NodeString = Replicate( @NodeString, @NodesCount );

  SET @doc = ( N'<?xml version="1.0" encoding="UTF-16"?>
    <SalesPlanQuotaCondition xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">' + 
		@NodeString + 
		N'</SalesPlanQuotaCondition>' );

	SELECT @doc; 

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
GRANT EXECUTE ON [dbo].[usp_GetSalePlanQuotaConditionBlank] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- редактирует информацию о приложении к расчёту
--
-- Входящие параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@t_SalesPlanQuotaItemDecode			приложение к расчёту
--
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_AddSalePlanQuotaItemDecode] 
	@SalesPlanQuota_Guid				D_GUID,
  @t_SalesPlanQuotaItemDecode	dbo.udt_SalesPlanQuotaItemDecode READONLY,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		DECLARE @SalesPlanQuotaItemDecode_Guid	uniqueidentifier;
		DECLARE @SalesPlanQuotaItem_Guid				uniqueidentifier;
		DECLARE @ProductOwner_Guid							uniqueidentifier;
		DECLARE @ProductType_Guid								uniqueidentifier;
		DECLARE @ObjectType_Id									int;
		DECLARE @ObjectDecode_Guid							uniqueidentifier;
		DECLARE @SalesPlanQuotaItemDecode_Quantity	float;
		DECLARE @SalesPlanQuotaItemDecode_Money			money;
		DECLARE @SalesPlanQuotaItemDecode_CalcQuota	numeric(18,5);
		DECLARE @SalesPlanQuotaItemDecode_Quota			numeric(18,5);
		DECLARE @ActionType_Id											int;

		DECLARE @ObjectTypeDepartTeamId	int = 0;
		DECLARE @ObjectTypeDepartId	int = 1;
		DECLARE @ObjectTypeCustomerId	int = 2;
		DECLARE @ObjectTypePartSubTypeId	int = 3;

		DECLARE @ActionTypeAdd_Id	int = 0;
		DECLARE @ActionTypeEdit_Id	int = 1;
		DECLARE @ActionTypeDelete_Id	int = 2;

		DECLARE crUpdate CURSOR FOR SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid,
			ProductOwner_Guid,	ProductType_Guid, ObjectType_Id, ObjectDecode_Guid,
			SalesPlanQuotaItemDecode_Quantity,	SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota,	SalesPlanQuotaItemDecode_Quota,
			ActionType_Id
		FROM @t_SalesPlanQuotaItemDecode;
		OPEN crUpdate;
		fetch next from crUpdate into @SalesPlanQuotaItemDecode_Guid, @SalesPlanQuotaItem_Guid,
			@ProductOwner_Guid,	@ProductType_Guid, @ObjectType_Id, @ObjectDecode_Guid,
			@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
			@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota,
			@ActionType_Id;
		while @@fetch_status = 0
			begin
				IF( ( @SalesPlanQuotaItem_Guid IS NULL ) 
						--OR (@SalesPlanQuotaItem_Guid  = '00000000-0000-0000-0000-000000000000') )
				OR ( @SalesPlanQuotaItem_Guid = ( SELECT [dbo].[GetNullGuid]() ) ) )	
					BEGIN
						SELECT Top 1 @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem]
						WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
							AND [ProductOwner_Guid] = @ProductOwner_Guid
							AND [ProductType_Guid] = @ProductType_Guid;

						IF( ( @SalesPlanQuotaItem_Guid IS NULL ) 
							--OR (@SalesPlanQuotaItem_Guid  = '00000000-0000-0000-0000-000000000000') )
							OR ( @SalesPlanQuotaItem_Guid = ( SELECT [dbo].[GetNullGuid]() ) ) )	
							BEGIN
								SET @SalesPlanQuotaItem_Guid = NEWID();
								INSERT INTO [dbo].[T_SalesPlanQuotaItem]( SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, 
									ProductOwner_Guid, ProductType_Guid, SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money )
								VALUES( @SalesPlanQuotaItem_Guid,  @SalesPlanQuota_Guid, @ProductOwner_Guid, @ProductType_Guid, 0, 0 );
							END
					END

				IF( @ObjectType_Id = @ObjectTypeCustomerId )
					BEGIN
						-- Клиент
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [Customer_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeCustomer]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeCustomer] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [Customer_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypeDepartTeamId )
					BEGIN
						-- Команда
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [DepartTeam_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								DepartTeam_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [DepartTeam_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypeDepartId )
					BEGIN
						-- Подразделение
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [Depart_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeDepart]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepart] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [Depart_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypePartSubTypeId )
					BEGIN
						-- Подгруппа
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [PartSubType_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodePartSubType]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 0, 0, 0, @SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodePartSubType] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [PartSubType_Guid] = @ObjectDecode_Guid;
					END

				fetch next from crUpdate into @SalesPlanQuotaItemDecode_Guid, @SalesPlanQuotaItem_Guid,
					@ProductOwner_Guid,	@ProductType_Guid, @ObjectType_Id, @ObjectDecode_Guid,
					@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
					@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota,
					@ActionType_Id;
			end -- while @@fetch_status = 0

		close crUpdate;
		deallocate crUpdate;

		-- Удаление
    
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
	

CREATE FUNCTION [dbo].[GetDepartGuidByDepartCode] ( @Depart_Code D_DEPARTCODE )
RETURNS D_GUID
WITH EXECUTE AS caller
AS
BEGIN
  
	DECLARE @Depart_Guid D_GUID;

	SELECT Top 1 @Depart_Guid = [Depart_Guid] FROM [dbo].[T_Depart]
	WHERE [Depart_Code] = @Depart_Code;

	RETURN @Depart_Guid;

end
GO

GRANT EXECUTE ON [dbo].[GetDepartGuidByDepartCode] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	

CREATE FUNCTION [dbo].[GetDepartTeamGuidByDepartCode] ( @Depart_Code D_DEPARTCODE )
RETURNS D_GUID
WITH EXECUTE AS caller
AS
BEGIN
  
	DECLARE @DepartTeam_Guid D_GUID;

	SELECT Top 1 @DepartTeam_Guid = [DepartTeam_Guid] FROM [dbo].[T_Depart]
	WHERE [Depart_Code] = @Depart_Code;

	RETURN @DepartTeam_Guid;

end
GO

GRANT EXECUTE ON [dbo].[GetDepartTeamGuidByDepartCode] TO [public]
GO


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

CREATE PROCEDURE [dbo].[usp_CalcSalePlanQuotaItemDecode] 
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
		WITH SHIP_BY_DEPART ( Owner_Guid, PartType_Guid, DepartTeam_Guid, SHIP_QTY, SHIP_QTY_GROUP, SHIP_MONEY, SHIP_MONEY_GROUP )
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
			SHIP_BY_DEPART.DepartTeam_Guid, cast( 0 as int), [dbo].[T_DepartTeam].DepartTeam_Name, 
			SHIP_QTY, SHIP_MONEY
		 FROM SHIP_BY_DEPART INNER JOIN [dbo].[T_DepartTeam] ON SHIP_BY_DEPART.DepartTeam_Guid = [dbo].[T_DepartTeam].DepartTeam_Guid;	 

		-- Клиент
		WITH SHIP_BY_DEPART ( Owner_Guid, PartType_Guid, Customer_Guid, SHIP_QTY, SHIP_QTY_GROUP, SHIP_MONEY, SHIP_MONEY_GROUP )
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
			SHIP_BY_DEPART.Customer_Guid, [dbo].[T_Customer].Customer_Id, [dbo].[T_Customer].Customer_Name,
			SHIP_QTY, SHIP_MONEY
		 FROM SHIP_BY_DEPART INNER JOIN [dbo].[T_Customer] ON SHIP_BY_DEPART.Customer_Guid = [dbo].[T_Customer].Customer_Guid;	 


		-- Подгруппа
		WITH SHIP_BY_DEPART ( Owner_Guid, PartType_Guid, PartSubType_Guid, SHIP_QTY, SHIP_QTY_GROUP, SHIP_MONEY, SHIP_MONEY_GROUP )
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
			SHIP_BY_DEPART.PartSubType_Guid, [dbo].[T_PartSubType].Partsubtype_Id, [dbo].[T_PartSubType].Partsubtype_Name,
			SHIP_QTY, SHIP_MONEY
		 FROM SHIP_BY_DEPART INNER JOIN [dbo].[T_PartSubType] ON SHIP_BY_DEPART.PartSubType_Guid = [dbo].[T_PartSubType].PartSubType_Guid;	 

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
		 --WHERE ( ( ProductOwner_Guid IN ( SELECT [Item_Guid] FROM @t_ProductTradeMarkList ) OR ( ProductType_Guid IN ( SELECT [Item_Guid] FROM @t_ProductTypeList ) )  )
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
GO

GRANT EXECUTE ON [dbo].[usp_CalcSalePlanQuotaItemDecode] TO [public]
GO

/****** Object:  StoredProcedure [dbo].[usp_CalcSalePlanQuotaItemDecode]    Script Date: 22.10.2013 11:14:00 ******/
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
GO


DROP PROCEDURE [dbo].[usp_AddSalePlanQuotaItemDecode] 
GO

DROP TYPE [dbo].[udt_SalesPlanQuotaItemDecode]
GO

CREATE TYPE [dbo].[udt_SalesPlanQuotaItemDecode] AS TABLE(
	[SalesPlanQuotaItemDecode_Guid] [uniqueidentifier] NULL,
	[SalesPlanQuotaItem_Guid] [uniqueidentifier] NULL,
	[SalesPlanQuota_Guid] [uniqueidentifier] NULL,
	[ProductOwner_Guid] [uniqueidentifier] NULL,
	[ProductType_Guid] [uniqueidentifier] NULL,
	[SalesPlanQuotaItem_Quantity]	[float] NULL, 
	[SalesPlanQuotaItem_Money] [money] NULL,
	[ObjectType_Id] [int] NULL,
	[ObjectDecode_Guid] [uniqueidentifier] NULL,
	[SalesPlanQuotaItemDecode_Quantity] [float] NULL,
	[SalesPlanQuotaItemDecode_Money] [money] NULL,
	[SalesPlanQuotaItemDecode_CalcQuota] [numeric](18, 5) NULL,
	[SalesPlanQuotaItemDecode_Quota] [numeric](18, 5) NULL,
	[ActionType_Id] [int] NULL
)
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- редактирует информацию о приложении к расчёту
--
-- Входящие параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@t_SalesPlanQuotaItemDecode			приложение к расчёту
--
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_AddSalePlanQuotaItemDecode] 
	@SalesPlanQuota_Guid				D_GUID,
  @t_SalesPlanQuotaItemDecode	dbo.udt_SalesPlanQuotaItemDecode READONLY,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		DECLARE @SalesPlanQuotaItemDecode_Guid			uniqueidentifier;
		DECLARE @SalesPlanQuotaItem_Guid						uniqueidentifier;
		DECLARE @ProductOwner_Guid									uniqueidentifier;
		DECLARE @ProductType_Guid										uniqueidentifier;
		DECLARE @ObjectType_Id											int;
		DECLARE @ObjectDecode_Guid									uniqueidentifier;
		DECLARE @SalesPlanQuotaItemDecode_Quantity	float;
		DECLARE @SalesPlanQuotaItemDecode_Money			money;
		DECLARE @SalesPlanQuotaItemDecode_CalcQuota	numeric(18,5);
		DECLARE @SalesPlanQuotaItemDecode_Quota			numeric(18,5);
		DECLARE @ActionType_Id											int;
		DECLARE @SalesPlanQuotaItem_Quantity				float; 
		DECLARE @SalesPlanQuotaItem_Money						money;

		DECLARE @ObjectTypeDepartTeamId							int = 0;
		DECLARE @ObjectTypeDepartId									int = 1;
		DECLARE @ObjectTypeCustomerId								int = 2;
		DECLARE @ObjectTypePartSubTypeId						int = 3;

		DECLARE @ActionTypeAdd_Id										int = 0;
		DECLARE @ActionTypeEdit_Id									int = 1;
		DECLARE @ActionTypeDelete_Id								int = 2;

		DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] WHERE [SalesPlanQuotaItem_Guid] IN ( SELECT [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid);
		DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] WHERE [SalesPlanQuotaItem_Guid] IN ( SELECT [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid);
		DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] WHERE [SalesPlanQuotaItem_Guid] IN ( SELECT [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid);
		DELETE FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] WHERE [SalesPlanQuotaItem_Guid] IN ( SELECT [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid);
		DELETE FROM [dbo].[T_SalesPlanQuotaItem] WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid;

		DECLARE crUpdate CURSOR FOR SELECT SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid,
			ProductOwner_Guid,	ProductType_Guid, ObjectType_Id, ObjectDecode_Guid,
			SalesPlanQuotaItemDecode_Quantity,	SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota,	SalesPlanQuotaItemDecode_Quota,
			ActionType_Id, SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money
		FROM @t_SalesPlanQuotaItemDecode;
		OPEN crUpdate;
		fetch next from crUpdate into @SalesPlanQuotaItemDecode_Guid, @SalesPlanQuotaItem_Guid,
			@ProductOwner_Guid,	@ProductType_Guid, @ObjectType_Id, @ObjectDecode_Guid,
			@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
			@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota,
			@ActionType_Id, @SalesPlanQuotaItem_Quantity, @SalesPlanQuotaItem_Money;
		while @@fetch_status = 0
			begin
				SET @SalesPlanQuotaItem_Guid = NULL;

				IF( ( @SalesPlanQuotaItem_Guid IS NULL ) 
						--OR (@SalesPlanQuotaItem_Guid  = '00000000-0000-0000-0000-000000000000') )
				OR ( @SalesPlanQuotaItem_Guid = ( SELECT [dbo].[GetNullGuid]() ) ) )	
					BEGIN
						SELECT Top 1 @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem]
						WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
							AND [ProductOwner_Guid] = @ProductOwner_Guid
							AND [ProductType_Guid] = @ProductType_Guid;

						IF( ( @SalesPlanQuotaItem_Guid IS NULL ) 
							--OR (@SalesPlanQuotaItem_Guid  = '00000000-0000-0000-0000-000000000000') )
							OR ( @SalesPlanQuotaItem_Guid = ( SELECT [dbo].[GetNullGuid]() ) ) )	
							BEGIN
								SET @SalesPlanQuotaItem_Guid = NEWID();
								INSERT INTO [dbo].[T_SalesPlanQuotaItem]( SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, 
									ProductOwner_Guid, ProductType_Guid, SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money )
								VALUES( @SalesPlanQuotaItem_Guid,  @SalesPlanQuota_Guid, @ProductOwner_Guid, @ProductType_Guid, 
									@SalesPlanQuotaItem_Quantity, @SalesPlanQuotaItem_Money );
							END
					END

				IF( @ObjectType_Id = @ObjectTypeCustomerId )
					BEGIN
						-- Клиент
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [Customer_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeCustomer]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 
								@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
								@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeCustomer] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [Customer_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypeDepartTeamId )
					BEGIN
						-- Команда
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [DepartTeam_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								DepartTeam_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 
								@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
								@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepartTeam] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [DepartTeam_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypeDepartId )
					BEGIN
						-- Подразделение
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [Depart_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodeDepart]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 
								@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
								@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodeDepart] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [Depart_Guid] = @ObjectDecode_Guid;
					END
				ELSE IF( @ObjectType_Id = @ObjectTypePartSubTypeId )
					BEGIN
						-- Подгруппа
						IF NOT EXISTS( SELECT [SalesPlanQuotaItemDecode_Guid] FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
														WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
															AND [PartSubType_Guid] = @ObjectDecode_Guid )
							-- добавление		
							INSERT INTO [dbo].[T_SalesPlanQuotaItemDecodePartSubType]( SalesPlanQuotaItemDecode_Guid, SalesPlanQuotaItem_Guid, 
								PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
								SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, 
								Record_Updated, Record_UserUdpated )
							VALUES( NEWID(), @SalesPlanQuotaItem_Guid, @ObjectDecode_Guid, 
								@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
								@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota, 
								sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
						ELSE
							-- редактирование
							UPDATE [dbo].[T_SalesPlanQuotaItemDecodePartSubType] SET [SalesPlanQuotaItemDecode_Quota] = @SalesPlanQuotaItemDecode_Quota
							WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid
								AND [PartSubType_Guid] = @ObjectDecode_Guid;
					END

				fetch next from crUpdate into @SalesPlanQuotaItemDecode_Guid, @SalesPlanQuotaItem_Guid,
					@ProductOwner_Guid,	@ProductType_Guid, @ObjectType_Id, @ObjectDecode_Guid,
					@SalesPlanQuotaItemDecode_Quantity,	@SalesPlanQuotaItemDecode_Money, 
					@SalesPlanQuotaItemDecode_CalcQuota,	@SalesPlanQuotaItemDecode_Quota,
					@ActionType_Id, @SalesPlanQuotaItem_Quantity, @SalesPlanQuotaItem_Money;
			end -- while @@fetch_status = 0

		close crUpdate;
		deallocate crUpdate;

		-- Удаление
    
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

GRANT EXECUTE ON [dbo].[usp_AddSalePlanQuotaItemDecode] TO [public]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetSalePlanQuotaItemDecodeDepart]    Script Date: 22.10.2013 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список записей из [dbo].[SalesPlanQuotaItemDecodeDepartView]
--
-- Входные параметры:
--		@SalesPlanQuota_Guid			- уи расчёта
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetSalePlanQuotaItemDecodeDepart] 
	@SalesPlanQuota_Guid			D_GUID,

  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY
		SELECT Depart_Code AS ObjectDecode_Name, SalesPlanQuotaItem_Guid, SalesPlanQuota_Guid, ProductOwner_Guid, ProductType_Guid, 
			SalesPlanQuotaItem_Quantity, SalesPlanQuotaItem_Money, SalesPlanQuotaItemDecode_Guid, Depart_Guid AS ObjectDecode_Guid, 
			SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, 
			SalesPlanQuotaItemDecode_CalcQuota, SalesPlanQuotaItemDecode_Quota, Owner_Id, Owner_Name, Parttype_Id, Parttype_Name, cast( 0 as int ) AS ObjectDecode_Id
		FROM [dbo].[SalesPlanQuotaItemDecodeDepartView]
		WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
		ORDER BY Owner_Name, Parttype_Name, Depart_Code;

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



CREATE NONCLUSTERED INDEX [INDX_T_SalesPlanQuotaItemDecodeCustomer_SalesPlanQuotaItem_Guid]
ON [dbo].[T_SalesPlanQuotaItemDecodeCustomer] ([SalesPlanQuotaItem_Guid])
INCLUDE ([SalesPlanQuotaItemDecode_Guid])
ON [INDEX]

GO

/****** Object:  StoredProcedure [dbo].[usp_GetSalePlanQuotaItemDecodeCustomer]    Script Date: 22.10.2013 14:11:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список товарных марок и групп утверждённого плана по маркам и группам
--
-- Входные параметры:
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetProductTradeMarkProductTypeForActiveCalcPlan] 
  @ERROR_NUM			int output,
  @ERROR_MES			nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';
	
  BEGIN TRY
		DECLARE @CalcPlan_Guid	D_GUID;
		DECLARE @CalcPlan_Date	D_DATE;
		
		SELECT @CalcPlan_Date = MAX( [CalcPlan_Date] )
		FROM [dbo].[T_CalcPlan]
		WHERE [CalcPlan_Year] = Year( GetDate() )
			AND [CalcPlan_IsUseForReport] = 1;
		
		IF( @CalcPlan_Date IS NOT NULL )
			SELECT @CalcPlan_Guid = [CalcPlan_Guid]
			FROM [dbo].[T_CalcPlan]
			WHERE [CalcPlan_Year] = Year( GetDate() )
				AND [CalcPlan_IsUseForReport] = 1
				AND [CalcPlan_Date] = @CalcPlan_Date; 

		IF( @CalcPlan_Guid IS NOT NULL )
			BEGIN
				WITH ConditionForCalculation ( ObjectTypeId, ObjectGuid, ObjectId, ObjectName  )
				AS
				(
					SELECT cast(0 as int) AS ObjectTypeId, [dbo].[T_CalcPlanItem].Owner_Guid  AS ObjectGuid, 
						[dbo].[T_Owner].Owner_Id AS ObjectId, [dbo].[T_Owner].Owner_Name AS ObjectName
					FROM [dbo].[T_CalcPlanItem] INNER JOIN [dbo].[T_Owner] ON [dbo].[T_CalcPlanItem].Owner_Guid = [dbo].[T_Owner].Owner_Guid
					WHERE [dbo].[T_CalcPlanItem].CalcPlan_Guid = @CalcPlan_Guid

					UNION ALL

					SELECT cast(1 as int) AS ObjectTypeId, [dbo].[T_CalcPlanItem].PartType_Guid  AS ObjectGuid, 
						[dbo].[T_Parttype].Parttype_Id AS ObjectId, [dbo].[T_Parttype].Parttype_Name AS ObjectName
					FROM [dbo].[T_CalcPlanItem] INNER JOIN [dbo].[T_Parttype] ON [dbo].[T_CalcPlanItem].PartType_Guid = [dbo].[T_Parttype].Parttype_Guid
					WHERE [dbo].[T_CalcPlanItem].CalcPlan_Guid = @CalcPlan_Guid
				)
				SELECT DISTINCT ObjectTypeId, ObjectGuid, ObjectId, ObjectName
				FROM ConditionForCalculation
				ORDER BY ObjectTypeId, ObjectName
			END

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

GRANT EXECUTE ON [dbo].[usp_GetProductTradeMarkProductTypeForActiveCalcPlan] TO [public]
GO

CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanItem_CalcPlan_GuidMonthId]
ON [dbo].[T_CalcPlanItem] ([CalcPlan_Guid],[MonthId])
INCLUDE ([Owner_Guid],[PartType_Guid],[CalcPlan_Quantity],[CalcPlan_AllPrice])
GO

CREATE NONCLUSTERED INDEX [INDX_CalcPlanItem_CalcPlan_Guid]
ON [dbo].[T_CalcPlanItem] ([CalcPlan_Guid])
INCLUDE ([Owner_Guid],[PartType_Guid],[CalcPlan_Quantity],[CalcPlan_AllPrice])
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Группировка плана продаж по клиентам и подразделениям
--
-- Входные параметры:

--		@MinQty						минимальный порог плана по количеству в разрезе "Клиент-Подразделение"
--
-- Выходные параметры:
--  @ERROR_NUM					номер ошибки
--  @ERROR_MES					текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_CalcPlanDepartCustomerProductSubType_Group] 
	@MinQty				int,

  @ERROR_NUM		int output,
  @ERROR_MES		nvarchar( 4000 ) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		DECLARE @Owner_Guid uniqueidentifier;
		DECLARE @PartType_Guid uniqueidentifier;
		DECLARE @CalcPlan_Quantity float; 
		DECLARE @CalcPlan_AllPrice money;
		DECLARE @Depart_Guid uniqueidentifier; 
		DECLARE @SalesPlanQuotaItemDecode_Quantity float; 
		DECLARE @SalesPlanQuotaItemDecode_Money money; 
		DECLARE @SalesPlanQuotaItemDecode_Quota numeric( 18, 5 );
		DECLARE @AllItemDecode_Quota numeric( 18, 5 );
		DECLARE @Plan_Quantity float; 
		DECLARE @Plan_AllPrice money;
		DECLARE @CountDepart_Guid int;
		DECLARE @SalesPlanQuotaItem_Guid uniqueidentifier;

		DECLARE @Customer_Guid D_GUID;
		DECLARE @Customer_Quota numeric(18, 5);
		DECLARE @Customer_Quantity float;
		DECLARE @Customer_Money money;
		DECLARE @Customer_PlanQuantity float;
		DECLARE @Customer_PlanMoney money;

		DECLARE @Depart_Quota numeric(18, 5);
		DECLARE @Depart_Quantity float;
		DECLARE @Depart_Money money;
		DECLARE @Depart_PlanQuantity float;
		DECLARE @Depart_PlanMoney money;
		DECLARE @Depart_SumQuota numeric(18, 5);
		DECLARE @PlanDescription nvarchar(128);

		-- Группировка по предельно допустимому минимуму
		UPDATE #PlanItemDepartCustomer SET PlanDescription = '' WHERE PlanDescription IS NULL;

		INSERT INTO #PlanItemDepartCustomerGroup( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
			Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
			Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
			Plan_Quantity, Plan_AllPrice, PlanDescription )
		SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
			Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
			Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
			Plan_Quantity, Plan_AllPrice, PlanDescription
		FROM #PlanItemDepartCustomer
		WHERE Customer_Guid IS NOT NULL
			AND Depart_Guid IS NOT NULL
			AND Plan_Quantity >= @MinQty
		ORDER BY Owner_Guid, PartType_Guid, Customer_Guid;

		DECLARE @GroupRowCount int;
		DECLARE @DiffQty float;
		DECLARE @DiffMoney float;

		DECLARE crDepartCustomerGroup CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
			Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
			Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, PlanDescription,
			Plan_Quantity, Plan_AllPrice
		FROM #PlanItemDepartCustomer
		WHERE Customer_Guid IS NOT NULL
			AND Depart_Guid IS NOT NULL
			AND Plan_Quantity < @MinQty
		ORDER BY Owner_Guid, PartType_Guid, Customer_Guid;
		OPEN crDepartCustomerGroup;
		fetch next from crDepartCustomerGroup into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
			@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
			@Customer_Guid, @Customer_Quantity, @Customer_Money, @Customer_Quota, @PlanDescription,
			@Plan_Quantity, @Plan_AllPrice;
		while @@fetch_status = 0
			begin
				SET @GroupRowCount = NULL;
				SELECT @GroupRowCount = COUNT( * ) 
				FROM #PlanItemDepartCustomerGroup 
				WHERE Owner_Guid = @Owner_Guid 
					AND PartType_Guid = @PartType_Guid 
					AND Customer_Guid = @Customer_Guid;
				IF( @GroupRowCount IS NULL ) SET @GroupRowCount = 0;
				SET @DiffQty = 0;
				SET @DiffMoney = 0;
				IF( @GroupRowCount > 0 )
					BEGIN
						SET @DiffQty = ( ( ( 10000 * @Plan_Quantity)/@GroupRowCount ) * 0.0001 );
						SET @DiffMoney = ( ( ( 10000 * @Plan_AllPrice)/@GroupRowCount ) * 0.0001 );
						UPDATE #PlanItemDepartCustomerGroup 
						SET Plan_Quantity = ( Plan_Quantity + @DiffQty ),
								Plan_AllPrice = ( Plan_AllPrice + @DiffMoney )
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid 
							AND Customer_Guid = @Customer_Guid;
					END
				ELSE
					INSERT INTO #PlanItemDepartCustomerGroup( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
						Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
						Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
						Plan_Quantity, Plan_AllPrice, PlanDescription )
					VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
						@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
						@Customer_Guid, @Customer_Quantity, @Customer_Money, @Customer_Quota, 
						@Plan_Quantity, @Plan_AllPrice, @PlanDescription );

			fetch next from crDepartCustomerGroup into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
				@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
				@Customer_Guid, @Customer_Quantity, @Customer_Money, @Customer_Quota, @PlanDescription,
				@Plan_Quantity, @Plan_AllPrice;
			end -- while @@fetch_status = 0

		close crDepartCustomerGroup;
		deallocate crDepartCustomerGroup;

		INSERT INTO #PlanItemDepartCustomerGroup( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
			Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
			Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
			Plan_Quantity, Plan_AllPrice, PlanDescription )
		SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
			Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
			Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
			Plan_Quantity, Plan_AllPrice, PlanDescription
		FROM #PlanItemDepartCustomer
		WHERE ( Customer_Guid IS NULL ) OR ( Depart_Guid IS NULL );		

		--SELECT * FROM #PlanItemDepartCustomerGroup
		--ORDER BY Owner_Guid, PartType_Guid, Customer_Guid, Depart_Guid;

		-- в #PlanItemDepartCustomerGroup находятся "укрупнённые записи"
		-- всех записи, у которых ( Plan_Quantity < @MinQty ), необходимо объединить с теми, где план превышае минимальный порог

		TRUNCATE TABLE #PlanItemDepartCustomer;
		INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
			Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
			Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
			Plan_Quantity, Plan_AllPrice, PlanDescription)
		SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
			Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
			Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
			Plan_Quantity, Plan_AllPrice, PlanDescription
		FROM #PlanItemDepartCustomerGroup
		WHERE Plan_Quantity < @MinQty
			AND ( ( Customer_Guid IS NOT NULL ) OR ( Depart_Guid IS NOT NULL ) ); 

		DELETE FROM #PlanItemDepartCustomerGroup
		WHERE Plan_Quantity < @MinQty
			AND ( ( Customer_Guid IS NOT NULL ) OR ( Depart_Guid IS NOT NULL ) ); 

		DECLARE @GroupDepart_Guid D_GUID;
		DECLARE @GroupCustomer_Guid D_GUID;
		DECLARE @MinGroupQty float;

		DECLARE crDepartCustomerGroup CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
			Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
			Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, PlanDescription,
			Plan_Quantity, Plan_AllPrice
		FROM #PlanItemDepartCustomer
		ORDER BY Owner_Guid, PartType_Guid, Customer_Guid;
		OPEN crDepartCustomerGroup;
		fetch next from crDepartCustomerGroup into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
			@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
			@Customer_Guid, @Customer_Quantity, @Customer_Money, @Customer_Quota, @PlanDescription,
			@Plan_Quantity, @Plan_AllPrice;
		while @@fetch_status = 0
			begin
				SET @GroupDepart_Guid = NULL;
				SET @GroupCustomer_Guid = NULL;
				SET @MinGroupQty = 0;

				SELECT @MinGroupQty = MIN( Plan_Quantity ) FROM #PlanItemDepartCustomerGroup
				WHERE Owner_Guid = @Owner_Guid 
					AND PartType_Guid = @PartType_Guid 
					AND Customer_Guid IS NOT NULL
					AND Depart_Guid IS NOT NULL
					AND Plan_Quantity >= @MinQty;
				IF( ( @MinGroupQty IS NOT NULL ) AND ( @MinGroupQty > 0 ) ) 
					BEGIN
						SELECT @GroupDepart_Guid = Depart_Guid, @GroupCustomer_Guid = Customer_Guid 
						FROM #PlanItemDepartCustomerGroup 
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid 
							AND Customer_Guid IS NOT NULL
							AND Depart_Guid IS NOT NULL
							AND Plan_Quantity = @MinGroupQty;
				
						UPDATE #PlanItemDepartCustomerGroup 
						SET Plan_Quantity = ( Plan_Quantity + @Plan_Quantity ),
								Plan_AllPrice = ( Plan_AllPrice + @Plan_AllPrice )
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid 
							AND ( ( Customer_Guid IS NOT NULL ) AND ( Customer_Guid = @GroupCustomer_Guid ) )
							AND ( ( Depart_Guid IS NOT NULL ) AND ( Depart_Guid = @GroupDepart_Guid ) )
							AND Plan_Quantity = @MinGroupQty;
					END
				ELSE
					BEGIN
						SELECT @MinGroupQty = MAX( Plan_Quantity ) FROM #PlanItemDepartCustomerGroup
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid 
							AND Customer_Guid IS NOT NULL
							AND Depart_Guid IS NOT NULL
							AND Plan_Quantity < @MinQty;
						IF( ( @MinGroupQty IS NOT NULL ) AND ( @MinGroupQty > 0 ) ) 
							BEGIN
								SELECT @GroupDepart_Guid = Depart_Guid, @GroupCustomer_Guid = Customer_Guid 
								FROM #PlanItemDepartCustomerGroup 
								WHERE Owner_Guid = @Owner_Guid 
									AND PartType_Guid = @PartType_Guid 
									AND Customer_Guid IS NOT NULL
									AND Depart_Guid IS NOT NULL
									AND Plan_Quantity = @MinGroupQty;
				
								UPDATE #PlanItemDepartCustomerGroup 
								SET Plan_Quantity = ( Plan_Quantity + @Plan_Quantity ),
										Plan_AllPrice = ( Plan_AllPrice + @Plan_AllPrice )
								WHERE Owner_Guid = @Owner_Guid 
									AND PartType_Guid = @PartType_Guid 
									AND ( ( Customer_Guid IS NOT NULL ) AND ( Customer_Guid = @GroupCustomer_Guid ) )
									AND ( ( Depart_Guid IS NOT NULL ) AND ( Depart_Guid = @GroupDepart_Guid ) )
									AND Plan_Quantity = @MinGroupQty;
							END
						ELSE
							INSERT INTO #PlanItemDepartCustomerGroup( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
								Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
								Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
								Plan_Quantity, Plan_AllPrice, PlanDescription )
							VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
								@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
								@Customer_Guid, @Customer_Quantity, @Customer_Money, @Customer_Quota, 
								@Plan_Quantity, @Plan_AllPrice, @PlanDescription );
					END

			fetch next from crDepartCustomerGroup into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
				@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
				@Customer_Guid, @Customer_Quantity, @Customer_Money, @Customer_Quota, @PlanDescription,
				@Plan_Quantity, @Plan_AllPrice;
			end -- while @@fetch_status = 0

		close crDepartCustomerGroup;
		deallocate crDepartCustomerGroup;


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

GRANT EXECUTE ON [dbo].[usp_CalcPlanDepartCustomerProductSubType_Group] TO [public]
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Расчёт плана продаж азрезе подразделений, клиентов и подгрупп
--
-- Входные параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@CalcPlan_Guid									УИ расчёта "Марка-Группа"
--		@MonthId												номер месяца
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_CalcPlanDepartCustomerProductSubType] 
	@SalesPlanQuota_Guid				D_GUID,
	@CalcPlan_Guid							D_GUID,
	@MonthId										D_ID,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		-- план на указанный месяц по маркам и группам
		CREATE TABLE #CalcPlanItem( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money );
		INSERT INTO #CalcPlanItem(  Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice )
		SELECT Owner_Guid, PartType_Guid, SUM( CalcPlan_Quantity ), SUM( CalcPlan_AllPrice )
		FROM [dbo].[T_CalcPlanItem]
		WHERE [CalcPlan_Guid] = @CalcPlan_Guid
			AND [MonthId] = @MonthId
			AND [CalcPlan_Quantity] > 0
		GROUP BY Owner_Guid, PartType_Guid;

		-- план разбивается по подразделениям
		CREATE TABLE #PlanItemDepart( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		-- план разбивается по клиентам
		CREATE TABLE #PlanItemCustomer( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Customer_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		-- план разбивается по подгруппам
		CREATE TABLE #PlanItemPartSubType( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			PartSubType_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		CREATE TABLE #PlanItemDepartCustomer( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, Depart_Quantity float, Depart_Money money, Depart_Quota numeric( 18, 5 ), 
			Customer_Guid uniqueidentifier, Customer_Quantity float, Customer_Money money, Customer_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money, PlanDescription nvarchar(128)  );

		CREATE TABLE #PlanItemDepartCustomerGroup( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, Depart_Quantity float, Depart_Money money, Depart_Quota numeric( 18, 5 ), 
			Customer_Guid uniqueidentifier, Customer_Quantity float, Customer_Money money, Customer_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money, PlanDescription nvarchar(128)  );

		CREATE TABLE #PlanItemDepartCustomerPartSubtype( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, 
			Customer_Guid uniqueidentifier, 
			PartSubType_Guid uniqueidentifier, 
			Plan_Quantity float, Plan_AllPrice money );

		CREATE TABLE #ItemQuotaDepart( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );
		CREATE TABLE #ItemQuotaCustomer( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );
		CREATE TABLE #ItemQuotaPartSubType( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );

		DECLARE @Owner_Guid uniqueidentifier;
		DECLARE @PartType_Guid uniqueidentifier;
		DECLARE @CalcPlan_Quantity float; 
		DECLARE @CalcPlan_AllPrice money;
		DECLARE @Depart_Guid uniqueidentifier; 
		DECLARE @SalesPlanQuotaItemDecode_Quantity float; 
		DECLARE @SalesPlanQuotaItemDecode_Money money; 
		DECLARE @SalesPlanQuotaItemDecode_Quota numeric( 18, 5 );
		DECLARE @AllItemDecode_Quota numeric( 18, 5 );
		DECLARE @Plan_Quantity float; 
		DECLARE @Plan_AllPrice money;
		DECLARE @CountDepart_Guid int;
		DECLARE @SalesPlanQuotaItem_Guid uniqueidentifier;

		DECLARE crPlanItem CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanItem;
		fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				SET @SalesPlanQuotaItem_Guid = NULL;

				SELECT @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] 
				WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
					AND [ProductOwner_Guid] = @Owner_Guid
					AND [ProductType_Guid] = @PartType_Guid;

				IF( @SalesPlanQuotaItem_Guid IS NOT NULL )
					BEGIN
						-- сумма долей подразделений в рамках записи "Марка-Группа" 
						SET @AllItemDecode_Quota = 0;
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;
						IF( @AllItemDecode_Quota IS NULL ) SET @AllItemDecode_Quota = 0;

						-- если сумма < 1, то необходимо запросить дополнительные подзразделения
						-- если же сумма рана нулю, то запрашиваются все подразделения команды, которая работает с указанной маркой

						DELETE FROM #ItemQuotaDepart;

						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								-- выборка подразделений команды, работающей с маркой
								INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Depart_Guid], 0, 0, 10000/( COUNT(Depart_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_Depart]
								WHERE [DepartTeam_Guid] IS NOT NULL
									AND [DepartTeam_Guid] IN ( SELECT TOP 1 [DepartTeam_Guid] FROM [dbo].[T_DepartTeamOwner] WHERE [Owner_Guid] = @Owner_Guid );
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Depart_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money,  ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [Depart_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid ;
								
							END
						
						INSERT INTO #PlanItemDepart( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaDepart; 

						-- сумма долей клиентов в рамках записи "Марка-Группа" 
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;

						-- если сумма < 1, то необходимо запросить дополнительных клиентов 
						-- если же сумма рана нулю, то запрашиваются все клиенты, которые работают с указанной маркой (через подразделение)
						DELETE FROM #ItemQuotaCustomer;

						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Customer_Guid], 0, 0, 10000/( COUNT(Customer_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_Customer]
								WHERE [dbo].[T_Customer].CustomerActiveType_Guid = 'CDD98D87-378A-48F1-BF46-ED7FE96BBD68'
									AND [Customer_Guid] IN ( SELECT [Customer_Guid] FROM [dbo].[T_CustomerDepart] 
																						WHERE [Depart_Guid] IN ( SELECT [Depart_Guid] FROM #PlanItemDepart
																																			WHERE Owner_Guid = @Owner_Guid AND PartType_Guid =  @PartType_Guid ) );
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Customer_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money,  ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [Customer_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid ;
								
							END
						
						INSERT INTO #PlanItemCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaCustomer; 

						-- сумма долей подгрупп в рамках записи "Марка-Группа" 
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;

						-- если сумма < 1, то необходимо запросить дополнительные подгруппы 
						-- если же сумма рана нулю, то запрашиваются все подгруппы, входящие в указанную марку
						DELETE FROM #ItemQuotaPartSubType;

						IF( @AllItemDecode_Quota IS NULL ) SET @AllItemDecode_Quota = 0;
						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								IF EXISTS( SELECT [PartSubType_Guid]	FROM [dbo].[T_PartSubType]
														WHERE [Partsubtype_IsActive] = 1									
															AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																												WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																									WHERE Owner_Guid = @Owner_Guid ) )
															AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																												WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																									WHERE [Parttype_Guid] = @PartType_Guid ) ) )
									BEGIN
										INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
										SELECT [PartSubType_Guid], 0, 0, 10000/( COUNT(PartSubType_Guid)  OVER( )) * 0.0001
										FROM [dbo].[T_PartSubType]
										WHERE [Partsubtype_IsActive] = 1									
											AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																								WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																					WHERE Owner_Guid = @Owner_Guid ) )
											AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																								WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																					WHERE [Parttype_Guid] = @PartType_Guid ) );
									END
								ELSE
									BEGIN
										PRINT 'Не найдено:'
										PRINT @Owner_Guid;
										PRINT @PartType_Guid;
									END
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [PartSubType_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [PartSubType_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;
								
							END
						
						INSERT INTO #PlanItemPartSubType( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaPartSubType; 
					END
				ELSE
					BEGIN
						-- в долях клиентов сочетание марки и группы не найдено
						DELETE FROM #ItemQuotaCustomer;
						INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
						SELECT [Customer_Guid], 0, 0, 10000/( COUNT(Customer_Guid)  OVER( )) * 0.0001
						FROM [dbo].[T_Customer]
						WHERE [dbo].[T_Customer].CustomerActiveType_Guid = 'CDD98D87-378A-48F1-BF46-ED7FE96BBD68'
							AND [Customer_Guid] IN ( SELECT [Customer_Guid] FROM [dbo].[T_CustomerDepart] 
																				WHERE [Depart_Guid] IN ( SELECT [Depart_Guid] FROM #PlanItemDepart
																																	WHERE Owner_Guid = @Owner_Guid AND PartType_Guid =  @PartType_Guid ) );
						INSERT INTO #PlanItemCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaCustomer; 

						DELETE FROM #ItemQuotaDepart;
						INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
						SELECT [Depart_Guid], 0, 0, 10000/( COUNT(Depart_Guid)  OVER( )) * 0.0001
						FROM [dbo].[T_Depart]
						WHERE [DepartTeam_Guid] IS NOT NULL
							AND [DepartTeam_Guid] IN ( SELECT TOP 1 [DepartTeam_Guid] FROM [dbo].[T_DepartTeamOwner] WHERE [Owner_Guid] = @Owner_Guid );

						INSERT INTO #PlanItemDepart( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaDepart; 


						-- в долях товарных подгрупп сочетание марки и группы не найдено
						DELETE FROM  #ItemQuotaPartSubType;
						IF EXISTS( SELECT [PartSubType_Guid]	FROM [dbo].[T_PartSubType]
												WHERE --[Partsubtype_IsActive] = 1 AND								
													 [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																										WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																							WHERE Owner_Guid = @Owner_Guid ) )
													AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																										WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																							WHERE [Parttype_Guid] = @PartType_Guid ) ) )
							BEGIN
								PRINT 'Поймал!'
								INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [PartSubType_Guid], 0, 0, 10000/( COUNT(PartSubType_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_PartSubType]
								WHERE --[Partsubtype_IsActive] = 1		AND 							
									 [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																						WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																			WHERE Owner_Guid = @Owner_Guid ) )
									AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																						WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																			WHERE [Parttype_Guid] = @PartType_Guid ) );

								INSERT INTO #PlanItemPartSubType( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
									PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
								SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
									Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
								FROM 	#ItemQuotaPartSubType; 

							END
					END
				fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanItem;
		deallocate crPlanItem;

		UPDATE #PlanItemDepart SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		UPDATE #PlanItemCustomer SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		UPDATE #PlanItemPartSubType SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		--SELECT * FROM #CalcPlanItem;
		--SELECT * FROM #PlanItemDepart;
		--SELECT * FROM #PlanItemCustomer;
		--SELECT * FROM #PlanItemPartSubType;

		DECLARE @Customer_Guid D_GUID;
		DECLARE @Customer_Quota numeric(18, 5);
		DECLARE @Customer_Quantity float;
		DECLARE @Customer_Money money;
		DECLARE @Customer_PlanQuantity float;
		DECLARE @Customer_PlanMoney money;

		DECLARE @Depart_Quota numeric(18, 5);
		DECLARE @Depart_Quantity float;
		DECLARE @Depart_Money money;
		DECLARE @Depart_PlanQuantity float;
		DECLARE @Depart_PlanMoney money;
		DECLARE @Depart_SumQuota numeric(18, 5);
		DECLARE @PlanDescription nvarchar(128);

		DECLARE crPlanItemDepartCustomer CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanItemDepartCustomer;
		fetch next from crPlanItemDepartCustomer into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				
				-- для каждой группы записей "Товарная марка - Товарная группа" 
				-- запрашивается план по клиентам из #PlanItemCustomer,  а затем для каждого клиента необходимо получить список подразделений, с которыми он работает и распределить план клиента по этим подразделениям
				-- привязка клиента к подразделениям находится в [dbo].[T_CustomerDepart], а план по подразделениям в #PlanItemDepart
				IF EXISTS( SELECT Customer_Guid FROM #PlanItemCustomer	WHERE Owner_Guid = @Owner_Guid 	AND PartType_Guid = @PartType_Guid )
					BEGIN
						DECLARE crCustomerPlan CURSOR FOR SELECT	Customer_Guid, SalesPlanQuotaItemDecode_Quota, 
							SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Plan_Quantity, Plan_AllPrice
						FROM #PlanItemCustomer
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid;
						OPEN crCustomerPlan;
						fetch next from crCustomerPlan into @Customer_Guid, @Customer_Quota, @Customer_Quantity, @Customer_Money, @Customer_PlanQuantity, @Customer_PlanMoney;
						while @@fetch_status = 0
							begin
								SET @Depart_SumQuota = 0;
								IF EXISTS( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  )
									BEGIN
										-- клиенту назначены подразделения
										SELECT @Depart_SumQuota = SUM( SalesPlanQuotaItemDecode_Quota ) 
										FROM #PlanItemDepart
										WHERE Owner_Guid = @Owner_Guid 
											AND PartType_Guid = @PartType_Guid
											AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  ); 
										IF( @Depart_SumQuota IS NULL ) SET @Depart_SumQuota = 0;
										
										IF EXISTS( SELECT Depart_Guid FROM #PlanItemDepart
																WHERE Owner_Guid = @Owner_Guid 
																	AND PartType_Guid = @PartType_Guid
																	AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  ) )
											BEGIN
												DECLARE crDepartPlan CURSOR FOR SELECT  Depart_Guid, SalesPlanQuotaItemDecode_Quota, 
													SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Plan_Quantity, Plan_AllPrice
												FROM #PlanItemDepart
												WHERE Owner_Guid = @Owner_Guid 
													AND PartType_Guid = @PartType_Guid
													AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  );
												OPEN crDepartPlan;
												fetch next from crDepartPlan into @Depart_Guid, @Depart_Quota, @Depart_Quantity, @Depart_Money, @Depart_PlanQuantity, @Depart_PlanMoney;
												while @@fetch_status = 0
													begin
														-- вставка в итоговую таблицу
														INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
															Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
															Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
															Plan_Quantity, Plan_AllPrice )
														VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
															@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
															@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
															( ( @Depart_Quota/@Depart_SumQuota ) * @Customer_PlanQuantity ), 
															( ( @Depart_Quota/@Depart_SumQuota ) * @Customer_PlanMoney ) 
															);	

														fetch next from crDepartPlan into @Depart_Guid, @Depart_Quota, @Depart_Quantity, @Depart_Money, @Depart_PlanQuantity, @Depart_PlanMoney;
													end -- while @@fetch_status = 0

												close crDepartPlan;
												deallocate crDepartPlan;									
											END
										ELSE
											BEGIN
												INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
													Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
													Plan_Quantity, Plan_AllPrice, PlanDescription )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													NULL, 0, 0, 0, 
													@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
													@Customer_PlanQuantity, 
													@Customer_PlanMoney, 'для клиента нет разбивки плана по подразделениям' 
													);										
											END
									END																																																																																																																													
								ELSE
									BEGIN
										INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
											Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
											Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
											Plan_Quantity, Plan_AllPrice, PlanDescription )
										VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
											NULL, 0, 0, 0, 
											@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
											@Customer_PlanQuantity, 
											@Customer_PlanMoney, 'для клиента не назначены подразделения' 
											);										
									END						

								fetch next from crCustomerPlan into @Customer_Guid, @Customer_Quota, @Customer_Quantity, @Customer_Money, @Customer_PlanQuantity, @Customer_PlanMoney;
							end -- while @@fetch_status = 0

						close crCustomerPlan;
						deallocate crCustomerPlan;
					END
				ELSE
					BEGIN
						INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
							Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
							Plan_Quantity, Plan_AllPrice, PlanDescription )
						VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice,
							NULL, 0, 0, 0, 
							NULL, 0, 0, 0,
							@CalcPlan_Quantity, @CalcPlan_AllPrice, 'для марки и группы нет разбивки плана по клиентам' );
					END

				fetch next from crPlanItemDepartCustomer into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanItemDepartCustomer;
		deallocate crPlanItemDepartCustomer;

		-- Группировка по предельно допустимому минимуму
		DECLARE @MinQty int = 100;

		UPDATE #PlanItemDepartCustomer SET PlanDescription = '' WHERE PlanDescription IS NULL;

		EXEC [dbo].[usp_CalcPlanDepartCustomerProductSubType_Group] @MinQty = @MinQty,  @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;


		--SELECT * FROM #PlanItemDepartCustomerGroup
		--ORDER BY Owner_Guid, PartType_Guid, Customer_Guid, Depart_Guid;

		-- итоговый план по марке-группе-клиенту-подразделению-подгруппе

		CREATE TABLE #tmpPlanItemDepartCustomer(  
			Depart_Guid uniqueidentifier, Customer_Guid uniqueidentifier,  
			Plan_Quantity float, Plan_AllPrice money, PlanReserv_Quantity float, PlanReserv_AllPrice money  );

		DECLARE @Plan_Quantity_CustomerDepart float;	
		DECLARE @Plan_AllPrice_CustomerDepart money;
		DECLARE @Plan_Quantity_PartSubType float;	
		DECLARE @Plan_AllPrice_PartSubType money;
		DECLARE @Plan_Quantity_Ost float;	
		DECLARE @Plan_AllPrice_Ost money;
		DECLARE @tmpAllPrice money;
		DECLARE @CustomerDepartCount int;
		DECLARE @RecordNum int;
		DECLARE @PartSubType_Guid D_GUID;

		DECLARE crPlanTotal CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanTotal;
		fetch next from crPlanTotal into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				DELETE FROM #tmpPlanItemDepartCustomer;

				INSERT INTO #tmpPlanItemDepartCustomer( Depart_Guid, Customer_Guid,  
					Plan_Quantity, Plan_AllPrice, PlanReserv_Quantity, PlanReserv_AllPrice )
				SELECT Depart_Guid, Customer_Guid, Plan_Quantity, Plan_AllPrice, 0, 0
				FROM #PlanItemDepartCustomerGroup
				WHERE  Owner_Guid = Owner_Guid AND PartType_Guid = @PartType_Guid;

				DECLARE crPartSubTypePlan CURSOR FOR SELECT PartSubType_Guid, Plan_Quantity,	Plan_AllPrice
				FROM #PlanItemPartSubType
				WHERE  Owner_Guid = @Owner_Guid AND PartType_Guid = @PartType_Guid
				ORDER BY Plan_Quantity DESC;
				OPEN crPartSubTypePlan;
				fetch next from crPartSubTypePlan into @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType;
				while @@fetch_status = 0
					begin
						
						DECLARE crCustomerDepart CURSOR FOR SELECT Depart_Guid, Customer_Guid,  
							Plan_Quantity, Plan_AllPrice, ( Plan_Quantity - PlanReserv_Quantity  ), 
							( Plan_AllPrice - PlanReserv_AllPrice )
						FROM #tmpPlanItemDepartCustomer
						WHERE ( ( Plan_Quantity - PlanReserv_Quantity ) > 0 )
							AND ( ( Plan_AllPrice - PlanReserv_AllPrice ) > 0 )
						ORDER BY ( Plan_Quantity - PlanReserv_Quantity ) DESC;
						OPEN crCustomerDepart;
						fetch next from crCustomerDepart into @Depart_Guid, @Customer_Guid, @Plan_Quantity_CustomerDepart, @Plan_AllPrice_CustomerDepart, 
							@Plan_Quantity_Ost, @Plan_AllPrice_Ost;
						while @@fetch_status = 0
							begin
								IF( ( @Plan_Quantity_Ost IS NOT NULL ) AND ( @Plan_Quantity_Ost > 1 ) AND 
										--( @Plan_AllPrice_Ost IS NOT NULL ) AND ( @Plan_AllPrice_Ost > 0 ) AND 
										( @Plan_Quantity_PartSubType > 0 )  )
									BEGIN
										IF( @Plan_Quantity_Ost >= @Plan_Quantity_PartSubType ) 
											--	( @Plan_AllPrice_Ost >= @Plan_AllPrice_PartSubType ) )
											BEGIN
												UPDATE #tmpPlanItemDepartCustomer 
													SET PlanReserv_Quantity = ( PlanReserv_Quantity + @Plan_Quantity_PartSubType ), 
															PlanReserv_AllPrice = ( PlanReserv_AllPrice + @Plan_AllPrice_PartSubType )
												WHERE Depart_Guid = @Depart_Guid AND Customer_Guid = @Customer_Guid;

												INSERT INTO #PlanItemDepartCustomerPartSubtype( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Customer_Guid, PartSubType_Guid, Plan_Quantity, Plan_AllPrice )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													@Depart_Guid, @Customer_Guid, @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType );

												SET @Plan_Quantity_PartSubType = 0;
												SET @Plan_AllPrice_PartSubType = 0;
											END
										ELSE
											BEGIN
												SET @tmpAllPrice = 0;
												SET @tmpAllPrice =  (( ( 10000 * @Plan_Quantity_Ost)/@Plan_Quantity_PartSubType ) * 0.0001 ) * @Plan_AllPrice_PartSubType;

												UPDATE #tmpPlanItemDepartCustomer 
													SET PlanReserv_Quantity = ( PlanReserv_Quantity + @Plan_Quantity_Ost ), --Plan_Quantity, 
															PlanReserv_AllPrice =  ( PlanReserv_AllPrice + @tmpAllPrice ) 
												WHERE Depart_Guid = @Depart_Guid AND Customer_Guid = @Customer_Guid;

												INSERT INTO #PlanItemDepartCustomerPartSubtype( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Customer_Guid, PartSubType_Guid, Plan_Quantity, Plan_AllPrice )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													@Depart_Guid, @Customer_Guid, @PartSubType_Guid, @Plan_Quantity_Ost, @tmpAllPrice );

												SET @Plan_Quantity_PartSubType = ( @Plan_Quantity_PartSubType - @Plan_Quantity_Ost );
												SET @Plan_AllPrice_PartSubType = ( @Plan_AllPrice_PartSubType - @tmpAllPrice );
											END

									END

								fetch next from crCustomerDepart into @Depart_Guid, @Customer_Guid, @Plan_Quantity_CustomerDepart, @Plan_AllPrice_CustomerDepart, 
							@Plan_Quantity_Ost, @Plan_AllPrice_Ost;
							end -- while @@fetch_status = 0

						close crCustomerDepart;
						deallocate crCustomerDepart;

						fetch next from crPartSubTypePlan into @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType;
					end -- while @@fetch_status = 0

				close crPartSubTypePlan;
				deallocate crPartSubTypePlan;

				fetch next from crPlanTotal into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanTotal;
		deallocate crPlanTotal;

		DELETE FROM #PlanItemDepartCustomerPartSubtype WHERE Plan_Quantity  < 1;
		UPDATE #PlanItemDepartCustomerPartSubtype SET Plan_Quantity = ROUND( Plan_Quantity, 0 );

		SELECT * FROM #PlanItemDepartCustomerPartSubtype
		ORDER BY Owner_Guid, PartType_Guid, Customer_Guid, Depart_Guid, PartSubType_Guid;

		DROP TABLE #tmpPlanItemDepartCustomer;
		DROP TABLE #PlanItemDepartCustomerPartSubtype;
		DROP TABLE #PlanItemDepartCustomerGroup;
		DROP TABLE #PlanItemDepartCustomer;
		DROP TABLE #PlanItemPartSubType;
		DROP TABLE #PlanItemCustomer;
		DROP TABLE #PlanItemDepart;
		DROP TABLE #ItemQuotaDepart;
		DROP TABLE #CalcPlanItem;

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

GRANT EXECUTE ON [dbo].[usp_CalcPlanDepartCustomerProductSubType] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Расчёт плана продаж азрезе подразделений, клиентов и подгрупп
--
-- Входные параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@CalcPlan_Guid									УИ расчёта "Марка-Группа"
--		@MonthId												номер месяца
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_CalcPlanDepartCustomerProductSubType] 
	@SalesPlanQuota_Guid				D_GUID,
	@CalcPlan_Guid							D_GUID,
	@MonthId										D_ID,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		-- план на указанный месяц по маркам и группам
		CREATE TABLE #CalcPlanItem( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money );
		INSERT INTO #CalcPlanItem(  Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice )
		SELECT Owner_Guid, PartType_Guid, SUM( CalcPlan_Quantity ), SUM( CalcPlan_AllPrice )
		FROM [dbo].[T_CalcPlanItem]
		WHERE [CalcPlan_Guid] = @CalcPlan_Guid
			AND [MonthId] = @MonthId
			AND [CalcPlan_Quantity] > 0
		GROUP BY Owner_Guid, PartType_Guid;

		-- план разбивается по подразделениям
		CREATE TABLE #PlanItemDepart( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		-- план разбивается по клиентам
		CREATE TABLE #PlanItemCustomer( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Customer_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		-- план разбивается по подгруппам
		CREATE TABLE #PlanItemPartSubType( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			PartSubType_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		CREATE TABLE #PlanItemDepartCustomer( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, Depart_Quantity float, Depart_Money money, Depart_Quota numeric( 18, 5 ), 
			Customer_Guid uniqueidentifier, Customer_Quantity float, Customer_Money money, Customer_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money, PlanDescription nvarchar(128)  );

		CREATE TABLE #PlanItemDepartCustomerGroup( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, Depart_Quantity float, Depart_Money money, Depart_Quota numeric( 18, 5 ), 
			Customer_Guid uniqueidentifier, Customer_Quantity float, Customer_Money money, Customer_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money, PlanDescription nvarchar(128)  );

		CREATE TABLE #PlanItemDepartCustomerPartSubtype( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, 
			Customer_Guid uniqueidentifier, 
			PartSubType_Guid uniqueidentifier, 
			Plan_Quantity float, Plan_AllPrice money );

		CREATE TABLE #ItemQuotaDepart( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );
		CREATE TABLE #ItemQuotaCustomer( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );
		CREATE TABLE #ItemQuotaPartSubType( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );

		DECLARE @Owner_Guid uniqueidentifier;
		DECLARE @PartType_Guid uniqueidentifier;
		DECLARE @CalcPlan_Quantity float; 
		DECLARE @CalcPlan_AllPrice money;
		DECLARE @Depart_Guid uniqueidentifier; 
		DECLARE @SalesPlanQuotaItemDecode_Quantity float; 
		DECLARE @SalesPlanQuotaItemDecode_Money money; 
		DECLARE @SalesPlanQuotaItemDecode_Quota numeric( 18, 5 );
		DECLARE @AllItemDecode_Quota numeric( 18, 5 );
		DECLARE @Plan_Quantity float; 
		DECLARE @Plan_AllPrice money;
		DECLARE @CountDepart_Guid int;
		DECLARE @SalesPlanQuotaItem_Guid uniqueidentifier;

		DECLARE crPlanItem CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanItem;
		fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				SET @SalesPlanQuotaItem_Guid = NULL;

				SELECT @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] 
				WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
					AND [ProductOwner_Guid] = @Owner_Guid
					AND [ProductType_Guid] = @PartType_Guid;

				IF( @SalesPlanQuotaItem_Guid IS NOT NULL )
					BEGIN
						-- сумма долей подразделений в рамках записи "Марка-Группа" 
						SET @AllItemDecode_Quota = 0;
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;
						IF( @AllItemDecode_Quota IS NULL ) SET @AllItemDecode_Quota = 0;

						-- если сумма < 1, то необходимо запросить дополнительные подзразделения
						-- если же сумма рана нулю, то запрашиваются все подразделения команды, которая работает с указанной маркой

						DELETE FROM #ItemQuotaDepart;

						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								-- выборка подразделений команды, работающей с маркой
								INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Depart_Guid], 0, 0, 10000/( COUNT(Depart_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_Depart]
								WHERE [DepartTeam_Guid] IS NOT NULL
									AND [DepartTeam_Guid] IN ( SELECT TOP 1 [DepartTeam_Guid] FROM [dbo].[T_DepartTeamOwner] WHERE [Owner_Guid] = @Owner_Guid );
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Depart_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money,  ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [Depart_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid ;
								
							END
						
						INSERT INTO #PlanItemDepart( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaDepart; 

						-- сумма долей клиентов в рамках записи "Марка-Группа" 
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;

						-- если сумма < 1, то необходимо запросить дополнительных клиентов 
						-- если же сумма рана нулю, то запрашиваются все клиенты, которые работают с указанной маркой (через подразделение)
						DELETE FROM #ItemQuotaCustomer;

						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Customer_Guid], 0, 0, 10000/( COUNT(Customer_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_Customer]
								WHERE [dbo].[T_Customer].CustomerActiveType_Guid = 'CDD98D87-378A-48F1-BF46-ED7FE96BBD68'
									AND [Customer_Guid] IN ( SELECT [Customer_Guid] FROM [dbo].[T_CustomerDepart] 
																						WHERE [Depart_Guid] IN ( SELECT [Depart_Guid] FROM #PlanItemDepart
																																			WHERE Owner_Guid = @Owner_Guid AND PartType_Guid =  @PartType_Guid ) );
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Customer_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money,  ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [Customer_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid ;
								
							END
						
						INSERT INTO #PlanItemCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaCustomer; 

						-- сумма долей подгрупп в рамках записи "Марка-Группа" 
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;

						-- если сумма < 1, то необходимо запросить дополнительные подгруппы 
						-- если же сумма рана нулю, то запрашиваются все подгруппы, входящие в указанную марку
						DELETE FROM #ItemQuotaPartSubType;

						IF( @AllItemDecode_Quota IS NULL ) SET @AllItemDecode_Quota = 0;
						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								IF EXISTS( SELECT [PartSubType_Guid]	FROM [dbo].[T_PartSubType]
														WHERE [Partsubtype_IsActive] = 1									
															AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																												WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																									WHERE Owner_Guid = @Owner_Guid ) )
															AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																												WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																									WHERE [Parttype_Guid] = @PartType_Guid ) ) )
									BEGIN
										INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
										SELECT [PartSubType_Guid], 0, 0, 10000/( COUNT(PartSubType_Guid)  OVER( )) * 0.0001
										FROM [dbo].[T_PartSubType]
										WHERE [Partsubtype_IsActive] = 1									
											AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																								WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																					WHERE Owner_Guid = @Owner_Guid ) )
											AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																								WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																					WHERE [Parttype_Guid] = @PartType_Guid ) );
									END
								ELSE
									BEGIN
										PRINT 'Не найдено:'
										PRINT @Owner_Guid;
										PRINT @PartType_Guid;
									END
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [PartSubType_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [PartSubType_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;
								
							END
						
						INSERT INTO #PlanItemPartSubType( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaPartSubType; 
					END
				ELSE
					BEGIN
						-- в долях клиентов сочетание марки и группы не найдено
						DELETE FROM #ItemQuotaCustomer;
						INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
						SELECT [Customer_Guid], 0, 0, 10000/( COUNT(Customer_Guid)  OVER( )) * 0.0001
						FROM [dbo].[T_Customer]
						WHERE [dbo].[T_Customer].CustomerActiveType_Guid = 'CDD98D87-378A-48F1-BF46-ED7FE96BBD68'
							AND [Customer_Guid] IN ( SELECT [Customer_Guid] FROM [dbo].[T_CustomerDepart] 
																				WHERE [Depart_Guid] IN ( SELECT [Depart_Guid] FROM #PlanItemDepart
																																	WHERE Owner_Guid = @Owner_Guid AND PartType_Guid =  @PartType_Guid ) );
						INSERT INTO #PlanItemCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaCustomer; 

						DELETE FROM #ItemQuotaDepart;
						INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
						SELECT [Depart_Guid], 0, 0, 10000/( COUNT(Depart_Guid)  OVER( )) * 0.0001
						FROM [dbo].[T_Depart]
						WHERE [DepartTeam_Guid] IS NOT NULL
							AND [DepartTeam_Guid] IN ( SELECT TOP 1 [DepartTeam_Guid] FROM [dbo].[T_DepartTeamOwner] WHERE [Owner_Guid] = @Owner_Guid );

						INSERT INTO #PlanItemDepart( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaDepart; 


						-- в долях товарных подгрупп сочетание марки и группы не найдено
						DELETE FROM  #ItemQuotaPartSubType;
						IF EXISTS( SELECT [PartSubType_Guid]	FROM [dbo].[T_PartSubType]
												WHERE --[Partsubtype_IsActive] = 1 AND								
													 [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																										WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																							WHERE Owner_Guid = @Owner_Guid ) )
													AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																										WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																							WHERE [Parttype_Guid] = @PartType_Guid ) ) )
							BEGIN
								PRINT 'Поймал!'
								INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [PartSubType_Guid], 0, 0, 10000/( COUNT(PartSubType_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_PartSubType]
								WHERE --[Partsubtype_IsActive] = 1		AND 							
									 [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																						WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																			WHERE Owner_Guid = @Owner_Guid ) )
									AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																						WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																			WHERE [Parttype_Guid] = @PartType_Guid ) );

								INSERT INTO #PlanItemPartSubType( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
									PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
								SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
									Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
								FROM 	#ItemQuotaPartSubType; 

							END
					END
				fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanItem;
		deallocate crPlanItem;

		UPDATE #PlanItemDepart SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		UPDATE #PlanItemCustomer SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		UPDATE #PlanItemPartSubType SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		--SELECT * FROM #CalcPlanItem;
		--SELECT * FROM #PlanItemDepart;
		--SELECT * FROM #PlanItemCustomer;
		--SELECT * FROM #PlanItemPartSubType;

		DECLARE @Customer_Guid D_GUID;
		DECLARE @Customer_Quota numeric(18, 5);
		DECLARE @Customer_Quantity float;
		DECLARE @Customer_Money money;
		DECLARE @Customer_PlanQuantity float;
		DECLARE @Customer_PlanMoney money;

		DECLARE @Depart_Quota numeric(18, 5);
		DECLARE @Depart_Quantity float;
		DECLARE @Depart_Money money;
		DECLARE @Depart_PlanQuantity float;
		DECLARE @Depart_PlanMoney money;
		DECLARE @Depart_SumQuota numeric(18, 5);
		DECLARE @PlanDescription nvarchar(128);

		DECLARE crPlanItemDepartCustomer CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanItemDepartCustomer;
		fetch next from crPlanItemDepartCustomer into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				
				-- для каждой группы записей "Товарная марка - Товарная группа" 
				-- запрашивается план по клиентам из #PlanItemCustomer,  а затем для каждого клиента необходимо получить список подразделений, с которыми он работает и распределить план клиента по этим подразделениям
				-- привязка клиента к подразделениям находится в [dbo].[T_CustomerDepart], а план по подразделениям в #PlanItemDepart
				IF EXISTS( SELECT Customer_Guid FROM #PlanItemCustomer	WHERE Owner_Guid = @Owner_Guid 	AND PartType_Guid = @PartType_Guid )
					BEGIN
						DECLARE crCustomerPlan CURSOR FOR SELECT	Customer_Guid, SalesPlanQuotaItemDecode_Quota, 
							SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Plan_Quantity, Plan_AllPrice
						FROM #PlanItemCustomer
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid;
						OPEN crCustomerPlan;
						fetch next from crCustomerPlan into @Customer_Guid, @Customer_Quota, @Customer_Quantity, @Customer_Money, @Customer_PlanQuantity, @Customer_PlanMoney;
						while @@fetch_status = 0
							begin
								SET @Depart_SumQuota = 0;
								IF EXISTS( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  )
									BEGIN
										-- клиенту назначены подразделения
										SELECT @Depart_SumQuota = SUM( SalesPlanQuotaItemDecode_Quota ) 
										FROM #PlanItemDepart
										WHERE Owner_Guid = @Owner_Guid 
											AND PartType_Guid = @PartType_Guid
											AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  ); 
										IF( @Depart_SumQuota IS NULL ) SET @Depart_SumQuota = 0;
										
										IF EXISTS( SELECT Depart_Guid FROM #PlanItemDepart
																WHERE Owner_Guid = @Owner_Guid 
																	AND PartType_Guid = @PartType_Guid
																	AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  ) )
											BEGIN
												DECLARE crDepartPlan CURSOR FOR SELECT  Depart_Guid, SalesPlanQuotaItemDecode_Quota, 
													SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Plan_Quantity, Plan_AllPrice
												FROM #PlanItemDepart
												WHERE Owner_Guid = @Owner_Guid 
													AND PartType_Guid = @PartType_Guid
													AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  );
												OPEN crDepartPlan;
												fetch next from crDepartPlan into @Depart_Guid, @Depart_Quota, @Depart_Quantity, @Depart_Money, @Depart_PlanQuantity, @Depart_PlanMoney;
												while @@fetch_status = 0
													begin
														-- вставка в итоговую таблицу
														INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
															Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
															Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
															Plan_Quantity, Plan_AllPrice )
														VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
															@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
															@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
															( ( @Depart_Quota/@Depart_SumQuota ) * @Customer_PlanQuantity ), 
															( ( @Depart_Quota/@Depart_SumQuota ) * @Customer_PlanMoney ) 
															);	

														fetch next from crDepartPlan into @Depart_Guid, @Depart_Quota, @Depart_Quantity, @Depart_Money, @Depart_PlanQuantity, @Depart_PlanMoney;
													end -- while @@fetch_status = 0

												close crDepartPlan;
												deallocate crDepartPlan;									
											END
										ELSE
											BEGIN
												INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
													Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
													Plan_Quantity, Plan_AllPrice, PlanDescription )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													NULL, 0, 0, 0, 
													@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
													@Customer_PlanQuantity, 
													@Customer_PlanMoney, 'для клиента нет разбивки плана по подразделениям' 
													);										
											END
									END																																																																																																																													
								ELSE
									BEGIN
										INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
											Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
											Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
											Plan_Quantity, Plan_AllPrice, PlanDescription )
										VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
											NULL, 0, 0, 0, 
											@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
											@Customer_PlanQuantity, 
											@Customer_PlanMoney, 'для клиента не назначены подразделения' 
											);										
									END						

								fetch next from crCustomerPlan into @Customer_Guid, @Customer_Quota, @Customer_Quantity, @Customer_Money, @Customer_PlanQuantity, @Customer_PlanMoney;
							end -- while @@fetch_status = 0

						close crCustomerPlan;
						deallocate crCustomerPlan;
					END
				ELSE
					BEGIN
						INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
							Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
							Plan_Quantity, Plan_AllPrice, PlanDescription )
						VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice,
							NULL, 0, 0, 0, 
							NULL, 0, 0, 0,
							@CalcPlan_Quantity, @CalcPlan_AllPrice, 'для марки и группы нет разбивки плана по клиентам' );
					END

				fetch next from crPlanItemDepartCustomer into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanItemDepartCustomer;
		deallocate crPlanItemDepartCustomer;

		-- Группировка по предельно допустимому минимуму
		DECLARE @MinQty int = 100;

		UPDATE #PlanItemDepartCustomer SET PlanDescription = '' WHERE PlanDescription IS NULL;

		EXEC [dbo].[usp_CalcPlanDepartCustomerProductSubType_Group] @MinQty = @MinQty,  @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;


		--SELECT * FROM #PlanItemDepartCustomerGroup
		--ORDER BY Owner_Guid, PartType_Guid, Customer_Guid, Depart_Guid;

		-- итоговый план по марке-группе-клиенту-подразделению-подгруппе

		CREATE TABLE #tmpPlanItemDepartCustomer(  
			Depart_Guid uniqueidentifier, Customer_Guid uniqueidentifier,  
			Plan_Quantity float, Plan_AllPrice money, PlanReserv_Quantity float, PlanReserv_AllPrice money  );

		DECLARE @Plan_Quantity_CustomerDepart float;	
		DECLARE @Plan_AllPrice_CustomerDepart money;
		DECLARE @Plan_Quantity_PartSubType float;	
		DECLARE @Plan_AllPrice_PartSubType money;
		DECLARE @Plan_Quantity_Ost float;	
		DECLARE @Plan_AllPrice_Ost money;
		DECLARE @tmpAllPrice money;
		DECLARE @CustomerDepartCount int;
		DECLARE @RecordNum int;
		DECLARE @PartSubType_Guid D_GUID;

		DECLARE crPlanTotal CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanTotal;
		fetch next from crPlanTotal into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				DELETE FROM #tmpPlanItemDepartCustomer;

				INSERT INTO #tmpPlanItemDepartCustomer( Depart_Guid, Customer_Guid,  
					Plan_Quantity, Plan_AllPrice, PlanReserv_Quantity, PlanReserv_AllPrice )
				SELECT Depart_Guid, Customer_Guid, Plan_Quantity, Plan_AllPrice, 0, 0
				FROM #PlanItemDepartCustomerGroup
				WHERE  Owner_Guid = Owner_Guid AND PartType_Guid = @PartType_Guid;

				DECLARE crPartSubTypePlan CURSOR FOR SELECT PartSubType_Guid, Plan_Quantity,	Plan_AllPrice
				FROM #PlanItemPartSubType
				WHERE  Owner_Guid = @Owner_Guid AND PartType_Guid = @PartType_Guid
				ORDER BY Plan_Quantity DESC;
				OPEN crPartSubTypePlan;
				fetch next from crPartSubTypePlan into @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType;
				while @@fetch_status = 0
					begin
						
						DECLARE crCustomerDepart CURSOR FOR SELECT Depart_Guid, Customer_Guid,  
							Plan_Quantity, Plan_AllPrice, ( Plan_Quantity - PlanReserv_Quantity  ), 
							( Plan_AllPrice - PlanReserv_AllPrice )
						FROM #tmpPlanItemDepartCustomer
						WHERE ( ( Plan_Quantity - PlanReserv_Quantity ) > 0 )
							AND ( ( Plan_AllPrice - PlanReserv_AllPrice ) > 0 )
						ORDER BY ( Plan_Quantity - PlanReserv_Quantity ) DESC;
						OPEN crCustomerDepart;
						fetch next from crCustomerDepart into @Depart_Guid, @Customer_Guid, @Plan_Quantity_CustomerDepart, @Plan_AllPrice_CustomerDepart, 
							@Plan_Quantity_Ost, @Plan_AllPrice_Ost;
						while @@fetch_status = 0
							begin
								IF( ( @Plan_Quantity_Ost IS NOT NULL ) AND ( @Plan_Quantity_Ost > 1 ) AND 
										--( @Plan_AllPrice_Ost IS NOT NULL ) AND ( @Plan_AllPrice_Ost > 0 ) AND 
										( @Plan_Quantity_PartSubType > 0 )  )
									BEGIN
										IF( @Plan_Quantity_Ost >= @Plan_Quantity_PartSubType ) 
											--	( @Plan_AllPrice_Ost >= @Plan_AllPrice_PartSubType ) )
											BEGIN
												UPDATE #tmpPlanItemDepartCustomer 
													SET PlanReserv_Quantity = ( PlanReserv_Quantity + @Plan_Quantity_PartSubType ), 
															PlanReserv_AllPrice = ( PlanReserv_AllPrice + @Plan_AllPrice_PartSubType )
												WHERE Depart_Guid = @Depart_Guid AND Customer_Guid = @Customer_Guid;

												INSERT INTO #PlanItemDepartCustomerPartSubtype( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Customer_Guid, PartSubType_Guid, Plan_Quantity, Plan_AllPrice )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													@Depart_Guid, @Customer_Guid, @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType );

												SET @Plan_Quantity_PartSubType = 0;
												SET @Plan_AllPrice_PartSubType = 0;
											END
										ELSE
											BEGIN
												SET @tmpAllPrice = 0;
												SET @tmpAllPrice =  (( ( 10000 * @Plan_Quantity_Ost)/@Plan_Quantity_PartSubType ) * 0.0001 ) * @Plan_AllPrice_PartSubType;

												UPDATE #tmpPlanItemDepartCustomer 
													SET PlanReserv_Quantity = ( PlanReserv_Quantity + @Plan_Quantity_Ost ), --Plan_Quantity, 
															PlanReserv_AllPrice =  ( PlanReserv_AllPrice + @tmpAllPrice ) 
												WHERE Depart_Guid = @Depart_Guid AND Customer_Guid = @Customer_Guid;

												INSERT INTO #PlanItemDepartCustomerPartSubtype( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Customer_Guid, PartSubType_Guid, Plan_Quantity, Plan_AllPrice )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													@Depart_Guid, @Customer_Guid, @PartSubType_Guid, @Plan_Quantity_Ost, @tmpAllPrice );

												SET @Plan_Quantity_PartSubType = ( @Plan_Quantity_PartSubType - @Plan_Quantity_Ost );
												SET @Plan_AllPrice_PartSubType = ( @Plan_AllPrice_PartSubType - @tmpAllPrice );
											END

									END

								fetch next from crCustomerDepart into @Depart_Guid, @Customer_Guid, @Plan_Quantity_CustomerDepart, @Plan_AllPrice_CustomerDepart, 
							@Plan_Quantity_Ost, @Plan_AllPrice_Ost;
							end -- while @@fetch_status = 0

						close crCustomerDepart;
						deallocate crCustomerDepart;

						fetch next from crPartSubTypePlan into @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType;
					end -- while @@fetch_status = 0

				close crPartSubTypePlan;
				deallocate crPartSubTypePlan;

				fetch next from crPlanTotal into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanTotal;
		deallocate crPlanTotal;

		DELETE FROM #PlanItemDepartCustomerPartSubtype WHERE Plan_Quantity  < 1;
		UPDATE #PlanItemDepartCustomerPartSubtype SET Plan_Quantity = ROUND( Plan_Quantity, 0 );

		-- корректировка погрешности округления
		DECLARE @DiffPlan_Quantity float;
		DECLARE @DiffPlan_AllPrice money;

		DECLARE crPlanItem CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanItem;
		fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				SET @DiffPlan_Quantity = 0;
				SET @DiffPlan_AllPrice = 0;

				SELECT @DiffPlan_Quantity = ( @CalcPlan_Quantity - SUM( Plan_Quantity ) ), 
						 	 @DiffPlan_AllPrice = ( @CalcPlan_AllPrice - SUM( Plan_AllPrice ) )
				FROM #PlanItemDepartCustomerPartSubtype
				WHERE Owner_Guid = @Owner_Guid AND  PartType_Guid = @PartType_Guid;

				SET @Depart_Guid = NULL;
				SET @Customer_Guid = NULL;
				SET @PartSubType_Guid = NULL;

				SELECT TOP 1 @Depart_Guid = Depart_Guid, @Customer_Guid = Customer_Guid, @PartSubType_Guid = PartSubType_Guid
								FROM #PlanItemDepartCustomerPartSubtype
								WHERE Owner_Guid = @Owner_Guid AND  PartType_Guid = @PartType_Guid
									AND ( ( Plan_Quantity + @DiffPlan_Quantity )  > 0 )
									AND ( ( Plan_AllPrice + @DiffPlan_AllPrice ) > 0 );					

				IF( ( @Depart_Guid IS NOT NULL ) AND ( @Customer_Guid IS NOT NULL ) AND ( @PartSubType_Guid IS NOT NULL ) )
					BEGIN
						UPDATE #PlanItemDepartCustomerPartSubtype 
						SET Plan_Quantity = ( Plan_Quantity + @DiffPlan_Quantity ), 
								Plan_AllPrice = ( Plan_AllPrice + @DiffPlan_AllPrice )			
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid
							AND Depart_Guid = @Depart_Guid
							AND Customer_Guid = @Customer_Guid
							AND PartSubType_Guid = @PartSubType_Guid
									AND ( ( Plan_Quantity + @DiffPlan_Quantity )  > 0 )
									AND ( ( Plan_AllPrice + @DiffPlan_AllPrice ) > 0 );					
					END

				fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		SELECT * FROM #PlanItemDepartCustomerPartSubtype
		ORDER BY Owner_Guid, PartType_Guid, Customer_Guid, Depart_Guid, PartSubType_Guid;

		DROP TABLE #tmpPlanItemDepartCustomer;
		DROP TABLE #PlanItemDepartCustomerPartSubtype;
		DROP TABLE #PlanItemDepartCustomerGroup;
		DROP TABLE #PlanItemDepartCustomer;
		DROP TABLE #PlanItemPartSubType;
		DROP TABLE #PlanItemCustomer;
		DROP TABLE #PlanItemDepart;
		DROP TABLE #ItemQuotaDepart;
		DROP TABLE #CalcPlanItem;

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

-- Расчёт плана продаж азрезе подразделений, клиентов и подгрупп
--
-- Входные параметры:

--		@SalesPlanQuota_Guid						УИ расчёта
--		@CalcPlan_Guid									УИ расчёта "Марка-Группа"
--		@MonthId												номер месяца
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_CalcPlanDepartCustomerProductSubType] 
	@SalesPlanQuota_Guid				D_GUID,
	@CalcPlan_Guid							D_GUID,
	@MonthId										D_ID,

  @ERROR_NUM									int output,
  @ERROR_MES									nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		-- план на указанный месяц по маркам и группам
		CREATE TABLE #CalcPlanItem( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money );
		INSERT INTO #CalcPlanItem(  Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice )
		SELECT Owner_Guid, PartType_Guid, SUM( CalcPlan_Quantity ), SUM( CalcPlan_AllPrice )
		FROM [dbo].[T_CalcPlanItem]
		WHERE [CalcPlan_Guid] = @CalcPlan_Guid
			AND [MonthId] = @MonthId
			AND [CalcPlan_Quantity] > 0
		GROUP BY Owner_Guid, PartType_Guid;

		-- план разбивается по подразделениям
		CREATE TABLE #PlanItemDepart( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		-- план разбивается по клиентам
		CREATE TABLE #PlanItemCustomer( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Customer_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		-- план разбивается по подгруппам
		CREATE TABLE #PlanItemPartSubType( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			PartSubType_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, SalesPlanQuotaItemDecode_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money );

		CREATE TABLE #PlanItemDepartCustomer( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, Depart_Quantity float, Depart_Money money, Depart_Quota numeric( 18, 5 ), 
			Customer_Guid uniqueidentifier, Customer_Quantity float, Customer_Money money, Customer_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money, PlanDescription nvarchar(128)  );

		CREATE TABLE #PlanItemDepartCustomerGroup( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, Depart_Quantity float, Depart_Money money, Depart_Quota numeric( 18, 5 ), 
			Customer_Guid uniqueidentifier, Customer_Quantity float, Customer_Money money, Customer_Quota numeric( 18, 5 ), 
			Plan_Quantity float, Plan_AllPrice money, PlanDescription nvarchar(128)  );

		CREATE TABLE #PlanItemDepartCustomerPartSubtype( Owner_Guid uniqueidentifier, PartType_Guid uniqueidentifier, CalcPlan_Quantity float, CalcPlan_AllPrice money, 
			Depart_Guid uniqueidentifier, 
			Customer_Guid uniqueidentifier, 
			PartSubType_Guid uniqueidentifier, 
			Plan_Quantity float, Plan_AllPrice money );

		CREATE TABLE #ItemQuotaDepart( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );
		CREATE TABLE #ItemQuotaCustomer( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );
		CREATE TABLE #ItemQuotaPartSubType( Object_Guid uniqueidentifier, SalesPlanQuotaItemDecode_Quantity float, SalesPlanQuotaItemDecode_Money money, Quota numeric( 18, 5 ) );

		DECLARE @Owner_Guid uniqueidentifier;
		DECLARE @PartType_Guid uniqueidentifier;
		DECLARE @CalcPlan_Quantity float; 
		DECLARE @CalcPlan_AllPrice money;
		DECLARE @Depart_Guid uniqueidentifier; 
		DECLARE @SalesPlanQuotaItemDecode_Quantity float; 
		DECLARE @SalesPlanQuotaItemDecode_Money money; 
		DECLARE @SalesPlanQuotaItemDecode_Quota numeric( 18, 5 );
		DECLARE @AllItemDecode_Quota numeric( 18, 5 );
		DECLARE @Plan_Quantity float; 
		DECLARE @Plan_AllPrice money;
		DECLARE @CountDepart_Guid int;
		DECLARE @SalesPlanQuotaItem_Guid uniqueidentifier;

		DECLARE crPlanItem CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanItem;
		fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				SET @SalesPlanQuotaItem_Guid = NULL;

				SELECT @SalesPlanQuotaItem_Guid = [SalesPlanQuotaItem_Guid] FROM [dbo].[T_SalesPlanQuotaItem] 
				WHERE [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid
					AND [ProductOwner_Guid] = @Owner_Guid
					AND [ProductType_Guid] = @PartType_Guid;

				IF( @SalesPlanQuotaItem_Guid IS NOT NULL )
					BEGIN
						-- сумма долей подразделений в рамках записи "Марка-Группа" 
						SET @AllItemDecode_Quota = 0;
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;
						IF( @AllItemDecode_Quota IS NULL ) SET @AllItemDecode_Quota = 0;

						-- если сумма < 1, то необходимо запросить дополнительные подзразделения
						-- если же сумма рана нулю, то запрашиваются все подразделения команды, которая работает с указанной маркой

						DELETE FROM #ItemQuotaDepart;

						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								-- выборка подразделений команды, работающей с маркой
								INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Depart_Guid], 0, 0, 10000/( COUNT(Depart_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_Depart]
								WHERE [DepartTeam_Guid] IS NOT NULL
									AND [DepartTeam_Guid] IN ( SELECT TOP 1 [DepartTeam_Guid] FROM [dbo].[T_DepartTeamOwner] WHERE [Owner_Guid] = @Owner_Guid );
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Depart_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money,  ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [Depart_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodeDepart] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid ;
								
							END
						
						INSERT INTO #PlanItemDepart( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaDepart; 

						-- сумма долей клиентов в рамках записи "Марка-Группа" 
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;

						-- если сумма < 1, то необходимо запросить дополнительных клиентов 
						-- если же сумма рана нулю, то запрашиваются все клиенты, которые работают с указанной маркой (через подразделение)
						DELETE FROM #ItemQuotaCustomer;

						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Customer_Guid], 0, 0, 10000/( COUNT(Customer_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_Customer]
								WHERE [dbo].[T_Customer].CustomerActiveType_Guid = 'CDD98D87-378A-48F1-BF46-ED7FE96BBD68'
									AND [Customer_Guid] IN ( SELECT [Customer_Guid] FROM [dbo].[T_CustomerDepart] 
																						WHERE [Depart_Guid] IN ( SELECT [Depart_Guid] FROM #PlanItemDepart
																																			WHERE Owner_Guid = @Owner_Guid AND PartType_Guid =  @PartType_Guid ) );
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [Customer_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money,  ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [Customer_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodeCustomer] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid ;
								
							END
						
						INSERT INTO #PlanItemCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaCustomer; 

						-- сумма долей подгрупп в рамках записи "Марка-Группа" 
						SELECT @AllItemDecode_Quota = SUM( [SalesPlanQuotaItemDecode_Quota] ) FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
						WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;

						-- если сумма < 1, то необходимо запросить дополнительные подгруппы 
						-- если же сумма рана нулю, то запрашиваются все подгруппы, входящие в указанную марку
						DELETE FROM #ItemQuotaPartSubType;

						IF( @AllItemDecode_Quota IS NULL ) SET @AllItemDecode_Quota = 0;
						IF( @AllItemDecode_Quota > 1 ) SET @AllItemDecode_Quota = 1;
						IF( @AllItemDecode_Quota = 0 )
							BEGIN
								IF EXISTS( SELECT [PartSubType_Guid]	FROM [dbo].[T_PartSubType]
														WHERE [Partsubtype_IsActive] = 1									
															AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																												WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																									WHERE Owner_Guid = @Owner_Guid ) )
															AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																												WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																									WHERE [Parttype_Guid] = @PartType_Guid ) ) )
									BEGIN
										INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
										SELECT [PartSubType_Guid], 0, 0, 10000/( COUNT(PartSubType_Guid)  OVER( )) * 0.0001
										FROM [dbo].[T_PartSubType]
										WHERE [Partsubtype_IsActive] = 1									
											AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																								WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																					WHERE Owner_Guid = @Owner_Guid ) )
											AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																								WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																					WHERE [Parttype_Guid] = @PartType_Guid ) );
									END
								ELSE
									BEGIN
										PRINT 'Не найдено:'
										PRINT @Owner_Guid;
										PRINT @PartType_Guid;
									END
							END
						ELSE IF( @AllItemDecode_Quota <= 1 )
							BEGIN
								INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [PartSubType_Guid], SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, ( [SalesPlanQuotaItemDecode_Quota]  + ( 10000 * ( 1 - @AllItemDecode_Quota )/( COUNT( [PartSubType_Guid] ) OVER( ) ) * 0.0001 ) ) 
								FROM [dbo].[T_SalesPlanQuotaItemDecodePartSubType] 
								WHERE [SalesPlanQuotaItem_Guid] = @SalesPlanQuotaItem_Guid;
								
							END
						
						INSERT INTO #PlanItemPartSubType( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaPartSubType; 
					END
				ELSE
					BEGIN
						-- в долях клиентов сочетание марки и группы не найдено
						DELETE FROM #ItemQuotaCustomer;
						INSERT INTO #ItemQuotaCustomer( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
						SELECT [Customer_Guid], 0, 0, 10000/( COUNT(Customer_Guid)  OVER( )) * 0.0001
						FROM [dbo].[T_Customer]
						WHERE [dbo].[T_Customer].CustomerActiveType_Guid = 'CDD98D87-378A-48F1-BF46-ED7FE96BBD68'
							AND [Customer_Guid] IN ( SELECT [Customer_Guid] FROM [dbo].[T_CustomerDepart] 
																				WHERE [Depart_Guid] IN ( SELECT [Depart_Guid] FROM #PlanItemDepart
																																	WHERE Owner_Guid = @Owner_Guid AND PartType_Guid =  @PartType_Guid ) );
						INSERT INTO #PlanItemCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Customer_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaCustomer; 

						DELETE FROM #ItemQuotaDepart;
						INSERT INTO #ItemQuotaDepart( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
						SELECT [Depart_Guid], 0, 0, 10000/( COUNT(Depart_Guid)  OVER( )) * 0.0001
						FROM [dbo].[T_Depart]
						WHERE [DepartTeam_Guid] IS NOT NULL
							AND [DepartTeam_Guid] IN ( SELECT TOP 1 [DepartTeam_Guid] FROM [dbo].[T_DepartTeamOwner] WHERE [Owner_Guid] = @Owner_Guid );

						INSERT INTO #PlanItemDepart( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
						SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
							Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
						FROM 	#ItemQuotaDepart; 


						-- в долях товарных подгрупп сочетание марки и группы не найдено
						DELETE FROM  #ItemQuotaPartSubType;
						IF EXISTS( SELECT [PartSubType_Guid]	FROM [dbo].[T_PartSubType]
												WHERE --[Partsubtype_IsActive] = 1 AND								
													 [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																										WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																							WHERE Owner_Guid = @Owner_Guid ) )
													AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																										WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																							WHERE [Parttype_Guid] = @PartType_Guid ) ) )
							BEGIN
								PRINT 'Поймал!'
								INSERT INTO #ItemQuotaPartSubType( Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota )
								SELECT [PartSubType_Guid], 0, 0, 10000/( COUNT(PartSubType_Guid)  OVER( )) * 0.0001
								FROM [dbo].[T_PartSubType]
								WHERE --[Partsubtype_IsActive] = 1		AND 							
									 [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																						WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsOwner]
																																			WHERE Owner_Guid = @Owner_Guid ) )
									AND [Partsubtype_Guid] IN ( SELECT [Partsubtype_Guid]  FROM [dbo].[T_PartsPartsubtype]
																						WHERE [Parts_Guid] IN ( SELECT [Parts_Guid] FROM [dbo].[T_PartsParttype]
																																			WHERE [Parttype_Guid] = @PartType_Guid ) );

								INSERT INTO #PlanItemPartSubType( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
									PartSubType_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, SalesPlanQuotaItemDecode_Quota )
								SELECT @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
									Object_Guid, SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Quota
								FROM 	#ItemQuotaPartSubType; 

							END
					END
				fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanItem;
		deallocate crPlanItem;

		UPDATE #PlanItemDepart SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		UPDATE #PlanItemCustomer SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		UPDATE #PlanItemPartSubType SET Plan_Quantity = CalcPlan_Quantity * SalesPlanQuotaItemDecode_Quota , 
			Plan_AllPrice = CalcPlan_AllPrice * SalesPlanQuotaItemDecode_Quota;

		SELECT T_Owner.Owner_Name, T_Parttype.Parttype_Name, #CalcPlanItem.CalcPlan_Quantity, #CalcPlanItem.CalcPlan_AllPrice
		FROM #CalcPlanItem INNER JOIN T_Owner ON #CalcPlanItem.Owner_Guid = T_Owner.Owner_Guid 
			INNER JOIN T_Parttype ON #CalcPlanItem.PartType_Guid = T_Parttype.Parttype_Guid
		ORDER BY T_Owner.Owner_Name, T_Parttype.Parttype_Name;

		--SELECT * FROM #PlanItemDepart;
		--SELECT * FROM #PlanItemCustomer;
		--SELECT * FROM #PlanItemPartSubType;

		DECLARE @Customer_Guid D_GUID;
		DECLARE @Customer_Quota numeric(18, 5);
		DECLARE @Customer_Quantity float;
		DECLARE @Customer_Money money;
		DECLARE @Customer_PlanQuantity float;
		DECLARE @Customer_PlanMoney money;

		DECLARE @Depart_Quota numeric(18, 5);
		DECLARE @Depart_Quantity float;
		DECLARE @Depart_Money money;
		DECLARE @Depart_PlanQuantity float;
		DECLARE @Depart_PlanMoney money;
		DECLARE @Depart_SumQuota numeric(18, 5);
		DECLARE @PlanDescription nvarchar(128);

		DECLARE crPlanItemDepartCustomer CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanItemDepartCustomer;
		fetch next from crPlanItemDepartCustomer into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				
				-- для каждой группы записей "Товарная марка - Товарная группа" 
				-- запрашивается план по клиентам из #PlanItemCustomer,  а затем для каждого клиента необходимо получить список подразделений, с которыми он работает и распределить план клиента по этим подразделениям
				-- привязка клиента к подразделениям находится в [dbo].[T_CustomerDepart], а план по подразделениям в #PlanItemDepart
				IF EXISTS( SELECT Customer_Guid FROM #PlanItemCustomer	WHERE Owner_Guid = @Owner_Guid 	AND PartType_Guid = @PartType_Guid )
					BEGIN
						DECLARE crCustomerPlan CURSOR FOR SELECT	Customer_Guid, SalesPlanQuotaItemDecode_Quota, 
							SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Plan_Quantity, Plan_AllPrice
						FROM #PlanItemCustomer
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid;
						OPEN crCustomerPlan;
						fetch next from crCustomerPlan into @Customer_Guid, @Customer_Quota, @Customer_Quantity, @Customer_Money, @Customer_PlanQuantity, @Customer_PlanMoney;
						while @@fetch_status = 0
							begin
								SET @Depart_SumQuota = 0;
								IF EXISTS( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  )
									BEGIN
										-- клиенту назначены подразделения
										SELECT @Depart_SumQuota = SUM( SalesPlanQuotaItemDecode_Quota ) 
										FROM #PlanItemDepart
										WHERE Owner_Guid = @Owner_Guid 
											AND PartType_Guid = @PartType_Guid
											AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  ); 
										IF( @Depart_SumQuota IS NULL ) SET @Depart_SumQuota = 0;
										
										IF EXISTS( SELECT Depart_Guid FROM #PlanItemDepart
																WHERE Owner_Guid = @Owner_Guid 
																	AND PartType_Guid = @PartType_Guid
																	AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  ) )
											BEGIN
												DECLARE crDepartPlan CURSOR FOR SELECT  Depart_Guid, SalesPlanQuotaItemDecode_Quota, 
													SalesPlanQuotaItemDecode_Quantity, SalesPlanQuotaItemDecode_Money, Plan_Quantity, Plan_AllPrice
												FROM #PlanItemDepart
												WHERE Owner_Guid = @Owner_Guid 
													AND PartType_Guid = @PartType_Guid
													AND Depart_Guid IN ( SELECT Depart_Guid FROM [dbo].[T_CustomerDepart] WHERE [Customer_Guid] = @Customer_Guid  );
												OPEN crDepartPlan;
												fetch next from crDepartPlan into @Depart_Guid, @Depart_Quota, @Depart_Quantity, @Depart_Money, @Depart_PlanQuantity, @Depart_PlanMoney;
												while @@fetch_status = 0
													begin
														-- вставка в итоговую таблицу
														INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
															Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
															Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
															Plan_Quantity, Plan_AllPrice )
														VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
															@Depart_Guid, @Depart_Quantity, @Depart_Money, @Depart_Quota, 
															@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
															( ( @Depart_Quota/@Depart_SumQuota ) * @Customer_PlanQuantity ), 
															( ( @Depart_Quota/@Depart_SumQuota ) * @Customer_PlanMoney ) 
															);	

														fetch next from crDepartPlan into @Depart_Guid, @Depart_Quota, @Depart_Quantity, @Depart_Money, @Depart_PlanQuantity, @Depart_PlanMoney;
													end -- while @@fetch_status = 0

												close crDepartPlan;
												deallocate crDepartPlan;									
											END
										ELSE
											BEGIN
												INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
													Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
													Plan_Quantity, Plan_AllPrice, PlanDescription )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													NULL, 0, 0, 0, 
													@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
													@Customer_PlanQuantity, 
													@Customer_PlanMoney, 'для клиента нет разбивки плана по подразделениям' 
													);										
											END
									END																																																																																																																													
								ELSE
									BEGIN
										INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
											Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
											Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
											Plan_Quantity, Plan_AllPrice, PlanDescription )
										VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
											NULL, 0, 0, 0, 
											@Customer_Guid, @Customer_Quantity, @Customer_Money,  @Customer_Quota, 
											@Customer_PlanQuantity, 
											@Customer_PlanMoney, 'для клиента не назначены подразделения' 
											);										
									END						

								fetch next from crCustomerPlan into @Customer_Guid, @Customer_Quota, @Customer_Quantity, @Customer_Money, @Customer_PlanQuantity, @Customer_PlanMoney;
							end -- while @@fetch_status = 0

						close crCustomerPlan;
						deallocate crCustomerPlan;
					END
				ELSE
					BEGIN
						INSERT INTO #PlanItemDepartCustomer( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
							Depart_Guid, Depart_Quantity, Depart_Money, Depart_Quota, 
							Customer_Guid, Customer_Quantity, Customer_Money, Customer_Quota, 
							Plan_Quantity, Plan_AllPrice, PlanDescription )
						VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice,
							NULL, 0, 0, 0, 
							NULL, 0, 0, 0,
							@CalcPlan_Quantity, @CalcPlan_AllPrice, 'для марки и группы нет разбивки плана по клиентам' );
					END

				fetch next from crPlanItemDepartCustomer into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanItemDepartCustomer;
		deallocate crPlanItemDepartCustomer;

		-- Группировка по предельно допустимому минимуму
		DECLARE @MinQty int = 100;

		UPDATE #PlanItemDepartCustomer SET PlanDescription = '' WHERE PlanDescription IS NULL;

		EXEC [dbo].[usp_CalcPlanDepartCustomerProductSubType_Group] @MinQty = @MinQty,  @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;


		--SELECT * FROM #PlanItemDepartCustomerGroup
		--ORDER BY Owner_Guid, PartType_Guid, Customer_Guid, Depart_Guid;

		-- итоговый план по марке-группе-клиенту-подразделению-подгруппе

		CREATE TABLE #tmpPlanItemDepartCustomer(  
			Depart_Guid uniqueidentifier, Customer_Guid uniqueidentifier,  
			Plan_Quantity float, Plan_AllPrice money, PlanReserv_Quantity float, PlanReserv_AllPrice money  );

		DECLARE @Plan_Quantity_CustomerDepart float;	
		DECLARE @Plan_AllPrice_CustomerDepart money;
		DECLARE @Plan_Quantity_PartSubType float;	
		DECLARE @Plan_AllPrice_PartSubType money;
		DECLARE @Plan_Quantity_Ost float;	
		DECLARE @Plan_AllPrice_Ost money;
		DECLARE @tmpAllPrice money;
		DECLARE @CustomerDepartCount int;
		DECLARE @RecordNum int;
		DECLARE @PartSubType_Guid D_GUID;

		DECLARE crPlanTotal CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanTotal;
		fetch next from crPlanTotal into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				DELETE FROM #tmpPlanItemDepartCustomer;

				INSERT INTO #tmpPlanItemDepartCustomer( Depart_Guid, Customer_Guid,  
					Plan_Quantity, Plan_AllPrice, PlanReserv_Quantity, PlanReserv_AllPrice )
				SELECT Depart_Guid, Customer_Guid, Plan_Quantity, Plan_AllPrice, 0, 0
				FROM #PlanItemDepartCustomerGroup
				WHERE  Owner_Guid = Owner_Guid AND PartType_Guid = @PartType_Guid;

				DECLARE crPartSubTypePlan CURSOR FOR SELECT PartSubType_Guid, Plan_Quantity,	Plan_AllPrice
				FROM #PlanItemPartSubType
				WHERE  Owner_Guid = @Owner_Guid AND PartType_Guid = @PartType_Guid
				ORDER BY Plan_Quantity DESC;
				OPEN crPartSubTypePlan;
				fetch next from crPartSubTypePlan into @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType;
				while @@fetch_status = 0
					begin
						
						DECLARE crCustomerDepart CURSOR FOR SELECT Depart_Guid, Customer_Guid,  
							Plan_Quantity, Plan_AllPrice, ( Plan_Quantity - PlanReserv_Quantity  ), 
							( Plan_AllPrice - PlanReserv_AllPrice )
						FROM #tmpPlanItemDepartCustomer
						WHERE ( ( Plan_Quantity - PlanReserv_Quantity ) > 0 )
							AND ( ( Plan_AllPrice - PlanReserv_AllPrice ) > 0 )
						ORDER BY ( Plan_Quantity - PlanReserv_Quantity ) DESC;
						OPEN crCustomerDepart;
						fetch next from crCustomerDepart into @Depart_Guid, @Customer_Guid, @Plan_Quantity_CustomerDepart, @Plan_AllPrice_CustomerDepart, 
							@Plan_Quantity_Ost, @Plan_AllPrice_Ost;
						while @@fetch_status = 0
							begin
								IF( ( @Plan_Quantity_Ost IS NOT NULL ) AND ( @Plan_Quantity_Ost > 1 ) AND 
										--( @Plan_AllPrice_Ost IS NOT NULL ) AND ( @Plan_AllPrice_Ost > 0 ) AND 
										( @Plan_Quantity_PartSubType > 0 )  )
									BEGIN
										IF( @Plan_Quantity_Ost >= @Plan_Quantity_PartSubType ) 
											--	( @Plan_AllPrice_Ost >= @Plan_AllPrice_PartSubType ) )
											BEGIN
												UPDATE #tmpPlanItemDepartCustomer 
													SET PlanReserv_Quantity = ( PlanReserv_Quantity + @Plan_Quantity_PartSubType ), 
															PlanReserv_AllPrice = ( PlanReserv_AllPrice + @Plan_AllPrice_PartSubType )
												WHERE Depart_Guid = @Depart_Guid AND Customer_Guid = @Customer_Guid;

												INSERT INTO #PlanItemDepartCustomerPartSubtype( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Customer_Guid, PartSubType_Guid, Plan_Quantity, Plan_AllPrice )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													@Depart_Guid, @Customer_Guid, @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType );

												SET @Plan_Quantity_PartSubType = 0;
												SET @Plan_AllPrice_PartSubType = 0;
											END
										ELSE
											BEGIN
												SET @tmpAllPrice = 0;
												SET @tmpAllPrice =  (( ( 10000 * @Plan_Quantity_Ost)/@Plan_Quantity_PartSubType ) * 0.0001 ) * @Plan_AllPrice_PartSubType;

												UPDATE #tmpPlanItemDepartCustomer 
													SET PlanReserv_Quantity = ( PlanReserv_Quantity + @Plan_Quantity_Ost ), --Plan_Quantity, 
															PlanReserv_AllPrice =  ( PlanReserv_AllPrice + @tmpAllPrice ) 
												WHERE Depart_Guid = @Depart_Guid AND Customer_Guid = @Customer_Guid;

												INSERT INTO #PlanItemDepartCustomerPartSubtype( Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
													Depart_Guid, Customer_Guid, PartSubType_Guid, Plan_Quantity, Plan_AllPrice )
												VALUES( @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice, 
													@Depart_Guid, @Customer_Guid, @PartSubType_Guid, @Plan_Quantity_Ost, @tmpAllPrice );

												SET @Plan_Quantity_PartSubType = ( @Plan_Quantity_PartSubType - @Plan_Quantity_Ost );
												SET @Plan_AllPrice_PartSubType = ( @Plan_AllPrice_PartSubType - @tmpAllPrice );
											END

									END

								fetch next from crCustomerDepart into @Depart_Guid, @Customer_Guid, @Plan_Quantity_CustomerDepart, @Plan_AllPrice_CustomerDepart, 
							@Plan_Quantity_Ost, @Plan_AllPrice_Ost;
							end -- while @@fetch_status = 0

						close crCustomerDepart;
						deallocate crCustomerDepart;

						fetch next from crPartSubTypePlan into @PartSubType_Guid, @Plan_Quantity_PartSubType, @Plan_AllPrice_PartSubType;
					end -- while @@fetch_status = 0

				close crPartSubTypePlan;
				deallocate crPartSubTypePlan;

				fetch next from crPlanTotal into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0

		close crPlanTotal;
		deallocate crPlanTotal;

		DELETE FROM #PlanItemDepartCustomerPartSubtype WHERE Plan_Quantity  < 1;
		UPDATE #PlanItemDepartCustomerPartSubtype SET Plan_Quantity = ROUND( Plan_Quantity, 0 );

		-- корректировка погрешности округления
		DECLARE @DiffPlan_Quantity float;
		DECLARE @DiffPlan_AllPrice money;

		DECLARE crPlanItem CURSOR FOR SELECT Owner_Guid, PartType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice
		FROM #CalcPlanItem
		ORDER BY Owner_Guid, PartType_Guid;
		OPEN crPlanItem;
		fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
		while @@fetch_status = 0
			begin
				SET @DiffPlan_Quantity = 0;
				SET @DiffPlan_AllPrice = 0;

				SELECT @DiffPlan_Quantity = ( @CalcPlan_Quantity - SUM( Plan_Quantity ) ), 
						 	 @DiffPlan_AllPrice = ( @CalcPlan_AllPrice - SUM( Plan_AllPrice ) )
				FROM #PlanItemDepartCustomerPartSubtype
				WHERE Owner_Guid = @Owner_Guid AND  PartType_Guid = @PartType_Guid;

				SET @Depart_Guid = NULL;
				SET @Customer_Guid = NULL;
				SET @PartSubType_Guid = NULL;

				SELECT TOP 1 @Depart_Guid = Depart_Guid, @Customer_Guid = Customer_Guid, @PartSubType_Guid = PartSubType_Guid
								FROM #PlanItemDepartCustomerPartSubtype
								WHERE Owner_Guid = @Owner_Guid AND  PartType_Guid = @PartType_Guid
									AND ( ( Plan_Quantity + @DiffPlan_Quantity )  > 0 )
									AND ( ( Plan_AllPrice + @DiffPlan_AllPrice ) > 0 );					

				IF( ( @Depart_Guid IS NOT NULL ) AND ( @Customer_Guid IS NOT NULL ) AND ( @PartSubType_Guid IS NOT NULL ) )
					BEGIN
						UPDATE #PlanItemDepartCustomerPartSubtype 
						SET Plan_Quantity = ( Plan_Quantity + @DiffPlan_Quantity ), 
								Plan_AllPrice = ( Plan_AllPrice + @DiffPlan_AllPrice )			
						WHERE Owner_Guid = @Owner_Guid 
							AND PartType_Guid = @PartType_Guid
							AND Depart_Guid = @Depart_Guid
							AND Customer_Guid = @Customer_Guid
							AND PartSubType_Guid = @PartSubType_Guid
									AND ( ( Plan_Quantity + @DiffPlan_Quantity )  > 0 )
									AND ( ( Plan_AllPrice + @DiffPlan_AllPrice ) > 0 );					
					END

				fetch next from crPlanItem into @Owner_Guid, @PartType_Guid, @CalcPlan_Quantity, @CalcPlan_AllPrice;
			end -- while @@fetch_status = 0
		close crPlanItem;
		deallocate crPlanItem;

		SELECT T_Owner.Owner_Name, T_Parttype.Parttype_Name, 
			#PlanItemDepartCustomerPartSubtype.CalcPlan_Quantity,
			#PlanItemDepartCustomerPartSubtype.CalcPlan_AllPrice,
			T_DepartTeam.DepartTeam_Name, 
			T_Depart.Depart_Code, T_Customer.Customer_Name, T_Partsubtype.Partsubtype_Name, 
			#PlanItemDepartCustomerPartSubtype.Plan_Quantity, 
			#PlanItemDepartCustomerPartSubtype.Plan_AllPrice 
		FROM #PlanItemDepartCustomerPartSubtype INNER JOIN T_Owner ON #PlanItemDepartCustomerPartSubtype.Owner_Guid = T_Owner.Owner_Guid 
			INNER JOIN T_Parttype ON #PlanItemDepartCustomerPartSubtype.PartType_Guid = T_Parttype.Parttype_Guid
			LEFT OUTER JOIN T_Partsubtype ON #PlanItemDepartCustomerPartSubtype.PartSubType_Guid = T_Partsubtype.Partsubtype_Guid
			LEFT OUTER JOIN T_Customer ON #PlanItemDepartCustomerPartSubtype.Customer_Guid = T_Customer.Customer_Guid
			LEFT OUTER JOIN T_Depart ON #PlanItemDepartCustomerPartSubtype.Depart_Guid = T_Depart.Depart_Guid
			LEFT OUTER JOIN T_DepartTeam ON T_Depart.DepartTeam_Guid = T_DepartTeam.DepartTeam_Guid
		ORDER BY  T_Owner.Owner_Name, T_Parttype.Parttype_Name, T_DepartTeam.DepartTeam_Name, 
			T_Depart.Depart_Code, T_Customer.Customer_Name, T_Partsubtype.Partsubtype_Name;

		DROP TABLE #tmpPlanItemDepartCustomer;
		DROP TABLE #PlanItemDepartCustomerPartSubtype;
		DROP TABLE #PlanItemDepartCustomerGroup;
		DROP TABLE #PlanItemDepartCustomer;
		DROP TABLE #PlanItemPartSubType;
		DROP TABLE #PlanItemCustomer;
		DROP TABLE #PlanItemDepart;
		DROP TABLE #ItemQuotaDepart;
		DROP TABLE #CalcPlanItem;

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
