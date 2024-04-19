USE [ADM_weighingconveyor_scada]
GO
/****** Object:  StoredProcedure [dbo].[select_all_session]    Script Date: 4/18/2024 3:02:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[select_all_session]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * 
FROM [dbo].[weigh_session] 
WHERE session_code = (SELECT TOP 1 session_code FROM [dbo].[tmp_printreport]);
END
