-- ==============================================
-- Create dml trigger template Azure SQL Database 
-- ==============================================
-- Drop the dml trigger if it already exists
IF EXISTS(
  SELECT *
    FROM sys.triggers
   WHERE name = N'OnCreateUpdatePostingRequestStatus'
     AND parent_class_desc = N'OBJECT_OR_COLUMN'
)
	DROP TRIGGER LTMS.OnCreateUpdatePostingRequestStatus
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dominik Schulte
-- Create date: 03.06.2020
-- Description:	Updates PostingRequests.Status from Pending --> Confirmed when new Transactions are inserted in LTMS
-- =============================================
CREATE TRIGGER LTMS.OnCreateUpdatePostingRequestStatus 
   ON  LTMS.Transactions 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    UPDATE PostingRequests
	SET [Status] = 1
	FROM inserted I INNER JOIN PostingRequests P ON I.RowGuid = P.RefLtmsTransactionId
	where P.[Status] = 0

END
GO
