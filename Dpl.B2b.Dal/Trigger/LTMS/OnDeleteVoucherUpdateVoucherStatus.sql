-- ==============================================
-- Create dml trigger template Azure SQL Database 
-- ==============================================
-- Drop the dml trigger if it already exists
IF EXISTS(
  SELECT *
    FROM sys.triggers
   WHERE name = N'OnDeleteUpdateVoucherStatus'
     AND parent_class_desc = N'OBJECT_OR_COLUMN'
)
	DROP TRIGGER LTMS.OnDeleteUpdateVoucherStatus
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dominik Schulte
-- Create date: 17.09.2020
-- Description:	Updates Voucher.Status to canceled if Voucher has been deleted in LTMS
-- =============================================
CREATE TRIGGER LTMS.OnDeleteUpdateVoucherStatus 
   ON  LTMS.Vouchers 
   AFTER UPDATE
AS 
BEGIN
    DECLARE @VoucherStatusCanceled INT = 255

    -- Update Voucher.Status if LTMS.Internal.VoucherState_ID chanched
    UPDATE dbo.Vouchers
    SET [Status] = @VoucherStatusCanceled
    FROM 
	inserted LV --LTMS.Internal    
	INNER JOIN deleted D ON LV.ID = D.ID
    INNER JOIN LTMS.Internals I ON LV.ID = I.ID  
    INNER JOIN dbo.Vouchers V ON I.Legacy_ID = V.RowGuid
    WHERE 
	D.DeleteTime IS NULL AND  --Old value: not deleted
	LV.DeleteTime IS NOT NULL AND --New value: deleted
	V.[Status] != @VoucherStatusCanceled
END
GO


-- =============================================
-- Author:		Dominik Schulte
-- Create date: 17.09.2020
-- Description:	Sets ALL Voucher.Status to canceled if the Voucher has been deleted in LTMS ***after Reinitialize Replikation***
-- =============================================
BEGIN TRANSACTION
 
    DECLARE @VoucherStatusCanceled INT = 255

    UPDATE dbo.Vouchers
    SET [Status] = @VoucherStatusCanceled OUTPUT inserted.ID,deleted.[Status] AS OldStatus,inserted.[Status] AS NewStatus
    FROM LTMS.Internals I 
    INNER JOIN LTMS.Vouchers LV ON I.ID = LV.ID 
	INNER JOIN Vouchers V ON I.Legacy_ID = V.RowGuid 
    WHERE 
    LV.DeleteTime IS NOT NULL AND V.[Status] != @VoucherStatusCanceled

COMMIT TRANSACTION
GO