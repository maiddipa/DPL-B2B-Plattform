USE [master]
GO

-- ********** ACHTUNG Passwort muss noch eingetragen werden! *************************

/****** Object:  LinkedServer [BMFCH874FJ]    Script Date: 23.06.2020 09:57:50 ******/
EXEC master.dbo.sp_addlinkedserver @server = N'BMFCH874FJ', @srvproduct=N'', @provider=N'SQLNCLI', @datasrc=N'bmfch874fj.database.windows.net', @catalog=N'OlmaDbDvit'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'BMFCH874FJ',@useself=N'False',@locallogin=NULL,@rmtuser=N'DplLtmsDbDvitUser',@rmtpassword='########'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'rpc', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'rpc out', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'BMFCH874FJ', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


