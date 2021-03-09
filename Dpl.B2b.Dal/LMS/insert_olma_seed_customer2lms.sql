BEGIN TRAN

--USE [LMS]
GO
DECLARE @CustomerID int;

-- Handel Headquarter
SET @CustomerID = 25896;
IF NOT EXISTS (SELECT AdressNr FROM [dbo].[LMS_CUSTOMER] WHERE [AdressNr] = @CustomerID)
BEGIN
INSERT INTO [dbo].[LMS_CUSTOMER]
           ([AdressNr]
           ,[ClientID]
           ,[Anrede]
           ,[Kontoname]
           ,[Strasse]
           ,[LKZ]
           ,[PLZ]
           ,[Ort]
           ,[Land]
           ,[Telefon]
           ,[Fax]
           ,[EMail]
			  )
     VALUES
           (@CustomerID --<AdressNr, int,>
           ,1 --<ClientID, int,>
           ,'' --<Anrede, nvarchar(255),>
           ,'Handel Headquarter' --<Kontoname, nvarchar(255),>
           ,'Raiffeisenstraße 5' --<Strasse, nvarchar(255),>
           ,NULL --<LKZ, nvarchar(255),>
           ,'58638' --<PLZ, nvarchar(255),>
           ,'Iserlohn' --<Ort, nvarchar(255),>
           ,'Deutschland' --<Land, nvarchar(255),>
           ,'' --<Telefon, nvarchar(255),>
           ,'' --<Fax, nvarchar(255),>
           ,'' --<EMail, nvarchar(255),>
           )
END

-- Dpl Zentrale
SET @CustomerID = 55896;
IF NOT EXISTS (SELECT AdressNr FROM [dbo].[LMS_CUSTOMER] WHERE [AdressNr] = @CustomerID)
BEGIN
INSERT INTO [dbo].[LMS_CUSTOMER]
           ([AdressNr]
           ,[ClientID]
           ,[Anrede]
           ,[Kontoname]
           ,[Strasse]
           ,[LKZ]
           ,[PLZ]
           ,[Ort]
           ,[Land]
           ,[Telefon]
           ,[Fax]
           ,[EMail]
			  )
     VALUES
           (@CustomerID --<AdressNr, int,>
           ,1 --<ClientID, int,>
           ,'' --<Anrede, nvarchar(255),>
           ,'Dpl Zentrale' --<Kontoname, nvarchar(255),>
           ,'Overweg 12' --<Strasse, nvarchar(255),>
           ,NULL --<LKZ, nvarchar(255),>
           ,'59494' --<PLZ, nvarchar(255),>
           ,'Soest' --<Ort, nvarchar(255),>
           ,'Deutschland' --<Land, nvarchar(255),>
           ,'' --<Telefon, nvarchar(255),>
           ,'' --<Fax, nvarchar(255),>
           ,'' --<EMail, nvarchar(255),>
           )
END

-- Lieferant 1
SET @CustomerID = 99874;
IF NOT EXISTS (SELECT AdressNr FROM [dbo].[LMS_CUSTOMER] WHERE [AdressNr] = @CustomerID)
BEGIN
INSERT INTO [dbo].[LMS_CUSTOMER]
           ([AdressNr]
           ,[ClientID]
           ,[Anrede]
           ,[Kontoname]
           ,[Strasse]
           ,[LKZ]
           ,[PLZ]
           ,[Ort]
           ,[Land]
           ,[Telefon]
           ,[Fax]
           ,[EMail]
			  )
     VALUES
           (@CustomerID --<AdressNr, int,>
           ,1 --<ClientID, int,>
           ,'' --<Anrede, nvarchar(255),>
           ,'Lieferant 1' --<Kontoname, nvarchar(255),>
           ,'Loading street 3' --<Strasse, nvarchar(255),>
           ,NULL --<LKZ, nvarchar(255),>
           ,'99999' --<PLZ, nvarchar(255),>
           ,'Loading city 3' --<Ort, nvarchar(255),>
           ,'Deutschland' --<Land, nvarchar(255),>
           ,'' --<Telefon, nvarchar(255),>
           ,'' --<Fax, nvarchar(255),>
           ,'' --<EMail, nvarchar(255),>
           )
END

-- Depot 1
SET @CustomerID = 78566;
IF NOT EXISTS (SELECT AdressNr FROM [dbo].[LMS_CUSTOMER] WHERE [AdressNr] = @CustomerID)
BEGIN
INSERT INTO [dbo].[LMS_CUSTOMER]
           ([AdressNr]
           ,[ClientID]
           ,[Anrede]
           ,[Kontoname]
           ,[Strasse]
           ,[LKZ]
           ,[PLZ]
           ,[Ort]
           ,[Land]
           ,[Telefon]
           ,[Fax]
           ,[EMail]
			  )
     VALUES
           (@CustomerID --<AdressNr, int,>
           ,1 --<ClientID, int,>
           ,'' --<Anrede, nvarchar(255),>
           ,'Depot 1' --<Kontoname, nvarchar(255),>
           ,'Zur Hubertushalle 4' --<Strasse, nvarchar(255),>
           ,NULL --<LKZ, nvarchar(255),>
           ,'59846' --<PLZ, nvarchar(255),>
           ,'Sundern' --<Ort, nvarchar(255),>
           ,'Deutschland' --<Land, nvarchar(255),>
           ,'' --<Telefon, nvarchar(255),>
           ,'' --<Fax, nvarchar(255),>
           ,'' --<EMail, nvarchar(255),>
           )
END

-- Spedition 1
SET @CustomerID = 11472;
IF NOT EXISTS (SELECT AdressNr FROM [dbo].[LMS_CUSTOMER] WHERE [AdressNr] = @CustomerID)
BEGIN
INSERT INTO [dbo].[LMS_CUSTOMER]
           ([AdressNr]
           ,[ClientID]
           ,[Anrede]
           ,[Kontoname]
           ,[Strasse]
           ,[LKZ]
           ,[PLZ]
           ,[Ort]
           ,[Land]
           ,[Telefon]
           ,[Fax]
           ,[EMail]
			  )
     VALUES
           (@CustomerID --<AdressNr, int,>
           ,1 --<ClientID, int,>
           ,'' --<Anrede, nvarchar(255),>
           ,'Spedition 1' --<Kontoname, nvarchar(255),>
           ,'Romstraße 1' --<Strasse, nvarchar(255),>
           ,NULL --<LKZ, nvarchar(255),>
           ,'97424' --<PLZ, nvarchar(255),>
           ,'Schweinfurt' --<Ort, nvarchar(255),>
           ,'Deutschland' --<Land, nvarchar(255),>
           ,'' --<Telefon, nvarchar(255),>
           ,'' --<Fax, nvarchar(255),>
           ,'' --<EMail, nvarchar(255),>
           )
END

COMMIT TRAN

