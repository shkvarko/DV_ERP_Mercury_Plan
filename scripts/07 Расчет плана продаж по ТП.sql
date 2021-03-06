USE [ERP_Mercury]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProductOwnersForDepartTeam]    Script Date: 20.01.2014 10:50:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список команд, назначенных товарной марке
--
-- Входные параметры:
--
--		@ProductOwner_Guid - уникальный идентификатор товарной марки
--
--
-- Выходные параметры:
--
--  @ERROR_NUM - номер ошибки
--  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_GetDepartTeamsForProductOwner] 
  @ProductOwner_Guid	D_GUID,
  
  @ERROR_NUM					int output,
  @ERROR_MES					nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    IF NOT EXISTS ( SELECT [Owner_Guid] FROM [dbo].[T_Owner] WHERE [Owner_Guid] = @ProductOwner_Guid )
     BEGIN
       SET @ERROR_NUM = 1;
       SET @ERROR_MES = 'В базе данных не найдена товарная марка с указанным идентификатором.' + nChar(13) + nChar(10) + 
				'УИ: ' + CONVERT( nvarchar(36), @ProductOwner_Guid );
       RETURN @ERROR_NUM;
     END

		SELECT DepartTeam_Guid, DepartTeam_Name, DepartTeam_Description, DepartTeam_IsActive, Record_IsActive, Record_ParentGuid
		FROM [dbo].[T_DepartTeam]
		WHERE [DepartTeam_Guid] IN ( SELECT [DepartTeam_Guid] FROM dbo.T_DepartTeamOwner WHERE [Owner_Guid] = @ProductOwner_Guid )
		ORDER BY DepartTeam_Name;

	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';
		
	RETURN @ERROR_NUM;
END

GO
GRANT EXECUTE ON [dbo].[usp_GetDepartTeamsForProductOwner] TO [public]
GO

