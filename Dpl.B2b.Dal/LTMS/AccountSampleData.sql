DECLARE @AccountId INT = 6862
DECLARE @Accounts TABLE (ID INT);

INSERT INTO @Accounts
SELECT DISTINCT
    AH.ID
FROM
    ( SELECT DISTINCT
            B.Account_ID
        FROM  (SELECT Transaction_ID AS ID
                  FROM dbo.Bookings B
                  WHERE B.Account_ID = @AccountId
              ) T
            INNER JOIN dbo.Bookings B
                ON B.Transaction_ID = T.ID              
    ) X
    JOIN dbo.AccountHierarchy AH1
        ON AH1.ID = X.Account_ID,
    [dbo].[AccountHierarchy] AH
WHERE AH.Lft <= AH1.Lft
      AND AH.Rgt >= AH1.Rgt;

	   SELECT ID,
        RowGuid,
        Name,
        Parent_ID,
        AccountType_ID,
        AccountNumber,
        CustomerNumber,
        AddressId,
        Description,
        PathByName,
        OptimisticLockField,
        CreateUser,
        CreateTime,
        UpdateUser,
        UpdateTime,
        FullName,
        PathName,
        Inactive,
        Locked,
        ResponsiblePerson_ID,
        InvoiceAccount_ID,
        KeyAccountManager_ID,
        Salesperson_ID
    FROM dbo.Accounts
WHERE ID IN
      (
          SELECT DISTINCT
              ID
          FROM
          (
              SELECT ID
              FROM @Accounts
              UNION
              SELECT InvoiceAccount_ID AS ID  -- Rechnungskonten müssen noch rein
              FROM dbo.Accounts
                  JOIN @Accounts
                      ON [@Accounts].ID = Accounts.ID
          ) X
      )
      ORDER BY ID;


