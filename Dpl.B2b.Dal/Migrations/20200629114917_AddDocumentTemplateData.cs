using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddDocumentTemplateData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var basePath = System.IO.Path.GetDirectoryName(typeof(OlmaDbContext).Assembly.Location);

            Func<string, byte[]> getTemplate =
                (path) => System.IO.File.ReadAllBytes(System.IO.Path.Combine(basePath, path));

            migrationBuilder.UpdateData("DocumentTemplates", "Id", 1, "Data", getTemplate("Seed\\report-template-VoucherCommon.xml"));
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 2, "Data", getTemplate("Seed\\report-template-LoadCarrierReceiptExchange.xml"));
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 3, "Data", getTemplate("Seed\\report-template-LoadCarrierReceiptPickup.xml"));
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 4, "Data", getTemplate("Seed\\report-template-LoadCarrierReceiptDelivery.xml"));
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 5, "Data", getTemplate("Seed\\report-template-TransportVoucher.xml"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 1, "Data", null);
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 2, "Data", null);
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 3, "Data", null);
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 4, "Data", null);
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 5, "Data", null);
        }
    }
}
