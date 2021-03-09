BEGIN TRAN

SET IDENTITY_INSERT [dbo].[LMS_QUALITY] ON

IF NOT EXISTS (SELECT * FROM [dbo].[LMS_QUALITY] WHERE [QualityId] = 30)
INSERT INTO [dbo].[LMS_QUALITY]
           ([QualityId]
			  ,[Name]
           ,[ArtNrShortName]
           ,[QualityValue]
           ,[DisplayOrder]
           ,[CreationDate]
           ,[CreatedBy]
           ,[IstStandard])
     VALUES
           (30
			  ,'Intakt'
           ,'INT'
           ,7
           ,14
           ,'30.11.2020'
           ,'schmidt'
           ,1)

SET IDENTITY_INSERT [dbo].[LMS_QUALITY] OFF

--SELECT * FROM [dbo].[LMS_QUALITY]


SET IDENTITY_INSERT [dbo].[LMS_PALLETTYPE] ON

IF NOT EXISTS (SELECT * FROM [dbo].[LMS_PALLETTYPE] WHERE [PalletTypeId] = 13)
INSERT INTO [dbo].[LMS_PALLETTYPE]
           ([PalletTypeId]
			  ,[Name]
           ,[ShortName]
           ,[ArtNrShortName]
           ,[IsMixType]
           ,[CreationDate]
           ,[CreatedBy]
           ,[IsBasePallet]
           ,[QuantityPerEur]
           ,[BaseLoadCarrierRequirement])
     VALUES
           (13
			  ,'ORBIS Gitterpak Deckel'
           ,'HDB1208D'
           ,'OR'
           ,0
           ,'30.11.2020'
           ,'schmidt'
           ,0
           ,1
           ,0)

IF NOT EXISTS (SELECT * FROM [dbo].[LMS_PALLETTYPE] WHERE [PalletTypeId] = 14)
INSERT INTO [dbo].[LMS_PALLETTYPE]
           ([PalletTypeId]
			  ,[Name]
           ,[ShortName]
           ,[ArtNrShortName]
           ,[IsMixType]
           ,[CreationDate]
           ,[CreatedBy]
           ,[IsBasePallet]
           ,[QuantityPerEur]
           ,[BaseLoadCarrierRequirement])
     VALUES
           (14
			  ,'ORBIS Gitterpak mit Deckel'
           ,'HDB1208mD'
           ,'OD'
           ,0
           ,'30.11.2020'
           ,'schmidt'
           ,0
           ,1
           ,0)

IF NOT EXISTS (SELECT * FROM [dbo].[LMS_PALLETTYPE] WHERE [PalletTypeId] = 15)
INSERT INTO [dbo].[LMS_PALLETTYPE]
           ([PalletTypeId]
			  ,[Name]
           ,[ShortName]
           ,[ArtNrShortName]
           ,[IsMixType]
           ,[CreationDate]
           ,[CreatedBy]
           ,[IsBasePallet]
           ,[QuantityPerEur]
           ,[BaseLoadCarrierRequirement])
     VALUES
           (15
			  ,'EPAL7'
           ,'EPAL7'
           ,'E7'
           ,0
           ,'30.11.2020'
           ,'schmidt'
           ,0
           ,2
           ,1)

SET IDENTITY_INSERT [dbo].[LMS_PALLETTYPE] OFF

--SELECT * FROM [dbo].[LMS_PALLETTYPE]


IF NOT EXISTS (SELECT * FROM [dbo].[LMS_QUALI2PALLET] WHERE [PalletTypeId] = 13 AND [QualityId] = 2)
BEGIN
INSERT INTO [dbo].[LMS_QUALI2PALLET]([QualityId],[PalletTypeId],[CreationDate],[CreatedBy]) VALUES(5, 13, '30.11.2020', 'schmidt')
INSERT INTO [dbo].[LMS_QUALI2PALLET]([QualityId],[PalletTypeId],[CreationDate],[CreatedBy]) VALUES(30, 13, '30.11.2020', 'schmidt')
INSERT INTO [dbo].[LMS_QUALI2PALLET]([QualityId],[PalletTypeId],[CreationDate],[CreatedBy]) VALUES(2, 13, '30.11.2020', 'schmidt')

INSERT INTO [dbo].[LMS_QUALI2PALLET]([QualityId],[PalletTypeId],[CreationDate],[CreatedBy]) VALUES(5, 14, '30.11.2020', 'schmidt')
INSERT INTO [dbo].[LMS_QUALI2PALLET]([QualityId],[PalletTypeId],[CreationDate],[CreatedBy]) VALUES(30, 14, '30.11.2020', 'schmidt')
INSERT INTO [dbo].[LMS_QUALI2PALLET]([QualityId],[PalletTypeId],[CreationDate],[CreatedBy]) VALUES(2, 14, '30.11.2020', 'schmidt')
END

IF NOT EXISTS (SELECT * FROM [dbo].[LMS_QUALI2PALLET] WHERE [PalletTypeId] = 15 AND [QualityId] = 5)
BEGIN
INSERT INTO [dbo].[LMS_QUALI2PALLET]([QualityId],[PalletTypeId],[CreationDate],[CreatedBy]) VALUES(5, 15, '30.11.2020', 'schmidt')
END
--SELECT * FROM [LMS_QUALI2PALLET] ORDER BY dbo.LMS_QUALI2PALLET.PalletTypeId, dbo.LMS_QUALI2PALLET.QualityId

COMMIT TRAN
