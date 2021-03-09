USE [master]
GO

/****** Object:  LinkedServer [MK56BN7128_Test]    Script Date: 23.10.2020 07:51:32 ******/
EXEC master.dbo.sp_addlinkedserver @server = N'MK56BN7128_Test', @srvproduct=N'', @provider=N'SQLNCLI', @datasrc=N'mk56bn7128.database.windows.net', @catalog=N'OlmaDbTest'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'MK56BN7128_Test',@useself=N'False',@locallogin=NULL,@rmtuser=N'DplLtmsDbFaitUser',@rmtpassword='Rm4TA87KLq4tG45p'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'rpc', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'rpc out', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'MK56BN7128_Test', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


