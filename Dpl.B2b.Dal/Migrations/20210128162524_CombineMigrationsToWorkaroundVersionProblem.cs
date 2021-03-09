using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class CombineMigrationsToWorkaroundVersionProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidForMounts",
                table: "CustomerDocumentSettings");

            migrationBuilder.AddColumn<int>(
                name: "ValidForMonths",
                table: "CustomerDocumentSettings",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "CustomDocumentLabel",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "FR", "FRA", "F" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "AT", "AUT", "AT" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "CH", "CHE", "CH" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "PL", "POL", "PL" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "GB", "GBR", "GB" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "CZ", "CZE", "CZ" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "NL", "NLD", "NL" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "IT", "ITA", "I" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "BE", "BEL", "B" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "GR", "GRC", "GR" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "DK", "DNK", "DK" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "LU", "LUX", "L" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "BG", "BGR", "BG" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "SK", "SVK", "SK" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "SI", "SVN", "SLO" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "HU", "HUN", "H" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "SE", "SWE", "S" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "FI", "FIN", "FIN" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "ES", "ESP", "E" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "LT", "LTU", "LT" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "RS", "SRB", "SRB" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "RO", "ROU", "RO" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "UA", "UKR", "UA" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "LI", "LIE", "FL" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "HR", "HRV", "HR" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "LV", "LVA", "LV" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "CY", "CYP", "CY" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "MK", "MKD", "NMK" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "PT", "PRT", "P" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "EE", "EST", "EST" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "BH", "BIH", "BIH" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "NO", "NOR0", "N" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "XK", "XKX", "RKS" });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { "ME", "MNE", "MNE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidForMonths",
                table: "CustomerDocumentSettings");

            migrationBuilder.AddColumn<int>(
                name: "ValidForMounts",
                table: "CustomerDocumentSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "CustomDocumentLabel",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Iso2Code", "Iso3Code", "LicensePlateCode" },
                values: new object[] { null, null, null });
        }
    }
}
