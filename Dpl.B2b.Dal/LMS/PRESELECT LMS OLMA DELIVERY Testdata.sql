﻿SELECT
	--[DiliveryId]
	[OldDate]
	,[Quantity]
	,[Quality]
	,[year]
	,[month]
	,[cw]
	,[ZipCode]
	,[TempAllocAvail]
	,[day]
	,[finished]
	,[ClientId]
	,[CountryId]
	,[CustomerId]
	,[DistributorId]
	,[LoadingPointId]
	,[CarrierId]
	,CONVERT(VARCHAR(33), [FromDate], 126) AS [FromDate]
	,CONVERT(VARCHAR(33), [UntilDate], 126) AS [UntilDate]
	,CONVERT(VARCHAR(33), [FromTime], 126) AS [FromTime]
	,CONVERT(VARCHAR(33), [UntilTime], 126) AS [UntilTime]
	,[Comment]
	,[IsFix]
	,[PalletType]
	,[ContractNo]
	,[DeliveryNoteNo]
	,[TransactionNo]
	,[CategoryId]
	,CONVERT(VARCHAR(33), [CreationDate], 126) AS [CreationDate]
	,NULL AS [ModificationDate]
	,[DeletionDate]
	,[CreatedBy]
	,NULL AS [ModifiedBy]
	,[DeletedBy]
	,[Rejection]
	,[Reklamation]
	,[RejectionReasonId]
	,[RejectionComment]
	,[RejectionQuantity]
	,[RejectionApproachId]
	,[RejectionBy]
	,[RejectionDate]
	,[RejectionDone]
	,[RejectionDoneComment]
	,[RejectionNextReceiptNo]
	,[RejectionDoneBy]
	,[RejectionDoneDate]
	,[RejectionCost]
	,[ContactPerson]
	,[ContactDetails]
	,[IsMixedDelivery]
	,[ManualDistributorAddress]
	,[IsSmallAmount]
	,[ZuAvisieren]
	,[IstAvisiert]
	,[AvisiertVon]
	,[AvisiertAm]
	,[LSManAdrAnrede]
	,[LSManAdrName1]
	,[LSManAdrName2]
	,[LSManAdrName3]
	,[LSManAdrPLZ]
	,[LSManAdrOrt]
	,[LSManAdrStrasse]
	,[LSManAdrLand]
	,[LAManAdrAnrede]
	,[LAManAdrName1]
	,[LAManAdrName2]
	,[LAManAdrName3]
	,[LAManAdrPLZ]
	,[LAManAdrOrt]
	,[LAManAdrStrasse]
	,[LAManAdrLand]
	,[DeliveryTime]
	,[FrachtauftragNo]
	,[Notiz]
	,[Kilometer]
	,[Frachtpreis]
	,[Prioritaet]
	,[Bestellnummer]
	--,[Geblockt]
	--,[GeblocktVon]
	--,[GeblocktDatum]
	--,[GeblocktAufgehobenVon]
	--,[GeblocktAufgehobenDatum]
	--,[GeblocktKommentar]
	--,[GeblocktFuer]
	--,[Storniert]
	--,[StorniertVon]
	--,[StorniertDatum]
	--,[StorniertKommentar]
	--,[InBearbeitungVon]
	--,[InBearbeitungDatumZeit]
	,0 AS [Revision]
	,[Fixmenge]
	,[Jumboladung]
	,[IstBedarf]
	,[BusinessTypeId]
	,[IstPlanmenge]
	,[Rahmenauftragsnummer]
	,[FvKoffer]
	,[FvPlane]
	,[FvStapelhoehe]
	,[FvVerladerichtungLaengs]
	,[DispoNotiz]
	,[OrderManagementID]
	,[Einzelpreis]
	,[ExterneNummer]
	--,[RowGuid]
	--,[VersionCol]
	,[StackHeightMin]
	,[StackHeightMax]
	,[SupportsRearLoading]
	,[SupportsSideLoading]
	,[SupportsJumboVehicles]
	,[SupportsIndividual]
	,[BasePalletTypeId]
	,[BaseQualityId]
	,[ShowOnline]
	,[IsCreatedOnline]
	,[NeedsBalanceApproval]
	,[BalanceApproved]
	,[OnlineComment]
	,[LiveDocumentNumber]
	,[ExpressCode]
	,[SupportsPartialMatching]
	,[HasBasePallets]
	,[LoadingLocationId]
FROM [LMS].[dbo].[LMS_DELIVERY]
WHERE CONVERT(DATE, CreationDate) = '04.06.2020'
	AND CreatedBy = 'schmidt'
	AND Quantity > 1
--WHERE CreatedBy = 'schmidt'
ORDER BY [DiliveryId] DESC
