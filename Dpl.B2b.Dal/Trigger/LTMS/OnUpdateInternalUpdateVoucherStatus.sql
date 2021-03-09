-- ==============================================
-- Create dml trigger template Azure SQL Database 
-- ==============================================
-- Drop the dml trigger if it already exists
IF EXISTS(
  SELECT *
    FROM sys.triggers
   WHERE name = N'OnUpdateUpdateVoucherStatus'
     AND parent_class_desc = N'OBJECT_OR_COLUMN'
)
	DROP TRIGGER LTMS.OnUpdateUpdateVoucherStatus
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dominik Schulte
-- Create date: 17.09.2020
-- Description:	Updates Voucher.Status if LTMS.Internal.VoucherState_ID chanched
-- =============================================
CREATE TRIGGER LTMS.OnUpdateUpdateVoucherStatus 
   ON  LTMS.Internals 
   AFTER UPDATE
AS 
BEGIN
    DECLARE @VoucherStatusIssued INT = 0
    DECLARE @VoucherStatusSubmitted INT = 1 -- obsolet
    DECLARE @VoucherStatusAccounted INT = 2
    DECLARE @VoucherStatusExpired INT = 99
    DECLARE @VoucherStatusCanceled INT = 255


    DECLARE @VoucherStateMapping TABLE (OLMA_StateId INT,LTMS_StateId CHAR(1),OLMA_StateId_Reverse INT PRIMARY KEY(LTMS_StateId) )
    INSERT INTO @VoucherStateMapping
    VALUES
    (@VoucherStatusAccounted,'A',@VoucherStatusAccounted),
    (@VoucherStatusAccounted,'B',-1),  --State does not exist in OLMA at the moment
    (@VoucherStatusIssued,'D',@VoucherStatusIssued),
    (@VoucherStatusCanceled,'S',@VoucherStatusCanceled),
    (@VoucherStatusExpired,'V',@VoucherStatusExpired) --not implemented in LTMS at the moment 

    -- Update Voucher.Status if LTMS.Internal.VoucherState_ID chanched
    UPDATE dbo.Vouchers
    SET [Status] = M.OLMA_StateId
    FROM 
    inserted I --LTMS.Internal
    INNER JOIN deleted D ON I.ID = D.ID
    INNER JOIN LTMS.Vouchers LV ON I.ID = LV.ID 
    INNER JOIN dbo.Vouchers V ON I.Legacy_ID = V.RowGuid
    INNER JOIN @VoucherStateMapping M ON I.VoucherState_ID = M.LTMS_StateId
    WHERE I.VoucherState_ID != D.VoucherState_ID  
END
GO


-- =============================================
-- Author:		Dominik Schulte
-- Create date: 17.09.2020
-- Description:	Updates ALL Voucher.Status if LTMS.Internal.VoucherState_ID does not match after Reinitialize Replikation
-- =============================================
BEGIN TRANSACTION
    DECLARE @VoucherStatusIssued INT = 0
    DECLARE @VoucherStatusSubmitted INT = 1 -- obsolet
    DECLARE @VoucherStatusAccounted INT = 2
    DECLARE @VoucherStatusExpired INT = 99
    DECLARE @VoucherStatusCanceled INT = 255


    DECLARE @VoucherStateMapping TABLE (OLMA_StateId INT,LTMS_StateId CHAR(1),OLMA_StateId_Reverse INT PRIMARY KEY(LTMS_StateId) )
    INSERT INTO @VoucherStateMapping
    VALUES
    (@VoucherStatusAccounted,'A',@VoucherStatusAccounted),
    (@VoucherStatusAccounted,'B',-1),  --State does not exist in OLMA at the moment
    (@VoucherStatusIssued,'D',@VoucherStatusIssued),
    (@VoucherStatusCanceled,'S',@VoucherStatusCanceled),
    (@VoucherStatusExpired,'V',@VoucherStatusExpired) --not implemented in LTMS at the moment 

    UPDATE dbo.Vouchers
    SET [Status] = M.OLMA_StateId OUTPUT inserted.ID,deleted.[Status] AS OldStatus,inserted.[Status] AS NewStatus
    FROM LTMS.Internals I 
    INNER JOIN LTMS.Vouchers LV ON I.ID = LV.ID 
    INNER JOIN Vouchers V ON Legacy_ID = V.RowGuid
    INNER JOIN @VoucherStateMapping M ON I.VoucherState_ID = M.LTMS_StateId
    INNER JOIN @VoucherSTateMapping RM ON V.[Status] = RM.OLMA_StateId_Reverse
    WHERE 
    M.OLMA_StateId != RM.OLMA_StateId

COMMIT TRANSACTION
GO