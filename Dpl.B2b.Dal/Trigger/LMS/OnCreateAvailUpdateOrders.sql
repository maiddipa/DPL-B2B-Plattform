-- ==============================================
-- Create dml trigger template Azure SQL Database 
-- ==============================================
-- Drop the dml trigger if it already exists
IF EXISTS(
  SELECT *
    FROM sys.triggers
   WHERE name = N'OnCreateAvailUpdateOrders'
     AND parent_class_desc = N'OBJECT_OR_COLUMN'
)
	DROP TRIGGER LMS.OnCreateAvailUpdateOrders
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dominik Schulte
-- Create date: 03.06.2020
-- Description:	Updates Orders.Status when new Availability is inserted in LMS
-- =============================================
CREATE TRIGGER LMS.OnCreateAvailUpdateOrders 
   ON  LMS.LMS_AVAILABILITY 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE dbo.Orders
	SET [Status] = 1
	FROM inserted I 
	INNER JOIN Orders O ON I.RowGuid =  O.RefLmsOrderRowGuid
	where O.[Status] = 0

END
GO
