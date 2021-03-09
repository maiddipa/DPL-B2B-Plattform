using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Dpl.B2b.BusinessLogic.Reporting.DataSource;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Reporting
{
    // see https://docs.devexpress.com/XtraReports/400211/create-end-user-reporting-applications/web-reporting/asp-net-core-reporting/end-user-report-designer/api-and-customization/implement-a-report-storage
    public class ReportingStorageService : ReportStorageWebExtension
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public ReportingStorageService(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }

        public override bool CanSetData(string url)
        {
            var reportInfo = _reportGeneratorService.GetReportInfoFromUrl(url);
            return reportInfo != null;
        }

        public override bool IsValidUrl(string url)
        {
            var reportInfo = _reportGeneratorService.GetReportInfoFromUrl(url);
            return reportInfo != null;
        }

        public override byte[] GetData(string url)
        {
            var reportInfo = _reportGeneratorService.GetReportInfoFromUrl(url);

            if(reportInfo.Mode == ReportMode.Edit)
            {
                return _reportGeneratorService.GetReportTemplate(reportInfo.CustomerId.Value, (PrintType)reportInfo.PrintType, reportInfo.LanguageId);
            } else if (reportInfo.Mode == ReportMode.View)
            {
                return ((IWrappedResponse<Report>) _reportGeneratorService.GenerateReportForLanguage(reportInfo.DocumentId.Value, reportInfo.LanguageId, 1, -60)).Data.Document; //-60 is date time offset for germany
            }

            throw new ArgumentOutOfRangeException("Report mode needs to be either edit or view.");
        }

        public override Dictionary<string, string> GetUrls()
        {
            return _reportGeneratorService.GetReportUrlsForCurrentUser();
        }

        public override void SetData(XtraReport report, string url)
        {
            var reportInfo = _reportGeneratorService.GetReportInfoFromUrl(url);            
            _reportGeneratorService.SaveReportTemplate(reportInfo.CustomerId.Value, reportInfo.PrintType.Value, reportInfo.LanguageId, report);
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            SetData(report, defaultUrl);
            return defaultUrl;
        }
    }
}
