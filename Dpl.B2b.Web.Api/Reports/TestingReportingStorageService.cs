using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Dpl.B2b.Contracts;
using Microsoft.AspNetCore.Hosting;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.Web.Api.Reporting
{

    // see https://docs.devexpress.com/XtraReports/400211/create-end-user-reporting-applications/web-reporting/asp-net-core-reporting/end-user-report-designer/api-and-customization/implement-a-report-storage
    public class TestingReportingStorageService : ReportStorageWebExtension
    {

        public TestingReportingStorageService(IWebHostEnvironment env)
        {
            _reportDirectory = System.IO.Path.Combine(env.ContentRootPath, "Reports");
        }
        // the below example was taken from the .netcore 3 + angular starter project on the devexpress site
        // it shows which methods need to be implemented
        // it uses file storage to store the reports where we would use the database

        readonly string _reportDirectory;
        const string FileExtension = ".repx";
        public TestingReportingStorageService(string reportDirectory)
        {
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }
            this._reportDirectory = reportDirectory;
        }

        public override bool CanSetData(string url)
        {
            // Determines whether or not it is possible to store a report by a given URL. 
            // For instance, make the CanSetData method return false for reports that should be read-only in your storage. 
            // This method is called only for valid URLs (i.e., if the IsValidUrl method returned true) before the SetData method is called.

            return true;
        }

        public override bool IsValidUrl(string url)
        {
            // Determines whether or not the URL passed to the current Report Storage is valid. 
            // For instance, implement your own logic to prohibit URLs that contain white spaces or some other special characters. 
            // This method is called before the CanSetData and GetData methods.

            return true;
        }

        public override byte[] GetData(string url)
        {
            // Returns report layout data stored in a Report Storage using the specified URL. 
            // This method is called only for valid URLs after the IsValidUrl method is called.
            try
            {
                var basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                return File.ReadAllBytes(Path.Combine(basePath, "Reports\\XtraReport.repx"));

                //return File.ReadAllBytes(Path.Combine(_reportDirectory, url + FileExtension));
            }
            catch (Exception)
            {
                throw new FaultException(new FaultReason(string.Format("Could not find report '{0}'.", url)), new FaultCode("Server"), "GetData");
            }
        }

        public override Dictionary<string, string> GetUrls()
        {
            // Returns a dictionary of the existing report URLs and display names. 
            // This method is called when running the Report Designer, 
            // before the Open Report and Save Report dialogs are shown and after a new report is saved to a storage.

            return Directory.GetFiles(_reportDirectory, "*" + FileExtension)
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToDictionary<string, string>(x => x);
        }

        public override void SetData(XtraReport report, string url)
        {
            // Stores the specified report to a Report Storage using the specified URL. 
            // This method is called only after the IsValidUrl and CanSetData methods are called.
            report.SaveLayoutToXml(Path.Combine(_reportDirectory, url + FileExtension));
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            // Stores the specified report using a new URL. 
            // The IsValidUrl and CanSetData methods are never called before this method. 
            // You can validate and correct the specified URL directly in the SetNewData method implementation 
            // and return the resulting URL used to save a report in your storage.
            SetData(report, defaultUrl);
            return defaultUrl;
        }
    }
}
