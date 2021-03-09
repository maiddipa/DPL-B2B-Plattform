USE [master]
GO

-- ********** ACHTUNG Passwort muss noch eingetragen werden! *************************

/****** Object:  LinkedServer [MK56BN7128]    Script Date: 23.06.2020 09:57:50 ******/
EXEC master.dbo.sp_addlinkedserver @server = N'MK56BN7128', @srvproduct=N'', @provider=N'SQLNCLI', @datasrc=N'mk56bn7128.database.windows.net', @catalog=N'OlmaDbFait'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'MK56BN7128',@useself=N'False',@locallogin=NULL,@rmtuser=N'DplLtmsDbFaitUser',@rmtpassword='######'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'rpc', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'rpc out', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


