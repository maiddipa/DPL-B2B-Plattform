using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class ReportTemplatesUpdatedLoadCarrierReceiptPickupForCommentTemplateFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         var basePath = System.IO.Path.GetDirectoryName(typeof(OlmaDbContext).Assembly.Location);

         Func<string, byte[]> getTemplate =
             (path) => System.IO.File.ReadAllBytes(System.IO.Path.Combine(basePath, path));

         migrationBuilder.UpdateData("DocumentTemplates", "Id", 3, "Data", getTemplate("Seed\\report-template-LoadCarrierReceiptPickup.xml"));

         }

      protected override void Down(MigrationBuilder migrationBuilder)
        {
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 3, "Data", null);

         }
      }
}
