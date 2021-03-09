using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Util;
using System.Threading;
using System.Data.Common;
using System.Globalization;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Common.Enumerations;

namespace Live2LMSService
   {
   class LiveDBTools
      {
      public static int LMSClientID = 1;
      public static string LMSServiceUser = "LiveOnline";

      public static bool CheckDelivery(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         errorReason = "";
         errorDescription = "";

         // Check CustomerID
         if (!CheckCustomerIDExists(config, CurrentSqlTransaction, syncRequest.CustomerNumber))
            {
            errorReason = ServiceMain.ErrorReasonCustomerNumber;
            errorDescription = ServiceMain.ErrorDescriptionCustomerNumber + syncRequest.CustomerNumber;
            return false;
            }

         // Check CustomerNumberLoadingLocation
         if (syncRequest.TransportType == OrderTransportType.ProvidedByOthers && !CheckCustomerIDExists(config, CurrentSqlTransaction, syncRequest.CustomerNumberLoadingLocation))
            {
            errorReason = ServiceMain.ErrorReasonCustomerNumberLoadingLocation;
            errorDescription = ServiceMain.ErrorDescriptionCustomerNumberLoadingLocation + syncRequest.CustomerNumberLoadingLocation;
            return false;
            }

         // Check RefLmsLoadCarrierId
         if (syncRequest.RefLmsLoadCarrierId == 0)
            {
            errorReason = ServiceMain.ErrorReasonRefLmsLoadCarrierId;
            errorDescription = ServiceMain.ErrorDescriptionRefLmsLoadCarrierId;
            return false;
            }

         return true;
         }

      public static bool InsertDelivery(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         if (syncRequest.Type != OrderType.Demand)
            {
            return false;
            }

         // Artikel und Qualität aus RefLmsLoadCarrierId und RefLmsBaseLoadCarrierId ermitteln
         int quality = syncRequest.RefLmsLoadCarrierId % 1000;
         int palletType = syncRequest.RefLmsLoadCarrierId / 1000;
         int baseQualityId = 0;
         int basePalletTypeId = 0;
         if (syncRequest.RefLmsBaseLoadCarrierId != 0)
            {
            baseQualityId = (int)syncRequest.RefLmsBaseLoadCarrierId % 1000;
            basePalletTypeId = (int)syncRequest.RefLmsBaseLoadCarrierId / 1000;
            }

         string QueryCommandString = @"
INSERT INTO [" + config.LMSSchemaName + @"].[LMS_DELIVERY]
           ([OldDate]
           ,[Quantity]
           ,[Quality]
           ,[year]
           ,[month]
           ,[cw]
           ,[ZipCode]
           ,[TempAllocAvail]
           ,[day]
           ,[finished]
           ,[ClientId]
           ,[CountryId]
           ,[CustomerId]
           ,[DistributorId]
           ,[LoadingPointId]
           ,[CarrierId]
           ,[FromDate]
           ,[UntilDate]
           ,[FromTime]
           ,[UntilTime]
           ,[Comment]
           ,[IsFix]
           ,[PalletType]
           ,[ContractNo]
           ,[DeliveryNoteNo]
           ,[TransactionNo]
           ,[CategoryId]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[Rejection]
           ,[Reklamation]
           ,[RejectionReasonId]
           ,[RejectionComment]
           ,[RejectionQuantity]
           ,[RejectionApproachId]
           ,[RejectionBy]
           ,[RejectionDate]
           ,[RejectionDone]
           ,[RejectionDoneComment]
           ,[RejectionNextReceiptNo]
           ,[RejectionDoneBy]
           ,[RejectionDoneDate]
           ,[RejectionCost]
           ,[ContactPerson]
           ,[ContactDetails]
           ,[IsMixedDelivery]
           ,[ManualDistributorAddress]
           ,[IsSmallAmount]
           ,[ZuAvisieren]
           ,[IstAvisiert]
           ,[AvisiertVon]
           ,[AvisiertAm]
           ,[LSManAdrAnrede]
           ,[LSManAdrName1]
           ,[LSManAdrName2]
           ,[LSManAdrName3]
           ,[LSManAdrPLZ]
           ,[LSManAdrOrt]
           ,[LSManAdrStrasse]
           ,[LSManAdrLand]
           ,[LAManAdrAnrede]
           ,[LAManAdrName1]
           ,[LAManAdrName2]
           ,[LAManAdrName3]
           ,[LAManAdrPLZ]
           ,[LAManAdrOrt]
           ,[LAManAdrStrasse]
           ,[LAManAdrLand]
           ,[DeliveryTime]
           ,[FrachtauftragNo]
           ,[Notiz]
           ,[Kilometer]
           ,[Frachtpreis]
           ,[Prioritaet]
           ,[Bestellnummer]
           ,[Geblockt]
           ,[GeblocktVon]
           ,[GeblocktDatum]
           ,[GeblocktAufgehobenVon]
           ,[GeblocktAufgehobenDatum]
           ,[GeblocktKommentar]
           ,[GeblocktFuer]
           ,[Storniert]
           ,[StorniertVon]
           ,[StorniertDatum]
           ,[StorniertKommentar]
           ,[InBearbeitungVon]
           ,[InBearbeitungDatumZeit]
           ,[Revision]
           ,[Fixmenge]
           ,[Jumboladung]
           ,[IstBedarf]
           ,[BusinessTypeId]
           ,[IstPlanmenge]
           ,[Rahmenauftragsnummer]
           ,[FvKoffer]
           ,[FvPlane]
           ,[FvStapelhoehe]
           ,[FvVerladerichtungLaengs]
           ,[DispoNotiz]
           ,[OrderManagementID]
           ,[Einzelpreis]
           ,[ExterneNummer]
           ,[RowGuid]
           ,[StackHeightMin]
           ,[StackHeightMax]
           ,[SupportsRearLoading]
           ,[SupportsSideLoading]
           ,[SupportsJumboVehicles]
           ,[SupportsIndividual]
           ,[BasePalletTypeId]
           ,[BaseQualityId]
           ,[ShowOnline]
           ,[IsCreatedOnline]
           ,[NeedsBalanceApproval]
           ,[BalanceApproved]
           ,[LiveDocumentNumber]
           ,[ExpressCode]
           ,[OnlineComment]
           ,[SupportsPartialMatching]
           ,[HasBasePallets]
           ,[LoadingLocationId]
           ,[LtmsAccountId]
           ,[LtmsAccountNumber]
           )
     VALUES
           (@OldDate
           ,@Quantity
           ,@Quality
           ,@year
           ,@month
           ,@cw
           ,@ZipCode
           ,@TempAllocAvail
           ,@day
           ,@finished
           ,@ClientId
           ,@CountryId
           ,@CustomerId
           ,@DistributorId
           ,@LoadingPointId
           ,@CarrierId
           ,@FromDate
           ,@UntilDate
           ,@FromTime
           ,@UntilTime
           ,@Comment
           ,@IsFix
           ,@PalletType
           ,@ContractNo
           ,@DeliveryNoteNo
           ,@TransactionNo
           ,@CategoryId
           ,@CreationDate
           ,@ModificationDate
           ,@DeletionDate
           ,@CreatedBy
           ,@ModifiedBy
           ,@DeletedBy
           ,@Rejection
           ,@Reklamation
           ,@RejectionReasonId
           ,@RejectionComment
           ,@RejectionQuantity
           ,@RejectionApproachId
           ,@RejectionBy
           ,@RejectionDate
           ,@RejectionDone
           ,@RejectionDoneComment
           ,@RejectionNextReceiptNo
           ,@RejectionDoneBy
           ,@RejectionDoneDate
           ,@RejectionCost
           ,@ContactPerson
           ,@ContactDetails
           ,@IsMixedDelivery
           ,@ManualDistributorAddress
           ,@IsSmallAmount
           ,@ZuAvisieren
           ,@IstAvisiert
           ,@AvisiertVon
           ,@AvisiertAm
           ,@LSManAdrAnrede
           ,@LSManAdrName1
           ,@LSManAdrName2
           ,@LSManAdrName3
           ,@LSManAdrPLZ
           ,@LSManAdrOrt
           ,@LSManAdrStrasse
           ,@LSManAdrLand
           ,@LAManAdrAnrede
           ,@LAManAdrName1
           ,@LAManAdrName2
           ,@LAManAdrName3
           ,@LAManAdrPLZ
           ,@LAManAdrOrt
           ,@LAManAdrStrasse
           ,@LAManAdrLand
           ,@DeliveryTime
           ,@FrachtauftragNo
           ,@Notiz
           ,@Kilometer
           ,@Frachtpreis
           ,@Prioritaet
           ,@Bestellnummer
           ,@Geblockt
           ,@GeblocktVon
           ,@GeblocktDatum
           ,@GeblocktAufgehobenVon
           ,@GeblocktAufgehobenDatum
           ,@GeblocktKommentar
           ,@GeblocktFuer
           ,@Storniert
           ,@StorniertVon
           ,@StorniertDatum
           ,@StorniertKommentar
           ,@InBearbeitungVon
           ,@InBearbeitungDatumZeit
           ,@Revision
           ,@Fixmenge
           ,@Jumboladung
           ,@IstBedarf
           ,@BusinessTypeId
           ,@IstPlanmenge
           ,@Rahmenauftragsnummer
           ,@FvKoffer
           ,@FvPlane
           ,@FvStapelhoehe
           ,@FvVerladerichtungLaengs
           ,@DispoNotiz
           ,@OrderManagementID
           ,@Einzelpreis
           ,@ExterneNummer
           ,@RowGuid
           ,@StackHeightMin
           ,@StackHeightMax
           ,@SupportsRearLoading
           ,@SupportsSideLoading
           ,@SupportsJumboVehicles
           ,@SupportsIndividual
           ,@BasePalletTypeId
           ,@BaseQualityId
           ,@ShowOnline
           ,@IsCreatedOnline
           ,@NeedsBalanceApproval
           ,@BalanceApproved
           ,@LiveDocumentNumber
           ,@ExpressCode
           ,@OnlineComment
           ,@SupportsPartialMatching
           ,@HasBasePallets
           ,@LoadingLocationId
           ,@LtmsAccountId
           ,@LtmsAccountNumber
           );
SELECT SCOPE_IDENTITY()
";

         int newDeliveryId = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("OldDate", null));
            commandParameterList.Add(new SqlParameter("Quantity", syncRequest.LoadCarrierQuantity));
            commandParameterList.Add(new SqlParameter("Quality", quality));
            commandParameterList.Add(new SqlParameter("year", syncRequest.EarliestFulfillmentDateTime.Year));
            commandParameterList.Add(new SqlParameter("month", syncRequest.EarliestFulfillmentDateTime.Month));
            commandParameterList.Add(new SqlParameter("cw", CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(syncRequest.EarliestFulfillmentDateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)));
            commandParameterList.Add(new SqlParameter("ZipCode", -1));
            commandParameterList.Add(new SqlParameter("TempAllocAvail", (object)0));
            commandParameterList.Add(new SqlParameter("day", null));
            commandParameterList.Add(new SqlParameter("finished", false));
            commandParameterList.Add(new SqlParameter("ClientId", LiveDBTools.LMSClientID));
            commandParameterList.Add(new SqlParameter("CountryId", 1)); // TODO
            commandParameterList.Add(new SqlParameter("CustomerId", syncRequest.CustomerNumber));
            commandParameterList.Add(new SqlParameter("DistributorId", syncRequest.CustomerNumberLoadingLocation != 0 ? syncRequest.CustomerNumberLoadingLocation : syncRequest.CustomerNumber));
            commandParameterList.Add(new SqlParameter("LoadingPointId", -1));
            commandParameterList.Add(new SqlParameter("CarrierId", -1));
            commandParameterList.Add(new SqlParameter("FromDate", syncRequest.EarliestFulfillmentDateTime.Date));
            commandParameterList.Add(new SqlParameter("UntilDate", syncRequest.LatestFulfillmentDateTime.Date));
            commandParameterList.Add(new SqlParameter("FromTime", "00:00"));
            commandParameterList.Add(new SqlParameter("UntilTime", "00:00"));
            commandParameterList.Add(new SqlParameter("Comment", ""));
            commandParameterList.Add(new SqlParameter("IsFix", (syncRequest.EarliestFulfillmentDateTime.Date == syncRequest.LatestFulfillmentDateTime.Date)));
            commandParameterList.Add(new SqlParameter("PalletType", palletType));
            commandParameterList.Add(new SqlParameter("ContractNo", syncRequest.VauNumber ?? ""));
            commandParameterList.Add(new SqlParameter("DeliveryNoteNo", ""));
            commandParameterList.Add(new SqlParameter("TransactionNo", ""));
            commandParameterList.Add(new SqlParameter("CategoryId", syncRequest.TransportType == OrderTransportType.Self ? 3 : (object)0));
            commandParameterList.Add(new SqlParameter("CreationDate", DateTime.Now));
            commandParameterList.Add(new SqlParameter("ModificationDate", null));
            commandParameterList.Add(new SqlParameter("DeletionDate", null));
            commandParameterList.Add(new SqlParameter("CreatedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModifiedBy", null));
            commandParameterList.Add(new SqlParameter("DeletedBy", null));
            commandParameterList.Add(new SqlParameter("Rejection", false));
            commandParameterList.Add(new SqlParameter("Reklamation", false));
            commandParameterList.Add(new SqlParameter("RejectionReasonId", null));
            commandParameterList.Add(new SqlParameter("RejectionComment", ""));
            commandParameterList.Add(new SqlParameter("RejectionQuantity", (object)0));
            commandParameterList.Add(new SqlParameter("RejectionApproachId", null));
            commandParameterList.Add(new SqlParameter("RejectionBy", null));
            commandParameterList.Add(new SqlParameter("RejectionDate", null));
            commandParameterList.Add(new SqlParameter("RejectionDone", false));
            commandParameterList.Add(new SqlParameter("RejectionDoneComment", ""));
            commandParameterList.Add(new SqlParameter("RejectionNextReceiptNo", ""));
            commandParameterList.Add(new SqlParameter("RejectionDoneBy", null));
            commandParameterList.Add(new SqlParameter("RejectionDoneDate", null));
            commandParameterList.Add(new SqlParameter("RejectionCost", (object)0));
            commandParameterList.Add(new SqlParameter("ContactPerson", syncRequest.Person.LastName + ", " + syncRequest.Person.FirstName));
            commandParameterList.Add(new SqlParameter("ContactDetails", "Tel: " + syncRequest.Person.PhoneNumber + "; EMail: " + syncRequest.Person.Email));
            commandParameterList.Add(new SqlParameter("IsMixedDelivery", false));
            commandParameterList.Add(new SqlParameter("ManualDistributorAddress", ""));
            commandParameterList.Add(new SqlParameter("IsSmallAmount", false));
            commandParameterList.Add(new SqlParameter("ZuAvisieren", false));
            commandParameterList.Add(new SqlParameter("IstAvisiert", false));
            commandParameterList.Add(new SqlParameter("AvisiertVon", null));
            commandParameterList.Add(new SqlParameter("AvisiertAm", null));
            commandParameterList.Add(new SqlParameter("LSManAdrAnrede", "")); // Ladestelle
            commandParameterList.Add(new SqlParameter("LSManAdrName1", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrName2", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrName3", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrPLZ", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrOrt", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrStrasse", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrLand", ""));
            commandParameterList.Add(new SqlParameter("LAManAdrAnrede", "")); // Lieferadresse
            commandParameterList.Add(new SqlParameter("LAManAdrName1", ""));
            commandParameterList.Add(new SqlParameter("LAManAdrName2", ""));
            commandParameterList.Add(new SqlParameter("LAManAdrName3", ""));
            commandParameterList.Add(new SqlParameter("LAManAdrPLZ", syncRequest.Address?.PostalCode ?? ""));
            commandParameterList.Add(new SqlParameter("LAManAdrOrt", syncRequest.Address?.City ?? ""));
            commandParameterList.Add(new SqlParameter("LAManAdrStrasse", syncRequest.Address?.Street1 ?? ""));
            commandParameterList.Add(new SqlParameter("LAManAdrLand", syncRequest.Address?.CountryName ?? ""));
            commandParameterList.Add(new SqlParameter("DeliveryTime", syncRequest.BusinessHoursString == null ? "" : syncRequest.BusinessHoursString.Substring(0, 128)));
            commandParameterList.Add(new SqlParameter("FrachtauftragNo", ""));
            commandParameterList.Add(new SqlParameter("Notiz", ""));
            commandParameterList.Add(new SqlParameter("Kilometer", (object)0));
            commandParameterList.Add(new SqlParameter("Frachtpreis", (object)0));
            commandParameterList.Add(new SqlParameter("Prioritaet", 4));
            commandParameterList.Add(new SqlParameter("Bestellnummer", ""));
            commandParameterList.Add(new SqlParameter("Geblockt", false));
            commandParameterList.Add(new SqlParameter("GeblocktVon", null));
            commandParameterList.Add(new SqlParameter("GeblocktDatum", null));
            commandParameterList.Add(new SqlParameter("GeblocktAufgehobenVon", null));
            commandParameterList.Add(new SqlParameter("GeblocktAufgehobenDatum", null));
            commandParameterList.Add(new SqlParameter("GeblocktKommentar", null));
            commandParameterList.Add(new SqlParameter("GeblocktFuer", null));
            commandParameterList.Add(new SqlParameter("Storniert", false));
            commandParameterList.Add(new SqlParameter("StorniertVon", null));
            commandParameterList.Add(new SqlParameter("StorniertDatum", null));
            commandParameterList.Add(new SqlParameter("StorniertKommentar", null));
            commandParameterList.Add(new SqlParameter("InBearbeitungVon", null));
            commandParameterList.Add(new SqlParameter("InBearbeitungDatumZeit", null));
            commandParameterList.Add(new SqlParameter("Revision", (object)0));
            commandParameterList.Add(new SqlParameter("Fixmenge", false));
            commandParameterList.Add(new SqlParameter("Jumboladung", syncRequest.SupportsJumboVehicles));
            commandParameterList.Add(new SqlParameter("IstBedarf", syncRequest.TransportType == OrderTransportType.Self));
            commandParameterList.Add(new SqlParameter("BusinessTypeId", 1)); // Businesstype Poolingkunde
            commandParameterList.Add(new SqlParameter("IstPlanmenge", false));
            commandParameterList.Add(new SqlParameter("Rahmenauftragsnummer", ""));
            commandParameterList.Add(new SqlParameter("FvKoffer", syncRequest.SupportsRearLoading));
            commandParameterList.Add(new SqlParameter("FvPlane", syncRequest.SupportsSideLoading));
            commandParameterList.Add(new SqlParameter("FvStapelhoehe", syncRequest.StackHeightMax));
            commandParameterList.Add(new SqlParameter("FvVerladerichtungLaengs", null));
            commandParameterList.Add(new SqlParameter("DispoNotiz", ""));
            commandParameterList.Add(new SqlParameter("OrderManagementID", ""));
            commandParameterList.Add(new SqlParameter("Einzelpreis", (object)0));
            commandParameterList.Add(new SqlParameter("ExterneNummer", syncRequest.ExternalReferenceNumber));
            commandParameterList.Add(new SqlParameter("RowGuid", syncRequest.OrderRowGuid));
            commandParameterList.Add(new SqlParameter("StackHeightMin", syncRequest.StackHeightMin));
            commandParameterList.Add(new SqlParameter("StackHeightMax", syncRequest.StackHeightMax));
            commandParameterList.Add(new SqlParameter("SupportsRearLoading", syncRequest.SupportsRearLoading));
            commandParameterList.Add(new SqlParameter("SupportsSideLoading", syncRequest.SupportsSideLoading));
            commandParameterList.Add(new SqlParameter("SupportsJumboVehicles", syncRequest.SupportsJumboVehicles));
            commandParameterList.Add(new SqlParameter("SupportsIndividual", null));
            commandParameterList.Add(new SqlParameter("BasePalletTypeId", basePalletTypeId));
            commandParameterList.Add(new SqlParameter("BaseQualityId", baseQualityId));
            commandParameterList.Add(new SqlParameter("ShowOnline", false));
            commandParameterList.Add(new SqlParameter("IsCreatedOnline", true));
            commandParameterList.Add(new SqlParameter("NeedsBalanceApproval", false)); // TODO
            commandParameterList.Add(new SqlParameter("BalanceApproved", false));
            commandParameterList.Add(new SqlParameter("LiveDocumentNumber", syncRequest.OrderNumber));
            commandParameterList.Add(new SqlParameter("ExpressCode", ""));
            commandParameterList.Add(new SqlParameter("OnlineComment", "")); // TODO
            commandParameterList.Add(new SqlParameter("SupportsPartialMatching", syncRequest.SupportsPartialMatching));
            commandParameterList.Add(new SqlParameter("HasBasePallets", syncRequest.RefLmsBaseLoadCarrierId != 0));
            commandParameterList.Add(new SqlParameter("LoadingLocationId", syncRequest.LoadingLocationId));
            commandParameterList.Add(new SqlParameter("LtmsAccountId", syncRequest.RefLtmsAccountId));
            commandParameterList.Add(new SqlParameter("LtmsAccountNumber", syncRequest.RefLtmsAccountNumber));
            #endregion

            // Decimal Konvertierung wird wegen komischer Fehlermeldung beim direkten Konvertieren in int benötigt.
            newDeliveryId = Decimal.ToInt32((decimal)SqlHelper.ExecuteScalar(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray()));
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (!config.DevelopmentEnvironment)
            {
            // Insert DeliveryDetails
            if (syncRequest.EarliestFulfillmentDateTime.Date != syncRequest.LatestFulfillmentDateTime.Date)
               {
               return InsertDeliveryDetails(config, CurrentSqlTransaction, newDeliveryId, syncRequest, out errorReason, out errorDescription);
               }
            }
         if (syncRequest.RefLmsPermanentAvailabilityRowGuid != null)
            {
            // Zuordnung an Dauerverfügbarkeit, zunächst Kopie von Dauerverfügbarkeit erzeugen
            if (!InsertPermanentAvailabilityChild(config, CurrentSqlTransaction, syncRequest, out errorReason, out errorDescription))
               {
               return false;
               }
            }
         if (syncRequest.RefLmsAvailabilityRowGuid != null)
            {
            // Zuordnung erzeugen
            if (!AllocateAvailabilityAndDelivery(config, CurrentSqlTransaction, syncRequest, out errorReason, out errorDescription))
               {
               return false;
               }
            }

         return true;
         }

      public static bool InsertDeliveryDetails(Config config, SqlTransaction CurrentSqlTransaction, int deliveryId, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         if (syncRequest.Type != OrderType.Demand)
            {
            return false;
            }

         string QueryCommandString = @"
INSERT INTO [" + config.LMSSchemaName + @"].[LMS_DELIVERYDETAIL]
           ([DeliveryId]
           ,[Date]
           ,[Quantity]
           ,[year]
           ,[month]
           ,[cw]
           ,[Finished]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy])
     VALUES
           (@DeliveryId
           ,@Date
           ,@Quantity
           ,@year
           ,@month
           ,@cw
           ,@Finished
           ,@CreationDate
           ,@ModificationDate
           ,@DeletionDate
           ,@CreatedBy
           ,@ModifiedBy
           ,@DeletedBy)
	";

         DateTime dt = syncRequest.EarliestFulfillmentDateTime.Date;

         while (dt.Date <= syncRequest.LatestFulfillmentDateTime.Date)
            {

            try
               {
               #region SqlParams
               commandParameterList.Clear();
               commandParameterList.Add(new SqlParameter("DeliveryId", deliveryId));
               commandParameterList.Add(new SqlParameter("Date", dt.Date));
               commandParameterList.Add(new SqlParameter("Quantity", syncRequest.LoadCarrierQuantity));
               commandParameterList.Add(new SqlParameter("year", dt.Year));
               commandParameterList.Add(new SqlParameter("month", dt.Month));
               commandParameterList.Add(new SqlParameter("cw", CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)));
               commandParameterList.Add(new SqlParameter("finished", false));
               commandParameterList.Add(new SqlParameter("ZipCode", -1));
               commandParameterList.Add(new SqlParameter("CreationDate", DateTime.Now));
               commandParameterList.Add(new SqlParameter("ModificationDate", null));
               commandParameterList.Add(new SqlParameter("DeletionDate", null));
               commandParameterList.Add(new SqlParameter("CreatedBy", LiveDBTools.LMSServiceUser));
               commandParameterList.Add(new SqlParameter("ModifiedBy", null));
               commandParameterList.Add(new SqlParameter("DeletedBy", null));
               #endregion

               int NumAffectedRows = 0;
               NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
               }
            catch (Exception ex)
               {
               errorReason = ServiceMain.ErrorReasonException;
               errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
               return false;
               }
            dt = dt.AddDays(1);
            }

         return true;
         }

      public static bool CancelDelivery(Config config, SqlTransaction CurrentSqlTransaction, OrderCancelSyncRequest cancelRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         if (cancelRequest.Type != OrderType.Demand)
            {
            return false;
            }

         string QueryCommandString = @"
         UPDATE [" + config.LMSSchemaName + @"].[LMS_DELIVERY]
         SET
            Storniert = @Storniert,
            StorniertVon = @StorniertVon,
            StorniertDatum = @StorniertDatum,
            StorniertKommentar = ISNULL(StorniertKommentar, '') + (CASE WHEN ISNULL(StorniertKommentar, '') = '' THEN '' ELSE CHAR(13) END) + @StorniertKommentar,
            Revision = Revision + 1
         WHERE 
            (RowGuid = @OrderRowGuid)";
         if (!cancelRequest.IsApproved)
            {
            QueryCommandString += @" 
            AND (Storniert IS NULL OR Storniert = 0)
            AND (DeletedBy IS NULL)
            AND (SELECT ISNULL(SUM(Quantity), 0) FROM [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI] WHERE DeliveryID = (SELECT DiliveryID FROM [" + config.LMSSchemaName + @"].[LMS_DELIVERY] WHERE RowGuid = @OrderRowGuid) AND DeletedBy IS NULL AND FrachtpapiereErstellt = 1) = 0
            "; // TODO VersionCol prüfen
            }

         // Folgende Kriterien erstmal entfernt
         //AND(finished = 0)
         //AND(Geblockt IS NULL OR Geblockt = 0)
         //AND(InBearbeitungVon IS NULL)

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("OrderRowGuid", cancelRequest.OrderRowGuid));
            commandParameterList.Add(new SqlParameter("Storniert", 1));
            commandParameterList.Add(new SqlParameter("StorniertVon", cancelRequest.Person == null ? "" : cancelRequest.Person.LastName + ", " + cancelRequest.Person.FirstName));
            commandParameterList.Add(new SqlParameter("StorniertDatum", DateTime.Now));
            commandParameterList.Add(new SqlParameter("StorniertKommentar", "(ONLINE): " + cancelRequest.Note ?? ""));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (NumAffectedRows == 0)
            {
            // Prüfung, an welcher Spalte es gescheitert ist
            QueryCommandString = @"
            SELECT
               RowGuid 
               , Storniert
               , finished
               , DeletedBy
               , Geblockt
               , InBearbeitungVon
               , (SELECT ISNULL(SUM(Quantity), 0) FROM [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI] WHERE DeliveryID = (SELECT DiliveryID FROM [" + config.LMSSchemaName + @"].[LMS_DELIVERY] WHERE RowGuid = @OrderRowGuid) AND DeletedBy IS NULL AND FrachtpapiereErstellt = 1) AS NumZugeordnet
            FROM [" + config.LMSSchemaName + @"].[LMS_DELIVERY]
            WHERE 
               (RowGuid = @OrderRowGuid) 
            ";

            commandParameterList.Clear();
            commandParameterList.Add(new SqlParameter("OrderRowGuid", cancelRequest.OrderRowGuid));

            DataSet dataRows = new DataSet();
            SqlHelper.FillDataset(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), dataRows, null, commandParameterList.ToArray());
            if (dataRows.Tables.Count != 1 || dataRows.Tables[0].Rows.Count != 1)
               {
               // Datensatz nicht gefunden
               errorReason = ServiceMain.ErrorReasonRowNotFound;
               errorDescription = ServiceMain.ErrorDescriptionRowNotFound + cancelRequest.OrderRowGuid + " (LMS_DELIVERY)";
               return false;
               }
            DataRow row = dataRows.Tables[0].Rows[0];
            // Storniert
            if (row.Field<bool?>("Storniert") == true)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "Storniert";
               return false;
               }
            // finished
            if (row.Field<bool?>("finished") == true)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "finished";
               return false;
               }
            // DeletedBy
            if (row.Field<string>("DeletedBy") != null)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "DeletedBy";
               return false;
               }
            // Geblockt
            if (row.Field<bool?>("Geblockt") == true)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "Geblockt";
               return false;
               }
            // InBearbeitungVon
            if (row.Field<string>("InBearbeitungVon") != null)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "InBearbeitungVon";
               return false;
               }
            // NumZugeordnet
            if (row.Field<int>("NumZugeordnet") != 0)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "NumZugeordnet";
               return false;
               }
            errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
            errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "UnKnown";
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool CheckAvailability(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         errorReason = "";
         errorDescription = "";

         // Check CustomerNumber
         if (!CheckCustomerIDExists(config, CurrentSqlTransaction, syncRequest.CustomerNumber))
            {
            errorReason = ServiceMain.ErrorReasonCustomerNumber;
            errorDescription = ServiceMain.ErrorDescriptionCustomerNumber + syncRequest.CustomerNumber;
            return false;
            }

         // Check CustomerNumberLoadingLocation
         if (syncRequest.TransportType == OrderTransportType.ProvidedByOthers && !CheckCustomerIDExists(config, CurrentSqlTransaction, syncRequest.CustomerNumberLoadingLocation))
            {
            errorReason = ServiceMain.ErrorReasonCustomerNumberLoadingLocation;
            errorDescription = ServiceMain.ErrorDescriptionCustomerNumberLoadingLocation + syncRequest.CustomerNumberLoadingLocation;
            return false;
            }

         // Check RefLmsLoadCarrierId
         if (syncRequest.RefLmsLoadCarrierId == 0)
            {
            errorReason = ServiceMain.ErrorReasonRefLmsLoadCarrierId;
            errorDescription = ServiceMain.ErrorDescriptionRefLmsLoadCarrierId;
            return false;
            }

         return true;
         }

      public static bool CheckPermanentAvailability(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         errorReason = "";
         errorDescription = "";

         // Check CustomerNumber
         if (!CheckCustomerIDExists(config, CurrentSqlTransaction, syncRequest.CustomerNumber))
            {
            errorReason = ServiceMain.ErrorReasonCustomerNumber;
            errorDescription = ServiceMain.ErrorDescriptionCustomerNumber + syncRequest.CustomerNumber;
            return false;
            }

         // Check RefLmsLoadCarrierId
         if (syncRequest.RefLmsLoadCarrierId == 0)
            {
            errorReason = ServiceMain.ErrorReasonRefLmsLoadCarrierId;
            errorDescription = ServiceMain.ErrorDescriptionRefLmsLoadCarrierId;
            return false;
            }

         return true;
         }

      public static bool InsertAvailability(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         if (syncRequest.Type != OrderType.Supply)
            {
            return false;
            }

         // Artikel und Qualität aus RefLmsLoadCarrierId und RefLmsBaseLoadCarrierId ermitteln
         int quality = syncRequest.RefLmsLoadCarrierId % 1000;
         int palletType = syncRequest.RefLmsLoadCarrierId / 1000;
         int baseQualityId = 0;
         int basePalletTypeId = 0;
         if (syncRequest.RefLmsBaseLoadCarrierId != 0)
            {
            baseQualityId = (int)syncRequest.RefLmsBaseLoadCarrierId % 1000;
            basePalletTypeId = (int)syncRequest.RefLmsBaseLoadCarrierId / 1000;
            }

         string QueryCommandString = @"
INSERT INTO [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
           ([ZipCode]
           ,[Quantity]
           ,[TempAlloc]
           ,[TempReserved]
           ,[BBD]
           ,[IsManual]
           ,[TypeId]
           ,[ClientId]
           ,[Reliability]
           ,[ReliabilityQuantity]
           ,[ReliabilityQuality]
           ,[AvailableFromDate]
           ,[AvailableUntilDate]
           ,[AvailableFromTime]
           ,[AvailableUntilTime]
           ,[ContactId]
           ,[LoadingPointId]
           ,[QualityId]
           ,[AvailabilityTypeId]
           ,[CommentText]
           ,[CountryId]
           ,[AvailabilitySubTypeId]
           ,[PalletTypeId]
           ,[ContactAddressInfo]
           ,[ContractNo]
           ,[DeliveryNoteNo]
           ,[TransactionNo]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[ContactPerson]
           ,[ContactDetails]
           ,[RepositoryHours]
           ,[finished]
           ,[IsMixedAvailability]
           ,[LSManAdrAnrede]
           ,[LSManAdrName1]
           ,[LSManAdrName2]
           ,[LSManAdrName3]
           ,[LSManAdrPLZ]
           ,[LSManAdrOrt]
           ,[LSManAdrStrasse]
           ,[LSManAdrLand]
           ,[PalettenProMonat]
           ,[PalettenProWoche]
           ,[Notiz]
           ,[Geblockt]
           ,[GeblocktVon]
           ,[GeblocktDatum]
           ,[GeblocktAufgehobenVon]
           ,[GeblocktAufgehobenDatum]
           ,[GeblocktKommentar]
           ,[GeblocktFuer]
           ,[InBearbeitungVon]
           ,[InBearbeitungDatumZeit]
           ,[Revision]
           ,[Storniert]
           ,[StorniertVon]
           ,[StorniertDatum]
           ,[StorniertKommentar]
           ,[Rejection]
           ,[Reklamation]
           ,[RejectionReasonID]
           ,[RejectionComment]
           ,[RejectionQuantity]
           ,[RejectionApproachID]
           ,[RejectionBy]
           ,[RejectionDate]
           ,[RejectionDone]
           ,[RejectionDoneComment]
           ,[RejectionNextReceiptNo]
           ,[RejectionDoneBy]
           ,[RejectionDoneDate]
           ,[RejectionCost]
           ,[ICDroht]
           ,[ICDrohtVon]
           ,[ICDrohtAm]
           ,[ICDurchfuehren]
           ,[ICDurchfuehrenVon]
           ,[ICDurchfuehrenAm]
           ,[ZuAvisieren]
           ,[IstAvisiert]
           ,[AvisiertVon]
           ,[AvisiertAm]
           ,[IstAusAblehnung]
           ,[IcGrundID]
           ,[OrderManagementID]
           ,[RowGuid]
           ,[StackHeightMin]
           ,[StackHeightMax]
           ,[SupportsRearLoading]
           ,[SupportsSideLoading]
           ,[SupportsJumboVehicles]
           ,[SupportsIndividual]
           ,[BasePalletTypeId]
           ,[BaseQualityId]
           ,[ShowOnline]
           ,[IsCreatedOnline]
           ,[SupportsPartialMatching]
           ,[IsHotspotOffer]
           ,[HotspotComment]
           ,[HotspotOfferUntil]
           ,[LiveDocumentNumber]
           ,[ExpressCode]
           ,[OnlineComment]
           ,[HasBasePallets]
           ,[LoadingLocationId]
           ,[LtmsAccountId]
           ,[LtmsAccountNumber]
           ,[SelfDelivery]
           )
     VALUES
           (@ZipCode
           ,@Quantity
           ,@TempAlloc
           ,@TempReserved
           ,@BBD
           ,@IsManual
           ,@TypeId
           ,@ClientId
           ,@Reliability
           ,@ReliabilityQuantity
           ,@ReliabilityQuality
           ,@AvailableFromDate
           ,@AvailableUntilDate
           ,@AvailableFromTime
           ,@AvailableUntilTime
           ,@ContactId
           ,@LoadingPointId
           ,@QualityId
           ,@AvailabilityTypeId
           ,@CommentText
           ,@CountryId
           ,@AvailabilitySubTypeId
           ,@PalletTypeId
           ,@ContactAddressInfo
           ,@ContractNo
           ,@DeliveryNoteNo
           ,@TransactionNo
           ,@CreationDate
           ,@ModificationDate
           ,@DeletionDate
           ,@CreatedBy
           ,@ModifiedBy
           ,@DeletedBy
           ,@ContactPerson
           ,@ContactDetails
           ,@RepositoryHours
           ,@finished
           ,@IsMixedAvailability
           ,@LSManAdrAnrede
           ,@LSManAdrName1
           ,@LSManAdrName2
           ,@LSManAdrName3
           ,@LSManAdrPLZ
           ,@LSManAdrOrt
           ,@LSManAdrStrasse
           ,@LSManAdrLand
           ,@PalettenProMonat
           ,@PalettenProWoche
           ,@Notiz
           ,@Geblockt
           ,@GeblocktVon
           ,@GeblocktDatum
           ,@GeblocktAufgehobenVon
           ,@GeblocktAufgehobenDatum
           ,@GeblocktKommentar
           ,@GeblocktFuer
           ,@InBearbeitungVon
           ,@InBearbeitungDatumZeit
           ,@Revision
           ,@Storniert
           ,@StorniertVon
           ,@StorniertDatum
           ,@StorniertKommentar
           ,@Rejection
           ,@Reklamation
           ,@RejectionReasonID
           ,@RejectionComment
           ,@RejectionQuantity
           ,@RejectionApproachID
           ,@RejectionBy
           ,@RejectionDate
           ,@RejectionDone
           ,@RejectionDoneComment
           ,@RejectionNextReceiptNo
           ,@RejectionDoneBy
           ,@RejectionDoneDate
           ,@RejectionCost
           ,@ICDroht
           ,@ICDrohtVon
           ,@ICDrohtAm
           ,@ICDurchfuehren
           ,@ICDurchfuehrenVon
           ,@ICDurchfuehrenAm
           ,@ZuAvisieren
           ,@IstAvisiert
           ,@AvisiertVon
           ,@AvisiertAm
           ,@IstAusAblehnung
           ,@IcGrundID
           ,@OrderManagementID
           ,@RowGuid
           ,@StackHeightMin
           ,@StackHeightMax
           ,@SupportsRearLoading
           ,@SupportsSideLoading
           ,@SupportsJumboVehicles
           ,@SupportsIndividual
           ,@BasePalletTypeId
           ,@BaseQualityId
           ,@ShowOnline
           ,@IsCreatedOnline
           ,@SupportsPartialMatching
           ,@IsHotspotOffer
           ,@HotspotComment
           ,@HotspotOfferUntil
           ,@LiveDocumentNumber
           ,@HotspotOfferUntil
           ,@OnlineComment
           ,@HasBasePallets
           ,@LoadingLocationId
           ,@LtmsAccountId
           ,@LtmsAccountNumber
           ,@SelfDelivery
	)";

         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("ZipCode", syncRequest.Address?.PostalCode ?? ""));
            commandParameterList.Add(new SqlParameter("Quantity", syncRequest.LoadCarrierQuantity));
            commandParameterList.Add(new SqlParameter("TempAlloc", (object)0));
            commandParameterList.Add(new SqlParameter("TempReserved", (object)0));
            commandParameterList.Add(new SqlParameter("BBD", null));
            commandParameterList.Add(new SqlParameter("IsManual", true));
            commandParameterList.Add(new SqlParameter("TypeId", 4)); // Freistellung TODO
            commandParameterList.Add(new SqlParameter("ClientId", LiveDBTools.LMSClientID));
            commandParameterList.Add(new SqlParameter("Reliability", -1));
            commandParameterList.Add(new SqlParameter("ReliabilityQuantity", -1));
            commandParameterList.Add(new SqlParameter("ReliabilityQuality", -1));
            commandParameterList.Add(new SqlParameter("AvailableFromDate", syncRequest.EarliestFulfillmentDateTime.Date));
            commandParameterList.Add(new SqlParameter("AvailableUntilDate", syncRequest.LatestFulfillmentDateTime.Date));
            commandParameterList.Add(new SqlParameter("AvailableFromTime", "00:00"));
            commandParameterList.Add(new SqlParameter("AvailableUntilTime", "00:00"));
            commandParameterList.Add(new SqlParameter("ContactId", syncRequest.CustomerNumber));
            commandParameterList.Add(new SqlParameter("LoadingPointId", syncRequest.CustomerNumberLoadingLocation != 0 ? syncRequest.CustomerNumberLoadingLocation : syncRequest.CustomerNumber));
            commandParameterList.Add(new SqlParameter("QualityId", quality));
            commandParameterList.Add(new SqlParameter("AvailabilityTypeId", GetAvailabilityTypeIdFromAccountType(syncRequest.RefLtmsAccountNumber)));
            commandParameterList.Add(new SqlParameter("CommentText", ""));
            commandParameterList.Add(new SqlParameter("CountryId", 1)); // TODO
            commandParameterList.Add(new SqlParameter("AvailabilitySubTypeId", (object)0));
            commandParameterList.Add(new SqlParameter("PalletTypeId", palletType));
            commandParameterList.Add(new SqlParameter("ContactAddressInfo", ""));
            commandParameterList.Add(new SqlParameter("ContractNo", syncRequest.VauNumber ?? ""));
            commandParameterList.Add(new SqlParameter("DeliveryNoteNo", ""));
            commandParameterList.Add(new SqlParameter("TransactionNo", ""));
            commandParameterList.Add(new SqlParameter("CreationDate", DateTime.Now));
            commandParameterList.Add(new SqlParameter("ModificationDate", null));
            commandParameterList.Add(new SqlParameter("DeletionDate", null));
            commandParameterList.Add(new SqlParameter("CreatedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModifiedBy", null));
            commandParameterList.Add(new SqlParameter("DeletedBy", null));
            commandParameterList.Add(new SqlParameter("ContactPerson", syncRequest.Person.LastName + ", " + syncRequest.Person.FirstName));
            commandParameterList.Add(new SqlParameter("ContactDetails", "Tel: " + syncRequest.Person.PhoneNumber + "; EMail: " + syncRequest.Person.Email));
            commandParameterList.Add(new SqlParameter("RepositoryHours", syncRequest.BusinessHoursString == null ? "" : syncRequest.BusinessHoursString.Substring(0, 128)));
            commandParameterList.Add(new SqlParameter("finished", false));
            commandParameterList.Add(new SqlParameter("IsMixedAvailability", false));
            commandParameterList.Add(new SqlParameter("LSManAdrAnrede", "")); // Ladestelle
            commandParameterList.Add(new SqlParameter("LSManAdrName1", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrName2", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrName3", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrPLZ", syncRequest.Address?.PostalCode ?? ""));
            commandParameterList.Add(new SqlParameter("LSManAdrOrt", syncRequest.Address?.City ?? ""));
            commandParameterList.Add(new SqlParameter("LSManAdrStrasse", syncRequest.Address?.Street1 ?? ""));
            commandParameterList.Add(new SqlParameter("LSManAdrLand", syncRequest.Address?.CountryName ?? ""));
            commandParameterList.Add(new SqlParameter("PalettenProMonat", (object)0));
            commandParameterList.Add(new SqlParameter("PalettenProWoche", (object)0));
            commandParameterList.Add(new SqlParameter("Notiz", ""));
            commandParameterList.Add(new SqlParameter("Geblockt", false));
            commandParameterList.Add(new SqlParameter("GeblocktVon", null));
            commandParameterList.Add(new SqlParameter("GeblocktDatum", null));
            commandParameterList.Add(new SqlParameter("GeblocktAufgehobenVon", null));
            commandParameterList.Add(new SqlParameter("GeblocktAufgehobenDatum", null));
            commandParameterList.Add(new SqlParameter("GeblocktKommentar", null));
            commandParameterList.Add(new SqlParameter("GeblocktFuer", null));
            commandParameterList.Add(new SqlParameter("InBearbeitungVon", null));
            commandParameterList.Add(new SqlParameter("InBearbeitungDatumZeit", null));
            commandParameterList.Add(new SqlParameter("Revision", (object)0));
            commandParameterList.Add(new SqlParameter("Storniert", false));
            commandParameterList.Add(new SqlParameter("StorniertVon", null));
            commandParameterList.Add(new SqlParameter("StorniertDatum", null));
            commandParameterList.Add(new SqlParameter("StorniertKommentar", null));
            commandParameterList.Add(new SqlParameter("Rejection", false));
            commandParameterList.Add(new SqlParameter("Reklamation", false));
            commandParameterList.Add(new SqlParameter("RejectionReasonID", null));
            commandParameterList.Add(new SqlParameter("RejectionComment", ""));
            commandParameterList.Add(new SqlParameter("RejectionQuantity", (object)0));
            commandParameterList.Add(new SqlParameter("RejectionApproachID", null));
            commandParameterList.Add(new SqlParameter("RejectionBy", null));
            commandParameterList.Add(new SqlParameter("RejectionDate", null));
            commandParameterList.Add(new SqlParameter("RejectionDone", false));
            commandParameterList.Add(new SqlParameter("RejectionDoneComment", ""));
            commandParameterList.Add(new SqlParameter("RejectionNextReceiptNo", ""));
            commandParameterList.Add(new SqlParameter("RejectionDoneBy", null));
            commandParameterList.Add(new SqlParameter("RejectionDoneDate", null));
            commandParameterList.Add(new SqlParameter("RejectionCost", (object)0));
            commandParameterList.Add(new SqlParameter("ICDroht", false));
            commandParameterList.Add(new SqlParameter("ICDrohtVon", null));
            commandParameterList.Add(new SqlParameter("ICDrohtAm", null));
            commandParameterList.Add(new SqlParameter("ICDurchfuehren", false));
            commandParameterList.Add(new SqlParameter("ICDurchfuehrenVon", null));
            commandParameterList.Add(new SqlParameter("ICDurchfuehrenAm", null));
            commandParameterList.Add(new SqlParameter("ZuAvisieren", false));
            commandParameterList.Add(new SqlParameter("IstAvisiert", false));
            commandParameterList.Add(new SqlParameter("AvisiertVon", null));
            commandParameterList.Add(new SqlParameter("AvisiertAm", null));
            commandParameterList.Add(new SqlParameter("IstAusAblehnung", false));
            commandParameterList.Add(new SqlParameter("IcGrundID", 1));
            commandParameterList.Add(new SqlParameter("OrderManagementID", ""));
            commandParameterList.Add(new SqlParameter("RowGuid", syncRequest.OrderRowGuid));
            commandParameterList.Add(new SqlParameter("StackHeightMin", syncRequest.StackHeightMin));
            commandParameterList.Add(new SqlParameter("StackHeightMax", syncRequest.StackHeightMax));
            commandParameterList.Add(new SqlParameter("SupportsRearLoading", syncRequest.SupportsRearLoading));
            commandParameterList.Add(new SqlParameter("SupportsSideLoading", syncRequest.SupportsSideLoading));
            commandParameterList.Add(new SqlParameter("SupportsJumboVehicles", syncRequest.SupportsJumboVehicles));
            commandParameterList.Add(new SqlParameter("SupportsIndividual", null));
            commandParameterList.Add(new SqlParameter("BasePalletTypeId", basePalletTypeId));
            commandParameterList.Add(new SqlParameter("BaseQualityId", baseQualityId));
            //commandParameterList.Add(new SqlParameter("BaseQuantity", syncRequest.RefLmsBaseLoadCarrierId != null && syncRequest.RefLmsBaseLoadCarrierId != 0 ? syncRequest.NumberOfStacks : (object)0));
            commandParameterList.Add(new SqlParameter("ShowOnline", false));
            commandParameterList.Add(new SqlParameter("IsCreatedOnline", true));
            commandParameterList.Add(new SqlParameter("SupportsPartialMatching", syncRequest.SupportsPartialMatching));
            commandParameterList.Add(new SqlParameter("IsHotspotOffer", false));
            commandParameterList.Add(new SqlParameter("HotspotComment", ""));
            commandParameterList.Add(new SqlParameter("HotspotOfferUntil", null));
            commandParameterList.Add(new SqlParameter("LiveDocumentNumber", syncRequest.OrderNumber));
            commandParameterList.Add(new SqlParameter("ExpressCode", ""));
            commandParameterList.Add(new SqlParameter("OnlineComment", "")); // TODO
            commandParameterList.Add(new SqlParameter("HasBasePallets", (syncRequest.RefLmsBaseLoadCarrierId ?? 0) != 0));
            commandParameterList.Add(new SqlParameter("LoadingLocationId", syncRequest.LoadingLocationId));
            commandParameterList.Add(new SqlParameter("LtmsAccountId", syncRequest.RefLtmsAccountId));
            commandParameterList.Add(new SqlParameter("LtmsAccountNumber", syncRequest.RefLtmsAccountNumber));
            commandParameterList.Add(new SqlParameter("SelfDelivery", syncRequest.TransportType == OrderTransportType.Self));
            #endregion

            int NumAffectedRows = 0;
            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (syncRequest.RefLmsDeliveryRowGuid != null)
            {
            // Zuordnung erzeugen
            if (!AllocateAvailabilityAndDelivery(config, CurrentSqlTransaction, syncRequest, out errorReason, out errorDescription))
               {
               return false;
               }
            }

         return true;
         }

      public static bool InsertPermanentAvailability(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         if (syncRequest.Type != OrderType.Supply || !syncRequest.IsPermanent)
            {
            return false;
            }

         // Artikel und Qualität aus RefLmsLoadCarrierId und RefLmsBaseLoadCarrierId ermitteln
         int quality = syncRequest.RefLmsLoadCarrierId % 1000;
         int palletType = syncRequest.RefLmsLoadCarrierId / 1000;
         int baseQualityId = 0;
         int basePalletTypeId = 0;
         if (syncRequest.RefLmsBaseLoadCarrierId != null && syncRequest.RefLmsBaseLoadCarrierId != 0)
            {
            baseQualityId = (int)syncRequest.RefLmsBaseLoadCarrierId % 1000;
            basePalletTypeId = (int)syncRequest.RefLmsBaseLoadCarrierId / 1000;
            }

         // Evtl. Default LoadingLocationID ermitteln
         if (syncRequest.LoadingLocationId == null || syncRequest.LoadingLocationId == 0)
            {
            if(!SetDefaultLoadingLocationIdFromCustomerID(config, CurrentSqlTransaction, syncRequest,
               out errorReason, out errorDescription))
               {
               return false;
               }
            }

         // Prüfen, ob Verfügbarkeit bereits vorhanden
         if (CheckPermanentAvailabilityExists(config, CurrentSqlTransaction, syncRequest))
            {
            errorReason = ServiceMain.ErrorReasonPermanentAvailabilityAlreadyExists;
            errorDescription = ServiceMain.ErrorDescriptionPermanentAvailabilityAlreadyExists
                + "AccountNumber " + syncRequest.RefLtmsAccountNumber
                + ", LmsLoadCarrierId: " + syncRequest.RefLmsLoadCarrierId
                + ", LmsBaseLoadCarrierId: " + (int)syncRequest.RefLmsBaseLoadCarrierId
                + ", LoadingLocationId: " + syncRequest.LoadingLocationId;
            return false;
            }

         string QueryCommandString = @"
INSERT INTO [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
           ([ZipCode]
           ,[Quantity]
           ,[TempAlloc]
           ,[TempReserved]
           ,[BBD]
           ,[IsManual]
           ,[TypeId]
           ,[ClientId]
           ,[Reliability]
           ,[ReliabilityQuantity]
           ,[ReliabilityQuality]
           ,[AvailableFromDate]
           ,[AvailableUntilDate]
           ,[AvailableFromTime]
           ,[AvailableUntilTime]
           ,[ContactId]
           ,[LoadingPointId]
           ,[QualityId]
           ,[AvailabilityTypeId]
           ,[CommentText]
           ,[CountryId]
           ,[AvailabilitySubTypeId]
           ,[PalletTypeId]
           ,[ContactAddressInfo]
           ,[ContractNo]
           ,[DeliveryNoteNo]
           ,[TransactionNo]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[ContactPerson]
           ,[ContactDetails]
           ,[RepositoryHours]
           ,[finished]
           ,[IsMixedAvailability]
           ,[LSManAdrAnrede]
           ,[LSManAdrName1]
           ,[LSManAdrName2]
           ,[LSManAdrName3]
           ,[LSManAdrPLZ]
           ,[LSManAdrOrt]
           ,[LSManAdrStrasse]
           ,[LSManAdrLand]
           ,[PalettenProMonat]
           ,[PalettenProWoche]
           ,[Notiz]
           ,[Geblockt]
           ,[GeblocktVon]
           ,[GeblocktDatum]
           ,[GeblocktAufgehobenVon]
           ,[GeblocktAufgehobenDatum]
           ,[GeblocktKommentar]
           ,[GeblocktFuer]
           ,[InBearbeitungVon]
           ,[InBearbeitungDatumZeit]
           ,[Revision]
           ,[Storniert]
           ,[StorniertVon]
           ,[StorniertDatum]
           ,[StorniertKommentar]
           ,[Rejection]
           ,[Reklamation]
           ,[RejectionReasonID]
           ,[RejectionComment]
           ,[RejectionQuantity]
           ,[RejectionApproachID]
           ,[RejectionBy]
           ,[RejectionDate]
           ,[RejectionDone]
           ,[RejectionDoneComment]
           ,[RejectionNextReceiptNo]
           ,[RejectionDoneBy]
           ,[RejectionDoneDate]
           ,[RejectionCost]
           ,[ICDroht]
           ,[ICDrohtVon]
           ,[ICDrohtAm]
           ,[ICDurchfuehren]
           ,[ICDurchfuehrenVon]
           ,[ICDurchfuehrenAm]
           ,[ZuAvisieren]
           ,[IstAvisiert]
           ,[AvisiertVon]
           ,[AvisiertAm]
           ,[IstAusAblehnung]
           ,[IcGrundID]
           ,[OrderManagementID]
           ,[RowGuid]
           ,[StackHeightMin]
           ,[StackHeightMax]
           ,[SupportsRearLoading]
           ,[SupportsSideLoading]
           ,[SupportsJumboVehicles]
           ,[SupportsIndividual]
           ,[BasePalletTypeId]
           ,[BaseQualityId]
           ,[ShowOnline]
           ,[IsCreatedOnline]
           ,[SupportsPartialMatching]
           ,[IsHotspotOffer]
           ,[HotspotComment]
           ,[HotspotOfferUntil]
           ,[LiveDocumentNumber]
           ,[ExpressCode]
           ,[OnlineComment]
           ,[HasBasePallets]
           ,[LoadingLocationId]
           ,[LtmsAccountId]
           ,[LtmsAccountNumber]
           ,[SelfDelivery]
           ,[IsPermanent]
           )
     VALUES
           (@ZipCode
           ,@Quantity
           ,@TempAlloc
           ,@TempReserved
           ,@BBD
           ,@IsManual
           ,@TypeId
           ,@ClientId
           ,@Reliability
           ,@ReliabilityQuantity
           ,@ReliabilityQuality
           ,@AvailableFromDate
           ,@AvailableUntilDate
           ,@AvailableFromTime
           ,@AvailableUntilTime
           ,@ContactId
           ,@LoadingPointId
           ,@QualityId
           ,@AvailabilityTypeId
           ,@CommentText
           ,@CountryId
           ,@AvailabilitySubTypeId
           ,@PalletTypeId
           ,@ContactAddressInfo
           ,@ContractNo
           ,@DeliveryNoteNo
           ,@TransactionNo
           ,@CreationDate
           ,@ModificationDate
           ,@DeletionDate
           ,@CreatedBy
           ,@ModifiedBy
           ,@DeletedBy
           ,@ContactPerson
           ,@ContactDetails
           ,@RepositoryHours
           ,@finished
           ,@IsMixedAvailability
           ,@LSManAdrAnrede
           ,@LSManAdrName1
           ,@LSManAdrName2
           ,@LSManAdrName3
           ,@LSManAdrPLZ
           ,@LSManAdrOrt
           ,@LSManAdrStrasse
           ,@LSManAdrLand
           ,@PalettenProMonat
           ,@PalettenProWoche
           ,@Notiz
           ,@Geblockt
           ,@GeblocktVon
           ,@GeblocktDatum
           ,@GeblocktAufgehobenVon
           ,@GeblocktAufgehobenDatum
           ,@GeblocktKommentar
           ,@GeblocktFuer
           ,@InBearbeitungVon
           ,@InBearbeitungDatumZeit
           ,@Revision
           ,@Storniert
           ,@StorniertVon
           ,@StorniertDatum
           ,@StorniertKommentar
           ,@Rejection
           ,@Reklamation
           ,@RejectionReasonID
           ,@RejectionComment
           ,@RejectionQuantity
           ,@RejectionApproachID
           ,@RejectionBy
           ,@RejectionDate
           ,@RejectionDone
           ,@RejectionDoneComment
           ,@RejectionNextReceiptNo
           ,@RejectionDoneBy
           ,@RejectionDoneDate
           ,@RejectionCost
           ,@ICDroht
           ,@ICDrohtVon
           ,@ICDrohtAm
           ,@ICDurchfuehren
           ,@ICDurchfuehrenVon
           ,@ICDurchfuehrenAm
           ,@ZuAvisieren
           ,@IstAvisiert
           ,@AvisiertVon
           ,@AvisiertAm
           ,@IstAusAblehnung
           ,@IcGrundID
           ,@OrderManagementID
           ,@RowGuid
           ,@StackHeightMin
           ,@StackHeightMax
           ,@SupportsRearLoading
           ,@SupportsSideLoading
           ,@SupportsJumboVehicles
           ,@SupportsIndividual
           ,@BasePalletTypeId
           ,@BaseQualityId
           ,@ShowOnline
           ,@IsCreatedOnline
           ,@SupportsPartialMatching
           ,@IsHotspotOffer
           ,@HotspotComment
           ,@HotspotOfferUntil
           ,@LiveDocumentNumber
           ,@HotspotOfferUntil
           ,@OnlineComment
           ,@HasBasePallets
           ,@LoadingLocationId
           ,@LtmsAccountId
           ,@LtmsAccountNumber
           ,@SelfDelivery
           ,@IsPermanent
	)";

         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("ZipCode", syncRequest.Address?.PostalCode ?? ""));
            commandParameterList.Add(new SqlParameter("Quantity", syncRequest.LoadCarrierQuantity));
            commandParameterList.Add(new SqlParameter("TempAlloc", (object)0));
            commandParameterList.Add(new SqlParameter("TempReserved", (object)0));
            commandParameterList.Add(new SqlParameter("BBD", null));
            commandParameterList.Add(new SqlParameter("IsManual", true));
            commandParameterList.Add(new SqlParameter("TypeId", 2)); // Depotpartner
            commandParameterList.Add(new SqlParameter("ClientId", LiveDBTools.LMSClientID));
            commandParameterList.Add(new SqlParameter("Reliability", -1));
            commandParameterList.Add(new SqlParameter("ReliabilityQuantity", -1));
            commandParameterList.Add(new SqlParameter("ReliabilityQuality", -1));
            commandParameterList.Add(new SqlParameter("AvailableFromDate", syncRequest.EarliestFulfillmentDateTime.Date));
            commandParameterList.Add(new SqlParameter("AvailableUntilDate", syncRequest.LatestFulfillmentDateTime.Date));
            commandParameterList.Add(new SqlParameter("AvailableFromTime", "00:00")); // TODO?
            commandParameterList.Add(new SqlParameter("AvailableUntilTime", "00:00")); // TODO?
            commandParameterList.Add(new SqlParameter("ContactId", syncRequest.CustomerNumber));
            commandParameterList.Add(new SqlParameter("LoadingPointId", syncRequest.CustomerNumberLoadingLocation != 0 ? syncRequest.CustomerNumberLoadingLocation : syncRequest.CustomerNumber));
            commandParameterList.Add(new SqlParameter("QualityId", quality));
            commandParameterList.Add(new SqlParameter("AvailabilityTypeId", GetAvailabilityTypeIdFromAccountType(syncRequest.RefLtmsAccountNumber)));
            commandParameterList.Add(new SqlParameter("CommentText", ""));
            commandParameterList.Add(new SqlParameter("CountryId", 1)); // TODO
            commandParameterList.Add(new SqlParameter("AvailabilitySubTypeId", (object)0));
            commandParameterList.Add(new SqlParameter("PalletTypeId", palletType));
            commandParameterList.Add(new SqlParameter("ContactAddressInfo", ""));
            commandParameterList.Add(new SqlParameter("ContractNo", syncRequest.VauNumber ?? ""));
            commandParameterList.Add(new SqlParameter("DeliveryNoteNo", ""));
            commandParameterList.Add(new SqlParameter("TransactionNo", ""));
            commandParameterList.Add(new SqlParameter("CreationDate", DateTime.Now));
            commandParameterList.Add(new SqlParameter("ModificationDate", null));
            commandParameterList.Add(new SqlParameter("DeletionDate", null));
            commandParameterList.Add(new SqlParameter("CreatedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModifiedBy", null));
            commandParameterList.Add(new SqlParameter("DeletedBy", null));
            commandParameterList.Add(new SqlParameter("ContactPerson", syncRequest.Person != null ? syncRequest.Person.LastName + ", " + syncRequest.Person.FirstName : ""));
            commandParameterList.Add(new SqlParameter("ContactDetails", syncRequest.Person != null ? "Tel: " + syncRequest.Person.PhoneNumber + "; EMail: " + syncRequest.Person.Email : ""));
            commandParameterList.Add(new SqlParameter("RepositoryHours", syncRequest.BusinessHoursString == null ? "" : syncRequest.BusinessHoursString.Substring(0, 128)));
            commandParameterList.Add(new SqlParameter("finished", false));
            commandParameterList.Add(new SqlParameter("IsMixedAvailability", false));
            commandParameterList.Add(new SqlParameter("LSManAdrAnrede", "")); // Ladestelle
            commandParameterList.Add(new SqlParameter("LSManAdrName1", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrName2", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrName3", ""));
            commandParameterList.Add(new SqlParameter("LSManAdrPLZ", syncRequest.Address?.PostalCode ?? ""));
            commandParameterList.Add(new SqlParameter("LSManAdrOrt", syncRequest.Address?.City ?? ""));
            commandParameterList.Add(new SqlParameter("LSManAdrStrasse", syncRequest.Address?.Street1 ?? ""));
            commandParameterList.Add(new SqlParameter("LSManAdrLand", syncRequest.Address?.CountryName ?? ""));
            commandParameterList.Add(new SqlParameter("PalettenProMonat", (object)0));
            commandParameterList.Add(new SqlParameter("PalettenProWoche", (object)0));
            commandParameterList.Add(new SqlParameter("Notiz", ""));
            commandParameterList.Add(new SqlParameter("Geblockt", false));
            commandParameterList.Add(new SqlParameter("GeblocktVon", null));
            commandParameterList.Add(new SqlParameter("GeblocktDatum", null));
            commandParameterList.Add(new SqlParameter("GeblocktAufgehobenVon", null));
            commandParameterList.Add(new SqlParameter("GeblocktAufgehobenDatum", null));
            commandParameterList.Add(new SqlParameter("GeblocktKommentar", null));
            commandParameterList.Add(new SqlParameter("GeblocktFuer", null));
            commandParameterList.Add(new SqlParameter("InBearbeitungVon", null));
            commandParameterList.Add(new SqlParameter("InBearbeitungDatumZeit", null));
            commandParameterList.Add(new SqlParameter("Revision", (object)0));
            commandParameterList.Add(new SqlParameter("Storniert", false));
            commandParameterList.Add(new SqlParameter("StorniertVon", null));
            commandParameterList.Add(new SqlParameter("StorniertDatum", null));
            commandParameterList.Add(new SqlParameter("StorniertKommentar", null));
            commandParameterList.Add(new SqlParameter("Rejection", false));
            commandParameterList.Add(new SqlParameter("Reklamation", false));
            commandParameterList.Add(new SqlParameter("RejectionReasonID", null));
            commandParameterList.Add(new SqlParameter("RejectionComment", ""));
            commandParameterList.Add(new SqlParameter("RejectionQuantity", (object)0));
            commandParameterList.Add(new SqlParameter("RejectionApproachID", null));
            commandParameterList.Add(new SqlParameter("RejectionBy", null));
            commandParameterList.Add(new SqlParameter("RejectionDate", null));
            commandParameterList.Add(new SqlParameter("RejectionDone", false));
            commandParameterList.Add(new SqlParameter("RejectionDoneComment", ""));
            commandParameterList.Add(new SqlParameter("RejectionNextReceiptNo", ""));
            commandParameterList.Add(new SqlParameter("RejectionDoneBy", null));
            commandParameterList.Add(new SqlParameter("RejectionDoneDate", null));
            commandParameterList.Add(new SqlParameter("RejectionCost", (object)0));
            commandParameterList.Add(new SqlParameter("ICDroht", false));
            commandParameterList.Add(new SqlParameter("ICDrohtVon", null));
            commandParameterList.Add(new SqlParameter("ICDrohtAm", null));
            commandParameterList.Add(new SqlParameter("ICDurchfuehren", false));
            commandParameterList.Add(new SqlParameter("ICDurchfuehrenVon", null));
            commandParameterList.Add(new SqlParameter("ICDurchfuehrenAm", null));
            commandParameterList.Add(new SqlParameter("ZuAvisieren", false));
            commandParameterList.Add(new SqlParameter("IstAvisiert", false));
            commandParameterList.Add(new SqlParameter("AvisiertVon", null));
            commandParameterList.Add(new SqlParameter("AvisiertAm", null));
            commandParameterList.Add(new SqlParameter("IstAusAblehnung", false));
            commandParameterList.Add(new SqlParameter("IcGrundID", 1));
            commandParameterList.Add(new SqlParameter("OrderManagementID", ""));
            commandParameterList.Add(new SqlParameter("RowGuid", syncRequest.OrderRowGuid));
            commandParameterList.Add(new SqlParameter("StackHeightMin", syncRequest.StackHeightMin));
            commandParameterList.Add(new SqlParameter("StackHeightMax", syncRequest.StackHeightMax));
            commandParameterList.Add(new SqlParameter("SupportsRearLoading", syncRequest.SupportsRearLoading));
            commandParameterList.Add(new SqlParameter("SupportsSideLoading", syncRequest.SupportsSideLoading));
            commandParameterList.Add(new SqlParameter("SupportsJumboVehicles", syncRequest.SupportsJumboVehicles));
            commandParameterList.Add(new SqlParameter("SupportsIndividual", null));
            commandParameterList.Add(new SqlParameter("BasePalletTypeId", basePalletTypeId));
            commandParameterList.Add(new SqlParameter("BaseQualityId", baseQualityId));
            //commandParameterList.Add(new SqlParameter("BaseQuantity", syncRequest.RefLmsBaseLoadCarrierId != null && syncRequest.RefLmsBaseLoadCarrierId != 0 ? syncRequest.NumberOfStacks : (object)0));
            commandParameterList.Add(new SqlParameter("ShowOnline", true));
            commandParameterList.Add(new SqlParameter("IsCreatedOnline", true));
            commandParameterList.Add(new SqlParameter("SupportsPartialMatching", syncRequest.SupportsPartialMatching));
            commandParameterList.Add(new SqlParameter("IsHotspotOffer", false));
            commandParameterList.Add(new SqlParameter("HotspotComment", ""));
            commandParameterList.Add(new SqlParameter("HotspotOfferUntil", null));
            commandParameterList.Add(new SqlParameter("LiveDocumentNumber", syncRequest.OrderNumber));
            commandParameterList.Add(new SqlParameter("ExpressCode", ""));
            commandParameterList.Add(new SqlParameter("OnlineComment", "")); // TODO
            commandParameterList.Add(new SqlParameter("HasBasePallets", (syncRequest.RefLmsBaseLoadCarrierId ?? 0) != 0));
            commandParameterList.Add(new SqlParameter("LoadingLocationId", syncRequest.LoadingLocationId));
            commandParameterList.Add(new SqlParameter("LtmsAccountId", syncRequest.RefLtmsAccountId));
            commandParameterList.Add(new SqlParameter("LtmsAccountNumber", syncRequest.RefLtmsAccountNumber));
            commandParameterList.Add(new SqlParameter("SelfDelivery", syncRequest.TransportType == OrderTransportType.Self));
            commandParameterList.Add(new SqlParameter("IsPermanent", true));
            #endregion

            int NumAffectedRows = 0;
            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         return true;
         }

      public static bool UpdatePermanentAvailability(Config config, SqlTransaction CurrentSqlTransaction, OrderUpdateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         int NumAffectedRows = 0;

         if (syncRequest.Type != OrderType.Supply || !syncRequest.IsPermanent)
            {
            return false;
            }

         // Artikel und Qualität aus RefLmsLoadCarrierId und RefLmsBaseLoadCarrierId ermitteln
         int quality = syncRequest.RefLmsLoadCarrierId % 1000;
         int palletType = syncRequest.RefLmsLoadCarrierId / 1000;

         // Prüfen, ob Verfügbarkeit vorhanden
         if (!CheckPermanentAvailabilityExists(config, CurrentSqlTransaction, syncRequest))
            {
            errorReason = ServiceMain.ErrorReasonPermanentAvailabilityNotExists;
            errorDescription = ServiceMain.ErrorDescriptionPermanentAvailabilityNotExists
                + "AccountNumber " + syncRequest.RefLtmsAccountNumber
                + ", LmsLoadCarrierId: " + syncRequest.RefLmsLoadCarrierId;
            return false;
            }

         string QueryCommandString = @"
UPDATE [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
           SET [Quantity] = @Quantity
           ,[AvailableFromDate] = @AvailableFromDate
           ,[AvailableUntilDate] = @AvailableUntilDate
           ,[ModificationDate] = @ModificationDate
           ,[ModifiedBy] = @ModifiedBy
           ,[Finished] = @Finished
           ,[Revision] = Revision + 1
WHERE 
         (LtmsAccountNumber = @LtmsAccountNumber)
         AND (IsPermanent = 1)
         AND (finished = 0)
         AND (DeletedBy IS NULL)
         AND (PalletTypeId = @PalletTypeId)
         AND (QualityId = @QualityId)
";

         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("Quantity", syncRequest.LoadCarrierQuantity));
            commandParameterList.Add(new SqlParameter("AvailableFromDate", syncRequest.EarliestFulfillmentDateTime.Date));
            commandParameterList.Add(new SqlParameter("AvailableUntilDate", syncRequest.LatestFulfillmentDateTime.Date));
            commandParameterList.Add(new SqlParameter("ModificationDate", DateTime.Now));
            commandParameterList.Add(new SqlParameter("ModifiedBy", LiveDBTools.LMSServiceUser));

            commandParameterList.Add(new SqlParameter("Finished", syncRequest.LoadCarrierQuantity == 0));

            commandParameterList.Add(new SqlParameter("LtmsAccountNumber", syncRequest.RefLtmsAccountNumber));
            commandParameterList.Add(new SqlParameter("PalletTypeId", palletType));
            commandParameterList.Add(new SqlParameter("QualityId", quality));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            if( NumAffectedRows > 1)
               {
               errorReason = ServiceMain.ErrorReasonTooManyUpdateHits;
               errorDescription = ServiceMain.ErrorDescriptionTooManyUpdateHits
                  + "AccountNumber " + syncRequest.RefLtmsAccountNumber
                  + ", LmsLoadCarrierId: " + syncRequest.RefLmsLoadCarrierId;
               return false;
               }
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool InsertPermanentAvailabilityChild(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         int NumAffectedRows = 0;

         // Artikel und Qualität aus RefLmsLoadCarrierId und RefLmsBaseLoadCarrierId ermitteln
         int quality = syncRequest.RefLmsLoadCarrierId % 1000;
         int palletType = syncRequest.RefLmsLoadCarrierId / 1000;

         //// Prüfen, ob Verfügbarkeit vorhanden
         //if (!CheckPermanentAvailabilityExists(config, CurrentSqlTransaction, syncRequest))
         //   {
         //   errorReason = ServiceMain.ErrorReasonPermanentAvailabilityNotExists;
         //   errorDescription = ServiceMain.ErrorDescriptionPermanentAvailabilityNotExists
         //       + "AccountNumber " + syncRequest.RefLtmsAccountNumber
         //       + ", LmsLoadCarrierId: " + syncRequest.RefLmsLoadCarrierId;
         //   return false;
         //   }

         string QueryCommandString = @"
   INSERT INTO [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
           ([ZipCode]
           ,[Quantity]
           ,[TempAlloc]
           ,[TempReserved]
           ,[BBD]
           ,[IsManual]
           ,[TypeId]
           ,[ClientId]
           ,[Reliability]
           ,[ReliabilityQuantity]
           ,[ReliabilityQuality]
           ,[AvailableFromDate]
           ,[AvailableUntilDate]
           ,[AvailableFromTime]
           ,[AvailableUntilTime]
           ,[ContactId]
           ,[LoadingPointId]
           ,[QualityId]
           ,[AvailabilityTypeId]
           ,[CommentText]
           ,[CountryId]
           ,[AvailabilitySubTypeId]
           ,[PalletTypeId]
           ,[ContactAddressInfo]
           ,[ContractNo]
           ,[DeliveryNoteNo]
           ,[TransactionNo]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[ContactPerson]
           ,[ContactDetails]
           ,[RepositoryHours]
           ,[finished]
           ,[IsMixedAvailability]
           ,[LSManAdrAnrede]
           ,[LSManAdrName1]
           ,[LSManAdrName2]
           ,[LSManAdrName3]
           ,[LSManAdrPLZ]
           ,[LSManAdrOrt]
           ,[LSManAdrStrasse]
           ,[LSManAdrLand]
           ,[PalettenProMonat]
           ,[PalettenProWoche]
           ,[Notiz]
           ,[Geblockt]
           ,[GeblocktVon]
           ,[GeblocktDatum]
           ,[GeblocktAufgehobenVon]
           ,[GeblocktAufgehobenDatum]
           ,[GeblocktKommentar]
           ,[GeblocktFuer]
           ,[InBearbeitungVon]
           ,[InBearbeitungDatumZeit]
           ,[Revision]
           ,[Storniert]
           ,[StorniertVon]
           ,[StorniertDatum]
           ,[StorniertKommentar]
           ,[Rejection]
           ,[Reklamation]
           ,[RejectionReasonID]
           ,[RejectionComment]
           ,[RejectionQuantity]
           ,[RejectionApproachID]
           ,[RejectionBy]
           ,[RejectionDate]
           ,[RejectionDone]
           ,[RejectionDoneComment]
           ,[RejectionNextReceiptNo]
           ,[RejectionDoneBy]
           ,[RejectionDoneDate]
           ,[RejectionCost]
           ,[ICDroht]
           ,[ICDrohtVon]
           ,[ICDrohtAm]
           ,[ICDurchfuehren]
           ,[ICDurchfuehrenVon]
           ,[ICDurchfuehrenAm]
           ,[ZuAvisieren]
           ,[IstAvisiert]
           ,[AvisiertVon]
           ,[AvisiertAm]
           ,[IstAusAblehnung]
           ,[IcGrundID]
           ,[OrderManagementID]
           ,[RowGuid]
           ,[StackHeightMin]
           ,[StackHeightMax]
           ,[SupportsRearLoading]
           ,[SupportsSideLoading]
           ,[SupportsJumboVehicles]
           ,[SupportsIndividual]
           ,[BasePalletTypeId]
           ,[BaseQualityId]
           ,[ShowOnline]
           ,[IsCreatedOnline]
           ,[SupportsPartialMatching]
           ,[IsHotspotOffer]
           ,[HotspotComment]
           ,[HotspotOfferUntil]
           ,[OnlineComment]
           ,[LiveDocumentNumber]
           ,[ExpressCode]
           ,[HasBasePallets]
           ,[LoadingLocationId]
           ,[LtmsAccountId]
           ,[LtmsAccountNumber]
           ,[SelfDelivery]
           ,[IsPermanent]
           ,[IsPermanentChild])
SELECT [ZipCode]
      ,@NewQuantity
      ,[TempAlloc]
      ,[TempReserved]
      ,[BBD]
      ,[IsManual]
      ,[TypeId]
      ,[ClientId]
      ,[Reliability]
      ,[ReliabilityQuantity]
      ,[ReliabilityQuality]
      ,[AvailableFromDate]
      ,[AvailableUntilDate]
      ,[AvailableFromTime]
      ,[AvailableUntilTime]
      ,[ContactId]
      ,[LoadingPointId]
      ,[QualityId]
      ,[AvailabilityTypeId]
      ,[CommentText]
      ,[CountryId]
      ,[AvailabilitySubTypeId]
      ,[PalletTypeId]
      ,[ContactAddressInfo]
      ,[ContractNo]
      ,[DeliveryNoteNo]
      ,[TransactionNo]
      ,GETDATE() --[CreationDate]
      ,NULL --[ModificationDate]
      ,NULL --[DeletionDate]
      ,@NewCreatedBy -- [CreatedBy]
      ,NULL --[ModifiedBy]
      ,NULL --[DeletedBy]
      ,[ContactPerson]
      ,[ContactDetails]
      ,[RepositoryHours]
      ,[finished]
      ,[IsMixedAvailability]
      ,[LSManAdrAnrede]
      ,[LSManAdrName1]
      ,[LSManAdrName2]
      ,[LSManAdrName3]
      ,[LSManAdrPLZ]
      ,[LSManAdrOrt]
      ,[LSManAdrStrasse]
      ,[LSManAdrLand]
      ,[PalettenProMonat]
      ,[PalettenProWoche]
      ,[Notiz]
      ,[Geblockt]
      ,[GeblocktVon]
      ,[GeblocktDatum]
      ,[GeblocktAufgehobenVon]
      ,[GeblocktAufgehobenDatum]
      ,[GeblocktKommentar]
      ,[GeblocktFuer]
      ,[InBearbeitungVon]
      ,[InBearbeitungDatumZeit]
      ,[Revision]
      ,[Storniert]
      ,[StorniertVon]
      ,[StorniertDatum]
      ,[StorniertKommentar]
      ,[Rejection]
      ,[Reklamation]
      ,[RejectionReasonID]
      ,[RejectionComment]
      ,[RejectionQuantity]
      ,[RejectionApproachID]
      ,[RejectionBy]
      ,[RejectionDate]
      ,[RejectionDone]
      ,[RejectionDoneComment]
      ,[RejectionNextReceiptNo]
      ,[RejectionDoneBy]
      ,[RejectionDoneDate]
      ,[RejectionCost]
      ,[ICDroht]
      ,[ICDrohtVon]
      ,[ICDrohtAm]
      ,[ICDurchfuehren]
      ,[ICDurchfuehrenVon]
      ,[ICDurchfuehrenAm]
      ,[ZuAvisieren]
      ,[IstAvisiert]
      ,[AvisiertVon]
      ,[AvisiertAm]
      ,[IstAusAblehnung]
      ,[IcGrundID]
      ,[OrderManagementID]
      ,@NewRowGuid
      ,[StackHeightMin]
      ,[StackHeightMax]
      ,[SupportsRearLoading]
      ,[SupportsSideLoading]
      ,[SupportsJumboVehicles]
      ,[SupportsIndividual]
      ,[BasePalletTypeId]
      ,[BaseQualityId]
      ,0
      ,[IsCreatedOnline]
      ,[SupportsPartialMatching]
      ,[IsHotspotOffer]
      ,[HotspotComment]
      ,[HotspotOfferUntil]
      ,[OnlineComment]
      ,[LiveDocumentNumber]
      ,[ExpressCode]
      ,[HasBasePallets]
      ,[LoadingLocationId]
      ,[LtmsAccountId]
      ,[LtmsAccountNumber]
      ,[SelfDelivery]
      ,0
      ,1
  FROM [dbo].[LMS_AVAILABILITY]
  WHERE RowGuid = @OldRowGuid;

SELECT SCOPE_IDENTITY()
";

         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("NewQuantity", syncRequest.LoadCarrierQuantity));
            commandParameterList.Add(new SqlParameter("NewCreatedBy", LiveDBTools.LMSServiceUser));

            commandParameterList.Add(new SqlParameter("NewRowGuid", syncRequest.RefLmsAvailabilityRowGuid));

            commandParameterList.Add(new SqlParameter("OldRowGuid", syncRequest.RefLmsPermanentAvailabilityRowGuid));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            if (NumAffectedRows > 1)
               {
               errorReason = ServiceMain.ErrorReasonTooManyUpdateHits;
               errorDescription = ServiceMain.ErrorDescriptionTooManyUpdateHits
                  + "AccountNumber " + syncRequest.RefLtmsAccountNumber
                  + ", LmsLoadCarrierId: " + syncRequest.RefLmsLoadCarrierId;
               return false;
               }
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool CancelAvailability(Config config, SqlTransaction CurrentSqlTransaction, OrderCancelSyncRequest cancelRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         if (cancelRequest.Type != OrderType.Supply)
            {
            return false;
            }

         string QueryCommandString = @"
         UPDATE [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
         SET
            Storniert = @Storniert,
            StorniertVon = @StorniertVon,
            StorniertDatum = @StorniertDatum,
            StorniertKommentar = ISNULL(StorniertKommentar, '') + (CASE WHEN ISNULL(StorniertKommentar, '') = '' THEN '' ELSE CHAR(13) END) + @StorniertKommentar,
            Revision = Revision + 1
         WHERE 
            (RowGuid = @OrderRowGuid)";
         if (!cancelRequest.IsApproved)
            {
            QueryCommandString += @"
            AND (Storniert IS NULL OR Storniert = 0)
            AND (DeletedBy IS NULL)
            AND (SELECT ISNULL(SUM(Quantity), 0) FROM [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI] WHERE AvailabilityID = (SELECT AvailabilityID FROM [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY] WHERE RowGuid = @OrderRowGuid) AND DeletedBy IS NULL AND FrachtpapiereErstellt = 1) = 0
            "; // TODO VersionCol prüfen
            }
         // Folgende Kriterien erstmal entfernt
         //AND(finished = 0)
         //AND(Geblockt IS NULL OR Geblockt = 0)
         //AND(InBearbeitungVon IS NULL)

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("OrderRowGuid", cancelRequest.OrderRowGuid));
            commandParameterList.Add(new SqlParameter("Storniert", 1));
            commandParameterList.Add(new SqlParameter("StorniertVon", cancelRequest.Person == null ? "" : cancelRequest.Person.LastName + ", " + cancelRequest.Person.FirstName));
            commandParameterList.Add(new SqlParameter("StorniertDatum", DateTime.Now));
            commandParameterList.Add(new SqlParameter("StorniertKommentar", "(ONLINE): " + cancelRequest.Note ?? ""));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (NumAffectedRows == 0)
            {
            // Prüfung, an welcher Spalte es gescheitert ist
            QueryCommandString = @"
            SELECT
               RowGuid 
               , Storniert
               , finished
               , DeletedBy
               , Geblockt
               , InBearbeitungVon
               , (SELECT ISNULL(SUM(Quantity), 0) FROM [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI] WHERE AvailabilityID = (SELECT AvailabilityID FROM [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY] WHERE RowGuid = @OrderRowGuid) AND DeletedBy IS NULL AND FrachtpapiereErstellt = 1) AS NumZugeordnet
            FROM [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
            WHERE 
               (RowGuid = @OrderRowGuid) 
            ";

            commandParameterList.Clear();
            commandParameterList.Add(new SqlParameter("OrderRowGuid", cancelRequest.OrderRowGuid));

            DataSet dataRows = new DataSet();
            SqlHelper.FillDataset(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), dataRows, null, commandParameterList.ToArray());
            if (dataRows.Tables.Count != 1 || dataRows.Tables[0].Rows.Count != 1)
               {
               // Datensatz nicht gefunden
               errorReason = ServiceMain.ErrorReasonRowNotFound;
               errorDescription = ServiceMain.ErrorDescriptionRowNotFound + cancelRequest.OrderRowGuid + " (LMS_AVAILABILITY)";
               return false;
               }
            DataRow row = dataRows.Tables[0].Rows[0];
            // Storniert
            if (row.Field<bool?>("Storniert") == true)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "Storniert";
               return false;
               }
            // finished
            if (row.Field<bool?>("finished") == true)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "finished";
               return false;
               }
            // DeletedBy
            if (row.Field<string>("DeletedBy") != null)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "DeletedBy";
               return false;
               }
            // Geblockt
            if (row.Field<bool?>("Geblockt") == true)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "Geblockt";
               return false;
               }
            // InBearbeitungVon
            if (row.Field<string>("InBearbeitungVon") != null)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "InBearbeitungVon";
               return false;
               }
            // NumZugeordnet
            if (row.Field<int>("NumZugeordnet") != 0)
               {
               errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
               errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "NumZugeordnet";
               return false;
               }
            errorReason = ServiceMain.ErrorReasonConditionColumnnValue;
            errorDescription = ServiceMain.ErrorDescriptionConditionColumnnValue + "UnKnown";
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool InsertAvail2Deli(Config config, SqlTransaction CurrentSqlTransaction, Avail2DeliCreateRequest avail2DeliCreateRequest,
         DataRow availabilityRow, DataRow deliveryRow,
         out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         string QueryCommandString = @"
INSERT INTO [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI]
           ([DeliveryID]
           ,[AvailabilityID]
           ,[DateOfRelation]
           ,[Quantity]
           ,[State]
           ,[Usr]
           ,[IsFix]
           ,[SpediteurID]
           ,[SpediteurManuell]
           ,[Kilometer]
           ,[Frachtpreis]
           ,[LieferscheinNr]
           ,[FrachtauftragsNr]
           ,[FrachtpapiereErstellt]
           ,[Notiz]
           ,[CreationDate]
           ,[ModificationDate]
           ,[DeletionDate]
           ,[CreatedBy]
           ,[ModifiedBy]
           ,[DeletedBy]
           ,[Bestellnummer]
           ,[ZustellterminZeitVon]
           ,[ZustellterminZeitBis]
           ,[LadeterminDatum]
           ,[LadeterminZeitVon]
           ,[LadeterminZeitBis]
           ,[InBearbeitungVon]
           ,[InBearbeitungDatumZeit]
           ,[SpediteurTelefon]
           ,[SpediteurFax]
           ,[SpediteurEMail]
           ,[Lieferkategorie]
           ,[AbweichenderArtikel]
           ,[FrachtvorschriftFelder]
           ,[FrachtvorschriftStapelhoehe]
           ,[Revision]
           ,[StatusGedrucktFrachtvorschrift]
           ,[StatusGedrucktZuordnung]
           ,[ICTransport]
           ,[CategoryId]
           ,[AnzahlBelegteStellplaetze]
           ,[Timocom]
           ,[RowGuid]
           ,[LoadCarrierStackHeight]
           ,[SupportsRearLoading]
           ,[SupportsSideLoading]
           ,[SupportsJumboVehicles]
           ,[SupportsIndividual]
           ,[HasBasePallets]
           ,[BasePalletTypeId]
           ,[BaseQualityId]
           ,[BaseQuantity]
           ,[ShowOnline]
           ,[ShowOnlineUntil]
           ,[IsCreatedOnline]
           ,[QualityId]
           ,[OnlineComment]
           ,[LiveDocumentNumber]
           ,[PalletTypeId]
           ,[ExpressCode])
     VALUES
           (@DeliveryID
           ,@AvailabilityID
           ,@DateOfRelation
           ,@Quantity
           ,@State
           ,@Usr
           ,@IsFix
           ,@SpediteurID
           ,@SpediteurManuell
           ,@Kilometer
           ,@Frachtpreis
           ,@LieferscheinNr
           ,@FrachtauftragsNr
           ,@FrachtpapiereErstellt
           ,@Notiz
           ,@CreationDate
           ,@ModificationDate
           ,@DeletionDate
           ,@CreatedBy
           ,@ModifiedBy
           ,@DeletedBy
           ,@Bestellnummer
           ,@ZustellterminZeitVon
           ,@ZustellterminZeitBis
           ,@LadeterminDatum
           ,@LadeterminZeitVon
           ,@LadeterminZeitBis
           ,@InBearbeitungVon
           ,@InBearbeitungDatumZeit
           ,@SpediteurTelefon
           ,@SpediteurFax
           ,@SpediteurEMail
           ,@Lieferkategorie
           ,@AbweichenderArtikel
           ,@FrachtvorschriftFelder
           ,@FrachtvorschriftStapelhoehe
           ,@Revision
           ,@StatusGedrucktFrachtvorschrift
           ,@StatusGedrucktZuordnung
           ,@ICTransport
           ,@CategoryId
           ,@AnzahlBelegteStellplaetze
           ,@Timocom
           ,@RowGuid
           ,@LoadCarrierStackHeight
           ,@SupportsRearLoading
           ,@SupportsSideLoading
           ,@SupportsJumboVehicles
           ,@SupportsIndividual
           ,@HasBasePallets
           ,@BasePalletTypeId
           ,@BaseQualityId
           ,@BaseQuantity
           ,@ShowOnline
           ,@ShowOnlineUntil
           ,@IsCreatedOnline
           ,@QualityId
           ,@OnlineComment
           ,@LiveDocumentNumber
           ,@PalletTypeId
           ,@ExpressCode)
	";

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("DeliveryID", avail2DeliCreateRequest.LmsDeliveryId));
            commandParameterList.Add(new SqlParameter("AvailabilityID", avail2DeliCreateRequest.LmsAvailabilityId));
            commandParameterList.Add(new SqlParameter("DateOfRelation", avail2DeliCreateRequest.DeliveryTimeDate)); // Zustelltermin Datum
            commandParameterList.Add(new SqlParameter("Quantity", avail2DeliCreateRequest.Quantity));
            commandParameterList.Add(new SqlParameter("State", 2)); // Zugeordnet
            commandParameterList.Add(new SqlParameter("Usr", null));
            commandParameterList.Add(new SqlParameter("IsFix", deliveryRow.Field<bool>("IsFix")));
            commandParameterList.Add(new SqlParameter("SpediteurID", -1));
            commandParameterList.Add(new SqlParameter("SpediteurManuell", ""));
            commandParameterList.Add(new SqlParameter("Kilometer", (object)0));
            commandParameterList.Add(new SqlParameter("Frachtpreis", (object)0));
            commandParameterList.Add(new SqlParameter("LieferscheinNr", deliveryRow.Field<string>("DeliveryNoteNo")));
            commandParameterList.Add(new SqlParameter("FrachtauftragsNr", deliveryRow.Field<string>("FrachtauftragNo")));
            commandParameterList.Add(new SqlParameter("FrachtpapiereErstellt", avail2DeliCreateRequest.Finished));
            commandParameterList.Add(new SqlParameter("Notiz", ""));
            commandParameterList.Add(new SqlParameter("CreationDate", DateTime.Now));
            commandParameterList.Add(new SqlParameter("ModificationDate", null));
            commandParameterList.Add(new SqlParameter("DeletionDate", null));
            commandParameterList.Add(new SqlParameter("CreatedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModifiedBy", null));
            commandParameterList.Add(new SqlParameter("DeletedBy", null));
            commandParameterList.Add(new SqlParameter("Bestellnummer", deliveryRow.Field<string>("Bestellnummer")));
            commandParameterList.Add(new SqlParameter("ZustellterminZeitVon", avail2DeliCreateRequest.DeliveryTimeTimeFrom));
            commandParameterList.Add(new SqlParameter("ZustellterminZeitBis", avail2DeliCreateRequest.DeliveryTimeTimeTo));
            commandParameterList.Add(new SqlParameter("LadeterminDatum", avail2DeliCreateRequest.LoadingTimeDate));
            commandParameterList.Add(new SqlParameter("LadeterminZeitVon", avail2DeliCreateRequest.LoadingTimeTimeFrom));
            commandParameterList.Add(new SqlParameter("LadeterminZeitBis", avail2DeliCreateRequest.LoadingTimeTimeTo));
            commandParameterList.Add(new SqlParameter("InBearbeitungVon", null));
            commandParameterList.Add(new SqlParameter("InBearbeitungDatumZeit", null));
            commandParameterList.Add(new SqlParameter("SpediteurTelefon", ""));
            commandParameterList.Add(new SqlParameter("SpediteurFax", ""));
            commandParameterList.Add(new SqlParameter("SpediteurEMail", ""));
            commandParameterList.Add(new SqlParameter("Lieferkategorie", deliveryRow.Field<bool>("IstBedarf") ? 3 : 6)); // 3 = Selbstabholer, 6 = Spedition 
            commandParameterList.Add(new SqlParameter("AbweichenderArtikel", ""));
            commandParameterList.Add(new SqlParameter("FrachtvorschriftFelder", "000110010")); // Depricated
            commandParameterList.Add(new SqlParameter("FrachtvorschriftStapelhoehe", deliveryRow.Field<int>("StackHeightMax")));
            commandParameterList.Add(new SqlParameter("Revision", (object)0));
            commandParameterList.Add(new SqlParameter("StatusGedrucktFrachtvorschrift", false));
            commandParameterList.Add(new SqlParameter("StatusGedrucktZuordnung", false));
            commandParameterList.Add(new SqlParameter("ICTransport", false));
            commandParameterList.Add(new SqlParameter("CategoryId", null));
            commandParameterList.Add(new SqlParameter("AnzahlBelegteStellplaetze", 0)); // TODO
            commandParameterList.Add(new SqlParameter("Timocom", false));
            commandParameterList.Add(new SqlParameter("RowGuid", Guid.NewGuid())); // TODO
            commandParameterList.Add(new SqlParameter("LoadCarrierStackHeight", deliveryRow.Field<int>("StackHeightMax")));
            commandParameterList.Add(new SqlParameter("SupportsRearLoading", avail2DeliCreateRequest.SupportsRearLoading));
            commandParameterList.Add(new SqlParameter("SupportsSideLoading", avail2DeliCreateRequest.SupportsSideLoading));
            commandParameterList.Add(new SqlParameter("SupportsJumboVehicles", avail2DeliCreateRequest.SupportsJumboVehicles));
            commandParameterList.Add(new SqlParameter("SupportsIndividual", avail2DeliCreateRequest.SupportsIndividual));
            commandParameterList.Add(new SqlParameter("HasBasePallets", avail2DeliCreateRequest.HasBasePallets));
            commandParameterList.Add(new SqlParameter("BasePalletTypeId", avail2DeliCreateRequest.BasePalletTypeId));
            commandParameterList.Add(new SqlParameter("BaseQualityId", avail2DeliCreateRequest.BaseQualityId));
            commandParameterList.Add(new SqlParameter("BaseQuantity", avail2DeliCreateRequest.BaseQuantity));
            commandParameterList.Add(new SqlParameter("ShowOnline", false));
            commandParameterList.Add(new SqlParameter("ShowOnlineUntil", DateTime.Now));
            commandParameterList.Add(new SqlParameter("IsCreatedOnline", true));
            commandParameterList.Add(new SqlParameter("QualityId", availabilityRow.Field<int>("QualityId")));
            commandParameterList.Add(new SqlParameter("OnlineComment", avail2DeliCreateRequest.OnlineNote));
            commandParameterList.Add(new SqlParameter("LiveDocumentNumber", avail2DeliCreateRequest.LiveDocumentNumber));
            commandParameterList.Add(new SqlParameter("PalletTypeId", availabilityRow.Field<int>("PalletTypeId")));
            commandParameterList.Add(new SqlParameter("ExpressCode", avail2DeliCreateRequest.DigitalCode));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool AllocateAvailabilityAndDelivery(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         // Availability holen
         Guid availabilityGuid = syncRequest.RefLmsAvailabilityRowGuid ?? syncRequest.OrderRowGuid;
         string QueryCommandString = @"
            SELECT * 
            FROM [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
            WHERE (RowGuid = @AvailibilityRowGuid) 
            ";

         commandParameterList.Clear();
         commandParameterList.Add(new SqlParameter("AvailibilityRowGuid", availabilityGuid));

         DataSet dataRows = new DataSet();
         SqlHelper.FillDataset(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), dataRows, null, commandParameterList.ToArray());
         if (dataRows.Tables.Count != 1 || dataRows.Tables[0].Rows.Count != 1)
            {
            // Datensatz nicht gefunden
            errorReason = ServiceMain.ErrorReasonRowNotFound;
            errorDescription = ServiceMain.ErrorDescriptionRowNotFound + availabilityGuid + " (LMS_AVAILABILITY)";
            return false;
            }
         DataRow availabilityRow = dataRows.Tables[0].Rows[0];

         // Delivery holen
         Guid deliveryGuid = syncRequest.RefLmsDeliveryRowGuid ?? syncRequest.OrderRowGuid;
         QueryCommandString = @"
            SELECT * 
            FROM [" + config.LMSSchemaName + @"].[LMS_DELIVERY]
            WHERE (RowGuid = @DeliveryRowGuid) 
            ";

         commandParameterList.Clear();
         commandParameterList.Add(new SqlParameter("DeliveryRowGuid", deliveryGuid));

         dataRows = new DataSet();
         SqlHelper.FillDataset(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), dataRows, null, commandParameterList.ToArray());
         if (dataRows.Tables.Count != 1 || dataRows.Tables[0].Rows.Count != 1)
            {
            // Datensatz nicht gefunden
            errorReason = ServiceMain.ErrorReasonRowNotFound;
            errorDescription = ServiceMain.ErrorDescriptionRowNotFound + deliveryGuid + " (LMS_DELIVERY)";
            return false;
            }
         DataRow deliveryRow = dataRows.Tables[0].Rows[0];

         // Artikel und Qualität aus RefLmsLoadCarrierId und RefLmsBaseLoadCarrierId ermitteln
         int quality = syncRequest.RefLmsLoadCarrierId % 1000;
         int palletType = syncRequest.RefLmsLoadCarrierId / 1000;
         int baseQualityId = 0;
         int basePalletTypeId = 0;

         bool supportsRearLoading;
         bool supportsSideLoading;
         bool supportsJumboVehicles;
         if (syncRequest.TransportType == OrderTransportType.Self)
            {
            if (syncRequest.Type == OrderType.Demand)
               {
               // Bei Selbstabholern Ladevorschriften der Verfügbarkeit nehmen
               supportsRearLoading = availabilityRow.Field<bool>("SupportsRearLoading");
               supportsSideLoading = availabilityRow.Field<bool>("SupportsSideLoading");
               supportsJumboVehicles = availabilityRow.Field<bool>("SupportsJumboVehicles");
               }
            else
               {
               // Bei Selbstanlieferern Ladevorschriften des Liefertermins nehmen
               supportsRearLoading = deliveryRow.Field<bool>("SupportsRearLoading");
               supportsSideLoading = deliveryRow.Field<bool>("SupportsSideLoading");
               supportsJumboVehicles = deliveryRow.Field<bool>("SupportsJumboVehicles");
               }
            }
         else
            {
            // Bei allgemeinen Zuordnungen nur gemeinsame Ladevorschriften nehmen
            supportsRearLoading = availabilityRow.Field<bool>("SupportsRearLoading") && deliveryRow.Field<bool>("SupportsRearLoading");
            supportsSideLoading = availabilityRow.Field<bool>("SupportsSideLoading") && deliveryRow.Field<bool>("SupportsSideLoading");
            supportsJumboVehicles = availabilityRow.Field<bool>("SupportsJumboVehicles") && deliveryRow.Field<bool>("SupportsJumboVehicles");
            }
         if (syncRequest.RefLmsBaseLoadCarrierId != null)
            {
            baseQualityId = (int)syncRequest.RefLmsBaseLoadCarrierId % 1000;
            basePalletTypeId = (int)syncRequest.RefLmsBaseLoadCarrierId / 1000;
            }

         Avail2DeliCreateRequest avail2DeliCreateRequest = new Avail2DeliCreateRequest
            {
            LmsAvailabilityId = availabilityRow.Field<int>("AvailabilityId"),
            LmsDeliveryId = deliveryRow.Field<int>("DiliveryID"),
            Finished = true,
            Quantity = syncRequest.LoadCarrierQuantity,
            DeliveryTimeDate = syncRequest.EarliestFulfillmentDateTime.Date,
            DeliveryTimeTimeFrom = "00:00",
            DeliveryTimeTimeTo = "23:59",
            LoadingTimeDate = syncRequest.EarliestFulfillmentDateTime.Date,
            LoadingTimeTimeFrom = "00:00",
            LoadingTimeTimeTo = "23:59",
            SupportsRearLoading = supportsRearLoading,
            SupportsSideLoading = supportsSideLoading,
            SupportsJumboVehicles = supportsJumboVehicles,
            SupportsIndividual = "",
            HasBasePallets = syncRequest.RefLmsBaseLoadCarrierId != null,
            BaseQuantity = syncRequest.BaseLoadCarrierQuantity,
            BasePalletTypeId = basePalletTypeId,
            BaseQualityId = baseQualityId,
            LiveDocumentNumber = "",
            DigitalCode = syncRequest.DigitalCode,
            OnlineNote = ""
            };

         if (!InsertAvail2Deli(config, CurrentSqlTransaction, avail2DeliCreateRequest, availabilityRow, deliveryRow, out errorReason, out errorDescription))
            {
            return false;
            }

         return true;
         }



      public static bool FullfillEntries(Config config, SqlTransaction CurrentSqlTransaction, OrderFulfillSyncRequest fullfillRequest,
         out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         if (fullfillRequest.RefLmsAvailabilityRowGuid != null)
            {
            if (!SetAvailabilityFinished(config, CurrentSqlTransaction, fullfillRequest, out errorReason, out errorDescription))
               {
               return false;
               }
            }

         if (fullfillRequest.RefLmsDeliveryRowGuid != null)
            {
            if (!SetDeliveryFinished(config, CurrentSqlTransaction, fullfillRequest, out errorReason, out errorDescription))
               {
               return false;
               }
            }

         //if (fullfillRequest.DigitalCode != null)
         //   {
         //   if (!SetAvail2DeliFinished(config, CurrentSqlTransaction, fullfillRequest, out errorReason, out errorDescription))
         //      {
         //      return false;
         //      }
         //   }

         return true;
         }

      public static bool RollbackFullfillEntries(Config config, SqlTransaction CurrentSqlTransaction, OrderRollbackFulfillmentSyncRequest rollbackFullfillRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         if (rollbackFullfillRequest.DigitalCode != null)
            {
            if (!SetAvailabilityUnFinished(config, CurrentSqlTransaction, rollbackFullfillRequest, out errorReason, out errorDescription))
               {
               return false;
               }
            if (!SetDeliveryUnFinished(config, CurrentSqlTransaction, rollbackFullfillRequest, out errorReason, out errorDescription))
               {
               return false;
               }
            //if (!SetAvail2DeliUnFinished(config, CurrentSqlTransaction, rollbackFullfillRequest, out errorReason, out errorDescription))
            //   {
            //   return false;
            //   }
            }

         return true;
         }

      public static bool SetAvailabilityFinished(Config config, SqlTransaction CurrentSqlTransaction, OrderFulfillSyncRequest fullfillRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         string QueryCommandString = @"
         UPDATE [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
         SET
            finished = 1,
            ModifiedBy = @ModifiedBy,
            ModificationDate = @ModificationDate,
            Revision = Revision + 1
         WHERE 
            (RowGuid = @RowGuid)";

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("RowGuid", fullfillRequest.RefLmsAvailabilityRowGuid));
            commandParameterList.Add(new SqlParameter("ModifiedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModificationDate", fullfillRequest.FulfillmentDateTime));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (NumAffectedRows == 0)
            {
            errorReason = ServiceMain.ErrorReasonRowNotFound;
            errorDescription = ServiceMain.ErrorDescriptionRowNotFound + "Availability " + fullfillRequest.RefLmsAvailabilityRowGuid;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool SetAvailabilityUnFinished(Config config, SqlTransaction CurrentSqlTransaction, OrderRollbackFulfillmentSyncRequest rollbackFullfillRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         string QueryCommandString = @"
         UPDATE [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
         SET
            finished = 0,
            ModifiedBy = @ModifiedBy,
            ModificationDate = @ModificationDate,
            Revision = Revision + 1
         WHERE 
            (AvailabilityId = (SELECT AvailabilityID FROM [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI] WHERE (ExpressCode = @DigitalCode) AND (DeletionDate IS NULL)))";

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("DigitalCode", rollbackFullfillRequest.DigitalCode));
            commandParameterList.Add(new SqlParameter("ModifiedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModificationDate", DateTime.Now));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (NumAffectedRows == 0)
            {
            errorReason = ServiceMain.ErrorReasonRowNotFound;
            errorDescription = ServiceMain.ErrorDescriptionRowNotFound + "Availability von Avail2Deli " + rollbackFullfillRequest.DigitalCode;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool SetDeliveryFinished(Config config, SqlTransaction CurrentSqlTransaction, OrderFulfillSyncRequest fullfillRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         string QueryCommandString = @"
         UPDATE [" + config.LMSSchemaName + @"].[LMS_DELIVERY]
         SET
            finished = 1,
            ModifiedBy = @ModifiedBy,
            ModificationDate = @ModificationDate,
            Revision = Revision + 1
         WHERE 
            (RowGuid = @RowGuid)";

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("RowGuid", fullfillRequest.RefLmsDeliveryRowGuid));
            commandParameterList.Add(new SqlParameter("ModifiedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModificationDate", fullfillRequest.FulfillmentDateTime));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (NumAffectedRows == 0)
            {
            errorReason = ServiceMain.ErrorReasonRowNotFound;
            errorDescription = ServiceMain.ErrorDescriptionRowNotFound + "Delivery " + fullfillRequest.RefLmsDeliveryRowGuid;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool SetDeliveryUnFinished(Config config, SqlTransaction CurrentSqlTransaction, OrderRollbackFulfillmentSyncRequest rollbackFullfillRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         string QueryCommandString = @"
         UPDATE [" + config.LMSSchemaName + @"].[LMS_DELIVERY]
         SET
            finished = 0,
            ModifiedBy = @ModifiedBy,
            ModificationDate = @ModificationDate,
            Revision = Revision + 1
         WHERE 
            (DiliveryId = (SELECT DeliveryID FROM [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI] WHERE (ExpressCode = @DigitalCode) AND (DeletionDate IS NULL)))";

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("DigitalCode", rollbackFullfillRequest.DigitalCode));
            commandParameterList.Add(new SqlParameter("ModifiedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModificationDate", DateTime.Now));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (NumAffectedRows == 0)
            {
            errorReason = ServiceMain.ErrorReasonRowNotFound;
            errorDescription = ServiceMain.ErrorDescriptionRowNotFound + "Delivery von Avail2Deli " + rollbackFullfillRequest.DigitalCode;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool SetAvail2DeliFinished(Config config, SqlTransaction CurrentSqlTransaction, OrderFulfillSyncRequest fullfillRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         string QueryCommandString = @"
         UPDATE [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI]
         SET
            FrachtpapiereErstellt = 1,
            ModifiedBy = @ModifiedBy,
            ModificationDate = @ModificationDate,
            Revision = Revision + 1
         WHERE 
            (ExpressCode = @DigitalCode)";

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("DigitalCode", fullfillRequest.DigitalCode));
            commandParameterList.Add(new SqlParameter("ModifiedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModificationDate", fullfillRequest.FulfillmentDateTime));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (NumAffectedRows == 0)
            {
            errorReason = ServiceMain.ErrorReasonRowNotFound;
            errorDescription = ServiceMain.ErrorDescriptionRowNotFound + "Avail2Deli " + fullfillRequest.DigitalCode;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      public static bool SetAvail2DeliUnFinished(Config config, SqlTransaction CurrentSqlTransaction, OrderRollbackFulfillmentSyncRequest rollbackFullfillRequest, out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         string QueryCommandString = @"
         UPDATE [" + config.LMSSchemaName + @"].[LMS_AVAIL2DELI]
         SET
            FrachtpapiereErstellt = 0,
            ModifiedBy = @ModifiedBy,
            ModificationDate = @ModificationDate,
            Revision = Revision + 1
         WHERE 
            (ExpressCode = @DigitalCode) AND (DeletionDate IS NULL)";

         int NumAffectedRows = 0;
         try
            {
            #region SqlParams
            commandParameterList.Add(new SqlParameter("DigitalCode", rollbackFullfillRequest.DigitalCode));
            commandParameterList.Add(new SqlParameter("ModifiedBy", LiveDBTools.LMSServiceUser));
            commandParameterList.Add(new SqlParameter("ModificationDate", DateTime.Now));
            #endregion

            NumAffectedRows = (int)SqlHelper.ExecuteNonQuery(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());
            }
         catch (Exception ex)
            {
            errorReason = ServiceMain.ErrorReasonException;
            errorDescription = ServiceMain.ErrorDescriptionException + ex.Message;
            return false;
            }

         if (NumAffectedRows == 0)
            {
            errorReason = ServiceMain.ErrorReasonRowNotFound;
            errorDescription = ServiceMain.ErrorDescriptionRowNotFound + "Avail2Deli " + rollbackFullfillRequest.DigitalCode;
            return false;
            }

         return (NumAffectedRows == 1);
         }

      private static string GetZipCodeFromCustomerID(Config config, SqlTransaction CurrentSqlTransaction, int CustomerID)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         string QueryCommandString = @"
            SELECT PLZ FROM [" + config.LMSSchemaName + @"].[LMS_CUSTOMER]
            WHERE [AdressNr] = @AddressNr";

         commandParameterList.Add(new SqlParameter("AddressNr", CustomerID));

         string zipCode = (string)SqlHelper.ExecuteScalar(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());

         return zipCode;
         }

      private static bool CheckCustomerIDExists(Config config, SqlTransaction CurrentSqlTransaction, int CustomerID)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         string QueryCommandString = @"
            SELECT COUNT(*) FROM [" + config.LMSSchemaName + @"].[LMS_CUSTOMER]
            WHERE [AdressNr] = @AddressNr";

         commandParameterList.Add(new SqlParameter("AddressNr", CustomerID));

         int NumRows = (int)SqlHelper.ExecuteScalar(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());

         return (NumRows > 0);
         }

      private static bool CheckPermanentAvailabilityExists(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         if (syncRequest.Type != OrderType.Supply || !syncRequest.IsPermanent)
            {
            return false;
            }

         // Artikel und Qualität aus RefLmsLoadCarrierId und RefLmsBaseLoadCarrierId ermitteln
         int quality = syncRequest.RefLmsLoadCarrierId % 1000;
         int palletType = syncRequest.RefLmsLoadCarrierId / 1000;
         int baseQualityId = 0;
         int basePalletTypeId = 0;
         if (syncRequest.RefLmsBaseLoadCarrierId != null && syncRequest.RefLmsBaseLoadCarrierId != 0)
            {
            baseQualityId = (int)syncRequest.RefLmsBaseLoadCarrierId % 1000;
            basePalletTypeId = (int)syncRequest.RefLmsBaseLoadCarrierId / 1000;
            }

         string QueryCommandString = @"
            SELECT COUNT(*) FROM [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
            WHERE 
                (LtmsAccountNumber = @LtmsAccountNumber)
                AND (IsPermanent = 1)
                AND (finished = 0)
                AND (DeletedBy IS NULL)
                AND (PalletTypeId = @PalletTypeId)
                AND (QualityId = @QualityId)
                AND (HasBasePallets = @HasBasePallets)
                AND (BasePalletTypeId = @BasePalletTypeId)
                AND (LoadingLocationId = @LoadingLocationId)
            ";

         #region SqlParams
         commandParameterList.Add(new SqlParameter("LtmsAccountNumber", syncRequest.RefLtmsAccountNumber));
         commandParameterList.Add(new SqlParameter("PalletTypeId", palletType));
         commandParameterList.Add(new SqlParameter("QualityId", quality));
         commandParameterList.Add(new SqlParameter("HasBasePallets", syncRequest.RefLmsBaseLoadCarrierId != 0));
         commandParameterList.Add(new SqlParameter("BasePalletTypeId", basePalletTypeId));
         commandParameterList.Add(new SqlParameter("LoadingLocationId", syncRequest.LoadingLocationId));
         #endregion

         int NumRows = (int)SqlHelper.ExecuteScalar(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());

         return (NumRows > 0);
         }

      private static bool CheckPermanentAvailabilityExists(Config config, SqlTransaction CurrentSqlTransaction, OrderUpdateSyncRequest syncRequest)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         if (syncRequest.Type != OrderType.Supply || !syncRequest.IsPermanent)
            {
            return false;
            }

         // Artikel und Qualität aus RefLmsLoadCarrierId und RefLmsBaseLoadCarrierId ermitteln
         int quality = syncRequest.RefLmsLoadCarrierId % 1000;
         int palletType = syncRequest.RefLmsLoadCarrierId / 1000;

         string QueryCommandString = @"
            SELECT COUNT(*) FROM [" + config.LMSSchemaName + @"].[LMS_AVAILABILITY]
            WHERE 
                (LtmsAccountNumber = @LtmsAccountNumber)
                AND (IsPermanent = 1)
                AND (finished = 0)
                AND (DeletedBy IS NULL)
                AND (PalletTypeId = @PalletTypeId)
                AND (QualityId = @QualityId)
            ";

         #region SqlParams
         commandParameterList.Add(new SqlParameter("LtmsAccountNumber", syncRequest.RefLtmsAccountNumber));
         commandParameterList.Add(new SqlParameter("PalletTypeId", palletType));
         commandParameterList.Add(new SqlParameter("QualityId", quality));
         #endregion

         int NumRows = (int)SqlHelper.ExecuteScalar(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), commandParameterList.ToArray());

         return (NumRows > 0);
         }

      private static bool SetDefaultLoadingLocationIdFromCustomerID(Config config, SqlTransaction CurrentSqlTransaction, OrderCreateSyncRequest syncRequest, 
         out string errorReason, out string errorDescription)
         {
         List<SqlParameter> commandParameterList = new List<SqlParameter>();

         errorReason = "";
         errorDescription = "";

         string QueryCommandString = @"
            SELECT TOP 1 * FROM [" + config.LMSSchemaName + @"].[LMS_LOADINGLOCATIONS_OLMA]
            WHERE [AdressNr] = @AddressNr";

         commandParameterList.Add(new SqlParameter("AddressNr", syncRequest.CustomerNumber));

         DataSet dataRows = new DataSet();

         SqlHelper.FillDataset(CurrentSqlTransaction, CommandType.Text, QueryCommandString.ToString(), dataRows, null, commandParameterList.ToArray());
         if (dataRows.Tables.Count != 1 || dataRows.Tables[0].Rows.Count != 1)
            {
            // Datensatz nicht gefunden
            errorReason = ServiceMain.ErrorReasonNoLoadingLocationForCustomer;
            errorDescription = ServiceMain.ErrorDescriptionNoLoadingLocationForCustomer + syncRequest.CustomerNumber;
            return false;
            }
         DataRow loadingLocationRow = dataRows.Tables[0].Rows[0];

         syncRequest.StackHeightMin = loadingLocationRow.Field<int>("StackHeightMin");
         syncRequest.StackHeightMax = loadingLocationRow.Field<int>("StackHeightMax");
         syncRequest.SupportsPartialMatching = true;
         syncRequest.SupportsRearLoading = loadingLocationRow.Field<bool>("SupportsRearLoading");
         syncRequest.SupportsSideLoading = loadingLocationRow.Field<bool>("SupportsSideLoading");
         syncRequest.SupportsJumboVehicles = loadingLocationRow.Field<bool>("SupportsJumboVehicles");

         syncRequest.LoadingLocationId = loadingLocationRow.Field<int>("LoadingLocationId");

         return true;
         }

      private static int GetAvailabilityTypeIdFromAccountType(string accountNumber)
         {
         string accountTypeString = "";
         try
            {
            accountTypeString = accountNumber.Split("-")[1];
            }
         catch
            {
            return 4; // Freistellung
            }

         // Table LMS_AVAILABILITYTYPE
         switch (accountTypeString)
            {
            case "HZL":
               return 11; // Handel
            case "TK":
               return 4; // Freistellung
            case "PM":
               return 4; // Freistellung
            case "DIS":
               return 4; // Freistellung
            case "DEP":
               return 2; // Depotpartner
            case "LDL":
               return 3; // Reparaturpartner
            case "LAG":
               return 2; // Depotpartner
            case "LIE":
               return 4; // Freistellung
            default:
               return 4; // Freistellung
            }
         }

      private static string GetValString(object ValueObject)
         {
         if (ValueObject is string)
            return ValueObject == null ? "NULL" : "'" + ValueObject.ToString().Replace("'", "''") + "'";
         if (ValueObject is int)
            return ValueObject == null ? "NULL" : ValueObject.ToString();
         if (ValueObject is DateTime)
            //return ValueObject == null ? "NULL" : "'" + ((DateTime)ValueObject).ToString("yyyy-MM-ddTHH:mm:ss") + "'";
            return ValueObject == null ? "NULL" : "'" + SqlHelper.GetDatabaseTimeString(((DateTime)ValueObject)) + "'";
         if (ValueObject is float)
            return ValueObject == null ? "NULL" : "'" + ((float)ValueObject).ToString("G", System.Globalization.CultureInfo.InvariantCulture) + "'";

         return ValueObject == null ? "NULL" : "'" + ValueObject.ToString() + "'";
         }

      private static string GetValStringNoNULLs(object ValueObject)
         {
         if (ValueObject is string)
            return ValueObject == null ? "" : "'" + ValueObject.ToString().Replace("'", "''") + "'";
         if (ValueObject is int)
            return ValueObject == null ? "0" : ValueObject.ToString();
         if (ValueObject is DateTime)
            //return ValueObject == null ? "NULL" : "'" + ((DateTime)ValueObject).ToString("yyyy-MM-ddTHH:mm:ss") + "'";
            return ValueObject == null ? SqlHelper.GetDatabaseTimeString(new DateTime(1900, 1, 1)) : "'" + SqlHelper.GetDatabaseTimeString(((DateTime)ValueObject)) + "'";
         if (ValueObject is float)
            return ValueObject == null ? "0" : "'" + ((float)ValueObject).ToString("G", System.Globalization.CultureInfo.InvariantCulture) + "'";

         return ValueObject == null ? "" : "'" + ValueObject.ToString() + "'";
         }

      private static object GetValNoNULLs(object ValueObject)
         {
         if (ValueObject is string)
            return ValueObject == null ? "" : ValueObject.ToString().Replace("'", "''");
         if (ValueObject is int)
            return ValueObject == null ? 0 : ValueObject;
         if (ValueObject is DateTime)
            //return ValueObject == null ? "NULL" : "'" + ((DateTime)ValueObject).ToString("yyyy-MM-ddTHH:mm:ss") + "'";
            return ValueObject == null ? new DateTime(1900, 1, 1) : (DateTime)ValueObject;
         if (ValueObject is float)
            return ValueObject == null ? (float)0 : ValueObject;

         return ValueObject == null ? "" : ValueObject;
         }

      public static DataSet GetDataSqlString(SqlConnection CurrentSQLConnection, string SqlCommandString)
         {
         SqlDataAdapter adapter = new SqlDataAdapter();
         adapter.SelectCommand = new SqlCommand(SqlCommandString, CurrentSQLConnection);
         DataSet dataset = new DataSet(); ;
         adapter.Fill(dataset);

         return dataset;
         }
      }

   public class Avail2DeliCreateRequest
      {
      public int LmsAvailabilityId { get; set; }
      public int LmsDeliveryId { get; set; }

      public bool Finished { get; set; }

      public int Quantity { get; set; }

      public DateTime DeliveryTimeDate { get; set; }
      public string DeliveryTimeTimeFrom { get; set; }
      public string DeliveryTimeTimeTo { get; set; }
      public DateTime LoadingTimeDate { get; set; }
      public string LoadingTimeTimeFrom { get; set; }
      public string LoadingTimeTimeTo { get; set; }
      public bool SupportsRearLoading { get; set; }
      public bool SupportsSideLoading { get; set; }
      public bool SupportsJumboVehicles { get; set; }
      public string SupportsIndividual { get; set; }
      public bool HasBasePallets { get; set; }
      public int BaseQuantity { get; set; }
      public int BasePalletTypeId { get; set; }
      public int BaseQualityId { get; set; }
      public bool IsCreatedOnline { get; set; }
      public string LiveDocumentNumber { get; set; }
      public string DigitalCode { get; set; }
      public string OnlineNote { get; set; }
      }

   }
