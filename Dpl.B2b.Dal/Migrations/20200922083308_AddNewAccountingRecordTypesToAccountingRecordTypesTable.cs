using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddNewAccountingRecordTypesToAccountingRecordTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "Description", "Name", "RefLtmBookingTypeId" },
                values: new object[] { "Gutschrift DPL-DPG Buchung", "Gutsch. DPL-DPG", "GUTDDG" });

            migrationBuilder.InsertData(
                table: "AccountingRecordTypes",
                columns: new[] { "Id", "Description", "Name", "RefLtmBookingTypeId" },
                values: new object[,]
                {
                    { 57, "Ausgleich DPL-DPG Buchung", "Ausgleich DPL-DPG", "AUSDDG" },
                    { 58, "Gutschrift Direkt Buchung", "Direktgutschrift", "GUTDBG" },
                    { 59, "Ausgleich Direkt Buchung", "Direktbuchung", "AUSDBG" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.UpdateData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "Description", "Name", "RefLtmBookingTypeId" },
                values: new object[] { "Digitale Pooling Gutschrift", "Digitale Pooling Gutschrift", "DPLDPG" });
        }
    }
}
