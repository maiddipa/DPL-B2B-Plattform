using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Dpl.B2b.BusinessLogic.Reporting.Barcode;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Reporting
    {
    public interface IReportGeneratorService
        {
        IWrappedResponse GenerateReportForLanguage(int documentId, int languageId, int numberOfCopies, int dateTimeOffsetInMinutes);
        byte[] GetReportTemplate(int customerId, PrintType printType, int languageId);

        void SaveReportTemplate(int customerId, PrintType printType, int languageId, XtraReport report);

        Dictionary<string, object> GetDataSourcesForTemplate(PrintType? printType, int customerId, int language);

        Dictionary<string, string> GetReportUrlsForCurrentUser();

        ReportInfo GetReportInfoFromUrl(string url);
        }


    // Test Links
    // Document Display
    // http://localhost:4200/reporting/viewer?documentId=<ID>&languageId=1
    // IDs ermitteln per SQL, IDs können wechseln
    /*
    SELECT 'Palettenannahme-Quittung/VoucherDirectReceipt' AS Report, ID FROM dbo.Documents WHERE dbo.Documents.Number = '84848-TS-667788'
    UNION
    SELECT 'Palettenannahme-Beleg/VoucherDigital' AS Report, ID FROM dbo.Documents WHERE dbo.Documents.Number = '84848-TS-445566'
    UNION
    SELECT 'Gutschrift/VoucherOriginal' AS Report, ID FROM dbo.Documents WHERE dbo.Documents.Number = '84848-TS-112233'
    UNION
    SELECT 'Annahme-Abgabebeleg/LoadCarrierReceiptExchange' AS Report, ID FROM dbo.Documents WHERE dbo.Documents.Number = '99669-TS-969696'
    UNION
    SELECT 'Ausgabebeleg/LoadCarrierReceiptPickup' AS Report, ID FROM dbo.Documents WHERE dbo.Documents.Number = '34120-TS-567567'
    UNION
    SELECT 'Annahmebeleg/LoadCarrierReceiptDelivery' AS Report, ID FROM dbo.Documents WHERE dbo.Documents.Number = '34567-TS-454647'
    UNION
    SELECT 'Transportbeleg/TransportVoucher' AS Report, ID FROM dbo.Documents WHERE dbo.Documents.Number = '84848-TS-998871'
    */
    /*
    Report	ID
    Annahme-Abgabebeleg/LoadCarrierReceiptExchange	9
    Annahmebeleg/LoadCarrierReceiptDelivery	3
    Ausgabebeleg/LoadCarrierReceiptPickup	6
    Gutschrift/VoucherOriginal	10
    Palettenannahme-Beleg/VoucherDigital	11
    Palettenannahme-Quittung/VoucherDirectReceipt	12
    Transportbeleg/TransportVoucher	17
    */

    // Designer
    // http://localhost:4200/reporting/designer?customerId=1&printType=<ID>&languageId=1
    // ID 1 = VoucherCommon
    // ID 2 = Annahme-Abgabebeleg/LoadCarrierReceiptExchange
    // ID 3 = Ausgabebeleg/LoadCarrierReceiptPickup
    // ID 4 = Lieferbeleg/LoadCarrierReceiptDelivery
    // ID 5 = Transportbeleg/TransportVoucher

    public class ReportGeneratorService : IReportGeneratorService
        {
        private readonly IRepository<Olma.Document> _documentRepo;
        private readonly IRepository<Olma.AdditionalField> _additionalFieldsRepo;
        private readonly IRepository<Olma.Customer> _customerRepo;
        private readonly IRepository<Olma.DocumentTemplate> _documentTemplateRepo;
        private readonly IRepository<Olma.DocumentTemplateLabel> _documentTemplateLabelRepo;
        private readonly IRepository<Olma.CustomDocumentLabel> _customDocumentTemplateLabelRepo;
        private readonly IRepository<Olma.DocumentType> _documentTypeRepo;
        private readonly IRepository<Olma.CustomerDocumentSetting> _customerDocumentSettingsRepo;
        private readonly IStorageService _storage;
        private readonly IRepository<Olma.VoucherReasonType> _voucherReasonTypeRepo;
        private readonly ILocalizationService _localizationService;
        private readonly int _languageIdGerman;

        // Localization TextTypes and FieldNames
        private const string textTypeVoucherCommonTitle = "DocumentTypeEnum";
        private const string fieldNameVoucherCommonTitle = "ReportVoucherCommonTitle";
        private const string textTypeVoucherReasonsTypeDescription = "VoucherReasonTypes";
        private const string fieldNameVoucherReasonTypesDescription = "DescriptionReport";
        private const string textTypeLoadCarrierTypesDescription = "LoadCarrierTypes";
        private const string fieldNameLoadCarrierTypesDescription = "LongName";
        private const string textTypeLoadCarrierQualitiesDescription = "LoadCarrierQualities";
        private const string fieldNameLoadCarrierQualitiesDescription = "";

        private const string voucherValidForMonthsReplaceToken = "$CustomerDocumentSettings.ValidForMonths$";
        private const int voucherValidForMonthsDefaultValue = 6;

        public ReportGeneratorService(
            IRepository<Olma.Document> documentRepo,
            IRepository<Olma.AdditionalField> additionalFieldsRepo,
            IRepository<Olma.Customer> customerRepo,
            IRepository<Olma.DocumentTemplate> documentTemplateRepo,
            IRepository<Olma.DocumentTemplateLabel> documentTemplateLabelRepo,
            IRepository<Olma.CustomDocumentLabel> customDocumentTemplateLabelRepo,
            IRepository<Olma.DocumentType> documentTypeRepo,
            IRepository<Olma.CustomerDocumentSetting> customerDocumentSettingsRepo,
            IStorageService storage,
            IRepository<Olma.VoucherReasonType> voucherReasonTypeRepo,
            ILocalizationService localizationService
            )
            {
            _documentTemplateRepo = documentTemplateRepo;
            _documentRepo = documentRepo;
            _additionalFieldsRepo = additionalFieldsRepo;
            _customerRepo = customerRepo;
            _documentTemplateLabelRepo = documentTemplateLabelRepo;
            _customDocumentTemplateLabelRepo = customDocumentTemplateLabelRepo;
            _documentTypeRepo = documentTypeRepo;
            _customerDocumentSettingsRepo = customerDocumentSettingsRepo;
            _storage = storage;
            _voucherReasonTypeRepo = voucherReasonTypeRepo;
            _localizationService = localizationService;
            _languageIdGerman = _localizationService.GetGermanLanguageId();
            }

        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource GetObjectDataSourceFromDataSet(LocalizedReportDataSet ds)
            {
            var ods = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource();
            ods.Name = "LocalizedReportDataSet";
            ods.DataSource = ds;

            return ods;
            }

        public byte[] GetReportTemplate(int customerId, PrintType printType, int languageId)
            {
            var documentTemplate = GetDocumentTemplate(printType, customerId);
            if (documentTemplate == null)
                {
                throw new Exception($"There is no report template for customer '{customerId}' and print type '{printType}'");
                }

            using (var ds = this.GetReportingDataset(documentTemplate, languageId, customerId))
                {

                var objectDataSource = GetObjectDataSourceFromDataSet(ds);

                // TODO check if we need to incorporate the schema only bit somewhere
                // if so we can use ds.Clone() as clone method copys the schema but not the data
                //using (var memoryStream = new MemoryStream())
                //{
                //    ds.WriteXml(memoryStream, XmlWriteMode.WriteSchema);
                //    reportDataSource = memoryStream.ToArray();
                //    memoryStream.Close();

                //}

                //var ods = GetObjectDataSourceFromDataSet(ds);

                // trying xml
                using (var template = new CustomReport())
                using (var memStream = new MemoryStream(documentTemplate.Data))
                using (var memXmlStream = new MemoryStream())
                    {
                    // for the below line works a report needs to be converted to xml format
                    // we have created a .NET Framework console app for that purpose
                    template.LoadLayoutFromXml(memStream);

                    // this is the bit that wires up the data binding in the report designer
                    template.DataSource = objectDataSource;

                    // necessary for localized field handling (CustomReport)
                    template.ReportDataSet = ds;
                    template.RebindLocalizedFields();
                    template.LoadBoundLabelTexts();
                    template.SaveLayoutToXml(memXmlStream);
                    return memXmlStream.ToArray();
                    }
                }
            }

        public void SaveReportTemplate(int customerId, PrintType printType, int languageId, XtraReport reportSource)
            {
            if (reportSource == null)
                {
                throw new ArgumentNullException("reportSource can not be null");
                }

#if DEBUG
            // HACK REMOVE - hardcode saving the report file to disk as well for testing
            using (var sourceStream = new MemoryStream())
                {
                reportSource.SaveLayoutToXml(sourceStream);
                // get root directory of solution
                var basePath = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), Constants.Documents.ContainerName);
                System.IO.File.WriteAllBytes(Path.Combine(basePath, $"report-template-{printType}-language-id-{languageId}.xml"), sourceStream.ToArray());
                }
#endif

            // trying xml
            using (var report = new CustomReport())
            using (var sourceStream = new MemoryStream())
            using (var validatedStream = new MemoryStream())
                {
                // this is neccessary to convert XtraReport to custom report
#if !DEBUG
                reportSource.SaveLayoutToXml(sourceStream);
#else
                // Kleiner Hack, auf diese Art kann die Template Datei manuell bearbeitet und danach gespeichert werden
                var basePath = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), Constants.Documents.ContainerName);
                sourceStream.Write(System.IO.File.ReadAllBytes(Path.Combine(basePath, $"report-template-{printType}-language-id-{languageId}.xml")));
#endif
                // previously code dom load option was used, this is NOT supported in .netcore
                //template.LoadLayout(memStream);
                // for the below line works a report needs to be converted to xml format
                // we have created a .NET Framework console app for that purpose
                report.LoadLayoutFromXml(sourceStream);
                report.SaveLayoutToXml(validatedStream);

                #region get existing template

                var baseQuery = _documentTemplateRepo.FindAll()
                    .AsNoTracking()
                    .Where(i => i.PrintType == printType)
                    .Include(i => i.Labels);

                // TODO check if labels should not ALWAYS be copied from default non customer specific template
                var existingTemplate = baseQuery
                    .Where(i => i.CustomerId == customerId)
                    .OrderByDescending(i => i.Version)
                    .Select(i => new { i.Version, i.Labels })
                    .FirstOrDefault();

                if (existingTemplate == null)
                    {
                    existingTemplate = baseQuery
                    .Where(i => i.IsDefault == true && i.CustomerId == null)
                    .OrderByDescending(i => i.Version)
                    .Select(i => new { i.Version, i.Labels })
                    .FirstOrDefault();
                    }

                #endregion

                var version = existingTemplate != null ? (existingTemplate.Version + 1) : 1;
                var labels = GetDocumentLabels(existingTemplate.Labels, languageId, report);

                var documentTemplateRecord = new Olma.DocumentTemplate()
                    {
                    CustomerId = customerId == 0 ? (int?)null : customerId,
                    PrintType = printType,
                    Version = version,
                    Labels = labels,
                    Data = validatedStream.ToArray(),
                    IsDefault = (customerId == 0)
                    };
                _documentTemplateRepo.Create(documentTemplateRecord);
                _documentTemplateRepo.Save();
                }

            ICollection<Olma.DocumentTemplateLabel> GetDocumentLabels(ICollection<Olma.DocumentTemplateLabel> labelsSource, int languageId, CustomReport report)
                {

                // TODO discuss how document labels are generated (comment by niko)
                // Do non localized document labels exists? based on the datastructure i dont think so

                var currentLanguageLabels = report.GetLocalizedFields()
                   //.Where(x => x.LabelDataBinding.DataMember.StartsWith("ReportLabel")) // No database bound fields
                   .Select(elem => new Olma.DocumentTemplateLabel()
                       {
                       Label = elem.Control.Name,
                       LanguageId = languageId,
                       Text = elem.Control.Text,
                       }).ToList();

                var otherLanguageLabels = labelsSource
                        .Where(i => i.LanguageId != languageId)
                        .Where(i => currentLanguageLabels.Select(x => x.Label).ToList().Contains(i.Label))   // Select only labels that are in current template, handle deletes
                        .Select(label =>
                    {
                        label.Id = 0; // reset id as we wanne create new labels that are added to the new template
                        label.DocumentTemplateId = 0;
                        return label;
                    }).ToList();

                return otherLanguageLabels.Union(currentLanguageLabels).ToList();
                }
            }

        public IWrappedResponse GenerateReportForLanguage(int documentId, int languageId, int numberOfCopies,
            int dateTimeOffsetInMinutes)
        {
            var document = _documentRepo.FindAll()
                // HACK ignore query filters for testing
                .IgnoreQueryFilters()
                .Where(i => i.Id == documentId)
                .Include(i => i.CustomerDivision)
                .Include(i => i.Type)
                .FirstOrDefault();

            var languageLocale = _localizationService.GetLocale(languageId);

            var reportEntity = GetDocumentTemplate(document.Type.PrintType, document.CustomerDivision.CustomerId);

            if (reportEntity == null)
            {
                throw new Exception($"There is no report template for document '{document.Id}'");
            }

            document.LanguageId = languageId;

            // Watermark
            var textWatermark = new XRWatermark();
            string copyWatermarkText =
                _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportWatermarkCopy);
            if (copyWatermarkText == null || copyWatermarkText.Trim() == "")
            {
                copyWatermarkText = "Kopie";
            }

            textWatermark.Text = copyWatermarkText;
            textWatermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
            textWatermark.Font = new Font(textWatermark.Font.FontFamily, 120);
            textWatermark.ForeColor = Color.Gray;
            textWatermark.TextTransparency = 150;
            textWatermark.ShowBehind = false;

            var textWatermarkDe = new XRWatermark();
            string copyWatermarkTextDe =
                _localizationService.GetLocalizationTextForEnum(_languageIdGerman,
                    AdhocTranslations.ReportWatermarkCopy);
            if (copyWatermarkTextDe == null || copyWatermarkTextDe.Trim() == "")
            {
                copyWatermarkTextDe = "Kopie";
            }

            textWatermarkDe.CopyFrom(textWatermark);
            textWatermarkDe.Text = copyWatermarkTextDe;

            var textWatermarkCancellation = new XRWatermark();
            string copyWatermarkCancellationText =
                _localizationService.GetLocalizationTextForEnum(languageId,
                    AdhocTranslations.ReportWatermarkCancellation);
            if (copyWatermarkCancellationText == null || copyWatermarkCancellationText.Trim() == "")
            {
                copyWatermarkCancellationText = "Stornierung";
            }

            textWatermarkCancellation.Text = copyWatermarkCancellationText;
            textWatermarkCancellation.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
            textWatermarkCancellation.Font = new Font(textWatermarkCancellation.Font.FontFamily, 120);
            textWatermarkCancellation.ForeColor = Color.Gray;
            textWatermarkCancellation.TextTransparency = 150;
            textWatermarkCancellation.ShowBehind = false;

            var pdfDocumentTitle = "Beleg"; // HACK Hardcoded fallback document title
            if (document.Type != null && document.Type.Name != null)
            {
                pdfDocumentTitle =
                    _localizationService.GetLocalizationTextForEntity(languageId, "DocumentTypes", document.TypeId);
            }

            var pdfDocumentTitleDe = "Beleg"; // HACK Hardcoded fallback document title
            if (document.Type != null && document.Type.Name != null)
            {
                pdfDocumentTitleDe =
                    _localizationService.GetLocalizationTextForEntity(_languageIdGerman, "DocumentTypes",
                        document.TypeId);
            }
            var reportForResponse = new Report();

            // Archive file (ist immer deutsche Kopie)
            if (languageLocale == "de")
            {
                // German copy will be created later, so just add file reference to the same filename here
                string documentName = $"{documentId}-{_languageIdGerman}-copy.pdf";
                reportForResponse.DocumentArchiveName = documentName;
                AddFileInfoToDocument(document, DocumentFileType.Archive, documentName);

                _documentRepo.Update(document);
            }
            else if (languageLocale != "de")
            {
                // Create additional german version of report
                using (var report = new CustomReport())
                {
                    using (MemoryStream stream = new MemoryStream(reportEntity.Data))
                    {
                        try
                        {
                            report.LoadLayoutFromXml(stream);
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Failed to load report template. Check InnerException.", exception);
                        }
                    }

                    using (var dataset = GetReportingDataset(reportEntity, _languageIdGerman,
                        document.CustomerDivision.CustomerId))
                    {
                        FillReportingDataset(dataset, document, _languageIdGerman, dateTimeOffsetInMinutes, reportEntity.Id);

                        // Original
                        report.DataSource = GetObjectDataSourceFromDataSet(dataset);
                        if (report.Parameters["ReportCultureCode"] != null)
                        {
                            report.Parameters["ReportCultureCode"].Value =
                                _localizationService.GetLocale(_languageIdGerman);
                        }

                        // necessary for localized field handling (CustomReport)
                        report.ReportDataSet = dataset;
                        report.RebindLocalizedFields();
                        report.LoadBoundLabelTexts();

                        SetReportExportOptions(report, pdfDocumentTitleDe);
                        report.Watermark.CopyFrom(textWatermarkDe);

                        report.CreateDocument();

                        using (MemoryStream stream = new MemoryStream())
                        {
                            try
                            {
                                // Save german copy
                                report.ExportToPdf(stream);
                                string documentName = $"{documentId}-{_languageIdGerman}-copy.pdf";
                                var downloadUrl = _storage.Write(documentName, stream).Result;
                                AddFileInfoToDocument(document, DocumentFileType.Archive, documentName);

                                _documentRepo.Update(document);
                            }
                            catch (Exception exception)
                            {
                                throw new Exception("Failed to generate pdf report. Check InnerException.", exception);
                            }
                        }
                    }
                }
            }

            using (var report = new CustomReport())
            {
                using (var reportCopy = new CustomReport())
                {
                    using (var reportCancellation = new CustomReport())
                    {
                        using (MemoryStream stream = new MemoryStream(reportEntity.Data))
                        {
                            try
                            {
                                report.LoadLayoutFromXml(stream);
                                reportCopy.LoadLayoutFromXml(stream);
                                reportCancellation.LoadLayoutFromXml(stream);
                            }
                            catch (Exception exception)
                            {
                                throw new Exception("Failed to load report template. Check InnerException.", exception);
                            }
                        }

                        using (var dataset = GetReportingDataset(reportEntity, languageId,
                            document.CustomerDivision.CustomerId))
                        {
                            FillReportingDataset(dataset, document, languageId, dateTimeOffsetInMinutes, reportEntity.Id);

                            // Original
                            report.DataSource = GetObjectDataSourceFromDataSet(dataset);
                            if (report.Parameters["ReportCultureCode"] != null)
                            {
                                report.Parameters["ReportCultureCode"].Value = languageLocale;
                            }

                            // necessary for localized field handling (CustomReport)
                            report.ReportDataSet = dataset;
                            report.RebindLocalizedFields();
                            report.LoadBoundLabelTexts();

                            SetReportExportOptions(report, pdfDocumentTitle);

                            report.CreateDocument();

                            // Copy
                            reportCopy.DataSource = GetObjectDataSourceFromDataSet(dataset);
                            if (reportCopy.Parameters["ReportCultureCode"] != null)
                            {
                                reportCopy.Parameters["ReportCultureCode"].Value = languageLocale;
                            }

                            // necessary for localized field handling (CustomReport)
                            reportCopy.ReportDataSet = dataset;
                            reportCopy.RebindLocalizedFields();
                            reportCopy.LoadBoundLabelTexts();

                            SetReportExportOptions(reportCopy, pdfDocumentTitle);
                            reportCopy.Watermark.CopyFrom(textWatermark);
                            reportCopy.CreateDocument();

                            // Cancellation
                            reportCancellation.DataSource = GetObjectDataSourceFromDataSet(dataset);
                            if (reportCancellation.Parameters["ReportCultureCode"] != null)
                            {
                                reportCancellation.Parameters["ReportCultureCode"].Value = languageLocale;
                            }

                            // necessary for localized field handling (CustomReport)
                            reportCancellation.ReportDataSet = dataset;
                            reportCancellation.RebindLocalizedFields();
                            reportCancellation.LoadBoundLabelTexts();

                            SetReportExportOptions(reportCancellation, pdfDocumentTitle);
                            reportCancellation.Watermark.CopyFrom(textWatermarkCancellation);
                            reportCancellation.CreateDocument();

                            using (MemoryStream stream = new MemoryStream())
                            {
                                try
                                {
                                    // Save original
                                    report.ExportToPdf(stream);
                                    string documentName = $"{documentId}-{languageId}-original.pdf";
                                    reportForResponse.DocumentOriginalName = documentName;
                                    var downloadUrl = _storage.Write(documentName, stream).Result;
                                    AddFileInfoToDocument(document, DocumentFileType.Original, documentName);

                                    // Save copy
                                    stream.SetLength(0);
                                    reportCopy.ExportToPdf(stream);
                                    documentName = $"{documentId}-{languageId}-copy.pdf";
                                    reportForResponse.DocumentCopyName = documentName;
                                    downloadUrl = _storage.Write(documentName, stream).Result;
                                    AddFileInfoToDocument(document, DocumentFileType.Copy, documentName);

                                    // Save cancellation
                                    stream.SetLength(0);
                                    reportCancellation.ExportToPdf(stream);
                                    documentName = $"{documentId}-{languageId}-cancellation.pdf";
                                    reportForResponse.DocumentCancellationName = documentName;
                                    downloadUrl = _storage.Write(documentName, stream).Result;
                                    AddFileInfoToDocument(document, DocumentFileType.Cancellation, documentName);

                                    // Save combination
                                    if (numberOfCopies > 0)
                                    {
                                        for (int i = 0; i < numberOfCopies; i++)
                                        {
                                            // If you want to add more than 1 page, then the report copy neeeds to be cloned. Otherwise the same page is added only once.
                                            var ps = ClonePrintingSystem(reportCopy.PrintingSystem);
                                            report.Pages.AddRange(ps.Pages);
                                        }
                                    }

                                    for (int i = 1; i < report.Pages.Count; i++)
                                    {
                                        report.Pages[i].AssignWatermark(textWatermark);
                                    }

                                    stream.SetLength(0);
                                    report.ExportToPdf(stream);
                                    documentName = $"{documentId}-{languageId}.pdf";
                                    reportForResponse.DocumentCompositeName = documentName;
                                    downloadUrl = _storage.Write(documentName, stream).Result;
                                    AddFileInfoToDocument(document, DocumentFileType.Composite, documentName);

                                    // HACK Remove development code for production
                                    //#if DEBUG
                                    //                            documentName = $"document-type-id-{document.TypeId}-language-id-{languageId}.pdf";
                                    //#endif

                                    _documentRepo.Update(document);
                                    _documentRepo.Save();
                                }
                                catch (Exception exception)
                                {
                                    throw new Exception("Failed to generate pdf report. Check InnerException.",
                                        exception);
                                }
                            }

                            using (MemoryStream stream = new MemoryStream())
                            {
                                try
                                {
                                    report.SaveLayoutToXml(stream);
                                    reportForResponse.Document = stream.ToArray();
                                    return new WrappedResponse<Report>
                                    {
                                        ResultType = ResultType.Created,
                                        Data = reportForResponse
                                    };
                                }
                                catch (Exception exception)
                                {
                                    throw new Exception(
                                        "Failed to generate xml version of report. Check InnerException.", exception);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SetReportExportOptions(CustomReport report, string pdfDocumentTitle)
            {
            report.ExportOptions.Pdf.Compressed = true;
            report.ExportOptions.Pdf.ImageQuality = PdfJpegImageQuality.Highest;
            report.ExportOptions.Pdf.NeverEmbeddedFonts = "Tahoma;Courier New;Arial";
            report.ExportOptions.Pdf.DocumentOptions.Application = "Olma";
            report.ExportOptions.Pdf.DocumentOptions.Author = "Olma";
            report.ExportOptions.Pdf.DocumentOptions.Title = pdfDocumentTitle;
            }

        private void AddFileInfoToDocument(Olma.Document document, DocumentFileType fileType, string documentName)
            {
            if (document.Files == null)
                {
                document.Files = new List<Olma.DocumentFile>();
                }

            document.Files.Add(new Olma.DocumentFile()
                {
                FileType = fileType,
                File = new Olma.File()
                    {
                    Name = documentName,
                    InternalFullPathAndName = documentName,
                    }
                });
            }

        private PrintingSystemBase ClonePrintingSystem(PrintingSystemBase source)
            {
            using (Stream stream = new MemoryStream())
                {
                source.SaveDocument(stream);
                PrintingSystemBase clone = new PrintingSystemBase();
                clone.LoadDocument(stream);
                return clone;
                }
            }

        protected Olma.DocumentTemplate GetDocumentTemplate(PrintType printType, int customerId)
            {
            var baseQuery = _documentTemplateRepo.FindAll()
                .AsNoTracking()
                .Where(i => i.PrintType == printType);

            var template = baseQuery.Where(i => i.CustomerId == customerId)
                .OrderByDescending(i => i.Version)
                .FirstOrDefault();

            if (template == null)
                {
                template = baseQuery.Where(i => i.IsDefault == true && i.CustomerId == null)
                    .OrderByDescending(i => i.Version)
                    .FirstOrDefault();
                }

            return template;
            }

        public Dictionary<string, object> GetDataSourcesForTemplate(PrintType? printType, int customerId, int language)
            {
            return null;
            // it seems like this method is only needed when new reports are created
            //var documentTemplate = GetDocumentTemplate(documentTypeId, customerId);
            //var dataSources = new Dictionary<string, object>();
            //dataSources.Add("Datasources", GetObjectDataSourceFromDataSet(GetReportingDataset(documentTemplate, language, customerId)));

            //return dataSources;
            }


        protected LocalizedReportDataSet GetReportingDataset(Olma.DocumentTemplate documentTemplate, int language, int customerId)
            {
            var dataset = new LocalizedReportDataSet();

            // create 'transposed' ReportText DataTable for the labels
            var labelEntities = _documentTemplateLabelRepo.FindAll()
                .AsNoTracking()
                .Where(i => i.DocumentTemplateId == documentTemplate.Id && i.LanguageId == language).Future();
            
            var customLabelEntities = _customDocumentTemplateLabelRepo.FindAll()
                .AsNoTracking()
                .Where(i => i.CustomerId == customerId 
                            && i.DocumentTemplateId == documentTemplate.Id && i.LanguageId == language).Future();

            // Add Label Keys as Columns
            foreach (var label in labelEntities)
                {
                dataset.ReportLabel.Columns.Add(new DataColumn(label.Label, typeof(string)));
                }

            // Create 1 row with all the label values
            var labelRow = dataset.ReportLabel.NewReportLabelRow();

            foreach (var label in labelEntities)
            {
                var customLabel = customLabelEntities.SingleOrDefault(cl => cl.ReportLabel == label.Label);
                labelRow[label.Label] = customLabel != null ? customLabel.Text : label.Text;
            }
            dataset.ReportLabel.Rows.Add(labelRow);

            // create transposed DataTable for additional Fields to select from in the report designer
            var additionalFieldEntities = _additionalFieldsRepo.FindAll()
                .AsNoTracking()
                .Where(i => i.CustomerId == customerId && i.PrintType == documentTemplate.PrintType);

            // add additional fields as columns
            foreach (var additionalFieldEntity in additionalFieldEntities)
                {
                dataset.Zusatzfelder.Columns.Add(new DataColumn(additionalFieldEntity.Name, typeof(string)));
                dataset.AdditionalFieldLabel.Columns.Add(new DataColumn(additionalFieldEntity.Name, typeof(string)));
                dataset.AdditionalFieldValue.Columns.Add(new DataColumn(additionalFieldEntity.Name, typeof(string)));
                }

            // write data type into Zusatzfelder Cols
            var zusatzfelderRow = dataset.Zusatzfelder.NewZusatzfelderRow();
            foreach (var additionalFieldEntity in additionalFieldEntities)
                {
                zusatzfelderRow[additionalFieldEntity.Name] = additionalFieldEntity.Type.ToString();
                }

            dataset.Zusatzfelder.Rows.Add(zusatzfelderRow);

            return dataset;
            }

        protected void FillReportingDataset(LocalizedReportDataSet preparedDataset, Olma.Document document, int languageId, int dateTimeOffsetInMinutes,
           int documentTemplateId)
            {
            if (document.Type.PrintType == PrintType.VoucherCommon)
                {
                FillReportingDatasetVoucherCommon(preparedDataset, document.Type.PrintType, document, languageId, dateTimeOffsetInMinutes, documentTemplateId);
                }
            else if (document.Type.PrintType == PrintType.TransportVoucher)
                {
                FillReportingDatasetTransportVoucher(preparedDataset, document, languageId, dateTimeOffsetInMinutes);
                }
            else if (document.Type.PrintType == PrintType.LoadCarrierReceiptExchange)
                {
                FillReportingDatasetLoadCarrierReceiptExchange(preparedDataset, document, languageId, dateTimeOffsetInMinutes);
                }
            else if (document.Type.PrintType == PrintType.LoadCarrierReceiptPickup)
                {
                FillReportingDatasetLoadCarrierReceiptPickup(preparedDataset, document, languageId, dateTimeOffsetInMinutes);
                }
            else if (document.Type.PrintType == PrintType.LoadCarrierReceiptDelivery)
                {
                FillReportingDatasetLoadCarrierReceiptDelivery(preparedDataset, document, languageId, dateTimeOffsetInMinutes);
                }

            //FillAdditionalFields(unitOfWork, preparedDataset, document, language);
            }

        //        protected void FillAdditionalFields(LocalizedReportDataSet preparedDataset, Olma.Document document, int languageId)
        //        {
        //            var additionalFieldValueRow = preparedDataset.AdditionalFieldValue.NewAdditionalFieldValueRow();
        //            var additionalFieldLabelRow = preparedDataset.AdditionalFieldLabel.NewAdditionalFieldLabelRow();

        //            var additionalFieldValueEntities = unitOfWork.GetRepository<Olma.AdditionalFieldValue>()
        //                .Get(elem => elem.DocumentId == document.Id, null, "AdditionalField,AdditionalField.FieldTexts");

        //            foreach (var additionalFieldValueEntity in additionalFieldValueEntities)
        //            {

        //                additionalFieldValueRow[additionalFieldValueEntity.AdditionalField.FieldName] =
        //                    additionalFieldValueEntity.FieldValue;
        //                additionalFieldLabelRow[additionalFieldValueEntity.AdditionalField.FieldName] =
        //                    additionalFieldValueEntity.AdditionalField.FieldTexts.FirstOrDefault(
        //                        _ => _.LanguageId == languageId);
        //            }
        //        }

        protected void FillReportingDatasetLoadCarrierReceiptExchange(LocalizedReportDataSet preparedDataset, Olma.Document document, int languageId, int dateTimeOffsetInMinutes)
            {
            var documentEntity = _documentRepo.FindAll()
#if DEBUG
                .IgnoreQueryFilters()
#endif
                .AsNoTracking()
                .Where(i => i.Id == document.Id)
                .Include(x => x.CustomerDivision)
                .Include(x => x.CustomerDivision.Customer)
                .Include(x => x.CustomerDivision.PostingAccount)
                .Include(x => x.CustomerDivision.PostingAccount.Address)
                .Include(x => x.CustomerDivision.PostingAccount.Address.Country)

                .Include(x => x.LoadCarrierReceipt)
                .Include(x => x.LoadCarrierReceipt.CustomerDivision)
                .Include(x => x.LoadCarrierReceipt.CustomerDivision.Customer)

                .Include(x => x.LoadCarrierReceipt.CreatedBy)
                .Include(x => x.LoadCarrierReceipt.CreatedBy.Person)

                .Include(x => x.LoadCarrierReceipt.LicensePlateCountry)

                .Include(x => x.LoadCarrierReceipt.ShipperAddress)
                .Include(x => x.LoadCarrierReceipt.ShipperAddress.Country)

                .Include(x => x.LoadCarrierReceipt.Positions)
                .Include(x => x.LoadCarrierReceipt.Positions).ThenInclude(x => x.LoadCarrier.Type)
                .Include(x => x.LoadCarrierReceipt.Positions).ThenInclude(x => x.LoadCarrier.Quality)

                .Select(document => document)
                .FirstOrDefault();

            //var entity = unitOfWork.GetRepository<PoolingVoucher>()
            //    .Get(elem => elem.DocumentId == document.Id, null, "")
            //    .FirstOrDefault();

            if (documentEntity == null)
                {
                // @TODO: error handling
                return;
                }

            var loadCarrierReceiptRow = preparedDataset.LoadCarrierReceipt.NewLoadCarrierReceiptRow();
            loadCarrierReceiptRow.DocumentID = documentEntity.Id;

            var voucherTopTitle = _localizationService.GetLocalizationTextForEnum(languageId, (DocumentTypeEnum)documentEntity.TypeId, fieldNameVoucherCommonTitle);
            //string voucherTopTitle = "DPL-Pooling Tausch-Quittung-Nr.";
            loadCarrierReceiptRow.VoucherTopTitle = voucherTopTitle;

            string voucherTitle = "Quittung über Direktbuchung  (keine Einreichung durch Empfänger notwendig)"; // TODO
            loadCarrierReceiptRow.VoucherTitle = voucherTitle;

            string voucherShownYesText = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportVoucherShownYes);
            string voucherShownNoText = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportVoucherShownNo);

            loadCarrierReceiptRow.Number = documentEntity.Number;
            loadCarrierReceiptRow.DeliveryNoteNumber = (documentEntity.LoadCarrierReceipt.DeliveryNoteNumber ?? "");
            //+ " " + (documentEntity.LoadCarrierReceipt.DeliveryNoteShown != null && documentEntity.LoadCarrierReceipt.DeliveryNoteShown == true ? voucherShownYesText : voucherShownNoText);
            loadCarrierReceiptRow.PickUpNoteNumber = (documentEntity.LoadCarrierReceipt.PickUpNoteNumber ?? "");
            //+ " " + (documentEntity.LoadCarrierReceipt.PickUpNoteShown != null && documentEntity.LoadCarrierReceipt.PickUpNoteShown == true ? voucherShownYesText : voucherShownNoText);
            loadCarrierReceiptRow.CustomerReference = documentEntity.LoadCarrierReceipt.CustomerReference ?? "";

            loadCarrierReceiptRow.IssueDate = GetLocalDateTime(documentEntity.IssuedDateTime, dateTimeOffsetInMinutes).Date;

            string digitalCodeYesNoText = _localizationService
                .GetLocalizationTextForEnum(languageId,
                documentEntity.LoadCarrierReceipt.DigitalCode != null ? AdhocTranslations.ReportDigitalCodeYes : AdhocTranslations.ReportDigitalCodeNo);
            loadCarrierReceiptRow.DigitalCodeYesNo = digitalCodeYesNoText;

            string voucherLongTextText = _localizationService
                .GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportLoadCarrierReceiptExchangeLongText);
            loadCarrierReceiptRow.VoucherLongText = voucherLongTextText;

            //if (document.Division?.Customer != null)
            if (documentEntity.LoadCarrierReceipt.CustomerDivision.Customer != null)
                {
                var issuingCustomer = documentEntity.CustomerDivision.Customer;
                loadCarrierReceiptRow.CustomerNumber = issuingCustomer.Id.ToString();
                loadCarrierReceiptRow.IssuerID = documentEntity.LoadCarrierReceipt.CustomerDivision.Id.ToString(); // TODO Right number???

                var customerAddress = documentEntity.CustomerDivision.PostingAccount.Address;
                if (customerAddress.Country != null)
                    {
                    loadCarrierReceiptRow.Issuer = $"{issuingCustomer.Name}\n"
                        + $"{customerAddress.Street1}\n"
                        + $"{customerAddress.Country.Iso2Code}-{customerAddress.PostalCode} {customerAddress.City}";
                    }
                }
            else
                {
                //@TODO: error handling
                }

            loadCarrierReceiptRow.Recipient_Name = documentEntity.LoadCarrierReceipt.TruckDriverCompanyName;

            loadCarrierReceiptRow.LicensePlate = documentEntity.LoadCarrierReceipt.LicensePlate;
            loadCarrierReceiptRow.LicensePlateCountry = documentEntity.LoadCarrierReceipt.LicensePlateCountry.LicensePlateCode;
            loadCarrierReceiptRow.LicensePlateCombined = "(" + loadCarrierReceiptRow.LicensePlateCountry + ")   " + loadCarrierReceiptRow.LicensePlate;

            loadCarrierReceiptRow.TruckDriverName = documentEntity.LoadCarrierReceipt.TruckDriverName;
            loadCarrierReceiptRow.TruckDriverCompanyName = documentEntity.LoadCarrierReceipt.TruckDriverCompanyName;

            var issuerPerson = documentEntity.LoadCarrierReceipt.CreatedBy?.Person;
            if (issuerPerson != null)
                {
                loadCarrierReceiptRow.IssuerName =
                        $"{issuerPerson.FirstName} {issuerPerson.LastName}";
                }

            // Shipper
            loadCarrierReceiptRow.ShipperName = documentEntity.LoadCarrierReceipt.ShipperCompanyName;

            var shipperAddress = documentEntity.LoadCarrierReceipt.ShipperAddress;
            if (shipperAddress != null)
                {
                loadCarrierReceiptRow.ShipperCity = shipperAddress.City;
                loadCarrierReceiptRow.ShipperPostalCode = shipperAddress.PostalCode;
                loadCarrierReceiptRow.ShipperStreet = shipperAddress.Street1;
                loadCarrierReceiptRow.ShipperCountry = shipperAddress.Country?.Name;
                }

            // Barcodes
            loadCarrierReceiptRow.Barcode = documentEntity.Number;
            loadCarrierReceiptRow.BarcodeCustomerNumber = loadCarrierReceiptRow.CustomerNumber;

            preparedDataset.LoadCarrierReceipt.Rows.Add(loadCarrierReceiptRow);

            var positions = documentEntity.LoadCarrierReceipt.Positions;

            foreach (var position in positions)
                {
                var palletDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierTypesDescription, position.LoadCarrier.Type.Id, fieldNameLoadCarrierTypesDescription);
                var palletQualityDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierQualitiesDescription, position.LoadCarrier.Quality.Id);

                var loadCarrierReceiptPalletCount =
                    preparedDataset.LoadCarrierReceiptPalletCounts.NewLoadCarrierReceiptPalletCountsRow();
                loadCarrierReceiptPalletCount.PalletCount = position.LoadCarrierQuantity;
                loadCarrierReceiptPalletCount.PalletDescription = palletDescription;
                loadCarrierReceiptPalletCount.PalletQualityDescription = palletQualityDescription;
                loadCarrierReceiptPalletCount.DocumentId = document.Id;
                loadCarrierReceiptPalletCount.Pallet2QualityId = position.LoadCarrierId;
                loadCarrierReceiptPalletCount.Sorting = position.LoadCarrier.Type.Order * 1000 + position.LoadCarrier.Order;
                preparedDataset.LoadCarrierReceiptPalletCounts.Rows.Add(loadCarrierReceiptPalletCount);
                }
            }

        protected void FillReportingDatasetLoadCarrierReceiptPickup(LocalizedReportDataSet preparedDataset, Olma.Document document, int languageId, int dateTimeOffsetInMinutes)
            {
            var documentEntity = _documentRepo.FindAll()
#if DEBUG
                .IgnoreQueryFilters()
#endif
                .AsNoTracking()
                .Where(i => i.Id == document.Id)
                .Include(x => x.CustomerDivision)
                .Include(x => x.CustomerDivision.Customer)
                .Include(x => x.CustomerDivision.PostingAccount)
                .Include(x => x.CustomerDivision.PostingAccount.Address)
                .Include(x => x.CustomerDivision.PostingAccount.Address.Country)

                .Include(x => x.LoadCarrierReceipt)
                .Include(x => x.LoadCarrierReceipt.CustomerDivision)
                .Include(x => x.LoadCarrierReceipt.CustomerDivision.Customer)

                .Include(x => x.LoadCarrierReceipt.CreatedBy)
                .Include(x => x.LoadCarrierReceipt.CreatedBy.Person)

                .Include(x => x.LoadCarrierReceipt.LicensePlateCountry)

                .Include(x => x.LoadCarrierReceipt.ShipperAddress).ThenInclude(sa => sa.Country)

                .Include(x => x.LoadCarrierReceipt.Positions)
                .Include(x => x.LoadCarrierReceipt.Positions).ThenInclude(x => x.LoadCarrier.Type)
                .Include(x => x.LoadCarrierReceipt.Positions).ThenInclude(x => x.LoadCarrier.Quality)

                .Select(document => document)
                .FirstOrDefault();

            //var entity = unitOfWork.GetRepository<PoolingVoucher>()
            //    .Get(elem => elem.DocumentId == document.Id, null, "")
            //    .FirstOrDefault();

            if (documentEntity == null)
                {
                // @TODO: error handling
                return;
                }

            var loadCarrierReceiptRow = preparedDataset.LoadCarrierReceipt.NewLoadCarrierReceiptRow();
            loadCarrierReceiptRow.DocumentID = documentEntity.Id;

            var voucherTopTitle = _localizationService.GetLocalizationTextForEnum(languageId, (DocumentTypeEnum)documentEntity.TypeId, fieldNameVoucherCommonTitle);
            //string voucherTopTitle = "DPL-Pooling Ausgabe-Quittung-Nr.";
            loadCarrierReceiptRow.VoucherTopTitle = voucherTopTitle;

            string voucherTitle = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportLoadCarrierReceiptPickupVoucherTitle);
            loadCarrierReceiptRow.VoucherTitle = voucherTitle;

            loadCarrierReceiptRow.DPLAddressAndContact = _localizationService
                .GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportDPLAdressAndContacts);

            string voucherShownYesText = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportVoucherShownYes);
            string voucherShownNoText = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportVoucherShownNo);

            loadCarrierReceiptRow.Number = documentEntity.Number;
            loadCarrierReceiptRow.DeliveryNoteNumber = (documentEntity.LoadCarrierReceipt.DeliveryNoteNumber ?? "");
            //+ " " + (documentEntity.LoadCarrierReceipt.DeliveryNoteShown != null && documentEntity.LoadCarrierReceipt.DeliveryNoteShown == true ? voucherShownYesText : voucherShownNoText);
            loadCarrierReceiptRow.PickUpNoteNumber = (documentEntity.LoadCarrierReceipt.PickUpNoteNumber ?? "");
            //+ " " + (documentEntity.LoadCarrierReceipt.PickUpNoteShown != null && documentEntity.LoadCarrierReceipt.PickUpNoteShown == true ? voucherShownYesText : voucherShownNoText);
            loadCarrierReceiptRow.CustomerReference = documentEntity.LoadCarrierReceipt.CustomerReference ?? "";

            loadCarrierReceiptRow.IssueDate = GetLocalDateTime(documentEntity.IssuedDateTime, dateTimeOffsetInMinutes);

            loadCarrierReceiptRow.Comment = documentEntity.LoadCarrierReceipt.Note;

            string digitalCodeYesNoText = _localizationService
                .GetLocalizationTextForEnum(languageId,
                documentEntity.LoadCarrierReceipt.DigitalCode != null ? AdhocTranslations.ReportDigitalCodeYes : AdhocTranslations.ReportDigitalCodeNo);
            loadCarrierReceiptRow.DigitalCodeYesNo = digitalCodeYesNoText;

            string voucherLongTextText = _localizationService
                .GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportLoadCarrierReceiptPickupLongText);
            loadCarrierReceiptRow.VoucherLongText = voucherLongTextText;

            //if (document.Division?.Customer != null)
            if (documentEntity.LoadCarrierReceipt.CustomerDivision.Customer != null)
                {
                var issuingCustomer = documentEntity.CustomerDivision.Customer;
                loadCarrierReceiptRow.CustomerNumber = issuingCustomer.Id.ToString();
                loadCarrierReceiptRow.IssuerID = documentEntity.LoadCarrierReceipt.CustomerDivision.Id.ToString(); // TODO Right number???

                var customerAddress = documentEntity.CustomerDivision.PostingAccount.Address;
                if (customerAddress.Country != null)
                    {
                    loadCarrierReceiptRow.Issuer = $"{issuingCustomer.Name}\n"
                        + $"{customerAddress.Street1}\n"
                        + $"{customerAddress.Country.Iso2Code}-{customerAddress.PostalCode} {customerAddress.City}";
                    }
                }
            else
                {
                //@TODO: error handling
                }

            loadCarrierReceiptRow.Recipient_Name = documentEntity.LoadCarrierReceipt.TruckDriverCompanyName;

            loadCarrierReceiptRow.LicensePlate = documentEntity.LoadCarrierReceipt.LicensePlate;
            loadCarrierReceiptRow.LicensePlateCountry = documentEntity.LoadCarrierReceipt.LicensePlateCountry.LicensePlateCode;
            loadCarrierReceiptRow.LicensePlateCombined = "(" + loadCarrierReceiptRow.LicensePlateCountry + ")   " + loadCarrierReceiptRow.LicensePlate;

            loadCarrierReceiptRow.TruckDriverName = documentEntity.LoadCarrierReceipt.TruckDriverName;
            loadCarrierReceiptRow.TruckDriverCompanyName = documentEntity.LoadCarrierReceipt.TruckDriverCompanyName;

            var issuerPerson = documentEntity.LoadCarrierReceipt.CreatedBy?.Person;
            if (issuerPerson != null)
                {
                loadCarrierReceiptRow.IssuerName =
                        $"{issuerPerson.FirstName} {issuerPerson.LastName}";
                }

            // Shipper
            loadCarrierReceiptRow.ShipperName = documentEntity.LoadCarrierReceipt.ShipperCompanyName;

            var shipperAddress = documentEntity.LoadCarrierReceipt.ShipperAddress;
            if (shipperAddress != null)
                {
                loadCarrierReceiptRow.ShipperCity = shipperAddress.City;
                loadCarrierReceiptRow.ShipperPostalCode = shipperAddress.PostalCode;
                loadCarrierReceiptRow.ShipperStreet = shipperAddress.Street1;
                loadCarrierReceiptRow.ShipperCountry = shipperAddress.Country?.Name;
                }

            // Barcodes
            loadCarrierReceiptRow.Barcode = documentEntity.Number;
            loadCarrierReceiptRow.BarcodeCustomerNumber = loadCarrierReceiptRow.CustomerNumber;

            preparedDataset.LoadCarrierReceipt.Rows.Add(loadCarrierReceiptRow);

            var positions = documentEntity.LoadCarrierReceipt.Positions;

            foreach (var position in positions)
                {
                var palletDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierTypesDescription, position.LoadCarrier.Type.Id, fieldNameLoadCarrierTypesDescription);
                var palletQualityDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierQualitiesDescription, position.LoadCarrier.Quality.Id);

                var loadCarrierReceiptPalletCount =
                    preparedDataset.LoadCarrierReceiptPalletCounts.NewLoadCarrierReceiptPalletCountsRow();
                loadCarrierReceiptPalletCount.PalletCount = position.LoadCarrierQuantity;
                loadCarrierReceiptPalletCount.PalletDescription = palletDescription;
                loadCarrierReceiptPalletCount.PalletQualityDescription = palletQualityDescription;
                loadCarrierReceiptPalletCount.DocumentId = document.Id;
                loadCarrierReceiptPalletCount.Pallet2QualityId = position.LoadCarrierId;
                loadCarrierReceiptPalletCount.Sorting = position.LoadCarrier.Type.Order * 1000 + position.LoadCarrier.Order;
                preparedDataset.LoadCarrierReceiptPalletCounts.Rows.Add(loadCarrierReceiptPalletCount);
                }
            }

        protected void FillReportingDatasetLoadCarrierReceiptDelivery(LocalizedReportDataSet preparedDataset, Olma.Document document, int languageId, int dateTimeOffsetInMinutes)
            {
            var documentEntity = _documentRepo.FindAll()
#if DEBUG
                .IgnoreQueryFilters()
#endif
                .AsNoTracking()
                .Where(i => i.Id == document.Id)
                .Include(x => x.CustomerDivision)
                .Include(x => x.CustomerDivision.Customer)
                .Include(x => x.CustomerDivision.PostingAccount)
                .Include(x => x.CustomerDivision.PostingAccount.Address)
                .Include(x => x.CustomerDivision.PostingAccount.Address.Country)

                .Include(x => x.LoadCarrierReceipt)
                .Include(x => x.LoadCarrierReceipt.CustomerDivision)
                .Include(x => x.LoadCarrierReceipt.CustomerDivision.Customer)

                .Include(x => x.LoadCarrierReceipt.CreatedBy)
                .Include(x => x.LoadCarrierReceipt.CreatedBy.Person)

                .Include(x => x.LoadCarrierReceipt.LicensePlateCountry)

                .Include(x => x.LoadCarrierReceipt.ShipperAddress).ThenInclude(sa => sa.Country)

                .Include(x => x.LoadCarrierReceipt.Positions)
                .Include(x => x.LoadCarrierReceipt.Positions).ThenInclude(x => x.LoadCarrier.Type)
                .Include(x => x.LoadCarrierReceipt.Positions).ThenInclude(x => x.LoadCarrier.Quality)

                .Select(document => document)
                .FirstOrDefault();

            //var entity = unitOfWork.GetRepository<PoolingVoucher>()
            //    .Get(elem => elem.DocumentId == document.Id, null, "")
            //    .FirstOrDefault();

            if (documentEntity == null)
                {
                // @TODO: error handling
                return;
                }

            var loadCarrierReceiptRow = preparedDataset.LoadCarrierReceipt.NewLoadCarrierReceiptRow();
            loadCarrierReceiptRow.DocumentID = documentEntity.Id;

            var voucherTopTitle = _localizationService.GetLocalizationTextForEnum(languageId, (DocumentTypeEnum)documentEntity.TypeId, fieldNameVoucherCommonTitle);
            //string voucherTopTitle = "DPL-Pooling Annahme-Quittung-Nr."; // TODO
            loadCarrierReceiptRow.VoucherTopTitle = voucherTopTitle;

            string voucherTitle; // TODO
            if (documentEntity.LoadCarrierReceipt.DigitalCode != null && documentEntity.LoadCarrierReceipt.PostingAccountId != null)
                {
                //"Quittung über Direktbuchung  (keine Einreichung mehr notwendig)"
                voucherTitle = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportLoadCarrierDeliveryVoucherTitleDirectBooking);
                }
            else
                {
                //"Digitale Einreichung  (in DPL-Account des Belegempfängers)"
                //    + "\nOriginal-Beleg-Einreichung  (durch Belegempfänger bei DPL)"
                voucherTitle = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportLoadCarrierDeliveryDigitalSubmission); 
                }
            loadCarrierReceiptRow.VoucherTitle = voucherTitle;

            loadCarrierReceiptRow.DPLAddressAndContact = _localizationService
                .GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportDPLAdressAndContacts);

            string voucherShownYesText = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportVoucherShownYes);
            string voucherShownNoText = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportVoucherShownNo);

            loadCarrierReceiptRow.Number = documentEntity.Number;
            loadCarrierReceiptRow.DeliveryNoteNumber = (documentEntity.LoadCarrierReceipt.DeliveryNoteNumber ?? "");
            //+ " " + (documentEntity.LoadCarrierReceipt.DeliveryNoteShown != null && documentEntity.LoadCarrierReceipt.DeliveryNoteShown == true ? voucherShownYesText : voucherShownNoText);
            loadCarrierReceiptRow.PickUpNoteNumber = (documentEntity.LoadCarrierReceipt.PickUpNoteNumber ?? "");
            //+ " " + (documentEntity.LoadCarrierReceipt.PickUpNoteShown != null && documentEntity.LoadCarrierReceipt.PickUpNoteShown == true ? voucherShownYesText : voucherShownNoText);
            loadCarrierReceiptRow.CustomerReference = documentEntity.LoadCarrierReceipt.CustomerReference ?? "";

            loadCarrierReceiptRow.IssueDate = GetLocalDateTime(documentEntity.IssuedDateTime, dateTimeOffsetInMinutes);

            string digitalCodeYesNoText = _localizationService
                .GetLocalizationTextForEnum(languageId,
                documentEntity.LoadCarrierReceipt.DigitalCode != null ? AdhocTranslations.ReportDigitalCodeYes : AdhocTranslations.ReportDigitalCodeNo);
            loadCarrierReceiptRow.DigitalCodeYesNo = digitalCodeYesNoText;

            string voucherLongTextText = _localizationService
                .GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportLoadCarrierReceiptDeliveryLongText);
            loadCarrierReceiptRow.VoucherLongText = voucherLongTextText;

            //if (document.Division?.Customer != null)
            if (documentEntity.LoadCarrierReceipt.CustomerDivision.Customer != null)
                {
                var issuingCustomer = documentEntity.CustomerDivision.Customer;
                loadCarrierReceiptRow.CustomerNumber = issuingCustomer.Id.ToString();
                loadCarrierReceiptRow.IssuerID = documentEntity.LoadCarrierReceipt.CustomerDivision.Id.ToString(); // TODO Right number???

                var customerAddress = documentEntity.CustomerDivision.PostingAccount.Address;
                if (customerAddress.Country != null)
                    {
                    loadCarrierReceiptRow.Issuer = $"{issuingCustomer.Name}\n"
                        + $"{customerAddress.Street1}\n"
                        + $"{customerAddress.Country.Iso2Code}-{customerAddress.PostalCode} {customerAddress.City}";
                    }
                }
            else
                {
                //@TODO: error handling
                }

            loadCarrierReceiptRow.Recipient_Name = documentEntity.LoadCarrierReceipt.TruckDriverCompanyName;

            loadCarrierReceiptRow.LicensePlate = documentEntity.LoadCarrierReceipt.LicensePlate;
            loadCarrierReceiptRow.LicensePlateCountry = documentEntity.LoadCarrierReceipt.LicensePlateCountry.LicensePlateCode;
            loadCarrierReceiptRow.LicensePlateCombined = "(" + loadCarrierReceiptRow.LicensePlateCountry + ")   " + loadCarrierReceiptRow.LicensePlate;

            loadCarrierReceiptRow.TruckDriverName = documentEntity.LoadCarrierReceipt.TruckDriverName;
            loadCarrierReceiptRow.TruckDriverCompanyName = documentEntity.LoadCarrierReceipt.TruckDriverCompanyName;

            var issuerPerson = documentEntity.LoadCarrierReceipt.CreatedBy?.Person;
            if (issuerPerson != null)
                {
                loadCarrierReceiptRow.IssuerName =
                        $"{issuerPerson.FirstName} {issuerPerson.LastName}";
                }

            // Shipper
            loadCarrierReceiptRow.ShipperName = documentEntity.LoadCarrierReceipt.ShipperCompanyName;

            var shipperAddress = documentEntity.LoadCarrierReceipt.ShipperAddress;
            if (shipperAddress != null)
                {
                loadCarrierReceiptRow.ShipperCity = shipperAddress.City;
                loadCarrierReceiptRow.ShipperPostalCode = shipperAddress.PostalCode;
                loadCarrierReceiptRow.ShipperStreet = shipperAddress.Street1;
                loadCarrierReceiptRow.ShipperCountry = shipperAddress.Country?.Name;
                }

            // Barcodes
            loadCarrierReceiptRow.Barcode = documentEntity.Number;
            loadCarrierReceiptRow.BarcodeCustomerNumber = loadCarrierReceiptRow.CustomerNumber;

            preparedDataset.LoadCarrierReceipt.Rows.Add(loadCarrierReceiptRow);

            var positions = documentEntity.LoadCarrierReceipt.Positions;

            foreach (var position in positions)
                {
                var palletDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierTypesDescription, position.LoadCarrier.Type.Id, fieldNameLoadCarrierTypesDescription);
                var palletQualityDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierQualitiesDescription, position.LoadCarrier.Quality.Id);

                var loadCarrierReceiptPalletCount =
                    preparedDataset.LoadCarrierReceiptPalletCounts.NewLoadCarrierReceiptPalletCountsRow();
                loadCarrierReceiptPalletCount.PalletCount = position.LoadCarrierQuantity;
                loadCarrierReceiptPalletCount.PalletDescription = palletDescription;
                loadCarrierReceiptPalletCount.PalletQualityDescription = palletQualityDescription;
                loadCarrierReceiptPalletCount.DocumentId = document.Id;
                loadCarrierReceiptPalletCount.Pallet2QualityId = position.LoadCarrierId;
                loadCarrierReceiptPalletCount.Sorting = position.LoadCarrier.Type.Order * 1000 + position.LoadCarrier.Order;
                preparedDataset.LoadCarrierReceiptPalletCounts.Rows.Add(loadCarrierReceiptPalletCount);
                }
            }

        protected void FillReportingDatasetTransportVoucher(LocalizedReportDataSet preparedDataset, Olma.Document document, int languageId, int dateTimeOffsetInMinutes)
            {
            var documentEntity = _documentRepo.FindAll()
                .IgnoreQueryFilters()   // IgnoreQueryFilters needed here because report needs access to data of different customers
                .AsNoTracking()
                .Where(i => i.Id == document.Id)
                .Include(x => x.CustomerDivision)
                .Include(x => x.CustomerDivision.Customer)
                .Include(x => x.CustomerDivision.PostingAccount)
                .Include(x => x.CustomerDivision.PostingAccount.Address)
                .Include(x => x.CustomerDivision.PostingAccount.Address.Country)

                .Include(x => x.OrderMatch)
                .Include(x => x.OrderMatch.Supply)
                .Include(x => x.OrderMatch.Supply.Order)
                .Include(x => x.OrderMatch.Supply.Order.LoadingLocation)
                .Include(x => x.OrderMatch.Supply.Order.LoadingLocation.Address)
                .Include(x => x.OrderMatch.Supply.Order.LoadingLocation.Address.Country)
                .Include(x => x.OrderMatch.Supply.Order.LoadingLocation.CustomerDivision)
                .Include(x => x.OrderMatch.Supply.Order.LoadingLocation.CustomerDivision.Customer)

                .Include(x => x.OrderMatch)
                .Include(x => x.OrderMatch.Demand)
                .Include(x => x.OrderMatch.Demand.Order)
                .Include(x => x.OrderMatch.Demand.Order.LoadingLocation)
                .Include(x => x.OrderMatch.Demand.Order.LoadingLocation.Address)
                .Include(x => x.OrderMatch.Demand.Order.LoadingLocation.Address.Country)
                .Include(x => x.OrderMatch.Demand.Order.LoadingLocation.CustomerDivision)
                .Include(x => x.OrderMatch.Demand.Order.LoadingLocation.CustomerDivision.Customer)

                .Include(x => x.OrderMatch.CreatedBy)
                .Include(x => x.OrderMatch.CreatedBy.Person)

                .Include(x => x.OrderMatch.LoadCarrier)
                .Include(x => x.OrderMatch.LoadCarrier.Type)
                .Include(x => x.OrderMatch.LoadCarrier.Quality)

                .Select(document => document)
                .FirstOrDefault();

            if (documentEntity == null)
                {
                // @TODO: error handling
                return;
                }

            var transportVoucherRow = preparedDataset.TransportVoucher.NewTransportVoucherRow();
            transportVoucherRow.DocumentID = document.Id;

            var voucherTopTitle = _localizationService.GetLocalizationTextForEnum(languageId, (DocumentTypeEnum)documentEntity.TypeId, fieldNameVoucherCommonTitle);
            //string voucherTopTitle = "Transport Beleg/Ladevorschriften";
            transportVoucherRow.VoucherTopTitle = voucherTopTitle;


            transportVoucherRow.IssueDate = GetLocalDateTime(documentEntity.IssuedDateTime, dateTimeOffsetInMinutes).Date;
            transportVoucherRow.Number = String.IsNullOrEmpty(document.Number) ? "-" : document.Number;

            transportVoucherRow.DPLAddressAndContact = _localizationService
               .GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportDPLAdressAndContacts);

            var issuingCustomer = documentEntity.CustomerDivision.Customer;
            if (issuingCustomer != null)
                {
                transportVoucherRow.CustomerNumber = issuingCustomer.Id.ToString();

                var customerAddress = documentEntity.CustomerDivision.Customer.Address;
                transportVoucherRow.Issuer = $"{issuingCustomer.Name}\n";
                if (customerAddress != null && customerAddress.Country != null)
                    {
                    transportVoucherRow.Issuer += $"{customerAddress.Street1}\n"
                        + $"{customerAddress.Country.Iso2Code}-{customerAddress.PostalCode} {customerAddress.City}";
                    }
                }
            else
                {
                //@TODO: error handling
                }

            var issuerPerson = documentEntity.OrderMatch.CreatedBy?.Person;
            if (issuerPerson != null)
                {
                transportVoucherRow.IssuerName = $"{issuerPerson.FirstName} {issuerPerson.LastName}";
                }

            transportVoucherRow.DigitalCode = documentEntity.OrderMatch.DigitalCode ?? "";

            // Supply
            if (documentEntity.OrderMatch.Supply != null)
                {
                transportVoucherRow.SupplyName = documentEntity.OrderMatch.Supply.Order.LoadingLocation.CustomerDivision.Customer.Name;

                var supplyAddress = documentEntity.OrderMatch.Supply.Order.LoadingLocation.Address;
                if (supplyAddress != null)
                    {
                    transportVoucherRow.SupplyCity = supplyAddress.City;
                    transportVoucherRow.SupplyPostalCode = supplyAddress.PostalCode;
                    transportVoucherRow.SupplyStreet = supplyAddress.Street1;
                    transportVoucherRow.SupplyCountry = supplyAddress.Country?.Name;
                    transportVoucherRow.SupplyAddress = $"{supplyAddress.Street1}\n"
                        + $"{supplyAddress.Country?.Iso2Code}-{supplyAddress.PostalCode} {supplyAddress.City}";
                    }
                }

            // Demand
            if (documentEntity.OrderMatch.Demand != null)
                {
                transportVoucherRow.DemandName = documentEntity.OrderMatch.Demand.Order.LoadingLocation.CustomerDivision.Customer.Name;

                var demandAddress = documentEntity.OrderMatch.Demand.Order.LoadingLocation.Address;
                if (demandAddress != null)
                    {
                    transportVoucherRow.DemandCity = demandAddress.City;
                    transportVoucherRow.DemandPostalCode = demandAddress.PostalCode;
                    transportVoucherRow.DemandStreet = demandAddress.Street1;
                    transportVoucherRow.DemandCountry = demandAddress.Country?.Name;
                    transportVoucherRow.DemandAddress = $"{demandAddress.Street1}\n"
                        + $"{demandAddress.Country?.Iso2Code}-{demandAddress.PostalCode} {demandAddress.City}";
                    }
                }

            // Barcodes
            transportVoucherRow.Barcode = documentEntity.Number;
            transportVoucherRow.BarcodeCustomerNumber = transportVoucherRow.CustomerNumber;

            transportVoucherRow.SupplyFulfillmentDateTime = documentEntity.OrderMatch.Supply.Detail.PlannedFulfillmentDateTime;
            transportVoucherRow.DemandFulfillmentDateTime = documentEntity.OrderMatch.Demand.Detail.PlannedFulfillmentDateTime;

            transportVoucherRow.LoadCarrierStackHeight = documentEntity.OrderMatch.LoadCarrierStackHeight;
            transportVoucherRow.SupportsRearLoading = documentEntity.OrderMatch.SupportsRearLoading;
            transportVoucherRow.SupportsSideLoading = documentEntity.OrderMatch.SupportsSideLoading;
            transportVoucherRow.SupportsJumboVehicles = documentEntity.OrderMatch.SupportsJumboVehicles;

            preparedDataset.TransportVoucher.Rows.Add(transportVoucherRow);

            int displayOrder = 1;
            var palletDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierTypesDescription,
                documentEntity.OrderMatch.LoadCarrier.Type.Id, fieldNameLoadCarrierTypesDescription);
            var palletQualityDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierQualitiesDescription,
                documentEntity.OrderMatch.LoadCarrier.Quality.Id);

            var transportVoucherPalletCount =
                preparedDataset.TransportVoucherPalletCounts.NewTransportVoucherPalletCountsRow();
            transportVoucherPalletCount.PalletCount = documentEntity.OrderMatch.LoadCarrierQuantity;
            transportVoucherPalletCount.PalletDescription = palletDescription == null ? "" : palletDescription;
            transportVoucherPalletCount.PalletQualityDescription = palletQualityDescription == null ? "" : palletQualityDescription;
            transportVoucherPalletCount.Pallet2QualityId = documentEntity.OrderMatch.LoadCarrier.Id;
            transportVoucherPalletCount.Sorting = displayOrder;
            preparedDataset.TransportVoucherPalletCounts.Rows.Add(transportVoucherPalletCount);

            }

        protected void FillReportingDatasetVoucherCommon(LocalizedReportDataSet preparedDataset, PrintType printType, Olma.Document document, int languageId, int dateTimeOffsetInMinutes,
           int documentTemplateId)
            {
            var documentEntity = _documentRepo.FindAll()
#if DEBUG
                .IgnoreQueryFilters()
#endif
                .AsNoTracking()
                .Where(i => i.Id == document.Id)
                .Include(x => x.CustomerDivision)
                .Include(x => x.CustomerDivision.Customer)
                .Include(x => x.CustomerDivision.PostingAccount)
                .Include(x => x.CustomerDivision.PostingAccount.Address)
                .Include(x => x.CustomerDivision.PostingAccount.Address.Country)

                .Include(x => x.Voucher)
                .Include(x => x.Voucher.CustomerDivision)
                .Include(x => x.Voucher.CustomerDivision.Customer)

                .Include(x => x.Voucher.CreatedBy)
                .Include(x => x.Voucher.CreatedBy.Person)

                .Include(x => x.Voucher.LicensePlateCountry)

                .Include(x => x.Voucher.Recipient)
                .Include(x => x.Voucher.Recipient.Address)
                .Include(x => x.Voucher.Recipient.Address.Country)
                .Include(x => x.Voucher.Supplier)
                .Include(x => x.Voucher.Supplier.Address)
                .Include(x => x.Voucher.Supplier.Address.Country)
                .Include(x => x.Voucher.Shipper)
                .Include(x => x.Voucher.Shipper.Address)
                .Include(x => x.Voucher.Shipper.Address.Country)
                .Include(x => x.Voucher.SubShipper)
                .Include(x => x.Voucher.SubShipper.Address)
                .Include(x => x.Voucher.SubShipper.Address.Country)
                .Include(x => x.Voucher.ExpressCode)

                .Include(x => x.Voucher.Positions)
                .Include(x => x.Voucher.Positions).ThenInclude(x => x.LoadCarrier.Type)
                .Include(x => x.Voucher.Positions).ThenInclude(x => x.LoadCarrier.Quality)

                .Select(document => document)
                .FirstOrDefault();

            if (documentEntity == null)
                {
                // TODO: error handling
                return;
                }

            // Gültigkeitsdauer holen
            var customerDocumentSettingEntity = _customerDocumentSettingsRepo.FindAll()
#if DEBUG
                .IgnoreQueryFilters()
#endif
                .AsNoTracking()
                .Where(i => i.CustomerId == documentEntity.CustomerDivision.CustomerId
                    && i.DocumentTypeId == document.TypeId
                    && i.LoadCarrierTypeId == null)
                .Select(document => document)
                .FirstOrDefault();

            var palletAcceptanceCommonRow = preparedDataset.PalletAcceptanceCommon.NewPalletAcceptanceCommonRow();
            palletAcceptanceCommonRow.DocumentID = documentEntity.Id;
            palletAcceptanceCommonRow.Number = documentEntity.Number;
            palletAcceptanceCommonRow.IssueDate = GetLocalDateTime(documentEntity.IssuedDateTime, dateTimeOffsetInMinutes);

            palletAcceptanceCommonRow.CustomerReference = documentEntity.Voucher.CustomerReference;
            palletAcceptanceCommonRow.CustomerTransportFlag = documentEntity.Voucher.ProcurementLogistics;

            palletAcceptanceCommonRow.DPLAddressAndContact = _localizationService
                .GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportDPLAdressAndContacts);

            palletAcceptanceCommonRow.Comment = documentEntity.Voucher.Note;

            string digitalCodeYesNoText = _localizationService
                .GetLocalizationTextForEnum(languageId,
                documentEntity.Voucher.ExpressCodeId.HasValue ? AdhocTranslations.ReportDigitalCodeYes : AdhocTranslations.ReportDigitalCodeNo);
            palletAcceptanceCommonRow.DigitalCodeYesNo = digitalCodeYesNoText;

            string voucherLongTextText = null;
            if (document.Type.Type == DocumentTypeEnum.VoucherOriginal)
               {
               // Text zuerst in CustomDocumentLabel suchen, Special für Kaufland
               var customLabelEntities = _customDocumentTemplateLabelRepo.FindAll()
                     .AsNoTracking()
                     .Where(i => i.CustomerId == documentEntity.CustomerDivision.CustomerId
                                  && i.DocumentTemplateId == documentTemplateId
                                  && i.LanguageId == languageId
                                  && i.ReportLabel == "VoucherOriginal_VoucherLongText")
                     .FirstOrDefault();
                  voucherLongTextText = customLabelEntities?.Text;
               }
            if(voucherLongTextText == null)
               {
               voucherLongTextText = _localizationService
                  .GetLocalizationTextForEnum(languageId, document.Type.Type == DocumentTypeEnum.VoucherDirectReceipt
                     ? AdhocTranslations.ReportVoucherCommonDirectReceiptLongText
                     : AdhocTranslations.ReportVoucherCommonLongText);
               }

            int voucherValidForMonths = customerDocumentSettingEntity == null || customerDocumentSettingEntity.ValidForMonths == null
                ? voucherValidForMonthsDefaultValue
                : (int)customerDocumentSettingEntity.ValidForMonths;
            voucherLongTextText = voucherLongTextText.Replace(voucherValidForMonthsReplaceToken, voucherValidForMonths.ToString());
            palletAcceptanceCommonRow.VoucherLongText = voucherLongTextText;

            var voucherTitle = _localizationService.GetLocalizationTextForEnum(languageId, (DocumentTypeEnum)documentEntity.TypeId, fieldNameVoucherCommonTitle);
            string voucherSubTitle;
            switch (document.Type.Type)
                {
                case DocumentTypeEnum.VoucherDigital:
                    //voucherTitle = "Digitale Einreichung  (in DPL-Account des Belegempfängers)";
                    voucherSubTitle = "";
                    break;
                case DocumentTypeEnum.VoucherDirectReceipt:
                    //voucherTitle = "Quittung über Direktbuchung  (keine Einreichung mehr notwendig)";
                    voucherSubTitle = _localizationService.GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportVoucherJustReceipt);
                    break;
                case DocumentTypeEnum.VoucherOriginal:
                    //voucherTitle = "Original-Beleg-Einreichung  (durch Belegempfänger bei DPL)";
                    voucherSubTitle = "";
                    break;
                default:
                    voucherTitle = "Unbekannter DocumentType";
                    voucherSubTitle = "";
                    break;
                }
            palletAcceptanceCommonRow.VoucherTitle = voucherTitle;
            palletAcceptanceCommonRow.VoucherSubTitle = voucherSubTitle;

            if (documentEntity.Voucher.CustomerDivision.Customer != null)
                {
                var issuingCustomer = documentEntity.CustomerDivision.Customer;
                palletAcceptanceCommonRow.CustomerNumber = issuingCustomer.Id.ToString();

                var customerAddress = documentEntity.CustomerDivision.PostingAccount.Address;
                if (customerAddress.Country != null)
                    {
                    palletAcceptanceCommonRow.Issuer = $"{issuingCustomer.Name} - "
                        + $"{customerAddress.Street1} - "
                        + $"{customerAddress.Country.Iso2Code}-{customerAddress.PostalCode} {customerAddress.City}";
                    }
                }
            else
                {
                // TODO: error handling
                }

            palletAcceptanceCommonRow.DigitalCode = documentEntity.Voucher.ExpressCode?.DigitalCode ?? "";
            
            palletAcceptanceCommonRow.LicensePlate = documentEntity.Voucher.LicensePlate ?? string.Empty;
            palletAcceptanceCommonRow.LicensePlateCountry = documentEntity.Voucher.LicensePlateCountry?.LicensePlateCode ?? string.Empty;
            palletAcceptanceCommonRow.LicensePlateCombined = "(" + palletAcceptanceCommonRow.LicensePlateCountry + ")   " + palletAcceptanceCommonRow.LicensePlate;
            palletAcceptanceCommonRow.LicensePlateCombined = palletAcceptanceCommonRow.LicensePlateCombined.Replace("()", string.Empty).Trim();
            
            palletAcceptanceCommonRow.TruckDriverName = documentEntity.Voucher.TruckDriverName ?? string.Empty;
            palletAcceptanceCommonRow.TruckDriverCompanyName = documentEntity.Voucher.TruckDriverCompanyName ?? string.Empty;

            var issuerPerson = documentEntity.Voucher.CreatedBy?.Person;
            if (issuerPerson != null)
                {
                palletAcceptanceCommonRow.IssuerName =
                     $"{issuerPerson.FirstName} {issuerPerson.LastName}";
                }

            // Recipient
            if (documentEntity.Voucher.Recipient != null)
                {
                if (document.Type.Type == DocumentTypeEnum.VoucherDirectReceipt
                    || document.Type.Type == DocumentTypeEnum.VoucherDigital)
                    {
                    string RecipientName = _localizationService
                        .GetLocalizationTextForEnum(languageId, AdhocTranslations.ReportDPLPoolingMember);
                    palletAcceptanceCommonRow.RecipientName = RecipientName;
                    }
                else
                    {
                    palletAcceptanceCommonRow.RecipientName = documentEntity.Voucher.Recipient.CompanyName;
                    }

                var receipientAddress = documentEntity.Voucher.Recipient.Address;
                if (receipientAddress != null)
                    {
                    palletAcceptanceCommonRow.RecipientCity = receipientAddress.City;
                    palletAcceptanceCommonRow.RecipientPostalCode = receipientAddress.PostalCode;
                    palletAcceptanceCommonRow.RecipientStreet = receipientAddress.Street1;
                    palletAcceptanceCommonRow.RecipientCountry = receipientAddress.Country?.Name;
                    }
                }

            // Supplier
            if (documentEntity.Voucher.Supplier != null)
                {
                palletAcceptanceCommonRow.SupplierName = documentEntity.Voucher.Supplier.CompanyName;

                var supplierAddress = documentEntity.Voucher.Supplier.Address;
                if (supplierAddress != null)
                    {
                    palletAcceptanceCommonRow.SupplierCity = supplierAddress.City;
                    palletAcceptanceCommonRow.SupplierPostalCode = supplierAddress.PostalCode;
                    palletAcceptanceCommonRow.SupplierStreet = supplierAddress.Street1;
                    palletAcceptanceCommonRow.SupplierCountry = supplierAddress.Country?.Name;
                    }
                }

            // Shipper
            if (documentEntity.Voucher.Shipper != null)
                {
                palletAcceptanceCommonRow.ShipperName = documentEntity.Voucher.Shipper.CompanyName;

                var shipperAddress = documentEntity.Voucher.Shipper.Address;
                if (shipperAddress != null)
                    {
                    palletAcceptanceCommonRow.ShipperCity = shipperAddress.City;
                    palletAcceptanceCommonRow.ShipperPostalCode = shipperAddress.PostalCode;
                    palletAcceptanceCommonRow.ShipperStreet = shipperAddress.Street1;
                    palletAcceptanceCommonRow.ShipperCountry = shipperAddress.Country?.Name;
                    }
                }

            // SubShipper
            if (documentEntity.Voucher.SubShipper != null)
                {
                palletAcceptanceCommonRow.SubShipperName = documentEntity.Voucher.SubShipper.CompanyName;

                var subShipperAddress = documentEntity.Voucher.SubShipper.Address;
                if (subShipperAddress != null)
                    {
                    palletAcceptanceCommonRow.SubShipperCity = subShipperAddress.City;
                    palletAcceptanceCommonRow.SubShipperPostalCode = subShipperAddress.PostalCode;
                    palletAcceptanceCommonRow.SubShipperStreet = subShipperAddress.Street1;
                    palletAcceptanceCommonRow.SubShipperCountry = subShipperAddress.Country?.Name;
                    }
                }

            // Barcodes
            palletAcceptanceCommonRow.Barcode = documentEntity.Number;
            palletAcceptanceCommonRow.BarcodeCustomerNumber = palletAcceptanceCommonRow.CustomerNumber;

            preparedDataset.PalletAcceptanceCommon.Rows.Add(palletAcceptanceCommonRow);


            var palletCounts = documentEntity.Voucher.Positions;

            foreach (var palletCount in palletCounts)
                {
                var palletDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierTypesDescription,
                    palletCount.LoadCarrier.Type.Id, fieldNameLoadCarrierTypesDescription);
                var palletQualityDescription = _localizationService.GetLocalizationTextForEntity(languageId, textTypeLoadCarrierQualitiesDescription,
                    palletCount.LoadCarrier.Quality.Id, fieldNameLoadCarrierQualitiesDescription);

                var palletAcceptanceCommonPalletCount =
                    preparedDataset.PalletAcceptanceCommonPalletCounts.NewPalletAcceptanceCommonPalletCountsRow();
                palletAcceptanceCommonPalletCount.PalletCount = palletCount.LoadCarrierQuantity;
                palletAcceptanceCommonPalletCount.PalletDescription = palletDescription == null ? "" : palletDescription;
                palletAcceptanceCommonPalletCount.PalletQualityDescription = palletQualityDescription == null ? "" : palletQualityDescription;
                palletAcceptanceCommonPalletCount.Pallet2QualityId = palletCount.LoadCarrierId;
                palletAcceptanceCommonPalletCount.Sorting = palletCount.LoadCarrier.Type.Order * 1000 + palletCount.LoadCarrier.Order;
                preparedDataset.PalletAcceptanceCommonPalletCounts.Rows.Add(palletAcceptanceCommonPalletCount);
                }


            var voucherReasonTypeSelection = _voucherReasonTypeRepo.FindAll()
                .AsNoTracking()
                .Where(i => i.Id >= 0)
                .OrderBy(x => x.Order)
                .Select(voucherReasonType => voucherReasonType).ToList();

            foreach (var reason in voucherReasonTypeSelection)
                {
                var reasonText = _localizationService.GetLocalizationTextForEntity(languageId, textTypeVoucherReasonsTypeDescription, reason.Id, fieldNameVoucherReasonTypesDescription);

                var palletAcceptanceCommonReasonRow = preparedDataset.PalletAcceptanceCommonReason.NewPalletAcceptanceCommonReasonRow();
                palletAcceptanceCommonReasonRow.DisplayOrder = reason.Order;
                palletAcceptanceCommonReasonRow.Text = reasonText;
                palletAcceptanceCommonReasonRow.Selected = documentEntity.Voucher.ReasonTypeId == reason.Id;
                if ((document.Type.Type != DocumentTypeEnum.VoucherDirectReceipt && document.Type.Type != DocumentTypeEnum.VoucherDigital)
                    || palletAcceptanceCommonReasonRow.Selected)
                    {
                    // Bei Direktbuchung oder Digitaler Einreichung nur ausgewählten Grund anzeigen
                    preparedDataset.PalletAcceptanceCommonReason.Rows.Add(palletAcceptanceCommonReasonRow);
                    }
                }
            }

        public ReportInfo GetReportInfoFromUrl(string url)
            {
            var queryString = System.Web.HttpUtility.ParseQueryString(url);

            var modeString = queryString.Get("mode");
            switch (modeString)
                {
                case "edit":
                        {
                        var customerIdString = queryString.Get("customerId");
                        var printTypeString = queryString.Get("printType");
                        var languageIdString = queryString.Get("languageId");
                        if (customerIdString == null || printTypeString == null || languageIdString == null)
                            {
                            return null;
                            }

                        int customerId;
                        int printType;
                        int languageId;

                        if (!int.TryParse(customerIdString, out customerId) || !int.TryParse(printTypeString, out printType) || !int.TryParse(languageIdString, out languageId))
                            {
                            return null;
                            }

                        return new ReportInfo()
                            {
                            Mode = ReportMode.Edit,
                            CustomerId = customerId,
                            PrintType = (PrintType)printType,
                            LanguageId = languageId
                            };
                        }
                case "view":
                        {
                        var documentIdString = queryString.Get("documentId");
                        var languageIdString = queryString.Get("languageId");
                        if (documentIdString == null || languageIdString == null)
                            {
                            return null;
                            }

                        int documentId;
                        int languageId;

                        if (!int.TryParse(documentIdString, out documentId) || !int.TryParse(languageIdString, out languageId))
                            {
                            return null;
                            }

                        return new ReportInfo()
                            {
                            Mode = ReportMode.View,
                            DocumentId = documentId,
                            LanguageId = languageId
                            };
                        }
                default:
                    throw new ArgumentOutOfRangeException($"Report mode needs to be either edit or view but was: {modeString}");
                }
            }

        public Dictionary<string, string> GetReportUrlsForCurrentUser()
            {
            // TODO add security here for editing document templates
            var customers = _customerRepo.FindAll()
                .AsNoTracking()
                .ToList();

            var languages = _localizationService.GetSupportedLanguages();

            var documentTypes = _documentTypeRepo.FindAll()
                .AsNoTracking()
                .Where(i => i.HasReport)
                .ToList();

            var reportDictionary = customers
                .SelectMany(c => documentTypes
                    .SelectMany(d => languages
                        .Select(l => new
                            {
                            Name = $"{c.Name} - {d.Name} - {l.Locale}",
                            Url = $"mode=edit&printType={(int)d.PrintType}&documentType={d.Id}&customerId={c.Id}&languageId={l.Id}"
                            })
                    )
                ).ToDictionary(i => i.Url, i => i.Name);

            return reportDictionary;
            }

        private DateTime GetLocalDateTime(DateTime date, int dateTimeOffsetInMinutes)
            {
            return date.AddMinutes(dateTimeOffsetInMinutes * -1);
            }
        }

    public class ReportInfo
        {
        public ReportMode Mode { get; set; }

        public int LanguageId { get; set; }

        // edit
        public int? CustomerId { get; set; }
        public PrintType? PrintType { get; set; }

        // view
        public int? DocumentId { get; set; }
        }

    public enum ReportMode
        {
        Edit = 0,
        View = 1
        }
    }
