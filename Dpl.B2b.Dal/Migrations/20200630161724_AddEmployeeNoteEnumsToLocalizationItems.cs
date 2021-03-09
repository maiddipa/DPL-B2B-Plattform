using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddEmployeeNoteEnumsToLocalizationItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocalizationItems",
                columns: new[] { "Id", "FieldName", "Name", "Reference", "Type" },
                values: new object[] { 125, null, "EmployeeNoteType", "Dpl.B2b.Common.Enumerations.EmployeeNoteType", 1 });

            migrationBuilder.InsertData(
                table: "LocalizationItems",
                columns: new[] { "Id", "FieldName", "Name", "Reference", "Type" },
                values: new object[] { 126, null, "EmployeeNoteReason", "Dpl.B2b.Common.Enumerations.EmployeeNoteReason", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 126);
        }
    }
}
