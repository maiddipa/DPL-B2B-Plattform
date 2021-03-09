DECLARE @AccountId INT = 6862
DECLARE @FromDate datetime = '01.01.2019' 
DECLARE @ToDate datetime = '31.12.2020' 

SELECT DISTINCT R.ID
	,R.Name
	,R.ReportName
	,R.FileName
	,R.ReportFile
	,NULL AS FilePath
	,R.ReportUrl
	,R.StartDate
	,R.EndDate
	,R.Quarter
	,R.[Month]
	,R.AddressId
	,R.CustomerNumber
	,NULL AS AdditionalParams
	,R.CreateUser
	,R.CreateTime
	,R.UpdateUser
	,R.UpdateTime
	,R.ReportState_ID
	,R.[Year]
	,R.MatchedUser
	,R.MatchedTime
	,R.DeleteUser
	,R.DeleteTime
	,R.OptimisticLockField
	,R.ReportType_ID
	,R.PrimaryAccount_ID
	,R.DOC_ID
	,R.gdoc_id
FROM dbo.Reports R INNER JOIN dbo.ReportBookings RB ON R.ID = RB.Report_ID INNER JOIN Bookings B ON RB.Booking_ID = B.ID INNER JOIN Transactions T ON B.Transaction_ID = T.ID
WHERE R.PrimaryAccount_ID = @AccountId
AND Valuta between @FromDate AND  @ToDate
ORDER BY R.ID