-- Drop the dml trigger if it already exists
IF EXISTS(
  SELECT *
    FROM sys.triggers
   WHERE name = N'OnUpdateAvail2DeliUpdateOrderMatch'
     AND parent_class_desc = N'OBJECT_OR_COLUMN'
)
	DROP TRIGGER LMS.OnUpdateAvail2DeliUpdateOrderMatch
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dominik Schulte
-- Create date: 14.08.2020
-- Description:	Updates OrderMatch and OrderLoadDetails Status to reflect change
-- =============================================
CREATE TRIGGER LMS.OnUpdateAvail2DeliUpdateOrderMatch 
   ON  LMS.LMS_AVAIL2DELI 
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OrderTypeDemand int = 1
	DECLARE @OrderTypeSupply int = 0

	DECLARE @OrderStatusPending int = 0
	DECLARE @OrderStatusTransportScheduled int = 2
	DECLARE @OrderStatusTransportCanceled int = 3

	DECLARE @OrderLoadStatusPending int = 0
	DECLARE @OrderLoadStatusTransportPlanned int = 1
	DECLARE @OrderLoadStatusCanceled int = 6

	DECLARE @OrderTransportTypeSelf int = 0 
	DECLARE @OrderTransportTypeProvidedByOthers int = 1 
	
	DECLARE @LmsLieferkategorieSelbstabholer INT = 3


	-- HOW TO Handle switching between SelfTransport and other TransportTypes
	-- Update TransportType on OrderMatch
	-- if switch to SelfTransport
	--		delete OrderLoadDetail for OrderLoad (Demand)
	--	    link same existing OrderLoadDetail from (Supply) also to Demand
	-- if switch from SelfTranpsort to other TransportTypes
	--		create OrderLoadDetail for OrderLoad (Demand)
	--	    link new OrderLoadDetail to Orderload (Demand)


	-- Mark OrderLoad as deleted if Avail2Deli is (soft) deleted
	IF EXISTS(SELECT * FROM inserted I INNER JOIN deleted D ON I.Avail2DeliID = D.Avail2DeliID 
			WHERE I.DeletionDate is not null)
	BEGIN
		Update OL 
			SET 
			isDeleted = CASE WHEN I.DeletionDate IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END,
			DeletedAt = I.DeletionDate
			FROM inserted I INNER JOIN deleted D ON I.Avail2DeliID = D.Avail2DeliID
				INNER JOIN LMS.LMS_DELIVERY DL ON I.DeliveryID = DL.DiliveryId
				INNER JOIN dbo.Orders O on DL.RowGuid = O.RefLmsOrderRowGuid
				INNER JOIN OrderLoad OL ON O.Id = OL.OrderId and OL.IsDeleted = CAST(0 AS BIT)		
				WHERE I.DeletionDate IS NOT NULL AND D.DeletionDate IS NULL
		Update AOL 
			SET 
			isDeleted = CASE WHEN I.DeletionDate IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END,
			DeletedAt = I.DeletionDate
			FROM inserted I INNER JOIN deleted D ON I.Avail2DeliID = D.Avail2DeliID
				INNER JOIN LMS.LMS_AVAILABILITY AV ON I.AvailabilityID = AV.AvailabilityId
				INNER JOIN dbo.Orders AO on AV.RowGuid = AO.RefLmsOrderRowGuid
				INNER JOIN dbo.OrderLoad AOL ON AO.Id = AOL.OrderId AND AOL.IsDeleted = CAST(0 AS BIT)			
				WHERE I.DeletionDate IS NOT NULL AND D.DeletionDate IS NULL
	END


	-- Switch to SelfTransport
	IF EXISTS (SELECT * FROM inserted I INNER JOIN deleted D ON I.Avail2DeliID = D.Avail2DeliID 
			WHERE I.Lieferkategorie = @LmsLieferkategorieSelbstabholer AND D.Lieferkategorie != I.Lieferkategorie AND I.DeletionDate IS NULL) 
	BEGIN
		DECLARE @OrderDetailsToDelete TABLE (Id INT Primary Key (Id))
		-- Take OrderLoadDetail from Supply / Availabiltity
		Update OL 
		SET DetailId = AOL.DetailId
		output deleted.DetailId INTO @OrderDetailsToDelete (Id)
		FROM inserted I INNER JOIN deleted D ON I.Avail2DeliID = D.Avail2DeliID
			INNER JOIN LMS.LMS_DELIVERY DL ON I.DeliveryID = DL.DiliveryId
			INNER JOIN dbo.Orders O on DL.RowGuid = O.RefLmsOrderRowGuid
			INNER JOIN OrderLoad OL ON O.Id = OL.OrderId and OL.IsDeleted = CAST(0 AS BIT)
			INNER JOIN LMS.LMS_AVAILABILITY AV ON I.AvailabilityID = AV.AvailabilityId
			INNER JOIN dbo.Orders AO on AV.RowGuid = AO.RefLmsOrderRowGuid
			INNER JOIN dbo.OrderLoad AOL ON AO.Id = AOL.OrderId AND AOL.IsDeleted = CAST(0 AS BIT)			
			WHERE I.Lieferkategorie = @LmsLieferkategorieSelbstabholer AND D.Lieferkategorie != @LmsLieferkategorieSelbstabholer

         -- Delete OrderLoadDetail for Demand
			Delete O from dbo.OrderLoadDetail O INNER JOIN @OrderDetailsToDelete D ON O.ID = D.Id
	END

	-- Switch from SelfTransport to any other TransportType
	IF EXISTS (SELECT * FROM inserted I INNER JOIN deleted D ON I.Avail2DeliID = D.Avail2DeliID 
			WHERE D.Lieferkategorie = @LmsLieferkategorieSelbstabholer AND D.Lieferkategorie != I.Lieferkategorie AND I.DeletionDate IS NULL) 
	BEGIN
		DECLARE @DemandOrderLoadDetails TABLE(
			[Id] [int]  NOT NULL,
			[Status] [int] NOT NULL,
			[PlannedFulfillmentDateTime] [datetime2](7) NOT NULL,
			[ActualFulfillmentDateTime] [datetime2](7) NULL,
			[LoadCarrierReceiptId] [int] NULL,
			[AddressId] [int] NOT NULL,
		 PRIMARY KEY 
		(
			[Id] ASC
		))

		-- Insert new OrderLoadDetail for Demand and safe new Records to @DemandOrderLoadDetails
		INSERT INTO [dbo].[OrderLoadDetail]
				   ([Status]
				   ,[PlannedFulfillmentDateTime]
				   ,[ActualFulfillmentDateTime]
				   ,[LoadCarrierReceiptId]
				   ,[AddressId])    Output inserted.* INTO @DemandOrderLoadDetails 
		select 
		CASE I.FrachtpapiereErstellt WHEN 1 THEN @OrderLoadStatusTransportPlanned ELSE  @OrderLoadStatusPending END	 as [Status],
			I.DateOfRelation as [PlannedFulfillmentDateTime], --TODO: Check if correctly mappted
			null as [ActualFulfillmentDateTime], 
			null as  [LoadCarrierReceiptId], 
			LL.AddressId  
		from inserted I 
		INNER JOIN deleted D ON I.Avail2DeliID = D.Avail2DeliID 
		INNER JOIN LMS.LMS_DELIVERY DL ON I.DeliveryID = DL.DiliveryId AND DL.DeletionDate IS NULL
		INNER JOIN dbo.LoadingLocations LL ON 
			(DL.LoadingLocationId = LL.Id) 
			WHERE I.Lieferkategorie != @LmsLieferkategorieSelbstabholer AND D.Lieferkategorie = @LmsLieferkategorieSelbstabholer
		-- Link OrderLoad to the new OrderLoadDetail
		Update OrderLoad
		SET DetailId = OLD.Id
		FROM inserted I INNER JOIN deleted D ON I.Avail2DeliID = D.Avail2DeliID
			INNER JOIN LMS.LMS_DELIVERY DL ON I.DeliveryID = DL.DiliveryId
			INNER JOIN dbo.Orders O on DL.RowGuid = O.RefLmsOrderRowGuid
			INNER JOIN dbo.OrderLoad OL ON O.Id = OL.OrderId and OL.IsDeleted = CAST(0 AS BIT)
			Cross JOIN @DemandOrderLoadDetails OLD
			WHERE I.Lieferkategorie != @LmsLieferkategorieSelbstabholer AND D.Lieferkategorie = @LmsLieferkategorieSelbstabholer
	END


	UPDATE OrderMatches
	SET [Status] = CASE 
	WHEN I.DeletionDate IS NOT NULL THEN @OrderStatusTransportCanceled
	WHEN I.FrachtpapiereErstellt = CAST( 1 AS BIT) THEN @OrderStatusTransportScheduled 
	WHEN I.FrachtpapiereErstellt = CAST( 0 AS BIT) AND D.FrachtpapiereErstellt = CAST(1 AS BIT) THEN @OrderStatusPending
	END,
	[TransportType] = CASE I.Lieferkategorie   --- Update TransportType for SelfTransport
	WHEN @LmsLieferkategorieSelbstabholer THEN @OrderTransportTypeSelf
	ELSE @OrderTransportTypeProvidedByOthers END,
	SelfTransportSide = CASE  
	WHEN I.Lieferkategorie =  @LmsLieferkategorieSelbstabholer AND DL.IstBedarf = CAST(1 AS BIT) THEN @OrderTypeDemand 
	WHEN I.Lieferkategorie =  @LmsLieferkategorieSelbstabholer AND A.SelfDelivery = CAST(1 AS BIT) THEN @OrderTypeSupply
	ELSE NULL END
    from Inserted I INNER JOIN Deleted D on I.Avail2DeliID = D.Avail2DeliID
	INNER JOIN LMS.LMS_AVAILABILITY A ON I.AvailabilityID = A.AvailabilityId AND A.DeletionDate IS NULL
	INNER JOIN LMS.LMS_DELIVERY DL ON I.DeliveryID = DL.DiliveryId AND DL.DeletionDate IS NULL
	INNER JOIN dbo.Orders O ON O.RefLmsOrderRowGuid = A.RowGuid OR O.RefLmsOrderRowGuid = DL.RowGuid
	INNER JOIN OrderLoad OL ON O.Id = OL.OrderId and OL.IsDeleted = CAST(0 AS BIT)
	INNER JOIN dbo.OrderMatches OM on OM.IsDeleted = cast(0 AS BIT) AND (
		(OL.Id = OM.DemandId AND O.[Type] = @OrderTypeDemand) OR 
		(OL.Id = OM.SupplyId AND O.[Type] = @OrderTypeSupply))	
		WHERE I.FrachtpapiereErstellt != D.FrachtpapiereErstellt OR 
		I.DeletionDate IS NOT NULL 

	UPDATE OrderLoadDetail  
	SET [Status] = CASE 
	WHEN I.FrachtpapiereErstellt = CAST( 1 AS BIT) THEN @OrderLoadStatusTransportPlanned 
	WHEN I.FrachtpapiereErstellt = CAST( 0 AS BIT) AND D.FrachtpapiereErstellt = CAST(1 AS BIT) THEN @OrderLoadStatusPending
	END	
    from Inserted I INNER JOIN Deleted D on I.Avail2DeliID = D.Avail2DeliID
	INNER JOIN LMS.LMS_AVAILABILITY A ON I.AvailabilityID = A.AvailabilityId AND A.DeletionDate IS NULL
	INNER JOIN LMS.LMS_DELIVERY DL ON I.DeliveryID = DL.DiliveryId AND DL.DeletionDate IS NULL
	INNER JOIN dbo.Orders O ON O.RefLmsOrderRowGuid = A.RowGuid OR O.RefLmsOrderRowGuid = DL.RowGuid	
	INNER JOIN OrderLoad OL ON O.Id = OL.OrderId and OL.IsDeleted = CAST(0 AS BIT)
	INNER JOIN dbo.OrderLoadDetail OD on OL.DetailId = OD.ID
	WHERE I.FrachtpapiereErstellt != D.FrachtpapiereErstellt 
END
GO
