DECLARE @AccountId INT = 6862
DECLARE @FromDate datetime = '01.01.2019' 
DECLARE @ToDate datetime = '31.12.2020' 

SELECT  P.ID AS ProcessId,
       P.ReferenceNumber AS ProcessReferenceNumber,
       P.Name AS ProcessName,
       P.ProcessState_ID AS ProcessStateId,
       P.ProcessType_ID AS ProcessTypeId,
       P.CreateUser AS ProcessCreateUser,
       P.CreateTime AS ProcessCreateTime,
       P.UpdateUser AS ProcessUpdateUser,
       P.UpdateTime AS ProcessUpdateTime,
       P.DeleteUser AS ProcessDeleteUser,
       P.DeleteTime AS ProcessDeleteTime,
       P.OptimisticLockField AS ProcessOptimisticLockField,
       P.IntDescription AS ProcessIntDescription,             
       P.ExtDescription AS ProcessExtDescription,      
       P.ChangedTime AS ProcessChangedTme,
       T.ID AS TransactionId,
       T.RowGuid AS TransactionRowGuid,
       T.TransactionType_ID TransactionTypeId,
       T.ReferenceNumber AS TransactionReferenceNumber,      
       T.Valuta ,
       T.ExtDescription AS TransactionIntDescription,             
       T.ExtDescription AS TransactionExtDescription,      
       T.Process_ID AS TransactionProcessId,
       T.CreateUser AS TransactionCreateUser,
       T.CreateTime AS TransactionCreateTime,
       T.UpdateUser AS TransactionUpdateUser,
       T.UpdateTime AS TransactionUpdatetime,
       T.TransactionState_ID TransactionStateId,
       T.Cancellation_ID AS CancellationId,
       T.DeleteUser AS TransactionDeleteUser,
       T.DeleteTime AS TransactionDeleteTime,
       T.OptimisticLockField AS TransactionOptimisticLockField,
       T.ChangedTime AS TransactionChangedTime,
       B.ID AS BookingId,
       B.BookingType_ID AS TransactionBookingId,
       B.ReferenceNumber AS BookingReferenceNumber,      
       B.BookingDate ,              
       B.ExtDescription AS BookingExtDescription,      
       B.Quantity ,
       B.Article_ID AS ArticleId,
       B.Quality_ID AS QualityId,
       B.IncludeInBalance ,
       B.Matched ,
       B.CreateUser AS BookingCreateUser,
       B.CreateTime AS BookingCreateTime,
       B.UpdateUser AS BookingUpdateUser,
       B.UpdateTime AS BookingUpdateTime,
       B.Transaction_ID AS BookingTransactionId,
       B.Account_ID AS AccountId,
       B.MatchedUser ,
       B.MatchedTime ,
       B.AccountDirection ,
       B.DeleteUser AS BookingDeleteUser,
       B.DeleteTime AS BookingDeleteTime ,
       B.OptimisticLockField AS BookingOptimisticLockField,
       B.ChangedTime AS BookingChangedTime,
       B.RowModified AS BookingRowModified,
       B.Computed ,
       B.RowGuid 
FROM dbo.Processes P INNER JOIN dbo.Transactions T ON T.Process_ID = P.ID INNER JOIN dbo.Bookings B ON B.Transaction_ID = T.ID
INNER JOIN
      (
          SELECT DISTINCT
              T.ID
          FROM dbo.Transactions T               
              INNER JOIN dbo.Bookings B
                  ON B.Transaction_ID = T.ID
          WHERE B.Account_ID = @AccountId
                AND T.Valuta BETWEEN @FromDate AND @ToDate
      ) C ON B.Transaction_ID = C.ID
ORDER BY P.ID,T.ID,B.ID
