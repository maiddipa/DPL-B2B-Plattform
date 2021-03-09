using DevExpress.XtraReports.Web.ReportDesigner;
using Dpl.B2b.BusinessLogic.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    //[Authorize]
    public class ReportingController : ControllerBase
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public ReportingController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }

        //[HttpGet("designer-model")]
        [HttpPost("designer-model")]
        public ActionResult PostReportDesignerModel([FromForm] DesignerModelRequest request)
        {
            var info = _reportGeneratorService.GetReportInfoFromUrl(request.ReportUrl);

            if(info.Mode != ReportMode.Edit)
            {
                throw new System.Exception("This endpoint can only be called from designer (edit mode)");
            }

            string modelJsonScript =
                new ReportDesignerClientSideModelGenerator(HttpContext.RequestServices)
                    .GetJsonModelScript(
                        request.ReportUrl,                 // The URL of a report that is opened in the Report Designer when the application starts.
                        _reportGeneratorService.GetDataSourcesForTemplate(info.PrintType, info.CustomerId.Value, info.LanguageId), // Available data sources in the Report Designer that can be added to reports.
                        "DXXRD",   // The URI path of the default controller that processes requests from the Report Designer.
                        "DXXRDV",// The URI path of the default controller that that processes requests from the Web Document Viewer.
                        "DXXQB"      // The URI path of the default controller that processes requests from the Query Builder.
                    );
            return Content(modelJsonScript, "application/json");
        }

        public class DesignerModelRequest
        {
            public string ReportUrl { get; set; }
        }
    }
}