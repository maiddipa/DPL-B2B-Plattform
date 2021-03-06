USE [master]
GO
/****** Object:  Database [LMS]    Script Date: 29/01/2020 12:07:40 ******/
CREATE DATABASE [LMS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LMS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\LMS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LMS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\LMS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [LMS] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LMS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LMS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LMS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LMS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LMS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LMS] SET ARITHABORT OFF 
GO
ALTER DATABASE [LMS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LMS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LMS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LMS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LMS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LMS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LMS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LMS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LMS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LMS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LMS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LMS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LMS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LMS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LMS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LMS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LMS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LMS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LMS] SET  MULTI_USER 
GO
ALTER DATABASE [LMS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LMS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LMS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LMS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LMS] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LMS] SET QUERY_STORE = OFF
GO
USE [LMS]
GO
/****** Object:  Schema [DPL-SOEST\Fitze]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\Fitze]
GO
/****** Object:  Schema [DPL-SOEST\Gerasch]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\Gerasch]
GO
/****** Object:  Schema [DPL-SOEST\Klaass]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\Klaass]
GO
/****** Object:  Schema [DPL-SOEST\lemsky]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\lemsky]
GO
/****** Object:  Schema [DPL-SOEST\Oevermann]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\Oevermann]
GO
/****** Object:  Schema [DPL-SOEST\Pauli]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\Pauli]
GO
/****** Object:  Schema [DPL-SOEST\Preciado]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\Preciado]
GO
/****** Object:  Schema [DPL-SOEST\Richert]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\Richert]
GO
/****** Object:  Schema [DPL-SOEST\schroeter]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\schroeter]
GO
/****** Object:  Schema [DPL-SOEST\Trojan]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [DPL-SOEST\Trojan]
GO
/****** Object:  Schema [fleper]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [fleper]
GO
/****** Object:  Schema [kaese]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [kaese]
GO
/****** Object:  Schema [Kruse]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [Kruse]
GO
/****** Object:  Schema [lammers]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [lammers]
GO
/****** Object:  Schema [Revinsek]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [Revinsek]
GO
/****** Object:  Schema [Schauerte]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [Schauerte]
GO
/****** Object:  Schema [Wawra]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [Wawra]
GO
/****** Object:  Schema [welge]    Script Date: 29/01/2020 12:07:41 ******/
CREATE SCHEMA [welge]
GO
/****** Object:  UserDefinedFunction [dbo].[GetDistanceFromCoords]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--/****** Object:  User [wertlos]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [wertlos] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [OM_Service]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [OM_Service] FOR LOGIN [OM_Service] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [lmsadmin]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [lmsadmin] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [GSDadmin]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [GSDadmin] FOR LOGIN [GSDadmin] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [ERPFrame]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [ERPFrame] FOR LOGIN [ERPFrame] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Zaerle]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Zaerle] FOR LOGIN [DPL-SOEST\Zaerle] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Wietfeld]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Wietfeld] FOR LOGIN [DPL-SOEST\Wietfeld] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Wiegard]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Wiegard] FOR LOGIN [DPL-SOEST\Wiegard] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Wenke]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Wenke] FOR LOGIN [DPL-SOEST\Wenke] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Welge]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Welge] FOR LOGIN [DPL-SOEST\Welge] WITH DEFAULT_SCHEMA=[welge]
--GO
--/****** Object:  User [DPL-SOEST\Wawra]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Wawra] FOR LOGIN [DPL-SOEST\Wawra] WITH DEFAULT_SCHEMA=[Wawra]
--GO
--/****** Object:  User [DPL-SOEST\Uhlenbrock]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Uhlenbrock] FOR LOGIN [DPL-SOEST\Uhlenbrock] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Trojan]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Trojan] FOR LOGIN [DPL-SOEST\Trojan] WITH DEFAULT_SCHEMA=[DPL-SOEST\Trojan]
--GO
--/****** Object:  User [DPL-SOEST\Sons]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Sons] FOR LOGIN [DPL-SOEST\sons] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Siepe]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Siepe] FOR LOGIN [DPL-SOEST\Siepe] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\schroeter]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\schroeter] FOR LOGIN [DPL-SOEST\schroeter] WITH DEFAULT_SCHEMA=[DPL-SOEST\schroeter]
--GO
--/****** Object:  User [DPL-SOEST\schmidt]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\schmidt] FOR LOGIN [DPL-SOEST\schmidt] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Schmal]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Schmal] FOR LOGIN [DPL-SOEST\Schmal] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Schloesser]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Schloesser] FOR LOGIN [DPL-SOEST\Schloesser] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Schlicht]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Schlicht] FOR LOGIN [DPL-SOEST\Schlicht] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Schauerte]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Schauerte] FOR LOGIN [DPL-SOEST\Schauerte] WITH DEFAULT_SCHEMA=[Schauerte]
--GO
--/****** Object:  User [DPL-SOEST\Schaefers]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Schaefers] FOR LOGIN [DPL-SOEST\Schaefers] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\risse]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\risse] FOR LOGIN [DPL-SOEST\risse] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Rippel]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Rippel] FOR LOGIN [DPL-SOEST\Rippel] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Rietze]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Rietze] FOR LOGIN [DPL-SOEST\rietze] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Richert]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Richert] FOR LOGIN [DPL-SOEST\Richert] WITH DEFAULT_SCHEMA=[DPL-SOEST\Richert]
--GO
--/****** Object:  User [DPL-SOEST\Revinsek]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Revinsek] FOR LOGIN [DPL-SOEST\Revinsek] WITH DEFAULT_SCHEMA=[Revinsek]
--GO
--/****** Object:  User [DPL-SOEST\Remken]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Remken] FOR LOGIN [DPL-SOEST\Remken] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Preciado]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Preciado] FOR LOGIN [DPL-SOEST\Preciado] WITH DEFAULT_SCHEMA=[DPL-SOEST\Preciado]
--GO
--/****** Object:  User [DPL-SOEST\Pauli]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Pauli] FOR LOGIN [DPL-SOEST\Pauli] WITH DEFAULT_SCHEMA=[DPL-SOEST\Pauli]
--GO
--/****** Object:  User [DPL-SOEST\Oevermann]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Oevermann] FOR LOGIN [DPL-SOEST\Oevermann] WITH DEFAULT_SCHEMA=[DPL-SOEST\Oevermann]
--GO
--/****** Object:  User [DPL-SOEST\Noortgate]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Noortgate] FOR LOGIN [DPL-SOEST\Noortgate] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Millentrup]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Millentrup] FOR LOGIN [DPL-SOEST\Millentrup] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Meinhardt]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Meinhardt] FOR LOGIN [DPL-SOEST\Meinhardt] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Luff]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Luff] FOR LOGIN [DPL-SOEST\Luff] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\LMS Benutzer]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\LMS Benutzer] FOR LOGIN [DPL-SOEST\LMS Benutzer]
--GO
--/****** Object:  User [DPL-SOEST\lemsky]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\lemsky] FOR LOGIN [DPL-SOEST\lemsky] WITH DEFAULT_SCHEMA=[DPL-SOEST\lemsky]
--GO
--/****** Object:  User [DPL-SOEST\Lehr]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Lehr] FOR LOGIN [DPL-SOEST\Lehr] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Lammers]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Lammers] FOR LOGIN [DPL-SOEST\Lammers] WITH DEFAULT_SCHEMA=[lammers]
--GO
--/****** Object:  User [DPL-SOEST\Ladham]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Ladham] FOR LOGIN [DPL-SOEST\Ladham] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Kuehler]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Kuehler] FOR LOGIN [DPL-SOEST\Kuehler] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Kruse]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Kruse] FOR LOGIN [DPL-SOEST\Kruse] WITH DEFAULT_SCHEMA=[Kruse]
--GO
--/****** Object:  User [DPL-SOEST\Krause]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Krause] FOR LOGIN [DPL-SOEST\Krause] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Kloster]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Kloster] FOR LOGIN [DPL-SOEST\Kloster] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Klaass]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Klaass] FOR LOGIN [DPL-SOEST\Klaass] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Kirchhoff]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Kirchhoff] FOR LOGIN [DPL-SOEST\Kirchhoff] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Kaese]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Kaese] FOR LOGIN [DPL-SOEST\Kaese] WITH DEFAULT_SCHEMA=[kaese]
--GO
--/****** Object:  User [DPL-SOEST\Hillebrand]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Hillebrand] FOR LOGIN [DPL-SOEST\Hillebrand] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Habesyan]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Habesyan] FOR LOGIN [DPL-SOEST\Habesyan] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Gerasch]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Gerasch] FOR LOGIN [DPL-SOEST\Gerasch] WITH DEFAULT_SCHEMA=[DPL-SOEST\Gerasch]
--GO
--/****** Object:  User [DPL-SOEST\Fleper]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Fleper] FOR LOGIN [DPL-SOEST\Fleper] WITH DEFAULT_SCHEMA=[fleper]
--GO
--/****** Object:  User [DPL-SOEST\Fitze]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Fitze] FOR LOGIN [DPL-SOEST\Fitze] WITH DEFAULT_SCHEMA=[DPL-SOEST\Fitze]
--GO
--/****** Object:  User [DPL-SOEST\Eulentrop]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Eulentrop] FOR LOGIN [DPL-SOEST\Eulentrop] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\ebbesmeier]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\ebbesmeier] FOR LOGIN [DPL-SOEST\ebbesmeier] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Drengk]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Drengk] FOR LOGIN [DPL-SOEST\Drengk] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\DPLReporting]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\DPLReporting] FOR LOGIN [DPL-SOEST\DPLReporting]
--GO
--/****** Object:  User [DPL-SOEST\dev-user]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\dev-user] FOR LOGIN [DPL-SOEST\dev-user] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Boecker]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Boecker] FOR LOGIN [DPL-SOEST\Boecker] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  User [DPL-SOEST\Bock]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE USER [DPL-SOEST\Bock] FOR LOGIN [DPL-SOEST\Bock] WITH DEFAULT_SCHEMA=[dbo]
--GO
--/****** Object:  DatabaseRole [LMS_StandardBenutzer]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE ROLE [LMS_StandardBenutzer]
--GO
--/****** Object:  DatabaseRole [DPLReportingUser]    Script Date: 01.10.2019 11:04:22 ******/
--CREATE ROLE [DPLReportingUser]
--GO
--ALTER ROLE [db_owner] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_accessadmin] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_securityadmin] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_ddladmin] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_backupoperator] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_denydatareader] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_denydatawriter] ADD MEMBER [wertlos]
--GO
--ALTER ROLE [db_owner] ADD MEMBER [lmsadmin]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [lmsadmin]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [lmsadmin]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [GSDadmin]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [GSDadmin]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [GSDadmin]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Zaerle]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Zaerle]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Zaerle]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Wietfeld]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Wietfeld]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Wietfeld]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Wiegard]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Wiegard]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Wiegard]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Wenke]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Wenke]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Wenke]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Welge]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Welge]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Welge]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Wawra]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Wawra]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Wawra]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Uhlenbrock]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Uhlenbrock]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Uhlenbrock]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Trojan]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Trojan]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Trojan]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Sons]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Sons]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Sons]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Siepe]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Siepe]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Siepe]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\schroeter]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\schroeter]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\schroeter]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\schmidt]
--GO
--ALTER ROLE [db_owner] ADD MEMBER [DPL-SOEST\schmidt]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\schmidt]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\schmidt]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Schmal]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Schmal]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Schmal]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Schloesser]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Schloesser]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Schloesser]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Schlicht]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Schlicht]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Schlicht]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Schauerte]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Schauerte]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Schauerte]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Schaefers]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Schaefers]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Schaefers]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\risse]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\risse]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\risse]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Rippel]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Rippel]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Rippel]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Rietze]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Rietze]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Rietze]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Richert]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Richert]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Richert]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Revinsek]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Revinsek]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Revinsek]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Remken]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Remken]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Remken]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Preciado]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Preciado]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Preciado]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Pauli]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Pauli]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Pauli]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Oevermann]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Oevermann]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Oevermann]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Noortgate]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Noortgate]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Noortgate]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Millentrup]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Millentrup]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Millentrup]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Meinhardt]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Meinhardt]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Meinhardt]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Luff]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Luff]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Luff]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\LMS Benutzer]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\LMS Benutzer]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\LMS Benutzer]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\lemsky]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\lemsky]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\lemsky]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Lehr]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Lehr]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Lehr]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Lammers]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Lammers]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Lammers]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Ladham]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Kruse]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Kruse]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Kruse]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Krause]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Krause]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Krause]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Kloster]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Kloster]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Kloster]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Klaass]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Klaass]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Klaass]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Kirchhoff]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Kirchhoff]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Kirchhoff]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Kaese]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Kaese]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Kaese]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Hillebrand]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Hillebrand]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Hillebrand]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Habesyan]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Habesyan]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Habesyan]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Gerasch]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Gerasch]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Gerasch]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Fleper]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Fleper]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Fleper]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Fitze]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Fitze]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Fitze]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Eulentrop]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Eulentrop]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Eulentrop]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\ebbesmeier]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\ebbesmeier]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\ebbesmeier]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Drengk]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Drengk]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Drengk]
--GO
--ALTER ROLE [DPLReportingUser] ADD MEMBER [DPL-SOEST\DPLReporting]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\DPLReporting]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\dev-user]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\dev-user]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\dev-user]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Boecker]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Boecker]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Boecker]
--GO
--ALTER ROLE [LMS_StandardBenutzer] ADD MEMBER [DPL-SOEST\Bock]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPL-SOEST\Bock]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [DPL-SOEST\Bock]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [LMS_StandardBenutzer]
--GO
--ALTER ROLE [db_datawriter] ADD MEMBER [LMS_StandardBenutzer]
--GO
--ALTER ROLE [db_datareader] ADD MEMBER [DPLReportingUser]
--GO
--/****** Object:  Schema [DPL-SOEST\Fitze]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\Fitze]
--GO
--/****** Object:  Schema [DPL-SOEST\Gerasch]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\Gerasch]
--GO
--/****** Object:  Schema [DPL-SOEST\Klaass]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\Klaass]
--GO
--/****** Object:  Schema [DPL-SOEST\lemsky]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\lemsky]
--GO
--/****** Object:  Schema [DPL-SOEST\Oevermann]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\Oevermann]
--GO
--/****** Object:  Schema [DPL-SOEST\Pauli]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\Pauli]
--GO
--/****** Object:  Schema [DPL-SOEST\Preciado]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\Preciado]
--GO
--/****** Object:  Schema [DPL-SOEST\Richert]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\Richert]
--GO
--/****** Object:  Schema [DPL-SOEST\schroeter]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\schroeter]
--GO
--/****** Object:  Schema [DPL-SOEST\Trojan]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [DPL-SOEST\Trojan]
--GO
--/****** Object:  Schema [fleper]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [fleper]
--GO
--/****** Object:  Schema [kaese]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [kaese]
--GO
--/****** Object:  Schema [Kruse]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [Kruse]
--GO
--/****** Object:  Schema [lammers]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [lammers]
--GO
--/****** Object:  Schema [Revinsek]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [Revinsek]
--GO
--/****** Object:  Schema [Schauerte]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [Schauerte]
--GO
--/****** Object:  Schema [Wawra]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [Wawra]
--GO
--/****** Object:  Schema [welge]    Script Date: 01.10.2019 11:04:23 ******/
--CREATE SCHEMA [welge]
--GO
--/****** Object:  UserDefinedFunction [dbo].[GetDistanceFromCoords]    Script Date: 01.10.2019 11:04:23 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetDistanceFromCoords] 
(
	-- Add the parameters for the function here
	-- Add the parameters for the stored procedure here
	@Lat1 float, 
	@Lon1 float,
	@Lat2 float, 
	@Lon2 float
)
RETURNS float
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar float;

    SET @ResultVar = ACOS(SIN(@Lat1 * PI() / 180) * SIN(@Lat2 * PI() / 180) + COS(@Lat1 * PI() / 180) * COS(@Lat2 * PI() / 180) 
                                                   * COS(@Lon1 * PI() / 180 - @Lon2 * PI() / 180)) * 6380;
	-- Return the result of the function
	RETURN @ResultVar;

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetDistanceFromZipcodes]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetDistanceFromZipcodes] 
(
	-- Add the parameters for the function here
	@PLZ1 varchar(10),
	@PLZ2 varchar(10)
)
RETURNS int
AS
BEGIN
DECLARE @Distance INT

DECLARE @c1lon float;
DECLARE @c1lat float;
DECLARE @c2lon float;
DECLARE @c2lat float;

SET @PLZ1 = RTRIM(@PLZ1)
SET @PLZ2 = RTRIM(@PLZ2)

SELECT TOP 1 @c1lon = coords1.lon, @c1lat = coords1.lat
   FROM geo_coordinates AS coords1
      , geo_textdata AS td1
   WHERE coords1.loc_id = td1.loc_id
--      and td1.text_val like @PLZ1 + '%'
      and td1.text_val = @PLZ1
      and td1.text_type = '500300000';

SELECT @c2lon = coords2.lon, @c2lat = coords2.lat
   FROM geo_coordinates AS coords2 
      , geo_textdata AS td2
   WHERE coords2.loc_id = td2.loc_id
--      and td2.text_val like @PLZ2 + '%'
      and td2.text_val = @PLZ2
      and td2.text_type = '500300000';

SET @Distance = 
   CASE WHEN @c1lon IS NULL OR @c2lon IS NULL 
   THEN 99999 
   ELSE dbo.GetDistanceFromCoords(@c1lat, @c1lon, @c2lat, @c2lon)
   END

RETURN @Distance
END



GO
/****** Object:  UserDefinedFunction [dbo].[Kalenderwoche]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[Kalenderwoche] 
(
	-- Add the parameters for the function here
	@DateVar DateTime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ISOweek int;

	SELECT @ISOweek = DATEPART(WK, @DateVar) + 1 -DATEPART(wk, 'Jan 04,' + CAST(DATEPART(yy,@DateVar) AS CHAR(4))) ;
	IF @ISOweek = 0
	   SELECT @ISOweek = DATEPART(wk, CAST(24 + DATEPART(day, @DateVar) AS CHAR(2))+ '.12.' + CAST(DATEPART(yy,@DateVar) - 1 AS CHAR(4))) + 1;

	-- Return the result of the function
	RETURN @ISOweek;
END
GO
/****** Object:  Table [dbo].[LMS_AVAILABILITY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_AVAILABILITY](
	[AvailabilityId] [int] IDENTITY(5,1) NOT NULL,
	[ZipCode] [varchar](10) NULL,
	[Quantity] [int] NOT NULL,
	[TempAlloc] [bit] NULL,
	[TempReserved] [bit] NULL,
	[BBD] [datetime] NULL,
	[IsManual] [bit] NOT NULL,
	[TypeId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
	[Reliability] [int] NULL,
	[ReliabilityQuantity] [int] NULL,
	[ReliabilityQuality] [int] NULL,
	[AvailableFromDate] [datetime] NULL,
	[AvailableUntilDate] [datetime] NULL,
	[AvailableFromTime] [datetime] NULL,
	[AvailableUntilTime] [datetime] NULL,
	[ContactId] [int] NULL,
	[LoadingPointId] [int] NULL,
	[QualityId] [int] NULL,
	[AvailabilityTypeId] [int] NULL,
	[CommentText] [varchar](1000) NULL,
	[CountryId] [int] NULL,
	[AvailabilitySubTypeId] [int] NULL,
	[PalletTypeId] [int] NULL,
	[ContactAddressInfo] [varchar](255) NULL,
	[ContractNo] [varchar](32) NULL,
	[DeliveryNoteNo] [varchar](32) NULL,
	[TransactionNo] [varchar](32) NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
	[ContactPerson] [varchar](64) NULL,
	[ContactDetails] [varchar](64) NULL,
	[RepositoryHours] [varchar](128) NULL,
	[finished] [bit] NOT NULL,
	[IsMixedAvailability] [bit] NOT NULL,
	[LSManAdrAnrede] [varchar](50) NULL,
	[LSManAdrName1] [varchar](128) NULL,
	[LSManAdrName2] [varchar](128) NULL,
	[LSManAdrName3] [varchar](128) NULL,
	[LSManAdrPLZ] [varchar](15) NULL,
	[LSManAdrOrt] [varchar](128) NULL,
	[LSManAdrStrasse] [varchar](128) NULL,
	[LSManAdrLand] [varchar](50) NULL,
	[PalettenProMonat] [int] NOT NULL,
	[PalettenProWoche] [int] NOT NULL,
	[Notiz] [varchar](1024) NULL,
	[Geblockt] [bit] NULL,
	[GeblocktVon] [varchar](32) NULL,
	[GeblocktDatum] [datetime] NULL,
	[GeblocktAufgehobenVon] [varchar](32) NULL,
	[GeblocktAufgehobenDatum] [datetime] NULL,
	[GeblocktKommentar] [varchar](1024) NULL,
	[GeblocktFuer] [varchar](32) NULL,
	[InBearbeitungVon] [varchar](32) NULL,
	[InBearbeitungDatumZeit] [datetime] NULL,
	[Revision] [int] NULL,
	[Storniert] [bit] NULL,
	[StorniertVon] [varchar](32) NULL,
	[StorniertDatum] [datetime] NULL,
	[StorniertKommentar] [varchar](1024) NULL,
	[Rejection] [bit] NULL,
	[Reklamation] [bit] NULL,
	[RejectionReasonID] [int] NULL,
	[RejectionComment] [varchar](1024) NULL,
	[RejectionQuantity] [int] NULL,
	[RejectionApproachID] [int] NULL,
	[RejectionBy] [varchar](32) NULL,
	[RejectionDate] [datetime] NULL,
	[RejectionDone] [bit] NULL,
	[RejectionDoneComment] [varchar](1024) NULL,
	[RejectionNextReceiptNo] [varchar](64) NULL,
	[RejectionDoneBy] [varchar](32) NULL,
	[RejectionDoneDate] [datetime] NULL,
	[RejectionCost] [float] NULL,
	[ICDroht] [bit] NULL,
	[ICDrohtVon] [varchar](32) NULL,
	[ICDrohtAm] [datetime] NULL,
	[ICDurchfuehren] [bit] NULL,
	[ICDurchfuehrenVon] [varchar](32) NULL,
	[ICDurchfuehrenAm] [datetime] NULL,
	[ZuAvisieren] [bit] NULL,
	[IstAvisiert] [bit] NULL,
	[AvisiertVon] [varchar](32) NULL,
	[AvisiertAm] [datetime] NULL,
	[IstAusAblehnung] [bit] NULL,
	[IcGrundID] [int] NULL,
	[OrderManagementID] [varchar](20) NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_LMS_AVAILABILITY] PRIMARY KEY CLUSTERED 
(
	[AvailabilityId] ASC,
	[IsManual] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_AVAILABILITY_LTV]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_AVAILABILITY_LTV](
	[AvailabilityId] [int] NOT NULL,
	[ZipCode] [varchar](10) NULL,
	[Quantity] [int] NOT NULL,
	[TempAlloc] [bit] NULL,
	[TempReserved] [bit] NULL,
	[BBD] [datetime] NULL,
	[IsManual] [bit] NOT NULL,
	[TypeId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
	[Reliability] [int] NULL,
	[ReliabilityQuantity] [int] NULL,
	[ReliabilityQuality] [int] NULL,
	[AvailableFromDate] [datetime] NULL,
	[AvailableUntilDate] [datetime] NULL,
	[AvailableFromTime] [datetime] NULL,
	[AvailableUntilTime] [datetime] NULL,
	[ContactId] [int] NULL,
	[LoadingPointId] [int] NULL,
	[QualityId] [int] NULL,
	[AvailabilityTypeId] [int] NULL,
	[CommentText] [varchar](1000) NULL,
	[CountryId] [int] NULL,
	[AvailabilitySubTypeId] [int] NULL,
	[PalletTypeId] [int] NULL,
	[ContactAddressInfo] [varchar](255) NULL,
	[ContractNo] [varchar](32) NULL,
	[DeliveryNoteNo] [varchar](32) NULL,
	[TransactionNo] [varchar](32) NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
	[ContactPerson] [varchar](64) NULL,
	[ContactDetails] [varchar](64) NULL,
	[RepositoryHours] [varchar](128) NULL,
	[finished] [bit] NOT NULL,
	[IsMixedAvailability] [bit] NOT NULL,
	[ManAdrAnrede] [varchar](50) NULL,
	[ManAdrName1] [varchar](128) NULL,
	[ManAdrName2] [varchar](128) NULL,
	[ManAdrName3] [varchar](128) NULL,
	[ManAdrPLZ] [varchar](15) NULL,
	[ManAdrOrt] [varchar](128) NULL,
	[ManAdrStrasse] [varchar](128) NULL,
	[ManAdrLand] [varchar](50) NULL,
 CONSTRAINT [PK_LMS_AVAILABILITY_LTV] PRIMARY KEY CLUSTERED 
(
	[AvailabilityId] ASC,
	[IsManual] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[LMS_VIEW_AVAILABILITY_COMBINED]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LMS_VIEW_AVAILABILITY_COMBINED]
AS
SELECT        AvailabilityId, ZipCode, Quantity, TempAlloc, TempReserved, BBD, IsManual, TypeId, ClientId, Reliability, ReliabilityQuantity, ReliabilityQuality, AvailableFromDate, 
                         AvailableUntilDate, AvailableFromTime, AvailableUntilTime, ContactId, LoadingPointId, QualityId, AvailabilityTypeId, CreationDate, ModificationDate, DeletionDate, 
                         CreatedBy, ModifiedBy, DeletedBy, CommentText, CountryId, AvailabilitySubTypeId, PalletTypeId, ContactAddressInfo, ContractNo, DeliveryNoteNo, TransactionNo, 
                         RepositoryHours, ContactDetails, ContactPerson, finished, IsMixedAvailability, LSManAdrAnrede, LSManAdrName1, LSManAdrName2, LSManAdrName3, 
                         LSManAdrPLZ, LSManAdrOrt, LSManAdrStrasse, LSManAdrLand, PalettenProMonat, PalettenProWoche, Notiz, Geblockt, GeblocktVon, GeblocktDatum, 
                         GeblocktAufgehobenVon, GeblocktAufgehobenDatum, GeblocktKommentar, GeblocktFuer, InBearbeitungVon, InBearbeitungDatumZeit, Revision, Rejection, 
                         Reklamation, RejectionReasonId, RejectionQuantity, RejectionApproachID, RejectionComment, RejectionBy, RejectionDate, RejectionDone, RejectionDoneComment, 
                         RejectionNextReceiptNo, RejectionDoneBy, RejectionDoneDate, RejectionCost, Storniert, StorniertVon, StorniertDatum, StorniertKommentar, ICDroht, ICDrohtVon, 
                         ICDrohtAm, ICDurchfuehren, ICDurchfuehrenVon, ICDurchfuehrenAm, ICGrundID, ZuAvisieren, IstAvisiert, AvisiertVon, AvisiertAm, IstAusAblehnung, OrderManagementID
FROM            dbo.LMS_AVAILABILITY
UNION ALL
SELECT        AvailabilityId AS AvailabitlyID, ZipCode, Quantity, TempAlloc, TempReserved, BBD, IsManual, TypeId, ClientId, Reliability, ReliabilityQuantity, ReliabilityQuality, 
                         AvailableFromDate, AvailableUntilDate, AvailableFromTime, AvailableUntilTime, ContactId, ContactId AS Expr2, QualityId, 10 AS Expr1, CreationDate, 
                         ModificationDate, DeletionDate, CreatedBy, ModifiedBy, DeletedBy, CommentText, CountryId, AvailabilitySubTypeId, PalletTypeId, ContactAddressInfo, ContractNo, 
                         DeliveryNoteNo, TransactionNo, RepositoryHours, ContactDetails, ContactPerson, finished, IsMixedAvailability, '' AS LSManAdrAnrede, '' AS LSManAdrName1, 
                         '' AS LSManAdrName2, '' AS LSManAdrName3, '' AS LSManAdrPLZ, '' AS LSManAdrOrt, '' AS LSManAdrStrasse, '' AS LSManAdrLand, 0 AS PalettenProMonat, 
                         0 AS PalettenProWoche, '' AS Notiz, 0 AS Geblockt, '' AS GeblocktVon, NULL AS GeblocktDatum, '' AS GeblocktAufgehobenVon, NULL AS GeblocktAufgehobenDatum, 
                         '' AS GeblocktKommentar, '' AS GeblocktFuer, NULL AS InBearbeitungVon, NULL AS InBearbeitungDatumZeit, 0 AS Revision, 0 AS Rejection, 0 AS Reklamation, NULL 
                         AS RejectionReasonId, NULL AS RejectionQuantity, NULL AS RejectionApproachID, NULL AS RejectionComment, NULL AS RejectionBy, NULL 
                         AS RejectionDate, NULL AS RejectionDone, NULL AS RejectionDoneComment, NULL AS RejectionNextReceiptNo, NULL AS RejectionDoneBy, NULL 
                         AS RejectionDoneDate, 0 AS RejectionCost, 0 AS Storniert, NULL AS StorniertVon, NULL AS StorniertDatum, NULL AS StorniertKommentar, 0 AS ICDroht, NULL 
                         AS ICDrohtVon, NULL AS ICDrohtAm, 0 AS ICDurchfuehren, NULL AS ICDurchfuehrenVon, NULL AS ICDurchfuehrenAm, 0 AS ICGrundID, 0 AS ZuAvisieren, 
                         0 AS IstAvisiert, NULL AS AvisiertVon, NULL AS AvisiertAm, 0 AS IstAusAblehnung, NULL AS OrderManagementID
FROM            dbo.LMS_AVAILABILITY_LTV
GO
/****** Object:  Table [dbo].[LMS_AVAIL2DELI]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_AVAIL2DELI](
	[Avail2DeliID] [int] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_LMS_AVAIL2DELI] PRIMARY KEY CLUSTERED 
(
	[Avail2DeliID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_AVAILABILITYTYPE]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_AVAILABILITYTYPE](
	[TypeId] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_AVAILABILITYTYPE] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_COUNTRY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_COUNTRY](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](64) NOT NULL,
	[ShortName] [varchar](16) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_COUNTRY] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_LMS_COUNTRY] UNIQUE NONCLUSTERED 
(
	[ShortName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_CUSTOMER]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_CUSTOMER](
	[AdressNr] [int] NOT NULL,
	[ClientID] [int] NOT NULL,
	[Anrede] [nvarchar](255) NULL,
	[Kontoname] [nvarchar](255) NULL,
	[Kontoname2] [nvarchar](255) NULL,
	[Strasse] [nvarchar](255) NULL,
	[LKZ] [nvarchar](255) NULL,
	[PLZ] [nvarchar](255) NULL,
	[Postfach] [nvarchar](255) NULL,
	[Ort] [nvarchar](255) NULL,
	[Land] [nvarchar](255) NULL,
	[Telefon] [nvarchar](255) NULL,
	[Fax] [nvarchar](255) NULL,
	[EMail] [nvarchar](255) NULL,
	[www] [nvarchar](255) NULL,
	[AZeiten] [nvarchar](255) NULL,
	[Notiz] [nvarchar](max) NULL,
	[WinlineAdresse] [float] NULL,
	[GWAdresse] [float] NULL,
	[ZusatzAdresse] [float] NULL,
	[WinlineKonto] [nvarchar](255) NULL,
	[GWGGUID] [nvarchar](255) NULL,
	[inaktiv] [float] NULL,
	[Angelegt] [datetime] NULL,
	[Geaendert] [datetime] NULL,
 CONSTRAINT [PK_LMS_CUSTOMER_1] PRIMARY KEY CLUSTERED 
(
	[AdressNr] ASC,
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_DELIVERY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_DELIVERY](
	[DiliveryId] [int] IDENTITY(17,1) NOT NULL,
	[OldDate] [datetime] NULL,
	[Quantity] [int] NOT NULL,
	[Quality] [int] NOT NULL,
	[year] [int] NULL,
	[month] [int] NULL,
	[cw] [int] NULL,
	[ZipCode] [int] NULL,
	[TempAllocAvail] [int] NOT NULL,
	[day] [int] NULL,
	[finished] [bit] NOT NULL,
	[ClientId] [int] NOT NULL,
	[CountryId] [int] NULL,
	[CustomerId] [int] NULL,
	[DistributorId] [int] NULL,
	[LoadingPointId] [int] NULL,
	[CarrierId] [int] NULL,
	[FromDate] [datetime] NULL,
	[UntilDate] [datetime] NULL,
	[FromTime] [datetime] NULL,
	[UntilTime] [datetime] NULL,
	[Comment] [varchar](255) NULL,
	[IsFix] [bit] NULL,
	[PalletType] [int] NULL,
	[ContractNo] [varchar](32) NULL,
	[DeliveryNoteNo] [varchar](32) NULL,
	[TransactionNo] [varchar](32) NULL,
	[CategoryId] [int] NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
	[Rejection] [bit] NOT NULL,
	[Reklamation] [bit] NULL,
	[RejectionReasonId] [int] NULL,
	[RejectionComment] [varchar](1024) NULL,
	[RejectionQuantity] [int] NULL,
	[RejectionApproachId] [int] NULL,
	[RejectionBy] [varchar](32) NULL,
	[RejectionDate] [datetime] NULL,
	[RejectionDone] [bit] NOT NULL,
	[RejectionDoneComment] [varchar](1024) NULL,
	[RejectionNextReceiptNo] [varchar](64) NULL,
	[RejectionDoneBy] [varchar](32) NULL,
	[RejectionDoneDate] [datetime] NULL,
	[RejectionCost] [float] NULL,
	[ContactPerson] [varchar](64) NULL,
	[ContactDetails] [varchar](64) NULL,
	[IsMixedDelivery] [bit] NOT NULL,
	[ManualDistributorAddress] [varchar](255) NULL,
	[IsSmallAmount] [bit] NOT NULL,
	[ZuAvisieren] [bit] NOT NULL,
	[IstAvisiert] [bit] NOT NULL,
	[AvisiertVon] [varchar](32) NULL,
	[AvisiertAm] [datetime] NULL,
	[LSManAdrAnrede] [varchar](50) NULL,
	[LSManAdrName1] [varchar](128) NULL,
	[LSManAdrName2] [varchar](128) NULL,
	[LSManAdrName3] [varchar](128) NULL,
	[LSManAdrPLZ] [varchar](15) NULL,
	[LSManAdrOrt] [varchar](128) NULL,
	[LSManAdrStrasse] [varchar](128) NULL,
	[LSManAdrLand] [varchar](50) NULL,
	[LAManAdrAnrede] [varchar](50) NULL,
	[LAManAdrName1] [varchar](128) NULL,
	[LAManAdrName2] [varchar](128) NULL,
	[LAManAdrName3] [varchar](128) NULL,
	[LAManAdrPLZ] [varchar](15) NULL,
	[LAManAdrOrt] [varchar](128) NULL,
	[LAManAdrStrasse] [varchar](128) NULL,
	[LAManAdrLand] [varchar](50) NULL,
	[DeliveryTime] [varchar](128) NULL,
	[FrachtauftragNo] [varchar](32) NULL,
	[Notiz] [varchar](1024) NULL,
	[Kilometer] [int] NULL,
	[Frachtpreis] [float] NULL,
	[Prioritaet] [varchar](16) NULL,
	[Bestellnummer] [varchar](32) NULL,
	[Geblockt] [bit] NULL,
	[GeblocktVon] [varchar](32) NULL,
	[GeblocktDatum] [datetime] NULL,
	[GeblocktAufgehobenVon] [varchar](32) NULL,
	[GeblocktAufgehobenDatum] [datetime] NULL,
	[GeblocktKommentar] [varchar](1024) NULL,
	[GeblocktFuer] [varchar](32) NULL,
	[Storniert] [bit] NULL,
	[StorniertVon] [varchar](32) NULL,
	[StorniertDatum] [datetime] NULL,
	[StorniertKommentar] [varchar](1024) NULL,
	[InBearbeitungVon] [varchar](32) NULL,
	[InBearbeitungDatumZeit] [datetime] NULL,
	[Revision] [int] NULL,
	[Fixmenge] [bit] NULL,
	[Jumboladung] [bit] NULL,
	[IstBedarf] [bit] NULL,
	[BusinessTypeId] [int] NULL,
	[IstPlanmenge] [bit] NULL,
	[Rahmenauftragsnummer] [varchar](32) NULL,
	[FvKoffer] [bit] NULL,
	[FvPlane] [bit] NULL,
	[FvStapelhoehe] [int] NULL,
	[FvVerladerichtungLaengs] [bit] NULL,
	[DispoNotiz] [varchar](1024) NULL,
	[OrderManagementID] [varchar](20) NULL,
	[Einzelpreis] [decimal](18, 6) NULL,
	[ExterneNummer] [varchar](25) NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_LMS_DELIVERY] PRIMARY KEY CLUSTERED 
(
	[DiliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_DELIVERYCATEGORY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_DELIVERYCATEGORY](
	[CategoryId] [int] NOT NULL,
	[ShortName] [varchar](3) NOT NULL,
	[Name] [varchar](32) NOT NULL,
	[R] [int] NULL,
	[G] [int] NULL,
	[B] [int] NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_DELIVERYCATEGORY] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_ICGRUND]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_ICGRUND](
	[IcGrundId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](96) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_ICGRUND] PRIMARY KEY CLUSTERED 
(
	[IcGrundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_PALLETTYPE]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_PALLETTYPE](
	[PalletTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](64) NOT NULL,
	[ShortName] [varchar](16) NULL,
	[ArtNrShortName] [varchar](2) NULL,
	[IsMixType] [bit] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_PALLETTYPE] PRIMARY KEY CLUSTERED 
(
	[PalletTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_QUALITY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_QUALITY](
	[QualityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ArtNrShortName] [varchar](5) NULL,
	[QualityValue] [int] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
	[IstStandard] [bit] NULL,
 CONSTRAINT [PK_LMS_QUALITY] PRIMARY KEY CLUSTERED 
(
	[QualityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[LMSAuswertung]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[LMSAuswertung]
AS
SELECT        d.DiliveryId AS DeliveryID, av.AvailabilityId, pt.Name AS Palette, d.PalletType AS PaletteID, q.Name AS Qualitaet, d.Quality AS QualitaetID, d.Quantity AS Menge,
                             (SELECT        ISNULL(SUM(Quantity), 0) AS Expr1
                               FROM            dbo.LMS_AVAIL2DELI
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)) AS MengeZugeordnet,
                             (SELECT        ISNULL(SUM(Quantity), 0) AS Expr1
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_10
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 1) AND (DeletedBy IS NULL)) AS MengeReserviert,
                             (SELECT        TOP (1) DateOfRelation
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_9
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS Zustelltermin,
                             (SELECT        TOP (1) ZustellterminZeitVon
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_13
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS ZustellterminZeitVon,
                             (SELECT        TOP (1) ZustellterminZeitBis
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_12
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS ZustellterminZeitBis,
                             (SELECT        TOP (1) LadeterminDatum
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_8
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS Ladetermin,
                             (SELECT        TOP (1) LadeterminZeitVon
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_2
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS LadeterminZeitVon,
                             (SELECT        TOP (1) LadeterminZeitBis
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_1
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS LadeterminZeitBis, d.Prioritaet, CAST((CASE WHEN
                             (SELECT        COUNT(*)
                               FROM            dbo.LMS_AVAIL2DELI
                               WHERE        (DeliveryID = d .DiliveryID) AND (State = 2) AND (ICTransport = 1) AND (DeletedBy IS NULL)) > 0 THEN 1 ELSE 0 END) AS Bit) AS ICTransport, 
                              CAST((CASE WHEN
                             (SELECT        COUNT(*)
                               FROM            dbo.LMS_AVAIL2DELI
                               WHERE        (DeliveryID = d .DiliveryID) AND (State = 2) AND (Timocom = 1) AND (DeletedBy IS NULL)) > 0 THEN 1 ELSE 0 END) AS Bit) AS Timocom, 
                         ISNULL
                             ((SELECT        COUNT(*) AS Expr1
                                 FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_11
                                 WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)), 0) AS Ladungen, ISNULL
                             ((SELECT        SUM(Kilometer) AS Expr1
                                 FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_7
                                 WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)), d.Kilometer) AS Kilometer, ISNULL
                             ((SELECT        SUM(Frachtpreis) AS Expr1
                                 FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_6
                                 WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)), d.Frachtpreis) AS Frachtpreis,
                             (SELECT        TOP (1) ISNULL(ModificationDate, CreationDate) AS Expr1
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_14
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS ZuordnungLetzteAktualisierung,
                             (SELECT        TOP (1) CreationDate AS Expr1
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_14
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS ZuordnungAm,
                             (SELECT        TOP (1) CreatedBy AS Expr1
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_14
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) AS ZuordnungVon, DeliCat.ShortName AS LieferKategorie, DeliCat.CategoryId AS LieferKategorieID, d.finished, d.IsMixedDelivery, 
                         d.IsSmallAmount, d.Storniert, d.Fixmenge, d.Jumboladung, d.IstBedarf, d.CustomerId AS KundeNummer, cust_kunde.Kontoname AS KundeName, 
                         cust_kunde.PLZ AS KundePLZ, cust_kunde.Ort AS KundeOrt, cust_kunde.Strasse AS KundeStrasse, d.DistributorId AS LieferadresseNummer, 
                         (CASE WHEN d .DistributorId > 0 THEN cust_lieferadresse.kontoname ELSE d .LAManAdrName1 END) AS LieferadresseName, 
                         (CASE WHEN d .DistributorId > 0 THEN cust_lieferadresse.PLZ ELSE d .LAManAdrPLZ END) AS LieferadressePLZ, 
                         (CASE WHEN d .DistributorId > 0 THEN cust_lieferadresse.Ort ELSE d .LAManAdrOrt END) AS LieferadresseOrt, 
                         (CASE WHEN d .DistributorId > 0 THEN cust_lieferadresse.Strasse ELSE d .LAManAdrStrasse END) AS LieferadresseStrasse, 
                         (CASE WHEN av.LoadingPointId >= 0 THEN av.LoadingPointId ELSE av.contactid END) AS LadestelleNummer, 
                         (CASE WHEN av.LoadingPointId = - 1 THEN av_kunde.kontoname WHEN av.LoadingPointId > 0 THEN av_ladestelle.kontoname ELSE av.LSManAdrName1 END) 
                         AS LadestelleName, (CASE WHEN av.LoadingPointId = - 1 THEN av_kunde.PLZ WHEN av.LoadingPointId > 0 THEN av_ladestelle.PLZ ELSE av.LSManAdrPLZ END) 
                         AS LadestellePLZ, (CASE WHEN av.LoadingPointId = - 1 THEN av_kunde.Ort WHEN av.LoadingPointId > 0 THEN av_ladestelle.Ort ELSE av.LSManAdrOrt END) 
                         AS LadestelleOrt, 
                         (CASE WHEN av.LoadingPointId = - 1 THEN av_kunde.Strasse WHEN av.LoadingPointId > 0 THEN av_ladestelle.Strasse ELSE av.LSManAdrStrasse END) 
                         AS LadestelleStrasse,
                             (SELECT        MIN(SpediteurID) AS Expr1
                               FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_5
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)) AS SpediteurID, ISNULL
                             ((SELECT        MIN(SpediteurManuell) AS Expr1
                                 FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_4
                                 WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)), '') AS SpediteurManuell, ISNULL
                             ((SELECT        Kontoname
                                 FROM            dbo.LMS_CUSTOMER
                                 WHERE        (AdressNr = ISNULL
                                                              ((SELECT        MIN(SpediteurID) AS Expr1
                                                                  FROM            dbo.LMS_AVAIL2DELI AS LMS_AVAIL2DELI_3
                                                                  WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)), 0)) AND (ClientID = d.ClientId)), '') AS Spediteur, 
                         av.ContractNo AS AuftragsNrVonVerfuegbarkeit,
                             (SELECT        Name
                               FROM            dbo.LMS_AVAILABILITYTYPE
                               WHERE        (av.AvailabilityTypeId = TypeId)) AS ArtVonVerfuegbarkeit, av.AvailabilityTypeId AS ArtIDVonVerfuegbarkeit, 
                         av.AvailableFromDate AS VerfuegbarkeitDatumVon, av.AvailableUntilDate AS VerfuegbarkeitDatumBis, ISNULL(av.ICDroht, 0) AS ICDrohtVerfuegbarkeit, 
                         av.ICDrohtVon AS ICDrohtVonVerfuegbarkeit, av.ICDrohtAm AS ICDrohtAmVerfuegbarkeit, ISNULL(av.ICDurchfuehren, 0) AS ICDurchfuehrenVerfuegbarkeit, 
                         av.ICDurchfuehrenVon AS ICDurchfuehrenVonVerfuegbarkeit, av.ICDurchfuehrenAm AS ICDurchfuehrenAmVerfuegbarkeit, av.ICDurchfuehrenVon, av.IcGrundID, 
                         icg.Name AS ICGrund, d.ClientId, d.CountryId, d.FromDate AS LieferterminDatumVon, d.UntilDate AS LieferterminDatumBis, d.IsFix AS IstFixtermin, d.Comment, 
                         d.ContractNo AS AuftragsNummer, d.DeliveryNoteNo AS LieferscheinNummer, d.FrachtauftragNo AS FrachtauftragsNummer, d.Bestellnummer, 
                         d.TransactionNo AS TransaktionsNummer, d.Rejection, d.Reklamation, d.RejectionReasonId, d.RejectionQuantity, d.RejectionApproachId, d.RejectionDoneDate, 
                         d.RejectionCost, d.Notiz, d.CreatedBy AS LieferterminErstelltVon, d.CreationDate AS LieferterminErstelltAm, d.ModifiedBy AS LieferterminGeaendertVon, 
                         d.ModificationDate AS LieferterminGeaendertAm, av.CreatedBy AS VerfuegbarkeitErstelltVon, av.CreationDate AS VerfuegbarkeitErstelltAm, 
                         av.ModifiedBy AS VerfuegbarkeitGeaendertVon, av.ModificationDate AS VerfuegbarkeitGeaendertAm,
                         d.Einzelpreis
FROM            dbo.LMS_DELIVERY AS d LEFT OUTER JOIN
                         dbo.LMS_AVAILABILITY AS av ON av.AvailabilityId =
                             (SELECT        TOP (1) AvailabilityID
                               FROM            dbo.LMS_AVAIL2DELI
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) LEFT OUTER JOIN
                         dbo.LMS_PALLETTYPE AS pt ON d.PalletType = pt.PalletTypeId LEFT OUTER JOIN
                         dbo.LMS_QUALITY AS q ON d.Quality = q.QualityId LEFT OUTER JOIN
                         dbo.LMS_COUNTRY AS c ON d.CountryId = c.CountryId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS cust_kunde ON cust_kunde.AdressNr = d.CustomerId AND cust_kunde.ClientID = d.ClientId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS cust_lieferadresse ON cust_lieferadresse.AdressNr = d.DistributorId AND cust_lieferadresse.ClientID = d.ClientId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS av_kunde ON av_kunde.AdressNr = av.ContactId AND av_kunde.ClientID = av.ClientId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS av_ladestelle ON av_ladestelle.AdressNr = av.LoadingPointId AND av_ladestelle.ClientID = av.ClientId LEFT OUTER JOIN
                         dbo.LMS_DELIVERYCATEGORY AS DeliCat ON DeliCat.CategoryId =
                             (SELECT        TOP (1) Lieferkategorie
                               FROM            dbo.LMS_AVAIL2DELI
                               WHERE        (DeliveryID = d.DiliveryId) AND (State = 2) AND (DeletedBy IS NULL)
                               ORDER BY DateOfRelation) LEFT OUTER JOIN
                         dbo.LMS_ICGRUND AS icg ON av.IcGrundID = icg.IcGrundId
WHERE        (d.DeletedBy IS NULL) AND (c.DeletedBy IS NULL)



GO
/****** Object:  Table [dbo].[LMS_APPROACH]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_APPROACH](
	[ApproachId] [int] IDENTITY(1,1) NOT NULL,
	[Belegtyp] [int] NOT NULL,
	[ShortName] [varchar](3) NOT NULL,
	[Name] [varchar](64) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_APPROACH] PRIMARY KEY CLUSTERED 
(
	[ApproachId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_BUSINESSTYPE]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_BUSINESSTYPE](
	[BusinessTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](64) NOT NULL,
	[ShortName] [varchar](16) NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_BUSINESSTYPE] PRIMARY KEY CLUSTERED 
(
	[BusinessTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_REJECTIONREASON]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_REJECTIONREASON](
	[RejectionReasonId] [int] IDENTITY(1,1) NOT NULL,
	[BelegTyp] [int] NULL,
	[ShortName] [varchar](3) NOT NULL,
	[Name] [varchar](96) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_REJECTIONREASON] PRIMARY KEY CLUSTERED 
(
	[RejectionReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[LMSAuswertungLiefertermin]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[LMSAuswertungLiefertermin]
AS
SELECT        d.DiliveryId AS DeliveryID, d.FromDate AS DatumVon, d.UntilDate AS DatumBis, d.Quantity AS Menge, d.PalletType AS PaletteID, p.Name AS PaletteName, 
                         d.Quality AS QualitaetID, q.Name AS QualitaetName, d.IsMixedDelivery AS Mischladung, bt.Name AS Geschaeftsart, d.ClientId AS MandantID, c.ShortName AS Land, 
                         d.CustomerId AS KundeNummer, cust_kunde.Kontoname AS KundeName, cust_kunde.PLZ AS KundePLZ, cust_kunde.Ort AS KundeOrt, 
                         cust_kunde.Strasse AS KundeStrasse, d.DistributorId AS LieferadresseNummer, 
                         (CASE WHEN d .DistributorId > 0 THEN cust_lieferadresse.kontoname ELSE d .LAManAdrName1 END) AS LieferadresseName, 
                         (CASE WHEN d .DistributorId > 0 THEN cust_lieferadresse.PLZ ELSE d .LAManAdrPLZ END) AS LieferadressePLZ, 
                         (CASE WHEN d .DistributorId > 0 THEN cust_lieferadresse.Ort ELSE d .LAManAdrOrt END) AS LieferadresseOrt, 
                         (CASE WHEN d .DistributorId > 0 THEN cust_lieferadresse.Strasse ELSE d .LAManAdrStrasse END) AS LieferadresseStrasse, 
                         d.LoadingPointId AS LadestelleNummer, (CASE WHEN d .LoadingPointId > 0 THEN cust_ladestelle.kontoname ELSE d .LSManAdrName1 END) AS LadestelleName, 
                         (CASE WHEN d .LoadingPointId > 0 THEN cust_ladestelle.PLZ ELSE d .LSManAdrPLZ END) AS LadestellePLZ, 
                         (CASE WHEN d .LoadingPointId > 0 THEN cust_ladestelle.Ort ELSE d .LSManAdrOrt END) AS LadestelleOrt, 
                         (CASE WHEN d .LoadingPointId > 0 THEN cust_ladestelle.Strasse ELSE d .LSManAdrStrasse END) AS LadestelleStrasse, d.ContractNo AS Auftragsnummer, 
                         d.DeliveryNoteNo AS Lieferscheinnummer, d.TransactionNo AS Vorgangsnummer, d.FrachtauftragNo AS Frachtauftragsnummer, d.Bestellnummer, 
                         d.finished AS Erledigt, d.Rejection AS Ablehnung, d.Reklamation, d.RejectionQuantity AS AblehnungMenge, rr.Name AS AblehnungGrund, 
                         d.RejectionComment AS AblehnungKommentar, ap.Name AS AblehnungVerfahrensweise, d.RejectionBy AS AblehnungVon, d.RejectionDate AS AblehnungAm, 
                         d.RejectionDoneComment, d.RejectionNextReceiptNo AS AblehnungFolgebeleg, d.RejectionDoneBy AS AblehnungBearbeitetVon, 
                         d.RejectionDoneDate AS AblehnungBearbeitetAm, d.RejectionCost AS AblehnungKosten, d.IsSmallAmount AS Kleinstmengenfreistellung, d.Fixmenge, d.Jumboladung,
                          d.IstBedarf, d.IstPlanmenge, d.Notiz, d.Prioritaet, d.ZuAvisieren, d.IstAvisiert, d.AvisiertVon, d.AvisiertAm, d.Geblockt, d.GeblocktVon, d.GeblocktDatum, 
                         d.GeblocktAufgehobenVon, d.GeblocktAufgehobenDatum, d.GeblocktKommentar, d.GeblocktFuer, d.Storniert, d.StorniertVon, d.StorniertDatum, d.StorniertKommentar, 
                         d.CreationDate AS ErzeugtAm, d.ModificationDate AS GeaendertAm, d.DeletionDate AS GeloeschtAm, d.CreatedBy AS ErzeugtVon, d.ModifiedBy AS GeaendertVon, 
                         d.DeletedBy AS GeloeschtVon,
                         d.Einzelpreis
FROM            dbo.LMS_DELIVERY AS d LEFT OUTER JOIN
                         dbo.LMS_PALLETTYPE AS p ON d.PalletType = p.PalletTypeId LEFT OUTER JOIN
                         dbo.LMS_QUALITY AS q ON d.Quality = q.QualityId LEFT OUTER JOIN
                         dbo.LMS_COUNTRY AS c ON d.CountryId = c.CountryId LEFT OUTER JOIN
                         dbo.LMS_REJECTIONREASON AS rr ON d.RejectionReasonId = rr.RejectionReasonId LEFT OUTER JOIN
                         dbo.LMS_APPROACH AS ap ON d.RejectionApproachId = ap.ApproachId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS cust ON cust.AdressNr = d.DistributorId AND cust.ClientID = d.ClientId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS cust_kunde ON cust_kunde.AdressNr = d.CustomerId AND cust_kunde.ClientID = d.ClientId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS cust_lieferadresse ON cust_lieferadresse.AdressNr = d.DistributorId AND cust_lieferadresse.ClientID = d.ClientId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS cust_ladestelle ON cust_ladestelle.AdressNr = d.LoadingPointId AND cust_ladestelle.ClientID = d.ClientId LEFT OUTER JOIN
                         dbo.LMS_BUSINESSTYPE AS bt ON d.BusinessTypeId = bt.BusinessTypeId
WHERE        (d.DeletedBy IS NULL)

GO
/****** Object:  View [dbo].[LMSAuswertungVerfuegbarkeit]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[LMSAuswertungVerfuegbarkeit]
AS
SELECT DISTINCT 
                         av.AvailabilityId AS AvailabilityID, av.AvailableFromDate AS DatumVon, av.AvailableUntilDate AS DatumBis, av.Quantity AS Menge, 
                         av.Quantity - (SELECT ISNULL(SUM(Quantity), 0) FROM dbo.LMS_AVAIL2DELI WHERE (AvailabilityID = av.AvailabilityID) AND (State = 2) AND (DeletedBy IS NULL)) AS Frei,
                         av.PalletTypeId AS PaletteID, 
                         p.Name AS PaletteName, av.QualityId AS QualitaetID, q.Name AS QualitaetName, av.IsMixedAvailability AS Mischladung, at.Name AS Art, av.ClientId AS MandantID, 
                         c.ShortName AS Land, av.ContactId AS KontaktNummer, cust_kontakt.Kontoname AS KontaktName, cust_kontakt.PLZ AS KontaktPLZ, cust_kontakt.Ort AS KontaktOrt,
                             (SELECT        CASE av.LoadingPointId WHEN - 1 THEN av.contactid ELSE av.LoadingPointId END AS Expr1) AS LadeadresseNummer, 
                         (CASE WHEN av.LoadingPointId > 0 THEN cust_ladeadresse.kontoname ELSE av.LSManAdrName1 END) AS LadeadresseName, 
                         (CASE WHEN av.LoadingPointId > 0 THEN cust_ladeadresse.PLZ ELSE av.LSManAdrPLZ END) AS LadeadressePLZ, 
                         (CASE WHEN av.LoadingPointId > 0 THEN cust_ladeadresse.Ort ELSE av.LSManAdrOrt END) AS LadeadresseOrt, av.ContractNo AS Abholscheinnummer, 
                         av.DeliveryNoteNo AS Lieferscheinnummer, av.TransactionNo AS Vorgangsnummer, av.finished AS Erledigt, av.Rejection AS Ablehnung, av.Reklamation, 
                         av.RejectionQuantity AS AblehnungMenge, rr.Name AS AblehnungGrund, av.RejectionComment AS AblehnungKommentar, ap.Name AS AblehnungVerfahrensweise, 
                         av.RejectionBy AS AblehnungVon, av.RejectionDate AS AblehnungAm, av.RejectionDoneComment, av.RejectionNextReceiptNo AS AblehnungFolgebeleg, 
                         av.RejectionDoneBy AS AblehnungBearbeitetVon, av.RejectionDoneDate AS AblehnungBearbeitetAm, av.RejectionCost AS AblehnungKosten, av.Notiz, av.Geblockt, 
                         av.GeblocktVon, av.GeblocktDatum, av.GeblocktAufgehobenVon, av.GeblocktAufgehobenDatum, av.GeblocktKommentar, av.GeblocktFuer, av.Storniert, 
                         av.StorniertVon, av.StorniertDatum, av.StorniertKommentar, av.ICDroht, av.ICDrohtVon, av.ICDrohtAm, av.ICDurchfuehren, av.ICDurchfuehrenVon, 
                         av.ICDurchfuehrenAm, av.ZuAvisieren, av.IstAvisiert, av.AvisiertVon, av.AvisiertAm, av.CreationDate AS ErzeugtAm, av.ModificationDate AS GeaendertAm, 
                         av.DeletionDate AS GeloeschtAm, av.CreatedBy AS ErzeugtVon, av.ModifiedBy AS GeaendertVon, av.DeletedBy AS GeloeschtVon
FROM            dbo.LMS_VIEW_AVAILABILITY_COMBINED AS av LEFT OUTER JOIN
                         dbo.LMS_PALLETTYPE AS p ON av.PalletTypeId = p.PalletTypeId LEFT OUTER JOIN
                         dbo.LMS_QUALITY AS q ON av.QualityId = q.QualityId LEFT OUTER JOIN
                         dbo.LMS_AVAILABILITYTYPE AS at ON av.AvailabilityTypeId = at.TypeId LEFT OUTER JOIN
                         dbo.LMS_COUNTRY AS c ON av.CountryId = c.CountryId LEFT OUTER JOIN
                         dbo.LMS_REJECTIONREASON AS rr ON av.RejectionReasonId = rr.RejectionReasonId LEFT OUTER JOIN
                         dbo.LMS_APPROACH AS ap ON av.RejectionApproachID = ap.ApproachId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS cust_kontakt ON cust_kontakt.AdressNr = av.ContactId AND cust_kontakt.ClientID = av.ClientId LEFT OUTER JOIN
                         dbo.LMS_CUSTOMER AS cust_ladeadresse ON cust_ladeadresse.AdressNr = av.LoadingPointId AND cust_ladeadresse.ClientID = av.ClientId
WHERE        (av.DeletedBy IS NULL) AND (av.AvailabilityTypeId <> 10)

GO
/****** Object:  Table [dbo].[GEO_coordinates]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEO_coordinates](
	[loc_id] [int] NOT NULL,
	[coord_type] [int] NOT NULL,
	[lon] [float] NULL,
	[lat] [float] NULL,
	[coord_subtype] [int] NULL,
	[valid_since] [datetime] NULL,
	[date_type_since] [int] NULL,
	[valid_until] [datetime] NOT NULL,
	[date_type_until] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEO_textdata]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEO_textdata](
	[loc_id] [int] NOT NULL,
	[text_type] [int] NOT NULL,
	[text_val] [varchar](255) NOT NULL,
	[text_locale] [varchar](5) NULL,
	[is_native_lang] [smallint] NULL,
	[is_default_name] [smallint] NULL,
	[valid_since] [datetime] NULL,
	[date_type_since] [int] NULL,
	[valid_until] [datetime] NOT NULL,
	[date_type_until] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[GetPLZInDistance]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetPLZInDistance] 
(	
	-- Add the parameters for the function here
	@PLZ varchar(10),
	@Radius float
)
RETURNS TABLE 
AS
RETURN 
(
SELECT     DISTINCT td.text_val AS PLZ
FROM dbo.GEO_coordinates AS c INNER JOIN
   dbo.GEO_textdata AS td ON c.loc_id = td.loc_id CROSS JOIN
   (SELECT c.lon, c.lat
    FROM dbo.GEO_coordinates AS c INNER JOIN
       dbo.GEO_textdata AS td ON c.loc_id = td.loc_id
    WHERE (td.text_val LIKE @PLZ + '%') AND (td.text_type = '500300000')) AS plz
WHERE (td.text_type = '500300000') AND (dbo.GetDistanceFromCoords(c.lat, c.lon, plz.lat, plz.lon) <= @Radius))

GO
/****** Object:  View [dbo].[LMS_VIEW_AVAILABILITY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LMS_VIEW_AVAILABILITY]
AS
SELECT        AvailabilityId, ZipCode, Quantity, TempAlloc, TempReserved, BBD, IsManual, TypeId, ClientId, Reliability, ReliabilityQuantity, ReliabilityQuality, AvailableFromDate, 
                         AvailableUntilDate, AvailableFromTime, AvailableUntilTime, ContactId, LoadingPointId, QualityId, AvailabilityTypeId, CreationDate, ModificationDate, DeletionDate, 
                         CreatedBy, ModifiedBy, DeletedBy, CommentText, CountryId, AvailabilitySubTypeId, PalletTypeId, ContactAddressInfo, ContractNo, DeliveryNoteNo, TransactionNo, 
                         RepositoryHours, ContactDetails, ContactPerson, finished, IsMixedAvailability, LSManAdrAnrede, LSManAdrName1, LSManAdrName2, LSManAdrName3, 
                         LSManAdrPLZ, LSManAdrOrt, LSManAdrStrasse, LSManAdrLand, PalettenProMonat, PalettenProWoche, Notiz, Geblockt, GeblocktVon, GeblocktDatum, 
                         GeblocktAufgehobenVon, GeblocktAufgehobenDatum, GeblocktKommentar, GeblocktFuer, InBearbeitungVon, InBearbeitungDatumZeit, Revision, Rejection, 
                         RejectionReasonID, RejectionQuantity, RejectionApproachID, RejectionComment, RejectionBy, RejectionDate, RejectionDone, RejectionDoneComment, 
                         RejectionNextReceiptNo, RejectionDoneBy, RejectionDoneDate, Storniert, StorniertVon, StorniertDatum, StorniertKommentar, IstAusAblehnung, RejectionCost, 
                         ICDroht, ICDrohtVon, ICDrohtAm, ICDurchfuehren, ICDurchfuehrenVon, ICDurchfuehrenAm, ZuAvisieren, IstAvisiert, AvisiertVon, AvisiertAm, Reklamation, 
                         OrderManagementID
FROM            dbo.LMS_AVAILABILITY
GO
/****** Object:  Table [dbo].[EXT_BELEGDATEN]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXT_BELEGDATEN](
	[Belegnummer] [char](15) NULL,
	[Vorgangsnummer] [varchar](10) NULL,
	[StandardadresseNummer] [char](10) NULL,
	[Standardadresse] [varchar](128) NULL,
	[Strasse_Standardadresse] [varchar](128) NULL,
	[PLZ_Standardadresse] [char](15) NULL,
	[Ort_Standardadresse] [varchar](128) NULL,
	[Land_Standardadresse] [varchar](50) NULL,
	[LieferkundenNummer] [char](10) NULL,
	[Lieferadresse] [varchar](128) NULL,
	[LieferStrasse] [varchar](128) NULL,
	[LieferPLZ] [char](15) NULL,
	[LieferOrt] [varchar](128) NULL,
	[LieferLand] [varchar](50) NULL,
	[LieferOeffnungszeiten] [varchar](200) NULL,
	[Liefern im Auftrag Nummer] [char](10) NULL,
	[Liefern im Auftrag] [varchar](128) NULL,
	[Liefern im Auftrag Strasse] [varchar](128) NULL,
	[Liefern im Auftrag PLZ] [char](15) NULL,
	[Liefern im Auftrag Ort] [varchar](128) NULL,
	[Liefern im Auftrag Land] [varchar](50) NULL,
	[LadeAdresseNummer] [char](10) NULL,
	[LadeAdresse] [varchar](128) NULL,
	[LadeAdresseStrasse] [varchar](128) NULL,
	[LadeAdressePLZ] [char](15) NULL,
	[LadeAdresseOrt] [varchar](128) NULL,
	[LadeAdresseLand] [varchar](50) NULL,
	[LadeAdresseOeffnungszeiten] [varchar](200) NULL,
	[Laden im Auftrage Nummer] [char](10) NULL,
	[Laden im Auftrage] [varchar](128) NULL,
	[Laden im Auftrage Strasse] [varchar](128) NULL,
	[Laden im Auftrage PLZ] [char](15) NULL,
	[Laden im Auftrage Ort] [varchar](128) NULL,
	[Laden im Auftrage Land] [varchar](50) NULL,
	[TransporteurNummer] [char](10) NULL,
	[Transporteur] [varchar](128) NULL,
	[TransporteurStrasse] [varchar](128) NULL,
	[TransporteurPLZ] [char](15) NULL,
	[TransporteurOrt] [varchar](128) NULL,
	[TransporteurLand] [varchar](50) NULL,
	[Ansprechpartner] [varchar](128) NULL,
	[Termin] [datetime] NULL,
	[Artikelnummer] [varchar](50) NULL,
	[Menge] [decimal](18, 7) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEO_cache_data]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEO_cache_data](
	[CacheId] [int] NOT NULL,
	[ZipCode] [varchar](10) NOT NULL,
 CONSTRAINT [PK_GEO_cache_data] PRIMARY KEY CLUSTERED 
(
	[CacheId] ASC,
	[ZipCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEO_cache_head]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEO_cache_head](
	[CacheId] [int] NOT NULL,
	[ZipCode] [varchar](50) NOT NULL,
	[Radiant] [int] NOT NULL,
 CONSTRAINT [PK_GEO_cache_head] PRIMARY KEY CLUSTERED 
(
	[CacheId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEO_locations]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEO_locations](
	[loc_id] [int] NOT NULL,
	[loc_type] [int] NOT NULL,
 CONSTRAINT [PK__GEO_locations__1ED998B2] PRIMARY KEY CLUSTERED 
(
	[loc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_AVAILABILITY_HISTORY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_AVAILABILITY_HISTORY](
	[AvailabilityHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityId] [int] NULL,
	[Geblockt] [bit] NULL,
	[GeblocktVon] [varchar](32) NULL,
	[GeblocktDatum] [datetime] NULL,
	[GeblocktAufgehobenVon] [varchar](32) NULL,
	[GeblocktAufgehobenDatum] [datetime] NULL,
	[GeblocktFuer] [varchar](32) NULL,
	[AvailableFromDate] [datetime] NULL,
	[AvailableUntilDate] [datetime] NULL,
	[DataCreatedBy] [varchar](32) NULL,
	[DataCreationDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[CreationDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_AVAILABILITYSUBTYPE]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_AVAILABILITYSUBTYPE](
	[SubTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ClassificationId] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_AVAILABILITYSUBTYPE] PRIMARY KEY CLUSTERED 
(
	[SubTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_BENUTZER]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_BENUTZER](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Benutzername] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Vorname] [varchar](50) NOT NULL,
	[Gruppe_ID] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
	[EMail] [varchar](256) NULL,
 CONSTRAINT [PK_LMS_BENUTZER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_CLASSIFICATION]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_CLASSIFICATION](
	[ClassificationId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](32) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_CLASSIFICATION] PRIMARY KEY CLUSTERED 
(
	[ClassificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_CLIENT]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_CLIENT](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[Shortname] [varchar](16) NOT NULL,
	[Name] [varchar](64) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_CLIENT] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_LMS_CLIENT] UNIQUE NONCLUSTERED 
(
	[Shortname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_CONFIG]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_CONFIG](
	[LMS_Gesperrt] [bit] NOT NULL,
	[Prog_AktuelleVersion1] [int] NOT NULL,
	[Prog_AktuelleVersion2] [int] NOT NULL,
	[Prog_AktuelleVersion3] [int] NOT NULL,
	[Prog_AktuelleVersion4] [int] NOT NULL,
	[Prog_VersionErzwingen] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_CUSTOMER_TAB_BAK]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_CUSTOMER_TAB_BAK](
	[AdressNr] [int] NOT NULL,
	[Anrede] [nvarchar](255) NULL,
	[Kontoname] [nvarchar](255) NULL,
	[Kontoname2] [nvarchar](255) NULL,
	[Strasse] [nvarchar](255) NULL,
	[LKZ] [nvarchar](255) NULL,
	[PLZ] [nvarchar](255) NULL,
	[Postfach] [nvarchar](255) NULL,
	[Ort] [nvarchar](255) NULL,
	[Land] [nvarchar](255) NULL,
	[Telefon] [nvarchar](255) NULL,
	[Fax] [nvarchar](255) NULL,
	[EMail] [nvarchar](255) NULL,
	[www] [nvarchar](255) NULL,
	[AZeiten] [nvarchar](255) NULL,
	[Notiz] [nvarchar](max) NULL,
	[WinlineAdresse] [float] NULL,
	[GWAdresse] [float] NULL,
	[ZusatzAdresse] [float] NULL,
	[WinlineKonto] [nvarchar](255) NULL,
	[GWGGUID] [nvarchar](255) NULL,
	[inaktiv] [float] NULL,
	[Angelegt] [datetime] NULL,
	[Geaendert] [datetime] NULL,
 CONSTRAINT [PK_LMS_CUSTOMER] PRIMARY KEY CLUSTERED 
(
	[AdressNr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_DATEFILTERFIELD]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_DATEFILTERFIELD](
	[DateFilterFieldId] [int] IDENTITY(1,1) NOT NULL,
	[DatabaseName] [varchar](50) NOT NULL,
	[DisplayName] [varchar](50) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_DATEFILTERFIELD] PRIMARY KEY CLUSTERED 
(
	[DateFilterFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_DELIVERY_HISTORY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_DELIVERY_HISTORY](
	[DeliveryHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryId] [int] NULL,
	[Geblockt] [bit] NULL,
	[GeblocktVon] [varchar](32) NULL,
	[GeblocktDatum] [datetime] NULL,
	[GeblocktAufgehobenVon] [varchar](32) NULL,
	[GeblocktAufgehobenDatum] [datetime] NULL,
	[GeblocktFuer] [varchar](32) NULL,
	[FromDate] [datetime] NULL,
	[UntilDate] [datetime] NULL,
	[DataCreatedBy] [varchar](32) NULL,
	[DataCreationDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[CreationDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_DELIVERYDETAIL]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_DELIVERYDETAIL](
	[DeliveryDetailId] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Quantity] [int] NOT NULL,
	[year] [int] NOT NULL,
	[month] [int] NOT NULL,
	[cw] [int] NOT NULL,
	[Finished] [bit] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_DELIVERYDETAIL] PRIMARY KEY CLUSTERED 
(
	[DeliveryDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_FILTER]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_FILTER](
	[FilterID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](64) NOT NULL,
	[Filter] [varchar](512) NOT NULL,
	[FilterType] [tinyint] NOT NULL,
	[Sort] [varchar](5) NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_FILTER] PRIMARY KEY CLUSTERED 
(
	[FilterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_GRUPPE]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_GRUPPE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Gruppenname] [varchar](50) NOT NULL,
	[Gruppenbezeichnung] [varchar](50) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatetedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_Gruppe] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_NOTE]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_NOTE](
	[NoteId] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityId] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_NOTE] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_PRIORITAET]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_PRIORITAET](
	[PrioritaetId] [varchar](16) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_PRIORITAET] PRIMARY KEY CLUSTERED 
(
	[PrioritaetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_QUALI2PALLET]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_QUALI2PALLET](
	[QualityId] [int] NOT NULL,
	[PalletTypeId] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_QUALI2PALLET] PRIMARY KEY CLUSTERED 
(
	[QualityId] ASC,
	[PalletTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_RELIABILITY]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_RELIABILITY](
	[ReliabilityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](32) NOT NULL,
	[ShortName] [varchar](18) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[DeletionDate] [datetime] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[DeletedBy] [varchar](32) NULL,
 CONSTRAINT [PK_LMS_RELIABILITY] PRIMARY KEY CLUSTERED 
(
	[ReliabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_REPORT]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_REPORT](
	[ReportId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Statement] [varchar](max) NOT NULL,
	[Parameter] [varchar](128) NULL,
 CONSTRAINT [PK_LMS_REPORT] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LMS_SIGHT]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LMS_SIGHT](
	[SightId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Data] [image] NOT NULL,
	[Sort] [varchar](5) NOT NULL,
	[SightType] [tinyint] NOT NULL,
	[Global] [bit] NULL,
	[Gruppe] [bit] NULL,
	[CreatedBy] [varchar](32) NOT NULL,
	[CreatedByGroup] [int] NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModifiedBy] [varchar](32) NULL,
	[ModificationDate] [datetime] NULL,
	[DeletedBy] [varchar](32) NULL,
	[DeletionDate] [datetime] NULL,
 CONSTRAINT [PK_LMS_SIGHT] PRIMARY KEY CLUSTERED 
(
	[SightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OM_ExchangeQueue]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OM_ExchangeQueue](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OMType] [int] NOT NULL,
	[OMTask] [int] NOT NULL,
	[LMSStatus] [int] NOT NULL,
	[OMStatus] [int] NOT NULL,
	[Availability_ID] [int] NULL,
	[Delivery_ID] [int] NULL,
	[Avail2Deli_ID] [int] NULL,
	[OrderManagement_ID] [varchar](20) NOT NULL,
	[Comment] [varchar](1024) NULL,
	[Article_ID] [int] NOT NULL,
	[Quality_ID] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[CreateUser] [varchar](50) NULL,
	[ChangeDate] [datetime] NULL,
	[ChangeUser] [varchar](50) NULL,
	[ExecutionDate] [datetime] NULL,
	[PreferredPlace] [varchar](128) NULL,
	[PreferredTime] [varchar](64) NULL,
	[ABNumber] [varchar](50) NULL,
	[LSNumber] [varchar](50) NULL,
	[AUNumber] [varchar](50) NULL,
	[CompanyName] [varchar](128) NULL,
	[Street] [varchar](128) NULL,
	[StreetNumber] [varchar](20) NULL,
	[PostalCode] [varchar](20) NULL,
	[City] [varchar](128) NULL,
	[Country] [varchar](128) NULL,
	[ExecutionPeriodBegin] [datetime] NULL,
	[ExecutionPeriodEnd] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Index [IDX_loc_id]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_loc_id] ON [dbo].[GEO_coordinates]
(
	[loc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_loc_id]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_loc_id] ON [dbo].[GEO_textdata]
(
	[loc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_text_val]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_text_val] ON [dbo].[GEO_textdata]
(
	[text_val] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_Avail2Deli_AvailabilityID]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_Avail2Deli_AvailabilityID] ON [dbo].[LMS_AVAIL2DELI]
(
	[AvailabilityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_Avail2Deli_DateOfRelation]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_Avail2Deli_DateOfRelation] ON [dbo].[LMS_AVAIL2DELI]
(
	[DateOfRelation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_Avail2Deli_DeliveryID]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_Avail2Deli_DeliveryID] ON [dbo].[LMS_AVAIL2DELI]
(
	[DeliveryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_Avail2Deli_LadeterminDatum]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_Avail2Deli_LadeterminDatum] ON [dbo].[LMS_AVAIL2DELI]
(
	[LadeterminDatum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AvailabilityID]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AvailabilityID] ON [dbo].[LMS_AVAILABILITY]
(
	[AvailabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AvailableFromDate]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AvailableFromDate] ON [dbo].[LMS_AVAILABILITY]
(
	[AvailableFromDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AvailableUntilDate]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AvailableUntilDate] ON [dbo].[LMS_AVAILABILITY]
(
	[AvailableUntilDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_Zipcode]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_Zipcode] ON [dbo].[LMS_AVAILABILITY]
(
	[ZipCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AVAILABILITY_HISTORY_AvailabilityId]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AVAILABILITY_HISTORY_AvailabilityId] ON [dbo].[LMS_AVAILABILITY_HISTORY]
(
	[AvailabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AVAILABILITY_HISTORY_DataCreationDate]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AVAILABILITY_HISTORY_DataCreationDate] ON [dbo].[LMS_AVAILABILITY_HISTORY]
(
	[DataCreationDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AvailabilityLtvFromDate]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AvailabilityLtvFromDate] ON [dbo].[LMS_AVAILABILITY_LTV]
(
	[AvailableFromDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AvailabilityLtvUntilDate]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AvailabilityLtvUntilDate] ON [dbo].[LMS_AVAILABILITY_LTV]
(
	[AvailableUntilDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_AvailabilityLtvZipcode]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AvailabilityLtvZipcode] ON [dbo].[LMS_AVAILABILITY_LTV]
(
	[ZipCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_LMS_AVAILABILITYTYPE]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IX_LMS_AVAILABILITYTYPE] ON [dbo].[LMS_AVAILABILITYTYPE]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AdressNr]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_AdressNr] ON [dbo].[LMS_CUSTOMER]
(
	[AdressNr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_Kontoname]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_Kontoname] ON [dbo].[LMS_CUSTOMER]
(
	[Kontoname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_PLZ]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_PLZ] ON [dbo].[LMS_CUSTOMER]
(
	[PLZ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_DeliveryFromDate]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_DeliveryFromDate] ON [dbo].[LMS_DELIVERY]
(
	[FromDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_DeliveryUntilDate]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_DeliveryUntilDate] ON [dbo].[LMS_DELIVERY]
(
	[UntilDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_DeliveryZipcode]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_DeliveryZipcode] ON [dbo].[LMS_DELIVERY]
(
	[ZipCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_DELIVERY_HISTORY_DataCreationDate]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_DELIVERY_HISTORY_DataCreationDate] ON [dbo].[LMS_DELIVERY_HISTORY]
(
	[DataCreationDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_DELIVERY_HISTORY_DeliveryId]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_DELIVERY_HISTORY_DeliveryId] ON [dbo].[LMS_DELIVERY_HISTORY]
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_LMS_DELIVERYCATEGORY]    Script Date: 29/01/2020 12:07:41 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_LMS_DELIVERYCATEGORY] ON [dbo].[LMS_DELIVERYCATEGORY]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_DeliveryID]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [IDX_DeliveryID] ON [dbo].[LMS_DELIVERYDETAIL]
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [lmsunique]    Script Date: 29/01/2020 12:07:41 ******/
CREATE NONCLUSTERED INDEX [lmsunique] ON [dbo].[LMS_NOTE]
(
	[AvailabilityId] ASC,
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LMS_APPROACH] ADD  CONSTRAINT [DF_LMS_APPROACH_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_State]  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_AbweichenderArtikel]  DEFAULT ('') FOR [AbweichenderArtikel]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_FrachtvorschriftFelder]  DEFAULT ('') FOR [FrachtvorschriftFelder]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_FrachtvorschriftStapelhoehe]  DEFAULT ((0)) FOR [FrachtvorschriftStapelhoehe]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_Revision]  DEFAULT ((0)) FOR [Revision]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_StatusGedruckFrachtvorschrift]  DEFAULT ((0)) FOR [StatusGedrucktFrachtvorschrift]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_StatusGedrucktZuordnung]  DEFAULT ((0)) FOR [StatusGedrucktZuordnung]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_ICTransport]  DEFAULT ((0)) FOR [ICTransport]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_Timocom]  DEFAULT ((0)) FOR [Timocom]
GO
ALTER TABLE [dbo].[LMS_AVAIL2DELI] ADD  CONSTRAINT [DF_LMS_AVAIL2DELI_RowGuid]  DEFAULT (newid()) FOR [RowGuid]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_TempAlloc]  DEFAULT ((0)) FOR [TempAlloc]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_TempReserved]  DEFAULT ((0)) FOR [TempReserved]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_IsManual]  DEFAULT ((1)) FOR [IsManual]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_Type]  DEFAULT ((1)) FOR [TypeId]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_ClientId]  DEFAULT ((1)) FOR [ClientId]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_AvailableFromDate]  DEFAULT (CONVERT([datetime],CONVERT([varchar],getdate(),(104))+' 00:00:00',(0))) FOR [AvailableFromDate]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_AvailableFromTime]  DEFAULT (getdate()) FOR [AvailableFromTime]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_finished]  DEFAULT ((0)) FOR [finished]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_IsMixedDelivery]  DEFAULT ((0)) FOR [IsMixedAvailability]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_Geblockt]  DEFAULT ((0)) FOR [Geblockt]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_Revision]  DEFAULT ((0)) FOR [Revision]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_Storniert]  DEFAULT ((0)) FOR [Storniert]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_Rejection]  DEFAULT ((0)) FOR [Rejection]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_Reklamation]  DEFAULT ((0)) FOR [Reklamation]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_RejectionDone]  DEFAULT ((0)) FOR [RejectionDone]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_RejectionCost]  DEFAULT ((0)) FOR [RejectionCost]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_IDDroht]  DEFAULT ((0)) FOR [ICDroht]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_ICDurchfuehren]  DEFAULT ((0)) FOR [ICDurchfuehren]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_ZuAvisieren]  DEFAULT ((0)) FOR [ZuAvisieren]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_IstAvisiert]  DEFAULT ((0)) FOR [IstAvisiert]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_IcGrundID]  DEFAULT ((1)) FOR [IcGrundID]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_RowGuid]  DEFAULT (newid()) FOR [RowGuid]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_TempAlloc]  DEFAULT ((0)) FOR [TempAlloc]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_TempReserved]  DEFAULT ((0)) FOR [TempReserved]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_IsManual]  DEFAULT ((1)) FOR [IsManual]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_Type]  DEFAULT ((1)) FOR [TypeId]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_ClientId]  DEFAULT ((1)) FOR [ClientId]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_AvailableFromDate]  DEFAULT (CONVERT([datetime],CONVERT([varchar],getdate(),(104))+' 00:00:00',(0))) FOR [AvailableFromDate]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_AvailableFromTime]  DEFAULT (getdate()) FOR [AvailableFromTime]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_finished]  DEFAULT ((0)) FOR [finished]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITY_LTV] ADD  CONSTRAINT [DF_LMS_AVAILABILITY_LTV_IsMixedDelivery]  DEFAULT ((0)) FOR [IsMixedAvailability]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITYSUBTYPE] ADD  CONSTRAINT [DF_LMS_AVAILABILITYSUBTYPE_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_AVAILABILITYTYPE] ADD  CONSTRAINT [DF_LMS_AVAILABILITYTYPE_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_CLASSIFICATION] ADD  CONSTRAINT [DF_LMS_CLASSIFICATION_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_CLIENT] ADD  CONSTRAINT [DF_LMS_CLIENT_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_COUNTRY] ADD  CONSTRAINT [DF_LMS_COUNTRY_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_DATEFILTERFIELD] ADD  CONSTRAINT [DF_LMS_DATEFILTERFIELD_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_TempAllocAvail]  DEFAULT ((0)) FOR [TempAllocAvail]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_finished]  DEFAULT ((0)) FOR [finished]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_ClientId]  DEFAULT ((1)) FOR [ClientId]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_CountryId]  DEFAULT ((1)) FOR [CountryId]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_Reklamation]  DEFAULT ((0)) FOR [Reklamation]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_RejectionCost]  DEFAULT ((0)) FOR [RejectionCost]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_IsMixedDelivery]  DEFAULT ((0)) FOR [IsMixedDelivery]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_Geblockt]  DEFAULT ((0)) FOR [Geblockt]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_Storniert]  DEFAULT ((0)) FOR [Storniert]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_Revision]  DEFAULT ((0)) FOR [Revision]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_Fixmenge]  DEFAULT ((0)) FOR [Fixmenge]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_Jumboladung]  DEFAULT ((0)) FOR [Jumboladung]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_IstBedarf]  DEFAULT ((0)) FOR [IstBedarf]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_IstPlanmenge]  DEFAULT ((0)) FOR [IstPlanmenge]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_FvKoffer]  DEFAULT ((0)) FOR [FvKoffer]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_FvPlane]  DEFAULT ((0)) FOR [FvPlane]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_FvStapelhoehe]  DEFAULT ((0)) FOR [FvStapelhoehe]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_FvVerladerichtungLaengs]  DEFAULT ((0)) FOR [FvVerladerichtungLaengs]
GO
ALTER TABLE [dbo].[LMS_DELIVERY] ADD  CONSTRAINT [DF_LMS_DELIVERY_RowGuid]  DEFAULT (newid()) FOR [RowGuid]
GO
ALTER TABLE [dbo].[LMS_DELIVERYCATEGORY] ADD  CONSTRAINT [DF_LMS_DELIVERYCATEGORY_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_DELIVERYDETAIL] ADD  CONSTRAINT [DF_LMS_DELIVERYDETAIL_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[LMS_DELIVERYDETAIL] ADD  CONSTRAINT [DF_LMS_DELIVERYDETAIL_Finished]  DEFAULT ((0)) FOR [Finished]
GO
ALTER TABLE [dbo].[LMS_DELIVERYDETAIL] ADD  CONSTRAINT [DF_LMS_DELIVERYDETAIL_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_FILTER] ADD  CONSTRAINT [DF_LMS_FILTER_FilterType]  DEFAULT ((1)) FOR [FilterType]
GO
ALTER TABLE [dbo].[LMS_FILTER] ADD  CONSTRAINT [DF_LMS_FILTER_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_ICGRUND] ADD  CONSTRAINT [DF_LMS_ICGRUND_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_NOTE] ADD  CONSTRAINT [DF_LMS_NOTE_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_PALLETTYPE] ADD  CONSTRAINT [DF_LMS_PALLETTYPE_IsMixdelivery]  DEFAULT ((0)) FOR [IsMixType]
GO
ALTER TABLE [dbo].[LMS_PALLETTYPE] ADD  CONSTRAINT [DF_LMS_PALLETTYPE_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_PRIORITAET] ADD  CONSTRAINT [DF_LMS_PRIORITAET_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_QUALI2PALLET] ADD  CONSTRAINT [DF_LMS_QUALI2PALLET_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_QUALITY] ADD  CONSTRAINT [DF_LMS_QUALITY_QualityValue]  DEFAULT ((1)) FOR [QualityValue]
GO
ALTER TABLE [dbo].[LMS_QUALITY] ADD  CONSTRAINT [DF_LMS_QUALITY_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_REJECTIONREASON] ADD  CONSTRAINT [DF_LMS_REJECTIONREASON_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_RELIABILITY] ADD  CONSTRAINT [DF_LMS_RELIABILITY_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LMS_SIGHT] ADD  CONSTRAINT [DF_LMS_SIGHT_Sort]  DEFAULT ((99999)) FOR [Sort]
GO
ALTER TABLE [dbo].[LMS_SIGHT] ADD  CONSTRAINT [DF_LMS_SIGHT_Global]  DEFAULT ((0)) FOR [Global]
GO
ALTER TABLE [dbo].[LMS_SIGHT] ADD  CONSTRAINT [DF_LMS_SIGHT_Gruppe]  DEFAULT ((0)) FOR [Gruppe]
GO
ALTER TABLE [dbo].[LMS_SIGHT] ADD  CONSTRAINT [DF_LMS_SIGHT_CreatedByGroup]  DEFAULT ((7)) FOR [CreatedByGroup]
GO
ALTER TABLE [dbo].[LMS_SIGHT] ADD  CONSTRAINT [DF_LMS_SIGHT_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[OM_ExchangeQueue] ADD  CONSTRAINT [DF_OM_ExchangeQueue_OMTask]  DEFAULT ((0)) FOR [OMTask]
GO
ALTER TABLE [dbo].[OM_ExchangeQueue] ADD  CONSTRAINT [DF_OM_ExchangeQueue_Article_ID]  DEFAULT ((0)) FOR [Article_ID]
GO
ALTER TABLE [dbo].[OM_ExchangeQueue] ADD  CONSTRAINT [DF_OM_ExchangeQueue_Quality_ID]  DEFAULT ((0)) FOR [Quality_ID]
GO
ALTER TABLE [dbo].[GEO_coordinates]  WITH CHECK ADD  CONSTRAINT [FK__GEO_coord__loc_i__22AA2996] FOREIGN KEY([loc_id])
REFERENCES [dbo].[GEO_locations] ([loc_id])
GO
ALTER TABLE [dbo].[GEO_coordinates] CHECK CONSTRAINT [FK__GEO_coord__loc_i__22AA2996]
GO
ALTER TABLE [dbo].[GEO_textdata]  WITH CHECK ADD  CONSTRAINT [FK__GEO_textd__loc_i__24927208] FOREIGN KEY([loc_id])
REFERENCES [dbo].[GEO_locations] ([loc_id])
GO
ALTER TABLE [dbo].[GEO_textdata] CHECK CONSTRAINT [FK__GEO_textd__loc_i__24927208]
GO
ALTER TABLE [dbo].[LMS_DELIVERYDETAIL]  WITH CHECK ADD  CONSTRAINT [FK_LMS_DELIVERYDETAIL_LMS_DELIVERY] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[LMS_DELIVERY] ([DiliveryId])
GO
ALTER TABLE [dbo].[LMS_DELIVERYDETAIL] CHECK CONSTRAINT [FK_LMS_DELIVERYDETAIL_LMS_DELIVERY]
GO
ALTER TABLE [dbo].[LMS_QUALI2PALLET]  WITH CHECK ADD  CONSTRAINT [FK_LMS_QUALI2PALLET_LMS_PALLETTYPE] FOREIGN KEY([PalletTypeId])
REFERENCES [dbo].[LMS_PALLETTYPE] ([PalletTypeId])
GO
ALTER TABLE [dbo].[LMS_QUALI2PALLET] CHECK CONSTRAINT [FK_LMS_QUALI2PALLET_LMS_PALLETTYPE]
GO
ALTER TABLE [dbo].[LMS_QUALI2PALLET]  WITH CHECK ADD  CONSTRAINT [FK_LMS_QUALI2PALLET_LMS_QUALITY] FOREIGN KEY([QualityId])
REFERENCES [dbo].[LMS_QUALITY] ([QualityId])
GO
ALTER TABLE [dbo].[LMS_QUALI2PALLET] CHECK CONSTRAINT [FK_LMS_QUALI2PALLET_LMS_QUALITY]
GO
ALTER TABLE [dbo].[GEO_coordinates]  WITH CHECK ADD  CONSTRAINT [CK__GEO_coord__coord__239E4DCF] CHECK  (([coord_type]=(200100000)))
GO
ALTER TABLE [dbo].[GEO_coordinates] CHECK CONSTRAINT [CK__GEO_coord__coord__239E4DCF]
GO
ALTER TABLE [dbo].[GEO_locations]  WITH CHECK ADD  CONSTRAINT [CK__GEO_locat__loc_t__21B6055D] CHECK  (([loc_type]=(100100000) OR [loc_type]=(100200000) OR [loc_type]=(100300000) OR [loc_type]=(100400000) OR [loc_type]=(100500000) OR [loc_type]=(100600000) OR [loc_type]=(100700000) OR [loc_type]=(100800000) OR [loc_type]=(1)))
GO
ALTER TABLE [dbo].[GEO_locations] CHECK CONSTRAINT [CK__GEO_locat__loc_t__21B6055D]
GO
/****** Object:  StoredProcedure [dbo].[AddAvailabilityHistory]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddAvailabilityHistory] 
	-- Add the parameters for the stored procedure here
	@AvailabilityId int,
   @Username varchar(32)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO [dbo].[LMS_AVAILABILITY_HISTORY]
           ([AvailabilityId]
           ,[Geblockt]
           ,[GeblocktVon]
           ,[GeblocktDatum]
           ,[GeblocktAufgehobenVon]
           ,[GeblocktAufgehobenDatum]
           ,[GeblocktFuer]
           ,[AvailableFromDate]
           ,[AvailableUntilDate]
           ,[DataCreatedBy]
           ,[DataCreationDate]
           ,[CreatedBy]
           ,[CreationDate])
     SELECT [AvailabilityId]
           ,[Geblockt]
           ,[GeblocktVon]
           ,[GeblocktDatum]
           ,[GeblocktAufgehobenVon]
           ,[GeblocktAufgehobenDatum]
           ,[GeblocktFuer]
           ,[AvailableFromDate]
           ,[AvailableUntilDate]
           ,ISNULL([ModifiedBy], [CreatedBy])
           ,ISNULL([ModificationDate], [CreationDate])
           ,@Username
           ,GETDATE()
      FROM dbo.LMS_AVAILABILITY WHERE AvailabilityId = @AvailabilityId
END
GO
/****** Object:  StoredProcedure [dbo].[AddDeliveryHistory]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddDeliveryHistory] 
	-- Add the parameters for the stored procedure here
	@DeliveryId int,
   @Username varchar(32)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO [dbo].[LMS_DELIVERY_HISTORY]
           ([DeliveryId]
           ,[Geblockt]
           ,[GeblocktVon]
           ,[GeblocktDatum]
           ,[GeblocktAufgehobenVon]
           ,[GeblocktAufgehobenDatum]
           ,[GeblocktFuer]
           ,[FromDate]
           ,[UntilDate]
           ,[DataCreatedBy]
           ,[DataCreationDate]
           ,[CreatedBy]
           ,[CreationDate])
     SELECT [DiliveryId]
           ,[Geblockt]
           ,[GeblocktVon]
           ,[GeblocktDatum]
           ,[GeblocktAufgehobenVon]
           ,[GeblocktAufgehobenDatum]
           ,[GeblocktFuer]
           ,[FromDate]
           ,[UntilDate]
           ,ISNULL([ModifiedBy], [CreatedBy])
           ,ISNULL([ModificationDate], [CreationDate])
           ,@Username
           ,GETDATE()
      FROM LMS_DELIVERY WHERE DiliveryId = @DeliveryId
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateLMSFromERP]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateLMSFromERP]
	-- Add the parameters for the stored procedure here
	@ZuordnungID int,
   @LieferscheinNummer varchar(32),
   @FrachtauftragNummer varchar(32)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   DECLARE @DeliveryID int;

   DECLARE @OrderManagementID varchar(64);
   DECLARE @ExecutionPeriodStart DateTime;
   DECLARE @ExecutionPeriodEnd DateTime;
   DECLARE @ArticleID int;
   DECLARE @QualityID int;

   -- Delivery ID ermitteln
   SELECT @DeliveryID=DeliveryId 
      FROM dbo.LMS_AVAIL2DELI 
      WHERE Avail2DeliId = @ZuordnungID

   -- Daten für Ordermanagement Update ermitteln
   SELECT @OrderManagementID = ISNULL(OrderManagementID, ''), @ExecutionPeriodStart = FromDate, @ExecutionPeriodEnd = UntilDate,
         @ArticleID = PalletType, @QualityID = Quality
      FROM dbo.LMS_DELIVERY 
      WHERE DiliveryID = @DeliveryID

	BEGIN TRANSACTION

   -- Zuordnung updaten
   UPDATE dbo.LMS_AVAIL2DELI SET
      LieferscheinNr = @LieferscheinNummer
      , FrachtauftragsNr = @FrachtauftragNummer
   WHERE Avail2DeliId = @ZuordnungID
	IF @@ERROR <> 0
		BEGIN
		ROLLBACK

		RETURN
	   END

   -- Liefertermin updaten
   UPDATE dbo.LMS_DELIVERY
   SET 
      DeliveryNoteNo = @LieferscheinNummer
      , FrachtauftragNo = @FrachtauftragNummer
    WHERE Diliveryid = @DeliveryID
	IF @@ERROR <> 0
		BEGIN
		ROLLBACK

		RETURN
	   END

   -- Ordermanagement Update
   IF(@OrderManagementID != '')
      BEGIN
      INSERT INTO [dbo].[OM_ExchangeQueue]
           ([OMType]
           ,[OMTask]
           ,[LMSStatus]
           ,[OMStatus]
           ,[Availability_ID]
           ,[Delivery_ID]
           ,[Avail2Deli_ID]
           ,[OrderManagement_ID]
           ,[Comment]
           ,[Article_ID]
           ,[Quality_ID]
           ,[CreateDate]
           ,[CreateUser]
           ,[ChangeDate]
           ,[ChangeUser]
           ,[ExecutionDate]
           ,[PreferredPlace]
           ,[PreferredTime]
           ,[ABNumber]
           ,[LSNumber]
           ,[AUNumber]
           ,[CompanyName]
           ,[Street]
           ,[StreetNumber]
           ,[PostalCode]
           ,[City]
           ,[Country]
           ,[ExecutionPeriodBegin]
           ,[ExecutionPeriodEnd])
      VALUES
           (1
           ,0
           ,0
           ,7
           ,NULL
           ,@DeliveryID
           ,@ZuordnungID
           ,@OrderManagementID
           ,''
           ,@ArticleID
           ,@QualityID
           ,GETDATE()
           ,'ERP'
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,@LieferscheinNummer
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,@ExecutionPeriodStart
           ,@ExecutionPeriodEnd)
	   IF @@ERROR <> 0
		   BEGIN
		   ROLLBACK

		   RETURN
	      END
      END

	COMMIT

END


GO
/****** Object:  StoredProcedure [dbo].[UpdateCustomerFromERP]    Script Date: 29/01/2020 12:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomerFromERP]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	BEGIN TRANSACTION

    DELETE FROM dbo.LMS_CUSTOMER;
	IF @@ERROR <> 0
		BEGIN
		ROLLBACK

		RETURN
	END

	INSERT INTO dbo.LMS_CUSTOMER SELECT * FROM dbo.LMS_CUSTOMER_VIEW;
	IF @@ERROR <> 0
		BEGIN
		ROLLBACK

		RETURN
	END

	COMMIT
END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 = frei, 1 = reserviert, 2 = zugeordnet' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LMS_AVAIL2DELI', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'delete me' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LMS_AVAILABILITY', @level2type=N'COLUMN',@level2name=N'ZipCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'delete me' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LMS_AVAILABILITY', @level2type=N'COLUMN',@level2name=N'TempAlloc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'delete me' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LMS_AVAILABILITY', @level2type=N'COLUMN',@level2name=N'TempReserved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LMS_AVAILABILITY"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 241
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 79
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 30
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 8925
         Alias = 1545
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMS_VIEW_AVAILABILITY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMS_VIEW_AVAILABILITY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 3
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 51
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMS_VIEW_AVAILABILITY_COMBINED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMS_VIEW_AVAILABILITY_COMBINED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[20] 2[18] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "av"
            Begin Extent = 
               Top = 6
               Left = 917
               Bottom = 121
               Right = 1137
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pt"
            Begin Extent = 
               Top = 6
               Left = 296
               Bottom = 121
               Right = 465
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "q"
            Begin Extent = 
               Top = 6
               Left = 503
               Bottom = 121
               Right = 672
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 710
               Bottom = 121
               Right = 879
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cust_kunde"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 200
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cust_lieferadresse"
            Begin Extent = 
               Top = 126
               Left = 238
               Bottom = 241
               Right = 400
            End
            DisplayFlags = 280
            TopCo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertung'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'lumn = 0
         End
         Begin Table = "av_kunde"
            Begin Extent = 
               Top = 126
               Left = 438
               Bottom = 241
               Right = 600
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "av_ladestelle"
            Begin Extent = 
               Top = 126
               Left = 845
               Bottom = 241
               Right = 1007
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DeliCat"
            Begin Extent = 
               Top = 126
               Left = 638
               Bottom = 241
               Right = 807
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 73
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1965
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2625
         Alias = 2865
         Table = 1170
         Output = 1170
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertung'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertung'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2[61] 3) )"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2) )"
      End
      ActivePaneConfig = 5
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 131
               Right = 276
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 314
               Bottom = 131
               Right = 495
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "q"
            Begin Extent = 
               Top = 6
               Left = 533
               Bottom = 131
               Right = 714
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 752
               Bottom = 131
               Right = 933
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rr"
            Begin Extent = 
               Top = 132
               Left = 662
               Bottom = 257
               Right = 848
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ap"
            Begin Extent = 
               Top = 132
               Left = 886
               Bottom = 257
               Right = 1067
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cust"
            Begin Extent = 
               Top = 6
               Left = 971
               Bottom = 131
               Right = 1141
            End
            DisplayFlags = 280
            TopC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertungLiefertermin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'olumn = 0
         End
         Begin Table = "cust_kunde"
            Begin Extent = 
               Top = 132
               Left = 38
               Bottom = 257
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cust_lieferadresse"
            Begin Extent = 
               Top = 132
               Left = 246
               Bottom = 257
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cust_ladestelle"
            Begin Extent = 
               Top = 132
               Left = 454
               Bottom = 257
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "bt"
            Begin Extent = 
               Top = 258
               Left = 38
               Bottom = 383
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertungLiefertermin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertungLiefertermin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2[66] 3) )"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2) )"
      End
      ActivePaneConfig = 5
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "av"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 131
               Right = 276
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 314
               Bottom = 131
               Right = 495
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "q"
            Begin Extent = 
               Top = 6
               Left = 533
               Bottom = 131
               Right = 714
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "at"
            Begin Extent = 
               Top = 6
               Left = 752
               Bottom = 131
               Right = 933
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 971
               Bottom = 131
               Right = 1152
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rr"
            Begin Extent = 
               Top = 132
               Left = 38
               Bottom = 257
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ap"
            Begin Extent = 
               Top = 132
               Left = 262
               Bottom = 257
               Right = 443
            End
            DisplayFlags = 280
            TopColum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertungVerfuegbarkeit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'n = 0
         End
         Begin Table = "cust_kontakt"
            Begin Extent = 
               Top = 132
               Left = 481
               Bottom = 257
               Right = 651
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cust_ladeadresse"
            Begin Extent = 
               Top = 132
               Left = 689
               Bottom = 257
               Right = 859
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertungVerfuegbarkeit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LMSAuswertungVerfuegbarkeit'
GO
USE [master]
GO
ALTER DATABASE [LMS] SET  READ_WRITE 
GO
