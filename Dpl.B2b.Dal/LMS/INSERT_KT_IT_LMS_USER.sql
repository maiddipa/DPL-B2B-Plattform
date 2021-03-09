USE [LMS_Demo]
GO

IF NOT EXISTS (SELECT lb.Benutzername FROM dbo.LMS_BENUTZER lb WHERE lb.Benutzername = 'kt-it-user01')
INSERT INTO [dbo].[LMS_BENUTZER]
           ([Benutzername]
           ,[Name]
           ,[Vorname]
           ,[Gruppe_ID]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[EMail])
     VALUES
           ('kt-it-user01'
           ,'KT'
           ,'01'
           ,7
           ,'15.07.2020'
           ,NULL
           ,NULL
           ,'schmidt'
           ,NULL
           ,NULL
           ,'kt-it-user01@dpl-pooling.com')

IF NOT EXISTS (SELECT lb.Benutzername FROM dbo.LMS_BENUTZER lb WHERE lb.Benutzername = 'kt-it-user02')
INSERT INTO [dbo].[LMS_BENUTZER]
           ([Benutzername]
           ,[Name]
           ,[Vorname]
           ,[Gruppe_ID]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[EMail])
     VALUES
           ('kt-it-user02'
           ,'KT'
           ,'02'
           ,7
           ,'15.07.2020'
           ,NULL
           ,NULL
           ,'schmidt'
           ,NULL
           ,NULL
           ,'kt-it-user02@dpl-pooling.com')

IF NOT EXISTS (SELECT lb.Benutzername FROM dbo.LMS_BENUTZER lb WHERE lb.Benutzername = 'kt-it-user03')
INSERT INTO [dbo].[LMS_BENUTZER]
           ([Benutzername]
           ,[Name]
           ,[Vorname]
           ,[Gruppe_ID]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[EMail])
     VALUES
           ('kt-it-user03'
           ,'KT'
           ,'03'
           ,7
           ,'15.07.2020'
           ,NULL
           ,NULL
           ,'schmidt'
           ,NULL
           ,NULL
           ,'kt-it-user03@dpl-pooling.com')

IF NOT EXISTS (SELECT lb.Benutzername FROM dbo.LMS_BENUTZER lb WHERE lb.Benutzername = 'kt-it-user04')
INSERT INTO [dbo].[LMS_BENUTZER]
           ([Benutzername]
           ,[Name]
           ,[Vorname]
           ,[Gruppe_ID]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[EMail])
     VALUES
           ('kt-it-user04'
           ,'KT'
           ,'04'
           ,7
           ,'15.07.2020'
           ,NULL
           ,NULL
           ,'schmidt'
           ,NULL
           ,NULL
           ,'kt-it-user04@dpl-pooling.com')

