using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class CreateLmsOrdersView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // create LmsOrders_View
            migrationBuilder.Sql(@"
EXEC('
    CREATE VIEW [dbo].[LmsOrders_View]
    AS
	SELECT * FROM (
		SELECT
			LmsOrdersInner.Type, 
			LmsOrdersInner.Id, 
			RowGuid, 
			OlmaOrderId, 
			Geblockt, 
			GeblocktFuer, 
			LoadingLocationId, 
			FromDate, 
			UntilDate, 
			Quantity, 
			AvailableQuantity, 
			LC.Id AS LoadCarrierId, 
			BLC.Id AS BaseLoadCarrierId, 
			COALESCE (SupportsRearLoading, CAST(0 AS BIT)) AS SupportsRearLoading, 
			COALESCE (SupportsPartialMatching, CAST(0 AS BIT)) AS SupportsPartialMatching, 
			COALESCE (SupportsSideLoading, CAST(0 AS BIT)) AS SupportsSideLoading, 
			COALESCE (SupportsJumboVehicles, CAST(0 AS BIT)) AS SupportsJumboVehicles, 
			CASE WHEN StackHeightMin = 0 THEN 15 ELSE COALESCE (StackHeightMin, 15) END AS StackHeightMin, 
			CASE WHEN StackHeightMax = 0 THEN 15 ELSE COALESCE (StackHeightMax, 15) END AS StackHeightMax,
			IsAvailable
		FROM (
			SELECT 
				0 AS Type, 
				A.AvailabilityId AS Id, 
				A.RowGuid, 
				O.Id AS OlmaOrderId, 
				A.Geblockt, 
				A.GeblocktFuer, 
				A.LoadingLocationId, 
				A.AvailableFromDate AS FromDate, 
				A.AvailableUntilDate AS UntilDate, 
				A.Quantity, 
				A.Quantity - COALESCE( SUM(A2D.Quantity), 0) AS AvailableQuantity, 
				A.PalletTypeId, 
				A.QualityId, 
				A.BasePalletTypeId, 
				A.BaseQualityId, 
				A.SupportsPartialMatching, 
				A.SupportsRearLoading, 
				A.SupportsSideLoading, 
				A.SupportsJumboVehicles, 
				A.StackHeightMin, 
				A.StackHeightMax,
				CASE 
					WHEN 
						A.Storniert = CAST(0 AS BIT)
						AND A.Storniert = CAST(0 AS BIT)
						AND A.finished = CAST(0 AS BIT)
						AND (A.Geblockt = CAST(0 AS BIT) OR A.GeblocktFuer IS NULL OR GR.Gruppenname IS NOT NULL)
						AND (A.InBearbeitungVon IS NULL OR A.InBearbeitungDatumZeit <= DATEADD(hour, -6, GETDATE()))
					THEN CAST(1 AS BIT)
					ELSE CAST(0 AS BIT) 
				END AS IsAvailable
			FROM LMS.LMS_AVAILABILITY AS A 
			LEFT OUTER JOIN LMS.LMS_GRUPPE GR ON GR.Gruppenname = A.GeblocktFuer 
			LEFT OUTER JOIN LMS.LMS_AVAIL2DELI A2D ON A2D.AvailabilityID = A.AvailabilityId 
			LEFT OUTER JOIN dbo.Orders AS O ON O.RefLmsOrderRowGuid = A.RowGuid
			WHERE A.ClientId = 1
			AND A.DeletionDate IS NULL
			AND A2D.DeletionDate IS NULL
			--AND A.Storniert = CAST(0 AS BIT)
			--AND A.finished = CAST(0 AS BIT)
			--AND (A.Geblockt = CAST(0 AS BIT) OR A.GeblocktFuer IS NULL OR GR.Gruppenname IS NOT NULL)
			--AND (A.InBearbeitungVon IS NULL OR A.InBearbeitungDatumZeit <= DATEADD(hour, -6, GETDATE()))
			GROUP BY A.AvailabilityId, A.RowGuid, O.Id, A.Geblockt, A.GeblocktFuer, A.LoadingLocationId, A.AvailableFromDate, A.AvailableUntilDate, A.Quantity, A.PalletTypeId, A.QualityId, A.BasePalletTypeId, A.BaseQualityId, A.SupportsPartialMatching, A.SupportsRearLoading, A.SupportsSideLoading, A.SupportsJumboVehicles, A.StackHeightMin, A.StackHeightMax,
				A.Storniert, A.finished, GR.Gruppenname, A.InBearbeitungVon, A.InBearbeitungDatumZeit
        
			UNION
        
			SELECT 
				1 AS Type,
				D.DiliveryId AS Id,
				D.RowGuid,
				O.Id AS OlmaOrderId, 
				D.Geblockt, 
				D.GeblocktFuer, 
				D.LoadingLocationId, 
				D.FromDate, 
				D.UntilDate, 
				D.Quantity, 
				D.Quantity - COALESCE( SUM(A2D.Quantity), 0) AS AvailableQuantity, 
				D.PalletType AS PalletTypeId, 
				D.Quality AS QualityId, 
				D.BasePalletTypeId, 
				D.BaseQualityId, 
				D.SupportsPartialMatching, 
				D.SupportsRearLoading, 
				D.SupportsSideLoading, 
				D.SupportsJumboVehicles, 
				D.StackHeightMin, 
				D.StackHeightMax,
				CASE 
					WHEN 
						D.Storniert = CAST(0 AS BIT)
						AND D.finished = CAST(0 AS BIT)
						AND D.IstBedarf = CAST(0 AS BIT)
						AND (D.NeedsBalanceApproval IS NULL OR D.NeedsBalanceApproval = CAST(0 AS BIT) OR D.BalanceApproved = CAST(1 AS BIT))
						AND (D.Geblockt = CAST(0 AS BIT) OR D.GeblocktFuer IS NULL OR GR.Gruppenname IS NOT NULL)
						AND (D.InBearbeitungVon IS NULL OR D.InBearbeitungDatumZeit <= DATEADD(hour, -6, GETDATE()))   
					THEN CAST(1 AS BIT)
					ELSE CAST(0 AS BIT) 
				END AS IsAvailable
			FROM LMS.LMS_DELIVERY AS D 
			LEFT OUTER JOIN LMS.LMS_GRUPPE AS GR ON GR.Gruppenname = D.GeblocktFuer 
			LEFT OUTER JOIN LMS.LMS_AVAIL2DELI A2D ON A2D.DeliveryID = D.DiliveryId 
			LEFT OUTER JOIN dbo.Orders AS O ON O.RefLmsOrderRowGuid = D.RowGuid
			WHERE D.ClientId = 1
			AND D.DeletionDate IS NULL
			AND A2D.DeletionDate IS NULL
			--AND D.Storniert = CAST(0 AS BIT)
			--AND D.finished = CAST(0 AS BIT)
			--AND D.IstBedarf = CAST(0 AS BIT)
			--AND (D.NeedsBalanceApproval IS NULL OR D.NeedsBalanceApproval = CAST(0 AS BIT) OR D.BalanceApproved = CAST(1 AS BIT))
			--AND (D.Geblockt = CAST(0 AS BIT) OR D.GeblocktFuer IS NULL OR GR.Gruppenname IS NOT NULL)
			--AND (D.InBearbeitungVon IS NULL OR D.InBearbeitungDatumZeit <= DATEADD(hour, -6, GETDATE()))             
			GROUP BY D.DiliveryId, D.RowGuid, O.Id, D.Geblockt, D.GeblocktFuer, D.LoadingLocationId, D.FromDate, D.UntilDate, D.Quantity, D.PalletType, D.Quality, D.BasePalletTypeId, D.BaseQualityId, D.SupportsPartialMatching, D.SupportsRearLoading, D.SupportsSideLoading, D.SupportsJumboVehicles, D.StackHeightMin, D.StackHeightMax,
				D.Storniert, D.finished, D.IstBedarf, D.NeedsBalanceApproval, D.BalanceApproved, GR.Gruppenname, D.InBearbeitungVon, D.InBearbeitungDatumZeit
		) AS LmsOrdersInner
			INNER JOIN dbo.LoadCarrierQualityMappings AS LM ON LM.RefLmsQualityId = LmsOrdersInner.QualityId 
			INNER JOIN dbo.LoadCarrierQualities AS LQ ON LQ.Id = LM.LoadCarrierQualityId 
			INNER JOIN dbo.LoadCarrierTypes AS LT ON LT.RefLmsLoadCarrierTypeId = LmsOrdersInner.PalletTypeId 
			INNER JOIN dbo.LoadCarriers AS LC ON LC.QualityId = LQ.Id AND LC.TypeId = LT.Id 
			LEFT OUTER JOIN dbo.LoadCarrierQualityMappings AS BLM ON BLM.RefLmsQualityId = LmsOrdersInner.BaseQualityId 
			LEFT OUTER JOIN dbo.LoadCarrierQualities AS BLQ ON BLQ.Id = BLM.LoadCarrierQualityId 
			LEFT OUTER JOIN dbo.LoadCarrierTypes AS BLT ON BLT.RefLmsLoadCarrierTypeId = LmsOrdersInner.BasePalletTypeId 
			LEFT OUTER JOIN dbo.LoadCarriers AS BLC ON BLC.QualityId = BLQ.Id AND BLC.TypeId = BLT.Id 
	) AS LmsOrders
')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"EXEC('DROP VIEW dbo.[LmsOrders_View]')");
        }

    }
}
