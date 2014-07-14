USE [ERP_Mercury]
GO

-- таблица для "шапки" расчёта плана продаж по клиентам, подразделениям, подгруппам

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_CalcPlanByDepartCustomerSubType](
	[CalcPlanByDepartCustomerSubType_Guid]	[dbo].[D_GUID] NOT NULL,
	[CalcPlan_Guid]													[dbo].[D_GUID] NOT NULL,
	[SalesPlanQuota_Guid]										[dbo].[D_GUID] NOT NULL,

	[Plan_BeginDate]												[dbo].[D_DATE] NOT NULL,
	[Plan_EndDate]													[dbo].[D_DATE] NOT NULL,
	[Plan_Name]															[dbo].[D_NAME] NOT NULL,
	[Plan_Date]															[dbo].[D_DATE] NOT NULL,
	[Currency_Guid]													[dbo].[D_GUID] NOT NULL,
	[Plan_IsUseForReport]										[dbo].[D_YESNO] NOT NULL,
	[Plan_Description]											[dbo].[D_DESCRIPTION] NULL,

	[Record_Updated]												[dbo].[D_DATETIME] NULL,
	[Record_UserUpdated]										[dbo].[D_NAMESHORT] NULL,
 CONSTRAINT [PK_T_CalcPlanByDepartCustomerSubType] PRIMARY KEY CLUSTERED 
(
	[CalcPlanByDepartCustomerSubType_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubType_PeriodUseForReport] ON [dbo].[T_CalcPlanByDepartCustomerSubType]
(
	[Plan_BeginDate] ASC,
	[Plan_EndDate] ASC,
	[Plan_IsUseForReport] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubType]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubType_T_SalesPlanQuota] FOREIGN KEY([SalesPlanQuota_Guid])
REFERENCES [dbo].[T_SalesPlanQuota] ([SalesPlanQuota_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubType] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubType_T_SalesPlanQuota]
GO


ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubType]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubType_T_Currency] FOREIGN KEY([Currency_Guid])
REFERENCES [dbo].[T_Currency] ([Currency_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubType] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubType_T_Currency]
GO


ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubType]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubType_T_CalcPlan] FOREIGN KEY([CalcPlan_Guid])
REFERENCES [dbo].[T_CalcPlan] ([CalcPlan_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubType] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubType_T_CalcPlan]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_CalcPlanByDepartCustomerSubType_Archive](
	[CalcPlanByDepartCustomerSubType_Guid] [dbo].[D_GUID] NOT NULL,
	[CalcPlan_Guid] [dbo].[D_GUID] NOT NULL,
	[SalesPlanQuota_Guid] [dbo].[D_GUID] NOT NULL,
	[Plan_BeginDate] [dbo].[D_DATE] NOT NULL,
	[Plan_EndDate] [dbo].[D_DATE] NOT NULL,
	[Plan_Name] [dbo].[D_NAME] NOT NULL,
	[Plan_Date] [dbo].[D_DATE] NOT NULL,
	[Currency_Guid] [dbo].[D_GUID] NOT NULL,
	[Plan_IsUseForReport] [dbo].[D_YESNO] NOT NULL,
	[Plan_Description] [dbo].[D_DESCRIPTION] NULL,
	[Record_Updated] [dbo].[D_DATETIME] NULL,
	[Record_UserUpdated] [dbo].[D_NAMESHORT] NULL,
	[Action_TypeId] [dbo].[D_ID] NOT NULL
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubType_Archive_PK] ON [dbo].[T_CalcPlanByDepartCustomerSubType_Archive]
(
	[CalcPlanByDepartCustomerSubType_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubType_CalcPlan_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubType_Archive]
(
	[CalcPlan_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubType_SalesPlanQuota_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubType_Archive]
(
	[SalesPlanQuota_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubType_Currency_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubType_Archive]
(
	[Currency_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubType_Action_TypeId] ON [dbo].[T_CalcPlanByDepartCustomerSubType_Archive]
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
CREATE TRIGGER [dbo].[TG_CalcPlanByDepartCustomerSubType_AfterUpdate]
   ON  [dbo].[T_CalcPlanByDepartCustomerSubType] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_CalcPlanByDepartCustomerSubType_Archive ( CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid, 
		Plan_BeginDate, Plan_EndDate, Plan_Name, Plan_Date, Currency_Guid, Plan_IsUseForReport, Plan_Description, 
		Record_Updated, Record_UserUpdated, Action_TypeId )
	SELECT CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid, 
		Plan_BeginDate, Plan_EndDate, Plan_Name, Plan_Date, Currency_Guid, Plan_IsUseForReport, Plan_Description, 
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 0
	FROM inserted;

	UPDATE dbo.[T_CalcPlanByDepartCustomerSubType] SET Record_Updated = sysutcdatetime(), Record_UserUpdated = ( Host_Name() + ': ' + SUSER_SNAME() )
	WHERE [CalcPlanByDepartCustomerSubType_Guid] IN ( SELECT [CalcPlanByDepartCustomerSubType_Guid] FROM inserted );
END

GO

/****** Object:  Trigger [dbo].[TG_CalcPlan_AfterDelete]    Script Date: 28.10.2013 10:58:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер добавляет записи в случае их удаления в таблицу удаленных записей
-- =============================================
CREATE TRIGGER [dbo].[TG_CalcPlanByDepartCustomerSubType_AfterDelete]
   ON [dbo].[T_CalcPlanByDepartCustomerSubType] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.T_CalcPlanByDepartCustomerSubType_Archive ( CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid, 
		Plan_BeginDate, Plan_EndDate, Plan_Name, Plan_Date, Currency_Guid, Plan_IsUseForReport, Plan_Description, 
		Record_Updated, Record_UserUpdated, Action_TypeId )
	SELECT CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid, 
		Plan_BeginDate, Plan_EndDate, Plan_Name, Plan_Date, Currency_Guid, Plan_IsUseForReport, Plan_Description, 
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 2
	FROM deleted;
END

GO



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem](
	[CalcPlanByDepartCustomerSubTypeItem_Guid]	[dbo].[D_GUID] NOT NULL,
	[CalcPlanByDepartCustomerSubType_Guid]			[dbo].[D_GUID] NOT NULL,
	[Owner_Guid]																[dbo].[D_GUID] NOT NULL,
	[PartType_Guid]															[dbo].[D_GUID] NOT NULL,

	[Depart_Guid]																[dbo].[D_GUID] NULL,
	[Customer_Guid]															[dbo].[D_GUID] NULL,
	[PartSubType_Guid]													[dbo].[D_GUID] NULL,

	[CalcPlan_Quantity]													[dbo].[D_QUANTITY] NOT NULL,
	[CalcPlan_AllPrice]													[dbo].[D_MONEY] NOT NULL,
	
	[Record_Updated]														[dbo].[D_DATETIME] NULL,
	[Record_UserUpdated]												[dbo].[D_NAMESHORT] NULL,
 CONSTRAINT [PK_T_CalcPlanByDepartCustomerSubTypeItem] PRIMARY KEY CLUSTERED 
(
	[CalcPlanByDepartCustomerSubTypeItem_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_CalcPlanByDepartCustomerSubType] FOREIGN KEY([CalcPlanByDepartCustomerSubType_Guid])
REFERENCES [dbo].[T_CalcPlanByDepartCustomerSubType] ([CalcPlanByDepartCustomerSubType_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_CalcPlanByDepartCustomerSubType]
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Customer] FOREIGN KEY([Customer_Guid])
REFERENCES [dbo].[T_Customer] ([Customer_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Customer]
GO


ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Depart] FOREIGN KEY([Depart_Guid])
REFERENCES [dbo].[T_Depart] ([Depart_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Depart]
GO


ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Owner] FOREIGN KEY([Owner_Guid])
REFERENCES [dbo].[T_Owner] ([Owner_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Owner]
GO


ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Partsubtype] FOREIGN KEY([PartSubType_Guid])
REFERENCES [dbo].[T_Partsubtype] ([Partsubtype_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Partsubtype]
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]  WITH CHECK ADD  CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Parttype] FOREIGN KEY([PartType_Guid])
REFERENCES [dbo].[T_Parttype] ([Parttype_Guid])
GO

ALTER TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] CHECK CONSTRAINT [FK_T_CalcPlanByDepartCustomerSubTypeItem_T_Parttype]
GO


/****** Object:  Table [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]    Script Date: 28.10.2013 11:25:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive](
	[CalcPlanByDepartCustomerSubTypeItem_Guid] [dbo].[D_GUID] NOT NULL,
	[CalcPlanByDepartCustomerSubType_Guid] [dbo].[D_GUID] NOT NULL,
	[Owner_Guid] [dbo].[D_GUID] NOT NULL,
	[PartType_Guid] [dbo].[D_GUID] NOT NULL,
	[Depart_Guid] [dbo].[D_GUID] NULL,
	[Customer_Guid] [dbo].[D_GUID] NULL,
	[PartSubType_Guid] [dbo].[D_GUID] NULL,
	[CalcPlan_Quantity] [dbo].[D_QUANTITY] NOT NULL,
	[CalcPlan_AllPrice] [dbo].[D_MONEY] NOT NULL,
	[Record_Updated] [dbo].[D_DATETIME] NULL,
	[Record_UserUpdated] [dbo].[D_NAMESHORT] NULL,
	[Action_TypeId] [dbo].[D_ID] NOT NULL
) ON [PRIMARY]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubTypeItem_Archive_PK] ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive]
(
	[CalcPlanByDepartCustomerSubTypeItem_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubTypeItem_Archive_CalcPlanByDepartCustomerSubType_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive]
(
	[CalcPlanByDepartCustomerSubType_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubTypeItem_Archive_Owner_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive]
(
	[Owner_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubTypeItem_Archive_PartType_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive]
(
	[PartType_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubTypeItem_Archive_Depart_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive]
(
	[Depart_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubTypeItem_Archive_Customer_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive]
(
	[Customer_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubTypeItem_Archive_PartSubType_Guid] ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive]
(
	[PartSubType_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


CREATE NONCLUSTERED INDEX [INDX_T_CalcPlanByDepartCustomerSubTypeItem_Archive_Action_TypeId] ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem_Archive]
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
CREATE TRIGGER [dbo].[TG_CalcPlanByDepartCustomerSubTypeItem_AfterUpdate]
   ON  [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_CalcPlanByDepartCustomerSubTypeItem_Archive (CalcPlanByDepartCustomerSubTypeItem_Guid, CalcPlanByDepartCustomerSubType_Guid, 
		Owner_Guid, PartType_Guid, Depart_Guid, Customer_Guid, PartSubType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
		Record_Updated, Record_UserUpdated, Action_TypeId )
	SELECT CalcPlanByDepartCustomerSubTypeItem_Guid, CalcPlanByDepartCustomerSubType_Guid, 
		Owner_Guid, PartType_Guid, Depart_Guid, Customer_Guid, PartSubType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 0
	FROM inserted;

	UPDATE dbo.[T_CalcPlanByDepartCustomerSubTypeItem] SET Record_Updated = sysutcdatetime(), Record_UserUpdated = ( Host_Name() + ': ' + SUSER_SNAME() )
	WHERE [CalcPlanByDepartCustomerSubTypeItem_Guid] IN ( SELECT [CalcPlanByDepartCustomerSubTypeItem_Guid] FROM inserted );
END

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер добавляет записи в случае их удаления в таблицу удаленных записей
-- =============================================
CREATE TRIGGER [dbo].[TG_CalcPlanByDepartCustomerSubTypeItem_AfterDelete]
   ON [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.T_CalcPlanByDepartCustomerSubTypeItem_Archive (CalcPlanByDepartCustomerSubTypeItem_Guid, CalcPlanByDepartCustomerSubType_Guid, 
		Owner_Guid, PartType_Guid, Depart_Guid, Customer_Guid, PartSubType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
		Record_Updated, Record_UserUpdated, Action_TypeId )
	SELECT CalcPlanByDepartCustomerSubTypeItem_Guid, CalcPlanByDepartCustomerSubType_Guid, 
		Owner_Guid, PartType_Guid, Depart_Guid, Customer_Guid, PartSubType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, 
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 2
	FROM deleted;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CalcPlanByDepartCustomerSubTypeView]
AS
SELECT     dbo.T_CalcPlanByDepartCustomerSubType.CalcPlanByDepartCustomerSubType_Guid, dbo.T_CalcPlanByDepartCustomerSubType.CalcPlan_Guid, 
                      dbo.T_CalcPlanByDepartCustomerSubType.SalesPlanQuota_Guid, dbo.T_CalcPlanByDepartCustomerSubType.Plan_BeginDate, 
                      dbo.T_CalcPlanByDepartCustomerSubType.Plan_EndDate, dbo.T_CalcPlanByDepartCustomerSubType.Plan_Name, 
                      dbo.T_CalcPlanByDepartCustomerSubType.Plan_Date, dbo.T_CalcPlanByDepartCustomerSubType.Plan_IsUseForReport, 
                      dbo.T_CalcPlanByDepartCustomerSubType.Plan_Description, dbo.T_CalcPlan.CalcPlan_Year, dbo.T_CalcPlan.CalcPlan_Name, dbo.T_CalcPlan.CalcPlan_Date, 
                      dbo.T_CalcPlanByDepartCustomerSubType.Currency_Guid, dbo.T_CalcPlan.CalcPlan_IsUseForReport, dbo.T_SalesPlanQuota.SalesPlanQuota_Name, 
                      dbo.T_SalesPlanQuota.SalesPlanQuota_Date, dbo.T_SalesPlanQuota.SalesPlanQuota_BeginDate, dbo.T_SalesPlanQuota.SalesPlanQuota_EndDate, 
                      dbo.T_Currency.Currency_Abbr, dbo.T_Currency.Currency_Code, dbo.T_Currency.Currency_ShortName
FROM         dbo.T_CalcPlanByDepartCustomerSubType INNER JOIN
                      dbo.T_CalcPlan ON dbo.T_CalcPlanByDepartCustomerSubType.CalcPlan_Guid = dbo.T_CalcPlan.CalcPlan_Guid INNER JOIN
                      dbo.T_SalesPlanQuota ON dbo.T_CalcPlanByDepartCustomerSubType.SalesPlanQuota_Guid = dbo.T_SalesPlanQuota.SalesPlanQuota_Guid INNER JOIN
                      dbo.T_Currency ON dbo.T_CalcPlanByDepartCustomerSubType.Currency_Guid = dbo.T_Currency.Currency_Guid

GO
GRANT SELECT ON [dbo].[CalcPlanByDepartCustomerSubTypeView] TO [public]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CalcPlanByDepartCustomerSubTypeItemView]
AS
SELECT     dbo.T_DepartTeam.DepartTeam_Name, dbo.T_Depart.DepartTeam_Guid, dbo.T_Depart.Depart_Code, dbo.T_Parttype.Parttype_Id, dbo.T_Parttype.Parttype_Name, 
                      dbo.T_Owner.Owner_Id, dbo.T_Owner.Owner_Name, dbo.T_Partsubtype.Partsubtype_Id, dbo.T_Partsubtype.Partsubtype_Name, dbo.T_Customer.Customer_Id, 
                      dbo.T_Customer.Customer_Name, dbo.T_CalcPlanByDepartCustomerSubTypeItem.CalcPlanByDepartCustomerSubTypeItem_Guid, 
                      dbo.T_CalcPlanByDepartCustomerSubTypeItem.CalcPlanByDepartCustomerSubType_Guid, dbo.T_CalcPlanByDepartCustomerSubTypeItem.Owner_Guid, 
                      dbo.T_CalcPlanByDepartCustomerSubTypeItem.PartType_Guid, dbo.T_CalcPlanByDepartCustomerSubTypeItem.Depart_Guid, 
                      dbo.T_CalcPlanByDepartCustomerSubTypeItem.Customer_Guid, dbo.T_CalcPlanByDepartCustomerSubTypeItem.PartSubType_Guid, 
                      dbo.T_CalcPlanByDepartCustomerSubTypeItem.CalcPlan_Quantity, dbo.T_CalcPlanByDepartCustomerSubTypeItem.CalcPlan_AllPrice
FROM         dbo.T_CalcPlanByDepartCustomerSubTypeItem INNER JOIN
                      dbo.T_Customer ON dbo.T_CalcPlanByDepartCustomerSubTypeItem.Customer_Guid = dbo.T_Customer.Customer_Guid LEFT OUTER JOIN
                      dbo.T_Depart ON dbo.T_CalcPlanByDepartCustomerSubTypeItem.Depart_Guid = dbo.T_Depart.Depart_Guid LEFT OUTER JOIN
                      dbo.T_DepartTeam ON dbo.T_Depart.DepartTeam_Guid = dbo.T_DepartTeam.DepartTeam_Guid LEFT OUTER JOIN
                      dbo.T_Owner ON dbo.T_CalcPlanByDepartCustomerSubTypeItem.Owner_Guid = dbo.T_Owner.Owner_Guid INNER JOIN
                      dbo.T_Partsubtype ON dbo.T_CalcPlanByDepartCustomerSubTypeItem.PartSubType_Guid = dbo.T_Partsubtype.Partsubtype_Guid INNER JOIN
                      dbo.T_Parttype ON dbo.T_CalcPlanByDepartCustomerSubTypeItem.PartType_Guid = dbo.T_Parttype.Parttype_Guid

GO

GRANT SELECT ON [dbo].[CalcPlanByDepartCustomerSubTypeItemView] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает расшифровку плана продаж
--
-- Входные параметры:
--  @CalcPlanByDepartCustomerSubType_Guid - уникальный идентификатор расчета плана продаж
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetCalcPlanByDepartCustomerSubTypeItem] 
  @CalcPlanByDepartCustomerSubType_Guid D_GUID,
  
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

  BEGIN TRY

    SELECT CalcPlanByDepartCustomerSubTypeItem_Guid, CalcPlanByDepartCustomerSubType_Guid,  CalcPlan_Quantity, CalcPlan_AllPrice,
			Owner_Guid, Owner_Id, Owner_Name, 
			PartType_Guid, Parttype_Id, Parttype_Name, 
			DepartTeam_Guid,  DepartTeam_Name,
		  Depart_Guid, Depart_Code, 
			Customer_Guid, Customer_Id, Customer_Name, 
			PartSubType_Guid, Partsubtype_Id, Partsubtype_Name 
    FROM [dbo].[CalcPlanByDepartCustomerSubTypeItemView]
    WHERE [CalcPlanByDepartCustomerSubType_Guid] = @CalcPlanByDepartCustomerSubType_Guid
    ORDER BY Owner_Name, Parttype_Name, DepartTeam_Name, Depart_Code, Partsubtype_Name;

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

GRANT EXECUTE ON [dbo].[usp_GetCalcPlanByDepartCustomerSubTypeItem] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список расчётов плана, чьи периоды удовлетворяют заданным датам
--
-- Входные параметры:
--
--		@BeginDate	- начало периода поиска
--		@EndDate		- окончание периода поиска
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetCalcPlanByDepartCustomerSubType] 
	@BeginDate		D_DATE,
	@EndDate			D_DATE,

  @ERROR_NUM		int output,
  @ERROR_MES		nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

  BEGIN TRY

    SELECT CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid, Plan_BeginDate, Plan_EndDate, Plan_Name, Plan_Date, Plan_IsUseForReport, Plan_Description, 
			CalcPlan_Year, CalcPlan_Name, CalcPlan_Date, Currency_Guid, CalcPlan_IsUseForReport, 
			SalesPlanQuota_Name, SalesPlanQuota_Date, SalesPlanQuota_BeginDate, SalesPlanQuota_EndDate, 
			Currency_Abbr, Currency_Code, Currency_ShortName
    FROM [dbo].[CalcPlanByDepartCustomerSubTypeView]
		WHERE Plan_Date BETWEEN @BeginDate AND @EndDate
    ORDER BY Plan_BeginDate, Plan_IsUseForReport;

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

GRANT EXECUTE ON [dbo].[usp_GetCalcPlanByDepartCustomerSubType] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.[T_CalcPlanByDepartCustomerSubType]
--
-- Входные параметры:
--
--		@CalcPlan_Guid				уи плана продаж "Марка - Группа"
--		@SalesPlanQuota_Guid	уи коэффициента для плана
--		@Plan_Name						наименование
--		@Plan_BeginDate				начало планового периода
--		@Plan_EndDate					окончание планового периода
--		@Plan_Date						Дата регистрации плана
--		@Currency_Guid				УИ валюты,
--		@Plan_IsUseForReport	признак "использовать в отчётах"
--		@Plan_Description			примечание
--
--
-- Выходные параметры:
--
--  @CalcPlanByDepartCustomerSubType_Guid	уникальный идентификатор записи
--  @ERROR_NUM							номер ошибки
--  @ERROR_MES							текст ошибки
--
-- Результат:
--
--		0 - Успешное завершение
--		<>0 - ошибка

CREATE PROCEDURE [dbo].[usp_AddCalcPlanByDepartCustomerSubType] 
	@CalcPlan_Guid				D_GUID,
	@SalesPlanQuota_Guid	D_GUID,
	@Plan_Name						D_NAME,
	@Plan_BeginDate				D_DATE,
	@Plan_EndDate					D_DATE,
	@Plan_Date						D_DATE,
	@Currency_Guid				D_GUID,
	@Plan_IsUseForReport	D_YESNO = 0,
	@Plan_Description			D_DESCRIPTION = NULL,

  @CalcPlanByDepartCustomerSubType_Guid	D_GUID output,
  @ERROR_NUM						int output,
  @ERROR_MES						nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';
    SET @CalcPlanByDepartCustomerSubType_Guid = NULL;

    -- Проверяем наличие уи валюты
    IF NOT EXISTS ( SELECT Currency_Guid FROM dbo.T_Currency WHERE Currency_Guid =  @Currency_Guid)
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена валюта с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @Currency_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие уи плана (Марка-Группа)
    IF NOT EXISTS ( SELECT CalcPlan_Guid FROM dbo.T_CalcPlan WHERE CalcPlan_Guid =  @CalcPlan_Guid)
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найден коэффициент для плана с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @CalcPlan_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие уи коэффициентов расчёта
    IF NOT EXISTS ( SELECT [SalesPlanQuota_Guid] FROM [dbo].[T_SalesPlanQuota] WHERE [SalesPlanQuota_Guid] =  @SalesPlanQuota_Guid)
      BEGIN
        SET @ERROR_NUM = 3;
        SET @ERROR_MES = 'В базе данных не найден коэффициент для плана с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END

		IF( DATEDIFF( day, @Plan_BeginDate, @Plan_EndDate ) < 0 )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'Дата конца периода должна быть больше даты начала периода.';
        RETURN @ERROR_NUM;
      END

    DECLARE @NewID D_GUID;
    SET @NewID = NEWID ( );	
    
    INSERT INTO [dbo].[T_CalcPlanByDepartCustomerSubType]( CalcPlanByDepartCustomerSubType_Guid, CalcPlan_Guid, SalesPlanQuota_Guid, 
			Plan_BeginDate, Plan_EndDate, Plan_Name, Plan_Date, Currency_Guid, Plan_IsUseForReport, Plan_Description, Record_Updated, Record_UserUpdated )
    VALUES( @NewID, @CalcPlan_Guid, @SalesPlanQuota_Guid, 
			@Plan_BeginDate, @Plan_EndDate, @Plan_Name, @Plan_Date, @Currency_Guid, @Plan_IsUseForReport, @Plan_Description, 
			sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
    
    SET @CalcPlanByDepartCustomerSubType_Guid = @NewID;

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

GRANT EXECUTE ON [dbo].[usp_AddCalcPlanByDepartCustomerSubType] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- редактирует новую запись в таблицу dbo.[T_CalcPlanByDepartCustomerSubType]
--
-- Входные параметры:
--
--  @CalcPlanByDepartCustomerSubType_Guid	уникальный идентификатор записи
--		@CalcPlan_Guid				уи плана продаж "Марка - Группа"
--		@SalesPlanQuota_Guid	уи коэффициента для плана
--		@Plan_Name						наименование
--		@Plan_BeginDate				начало планового периода
--		@Plan_EndDate					окончание планового периода
--		@Plan_Date						Дата регистрации плана
--		@Currency_Guid				УИ валюты,
--		@Plan_IsUseForReport	признак "использовать в отчётах"
--		@Plan_Description			примечание
--
--
-- Выходные параметры:
--
--  @ERROR_NUM							номер ошибки
--  @ERROR_MES							текст ошибки
--
-- Результат:
--
--		0 - Успешное завершение
--		<>0 - ошибка

CREATE PROCEDURE [dbo].[usp_EditCalcPlanByDepartCustomerSubType] 
  @CalcPlanByDepartCustomerSubType_Guid	D_GUID,
	@CalcPlan_Guid				D_GUID,
	@SalesPlanQuota_Guid	D_GUID,
	@Plan_Name						D_NAME,
	@Plan_BeginDate				D_DATE,
	@Plan_EndDate					D_DATE,
	@Plan_Date						D_DATE,
	@Currency_Guid				D_GUID,
	@Plan_IsUseForReport	D_YESNO = 0,
	@Plan_Description			D_DESCRIPTION = NULL,

  @ERROR_NUM						int output,
  @ERROR_MES						nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие записи с указанным идентификатором
    IF NOT EXISTS ( SELECT CalcPlanByDepartCustomerSubType_Guid FROM dbo.T_CalcPlanByDepartCustomerSubType 
		WHERE CalcPlanByDepartCustomerSubType_Guid =  @CalcPlanByDepartCustomerSubType_Guid)
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден план с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @CalcPlanByDepartCustomerSubType_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие уи валюты
    IF NOT EXISTS ( SELECT Currency_Guid FROM dbo.T_Currency WHERE Currency_Guid =  @Currency_Guid)
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найдена валюта с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @Currency_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие уи плана (Марка-Группа)
    IF NOT EXISTS ( SELECT CalcPlan_Guid FROM dbo.T_CalcPlan WHERE CalcPlan_Guid =  @CalcPlan_Guid)
      BEGIN
        SET @ERROR_NUM = 3;
        SET @ERROR_MES = 'В базе данных не найден коэффициент для плана с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @CalcPlan_Guid  );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие уи коэффициентов расчёта
    IF NOT EXISTS ( SELECT [SalesPlanQuota_Guid] FROM [dbo].[T_SalesPlanQuota] WHERE [SalesPlanQuota_Guid] =  @SalesPlanQuota_Guid)
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найден коэффициент для плана с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @SalesPlanQuota_Guid  );
        RETURN @ERROR_NUM;
      END

		IF( DATEDIFF( day, @Plan_BeginDate, @Plan_EndDate ) < 0 )
      BEGIN
        SET @ERROR_NUM = 5;
        SET @ERROR_MES = 'Дата конца периода должна быть больше даты начала периода.';
        RETURN @ERROR_NUM;
      END

    
		UPDATE [dbo].[T_CalcPlanByDepartCustomerSubType] SET [CalcPlan_Guid] = @CalcPlan_Guid, [SalesPlanQuota_Guid] = @SalesPlanQuota_Guid, 
			[Plan_BeginDate] = @Plan_BeginDate, [Plan_EndDate] = @Plan_EndDate, [Plan_Name] = @Plan_Name, 
			[Plan_Date] = @Plan_Date, [Currency_Guid] = @Currency_Guid, [Plan_IsUseForReport] = @Plan_IsUseForReport,
			[Plan_Description] = @Plan_Description
		WHERE [CalcPlanByDepartCustomerSubType_Guid] = @CalcPlanByDepartCustomerSubType_Guid;

		IF( @Plan_IsUseForReport = 1 )
			BEGIN
				UPDATE [dbo].[T_CalcPlanByDepartCustomerSubType] SET [Plan_IsUseForReport] = 0
				WHERE [Plan_BeginDate] = @Plan_BeginDate AND [Plan_EndDate] = @Plan_EndDate
					AND [CalcPlanByDepartCustomerSubType_Guid] <> @CalcPlanByDepartCustomerSubType_Guid;
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

GRANT EXECUTE ON [dbo].[usp_EditCalcPlanByDepartCustomerSubType] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Удаляет запись в таблицу dbo.[T_CalcPlanByDepartCustomerSubType]
--
-- Входные параметры:
--
--  @CalcPlanByDepartCustomerSubType_Guid	уникальный идентификатор записи
--
-- Выходные параметры:
--
--  @ERROR_NUM							номер ошибки
--  @ERROR_MES							текст ошибки
--
-- Результат:
--
--		0 - Успешное завершение
--		<>0 - ошибка

CREATE PROCEDURE [dbo].[usp_DeleteCalcPlanByDepartCustomerSubType] 
  @CalcPlanByDepartCustomerSubType_Guid	D_GUID,

  @ERROR_NUM						int output,
  @ERROR_MES						nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие записи с указанным идентификатором
    IF NOT EXISTS ( SELECT CalcPlanByDepartCustomerSubType_Guid FROM dbo.T_CalcPlanByDepartCustomerSubType 
		WHERE CalcPlanByDepartCustomerSubType_Guid =  @CalcPlanByDepartCustomerSubType_Guid)
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден план с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @CalcPlanByDepartCustomerSubType_Guid  );
        RETURN @ERROR_NUM;
      END

		DELETE FROM [dbo].[T_CalcPlanByDepartCustomerSubTypeItem] WHERE [CalcPlanByDepartCustomerSubType_Guid] = @CalcPlanByDepartCustomerSubType_Guid;
		DELETE FROM [dbo].[T_CalcPlanByDepartCustomerSubType] WHERE [CalcPlanByDepartCustomerSubType_Guid] = @CalcPlanByDepartCustomerSubType_Guid;

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

GRANT EXECUTE ON [dbo].[usp_DeleteCalcPlanByDepartCustomerSubType] TO [public]
GO

CREATE TYPE [dbo].[udt_CalcPlanByDepartCustomerSubTypeItem] AS TABLE(

	[CalcPlanByDepartCustomerSubTypeItem_Guid] [dbo].[D_GUID]  NULL,
	[CalcPlanByDepartCustomerSubType_Guid] [dbo].[D_GUID]  NULL,
	[Owner_Guid] [dbo].[D_GUID]  NULL,
	[PartType_Guid] [dbo].[D_GUID]  NULL,
	[Depart_Guid] [dbo].[D_GUID] NULL,
	[Customer_Guid] [dbo].[D_GUID] NULL,
	[PartSubType_Guid] [dbo].[D_GUID] NULL,
	[CalcPlan_Quantity] [dbo].[D_QUANTITY]  NULL,
	[CalcPlan_AllPrice] [dbo].[D_MONEY]  NULL
)
GO

GRANT CONTROL ON TYPE::[dbo].[udt_CalcPlanByDepartCustomerSubTypeItem] TO [public]
GO
use [ERP_Mercury]
GO
GRANT REFERENCES ON TYPE::[dbo].[udt_CalcPlanByDepartCustomerSubTypeItem] TO [public]
GO
use [ERP_Mercury]
GO
GRANT TAKE OWNERSHIP ON TYPE::[dbo].[udt_CalcPlanByDepartCustomerSubTypeItem] TO [public]
GO
use [ERP_Mercury]
GO
GRANT VIEW DEFINITION ON TYPE::[dbo].[udt_CalcPlanByDepartCustomerSubTypeItem] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- сохраняет расшифровку плана продаж
--
-- Входные параметры:

--		@CalcPlanByDepartCustomerSubType_Guid		УИ плана
--		@t_CalcPlanByDepartCustomerSubTypeItem	расшифровка плана продаж
--
--
-- Выходные параметры:
--  @ERROR_NUM												номер ошибки
--  @ERROR_MES												текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_AddCalcPlanByDepartCustomerSubTypeItemList] 
	@CalcPlanByDepartCustomerSubType_Guid		D_GUID,
  @t_CalcPlanByDepartCustomerSubTypeItem	dbo.udt_CalcPlanByDepartCustomerSubTypeItem READONLY,

  @ERROR_NUM															int output,
  @ERROR_MES															nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие записи с указанным идентификатором
    IF NOT EXISTS ( SELECT CalcPlanByDepartCustomerSubType_Guid FROM dbo.T_CalcPlanByDepartCustomerSubType 
										WHERE CalcPlanByDepartCustomerSubType_Guid =  @CalcPlanByDepartCustomerSubType_Guid)
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найден план с указанным идетнификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CONVERT( nvarchar(36), @CalcPlanByDepartCustomerSubType_Guid  );
        RETURN @ERROR_NUM;
      END

		DELETE FROM [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]
		WHERE [CalcPlanByDepartCustomerSubType_Guid] = @CalcPlanByDepartCustomerSubType_Guid;

		INSERT INTO [dbo].[T_CalcPlanByDepartCustomerSubTypeItem]( CalcPlanByDepartCustomerSubTypeItem_Guid, CalcPlanByDepartCustomerSubType_Guid, 
			Owner_Guid, PartType_Guid, Depart_Guid, Customer_Guid, PartSubType_Guid, CalcPlan_Quantity, CalcPlan_AllPrice, Record_Updated, Record_UserUpdated )
		SELECT NEWID(), @CalcPlanByDepartCustomerSubType_Guid,	Owner_Guid, PartType_Guid, Depart_Guid,	Customer_Guid,	PartSubType_Guid,
			CalcPlan_Quantity,	CalcPlan_AllPrice, sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() )
		FROM @t_CalcPlanByDepartCustomerSubTypeItem;

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

GRANT EXECUTE ON [dbo].[usp_AddCalcPlanByDepartCustomerSubTypeItemList] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Расчёт плана продаж разрезе подразделений, клиентов и подгрупп
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
			#PlanItemDepartCustomerPartSubtype.Plan_AllPrice, 
			T_Owner.Owner_Id, T_Parttype.Parttype_Id, 
			T_Customer.Customer_Id, T_Partsubtype.Partsubtype_Id, 
			#PlanItemDepartCustomerPartSubtype.Owner_Guid, #PlanItemDepartCustomerPartSubtype.PartType_Guid, 
			#PlanItemDepartCustomerPartSubtype.Depart_Guid, #PlanItemDepartCustomerPartSubtype.Customer_Guid, 
			#PlanItemDepartCustomerPartSubtype.PartSubType_Guid, T_Depart.DepartTeam_Guid
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

		--SELECT T_Owner.Owner_Name, T_Parttype.Parttype_Name, #CalcPlanItem.CalcPlan_Quantity, #CalcPlanItem.CalcPlan_AllPrice
		--FROM #CalcPlanItem INNER JOIN T_Owner ON #CalcPlanItem.Owner_Guid = T_Owner.Owner_Guid 
		--	INNER JOIN T_Parttype ON #CalcPlanItem.PartType_Guid = T_Parttype.Parttype_Guid
		--ORDER BY T_Owner.Owner_Name, T_Parttype.Parttype_Name;

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
			#PlanItemDepartCustomerPartSubtype.Plan_AllPrice, 
			T_Owner.Owner_Id, T_Parttype.Parttype_Id, 
			T_Customer.Customer_Id, T_Partsubtype.Partsubtype_Id, 
			#PlanItemDepartCustomerPartSubtype.Owner_Guid, #PlanItemDepartCustomerPartSubtype.PartType_Guid, 
			#PlanItemDepartCustomerPartSubtype.Depart_Guid, #PlanItemDepartCustomerPartSubtype.Customer_Guid, 
			#PlanItemDepartCustomerPartSubtype.PartSubType_Guid, T_Depart.DepartTeam_Guid
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список записей из ( dbo.T_Settings )
--
-- Входные параметры:
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetImportDataInPlanByDepartCustomerSubtypeSettings] 
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

  BEGIN TRY

    SELECT Top 1 Settings_Guid, Settings_Name, Settings_XML
    FROM dbo.T_Settings
    WHERE Settings_Name = 'ImportDataInPlanByDepartCustomerSubtypeSettings';

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
GRANT EXECUTE ON [dbo].[usp_GetImportDataInPlanByDepartCustomerSubtypeSettings] TO [public]
GO


DECLARE @doc xml;
SET @doc = '<ImportDataInPlanByDepartCustomerSubtypeSettings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <ColumnItem TOOLS_ID="30" TOOLS_NAME="STARTROW" TOOLS_USERNAME="Начальная строка" TOOLS_DESCRIPTION="№ строки, с которой начинается импорт данных" TOOLS_VALUE="2" TOOLSTYPE_ID="4" />
  <ColumnItem TOOLS_ID="31" TOOLS_NAME="OWNER_ID" TOOLS_USERNAME="код ТМ" TOOLS_DESCRIPTION="№ столбца с кодом товарной марки" TOOLS_VALUE="1" TOOLSTYPE_ID="4" />
  <ColumnItem TOOLS_ID="32" TOOLS_NAME="PARTTYPE_ID" TOOLS_USERNAME="код Группы" TOOLS_DESCRIPTION="№ столбца с кодом товарной группы" TOOLS_VALUE="2" TOOLSTYPE_ID="4" />
  <ColumnItem TOOLS_ID="33" TOOLS_NAME="DEPART_CODE" TOOLS_USERNAME="код Подразделения" TOOLS_DESCRIPTION="№ столбца с кодом подразделения" TOOLS_VALUE="3" TOOLSTYPE_ID="4" />
  <ColumnItem TOOLS_ID="34" TOOLS_NAME="CUSTOMER_ID" TOOLS_USERNAME="код Клиента" TOOLS_DESCRIPTION="№ столбца с кодом клиента" TOOLS_VALUE="4" TOOLSTYPE_ID="4" />
  <ColumnItem TOOLS_ID="35" TOOLS_NAME="PARTSUBTYPE_ID" TOOLS_USERNAME="код Подгруппы" TOOLS_DESCRIPTION="№ столбца с кодом товарной подгруппы" TOOLS_VALUE="5" TOOLSTYPE_ID="4" />
  <ColumnItem TOOLS_ID="36" TOOLS_NAME="QUANTITY" TOOLS_USERNAME="Количество" TOOLS_DESCRIPTION="№ столбца с количеством" TOOLS_VALUE="6" TOOLSTYPE_ID="4" />
  <ColumnItem TOOLS_ID="37" TOOLS_NAME="ALLPRICE" TOOLS_USERNAME="Сумма" TOOLS_DESCRIPTION="№ столбца с суммой" TOOLS_VALUE="6" TOOLSTYPE_ID="4" />
</ImportDataInPlanByDepartCustomerSubtypeSettings>';

INSERT INTO [dbo].[T_Settings]( Settings_Guid, Settings_Name, Settings_XML, Record_Updated, Record_UserUdpated )
VALUES( NEWID(), 'ImportDataInPlanByDepartCustomerSubtypeSettings', @doc, GetDate(), 'Admin' );
