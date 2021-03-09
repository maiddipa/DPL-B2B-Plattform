DECLARE  @AccountId Int= 6862
--DECLARE @FromDate datetime = '01.01.2019' 
--DECLARE @ToDate datetime = '31.12.2020' 

DECLARE @T1 TABLE(Account_ID int ,Article varchar(50),BookingState nvarchar(50),Records int,Intact int,Defect int,Coordinated bit,IncludeInBalance bit,AccountDirection varchar(20),ReportState char(1))
INSERT INTO @T1 (Records,Intact,Defect,Coordinated,IncludeInBalance,AccountDirection,Account_ID,Article,ReportState,BookingState) --OUTPUT Inserted.* 
SELECT COUNT(B.ID) Records
	,SUM(Intact) AS Intact
	,SUM(Defect) AS Defect
	,MATCHED as Coordinated
	,IncludeInBalance
	,CASE AccountDirection WHEN 'Q' THEN 'Gutschriften' ELSE 'Belastungen' END AS Direction
	,Account_ID
	,Art.[Name]
	,R.ReportState_ID
	,CASE 
		WHEN MATCHED = 1 
			THEN 'Abgestimmt'
		WHEN MATCHED = 0 AND IncludeInBalance = 1 AND ReportState_ID = 'U'
			THEN 'InAbstimmung'				
		WHEN Matched = 0 AND IncludeInBalance = 1 AND ReportState_ID IS NULL
			THEN 'Unabgestimmt'
		WHEN Matched = 0 AND IncludeInBalance = 0 AND ReportState_ID IS NULL
			THEN 'Vorläufig'		
		ELSE 'Ungültig'
		END AS BookingState
FROM (Select *,CASE Quality_ID WHEN 2 THEN 0 ELSE Quantity END AS Intact,CASE Quality_ID WHEN 2 THEN Quantity ELSE 0 END AS Defect FROM Bookings) B
INNER JOIN Articles Art ON B.Article_ID = Art.ID
INNER JOIN Transactions T ON B.Transaction_ID = T.ID
LEFT JOIN (SELECT RB.Booking_ID,'U' AS ReportState_ID FROM dbo.Reports R INNER JOIN dbo.ReportBookings RB ON R.ID = RB.Report_ID
	WHERE R.DeleteTime IS NULL AND R.ReportState_ID = 'U'
	GROUP BY RB.Booking_ID) R ON B.ID  = R.Booking_ID
WHERE B.DeleteTime IS NULL AND BookingType_ID != 'POOL' --TODO: Check if this bookingtypes are also filter in OLMA
	AND Account_ID = @AccountId
  --AND T.Valuta BETWEEN @FromDate AND @ToDate
GROUP BY Account_ID
	,Article_ID
	,Art.Name
	,MATCHED
	,IncludeInBalance
	,AccountDirection
	,R.ReportState_ID

SELECT * FROM (
SELECT Account_ID,'01' as O,Article,'Saldo (aus bis dato abgestimmten Buchungen)' AS BalanceType,SUM(Records) AS Records,SUM(Intact) AS Intact,SUM(Defect) AS Defect  FROM @T1 WHERE BookingState = 'Abgestimmt'
GROUP BY Account_ID,Article,BookingState
UNION
SELECT Account_ID,'02' as O,Article,BookingState + 'e ' + AccountDirection,Records,Intact,Defect FROM @T1 WHERE BookingState =  'Vorläufig'
UNION
SELECT Account_ID,'03' as O,Article,BookingState + 'e ' + AccountDirection,Records,Intact,Defect FROM @T1 WHERE BookingState = 'Unabgestimmt'
UNION
SELECT Account_ID,'04' as O,Article,'In Abstimmung ' + AccountDirection,Records,Intact, Defect FROM @T1 WHERE BookingState = 'InAbstimmung'
UNION
SELECT Account_ID,'99' as O,Article,'Vorläufige Summe aller Buchungen',SUM(Records) AS Records,SUM(Intact) AS Intact, SUM(Defect) AS Defect FROM @T1 
GROUP BY Account_ID,Article) T 
--WHERE Article = 'EUR'
order by Account_ID, Article,O,BalanceType
