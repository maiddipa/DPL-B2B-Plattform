DECLARE @AccountId INT = 6862
DECLARE @FromDate datetime = '01.01.2019' 
DECLARE @ToDate datetime = '31.12.2020' 

SELECT rb.Report_ID
	,rb.Booking_ID
FROM Reports R
INNER JOIN dbo.ReportBookings rb ON r.id = rb.Report_ID
INNER JOIN Bookings b ON RB.Booking_ID = B.id AND B.Account_ID = @AccountId
INNER JOIN Transactions t ON B.Transaction_Id = T.ID 
WHERE R.PrimaryAccount_ID = @AccountId
AND T.Valuta BETWEEN @FromDate AND @ToDate
--AND Startdate >= @FromDate 
--AND EndDate <= @ToDate
ORDER BY R.ID,B.ID