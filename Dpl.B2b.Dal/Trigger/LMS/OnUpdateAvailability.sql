-- Drop the dml trigger if it already exists
IF EXISTS(
  SELECT *
    FROM sys.triggers
   WHERE name = N'OnUpdateAvailablitity'
     AND parent_class_desc = N'OBJECT_OR_COLUMN'
)
	DROP TRIGGER LMS.OnUpdateAvailablitity
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dominik Schulte
-- Create date: 14.09.2020
-- Description:	Updates Orders and OrderLoadDetails Canceled Status to reflect change
-- =============================================
CREATE TRIGGER LMS.OnUpdateAvailablitity 
   ON  LMS.LMS_AVAILABILITY 
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	-- Set Canceled Status
DECLARE @OrderStatusCanceled INT = 10;
DECLARE @OrderLoadDetailStatusCanceled INT = 6;

  UPDATE Orders
  SET [Status] = @OrderStatusCanceled 
  --output inserted.*
  FROM Orders O INNER JOIN inserted I ON O.RefLmsOrderRowGuid = I.RowGuid
  WHERE I.Storniert = 1 AND O.[Status] != @OrderStatusCanceled
  

  UPDATE OrderLoadDetail
  SET [Status] = @OrderLoadDetailStatusCanceled
  --output inserted.*
  FROM inserted I 
  INNER JOIN Orders O ON I.RowGuid = O.RefLmsOrderRowGuid
  INNER JOIN OrderLoad OL ON O.ID = OL.OrderId
  INNER JOIN OrderLoadDetail OD ON OL.DetailId = OD.Id
  WHERE  I.Storniert = 1 AND OD.[Status] != @OrderLoadDetailStatusCanceled  
END
GO
