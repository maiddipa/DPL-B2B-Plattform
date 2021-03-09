using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddedAdditionalCountriesAndCodesTURUSAIRLCHN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Iso2Code", "Iso3Code", "LicensePlateCode", "Name" },
                values: new object[,]
                {
                    { 36, "TR", "TUR", "TR", "Turkey" },
                    { 37, "CH", "CHN", "CHN", "China" },
                    { 38, "US", "USA", "USA", "United States" },
                    { 39, "IE", "IRL", "IRL", "Ireland" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 39);
        }
    }
}
