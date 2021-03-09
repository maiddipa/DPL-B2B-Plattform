-- ==============================================
-- Create dml trigger template Azure SQL Database 
-- ==============================================
-- Drop the dml trigger if it already exists
IF EXISTS(
  SELECT *
    FROM sys.triggers
   WHERE name = N'OnCreateAvail2DeliInsertOrderMatch'
     AND parent_class_desc = N'OBJECT_OR_COLUMN'
)
	DROP TRIGGER LMS.OnCreateAvail2DeliInsertOrderMatch
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dominik Schulte
-- Create date: 03.06.2020
-- Description:	Creates OrderMatches, OrderLoads and OrderLoadDetails when new Avail2Delis are inserted by LMS
-- =============================================
CREATE TRIGGER LMS.OnCreateAvail2DeliInsertOrderMatch 
   ON  LMS.LMS_AVAIL2DELI 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @OrderTypeDemand int = 1
DECLARE @OrderTypeSupply int = 0
DECLARE @OrderStatusPending int = 0
DECLARE @OrderLoadStatusPending int = 0
DECLARE @OrderLoadStatusTransportPlanned int = 1
DECLARE @OrderStatusTransportScheduled int = 2
DECLARE @OrderTransportTypeSelf INT = 0

DECLARE @LmsLieferkategorieSelbstabholer INT = 3

/* LoadCarrier Mappings */

DECLARE @LcMapping TABLE (
	Id INT
	,RefLmsLoadCarrierTypeId INT
	,RefLmsQualityId INT
	--,RefLmsQuality2PalletId INT 
	)
INSERT INTO @LcMapping --Output Inserted.*
select LC.Id
	,RefLmsLoadCarrierTypeId
	,RefLmsQualityId
	--,RefLmsQuality2PalletId 
FROM LoadCarriers LC 
INNER JOIN LoadCarrierTypes LT ON LC.TypeId = LT.ID
INNER JOIN LoadCarrierQualityMappings LQM ON LC.QualityId = LQM.LoadCarrierQualityId


/* Affected Avail2Delis 
   (All Avail2Deli with existing Orders but without OrderMatches)
*/
DECLARE @Avail2Delis TABLE(
	[Avail2DeliID] [int]  NOT NULL,
	[DeliveryID] [int] NOT NULL,
	[AvailabilityID] [int] NOT NULL,
	[DateOfRelation] [datetime] NOT NULL,
	[Quantity] [int] NOT NULL,
	[State] [int] NOT NULL,
	[Usr] [varchar](32) NULL,
	[IsFix] [bit] NOT NULL,
	[SpediteurID] [int] NULL,
	[SpediteurManuell] [varchar](64) NULL,
	[Kilometer] [int] NULL,
	[Frachtpreis] [float] NULL,
	[LieferscheinNr] [varchar](32) NULL,
	[FrachtauftragsNr] [varchar](32) NULL,
	[FrachtpapiereErstellt] [bit] NULL,
	[Notiz] [varchar](1024) NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
	[Bestellnummer] [varchar](32) NULL,
	[ZustellterminZeitVon] [varchar](5) NULL,
	[ZustellterminZeitBis] [varchar](5) NULL,
	[LadeterminDatum] [datetime] NULL,
	[LadeterminZeitVon] [varchar](5) NULL,
	[LadeterminZeitBis] [varchar](5) NULL,
	[InBearbeitungVon] [varchar](32) NULL,
	[InBearbeitungDatumZeit] [datetime] NULL,
	[SpediteurTelefon] [varchar](32) NULL,
	[SpediteurFax] [varchar](32) NULL,
	[SpediteurEMail] [varchar](64) NULL,
	[Lieferkategorie] [int] NULL,
	[AbweichenderArtikel] [varchar](128) NULL,
	[FrachtvorschriftFelder] [varchar](32) NULL,
	[FrachtvorschriftStapelhoehe] [int] NULL,
	[Revision] [int] NULL,
	[StatusGedrucktFrachtvorschrift] [bit] NULL,
	[StatusGedrucktZuordnung] [bit] NULL,
	[ICTransport] [bit] NULL,
	[CategoryId] [int] NULL,
	[AnzahlBelegteStellplaetze] [int] NULL,
	[Timocom] [bit] NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[LoadCarrierStackHeight] [int] NULL,
	[SupportsRearLoading] [bit] NULL,
	[SupportsSideLoading] [bit] NULL,
	[SupportsJumboVehicles] [bit] NULL,
	[SupportsIndividual] [varchar](128) NULL,
	[BasePalletTypeId] [int] NULL,
	[BaseQualityId] [int] NULL,
	[BaseQuantity] [int] NULL,
	[ShowOnline] [bit] NULL,
	[ShowOnlineUntil] [datetime] NULL,
	[IsCreatedOnline] [bit] NULL,
	[QualityId] [int] NULL,
	[OnlineComment] [varchar](1024) NULL,
	[LiveDocumentNumber] [varchar](64) NULL,
	[ExpressCode] [varchar](12) NULL,
	[PalletTypeId] [int] NULL,
	[HasBasePallets] [bit] NULL
	PRIMARY KEY CLUSTERED 
(
	[Avail2DeliID] ASC))
INSERT INTO @Avail2Delis 
	  --output inserted.* 
select  DISTINCT [Avail2DeliID]
      ,[DeliveryID]
      ,I.[AvailabilityID]
      ,[DateOfRelation]
      ,I.[Quantity]
      ,[State]
      ,[Usr]
      ,I.[IsFix]
      ,[SpediteurID]
      ,[SpediteurManuell]
      ,I.[Kilometer]
      ,I.[Frachtpreis]
      ,[LieferscheinNr]
      ,[FrachtauftragsNr]
      ,[FrachtpapiereErstellt]
      ,I.[Notiz]
      ,I.[CreationDate]
      ,I.[ModificationDate]
      ,I.[DeletionDate]
      ,I.[CreatedBy]
      ,I.[ModifiedBy]
      ,I.[DeletedBy]
      ,I.[Bestellnummer]
      ,[ZustellterminZeitVon]
      ,[ZustellterminZeitBis]
      ,[LadeterminDatum]
      ,[LadeterminZeitVon]
      ,[LadeterminZeitBis]
      ,I.[InBearbeitungVon]
      ,I.[InBearbeitungDatumZeit]
      ,[SpediteurTelefon]
      ,[SpediteurFax]
      ,[SpediteurEMail]
      ,[Lieferkategorie]
      ,[AbweichenderArtikel]
      ,[FrachtvorschriftFelder]
      ,[FrachtvorschriftStapelhoehe]
      ,I.[Revision]
      ,[StatusGedrucktFrachtvorschrift]
      ,[StatusGedrucktZuordnung]
      ,[ICTransport]
      ,I.[CategoryId]
      ,[AnzahlBelegteStellplaetze]
      ,[Timocom]
      ,I.[RowGuid]
      ,I.[LoadCarrierStackHeight]
      ,I.[SupportsRearLoading]
      ,I.[SupportsSideLoading]
      ,I.[SupportsJumboVehicles]
      ,I.[SupportsIndividual]
      ,I.[BasePalletTypeId]
      ,I.[BaseQualityId]
      ,I.[BaseQuantity]
      ,I.[ShowOnline]
      ,I.[ShowOnlineUntil]
      ,I.[IsCreatedOnline]
      ,I.[QualityId]
      ,I.[OnlineComment]
      ,I.[LiveDocumentNumber]
      ,I.[ExpressCode]
      ,I.[PalletTypeId]
      ,I.[HasBasePallets]
FROM inserted I 
INNER JOIN LMS.LMS_AVAILABILITY A ON I.AvailabilityID = A.AvailabilityId AND A.DeletionDate IS NULL
INNER JOIN LMS.LMS_DELIVERY D ON I.DeliveryID = D.DiliveryId AND D.DeletionDate IS NULL
INNER JOIN dbo.Orders O ON O.RefLmsOrderRowGuid = A.RowGuid OR O.RefLmsOrderRowGuid = D.RowGuid
LEFT JOIN OrderLoad OL ON O.Id = OL.OrderId and OL.IsDeleted = CAST(0 AS BIT)
LEFT JOIN dbo.OrderMatches OM on OM.IsDeleted = cast(0 AS BIT) AND (
		(OL.Id = OM.DemandId AND O.[Type] = @OrderTypeDemand) OR 
		(OL.Id = OM.SupplyId AND O.[Type] = @OrderTypeSupply))
WHERE I.IsCreatedOnline = CAST(0 AS BIT) AND  OM.Id IS NULL AND I.DeletionDate  IS NULL

DECLARE @SupplyOrderLoadDetails TABLE(
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

-- OrderLoadDetails for Availability 

DECLARE @SupplyOrderLoad TABLE(
	[Id] [int]  NOT NULL,
	[CreatedById] [int] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[ChangedById] [int] NULL,
	[ChangedAt] [datetime2](7) NULL,
	[DeletedById] [int] NULL,
	[DeletedAt] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
	[OrderId] [int] NOT NULL,
	[DemandOrderMatchId] [int] NULL,
	[SupplyOrderMatchId] [int] NULL,
	[DetailId] [int] NOT NULL,
 PRIMARY KEY 
(
	[Id] ASC
))

-- OrderLaodDetails (OLD) for Availabiltiys are handeled separate for SelfTransport and NON-Selftransports 
-- because on SalfTransport OrderLoadDetails for Supply and Demand are the same
-- OLD are loaded into separate Table-Variables

--OderLoadDetails for Availability (SelfTransport)

INSERT INTO [dbo].[OrderLoadDetail]
           ([Status]
           ,[PlannedFulfillmentDateTime]
           ,[ActualFulfillmentDateTime]
           ,[LoadCarrierReceiptId]
           ,[AddressId])    
		   Output inserted.* INTO @DemandOrderLoadDetails -- Same OrderLoadDetails must also been added to Demand
select 
	CASE A.FrachtpapiereErstellt WHEN 1 THEN @OrderLoadStatusTransportPlanned ELSE  @OrderLoadStatusPending END	 as [Status],
	A.LadeterminDatum as [PlannedFulfillmentDateTime], 
	null as [ActualFulfillmentDateTime], 
	null as  [LoadCarrierReceiptId], 
	LL.AddressId 
from @Avail2Delis A 
INNER JOIN LMS.LMS_AVAILABILITY D ON A.AvailabilityID = D.AvailabilityId AND D.DeletionDate IS NULL
INNER JOIN dbo.LoadingLocations LL ON D.LoadingLocationId = LL.Id
WHERE Lieferkategorie = @LmsLieferkategorieSelbstabholer

-- OrderLoadDetails for Availability (NON-SelfTransport)
INSERT INTO [dbo].[OrderLoadDetail]
           ([Status]
           ,[PlannedFulfillmentDateTime]
           ,[ActualFulfillmentDateTime]
           ,[LoadCarrierReceiptId]
           ,[AddressId])    Output inserted.* INTO @SupplyOrderLoadDetails 
select 
	CASE A.FrachtpapiereErstellt WHEN 1 THEN @OrderLoadStatusTransportPlanned ELSE  @OrderLoadStatusPending END	 as [Status],
	A.LadeterminDatum as [PlannedFulfillmentDateTime], 
	null as [ActualFulfillmentDateTime], 
	null as  [LoadCarrierReceiptId], 
	LL.AddressId 
from @Avail2Delis A 
INNER JOIN LMS.LMS_AVAILABILITY D ON A.AvailabilityID = D.AvailabilityId AND D.DeletionDate IS NULL
INNER JOIN dbo.LoadingLocations LL ON D.LoadingLocationId = LL.Id
WHERE Lieferkategorie != @LmsLieferkategorieSelbstabholer

INSERT INTO [dbo].[OrderLoad]
           (
		   [CreatedById],
		   [CreatedAt]
           ,[IsDeleted]
           ,[OrderId]
           ,[DemandOrderMatchId]
           ,[SupplyOrderMatchId]
           ,[DetailId]) 
		   output inserted.* INTO @SupplyOrderLoad
     SELECT 
           NULL AS CreatedById,
           CAST(A2D.CreationDate as datetime2) as [CreatedAt] 
           ,CAST(0 as BIT) AS IsDeleted
           ,O.Id As OrderId
           ,NULL AS DemandOrderMatchId
           ,NULL AS SupplyOrderMatchId
           ,OLD.Id as DetailId		   
FROM @Avail2Delis A2D 
INNER JOIN LMS.LMS_AVAILABILITY A ON A2D.AvailabilityID = A.AvailabilityId
INNER JOIN dbo.Orders O on A.RowGuid = O.RefLmsOrderRowGuid
Cross JOIN (SELECT * FROM @SupplyOrderLoadDetails UNION SELECT * FROM @DemandOrderLoadDetails) OLD --Supply and DemandOrderLoadDetails (SelfTransport)



-- OrderLoadDetails for Delivery 


DECLARE @DemandOrderLoad TABLE(
	[Id] [int]  NOT NULL,
	[CreatedById] [int] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[ChangedById] [int] NULL,
	[ChangedAt] [datetime2](7) NULL,
	[DeletedById] [int] NULL,
	[DeletedAt] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
	[OrderId] [int] NOT NULL,
	[DemandOrderMatchId] [int] NULL,
	[SupplyOrderMatchId] [int] NULL,
	[DetailId] [int] NOT NULL,
 PRIMARY KEY 
(
	[Id] ASC
))



INSERT INTO [dbo].[OrderLoadDetail]
           ([Status]
           ,[PlannedFulfillmentDateTime]
           ,[ActualFulfillmentDateTime]
           ,[LoadCarrierReceiptId]
           ,[AddressId])    Output inserted.* INTO @DemandOrderLoadDetails 
select 
CASE A.FrachtpapiereErstellt WHEN 1 THEN @OrderLoadStatusTransportPlanned ELSE  @OrderLoadStatusPending END	 as [Status],
	A.DateOfRelation as [PlannedFulfillmentDateTime], 
	null as [ActualFulfillmentDateTime], 
	null as  [LoadCarrierReceiptId], 
	LL.AddressId  
from @Avail2Delis A 
INNER JOIN LMS.LMS_DELIVERY D ON A.DeliveryID = D.DiliveryId AND D.DeletionDate IS NULL
INNER JOIN dbo.LoadingLocations LL ON 
	(D.LoadingLocationId = LL.Id) 
	WHERE A.Lieferkategorie != @LmsLieferkategorieSelbstabholer --exclude SelfTransports because they are already in the @DemandOrderLoadDetails-Table


INSERT INTO [dbo].[OrderLoad]
           (
		   [CreatedById],
		   [CreatedAt]
           ,[IsDeleted]
           ,[OrderId]
           ,[DemandOrderMatchId]
           ,[SupplyOrderMatchId]
           ,[DetailId]) 
		   output inserted.* INTO @DemandOrderLoad
     SELECT 
           NULL AS CreatedById,
           CAST(A2D.CreationDate as datetime2) as [CreatedAt] 
           ,CAST(0 as BIT) AS IsDeleted
           ,O.Id As OrderId
           ,NULL AS DemandOrderMatchId
           ,NULL AS SupplyOrderMatchId
           ,OLD.Id as DetailId		   
FROM @Avail2Delis A2D 
INNER JOIN LMS.LMS_DELIVERY D ON A2D.DeliveryID = D.DiliveryId
INNER JOIN dbo.Orders O on D.RowGuid = O.RefLmsOrderRowGuid
Cross JOIN @DemandOrderLoadDetails OLD


INSERT INTO dbo.OrderMatches
           ([CreatedById]
           ,[CreatedAt]
           ,[IsDeleted]
           ,[OrderMatchNumber]
           ,[RefLmsAvailabilityRowGuid]
           ,[RefLmsDeliveryRowGuid]
           ,[DigitalCode]
           ,[TransportType]
           ,[SelfTransportSide]
           ,[TransportId]
           ,[Status]
           ,[LoadCarrierId]
           ,[BaseLoadCarrierId]
           ,[DocumentId]
           ,[LoadCarrierStackHeight]
           ,[LoadCarrierQuantity]
           ,[NumberOfStacks]
           ,[BaseLoadCarrierQuantity]
           ,[SupportsRearLoading]
           ,[SupportsSideLoading]
           ,[SupportsJumboVehicles]
           ,[DemandId]
           ,[SupplyId])
		--   output inserted.*
SELECT  
		   NULL AS CreatedById,  --TODO: Sysstem User?
A2D.CreationDate as CreatedAt,
0 AS           IsDeleted, 
		   NULL AS  OrderMatchNumber, --TODO: witch OrderNumber
A2.RowGuid AS RefLmsAvailabilityRowGuid,
D2.RowGuid AS RefLmsDeliveryRowGuid,
A2D.ExpressCode AS DigitalCode,
CASE A2D.Lieferkategorie WHEN @LmsLieferkategorieSelbstabholer THEN @OrderTransportTypeSelf ELSE 1 END  AS TransportType, 
CASE 
WHEN D2.IstBedarf  = CAST(1 AS BIT)  AND A2D.Lieferkategorie =  @LmsLieferkategorieSelbstabholer THEN @OrderTypeDemand 
WHEN A2.SelfDelivery = CAST(1 AS BIT) AND A2D.Lieferkategorie =  @LmsLieferkategorieSelbstabholer THEN @OrderTypeSupply
ELSE NULL END AS SelfTransportSide,  
           NULL AS TransportId, 
CASE A2D.FrachtpapiereErstellt WHEN 1 THEN @OrderStatusTransportScheduled ELSE  @OrderStatusPending END AS Status, 
LC.Id AS LoadCarrierId, 
BLC.Id AS BaseLoadCarrierId, 
           NULL AS DocumentId, 
A2D.LoadCarrierStackHeight AS LoadCarrierStackHeight, 
A2D.Quantity AS LoadCarrierQuantity,
A2D.AnzahlBelegteStellplaetze AS NumberOfStacks,
ISNULL(A2D.BaseQuantity,0) AS BaseLoadCarrierQuantity, 
A2D.SupportsRearLoading AS SupportsRearLoading, 
A2D.SupportsSideLoading AS SupportsSideLoading, 
A2D.SupportsJumboVehicles AS SupportsJumboVehicles, 
DOL.Id AS DemandId, 
SOL.Id AS SupplyId  
FROM @Avail2Delis A2D 
LEFT JOIN LMS.LMS_AVAILABILITY A2 ON A2.DeletionDate IS NULL AND A2D.AvailabilityID = A2.AvailabilityId
LEFT JOIN LMS.LMS_DELIVERY D2 ON D2.DeletionDate IS NULL AND A2D.DeliveryID = D2.DiliveryId
LEFT JOIN dbo.Orders S ON A2.RowGuid = S.RefLmsOrderRowGuid AND  S.[Type] = @OrderTypeSupply
LEFT JOIN dbo.Orders D ON D2.RowGuid = D.RefLmsOrderRowGuid AND D.[Type] = @OrderTypeDemand
LEFT JOIN @SupplyOrderLoad SOL ON S.Id = SOL.OrderId
LEFT JOIN @DemandOrderLoad DOL ON D.ID = DOL.OrderId

LEFT JOIN @LcMapping BLC ON (A2D.BasePalletTypeId != 0 AND A2D.BasePalletTypeId = BLC.RefLmsLoadCarrierTypeId AND  A2D.BaseQualityId = BLC.RefLmsQualityId)
INNER JOIN @LcMapping LC ON (A2D.PalletTypeId = LC.RefLmsLoadCarrierTypeId AND  A2D.QualityId  = LC.RefLmsQualityId)



END
GO
