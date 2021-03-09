--UPDATE LMS.dbo.LMS_AVAILABILITY SET LoadingLocationId = NULL WHERE LoadingLocationId IS NOT NULL
--UPDATE LMS.dbo.LMS_DELIVERY SET LoadingLocationId = NULL WHERE LoadingLocationId IS NOT NULL

UPDATE A SET LoadingLocationId = tmp.LoadingLocationId
FROM LMS.dbo.LMS_AVAILABILITY A
INNER JOIN (
	SELECT DISTINCT L.Id as LoadingLocationId, A.RefLmsAddressNumber
	from [BMFCH874FJ].[OlmaDbDvit].[dbo].LoadingLocations L JOIN OPENQUERY([BMFCH874FJ], 'SELECT Id, RefLmsAddressNumber FROM [OlmaDbDvit].[dbo].Addresses') A ON A.Id = L.AddressId
	LEFT OUTER JOIN LMS.dbo.LMS_AVAILABILITY AV ON A.RefLmsAddressNumber = AV.LoadingPointId
	WHERE A.RefLmsAddressNumber is not null
	AND ((AV.AvailabilityId IS NOT NULL AND AV.LoadingLocationId is NULL) )

) tmp on tmp.RefLmsAddressNumber = A.LoadingPointId
AND A.LoadingLocationId is NULL

GO

UPDATE D SET LoadingLocationId = tmp.LoadingLocationId
FROM LMS.dbo.LMS_DELIVERY D
INNER JOIN (
	SELECT DISTINCT L.Id as LoadingLocationId, A.RefLmsAddressNumber
	from [BMFCH874FJ].[OlmaDbDvit].[dbo].LoadingLocations L JOIN OPENQUERY([BMFCH874FJ], 'SELECT Id, RefLmsAddressNumber FROM [OlmaDbDvit].[dbo].Addresses') A ON A.Id = L.AddressId	
	LEFT OUTER JOIN LMS.dbo.LMS_DELIVERY D ON A.RefLmsAddressNumber = CASE WHEN D.LoadingPointId <> -1 THEN D.LoadingPointId WHEN D.DistributorId <> -1 THEN D.DistributorId ELSE D.CustomerId END
	WHERE A.RefLmsAddressNumber is not null
	AND ((D.DiliveryId IS NOT NULL AND D.LoadingLocationId is NULL))
) tmp on tmp.RefLmsAddressNumber = CASE WHEN D.LoadingPointId <> -1 THEN D.LoadingPointId WHEN D.DistributorId <> -1 THEN D.DistributorId ELSE D.CustomerId END
AND D.LoadingLocationId is NULL
