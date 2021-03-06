/****** Object:  Database [LtmsDbFait]    Script Date: 19.02.2020 15:03:04 ******/
DROP DATABASE [LtmsDbFait]
GO

/****** Object:  Database [LtmsDbFait]    Script Date: 19.02.2020 15:03:05 ******/
CREATE DATABASE [LtmsDbFait]  (EDITION = 'Basic', SERVICE_OBJECTIVE = 'Basic', MAXSIZE = 2 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS;
GO

ALTER DATABASE [LtmsDbFait] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [LtmsDbFait] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [LtmsDbFait] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [LtmsDbFait] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [LtmsDbFait] SET ARITHABORT OFF 
GO

ALTER DATABASE [LtmsDbFait] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [LtmsDbFait] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [LtmsDbFait] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [LtmsDbFait] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [LtmsDbFait] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [LtmsDbFait] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [LtmsDbFait] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [LtmsDbFait] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [LtmsDbFait] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO

ALTER DATABASE [LtmsDbFait] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [LtmsDbFait] SET READ_COMMITTED_SNAPSHOT ON 
GO

ALTER DATABASE [LtmsDbFait] SET  MULTI_USER 
GO

ALTER DATABASE [LtmsDbFait] SET ENCRYPTION ON
GO

ALTER DATABASE [LtmsDbFait] SET QUERY_STORE = ON
GO

ALTER DATABASE [LtmsDbFait] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 7), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 10, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [LtmsDbFait] SET  READ_WRITE 
GO


USE [LtmsDbFait]
/****** Object:  Schema [Condition]    Script Date: 29/01/2020 11:39:15 ******/
CREATE SCHEMA [Condition]
GO

/****** Object:  Schema [LTMS]    Script Date: 29/01/2020 11:39:15 ******/
CREATE SCHEMA [LTMS]
GO
/****** Object:  Schema [Transaction]    Script Date: 29/01/2020 11:39:15 ******/
CREATE SCHEMA [Transaction]
GO
/****** Object:  Schema [Voucher]    Script Date: 29/01/2020 11:39:15 ******/
CREATE SCHEMA [Voucher]
GO
/****** Object:  UserDefinedTableType [dbo].[BookingSyncStateType]    Script Date: 29/01/2020 11:39:15 ******/
CREATE TYPE [dbo].[BookingSyncStateType] AS TABLE(
	[ErrorCode] [int] NULL,
	[Account_ID] [int] NOT NULL,
	[ID] [int] NOT NULL,
	[LastSync] [datetime] NOT NULL,
	[SyncOperation] [int] NOT NULL,
	[ChangeOperation] [int] NOT NULL,
	[RequestEnqueued] [datetime] NOT NULL,
	[RequestDeQueued] [datetime] NULL,
	[RequestDeferred] [datetime] NULL,
	[ResponseEnqueued] [datetime] NULL,
	[ResponseDeQueued] [datetime] NULL,
	[ResponseDeferred] [datetime] NULL,
	[Acknowledged] [datetime] NULL,
	[SessionId] [nvarchar](128) NULL
)
GO
/****** Object:  UserDefinedFunction [dbo].[fn_account_path2id]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 18.10.2012
-- Description:	Konvertiert den Pfad eines Kontos zu seiner ID
-- =============================================
CREATE FUNCTION [dbo].[fn_account_path2id] (@pathbyname NVARCHAR(1500))
RETURNS INT
AS 
    BEGIN
	-- Declare the return variable here
        DECLARE @id INT

	-- Add the T-SQL statements to compute the return value here
        SELECT  @id = ID
        FROM    dbo.Accounts
        WHERE   PathByName = @pathbyname

	-- Return the result of the function
        RETURN @id

    END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_calculate_pathbyid]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 25.09.2012
-- Description:	Liefert ein neu berechnetes PathById als Ergebniss in der Schreibweise /ID/ zurück
-- =============================================
CREATE FUNCTION [dbo].[fn_calculate_pathbyid] (@id INT)
RETURNS NVARCHAR(1500)
AS 
    BEGIN
		-- Declare the return variable here
        DECLARE @ResultVar VARCHAR(1500)
			
        SELECT  @ResultVar = CONVERT(VARCHAR(1500), COALESCE(@ResultVar, '/') + CAST(dbo.Accounts.ID AS VARCHAR(64)) + '/')
        FROM    dbo.fn_query_single_path_lr(@id)
                JOIN dbo.Accounts
                ON dbo.fn_query_single_path_lr.ID = dbo.Accounts.ID
		
		-- Return the result of the function
        RETURN @ResultVar

    END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_calculate_pathbyname]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 25.09.2012
-- Description:	Liefert ein neu berechnetes PathByName als Ergebniss in der Schreibweise ROOT bzw ROOT:ID zurück
-- =============================================
CREATE FUNCTION [dbo].[fn_calculate_pathbyname] (@id INT)
RETURNS VARCHAR(1500)
AS 
    BEGIN
		-- Declare the return variable here
        DECLARE @ResultVar VARCHAR(1500)
			
        SELECT  @ResultVar = CONVERT(VARCHAR(1500), COALESCE(@ResultVar + ':', '') + dbo.Accounts.PathName)
        FROM    dbo.fn_query_single_path_lr(@id)
                JOIN dbo.Accounts
                ON dbo.fn_query_single_path_lr.ID = dbo.Accounts.ID
		
		-- Return the result of the function
        RETURN @ResultVar

    END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_direction2sign]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 22062017
-- Description:	Einfach konvertierung von Richtung nach Vorzeichen
-- =============================================
CREATE FUNCTION [dbo].[fn_direction2sign] (
    -- Add the parameters for the function here
    @account_direction CHAR(1))
RETURNS INT
AS
BEGIN
    -- Declare the return variable here
    DECLARE @sign int;

    IF (@account_direction = 'Q')
        SET @sign = -1;
    ELSE
        SET @sign = 1;

    -- Return the result of the function
    RETURN @sign;

END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_accountid]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 02.02.2009
-- Description:	Liefert die AccountId für eine Kontonummer
-- =============================================
CREATE FUNCTION [dbo].[fn_get_accountid]
(	
	@AccountNumber varchar(13)	
)
RETURNS int
AS
BEGIN	
	DECLARE @ID int;
	
	SET @ID= (SELECT ID FROM Accounts WHERE AccountNumber = @AccountNumber)			
		
	RETURN @ID;
	
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_BalanceRentTable]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_get_BalanceRentTable]
    (
      @ParPrimaryAccountNumber VARCHAR(13) ,
      @ParAccountNumbers NVARCHAR(2000) ,
      @ParBalanceDateFrom DATE ,
      @ParBalanceDateTo DATE ,
      @ParArticleID INT ,
      @ParQualityIDs NVARCHAR(2000) ,
      @ParFactor INT ,
      @ParIncludeInBalance BIT ,
      @ParMatched BIT ,
      @ParPreInvoiceTotal DECIMAL(20, 2)
    )
RETURNS @T TABLE
    (
      [BalanceDate] DATE NULL ,
      [IntDescription] VARCHAR(64) COLLATE Latin1_General_CI_AS
                                   NULL ,
      [Balance] INT NULL ,
      [BalanceWithFreeStock] INT NULL ,
      [Rent] MONEY NULL ,
      [RentDay] SMALLMONEY NULL ,
      [FreeStockDay] INT NULL
    )
    WITH EXEC AS CALLER
AS
    BEGIN
        DECLARE @MaxDaysFreeRent INT;

        DECLARE @AccountsTable TABLE
            (
              listpos INT IDENTITY(1, 1)
                          NOT NULL ,
              [str] VARCHAR(800) NOT NULL ,
              nstr NVARCHAR(800) NOT NULL ,
              UNIQUE ( [str], listpos )
            );
   
        INSERT  INTO @AccountsTable
                ( [str] ,
                  nstr
                )
                SELECT  [str] ,
                        nstr
                FROM    dbo.fn_iter_charlist_to_tbl(@ParAccountNumbers, ' ');

        DECLARE @QualitiesTable TABLE
            (
              listpos INT IDENTITY(1, 1)
                          NOT NULL ,
              number INT NOT NULL ,
              UNIQUE ( number, listpos )
            );
   
        INSERT  INTO @QualitiesTable
                ( number
                )
                SELECT  number
                FROM    dbo.fn_iter_intlist_to_tbl(@ParQualityIDs);

        SELECT  @MaxDaysFreeRent = ISNULL(MAX(cond.Quantity), 0)
        FROM    dbo.vConditions AS cond
                JOIN dbo.Accounts acc ON cond.Account_ID = acc.ID
      --LEFT JOIN @QualitiesTable qid_list ON cond.Quality_ID = qid_list.Number
        WHERE   ( cond.Term_ID = 'MF' )
                AND ( cond.Quality_ID IS NULL )
                AND ( acc.AccountNumber = @ParPrimaryAccountNumber )
                AND ( cond.Article_ID = @ParArticleID );

        WITH    MyBookings ( BalanceDate, BookingDate, Quantity )
                  AS (
      -- Saldo vor Abrechnungszeitraum
                       SELECT   DATEADD(d, -( @MaxDaysFreeRent + 1 ),
                                        @ParBalanceDateFrom) AS BalanceDate ,
                                DATEADD(d, -( @MaxDaysFreeRent + 1 ),
                                        @ParBalanceDateFrom) BookingDate ,
                                SUM(( @ParFactor * Quantity )) AS Quantity
                       FROM     Bookings
                                JOIN dbo.Accounts ON Bookings.Account_ID = dbo.Accounts.ID
                                JOIN @AccountsTable baid_list ON Accounts.AccountNumber = baid_list.str
                                JOIN @QualitiesTable qid_list ON Bookings.Quality_ID = qid_list.number
                                LEFT OUTER JOIN dbo.Transactions ON dbo.Bookings.Transaction_ID = dbo.Transactions.ID
                       WHERE    ( @ParIncludeInBalance IS NULL
                                  OR IncludeInBalance = @ParIncludeInBalance
                                )
                                AND ( @ParMatched IS NULL
                                      OR Matched = @ParMatched
                                    )
                                AND ( Bookings.Article_ID = @ParArticleID )
                                AND ( BookingDate < DATEADD(d,
                                                            -@MaxDaysFreeRent,
                                                            @ParBalanceDateFrom) )
                                AND ( Bookings.DeleteUser IS NULL )
         --AND ((TransactionState_ID != 4) AND (TransactionType_ID != 'STORNO'))
                       UNION ALL

      -- Buchungen aus Abrechnungszeitraum
                       SELECT   BalanceDate ,
                                BookingDate ,
                                Quantity
                       FROM     -- Neu
                                dbo.fn_get_DatesRentTableLimited(@ParPrimaryAccountNumber,
                                                              @ParAccountNumbers,
                                                              @ParArticleID,
                                                              @ParQualityIDs,
                                                              @ParFactor,
                                                              @ParIncludeInBalance,
                                                              @ParMatched,
                                                              DATEADD(d,
                                                              -@MaxDaysFreeRent,
                                                              @ParBalanceDateFrom))
                       WHERE    ( BalanceDate <= @ParBalanceDateTo )

      -- Alt
      --   dbo.fn_get_DatesRentTable(@ParPrimaryAccountNumber, @ParAccountNumbers, @ParArticleID, @ParQualityIDs, @ParFactor, @ParIncludeInBalance, @ParMatched)
      --WHERE 
      --   (BalanceDate <= @ParBalanceDateTo)
      --   AND (BookingDate >= DATEADD(d, -@MaxDaysFreeRent, @ParBalanceDateFrom))
                     ),
                BookingBalanceDatesTable ( BalanceDate )
                  AS ( SELECT   ( SELECT    MIN(BalanceDate)
                                  FROM      MyBookings
                                ) AS BalanceDate
                       UNION ALL
                       SELECT   DATEADD(dd, 1, BalanceDate)
                       FROM     BookingBalanceDatesTable
                       WHERE    DATEADD(dd, 1, BalanceDate) <= @ParBalanceDateTo
                     ),
                QueryDatesTable ( BalanceDate )
                  AS ( SELECT   CAST(@ParBalanceDateFrom AS DATE) AS BalanceDate
                       UNION ALL
                       SELECT   DATEADD(dd, 1, BalanceDate)
                       FROM     QueryDatesTable
                       WHERE    DATEADD(dd, 1, BalanceDate) <= @ParBalanceDateTo
                     ),
                GroupBalanceDateTable ( BalanceDate, Quantity )
                  AS ( SELECT TOP 100000000
                                BookingBalanceDatesTable.BalanceDate AS BalanceDate ,
                                SUM(ISNULL(MyBookings.Quantity, 0)) AS Quantity
                       FROM     MyBookings
                                RIGHT JOIN BookingBalanceDatesTable ON BookingBalanceDatesTable.BalanceDate = MyBookings.BalanceDate
                       GROUP BY BookingBalanceDatesTable.BalanceDate
                       ORDER BY BalanceDate
                     ),
                BalanceRentTable ( BalanceDate, Balance, BalanceWithFreeStock, Rent, RentDay, FreeStockDay )
                  AS ( SELECT   qdt.BalanceDate ,
                                SUM(x.Quantity) AS Balance ,
                                ( CASE WHEN SUM(x.Quantity) > 0
                                       THEN SUM(x.Quantity)
                                       WHEN SUM(x.Quantity)
                                            + ISNULL(dbo.fn_get_ConditionQuantity('FB',
                                                              @ParPrimaryAccountNumber,
                                                              qdt.BalanceDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs),
                                                     0) > 0 THEN 0
                                       ELSE SUM(x.Quantity)
                                            + ISNULL(dbo.fn_get_ConditionQuantity('FB',
                                                              @ParPrimaryAccountNumber,
                                                              qdt.BalanceDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs),
                                                     0)
                                  END ) AS BalanceWithFreeStock ,
                                ( CASE WHEN ( CASE WHEN SUM(x.Quantity) > 0
                                                   THEN SUM(x.Quantity)
                                                   WHEN SUM(x.Quantity)
                                                        + ISNULL(dbo.fn_get_ConditionQuantity('FB',
                                                              @ParPrimaryAccountNumber,
                                                              qdt.BalanceDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs),
                                                              0) > 0 THEN 0
                                                   ELSE SUM(x.Quantity)
                                                        + ISNULL(dbo.fn_get_ConditionQuantity('FB',
                                                              @ParPrimaryAccountNumber,
                                                              qdt.BalanceDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs),
                                                              0)
                                              END ) < 0
                                       THEN CAST(dbo.fn_get_ConditionAmount('M',
                                                              @ParPrimaryAccountNumber,
                                                              qdt.BalanceDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs) AS MONEY)
                                            * -( CASE WHEN SUM(x.Quantity) > 0
                                                      THEN SUM(x.Quantity)
                                                      WHEN SUM(x.Quantity)
                                                           + ISNULL(dbo.fn_get_ConditionQuantity('FB',
                                                              @ParPrimaryAccountNumber,
                                                              qdt.BalanceDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs),
                                                              0) > 0 THEN 0
                                                      ELSE SUM(x.Quantity)
                                                           + ISNULL(dbo.fn_get_ConditionQuantity('FB',
                                                              @ParPrimaryAccountNumber,
                                                              qdt.BalanceDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs),
                                                              0)
                                                 END )
                                       ELSE 0
                                  END ) AS Rent ,
                                dbo.fn_get_ConditionAmount('M',
                                                           @ParPrimaryAccountNumber,
                                                           qdt.BalanceDate,
                                                           @ParArticleID,
                                                           @ParQualityIDs) AS RentDay ,
                                ISNULL(dbo.fn_get_ConditionQuantity('FB',
                                                              @ParPrimaryAccountNumber,
                                                              qdt.BalanceDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs),
                                       0) AS FreeStockDay
                       FROM     QueryDatesTable qdt
                                LEFT JOIN GroupBalanceDateTable ON GroupBalanceDateTable.BalanceDate = qdt.BalanceDate
                                LEFT JOIN GroupBalanceDateTable x ON x.BalanceDate <= GroupBalanceDateTable.BalanceDate
                       GROUP BY qdt.BalanceDate
                     )
            --SELECT * 
   --FROM mybookings
   --order by BalanceDate

   INSERT   INTO @T
            SELECT  *
            FROM    ( SELECT    NULL AS BalanceDate ,
                                'Vorabrechnung' AS IntDescription ,
                                NULL AS Balance ,
                                NULL AS BalanceWithFreeStock ,
                                -@ParPreInvoiceTotal AS Rent ,
                                0 AS RentDay ,
                                0 AS FreeStockDay
                      WHERE     ( @ParPreInvoiceTotal != 0 )
                      UNION ALL
                      SELECT    BalanceDate ,
                                'Miete' AS IntDescription ,
                                ( @ParFactor * ISNULL(Balance, 0) ) AS Balance ,
                                ( @ParFactor * ISNULL(BalanceWithFreeStock, 0) ) AS BalanceWithFreeStock ,
                                Rent ,
                                RentDay ,
                                FreeStockDay
                      FROM      BalanceRentTable
                    ) X
   OPTION   ( MAXRECURSION 0 );

        RETURN;
    END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_ConditionAmount]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_get_ConditionAmount]
    (
      @ParConditionTermID VARCHAR(6) ,
      @ParPrimaryAccountNumber VARCHAR(13) ,
      @ParBookingDate DATE ,
      @ParArticleID INT ,
      @ParQualityIDs VARCHAR(1024)
    )
RETURNS SMALLMONEY
AS
    BEGIN
	-- Declare the return variable here
        DECLARE @ResultVar SMALLMONEY;

        SELECT TOP 1
                @ResultVar = cond.Amount
        FROM    dbo.vConditions AS cond
                JOIN dbo.Accounts acc ON cond.Account_ID = acc.ID
      --LEFT JOIN dbo.fn_iter_intlist_to_tbl(@ParQualityIDs) qid_list ON cond.Quality_ID = qid_list.Number
        WHERE   ( cond.Term_ID = @ParConditionTermID )
                AND ( cond.Quality_ID IS NULL )
                AND ( acc.AccountNumber = @ParPrimaryAccountNumber )
                AND ( cond.Article_ID = @ParArticleID )
                AND ( cond.ValidFrom <= @ParBookingDate )
                AND ( cond.ValidUntil IS NULL
                      OR @ParBookingDate <= cond.ValidUntil
                    ); 

	-- Return the result of the function
        RETURN @ResultVar;

    END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_ConditionQuantity]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_get_ConditionQuantity]
    (
      @ParConditionTermID VARCHAR(6) ,
      @ParPrimaryAccountNumber VARCHAR(13) ,
      @ParBookingDate DATE ,
      @ParArticleID INT ,
      @ParQualityIDs VARCHAR(1024)
    )
RETURNS INT
AS
    BEGIN
	-- Declare the return variable here
        DECLARE @ResultVar INT;

        SELECT TOP 1
                @ResultVar = cond.Quantity
        FROM    dbo.vConditions AS cond
                JOIN dbo.Accounts acc ON cond.Account_ID = acc.ID
      --LEFT JOIN dbo.fn_iter_intlist_to_tbl(@ParQualityIDs) qid_list ON cond.Quality_ID = qid_list.Number
        WHERE   ( cond.Term_ID = @ParConditionTermID )
                AND ( cond.Quality_ID IS NULL )
                AND ( acc.AccountNumber = @ParPrimaryAccountNumber )
                AND ( cond.Article_ID = @ParArticleID )
                AND ( cond.ValidFrom <= @ParBookingDate )
                AND ( cond.ValidUntil IS NULL
                      OR @ParBookingDate <= cond.ValidUntil
                    ); 

	-- Return the result of the function
        RETURN @ResultVar;

    END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_DatesRentTableLimited]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_get_DatesRentTableLimited]
    (
      @ParPrimaryAccountNumber VARCHAR(13) ,
      @ParAccountNumbers NVARCHAR(2000) ,
      @ParArticleID INT ,
      @ParQualityIDs NVARCHAR(2000) ,
      @ParFactor INT ,
      @ParIncludeInBalance BIT ,
      @ParMatched BIT ,
      @MinBookingDate DATETIME
    )
RETURNS @T TABLE
    (
      [Booking_ID] INT ,
      [BalanceDate] DATETIME ,
      [BookingDate] DATETIME ,
      [Quantity] INT
    )
    WITH EXEC AS CALLER
AS
    BEGIN

        DECLARE @AccountsTable TABLE
            (
              listpos INT IDENTITY(1, 1)
                          NOT NULL ,
              [str] VARCHAR(800) NOT NULL ,
              nstr NVARCHAR(800) NOT NULL ,
              UNIQUE ( [str], listpos )
            );
   
        INSERT  INTO @AccountsTable
                ( [str] ,
                  nstr
                )
                SELECT  [str] ,
                        nstr
                FROM    dbo.fn_iter_charlist_to_tbl(@ParAccountNumbers, ' ');

        DECLARE @QualitiesTable TABLE
            (
              listpos INT IDENTITY(1, 1)
                          NOT NULL ,
              number INT NOT NULL ,
              UNIQUE ( number, listpos )
            );
   
        INSERT  INTO @QualitiesTable
                ( number
                )
                SELECT  number
                FROM    dbo.fn_iter_intlist_to_tbl(@ParQualityIDs);


        WITH    MyBookings ( Booking_ID, BalanceDate, BookingDate, Quantity )
                  AS ( SELECT TOP 100000000
                                book.ID ,
                                DATEADD(dd,
                                        ( CASE WHEN ( ( SELECT
                                                              COUNT(DISTINCT b.Account_ID)
                                                        FROM  Bookings b
                                                              JOIN dbo.Accounts ON b.Account_ID = dbo.Accounts.ID
                                                              JOIN @AccountsTable baid_list ON Accounts.AccountNumber = baid_list.str
                                                        WHERE b.Transaction_ID = book.Transaction_ID
                                                      ) > 1 )
                                                    OR ( ( @ParFactor
                                                           * book.Quantity ) > 0
                                                         AND ( bt.OneDayDelayForRent = 0 )
                                                       )
                                                    OR ( ( @ParFactor
                                                           * book.Quantity ) < 0
                                                         AND ( bt.IncludeDaysFreeRentCondition = 0 )
                                                       ) THEN 0 -- Umbuchungen innerhalb der angegebenen Konten
                                               WHEN ( @ParFactor
                                                      * book.Quantity ) > 0
                                               THEN 1
                                               ELSE ISNULL(dbo.fn_get_ConditionQuantity('MF',
                                                              @ParPrimaryAccountNumber,
                                                              book.BookingDate,
                                                              @ParArticleID,
                                                              @ParQualityIDs),
                                                           0)
                                          END ), book.BookingDate) AS BalanceDate ,
                                book.BookingDate ,
                                ( @ParFactor * book.Quantity ) AS Quantity
                       FROM     dbo.Bookings AS book
                                JOIN dbo.Accounts ON book.Account_ID = dbo.Accounts.ID
                                JOIN @AccountsTable baid_list ON Accounts.AccountNumber = baid_list.str
                                LEFT JOIN dbo.BookingTypes AS bt ON book.BookingType_ID = bt.ID
                                LEFT JOIN dbo.Transactions ON book.Transaction_ID = dbo.Transactions.ID
                                JOIN @QualitiesTable qid_list ON book.Quality_ID = qid_list.number
                       WHERE    book.BookingDate >= @MinBookingDate
                                AND ( @ParIncludeInBalance IS NULL
                                      OR IncludeInBalance = @ParIncludeInBalance
                                    )
                                AND ( @ParMatched IS NULL
                                      OR Matched = @ParMatched
                                    )
                                AND ( book.Article_ID = @ParArticleID )
                                AND ( book.DeleteUser IS NULL )
                                AND ( ( TransactionState_ID != 4 )
                                      AND ( TransactionType_ID != 'STORNO' )
                                    )
      --ORDER BY DATEADD(dd, (CASE WHEN (@ParFactor * Quantity) > 0 THEN @ParDaysDelay ELSE CAST(@ParDaysFreeRent AS INT) END), BookingDate)
                     )
            INSERT  INTO @T
                    SELECT  *
                    FROM    MyBookings;
   --ORDER BY BalanceDate

        RETURN;

    END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_FilteredAccountIDList]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_get_FilteredAccountIDList]
(@ParAccountNumbers varchar(MAX), @ParAccountTypesName varchar(MAX), @ParTagsName varchar(MAX), @ParExcludeTagsName varchar(MAX), @ParUsername varchar(MAX))
RETURNS @AccountsTable table
(
[Account_ID] int,
   UNIQUE (Account_ID)
)
WITH EXEC AS CALLER
AS
BEGIN

DECLARE @TagsTable
   TABLE
   (
   listpos int IDENTITY(1, 1) NOT NULL,
   [str] varchar(800) NOT NULL,
   nstr nvarchar(800) NOT NULL,
   UNIQUE ([str], listpos)
   )
   
   INSERT INTO @TagsTable ([str], nstr) SELECT [str], nstr FROM dbo.fn_iter_charlist_to_tbl(@ParTagsName, '|');
   
DECLARE @ExcludeTagsTable
   TABLE
   (
   listpos int IDENTITY(1, 1) NOT NULL,
   [str] varchar(800) NOT NULL,
   nstr nvarchar(800) NOT NULL,
   UNIQUE ([str], listpos)
   )
   
   INSERT INTO @ExcludeTagsTable ([str], nstr) SELECT [str], nstr FROM dbo.fn_iter_charlist_to_tbl(@ParExcludeTagsName, '|');
   
IF @ParAccountNumbers IS NOT NULL AND @ParAccountNumbers != ''
   INSERT INTO @AccountsTable (Account_ID)
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
         JOIN dbo.fn_iter_charlist_to_tbl(@ParAccountNumbers, ' ') baid_list ON acc.AccountNumber = baid_list.str
ELSE IF @ParTagsName IS NULL
   INSERT INTO @AccountsTable (Account_ID) 
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
         LEFT JOIN dbo.AccountTypes at ON at.ID = acc.AccountType_ID
      WHERE
         (@ParAccountTypesName IS NULL OR at.Description = @ParAccountTypesName OR at.Name = @ParAccountTypesName)
      ORDER BY acc.ID
ELSE
   INSERT INTO @AccountsTable (Account_ID) 
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
         LEFT JOIN dbo.AccountTypes at ON at.ID = acc.AccountType_ID
         LEFT JOIN dbo.TagAccounts ta ON ta.Account_ID = acc.ID
         JOIN dbo.Tags t ON t.ID = ta.Tag_ID
         JOIN @TagsTable tt ON tt.nstr = t.Name
      WHERE
         (t.Discriminator = 'SystemTag' OR t.CreateUser = @ParUsername)
         AND (@ParAccountTypesName IS NULL OR at.Description = @ParAccountTypesName OR at.Name = @ParAccountTypesName)
      ORDER BY acc.ID

IF @ParExcludeTagsName IS NOT NULL
   DELETE FROM @AccountsTable
   WHERE Account_ID IN 
      (
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
         LEFT JOIN dbo.TagAccounts ta ON ta.Account_ID = acc.ID
         JOIN dbo.Tags t ON t.ID = ta.Tag_ID
         JOIN @ExcludeTagsTable tt ON tt.nstr = t.Name
      WHERE
         (t.Discriminator = 'SystemTag' OR t.CreateUser = @ParUsername)
      )

RETURN

END


GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_FilteredAccountIDListUser]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_get_FilteredAccountIDListUser]
(@ParAccountNumbers varchar(MAX), @ParAccountTypesName varchar(MAX), @ParTagsName varchar(MAX), @ParExcludeTagsName varchar(MAX), @ParUsername varchar(MAX), @ParAssignedUsername varchar(MAX))
RETURNS @AccountsTable table
(
[Account_ID] int,
   UNIQUE (Account_ID)
)
WITH EXEC AS CALLER
AS
BEGIN

DECLARE @TagsTable
   TABLE
   (
   listpos int IDENTITY(1, 1) NOT NULL,
   [str] varchar(800) NOT NULL,
   nstr nvarchar(800) NOT NULL,
   UNIQUE ([str], listpos)
   )
   
   INSERT INTO @TagsTable ([str], nstr) SELECT [str], nstr FROM dbo.fn_iter_charlist_to_tbl(@ParTagsName, '|');
   
DECLARE @ExcludeTagsTable
   TABLE
   (
   listpos int IDENTITY(1, 1) NOT NULL,
   [str] varchar(800) NOT NULL,
   nstr nvarchar(800) NOT NULL,
   UNIQUE ([str], listpos)
   )
   
   INSERT INTO @ExcludeTagsTable ([str], nstr) SELECT [str], nstr FROM dbo.fn_iter_charlist_to_tbl(@ParExcludeTagsName, '|');
   
IF @ParAccountNumbers IS NOT NULL AND @ParAccountNumbers != ''
   INSERT INTO @AccountsTable (Account_ID)
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
         JOIN dbo.fn_iter_charlist_to_tbl(@ParAccountNumbers, ' ') baid_list ON acc.AccountNumber = baid_list.str
ELSE IF @ParTagsName IS NULL
   INSERT INTO @AccountsTable (Account_ID) 
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
         LEFT JOIN dbo.AccountTypes at ON at.ID = acc.AccountType_ID
      WHERE
         (@ParAccountTypesName IS NULL OR at.Description = @ParAccountTypesName OR at.Name = @ParAccountTypesName)
      ORDER BY acc.ID
ELSE
   INSERT INTO @AccountsTable (Account_ID) 
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
         LEFT JOIN dbo.AccountTypes at ON at.ID = acc.AccountType_ID
         LEFT JOIN dbo.TagAccounts ta ON ta.Account_ID = acc.ID
         JOIN dbo.Tags t ON t.ID = ta.Tag_ID
         JOIN @TagsTable tt ON tt.nstr = t.Name
      WHERE
         (t.Discriminator = 'SystemTag' OR t.CreateUser = @ParUsername)
         AND (@ParAccountTypesName IS NULL OR at.Description = @ParAccountTypesName OR at.Name = @ParAccountTypesName)
      ORDER BY acc.ID

IF @ParExcludeTagsName IS NOT NULL
   DELETE FROM @AccountsTable
   WHERE Account_ID IN 
      (
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
         LEFT JOIN dbo.TagAccounts ta ON ta.Account_ID = acc.ID
         JOIN dbo.Tags t ON t.ID = ta.Tag_ID
         JOIN @ExcludeTagsTable tt ON tt.nstr = t.Name
      WHERE
         (t.Discriminator = 'SystemTag' OR t.CreateUser = @ParUsername)
      )

IF @ParAssignedUsername IS NOT NULL AND @ParAssignedUsername != ''
   DELETE FROM @AccountsTable
   WHERE Account_ID IN 
      (
      SELECT DISTINCT acc.ID
      FROM
         dbo.Accounts acc
      WHERE
          NOT ((ResponsiblePerson_ID IS NOT NULL AND ResponsiblePerson_ID = @ParAssignedUsername) 
            OR (KeyAccountManager_ID IS NOT NULL AND KeyAccountManager_ID = @ParAssignedUsername)
            OR (Salesperson_ID IS NOT NULL AND Salesperson_ID = @ParAssignedUsername))
      )

RETURN

END

GO
/****** Object:  UserDefinedFunction [dbo].[fn_getabc]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 16.05.2017
-- Description:	Liefert Schlüssel für Konto anhand Type und Kundennummer
-- =============================================
CREATE FUNCTION [dbo].[fn_getabc]
(
	@CNR varchar(12),
	@ATYPE varchar(3)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @AID int

	-- Add the T-SQL statements to compute the return value here
	SELECT @AID=ID FROM dbo.Accounts WHERE CustomerNumber=@CNR AND AccountType_ID=@ATYPE

	-- Return the result of the function
	RETURN @AID

END

GO
/****** Object:  UserDefinedFunction [dbo].[fn_iter_charlist_to_tbl]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_iter_charlist_to_tbl]
                 (@list      nvarchar(MAX),
                  @delimiter nchar(1) = N',')
      RETURNS @tbl TABLE (listpos int IDENTITY(1, 1) NOT NULL,
                          str     varchar(4000)      NOT NULL,
                          nstr    nvarchar(2000)     NOT NULL) AS

BEGIN
   DECLARE @endpos   int,
           @startpos int,
           @textpos  int,
           @chunklen smallint,
           @tmpstr   nvarchar(4000),
           @leftover nvarchar(4000),
           @tmpval   nvarchar(4000)

   SET @textpos = 1
   SET @leftover = ''
   WHILE @textpos <= datalength(@list) / 2
   BEGIN
      SET @chunklen = 4000 - datalength(@leftover) / 2
      SET @tmpstr = @leftover + substring(@list, @textpos, @chunklen)
      SET @textpos = @textpos + @chunklen

      SET @startpos = 0
      SET @endpos = charindex(@delimiter COLLATE Slovenian_BIN2, @tmpstr)

      WHILE @endpos > 0
      BEGIN
         SET @tmpval = ltrim(rtrim(substring(@tmpstr, @startpos + 1,
                                             @endpos - @startpos - 1)))
         INSERT @tbl (str, nstr) VALUES(@tmpval, @tmpval)
         SET @startpos = @endpos
         SET @endpos = charindex(@delimiter COLLATE Slovenian_BIN2,
                                 @tmpstr, @startpos + 1)
      END

      SET @leftover = right(@tmpstr, datalength(@tmpstr) / 2 - @startpos)
   END

   INSERT @tbl(str, nstr)
      VALUES (ltrim(rtrim(@leftover)), ltrim(rtrim(@leftover)))
   RETURN
END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_iter_intlist_to_tbl]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_iter_intlist_to_tbl] (@list nvarchar(MAX))
   RETURNS @tbl TABLE (listpos int IDENTITY(1, 1) NOT NULL,
                       number  int NOT NULL) AS
BEGIN
   DECLARE @startpos int,
           @endpos   int,
           @textpos  int,
           @chunklen smallint,
           @str      nvarchar(4000),
           @tmpstr   nvarchar(4000),
           @leftover nvarchar(4000)

   SET @textpos = 1
   SET @leftover = ''
   WHILE @textpos <= datalength(@list) / 2
   BEGIN
      SET @chunklen = 4000 - datalength(@leftover) / 2
      SET @tmpstr = ltrim(@leftover +
                    substring(@list, @textpos, @chunklen))
      SET @textpos = @textpos + @chunklen

      SET @startpos = 0
      SET @endpos = charindex(' ' COLLATE Slovenian_BIN2, @tmpstr)

      WHILE @endpos > 0
      BEGIN
         SET @str = substring(@tmpstr, @startpos + 1,
                              @endpos - @startpos - 1)
         IF @str <> ''
            INSERT @tbl (number) VALUES(convert(int, @str))
         SET @startpos = @endpos
         SET @endpos = charindex(' ' COLLATE Slovenian_BIN2,
                                 @tmpstr, @startpos + 1)
      END

      SET @leftover = right(@tmpstr, datalength(@tmpstr) / 2 - @startpos)
   END

   IF ltrim(rtrim(@leftover)) <> ''
      INSERT @tbl (number) VALUES(convert(int, @leftover))

   RETURN
END;
GO
/****** Object:  UserDefinedFunction [dbo].[udf_ConvertToFraction]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[udf_ConvertToFraction]
    (
      @NumToConvert DECIMAL(25, 10)
    )
RETURNS VARCHAR(75)
AS
    BEGIN
        DECLARE @output VARCHAR(75);
        DECLARE @wholenumber INT;
        DECLARE @DECIMAL DECIMAL(25, 10);
        DECLARE @num INT;
        DECLARE @denom INT;
        DECLARE @multiple INT;
        SET @wholenumber = CAST(@NumToConvert AS INT);
        SET @DECIMAL = @NumToConvert - @wholenumber;
        SET @multiple = CAST('1' + REPLICATE('0',
                                             LEN(CAST(CAST(REVERSE(SUBSTRING(CAST(@DECIMAL AS VARCHAR),
                                                              CHARINDEX('.',
                                                              CAST(@DECIMAL AS VARCHAR))
                                                              + 1,
                                                              LEN(CAST(@DECIMAL AS VARCHAR)))) AS INT) AS VARCHAR(10)))) AS INT);

        SET @num = @multiple * @DECIMAL;
        SET @denom = @multiple;
        IF @num > 0
            BEGIN

        --calculate the greatest common factor
  --AS long AS both numbers are even numbers, keep reducing them.

                WHILE (( @num % 2 ) + (@denom % 2 )) = 0
                    BEGIN
                        SET @denom = @denom / 2;       
                        SET @num = @num / 2;
                    END;

        --continue reducing numerator and denominator until one
     --is no longer evenly divisible by 5

                WHILE ((@num % 5) + (@denom % 5)) = 0
                    BEGIN
                        SET @denom = @denom / 5;
                        SET @num = @num / 5;
                    END;
                SET @output = CASE WHEN @wholenumber > 0
                                   THEN CONVERT(VARCHAR, @wholenumber)
                                   ELSE ''
                              END + ' ' + CONVERT(VARCHAR, @num) + '/'
                    + CONVERT(VARCHAR, @denom);
            END;

        ELSE
            BEGIN
       SET @num = @wholenumber;
    SET @denom = 1;
                SET @output = @wholenumber;
            END;
        RETURN (@output);
    END;


GO
/****** Object:  UserDefinedFunction [dbo].[udf_ConvertToFractionDenominator]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[udf_ConvertToFractionDenominator]
    (
      @NumToConvert DECIMAL(25, 10)
    )
RETURNS VARCHAR(75)
AS
    BEGIN
        DECLARE @output VARCHAR(75);
        DECLARE @wholenumber INT;
        DECLARE @DECIMAL DECIMAL(25, 10);
        DECLARE @num INT;
        DECLARE @denom INT;
        DECLARE @multiple INT;
        SET @wholenumber = CAST(@NumToConvert AS INT);
        SET @DECIMAL = @NumToConvert - @wholenumber;
        SET @multiple = CAST('1' + REPLICATE('0',
                                             LEN(CAST(CAST(REVERSE(SUBSTRING(CAST(@DECIMAL AS VARCHAR),
                                                              CHARINDEX('.',
                                                              CAST(@DECIMAL AS VARCHAR))
                                                              + 1,
                                                              LEN(CAST(@DECIMAL AS VARCHAR)))) AS INT) AS VARCHAR(10)))) AS INT);

        SET @num = @multiple * @DECIMAL;
        SET @denom = @multiple;
        IF @num > 0
            BEGIN

        --calculate the greatest common factor
  --AS long AS both numbers are even numbers, keep reducing them.

                WHILE (( @num % 2 ) + (@denom % 2 )) = 0
                    BEGIN
                        SET @denom = @denom / 2;       
                        SET @num = @num / 2;
                    END;

        --continue reducing numerator and denominator until one
     --is no longer evenly divisible by 5

                WHILE ((@num % 5) + (@denom % 5)) = 0
                    BEGIN
                        SET @denom = @denom / 5;
                        SET @num = @num / 5;
                    END;
                SET @output = CASE WHEN @wholenumber > 0
                                   THEN CONVERT(VARCHAR, @wholenumber)
                                   ELSE ''
                              END + ' ' + CONVERT(VARCHAR, @num) + '/'
                    + CONVERT(VARCHAR, @denom);
            END;

        ELSE
            BEGIN
       SET @num = @wholenumber;
    SET @denom = 1;
                SET @output = @wholenumber;
            END;
        RETURN (@denom);
    END;


GO
/****** Object:  UserDefinedFunction [dbo].[udf_ConvertToFractionNumerator]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[udf_ConvertToFractionNumerator]
    (
      @NumToConvert DECIMAL(25, 10)
    )
RETURNS VARCHAR(75)
AS
    BEGIN
        DECLARE @output VARCHAR(75);
        DECLARE @wholenumber INT;
        DECLARE @DECIMAL DECIMAL(25, 10);
        DECLARE @num INT;
        DECLARE @denom INT;
        DECLARE @multiple INT;
        SET @wholenumber = CAST(@NumToConvert AS INT);
        SET @DECIMAL = @NumToConvert - @wholenumber;
        SET @multiple = CAST('1' + REPLICATE('0',
                                             LEN(CAST(CAST(REVERSE(SUBSTRING(CAST(@DECIMAL AS VARCHAR),
                                                              CHARINDEX('.',
                                                              CAST(@DECIMAL AS VARCHAR))
                                                              + 1,
                                                              LEN(CAST(@DECIMAL AS VARCHAR)))) AS INT) AS VARCHAR(10)))) AS INT);

        SET @num = @multiple * @DECIMAL;
        SET @denom = @multiple;
        IF @num > 0
            BEGIN

        --calculate the greatest common factor
  --AS long AS both numbers are even numbers, keep reducing them.

                WHILE (( @num % 2 ) + (@denom % 2 )) = 0
                    BEGIN
                        SET @denom = @denom / 2;       
                        SET @num = @num / 2;
                    END;

        --continue reducing numerator and denominator until one
     --is no longer evenly divisible by 5

                WHILE ((@num % 5) + (@denom % 5)) = 0
                    BEGIN
                        SET @denom = @denom / 5;
                        SET @num = @num / 5;
                    END;
                SET @output = CASE WHEN @wholenumber > 0
                                   THEN CONVERT(VARCHAR, @wholenumber)
                                   ELSE ''
                              END + ' ' + CONVERT(VARCHAR, @num) + '/'
                    + CONVERT(VARCHAR, @denom);
            END;

        ELSE
            BEGIN
       SET @num = @wholenumber;
    SET @denom = 1;
                SET @output = @wholenumber;
            END;
  IF (@wholenumber >0 AND @denom != 1)
  BEGIN
   SET @num = @wholenumber * @denom + @num
  END;

        RETURN (@Num);
    END;


GO
/****** Object:  Table [dbo].[Terms]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Terms](
	[ID] [varchar](6) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[InvoiceDescription] [nvarchar](50) NOT NULL,
	[Unit] [nvarchar](8) NOT NULL,
	[GuiOrder] [int] NOT NULL,
	[InvoiceOrder] [int] NOT NULL,
	[InvisibleIf0] [bit] NOT NULL,
	[IsDecimal] [bit] NOT NULL,
	[DecimalPlaces] [int] NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[FeeTitle] [nvarchar](30) NULL,
	[ExtFeeTitle] [nvarchar](50) NULL,
	[BookingType_ID] [varchar](6) NULL,
	[KeySignature] [smallint] NULL,
	[IsQuantityDependent] [bit] NULL,
	[CanBeManuallyAdded] [bit] NULL,
	[MatchAllBookingTypes] [bit] NULL,
	[LowerLimit] [int] NULL,
	[UpperLimit] [int] NULL,
	[Interval] [int] NULL,
	[IntervalOption] [nvarchar](20) NULL,
	[ChargeOption] [nvarchar](20) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	[ChargeWith_ID] [varchar](6) NULL,
	[SwapAccount_ID] [int] NULL,
	[SwapRoundingType] [int] NULL,
	[SwapInBookingType_ID] [varchar](6) NULL,
	[SwapInPallet_ID] [smallint] NULL,
	[SwapOutBookingType_ID] [varchar](6) NULL,
	[SwapOutPallet_ID] [smallint] NULL,
 CONSTRAINT [PK_dbo.Terms] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Qualities]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Qualities](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[Order] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Qualities] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Articles]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [varchar](255) NOT NULL,
	[Order] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Articles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bookings]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bookings](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[BookingType_ID] [varchar](6) NOT NULL,
	[ReferenceNumber] [varchar](50) NULL,
	[BookingDate] [date] NOT NULL,
	[ExtDescription] [varchar](255) NULL,
	[Quantity] [int] NULL,
	[Article_ID] [smallint] NOT NULL,
	[Quality_ID] [smallint] NOT NULL,
	[IncludeInBalance] [bit] NOT NULL,
	[Matched] [bit] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[Transaction_ID] [int] NOT NULL,
	[Account_ID] [int] NOT NULL,
	[MatchedUser] [varchar](50) NULL,
	[MatchedTime] [date] NULL,
	[AccountDirection] [char](1) NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[ChangedTime]  AS (coalesce([UpdateTime],[CreateTime])),
	[RowModified] [datetime] NOT NULL,
	[Computed] [bit] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Bookings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Processes]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Processes](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ReferenceNumber] [varchar](50) NULL,
	[Name] [varchar](50) NOT NULL,
	[ProcessState_ID] [smallint] NOT NULL,
	[ProcessType_ID] [smallint] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[IntDescription] [varchar](255) NULL,
	[ExtDescription] [varchar](255) NULL,
	[ChangedTime]  AS (coalesce([UpdateTime],[CreateTime])),
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Processes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessStates]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessStates](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.ProcessStates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessTypes]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessTypes](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.ProcessTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionDetails]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionDetails](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Transaction_ID] [int] NOT NULL,
	[FieldName] [varchar](25) NOT NULL,
	[FieldValue] [varchar](500) NULL,
	[OptimisticLockField] [int] NOT NULL,
	[FieldDate] [datetime] NULL,
	[FieldInt] [int] NULL,
	[FieldDecimal] [smallmoney] NULL,
	[FieldBool] [bit] NULL,
	[FieldGuid] [uniqueidentifier] NULL,
	[FieldText] [nvarchar](max) NULL,
	[FieldType] [varchar](255) NULL,
 CONSTRAINT [PK_dbo.TransactionDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[TransactionType_ID] [varchar](6) NOT NULL,
	[ReferenceNumber] [varchar](100) NULL,
	[Valuta] [date] NOT NULL,
	[IntDescription] [varchar](255) NULL,
	[ExtDescription] [varchar](255) NULL,
	[Process_ID] [int] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[TransactionState_ID] [smallint] NOT NULL,
	[Cancellation_ID] [int] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[ChangedTime]  AS (coalesce([UpdateTime],[CreateTime])),
 CONSTRAINT [PK_dbo.Transactions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionStates]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionStates](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [varchar](255) NOT NULL,
	[Cancelable] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.TransactionStates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionTypes]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionTypes](
	[ID] [varchar](6) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.TransactionTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Condition].[Fees]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Condition].[Fees](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Amount] [money] NOT NULL,
	[InvoiceNumber] [nvarchar](65) NULL,
	[Term_ID] [varchar](6) NOT NULL,
	[Condition_ID] [int] NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[Account_ID] [int] NULL,
	[Booking_ID] [int] NULL,
	[FeeType] [tinyint] NOT NULL,
 CONSTRAINT [PK_Condition.Fees] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Parent_ID] [int] NULL,
	[AccountType_ID] [varchar](3) NOT NULL,
	[AccountNumber] [varchar](13) NULL,
	[CustomerNumber] [varchar](12) NULL,
	[AddressId] [int] NULL,
	[Description] [varchar](2000) NULL,
	[PathByName] [nvarchar](1500) NULL,
	[OptimisticLockField] [int] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[FullName] [nvarchar](128) NULL,
	[PathName] [nvarchar](128) NOT NULL,
	[Inactive] [bit] NOT NULL,
	[Locked] [bit] NOT NULL,
	[ResponsiblePerson_ID] [varchar](50) NULL,
	[InvoiceAccount_ID] [int] NULL,
	[KeyAccountManager_ID] [varchar](50) NULL,
	[Salesperson_ID] [varchar](50) NULL,
 CONSTRAINT [PK_dbo.Accounts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountTypes]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountTypes](
	[ID] [varchar](3) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.AccountTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingTypes]    Script Date: 29/01/2020 11:39:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingTypes](
	[ID] [varchar](6) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Order] [int] NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[LtmsUserTask] [varchar](5) NULL,
	[OneDayDelayForRent] [bit] NOT NULL,
	[IncludeDaysFreeRentCondition] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.BookingTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vBookingsForGridFees]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vBookingsForGridFees]
AS
SELECT        dbo.Bookings.ID, dbo.Bookings.BookingType_ID, dbo.BookingTypes.Name AS BookingTypesName, dbo.Bookings.ReferenceNumber, dbo.Bookings.BookingDate, 
                         dbo.Bookings.ExtDescription, Condition.Fees.Amount, Condition.Fees.ID AS Fee_ID, dbo.Terms.Name AS TermsName, dbo.Terms.ID AS Terms_ID, 
                         dbo.Terms.IsQuantityDependent AS TermsIsQuantityDependent, dbo.Bookings.IncludeInBalance, dbo.Bookings.Matched, dbo.Bookings.MatchedUser, 
                         dbo.Bookings.MatchedTime, dbo.Bookings.CreateUser, dbo.Bookings.CreateTime, dbo.Bookings.UpdateUser, dbo.Bookings.UpdateTime, dbo.Bookings.DeleteUser, 
                         dbo.Bookings.DeleteTime, dbo.Bookings.Account_ID, dbo.Bookings.Transaction_ID, dbo.Transactions.Process_ID, dbo.Bookings.Quantity, dbo.Bookings.Article_ID, 
                         dbo.Articles.Name AS ArticlesName, dbo.Articles.Description AS ArticlesDescription, dbo.Bookings.Quality_ID, dbo.Qualities.Name AS QualitiesName, 
                         dbo.Qualities.Description AS QualitiesDescription, dbo.Accounts.FullName, dbo.Accounts.AccountType_ID, dbo.AccountTypes.Name AS AccountTypesName, 
                         dbo.Accounts.AccountNumber, dbo.Accounts.CustomerNumber, dbo.Accounts.AddressId, dbo.Accounts.Description, 
                         dbo.Transactions.ReferenceNumber AS TransactionsReferencenumber, dbo.Transactions.Valuta AS TransactionsValuta, 
                         dbo.Transactions.IntDescription AS TransactionsIntDescription, dbo.Transactions.ExtDescription AS TransactionsExtDescription, 
                         dbo.Transactions.TransactionType_ID, dbo.TransactionTypes.Name AS TransactionTypesName, dbo.TransactionTypes.Description AS TransactionTypesDescription, 
                         dbo.Transactions.TransactionState_ID, dbo.TransactionStates.Name AS TransactionStatesName, dbo.TransactionStates.Description AS TransactionStatesDescription,
                          dbo.Transactions.Cancellation_ID AS TransactionCancellation_ID, dbo.Processes.Name AS ProcessesName, 
                         dbo.Processes.ReferenceNumber AS ProcessesReferencenumber, dbo.Processes.ProcessType_ID, dbo.Processes.IntDescription AS ProcessesIntDescription, 
                         dbo.Processes.ExtDescription AS ProcessesExtDescription, dbo.ProcessTypes.Name AS ProcessTypesName, 
                         dbo.ProcessTypes.Description AS ProcessTypesDescription, dbo.Processes.ProcessState_ID, dbo.ProcessStates.Name AS ProcessStatesName, 
                         dbo.ProcessStates.Description AS ProcessStatesDescription, TDdpl_ls.FieldValue AS dpl_ls_nr, TDdpl_ls.FieldDate AS dpl_ls_datum, 
                         Condition.Fees.InvoiceNumber AS dpl_re_nr, NULL AS dpl_re_datum, TDdpl_au.FieldValue AS dpl_au_nr, TDdpl_au.FieldDate AS dpl_au_datum, 
                         TDdpl_ab.FieldValue AS dpl_ab_nr, TDdpl_ab.FieldDate AS dpl_ab_datum, TDdpl_we.FieldValue AS dpl_we_nr, TDdpl_we.FieldDate AS dpl_we_datum, 
                         TDdpl_fa.FieldValue AS dpl_fa_nr, TDdpl_fa.FieldDate AS dpl_fa_datum, TDdpl_gu.FieldValue AS dpl_gu_nr, TDdpl_gu.FieldDate AS dpl_gu_datum, 
                         TDdpl_va.FieldValue AS dpl_va_nr, TDdpl_va.FieldDate AS dpl_va_datum, TDdpl_opg.FieldValue AS dpl_opg_nr, TDdpl_opg.FieldDate AS dpl_opg_datum, 
                         TDdpl_LTV.FieldValue AS dpl_LTV_nr, TDdpl_LTV.FieldDate AS dpl_LTV_buchungsdatum, TDext_ls.FieldValue AS ext_ls_nr, TDext_ls.FieldDate AS ext_ls_datum, 
                         TDext_re.FieldValue AS ext_re_nr, TDext_re.FieldDate AS ext_re_datum, TDext_db.FieldValue AS dpl_depot_beleg_nr, 
                         TDext_db.FieldDate AS dpl_depot_beleg_datum, TDext_snst.FieldValue AS ext_snst_nr, TDext_snst.FieldDate AS ext_snst_datum, 
                         TDext_abholer.FieldValue AS ext_abholer, TDext_anlieferer.FieldValue AS ext_anlieferer, TDext_kfz_kz.FieldValue AS ext_kfz_kz
FROM            dbo.Accounts LEFT OUTER JOIN
                         dbo.AccountTypes ON dbo.AccountTypes.ID = dbo.Accounts.AccountType_ID RIGHT OUTER JOIN
                         dbo.Bookings ON dbo.Accounts.ID = dbo.Bookings.Account_ID LEFT OUTER JOIN
                         Condition.Fees ON Condition.Fees.Booking_ID = dbo.Bookings.ID LEFT OUTER JOIN
                         dbo.Terms ON dbo.Terms.ID = Condition.Fees.Term_ID LEFT OUTER JOIN
                         dbo.BookingTypes ON dbo.BookingTypes.ID = dbo.Bookings.BookingType_ID LEFT OUTER JOIN
                         dbo.Articles ON dbo.Articles.ID = dbo.Bookings.Article_ID LEFT OUTER JOIN
                         dbo.Qualities ON dbo.Qualities.ID = dbo.Bookings.Quality_ID LEFT OUTER JOIN
                         dbo.Transactions ON dbo.Bookings.Transaction_ID = dbo.Transactions.ID LEFT OUTER JOIN
                         dbo.TransactionTypes ON dbo.Transactions.TransactionType_ID = dbo.TransactionTypes.ID LEFT OUTER JOIN
                         dbo.TransactionStates ON dbo.Transactions.TransactionState_ID = dbo.TransactionStates.ID LEFT OUTER JOIN
                         dbo.Processes ON dbo.Transactions.Process_ID = dbo.Processes.ID LEFT OUTER JOIN
                         dbo.ProcessTypes ON dbo.Processes.ProcessType_ID = dbo.ProcessTypes.ID LEFT OUTER JOIN
                         dbo.ProcessStates ON dbo.ProcessStates.ID = dbo.Processes.ProcessState_ID LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_ls ON TDdpl_ls.Transaction_ID = dbo.Transactions.ID AND TDdpl_ls.FieldName = 'LS' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_au ON TDdpl_au.Transaction_ID = dbo.Transactions.ID AND TDdpl_au.FieldName = 'AU' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_ab ON TDdpl_ab.Transaction_ID = dbo.Transactions.ID AND TDdpl_ab.FieldName = 'AB' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_we ON TDdpl_we.Transaction_ID = dbo.Transactions.ID AND TDdpl_we.FieldName = 'WE' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_fa ON TDdpl_fa.Transaction_ID = dbo.Transactions.ID AND TDdpl_fa.FieldName = 'FA' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_gu ON TDdpl_gu.Transaction_ID = dbo.Transactions.ID AND TDdpl_gu.FieldName = 'GU' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_va ON TDdpl_va.Transaction_ID = dbo.Transactions.ID AND TDdpl_va.FieldName = 'VA' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_opg ON TDdpl_opg.Transaction_ID = dbo.Transactions.ID AND TDdpl_opg.FieldName = 'DPG' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_LTV ON TDdpl_LTV.Transaction_ID = dbo.Transactions.ID AND TDdpl_LTV.FieldName = 'LTV' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_ls ON TDext_ls.Transaction_ID = dbo.Transactions.ID AND TDext_ls.FieldName = 'EXTLS' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_re ON TDext_re.Transaction_ID = dbo.Transactions.ID AND TDext_re.FieldName = 'EXTRE' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_db ON TDext_db.Transaction_ID = dbo.Transactions.ID AND TDext_db.FieldName = 'EXTDB' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_snst ON TDext_snst.Transaction_ID = dbo.Transactions.ID AND TDext_snst.FieldName = 'SONST' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_abholer ON TDext_abholer.Transaction_ID = dbo.Transactions.ID AND TDext_abholer.FieldName = 'ABHOLER' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_anlieferer ON TDext_anlieferer.Transaction_ID = dbo.Transactions.ID AND 
                         TDext_anlieferer.FieldName = 'ANLIEFERER' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_kfz_kz ON TDext_kfz_kz.Transaction_ID = dbo.Transactions.ID AND TDext_kfz_kz.FieldName = 'KFZ'
WHERE
   Condition.Fees.DeleteUser IS NULL

GO
/****** Object:  View [dbo].[vBookingsSimple]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vBookingsSimple]
AS
SELECT        dbo.Bookings.ID, dbo.Bookings.BookingType_ID, dbo.BookingTypes.Name AS BookingTypesName, dbo.Bookings.ReferenceNumber, dbo.Bookings.BookingDate, 
                         dbo.Bookings.ExtDescription,
                             (SELECT        SUM(Amount) AS Amount
                               FROM            Condition.Fees
                               WHERE        (Booking_ID = dbo.Bookings.ID) AND (Condition.Fees.DeleteUser IS NULL)) AS Amount, dbo.Bookings.IncludeInBalance, dbo.Bookings.Matched, dbo.Bookings.MatchedUser, 
                         dbo.Bookings.MatchedTime, dbo.Bookings.CreateUser, dbo.Bookings.CreateTime, dbo.Bookings.UpdateUser, dbo.Bookings.UpdateTime, dbo.Bookings.DeleteUser, 
                         dbo.Bookings.DeleteTime, dbo.Bookings.Account_ID, dbo.Bookings.Transaction_ID, dbo.Transactions.Process_ID, dbo.Bookings.Quantity, dbo.Bookings.Article_ID, 
                         dbo.Articles.Name AS ArticlesName, dbo.Articles.Description AS ArticlesDescription, dbo.Bookings.Quality_ID, dbo.Qualities.Name AS QualitiesName, 
                         dbo.Qualities.Description AS QualitiesDescription, dbo.Accounts.FullName, dbo.Accounts.AccountType_ID, dbo.AccountTypes.Name AS AccountTypesName, 
                         dbo.Accounts.AccountNumber, dbo.Accounts.CustomerNumber, dbo.Accounts.AddressId, dbo.Accounts.Description, 
                         dbo.Transactions.ReferenceNumber AS TransactionsReferencenumber, dbo.Transactions.Valuta AS TransactionsValuta, 
                         dbo.Transactions.IntDescription AS TransactionsIntDescription, dbo.Transactions.ExtDescription AS TransactionsExtDescription, 
                         dbo.Transactions.TransactionType_ID, dbo.TransactionTypes.Name AS TransactionTypesName, dbo.TransactionTypes.Description AS TransactionTypesDescription, 
                         dbo.Transactions.TransactionState_ID, dbo.TransactionStates.Name AS TransactionStatesName, dbo.TransactionStates.Description AS TransactionStatesDescription,
                          dbo.Processes.Name AS ProcessesName, dbo.Processes.ReferenceNumber AS ProcessesReferencenumber, dbo.Processes.ProcessType_ID, 
                         dbo.Processes.IntDescription AS ProcessesIntDescription, dbo.Processes.ExtDescription AS ProcessesExtDescription, 
                         dbo.ProcessTypes.Name AS ProcessTypesName, dbo.ProcessTypes.Description AS ProcessTypesDescription, dbo.Processes.ProcessState_ID, 
                         dbo.ProcessStates.Name AS ProcessStatesName, dbo.ProcessStates.Description AS ProcessStatesDescription
FROM            dbo.Accounts LEFT OUTER JOIN
                         dbo.AccountTypes ON dbo.AccountTypes.ID = dbo.Accounts.AccountType_ID RIGHT OUTER JOIN
                         dbo.Bookings ON dbo.Accounts.ID = dbo.Bookings.Account_ID LEFT OUTER JOIN
                         dbo.BookingTypes ON dbo.BookingTypes.ID = dbo.Bookings.BookingType_ID LEFT OUTER JOIN
                         dbo.Articles ON dbo.Articles.ID = dbo.Bookings.Article_ID LEFT OUTER JOIN
                         dbo.Qualities ON dbo.Qualities.ID = dbo.Bookings.Quality_ID LEFT OUTER JOIN
                         dbo.Transactions ON dbo.Bookings.Transaction_ID = dbo.Transactions.ID LEFT OUTER JOIN
                         dbo.TransactionTypes ON dbo.Transactions.TransactionType_ID = dbo.TransactionTypes.ID LEFT OUTER JOIN
                         dbo.TransactionStates ON dbo.Transactions.TransactionState_ID = dbo.TransactionStates.ID LEFT OUTER JOIN
                         dbo.Processes ON dbo.Transactions.Process_ID = dbo.Processes.ID LEFT OUTER JOIN
                         dbo.ProcessTypes ON dbo.Processes.ProcessType_ID = dbo.ProcessTypes.ID LEFT OUTER JOIN
                         dbo.ProcessStates ON dbo.ProcessStates.ID = dbo.Processes.ProcessState_ID

GO
/****** Object:  View [dbo].[vBookingsSimpleFees]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vBookingsSimpleFees]
AS
SELECT        dbo.Bookings.ID, dbo.Bookings.BookingType_ID, dbo.BookingTypes.Name AS BookingTypesName, dbo.Bookings.ReferenceNumber, dbo.Bookings.BookingDate, 
                         dbo.Bookings.ExtDescription,
                         Condition.Fees.Amount AS Amount, Condition.Fees.ID AS Fee_ID, dbo.Bookings.IncludeInBalance, dbo.Bookings.Matched, dbo.Bookings.MatchedUser, 
                         dbo.Bookings.MatchedTime, dbo.Bookings.CreateUser, dbo.Bookings.CreateTime, dbo.Bookings.UpdateUser, dbo.Bookings.UpdateTime, dbo.Bookings.DeleteUser, 
                         dbo.Bookings.DeleteTime, dbo.Bookings.Account_ID, dbo.Bookings.Transaction_ID, dbo.Transactions.Process_ID, dbo.Bookings.Quantity, dbo.Bookings.Article_ID, 
                         dbo.Articles.Name AS ArticlesName, dbo.Articles.Description AS ArticlesDescription, dbo.Bookings.Quality_ID, dbo.Qualities.Name AS QualitiesName, 
                         dbo.Qualities.Description AS QualitiesDescription, dbo.Accounts.FullName, dbo.Accounts.AccountType_ID, dbo.AccountTypes.Name AS AccountTypesName, 
                         dbo.Accounts.AccountNumber, dbo.Accounts.CustomerNumber, dbo.Accounts.AddressId, dbo.Accounts.Description, 
                         dbo.Transactions.ReferenceNumber AS TransactionsReferencenumber, dbo.Transactions.Valuta AS TransactionsValuta, 
                         dbo.Transactions.IntDescription AS TransactionsIntDescription, dbo.Transactions.ExtDescription AS TransactionsExtDescription, 
                         dbo.Transactions.TransactionType_ID, dbo.TransactionTypes.Name AS TransactionTypesName, dbo.TransactionTypes.Description AS TransactionTypesDescription, 
                         dbo.Transactions.TransactionState_ID, dbo.TransactionStates.Name AS TransactionStatesName, dbo.TransactionStates.Description AS TransactionStatesDescription,
                          dbo.Processes.Name AS ProcessesName, dbo.Processes.ReferenceNumber AS ProcessesReferencenumber, dbo.Processes.ProcessType_ID, 
                         dbo.Processes.IntDescription AS ProcessesIntDescription, dbo.Processes.ExtDescription AS ProcessesExtDescription, 
                         dbo.ProcessTypes.Name AS ProcessTypesName, dbo.ProcessTypes.Description AS ProcessTypesDescription, dbo.Processes.ProcessState_ID, 
                         dbo.ProcessStates.Name AS ProcessStatesName, dbo.ProcessStates.Description AS ProcessStatesDescription,
                         Condition.Fees.InvoiceNumber AS dpl_re_nr
FROM            dbo.Accounts LEFT OUTER JOIN
                         dbo.AccountTypes ON dbo.AccountTypes.ID = dbo.Accounts.AccountType_ID RIGHT OUTER JOIN
                         dbo.Bookings ON dbo.Accounts.ID = dbo.Bookings.Account_ID LEFT OUTER JOIN
                         Condition.Fees ON Condition.Fees.Booking_ID = dbo.Bookings.ID LEFT OUTER JOIN
                         dbo.BookingTypes ON dbo.BookingTypes.ID = dbo.Bookings.BookingType_ID LEFT OUTER JOIN
                         dbo.Articles ON dbo.Articles.ID = dbo.Bookings.Article_ID LEFT OUTER JOIN
                         dbo.Qualities ON dbo.Qualities.ID = dbo.Bookings.Quality_ID LEFT OUTER JOIN
                         dbo.Transactions ON dbo.Bookings.Transaction_ID = dbo.Transactions.ID LEFT OUTER JOIN
                         dbo.TransactionTypes ON dbo.Transactions.TransactionType_ID = dbo.TransactionTypes.ID LEFT OUTER JOIN
                         dbo.TransactionStates ON dbo.Transactions.TransactionState_ID = dbo.TransactionStates.ID LEFT OUTER JOIN
                         dbo.Processes ON dbo.Transactions.Process_ID = dbo.Processes.ID LEFT OUTER JOIN
                         dbo.ProcessTypes ON dbo.Processes.ProcessType_ID = dbo.ProcessTypes.ID LEFT OUTER JOIN
                         dbo.ProcessStates ON dbo.ProcessStates.ID = dbo.Processes.ProcessState_ID
WHERE
   Condition.Fees.DeleteUser IS NULL

GO
/****** Object:  Table [Condition].[BookingDependent]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Condition].[BookingDependent](
	[ID] [int] NOT NULL,
	[Quantity] [int] NULL,
	[Amount] [smallmoney] NULL,
	[Article_ID] [smallint] NOT NULL,
	[Quality_ID] [smallint] NULL,
 CONSTRAINT [PK_Condition.BookingDependent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conditions]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conditions](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ValidFrom] [date] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[Account_ID] [int] NOT NULL,
	[Term_ID] [varchar](6) NOT NULL,
	[Note] [nvarchar](255) NULL,
	[ValidUntil] [date] NULL,
	[MatchingOrder] [smallint] NULL,
	[InvoicingDay] [int] NULL,
	[Discriminator] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.Conditions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vConditions]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vConditions]
AS
    SELECT  dbo.Conditions.ID ,
            dbo.Conditions.ValidFrom ,
            dbo.Conditions.CreateUser ,
            dbo.Conditions.CreateTime ,
            dbo.Conditions.UpdateUser ,
            dbo.Conditions.UpdateTime ,
            dbo.Conditions.Account_ID ,
            dbo.Conditions.Term_ID ,
            dbo.Conditions.Note ,
            dbo.Conditions.ValidUntil ,
            dbo.Conditions.MatchingOrder ,
            dbo.Conditions.InvoicingDay ,
            dbo.Conditions.Discriminator ,
            Condition.BookingDependent.Quantity ,
            Condition.BookingDependent.Amount ,
            Condition.BookingDependent.Article_ID ,
            Condition.BookingDependent.Quality_ID
    FROM    Condition.BookingDependent
            INNER JOIN dbo.Conditions ON Condition.BookingDependent.ID = dbo.Conditions.ID;
GO
/****** Object:  Table [dbo].[Pallet]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pallet](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[Article_ID] [smallint] NOT NULL,
	[Quality_ID] [smallint] NOT NULL,
	[Order] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Pallet] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [LTMS].[vBookingsFlat]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [LTMS].[vBookingsFlat]

AS
    SELECT  P.ID AS PID ,
            T.ID AS TID ,
            B.ID AS BID ,
            LT.ID AS LID ,
            B.Account_ID AS AID ,
            P.ReferenceNumber AS P_REF ,
            T.ReferenceNumber AS T_REF ,
            B.ReferenceNumber AS B_REF ,
            P.Name AS P_NAME ,
            P.ProcessState_ID AS P_STATE ,
            T.TransactionState_ID AS T_STATE ,
            P.ProcessType_ID AS P_TYPE ,
            T.TransactionType_ID AS T_TYPE ,
            B.BookingType_ID AS B_TYPE ,
            P.IntDescription AS P_INT_TEXT ,
            P.ExtDescription AS P_EXT_TEXT ,
            A.AccountNumber AS A_NUMBER ,
            A.AccountType_ID AS A_TYPE ,
            A.Name AS A_NAME ,
            A.CustomerNumber ,
            T.RowGuid ,
            T.Valuta ,
            T.IntDescription ,
            T.Cancellation_ID ,
            B.BookingDate ,
            B.ExtDescription ,
            LT.Name AS LT ,
            B.Quantity ,
            B.IncludeInBalance ,
            B.Matched ,
            B.MatchedUser ,
            B.MatchedTime ,
            B.AccountDirection ,
            B.RowModified ,
            P.CreateUser AS P_CreateUser ,
            P.CreateTime AS P_CreateTime ,
            T.CreateUser AS T_CreateUser ,
            T.CreateTime AS T_CreateTime ,
            B.CreateUser AS B_CreateUser ,
            B.CreateTime AS B_CreateTime ,
            P.UpdateUser AS P_UpdateUser ,
            P.UpdateTime AS P_UpdateTime ,
            T.UpdateUser AS T_UpdateUser ,
            T.UpdateTime AS T_UpdateTime ,
            B.UpdateUser AS B_UpdateUser ,
            B.UpdateTime AS B_UpdateTime ,
            P.ChangedTime AS P_ChangedTime ,
            T.ChangedTime AS T_ChangedTime ,
            B.ChangedTime AS B_ChangedTime ,
            P.OptimisticLockField AS P_OptimisticLock ,
            T.OptimisticLockField AS T_OptimisticLock ,
            B.OptimisticLockField AS B_OptimisticLock
    FROM    dbo.Processes AS P
            INNER JOIN dbo.Transactions AS T ON T.Process_ID = P.ID
            INNER JOIN dbo.Bookings AS B ON B.Transaction_ID = T.ID
                                            AND P.DeleteUser IS NULL
                                            AND P.DeleteTime IS NULL
                                            AND T.DeleteUser IS NULL
                                            AND T.DeleteTime IS NULL
                                            AND B.DeleteUser IS NULL
                                            AND B.DeleteTime IS NULL
            INNER JOIN dbo.Pallet AS LT ON LT.Article_ID = B.Article_ID
                                           AND LT.Quality_ID = B.Quality_ID
            INNER JOIN dbo.Accounts AS A ON B.Account_ID = A.ID;


GO
/****** Object:  UserDefinedFunction [LTMS].[fn_query_accounts_import_to_ltms]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 10.04.2014
-- Description:	Liefert Die KontoId aller LAMA Konten die zum Senden an den LTMS-Server gekennzeichnet sind
-- =============================================
CREATE FUNCTION [LTMS].[fn_query_accounts_import_to_ltms] ( )
RETURNS TABLE
AS
RETURN
    --( SELECT    Subordinate_ID AS ID
    --  FROM      dbo.AccountDetails AS AD
    --            JOIN [dbo].[fn_accounts_descendant_hid]() AS D ON AD.Account_ID = D.ID
    --                                                              AND FieldName = 'LtmsImportService'
    --            JOIN dbo.AccountHierarchy ON D.ID = dbo.AccountHierarchy.ID
    --                                         AND FieldBool = 1

	(SELECT ID FROM dbo.Accounts WHERE AccountType_ID='HZL'
    )
GO
/****** Object:  UserDefinedFunction [LTMS].[fn_syncmap]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [LTMS].[fn_syncmap] ( )
RETURNS TABLE
AS
RETURN
    (
	-- Add the SELECT statement with parameter references here
      SELECT    P.ID PID ,
                T.ID TID ,
                B.ID BID ,
                B.IncludeInBalance
      FROM      LtmsDbFait.dbo.Accounts AS A
                INNER JOIN LTMS.fn_query_accounts_import_to_ltms() AS ltmsaccounts ON A.ID = ltmsaccounts.ID
                INNER JOIN dbo.Bookings AS B ON B.Account_ID = A.ID
                                                AND B.DeleteTime IS NULL
                JOIN dbo.BookingTypes BT ON BT.ID = B.BookingType_ID
                                            AND BT.LtmsUserTask IS NOT NULL
                JOIN dbo.Transactions AS T ON T.ID = B.Transaction_ID
                                              AND T.DeleteTime IS NULL
                                              AND ( T.Cancellation_ID IS NULL
                                                    OR T.TransactionState_ID NOT IN ( 3, 4 )
                                                  )
                INNER JOIN dbo.Processes AS P ON P.ID = T.Process_ID
                                                 AND P.DeleteTime IS NULL
                INNER JOIN dbo.Pallet AS LT ON B.Article_ID = LT.Article_ID
                                               AND B.Quality_ID = LT.Quality_ID
    );
GO
/****** Object:  View [LTMS].[SyncMap]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [LTMS].[SyncMap]
AS
SELECT        fn_syncmap_1.*
FROM            LTMS.fn_syncmap() AS fn_syncmap_1
GO
/****** Object:  View [dbo].[vBookingsPivot]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vBookingsPivot]
   AS
      (
      SELECT
            [ID]
            , BookingType_ID
            , ReferenceNumber
            , BookingDate
            , ExtDescription
            , IncludeInBalance
            , Matched
            , MatchedUser
            , MatchedTime
            , CreateUser
            , CreateTime
            , UpdateUser
            , UpdateTime
            , DeleteUser
            , DeleteTime
            , Account_ID
            , Transaction_ID
            , ISNULL([DD_1A], 0) AS [DD_1A], ISNULL([DD_2A], 0) AS [DD_2A], ISNULL([DD_2APlus], 0) AS [DD_2APlus], ISNULL([DD_2B], 0) AS [DD_2B], ISNULL([DD_2BG], 0) AS [DD_2BG], ISNULL([DD_2BMT], 0) AS [DD_2BMT], ISNULL([DD_2BRmp], 0) AS [DD_2BRmp], ISNULL([DD_Chep], 0) AS [DD_Chep], ISNULL([DD_D], 0) AS [DD_D], ISNULL([DD_I], 0) AS [DD_I], ISNULL([DD_Neu], 0) AS [DD_Neu], ISNULL([DD_Schr], 0) AS [DD_Schr], ISNULL([DD_U], 0) AS [DD_U], ISNULL([E1EP_D], 0) AS [E1EP_D], ISNULL([E1EP_I], 0) AS [E1EP_I], ISNULL([E1EP_Neu], 0) AS [E1EP_Neu], ISNULL([E2EP_D], 0) AS [E2EP_D], ISNULL([E2EP_I], 0) AS [E2EP_I], ISNULL([E2EP_Neu], 0) AS [E2EP_Neu], ISNULL([E2KR_D], 0) AS [E2KR_D], ISNULL([E2KR_I], 0) AS [E2KR_I], ISNULL([E2KR_Neu], 0) AS [E2KR_Neu], ISNULL([EUR_1A], 0) AS [EUR_1A], ISNULL([EUR_2A], 0) AS [EUR_2A], ISNULL([EUR_2B], 0) AS [EUR_2B], ISNULL([EUR_Chep], 0) AS [EUR_Chep], ISNULL([EUR_D], 0) AS [EUR_D], ISNULL([EUR_EW], 0) AS [EUR_EW], ISNULL([EUR_I], 0) AS [EUR_I], ISNULL([EUR_MPal], 0) AS [EUR_MPal], ISNULL([EUR_Neu], 0) AS [EUR_Neu], ISNULL([EUR_Schr], 0) AS [EUR_Schr], ISNULL([EUR_U], 0) AS [EUR_U], ISNULL([GB_1A], 0) AS [GB_1A], ISNULL([GB_D], 0) AS [GB_D], ISNULL([GB_EigV], 0) AS [GB_EigV], ISNULL([GB_I], 0) AS [GB_I], ISNULL([GB_U], 0) AS [GB_U], ISNULL([H1_D], 0) AS [H1_D], ISNULL([H1_DMiete_I], 0) AS [H1_DMiete_I], ISNULL([H1_dreckig], 0) AS [H1_dreckig], ISNULL([H1_EW], 0) AS [H1_EW], ISNULL([H1_I], 0) AS [H1_I], ISNULL([H1_mK], 0) AS [H1_mK], ISNULL([H1_1A], 0) AS [H1_neu], ISNULL([H1_ODmK], 0) AS [H1_ODmK], ISNULL([H1_ODoK], 0) AS [H1_ODoK], ISNULL([H1_oK], 0) AS [H1_oK], ISNULL([H1_sauber], 0) AS [H1_sauber], ISNULL([H1_Schr], 0) AS [H1_Schr], ISNULL([H1_U], 0) AS [H1_U], ISNULL([H1GD_I], 0) AS [H1GD_I] FROM
         (SELECT
            b.[ID]
            , b.BookingType_ID
            , b.ReferenceNumber
            , b.BookingDate
            , b.ExtDescription
            , b.IncludeInBalance
            , b.Matched
            , b.MatchedUser
            , b.MatchedTime
            , b.CreateUser
            , b.CreateTime
            , b.UpdateUser
            , b.UpdateTime
            , b.DeleteUser
            , b.DeleteTime
            , b.Account_ID
            , b.Transaction_ID
            , a.Name + '_' + q.Name AS ArticleQuality
            , b.Quantity
         FROM Bookings AS b
            LEFT JOIN Articles AS a ON a.ID = b.Article_ID
            LEFT JOIN Qualities AS q ON q.ID = b.Quality_ID
         ) src
      PIVOT
         (SUM(Quantity) FOR ArticleQuality IN ([DD_1A], [DD_2A], [DD_2APlus], [DD_2B], [DD_2BG], [DD_2BMT], [DD_2BRmp], [DD_Chep], [DD_D], [DD_I], [DD_Neu], [DD_Schr], [DD_U], [E1EP_D], [E1EP_I], [E1EP_Neu], [E2EP_D], [E2EP_I], [E2EP_Neu], [E2KR_D], [E2KR_I], [E2KR_Neu], [EUR_1A], [EUR_2A], [EUR_2B], [EUR_Chep], [EUR_D], [EUR_EW], [EUR_I], [EUR_MPal], [EUR_Neu], [EUR_Schr], [EUR_U], [GB_1A], [GB_D], [GB_EigV], [GB_I], [GB_U], [H1_D], [H1_DMiete_I], [H1_dreckig], [H1_EW], [H1_I], [H1_mK], [H1_1A], [H1_ODmK], [H1_ODoK], [H1_oK], [H1_sauber], [H1_Schr], [H1_U], [H1GD_I])) p)
GO
/****** Object:  View [dbo].[vBookingsPivotForGrid]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vBookingsPivotForGrid]
AS
SELECT        dbo.vBookingsPivot.ID, dbo.vBookingsPivot.BookingType_ID, dbo.BookingTypes.Name AS BookingTypesName, dbo.vBookingsPivot.ReferenceNumber, 
                         dbo.vBookingsPivot.BookingDate, dbo.vBookingsPivot.ExtDescription,
                             (SELECT        SUM(Amount) AS Amount
                               FROM            Condition.Fees
                               WHERE        (Booking_ID = dbo.vBookingsPivot.ID) AND (Condition.Fees.DeleteUser IS NULL)) AS Amount, dbo.vBookingsPivot.IncludeInBalance, dbo.vBookingsPivot.Matched, 
                         dbo.vBookingsPivot.MatchedUser, dbo.vBookingsPivot.MatchedTime, dbo.vBookingsPivot.CreateUser, dbo.vBookingsPivot.CreateTime, 
                         dbo.vBookingsPivot.UpdateUser, dbo.vBookingsPivot.UpdateTime, dbo.vBookingsPivot.DeleteUser, dbo.vBookingsPivot.DeleteTime, 
                         dbo.vBookingsPivot.Account_ID, dbo.vBookingsPivot.Transaction_ID, dbo.Transactions.Process_ID, dbo.vBookingsPivot.DD_1A, dbo.vBookingsPivot.DD_2A, 
                         dbo.vBookingsPivot.DD_2B, dbo.vBookingsPivot.DD_2BMT, dbo.vBookingsPivot.DD_2BRmp, dbo.vBookingsPivot.DD_D, dbo.vBookingsPivot.DD_I, 
                         dbo.vBookingsPivot.DD_U, dbo.vBookingsPivot.DD_Schr, dbo.vBookingsPivot.DD_Chep, dbo.vBookingsPivot.DD_Neu, dbo.vBookingsPivot.DD_2BG, dbo.vBookingsPivot.DD_2APlus,  
                         dbo.vBookingsPivot.EUR_1A, 
                         dbo.vBookingsPivot.EUR_2A, dbo.vBookingsPivot.EUR_2B, dbo.vBookingsPivot.EUR_D, dbo.vBookingsPivot.EUR_I, dbo.vBookingsPivot.EUR_U, 
                         dbo.vBookingsPivot.EUR_Schr, dbo.vBookingsPivot.EUR_EW, dbo.vBookingsPivot.EUR_Chep, dbo.vBookingsPivot.EUR_MPal, dbo.vBookingsPivot.EUR_Neu, 
                         dbo.vBookingsPivot.GB_1A, dbo.vBookingsPivot.GB_D, dbo.vBookingsPivot.GB_I, dbo.vBookingsPivot.GB_U, dbo.vBookingsPivot.GB_EigV, 
                         dbo.vBookingsPivot.H1_neu, dbo.vBookingsPivot.H1_D, dbo.vBookingsPivot.H1_mK, dbo.vBookingsPivot.H1_oK, dbo.vBookingsPivot.H1_I, 
                         dbo.vBookingsPivot.H1_sauber, dbo.vBookingsPivot.H1_dreckig, dbo.vBookingsPivot.H1_U, dbo.vBookingsPivot.H1_ODmK, dbo.vBookingsPivot.H1_ODoK, 
                         dbo.vBookingsPivot.H1_Schr, dbo.vBookingsPivot.H1_EW, dbo.vBookingsPivot.H1GD_I, 
                         dbo.vBookingsPivot.E2EP_Neu, dbo.vBookingsPivot.E2EP_I, dbo.vBookingsPivot.E2EP_D,
                         dbo.vBookingsPivot.E1EP_Neu, dbo.vBookingsPivot.E1EP_I, dbo.vBookingsPivot.E1EP_D,
                         dbo.vBookingsPivot.E2KR_Neu, dbo.vBookingsPivot.E2KR_I, dbo.vBookingsPivot.E2KR_D,
                         dbo.Accounts.FullName, dbo.Accounts.AccountType_ID, 
                         dbo.AccountTypes.Name AS AccountTypesName, dbo.Accounts.AccountNumber, dbo.Accounts.CustomerNumber, dbo.Accounts.AddressId, dbo.Accounts.Description, 
                         dbo.Transactions.ReferenceNumber AS TransactionsReferencenumber, dbo.Transactions.Valuta AS TransactionsValuta, 
                         dbo.Transactions.IntDescription AS TransactionsIntDescription, dbo.Transactions.ExtDescription AS TransactionsExtDescription, 
                         dbo.Transactions.TransactionType_ID, dbo.TransactionTypes.Name AS TransactionTypesName, dbo.TransactionTypes.Description AS TransactionTypesDescription, 
                         dbo.Transactions.TransactionState_ID, dbo.TransactionStates.Name AS TransactionStatesName, dbo.TransactionStates.Description AS TransactionStatesDescription,
                          dbo.Processes.Name AS ProcessesName, dbo.Processes.ReferenceNumber AS ProcessesReferencenumber, dbo.Processes.ProcessType_ID, 
                         dbo.Processes.IntDescription AS ProcessesIntDescription, dbo.Processes.ExtDescription AS ProcessesExtDescription, 
                         dbo.ProcessTypes.Name AS ProcessTypesName, dbo.ProcessTypes.Description AS ProcessTypesDescription, dbo.Processes.ProcessState_ID, 
                         dbo.ProcessStates.Name AS ProcessStatesName, dbo.ProcessStates.Description AS ProcessStatesDescription, TDdpl_ls.FieldValue AS dpl_ls_nr, 
                         TDdpl_ls.FieldDate AS dpl_ls_datum,
                             (SELECT        TOP (1) InvoiceNumber
                               FROM            Condition.Fees AS Fees_1
                               WHERE        (Booking_ID = dbo.vBookingsPivot.ID) AND (Fees_1.DeleteUser IS NULL)) AS dpl_re_nr, NULL AS dpl_re_datum, TDdpl_au.FieldValue AS dpl_au_nr, 
                         TDdpl_au.FieldDate AS dpl_au_datum, TDdpl_ab.FieldValue AS dpl_ab_nr, TDdpl_ab.FieldDate AS dpl_ab_datum, TDdpl_we.FieldValue AS dpl_we_nr, 
                         TDdpl_we.FieldDate AS dpl_we_datum, TDdpl_fa.FieldValue AS dpl_fa_nr, TDdpl_fa.FieldDate AS dpl_fa_datum, TDdpl_gu.FieldValue AS dpl_gu_nr, 
                         TDdpl_gu.FieldDate AS dpl_gu_datum, TDdpl_va.FieldValue AS dpl_va_nr, TDdpl_va.FieldDate AS dpl_va_datum, TDdpl_opg.FieldValue AS dpl_opg_nr, 
                         TDdpl_opg.FieldDate AS dpl_opg_datum, TDdpl_LTV.FieldValue AS dpl_LTV_nr, TDdpl_LTV.FieldDate AS dpl_LTV_buchungsdatum, TDext_ls.FieldValue AS ext_ls_nr, 
                         TDext_ls.FieldDate AS ext_ls_datum, TDext_re.FieldValue AS ext_re_nr, TDext_re.FieldDate AS ext_re_datum, TDext_db.FieldValue AS dpl_depot_beleg_nr, 
                         TDext_db.FieldDate AS dpl_depot_beleg_datum, TDext_snst.FieldValue AS ext_snst_nr, TDext_snst.FieldDate AS ext_snst_datum, 
                         TDext_abholer.FieldValue AS ext_abholer, TDext_anlieferer.FieldValue AS ext_anlieferer, TDext_kfz_kz.FieldValue AS ext_kfz_kz
FROM            dbo.Accounts LEFT OUTER JOIN
                         dbo.AccountTypes ON dbo.AccountTypes.ID = dbo.Accounts.AccountType_ID RIGHT OUTER JOIN
                         dbo.vBookingsPivot ON dbo.Accounts.ID = dbo.vBookingsPivot.Account_ID LEFT OUTER JOIN
                         dbo.BookingTypes ON dbo.BookingTypes.ID = dbo.vBookingsPivot.BookingType_ID LEFT OUTER JOIN
                         dbo.Transactions ON dbo.vBookingsPivot.Transaction_ID = dbo.Transactions.ID LEFT OUTER JOIN
                         dbo.TransactionTypes ON dbo.Transactions.TransactionType_ID = dbo.TransactionTypes.ID LEFT OUTER JOIN
                         dbo.TransactionStates ON dbo.Transactions.TransactionState_ID = dbo.TransactionStates.ID LEFT OUTER JOIN
                         dbo.Processes ON dbo.Transactions.Process_ID = dbo.Processes.ID LEFT OUTER JOIN
                         dbo.ProcessTypes ON dbo.Processes.ProcessType_ID = dbo.ProcessTypes.ID LEFT OUTER JOIN
                         dbo.ProcessStates ON dbo.ProcessStates.ID = dbo.Processes.ProcessState_ID LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_ls ON TDdpl_ls.Transaction_ID = dbo.Transactions.ID AND TDdpl_ls.FieldName = 'LS' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_au ON TDdpl_au.Transaction_ID = dbo.Transactions.ID AND TDdpl_au.FieldName = 'AU' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_ab ON TDdpl_ab.Transaction_ID = dbo.Transactions.ID AND TDdpl_ab.FieldName = 'AB' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_we ON TDdpl_we.Transaction_ID = dbo.Transactions.ID AND TDdpl_we.FieldName = 'WE' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_fa ON TDdpl_fa.Transaction_ID = dbo.Transactions.ID AND TDdpl_fa.FieldName = 'FA' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_gu ON TDdpl_gu.Transaction_ID = dbo.Transactions.ID AND TDdpl_gu.FieldName = 'GU' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_va ON TDdpl_va.Transaction_ID = dbo.Transactions.ID AND TDdpl_va.FieldName = 'VA' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_opg ON TDdpl_opg.Transaction_ID = dbo.Transactions.ID AND TDdpl_opg.FieldName = 'DPG' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_LTV ON TDdpl_LTV.Transaction_ID = dbo.Transactions.ID AND TDdpl_LTV.FieldName = 'LTV' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_ls ON TDext_ls.Transaction_ID = dbo.Transactions.ID AND TDext_ls.FieldName = 'EXTLS' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_re ON TDext_re.Transaction_ID = dbo.Transactions.ID AND TDext_re.FieldName = 'EXTRE' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_db ON TDext_db.Transaction_ID = dbo.Transactions.ID AND TDext_db.FieldName = 'EXTDB' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_snst ON TDext_snst.Transaction_ID = dbo.Transactions.ID AND TDext_snst.FieldName = 'SONST' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_abholer ON TDext_abholer.Transaction_ID = dbo.Transactions.ID AND TDext_abholer.FieldName = 'ABHOLER' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_anlieferer ON TDext_anlieferer.Transaction_ID = dbo.Transactions.ID AND 
                         TDext_anlieferer.FieldName = 'ANLIEFERER' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_kfz_kz ON TDext_kfz_kz.Transaction_ID = dbo.Transactions.ID AND TDext_kfz_kz.FieldName = 'KFZ'

GO
/****** Object:  UserDefinedFunction [LTMS].[fn_query_dpg]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 10.04.2014
-- Description: Liefert die DPG Daten zum Vergleichen mit LTMS
-- =============================================
CREATE FUNCTION [LTMS].[fn_query_dpg] ( @from DATE )
RETURNS TABLE
AS
RETURN
    ( SELECT    dbo.TransactionDetails.FieldGuid AS LtmsDpgId ,
                dbo.Transactions.ID AS TransactionId ,
                dbo.Processes.ReferenceNumber AS SubmissionNumber ,
                dbo.Bookings.ReferenceNumber ,
                dbo.Bookings.BookingType_ID ,
                dbo.Accounts.AccountNumber ,
                dbo.Transactions.Valuta ,
                dbo.Bookings.BookingDate ,
                CONVERT(VARCHAR(6), dbo.Pallet.Name) AS Pallet ,
                dbo.Bookings.Quantity
      FROM      dbo.Transactions
                INNER JOIN dbo.Processes ON dbo.Transactions.Process_ID = dbo.Processes.ID
                                            AND dbo.Processes.ProcessState_ID IN ( 1, 2 )
                                            AND dbo.Transactions.DeleteTime IS NULL
                INNER JOIN dbo.TransactionDetails ON dbo.Transactions.ID = dbo.TransactionDetails.Transaction_ID
                                                     AND dbo.TransactionDetails.FieldName = 'DPG'
                INNER JOIN dbo.Bookings ON dbo.Transactions.ID = dbo.Bookings.Transaction_ID
                INNER JOIN dbo.Accounts ON dbo.Bookings.Account_ID = dbo.Accounts.ID
                INNER JOIN dbo.Pallet ON dbo.Bookings.Article_ID = dbo.Pallet.Article_ID
                                         AND dbo.Bookings.Quality_ID = dbo.Pallet.Quality_ID
      WHERE     ( dbo.Transactions.DeleteTime IS NULL )
                AND ( dbo.Transactions.TransactionState_ID IN ( 1, 2 ) )
                AND ( dbo.Transactions.CreateTime >= @from )
    )
GO
/****** Object:  View [LTMS].[vDPG]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [LTMS].[vDPG]
AS
SELECT        LtmsDpgId, TransactionId, SubmissionNumber, ReferenceNumber, BookingType_ID, AccountNumber, Valuta, BookingDate, Pallet, Quantity
FROM            LTMS.fn_query_dpg('01.03.2014') AS fn_query_dpg_1
GO
/****** Object:  View [dbo].[vBookingsForGrid]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vBookingsForGrid]
AS
SELECT        dbo.Bookings.ID, dbo.Bookings.BookingType_ID, dbo.BookingTypes.Name AS BookingTypesName, dbo.Bookings.ReferenceNumber, dbo.Bookings.BookingDate, 
                         dbo.Bookings.ExtDescription,
                             (SELECT        SUM(Amount) AS Amount
                               FROM            Condition.Fees
                               WHERE        (Booking_ID = dbo.Bookings.ID) AND (Condition.Fees.DeleteUser IS NULL)) AS Amount, dbo.Bookings.IncludeInBalance, dbo.Bookings.Matched, dbo.Bookings.MatchedUser, 
                         dbo.Bookings.MatchedTime, dbo.Bookings.CreateUser, dbo.Bookings.CreateTime, dbo.Bookings.UpdateUser, dbo.Bookings.UpdateTime, dbo.Bookings.DeleteUser, 
                         dbo.Bookings.DeleteTime, dbo.Bookings.Account_ID, dbo.Bookings.Transaction_ID, dbo.Transactions.Process_ID, dbo.Bookings.Quantity, dbo.Bookings.Article_ID, 
                         dbo.Articles.Name AS ArticlesName, dbo.Articles.Description AS ArticlesDescription, dbo.Bookings.Quality_ID, dbo.Qualities.Name AS QualitiesName, 
                         dbo.Qualities.Description AS QualitiesDescription, dbo.Accounts.FullName, dbo.Accounts.AccountType_ID, dbo.AccountTypes.Name AS AccountTypesName, 
                         dbo.Accounts.AccountNumber, dbo.Accounts.CustomerNumber, dbo.Accounts.AddressId, dbo.Accounts.Description, 
                         dbo.Transactions.ReferenceNumber AS TransactionsReferencenumber, dbo.Transactions.Valuta AS TransactionsValuta, 
                         dbo.Transactions.IntDescription AS TransactionsIntDescription, dbo.Transactions.ExtDescription AS TransactionsExtDescription, 
                         dbo.Transactions.TransactionType_ID, dbo.TransactionTypes.Name AS TransactionTypesName, dbo.TransactionTypes.Description AS TransactionTypesDescription, 
                         dbo.Transactions.TransactionState_ID, dbo.TransactionStates.Name AS TransactionStatesName, dbo.TransactionStates.Description AS TransactionStatesDescription,
                          dbo.Transactions.Cancellation_ID AS TransactionCancellation_ID, dbo.Processes.Name AS ProcessesName, 
                         dbo.Processes.ReferenceNumber AS ProcessesReferencenumber, dbo.Processes.ProcessType_ID, dbo.Processes.IntDescription AS ProcessesIntDescription, 
                         dbo.Processes.ExtDescription AS ProcessesExtDescription, dbo.ProcessTypes.Name AS ProcessTypesName, 
                         dbo.ProcessTypes.Description AS ProcessTypesDescription, dbo.Processes.ProcessState_ID, dbo.ProcessStates.Name AS ProcessStatesName, 
                         dbo.ProcessStates.Description AS ProcessStatesDescription, TDdpl_ls.FieldValue AS dpl_ls_nr, TDdpl_ls.FieldDate AS dpl_ls_datum,
                             (SELECT        TOP (1) InvoiceNumber
                               FROM            Condition.Fees AS Fees_1
                               WHERE        (Booking_ID = dbo.Bookings.ID) AND (Fees_1.DeleteUser IS NULL)) AS dpl_re_nr, NULL AS dpl_re_datum, TDdpl_au.FieldValue AS dpl_au_nr, TDdpl_au.FieldDate AS dpl_au_datum, 
                         TDdpl_ab.FieldValue AS dpl_ab_nr, TDdpl_ab.FieldDate AS dpl_ab_datum, TDdpl_we.FieldValue AS dpl_we_nr, TDdpl_we.FieldDate AS dpl_we_datum, 
                         TDdpl_fa.FieldValue AS dpl_fa_nr, TDdpl_fa.FieldDate AS dpl_fa_datum, TDdpl_gu.FieldValue AS dpl_gu_nr, TDdpl_gu.FieldDate AS dpl_gu_datum, 
                         TDdpl_va.FieldValue AS dpl_va_nr, TDdpl_va.FieldDate AS dpl_va_datum, TDdpl_opg.FieldValue AS dpl_opg_nr, TDdpl_opg.FieldDate AS dpl_opg_datum, 
                         TDdpl_LTV.FieldValue AS dpl_LTV_nr, TDdpl_LTV.FieldDate AS dpl_LTV_buchungsdatum, TDext_ls.FieldValue AS ext_ls_nr, TDext_ls.FieldDate AS ext_ls_datum, 
                         TDext_re.FieldValue AS ext_re_nr, TDext_re.FieldDate AS ext_re_datum, TDext_db.FieldValue AS dpl_depot_beleg_nr, 
                         TDext_db.FieldDate AS dpl_depot_beleg_datum, TDext_snst.FieldValue AS ext_snst_nr, TDext_snst.FieldDate AS ext_snst_datum, 
                         TDext_abholer.FieldValue AS ext_abholer, TDext_anlieferer.FieldValue AS ext_anlieferer, TDext_kfz_kz.FieldValue AS ext_kfz_kz
FROM            dbo.Accounts LEFT OUTER JOIN
                         dbo.AccountTypes ON dbo.AccountTypes.ID = dbo.Accounts.AccountType_ID RIGHT OUTER JOIN
                         dbo.Bookings ON dbo.Accounts.ID = dbo.Bookings.Account_ID LEFT OUTER JOIN
                         dbo.BookingTypes ON dbo.BookingTypes.ID = dbo.Bookings.BookingType_ID LEFT OUTER JOIN
                         dbo.Articles ON dbo.Articles.ID = dbo.Bookings.Article_ID LEFT OUTER JOIN
                         dbo.Qualities ON dbo.Qualities.ID = dbo.Bookings.Quality_ID LEFT OUTER JOIN
                         dbo.Transactions ON dbo.Bookings.Transaction_ID = dbo.Transactions.ID LEFT OUTER JOIN
                         dbo.TransactionTypes ON dbo.Transactions.TransactionType_ID = dbo.TransactionTypes.ID LEFT OUTER JOIN
                         dbo.TransactionStates ON dbo.Transactions.TransactionState_ID = dbo.TransactionStates.ID LEFT OUTER JOIN
                         dbo.Processes ON dbo.Transactions.Process_ID = dbo.Processes.ID LEFT OUTER JOIN
                         dbo.ProcessTypes ON dbo.Processes.ProcessType_ID = dbo.ProcessTypes.ID LEFT OUTER JOIN
                         dbo.ProcessStates ON dbo.ProcessStates.ID = dbo.Processes.ProcessState_ID LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_ls ON TDdpl_ls.Transaction_ID = dbo.Transactions.ID AND TDdpl_ls.FieldName = 'LS' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_au ON TDdpl_au.Transaction_ID = dbo.Transactions.ID AND TDdpl_au.FieldName = 'AU' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_ab ON TDdpl_ab.Transaction_ID = dbo.Transactions.ID AND TDdpl_ab.FieldName = 'AB' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_we ON TDdpl_we.Transaction_ID = dbo.Transactions.ID AND TDdpl_we.FieldName = 'WE' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_fa ON TDdpl_fa.Transaction_ID = dbo.Transactions.ID AND TDdpl_fa.FieldName = 'FA' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_gu ON TDdpl_gu.Transaction_ID = dbo.Transactions.ID AND TDdpl_gu.FieldName = 'GU' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_va ON TDdpl_va.Transaction_ID = dbo.Transactions.ID AND TDdpl_va.FieldName = 'VA' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_opg ON TDdpl_opg.Transaction_ID = dbo.Transactions.ID AND TDdpl_opg.FieldName = 'DPG' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDdpl_LTV ON TDdpl_LTV.Transaction_ID = dbo.Transactions.ID AND TDdpl_LTV.FieldName = 'LTV' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_ls ON TDext_ls.Transaction_ID = dbo.Transactions.ID AND TDext_ls.FieldName = 'EXTLS' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_re ON TDext_re.Transaction_ID = dbo.Transactions.ID AND TDext_re.FieldName = 'EXTRE' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_db ON TDext_db.Transaction_ID = dbo.Transactions.ID AND TDext_db.FieldName = 'EXTDB' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_snst ON TDext_snst.Transaction_ID = dbo.Transactions.ID AND TDext_snst.FieldName = 'SONST' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_abholer ON TDext_abholer.Transaction_ID = dbo.Transactions.ID AND TDext_abholer.FieldName = 'ABHOLER' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_anlieferer ON TDext_anlieferer.Transaction_ID = dbo.Transactions.ID AND 
                         TDext_anlieferer.FieldName = 'ANLIEFERER' LEFT OUTER JOIN
                         dbo.TransactionDetails AS TDext_kfz_kz ON TDext_kfz_kz.Transaction_ID = dbo.Transactions.ID AND TDext_kfz_kz.FieldName = 'KFZ'

GO
/****** Object:  View [dbo].[vBookingsCompleteForGrid]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vBookingsCompleteForGrid]
AS
SELECT        dbo.vBookingsPivotForGrid.ID, dbo.vBookingsPivotForGrid.BookingType_ID, dbo.vBookingsPivotForGrid.BookingTypesName, 
                         dbo.vBookingsPivotForGrid.ReferenceNumber, dbo.vBookingsPivotForGrid.BookingDate, dbo.vBookingsPivotForGrid.ExtDescription, 
                         dbo.vBookingsPivotForGrid.Amount, dbo.vBookingsPivotForGrid.IncludeInBalance, dbo.vBookingsPivotForGrid.Matched, dbo.vBookingsPivotForGrid.MatchedUser, 
                         dbo.vBookingsPivotForGrid.MatchedTime, dbo.vBookingsPivotForGrid.CreateUser, dbo.vBookingsPivotForGrid.CreateTime, dbo.vBookingsPivotForGrid.UpdateUser, 
                         dbo.vBookingsPivotForGrid.UpdateTime, dbo.vBookingsPivotForGrid.DeleteUser, dbo.vBookingsPivotForGrid.DeleteTime, dbo.vBookingsPivotForGrid.Account_ID, 
                         dbo.vBookingsPivotForGrid.Transaction_ID, dbo.vBookingsPivotForGrid.Process_ID, dbo.vBookingsPivotForGrid.DD_1A, dbo.vBookingsPivotForGrid.DD_2A, 
                         dbo.vBookingsPivotForGrid.DD_2B, dbo.vBookingsPivotForGrid.DD_2BMT, dbo.vBookingsPivotForGrid.DD_2BRmp, dbo.vBookingsPivotForGrid.DD_D, 
                         dbo.vBookingsPivotForGrid.DD_I, dbo.vBookingsPivotForGrid.DD_U, dbo.vBookingsPivotForGrid.DD_Schr, dbo.vBookingsPivotForGrid.DD_Chep, 
                         dbo.vBookingsPivotForGrid.DD_Neu, dbo.vBookingsPivotForGrid.DD_2BG, dbo.vBookingsPivotForGrid.DD_2APlus, dbo.vBookingsPivotForGrid.EUR_1A, 
                         dbo.vBookingsPivotForGrid.EUR_2A, dbo.vBookingsPivotForGrid.EUR_2B, dbo.vBookingsPivotForGrid.EUR_D, dbo.vBookingsPivotForGrid.EUR_I, 
                         dbo.vBookingsPivotForGrid.EUR_U, dbo.vBookingsPivotForGrid.EUR_Schr, dbo.vBookingsPivotForGrid.EUR_EW, dbo.vBookingsPivotForGrid.EUR_Chep, 
                         dbo.vBookingsPivotForGrid.EUR_MPal, dbo.vBookingsPivotForGrid.EUR_Neu, dbo.vBookingsPivotForGrid.GB_1A, dbo.vBookingsPivotForGrid.GB_D, 
                         dbo.vBookingsPivotForGrid.GB_I, dbo.vBookingsPivotForGrid.GB_U, dbo.vBookingsPivotForGrid.GB_EigV, dbo.vBookingsPivotForGrid.H1_neu, 
                         dbo.vBookingsPivotForGrid.H1_D, dbo.vBookingsPivotForGrid.H1_mK, dbo.vBookingsPivotForGrid.H1_oK, dbo.vBookingsPivotForGrid.H1_I, 
                         dbo.vBookingsPivotForGrid.H1_sauber, dbo.vBookingsPivotForGrid.H1_dreckig, dbo.vBookingsPivotForGrid.H1_U, dbo.vBookingsPivotForGrid.H1_ODmK, 
                         dbo.vBookingsPivotForGrid.H1_ODoK, dbo.vBookingsPivotForGrid.H1_Schr, dbo.vBookingsPivotForGrid.H1_EW, dbo.vBookingsPivotForGrid.H1GD_I, 
                         dbo.vBookingsPivotForGrid.E2EP_Neu, dbo.vBookingsPivotForGrid.E2EP_I, dbo.vBookingsPivotForGrid.E2EP_D,
                         dbo.vBookingsPivotForGrid.E1EP_Neu, dbo.vBookingsPivotForGrid.E1EP_I, dbo.vBookingsPivotForGrid.E1EP_D,
                         dbo.vBookingsPivotForGrid.E2KR_Neu, dbo.vBookingsPivotForGrid.E2KR_I, dbo.vBookingsPivotForGrid.E2KR_D,
                         dbo.vBookingsPivotForGrid.FullName, dbo.vBookingsPivotForGrid.AccountNumber, dbo.vBookingsPivotForGrid.CustomerNumber, 
                         dbo.vBookingsPivotForGrid.AddressId, dbo.vBookingsPivotForGrid.Description, dbo.vBookingsPivotForGrid.TransactionsReferencenumber, 
                         dbo.vBookingsPivotForGrid.TransactionsIntDescription, dbo.vBookingsPivotForGrid.TransactionsExtDescription, dbo.vBookingsPivotForGrid.TransactionType_ID, 
                         dbo.vBookingsPivotForGrid.TransactionTypesName, dbo.vBookingsPivotForGrid.TransactionTypesDescription, dbo.vBookingsPivotForGrid.TransactionState_ID, 
                         dbo.vBookingsPivotForGrid.TransactionStatesName, dbo.vBookingsPivotForGrid.TransactionStatesDescription, dbo.vBookingsPivotForGrid.ProcessesName, 
                         dbo.vBookingsPivotForGrid.ProcessesReferencenumber, dbo.vBookingsPivotForGrid.ProcessType_ID, dbo.vBookingsPivotForGrid.ProcessTypesName, 
                         dbo.vBookingsPivotForGrid.ProcessTypesDescription, dbo.vBookingsPivotForGrid.ProcessState_ID, dbo.vBookingsPivotForGrid.ProcessStatesName, 
                         dbo.vBookingsPivotForGrid.ProcessStatesDescription, dbo.vBookingsPivotForGrid.dpl_ls_nr, dbo.vBookingsPivotForGrid.dpl_ls_datum, 
                         dbo.vBookingsPivotForGrid.dpl_re_nr, dbo.vBookingsPivotForGrid.dpl_re_datum, dbo.vBookingsPivotForGrid.dpl_au_nr, dbo.vBookingsPivotForGrid.dpl_au_datum, 
                         dbo.vBookingsPivotForGrid.dpl_we_nr, dbo.vBookingsPivotForGrid.dpl_we_datum, dbo.vBookingsPivotForGrid.dpl_fa_nr, dbo.vBookingsPivotForGrid.dpl_fa_datum, 
                         dbo.vBookingsPivotForGrid.dpl_depot_beleg_nr, dbo.vBookingsPivotForGrid.dpl_depot_beleg_datum, dbo.vBookingsPivotForGrid.dpl_opg_nr, 
                         dbo.vBookingsPivotForGrid.dpl_opg_datum, dbo.vBookingsPivotForGrid.dpl_LTV_nr, dbo.vBookingsPivotForGrid.dpl_LTV_buchungsdatum, 
                         dbo.vBookingsPivotForGrid.ext_ls_nr, dbo.vBookingsPivotForGrid.ext_ls_datum, dbo.vBookingsPivotForGrid.ext_re_nr, dbo.vBookingsPivotForGrid.ext_re_datum, 
                         dbo.vBookingsPivotForGrid.ext_snst_nr, dbo.vBookingsPivotForGrid.ext_snst_datum, dbo.vBookingsPivotForGrid.ext_abholer, 
                         dbo.vBookingsPivotForGrid.ext_anlieferer, dbo.vBookingsForGrid.Quantity, dbo.vBookingsForGrid.Article_ID, dbo.vBookingsForGrid.ArticlesName, 
                         dbo.vBookingsForGrid.ArticlesDescription, dbo.vBookingsForGrid.Quality_ID, dbo.vBookingsForGrid.QualitiesName, dbo.vBookingsForGrid.QualitiesDescription, 
                         dbo.vBookingsPivotForGrid.AccountType_ID, dbo.vBookingsPivotForGrid.AccountTypesName, dbo.vBookingsPivotForGrid.TransactionsValuta, 
                         dbo.vBookingsPivotForGrid.ProcessesIntDescription, dbo.vBookingsPivotForGrid.ProcessesExtDescription, dbo.vBookingsPivotForGrid.dpl_ab_nr, 
                         dbo.vBookingsPivotForGrid.dpl_ab_datum, dbo.vBookingsPivotForGrid.dpl_gu_nr, dbo.vBookingsPivotForGrid.dpl_gu_datum, dbo.vBookingsPivotForGrid.dpl_va_nr, 
                         dbo.vBookingsPivotForGrid.dpl_va_datum, dbo.vBookingsPivotForGrid.ext_kfz_kz, dbo.vBookingsForGrid.TransactionCancellation_ID
FROM            dbo.vBookingsPivotForGrid INNER JOIN
                         dbo.vBookingsForGrid ON dbo.vBookingsPivotForGrid.ID = dbo.vBookingsForGrid.ID

GO
/****** Object:  Table [dbo].[BookingDetails]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingDetails](
	[ID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FieldName] [varchar](25) NOT NULL,
	[FieldValue] [varchar](500) NULL,
	[OptimisticLockField] [int] NOT NULL,
	[Booking_ID] [int] NOT NULL,
	[FieldDate] [datetime] NULL,
	[FieldInt] [int] NULL,
	[FieldDecimal] [smallmoney] NULL,
	[FieldBool] [bit] NULL,
	[FieldText] [nvarchar](max) NULL,
	[FieldGuid] [uniqueidentifier] NULL,
	[FieldType] [varchar](255) NULL,
 CONSTRAINT [PK_dbo.BookingDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [LTMS].[fn_query_bookings_pre]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:  Alexander Schlicht
-- Create date: 10.04.2014
-- Description: Liefert Rohdaten zum Vergleichen von Buchungen mit dem LTMS
-- =============================================
CREATE FUNCTION [LTMS].[fn_query_bookings_pre] ( )
RETURNS TABLE
AS
RETURN
    ( SELECT    P.ID AS PID ,
                T.ID AS TID ,
                B.ID AS BID ,
                A.AccountNumber ,
                BT.ID AS BookingType_ID ,
                BT.LtmsUserTask AS UserTask ,
                T.ReferenceNumber ,
                T.Valuta ,
                B.BookingDate ,
                LT.Name AS Pallet ,
                B.Quantity ,
                B.Account_ID ,
                B.IncludeInBalance ,
                CAST(BD.FieldValue AS NVARCHAR(50)) AS BSN ,
                TD.FieldName AS FieldName ,
                TD.FieldValue AS FieldValue ,
                CAST(IIF(T.Cancellation_ID IS NOT NULL
                OR T.TransactionState_ID = 4, 1, 0) AS BIT) Storniert ,
                B.CreateTime CREATE_TIME ,
                B.CreateUser CREATE_USER ,
                B.UpdateTime UPDATE_TIME ,
                B.UpdateUser UPDATE_USER ,
                T.IntDescription MEMO
      FROM      dbo.Accounts AS A
                INNER JOIN LTMS.fn_query_accounts_import_to_ltms() AS ltmsaccounts ON A.ID = ltmsaccounts.ID
                INNER JOIN dbo.Bookings AS B ON B.Account_ID = A.ID
                                                AND B.DeleteTime IS NULL
                JOIN dbo.Transactions AS T ON T.ID = B.Transaction_ID
                                              AND T.DeleteTime IS NULL
                INNER JOIN dbo.Processes AS P ON P.ID = T.Process_ID
                                                 AND P.ProcessState_ID IN ( 0, 1, 2 )
                                                 AND P.DeleteTime IS NULL
                INNER JOIN dbo.BookingTypes AS BT ON B.BookingType_ID = BT.ID
                INNER JOIN dbo.Pallet AS LT ON B.Article_ID = LT.Article_ID
                                               AND B.Quality_ID = LT.Quality_ID
                LEFT OUTER JOIN dbo.TransactionDetails AS TD ON T.ID = TD.Transaction_ID
                                                                AND TD.FieldName IN ( 'LS', 'WE', 'EXTAU', 'AB' )
                LEFT OUTER JOIN dbo.BookingDetails AS BD ON B.ID = BD.Booking_ID
                                                            AND BD.FieldName = 'BSN'
      WHERE     ( BT.LtmsUserTask IS NOT NULL )
    );
    

GO
/****** Object:  UserDefinedFunction [LTMS].[fn_query_bookings]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:  <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE FUNCTION [LTMS].[fn_query_bookings] ( )
RETURNS TABLE
AS
RETURN
    ( SELECT    PID ,
                TID ,
                BID ,
                BookingType_ID ,
                UserTask ,
                ReferenceNumber ,
                Account_ID ,
                AccountNumber ,
                Valuta ,
                BookingDate ,
                IncludeInBalance ,
                Pallet ,
                Quantity ,
                BSN ,
                [LS] ,
                [WE] ,
                [EXTAU] ,
                [AB] ,
                Storniert, 
    CREATE_TIME ,
                CREATE_USER ,
                UPDATE_TIME ,
                UPDATE_USER,
    MEMO
      FROM      ( SELECT    PID ,
                            TID ,
                            BID ,
                            BookingType_ID ,
                            UserTask ,
                            ReferenceNumber ,
                            Account_ID ,
                            AccountNumber ,
                            Valuta ,
                            BookingDate ,
                            Pallet ,
                            IncludeInBalance ,
                            Quantity ,
                            BSN ,
                            FieldName ,
                            FieldValue ,
                            Storniert ,
                            CREATE_TIME ,
                            CREATE_USER ,
                            UPDATE_TIME ,
                            UPDATE_USER,
       MEMO
                  FROM      LTMS.fn_query_bookings_pre()
                ) AS P PIVOT ( MIN(FieldValue) FOR FieldName IN ( [LS], [WE], [EXTAU], [AB] ) ) AS PVT
    );

GO
/****** Object:  View [LTMS].[vBookings]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [LTMS].[vBookings]
AS
    SELECT  PID ,
            TID ,
            BID ,
            UserTask ,
            BookingType_ID ,
            ReferenceNumber ,
            BSN ,
            AccountNumber ,
            Account_ID ,
            Valuta ,
            BookingDate ,
            IncludeInBalance ,
            Pallet ,
            Quantity ,
            LS ,
            WE ,
            EXTAU ,
            AB ,
            Storniert ,
            CREATE_TIME ,
            CREATE_USER ,
            UPDATE_TIME ,
            UPDATE_USER ,
            MEMO
    FROM    LTMS.fn_query_bookings() AS fn_query_bookings_1;


GO
/****** Object:  UserDefinedFunction [dbo].[fn_indexfragmentierung]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht	
-- Create date: 11.05.2009
-- Description:	Indexprüfung/fragmentierung
-- =============================================
CREATE FUNCTION [dbo].[fn_indexfragmentierung] ( )
RETURNS TABLE
AS
RETURN
    ( SELECT    OBJECT_NAME(ps.OBJECT_ID) AS TabellenName,
                s.Name AS SchemaName,
                i.NAME AS IndexName,
                CAST(ps.avg_fragmentation_in_percent AS DECIMAL(6, 2)) AS Fragmentierung,
                CASE WHEN ps.avg_fragmentation_in_percent < 10 THEN 'nichts'
                     WHEN ps.avg_fragmentation_in_percent >= 10
                          AND ps.avg_fragmentation_in_percent < 40
                     THEN 'reorganize'
                     ELSE 'rebuild'
                END AS Erforderlich
      FROM      sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL,
                                               'limited') AS ps
                INNER JOIN sys.indexes AS i ON i.index_id = ps.index_id
                                               AND i.OBJECT_ID = ps.OBJECT_ID
                INNER JOIN sys.objects AS o ON ps.OBJECT_ID = o.OBJECT_ID
                INNER JOIN sys.schemas AS s ON o.SCHEMA_ID = s.SCHEMA_ID
      WHERE     OBJECTPROPERTY(i.OBJECT_ID, 'IsUserTable') = 1
    )
GO
/****** Object:  View [dbo].[vIndexfragmentierung]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vIndexfragmentierung]
AS
SELECT        TabellenName, SchemaName, IndexName, Fragmentierung, Erforderlich
FROM            dbo.fn_indexfragmentierung() AS t1
GO
/****** Object:  View [dbo].[vIndexRebuild]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vIndexRebuild]
AS
SELECT        TOP (100) PERCENT 'alter index ' + QUOTENAME(IndexName) + ' on ' + QUOTENAME(SchemaName) + '.' + QUOTENAME(TabellenName) 
                         + ' ' + 'rebuild' AS Kommando
FROM            dbo.fn_indexfragmentierung() AS table1
WHERE        (Erforderlich = 'rebuild')
ORDER BY TabellenName, IndexName
GO
/****** Object:  View [dbo].[vIndexReorg]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vIndexReorg]
AS
SELECT        TOP (100) PERCENT 'alter index ' + QUOTENAME(IndexName) + ' on ' + QUOTENAME(SchemaName) + '.' + QUOTENAME(TabellenName) 
                         + ' ' + Erforderlich AS Kommando
FROM            dbo.fn_indexfragmentierung() AS table1
WHERE        (Erforderlich = 'reorganize')
ORDER BY TabellenName, IndexName
GO
/****** Object:  UserDefinedFunction [dbo].[fn_accounts_adj2hid]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 25.09.2012
-- Description:	Konvertiert die Adjaceny List basierte Account Tabelle zu einer HID basierten Hierarchy Tabelle
-- =============================================
CREATE FUNCTION [dbo].[fn_accounts_adj2hid] ()
RETURNS TABLE
AS
RETURN
    (
	WITH    adj2hid(ID, Parent_ID, [PathByID])
              AS (SELECT    ID,
                            Parent_ID,
                            '/' + CAST(ID AS NVARCHAR(MAX)) + '/' AS [PathByID]
                  FROM      dbo.Accounts
                  WHERE     Parent_ID IS NULL
                  UNION ALL
                  SELECT    e.ID,
                            e.Parent_ID,
                            adj2hid.PathbyID + CAST(e.ID AS NVARCHAR(MAX)) + '/'
                  FROM      adj2hid
                            JOIN dbo.Accounts e ON e.Parent_ID = adj2hid.ID
                 )
    SELECT  adj2hid.ID,
            CAST(adj2hid.[PathByID] AS HIERARCHYID) AS HID
    FROM    adj2hid
)
GO
/****** Object:  UserDefinedFunction [dbo].[fn_accounts_adj2ni]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 25.09.2012
-- Description:	Konvertiert die Adjacency List -> Nested Intervals
-- =============================================
CREATE FUNCTION [dbo].[fn_accounts_adj2ni] ()
RETURNS TABLE
AS
RETURN
    (
	WITH    EmployeeRows
              AS (SELECT    ID,
                            CONVERT(VARCHAR(MAX), theRow) AS thePath,
                            CONVERT(BIGINT, 1) AS prevNumer,
                            CONVERT(BIGINT, 0) AS prevDenom,
                            CONVERT(BIGINT, theRow) AS currNumer,
                            CONVERT(BIGINT, 1) AS currDenom
                  FROM      (SELECT ID,
                                    ROW_NUMBER() OVER (ORDER BY ID) AS theRow
                             FROM   Accounts
                             WHERE  Parent_ID IS NULL
                            ) y
                  UNION ALL
                  SELECT    y.ID,
                            y.thePath + '.' + CONVERT(VARCHAR(MAX), y.theRow) AS thePath,
                            prevNumer = y.currNumer,
                            prevDenom = y.currDenom,
                            (y.currNumer * y.theRow) + y.prevNumer AS currNumer,
                            (y.currDenom * y.theRow) + y.prevDenom AS currDenom
                  FROM      (SELECT e.ID,
                                    x.thePath,
                                    x.currNumer,
                                    x.currDenom,
                                    x.prevNumer,
                                    x.prevDenom,
                                    ROW_NUMBER() OVER (ORDER BY e.ID) AS therow
                             FROM   EmployeeRows x
                                    JOIN Accounts e
                                    ON e.Parent_ID = x.ID
                            ) y
                 )
    SELECT  ID,
            thePath,
            currNumer AS startNumer,
            currDenom AS startDenom,
            currNumer + prevNumer AS endNumer,
            currDenom + prevDenom AS endDenom
    FROM    EmployeeRows
)
GO
/****** Object:  UserDefinedFunction [dbo].[fn_accounts_descendant_cte]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 25.09.2012
-- Description:	Liefert eine Liste von Konto-IDs (Childs-IDs) zurück die zu einem Knoten gehören, inklusive sich selbst.
--              Es wird eine recursive CTE Tabelle für die Ergebnissmenge verwendet
-- =============================================
CREATE FUNCTION [dbo].[fn_accounts_descendant_cte] (@id INT)
RETURNS TABLE
AS
RETURN
    (
	WITH    accountCTE
              AS (SELECT    ID,
                            Parent_ID
                  FROM      dbo.Accounts a
                  WHERE     ID = @id
                  UNION ALL
                  SELECT    Accounts.ID,
                            Accounts.Parent_ID
                  FROM      Accounts
                            INNER JOIN accountCTE ON dbo.Accounts.Parent_ID = accountCTE.ID
                 )
    SELECT  ID
    FROM    accountCTE
)
GO
/****** Object:  Table [dbo].[AccountHierarchy]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountHierarchy](
	[ID] [int] NOT NULL,
	[Parent_ID] [int] NULL,
	[HID] [hierarchyid] NULL,
	[TreeText] [nvarchar](255) NULL,
	[PathById] [nvarchar](4000) NULL,
	[TreeLevel] [smallint] NULL,
	[RowNumber] [int] NULL,
	[Lft] [int] NULL,
	[Rgt] [int] NULL,
	[HasChildren] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.AccountHierarchy] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_accounts_descendant_hid]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 25.09.2012
-- Description:	Liefert eine Liste von Konto-IDs (Childs-IDs) zurück die zu einem Knoten gehören, inklusive sich selbst.
--              Es wird die HID und ein Self Join für die Ergebnissmenge verwendet
-- =============================================
CREATE FUNCTION [dbo].[fn_accounts_descendant_hid] ()
RETURNS TABLE
    --WITH SCHEMABINDING
AS
RETURN
    (SELECT a1.ID,
            a2.ID AS Subordinate_ID
     FROM   dbo.AccountHierarchy AS a1
            INNER JOIN dbo.AccountHierarchy AS a2 ON a2.HID.IsDescendantOf(a1.HID) = 1
    )
GO
/****** Object:  UserDefinedFunction [dbo].[fn_accounts_hid2ns]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 25.09.2012
-- Description:	Konvertiert die HID zu einer Nested Set basierten Hierarchy Tabelle
-- =============================================
CREATE FUNCTION [dbo].[fn_accounts_hid2ns] ()
RETURNS TABLE
AS
RETURN
    (SELECT ah.ID,
            ROW_NUMBER() OVER (ORDER BY ah.HID.ToString()) AS RowNumber,
            (ROW_NUMBER() OVER (ORDER BY ah.HID.ToString()) * 2) - ah.HID.GetLevel() AS Lft,
            ((ROW_NUMBER() OVER (ORDER BY ah.HID.ToString()) * 2) - ah.HID.GetLevel()) + (SELECT    2 * (COUNT(*) - 1)
                                                                                          FROM      dbo.AccountHierarchy
                                                                                          WHERE     HID.IsDescendantOf(ah.HID) = 1
                                                                                         ) +1 AS Rgt
     FROM   dbo.AccountHierarchy AS ah
    )
GO
/****** Object:  UserDefinedFunction [dbo].[fn_query_single_path_lr]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 25.09.2012
-- Description:	Liefert eine Liste von Konto-IDs zurück die dem Pfad zum gegebenen Konto entsprechen 
-- =============================================
CREATE FUNCTION [dbo].[fn_query_single_path_lr] (@account_id INT)
RETURNS TABLE
AS
RETURN
    (SELECT parent.ID
     FROM   dbo.AccountHierarchy AS node,
            dbo.AccountHierarchy AS parent
     WHERE  node.lft BETWEEN parent.lft AND parent.rgt
            AND node.ID = @account_id
    )
GO
/****** Object:  Table [dbo].[AccountDetails]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountDetails](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FieldName] [varchar](25) NOT NULL,
	[FieldValue] [varchar](500) NULL,
	[OptimisticLockField] [int] NOT NULL,
	[Account_ID] [int] NOT NULL,
	[FieldDate] [datetime] NULL,
	[FieldInt] [int] NULL,
	[FieldDecimal] [smallmoney] NULL,
	[FieldBool] [bit] NULL,
	[FieldText] [nvarchar](max) NULL,
	[FieldGuid] [uniqueidentifier] NULL,
	[FieldType] [varchar](255) NULL,
 CONSTRAINT [PK_dbo.AccountDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vAccounts]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE VIEW [dbo].[vAccounts]
	AS
	SELECT  dbo.Accounts.ID ,
        dbo.Accounts.RowGuid ,
        dbo.Accounts.Name ,
        dbo.AccountDetails.FieldValue AS ExtName ,
        dbo.Accounts.Parent_ID ,
        dbo.Accounts.AddressId ,
        dbo.Accounts.Description ,
        dbo.Accounts.PathByName ,
        dbo.Accounts.CreateUser ,
        dbo.Accounts.CreateTime ,
        dbo.Accounts.UpdateUser ,
        dbo.Accounts.UpdateTime ,
        dbo.Accounts.OptimisticLockField
FROM    dbo.Accounts
        LEFT OUTER JOIN dbo.AccountDetails ON dbo.Accounts.ID = dbo.AccountDetails.Account_ID
                                              AND dbo.AccountDetails.FieldName = 'ExtName'
GO
/****** Object:  View [dbo].[vBookings]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vBookings]
AS
SELECT        dbo.Processes.ID AS Process_ID, dbo.Processes.Name AS ProcessName, dbo.ProcessStates.Name AS ProcessState, dbo.ProcessTypes.Name AS ProcessType, dbo.Accounts.AccountType_ID, 
                         dbo.Accounts.Name, dbo.Accounts.PathByName, dbo.Accounts.ID AS CustomerAccount_ID, dbo.Accounts.Parent_ID AS CustomerAccount_Parent_ID, dbo.Accounts.Description AS CustomerAccount_Description, 
                         dbo.Accounts.AddressId, dbo.Transactions.ID AS Transactions_ID, dbo.Transactions.Valuta, dbo.Transactions.ReferenceNumber AS Transaction_ReferenceNumber, 
                         dbo.Transactions.IntDescription AS Transaction_IntDescription, dbo.Transactions.ExtDescription AS Transaction_ExtDescription, dbo.Bookings.ID AS Booking_ID, dbo.Bookings.Transaction_ID, 
                         dbo.Bookings.BookingDate, dbo.Articles.Name + '_' + dbo.Qualities.Name AS CarrierName, dbo.Bookings.Article_ID, dbo.Bookings.Quality_ID, dbo.Bookings.Quantity, 
                         dbo.Bookings.ReferenceNumber AS Booking_ReferenceNumber, dbo.Bookings.IncludeInBalance, dbo.Bookings.Matched, dbo.Bookings.ExtDescription AS Booking_ExtDescription, Condition.Fees.Amount
FROM            dbo.Bookings INNER JOIN
                         dbo.Accounts ON dbo.Bookings.Account_ID = dbo.Accounts.ID INNER JOIN
                         dbo.Transactions ON dbo.Bookings.Transaction_ID = dbo.Transactions.ID INNER JOIN
                         dbo.Articles ON dbo.Bookings.Article_ID = dbo.Articles.ID INNER JOIN
                         dbo.Qualities ON dbo.Bookings.Quality_ID = dbo.Qualities.ID INNER JOIN
                         dbo.Processes ON dbo.Transactions.Process_ID = dbo.Processes.ID INNER JOIN
                         dbo.ProcessStates ON dbo.Processes.ProcessState_ID = dbo.ProcessStates.ID INNER JOIN
                         dbo.ProcessTypes ON dbo.Processes.ProcessType_ID = dbo.ProcessTypes.ID LEFT OUTER JOIN
                         Condition.Fees ON dbo.Bookings.ID = Condition.Fees.Booking_ID AND Condition.Fees.DeleteTime IS NULL
GO
/****** Object:  Table [Condition].[ExceptionalOppositeSideAccounts]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Condition].[ExceptionalOppositeSideAccounts](
	[Condition_ID] [int] NOT NULL,
	[Account_ID] [int] NOT NULL,
 CONSTRAINT [PK_Condition.ExceptionalOppositeSideAccounts] PRIMARY KEY CLUSTERED 
(
	[Condition_ID] ASC,
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Condition].[ExceptionalOppositeSideBookingTypes]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Condition].[ExceptionalOppositeSideBookingTypes](
	[Condition_ID] [int] NOT NULL,
	[BookingType_ID] [varchar](6) NOT NULL,
 CONSTRAINT [PK_Condition.ExceptionalOppositeSideBookingTypes] PRIMARY KEY CLUSTERED 
(
	[Condition_ID] ASC,
	[BookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Condition].[OnlyValidForOppositeSideAccounts]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Condition].[OnlyValidForOppositeSideAccounts](
	[Condition_ID] [int] NOT NULL,
	[Account_ID] [int] NOT NULL,
 CONSTRAINT [PK_Condition.OnlyValidForOppositeSideAccounts] PRIMARY KEY CLUSTERED 
(
	[Condition_ID] ASC,
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Condition].[OnlyValidForOppositeSideBookingTypes]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Condition].[OnlyValidForOppositeSideBookingTypes](
	[Condition_ID] [int] NOT NULL,
	[BookingType_ID] [varchar](6) NOT NULL,
 CONSTRAINT [PK_Condition.OnlyValidForOppositeSideBookingTypes] PRIMARY KEY CLUSTERED 
(
	[Condition_ID] ASC,
	[BookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Condition].[Swaps]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Condition].[Swaps](
	[ID] [int] NOT NULL,
	[InFactor] [int] NOT NULL,
	[OutFactor] [int] NOT NULL,
 CONSTRAINT [PK_Condition.Swaps] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountAssociations]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountAssociations](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Account_ID] [int] NOT NULL,
	[Associated_ID] [int] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Owner] [nvarchar](128) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AccountAssociations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountContacts]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountContacts](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Account_ID] [int] NOT NULL,
	[Contact_ID] [bigint] NOT NULL,
	[Note] [nvarchar](255) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[TransmissionMethod] [nvarchar](5) NULL,
	[ProcessAutomatically] [bit] NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AccountContacts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountDirections]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountDirections](
	[ID] [nvarchar](1) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_dbo.AccountDirections] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountSyncStates]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountSyncStates](
	[ID] [int] NOT NULL,
	[Requests] [int] NOT NULL,
	[RequestMessages] [int] NOT NULL,
	[RequestTotalSize] [bigint] NOT NULL,
	[Responses] [int] NOT NULL,
	[ResponseMessages] [int] NOT NULL,
	[ResponseTotalSize] [bigint] NOT NULL,
	[StorageTotalSize] [bigint] NOT NULL,
	[Errors] [int] NOT NULL,
	[LastErrorCode] [int] NOT NULL,
	[LastError] [nvarchar](max) NULL,
	[SyncMessage] [nvarchar](255) NULL,
	[Inserts] [int] NOT NULL,
	[Updates] [int] NOT NULL,
	[Deletes] [int] NOT NULL,
	[OptimisticLockField] [int] NOT NULL,
	[LastSync] [datetime] NOT NULL,
	[SyncOperation] [int] NOT NULL,
	[ChangeOperation] [int] NOT NULL,
	[RequestEnqueued] [datetime] NOT NULL,
	[RequestDeQueued] [datetime] NULL,
	[RequestDeferred] [datetime] NULL,
	[ResponseEnqueued] [datetime] NULL,
	[ResponseDeQueued] [datetime] NULL,
	[ResponseDeferred] [datetime] NULL,
	[Acknowledged] [datetime] NULL,
	[SessionId] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.AccountSyncStates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](1) NOT NULL,
	[TableName] [nvarchar](100) NOT NULL,
	[RecordID] [nvarchar](100) NOT NULL,
	[ColumnName] [nvarchar](100) NOT NULL,
	[OriginalValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AuditLogs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BatchQueries]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BatchQueries](
	[ID] [uniqueidentifier] NOT NULL,
	[BatchId] [uniqueidentifier] NOT NULL,
	[RowId] [int] NULL,
	[RowValue] [varchar](255) NULL,
 CONSTRAINT [PK_dbo.BatchQueries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingSyncStates]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingSyncStates](
	[ID] [int] NOT NULL,
	[ErrorCode] [int] NULL,
	[LastSync] [datetime] NOT NULL,
	[SyncOperation] [int] NOT NULL,
	[ChangeOperation] [int] NOT NULL,
	[RequestEnqueued] [datetime] NOT NULL,
	[RequestDeQueued] [datetime] NULL,
	[RequestDeferred] [datetime] NULL,
	[ResponseEnqueued] [datetime] NULL,
	[ResponseDeQueued] [datetime] NULL,
	[ResponseDeferred] [datetime] NULL,
	[Acknowledged] [datetime] NULL,
	[SessionId] [nvarchar](128) NULL,
	[Account_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.BookingSyncStates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportAccountMappings]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportAccountMappings](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ImportPackage_ID] [int] NOT NULL,
	[Name1] [nvarchar](50) NOT NULL,
	[Name2] [nvarchar](50) NOT NULL,
	[AccountNumber] [varchar](13) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.ImportAccountMappings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportBookings]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportBookings](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Quantity] [int] NOT NULL,
	[ImportTransaction_ID] [int] NOT NULL,
	[Article_ID] [smallint] NOT NULL,
	[Quality_ID] [smallint] NOT NULL,
	[IsSourceBooking] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ImportBookings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportPackages]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportPackages](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.ImportPackages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportStacks]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportStacks](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ImportPackage_ID] [int] NOT NULL,
	[Time] [datetime] NOT NULL,
	[User] [nvarchar](50) NULL,
	[FileName] [nvarchar](260) NULL,
 CONSTRAINT [PK_dbo.ImportStacks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportTransactions]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportTransactions](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ImportStack_ID] [int] NOT NULL,
	[ProcessName] [varchar](20) NULL,
	[BookingDate] [datetime] NOT NULL,
	[SourceAccountNumber] [varchar](13) NULL,
	[SourceName1] [nvarchar](50) NULL,
	[SourceName2] [nvarchar](50) NULL,
	[SourceExtDescription] [varchar](255) NULL,
	[DestinationAccountNumber] [varchar](13) NULL,
	[DestinationName1] [nvarchar](50) NULL,
	[DestinationName2] [nvarchar](50) NULL,
	[DestinationExtDescription] [varchar](255) NULL,
	[ReferenceNumber] [varchar](50) NULL,
	[IntDescription] [varchar](255) NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[Transaction_ID] [int] NULL,
 CONSTRAINT [PK_dbo.ImportTransactions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceAccounts]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceAccounts](
	[Invoice_ID] [int] NOT NULL,
	[Account_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.InvoiceAccounts] PRIMARY KEY CLUSTERED 
(
	[Invoice_ID] ASC,
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceFees]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceFees](
	[Invoice_ID] [int] NOT NULL,
	[Fee_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.InvoiceFees] PRIMARY KEY CLUSTERED 
(
	[Invoice_ID] ASC,
	[Fee_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceQualities]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceQualities](
	[Invoice_ID] [int] NOT NULL,
	[Quality_ID] [smallint] NOT NULL,
 CONSTRAINT [PK_dbo.InvoiceQualities] PRIMARY KEY CLUSTERED 
(
	[Invoice_ID] ASC,
	[Quality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GrandTotal] [decimal](18, 2) NULL,
	[Name] [nvarchar](128) NULL,
	[ReportName] [nvarchar](128) NULL,
	[FileName] [nvarchar](255) NULL,
	[ReportFile] [varbinary](max) NULL,
	[FilePath] [nvarchar](4000) NULL,
	[ReportUrl] [nvarchar](4000) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Year] [int] NULL,
	[Quarter] [int] NULL,
	[Month] [int] NULL,
	[AddressId] [int] NULL,
	[CustomerNumber] [nvarchar](12) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[PrimaryAccount_ID] [int] NOT NULL,
	[Article_ID] [smallint] NULL,
	[ReportType_ID] [varchar](50) NOT NULL,
	[Number] [nvarchar](100) NULL,
	[DOC_ID] [bigint] NULL,
	[gdoc_id] [bigint] NULL,
 CONSTRAINT [PK_dbo.Invoices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PalletChangeCopyDefinitions]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PalletChangeCopyDefinitions](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CopyExtDescription] [bit] NOT NULL,
	[CopyIntDescription] [bit] NOT NULL,
	[CopyVouchers] [bit] NOT NULL,
	[CopyInvoice] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PalletChangeCopyDefinitions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PalletChangeDefinitionDetails]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PalletChangeDefinitionDetails](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FieldName] [varchar](25) NOT NULL,
	[FieldValue] [varchar](500) NULL,
	[FieldDecimal] [decimal](18, 2) NULL,
	[FieldDate] [datetime] NULL,
	[FieldInt] [int] NULL,
	[FieldBool] [bit] NULL,
	[FieldText] [nvarchar](max) NULL,
	[FieldGuid] [uniqueidentifier] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[PalletChangeDefinition_ID] [int] NOT NULL,
	[FieldType] [varchar](255) NULL,
 CONSTRAINT [PK_dbo.PalletChangeDefinitionDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PalletChangeDefinitions]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PalletChangeDefinitions](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Enabled] [bit] NOT NULL,
	[IsDefault] [bit] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Factor] [int] NOT NULL,
	[CalculateFee] [bit] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[PalletChangeCopyDefinition_ID] [int] NOT NULL,
	[AccountDirection_ID] [nvarchar](1) NOT NULL,
	[Account_ID] [int] NOT NULL,
	[OppositeAccout_ID] [int] NULL,
	[Set_ID] [int] NOT NULL,
	[BookingTypeFrom_ID] [varchar](6) NOT NULL,
	[BookingTypeTo_ID] [varchar](6) NOT NULL,
	[Discriminator] [int] NOT NULL,
	[Automatically] [bit] NOT NULL,
	[DeleteOrphaned] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PalletChangeDefinitions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PalletChanges]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PalletChanges](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ChangeFrom_ID] [smallint] NOT NULL,
	[ChangeTo_ID] [smallint] NOT NULL,
	[PalletChangeSet_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.PalletChanges] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PalletChangeSets]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PalletChangeSets](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_dbo.PalletChangeSets] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PalletChangeTriggerPalletChangeDefinitions]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PalletChangeTriggerPalletChangeDefinitions](
	[PalletChangeTrigger_ID] [int] NOT NULL,
	[PalletChangeDefinition_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.PalletChangeTriggerPalletChangeDefinitions] PRIMARY KEY CLUSTERED 
(
	[PalletChangeTrigger_ID] ASC,
	[PalletChangeDefinition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PalletChangeTriggers]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PalletChangeTriggers](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[BookingType_ID] [varchar](6) NOT NULL,
	[BookingTypeOppositeSide_ID] [varchar](6) NULL,
	[AccountOppositeSide_ID] [int] NULL,
 CONSTRAINT [PK_dbo.PalletChangeTriggers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessDetails]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessDetails](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FieldName] [varchar](25) NOT NULL,
	[FieldValue] [varchar](500) NULL,
	[OptimisticLockField] [int] NOT NULL,
	[Process_ID] [int] NOT NULL,
	[FieldDate] [datetime] NULL,
	[FieldInt] [int] NULL,
	[FieldDecimal] [smallmoney] NULL,
	[FieldBool] [bit] NULL,
	[FieldText] [nvarchar](max) NULL,
	[FieldGuid] [uniqueidentifier] NULL,
	[FieldType] [varchar](255) NULL,
 CONSTRAINT [PK_dbo.ProcessDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Repair]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Repair](
	[Transaction_ID] [int] NOT NULL,
	[BSN] [nvarchar](50) NOT NULL,
	[MAP] [bit] NULL,
	[MINID] [int] NULL,
	[DEL] [bit] NULL,
 CONSTRAINT [PK_Repair] PRIMARY KEY CLUSTERED 
(
	[Transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportAccounts]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportAccounts](
	[Report_ID] [int] NOT NULL,
	[Account_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ReportAccounts] PRIMARY KEY CLUSTERED 
(
	[Report_ID] ASC,
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportBookings]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportBookings](
	[Report_ID] [int] NOT NULL,
	[Booking_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ReportBookings] PRIMARY KEY CLUSTERED 
(
	[Report_ID] ASC,
	[Booking_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportDetails]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportDetails](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FieldName] [varchar](25) NOT NULL,
	[FieldValue] [varchar](500) NULL,
	[FieldDate] [datetime] NULL,
	[FieldInt] [int] NULL,
	[FieldBool] [bit] NULL,
	[FieldDecimal] [smallmoney] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[Report_ID] [int] NOT NULL,
	[FieldText] [nvarchar](max) NULL,
	[FieldGuid] [uniqueidentifier] NULL,
	[FieldType] [varchar](255) NULL,
 CONSTRAINT [PK_dbo.ReportDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](128) NULL,
	[ReportName] [nvarchar](128) NULL,
	[FileName] [nvarchar](255) NULL,
	[ReportFile] [varbinary](max) NULL,
	[FilePath] [nvarchar](4000) NULL,
	[ReportUrl] [nvarchar](4000) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Quarter] [int] NULL,
	[Month] [int] NULL,
	[AddressId] [int] NULL,
	[CustomerNumber] [nvarchar](12) NULL,
	[AdditionalParams] [xml] NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[ReportState_ID] [char](1) NOT NULL,
	[Year] [int] NULL,
	[MatchedUser] [varchar](50) NULL,
	[MatchedTime] [date] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[ReportType_ID] [varchar](50) NOT NULL,
	[PrimaryAccount_ID] [int] NOT NULL,
	[DOC_ID] [bigint] NULL,
	[gdoc_id] [bigint] NULL,
 CONSTRAINT [PK_dbo.Reports] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportStates]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportStates](
	[ID] [char](1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.ReportStates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportTypeAccountTypes]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportTypeAccountTypes](
	[ReportType_ID] [varchar](50) NOT NULL,
	[AccountType_ID] [varchar](3) NOT NULL,
 CONSTRAINT [PK_dbo.ReportTypeAccountTypes] PRIMARY KEY CLUSTERED 
(
	[ReportType_ID] ASC,
	[AccountType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportTypes]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportTypes](
	[ID] [varchar](50) NOT NULL,
	[ReportName] [nvarchar](50) NOT NULL,
	[NamePattern] [nvarchar](50) NOT NULL,
	[FileNamePattern] [nvarchar](255) NOT NULL,
	[DropDownText] [nvarchar](100) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[BarcodePrefix] [nvarchar](max) NULL,
	[HasToBeReconcilied] [bit] NOT NULL,
	[CanBeStored] [bit] NOT NULL,
	[IsInvoice] [bit] NOT NULL,
	[DefaultMatchedFilter] [bit] NULL,
	[DefaultIncludeAssociationsOfType] [nvarchar](max) NULL,
	[ArticleIntrinsic] [bit] NULL,
	[IncludeAllArticles] [bit] NULL,
	[FileFormat] [varchar](10) NULL,
 CONSTRAINT [PK_dbo.ReportTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagAccounts]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagAccounts](
	[Tag_ID] [int] NOT NULL,
	[Account_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TagAccounts] PRIMARY KEY CLUSTERED 
(
	[Tag_ID] ASC,
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreateUser] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[Role] [nvarchar](128) NULL,
	[Owner] [nvarchar](128) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.Tags] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDetails]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDetails](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FieldName] [varchar](25) NOT NULL,
	[FieldValue] [varchar](500) NULL,
	[FieldDate] [datetime] NULL,
	[FieldInt] [int] NULL,
	[FieldBool] [bit] NULL,
	[FieldDecimal] [smallmoney] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[User_ID] [varchar](50) NOT NULL,
	[FieldText] [nvarchar](max) NULL,
	[FieldGuid] [uniqueidentifier] NULL,
	[FieldType] [varchar](255) NULL,
 CONSTRAINT [PK_dbo.UserDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [varchar](50) NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](128) NOT NULL,
	[LastName] [nvarchar](128) NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL,
	[Email] [nvarchar](128) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Initial] [nvarchar](4) NULL,
	[PersonnelNumber] [char](10) NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Transaction].[Swaps]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Transaction].[Swaps](
	[ID] [int] NOT NULL,
	[Term_ID] [varchar](6) NOT NULL,
	[Condition_ID] [int] NULL,
	[SwapAccount_ID] [int] NOT NULL,
	[InBooking_ID] [int] NOT NULL,
	[OutBooking_ID] [int] NOT NULL,
	[InFactor] [int] NOT NULL,
	[OutFactor] [int] NOT NULL,
	[Remainder] [float] NOT NULL,
 CONSTRAINT [PK_Transaction.Swaps] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[Debits]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[Debits](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ClearingDate] [date] NOT NULL,
	[PickUpDate] [date] NULL,
	[ReferenceNumber] [nvarchar](17) NULL,
	[PickupAddressCustomerNumber] [varchar](12) NULL,
	[Transaction_ID] [int] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[Debitor_ID] [int] NOT NULL,
	[DebitType_ID] [nvarchar](3) NOT NULL,
 CONSTRAINT [PK_Voucher.Debits] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[DebitTypes]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[DebitTypes](
	[ID] [nvarchar](3) NOT NULL,
	[Name] [nvarchar](25) NULL,
	[ProcessName] [nvarchar](30) NULL,
 CONSTRAINT [PK_Voucher.DebitTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[Externals]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[Externals](
	[ID] [bigint] NOT NULL,
	[NoExpirationDate] [bit] NOT NULL,
	[SubmitterCustomerNumber] [nvarchar](12) NOT NULL,
	[Submission_ID] [int] NOT NULL,
	[Debit_ID] [int] NULL,
 CONSTRAINT [PK_Voucher.Externals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[Internals]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[Internals](
	[ID] [bigint] NOT NULL,
	[VoucherReason_ID] [nvarchar](3) NULL,
	[VoucherState_ID] [nvarchar](1) NULL,
	[Legacy_ID] [uniqueidentifier] NOT NULL,
	[DateOfExpiryByLaw] [datetime] NOT NULL,
	[Matched] [bit] NOT NULL,
	[MatchedUser] [nvarchar](50) NULL,
	[MatchedTime] [datetime] NULL,
	[Tdl] [bit] NOT NULL,
	[Recipient] [nvarchar](75) NULL,
	[IntDescription] [nvarchar](50) NULL,
	[Submission_ID] [int] NULL,
	[SubmitterCustomerNumber] [nvarchar](12) NULL,
	[Creditor_ID] [int] NULL,
	[Transaction_ID] [int] NULL,
 CONSTRAINT [PK_Voucher.Internals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[Items]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[Items](
	[Voucher_ID] [bigint] NOT NULL,
	[Article_ID] [smallint] NOT NULL,
	[Quality_ID] [smallint] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_Voucher.Items] PRIMARY KEY CLUSTERED 
(
	[Voucher_ID] ASC,
	[Article_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[ReasonTypes]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[ReasonTypes](
	[ID] [nvarchar](3) NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Voucher.ReasonTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[ReceiptStateHistories]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[ReceiptStateHistories](
	[Voucher_ID] [bigint] NOT NULL,
	[Type] [int] NOT NULL,
	[Occurence] [datetime] NOT NULL,
 CONSTRAINT [PK_Voucher.ReceiptStateHistories] PRIMARY KEY CLUSTERED 
(
	[Voucher_ID] ASC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[Reports]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[Reports](
	[Voucher_ID] [bigint] NOT NULL,
	[Report_ID] [int] NOT NULL,
 CONSTRAINT [PK_Voucher.Reports] PRIMARY KEY CLUSTERED 
(
	[Voucher_ID] ASC,
	[Report_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[StateLogs]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[StateLogs](
	[ID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Voucher_ID] [bigint] NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[User] [varchar](50) NOT NULL,
	[State_ID] [nvarchar](1) NULL,
 CONSTRAINT [PK_Voucher.StateLogs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[StateTypes]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[StateTypes](
	[ID] [nvarchar](1) NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Voucher.StateTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[Submissions]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[Submissions](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[DateOfReceipt] [datetime] NOT NULL,
	[SubmitterCustomerNumber] [nvarchar](13) NOT NULL,
	[Creditor_ID] [int] NOT NULL,
	[CreditorBookingDate] [datetime] NOT NULL,
	[Number] [nvarchar](12) NOT NULL,
	[IntDescription] [nvarchar](255) NULL,
	[ExtDescription] [nvarchar](255) NULL,
	[ExtNumber] [nvarchar](50) NULL,
	[Process_ID] [int] NULL,
	[Transaction_ID] [int] NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Voucher.Submissions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Voucher].[Vouchers]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Voucher].[Vouchers](
	[ID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Number] [nvarchar](50) NULL,
	[Issuer_ID] [int] NOT NULL,
	[IssueDate] [date] NOT NULL,
	[DateOfExpiry] [datetime] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[DeleteUser] [varchar](50) NULL,
	[DeleteTime] [datetime] NULL,
	[OptimisticLockField] [int] NOT NULL,
	[ReverseNumber]  AS (reverse([Number])) PERSISTED,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Voucher.Vouchers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Article_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Article_ID] ON [Condition].[BookingDependent]
(
	[Article_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ID] ON [Condition].[BookingDependent]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quality_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Quality_ID] ON [Condition].[BookingDependent]
(
	[Quality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [Condition].[ExceptionalOppositeSideAccounts]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Condition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Condition_ID] ON [Condition].[ExceptionalOppositeSideAccounts]
(
	[Condition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingType_ID] ON [Condition].[ExceptionalOppositeSideBookingTypes]
(
	[BookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Condition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Condition_ID] ON [Condition].[ExceptionalOppositeSideBookingTypes]
(
	[Condition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [Condition].[Fees]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Booking_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Booking_ID] ON [Condition].[Fees]
(
	[Booking_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Booking_ID_Condition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Booking_ID_Condition_ID] ON [Condition].[Fees]
(
	[Booking_ID] ASC,
	[Condition_ID] ASC
)
WHERE ([DeleteTime] IS NULL AND [Booking_ID] IS NOT NULL AND [Condition_ID] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Booking_ID_Term_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Booking_ID_Term_ID] ON [Condition].[Fees]
(
	[Booking_ID] ASC,
	[Term_ID] ASC
)
WHERE ([DeleteTime] IS NULL AND [Condition_ID] IS NULL AND [Booking_ID] IS NOT NULL AND [Term_ID] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Condition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Condition_ID] ON [Condition].[Fees]
(
	[Condition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Term_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Term_ID] ON [Condition].[Fees]
(
	[Term_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [Condition].[OnlyValidForOppositeSideAccounts]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Condition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Condition_ID] ON [Condition].[OnlyValidForOppositeSideAccounts]
(
	[Condition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingType_ID] ON [Condition].[OnlyValidForOppositeSideBookingTypes]
(
	[BookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Condition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Condition_ID] ON [Condition].[OnlyValidForOppositeSideBookingTypes]
(
	[Condition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ID] ON [Condition].[Swaps]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[AccountAssociations]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Associated_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Associated_ID] ON [dbo].[AccountAssociations]
(
	[Associated_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[AccountContacts]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[AccountDetails]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AccountDetails_FieldName]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountDetails_FieldName] ON [dbo].[AccountDetails]
(
	[FieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AccountDetails_FieldValue]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountDetails_FieldValue] ON [dbo].[AccountDetails]
(
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AccountDetails_FNFV]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountDetails_FNFV] ON [dbo].[AccountDetails]
(
	[FieldName] ASC,
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountHierarchy_Lft]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountHierarchy_Lft] ON [dbo].[AccountHierarchy]
(
	[Lft] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountHierarchy_Parent_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountHierarchy_Parent_ID] ON [dbo].[AccountHierarchy]
(
	[Parent_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountHierarchy_Rgh]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountHierarchy_Rgh] ON [dbo].[AccountHierarchy]
(
	[Rgt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountHierarchy_RowNumer]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountHierarchy_RowNumer] ON [dbo].[AccountHierarchy]
(
	[RowNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountHierarchy_TreeLevel]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountHierarchy_TreeLevel] ON [dbo].[AccountHierarchy]
(
	[TreeLevel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ID] ON [dbo].[AccountHierarchy]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Accounts_AccountNumber_notnull]    Script Date: 29/01/2020 11:39:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Accounts_AccountNumber_notnull] ON [dbo].[Accounts]
(
	[AccountNumber] ASC
)
WHERE ([AccountNumber] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AccountType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountType_ID] ON [dbo].[Accounts]
(
	[AccountType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_FullName]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_FullName] ON [dbo].[Accounts]
(
	[FullName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceAccount_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceAccount_ID] ON [dbo].[Accounts]
(
	[InvoiceAccount_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_KeyAccountManager_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_KeyAccountManager_ID] ON [dbo].[Accounts]
(
	[KeyAccountManager_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Name]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Name] ON [dbo].[Accounts]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Parent_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Parent_ID] ON [dbo].[Accounts]
(
	[Parent_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_PathName]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_PathName] ON [dbo].[Accounts]
(
	[PathName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ResponsiblePerson_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ResponsiblePerson_ID] ON [dbo].[Accounts]
(
	[ResponsiblePerson_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Salesperson_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Salesperson_ID] ON [dbo].[Accounts]
(
	[Salesperson_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ID] ON [dbo].[AccountSyncStates]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_RowId]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_RowId] ON [dbo].[BatchQueries]
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RowValue]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_RowValue] ON [dbo].[BatchQueries]
(
	[RowValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Booking_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Booking_ID] ON [dbo].[BookingDetails]
(
	[Booking_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingDetails_FieldName]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingDetails_FieldName] ON [dbo].[BookingDetails]
(
	[FieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingDetails_FieldValue]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingDetails_FieldValue] ON [dbo].[BookingDetails]
(
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingDetails_FNFV]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingDetails_FNFV] ON [dbo].[BookingDetails]
(
	[FieldName] ASC,
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_BookingDate]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IDX_BookingDate] ON [dbo].[Bookings]
(
	[BookingDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_Bookings_Transaction_ID_BookingType_ID_Article_ID_PlusIncludes]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IDX_Bookings_Transaction_ID_BookingType_ID_Article_ID_PlusIncludes] ON [dbo].[Bookings]
(
	[Account_ID] ASC,
	[Transaction_ID] ASC,
	[BookingType_ID] ASC,
	[Article_ID] ASC,
	[ReferenceNumber] ASC
)
INCLUDE([BookingDate],[DeleteTime],[IncludeInBalance],[Matched],[Quantity]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idx_Nonclustered_Bookings_BookingDate_Article_ID_Matched_Account_ID_DeleteUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [idx_Nonclustered_Bookings_BookingDate_Article_ID_Matched_Account_ID_DeleteUser] ON [dbo].[Bookings]
(
	[BookingDate] ASC,
	[Article_ID] ASC,
	[Matched] ASC,
	[Account_ID] ASC,
	[DeleteUser] ASC,
	[ReferenceNumber] ASC
)
INCLUDE([IncludeInBalance],[Transaction_ID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[Bookings]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountID_DeleteTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountID_DeleteTime] ON [dbo].[Bookings]
(
	[Account_ID] ASC,
	[DeleteTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Article_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Article_ID] ON [dbo].[Bookings]
(
	[Article_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [IX_Bookings_ChangedTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Bookings_ChangedTime] ON [dbo].[Bookings]
(
	[ChangedTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingType_ID] ON [dbo].[Bookings]
(
	[BookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreateTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_CreateTime] ON [dbo].[Bookings]
(
	[CreateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CreateUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_CreateUser] ON [dbo].[Bookings]
(
	[CreateUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_DeleteTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_DeleteTime] ON [dbo].[Bookings]
(
	[DeleteTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DeleteUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_DeleteUser] ON [dbo].[Bookings]
(
	[DeleteUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quality_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Quality_ID] ON [dbo].[Bookings]
(
	[Quality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReferenceNumber]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReferenceNumber] ON [dbo].[Bookings]
(
	[ReferenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_RowGuid]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_RowGuid] ON [dbo].[Bookings]
(
	[RowGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_RowModified]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_RowModified] ON [dbo].[Bookings]
(
	[RowModified] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_ID] ON [dbo].[Bookings]
(
	[Transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_UpdateTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UpdateTime] ON [dbo].[Bookings]
(
	[UpdateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UpdateUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UpdateUser] ON [dbo].[Bookings]
(
	[UpdateUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Accoung_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Accoung_ID] ON [dbo].[BookingSyncStates]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ID] ON [dbo].[BookingSyncStates]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SessionId]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_SessionId] ON [dbo].[BookingSyncStates]
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[Conditions]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Term_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Term_ID] ON [dbo].[Conditions]
(
	[Term_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImportPackage_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ImportPackage_ID] ON [dbo].[ImportAccountMappings]
(
	[ImportPackage_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ImportPackage_ID_Name1_Name2]    Script Date: 29/01/2020 11:39:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ImportPackage_ID_Name1_Name2] ON [dbo].[ImportAccountMappings]
(
	[ImportPackage_ID] ASC,
	[Name1] ASC,
	[Name2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Article_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Article_ID] ON [dbo].[ImportBookings]
(
	[Article_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImportTransaction_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ImportTransaction_ID] ON [dbo].[ImportBookings]
(
	[ImportTransaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImportTransaction_ID_IsSourceBooking_Article_ID_Quality_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ImportTransaction_ID_IsSourceBooking_Article_ID_Quality_ID] ON [dbo].[ImportBookings]
(
	[ImportTransaction_ID] ASC,
	[IsSourceBooking] ASC,
	[Article_ID] ASC,
	[Quality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quality_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Quality_ID] ON [dbo].[ImportBookings]
(
	[Quality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImportPackage_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ImportPackage_ID] ON [dbo].[ImportStacks]
(
	[ImportPackage_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DestinationName1_DestinationName2]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_DestinationName1_DestinationName2] ON [dbo].[ImportTransactions]
(
	[DestinationName1] ASC,
	[DestinationName2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImportStack_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ImportStack_ID] ON [dbo].[ImportTransactions]
(
	[ImportStack_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SourceName1_SourceName2]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_SourceName1_SourceName2] ON [dbo].[ImportTransactions]
(
	[SourceName1] ASC,
	[SourceName2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_ID] ON [dbo].[ImportTransactions]
(
	[Transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[InvoiceAccounts]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invoice_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Invoice_ID] ON [dbo].[InvoiceAccounts]
(
	[Invoice_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Fee_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Fee_ID] ON [dbo].[InvoiceFees]
(
	[Fee_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invoice_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Invoice_ID] ON [dbo].[InvoiceFees]
(
	[Invoice_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invoice_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Invoice_ID] ON [dbo].[InvoiceQualities]
(
	[Invoice_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quality_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Quality_ID] ON [dbo].[InvoiceQualities]
(
	[Quality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Article_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Article_ID] ON [dbo].[Invoices]
(
	[Article_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_PrimaryAccount_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_PrimaryAccount_ID] ON [dbo].[Invoices]
(
	[PrimaryAccount_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReportType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReportType_ID] ON [dbo].[Invoices]
(
	[ReportType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Article_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Article_ID] ON [dbo].[Pallet]
(
	[Article_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quality_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Quality_ID] ON [dbo].[Pallet]
(
	[Quality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_PalletChangeDefinition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_PalletChangeDefinition_ID] ON [dbo].[PalletChangeDefinitionDetails]
(
	[PalletChangeDefinition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[PalletChangeDefinitions]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AccountDirection_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountDirection_ID] ON [dbo].[PalletChangeDefinitions]
(
	[AccountDirection_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingTypeFrom_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingTypeFrom_ID] ON [dbo].[PalletChangeDefinitions]
(
	[BookingTypeFrom_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingTypeTo_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingTypeTo_ID] ON [dbo].[PalletChangeDefinitions]
(
	[BookingTypeTo_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_OppositeAccout_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_OppositeAccout_ID] ON [dbo].[PalletChangeDefinitions]
(
	[OppositeAccout_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_PalletChangeCopyDefinition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_PalletChangeCopyDefinition_ID] ON [dbo].[PalletChangeDefinitions]
(
	[PalletChangeCopyDefinition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Set_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Set_ID] ON [dbo].[PalletChangeDefinitions]
(
	[Set_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChangeFrom_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ChangeFrom_ID] ON [dbo].[PalletChanges]
(
	[ChangeFrom_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChangeTo_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ChangeTo_ID] ON [dbo].[PalletChanges]
(
	[ChangeTo_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_PalletChangeSet_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_PalletChangeSet_ID] ON [dbo].[PalletChanges]
(
	[PalletChangeSet_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_PalletChangeDefinition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_PalletChangeDefinition_ID] ON [dbo].[PalletChangeTriggerPalletChangeDefinitions]
(
	[PalletChangeDefinition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_PalletChangeTrigger_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_PalletChangeTrigger_ID] ON [dbo].[PalletChangeTriggerPalletChangeDefinitions]
(
	[PalletChangeTrigger_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_AccountOppositeSide_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountOppositeSide_ID] ON [dbo].[PalletChangeTriggers]
(
	[AccountOppositeSide_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingType_ID] ON [dbo].[PalletChangeTriggers]
(
	[BookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingTypeOppositeSide_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingTypeOppositeSide_ID] ON [dbo].[PalletChangeTriggers]
(
	[BookingTypeOppositeSide_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Process_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Process_ID] ON [dbo].[ProcessDetails]
(
	[Process_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ProcessDetails_FieldName]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ProcessDetails_FieldName] ON [dbo].[ProcessDetails]
(
	[FieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ProcessDetails_FieldValue]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ProcessDetails_FieldValue] ON [dbo].[ProcessDetails]
(
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ProcessDetails_FNFV]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ProcessDetails_FNFV] ON [dbo].[ProcessDetails]
(
	[FieldName] ASC,
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreateTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_CreateTime] ON [dbo].[Processes]
(
	[CreateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CreateUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_CreateUser] ON [dbo].[Processes]
(
	[CreateUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_DeleteTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_DeleteTime] ON [dbo].[Processes]
(
	[DeleteTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DeleteUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_DeleteUser] ON [dbo].[Processes]
(
	[DeleteUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [IX_Processes_ChangedTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Processes_ChangedTime] ON [dbo].[Processes]
(
	[ChangedTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProcessState_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ProcessState_ID] ON [dbo].[Processes]
(
	[ProcessState_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProcessType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ProcessType_ID] ON [dbo].[Processes]
(
	[ProcessType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReferenceNumber]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReferenceNumber] ON [dbo].[Processes]
(
	[ReferenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_UpdateTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UpdateTime] ON [dbo].[Processes]
(
	[UpdateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UpdateUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UpdateUser] ON [dbo].[Processes]
(
	[UpdateUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[ReportAccounts]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Report_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Report_ID] ON [dbo].[ReportAccounts]
(
	[Report_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Booking_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Booking_ID] ON [dbo].[ReportBookings]
(
	[Booking_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Report_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Report_ID] ON [dbo].[ReportBookings]
(
	[Report_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Report_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Report_ID] ON [dbo].[ReportDetails]
(
	[Report_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReportDetails_FieldName]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReportDetails_FieldName] ON [dbo].[ReportDetails]
(
	[FieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReportDetails_FieldValue]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReportDetails_FieldValue] ON [dbo].[ReportDetails]
(
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReportDetails_FNFV]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReportDetails_FNFV] ON [dbo].[ReportDetails]
(
	[FieldName] ASC,
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_PrimaryAccount_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_PrimaryAccount_ID] ON [dbo].[Reports]
(
	[PrimaryAccount_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReportState_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReportState_ID] ON [dbo].[Reports]
(
	[ReportState_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReportType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReportType_ID] ON [dbo].[Reports]
(
	[ReportType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AccountType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_AccountType_ID] ON [dbo].[ReportTypeAccountTypes]
(
	[AccountType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReportType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReportType_ID] ON [dbo].[ReportTypeAccountTypes]
(
	[ReportType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Account_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Account_ID] ON [dbo].[TagAccounts]
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tag_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Tag_ID] ON [dbo].[TagAccounts]
(
	[Tag_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Tags_Unique]    Script Date: 29/01/2020 11:39:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Tags_Unique] ON [dbo].[Tags]
(
	[Name] ASC,
	[Role] ASC,
	[Owner] ASC,
	[Discriminator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BookingType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_BookingType_ID] ON [dbo].[Terms]
(
	[BookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ChargeWith_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ChargeWith_ID] ON [dbo].[Terms]
(
	[ChargeWith_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_SwapAccount_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_SwapAccount_ID] ON [dbo].[Terms]
(
	[SwapAccount_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SwapInBookingType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_SwapInBookingType_ID] ON [dbo].[Terms]
(
	[SwapInBookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_SwapInPallet_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_SwapInPallet_ID] ON [dbo].[Terms]
(
	[SwapInPallet_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SwapOutBookingType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_SwapOutBookingType_ID] ON [dbo].[Terms]
(
	[SwapOutBookingType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_SwapOutPallet_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_SwapOutPallet_ID] ON [dbo].[Terms]
(
	[SwapOutPallet_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_ID] ON [dbo].[TransactionDetails]
(
	[Transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TransactionDetails_FieldName]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionDetails_FieldName] ON [dbo].[TransactionDetails]
(
	[FieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TransactionDetails_FieldValue]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionDetails_FieldValue] ON [dbo].[TransactionDetails]
(
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TransactionDetails_FNFV]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionDetails_FNFV] ON [dbo].[TransactionDetails]
(
	[FieldName] ASC,
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cancellation_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Cancellation_ID] ON [dbo].[Transactions]
(
	[Cancellation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreateTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_CreateTime] ON [dbo].[Transactions]
(
	[CreateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CreateUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_CreateUser] ON [dbo].[Transactions]
(
	[CreateUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_DeleteTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_DeleteTime] ON [dbo].[Transactions]
(
	[DeleteTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DeleteUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_DeleteUser] ON [dbo].[Transactions]
(
	[DeleteUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Process_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Process_ID] ON [dbo].[Transactions]
(
	[Process_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ReferenceNumber]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReferenceNumber] ON [dbo].[Transactions]
(
	[ReferenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [IX_Transactions_ChangedTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Transactions_ChangedTime] ON [dbo].[Transactions]
(
	[ChangedTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionState_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionState_ID] ON [dbo].[Transactions]
(
	[TransactionState_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TransactionType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionType_ID] ON [dbo].[Transactions]
(
	[TransactionType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_UpdateTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UpdateTime] ON [dbo].[Transactions]
(
	[UpdateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UpdateUser]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UpdateUser] ON [dbo].[Transactions]
(
	[UpdateUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_User_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_User_ID] ON [dbo].[UserDetails]
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserDetails_FieldName]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UserDetails_FieldName] ON [dbo].[UserDetails]
(
	[FieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserDetails_FieldValue]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UserDetails_FieldValue] ON [dbo].[UserDetails]
(
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserDetails_FNFV]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_UserDetails_FNFV] ON [dbo].[UserDetails]
(
	[FieldName] ASC,
	[FieldValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IX_Condition_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Condition_ID] ON [Transaction].[Swaps]
(
	[Condition_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ID] ON [Transaction].[Swaps]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_InBooking_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_InBooking_ID] ON [Transaction].[Swaps]
(
	[InBooking_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OutBooking_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_OutBooking_ID] ON [Transaction].[Swaps]
(
	[OutBooking_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SwapAccount_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_SwapAccount_ID] ON [Transaction].[Swaps]
(
	[SwapAccount_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Term_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Term_ID] ON [Transaction].[Swaps]
(
	[Term_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Debitor_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Debitor_ID] ON [Voucher].[Debits]
(
	[Debitor_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DebitType_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_DebitType_ID] ON [Voucher].[Debits]
(
	[DebitType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_ID] ON [Voucher].[Debits]
(
	[Transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Debit_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Debit_ID] ON [Voucher].[Externals]
(
	[Debit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ID] ON [Voucher].[Externals]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Submission_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Submission_ID] ON [Voucher].[Externals]
(
	[Submission_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Creditor_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Creditor_ID] ON [Voucher].[Internals]
(
	[Creditor_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ID] ON [Voucher].[Internals]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Submission_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Submission_ID] ON [Voucher].[Internals]
(
	[Submission_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_ID] ON [Voucher].[Internals]
(
	[Transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_VoucherReason_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_VoucherReason_ID] ON [Voucher].[Internals]
(
	[VoucherReason_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_VoucherState_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_VoucherState_ID] ON [Voucher].[Internals]
(
	[VoucherState_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_VoucherItems_VoucherID_ArticleID_QualityID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IDX_VoucherItems_VoucherID_ArticleID_QualityID] ON [Voucher].[Items]
(
	[Voucher_ID] ASC,
	[Article_ID] ASC,
	[Quality_ID] ASC
)
INCLUDE([Quantity]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Article_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Article_ID] ON [Voucher].[Items]
(
	[Article_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Quality_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Quality_ID] ON [Voucher].[Items]
(
	[Quality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Voucher_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Voucher_ID] ON [Voucher].[Items]
(
	[Voucher_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Voucher_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Voucher_ID] ON [Voucher].[ReceiptStateHistories]
(
	[Voucher_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Report_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Report_ID] ON [Voucher].[Reports]
(
	[Report_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Voucher_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Voucher_ID] ON [Voucher].[Reports]
(
	[Voucher_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_State_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_State_ID] ON [Voucher].[StateLogs]
(
	[State_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Voucher_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Voucher_ID] ON [Voucher].[StateLogs]
(
	[Voucher_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Creditor_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Creditor_ID] ON [Voucher].[Submissions]
(
	[Creditor_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Process_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Process_ID] ON [Voucher].[Submissions]
(
	[Process_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_ID] ON [Voucher].[Submissions]
(
	[Transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_Vouvhers_IssuerID_DeleteUser_IssueDate]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IDX_Vouvhers_IssuerID_DeleteUser_IssueDate] ON [Voucher].[Vouchers]
(
	[Issuer_ID] ASC,
	[DeleteUser] ASC,
	[IssueDate] ASC
)
INCLUDE([ID],[Number]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Issuer_ID]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Issuer_ID] ON [Voucher].[Vouchers]
(
	[Issuer_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_NumberIssuerIdDeleteTime]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_NumberIssuerIdDeleteTime] ON [Voucher].[Vouchers]
(
	[Number] ASC,
	[Issuer_ID] ASC,
	[DeleteTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [IX_ReverseNumber]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_ReverseNumber] ON [Voucher].[Vouchers]
(
	[ReverseNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vouchers_Issuer_ID_DeleteTime_81908]    Script Date: 29/01/2020 11:39:16 ******/
CREATE NONCLUSTERED INDEX [IX_Vouchers_Issuer_ID_DeleteTime_81908] ON [Voucher].[Vouchers]
(
	[Issuer_ID] ASC,
	[DeleteTime] ASC
)
INCLUDE([ID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [Condition].[Fees] ADD  DEFAULT ((1)) FOR [FeeType]
GO
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Accounts_RowGuid]  DEFAULT (newsequentialid()) FOR [RowGuid]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ('') FOR [PathName]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0)) FOR [Inactive]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0)) FOR [Locked]
GO
ALTER TABLE [dbo].[Articles] ADD  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [dbo].[Bookings] ADD  DEFAULT ((0)) FOR [OptimisticLockField]
GO
ALTER TABLE [dbo].[Bookings] ADD  DEFAULT (getdate()) FOR [RowModified]
GO
ALTER TABLE [dbo].[Bookings] ADD  DEFAULT ((0)) FOR [Computed]
GO
ALTER TABLE [dbo].[Bookings] ADD  DEFAULT (newsequentialid()) FOR [RowGuid]
GO
ALTER TABLE [dbo].[BookingSyncStates] ADD  DEFAULT ((0)) FOR [Account_ID]
GO
ALTER TABLE [dbo].[BookingTypes] ADD  DEFAULT ((0)) FOR [OneDayDelayForRent]
GO
ALTER TABLE [dbo].[BookingTypes] ADD  DEFAULT ((0)) FOR [IncludeDaysFreeRentCondition]
GO
ALTER TABLE [dbo].[Conditions] ADD  DEFAULT ((1)) FOR [InvoicingDay]
GO
ALTER TABLE [dbo].[Conditions] ADD  DEFAULT ('ConditionBookingDependent') FOR [Discriminator]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT ('') FOR [ReportType_ID]
GO
ALTER TABLE [dbo].[Pallet] ADD  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] ADD  DEFAULT ((0)) FOR [Automatically]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] ADD  DEFAULT ((0)) FOR [DeleteOrphaned]
GO
ALTER TABLE [dbo].[Processes] ADD  DEFAULT ((0)) FOR [OptimisticLockField]
GO
ALTER TABLE [dbo].[Processes] ADD  CONSTRAINT [DF_Processes_RowGuid]  DEFAULT (newid()) FOR [RowGuid]
GO
ALTER TABLE [dbo].[Qualities] ADD  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT ((0)) FOR [OptimisticLockField]
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT ('') FOR [ReportType_ID]
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT ((0)) FOR [PrimaryAccount_ID]
GO
ALTER TABLE [dbo].[Terms] ADD  DEFAULT ((0)) FOR [MatchAllBookingTypes]
GO
ALTER TABLE [dbo].[Terms] ADD  DEFAULT ('TermBookingDependent') FOR [Discriminator]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0)) FOR [TransactionState_ID]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0)) FOR [OptimisticLockField]
GO
ALTER TABLE [dbo].[TransactionStates] ADD  DEFAULT ((0)) FOR [Cancelable]
GO
ALTER TABLE [Voucher].[Submissions] ADD  CONSTRAINT [DF_Submissions_RowGuid]  DEFAULT (newid()) FOR [RowGuid]
GO
ALTER TABLE [Voucher].[Vouchers] ADD  CONSTRAINT [DF_Vouchers_RowGuid]  DEFAULT (newid()) FOR [RowGuid]
GO
ALTER TABLE [Condition].[BookingDependent]  WITH CHECK ADD  CONSTRAINT [FK_Condition.BookingDependent_dbo.Conditions_ID] FOREIGN KEY([ID])
REFERENCES [dbo].[Conditions] ([ID])
GO
ALTER TABLE [Condition].[BookingDependent] CHECK CONSTRAINT [FK_Condition.BookingDependent_dbo.Conditions_ID]
GO
ALTER TABLE [Condition].[ExceptionalOppositeSideAccounts]  WITH CHECK ADD  CONSTRAINT [FK_Condition.ExceptionalOppositeSideAccounts_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [Condition].[ExceptionalOppositeSideAccounts] CHECK CONSTRAINT [FK_Condition.ExceptionalOppositeSideAccounts_dbo.Accounts_Account_ID]
GO
ALTER TABLE [Condition].[ExceptionalOppositeSideAccounts]  WITH CHECK ADD  CONSTRAINT [FK_Condition.ExceptionalOppositeSideAccounts_dbo.Conditions_Condition_ID] FOREIGN KEY([Condition_ID])
REFERENCES [dbo].[Conditions] ([ID])
GO
ALTER TABLE [Condition].[ExceptionalOppositeSideAccounts] CHECK CONSTRAINT [FK_Condition.ExceptionalOppositeSideAccounts_dbo.Conditions_Condition_ID]
GO
ALTER TABLE [Condition].[ExceptionalOppositeSideBookingTypes]  WITH CHECK ADD  CONSTRAINT [FK_Condition.ExceptionalOppositeSideBookingTypes_dbo.BookingTypes_BookingType_ID] FOREIGN KEY([BookingType_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [Condition].[ExceptionalOppositeSideBookingTypes] CHECK CONSTRAINT [FK_Condition.ExceptionalOppositeSideBookingTypes_dbo.BookingTypes_BookingType_ID]
GO
ALTER TABLE [Condition].[ExceptionalOppositeSideBookingTypes]  WITH CHECK ADD  CONSTRAINT [FK_Condition.ExceptionalOppositeSideBookingTypes_dbo.Conditions_Condition_ID] FOREIGN KEY([Condition_ID])
REFERENCES [dbo].[Conditions] ([ID])
GO
ALTER TABLE [Condition].[ExceptionalOppositeSideBookingTypes] CHECK CONSTRAINT [FK_Condition.ExceptionalOppositeSideBookingTypes_dbo.Conditions_Condition_ID]
GO
ALTER TABLE [Condition].[Fees]  WITH CHECK ADD  CONSTRAINT [FK_Condition.Fees_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [Condition].[Fees] CHECK CONSTRAINT [FK_Condition.Fees_dbo.Accounts_Account_ID]
GO
ALTER TABLE [Condition].[Fees]  WITH CHECK ADD  CONSTRAINT [FK_Condition.Fees_dbo.Bookings_Booking_ID] FOREIGN KEY([Booking_ID])
REFERENCES [dbo].[Bookings] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [Condition].[Fees] CHECK CONSTRAINT [FK_Condition.Fees_dbo.Bookings_Booking_ID]
GO
ALTER TABLE [Condition].[Fees]  WITH CHECK ADD  CONSTRAINT [FK_Condition.Fees_dbo.Conditions_Condition_ID] FOREIGN KEY([Condition_ID])
REFERENCES [dbo].[Conditions] ([ID])
GO
ALTER TABLE [Condition].[Fees] CHECK CONSTRAINT [FK_Condition.Fees_dbo.Conditions_Condition_ID]
GO
ALTER TABLE [Condition].[Fees]  WITH CHECK ADD  CONSTRAINT [FK_Condition.Fees_dbo.Terms_Term_ID] FOREIGN KEY([Term_ID])
REFERENCES [dbo].[Terms] ([ID])
GO
ALTER TABLE [Condition].[Fees] CHECK CONSTRAINT [FK_Condition.Fees_dbo.Terms_Term_ID]
GO
ALTER TABLE [Condition].[OnlyValidForOppositeSideAccounts]  WITH CHECK ADD  CONSTRAINT [FK_Condition.OnlyValidForOppositeSideAccounts_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [Condition].[OnlyValidForOppositeSideAccounts] CHECK CONSTRAINT [FK_Condition.OnlyValidForOppositeSideAccounts_dbo.Accounts_Account_ID]
GO
ALTER TABLE [Condition].[OnlyValidForOppositeSideAccounts]  WITH CHECK ADD  CONSTRAINT [FK_Condition.OnlyValidForOppositeSideAccounts_dbo.Conditions_Condition_ID] FOREIGN KEY([Condition_ID])
REFERENCES [dbo].[Conditions] ([ID])
GO
ALTER TABLE [Condition].[OnlyValidForOppositeSideAccounts] CHECK CONSTRAINT [FK_Condition.OnlyValidForOppositeSideAccounts_dbo.Conditions_Condition_ID]
GO
ALTER TABLE [Condition].[OnlyValidForOppositeSideBookingTypes]  WITH CHECK ADD  CONSTRAINT [FK_Condition.OnlyValidForOppositeSideBookingTypes_dbo.BookingTypes_BookingType_ID] FOREIGN KEY([BookingType_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [Condition].[OnlyValidForOppositeSideBookingTypes] CHECK CONSTRAINT [FK_Condition.OnlyValidForOppositeSideBookingTypes_dbo.BookingTypes_BookingType_ID]
GO
ALTER TABLE [Condition].[OnlyValidForOppositeSideBookingTypes]  WITH CHECK ADD  CONSTRAINT [FK_Condition.OnlyValidForOppositeSideBookingTypes_dbo.Conditions_Condition_ID] FOREIGN KEY([Condition_ID])
REFERENCES [dbo].[Conditions] ([ID])
GO
ALTER TABLE [Condition].[OnlyValidForOppositeSideBookingTypes] CHECK CONSTRAINT [FK_Condition.OnlyValidForOppositeSideBookingTypes_dbo.Conditions_Condition_ID]
GO
ALTER TABLE [Condition].[Swaps]  WITH CHECK ADD  CONSTRAINT [FK_Condition.Swaps_dbo.Conditions_ID] FOREIGN KEY([ID])
REFERENCES [dbo].[Conditions] ([ID])
GO
ALTER TABLE [Condition].[Swaps] CHECK CONSTRAINT [FK_Condition.Swaps_dbo.Conditions_ID]
GO
ALTER TABLE [dbo].[AccountAssociations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AccountAssociations_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccountAssociations] CHECK CONSTRAINT [FK_dbo.AccountAssociations_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[AccountAssociations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AccountAssociations_dbo.Accounts_Associated_ID] FOREIGN KEY([Associated_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[AccountAssociations] CHECK CONSTRAINT [FK_dbo.AccountAssociations_dbo.Accounts_Associated_ID]
GO
ALTER TABLE [dbo].[AccountContacts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.AccountContacts_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccountContacts] CHECK CONSTRAINT [FK_dbo.AccountContacts_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[AccountDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AccountDetails_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccountDetails] CHECK CONSTRAINT [FK_dbo.AccountDetails_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[AccountHierarchy]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AccountHierarchy_dbo.Accounts_ID] FOREIGN KEY([ID])
REFERENCES [dbo].[Accounts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccountHierarchy] CHECK CONSTRAINT [FK_dbo.AccountHierarchy_dbo.Accounts_ID]
GO
ALTER TABLE [dbo].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Accounts_dbo.Accounts_InvoiceAccount_ID] FOREIGN KEY([InvoiceAccount_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_dbo.Accounts_dbo.Accounts_InvoiceAccount_ID]
GO
ALTER TABLE [dbo].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Accounts_dbo.Accounts_Parent_ID] FOREIGN KEY([Parent_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_dbo.Accounts_dbo.Accounts_Parent_ID]
GO
ALTER TABLE [dbo].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Accounts_dbo.AccountTypes_AccountType_ID] FOREIGN KEY([AccountType_ID])
REFERENCES [dbo].[AccountTypes] ([ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_dbo.Accounts_dbo.AccountTypes_AccountType_ID]
GO
ALTER TABLE [dbo].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Accounts_dbo.Users_KeyAccountManager_ID] FOREIGN KEY([KeyAccountManager_ID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_dbo.Accounts_dbo.Users_KeyAccountManager_ID]
GO
ALTER TABLE [dbo].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Accounts_dbo.Users_ResponsiblePerson_ID] FOREIGN KEY([ResponsiblePerson_ID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_dbo.Accounts_dbo.Users_ResponsiblePerson_ID]
GO
ALTER TABLE [dbo].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.Accounts_dbo.Users_Salesperson_ID] FOREIGN KEY([Salesperson_ID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_dbo.Accounts_dbo.Users_Salesperson_ID]
GO
ALTER TABLE [dbo].[AccountSyncStates]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AccountSyncStates_dbo.Accounts_ID] FOREIGN KEY([ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[AccountSyncStates] CHECK CONSTRAINT [FK_dbo.AccountSyncStates_dbo.Accounts_ID]
GO
ALTER TABLE [dbo].[BookingDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BookingDetails_dbo.Bookings_Booking_ID] FOREIGN KEY([Booking_ID])
REFERENCES [dbo].[Bookings] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookingDetails] CHECK CONSTRAINT [FK_dbo.BookingDetails_dbo.Bookings_Booking_ID]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Bookings_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_dbo.Bookings_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Bookings_dbo.Articles_Article_ID] FOREIGN KEY([Article_ID])
REFERENCES [dbo].[Articles] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_dbo.Bookings_dbo.Articles_Article_ID]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Bookings_dbo.BookingTypes_BookingType_ID] FOREIGN KEY([BookingType_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_dbo.Bookings_dbo.BookingTypes_BookingType_ID]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Bookings_dbo.Qualities_Quality_ID] FOREIGN KEY([Quality_ID])
REFERENCES [dbo].[Qualities] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_dbo.Bookings_dbo.Qualities_Quality_ID]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Bookings_dbo.Transactions_Transaction_ID] FOREIGN KEY([Transaction_ID])
REFERENCES [dbo].[Transactions] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_dbo.Bookings_dbo.Transactions_Transaction_ID]
GO
ALTER TABLE [dbo].[BookingSyncStates]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BookingSyncStates_dbo.Bookings_ID] FOREIGN KEY([ID])
REFERENCES [dbo].[Bookings] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookingSyncStates] CHECK CONSTRAINT [FK_dbo.BookingSyncStates_dbo.Bookings_ID]
GO
ALTER TABLE [dbo].[Conditions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Conditions_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Conditions] CHECK CONSTRAINT [FK_dbo.Conditions_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[Conditions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Conditions_dbo.Terms_Term_ID] FOREIGN KEY([Term_ID])
REFERENCES [dbo].[Terms] ([ID])
GO
ALTER TABLE [dbo].[Conditions] CHECK CONSTRAINT [FK_dbo.Conditions_dbo.Terms_Term_ID]
GO
ALTER TABLE [dbo].[ImportAccountMappings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ImportAccountMappings_dbo.ImportPackages_ImportPackage_ID] FOREIGN KEY([ImportPackage_ID])
REFERENCES [dbo].[ImportPackages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportAccountMappings] CHECK CONSTRAINT [FK_dbo.ImportAccountMappings_dbo.ImportPackages_ImportPackage_ID]
GO
ALTER TABLE [dbo].[ImportBookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ImportBookings_dbo.Articles_Article_ID] FOREIGN KEY([Article_ID])
REFERENCES [dbo].[Articles] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportBookings] CHECK CONSTRAINT [FK_dbo.ImportBookings_dbo.Articles_Article_ID]
GO
ALTER TABLE [dbo].[ImportBookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ImportBookings_dbo.ImportTransactions_ImportTransaction_ID] FOREIGN KEY([ImportTransaction_ID])
REFERENCES [dbo].[ImportTransactions] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportBookings] CHECK CONSTRAINT [FK_dbo.ImportBookings_dbo.ImportTransactions_ImportTransaction_ID]
GO
ALTER TABLE [dbo].[ImportBookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ImportBookings_dbo.Qualities_Quality_ID] FOREIGN KEY([Quality_ID])
REFERENCES [dbo].[Qualities] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportBookings] CHECK CONSTRAINT [FK_dbo.ImportBookings_dbo.Qualities_Quality_ID]
GO
ALTER TABLE [dbo].[ImportStacks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ImportStacks_dbo.ImportPackages_ImportPackage_ID] FOREIGN KEY([ImportPackage_ID])
REFERENCES [dbo].[ImportPackages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportStacks] CHECK CONSTRAINT [FK_dbo.ImportStacks_dbo.ImportPackages_ImportPackage_ID]
GO
ALTER TABLE [dbo].[ImportTransactions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ImportTransactions_dbo.ImportStacks_ImportStack_ID] FOREIGN KEY([ImportStack_ID])
REFERENCES [dbo].[ImportStacks] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportTransactions] CHECK CONSTRAINT [FK_dbo.ImportTransactions_dbo.ImportStacks_ImportStack_ID]
GO
ALTER TABLE [dbo].[ImportTransactions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ImportTransactions_dbo.Transactions_Transaction_ID] FOREIGN KEY([Transaction_ID])
REFERENCES [dbo].[Transactions] ([ID])
GO
ALTER TABLE [dbo].[ImportTransactions] CHECK CONSTRAINT [FK_dbo.ImportTransactions_dbo.Transactions_Transaction_ID]
GO
ALTER TABLE [dbo].[InvoiceAccounts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.InvoiceAccounts_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[InvoiceAccounts] CHECK CONSTRAINT [FK_dbo.InvoiceAccounts_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[InvoiceAccounts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.InvoiceAccounts_dbo.Invoices_Invoice_ID] FOREIGN KEY([Invoice_ID])
REFERENCES [dbo].[Invoices] ([ID])
GO
ALTER TABLE [dbo].[InvoiceAccounts] CHECK CONSTRAINT [FK_dbo.InvoiceAccounts_dbo.Invoices_Invoice_ID]
GO
ALTER TABLE [dbo].[InvoiceFees]  WITH CHECK ADD  CONSTRAINT [FK_dbo.InvoiceFees_Condition.Fees_Fee_ID] FOREIGN KEY([Fee_ID])
REFERENCES [Condition].[Fees] ([ID])
GO
ALTER TABLE [dbo].[InvoiceFees] CHECK CONSTRAINT [FK_dbo.InvoiceFees_Condition.Fees_Fee_ID]
GO
ALTER TABLE [dbo].[InvoiceFees]  WITH CHECK ADD  CONSTRAINT [FK_dbo.InvoiceFees_dbo.Invoices_Invoice_ID] FOREIGN KEY([Invoice_ID])
REFERENCES [dbo].[Invoices] ([ID])
GO
ALTER TABLE [dbo].[InvoiceFees] CHECK CONSTRAINT [FK_dbo.InvoiceFees_dbo.Invoices_Invoice_ID]
GO
ALTER TABLE [dbo].[InvoiceQualities]  WITH CHECK ADD  CONSTRAINT [FK_dbo.InvoiceQualities_dbo.Invoices_Invoice_ID] FOREIGN KEY([Invoice_ID])
REFERENCES [dbo].[Invoices] ([ID])
GO
ALTER TABLE [dbo].[InvoiceQualities] CHECK CONSTRAINT [FK_dbo.InvoiceQualities_dbo.Invoices_Invoice_ID]
GO
ALTER TABLE [dbo].[InvoiceQualities]  WITH CHECK ADD  CONSTRAINT [FK_dbo.InvoiceQualities_dbo.Qualities_Quality_ID] FOREIGN KEY([Quality_ID])
REFERENCES [dbo].[Qualities] ([ID])
GO
ALTER TABLE [dbo].[InvoiceQualities] CHECK CONSTRAINT [FK_dbo.InvoiceQualities_dbo.Qualities_Quality_ID]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Invoices_dbo.Accounts_PrimaryAccount_ID] FOREIGN KEY([PrimaryAccount_ID])
REFERENCES [dbo].[Accounts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_dbo.Invoices_dbo.Accounts_PrimaryAccount_ID]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Invoices_dbo.Articles_Article_ID] FOREIGN KEY([Article_ID])
REFERENCES [dbo].[Articles] ([ID])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_dbo.Invoices_dbo.Articles_Article_ID]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Invoices_dbo.ReportTypes_ReportType_ID] FOREIGN KEY([ReportType_ID])
REFERENCES [dbo].[ReportTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_dbo.Invoices_dbo.ReportTypes_ReportType_ID]
GO
ALTER TABLE [dbo].[Pallet]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Pallet_dbo.Articles_Article_ID] FOREIGN KEY([Article_ID])
REFERENCES [dbo].[Articles] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pallet] CHECK CONSTRAINT [FK_dbo.Pallet_dbo.Articles_Article_ID]
GO
ALTER TABLE [dbo].[Pallet]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Pallet_dbo.Qualities_Quality_ID] FOREIGN KEY([Quality_ID])
REFERENCES [dbo].[Qualities] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pallet] CHECK CONSTRAINT [FK_dbo.Pallet_dbo.Qualities_Quality_ID]
GO
ALTER TABLE [dbo].[PalletChangeDefinitionDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeDefinitionDetails_dbo.PalletChangeDefinitions_PalletChangeDefinition_ID] FOREIGN KEY([PalletChangeDefinition_ID])
REFERENCES [dbo].[PalletChangeDefinitions] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeDefinitionDetails] CHECK CONSTRAINT [FK_dbo.PalletChangeDefinitionDetails_dbo.PalletChangeDefinitions_PalletChangeDefinition_ID]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.AccountDirections_AccountDirection_ID] FOREIGN KEY([AccountDirection_ID])
REFERENCES [dbo].[AccountDirections] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.AccountDirections_AccountDirection_ID]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.Accounts_OppositeAccout_ID] FOREIGN KEY([OppositeAccout_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.Accounts_OppositeAccout_ID]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.BookingTypes_BookingTypeFrom_ID] FOREIGN KEY([BookingTypeFrom_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.BookingTypes_BookingTypeFrom_ID]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.BookingTypes_BookingTypeTo_ID] FOREIGN KEY([BookingTypeTo_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.BookingTypes_BookingTypeTo_ID]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.PalletChangeCopyDefinitions_PalletChangeCopyDefinition_ID] FOREIGN KEY([PalletChangeCopyDefinition_ID])
REFERENCES [dbo].[PalletChangeCopyDefinitions] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.PalletChangeCopyDefinitions_PalletChangeCopyDefinition_ID]
GO
ALTER TABLE [dbo].[PalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.PalletChangeSets_Set_ID] FOREIGN KEY([Set_ID])
REFERENCES [dbo].[PalletChangeSets] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeDefinitions_dbo.PalletChangeSets_Set_ID]
GO
ALTER TABLE [dbo].[PalletChanges]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChanges_dbo.Pallet_ChangeFrom_ID] FOREIGN KEY([ChangeFrom_ID])
REFERENCES [dbo].[Pallet] ([ID])
GO
ALTER TABLE [dbo].[PalletChanges] CHECK CONSTRAINT [FK_dbo.PalletChanges_dbo.Pallet_ChangeFrom_ID]
GO
ALTER TABLE [dbo].[PalletChanges]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChanges_dbo.Pallet_ChangeTo_ID] FOREIGN KEY([ChangeTo_ID])
REFERENCES [dbo].[Pallet] ([ID])
GO
ALTER TABLE [dbo].[PalletChanges] CHECK CONSTRAINT [FK_dbo.PalletChanges_dbo.Pallet_ChangeTo_ID]
GO
ALTER TABLE [dbo].[PalletChanges]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChanges_dbo.PalletChangeSets_PalletChangeSet_ID] FOREIGN KEY([PalletChangeSet_ID])
REFERENCES [dbo].[PalletChangeSets] ([ID])
GO
ALTER TABLE [dbo].[PalletChanges] CHECK CONSTRAINT [FK_dbo.PalletChanges_dbo.PalletChangeSets_PalletChangeSet_ID]
GO
ALTER TABLE [dbo].[PalletChangeTriggerPalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeTriggerPalletChangeDefinitions_dbo.PalletChangeDefinitions_PalletChangeDefinition_ID] FOREIGN KEY([PalletChangeDefinition_ID])
REFERENCES [dbo].[PalletChangeDefinitions] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeTriggerPalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeTriggerPalletChangeDefinitions_dbo.PalletChangeDefinitions_PalletChangeDefinition_ID]
GO
ALTER TABLE [dbo].[PalletChangeTriggerPalletChangeDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeTriggerPalletChangeDefinitions_dbo.PalletChangeTriggers_PalletChangeTrigger_ID] FOREIGN KEY([PalletChangeTrigger_ID])
REFERENCES [dbo].[PalletChangeTriggers] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeTriggerPalletChangeDefinitions] CHECK CONSTRAINT [FK_dbo.PalletChangeTriggerPalletChangeDefinitions_dbo.PalletChangeTriggers_PalletChangeTrigger_ID]
GO
ALTER TABLE [dbo].[PalletChangeTriggers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeTriggers_dbo.Accounts_AccountOppositeSide_ID] FOREIGN KEY([AccountOppositeSide_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeTriggers] CHECK CONSTRAINT [FK_dbo.PalletChangeTriggers_dbo.Accounts_AccountOppositeSide_ID]
GO
ALTER TABLE [dbo].[PalletChangeTriggers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeTriggers_dbo.BookingTypes_BookingType_ID] FOREIGN KEY([BookingType_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeTriggers] CHECK CONSTRAINT [FK_dbo.PalletChangeTriggers_dbo.BookingTypes_BookingType_ID]
GO
ALTER TABLE [dbo].[PalletChangeTriggers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PalletChangeTriggers_dbo.BookingTypes_BookingTypeOppositeSide_ID] FOREIGN KEY([BookingTypeOppositeSide_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [dbo].[PalletChangeTriggers] CHECK CONSTRAINT [FK_dbo.PalletChangeTriggers_dbo.BookingTypes_BookingTypeOppositeSide_ID]
GO
ALTER TABLE [dbo].[ProcessDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ProcessDetails_dbo.Processes_Process_ID] FOREIGN KEY([Process_ID])
REFERENCES [dbo].[Processes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProcessDetails] CHECK CONSTRAINT [FK_dbo.ProcessDetails_dbo.Processes_Process_ID]
GO
ALTER TABLE [dbo].[Processes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Processes_dbo.ProcessStates_ProcessState_ID] FOREIGN KEY([ProcessState_ID])
REFERENCES [dbo].[ProcessStates] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Processes] CHECK CONSTRAINT [FK_dbo.Processes_dbo.ProcessStates_ProcessState_ID]
GO
ALTER TABLE [dbo].[Processes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Processes_dbo.ProcessTypes_ProcessType_ID] FOREIGN KEY([ProcessType_ID])
REFERENCES [dbo].[ProcessTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Processes] CHECK CONSTRAINT [FK_dbo.Processes_dbo.ProcessTypes_ProcessType_ID]
GO
ALTER TABLE [dbo].[ReportAccounts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReportAccounts_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[ReportAccounts] CHECK CONSTRAINT [FK_dbo.ReportAccounts_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[ReportAccounts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReportAccounts_dbo.Reports_Report_ID] FOREIGN KEY([Report_ID])
REFERENCES [dbo].[Reports] ([ID])
GO
ALTER TABLE [dbo].[ReportAccounts] CHECK CONSTRAINT [FK_dbo.ReportAccounts_dbo.Reports_Report_ID]
GO
ALTER TABLE [dbo].[ReportBookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReportBookings_dbo.Bookings_Booking_ID] FOREIGN KEY([Booking_ID])
REFERENCES [dbo].[Bookings] ([ID])
GO
ALTER TABLE [dbo].[ReportBookings] CHECK CONSTRAINT [FK_dbo.ReportBookings_dbo.Bookings_Booking_ID]
GO
ALTER TABLE [dbo].[ReportBookings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReportBookings_dbo.Reports_Report_ID] FOREIGN KEY([Report_ID])
REFERENCES [dbo].[Reports] ([ID])
GO
ALTER TABLE [dbo].[ReportBookings] CHECK CONSTRAINT [FK_dbo.ReportBookings_dbo.Reports_Report_ID]
GO
ALTER TABLE [dbo].[ReportDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReportDetails_dbo.Reports_Report_ID] FOREIGN KEY([Report_ID])
REFERENCES [dbo].[Reports] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReportDetails] CHECK CONSTRAINT [FK_dbo.ReportDetails_dbo.Reports_Report_ID]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Reports_dbo.Accounts_PrimaryAccount_ID] FOREIGN KEY([PrimaryAccount_ID])
REFERENCES [dbo].[Accounts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_dbo.Reports_dbo.Accounts_PrimaryAccount_ID]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Reports_dbo.ReportStates_ReportState_ID] FOREIGN KEY([ReportState_ID])
REFERENCES [dbo].[ReportStates] ([ID])
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_dbo.Reports_dbo.ReportStates_ReportState_ID]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Reports_dbo.ReportTypes_ReportType_ID] FOREIGN KEY([ReportType_ID])
REFERENCES [dbo].[ReportTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_dbo.Reports_dbo.ReportTypes_ReportType_ID]
GO
ALTER TABLE [dbo].[ReportTypeAccountTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReportTypeAccountTypes_dbo.AccountTypes_AccountType_ID] FOREIGN KEY([AccountType_ID])
REFERENCES [dbo].[AccountTypes] ([ID])
GO
ALTER TABLE [dbo].[ReportTypeAccountTypes] CHECK CONSTRAINT [FK_dbo.ReportTypeAccountTypes_dbo.AccountTypes_AccountType_ID]
GO
ALTER TABLE [dbo].[ReportTypeAccountTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReportTypeAccountTypes_dbo.ReportTypes_ReportType_ID] FOREIGN KEY([ReportType_ID])
REFERENCES [dbo].[ReportTypes] ([ID])
GO
ALTER TABLE [dbo].[ReportTypeAccountTypes] CHECK CONSTRAINT [FK_dbo.ReportTypeAccountTypes_dbo.ReportTypes_ReportType_ID]
GO
ALTER TABLE [dbo].[TagAccounts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TagAccounts_dbo.Accounts_Account_ID] FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[TagAccounts] CHECK CONSTRAINT [FK_dbo.TagAccounts_dbo.Accounts_Account_ID]
GO
ALTER TABLE [dbo].[TagAccounts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TagAccounts_dbo.Tags_Tag_ID] FOREIGN KEY([Tag_ID])
REFERENCES [dbo].[Tags] ([ID])
GO
ALTER TABLE [dbo].[TagAccounts] CHECK CONSTRAINT [FK_dbo.TagAccounts_dbo.Tags_Tag_ID]
GO
ALTER TABLE [dbo].[Terms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Terms_dbo.Accounts_SwapAccount_ID] FOREIGN KEY([SwapAccount_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[Terms] CHECK CONSTRAINT [FK_dbo.Terms_dbo.Accounts_SwapAccount_ID]
GO
ALTER TABLE [dbo].[Terms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Terms_dbo.BookingTypes_BookingType_ID] FOREIGN KEY([BookingType_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [dbo].[Terms] CHECK CONSTRAINT [FK_dbo.Terms_dbo.BookingTypes_BookingType_ID]
GO
ALTER TABLE [dbo].[Terms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Terms_dbo.BookingTypes_SwapInBookingType_ID] FOREIGN KEY([SwapInBookingType_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [dbo].[Terms] CHECK CONSTRAINT [FK_dbo.Terms_dbo.BookingTypes_SwapInBookingType_ID]
GO
ALTER TABLE [dbo].[Terms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Terms_dbo.BookingTypes_SwapOutBookingType_ID] FOREIGN KEY([SwapOutBookingType_ID])
REFERENCES [dbo].[BookingTypes] ([ID])
GO
ALTER TABLE [dbo].[Terms] CHECK CONSTRAINT [FK_dbo.Terms_dbo.BookingTypes_SwapOutBookingType_ID]
GO
ALTER TABLE [dbo].[Terms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Terms_dbo.Pallet_SwapInPallet_ID] FOREIGN KEY([SwapInPallet_ID])
REFERENCES [dbo].[Pallet] ([ID])
GO
ALTER TABLE [dbo].[Terms] CHECK CONSTRAINT [FK_dbo.Terms_dbo.Pallet_SwapInPallet_ID]
GO
ALTER TABLE [dbo].[Terms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Terms_dbo.Pallet_SwapOutPallet_ID] FOREIGN KEY([SwapOutPallet_ID])
REFERENCES [dbo].[Pallet] ([ID])
GO
ALTER TABLE [dbo].[Terms] CHECK CONSTRAINT [FK_dbo.Terms_dbo.Pallet_SwapOutPallet_ID]
GO
ALTER TABLE [dbo].[Terms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Terms_dbo.Terms_ChargeWith_ID] FOREIGN KEY([ChargeWith_ID])
REFERENCES [dbo].[Terms] ([ID])
GO
ALTER TABLE [dbo].[Terms] CHECK CONSTRAINT [FK_dbo.Terms_dbo.Terms_ChargeWith_ID]
GO
ALTER TABLE [dbo].[TransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TransactionDetails_dbo.Transactions_Transaction_ID] FOREIGN KEY([Transaction_ID])
REFERENCES [dbo].[Transactions] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransactionDetails] CHECK CONSTRAINT [FK_dbo.TransactionDetails_dbo.Transactions_Transaction_ID]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transactions_dbo.Processes_Process_ID] FOREIGN KEY([Process_ID])
REFERENCES [dbo].[Processes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_dbo.Transactions_dbo.Processes_Process_ID]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transactions_dbo.Transactions_Cancellation_ID] FOREIGN KEY([Cancellation_ID])
REFERENCES [dbo].[Transactions] ([ID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_dbo.Transactions_dbo.Transactions_Cancellation_ID]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transactions_dbo.TransactionStates_TransactionState_ID] FOREIGN KEY([TransactionState_ID])
REFERENCES [dbo].[TransactionStates] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_dbo.Transactions_dbo.TransactionStates_TransactionState_ID]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transactions_dbo.TransactionTypes_TransactionType_ID] FOREIGN KEY([TransactionType_ID])
REFERENCES [dbo].[TransactionTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_dbo.Transactions_dbo.TransactionTypes_TransactionType_ID]
GO
ALTER TABLE [dbo].[UserDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserDetails_dbo.Users_User_ID] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserDetails] CHECK CONSTRAINT [FK_dbo.UserDetails_dbo.Users_User_ID]
GO
ALTER TABLE [Transaction].[Swaps]  WITH CHECK ADD  CONSTRAINT [FK_Transaction.Swaps_Condition.Swaps_Condition_ID] FOREIGN KEY([Condition_ID])
REFERENCES [Condition].[Swaps] ([ID])
GO
ALTER TABLE [Transaction].[Swaps] CHECK CONSTRAINT [FK_Transaction.Swaps_Condition.Swaps_Condition_ID]
GO
ALTER TABLE [Transaction].[Swaps]  WITH CHECK ADD  CONSTRAINT [FK_Transaction.Swaps_dbo.Accounts_SwapAccount_ID] FOREIGN KEY([SwapAccount_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [Transaction].[Swaps] CHECK CONSTRAINT [FK_Transaction.Swaps_dbo.Accounts_SwapAccount_ID]
GO
ALTER TABLE [Transaction].[Swaps]  WITH CHECK ADD  CONSTRAINT [FK_Transaction.Swaps_dbo.Bookings_InBooking_ID] FOREIGN KEY([InBooking_ID])
REFERENCES [dbo].[Bookings] ([ID])
GO
ALTER TABLE [Transaction].[Swaps] CHECK CONSTRAINT [FK_Transaction.Swaps_dbo.Bookings_InBooking_ID]
GO
ALTER TABLE [Transaction].[Swaps]  WITH CHECK ADD  CONSTRAINT [FK_Transaction.Swaps_dbo.Bookings_OutBooking_ID] FOREIGN KEY([OutBooking_ID])
REFERENCES [dbo].[Bookings] ([ID])
GO
ALTER TABLE [Transaction].[Swaps] CHECK CONSTRAINT [FK_Transaction.Swaps_dbo.Bookings_OutBooking_ID]
GO
ALTER TABLE [Transaction].[Swaps]  WITH CHECK ADD  CONSTRAINT [FK_Transaction.Swaps_dbo.Terms_Term_ID] FOREIGN KEY([Term_ID])
REFERENCES [dbo].[Terms] ([ID])
GO
ALTER TABLE [Transaction].[Swaps] CHECK CONSTRAINT [FK_Transaction.Swaps_dbo.Terms_Term_ID]
GO
ALTER TABLE [Transaction].[Swaps]  WITH CHECK ADD  CONSTRAINT [FK_Transaction.Swaps_dbo.Transactions_ID] FOREIGN KEY([ID])
REFERENCES [dbo].[Transactions] ([ID])
GO
ALTER TABLE [Transaction].[Swaps] CHECK CONSTRAINT [FK_Transaction.Swaps_dbo.Transactions_ID]
GO
ALTER TABLE [Voucher].[Debits]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Debits_dbo.Accounts_Debitor_ID] FOREIGN KEY([Debitor_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [Voucher].[Debits] CHECK CONSTRAINT [FK_Voucher.Debits_dbo.Accounts_Debitor_ID]
GO
ALTER TABLE [Voucher].[Debits]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Debits_dbo.Transactions_Transaction_ID] FOREIGN KEY([Transaction_ID])
REFERENCES [dbo].[Transactions] ([ID])
GO
ALTER TABLE [Voucher].[Debits] CHECK CONSTRAINT [FK_Voucher.Debits_dbo.Transactions_Transaction_ID]
GO
ALTER TABLE [Voucher].[Debits]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Debits_Voucher.DebitTypes_DebitType_ID] FOREIGN KEY([DebitType_ID])
REFERENCES [Voucher].[DebitTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [Voucher].[Debits] CHECK CONSTRAINT [FK_Voucher.Debits_Voucher.DebitTypes_DebitType_ID]
GO
ALTER TABLE [Voucher].[Externals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Externals_Voucher.Debits_Debit_ID] FOREIGN KEY([Debit_ID])
REFERENCES [Voucher].[Debits] ([ID])
GO
ALTER TABLE [Voucher].[Externals] CHECK CONSTRAINT [FK_Voucher.Externals_Voucher.Debits_Debit_ID]
GO
ALTER TABLE [Voucher].[Externals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Externals_Voucher.Submissions_Submission_ID] FOREIGN KEY([Submission_ID])
REFERENCES [Voucher].[Submissions] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [Voucher].[Externals] CHECK CONSTRAINT [FK_Voucher.Externals_Voucher.Submissions_Submission_ID]
GO
ALTER TABLE [Voucher].[Externals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Externals_Voucher.Vouchers_ID] FOREIGN KEY([ID])
REFERENCES [Voucher].[Vouchers] ([ID])
GO
ALTER TABLE [Voucher].[Externals] CHECK CONSTRAINT [FK_Voucher.Externals_Voucher.Vouchers_ID]
GO
ALTER TABLE [Voucher].[Internals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Internals_dbo.Accounts_Creditor_ID] FOREIGN KEY([Creditor_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [Voucher].[Internals] CHECK CONSTRAINT [FK_Voucher.Internals_dbo.Accounts_Creditor_ID]
GO
ALTER TABLE [Voucher].[Internals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Internals_dbo.Transactions_Transaction_ID] FOREIGN KEY([Transaction_ID])
REFERENCES [dbo].[Transactions] ([ID])
GO
ALTER TABLE [Voucher].[Internals] CHECK CONSTRAINT [FK_Voucher.Internals_dbo.Transactions_Transaction_ID]
GO
ALTER TABLE [Voucher].[Internals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Internals_Voucher.ReasonTypes_VoucherReason_ID] FOREIGN KEY([VoucherReason_ID])
REFERENCES [Voucher].[ReasonTypes] ([ID])
GO
ALTER TABLE [Voucher].[Internals] CHECK CONSTRAINT [FK_Voucher.Internals_Voucher.ReasonTypes_VoucherReason_ID]
GO
ALTER TABLE [Voucher].[Internals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Internals_Voucher.StateTypes_VoucherState_ID] FOREIGN KEY([VoucherState_ID])
REFERENCES [Voucher].[StateTypes] ([ID])
GO
ALTER TABLE [Voucher].[Internals] CHECK CONSTRAINT [FK_Voucher.Internals_Voucher.StateTypes_VoucherState_ID]
GO
ALTER TABLE [Voucher].[Internals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Internals_Voucher.Submissions_Submission_ID] FOREIGN KEY([Submission_ID])
REFERENCES [Voucher].[Submissions] ([ID])
GO
ALTER TABLE [Voucher].[Internals] CHECK CONSTRAINT [FK_Voucher.Internals_Voucher.Submissions_Submission_ID]
GO
ALTER TABLE [Voucher].[Internals]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Internals_Voucher.Vouchers_ID] FOREIGN KEY([ID])
REFERENCES [Voucher].[Vouchers] ([ID])
GO
ALTER TABLE [Voucher].[Internals] CHECK CONSTRAINT [FK_Voucher.Internals_Voucher.Vouchers_ID]
GO
ALTER TABLE [Voucher].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Items_dbo.Articles_Article_ID] FOREIGN KEY([Article_ID])
REFERENCES [dbo].[Articles] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [Voucher].[Items] CHECK CONSTRAINT [FK_Voucher.Items_dbo.Articles_Article_ID]
GO
ALTER TABLE [Voucher].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Items_dbo.Qualities_Quality_ID] FOREIGN KEY([Quality_ID])
REFERENCES [dbo].[Qualities] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [Voucher].[Items] CHECK CONSTRAINT [FK_Voucher.Items_dbo.Qualities_Quality_ID]
GO
ALTER TABLE [Voucher].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Items_Voucher.Vouchers_Voucher_ID] FOREIGN KEY([Voucher_ID])
REFERENCES [Voucher].[Vouchers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [Voucher].[Items] CHECK CONSTRAINT [FK_Voucher.Items_Voucher.Vouchers_Voucher_ID]
GO
ALTER TABLE [Voucher].[ReceiptStateHistories]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.ReceiptStateHistories_Voucher.Internals_Voucher_ID] FOREIGN KEY([Voucher_ID])
REFERENCES [Voucher].[Internals] ([ID])
GO
ALTER TABLE [Voucher].[ReceiptStateHistories] CHECK CONSTRAINT [FK_Voucher.ReceiptStateHistories_Voucher.Internals_Voucher_ID]
GO
ALTER TABLE [Voucher].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Reports_dbo.Reports_Report_ID] FOREIGN KEY([Report_ID])
REFERENCES [dbo].[Reports] ([ID])
GO
ALTER TABLE [Voucher].[Reports] CHECK CONSTRAINT [FK_Voucher.Reports_dbo.Reports_Report_ID]
GO
ALTER TABLE [Voucher].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Reports_Voucher.Vouchers_Voucher_ID] FOREIGN KEY([Voucher_ID])
REFERENCES [Voucher].[Vouchers] ([ID])
GO
ALTER TABLE [Voucher].[Reports] CHECK CONSTRAINT [FK_Voucher.Reports_Voucher.Vouchers_Voucher_ID]
GO
ALTER TABLE [Voucher].[StateLogs]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.StateLogs_Voucher.Internals_Voucher_ID] FOREIGN KEY([Voucher_ID])
REFERENCES [Voucher].[Internals] ([ID])
GO
ALTER TABLE [Voucher].[StateLogs] CHECK CONSTRAINT [FK_Voucher.StateLogs_Voucher.Internals_Voucher_ID]
GO
ALTER TABLE [Voucher].[StateLogs]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.StateLogs_Voucher.StateTypes_State_ID] FOREIGN KEY([State_ID])
REFERENCES [Voucher].[StateTypes] ([ID])
GO
ALTER TABLE [Voucher].[StateLogs] CHECK CONSTRAINT [FK_Voucher.StateLogs_Voucher.StateTypes_State_ID]
GO
ALTER TABLE [Voucher].[Submissions]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Submissions_dbo.Accounts_Creditor_ID] FOREIGN KEY([Creditor_ID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [Voucher].[Submissions] CHECK CONSTRAINT [FK_Voucher.Submissions_dbo.Accounts_Creditor_ID]
GO
ALTER TABLE [Voucher].[Submissions]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Submissions_dbo.Processes_Process_ID] FOREIGN KEY([Process_ID])
REFERENCES [dbo].[Processes] ([ID])
GO
ALTER TABLE [Voucher].[Submissions] CHECK CONSTRAINT [FK_Voucher.Submissions_dbo.Processes_Process_ID]
GO
ALTER TABLE [Voucher].[Submissions]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Submissions_dbo.Transactions_Transaction_ID] FOREIGN KEY([Transaction_ID])
REFERENCES [dbo].[Transactions] ([ID])
GO
ALTER TABLE [Voucher].[Submissions] CHECK CONSTRAINT [FK_Voucher.Submissions_dbo.Transactions_Transaction_ID]
GO
ALTER TABLE [Voucher].[Vouchers]  WITH CHECK ADD  CONSTRAINT [FK_Voucher.Vouchers_dbo.Accounts_Issuer_ID] FOREIGN KEY([Issuer_ID])
REFERENCES [dbo].[Accounts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [Voucher].[Vouchers] CHECK CONSTRAINT [FK_Voucher.Vouchers_dbo.Accounts_Issuer_ID]
GO
ALTER TABLE [Condition].[Fees]  WITH CHECK ADD  CONSTRAINT [CK_BookingIdOrAccountIdNotNull] CHECK  (([FeeType]=(1) AND [Booking_ID] IS NOT NULL OR [FeeType]=(2) AND [Account_ID] IS NOT NULL))
GO
ALTER TABLE [Condition].[Fees] CHECK CONSTRAINT [CK_BookingIdOrAccountIdNotNull]
GO
/****** Object:  StoredProcedure [dbo].[sp_account_is_subaccount_of]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 02.10.2012
-- Description:	Prüft ob ein Konto (@subordinate) in der Hierarchie ein Nachfolger eines anderen Kontos (@superior) ist
-- =============================================
CREATE PROCEDURE [dbo].[sp_account_is_subaccount_of]
    @superior INT,
    @subordinate INT,
    @issubaccount BIT OUTPUT
AS 
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

       --DECLARE @is_subaccount BIT
       --SET @is_subaccount = 0

        IF EXISTS ( SELECT  *
                    FROM    [dbo].[fn_query_single_path_lr](@subordinate)
                    WHERE   ID = @superior ) 
            SELECT @issubaccount = 1;
      
        ELSE 
            SELECT @issubaccount = 0;
              	
        RETURN @issubaccount
    END
GO
/****** Object:  StoredProcedure [dbo].[sp_clone_transaction]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:  <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_clone_transaction]
    -- Add the parameters for the stored procedure here
    @TID INT,
    @PID INT = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- clone Transaction
    INSERT INTO dbo.Transactions (RowGuid,
                                  TransactionType_ID,
                                  ReferenceNumber,
                                  Valuta,
                                  IntDescription,
                                  ExtDescription,
                                  Process_ID,
                                  CreateUser,
                                  CreateTime,
                                  UpdateUser,
                                  UpdateTime,
                                  TransactionState_ID,
                                  Cancellation_ID,
                                  DeleteUser,
                                  DeleteTime,
                                  OptimisticLockField)
    SELECT NEWID(),
           TransactionType_ID,
           ReferenceNumber,
           Valuta,
           IntDescription,
           ExtDescription,
           COALESCE(@PID, Process_ID),
           CreateUser,
           CreateTime,
           UpdateUser,
           UpdateTime,
           IIF(TransactionState_ID = 4, 1, TransactionState_ID),
           NULL,
           DeleteUser,
           DeleteTime,
           OptimisticLockField
      FROM dbo.Transactions
     WHERE ID = @TID;

    DECLARE @TID_NEW INT = @@IDENTITY;

    -- clone TransactionDetails
    INSERT INTO dbo.TransactionDetails (FieldName,
                                        FieldValue,
                                        OptimisticLockField,
                                        Transaction_ID,
                                        FieldDate,
                                        FieldInt,
                                        FieldDecimal,
                                        FieldBool,
                                        FieldText,
                                        FieldGuid,
                                        FieldType)
    SELECT FieldName,
           FieldValue,
           OptimisticLockField,
           @TID_NEW,
           FieldDate,
           FieldInt,
           FieldDecimal,
           FieldBool,
           FieldText,
           FieldGuid,
           FieldType
      FROM dbo.TransactionDetails
     WHERE Transaction_ID = @TID;

    -------------------------------------------------------------------------------------------------------------------------------    
    DECLARE @T1 TABLE (
        ROWNR INT,
        BID INT);

    INSERT INTO @T1
    SELECT ROW_NUMBER() OVER (ORDER BY B.ID) ROWNR,
           B.ID BID
      FROM dbo.Bookings B
     WHERE Transaction_ID = @TID;

    DECLARE @C INT = 1;
    DECLARE @CMAX INT;

    DECLARE @BID INT = -1;
    DECLARE @BID_NEW INT = -1;

    SELECT @CMAX = COUNT(*)
      FROM @T1;

    WHILE (@C <= @CMAX)
    BEGIN

        SELECT @BID = BID
          FROM @T1
         WHERE ROWNR = @C;

        INSERT INTO dbo.Bookings (BookingType_ID,
                                  ReferenceNumber,
                                  BookingDate,
                                  ExtDescription,
                                  Quantity,
                                  Article_ID,
                                  Quality_ID,
                                  IncludeInBalance,
                                  Matched,
                                  CreateUser,
                                  CreateTime,
                                  UpdateUser,
                                  UpdateTime,
                                  Transaction_ID,
                                  Account_ID,
                                  MatchedUser,
                                  MatchedTime,
                                  AccountDirection,
                                  DeleteUser,
                                  DeleteTime,
                                  OptimisticLockField,
                                  RowModified)
        SELECT BookingType_ID,
               ReferenceNumber,
               BookingDate,
               ExtDescription,
               Quantity,
               Article_ID,
               Quality_ID,
               IncludeInBalance,
               Matched,
               CreateUser,
               CreateTime,
               UpdateUser,
               UpdateTime,
               @TID_NEW,
               Account_ID,
               MatchedUser,
               MatchedTime,
               AccountDirection,
               DeleteUser,
               DeleteTime,
               OptimisticLockField,
               RowModified
          FROM dbo.Bookings
         WHERE ID = @BID;

        SET @BID_NEW = @@IDENTITY;

        INSERT INTO dbo.BookingDetails (FieldName,
                                        FieldValue,
                                        OptimisticLockField,
                                        Booking_ID,
                                        FieldDate,
                                        FieldInt,
                                        FieldDecimal,
                                        FieldBool,
                                        FieldText,
                                        FieldGuid,
                                        FieldType)
        SELECT FieldName,
               FieldValue,
               OptimisticLockField,
               @BID_NEW,
               FieldDate,
               FieldInt,
               FieldDecimal,
               FieldBool,
               FieldText,
               FieldGuid,
               FieldType
          FROM dbo.BookingDetails
         WHERE Booking_ID = @BID;

        SET @C = @C + 1;

    END;

    RETURN @TID_NEW;
END;


GO
/****** Object:  StoredProcedure [dbo].[sp_create_vbookingspivot]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_create_vbookingspivot]
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

DECLARE @ArticleQualityCombinations nvarchar(4000)
SELECT @ArticleQualityCombinations = N''

SELECT @ArticleQualityCombinations = @ArticleQualityCombinations + ', [' + ArticleQuality +']'
FROM
   (
   SELECT DISTINCT TOP 1000 p.Name AS ArticleQuality
   FROM Pallet AS p
   ORDER BY p.Name
   ) src

SET @ArticleQualityCombinations = SUBSTRING(@ArticleQualityCombinations, 3, LEN(@ArticleQualityCombinations))
--SELECT @ArticleQualityCombinations

DECLARE @ArticleQualityValuesCombinations nvarchar(4000)
SET @ArticleQualityValuesCombinations = N''

SELECT @ArticleQualityValuesCombinations = @ArticleQualityValuesCombinations + ', ISNULL([' + ArticleQuality +'], 0) AS [' + ArticleQuality +']'
FROM
   (
   SELECT DISTINCT TOP 1000 p.Name AS ArticleQuality
   FROM Pallet AS p
   ORDER BY p.Name
   ) src

SET @ArticleQualityValuesCombinations = SUBSTRING(@ArticleQualityValuesCombinations, 3, LEN(@ArticleQualityValuesCombinations))
--SELECT @ArticleQualityValuesCombinations

-- H1 Umbiegung 1A -> neu, 1A werden als neu angezeigt
SET @ArticleQualityValuesCombinations = REPLACE(@ArticleQualityValuesCombinations, 'ISNULL([H1_neu], 0) AS [H1_neu]', 'ISNULL([H1_1A], 0) AS [H1_neu]')
--SELECT @ArticleQualityValuesCombinations
SET @ArticleQualityCombinations = REPLACE(@ArticleQualityCombinations, '[H1_neu]', '[H1_1A]')
--SELECT @ArticleQualityCombinations

DECLARE @sql nvarchar(4000)

SET @sql = 'DROP VIEW [dbo].[vBookingsPivot]'
IF OBJECT_ID ('[dbo].[vBookingsPivot]', 'V') IS NOT NULL EXEC (@sql)

SET @sql =
   N'CREATE VIEW [dbo].[vBookingsPivot]
   AS
      (
      SELECT
            [ID]
            , BookingType_ID
            , ReferenceNumber
            , BookingDate
            , ExtDescription
            , IncludeInBalance
            , Matched
            , MatchedUser
            , MatchedTime
            , CreateUser
            , CreateTime
            , UpdateUser
            , UpdateTime
            , DeleteUser
            , DeleteTime
            , Account_ID
            , Transaction_ID
            , ' + @ArticleQualityValuesCombinations +
         ' FROM
         (SELECT
            b.[ID]
            , b.BookingType_ID
            , b.ReferenceNumber
            , b.BookingDate
            , b.ExtDescription
            , b.IncludeInBalance
            , b.Matched
            , b.MatchedUser
            , b.MatchedTime
            , b.CreateUser
            , b.CreateTime
            , b.UpdateUser
            , b.UpdateTime
            , b.DeleteUser
            , b.DeleteTime
            , b.Account_ID
            , b.Transaction_ID
            , a.Name + ''_'' + q.Name AS ArticleQuality
            , b.Quantity
         FROM Bookings AS b
            LEFT JOIN Articles AS a ON a.ID = b.Article_ID
            LEFT JOIN Qualities AS q ON q.ID = b.Quality_ID
         ) src
      PIVOT
         (SUM(Quantity) FOR ArticleQuality IN (' + @ArticleQualityCombinations + ')) p)
'

--SELECT @sql
EXEC (@sql)

END;

GO
/****** Object:  StoredProcedure [dbo].[sp_create_vbookingspivotAlt]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_create_vbookingspivotAlt] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @ArticleQualityCombinations nvarchar(4000)
SELECT @ArticleQualityCombinations = N''

SELECT @ArticleQualityCombinations = @ArticleQualityCombinations + ', [' + ArticleQuality +']'
FROM
   (
   SELECT DISTINCT TOP 1000 a.Name + '_' + q.Name AS ArticleQuality 
   FROM Bookings AS b
      LEFT JOIN Articles AS a ON a.ID = b.Article_ID
      LEFT JOIN Qualities AS q ON q.ID = b.Quality_ID
   ORDER BY a.Name + '_' + q.Name
   ) src

SET @ArticleQualityCombinations = SUBSTRING(@ArticleQualityCombinations, 3, LEN(@ArticleQualityCombinations))
--SELECT @ArticleQualityCombinations

DECLARE @ArticleQualityValuesCombinations nvarchar(4000)
SET @ArticleQualityValuesCombinations = N''

SELECT @ArticleQualityValuesCombinations = @ArticleQualityValuesCombinations + ', ISNULL([' + ArticleQuality +'], 0) AS [' + ArticleQuality +']'
FROM
   (
   SELECT DISTINCT TOP 1000 a.Name + '_' + q.Name AS ArticleQuality 
   FROM Bookings AS b
      LEFT JOIN Articles AS a ON a.ID = b.Article_ID
      LEFT JOIN Qualities AS q ON q.ID = b.Quality_ID
   ORDER BY a.Name + '_' + q.Name
   ) src

SET @ArticleQualityValuesCombinations = SUBSTRING(@ArticleQualityValuesCombinations, 3, LEN(@ArticleQualityValuesCombinations))
--SELECT @ArticleQualityValuesCombinations

DECLARE @sql nvarchar(4000)

SET @sql = 'DROP VIEW [dbo].[vBookingsPivot]'
IF OBJECT_ID ('[dbo].[vBookingsPivot]', 'V') IS NOT NULL EXEC (@sql)

SET @sql = 
   N'CREATE VIEW [dbo].[vBookingsPivot]
   AS
      (
      SELECT
            [ID]
            , BookingType_ID
            , ReferenceNumber
            , BookingDate
            , IntDescription
            , ExtDescription
            , Amount
            , IncludeInBalance
            , Matched
			   , MatchedUser
			   , MatchedTime
            , CreateUser
            , CreateTime
            , UpdateUser
            , UpdateTime
            , Account_ID
            , Transaction_ID
            , ' + @ArticleQualityValuesCombinations + 
         ' FROM 
         (SELECT 
            b.[ID]
            , b.BookingType_ID
            , b.ReferenceNumber
            , b.BookingDate
            , b.IntDescription
            , b.ExtDescription
            , b.Amount
            , b.IncludeInBalance
            , b.Matched
			   , b.MatchedUser
			   , b.MatchedTime
            , b.CreateUser
            , b.CreateTime
            , b.UpdateUser
            , b.UpdateTime
            , b.Account_ID
            , b.Transaction_ID
            , a.Name + ''_'' + q.Name AS ArticleQuality
            , b.Quantity
         FROM Bookings AS b
            LEFT JOIN Articles AS a ON a.ID = b.Article_ID
            LEFT JOIN Qualities AS q ON q.ID = b.Quality_ID
         ) src 
      PIVOT 
         (SUM(Quantity) FOR ArticleQuality IN (' + @ArticleQualityCombinations + ')) p
      )' 

--SELECT @sql
EXEC (@sql)

END;
GO
/****** Object:  StoredProcedure [dbo].[sp_find_clearing_definition_id]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	 Alexander Schlicht
-- Create date: 12.05.2015
-- Description: Abfrage der Clearing ID
-- =============================================
CREATE PROCEDURE [dbo].[sp_find_clearing_definition_id]
	-- Add the parameters for the stored procedure here
	   -- Add the parameters for the function here
    @fromAccountId INT ,
    @fromBookingTypeId VARCHAR(6) ,
    @accountDirection NVARCHAR(1) ,
    @toAccountId INT ,
    @toBookingTypeId VARCHAR(6) ,
    @clearingId INT OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @oppHID HIERARCHYID;
        SELECT  @oppHID = dbo.AccountHierarchy.HID
        FROM    dbo.AccountHierarchy
        WHERE   ID = @toAccountId;

    -- Insert statements for procedure here
        SET @clearingId = ( SELECT TOP ( 1 )
                                    PCD.ID
                            FROM    dbo.AccountHierarchy AS a1
                                    INNER JOIN dbo.AccountHierarchy AS a2 ON a2.HID.IsDescendantOf(a1.HID) = 1
                                    JOIN PalletChangeDefinitions PCD ON a1.ID = PCD.Account_ID
                                                              AND PCD.AccountDirection_ID = @accountDirection
                            WHERE   a2.ID = @fromAccountId
                                    AND PCD.Discriminator = 1
                                    AND ( EXISTS ( SELECT   1 AS [C1]
                                                   FROM     [dbo].[PalletChangeTriggerPalletChangeDefinitions]
                                                            AS PCDT
                                                            INNER JOIN [dbo].[PalletChangeTriggers]
                                                            AS [PCT] ON [PCT].[ID] = PCDT.[PalletChangeTrigger_ID]
                                                   WHERE    PCD.[ID] = PCDT.[PalletChangeDefinition_ID]
                                                            AND [PCT].[BookingType_ID] = @fromBookingTypeId
                                                            AND ( [PCT].[BookingTypeOppositeSide_ID] IS NULL
                                                              OR [PCT].[BookingTypeOppositeSide_ID] = @toBookingTypeId
                                                              )
                                                            AND ( [PCT].[AccountOppositeSide_ID] IS NULL
                                                              OR @oppHID.IsDescendantOf(( SELECT
                                                              HID
                                                              FROM
                                                              dbo.AccountHierarchy
                                                              WHERE
                                                              ID = [PCT].[AccountOppositeSide_ID]
                                                              )) = 1
                                                              ) ) )
                            ORDER BY a1.TreeLevel DESC
                          );
                         

	

	-- Return the result of the function
        RETURN @clearingId;
    END;

GO
/****** Object:  StoredProcedure [dbo].[sp_index_neuerstellungalle]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 11.05.2009
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_index_neuerstellungalle]
	
AS
BEGIN
    SET NOCOUNT ON ;

    
    DECLARE @TableName varchar(255) 

    DECLARE TableCursor CURSOR
        FOR SELECT  table_schema + '.' + table_name
            FROM    information_schema.tables
            WHERE   table_type = 'base table'
                    AND table_schema in ( 'dbo' )

    OPEN TableCursor 

    FETCH NEXT FROM TableCursor INTO @TableName 
    WHILE @@FETCH_STATUS = 0 
        BEGIN 
            PRINT @TableName
            DBCC DBREINDEX(@TableName,' ',90) 
            FETCH NEXT FROM TableCursor INTO @TableName 
        END 

    CLOSE TableCursor 

    DEALLOCATE TableCursor
END
GO
/****** Object:  StoredProcedure [dbo].[sp_index_rebuild]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 11.05.2009
-- Description:	Commands zum Rebuild aufrufen
-- =============================================
CREATE PROCEDURE [dbo].[sp_index_rebuild]
	
AS
BEGIN	
    DECLARE @cmd NVARCHAR(max)
    SET @cmd=''
    -- Sammle alle Index-Reorg-Anweisungen in der deklarierten Variablen
    SELECT @cmd = @cmd + Kommando + ';' + CHAR(13) + CHAR(10) FROM [dbo].[vIndexRebuild]
    -- Zur Information die Anweisung ausgeben
    PRINT @cmd
    -- Jetzt die Reorganisation starten
    EXEC (@cmd)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_index_reorg]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alexander Schlicht
-- Create date: 11.05.2009
-- Description:	Commands zur Reorganisation aufrufen
-- =============================================
CREATE PROCEDURE [dbo].[sp_index_reorg]
	
AS
BEGIN	
    DECLARE @cmd NVARCHAR(max)
    SET @cmd=''
    -- Sammle alle Index-Reorg-Anweisungen in der deklarierten Variablen
    SELECT @cmd = @cmd + Kommando + ';' + CHAR(13) + CHAR(10) FROM [dbo].[vIndexReorg]
    -- Zur Information die Anweisung ausgeben
    PRINT @cmd
    -- Jetzt die Reorganisation starten
    EXEC (@cmd)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_ImportTransaction]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dominik Schulte
-- Create date: 19.03.2014
-- Description:	Insert into ImportTransactions for SSIS Package
-- =============================================
CREATE PROCEDURE [dbo].[sp_insert_ImportTransaction]
	-- Add the parameters for the stored procedure here
		   @ImportStack_ID INT,
		   @ProcessName varchar(20),
           @BookingDate datetime,
           @SourceAccountNumber varchar(13),
           @SourceName1 nvarchar(50),
           @SourceName2 nvarchar(50),
           @SourceExtDescription varchar(255),
           @DestinationAccountNumber varchar(13),
           @DestinationName1 nvarchar(50),
           @DestinationName2 nvarchar(50),
           @DestinationExtDescription varchar(255),
           @ReferenceNumber varchar(50),
           @IntDescription varchar(255),
		@id INTEGER OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	INSERT INTO [dbo].[ImportTransactions]
			   ([ImportStack_ID]
			   ,[ProcessName]
			   ,[BookingDate]
			   ,[SourceAccountNumber]
			   ,[SourceName1]
			   ,[SourceName2]
			   ,[SourceExtDescription]
			   ,[DestinationAccountNumber]
			   ,[DestinationName1]
			   ,[DestinationName2]
			   ,[DestinationExtDescription]
			   ,[ReferenceNumber]
			   ,[IntDescription]           
			   ,[OptimisticLockField]
			   )
		 VALUES
			   (@ImportStack_ID,
			   @ProcessName,
			   @BookingDate,
			   @SourceAccountNumber, 
			   @SourceName1,
			   @SourceName2,
			   @SourceExtDescription, 
			   @DestinationAccountNumber,
			   @DestinationName1, 
			   @DestinationName2, 
			   @DestinationExtDescription,
			   @ReferenceNumber,
			   @IntDescription,0);

	SELECT  @id = SCOPE_IDENTITY();

END
GO
/****** Object:  StoredProcedure [dbo].[SP_MergeBookingSyncStates]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_MergeBookingSyncStates]
    @TVP BookingSyncStateType READONLY
AS
    BEGIN
        SET NOCOUNT ON;
        MERGE dbo.BookingSyncStates AS target
        USING
            ( SELECT    ID ,
						Account_ID,
                        ErrorCode ,
                        LastSync ,
                        SyncOperation ,
                        ChangeOperation ,
                        RequestEnqueued ,
                        RequestDeQueued ,
                        RequestDeferred ,
                        ResponseEnqueued ,
                        ResponseDeQueued ,
                        ResponseDeferred ,
                        Acknowledged ,
                        SessionId
              FROM      @TVP
            ) AS source
        ON ( target.ID = source.ID AND target.Account_ID = source.Account_ID)
        WHEN MATCHED THEN
            UPDATE SET
                    ErrorCode = source.ErrorCode ,
                    LastSync = source.LastSync ,
                    SyncOperation = source.SyncOperation ,
                    ChangeOperation = source.ChangeOperation ,
                    RequestEnqueued = source.RequestEnqueued ,
                    RequestDeQueued = source.RequestDeQueued ,
                    RequestDeferred = source.RequestDeferred ,
                    ResponseEnqueued = source.ResponseEnqueued ,
                    ResponseDeQueued = source.ResponseDeQueued ,
                    ResponseDeferred = source.ResponseDeferred ,
                    Acknowledged = source.Acknowledged ,
                    SessionId = source.SessionId
        WHEN NOT MATCHED THEN
            INSERT ( [ID] ,
                     [ErrorCode] ,
                     [LastSync] ,
                     [SyncOperation] ,
                     [ChangeOperation] ,
                     [RequestEnqueued] ,
                     [RequestDeQueued] ,
                     [RequestDeferred] ,
                     [ResponseEnqueued] ,
                     [ResponseDeQueued] ,
                     [ResponseDeferred] ,
                     [Acknowledged] ,
                     [SessionId],
					 [Account_ID]
                   )
            VALUES ( source.[ID] ,
                     source.[ErrorCode] ,
                     source.[LastSync] ,
                     source.[SyncOperation] ,
                     source.[ChangeOperation] ,
                     source.[RequestEnqueued] ,
                     source.[RequestDeQueued] ,
                     source.[RequestDeferred] ,
                     source.[ResponseEnqueued] ,
                     source.[ResponseDeQueued] ,
                     source.[ResponseDeferred] ,
                     source.[Acknowledged] ,
                     source.[SessionId],
					 source.[Account_ID]
                   );
    END;
	
GO
/****** Object:  StoredProcedure [dbo].[sp_refresh_accounts_hierarchy]    Script Date: 29/01/2020 11:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:      Alexander Schlicht
-- Create date: 25.09.2012
-- Description: Erstellt die Tabelle AccountHierarchy von der Adjacency Liste (Parent_ID > ID) aus der Tabelle Account.
-- =============================================
CREATE PROCEDURE [dbo].[sp_refresh_accounts_hierarchy]
AS 
    BEGIN           
        SET NOCOUNT ON;

        DECLARE @tranF BIT;     
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;
    
        IF ( @@TRANCOUNT = 0 ) 
            BEGIN
                BEGIN TRANSACTION;
                SET @tranF = 1;
            END 
    
        BEGIN TRY
        
            TRUNCATE TABLE dbo.AccountHierarchy
                          
            IF OBJECT_ID('tempdb..#tmp_accounts_hierarchy') IS NOT NULL 
                BEGIN
                    DROP TABLE #tmp_accounts_hierarchy
               END;
            WITH    AccountLevel ( ID, Parent_ID, TreeText, PathByID, PathByName, TreeLevel )
                      AS ( SELECT   ID ,
                                    Parent_ID ,
                                    CONVERT(VARCHAR(255), Name) AS [TreeText] ,
                                    '/' + CAST(ID AS VARCHAR(MAX)) + '/' AS [PathByID] ,
                                    CAST(PathName AS VARCHAR(1500)) AS [PathByName] ,
                                    1 AS [TreeLevel]
                          FROM     dbo.Accounts
                           WHERE    Parent_ID IS NULL
                           UNION ALL
                           SELECT   e.ID ,
                                    e.Parent_ID ,
                                    CONVERT(VARCHAR(255), REPLICATE('|  ',
                                                              TreeLevel)
                                    + e.PathName) ,
                                    x.PathByID + CAST(e.ID AS VARCHAR(MAX))
                                    + '/' ,
                                    CONVERT(VARCHAR(1500), x.PathByName + ':'
                                    + e.PathName) ,
                                    x.TreeLevel + 1 AS TreeLevel
                           FROM     AccountLevel x
                                    JOIN dbo.Accounts e ON e.Parent_ID = x.ID
                         )
                SELECT  a.[ID] ,
                        a.[Parent_ID] ,
                        HIERARCHYID::Parse(PathByID) AS [HID] ,
                        a.[TreeText] ,
                        a.[PathByID] ,
                        a.[PathByName] ,
                        a.[TreeLevel]
                INTO    #tmp_accounts_hierarchy
                FROM    AccountLevel a  

            INSERT  INTO dbo.AccountHierarchy
                    ( ID ,
                      Parent_ID ,
                      HID ,
                      TreeText ,
                      RowNumber ,
                      Lft ,
                      Rgt,
                      HasChildren,
                      TreeLevel,
                      PathById
                    )
            OUTPUT  INSERTED.*
                    -- url zu nested sets http://www.klempert.de/nested_sets/     
                    SELECT  ah.ID ,
                            ah.Parent_ID ,
                            ah.[HID] ,
                            ah.TreeText ,
                            ROW_NUMBER() OVER ( ORDER BY ah.HID.ToString() ) AS RowNumber ,
                            ( ROW_NUMBER() OVER ( ORDER BY ah.HID.ToString() )
                              * 2 ) - ah.HID.GetLevel() AS Lft ,
                            ( ( ROW_NUMBER() OVER ( ORDER BY ah.HID.ToString() )
                               * 2 ) - ah.HID.GetLevel() )
                            + ( SELECT  2 * ( COUNT(*) - 1 )
                                FROM    #tmp_accounts_hierarchy
                                WHERE   HID.IsDescendantOf(ah.HID) = 1
                             ) + 1 AS Rgt,
                              0,
                              TreeLevel,
                              PathByID
                    FROM    #tmp_accounts_hierarchy AS ah

-- Aktualisiere den Pfad in der Account Table                                                                                       
            UPDATE  Accounts
            SET     [PathByName] = tmp.PathByName 
                    --, entfernt um verwirung vorzubeugen
                    -- Accounts.UpdateTime = GETDATE() ,    
                    -- Accounts.UpdateUser = SUSER_NAME() ,
                    -- Accounts.OptimisticLockField = COALESCE(OptimisticLockField,0) + 1
            FROM    #tmp_accounts_hierarchy AS tmp
                    JOIN dbo.Accounts AS accounts ON tmp.ID = accounts.ID
            WHERE   ISNULL(accounts.[PathByName], '') <> tmp.PathByName

-- Aktualisiere HasChildren in der AccountHierarchy Tabelle
            UPDATE  tmp1
            SET     HasChildren = CASE WHEN tmp2.HID IS NULL
                                       THEN CONVERT(BIT, 0)
                                       ELSE CONVERT(BIT, 1)
                                  END
            FROM    dbo.AccountHierarchy AS tmp1
                    LEFT OUTER JOIN dbo.AccountHierarchy AS tmp2 ON tmp1.HID = tmp2.HID.GetAncestor(1)                               

            IF ( @tranF = 1 ) 
                COMMIT TRANSACTION      
            RETURN 1

        END TRY
        BEGIN CATCH
            IF ( @tranF = 1 ) 
                ROLLBACK TRANSACTION                                    

            SELECT  @ErrorMessage = ERROR_MESSAGE() ,
                    @ErrorSeverity = ERROR_SEVERITY() ,
                    @ErrorState = ERROR_STATE();

            RAISERROR (@ErrorMessage, -- Message text.
                    @ErrorSeverity, -- Severity.
                    @ErrorState -- State.
                    );
        END CATCH       
    END
GO
EXEC [LtmsDbFait].sys.sp_addextendedproperty @name=N'SQLSourceControl Scripts Location', @value=N'<?xml version="1.0" encoding="utf-16" standalone="yes"?>
<ISOCCompareLocation version="1" type="TfsLocation">
  <ServerUrl>http://dev-tfs:8080/tfs/it</ServerUrl>
  <SourceControlFolder>$/LAMA/DB/DPL</SourceControlFolder>
</ISOCCompareLocation>' 
GO
EXEC [LtmsDbFait].sys.sp_addextendedproperty @name=N'SQLSourceControl Migration Scripts Location', @value=N'<?xml version="1.0" encoding="utf-16" standalone="yes"?>
<ISOCCompareLocation version="1" type="TfsLocation">
  <ServerUrl>http://dev-tfs:8080/tfs/it</ServerUrl>
  <SourceControlFolder>$/LAMA/DB/DPL_Migration</SourceControlFolder>
</ISOCCompareLocation>' 
GO
EXEC [LtmsDbFait].sys.sp_addextendedproperty @name=N'SQLSourceControl Database Revision', @value=4959 
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'vorläufig null-> BL, true->checked, false->unchecked' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PalletChangeDefinitions', @level2type=N'COLUMN',@level2name=N'IsDefault'
GO
