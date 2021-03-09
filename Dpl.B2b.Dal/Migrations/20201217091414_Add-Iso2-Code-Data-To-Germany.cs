using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddIso2CodeDataToGermany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 1,
                column: "Iso2Code",
                value: "BW");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 2,
                column: "Iso2Code",
                value: "BY");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 3,
                column: "Iso2Code",
                value: "BE");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 4,
                column: "Iso2Code",
                value: "BB");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 5,
                column: "Iso2Code",
                value: "HB");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 6,
                column: "Iso2Code",
                value: "HH");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 7,
                column: "Iso2Code",
                value: "HE");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 8,
                column: "Iso2Code",
                value: "NI");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 9,
                column: "Iso2Code",
                value: "MV");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 10,
                column: "Iso2Code",
                value: "NW");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 11,
                column: "Iso2Code",
                value: "RP");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 12,
                column: "Iso2Code",
                value: "SL");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 13,
                column: "Iso2Code",
                value: "SN");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 14,
                column: "Iso2Code",
                value: "ST");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 15,
                column: "Iso2Code",
                value: "SH");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 16,
                column: "Iso2Code",
                value: "TH");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 1,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 3,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 4,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 5,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 6,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 7,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 8,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 9,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 10,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 11,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 12,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 13,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 14,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 15,
                column: "Iso2Code",
                value: null);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 16,
                column: "Iso2Code",
                value: null);
        }
    }
}
