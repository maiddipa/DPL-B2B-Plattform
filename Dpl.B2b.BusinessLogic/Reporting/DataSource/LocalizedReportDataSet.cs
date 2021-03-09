namespace Dpl.B2b.BusinessLogic.Reporting.DataSource
   {
   partial class LocalizedReportDataSet
      {
      partial class PalletAcceptanceCommonDataTable
         {
         }

      partial class TransportVoucherDataTable
         {
         }

      partial class LoadCarrierReceiptDataTable
         {
         }

      partial class PalletAcceptanceVoucherlessDataTable
         {
         }

      partial class PalletAcceptanceSlipDataTable
         {
         }
      #region Constantes
      /// <summary>
      /// Note: this key is used in the old OPMS System, do not change unless you know what you are doing
      /// </summary>
      private const string CryptKey = "The meaning of life, the universe and everything.";
      #endregion

      //public partial class DepotAdoptionDocumentRow
      //{
      //    #region Properties

      //    public string BarcodeContent
      //    {
      //        get
      //        {
      //            var barcodeString = new StringBuilder(128);

      //            barcodeString.Append(DocumentID).Append(";");
      //            barcodeString.Append(BarcodeCryptoUtils.GetMd5Hash(BarcodeMetaDataContent));

      //            return BarcodeCryptoUtils.EncryptWithRijndael(
      //                barcodeString.ToString(),
      //                CryptKey);
      //        }
      //    }

      //    public string BarcodeMetaDataContent
      //    {
      //        get
      //        {
      //            var content = new StringBuilder(2048);

      //            content.Append(DocumentID).Append(";");
      //            content.Append(Number).Append(";");
      //            content.Append(IssueDate).Append(";");
      //            content.Append(Deliverer_Firma).Append(";");
      //            content.Append(Deliverer_Lastname).Append(";");
      //            content.Append(Return_Shipper).Append(";");

      //            return content.ToString();
      //        }
      //    }

      //    #endregion
      //}

      //public partial class DebtVoucherRow
      //{
      //    #region Properties
      //    public string BarcodeContent
      //    {
      //        get
      //        {

      //            var barcodeString = new StringBuilder(128);

      //            barcodeString.Append(DocumentID).Append(";");
      //            barcodeString.Append(BarcodeCryptoUtils.GetMd5Hash(BarcodeMetaDataContent));

      //            return BarcodeCryptoUtils.EncryptWithRijndael(
      //                barcodeString.ToString(),
      //                CryptKey);
      //        }
      //    }

      //    public string BarcodeMetaDataContent
      //    {
      //        get
      //        {
      //            var content = new StringBuilder(2048);

      //            content.Append(DocumentID).Append(";");
      //            content.Append(Number).Append(";");
      //            content.Append(Debtee).Append(";");
      //            content.Append(IssueDate).Append(";");
      //            content.Append(DebteeName).Append(";");
      //            content.Append(Debtor_Name).Append(";");
      //            content.Append(DebtorNameCALCULATED).Append(";");

      //            return content.ToString();
      //        }
      //    }
      //    #endregion
      //}

      //public partial class PickupOrderRow
      //{
      //    public string BarcodeContent => Number;
      //}
      }
   }